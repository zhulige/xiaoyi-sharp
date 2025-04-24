namespace XiaoYiSharp_BlazorApp.Services
{
    public interface IAudioService
    {
        delegate Task PcmAudioEventHandler(byte[] pcm);
        event PcmAudioEventHandler? OnPcmAudioEvent;
        bool IsPlaying { get; }
        bool IsRecording { get; }
        void StartRecording();
        void StopRecording();
        void StartPlaying();
        void StopPlaying();
        void AddOutSamples(byte[] pcmData);
        void AddOutSamples(float[] pcmData);
    }
}
