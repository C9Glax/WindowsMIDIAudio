using NAudio.Midi;

namespace MidiAccess
{
    public static class MidiInformation
    {
        public static string[] ListInputDevices()
        {
            string[] names = new string[MidiIn.NumberOfDevices];
            for (int i = 0; i < MidiIn.NumberOfDevices; i++)
                names[i] = MidiIn.DeviceInfo(i).ProductName;
            return names;
        }
        public static string[] ListOutputDevices()
        {
            string[] names = new string[MidiOut.NumberOfDevices];
            for (int i = 0; i < MidiOut.NumberOfDevices; i++)
                names[i] = MidiOut.DeviceInfo(i).ProductName;
            return names;
        }

        public static MidiIn GetInputDeviceWithName(string name)
        {
            for (int i = 0; i < MidiIn.NumberOfDevices; i++)
                if (MidiIn.DeviceInfo(i).ProductName.Equals(name))
                    return new MidiIn(i);
            return null;
        }

        public static int GetPIDOfDevice(string name)
        {
            for (int i = 0; i < MidiIn.NumberOfDevices; i++)
                if (MidiIn.DeviceInfo(i).ProductName.Equals(name))
                    return MidiIn.DeviceInfo(i).ProductId;
            for (int i = 0; i < MidiOut.NumberOfDevices; i++)
                if (MidiOut.DeviceInfo(i).ProductName.Equals(name))
                    return MidiOut.DeviceInfo(i).ProductId;
            return -1;
        }

        public static MidiOut GetOutputDeviceWithName(string name)
        {
            for (int i = 0; i < MidiOut.NumberOfDevices; i++)
                if (MidiOut.DeviceInfo(i).ProductName.Equals(name))
                    return new MidiOut(i);
            return null;
        }

        public static MidiIn GetInputDeviceWithPID(int pid)
        {
            for (int i = 0; i < MidiIn.NumberOfDevices; i++)
                if (MidiIn.DeviceInfo(i).ProductId.Equals(pid))
                    return new MidiIn(i);
            return null;
        }

        public static MidiOut GetOutputDeviceWithPID(int pid)
        {
            for (int i = 0; i < MidiOut.NumberOfDevices; i++)
                if (MidiOut.DeviceInfo(i).ProductId.Equals(pid))
                    return new MidiOut(i);
            return null;
        }
    }
}
