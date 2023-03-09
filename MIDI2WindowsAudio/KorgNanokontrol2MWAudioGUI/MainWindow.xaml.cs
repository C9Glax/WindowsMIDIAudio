using System;
using System.Collections.Generic;
using System.ComponentModel;
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
        private readonly Brush buttonPressedColor = Brushes.Red;
        private readonly Brush buttonNotPressedColor = Brushes.White;
        // ReSharper disable once InconsistentNaming
        private KorgAndAudioKonnector? k2a;
        
        public MainWindow()
        {
            ConsoleWriter consoleListener = new ConsoleWriter();
            consoleListener.OnWrite += OnConsoleWrite;
            Console.SetOut(consoleListener);
            Console.SetError(consoleListener);
            ContentRendered += AfterInit;
            InitializeComponent();
            Console.WriteLine("Loading...");
        }

        private void OnConsoleWrite(object? sender, ConsoleWriterEventArgs e)
        {
            Dispatcher.Invoke(() =>
            {
                if(e.text != "\r\n")
                    ConsoleOutput.AppendText($"[{DateTime.Now.ToLongTimeString()}] {e.text}");
                else
                    ConsoleOutput.AppendText(e.text);
                ConsoleOutput.ScrollToLine(ConsoleOutput.LineCount - 1);
            });
        }

        private void AfterInit(object? sender, EventArgs eventArgs)
        {
            k2a = new KorgAndAudioKonnector();
            k2a.OnAudioControllerStateChanged += K2aOnOnAudioControllerStateChanged;
            Console.WriteLine("Loaded MIDI and Audio-Controllers");
            for (byte i = 0; i < k2a.bindings.groupAssignment.Length; i++)
            {
                if (k2a.bindings.groupAssignment[i] is not null)
                {
                    AudioController audioController = k2a.bindings.groupAssignment[i]!;
                    UpdateAudioGroupGui(i, audioController.name, audioController.isSolo, audioController.mute, false, audioController.volume);
                }
            }
            Console.WriteLine("Initialized Window.");
        }

        private void K2aOnOnAudioControllerStateChanged(AudioController sender)
        {
            if (k2a is not null)
            {
                if(Array.IndexOf(k2a.bindings.groupAssignment, sender) is { } groupNumber and not -1)
                    UpdateAudioGroupGui(groupNumber, sender.name, sender.isSolo, sender.mute, false, sender.volume);
            }
        }
        
        private void UpdateAudioGroupGui(int groupNumber, string name, bool isSolo, bool muted, bool record, float volume)
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
                devicesComboBox.Items.Clear();
                devicesComboBox.Items.Add(new ComboBoxItem()
                {
                    IsSelected = true,
                    Content = new Label()
                    {
                        Content = name
                    }
                });
                
                soloButton.Background = isSolo ? buttonPressedColor : buttonNotPressedColor;
                muteButton.Background = muted ? buttonPressedColor : buttonNotPressedColor;
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

        private void OnDeviceChangeDropDownOpening(object? sender, EventArgs eventArgs)
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

        private void OnDeviceDropDownClosing(object? sender, EventArgs e)
        {
            if (sender is ComboBox comboBox && k2a is not null)
            {
                Grid[] groups = { Group0, Group1, Group2, Group3, Group4, Group5, Group6, Group7 };
                Grid group = groups.Where(group => group == (Grid)comboBox.Parent).ToArray()[0];
                int groupNumber = Array.IndexOf(groups, group);
                if (k2a.bindings.groupAssignment[groupNumber] is { } audioController)
                {
                    UpdateAudioGroupGui(groupNumber, audioController.name, audioController.isSolo, audioController.mute, false, audioController.volume);
                }
            }
        }

        private void MainWindow_OnClosing(object? sender, CancelEventArgs e)
        {
            k2a?.Dispose();
            Environment.Exit(0);
        }
    }
}