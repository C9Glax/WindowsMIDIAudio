using CoreAudio;

namespace Audio_Handler;

public class WindowsAudioHandler
{
    private AudioDevice[] inputs { get; }
    private AudioDevice[] outputs { get; }
    
    public WindowsAudioHandler()
    {
        Guid guid = Guid.NewGuid();
        MMDeviceCollection mmoutputs = new MMDeviceEnumerator(guid).EnumerateAudioEndPoints(DataFlow.Render, DeviceState.Active);
        MMDeviceCollection mminputs = new MMDeviceEnumerator(guid).EnumerateAudioEndPoints(DataFlow.Capture, DeviceState.Active);

        HashSet<AudioDevice> tmpOutputs = new HashSet<AudioDevice>();
        foreach (MMDevice dev in mmoutputs)
        {
            tmpOutputs.Add(new AudioDevice(dev));
        }
        outputs = tmpOutputs.ToArray();
        
        HashSet<AudioDevice> tmpInputs = new HashSet<AudioDevice>();
        foreach (MMDevice dev in mminputs)
        {
            tmpInputs.Add(new AudioDevice(dev));
        }
        inputs = tmpInputs.ToArray();
    }

    public override string ToString()
    {
        string inputsStr = "[Inputs]\n";
        foreach (AudioDevice input in inputs)
        {
            inputsStr += string.Format("\t{0}\n", input.ToString());
            foreach (AudioSession session in input.GetSessions())
            {
                inputsStr += string.Format("\t\t{0}\n", session.ToString());
            }
        }
        string outputsStr = "[Outputs]\n";
        foreach (AudioDevice output in outputs)
        {
            inputsStr += string.Format("\t{0}\n", output.ToString());
            foreach (AudioSession session in output.GetSessions())
            {
                inputsStr += string.Format("\t\t{0}\n", session.ToString());
            }
        }
        return string.Format("{0}\n{1}", inputsStr, outputsStr);
    }
}