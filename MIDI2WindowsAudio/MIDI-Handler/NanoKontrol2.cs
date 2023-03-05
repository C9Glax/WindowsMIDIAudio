using Melanchall.DryWetMidi.Common;
using Melanchall.DryWetMidi.Core;
using Melanchall.DryWetMidi.Multimedia;

namespace MIDI_Handler;

public class NanoKontrol2
{
    private readonly InputDevice midiIn;
    private readonly OutputDevice midiOut;
    public static float maxValue = 127f;
    public event OnControlChangeEventHandler? OnControlChange;

    public NanoKontrol2(InputDevice nanoKontrolIn, OutputDevice nanoKontrolOut)
    {
        midiIn = nanoKontrolIn;
        midiOut = nanoKontrolOut;

        midiIn.EventReceived += OnEventReceived;
        midiIn.StartEventsListening();
    }
    
    private void OnEventReceived(object? sender, MidiEventReceivedEventArgs eventArgs)
    {
        if (eventArgs.Event is ControlChangeEvent midiEvent)
        {
            ControlChangeEventArgs newEventArgs;
            SevenBitNumber controlAbsoluteNumber = midiEvent.ControlNumber;
            SevenBitNumber controlValue = midiEvent.ControlValue;
            if (controlAbsoluteNumber >= 0 && controlAbsoluteNumber <= 7) //Faders
            {
                newEventArgs = new ControlChangeEventArgs(ControlChangeEventArgs.ControlType.Fader,
                    controlAbsoluteNumber, Convert.ToByte(controlAbsoluteNumber), controlValue);
            }else if (controlAbsoluteNumber >= 16 && controlAbsoluteNumber <= 23) //Knobs
            {
                newEventArgs = new ControlChangeEventArgs(ControlChangeEventArgs.ControlType.Knob,
                    controlAbsoluteNumber, Convert.ToByte(controlAbsoluteNumber - 16), controlValue);
            }else if (controlAbsoluteNumber >= 32 && controlAbsoluteNumber <= 39) //SoloButtons
            {
                newEventArgs = new ControlChangeEventArgs(ControlChangeEventArgs.ControlType.SoloButton,
                    controlAbsoluteNumber, Convert.ToByte(controlAbsoluteNumber - 32), controlValue);
            }else if (controlAbsoluteNumber >= 48 && controlAbsoluteNumber <= 55) //MuteButtons
            {
                newEventArgs = new ControlChangeEventArgs(ControlChangeEventArgs.ControlType.MuteButton,
                    controlAbsoluteNumber, Convert.ToByte(controlAbsoluteNumber - 48), controlValue);
            }else if (controlAbsoluteNumber >= 64 && controlAbsoluteNumber <= 71) //RecordButtons
            {
                newEventArgs = new ControlChangeEventArgs(ControlChangeEventArgs.ControlType.RecordButton,
                    controlAbsoluteNumber, Convert.ToByte(controlAbsoluteNumber - 64), controlValue);
            }else if (controlAbsoluteNumber == 58) //prevTrack
            {
                newEventArgs = new ControlChangeEventArgs(ControlChangeEventArgs.ControlType.ControlButton,
                    controlAbsoluteNumber, Convert.ToByte(controlAbsoluteNumber), controlValue, ControlChangeEventArgs.ControlButtonName.PrevTrack);
            }else if (controlAbsoluteNumber == 59) //nextTrack
            {
                newEventArgs = new ControlChangeEventArgs(ControlChangeEventArgs.ControlType.ControlButton,
                    controlAbsoluteNumber, Convert.ToByte(controlAbsoluteNumber), controlValue, ControlChangeEventArgs.ControlButtonName.NextTrack);
            }else if (controlAbsoluteNumber == 46) //cycle
            {
                newEventArgs = new ControlChangeEventArgs(ControlChangeEventArgs.ControlType.ControlButton,
                    controlAbsoluteNumber, Convert.ToByte(controlAbsoluteNumber), controlValue, ControlChangeEventArgs.ControlButtonName.Cycle);
            }else if (controlAbsoluteNumber == 60) //setMarker
            {
                newEventArgs = new ControlChangeEventArgs(ControlChangeEventArgs.ControlType.ControlButton,
                    controlAbsoluteNumber, Convert.ToByte(controlAbsoluteNumber), controlValue, ControlChangeEventArgs.ControlButtonName.SetMarker);
            }else if (controlAbsoluteNumber == 61) //prevMarker
            {
                newEventArgs = new ControlChangeEventArgs(ControlChangeEventArgs.ControlType.ControlButton,
                    controlAbsoluteNumber, Convert.ToByte(controlAbsoluteNumber), controlValue, ControlChangeEventArgs.ControlButtonName.PrevMarker);
            }else if (controlAbsoluteNumber == 62) //nextMarker
            {
                newEventArgs = new ControlChangeEventArgs(ControlChangeEventArgs.ControlType.ControlButton,
                    controlAbsoluteNumber, Convert.ToByte(controlAbsoluteNumber), controlValue, ControlChangeEventArgs.ControlButtonName.PrevTrack);
                
            }else if (controlAbsoluteNumber == 43) //backwards
            {
                newEventArgs = new ControlChangeEventArgs(ControlChangeEventArgs.ControlType.ControlButton,
                    controlAbsoluteNumber, Convert.ToByte(controlAbsoluteNumber), controlValue, ControlChangeEventArgs.ControlButtonName.Previous);
            }else if (controlAbsoluteNumber == 44) //forwards
            {
                newEventArgs = new ControlChangeEventArgs(ControlChangeEventArgs.ControlType.ControlButton,
                    controlAbsoluteNumber, Convert.ToByte(controlAbsoluteNumber), controlValue, ControlChangeEventArgs.ControlButtonName.Next);
            }else if (controlAbsoluteNumber == 42) //stop
            {
                newEventArgs = new ControlChangeEventArgs(ControlChangeEventArgs.ControlType.ControlButton,
                    controlAbsoluteNumber, Convert.ToByte(controlAbsoluteNumber), controlValue, ControlChangeEventArgs.ControlButtonName.Stop);
            }else if (controlAbsoluteNumber == 41) //play
            {
                newEventArgs = new ControlChangeEventArgs(ControlChangeEventArgs.ControlType.ControlButton,
                    controlAbsoluteNumber, Convert.ToByte(controlAbsoluteNumber), controlValue, ControlChangeEventArgs.ControlButtonName.Play);
            }else if (controlAbsoluteNumber == 45) //record
            {
                newEventArgs = new ControlChangeEventArgs(ControlChangeEventArgs.ControlType.ControlButton,
                    controlAbsoluteNumber, Convert.ToByte(controlAbsoluteNumber), controlValue, ControlChangeEventArgs.ControlButtonName.Record);
            }
            else
            {
                throw new Exception($"ControlNumber not recognised: {controlAbsoluteNumber}");
            }
            
            OnControlChange?.Invoke(this, newEventArgs);
        }
        else
        {
            throw new Exception($"Event not recognised: {eventArgs.Event}");
        }
    }
}