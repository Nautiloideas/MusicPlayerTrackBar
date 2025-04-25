# 音乐播放器轨道条控件 (MusicPlayerTrackBar)

## 简介

MusicPlayerTrackBar 是一个专为音乐播放器设计的现代化轨道条控件，提供丰富的外观选项和功能，可以轻松集成到各种音乐播放器应用中。

## 主要特性

-   **多种轨道样式**：平滑、圆角、玻璃、新拟物风格和波形图风格
-   **多种滑块样式**：圆形、方形、胶囊形和自定义图像
-   **波形显示**：支持显示音频波形，包括标准、镜像和条形波形
-   **缓冲进度显示**：直观展示音频缓冲状态
-   **时间显示**：可选择不同的时间显示格式和位置
-   **章节标记**：支持添加和显示音频章节标记
-   **动画效果**：平滑的滑块移动动画
-   **丰富的事件**：提供拖拽开始/结束、时间变化等事件

## 外观自定义

-   轨道颜色、高度
-   已播放进度颜色
-   缓冲区颜色
-   滑块颜色、大小和样式
-   时间显示格式和字体
-   波形颜色和类型

## 使用示例

```csharp
// 创建控件实例
MusicPlayerTrackBar trackBar = new MusicPlayerTrackBar
{
    TrackBarStyle = TrackBarStyle.Glass,
    ThumbStyle = ThumbStyle.Round,
    ProgressColor = Color.FromArgb(0, 154, 251),
    Duration = 180000,  // 3分钟音频
    TimeDisplay = TimeDisplayType.Always
};

// 注册事件
trackBar.TimeChanged += (sender, e) => {
    // 处理时间变化
    Console.WriteLine($"当前时间: {e.CurrentTime}ms");
};

// 设置波形数据
float[] waveformData = new float[100];
// 填充波形数据...
trackBar.SetWaveformData(waveformData);
trackBar.ShowWaveform = true;

// 添加章节标记
trackBar.AddChapterMarker(60000, "Chapter 1", Color.Red);
trackBar.AddChapterMarker(120000, "Chapter 2", Color.Green);
```

## 需求

-   .NET Framework 4.7.2 或更高版本
-   System.Drawing 命名空间
-   System.Windows.Forms 命名空间

## 许可

本控件基于 MIT 许可开源，欢迎自由使用和修改。
