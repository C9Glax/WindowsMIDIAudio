using Audio_Handler;
using MIDI_Handler;

namespace Korg2Audio;

public class KorgAndAudioKonnector
{

    // ReSharper disable once InconsistentNaming
    public bool KeepRunning = true;
    public readonly WindowsAudioHandler windowsAudioHandler;
    public readonly NanoKontrol2 nanoKontrol2;
    public Bindings bindings { get; }
    public delegate void WindowsAudioControllerStateChangedEventHandler(AudioController sender);
    public event WindowsAudioControllerStateChangedEventHandler? OnAudioControllerStateChanged;
    
    public delegate void NanoKontrol2EventEventHandler(object sender, ControlChangeEventArgs eventArgs);
    public event NanoKontrol2EventEventHandler? OnNanoKontrol2Event;
    public KorgAndAudioKonnector()
    {
        nanoKontrol2 = MidiHandler.GetNanoKontrol2();
        nanoKontrol2.OnControlChange += Nk2ControlEventHandler;
        windowsAudioHandler = new WindowsAudioHandler();
        windowsAudioHandler.OnAudioControllerStateChanged += WindowsAudioControllerStateChangeHandler;
        bindings = new();
        SampleBindings();
        Console.WriteLine(windowsAudioHandler);
        Console.WriteLine(bindings);
        
        Thread listen = new Thread(RunThreadRun);
        listen.Start();
    }

    private void WindowsAudioControllerStateChangeHandler(AudioController sender)
    {
        OnAudioControllerStateChanged?.Invoke(sender);
    }

    private void Nk2ControlEventHandler(object sender, ControlChangeEventArgs eventArgs)
    {
        Bindings.ControllerActions? actionExecuted =
            bindings.ExecuteControllerBinding(Convert.ToByte(eventArgs.absoluteControlNumber),
                Convert.ToByte(eventArgs.value));
        
        if (actionExecuted is Bindings.ControllerActions.Solo && bindings.groupAssignment[eventArgs.groupNumber] is { } soloController)
        {
            HashSet<AudioController> relatedControllers = new HashSet<AudioController>()
            {
                soloController
            };
            if (soloController.parentDeviceController is { } parentAudioController)
                relatedControllers.Add(parentAudioController);
            if(soloController.isDevice)
                foreach (AudioController audioController in soloController.GetChildSessions()!)
                    relatedControllers.Add(audioController);

            soloController.isSolo = soloController.soloMute || !soloController.isSolo;
            foreach (AudioController audioController in relatedControllers)
            {
                audioController.SetSoloMute(false);
            }
            
            foreach (AudioController audioController in windowsAudioHandler.controllers.Except(relatedControllers))
            {
                audioController.SetSoloMute(soloController.isSolo);
                audioController.isSolo = false;
            }
        }
        OnNanoKontrol2Event?.Invoke(sender, eventArgs);
    }

    private void RunThreadRun()
    {
        while (KeepRunning)
        {
            Thread.Sleep(1000);
        }
    }

    public void Dispose()
    {
        KeepRunning = false;
        nanoKontrol2.Dispose();
        windowsAudioHandler.Dispose();
    }

    public void SetBindingsForGroup(byte group, AudioController audioController)
    {
        bindings.AddControlBinding(Convert.ToByte(group + 00), Bindings.ControllerActions.SetVolume, audioController);
        //bindings.AddControlBinding(Convert.ToByte(i + 16), Bindings.ControllerActions.SetVolume, controllers[group]);
        bindings.AddControlBinding(Convert.ToByte(group + 32), Bindings.ControllerActions.Solo, audioController);
        bindings.AddControlBinding(Convert.ToByte(group + 48), Bindings.ControllerActions.Mute, audioController);
        //bindings.AddControlBinding(Convert.ToByte(group + 48), Bindings.ControllerActions.Record, controllers[group]);
        bindings.SetGroup(audioController, group);
    }

    private void SampleBindings()
    {
        AudioController[] controllers = windowsAudioHandler.controllers.Where(controller => !controller.name.All(char.IsDigit)).ToArray();
        for (byte i = 0; i < 8 && i < controllers.Length; i++)
        {
            SetBindingsForGroup(i, controllers[i]);
        }
        bindings.AddControlBinding(41, Bindings.ControllerActions.PlayPause, null);
        bindings.AddControlBinding(42, Bindings.ControllerActions.Stop, null);
        bindings.AddControlBinding(43, Bindings.ControllerActions.Previous, null);
        bindings.AddControlBinding(44, Bindings.ControllerActions.Next, null);

        for (byte i = 0; i < bindings.groupAssignment.Length && i < controllers.Length; i++)
        {
            bindings.SetGroup(controllers[i], i);
        }
    }
}