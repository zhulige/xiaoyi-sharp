using System.Threading.Tasks;
using XiaoYiSharp.Services;

namespace XiaoYiSharp
{
    public class XiaoYiAgent
    {
        public delegate Task MessageEventHandler(string type, string state, string message);
        public event MessageEventHandler? OnMessageEvent = null;

        public delegate Task AudioEventHandler(byte[] opus);
        public event AudioEventHandler? OnAudioEvent = null;

        public delegate Task IotThingsEventHandler(string commands);
        public event IotThingsEventHandler? OnIotThingsEvent = null;

        private static XiaoYi_WebSocketService? _webSocketService = new XiaoYi_WebSocketService();
        private static XiaoYi_OTAService? _otaService = new XiaoYi_OTAService();

        public string DeivceId { 
            get { return _webSocketService.DeviceId; }
            set { _webSocketService.DeviceId = value; _otaService.DeviceId = value; } 
        }
        public string OTAUrl
        {
            get { return _otaService.OTA_VERSION_URL; }
            set { _otaService.OTA_VERSION_URL = value; }
        }
        public string WebSocketUrl {
            get { return _webSocketService.WebSocketUrl; }
            set { _webSocketService.WebSocketUrl = value; }
        }
        public string WebSocketToken
        {
            get { return _webSocketService.WebSocketToken; }
            set { _webSocketService.WebSocketToken = value; }
        }
        public string? IotThings { get; set; } = string.Empty;

        
        public XiaoYiAgent() { }
        public void Start()
        {
            _otaService.Start();

            //_webSocketService = new XiaoYi_WebSocketService();
            _webSocketService.OnMessageEvent += WebSocketService_OnMessageEvent;
            _webSocketService.OnAudioEvent += WebSocketService_OnAudioEvent;
            _webSocketService.OnIotThingsEvent += WebSocketService_OnIotThingsEvent;
            _webSocketService.IotThings = IotThings;
            _webSocketService.Start();

            
        }
        public void Restart()
        {
            _webSocketService.Restart();
        }
        private async Task WebSocketService_OnIotThingsEvent(string commands)
        {
            if (OnIotThingsEvent != null) { 
                await OnIotThingsEvent(commands);
            }
        }
        private async Task WebSocketService_OnAudioEvent(byte[] opus)
        {
            if (OnAudioEvent!=null) {
                await OnAudioEvent(opus);
            }
        }
        private async Task WebSocketService_OnMessageEvent(string type, string state, string message)
        {
            if (OnMessageEvent != null)
            {
                await OnMessageEvent(type, state, message);
            }
        }
        public async Task SendMessage(string message,string url="")
        {
            if (_webSocketService != null)
            {
                await _webSocketService.SendMessage(message, url);
            }
        }
        public async Task SendDeviceStatus(string status)
        {
            if (_webSocketService != null)
            {
                await _webSocketService.SendDeviceStatus(status);
            }
        }
        public async Task SendAudio(byte[] opusData)
        {
            if (_webSocketService != null)
            {
                await _webSocketService.SendAudio(opusData);
            }
        }
        public async Task StartRecording()
        {
            if (_webSocketService != null)
            {
                await _webSocketService.StartRecording();
            }
        }
        public async Task StartRecordingAuto()
        {
            if (_webSocketService != null)
            {
                await _webSocketService.StartRecordingAuto();
            }
        }
        public async Task StopRecording()
        {
            if (_webSocketService != null)
            {
                await _webSocketService.StopRecording();
            }
        }
        public async Task Abort() {
            if (_webSocketService != null)
            {
                await _webSocketService.Abort();
            }
        }
    }
}
