using Melanchall.DryWetMidi.Multimedia;

namespace MIDI_Handler;

public class MidiHandler
{
    public NanoKontrol2? NanoKontrol2 { get; }
    
    
    public MidiHandler()
    {
        Console.WriteLine("Input Devices:");
        InputDevice? nanoKontrolIn = null;
        InputDevice[] iDevices = InputDevice.GetAll().ToArray();
        foreach (InputDevice inputDevice in iDevices)
        {
            if (inputDevice.Name.Contains("nanoKONTROL2", StringComparison.InvariantCultureIgnoreCase))
            {
                Console.Write("-->");
                nanoKontrolIn = inputDevice;
            }
            Console.WriteLine(inputDevice.Name);
        }
        
        Console.WriteLine("Output Devices:");
        OutputDevice[] oDevices = OutputDevice.GetAll().ToArray();
        OutputDevice? nanoKontrolOut = null;
        foreach (OutputDevice outputDevice in oDevices)
        {
            if (outputDevice.Name.Contains("nanoKONTROL2", StringComparison.InvariantCultureIgnoreCase))
            {
                Console.Write("-->");
                nanoKontrolOut = outputDevice;
            }
            Console.WriteLine(outputDevice.Name);
        }

        if (nanoKontrolIn is not null && nanoKontrolOut is not null)
        {
            NanoKontrol2 = new NanoKontrol2(nanoKontrolIn, nanoKontrolOut);
            NanoKontrol2.OnControlChange += ControlChange;
        }
    }

    private void ControlChange(object sender, ControlChangeEventArgs eventArgs)
    {
        Console.WriteLine(eventArgs.ToString());
    }
}