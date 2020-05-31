using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using WindowsSoundControl;
using MIDI2WindowsAudio;
using MidiAccess;

namespace MIDI2WindowsAudioTray
{
    public partial class TrayForm : Form
    {
        private AudioDeviceInfo.DeviceInfo[] devices;
        private MIDI2Win controller;
        private string midiIn, midiOut;

        public TrayForm()
        {
            InitializeComponent();
            this.listBoxMidiIn.Items.AddRange(MidiInformation.ListInputDevices());
            this.listBoxMidiOut.Items.AddRange(MidiInformation.ListOutputDevices());
        }

        private void LoadFile_Click(object sender, EventArgs e)
        {
            OpenFileDialog dialog = new OpenFileDialog();
            dialog.Filter = "TEXT (*.txt)|All files (*.*)";
            dialog.FilterIndex = 0;
            dialog.RestoreDirectory = true;
            if (dialog.ShowDialog() == DialogResult.OK)
                this.controller.ImportSettings(dialog.FileName);
        }

        private void SaveFile_Click(object sender, EventArgs e)
        {
            SaveFileDialog dialog = new SaveFileDialog();
            dialog.Filter = "TEXT (*.txt)|All files (*.*)";
            dialog.FilterIndex = 0;
            dialog.RestoreDirectory = true;
            if (dialog.ShowDialog() == DialogResult.OK)
                this.controller.ExportSettings(dialog.FileName);

        }

        private void toolStripExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void TrayForm_Resize(object sender, EventArgs e)
        {
            if (this.WindowState == FormWindowState.Minimized)
                this.Hide();
        }

        private void notifyIcon_DoubleClick(object sender, EventArgs e)
        {
            this.Show();
        }

        private void TrayForm_Shown(object sender, EventArgs e)
        {
            this.listBoxAudioDevices.Items.Add("Loading Audio Devices...");
            this.devices = AudioDeviceInfo.GetAudioDevices();
            this.listBoxAudioDevices.Items.Clear();
            foreach (AudioDeviceInfo.DeviceInfo device in devices)
            {
                this.listBoxAudioDevices.Items.Add(device);
            }
        }

        private void listBoxMidiIn_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (this.listBoxMidiIn.SelectedIndex == -1)
                return;
            this.midiIn = this.listBoxMidiIn.SelectedItem.ToString();
            this.listBoxMidiIn.Enabled = false;
            this.listBoxMidiOut.Enabled = true;
        }

        private void listBoxMidiOut_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (this.listBoxMidiOut.SelectedIndex == -1)
                return;
            this.midiOut = this.listBoxMidiOut.SelectedItem.ToString();
            this.controller = new MIDI2Win(Controller.FilterType.Name, this.midiIn, this.midiOut);
            this.controller.OnMidi += Controller_OnMidi;
            this.listBoxMidiOut.Enabled = false;
            this.listBoxAudioDevices.Enabled = true;
            this.btnAddControl.Enabled = true;
            this.fileToolStripMenuItem.Enabled = true;
            this.txtMute.Enabled = true;
            this.txtVolume.Enabled = true;
            this.toolStripLoad.Enabled = true;
            this.toolStripSave.Enabled = true;
        }

        private void btnAddControl_Click(object sender, EventArgs e)
        {
            try
            {
                string guid = ((AudioDeviceInfo.DeviceInfo)this.listBoxAudioDevices.SelectedItem).guid;
                int volumecontrol = Convert.ToInt32(this.txtVolume.Text);
                int mutecontrol = Convert.ToInt32(this.txtMute.Text);
                controller.AddControl(volumecontrol, mutecontrol, guid);
            }
            catch (FormatException) { }
        }

        private void Controller_OnMidi(object sender, MIDI2Win.MidiControlArgs e)
        {
            this.lblControl.Invoke((MethodInvoker) delegate () { this.lblControl.Text = string.Format("Last Pressed: {0}", e.control.ToString()); });
        }
    }
}
