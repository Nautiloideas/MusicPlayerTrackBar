namespace TestApp2
{
    partial class Form1
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            btnPlay = new Button();
            btnReset = new Button();
            lblStyleCaption = new Label();
            cboStyle = new ComboBox();
            lblThumbCaption = new Label();
            cboThumbStyle = new ComboBox();
            chkCompatibilityMode = new CheckBox();
            btnAddMarker = new Button();
            btnClearMarkers = new Button();
            lblStatus = new Label();
            lblTimeCaption = new Label();
            lblTimeInfo = new Label();
            musicPlayerTrackBar1 = new MusicPlayerTrackBar.MusicPlayerTrackBar();
            trackBar1 = new TrackBar();
            ((System.ComponentModel.ISupportInitialize)trackBar1).BeginInit();
            SuspendLayout();
            
            // btnPlay
            // 
            btnPlay.Location = new Point(20, 20);
            btnPlay.Name = "btnPlay";
            btnPlay.Size = new Size(120, 30);
            btnPlay.TabIndex = 0;
            btnPlay.Text = "播放/暂停";
            btnPlay.UseVisualStyleBackColor = true;
            btnPlay.Click += btnPlay_Click;
            
            // btnReset
            // 
            btnReset.Location = new Point(150, 20);
            btnReset.Name = "btnReset";
            btnReset.Size = new Size(120, 30);
            btnReset.TabIndex = 1;
            btnReset.Text = "重置";
            btnReset.UseVisualStyleBackColor = true;
            btnReset.Click += btnReset_Click;
            
            // lblStyleCaption
            // 
            lblStyleCaption.AutoSize = true;
            lblStyleCaption.Location = new Point(20, 70);
            lblStyleCaption.Name = "lblStyleCaption";
            lblStyleCaption.Size = new Size(100, 20);
            lblStyleCaption.TabIndex = 2;
            lblStyleCaption.Text = "轨道条样式:";
            
            // cboStyle
            // 
            cboStyle.DropDownStyle = ComboBoxStyle.DropDownList;
            cboStyle.FormattingEnabled = true;
            cboStyle.Items.AddRange(new object[] { "Flat", "Round" });
            cboStyle.Location = new Point(120, 70);
            cboStyle.Name = "cboStyle";
            cboStyle.Size = new Size(150, 30);
            cboStyle.TabIndex = 3;
            cboStyle.SelectedIndexChanged += cboStyle_SelectedIndexChanged;
            
            // lblThumbCaption
            // 
            lblThumbCaption.AutoSize = true;
            lblThumbCaption.Location = new Point(20, 110);
            lblThumbCaption.Name = "lblThumbCaption";
            lblThumbCaption.Size = new Size(100, 20);
            lblThumbCaption.TabIndex = 4;
            lblThumbCaption.Text = "滑块样式:";
            
            // cboThumbStyle
            // 
            cboThumbStyle.DropDownStyle = ComboBoxStyle.DropDownList;
            cboThumbStyle.FormattingEnabled = true;
            cboThumbStyle.Items.AddRange(new object[] { "Circle", "Rectangle", "Triangle" });
            cboThumbStyle.Location = new Point(120, 110);
            cboThumbStyle.Name = "cboThumbStyle";
            cboThumbStyle.Size = new Size(150, 30);
            cboThumbStyle.TabIndex = 5;
            cboThumbStyle.SelectedIndexChanged += cboThumbStyle_SelectedIndexChanged;
            
            // chkCompatibilityMode
            // 
            chkCompatibilityMode.AutoSize = true;
            chkCompatibilityMode.Checked = true;
            chkCompatibilityMode.CheckState = CheckState.Checked;
            chkCompatibilityMode.Location = new Point(20, 150);
            chkCompatibilityMode.Name = "chkCompatibilityMode";
            chkCompatibilityMode.Size = new Size(150, 30);
            chkCompatibilityMode.TabIndex = 9;
            chkCompatibilityMode.Text = "兼容模式";
            chkCompatibilityMode.UseVisualStyleBackColor = true;
            chkCompatibilityMode.CheckedChanged += chkCompatibilityMode_CheckedChanged;
            
            // btnAddMarker
            // 
            btnAddMarker.Location = new Point(20, 190);
            btnAddMarker.Name = "btnAddMarker";
            btnAddMarker.Size = new Size(120, 30);
            btnAddMarker.TabIndex = 10;
            btnAddMarker.Text = "添加标记";
            btnAddMarker.UseVisualStyleBackColor = true;
            btnAddMarker.Click += btnAddMarker_Click;
            
            // btnClearMarkers
            // 
            btnClearMarkers.Location = new Point(150, 190);
            btnClearMarkers.Name = "btnClearMarkers";
            btnClearMarkers.Size = new Size(120, 30);
            btnClearMarkers.TabIndex = 11;
            btnClearMarkers.Text = "清除标记";
            btnClearMarkers.UseVisualStyleBackColor = true;
            btnClearMarkers.Click += btnClearMarkers_Click;
            
            // musicPlayerTrackBar1
            // 
            musicPlayerTrackBar1.BackColor = Color.Transparent;
            musicPlayerTrackBar1.BufferedColor = Color.FromArgb(80, 80, 80);
            musicPlayerTrackBar1.BufferedTime = 0L;
            musicPlayerTrackBar1.CompatibilityMode = true;
            musicPlayerTrackBar1.CurrentTime = 0L;
            musicPlayerTrackBar1.Duration = 100000L;
            musicPlayerTrackBar1.Location = new Point(20, 240);
            musicPlayerTrackBar1.Name = "musicPlayerTrackBar1";
            musicPlayerTrackBar1.ProgressColor = Color.FromArgb(0, 154, 251);
            musicPlayerTrackBar1.Size = new Size(800, 30);
            musicPlayerTrackBar1.TabIndex = 12;
            musicPlayerTrackBar1.Text = "musicPlayerTrackBar1";
            musicPlayerTrackBar1.ThumbColor = Color.FromArgb(255, 255, 255);
            musicPlayerTrackBar1.ThumbImage = null;
            musicPlayerTrackBar1.ThumbSize = 16;
            musicPlayerTrackBar1.TrackColor = Color.FromArgb(50, 50, 50);
            
            // trackBar1
            // 
            trackBar1.Location = new Point(20, 290);
            trackBar1.Name = "trackBar1";
            trackBar1.Size = new Size(800, 45);
            trackBar1.TabIndex = 13;
            
            // lblStatus
            // 
            lblStatus.AutoSize = true;
            lblStatus.Font = new Font("Arial", 10F, FontStyle.Regular, GraphicsUnit.Point);
            lblStatus.Location = new Point(20, 340);
            lblStatus.Name = "lblStatus";
            lblStatus.Size = new Size(800, 20);
            lblStatus.TabIndex = 14;
            lblStatus.Text = "就绪";
            
            // lblTimeCaption
            // 
            lblTimeCaption.AutoSize = true;
            lblTimeCaption.Location = new Point(20, 370);
            lblTimeCaption.Name = "lblTimeCaption";
            lblTimeCaption.Size = new Size(100, 20);
            lblTimeCaption.TabIndex = 15;
            lblTimeCaption.Text = "当前进度:";
            
            // lblTimeInfo
            // 
            lblTimeInfo.AutoSize = true;
            lblTimeInfo.Font = new Font("Arial", 10F, FontStyle.Regular, GraphicsUnit.Point);
            lblTimeInfo.Location = new Point(120, 370);
            lblTimeInfo.Name = "lblTimeInfo";
            lblTimeInfo.Size = new Size(200, 20);
            lblTimeInfo.TabIndex = 16;
            lblTimeInfo.Text = "00:00 / 03:00";
            
            // Form1
            // 
            AutoScaleDimensions = new SizeF(7F, 17F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(900, 420);
            Controls.Add(btnPlay);
            Controls.Add(btnReset);
            Controls.Add(lblStyleCaption);
            Controls.Add(cboStyle);
            Controls.Add(lblThumbCaption);
            Controls.Add(cboThumbStyle);
            Controls.Add(chkCompatibilityMode);
            Controls.Add(btnAddMarker);
            Controls.Add(btnClearMarkers);
            Controls.Add(lblStatus);
            Controls.Add(lblTimeCaption);
            Controls.Add(lblTimeInfo);
            Controls.Add(musicPlayerTrackBar1);
            Controls.Add(trackBar1);
            Name = "Form1";
            Text = "音乐播放器 TrackBar 测试";
            ((System.ComponentModel.ISupportInitialize)trackBar1).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Button btnPlay;
        private Button btnReset;
        private Label lblStyleCaption;
        private ComboBox cboStyle;
        private Label lblThumbCaption;
        private ComboBox cboThumbStyle;
        private CheckBox chkCompatibilityMode;
        private Button btnAddMarker;
        private Button btnClearMarkers;
        private Label lblStatus;
        private Label lblTimeCaption;
        private Label lblTimeInfo;
        private MusicPlayerTrackBar.MusicPlayerTrackBar musicPlayerTrackBar1;
        private TrackBar trackBar1;
    }
}
