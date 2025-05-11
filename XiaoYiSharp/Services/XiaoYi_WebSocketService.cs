using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.WebSockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using XiaoYiSharp.Protocols;
using XiaoYiSharp.Utils;
using static System.Net.Mime.MediaTypeNames;

namespace XiaoYiSharp.Services
{
    public class XiaoYi_WebSocketService:IDisposable
    {
        public delegate Task MessageEventHandler(string type, string state, string message);
        public event MessageEventHandler? OnMessageEvent = null;

        public delegate Task AudioEventHandler(byte[] opus);
        public event AudioEventHandler? OnAudioEvent = null;

        public delegate Task IotThingsEventHandler(string commands);
        public event IotThingsEventHandler? OnIotThingsEvent = null;

        public string WebSocketUrl { get; set; } = "ws://coze.nbee.net/xiaozhi/v1";//"wss://api.tenclass.net/xiaozhi/v1/"; //"ws://192.168.31.113:8888/xiaozhi/v1/";//
        public string WebSocketToken { get; set; } = "test-token";
        public string DeviceId { get; set; } = Utils.SystemInfo.GetMacAddress();
        public string? IotThings { get; set; }

        private ClientWebSocket _webSocket = new ClientWebSocket();
        private string? _sessionId { get; set; }
        private bool _isFirst = true;

        public XiaoYi_WebSocketService() { }
        public void Start() {
            _webSocket.Options.SetRequestHeader("Authorization", "Bearer " + WebSocketToken);
            _webSocket.Options.SetRequestHeader("Protocol-Version", "1");
            _webSocket.Options.SetRequestHeader("Device-Id", DeviceId);
            _webSocket.Options.SetRequestHeader("Client-Id", Guid.NewGuid().ToString());
            _webSocket.ConnectAsync(new Uri(WebSocketUrl), CancellationToken.None);

            Task.Run(async () =>
            {
                await ReceiveMessagesAsync();
            });
        }
        public void Restart() {
            _webSocket = new ClientWebSocket();
            _isFirst = true;
            Start();
        }
        public void Stop() {
        }
        private async Task ReceiveMessagesAsync() {
            byte[] buffer = new byte[1024 * 10];
            while (true)
            {
                if (_webSocket.State == WebSocketState.Open)
                {
                    try
                    {
                        if (_isFirst)
                        {
                            _isFirst = false;
                            await SendMessageAsync(XiaoYiSharp.Protocols.XiaoYi_Protocol.Hello());
                        }

                        WebSocketReceiveResult result = await _webSocket.ReceiveAsync(new ArraySegment<byte>(buffer), CancellationToken.None);
                        byte[] messageBytes = new byte[result.Count];
                        Array.Copy(buffer, messageBytes, result.Count);
                        if (result.MessageType == WebSocketMessageType.Text)
                        {
                            var message = Encoding.UTF8.GetString(messageBytes);

                            LogConsole.ReceiveLine(message);
                            dynamic? msg = JsonConvert.DeserializeObject<dynamic>(message);
                            if (msg != null)
                            {
                                if (message.Contains("session_id"))
                                {
                                    _sessionId = (string)msg.session_id;
                                }
                                if (msg.type == "hello")
                                {
                                    if (!string.IsNullOrEmpty(IotThings))
                                        await SendMessageAsync(XiaoYiSharp.Protocols.XiaoYi_Protocol.Device_Info(IotThings));
                                }
                                if (msg.type == "stt") {
                                    string text = msg.text;
                                    if (!string.IsNullOrEmpty(text))
                                    {
                                        if (OnMessageEvent != null)
                                        {
                                            await OnMessageEvent("STT", "", text);
                                        }
                                    }
                                }
                                if (msg.type == "tts")
                                {
                                    if (msg.state == "sentence_start" || msg.state == "sentence_end")
                                    {
                                        string text = msg.text;
                                        string state = msg.state;
                                        if (!string.IsNullOrEmpty(text) && !string.IsNullOrEmpty(state))
                                        {
                                            if (OnMessageEvent != null)
                                            {
                                                await OnMessageEvent("TTS", state, text);
                                            }
                                        }
                                    }

                                    if (msg.state == "start" || msg.state == "stop")
                                    {
                                        string state = msg.state;
                                        if (!string.IsNullOrEmpty(state))
                                        {
                                            if (OnMessageEvent != null)
                                            {
                                                await OnMessageEvent("TTS", state, "");
                                            }
                                        }
                                    }
                                }
                                if (msg.type == "llm")
                                {
                                    string text = msg.text;
                                    string emotion = msg.emotion;
                                    if (!string.IsNullOrEmpty(text))
                                    {
                                        if (OnMessageEvent != null)
                                        {
                                            await OnMessageEvent("LLM", emotion, text);
                                        }
                                    }
                                }
                                if (msg.type == "iot")
                                {
                                    dynamic commands = msg.commands;
                                    if (!string.IsNullOrEmpty(commands))
                                    {
                                        if (OnIotThingsEvent != null)
                                        {
                                            await OnIotThingsEvent(commands);
                                        }
                                    }
                                }
                            }
                        }
                        if (result.MessageType == WebSocketMessageType.Binary)
                        {
                            if (OnAudioEvent != null)
                            {
                                await OnAudioEvent(messageBytes);
                            }
                        }
                        if (result.MessageType == WebSocketMessageType.Close)
                        {
                            // 处理关闭消息
                            await _webSocket.CloseAsync(WebSocketCloseStatus.NormalClosure, "Closing", CancellationToken.None);
                        }
                    }
                    catch (Exception ex)
                    {
                        LogConsole.WarningLine($"WebSocket {_webSocket.State} {ex.Message}");
                    }
                }
                else
                {
                    if (!_isFirst && _webSocket.State == WebSocketState.Closed)
                    {
                        LogConsole.WarningLine($"WebSocket {_webSocket.State} 重新连接...");
                        Restart();
                    }
                }
                await Task.Delay(10);
            }
        }
        private async Task SendMessageAsync(string message)
        {
            if (_webSocket.State == WebSocketState.Open)
            {
                byte[] messageBytes = Encoding.UTF8.GetBytes(message);
                await _webSocket.SendAsync(new ArraySegment<byte>(messageBytes), WebSocketMessageType.Text, true, CancellationToken.None);
                LogConsole.SendLine(message);
            }
        }
        private async Task SendAudioAsync(byte[] audio)
        {
            if (_webSocket.State == WebSocketState.Open)
            {
                await _webSocket.SendAsync(new ArraySegment<byte>(audio), WebSocketMessageType.Binary, true, CancellationToken.None);
            }
        }
        public async Task SendMessage(string message,string url="")
        {
            if(string.IsNullOrEmpty(url))
                await SendMessageAsync(XiaoYi_Protocol.Listen_Detect(message));
            else
                await SendMessageAsync(XiaoYi_Protocol.Listen_Detect(message, url));
        }
        public async Task SendAudio(byte[] audio)
        {
            await SendAudioAsync(audio);
        }
        public async Task SendDeviceStatus(string status)
        {
            await SendMessageAsync(XiaoYi_Protocol.Device_Status(status));
        }
        public async Task StartRecording()
        {
            await SendMessageAsync(XiaoYi_Protocol.Listen_Start("", "manual"));
        }
        public async Task StartRecordingAuto()
        {
            await SendMessageAsync(XiaoYi_Protocol.Listen_Start(_sessionId, "auto"));
        }
        public async Task StopRecording()
        {
            await SendMessageAsync(XiaoYi_Protocol.Listen_Stop(_sessionId));
        }
        public async Task Abort()
        {
            await SendMessageAsync(XiaoYi_Protocol.Abort());
        }
        public void Dispose()
        {
            _webSocket.Dispose();
        }
    }
}
