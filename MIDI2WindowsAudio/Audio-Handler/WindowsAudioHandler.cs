using CoreAudio;
using CoreAudio.Interfaces;

namespace Audio_Handler;

public class WindowsAudioHandler
{
    public HashSet<AudioController> controllers { get; private set; }

    public delegate void AudioControllerStateChangedEventHandler(AudioController sender);
    public event AudioControllerStateChangedEventHandler? OnAudioControllerStateChanged;
    public delegate void AudioControllerRemovedEventHandler(AudioController removed);
    public event AudioControllerRemovedEventHandler? OnAudioControllerRemoved;
    public delegate void AudioControllerAddedEventHandler(AudioController removed);
    public event AudioControllerAddedEventHandler? OnAudioControllerAdded;
    
    public WindowsAudioHandler()
    {
        Guid guid = Guid.NewGuid();
        MMDeviceCollection mmOutputs = new MMDeviceEnumerator(guid).EnumerateAudioEndPoints(DataFlow.Render, DeviceState.Active);
        MMDeviceCollection mmInputs = new MMDeviceEnumerator(guid).EnumerateAudioEndPoints(DataFlow.Capture, DeviceState.Active);

        controllers = new();
        foreach (MMDevice dev in mmOutputs)
        {
            AudioController newController = new AudioController(dev);
            controllers.Add(newController);
            if (dev.AudioSessionManager2 != null)
            {
                dev.AudioSessionManager2.OnSessionCreated += NewSessionCreatedHandler;
                foreach (AudioSessionControl2 session in dev.AudioSessionManager2.Sessions!)
                {
                    if (!session.IsSystemSoundsSession)
                    {
                        controllers.Add(new AudioController(session));
                        session.OnStateChanged += SessionStateChangedHandler;
                    }
                }
            }
        }
        
        foreach (MMDevice dev in mmInputs)
        {
            AudioController newController = new AudioController(dev);
            controllers.Add(newController);
            if (dev.AudioSessionManager2 != null)
            {
                dev.AudioSessionManager2.OnSessionCreated += NewSessionCreatedHandler;
                foreach (AudioSessionControl2 session in dev.AudioSessionManager2.Sessions!)
                {
                    if (!session.IsSystemSoundsSession)
                    {
                        controllers.Add(new AudioController(session));
                        session.OnStateChanged += SessionStateChangedHandler;
                    }
                }
            }
        }

        foreach (AudioController audioController in controllers)
        {
            audioController.OnStateChanged += AudioControllerOnStateChanged;
        }
    }

    private void AudioControllerOnStateChanged(AudioController sender)
    {
        OnAudioControllerStateChanged?.Invoke(sender);
    }

    private void SessionStateChangedHandler(object sender, AudioSessionState newState)
    {
        if (newState == AudioSessionState.AudioSessionStateExpired)
        {
            AudioSessionControl2 session = (AudioSessionControl2)sender; //TODO check type
            OnAudioControllerRemoved?.Invoke(controllers.First(audioController => audioController.id == session.ProcessID.ToString()));
            controllers.RemoveWhere(audioController => audioController.id == session.ProcessID.ToString());
        }
    }

    private void NewSessionCreatedHandler(object sender, IAudioSessionControl2 newSession)
    {
        Console.WriteLine(sender);
        //OnAudioControllerAdded?.Invoke();
        throw new NotImplementedException(); //TODO
    }

    public override string ToString()
    {
        string ret = $"[WindowsAudioHandler] {controllers.Count} Controllers:";
        foreach (AudioController audioController in controllers)
        {
            ret += $"\n{audioController}";
        }
        return ret;
    }

    public void Dispose()
    {
        foreach (AudioController controller in controllers)
        {
            controller.OnStateChanged -= AudioControllerOnStateChanged;
        }
        controllers = new HashSet<AudioController>();
        GC.Collect();
    }
}