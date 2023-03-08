using Melanchall.DryWetMidi.Common;

namespace MIDI_Handler;

public class ControlChangeEventArgs : EventArgs
{
    public ControlChangeEventArgs(ControlType controlType, SevenBitNumber absoluteControlNumber, byte groupNumber, SevenBitNumber value)
    {
        this.controlType = controlType;
        this.absoluteControlNumber = absoluteControlNumber;
        this.groupNumber = groupNumber;
        this.value = value;
        this.controlButtonName = null;
    }
        
    public ControlChangeEventArgs(ControlType controlType, SevenBitNumber absoluteControlNumber, byte groupNumber, SevenBitNumber value, ControlButtonName controlButtonName)
    {
        this.controlType = controlType;
        this.absoluteControlNumber = absoluteControlNumber;
        this.groupNumber = groupNumber;
        this.value = value;
        this.controlButtonName = controlButtonName;
    }

    public override string ToString()
    {
        return string.Format("{0} absoluteNumber: {1} localNumber: {2} value: {3} name?: {4}", controlType, absoluteControlNumber.ToString(), groupNumber.ToString(), value.ToString(),
            // ReSharper disable once HeapView.BoxingAllocation
            controlButtonName != null ? controlButtonName : "");
    }

    private ControlType controlType { get; }
    public SevenBitNumber absoluteControlNumber { get; }
    public byte groupNumber { get; }
    public SevenBitNumber value { get; }
        
    private ControlButtonName? controlButtonName { get; }
    public enum ControlType : byte
    {
        Fader = 0, //0-7
        Knob = 1, //16-23
        SoloButton = 2, //32-39
        MuteButton = 3, //48-55
        RecordButton = 4, //64-71
        ControlButton
    }

    public enum ControlButtonName
    {
        PrevTrack, NextTrack, Cycle, SetMarker, PrevMarker, NextMarker, Previous, Next, Stop, PlayPause, Record, NULL
    }
}

public delegate void OnControlChangeEventHandler(object sender, ControlChangeEventArgs eventArgs);