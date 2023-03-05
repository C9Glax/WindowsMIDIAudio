using Audio_Handler;
using MIDI_Handler;

namespace KorgNanokontrol2MWAudio;

public class KorgNanokontrol2MWAudio
{

    public bool KeepRunning = true;
    private BindingsManager bindings;
    private WindowsAudioHandler windowsAudioHandler;

    public static void Main(string[] args)
    {
        var _ = new KorgNanokontrol2MWAudio();
    }

    public KorgNanokontrol2MWAudio()
    {
        NanoKontrol2 nk2 = MidiHandler.GetNanoKontrol2();
        nk2.OnControlChange += Nk2OnControlChange;
        windowsAudioHandler = new WindowsAudioHandler();
        bindings = new BindingsManager();
        SampleBindings();
        foreach (AudioDevice ad in windowsAudioHandler.outputs)
            ad.OnStateChanged += OnAudioDeviceStateChanged;
        
        Thread listen = new Thread(RunThreadRun);
        listen.Start();
    }

    private void OnAudioDeviceStateChanged(object sender)
    {
        AudioDevice audioDevice = (AudioDevice)sender;//TODO object type check
        Console.WriteLine("Device changed: {0}", audioDevice);
        //TODO Flash LED
    }

    private void Nk2OnControlChange(object sender, ControlChangeEventArgs eventargs)
    {
        Console.WriteLine(eventargs);
        byte control = Convert.ToByte(eventargs.absoluteControlNumber);
        bindings.ExecuteBinding(control, eventargs);
        
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
        for (int i = 0; i < windowsAudioHandler.outputs.Length; i++)
        {
            bindings.AddBinding(Convert.ToByte(i), new Binding(Binding.AudioAction.SetVolume, windowsAudioHandler.outputs[i]));
            bindings.AddBinding(Convert.ToByte(i + 32), new Binding(Binding.AudioAction.Solo, windowsAudioHandler.outputs[i]));
            bindings.AddBinding(Convert.ToByte(i + 48), new Binding(Binding.AudioAction.Mute, windowsAudioHandler.outputs[i]));
        }
        bindings.AddBinding(43, new Binding(Binding.AudioAction.PreviousTrack));
        bindings.AddBinding(44, new Binding(Binding.AudioAction.NextTrack));
        bindings.AddBinding(42, new Binding(Binding.AudioAction.Stop));
        bindings.AddBinding(41, new Binding(Binding.AudioAction.PlayPause));
        Console.WriteLine(bindings);
    }
}