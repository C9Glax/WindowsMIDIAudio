using MIDI_Handler;
using Audio_Handler;

WindowsAudioHandler wah = new WindowsAudioHandler();
Console.WriteLine(wah.ToString());
MidiHandler mh = new MidiHandler();
Console.ReadKey();