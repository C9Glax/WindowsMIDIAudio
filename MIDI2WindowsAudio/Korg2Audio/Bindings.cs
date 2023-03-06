using Audio_Handler;
using MIDI_Handler;

namespace Korg2Audio;

public class Bindings
{
    private readonly Dictionary<byte, ControllerAction> controlBindings;
    public AudioController?[] groupAssignment { get; }

    public Bindings()
    {
        controlBindings = new();
        groupAssignment = new AudioController?[8];
    }

    public enum ControllerActions { SetVolume, Solo, Mute, Record, NextTrack, PreviousTrack, Next, Previous, PlayPause, Stop }


    public void SetGroup(AudioController audioController, byte group)
    {
        if (group < groupAssignment.Length)
        {
            groupAssignment[group] = audioController;
        }
    }

    public void AddControlBinding(byte controlAbsoluteNumber, ControllerActions controllerAction, AudioController? audioController)
    {
        controlBindings.Add(controlAbsoluteNumber, new ControllerAction(controllerAction, audioController));
    }

    public ControllerActions? ExecuteControllerBinding(byte controlAbsoluteNumber, byte value)
    {
        return controlBindings[controlAbsoluteNumber]?.Execute(value);
    }
    
    public override string ToString()
    {
        string ret = $"[Controller Bindings] {controlBindings.Count}";
        foreach (KeyValuePair<byte, ControllerAction> kv in controlBindings)
        {
            ret += $"\n{kv.Key.ToString().PadLeft(3).Substring(0,3)} {kv.Value}";
            
        }
        //TODO audioBindings
        return ret;
    }
    
    //TODO audioBindings

    private class ControllerAction
    {
        private readonly ControllerActions controllerActionToExecute;
        private readonly AudioController? audioController;

        public ControllerAction(ControllerActions controllerAction, AudioController? audioController)
        {
            controllerActionToExecute = controllerAction;
            this.audioController = audioController;
        }

        public ControllerActions? Execute(byte value)
        {
            switch (controllerActionToExecute)
            {
                case ControllerActions.Stop:
                    if(value != 0)
                        MediaController.Stop();
                    break;
                case ControllerActions.Previous:
                    if(value != 0)
                        MediaController.Previous();
                    break;
                case ControllerActions.Next:
                    if(value != 0)
                        MediaController.Next();
                    break;
                case ControllerActions.PlayPause:
                    if(value != 0)
                        MediaController.PlayPause();
                    break;
                case ControllerActions.SetVolume:
                    audioController?.SetVolume(value / NanoKontrol2.MaxValue); //TODO type check
                    break;
                case ControllerActions.Mute:
                    audioController?.SetMute(value != 0); //TODO type check
                    break;
                case ControllerActions.Solo:
                    return controllerActionToExecute;
                case ControllerActions.Record:
                    throw new NotImplementedException(); //TODO
                    break;
                case ControllerActions.NextTrack:
                    throw new NotImplementedException(); //TODO
                    break;
                case ControllerActions.PreviousTrack:
                    throw new NotImplementedException(); //TODO
                    break;
            }

            return null;
        }

        public override string ToString()
        {
            return $"{controllerActionToExecute.ToString().PadLeft(10).Substring(0,10)}{(audioController != null ? $" - {audioController}" : "")}";
        }
    }
}