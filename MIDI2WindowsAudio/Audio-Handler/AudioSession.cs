using System.Globalization;
using CoreAudio;

namespace Audio_Handler;

public class AudioSession
{
    private AudioSessionControl2 mmsession;
    public float volume { get; private set; }
    public bool muted { get; private set; }
    public bool groupMuted { get; private set; }
    public bool soloMuted { get; private set; }
    public string name { get; }

    public AudioSessionState sessionState { get; private set; }

    public delegate void StateChanged(object sender);

    public event StateChanged? OnStateChanged;
    
    public AudioSession(AudioSessionControl2 session)
    {
        mmsession = session;
        name = mmsession.DisplayName;
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
    
    public void Mute(bool mute, bool soloMute)
    {
        if (mmsession.SimpleAudioVolume == null)
            throw new Exception(); //TODO
        if (soloMute)
        {
            soloMuted = mute;
        }
        else
        {
            groupMuted = mute;
        }

        if (soloMuted || groupMuted)
        {
            mmsession.SimpleAudioVolume.Mute = true;
            muted = true;
        }
        else
        {
            mmsession.SimpleAudioVolume.Mute = false;
            muted = false;
        }
    }

    public uint GetProcessId()
    {
        return mmsession.ProcessID;
    }

    public override string ToString()
    {
        return string.Format("{0} Id: {3} State: {1} Volume: {2}", GetName().PadRight(40).Substring(0, 40), muted ? "muted" : "un-muted", volume.ToString(CultureInfo.CurrentCulture), GetProcessId().ToString());
    }
}