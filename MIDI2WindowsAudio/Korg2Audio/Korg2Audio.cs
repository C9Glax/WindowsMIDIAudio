using Audio_Handler;
using MIDI_Handler;

namespace KorgNanokontrol2MWAudio;

public class Korg2Audio
{

    public bool KeepRunning = true;
    private WindowsAudioHandler windowsAudioHandler;
    private NanoKontrol2 nanoKontrol2;
    public Bindings bindings { get; }
    public delegate void WindowsAudioControllerStateChangedEventHandler(AudioController sender);
    public event WindowsAudioControllerStateChangedEventHandler? OnAudioControllerStateChanged;
    
    public delegate void NanoKontrol2EventEventHandler(object sender, ControlChangeEventArgs eventargs);
    public event NanoKontrol2EventEventHandler? OnNanoKontrol2Event;
    public Korg2Audio()
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

    private void Nk2ControlEventHandler(object sender, ControlChangeEventArgs eventargs)
    {
        bindings.ExecuteControllerBinding(Convert.ToByte(eventargs.absoluteControlNumber), Convert.ToByte(eventargs.value));
        OnNanoKontrol2Event?.Invoke(sender, eventargs);
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
    }

    private void SampleBindings()
    {
        AudioController[] controllers = windowsAudioHandler.controllers.ToArray();
        for (byte i = 0; i < 8 && i < controllers.Length; i++)
        {
            bindings.AddControlBinding(Convert.ToByte(i + 00), Bindings.ControllerActions.SetVolume, controllers[i]);
            //bindings.AddControlBinding(Convert.ToByte(i + 16), Bindings.ControllerActions.SetVolume, controllers[i]);
            bindings.AddControlBinding(Convert.ToByte(i + 32), Bindings.ControllerActions.Solo, controllers[i]);
            bindings.AddControlBinding(Convert.ToByte(i + 48), Bindings.ControllerActions.Mute, controllers[i]);
            //bindings.AddControlBinding(Convert.ToByte(i + 48), Bindings.ControllerActions.Record, controllers[i]);
        }
        bindings.AddControlBinding(41, Bindings.ControllerActions.PlayPause, null);
        bindings.AddControlBinding(42, Bindings.ControllerActions.Stop, null);
        bindings.AddControlBinding(43, Bindings.ControllerActions.Previous, null);
        bindings.AddControlBinding(44, Bindings.ControllerActions.Next, null);

        for (byte i = 0; i < bindings.groupAssignment.Length; i++)
        {
            bindings.SetGroup(controllers[i], i);
        }
    }
}