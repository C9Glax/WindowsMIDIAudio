using Audio_Handler;
using MIDI_Handler;

namespace KorgNanokontrol2MWAudio;

public class Binding
{
    private AudioAction audioAction;
    private AudioDevice? device;
    private AudioSession? session;
    public enum AudioAction { SetVolume, Mute, Solo, PlayPause, Stop, NextTrack, PreviousTrack }

    public Binding(AudioAction action, AudioDevice device)
    {
        audioAction = action;
        this.device = device;
        this.session = null;
    }

    public Binding(AudioAction action, AudioSession session)
    {
        audioAction = action;
        this.session = session;
        this.device = null;
    }

    public Binding(AudioAction action)
    {
        if (!new AudioAction[]{
                AudioAction.PlayPause, AudioAction.Stop, AudioAction.NextTrack, AudioAction.PreviousTrack
            }.Contains(action))
        {
            throw new Exception(); //TODO
        }
        audioAction = action;
        this.session = null;
        this.device = null;
    }

    public void Execute(ControlChangeEventArgs eventArgs)
    {
        switch (audioAction)
        {
            case AudioAction.SetVolume:
                if(session != null)
                    session.SetVolumePercentage(eventArgs.value / NanoKontrol2.MaxValue);
                else
                    device!.SetVolumePercentage(eventArgs.value / NanoKontrol2.MaxValue);
                break;
            case AudioAction.Mute:
                if(session != null)
                    session.Mute(eventArgs.value != 0, false);
                else
                    device!.Mute(eventArgs.value != 0, false);
                break;
            case AudioAction.Solo:
                if(session != null)
                    session.Mute(eventArgs.value != 0, true);
                else
                    device!.Mute(eventArgs.value != 0, true);
                break;
            case AudioAction.PlayPause:
                if(eventArgs.value != 0)
                    MediaController.PlayPause();
                break;
            case AudioAction.Stop:
                if(eventArgs.value != 0)
                    MediaController.Stop();
                break;
            case AudioAction.NextTrack:
                if(eventArgs.value != 0)
                    MediaController.Next();
                break;
            case AudioAction.PreviousTrack:
                if(eventArgs.value != 0)
                    MediaController.Previous();
                break;
        }
    }

    public override string ToString()
    {
        return $"{(session != null ? session.ToString() : device?.ToString())} {audioAction.ToString()}";
    }
}