using System.Runtime.InteropServices;
using XiaoYiSharp;

namespace XiaoYiSharp_MauiBlazorApp.Services
{
    public class XiaoYi_AgentService
    {
        private readonly XiaoYiAgent _xiaoyiAgent;
        private readonly IAudioService _audioService;
        private readonly AudioOpusService _audioOpusService;
        public string QuestionMessae = " ";
        public string AnswerMessae = " ";
        public string Emotion = " ";
        public string EmotionText = "😊";
        public bool IsAutoRecording = false;
        public bool IsRecording { get { return _audioService.IsRecording; } }
        public XiaoYi_AgentService()
        {
            if (DeviceInfo.Platform == DevicePlatform.Android)
            {
                //_audioService = DependencyService.Get<IAudioService>();
                _audioService  = new AudioService();
                _audioService.OnPcmAudioEvent += AudioService_OnPcmAudioEvent;
            }
            if (DeviceInfo.Platform == DevicePlatform.iOS)
            { 
            
            }
            _audioOpusService = new AudioOpusService();

            _xiaoyiAgent = new XiaoYiSharp.XiaoYiAgent();
            _xiaoyiAgent.DeivceId = Global.DeivceId;
            _xiaoyiAgent.OTAUrl = Global.OTAUrl;
            _xiaoyiAgent.WebSocketUrl = Global.WebSocketUrl;
            _xiaoyiAgent.WebSocketToken = Global.WebSocketToken;
            _xiaoyiAgent.OnMessageEvent += XiaoyiAgent_OnMessageEvent;
            _xiaoyiAgent.OnAudioEvent += XiaoyiAgent_OnAudioEvent;
            _xiaoyiAgent.OnIotThingsEvent += XiaoyiAgent_OnIotThingsEvent;
            _xiaoyiAgent.IotThings = "[{\"name\":\"Speaker\",\"description\":\"当前 AI 机器人的扬声器\",\"properties\":{\"volume\":{\"description\":\"当前音量值\",\"type\":\"number\"}},\"methods\":{\"SetVolume\":{\"description\":\"设置音量\",\"parameters\":{\"volume\":{\"description\":\"0到100之间的整数\",\"type\":\"number\"}}}}},{\"name\":\"Backlight\",\"description\":\"当前 AI 机器人屏幕的亮度\",\"properties\":{\"brightness\":{\"description\":\"当前亮度值\",\"type\":\"number\"}},\"methods\":{\"SetBrightness\":{\"description\":\"设置亮度\",\"parameters\":{\"brightness\":{\"description\":\"0到100之间的整数\",\"type\":\"number\"}}}}}]";
            _xiaoyiAgent.Start();

        }
        public void Resatrt()
        {
            _xiaoyiAgent.DeivceId = Global.DeivceId;
            _xiaoyiAgent.OTAUrl = Global.OTAUrl;
            _xiaoyiAgent.WebSocketUrl = Global.WebSocketUrl;
            _xiaoyiAgent.WebSocketToken = Global.WebSocketToken;
            _xiaoyiAgent.IotThings = "[{\"name\":\"Speaker\",\"description\":\"当前 AI 机器人的扬声器\",\"properties\":{\"volume\":{\"description\":\"当前音量值\",\"type\":\"number\"}},\"methods\":{\"SetVolume\":{\"description\":\"设置音量\",\"parameters\":{\"volume\":{\"description\":\"0到100之间的整数\",\"type\":\"number\"}}}}},{\"name\":\"Backlight\",\"description\":\"当前 AI 机器人屏幕的亮度\",\"properties\":{\"brightness\":{\"description\":\"当前亮度值\",\"type\":\"number\"}},\"methods\":{\"SetBrightness\":{\"description\":\"设置亮度\",\"parameters\":{\"brightness\":{\"description\":\"0到100之间的整数\",\"type\":\"number\"}}}}}]";
            _xiaoyiAgent.Restart();
        }
        private async Task AudioService_OnPcmAudioEvent(byte[] pcm)
        {
            List<byte[]> opusList = _audioOpusService.ConvertPcmToOpus(pcm);
            foreach (var opus in opusList)
            {
                await _xiaoyiAgent.SendAudio(opus);
                //await Task.Delay(10);
            }
        }
        private async Task XiaoyiAgent_OnIotThingsEvent(string commands)
        {
            //throw new NotImplementedException();
        }
        private async Task XiaoyiAgent_OnAudioEvent(byte[] opus)
        {
            if (_audioService != null && _audioOpusService != null)
            {
                byte[] pcmData = _audioOpusService.ConvertOpusToPcm(opus);
                _audioService.AddOutSamples(pcmData);
            }
        }
        private async Task XiaoyiAgent_OnMessageEvent(string type, string state, string message)
        {
            if (type == "STT")
            {
                if(!string.IsNullOrEmpty(message))
                    QuestionMessae = message;
                //await StopRecording();
            }
            if (type == "TTS")
            {
                if (state == "start")
                {
                    
                }
                if (state == "stop")
                {
                    if (IsAutoRecording)
                        await AutoRecording();
                }
                else
                {
                    AnswerMessae = message;
                }
            }
            if (type == "LLM") { 
                EmotionText = message;
                Emotion = state;
            }
        }
        public async Task SendMessage(string text)
        {
            if (_xiaoyiAgent != null)
            {
                await _xiaoyiAgent.SendMessage(text);
            }
        }
        public async Task StartRecording()
        {
            await Task.Run(async () =>
            {
                await _xiaoyiAgent.StartRecording();
                _audioService.StartRecording();
            });
        }
        public async Task StartRecordingAuto()
        {
            await Task.Run(async () =>
            {
                await _xiaoyiAgent.StartRecordingAuto();
                _audioService.StartRecording();
            });
        }
        public async Task StopRecording()
        {
            await Task.Run(async () =>
            {
                await _xiaoyiAgent.StopRecording();
                _audioService.StopRecording();
            });

        }
        public async Task AutoRecording()
        {
            if (IsAutoRecording)
            {
                await StartRecordingAuto();
            }
            else
            {
                await StopRecording();
            }
        }
        public async Task Abort()
        {
            await _xiaoyiAgent.Abort();
        }
    }
}