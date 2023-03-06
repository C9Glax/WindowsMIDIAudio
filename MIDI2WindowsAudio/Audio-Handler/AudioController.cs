using System.Diagnostics;
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
    public string name { get; private set; }
    
    public string ID { get; }
    
    public delegate void StateChangedEventHandler(AudioController sender);

    public event StateChangedEventHandler? OnStateChanged;

    public AudioController(MMDevice device)
    {
        volumeController = device.AudioEndpointVolume!;
        mute = device.AudioEndpointVolume!.Mute;
        volume = device.AudioEndpointVolume!.MasterVolumeLevel / 100;
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
        name = session.DisplayName != "" ? session.DisplayName : session.ProcessID.ToString();
        ID = session.ProcessID.ToString();
        session.OnSimpleVolumeChanged += SessionVolumeChangeHandler;
        session.OnDisplayNameChanged += (sender, displayName) => name = displayName;
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
            ((AudioEndpointVolume)volumeController).MasterVolumeLevelScalar = newVolume;
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

    public override string ToString()
    {
        return $"{name.PadLeft(40).Substring(0,40)} - Mute (D/S): ({(mute ? "T" : "F")}/{(soloMute ? "T" : "F")}) Vol: {(volume * 100).ToString().PadLeft(5).Substring(0,5)} {(isSession ? "Session" : "")}{(isDevice ? "Device" : "")}";
    }
}