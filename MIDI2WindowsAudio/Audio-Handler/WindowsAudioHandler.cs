using CoreAudio;

namespace Audio_Handler;

public class WindowsAudioHandler
{
    public AudioDevice[] inputs { get; }
    public AudioDevice[] outputs { get; }

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
            inputsStr += $"\t{input}\n";
            foreach (AudioSession session in input.GetSessions())
            {
                inputsStr += $"\t\t{session}\n";
            }
        }
        string outputsStr = "[Outputs]\n";
        foreach (AudioDevice output in outputs)
        {
            inputsStr += $"\t{output}\n";
            foreach (AudioSession session in output.GetSessions())
            {
                inputsStr += $"\t\t{session}\n";
            }
        }
        return $"{inputsStr}\n{outputsStr}";
    }
}