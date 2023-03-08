using System;
using System.Collections.Generic;
using System.Linq;
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
        private readonly Brush buttonPressed = Brushes.Red;
        private readonly Brush buttonNotPressed = Brushes.White;
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

        private void AfterInit(object? sender, EventArgs eventArgs)
        {
            Console.WriteLine("Initialized Window");
            k2a = new KorgAndAudioKonnector();
            k2a.OnAudioControllerStateChanged += K2aOnOnAudioControllerStateChanged;
            for (byte i = 0; i < k2a.bindings.groupAssignment.Length; i++)
            {
                if (k2a.bindings.groupAssignment[i] is not null)
                {
                    AudioController audioController = k2a.bindings.groupAssignment[i]!;
                    UpdateAudioGroup(i, audioController.name, audioController.isSolo, audioController.mute, false, audioController.volume);
                }
            }
        }

        private void K2aOnOnAudioControllerStateChanged(AudioController sender)
        {
            if (k2a is not null)
            {
                if(Array.IndexOf(k2a.bindings.groupAssignment, sender) is { } groupNumber and not -1)
                    UpdateAudioGroup(groupNumber, sender.name, sender.isSolo, sender.mute, false, sender.volume);
            }
        }

        
        private void UpdateAudioGroup(int groupNumber, string name, bool isSolo, bool muted, bool record, float volume)
        {
            Grid[] groups = { Group0, Group1, Group2, Group3, Group4, Group5, Group6, Group7 };
            Dispatcher.Invoke(() =>
            {
                Grid group = groups[groupNumber];
                Button[] buttons = FindChildrenWithType<Button>(group).ToArray();
                Button soloButton = buttons.Where(button => button.Content.ToString() == "S").ToArray()[0];
                Button muteButton = buttons.Where(button => button.Content.ToString() == "M").ToArray()[0];
                Button recordButton = buttons.Where(button => button.Content.ToString() == "R").ToArray()[0];
                Slider volumeSlider = FindChildrenWithType<Slider>(group).ToArray()[0];
                ComboBox devicesComboBox = FindChildrenWithType<ComboBox>(group).ToArray()[0];
                foreach (ComboBoxItem item in devicesComboBox.Items)
                {
                    if (item.IsSelected)
                    {
                        item.Content = name;
                        break;
                    }
                }
                
                soloButton.Background = isSolo ? buttonPressed : buttonNotPressed;
                muteButton.Background = muted ? buttonPressed : buttonNotPressed;
                volumeSlider.Value = Math.Abs(volume * 100);
            });
        }

        private static IEnumerable<T> FindChildrenWithType<T>(DependencyObject depObj) where T : DependencyObject
        {
            for (int i = 0; i < VisualTreeHelper.GetChildrenCount(depObj); i++)
            {
                DependencyObject child = VisualTreeHelper.GetChild(depObj, i);
                if (child is T childType)
                {
                    yield return childType;
                }

                foreach (T childOfChild in FindChildrenWithType<T>(child))
                {
                    yield return childOfChild;
                }
            }
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
                    UpdateAudioGroup(i, audioController.name, audioController.isSolo, audioController.mute, false, audioController.volume);
                }
            }
            Console.WriteLine("Restarted");
        }

        private void OnDeviceContextMenuOpening(object? sender, EventArgs eventArgs)
        {
            if (sender is ComboBox comboBox && k2a is not null)
            {
                Grid[] groups = { Group0, Group1, Group2, Group3, Group4, Group5, Group6, Group7 };
                Grid group = groups.Where(group => group == (Grid)comboBox.Parent).ToArray()[0];
                int groupNumber = Array.IndexOf(groups, group);

                AudioController[] toList = k2a.windowsAudioHandler.controllers.Where(controller => !k2a.bindings.groupAssignment.Contains(controller)).ToArray();
                comboBox.Items.Clear();
                comboBox.Items.Add(new ComboBoxItem()
                {
                    Content = new Label()
                    {
                        Content = k2a.bindings.groupAssignment[groupNumber]?.name
                    }
                });

                foreach (AudioController audioController in toList)
                {
                    ComboBoxItem item = new ComboBoxItem()
                    {
                        Content = new Label()
                        {
                            Content = audioController.name
                        }
                    };
                    item.Selected += (_, _) =>
                    {
                        k2a.bindings.groupAssignment[groupNumber] = audioController;
                        k2a.SetBindingsForGroup(Convert.ToByte(groupNumber), audioController);
                    };
                    comboBox.Items.Add(item);
                }
                
            }
        }
    }
}