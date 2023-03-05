using Audio_Handler;
using MIDI_Handler;

namespace KorgNanokontrol2MWAudio;

public class KorgNanokontrol2MWAudio
{

    public bool KeepRunning = true;
    private AudioDevice[] groupAssignment = new AudioDevice[8];

    public static void Main(string[] args)
    {
        var _ = new KorgNanokontrol2MWAudio();
    }

    public KorgNanokontrol2MWAudio()
    {
        NanoKontrol2 nk2 = MidiHandler.GetNanoKontrol2();
        nk2.OnControlChange += Nk2OnControlChange;
        WindowsAudioHandler windowsAudioHandler = new WindowsAudioHandler();
        for (int i = 0; i < groupAssignment.Length && i < windowsAudioHandler.outputs.Length; i++)
        {
            groupAssignment[i] = windowsAudioHandler.outputs[i];
            groupAssignment[i].OnStateChanged += OnAudioDeviceStateChanged;
            Console.WriteLine("{0}: {1}", i.ToString(), groupAssignment[i]);
        }
        Thread listen = new Thread(RunThreadRun);
        listen.Start();
    }

    private void OnAudioDeviceStateChanged(object sender)
    {
        AudioDevice audioDevice = (AudioDevice)sender;//TODO object type check
        byte group = byte.MaxValue;
        for (byte i = 0; i < groupAssignment.Length; i++)
        {
            if (groupAssignment[i] == audioDevice)
            {
                group = i;
                break;
            }
        }
        Console.WriteLine("Device changed: {0}", groupAssignment[group]);
        //TODO Flash LED
    }

    private void Nk2OnControlChange(object sender, ControlChangeEventArgs eventargs)
    {
        switch (eventargs.controlType)
        {
            case ControlChangeEventArgs.ControlType.ControlButton:
                ControlButtonPressed(eventargs);
                break;
            case ControlChangeEventArgs.ControlType.Fader:
                FaderMoved(eventargs);
                break;
            case ControlChangeEventArgs.ControlType.MuteButton:
                MuteButtonPressed(eventargs);
                break;
            case ControlChangeEventArgs.ControlType.SoloButton:
                SoloButtonPressed(eventargs);
                break;
            default:
                Console.WriteLine(eventargs.ToString());
                break;
        }
    }

    private void FaderMoved(ControlChangeEventArgs args)
    {
        if (args.groupNumber <= groupAssignment.Length - 1)
        {
            float newVolume = args.value / NanoKontrol2.MaxValue;
            groupAssignment[args.groupNumber].SetVolumePercentage(newVolume);
        }
    }

    private void MuteButtonPressed(ControlChangeEventArgs args)
    {
        if (args.groupNumber <= groupAssignment.Length - 1)
        {
            groupAssignment[args.groupNumber].Mute(!groupAssignment[args.groupNumber].groupMuted, false);
        }
    }

    private void SoloButtonPressed(ControlChangeEventArgs args)
    {
        for (byte i = 0; i < groupAssignment.Length; i++)
        {
            if (i != args.groupNumber)
            {
                groupAssignment[i].Mute((args.value != 0), true);
            }
            else
            {
                groupAssignment[i].Mute(false, true);
            }
        }
    }

    private void ControlButtonPressed(ControlChangeEventArgs args)
    {
        if (args.value != 0)
            return;
        switch (args.controlButtonName)
        {
            case ControlChangeEventArgs.ControlButtonName.Play:
                MediaController.PlayPause();
                break;
            case ControlChangeEventArgs.ControlButtonName.Previous:
                MediaController.Previous();
                break;
            case ControlChangeEventArgs.ControlButtonName.Next:
                MediaController.Next();
                break;
            case ControlChangeEventArgs.ControlButtonName.Stop:
                MediaController.Stop();
                break;
            default:
                Console.WriteLine(args);
                break;
        }
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
}