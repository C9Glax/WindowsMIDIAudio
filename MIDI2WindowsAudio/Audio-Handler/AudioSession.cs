using CoreAudio;

namespace Audio_Handler;

public class AudioSession
{
    private AudioSessionControl2 mmsession;
    public float volume { get; private set; }
    public bool muted { get; private set; }

    public AudioSessionState sessionState { get; private set; }

    public delegate void StateChanged(object sender);

    public event StateChanged? OnStateChanged;
    
    public AudioSession(AudioSessionControl2 session)
    {
        mmsession = session;
        volume = mmsession.SimpleAudioVolume.MasterVolume;
        muted = mmsession.SimpleAudioVolume.Mute;
        mmsession.OnSimpleVolumeChanged += VolumeChanged;
        mmsession.OnStateChanged += SessionStateChanged;
    }

    private void SessionStateChanged(object sender, AudioSessionState newState)
    {
        sessionState = newState;
        OnStateChanged?.Invoke(this);
    }

    private void VolumeChanged(object sender, float volume, bool muted)
    {
        this.volume = volume;
        this.muted = muted;
        OnStateChanged?.Invoke(this);
    }

    public string GetName()
    {
        return mmsession.DisplayName;
    }

    public void SetVolumePercentage(float perc)
    {
        if (perc >= 0 && perc <= 1)
            mmsession.SimpleAudioVolume.MasterVolume = perc;
    }

    public void Mute(bool mute)
    {
        mmsession.SimpleAudioVolume.Mute = mute;
    }

    public uint GetProcessId()
    {
        return mmsession.ProcessID;
    }

    public override string ToString()
    {
        return string.Format("{0} Id: {3} State: {1} Volume: {2}", GetName(), muted ? "muted" : "un-muted", volume.ToString(), GetProcessId().ToString());
    }
}