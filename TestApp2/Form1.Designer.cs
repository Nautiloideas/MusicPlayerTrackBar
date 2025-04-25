namespace TestApp2
{
    partial class Form1
    {
        /// <summary>
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
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
        /// 设计器支持所需的方法 - 不要修改
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            panel1 = new Panel();
            lblDuration = new Label();
            numDuration = new NumericUpDown();
            chkCompatibilityMode = new CheckBox();
            label3 = new Label();
            cboTimeTextPosition = new ComboBox();
            label2 = new Label();
            cboThumbStyle = new ComboBox();
            label1 = new Label();
            cboStyle = new ComboBox();
            btnClearMarkers = new Button();
            btnAddMarker = new Button();
            btnReset = new Button();
            btnPlay = new Button();
            panel2 = new Panel();
            lblStatus = new Label();
            panel3 = new Panel();
            lblTimeInfo = new Label();
            musicPlayerTrackBar1 = new MusicPlayerTrackBar.MusicPlayerTrackBar();
            panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)numDuration).BeginInit();
            panel2.SuspendLayout();
            panel3.SuspendLayout();
            SuspendLayout();
            // 
            // panel1
            // 
            panel1.Controls.Add(lblDuration);
            panel1.Controls.Add(numDuration);
            panel1.Controls.Add(chkCompatibilityMode);
            panel1.Controls.Add(label3);
            panel1.Controls.Add(cboTimeTextPosition);
            panel1.Controls.Add(label2);
            panel1.Controls.Add(cboThumbStyle);
            panel1.Controls.Add(label1);
            panel1.Controls.Add(cboStyle);
            panel1.Controls.Add(btnClearMarkers);
            panel1.Controls.Add(btnAddMarker);
            panel1.Controls.Add(btnReset);
            panel1.Controls.Add(btnPlay);
            panel1.Dock = DockStyle.Top;
            panel1.Location = new Point(0, 0);
            panel1.Margin = new Padding(4, 4, 4, 4);
            panel1.Name = "panel1";
            panel1.Size = new Size(979, 142);
            panel1.TabIndex = 0;
            // 
            // lblDuration
            // 
            lblDuration.AutoSize = true;
            lblDuration.Location = new Point(382, 98);
            lblDuration.Name = "lblDuration";
            lblDuration.Size = new Size(80, 17);
            lblDuration.TabIndex = 14;
            lblDuration.Text = "总时长(秒)：";
            // 
            // numDuration
            // 
            numDuration.Location = new Point(478, 96);
            numDuration.Maximum = new decimal(new int[] { 7200, 0, 0, 0 });
            numDuration.Minimum = new decimal(new int[] { 1, 0, 0, 0 });
            numDuration.Name = "numDuration";
            numDuration.Size = new Size(140, 25);
            numDuration.TabIndex = 13;
            numDuration.Value = new decimal(new int[] { 60, 0, 0, 0 });
            numDuration.ValueChanged += numDuration_ValueChanged;
            // 
            // chkCompatibilityMode
            // 
            chkCompatibilityMode.AutoSize = true;
            chkCompatibilityMode.Location = new Point(860, 98);
            chkCompatibilityMode.Margin = new Padding(4, 4, 4, 4);
            chkCompatibilityMode.Name = "chkCompatibilityMode";
            chkCompatibilityMode.Size = new Size(75, 21);
            chkCompatibilityMode.TabIndex = 12;
            chkCompatibilityMode.Text = "兼容模式";
            chkCompatibilityMode.UseVisualStyleBackColor = true;
            chkCompatibilityMode.CheckedChanged += chkCompatibilityMode_CheckedChanged;
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Location = new Point(692, 26);
            label3.Margin = new Padding(4, 0, 4, 0);
            label3.Name = "label3";
            label3.Size = new Size(68, 17);
            label3.TabIndex = 9;
            label3.Text = "时间位置：";
            // 
            // cboTimeTextPosition
            // 
            cboTimeTextPosition.DropDownStyle = ComboBoxStyle.DropDownList;
            cboTimeTextPosition.FormattingEnabled = true;
            cboTimeTextPosition.Items.AddRange(new object[] { "不显示", "经典模式 (左侧总时长)", "尾部模式 (左侧当前, 右侧总时长)" });
            cboTimeTextPosition.Location = new Point(775, 21);
            cboTimeTextPosition.Margin = new Padding(4, 4, 4, 4);
            cboTimeTextPosition.Name = "cboTimeTextPosition";
            cboTimeTextPosition.Size = new Size(182, 25);
            cboTimeTextPosition.TabIndex = 8;
            cboTimeTextPosition.SelectedIndexChanged += cboTimeTextPosition_SelectedIndexChanged;
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(382, 62);
            label2.Margin = new Padding(4, 0, 4, 0);
            label2.Name = "label2";
            label2.Size = new Size(68, 17);
            label2.TabIndex = 7;
            label2.Text = "滑块样式：";
            // 
            // cboThumbStyle
            // 
            cboThumbStyle.DropDownStyle = ComboBoxStyle.DropDownList;
            cboThumbStyle.FormattingEnabled = true;
            cboThumbStyle.Items.AddRange(new object[] { "圆形", "方形", "无滑块" });
            cboThumbStyle.Location = new Point(478, 58);
            cboThumbStyle.Margin = new Padding(4, 4, 4, 4);
            cboThumbStyle.Name = "cboThumbStyle";
            cboThumbStyle.Size = new Size(140, 25);
            cboThumbStyle.TabIndex = 6;
            cboThumbStyle.SelectedIndexChanged += cboThumbStyle_SelectedIndexChanged;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(382, 26);
            label1.Margin = new Padding(4, 0, 4, 0);
            label1.Name = "label1";
            label1.Size = new Size(80, 17);
            label1.TabIndex = 5;
            label1.Text = "进度条样式：";
            // 
            // cboStyle
            // 
            cboStyle.DropDownStyle = ComboBoxStyle.DropDownList;
            cboStyle.FormattingEnabled = true;
            cboStyle.Items.AddRange(new object[] { "扁平", "圆角" });
            cboStyle.Location = new Point(478, 21);
            cboStyle.Margin = new Padding(4, 4, 4, 4);
            cboStyle.Name = "cboStyle";
            cboStyle.Size = new Size(140, 25);
            cboStyle.TabIndex = 4;
            cboStyle.SelectedIndexChanged += cboStyle_SelectedIndexChanged;
            // 
            // btnClearMarkers
            // 
            btnClearMarkers.Location = new Point(258, 55);
            btnClearMarkers.Margin = new Padding(4, 4, 4, 4);
            btnClearMarkers.Name = "btnClearMarkers";
            btnClearMarkers.Size = new Size(103, 33);
            btnClearMarkers.TabIndex = 3;
            btnClearMarkers.Text = "清除标记点";
            btnClearMarkers.UseVisualStyleBackColor = true;
            btnClearMarkers.Click += btnClearMarkers_Click;
            // 
            // btnAddMarker
            // 
            btnAddMarker.Location = new Point(148, 55);
            btnAddMarker.Margin = new Padding(4, 4, 4, 4);
            btnAddMarker.Name = "btnAddMarker";
            btnAddMarker.Size = new Size(103, 33);
            btnAddMarker.TabIndex = 2;
            btnAddMarker.Text = "添加标记点";
            btnAddMarker.UseVisualStyleBackColor = true;
            btnAddMarker.Click += btnAddMarker_Click;
            // 
            // btnReset
            // 
            btnReset.Location = new Point(38, 55);
            btnReset.Margin = new Padding(4, 4, 4, 4);
            btnReset.Name = "btnReset";
            btnReset.Size = new Size(103, 33);
            btnReset.TabIndex = 1;
            btnReset.Text = "重置";
            btnReset.UseVisualStyleBackColor = true;
            btnReset.Click += btnReset_Click;
            // 
            // btnPlay
            // 
            btnPlay.Location = new Point(38, 14);
            btnPlay.Margin = new Padding(4, 4, 4, 4);
            btnPlay.Name = "btnPlay";
            btnPlay.Size = new Size(322, 33);
            btnPlay.TabIndex = 0;
            btnPlay.Text = "播放";
            btnPlay.UseVisualStyleBackColor = true;
            btnPlay.Click += btnPlay_Click;
            // 
            // panel2
            // 
            panel2.Controls.Add(lblStatus);
            panel2.Dock = DockStyle.Bottom;
            panel2.Location = new Point(0, 366);
            panel2.Margin = new Padding(4, 4, 4, 4);
            panel2.Name = "panel2";
            panel2.Size = new Size(979, 52);
            panel2.TabIndex = 1;
            // 
            // lblStatus
            // 
            lblStatus.AutoSize = true;
            lblStatus.Location = new Point(36, 18);
            lblStatus.Margin = new Padding(4, 0, 4, 0);
            lblStatus.Name = "lblStatus";
            lblStatus.Size = new Size(44, 17);
            lblStatus.TabIndex = 0;
            lblStatus.Text = "状态：";
            // 
            // panel3
            // 
            panel3.Controls.Add(lblTimeInfo);
            panel3.Controls.Add(musicPlayerTrackBar1);
            panel3.Dock = DockStyle.Fill;
            panel3.Location = new Point(0, 142);
            panel3.Margin = new Padding(4, 4, 4, 4);
            panel3.Name = "panel3";
            panel3.Size = new Size(979, 224);
            panel3.TabIndex = 2;
            // 
            // lblTimeInfo
            // 
            lblTimeInfo.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            lblTimeInfo.AutoSize = true;
            lblTimeInfo.Location = new Point(856, 191);
            lblTimeInfo.Margin = new Padding(4, 0, 4, 0);
            lblTimeInfo.Name = "lblTimeInfo";
            lblTimeInfo.Size = new Size(75, 17);
            lblTimeInfo.TabIndex = 1;
            lblTimeInfo.Text = "00:00/00:00";
            // 
            // musicPlayerTrackBar1
            // 
            musicPlayerTrackBar1.BackColor = Color.Transparent;
            musicPlayerTrackBar1.BufferedColor = Color.Gray;
            musicPlayerTrackBar1.BufferedTime = 0L;
            musicPlayerTrackBar1.CompatibilityMode = false;
            musicPlayerTrackBar1.CurrentTime = 0L;
            musicPlayerTrackBar1.Dock = DockStyle.Fill;
            musicPlayerTrackBar1.Duration = 60000L;
            musicPlayerTrackBar1.Location = new Point(0, 0);
            musicPlayerTrackBar1.Margin = new Padding(2, 3, 2, 3);
            musicPlayerTrackBar1.Name = "musicPlayerTrackBar1";
            musicPlayerTrackBar1.ProgressColor = Color.FromArgb(52, 152, 219);
            musicPlayerTrackBar1.Size = new Size(979, 224);
            musicPlayerTrackBar1.TabIndex = 0;
            musicPlayerTrackBar1.ThumbColor = Color.White;
            musicPlayerTrackBar1.ThumbImage = null;
            musicPlayerTrackBar1.ThumbSize = 12;
            musicPlayerTrackBar1.TimeDisplay = MusicPlayerTrackBar.TimeDisplayType.Always;
            musicPlayerTrackBar1.TimeFont = new Font("Arial", 8F);
            musicPlayerTrackBar1.TrackBarStyle = MusicPlayerTrackBar.TrackBarStyle.Round;
            musicPlayerTrackBar1.TrackColor = Color.DarkGray;
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(7F, 17F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(979, 418);
            Controls.Add(panel3);
            Controls.Add(panel2);
            Controls.Add(panel1);
            Margin = new Padding(4, 4, 4, 4);
            Name = "Form1";
            Text = "音乐播放器进度条测试";
            panel1.ResumeLayout(false);
            panel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)numDuration).EndInit();
            panel2.ResumeLayout(false);
            panel2.PerformLayout();
            panel3.ResumeLayout(false);
            panel3.PerformLayout();
            ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Button btnClearMarkers;
        private System.Windows.Forms.Button btnAddMarker;
        private System.Windows.Forms.Button btnReset;
        private System.Windows.Forms.Button btnPlay;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Label lblStatus;
        private System.Windows.Forms.Panel panel3;
        private MusicPlayerTrackBar.MusicPlayerTrackBar musicPlayerTrackBar1;
        private System.Windows.Forms.Label lblTimeInfo;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox cboStyle;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ComboBox cboThumbStyle;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.ComboBox cboTimeTextPosition;
        private System.Windows.Forms.CheckBox chkCompatibilityMode;
        private System.Windows.Forms.NumericUpDown numDuration;
        private System.Windows.Forms.Label lblDuration;
    }
}
