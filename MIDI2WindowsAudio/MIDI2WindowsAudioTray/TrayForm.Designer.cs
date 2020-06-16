namespace MIDI2WindowsAudioTray
{
    partial class TrayForm
    {
        /// <summary>
        /// Erforderliche Designervariable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Verwendete Ressourcen bereinigen.
        /// </summary>
        /// <param name="disposing">True, wenn verwaltete Ressourcen gelöscht werden sollen; andernfalls False.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Vom Windows Form-Designer generierter Code

        /// <summary>
        /// Erforderliche Methode für die Designerunterstützung.
        /// Der Inhalt der Methode darf nicht mit dem Code-Editor geändert werden.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(TrayForm));
            this.contextMenuStrip = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.toolStripLoad = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSave = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripExit = new System.Windows.Forms.ToolStripMenuItem();
            this.notifyIcon = new System.Windows.Forms.NotifyIcon(this.components);
            this.menuStrip = new System.Windows.Forms.MenuStrip();
            this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.importToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.saveToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.listBoxAudioDevices = new System.Windows.Forms.ListBox();
            this.lblAudioDevices = new System.Windows.Forms.Label();
            this.btnAddControl = new System.Windows.Forms.Button();
            this.txtVolume = new System.Windows.Forms.TextBox();
            this.lblVolumeControl = new System.Windows.Forms.Label();
            this.lblMuteControl = new System.Windows.Forms.Label();
            this.txtMute = new System.Windows.Forms.TextBox();
            this.listBoxMidiOut = new System.Windows.Forms.ListBox();
            this.listBoxMidiIn = new System.Windows.Forms.ListBox();
            this.lblMidiIn = new System.Windows.Forms.Label();
            this.lblMidiOut = new System.Windows.Forms.Label();
            this.lblControl = new System.Windows.Forms.Label();
            this.exportToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.toolStripStatusText = new System.Windows.Forms.ToolStripStatusLabel();
            this.contextMenuStrip.SuspendLayout();
            this.menuStrip.SuspendLayout();
            this.statusStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // contextMenuStrip
            // 
            this.contextMenuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripLoad,
            this.toolStripSave,
            this.toolStripExit});
            this.contextMenuStrip.Name = "contextMenuStrip";
            this.contextMenuStrip.Size = new System.Drawing.Size(101, 70);
            // 
            // toolStripLoad
            // 
            this.toolStripLoad.Enabled = false;
            this.toolStripLoad.Name = "toolStripLoad";
            this.toolStripLoad.Size = new System.Drawing.Size(100, 22);
            this.toolStripLoad.Text = "Load";
            this.toolStripLoad.Click += new System.EventHandler(this.LoadFile_Click);
            // 
            // toolStripSave
            // 
            this.toolStripSave.Enabled = false;
            this.toolStripSave.Name = "toolStripSave";
            this.toolStripSave.Size = new System.Drawing.Size(100, 22);
            this.toolStripSave.Text = "Save";
            this.toolStripSave.Click += new System.EventHandler(this.SaveFile_Click);
            // 
            // toolStripExit
            // 
            this.toolStripExit.Name = "toolStripExit";
            this.toolStripExit.Size = new System.Drawing.Size(100, 22);
            this.toolStripExit.Text = "Exit";
            this.toolStripExit.Click += new System.EventHandler(this.toolStripExit_Click);
            // 
            // notifyIcon
            // 
            this.notifyIcon.ContextMenuStrip = this.contextMenuStrip;
            this.notifyIcon.Icon = ((System.Drawing.Icon)(resources.GetObject("notifyIcon.Icon")));
            this.notifyIcon.Text = "MIDI2WindowsAudio";
            this.notifyIcon.Visible = true;
            this.notifyIcon.DoubleClick += new System.EventHandler(this.notifyIcon_DoubleClick);
            // 
            // menuStrip
            // 
            this.menuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem});
            this.menuStrip.Location = new System.Drawing.Point(0, 0);
            this.menuStrip.Name = "menuStrip";
            this.menuStrip.Size = new System.Drawing.Size(624, 24);
            this.menuStrip.TabIndex = 1;
            this.menuStrip.Text = "menuStrip";
            // 
            // fileToolStripMenuItem
            // 
            this.fileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.importToolStripMenuItem,
            this.exportToolStripMenuItem,
            this.saveToolStripMenuItem});
            this.fileToolStripMenuItem.Enabled = false;
            this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            this.fileToolStripMenuItem.Size = new System.Drawing.Size(37, 20);
            this.fileToolStripMenuItem.Text = "File";
            // 
            // importToolStripMenuItem
            // 
            this.importToolStripMenuItem.Name = "importToolStripMenuItem";
            this.importToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.importToolStripMenuItem.Text = "Import";
            this.importToolStripMenuItem.Click += new System.EventHandler(this.LoadFile_Click);
            // 
            // saveToolStripMenuItem
            // 
            this.saveToolStripMenuItem.Name = "saveToolStripMenuItem";
            this.saveToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.saveToolStripMenuItem.Text = "Save";
            this.saveToolStripMenuItem.Click += new System.EventHandler(this.SaveFile_Click);
            // 
            // listBoxAudioDevices
            // 
            this.listBoxAudioDevices.Enabled = false;
            this.listBoxAudioDevices.FormattingEnabled = true;
            this.listBoxAudioDevices.Location = new System.Drawing.Point(221, 55);
            this.listBoxAudioDevices.Name = "listBoxAudioDevices";
            this.listBoxAudioDevices.Size = new System.Drawing.Size(300, 212);
            this.listBoxAudioDevices.TabIndex = 2;
            // 
            // lblAudioDevices
            // 
            this.lblAudioDevices.AutoSize = true;
            this.lblAudioDevices.Location = new System.Drawing.Point(218, 39);
            this.lblAudioDevices.Name = "lblAudioDevices";
            this.lblAudioDevices.Size = new System.Drawing.Size(73, 13);
            this.lblAudioDevices.TabIndex = 3;
            this.lblAudioDevices.Text = "AudioDevices";
            // 
            // btnAddControl
            // 
            this.btnAddControl.Enabled = false;
            this.btnAddControl.Location = new System.Drawing.Point(527, 124);
            this.btnAddControl.Name = "btnAddControl";
            this.btnAddControl.Size = new System.Drawing.Size(78, 23);
            this.btnAddControl.TabIndex = 4;
            this.btnAddControl.Text = "Bind";
            this.btnAddControl.UseVisualStyleBackColor = true;
            this.btnAddControl.Click += new System.EventHandler(this.btnAddControl_Click);
            // 
            // txtVolume
            // 
            this.txtVolume.Enabled = false;
            this.txtVolume.Location = new System.Drawing.Point(527, 55);
            this.txtVolume.Name = "txtVolume";
            this.txtVolume.Size = new System.Drawing.Size(78, 20);
            this.txtVolume.TabIndex = 5;
            // 
            // lblVolumeControl
            // 
            this.lblVolumeControl.AutoSize = true;
            this.lblVolumeControl.Location = new System.Drawing.Point(527, 39);
            this.lblVolumeControl.Name = "lblVolumeControl";
            this.lblVolumeControl.Size = new System.Drawing.Size(78, 13);
            this.lblVolumeControl.TabIndex = 6;
            this.lblVolumeControl.Text = "Volume Control";
            // 
            // lblMuteControl
            // 
            this.lblMuteControl.AutoSize = true;
            this.lblMuteControl.Location = new System.Drawing.Point(527, 82);
            this.lblMuteControl.Name = "lblMuteControl";
            this.lblMuteControl.Size = new System.Drawing.Size(67, 13);
            this.lblMuteControl.TabIndex = 8;
            this.lblMuteControl.Text = "Mute Control";
            // 
            // txtMute
            // 
            this.txtMute.Enabled = false;
            this.txtMute.Location = new System.Drawing.Point(527, 98);
            this.txtMute.Name = "txtMute";
            this.txtMute.Size = new System.Drawing.Size(78, 20);
            this.txtMute.TabIndex = 7;
            // 
            // listBoxMidiOut
            // 
            this.listBoxMidiOut.Enabled = false;
            this.listBoxMidiOut.FormattingEnabled = true;
            this.listBoxMidiOut.Location = new System.Drawing.Point(12, 172);
            this.listBoxMidiOut.Name = "listBoxMidiOut";
            this.listBoxMidiOut.Size = new System.Drawing.Size(203, 95);
            this.listBoxMidiOut.TabIndex = 9;
            this.listBoxMidiOut.SelectedIndexChanged += new System.EventHandler(this.listBoxMidiOut_SelectedIndexChanged);
            // 
            // listBoxMidiIn
            // 
            this.listBoxMidiIn.FormattingEnabled = true;
            this.listBoxMidiIn.Location = new System.Drawing.Point(12, 55);
            this.listBoxMidiIn.Name = "listBoxMidiIn";
            this.listBoxMidiIn.Size = new System.Drawing.Size(203, 82);
            this.listBoxMidiIn.TabIndex = 10;
            this.listBoxMidiIn.SelectedIndexChanged += new System.EventHandler(this.listBoxMidiIn_SelectedIndexChanged);
            // 
            // lblMidiIn
            // 
            this.lblMidiIn.AutoSize = true;
            this.lblMidiIn.Location = new System.Drawing.Point(12, 39);
            this.lblMidiIn.Name = "lblMidiIn";
            this.lblMidiIn.Size = new System.Drawing.Size(42, 13);
            this.lblMidiIn.TabIndex = 11;
            this.lblMidiIn.Text = "MIDI-In";
            // 
            // lblMidiOut
            // 
            this.lblMidiOut.AutoSize = true;
            this.lblMidiOut.Location = new System.Drawing.Point(12, 156);
            this.lblMidiOut.Name = "lblMidiOut";
            this.lblMidiOut.Size = new System.Drawing.Size(50, 13);
            this.lblMidiOut.TabIndex = 12;
            this.lblMidiOut.Text = "MIDI-Out";
            // 
            // lblControl
            // 
            this.lblControl.AutoSize = true;
            this.lblControl.Location = new System.Drawing.Point(527, 156);
            this.lblControl.Name = "lblControl";
            this.lblControl.Size = new System.Drawing.Size(71, 13);
            this.lblControl.TabIndex = 13;
            this.lblControl.Text = "Last Pressed:";
            // 
            // exportToolStripMenuItem
            // 
            this.exportToolStripMenuItem.Name = "exportToolStripMenuItem";
            this.exportToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.exportToolStripMenuItem.Text = "Export";
            this.exportToolStripMenuItem.Click += new System.EventHandler(this.exportToolStripMenuItem_Click);
            // 
            // statusStrip1
            // 
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripStatusText});
            this.statusStrip1.Location = new System.Drawing.Point(0, 276);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(624, 22);
            this.statusStrip1.TabIndex = 14;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // toolStripStatusText
            // 
            this.toolStripStatusText.Name = "toolStripStatusText";
            this.toolStripStatusText.Size = new System.Drawing.Size(39, 17);
            this.toolStripStatusText.Text = "Status";
            // 
            // TrayForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(624, 298);
            this.Controls.Add(this.statusStrip1);
            this.Controls.Add(this.lblControl);
            this.Controls.Add(this.lblMidiOut);
            this.Controls.Add(this.lblMidiIn);
            this.Controls.Add(this.listBoxMidiIn);
            this.Controls.Add(this.listBoxMidiOut);
            this.Controls.Add(this.lblMuteControl);
            this.Controls.Add(this.txtMute);
            this.Controls.Add(this.lblVolumeControl);
            this.Controls.Add(this.txtVolume);
            this.Controls.Add(this.btnAddControl);
            this.Controls.Add(this.lblAudioDevices);
            this.Controls.Add(this.listBoxAudioDevices);
            this.Controls.Add(this.menuStrip);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MainMenuStrip = this.menuStrip;
            this.MaximizeBox = false;
            this.Name = "TrayForm";
            this.ShowIcon = false;
            this.Text = "MIDI2WindowsAudio";
            this.Shown += new System.EventHandler(this.TrayForm_Shown);
            this.Resize += new System.EventHandler(this.TrayForm_Resize);
            this.contextMenuStrip.ResumeLayout(false);
            this.menuStrip.ResumeLayout(false);
            this.menuStrip.PerformLayout();
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ContextMenuStrip contextMenuStrip;
        private System.Windows.Forms.NotifyIcon notifyIcon;
        private System.Windows.Forms.ToolStripMenuItem toolStripLoad;
        private System.Windows.Forms.ToolStripMenuItem toolStripSave;
        private System.Windows.Forms.ToolStripMenuItem toolStripExit;
        private System.Windows.Forms.MenuStrip menuStrip;
        private System.Windows.Forms.ToolStripMenuItem fileToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem importToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem saveToolStripMenuItem;
        private System.Windows.Forms.ListBox listBoxAudioDevices;
        private System.Windows.Forms.Label lblAudioDevices;
        private System.Windows.Forms.Button btnAddControl;
        private System.Windows.Forms.TextBox txtVolume;
        private System.Windows.Forms.Label lblVolumeControl;
        private System.Windows.Forms.Label lblMuteControl;
        private System.Windows.Forms.TextBox txtMute;
        private System.Windows.Forms.ListBox listBoxMidiOut;
        private System.Windows.Forms.ListBox listBoxMidiIn;
        private System.Windows.Forms.Label lblMidiIn;
        private System.Windows.Forms.Label lblMidiOut;
        private System.Windows.Forms.Label lblControl;
        private System.Windows.Forms.ToolStripMenuItem exportToolStripMenuItem;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusText;
    }
}

