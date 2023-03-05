using System.Globalization;
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
    public string name { get; }

    private Dictionary<uint, AudioSession> sessions = new();
    
    public delegate void StateChanged(object sender);

    public event StateChanged? OnStateChanged;
    

    public AudioDevice(MMDevice device)
    {
        mmdevice = device;
        if (mmdevice.AudioEndpointVolume != null)
        {
            volume = mmdevice.AudioEndpointVolume.MasterVolumeLevel;
            muted = mmdevice.AudioEndpointVolume.Mute;
            name = mmdevice.DeviceFriendlyName;
            UpdateSessions();
            mmdevice.AudioEndpointVolume.OnVolumeNotification += OnVolumeChange;
        }
        else
        {
            throw new Exception(); //TODO
        }

        if (mmdevice.AudioSessionManager2 != null)
            mmdevice.AudioSessionManager2.OnSessionCreated += AddSession;
        else throw new Exception(); //TODO
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
        if (mmdevice.AudioSessionManager2 != null && mmdevice.AudioSessionManager2.Sessions != null)
            foreach (AudioSessionControl2 session in mmdevice.AudioSessionManager2.Sessions)
            {
                if (!session.IsSystemSoundsSession && !sessions.ContainsKey(session.ProcessID) &&
                    session.State != AudioSessionState.AudioSessionStateExpired)
                {
                    AudioSession audioSession = new AudioSession(session);
                    audioSession.OnStateChanged += AudioSessionUpdate;
                    sessions.Add(session.ProcessID, audioSession);
                }
            }
        else throw new Exception(); //TODO
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
        if (mmdevice.AudioEndpointVolume != null)
        {
            if (perc >= 0 && perc <= 1)
                mmdevice.AudioEndpointVolume.MasterVolumeLevelScalar = perc;
        }
        else throw new Exception(); //TODO
    }

    public void SetVolumeDecibel(float db)
    {
        if (mmdevice.AudioEndpointVolume != null)
        {
            AudioEndPointVolumeVolumeRange range = mmdevice.AudioEndpointVolume.VolumeRange;
            if (db >= range.MindB && db <= range.MaxdB)
                mmdevice.AudioEndpointVolume.MasterVolumeLevel = db;
        }
        else throw new Exception(); //TODO
    }

    public float GetMaxDb()
    {
        if (mmdevice.AudioEndpointVolume != null)
            return mmdevice.AudioEndpointVolume.VolumeRange.MaxdB;
        else throw new Exception(); //TODO
    }

    public float GetMinDb()
    {
        if (mmdevice.AudioEndpointVolume != null)
            return mmdevice.AudioEndpointVolume.VolumeRange.MindB;
        else throw new Exception(); //TODO
    }

    public void Mute(bool mute, bool soloMute)
    {
        if (mmdevice.AudioEndpointVolume == null)
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
        return
            $"{name.PadRight(40).Substring(0, 40)} - Mute (G/S): {(muted ? "muted" : "un-muted")} ({(groupMuted ? "t" : "f")}/{(soloMuted ? "t" : "f")}) Volume: {volume.ToString(CultureInfo.CurrentCulture)}";
    }
}