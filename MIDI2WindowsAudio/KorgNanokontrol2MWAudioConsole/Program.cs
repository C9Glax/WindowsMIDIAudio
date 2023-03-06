using Korg2Audio;

namespace KorgNanokontrol2MWAudio;

public class Program
{
    public static void Main(string[] args)
    {
        var _ = new Program();
    }

    private Program()
    {
        // ReSharper disable once InconsistentNaming
        KorgAndAudioKonnector k2a = new KorgAndAudioKonnector();
        k2a.OnNanoKontrol2Event += (_, eventArgs) => Console.WriteLine($"{eventArgs}");
        k2a.OnAudioControllerStateChanged += Console.WriteLine;
    }
}