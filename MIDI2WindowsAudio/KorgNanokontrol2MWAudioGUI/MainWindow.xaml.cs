using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using Audio_Handler;
using Korg2Audio;

namespace KorgNanokontrol2MWAudioGUI
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public sealed partial class MainWindow
    {
        private static Brush _buttonPressed = Brushes.Red;
        private static Brush _buttonNotPressed = Brushes.White;
        // ReSharper disable once InconsistentNaming
        private KorgAndAudioKonnector? k2a;
        
        public MainWindow()
        {
            ConsoleWriter consoleListener = new ConsoleWriter();
            consoleListener.OnWriteLine += OnConsoleNewLine;
            Console.SetOut(consoleListener);
            Console.SetError(consoleListener);
            ContentRendered += AfterInit;
            InitializeComponent();
        }

        private void OnConsoleNewLine(object? sender, ConsoleWriterEventArgs e)
        {
            Dispatcher.Invoke(() =>
            {
                ConsoleOutput.AppendText($"\n{e.text}");
                ConsoleOutput.ScrollToLine(ConsoleOutput.LineCount - 1);
            });
        }

        public void AfterInit(object? sender, EventArgs eventArgs)
        {
            Console.WriteLine("Initialized Window");
            k2a = new KorgAndAudioKonnector();
            k2a.OnAudioControllerStateChanged += K2aOnOnAudioControllerStateChanged;
            for (byte i = 0; i < k2a.bindings.groupAssignment.Length; i++)
            {
                if (k2a.bindings.groupAssignment[i] is not null)
                {
                    AudioController audioController = k2a.bindings.groupAssignment[i]!;
                    UpdateAudioGroup(i, audioController.name, audioController.soloMute, audioController.mute, false, audioController.volume);
                }
            }
        }

        private void K2aOnOnAudioControllerStateChanged(AudioController sender)
        {
            if (k2a != null)
                for (byte i = 0; i < k2a.bindings.groupAssignment.Length; i++)
                {
                    if (k2a.bindings.groupAssignment[i] is not null && k2a.bindings.groupAssignment[i] == sender)
                    {
                        UpdateAudioGroup(i, sender.name, sender.soloMute, sender.mute, false, sender.volume);
                        break;
                    }
                }
        }

        private void UpdateAudioGroup(byte groupNumber, string name, bool soloMuted, bool muted, bool record, float volume)
        {
            Grid[] groups = { Group0, Group1, Group2, Group3, Group4, Group5, Group6, Group7 };
            Dispatcher.Invoke(() =>
            {
                Grid group = groups[groupNumber];
                Label nameLabel = (Label)group.Children[0];
                Button soloButton = (Button)((StackPanel)group.Children[1]).Children[0];
                Button muteButton = (Button)((StackPanel)group.Children[1]).Children[1];
                Button recordButton = (Button)((StackPanel)group.Children[1]).Children[2];
                Slider volumeSlider = (Slider)group.Children[2];
                nameLabel.Content = name;
                soloButton.Background = soloMuted ? _buttonPressed : _buttonNotPressed;
                muteButton.Background = muted ? _buttonPressed : _buttonNotPressed;
                volumeSlider.Value = Math.Abs(volume * 100);
            });
        }

        private void OnClickRestart(object sender, RoutedEventArgs routedEventArgs)
        {
            ConsoleOutput.Text = "";
            Console.WriteLine("Restarting");
            k2a?.Dispose();
            k2a = new KorgAndAudioKonnector();
            k2a.OnAudioControllerStateChanged += K2aOnOnAudioControllerStateChanged;
            for (byte i = 0; i < k2a.bindings.groupAssignment.Length; i++)
            {
                if (k2a.bindings.groupAssignment[i] is not null)
                {
                    AudioController audioController = k2a.bindings.groupAssignment[i]!;
                    UpdateAudioGroup(i, audioController.name, audioController.soloMute, audioController.mute, false, audioController.volume);
                }
            }
            Console.WriteLine("Restarted");
        }
    }
}