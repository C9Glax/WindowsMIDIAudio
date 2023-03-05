using System.Globalization;
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
        if (mmsession.SimpleAudioVolume != null)
        {
            volume = mmsession.SimpleAudioVolume.MasterVolume;
            muted = mmsession.SimpleAudioVolume.Mute;
        }
        else throw new Exception(); //TODO

        mmsession.OnSimpleVolumeChanged += VolumeChanged;
        mmsession.OnStateChanged += SessionStateChanged;
    }

    private void SessionStateChanged(object sender, AudioSessionState newState)
    {
        sessionState = newState;
        OnStateChanged?.Invoke(this);
    }

    private void VolumeChanged(object sender, float newVolume, bool newMutedState)
    {
        volume = newVolume;
        muted = newMutedState;
        OnStateChanged?.Invoke(this);
    }

    public string GetName()
    {
        return mmsession.DisplayName;
    }

    public void SetVolumePercentage(float perc)
    {
        if (mmsession.SimpleAudioVolume != null)
        {
            if (perc >= 0 && perc <= 1)
                mmsession.SimpleAudioVolume.MasterVolume = perc;
        }
        else throw new Exception(); //TODO

    }

    public void Mute(bool mute)
    {
        if (mmsession.SimpleAudioVolume != null)
            mmsession.SimpleAudioVolume.Mute = mute;
        else throw new Exception(); //TODO
    }

    public uint GetProcessId()
    {
        return mmsession.ProcessID;
    }

    public override string ToString()
    {
        return string.Format("{0} Id: {3} State: {1} Volume: {2}", GetName(), muted ? "muted" : "un-muted", volume.ToString(CultureInfo.CurrentCulture), GetProcessId().ToString());
    }
}