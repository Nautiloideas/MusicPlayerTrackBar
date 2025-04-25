using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Windows.Forms;

namespace MusicPlayerTrackBar
{
    #region 枚举定义

    /// <summary>
    /// 表示轨道条样式
    /// </summary>
    public enum TrackBarStyle
    {
        Flat,   // 平面风格
        Round   // 圆角风格
    }
    
    /// <summary>
    /// 表示滑块样式
    /// </summary>
    public enum ThumbStyle
    {
        Rectangle,  // 矩形
        Circle,     // 圆形
        Triangle    // 三角形
    }
    
    /// <summary>
    /// 表示标记样式
    /// </summary>
    public enum MarkerStyle
    {
        /// <summary>
        /// 线条标记
        /// </summary>
        Line,
        
        /// <summary>
        /// 三角形标记
        /// </summary>
        Triangle,
        
        /// <summary>
        /// 圆形标记
        /// </summary>
        Circle
    }
    
    /// <summary>
    /// 表示时间显示方式
    /// </summary>
    public enum TimeDisplayType
    {
        /// <summary>
        /// 不显示
        /// </summary>
        None,
        
        /// <summary>
        /// 悬停时显示
        /// </summary>
        OnHover,
        
        /// <summary>
        /// 总是显示
        /// </summary>
        Always
    }
    
    /// <summary>
    /// 时间格式
    /// </summary>
    public enum TimeFormat
    {
        /// <summary>
        /// 标准格式 (mm:ss)
        /// </summary>
        Standard,
        
        /// <summary>
        /// 完整格式 (hh:mm:ss)
        /// </summary>
        Complete,
        
        /// <summary>
        /// 带毫秒 (mm:ss.ms)
        /// </summary>
        WithMilliseconds
    }
    
    /// <summary>
    /// 时间文本位置
    /// </summary>
    public enum TimeTextPosition
    {
        /// <summary>
        /// 不显示时间文本
        /// </summary>
        None,
        
        /// <summary>
        /// 进度条尾部模式（所有时间都显示在进度条后）
        /// </summary>
        EndOfTrack,
        
        /// <summary>
        /// 经典模式（当前时间在前，总时间在后）
        /// </summary>
        Classic
    }
    
    #endregion
    
    /// <summary>
    /// 时间点改变事件参数
    /// </summary>
    public class TimeChangedEventArgs : EventArgs
    {
        /// <summary>
        /// 获取改变后的时间（毫秒）
        /// </summary>
        public long CurrentTime { get; }
        
        /// <summary>
        /// 初始化 TimeChangedEventArgs 实例
        /// </summary>
        /// <param name="time">当前时间（毫秒）</param>
        public TimeChangedEventArgs(long time)
        {
            CurrentTime = time;
        }
    }
    
    /// <summary>
    /// 拖拽开始/结束事件参数
    /// </summary>
    public class DragEventArgs : EventArgs
    {
        /// <summary>
        /// 获取拖拽时间点（毫秒）
        /// </summary>
        public long Time { get; }
        
        /// <summary>
        /// 初始化 DragEventArgs 实例
        /// </summary>
        /// <param name="time">拖拽时间点（毫秒）</param>
        public DragEventArgs(long time)
        {
            Time = time;
        }
    }
    
    /// <summary>
    /// 时间点改变事件处理委托
    /// </summary>
    public delegate void TimeChangedEventHandler(object sender, TimeChangedEventArgs e);
    
    /// <summary>
    /// 拖拽事件处理委托
    /// </summary>
    public delegate void DragEventHandler(object sender, DragEventArgs e);
    
    /// <summary>
    /// 音乐播放器专用TrackBar控件，提供多种外观和音乐播放特性
    /// </summary>
    [Description("音乐播放器专用TrackBar控件，提供多种外观和音乐播放特性")]
    [ToolboxItem(true)]
    [ToolboxBitmap(typeof(MusicPlayerTrackBar), "MusicPlayerTrackBar")]
    [Designer(typeof(MusicPlayerTrackBarDesigner))]
    [DefaultEvent("TimeChanged")]
    public class MusicPlayerTrackBar : Control
    {
        #region 私有字段

        private Color _trackColor = Color.DarkGray;
        private Color _progressColor = Color.DodgerBlue;
        private Color _thumbColor = Color.White;
        private Color _bufferedColor = Color.Gray;
        private int _trackHeight = 5;
        private int _thumbSize = 12;
        private int _cornerRadius = 2;
        private TrackBarStyle _trackStyle = TrackBarStyle.Flat;
        private ThumbStyle _thumbStyle = ThumbStyle.Circle;
        private bool _showTimeTooltip = true;
        private bool _showBufferedProgress = true;
        private TimeSpan _duration = TimeSpan.Zero;
        private TimeSpan _currentTime = TimeSpan.Zero;
        private TimeSpan _bufferedTime = TimeSpan.Zero;
        private bool _isUserDragging = false;
        private Point _dragStartPoint;
        private List<TimeSpan> _markers = new List<TimeSpan>();
        private Color _markerColor = Color.Orange;
        private MarkerStyle _markerStyle = MarkerStyle.Line;
        private int _markerSize = 8;
        
        private TimeDisplayType _timeDisplay = TimeDisplayType.OnHover; // 时间显示方式
        private TimeFormat _timeFormat = TimeFormat.Standard; // 时间格式
        private TimeTextPosition _timeTextPosition = TimeTextPosition.Classic; // 时间文本位置
        
        private Font _timeFont = new Font("Arial", 8);       // 时间显示字体
        
        private bool _showMarkers = false;                   // 是否显示标记点
        private bool _enablePreview = false;                 // 是否启用预览功能
        
        private bool _isDragging = false;                    // 是否正在拖拽
        private bool _isHovered = false;                     // 是否鼠标悬停
        private Point _mousePosition = Point.Empty;          // 鼠标位置
        private RectangleF _thumbRect = RectangleF.Empty;    // 滑块区域
        
        // 章节标记
        private class ChapterMarker
        {
            public long Time { get; set; }             // 时间点
            public string Title { get; set; }          // 标题
            public Color Color { get; set; }           // 颜色
        }
        
        private readonly List<ChapterMarker> _chapterMarkers = new List<ChapterMarker>();
        
        // 缓存
        private Image _thumbImage = null;              // 自定义滑块图像
        
        // 动画
        private double _animatedProgress = 0;          // 动画进度值
        private bool _enableAnimation = true;          // 是否启用动画
        private Timer _animationTimer = null;          // 动画计时器
        
        // 添加新的字段用于拖拽功能
        private bool _isDraggingThumb = false;       // 是否正在拖拽滑块（而不是点击轨道）
        private long _dragCurrentTime = 0;           // 拖拽过程中的临时时间值
        private double _thumbAnimationScale = 1.0;   // 滑块缩放动画值
        private Timer _thumbAnimationTimer = null;   // 滑块动画计时器
        private bool _thumbAnimationDirection = true; // 动画方向: true为放大, false为缩小
        
        #endregion
        
        #region 事件

        /// <summary>
        /// 时间点改变时发生
        /// </summary>
        public event TimeChangedEventHandler TimeChanged;
        
        /// <summary>
        /// 拖拽开始时发生
        /// </summary>
        public event DragEventHandler DragStarted;
        
        /// <summary>
        /// 拖拽结束时发生
        /// </summary>
        public event DragEventHandler DragCompleted;
        
        /// <summary>
        /// 预览位置改变时发生
        /// </summary>
        public event TimeChangedEventHandler PreviewPositionChanged;

        #endregion
        
        #region 构造函数
        
        /// <summary>
        /// 初始化MusicPlayerTrackBar实例
        /// </summary>
        public MusicPlayerTrackBar()
        {
            SetStyle(ControlStyles.AllPaintingInWmPaint |
                    ControlStyles.UserPaint |
                    ControlStyles.ResizeRedraw |
                    ControlStyles.OptimizedDoubleBuffer |
                    ControlStyles.SupportsTransparentBackColor, true);
                    
            BackColor = Color.Transparent;
            Size = new Size(350, 30);
            
            // 初始化动画计时器
            _animationTimer = new Timer
            {
                Interval = 15,
                Enabled = false
            };
            _animationTimer.Tick += AnimationTimer_Tick;
            
            // 初始化滑块动画计时器
            _thumbAnimationTimer = new Timer
            {
                Interval = 10,
                Enabled = false
            };
            _thumbAnimationTimer.Tick += ThumbAnimationTimer_Tick;
            
            // 注册事件处理
            MouseDown += OnMouseDown;
            MouseUp += OnMouseUp;
            MouseMove += OnMouseMove;
            MouseEnter += OnMouseEnter;
            MouseLeave += OnMouseLeave;
        }
        
        /// <summary>
        /// 释放资源
        /// </summary>
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                // 释放托管资源
                _timeFont?.Dispose();
                _thumbImage?.Dispose();
                
                if (_animationTimer != null)
                {
                    _animationTimer.Tick -= AnimationTimer_Tick;
                    _animationTimer.Dispose();
                    _animationTimer = null;
                }
                
                if (_thumbAnimationTimer != null)
                {
                    _thumbAnimationTimer.Tick -= ThumbAnimationTimer_Tick;
                    _thumbAnimationTimer.Dispose();
                    _thumbAnimationTimer = null;
                }
                
                // 取消事件注册
                MouseDown -= OnMouseDown;
                MouseUp -= OnMouseUp;
                MouseMove -= OnMouseMove;
                MouseEnter -= OnMouseEnter;
                MouseLeave -= OnMouseLeave;
            }
            
            base.Dispose(disposing);
        }
        
        #endregion
        
        #region 公开属性
        
        /// <summary>
        /// 获取或设置音频总时长（毫秒）
        /// </summary>
        [Category("行为")]
        [Description("获取或设置音频总时长（毫秒）")]
        [DefaultValue(100000)]
        public long Duration
        {
            get => (long)_duration.TotalMilliseconds;
            set
            {
                if (value < 0)
                    value = 0;
                
                if ((long)_duration.TotalMilliseconds != value)
                {
                    _duration = TimeSpan.FromMilliseconds(value);
                    
                    // 确保当前时间不会超过总时长
                    if ((long)_currentTime.TotalMilliseconds > (long)_duration.TotalMilliseconds)
                        CurrentTime = (long)_duration.TotalMilliseconds;
                        
                    // 确保缓冲时间不会超过总时长
                    if ((long)_bufferedTime.TotalMilliseconds > (long)_duration.TotalMilliseconds)
                        BufferedTime = (long)_duration.TotalMilliseconds;
                        
                    Invalidate();
                }
            }
        }
        
        /// <summary>
        /// 获取或设置当前播放时间（毫秒）
        /// </summary>
        [Category("行为")]
        [Description("获取或设置当前播放时间（毫秒）")]
        [DefaultValue(0)]
        public long CurrentTime
        {
            get => (long)_currentTime.TotalMilliseconds;
            set
            {
                if (value < 0)
                    value = 0;
                else if (value > (long)_duration.TotalMilliseconds)
                    value = (long)_duration.TotalMilliseconds;
                    
                if ((long)_currentTime.TotalMilliseconds != value)
                {
                    // 如果是从拖拽状态结束，确保动画平滑
                    if (_isDraggingThumb)
                    {
                        // 不重置_animatedProgress，保持可视连续性
                        _animatedProgress = (double)_dragCurrentTime / _duration.TotalMilliseconds;
                    }
                    
                    _currentTime = TimeSpan.FromMilliseconds(value);
                    OnTimeChanged(value);
                    
                    // 更新动画目标值
                    if (_enableAnimation && !_isDragging)
                    {
                        _animationTimer.Start();
                    }
                    
                    Invalidate();
                }
            }
        }
        
        /// <summary>
        /// 获取或设置是否启用兼容模式（让Value、Maximum等传统属性生效）
        /// </summary>
        [Category("兼容性")]
        [Description("获取或设置是否启用兼容模式（让Value、Maximum等传统属性生效）")]
        [DefaultValue(true)]
        public bool CompatibilityMode { get; set; } = true;
        
        /// <summary>
        /// 获取或设置控件的当前值（0-100的百分比）
        /// </summary>
        [Category("兼容性")]
        [Description("获取或设置控件的当前值（0-100的百分比）")]
        [DefaultValue(0)]
        public int Value
        {
            get 
            {
                if (_duration.TotalMilliseconds <= 0) return 0;
                return (int)((_currentTime.TotalMilliseconds * 100) / _duration.TotalMilliseconds);
            }
            set
            {
                if (!CompatibilityMode)
                    return; // 兼容模式关闭时忽略此属性的设置
                    
                if (value < 0)
                    value = 0;
                else if (value > 100)
                    value = 100;
                
                // 转换为时间值
                CurrentTime = (long)(_duration.TotalMilliseconds * value / 100.0);
            }
        }
        
        /// <summary>
        /// 获取或设置控件的最大值
        /// </summary>
        [Category("兼容性")]
        [Description("获取或设置控件的最大值")]
        [DefaultValue(100)]
        public int Maximum 
        {
            get => 100;  // 固定为100
            set { /* 忽略设置 - 仅为兼容性 */ }
        }
        
        /// <summary>
        /// 获取或设置控件的最小值
        /// </summary>
        [Category("兼容性")]
        [Description("获取或设置控件的最小值")]
        [DefaultValue(0)]
        public int Minimum 
        {
            get => 0;  // 固定为0
            set { /* 忽略设置 - 仅为兼容性 */ }
        }
        
        /// <summary>
        /// 获取或设置缓冲时间（毫秒）
        /// </summary>
        [Category("行为")]
        [Description("获取或设置缓冲时间（毫秒）")]
        [DefaultValue(0)]
        public long BufferedTime
        {
            get => (long)_bufferedTime.TotalMilliseconds;
            set
            {
                if (value < 0)
                    value = 0;
                else if (value > (long)_duration.TotalMilliseconds)
                    value = (long)_duration.TotalMilliseconds;
                    
                if ((long)_bufferedTime.TotalMilliseconds != value)
                {
                    _bufferedTime = TimeSpan.FromMilliseconds(value);
                    Invalidate();
                }
            }
        }
        
        /// <summary>
        /// 获取或设置TrackBar样式
        /// </summary>
        [Category("外观")]
        [Description("获取或设置TrackBar样式")]
        [DefaultValue(typeof(TrackBarStyle), "Flat")]
        public TrackBarStyle TrackBarStyle
        {
            get => _trackStyle;
            set
            {
                if (_trackStyle != value)
                {
                    _trackStyle = value;
                    Invalidate();
                }
            }
        }
        
        /// <summary>
        /// 获取或设置滑块样式
        /// </summary>
        [Category("外观")]
        [Description("获取或设置滑块样式")]
        [DefaultValue(typeof(ThumbStyle), "Circle")]
        public ThumbStyle ThumbStyle
        {
            get => _thumbStyle;
            set
            {
                if (_thumbStyle != value)
                {
                    _thumbStyle = value;
                    Invalidate();
                }
            }
        }
        
        /// <summary>
        /// 获取或设置时间显示方式
        /// </summary>
        [Category("外观")]
        [Description("获取或设置时间显示方式")]
        [DefaultValue(typeof(TimeDisplayType), "OnHover")]
        public TimeDisplayType TimeDisplay
        {
            get => _timeDisplay;
            set
            {
                if (_timeDisplay != value)
                {
                    _timeDisplay = value;
                    Invalidate();
                }
            }
        }
        
        /// <summary>
        /// 获取或设置时间格式
        /// </summary>
        [Category("外观")]
        [Description("获取或设置时间格式")]
        [DefaultValue(typeof(TimeFormat), "Standard")]
        public TimeFormat TimeFormat
        {
            get => _timeFormat;
            set
            {
                if (_timeFormat != value)
                {
                    _timeFormat = value;
                    Invalidate();
                }
            }
        }
        
        /// <summary>
        /// 获取或设置时间文本位置
        /// </summary>
        [Category("外观")]
        [Description("获取或设置时间文本位置（None-不显示，EndOfTrack-进度条尾部模式，Classic-经典模式）")]
        [DefaultValue(typeof(TimeTextPosition), "Classic")]
        public TimeTextPosition TimeTextPosition
        {
            get => _timeTextPosition;
            set
            {
                if (_timeTextPosition != value)
                {
                    _timeTextPosition = value;
                    Invalidate();
                }
            }
        }
        
        /// <summary>
        /// 获取或设置轨道颜色
        /// </summary>
        [Category("外观")]
        [Description("获取或设置轨道颜色")]
        public Color TrackColor
        {
            get => _trackColor;
            set
            {
                if (_trackColor != value)
                {
                    _trackColor = value;
                    Invalidate();
                }
            }
        }
        
        /// <summary>
        /// 获取或设置进度颜色
        /// </summary>
        [Category("外观")]
        [Description("获取或设置进度颜色")]
        public Color ProgressColor
        {
            get => _progressColor;
            set
            {
                if (_progressColor != value)
                {
                    _progressColor = value;
                    Invalidate();
                }
            }
        }
        
        /// <summary>
        /// 获取或设置缓冲区颜色
        /// </summary>
        [Category("外观")]
        [Description("获取或设置缓冲区颜色")]
        public Color BufferedColor
        {
            get => _bufferedColor;
            set
            {
                if (_bufferedColor != value)
                {
                    _bufferedColor = value;
                    Invalidate();
                }
            }
        }
        
        /// <summary>
        /// 获取或设置滑块颜色
        /// </summary>
        [Category("外观")]
        [Description("获取或设置滑块颜色")]
        public Color ThumbColor
        {
            get => _thumbColor;
            set
            {
                if (_thumbColor != value)
                {
                    _thumbColor = value;
                    Invalidate();
                }
            }
        }
        
        /// <summary>
        /// 获取或设置轨道高度
        /// </summary>
        [Category("外观")]
        [Description("获取或设置轨道高度")]
        [DefaultValue(5)]
        public int TrackHeight
        {
            get => _trackHeight;
            set
            {
                if (_trackHeight != value && value > 0)
                {
                    _trackHeight = value;
                    Invalidate();
                }
            }
        }
        
        /// <summary>
        /// 获取或设置滑块大小
        /// </summary>
        [Category("外观")]
        [Description("获取或设置滑块大小")]
        public int ThumbSize
        {
            get => _thumbSize;
            set
            {
                if (_thumbSize != value && value > 0)
                {
                    _thumbSize = value;
                    Invalidate();
                }
            }
        }
        
        /// <summary>
        /// 获取或设置时间显示字体
        /// </summary>
        [Category("外观")]
        [Description("获取或设置时间显示字体")]
        public Font TimeFont
        {
            get => _timeFont;
            set
            {
                if (_timeFont != value)
                {
                    _timeFont?.Dispose();
                    _timeFont = value;
                    Invalidate();
                }
            }
        }
        
        /// <summary>
        /// 获取或设置是否显示缓冲进度
        /// </summary>
        [Category("行为")]
        [Description("获取或设置是否显示缓冲进度")]
        [DefaultValue(true)]
        public bool ShowBuffered
        {
            get => _showBufferedProgress;
            set
            {
                if (_showBufferedProgress != value)
                {
                    _showBufferedProgress = value;
                    Invalidate();
                }
            }
        }
        
        /// <summary>
        /// 获取或设置是否显示悬停时间
        /// </summary>
        [Category("行为")]
        [Description("获取或设置是否显示悬停时间")]
        [DefaultValue(true)]
        public bool ShowHoverTime
        {
            get => _showTimeTooltip;
            set
            {
                if (_showTimeTooltip != value)
                {
                    _showTimeTooltip = value;
                    Invalidate();
                }
            }
        }
        
        /// <summary>
        /// 获取或设置是否显示章节标记
        /// </summary>
        [Category("行为")]
        [Description("获取或设置是否显示章节标记")]
        [DefaultValue(false)]
        public bool ShowMarkers
        {
            get => _showMarkers;
            set
            {
                if (_showMarkers != value)
                {
                    _showMarkers = value;
                    Invalidate();
                }
            }
        }
        
        /// <summary>
        /// 获取或设置是否启用预览功能
        /// </summary>
        [Category("行为")]
        [Description("获取或设置是否启用预览功能")]
        [DefaultValue(false)]
        public bool EnablePreview
        {
            get => _enablePreview;
            set
            {
                if (_enablePreview != value)
                {
                    _enablePreview = value;
                    Invalidate();
                }
            }
        }
        
        /// <summary>
        /// 获取或设置是否启用动画效果
        /// </summary>
        [Category("行为")]
        [Description("获取或设置是否启用动画效果")]
        [DefaultValue(true)]
        public bool EnableAnimation
        {
            get => _enableAnimation;
            set
            {
                if (_enableAnimation != value)
                {
                    _enableAnimation = value;
                    if (!_enableAnimation && _animationTimer.Enabled)
                    {
                        _animationTimer.Stop();
                    }
                }
            }
        }
        
        /// <summary>
        /// 获取或设置自定义滑块图像
        /// </summary>
        [Category("外观")]
        [Description("获取或设置自定义滑块图像")]
        public Image ThumbImage
        {
            get => _thumbImage;
            set
            {
                if (_thumbImage != value)
                {
                    _thumbImage?.Dispose();
                    _thumbImage = value;
                    Invalidate();
                }
            }
        }
        
        #endregion
        
        #region 公共方法
        
        /// <summary>
        /// 添加章节标记
        /// </summary>
        /// <param name="time">时间点（毫秒）</param>
        /// <param name="title">标题</param>
        /// <param name="color">颜色</param>
        public void AddChapterMarker(long time, string title, Color color)
        {
            if (time < 0 || time > _duration.TotalMilliseconds)
                return;
                
            _chapterMarkers.Add(new ChapterMarker
            {
                Time = time,
                Title = title,
                Color = color
            });
            
            Invalidate();
        }
        
        /// <summary>
        /// 清除所有章节标记
        /// </summary>
        public void ClearChapterMarkers()
        {
            _chapterMarkers.Clear();
            Invalidate();
        }
        
        /// <summary>
        /// 将时间值格式化为字符串
        /// </summary>
        public string FormatTime(long milliseconds)
        {
            TimeSpan time = TimeSpan.FromMilliseconds(milliseconds);
            
            switch (_timeFormat)
            {
                case TimeFormat.Standard:
                    return time.ToString(@"mm\:ss");
                    
                case TimeFormat.Complete:
                    return time.Hours > 0 
                        ? time.ToString(@"hh\:mm\:ss") 
                        : time.ToString(@"mm\:ss");
                    
                case TimeFormat.WithMilliseconds:
                    return time.ToString(@"mm\:ss\.fff");
                    
                default:
                    return time.ToString(@"mm\:ss");
            }
        }
        
        #endregion
        
        #region 事件处理方法
        
        /// <summary>
        /// 处理鼠标按下事件
        /// </summary>
        protected virtual void OnMouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                // 计算滑块矩形
                _thumbRect = GetThumbRectangle();
                
                // 检查是否点击在滑块上
                if (_thumbRect.Contains(e.Location))
                {
                    // 按住滑块开始拖拽
                    _isDraggingThumb = true;
                    _isDragging = true;
                    _dragStartPoint = e.Location;
                    _dragCurrentTime = (long)_currentTime.TotalMilliseconds;
                    
                    // 开始滑块动画
                    StartThumbAnimation();
                    
                    // 触发拖拽开始事件
                    OnDragStarted(_dragCurrentTime);
                }
                else
                {
                    // 直接点击在轨道上，立即更新位置
                    _isDraggingThumb = false;
                    _isDragging = true;
                    
                    // 更新时间
                    long time = CalculateTimeFromPosition(e.X);
                    CurrentTime = time;
                    
                    // 触发拖拽开始事件
                    OnDragStarted(time);
                }
                
                // 捕获焦点
                Focus();
                
                // 更新绘制
                Invalidate();
            }
        }
        
        /// <summary>
        /// 处理鼠标松开事件
        /// </summary>
        protected virtual void OnMouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left && _isDragging)
            {
                long finalTime;
                
                // 使用当前拖拽位置或鼠标点击位置
                if (_isDraggingThumb)
                {
                    finalTime = _dragCurrentTime;
                }
                else
                {
                    finalTime = CalculateTimeFromPosition(e.X);
                }
                
                // 更新实际的播放时间
                // 注意：先更新CurrentTime，再重置状态，避免位置跳跃
                CurrentTime = finalTime;
                
                // 触发拖拽结束事件
                OnDragCompleted(finalTime);
                
                // 重置拖拽状态
                _isDraggingThumb = false;
                _isDragging = false;
                
                // 更新绘制
                Invalidate();
            }
        }
        
        /// <summary>
        /// 处理鼠标移动事件
        /// </summary>
        protected virtual void OnMouseMove(object sender, MouseEventArgs e)
        {
            // 更新鼠标位置
            _mousePosition = e.Location;
            
            if (_isDragging)
            {
                if (_isDraggingThumb)
                {
                    // 拖拽滑块模式
                    // 限制鼠标范围，确保滑块在轨道内
                    int x = Math.Max(0, Math.Min(e.X, Width));
                    
                    // 计算一个临时时间值，但不立即应用
                    _dragCurrentTime = CalculateTimeFromPosition(x);
                    
                    // 立即重绘，提高反应速度
                    Invalidate();
                }
                else
                {
                    // 直接点击轨道模式，实时更新CurrentTime
                    CurrentTime = CalculateTimeFromPosition(e.X);
                }
            }
            else if (_enablePreview && _isHovered)
            {
                // 预览功能，触发预览位置事件
                long previewTime = CalculateTimeFromPosition(e.X);
                OnPreviewPositionChanged(previewTime);
                
                // 更新绘制
                Invalidate();
            }
            else
            {
                // 非拖拽状态的鼠标移动也需要重绘（如显示悬停气泡等）
                Invalidate();
            }
        }
        
        /// <summary>
        /// 处理鼠标进入事件
        /// </summary>
        protected virtual void OnMouseEnter(object sender, EventArgs e)
        {
            _isHovered = true;
            Invalidate();
        }
        
        /// <summary>
        /// 处理鼠标离开事件
        /// </summary>
        protected virtual void OnMouseLeave(object sender, EventArgs e)
        {
            _isHovered = false;
            _mousePosition = Point.Empty;
            Invalidate();
        }
        
        /// <summary>
        /// 处理时间改变事件
        /// </summary>
        protected virtual void OnTimeChanged(long time)
        {
            TimeChanged?.Invoke(this, new TimeChangedEventArgs(time));
        }
        
        /// <summary>
        /// 处理拖拽开始事件
        /// </summary>
        protected virtual void OnDragStarted(long time)
        {
            DragStarted?.Invoke(this, new DragEventArgs(time));
        }
        
        /// <summary>
        /// 处理拖拽结束事件
        /// </summary>
        protected virtual void OnDragCompleted(long time)
        {
            DragCompleted?.Invoke(this, new DragEventArgs(time));
        }
        
        /// <summary>
        /// 处理预览位置改变事件
        /// </summary>
        protected virtual void OnPreviewPositionChanged(long time)
        {
            PreviewPositionChanged?.Invoke(this, new TimeChangedEventArgs(time));
        }
        
        /// <summary>
        /// 处理动画计时器事件
        /// </summary>
        private void AnimationTimer_Tick(object sender, EventArgs e)
        {
            double target = (double)_currentTime.TotalMilliseconds / _duration.TotalMilliseconds;
            double delta = target - _animatedProgress;
            
            // 平滑动画
            if (Math.Abs(delta) < 0.001)
            {
                _animatedProgress = target;
                _animationTimer.Stop();
            }
            else
            {
                _animatedProgress += delta * 0.2; // 动画速度因子
            }
            
            Invalidate();
        }
        
        /// <summary>
        /// 处理滑块动画计时器事件
        /// </summary>
        private void ThumbAnimationTimer_Tick(object sender, EventArgs e)
        {
            const double maxScale = 1.3;
            const double minScale = 1.0;
            const double scaleUpSpeed = 0.08;   // 放大速度
            const double scaleDownSpeed = 0.06; // 缩小速度 (稍慢，更平滑)
            
            if (_isDraggingThumb)
            {
                // 在拖拽过程中，逐渐放大到目标尺寸并保持
                if (_thumbAnimationScale < maxScale)
                {
                    _thumbAnimationScale += scaleUpSpeed;
                    if (_thumbAnimationScale > maxScale)
                        _thumbAnimationScale = maxScale;
                    
                    Invalidate();
                }
            }
            else
            {
                // 拖拽结束，逐渐恢复到原始尺寸
                if (_thumbAnimationScale > minScale)
                {
                    _thumbAnimationScale -= scaleDownSpeed;
                    if (_thumbAnimationScale < minScale)
                    {
                        _thumbAnimationScale = minScale;
                        _thumbAnimationTimer.Stop();
                    }
                    
                    Invalidate();
                }
                else
                {
                    _thumbAnimationTimer.Stop();
                }
            }
        }
        
        #endregion
        
        #region 辅助方法
        
        /// <summary>
        /// 获取轨道矩形区域
        /// </summary>
        private RectangleF GetTrackRectangle()
        {
            // 计算时间文本的宽度，为其留出空间
            SizeF currentTimeSize = new SizeF(0, 0);
            SizeF totalTimeSize = new SizeF(0, 0);
            
            using (Graphics g = CreateGraphics())
            {
                if (g != null)
                {
                    // 使用一个稍长的时间文本作为测量标准，确保有足够空间
                    currentTimeSize = g.MeasureString("00:00", _timeFont);
                    totalTimeSize = g.MeasureString("00:00", _timeFont);
                }
            }
            
            // 进度条左右两侧需要留出的空间（时间文本宽度加上一些间距）
            float leftMargin = (_timeTextPosition == TimeTextPosition.Classic) ? currentTimeSize.Width + 10 : 0;
            float rightMargin = (_timeTextPosition == TimeTextPosition.Classic || _timeTextPosition == TimeTextPosition.EndOfTrack) ? 
                                totalTimeSize.Width + 10 : 0;
            
            // 保持轨道在控件中央的垂直位置，但水平方向根据时间文本留出空间
            return new RectangleF(
                leftMargin,
                (Height - _trackHeight) / 2,
                Width - leftMargin - rightMargin,
                _trackHeight);
        }
        
        /// <summary>
        /// 获取缓冲矩形区域
        /// </summary>
        private RectangleF GetBufferedRectangle()
        {
            RectangleF trackRect = GetTrackRectangle();
            float bufferedWidth = (_duration.TotalMilliseconds <= 0) ? 0 : 
                                 (float)((double)_bufferedTime.TotalMilliseconds / _duration.TotalMilliseconds * trackRect.Width);
            
            return new RectangleF(
                trackRect.X,
                trackRect.Y,
                bufferedWidth,
                trackRect.Height);
        }
        
        /// <summary>
        /// 获取进度矩形区域
        /// </summary>
        private RectangleF GetProgressRectangle()
        {
            RectangleF trackRect = GetTrackRectangle();
            
            // 使用动画值或实际值，或临时拖拽值
            double progressValue;
            
            if (_isDraggingThumb)
            {
                // 如果正在拖拽滑块，使用临时拖拽值
                progressValue = (double)_dragCurrentTime / _duration.TotalMilliseconds;
            }
            else
            {
                // 否则使用正常动画或当前值
                progressValue = _enableAnimation ? _animatedProgress : (double)_currentTime.TotalMilliseconds / _duration.TotalMilliseconds;
            }
            
            // 限制在有效范围内
            progressValue = Math.Max(0, Math.Min(1, progressValue));
            
            float progressWidth = (float)(progressValue * trackRect.Width);
            
            return new RectangleF(
                trackRect.X,
                trackRect.Y,
                progressWidth,
                trackRect.Height);
        }
        
        /// <summary>
        /// 获取滑块矩形区域
        /// </summary>
        private RectangleF GetThumbRectangle()
        {
            RectangleF trackRect = GetTrackRectangle();
            
            // 使用动画值或实际值
            double progressValue;
            
            if (_isDraggingThumb)
            {
                // 如果正在拖拽滑块，使用临时拖拽值
                progressValue = (double)_dragCurrentTime / _duration.TotalMilliseconds;
            }
            else
            {
                // 否则使用正常动画或当前值
                progressValue = _enableAnimation ? _animatedProgress : (double)_currentTime.TotalMilliseconds / _duration.TotalMilliseconds;
            }
            
            // 限制在有效范围内
            progressValue = Math.Max(0, Math.Min(1, progressValue));
            
            float thumbPosition = trackRect.X + (float)(progressValue * trackRect.Width);
            
            // 如果有动画效果，应用缩放
            float actualThumbSize = _thumbSize;
            float offset = 0;
            
            if (_thumbAnimationScale > 1.0)
            {
                // 计算缩放后的尺寸和偏移
                float scaledSize = _thumbSize * (float)_thumbAnimationScale;
                offset = (scaledSize - _thumbSize) / 2;
                actualThumbSize = scaledSize;
            }
            
            return new RectangleF(
                thumbPosition - (actualThumbSize / 2),
                trackRect.Y + (trackRect.Height - actualThumbSize) / 2,
                actualThumbSize,
                actualThumbSize);
        }
        
        /// <summary>
        /// 根据横坐标计算时间
        /// </summary>
        private long CalculateTimeFromPosition(int x)
        {
            RectangleF trackRect = GetTrackRectangle();
            
            // 将鼠标位置限制在进度条范围内
            float trackX = Math.Max(trackRect.X, Math.Min(x, trackRect.X + trackRect.Width));
            
            // 计算比例（相对于进度条，而不是控件宽度）
            float proportion = (trackX - trackRect.X) / trackRect.Width;
            
            // 计算时间
            return (long)(proportion * _duration.TotalMilliseconds);
        }
        
        /// <summary>
        /// 根据时间计算横坐标位置
        /// </summary>
        private float CalculatePositionFromTime(long time)
        {
            if (_duration.TotalMilliseconds <= 0)
                return 0;
                
            RectangleF trackRect = GetTrackRectangle();
            float proportion = (float)((double)time / _duration.TotalMilliseconds);
            return trackRect.X + (proportion * trackRect.Width);
        }
        
        /// <summary>
        /// 启动滑块动画
        /// </summary>
        private void StartThumbAnimation()
        {
            // 确保计时器正在运行
            if (!_thumbAnimationTimer.Enabled)
            {
                _thumbAnimationTimer.Start();
            }
        }
        
        #endregion
        
        #region 绘制方法
        
        /// <summary>
        /// 绘制控件
        /// </summary>
        protected override void OnPaint(PaintEventArgs e)
        {
            if (e == null || e.Graphics == null)
            {
                base.OnPaint(e);
                return;
            }

            try
            {
                // 开启抗锯齿
                e.Graphics.SmoothingMode = SmoothingMode.HighQuality;
                e.Graphics.TextRenderingHint = System.Drawing.Text.TextRenderingHint.ClearTypeGridFit;
                
                // 根据不同的风格绘制轨道
                switch (_trackStyle)
                {
                    case TrackBarStyle.Flat:
                        DrawFlatStyle(e.Graphics);
                        break;
                    case TrackBarStyle.Round:
                        DrawRoundStyle(e.Graphics);
                        break;
                    default:
                        DrawRoundStyle(e.Graphics);
                        break;
                }
                
                // 绘制章节标记
                if (_showMarkers && _chapterMarkers.Count > 0)
                {
                    DrawChapterMarkers(e.Graphics);
                }
                
                // 绘制时间文本
                DrawTimeText(e.Graphics);
                
                // 绘制鼠标悬停时间提示，允许在拖动时也显示
                if (_showTimeTooltip && (_isHovered || _isDragging) && _mousePosition != Point.Empty)
                {
                    DrawHoverTimeIndicator(e.Graphics, _mousePosition.X);
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"MusicPlayerTrackBar绘制错误: {ex.Message}");
                
                // 出错时使用基本绘制保证控件显示
                try
                {
                    // 关闭抗锯齿
                    e.Graphics.SmoothingMode = SmoothingMode.Default;
                    
                    // 绘制简单的轨道
                    using (Brush trackBrush = new SolidBrush(_trackColor))
                    {
                        RectangleF trackRect = GetTrackRectangle();
                        e.Graphics.FillRectangle(trackBrush, trackRect);
                    }
                    
                    // 绘制简单的进度
                    using (Brush progressBrush = new SolidBrush(_progressColor))
                    {
                        RectangleF progressRect = GetProgressRectangle();
                        e.Graphics.FillRectangle(progressBrush, progressRect);
                    }
                    
                    // 绘制简单的滑块
                    using (Brush thumbBrush = new SolidBrush(_thumbColor))
                    {
                        RectangleF thumbRect = GetThumbRectangle();
                        e.Graphics.FillRectangle(thumbBrush, thumbRect);
                    }
                }
                catch
                {
                    // 如果简单绘制也失败，调用基类方法
                    base.OnPaint(e);
                }
            }
        }
        
        /// <summary>
        /// 绘制平面风格轨道
        /// </summary>
        private void DrawFlatStyle(Graphics g)
        {
            // 获取区域
            RectangleF trackRect = GetTrackRectangle();
            RectangleF bufferedRect = GetBufferedRectangle();
            RectangleF progressRect = GetProgressRectangle();
            _thumbRect = GetThumbRectangle();
            
            // 绘制轨道背景
            using (Brush trackBrush = new SolidBrush(_trackColor))
            {
                g.FillRectangle(trackBrush, trackRect);
            }
            
            // 绘制缓冲区
            if (_showBufferedProgress && _bufferedTime.TotalMilliseconds > 0)
            {
                using (Brush bufferedBrush = new SolidBrush(_bufferedColor))
                {
                    g.FillRectangle(bufferedBrush, bufferedRect);
                }
            }
            
            // 绘制进度
            using (Brush progressBrush = new SolidBrush(_progressColor))
            {
                g.FillRectangle(progressBrush, progressRect);
            }
            
            // 绘制滑块
            DrawThumb(g, _thumbRect);
        }
        
        /// <summary>
        /// 绘制圆角风格轨道
        /// </summary>
        private void DrawRoundStyle(Graphics g)
        {
            // 获取区域
            RectangleF trackRect = GetTrackRectangle();
            RectangleF bufferedRect = GetBufferedRectangle();
            RectangleF progressRect = GetProgressRectangle();
            _thumbRect = GetThumbRectangle();
            
            float radius = trackRect.Height / 2;
            
            // 绘制轨道背景
            using (Brush trackBrush = new SolidBrush(_trackColor))
            {
                g.FillRoundedRectangle(trackBrush, trackRect, radius);
            }
            
            // 绘制缓冲区
            if (_showBufferedProgress && _bufferedTime.TotalMilliseconds > 0)
            {
                using (Brush bufferedBrush = new SolidBrush(_bufferedColor))
                {
                    g.FillRoundedRectangle(bufferedBrush, bufferedRect, radius);
                }
            }
            
            // 绘制进度
            using (Brush progressBrush = new SolidBrush(_progressColor))
            {
                g.FillRoundedRectangle(progressBrush, progressRect, radius);
            }
            
            // 绘制滑块
            DrawThumb(g, _thumbRect);
        }
        
        /// <summary>
        /// 绘制章节标记
        /// </summary>
        private void DrawChapterMarkers(Graphics g)
        {
            foreach (ChapterMarker marker in _chapterMarkers)
            {
                float x = CalculatePositionFromTime(marker.Time);
                float y = Height / 2;
                
                // 绘制标记点
                using (Brush markerBrush = new SolidBrush(marker.Color))
                {
                    // 标记三角形
                    PointF[] trianglePoints = new PointF[]
                    {
                        new PointF(x, y - 5),
                        new PointF(x - 4, y - 11),
                        new PointF(x + 4, y - 11)
                    };
                    
                    g.FillPolygon(markerBrush, trianglePoints);
                }
                
                // 如果鼠标在标记点附近，显示标题
                if (_isHovered && Math.Abs(_mousePosition.X - x) < 10)
                {
                    string title = marker.Title;
                    SizeF textSize = g.MeasureString(title, _timeFont);
                    
                    // 绘制标题背景
                    RectangleF titleRect = new RectangleF(
                        x - textSize.Width / 2,
                        y - 30,
                        textSize.Width + 6,
                        textSize.Height + 4);
                        
                    using (Brush bgBrush = new SolidBrush(Color.FromArgb(200, Color.Black)))
                    {
                        g.FillRoundedRectangle(bgBrush, titleRect, 4);
                    }
                    
                    // 绘制标题文本
                    using (Brush textBrush = new SolidBrush(Color.White))
                    {
                        g.DrawString(title, _timeFont, textBrush, 
                            x - textSize.Width / 2 + 3, 
                            y - 29);
                    }
                }
            }
        }
        
        /// <summary>
        /// 绘制时间文本
        /// </summary>
        private void DrawTimeText(Graphics g)
        {
            if (_timeDisplay == TimeDisplayType.None)
                return;
                
            if (_timeDisplay == TimeDisplayType.OnHover && !_isHovered && !_isDragging)
                return;
                
            // 如果时间文本位置设置为None，则不显示
            if (_timeTextPosition == TimeTextPosition.None)
                return;
            
            // 使用当前时间或拖拽时间
            string currentTimeText;
            if (_isDraggingThumb)
            {
                // 在拖动过程中使用临时拖拽时间
                currentTimeText = FormatTime(_dragCurrentTime);
            }
            else
            {
                currentTimeText = FormatTime((long)_currentTime.TotalMilliseconds);
            }
            
            string totalTimeText = FormatTime((long)_duration.TotalMilliseconds);
            
            // 获取文本大小
            SizeF currentTimeSize = g.MeasureString(currentTimeText, _timeFont);
            SizeF totalTimeSize = g.MeasureString(totalTimeText, _timeFont);
            
            // 获取轨道位置，用于定位文本
            RectangleF trackRect = GetTrackRectangle();
            
            // 文本垂直位置 - 与轨道中心对齐
            float textY = trackRect.Y + (trackRect.Height - currentTimeSize.Height) / 2;
            
            // 定义文本位置变量
            float textX = 0;
            
            // 根据不同的文本位置进行绘制
            switch (_timeTextPosition)
            {
                case TimeTextPosition.EndOfTrack:
                    // 进度条尾部模式：所有时间文本都显示在进度条右侧
                    string combinedText = $"{currentTimeText} / {totalTimeText}";
                    SizeF combinedSize = g.MeasureString(combinedText, _timeFont);
                    
                    textX = trackRect.Right + 5; // 在进度条右侧留出一些间距
                    
                    // 绘制组合文本在进度条右侧
                    using (Brush textBrush = new SolidBrush(_trackColor))
                    {
                        g.DrawString(combinedText, _timeFont, textBrush, textX, textY);
                    }
                    break;
                    
                case TimeTextPosition.Classic:
                    // 经典模式：当前时间在左侧，总时间在右侧
                    using (Brush textBrush = new SolidBrush(_trackColor))
                    {
                        // 绘制当前时间在左侧
                        g.DrawString(currentTimeText, _timeFont, textBrush, 0, textY);
                        
                        // 绘制总时间在右侧
                        textX = trackRect.Right + 5; // 进度条右侧留出间距
                        g.DrawString(totalTimeText, _timeFont, textBrush, textX, textY);
                    }
                    break;
            }
        }
        
        /// <summary>
        /// 绘制鼠标悬停时间指示器
        /// </summary>
        private void DrawHoverTimeIndicator(Graphics g, int mouseX)
        {
            try
            {
                // 计算滑块位置
                _thumbRect = GetThumbRectangle();
                
                // 计算鼠标与滑块中心的距离
                float thumbCenterX = _thumbRect.X + _thumbRect.Width / 2;
                float distance = Math.Abs(mouseX - thumbCenterX);
                
                // 如果鼠标太靠近滑块，不显示时间指示器
                if (distance < _thumbRect.Width * 1.2)
                {
                    return;
                }
                
                // 计算鼠标位置对应的时间
                float ratio = (float)mouseX / (float)ClientRectangle.Width;
                TimeSpan hoverTime = TimeSpan.FromSeconds(_duration.TotalSeconds * ratio);
                
                // 准备时间文本
                string timeText = FormatTime((long)hoverTime.TotalMilliseconds);
                SizeF textSize = g.MeasureString(timeText, _timeFont);
                
                // 计算气泡大小和位置
                int bubbleWidth = (int)textSize.Width + 10;
                int bubbleHeight = (int)textSize.Height + 6;
                int bubbleX = mouseX - bubbleWidth / 2;
                int bubbleY = 5; // 改为正值，确保显示在控件内部的顶部
                
                // 确保气泡不超出控件边界
                bubbleX = Math.Max(0, Math.Min(bubbleX, Width - bubbleWidth));
                
                // 绘制气泡背景
                Rectangle bubbleRect = new Rectangle(bubbleX, bubbleY, bubbleWidth, bubbleHeight);
                using (Brush bubbleBrush = new SolidBrush(Color.FromArgb(220, Color.Black))) // 增加不透明度
                {
                    g.FillRoundedRectangle(bubbleBrush, bubbleRect, 4);
                }
                
                // 绘制指向线
                int lineX = mouseX;
                using (Pen linePen = new Pen(Color.FromArgb(180, Color.Black), 1))
                {
                    g.DrawLine(linePen, lineX, bubbleY + bubbleHeight, lineX, (Height - _trackHeight) / 2);
                }
                
                // 绘制时间文本
                using (Brush textBrush = new SolidBrush(Color.White))
                {
                    g.DrawString(timeText, _timeFont, textBrush, bubbleX + 5, bubbleY + 3);
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"DrawHoverTimeIndicator绘制错误: {ex.Message}");
            }
        }
        
        /// <summary>
        /// 绘制滑块
        /// </summary>
        private void DrawThumb(Graphics g, RectangleF thumbRect)
        {
            switch (_thumbStyle)
            {
                case ThumbStyle.Rectangle:
                    // 方形滑块
                    using (Brush thumbBrush = new SolidBrush(_thumbColor))
                    {
                        g.FillRectangle(thumbBrush, thumbRect);
                    }
                    
                    // 绘制边框
                    using (Pen borderPen = new Pen(_trackColor, 1))
                    {
                        g.DrawRectangle(borderPen, thumbRect.X, thumbRect.Y, 
                            thumbRect.Width, thumbRect.Height);
                    }
                    break;
                    
                case ThumbStyle.Circle:
                    // 圆形滑块
                    using (Brush thumbBrush = new SolidBrush(_thumbColor))
                    {
                        g.FillEllipse(thumbBrush, thumbRect);
                    }
                    
                    // 绘制边框
                    using (Pen borderPen = new Pen(_trackColor, 1))
                    {
                        g.DrawEllipse(borderPen, thumbRect);
                    }
                    
                    // 添加高光效果
                    RectangleF highlightRect = thumbRect;
                    highlightRect.Inflate(-thumbRect.Width * 0.3f, -thumbRect.Height * 0.3f);
                    highlightRect.Y -= thumbRect.Height * 0.1f;
                    
                    using (Brush highlightBrush = new SolidBrush(Color.FromArgb(90, Color.White)))
                    {
                        g.FillEllipse(highlightBrush, highlightRect);
                    }
                    break;
                    
                case ThumbStyle.Triangle:
                    // 三角形滑块
                    using (Brush thumbBrush = new SolidBrush(_thumbColor))
                    {
                        Point[] points = new Point[]
                        {
                            new Point((int)thumbRect.X, (int)(thumbRect.Y + thumbRect.Height / 2)),
                            new Point((int)(thumbRect.X + thumbRect.Width / 2), (int)thumbRect.Y),
                            new Point((int)(thumbRect.X + thumbRect.Width), (int)(thumbRect.Y + thumbRect.Height / 2))
                        };
                        
                        g.FillPolygon(thumbBrush, points);
                    }
                    
                    // 绘制边框
                    using (Pen borderPen = new Pen(_trackColor, 1))
                    {
                        Point[] points = new Point[]
                        {
                            new Point((int)thumbRect.X, (int)(thumbRect.Y + thumbRect.Height / 2)),
                            new Point((int)(thumbRect.X + thumbRect.Width / 2), (int)thumbRect.Y),
                            new Point((int)(thumbRect.X + thumbRect.Width), (int)(thumbRect.Y + thumbRect.Height / 2))
                        };
                        g.DrawPolygon(borderPen, points);
                    }
                    break;
            }
        }
        
        #endregion
    }
}

#region GraphicsPath扩展方法

/// <summary>
/// 提供GraphicsPath的扩展方法
/// </summary>
internal static class GraphicsPathExtensions
{
    /// <summary>
    /// 向路径添加圆角矩形
    /// </summary>
    public static void AddRoundedRectangle(this GraphicsPath path, RectangleF bounds, float radius)
    {
        if (radius <= 0)
        {
            path.AddRectangle(bounds);
            return;
        }
        
        // 确保半径不会超出矩形的一半
        radius = Math.Min(radius, Math.Min(bounds.Width / 2, bounds.Height / 2));
        
        // 创建圆角矩形路径
        float diameter = radius * 2;
        SizeF size = new SizeF(diameter, diameter);
        RectangleF arc = new RectangleF(bounds.Location, size);
        
        // 左上角弧
        path.AddArc(arc, 180, 90);
        
        // 顶边
        arc.X = bounds.Right - diameter;
        path.AddArc(arc, 270, 90);
        
        // 右边
        arc.Y = bounds.Bottom - diameter;
        path.AddArc(arc, 0, 90);
        
        // 底边
        arc.X = bounds.Left;
        path.AddArc(arc, 90, 90);
        
        path.CloseFigure();
    }
}

#endregion