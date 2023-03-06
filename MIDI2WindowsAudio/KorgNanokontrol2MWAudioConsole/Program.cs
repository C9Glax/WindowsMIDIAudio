using Audio_Handler;
using MIDI_Handler;

namespace KorgNanokontrol2MWAudio;

public class Program
{
    public static void Main(string[] args)
    {
        new Program();
    }

    public Program()
    {
        Korg2Audio k2a = new Korg2Audio();
        k2a.OnNanoKontrol2Event += (sender, eventargs) => Console.WriteLine($"{eventargs}");
        k2a.OnAudioControllerStateChanged += sender => Console.WriteLine(sender);
    }
}