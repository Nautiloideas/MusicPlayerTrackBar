# 音乐播放器轨道条控件 (MusicPlayerTrackBar)

## 简介

MusicPlayerTrackBar 是一个专为音乐播放器设计的现代化轨道条控件，提供丰富的外观选项和功能，可以轻松集成到各种音乐播放器应用中。控件支持拖拽功能、动画效果、章节标记和各种视觉样式定制。

## 主要特性

-   **多种轨道样式**：平面风格、圆角风格
-   **多种滑块样式**：矩形、圆形、三角形
-   **时间显示**：多种显示格式和位置选项
-   **缓冲进度显示**：直观展示音频缓冲状态
-   **章节标记**：支持添加和显示音频章节标记
-   **拖拽功能**：支持流畅的拖拽体验，包括滑块放大动画
-   **悬停预览**：显示鼠标悬停位置的时间提示
-   **丰富的事件**：提供拖拽开始/结束、时间变化等事件

## 配置选项

### 基本属性

| 属性           | 类型 | 默认值 | 说明                 |
| -------------- | ---- | ------ | -------------------- |
| `Duration`     | long | 100000 | 音频总时长（毫秒）   |
| `CurrentTime`  | long | 0      | 当前播放时间（毫秒） |
| `BufferedTime` | long | 0      | 缓冲时间（毫秒）     |

### 外观设置

| 属性            | 类型          | 默认值     | 说明                                    |
| --------------- | ------------- | ---------- | --------------------------------------- |
| `TrackBarStyle` | TrackBarStyle | Flat       | 轨道条样式（Flat、Round）               |
| `ThumbStyle`    | ThumbStyle    | Circle     | 滑块样式（Rectangle、Circle、Triangle） |
| `TrackColor`    | Color         | DarkGray   | 轨道颜色                                |
| `ProgressColor` | Color         | DodgerBlue | 进度颜色                                |
| `BufferedColor` | Color         | Gray       | 缓冲区颜色                              |
| `ThumbColor`    | Color         | White      | 滑块颜色                                |
| `TrackHeight`   | int           | 5          | 轨道高度                                |
| `ThumbSize`     | int           | 12         | 滑块大小                                |
| `ThumbImage`    | Image         | null       | 自定义滑块图像                          |
| `TimeFont`      | Font          | Arial, 8pt | 时间显示字体                            |

### 行为设置

| 属性               | 类型             | 默认值   | 说明                                             |
| ------------------ | ---------------- | -------- | ------------------------------------------------ |
| `TimeDisplay`      | TimeDisplayType  | OnHover  | 时间显示方式（None、OnHover、Always）            |
| `TimeFormat`       | TimeFormat       | Standard | 时间格式（Standard、Complete、WithMilliseconds） |
| `TimeTextPosition` | TimeTextPosition | Classic  | 时间文本位置（None、EndOfTrack、Classic）        |
| `ShowBuffered`     | bool             | true     | 是否显示缓冲进度                                 |
| `ShowHoverTime`    | bool             | true     | 是否显示悬停时间                                 |
| `ShowMarkers`      | bool             | false    | 是否显示章节标记                                 |
| `EnablePreview`    | bool             | false    | 是否启用预览功能                                 |
| `EnableAnimation`  | bool             | true     | 是否启用动画效果                                 |
| `MarkerStyle`      | MarkerStyle      | Line     | 标记样式（Line、Triangle、Circle）               |

### 兼容设置

| 属性                | 类型 | 默认值 | 说明                                   |
| ------------------- | ---- | ------ | -------------------------------------- |
| `CompatibilityMode` | bool | true   | 是否启用兼容模式                       |
| `Value`             | int  | 0      | 控件的当前值（0-100 的百分比）         |
| `Maximum`           | int  | 100    | 控件的最大值（固定为 100，仅用于兼容） |
| `Minimum`           | int  | 0      | 控件的最小值（固定为 0，仅用于兼容）   |

## 事件

| 事件                     | 说明               |
| ------------------------ | ------------------ |
| `TimeChanged`            | 时间点改变时发生   |
| `DragStarted`            | 拖拽开始时发生     |
| `DragCompleted`          | 拖拽结束时发生     |
| `PreviewPositionChanged` | 预览位置改变时发生 |

## 方法

| 方法                                                     | 说明                   |
| -------------------------------------------------------- | ---------------------- |
| `AddChapterMarker(long time, string title, Color color)` | 添加章节标记           |
| `ClearChapterMarkers()`                                  | 清除所有章节标记       |
| `FormatTime(long milliseconds)`                          | 将时间值格式化为字符串 |

## 使用示例

### 基本用法

```csharp
// 添加命名空间引用
using MusicPlayerTrackBar;

// 创建控件实例
MusicPlayerTrackBar trackBar = new MusicPlayerTrackBar
{
    // 设置基本属性
    Width = 400,
    Height = 30,
    Duration = 180000,  // 3分钟音频

    // 设置外观
    TrackBarStyle = TrackBarStyle.Round,
    ThumbStyle = ThumbStyle.Circle,
    ProgressColor = Color.FromArgb(0, 154, 251),
    TrackHeight = 6,
    ThumbSize = 14,

    // 设置行为
    TimeDisplay = TimeDisplayType.Always,
    TimeFormat = TimeFormat.Standard,
    TimeTextPosition = TimeTextPosition.Classic,
    EnableAnimation = true
};

// 添加到窗体
this.Controls.Add(trackBar);

// 注册事件
trackBar.TimeChanged += (sender, e) => {
    // 处理时间变化
    Console.WriteLine($"当前时间: {e.CurrentTime}ms");

    // 可以在这里更新其他UI元素，如当前时间显示
    timeLabel.Text = FormatTimeSpan(TimeSpan.FromMilliseconds(e.CurrentTime));
};

// 注册拖拽事件
trackBar.DragStarted += (sender, e) => {
    // 拖拽开始 - 可以暂停播放
    Console.WriteLine("开始拖拽");
};

trackBar.DragCompleted += (sender, e) => {
    // 拖拽结束 - 可以跳转到新位置并继续播放
    Console.WriteLine($"拖拽结束于: {e.Time}ms");

    // 通常这里会使用播放器的Seek方法跳转到指定位置
    myMediaPlayer.Position = TimeSpan.FromMilliseconds(e.Time);
    myMediaPlayer.Play();
};
```

### 添加缓冲进度和章节标记

```csharp
// 设置缓冲进度
trackBar.BufferedTime = 90000; // 已缓冲1分30秒
trackBar.ShowBuffered = true;

// 添加章节标记
trackBar.ShowMarkers = true;
trackBar.AddChapterMarker(30000, "第一章", Color.Red);
trackBar.AddChapterMarker(60000, "第二章", Color.Green);
trackBar.AddChapterMarker(120000, "第三章", Color.Blue);
```

### 播放器集成示例

```csharp
// 假设有一个媒体播放器实例
private MediaPlayer mediaPlayer;
private Timer updateTimer;

private void InitializePlayer()
{
    // 初始化播放器和UI
    mediaPlayer = new MediaPlayer();

    // 设置轨道条
    musicTrackBar.EnableAnimation = true;
    musicTrackBar.TimeDisplay = TimeDisplayType.Always;

    // 初始化更新计时器
    updateTimer = new Timer();
    updateTimer.Interval = 200; // 200ms更新一次
    updateTimer.Tick += (s, e) => {
        // 更新当前播放时间
        if (!musicTrackBar.IsDragging) {
            musicTrackBar.CurrentTime = (long)mediaPlayer.Position.TotalMilliseconds;
        }

        // 更新缓冲进度
        musicTrackBar.BufferedTime = (long)mediaPlayer.BufferedPosition.TotalMilliseconds;
    };

    // 注册事件
    musicTrackBar.DragCompleted += (s, e) => {
        mediaPlayer.Position = TimeSpan.FromMilliseconds(e.Time);
    };

    // 加载媒体文件时
    mediaPlayer.MediaOpened += (s, e) => {
        musicTrackBar.Duration = (long)mediaPlayer.NaturalDuration.TimeSpan.TotalMilliseconds;
        updateTimer.Start();
    };

    mediaPlayer.MediaEnded += (s, e) => {
        updateTimer.Stop();
        musicTrackBar.CurrentTime = 0;
    };
}
```

## 需求

-   .NET Framework 4.7.2 或更高版本
-   System.Drawing 命名空间
-   System.Windows.Forms 命名空间

## 许可

本控件基于 MIT 许可开源，欢迎自由使用和修改。
