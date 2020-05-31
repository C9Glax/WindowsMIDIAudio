using System.Collections.Generic;
using MidiAccess;
using WindowsSoundControl;
using System.IO;
using System;

namespace MIDI2WindowsAudio
{
    public class MIDI2Win
    {
        private readonly Controller midi;
        private readonly double faderMaxValue = 127;
        private readonly Dictionary<int, AudioDevice> volume;
        private readonly Dictionary<int, AudioDevice> mute;

        public event LogEventHandler OnLog;
        public delegate void LogEventHandler(object sender, LogArgs e);
        public class LogArgs : EventArgs
        {
            public string LogText { get; }
            public LogArgs(string text, params object[] replace)
            {
                this.LogText = string.Format(text, replace);
            }
        }

        public MIDI2Win(Controller.FilterType filter, string midiIn, string midiOut) {

            this.volume = new Dictionary<int, AudioDevice>();
            this.mute = new Dictionary<int, AudioDevice>();

            this.midi = new Controller(filter, midiIn, midiOut);
            this.midi.OnMidiMessageReceived += OnMidiMessageReceived;
        }

        private void OnMidiMessageReceived(object sender, Controller.MidiMessageReceivedEventArgs e)
        {
            this.OnLog?.Invoke(this, new LogArgs("Input: {0} Value: {1}", e.control, e.value));
            if (this.volume.ContainsKey(e.control))
            {
                double volume = e.value / faderMaxValue * 100;
                this.volume[e.control].SetVolume(volume);
                this.OnLog?.Invoke(this, new LogArgs("Action: {0} Volume: {1}", this.volume[e.control].Name, volume));
            }
            else if (this.mute.ContainsKey(e.control))
            {
                this.mute[e.control].ToggleMute();
                this.OnLog?.Invoke(this, new LogArgs("Action: {0} Mute: {1}", this.mute[e.control].Name, this.mute[e.control].IsMuted()));
            }
        }

        public void AddControl(int midiVolumeControl, int midiMuteControl, string guidWindowsAudioDevice)
        {
            AudioDevice newAudioDevice = new AudioDevice(guidWindowsAudioDevice);
            if (this.volume.ContainsKey(midiVolumeControl))
                this.volume[midiVolumeControl] = newAudioDevice;
            else
                this.volume.Add(midiVolumeControl, newAudioDevice);
            if (this.mute.ContainsKey(midiMuteControl))
                this.mute[midiMuteControl] = newAudioDevice;
            else
                this.mute.Add(midiMuteControl, newAudioDevice);
        }

        public void ExportSettings(string path)
        {
            Dictionary<string, string> settings = new Dictionary<string, string>();
            foreach (int volumeKey in this.volume.Keys)
                settings.Add(this.volume[volumeKey].GetGuid(), string.Format("{0},{1},",this.volume[volumeKey].GetGuid(), volumeKey));
            foreach (int muteKey in this.mute.Keys)
                settings[this.mute[muteKey].GetGuid()] += muteKey;
            File.WriteAllLines(path.EndsWith(".txt") ? path : path+".txt", settings.Values);
            this.OnLog?.Invoke(this, new LogArgs("Exported assignments to {0}", path));
        }

        public void ImportSettings(string path)
        {
            string[] settings = File.ReadAllLines(path);
            foreach(string assignment in settings)
            {
                string[] parts = assignment.Split(',');
                this.AddControl(Convert.ToInt32(parts[1]), Convert.ToInt32(parts[2]), parts[0]);
            }
            this.OnLog?.Invoke(this, new LogArgs("Read assignments from {0}", path));
        }
    }
}
