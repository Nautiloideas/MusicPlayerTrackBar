using System;
using System.Drawing;
using System.Windows.Forms;
using MusicPlayerTrackBar;

namespace TestApp2
{
    public partial class Form1 : Form
    {
        private bool _isPlaying = false;
        private System.Windows.Forms.Timer _timer;

        public Form1()
        {
            InitializeComponent();

            // 初始化UI和事件订阅
            _timer = new System.Windows.Forms.Timer();
            _timer.Interval = 100;
            _timer.Tick += Timer_Tick;

            // 事件订阅
            musicPlayerTrackBar1.TimeChanged += MusicPlayerTrackBar1_TimeChanged;
            musicPlayerTrackBar1.PreviewPositionChanged += MusicPlayerTrackBar1_PreviewPositionChanged;

            // 初始选择下拉框
            cboStyle.SelectedIndex = musicPlayerTrackBar1.TrackBarStyle == TrackBarStyle.Flat ? 0 : 1;
            cboThumbStyle.SelectedIndex = (int)musicPlayerTrackBar1.ThumbStyle;
            cboTimeTextPosition.SelectedIndex = (int)musicPlayerTrackBar1.TimeTextPosition;
            
            // 更新持续时间控件的值
            numDuration.Value = musicPlayerTrackBar1.Duration / 1000; // 转换为秒
            
            // 更新UI状态
            UpdateTimeDisplay();
            lblStatus.Text = "就绪";
            chkCompatibilityMode.Checked = musicPlayerTrackBar1.CompatibilityMode;
        }

        private void UpdateTimeDisplay()
        {
            // 格式化并显示时间信息
            string currentTime = FormatTime(musicPlayerTrackBar1.CurrentTime);
            string totalTime = FormatTime(musicPlayerTrackBar1.Duration);
            lblTimeInfo.Text = $"{currentTime} / {totalTime}";
        }

        private string FormatTime(long milliseconds)
        {
            TimeSpan time = TimeSpan.FromMilliseconds(milliseconds);
            int totalMinutes = (int)time.TotalMinutes;
            
            // 对于超过59分钟的时间，显示总分钟数而不是重置
            if (totalMinutes >= 60)
            {
                return $"{totalMinutes}:{time.Seconds:00}";
            }
            
            // 默认使用00:00格式
            return $"{time.Minutes:00}:{time.Seconds:00}";
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            // 更新当前时间和UI
            if (_isPlaying)
            {
                long newTime = musicPlayerTrackBar1.CurrentTime + _timer.Interval;
                if (newTime > musicPlayerTrackBar1.Duration)
                {
                    newTime = 0;
                    _isPlaying = false;
                    _timer.Stop();
                    btnPlay.Text = "播放";
                }
                musicPlayerTrackBar1.CurrentTime = newTime;
                UpdateTimeDisplay();
            }
        }

        private void MusicPlayerTrackBar1_TimeChanged(object sender, TimeChangedEventArgs e)
        {
            UpdateTimeDisplay();
            lblStatus.Text = $"时间改变: {FormatTime(e.CurrentTime)}";
        }

        private void MusicPlayerTrackBar1_PreviewPositionChanged(object sender, TimeChangedEventArgs e)
        {
            lblStatus.Text = $"预览位置: {FormatTime(e.CurrentTime)}";
        }

        private void btnPlay_Click(object sender, EventArgs e)
        {
            _isPlaying = !_isPlaying;
            if (_isPlaying)
            {
                _timer.Start();
                btnPlay.Text = "暂停";
            }
            else
            {
                _timer.Stop();
                btnPlay.Text = "播放";
            }
        }

        private void btnReset_Click(object sender, EventArgs e)
        {
            _isPlaying = false;
            _timer.Stop();
            btnPlay.Text = "播放";
            musicPlayerTrackBar1.CurrentTime = 0;
            UpdateTimeDisplay();
        }

        private void cboStyle_SelectedIndexChanged(object sender, EventArgs e)
        {
            musicPlayerTrackBar1.TrackBarStyle = cboStyle.SelectedIndex == 0 ? 
                TrackBarStyle.Flat : TrackBarStyle.Round;
        }

        private void cboThumbStyle_SelectedIndexChanged(object sender, EventArgs e)
        {
            musicPlayerTrackBar1.ThumbStyle = (ThumbStyle)cboThumbStyle.SelectedIndex;
        }

        private void cboTimeTextPosition_SelectedIndexChanged(object sender, EventArgs e)
        {
            musicPlayerTrackBar1.TimeTextPosition = (TimeTextPosition)cboTimeTextPosition.SelectedIndex;
        }

        private void chkCompatibilityMode_CheckedChanged(object sender, EventArgs e)
        {
            musicPlayerTrackBar1.CompatibilityMode = chkCompatibilityMode.Checked;
        }

        private void btnAddMarker_Click(object sender, EventArgs e)
        {
            TimeSpan markerTime = TimeSpan.FromMilliseconds(musicPlayerTrackBar1.CurrentTime);
            int totalMinutes = (int)markerTime.TotalMinutes;
            
            string markerTimeText;
            if (totalMinutes >= 60)
            {
                // 如果总分钟数大于等于60，使用总分钟格式
                markerTimeText = $"{totalMinutes}:{markerTime.Seconds:00}";
            }
            else
            {
                // 否则使用标准格式
                markerTimeText = $"{markerTime.Minutes:00}:{markerTime.Seconds:00}";
            }
            
            musicPlayerTrackBar1.AddChapterMarker(musicPlayerTrackBar1.CurrentTime, $"标记 {markerTimeText}", Color.OrangeRed);
            lblStatus.Text = $"添加标记点: {FormatTime(musicPlayerTrackBar1.CurrentTime)}";
        }

        private void btnClearMarkers_Click(object sender, EventArgs e)
        {
            musicPlayerTrackBar1.ClearChapterMarkers();
            lblStatus.Text = "已清除所有标记点";
        }
        
        private void numDuration_ValueChanged(object sender, EventArgs e)
        {
            // 将秒转换为毫秒并设置总时长
            long durationMs = (long)(numDuration.Value * 1000);
            musicPlayerTrackBar1.Duration = durationMs;
            
            // 确保当前时间不超过总时长
            if (musicPlayerTrackBar1.CurrentTime > durationMs)
            {
                musicPlayerTrackBar1.CurrentTime = 0;
            }
            
            // 更新显示
            UpdateTimeDisplay();
            lblStatus.Text = $"总时长已更新: {FormatTime(durationMs)}";
        }
    }
}
