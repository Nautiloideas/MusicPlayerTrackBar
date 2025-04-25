using System;
using System.Drawing;
using System.Windows.Forms;
using MusicPlayerTrackBar;

namespace TestApp2
{
    public partial class Form1 : Form
    {
        private System.Windows.Forms.Timer playTimer;
        private System.Windows.Forms.Timer bufferTimer;
        private Random random = new Random();
        private bool isPlaying = false;
        
        public Form1()
        {
            InitializeComponent();
            InitializeApp();
        }
        
        private void InitializeApp()
        {
            // 初始化定时器
            playTimer = new System.Windows.Forms.Timer
            {
                Interval = 100, // 100毫秒更新一次
                Enabled = false
            };
            playTimer.Tick += PlayTimer_Tick;
            
            bufferTimer = new System.Windows.Forms.Timer
            {
                Interval = 500, // 500毫秒更新一次
                Enabled = false
            };
            bufferTimer.Tick += BufferTimer_Tick;
            
            // 设置控件初始属性
            musicPlayerTrackBar1.Duration = 180000; // 3分钟
            musicPlayerTrackBar1.ShowBuffered = true;
            musicPlayerTrackBar1.ShowMarkers = true;
            musicPlayerTrackBar1.TimeDisplay = TimeDisplayType.Always;
            
            // 添加事件处理
            musicPlayerTrackBar1.TimeChanged += MusicPlayerTrackBar1_TimeChanged;
            musicPlayerTrackBar1.DragStarted += MusicPlayerTrackBar1_DragStarted;
            musicPlayerTrackBar1.DragCompleted += MusicPlayerTrackBar1_DragCompleted;
            
            // 设置常规TrackBar
            trackBar1.Maximum = 100;
            trackBar1.ValueChanged += TrackBar1_ValueChanged;
            
            // 设置下拉框初始选项
            cboStyle.SelectedIndex = 0;
            cboThumbStyle.SelectedIndex = 0;
            
            // 更新初始时间信息
            UpdateTimeInfo(0);
        }
        
        private void TrackBar1_ValueChanged(object sender, EventArgs e)
        {
            if (!isPlaying && chkCompatibilityMode.Checked)
            {
                musicPlayerTrackBar1.Value = trackBar1.Value;
            }
        }
        
        private void btnPlay_Click(object sender, EventArgs e)
        {
            isPlaying = !isPlaying;
            
            if (isPlaying)
            {
                playTimer.Start();
                bufferTimer.Start();
                lblStatus.Text = "播放中...";
            }
            else
            {
                playTimer.Stop();
                bufferTimer.Stop();
                lblStatus.Text = "已暂停";
            }
        }
        
        private void btnReset_Click(object sender, EventArgs e)
        {
            isPlaying = false;
            playTimer.Stop();
            bufferTimer.Stop();
            
            musicPlayerTrackBar1.CurrentTime = 0;
            musicPlayerTrackBar1.BufferedTime = 0;
            trackBar1.Value = 0;
            
            lblStatus.Text = "已重置";
            UpdateTimeInfo(0);
        }
        
        private void btnAddMarker_Click(object sender, EventArgs e)
        {
            AddRandomMarker();
        }
        
        private void btnClearMarkers_Click(object sender, EventArgs e)
        {
            musicPlayerTrackBar1.ClearChapterMarkers();
        }
        
        private void cboStyle_SelectedIndexChanged(object sender, EventArgs e)
        {
            musicPlayerTrackBar1.TrackBarStyle = (TrackBarStyle)cboStyle.SelectedIndex;
        }
        
        private void cboThumbStyle_SelectedIndexChanged(object sender, EventArgs e)
        {
            musicPlayerTrackBar1.ThumbStyle = (ThumbStyle)cboThumbStyle.SelectedIndex;
        }
        
        private void chkCompatibilityMode_CheckedChanged(object sender, EventArgs e)
        {
            musicPlayerTrackBar1.CompatibilityMode = chkCompatibilityMode.Checked;
        }
        
        private void PlayTimer_Tick(object sender, EventArgs e)
        {
            // 模拟播放进度
            long currentTime = musicPlayerTrackBar1.CurrentTime;
            currentTime += 500; // 每次增加500毫秒
            
            if (currentTime > musicPlayerTrackBar1.Duration)
            {
                // 播放结束
                currentTime = 0;
                isPlaying = false;
                playTimer.Stop();
                bufferTimer.Stop();
                lblStatus.Text = "播放完成";
            }
            
            musicPlayerTrackBar1.CurrentTime = currentTime;
            
            // 同步标准TrackBar
            if (chkCompatibilityMode.Checked)
            {
                trackBar1.Value = musicPlayerTrackBar1.Value;
            }
            
            UpdateTimeInfo(currentTime);
        }
        
        private void BufferTimer_Tick(object sender, EventArgs e)
        {
            // 模拟缓冲进度
            long bufferedTime = musicPlayerTrackBar1.BufferedTime;
            long targetBuffer = musicPlayerTrackBar1.CurrentTime + 20000; // 缓冲比当前位置快20秒
            
            if (bufferedTime < targetBuffer && bufferedTime < musicPlayerTrackBar1.Duration)
            {
                bufferedTime += 5000; // 每次增加5秒缓冲
                musicPlayerTrackBar1.BufferedTime = Math.Min(bufferedTime, musicPlayerTrackBar1.Duration);
            }
        }
        
        private void MusicPlayerTrackBar1_TimeChanged(object sender, TimeChangedEventArgs e)
        {
            UpdateTimeInfo(e.CurrentTime);
        }
        
        private void MusicPlayerTrackBar1_DragStarted(object sender, MusicPlayerTrackBar.DragEventArgs e)
        {
            lblStatus.Text = "拖动开始: " + musicPlayerTrackBar1.FormatTime(e.Time);
        }
        
        private void MusicPlayerTrackBar1_DragCompleted(object sender, MusicPlayerTrackBar.DragEventArgs e)
        {
            lblStatus.Text = "拖动结束: " + musicPlayerTrackBar1.FormatTime(e.Time);
            
            // 同步标准TrackBar
            if (chkCompatibilityMode.Checked)
            {
                trackBar1.Value = musicPlayerTrackBar1.Value;
            }
        }
        
        private void AddRandomMarker()
        {
            // 添加随机位置的标记
            long time = (long)(random.NextDouble() * musicPlayerTrackBar1.Duration);
            string title = "标记 " + (DateTime.Now.Ticks % 1000);
            
            // 随机颜色
            Color color = Color.FromArgb(
                random.Next(100, 255),
                random.Next(100, 255),
                random.Next(100, 255)
            );
            
            musicPlayerTrackBar1.AddChapterMarker(time, title, color);
            lblStatus.Text = "添加标记: " + title + " 在 " + musicPlayerTrackBar1.FormatTime(time);
        }
        
        private void UpdateTimeInfo(long time)
        {
            string currentTime = musicPlayerTrackBar1.FormatTime(time);
            string totalTime = musicPlayerTrackBar1.FormatTime(musicPlayerTrackBar1.Duration);
            lblTimeInfo.Text = $"{currentTime} / {totalTime}";
        }
    }
}
