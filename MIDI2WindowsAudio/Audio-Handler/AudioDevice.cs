using CoreAudio;
using CoreAudio.Interfaces;

namespace Audio_Handler;

public class AudioDevice
{
    private MMDevice mmdevice;
    public float volume { get; private set; }
    public bool muted { get; private set; }
    public bool groupMuted { get; private set; }
    public bool soloMuted { get; private set; }
    public string Name { get; }

    private Dictionary<uint, AudioSession> sessions = new();
    
    public delegate void StateChanged(object sender);

    public event StateChanged? OnStateChanged;
    

    public AudioDevice(MMDevice device)
    {
        mmdevice = device;
        volume = mmdevice.AudioEndpointVolume.MasterVolumeLevel;
        muted = mmdevice.AudioEndpointVolume.Mute;
        Name = mmdevice.DeviceFriendlyName;
        UpdateSessions();
        mmdevice.AudioEndpointVolume.OnVolumeNotification += OnVolumeChange;
        mmdevice.AudioSessionManager2.OnSessionCreated += AddSession;
    }

    private void AddSession(object sender, IAudioSessionControl2 newSession)
    {
        UpdateSessions();
    }

    public AudioSession[] GetSessions()
    {
        return sessions.Values.ToArray();
    }
    
    private void UpdateSessions()
    {
        foreach (AudioSessionControl2 session in mmdevice.AudioSessionManager2.Sessions)
        {
            if (!session.IsSystemSoundsSession && !sessions.ContainsKey(session.ProcessID) && session.State != AudioSessionState.AudioSessionStateExpired)
            {
                AudioSession audioSession = new AudioSession(session);
                audioSession.OnStateChanged += AudioSessionUpdate;
                sessions.Add(session.ProcessID, audioSession);
            }
        }
    }

    private void AudioSessionUpdate(object sender)
    {
        //TODO Check object type
        AudioSession audioSession = (AudioSession)sender;
        if (audioSession.sessionState == AudioSessionState.AudioSessionStateExpired)
        {
            sessions.Remove(audioSession.GetProcessId());
        }
    }

    private void OnVolumeChange(AudioVolumeNotificationData data)
    {
        volume = data.MasterVolume;
        muted = data.Muted;
        OnStateChanged?.Invoke(this);
    }
    
    public void SetVolumePercentage(float perc)
    {
        if(perc >= 0 && perc <= 1)
            mmdevice.AudioEndpointVolume.MasterVolumeLevelScalar = perc;
    }

    public void SetVolumeDecibel(float db)
    {
        AudioEndPointVolumeVolumeRange range = mmdevice.AudioEndpointVolume.VolumeRange;
        if (db >= range.MindB && db <= range.MaxdB)
            mmdevice.AudioEndpointVolume.MasterVolumeLevel = db;
    }

    public float GetMaxDB()
    {
        return mmdevice.AudioEndpointVolume.VolumeRange.MaxdB;
    }

    public float GetMinDB()
    {
        return mmdevice.AudioEndpointVolume.VolumeRange.MindB;
    }

    public void Mute(bool mute, bool soloMute)
    {
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
            mmdevice.AudioEndpointVolume.Mute = true;
            muted = true;
        }
        else
        {
            mmdevice.AudioEndpointVolume.Mute = false;
            muted = false;
        }
    }

    public override string ToString()
    {
        return string.Format("{0} - Mute (G/S): {1} ({2}/{3}) Volume: {4}", Name.PadRight(30).Substring(0,30), muted ? "muted" : "un-muted", groupMuted ? "t" : "f", soloMuted ? "t" : "f" ,volume.ToString());
    }
}