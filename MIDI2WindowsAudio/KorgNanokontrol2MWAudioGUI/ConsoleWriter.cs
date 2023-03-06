using System;
using System.IO;
using System.Text;

namespace KorgNanokontrol2MWAudioGUI;

public class ConsoleWriter : TextWriter
{
    public override Encoding Encoding { get; }
    public event EventHandler<ConsoleWriterEventArgs>? OnWrite;
    public event EventHandler<ConsoleWriterEventArgs>? OnWriteLine;

    public ConsoleWriter()
    {
        Encoding = Encoding.UTF8;
    }

    public override void Write(string? text)
    {
        if(text is not null)
            this.OnWrite?.Invoke(this, new ConsoleWriterEventArgs(text));
        base.Write(text);
    }

    public override void WriteLine(string? text)
    {
        if (text is not null)
            this.OnWriteLine?.Invoke(this, new ConsoleWriterEventArgs(text));
        base.WriteLine(text);
    }
}

public class ConsoleWriterEventArgs : EventArgs
{
    public string text { get; }
    public ConsoleWriterEventArgs(string text)
    {
        this.text = text;
    }
}