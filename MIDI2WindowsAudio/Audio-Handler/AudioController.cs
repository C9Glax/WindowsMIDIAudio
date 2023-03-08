using System.Globalization;
using CoreAudio;

namespace Audio_Handler;

public class AudioController
{
    private readonly object volumeController;
    public bool isDevice { get; }
    public bool isSession { get; }
    
    public AudioController? parentDeviceController { get; }

    private readonly HashSet<AudioController>? childrenSessionControllers;
    
    public float volume { get; private set; }
    public bool mute { get; private set; }
    public bool soloMute { get; private set; }
    public bool isSolo { get; set; }
    public string name { get; private set; }
    
    public string id { get; }
    
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
        id = device.ID;
        parentDeviceController = null;
        childrenSessionControllers = new HashSet<AudioController>();
        device.AudioEndpointVolume.OnVolumeNotification += DeviceVolumeChangeHandler;
    }

    public void AddSession(AudioController sessionController)
    {
        if (childrenSessionControllers is not null)
            childrenSessionControllers.Add(sessionController);
    }

    public AudioController[]? GetChildSessions()
    {
        return this.childrenSessionControllers?.ToArray();
    }

    private void DeviceVolumeChangeHandler(AudioVolumeNotificationData data)
    {
        this.SetVolume(data.MasterVolume);
        this.SetMute(data.Muted);
    }

    public AudioController(AudioSessionControl2 session, AudioController parentDeviceController)
    {
        volumeController = session.SimpleAudioVolume!;
        mute = session.SimpleAudioVolume!.Mute;
        volume = session.SimpleAudioVolume!.MasterVolume;
        soloMute = false;
        isSession = true;
        name = session.DisplayName != "" ? session.DisplayName : session.ProcessID.ToString();
        id = session.ProcessID.ToString();
        this.parentDeviceController = parentDeviceController;
        childrenSessionControllers = null;
        session.OnSimpleVolumeChanged += SessionVolumeChangeHandler;
        session.OnDisplayNameChanged += (_, displayName) => name = displayName;
    }

    private void SessionVolumeChangeHandler(object sender, float newVolume, bool newMute)
    {
        this.SetVolume(newVolume);
        this.SetMute(newMute);
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
        soloMute = setSoloMute;
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
        return
            $"{name.PadRight(40)[..40]} - " +
            $"Mute (D/S): ({(mute ? "T" : "F")}/{(soloMute ? "T" : "F")}) " +
            $"Vol: {(volume * 100).ToString(CultureInfo.InvariantCulture).PadLeft(5)[..5]} " +
            $"{(isSession ? "Session" : "")}{(isDevice ? "Device" : "")}";
    }
}