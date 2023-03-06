using CoreAudio;

namespace Audio_Handler;

public class AudioController
{
    private object volumeController;
    private bool isDevice = false;
    private bool isSession = false;
    
    public float volume { get; private set; }
    public bool mute { get; private set; }
    public bool soloMute { get; private set; }
    public string name { get; }
    
    public string ID { get; }
    
    public delegate void StateChangedEventHandler(AudioController sender);

    public event StateChangedEventHandler? OnStateChanged;

    public AudioController(MMDevice device)
    {
        volumeController = device.AudioEndpointVolume!;
        mute = device.AudioEndpointVolume!.Mute;
        volume = device.AudioEndpointVolume!.MasterVolumeLevel;
        soloMute = false;
        isDevice = true;
        name = device.DeviceFriendlyName;
        ID = device.ID;
        device.AudioEndpointVolume.OnVolumeNotification += DeviceVolumeChangeHandler;
    }

    private void DeviceVolumeChangeHandler(AudioVolumeNotificationData data)
    {
        this.SetVolume(data.MasterVolume);
        this.SetMute(data.Muted);
        OnStateChanged?.Invoke(this);
    }

    public AudioController(AudioSessionControl2 session)
    {
        volumeController = session.SimpleAudioVolume!;
        mute = session.SimpleAudioVolume!.Mute;
        volume = session.SimpleAudioVolume!.MasterVolume;
        soloMute = false;
        isSession = true;
        name = session.DisplayName;
        ID = session.ProcessID.ToString();
        session.OnSimpleVolumeChanged += SessionVolumeChangeHandler;
    }

    private void SessionVolumeChangeHandler(object sender, float newVolume, bool newMute)
    {
        this.SetVolume(newVolume);
        this.SetMute(newMute);
        OnStateChanged?.Invoke(this);
    }

    public void SetVolume(float newVolume)
    {
        if (isDevice)
        {
            ((AudioEndpointVolume)volumeController).MasterVolumeLevel = newVolume;
            volume = newVolume;
        }else if (isSession)
        {
            ((SimpleAudioVolume)volumeController).MasterVolume = newVolume;
            volume = newVolume;
        }
        OnStateChanged?.Invoke(this);
    }

    public void SetMute(bool setMute)
    {
        this.mute = setMute;
        UpdateMuteStatus();
    }

    public void SetSoloMute(bool setSoloMute)
    {
        this.soloMute = setSoloMute;
        UpdateMuteStatus();
    }

    private void UpdateMuteStatus()
    {
        if (mute || soloMute)
        {
            if (isDevice)
            {
                ((AudioEndpointVolume)volumeController).Mute = true;
            }else if (isSession)
            {
                ((SimpleAudioVolume)volumeController).Mute = true;
            }
        }
        else
        {
            if (isDevice)
            {
                ((AudioEndpointVolume)volumeController).Mute = false;
            }else if (isSession)
            {
                ((SimpleAudioVolume)volumeController).Mute = false;
            }
        }
        OnStateChanged?.Invoke(this);
    }

    public void InvokeStateChange()
    {
        OnStateChanged?.Invoke(this);
    }

    public override string ToString()
    {
        return $"{name.PadRight(40).Substring(0, 40)} - Mute (D/S): ({mute}/{soloMute}) Vol: {volume} {(isSession ? "Session" : "")}{(isDevice ? "Device" : "")}";
    }
}