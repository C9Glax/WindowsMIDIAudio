using Melanchall.DryWetMidi.Core;
using Melanchall.DryWetMidi.Multimedia;

namespace MIDI_Handler;

public class NanoKontrol2
{
    private readonly InputDevice midiIn;
    private readonly OutputDevice midiOut;

    public const float MaxValue = 127f;
    public event OnControlChangeEventHandler? OnControlChange;

    public NanoKontrol2(InputDevice nanoKontrolIn, OutputDevice nanoKontrolOut)
    {
        midiIn = nanoKontrolIn;
        midiOut = nanoKontrolOut;

        midiIn.EventReceived += OnEventReceived;
        midiIn.ErrorOccurred += (sender, args) => Console.WriteLine(args);
        midiIn.StartEventsListening();
    }

    private void HandleMidiEvent(MidiEventReceivedEventArgs eventArgs)
    {
        if (eventArgs.Event is ControlChangeEvent midiEvent)
        {
            ControlChangeEventArgs newEventArgs;
            ControlChangeEventArgs.ControlType newEventControlType = ControlChangeEventArgs.ControlType.ControlButton;
            ControlChangeEventArgs.ControlButtonName newEventControlButtonName = ControlChangeEventArgs.ControlButtonName.NULL;
            byte controlGroup = byte.MaxValue;

            switch (midiEvent.ControlNumber)
            {
                case 46: //Cycle
                    newEventControlButtonName = ControlChangeEventArgs.ControlButtonName.Cycle;
                    break;
                case 41: //PlayPause
                    newEventControlButtonName = ControlChangeEventArgs.ControlButtonName.PlayPause;
                    break;
                case 42: //Stop
                    newEventControlButtonName = ControlChangeEventArgs.ControlButtonName.Stop;
                    break;
                case 43: //Previous
                    newEventControlButtonName = ControlChangeEventArgs.ControlButtonName.Previous;
                    break;
                case 44: //Next
                    newEventControlButtonName = ControlChangeEventArgs.ControlButtonName.Next;
                    break;
                case 45: //Record
                    newEventControlButtonName = ControlChangeEventArgs.ControlButtonName.Record;
                    break;
                case 58: //Track Previous
                    newEventControlButtonName = ControlChangeEventArgs.ControlButtonName.PrevTrack;
                    break;
                case 59: //Track Next
                    newEventControlButtonName = ControlChangeEventArgs.ControlButtonName.NextTrack;
                    break;
                case 60: //Marker Set
                    newEventControlButtonName = ControlChangeEventArgs.ControlButtonName.SetMarker;
                    break;
                case 61: //Marker Previous
                    newEventControlButtonName = ControlChangeEventArgs.ControlButtonName.PrevMarker;
                    break;
                case 62: //Marker Next
                    newEventControlButtonName = ControlChangeEventArgs.ControlButtonName.NextMarker;
                    break;
                default:
                    newEventControlType = (ControlChangeEventArgs.ControlType)Convert.ToByte(Math.Floor(midiEvent.ControlNumber / 16.0));
                    controlGroup = Convert.ToByte(midiEvent.ControlNumber % 16);
                    if (controlGroup > 7)
                    {
                        throw new Exception($"Invalid group {controlGroup} {midiEvent.ControlNumber}");
                    }
                    break;
            }

            if (newEventControlType == ControlChangeEventArgs.ControlType.ControlButton) //GlobalControl
            {
                newEventArgs = new ControlChangeEventArgs(newEventControlType, midiEvent.ControlNumber,
                    controlGroup, midiEvent.ControlValue, newEventControlButtonName);
            }
            else //GroupControl
            {
                newEventArgs = new ControlChangeEventArgs(newEventControlType, midiEvent.ControlNumber,
                    controlGroup, midiEvent.ControlValue);
            }
            
            OnControlChange?.Invoke(this, newEventArgs);
        }
        else
        {
            throw new Exception($"Event not recognised: {eventArgs.Event}");
        }
    }
    
    private void OnEventReceived(object? sender, MidiEventReceivedEventArgs eventArgs)
    {
        Task newTask = new Task(() => HandleMidiEvent(eventArgs));
        newTask.Start();
    }

    public void Dispose()
    {
        midiIn.StopEventsListening();
        midiIn.Dispose();
    }
}