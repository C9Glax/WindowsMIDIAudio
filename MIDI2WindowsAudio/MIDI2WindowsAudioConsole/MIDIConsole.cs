using System;
using MidiAccess;
using MIDI2WindowsAudio;
using WindowsSoundControl;

namespace MIDI2WindowsAudioConsole
{
    class MIDIConsole
    {

        public MIDIConsole()
        {
            Console.WriteLine("Select MIDI-Input Device:");
            string[] midiInputDevices = MidiInformation.ListInputDevices();
            for (ushort ini = 0; ini < midiInputDevices.Length; ini++)
                Console.WriteLine("{0}) {1}", ini + 1, midiInputDevices[ini]);
            string midiInDevice = midiInputDevices[Convert.ToInt32(Console.ReadLine()) - 1];

            Console.WriteLine("Select MIDI-Output Device:");
            string[] midiOutputDevices = MidiInformation.ListOutputDevices();
            for (ushort outi = 0; outi < midiOutputDevices.Length; outi++)
                Console.WriteLine("{0}) {1}", outi + 1, midiOutputDevices[outi]);
            string midiOutDevice = midiOutputDevices[Convert.ToInt32(Console.ReadLine()) - 1];

            MIDI2Win controller = new MIDI2Win(Controller.FilterType.Name, midiInDevice, midiOutDevice);
            controller.OnLog += (s, e) => { Console.WriteLine("[{0}] {1}", DateTime.Now.ToLocalTime() ,e.LogText); };

            Console.WriteLine("Loading Audio Devices...");
            AudioDeviceInfo.DeviceInfo[] devices = AudioDeviceInfo.GetAudioDevices();

            Console.Clear();
            Console.WriteLine("MIDI2WindowsAudio");
            Console.WriteLine("Type 'help' for available commands");
            string line;
            do
            {
                line = Console.ReadLine();
                string[] split = line.Split(' ');
                switch(line.Split(' ')[0])
                {
                    case "help":
                        Console.WriteLine("Commands:");
                        Console.WriteLine("  quit");
                        Console.WriteLine("  listaudiodevices");
                        Console.WriteLine("  addcontrol <audiodeviceindex> <volumecontrol> <mutecontrol>");
                        Console.WriteLine("  load <path>");
                        Console.WriteLine("  save <path>");
                        break;
                    case "listaudiodevices":
                        for (ushort audioi = 0; audioi < devices.Length; audioi++)
                            Console.WriteLine("{0}) {1}", audioi + 1, devices[audioi].name);
                        break;
                    case "addcontrol":
                        if(split.Length != 4)
                            Console.WriteLine("Correct: addcontrol <audiodeviceindex> <volumecontrol> <mutecontrol>");
                        try
                        {
                            string guid = devices[Convert.ToInt32(split[1])].guid;
                            int volumecontrol = Convert.ToInt32(split[2]);
                            int mutecontrol = Convert.ToInt32(split[3]);

                            controller.AddControl(volumecontrol, mutecontrol, guid);
                        }
                        catch (FormatException)
                        {
                            Console.WriteLine("Variables have to be integers.");
                        }
                        break;
                    case "load":
                        if (split.Length != 2)
                            Console.WriteLine("Correct: load <path>");
                        controller.ImportSettings(split[1]);
                        break;
                    case "save":
                        if (split.Length != 2)
                            Console.WriteLine("Correct: save <path>");
                        controller.ExportSettings(split[1]);
                        break;
                    case "quit":break;
                    default:
                        Console.WriteLine("Type 'help' for available commands");
                        break;
                }
            } while (line != "quit");
        }

        static void Main(string[] args)
        {
            new MIDIConsole();
        }
    }
}
