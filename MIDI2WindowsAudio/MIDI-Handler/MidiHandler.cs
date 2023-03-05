using Melanchall.DryWetMidi.Multimedia;

namespace MIDI_Handler;

public static class MidiHandler
{
    public static NanoKontrol2 GetNanoKontrol2()
    {
        //Console.WriteLine("Input Devices:");
        InputDevice? nanoKontrolIn = null;
        InputDevice[] iDevices = InputDevice.GetAll().ToArray();
        foreach (InputDevice inputDevice in iDevices)
        {
            if (inputDevice.Name.Contains("nanoKONTROL2", StringComparison.InvariantCultureIgnoreCase))
            {
                //Console.Write("-->");
                nanoKontrolIn = inputDevice;
            }
            //Console.WriteLine(inputDevice.Name);
        }
        
        //Console.WriteLine("Output Devices:");
        OutputDevice[] oDevices = OutputDevice.GetAll().ToArray();
        OutputDevice? nanoKontrolOut = null;
        foreach (OutputDevice outputDevice in oDevices)
        {
            if (outputDevice.Name.Contains("nanoKONTROL2", StringComparison.InvariantCultureIgnoreCase))
            {
                //Console.Write("-->");
                nanoKontrolOut = outputDevice;
            }
            //Console.WriteLine(outputDevice.Name);
        }

        if (nanoKontrolIn is not null && nanoKontrolOut is not null)
        {
            return new NanoKontrol2(nanoKontrolIn, nanoKontrolOut);
        }
        else
        {
            throw new Exception();//TODO
        }
    }
}