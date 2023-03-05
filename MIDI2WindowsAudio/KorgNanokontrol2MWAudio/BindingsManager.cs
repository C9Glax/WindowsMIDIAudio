using System.Globalization;
using MIDI_Handler;

namespace KorgNanokontrol2MWAudio;

public class BindingsManager
{

    private Dictionary<byte, Binding> bindings;

    public BindingsManager()
    {
        bindings = new();
    }

    public void AddBinding(byte controlNumber, Binding action)
    {
        bindings.TryAdd(controlNumber, action);
    }

    public void ExecuteBinding(byte controlNumber, ControlChangeEventArgs eventArgs)
    {
        bindings[controlNumber]?.Execute(eventArgs);
    }

    public override string ToString()
    {
        string ret = "[Bindings]\n";
        foreach (KeyValuePair<byte, Binding> kv in bindings)
        {
            ret += $"{kv.Key.ToString(CultureInfo.InvariantCulture),2} {kv.Value}\n";
        }
        return ret;
    }
}