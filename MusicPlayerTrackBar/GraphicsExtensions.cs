using System;
using System.Drawing;
using System.Drawing.Drawing2D;

namespace MusicPlayerTrackBar
{
    /// <summary>
    /// 提供Graphics对象的扩展方法
    /// </summary>
    internal static class GraphicsExtensions
    {
        /// <summary>
        /// 绘制圆角矩形
        /// </summary>
        /// <param name="graphics">Graphics对象</param>
        /// <param name="pen">画笔</param>
        /// <param name="bounds">矩形范围</param>
        /// <param name="radius">圆角半径</param>
        public static void DrawRoundedRectangle(this Graphics graphics, Pen pen, RectangleF bounds, float radius)
        {
            if (graphics == null || pen == null)
                return;

            // A shortcut when there are no rounded corners.
            if (radius <= 0)
            {
                graphics.DrawRectangle(pen, bounds.X, bounds.Y, bounds.Width, bounds.Height);
                return;
            }

            try 
            {
                using (GraphicsPath path = CreateRoundedRectanglePath(bounds, radius))
                {
                    graphics.DrawPath(pen, path);
                }
            }
            catch
            {
                // 如果圆角矩形绘制失败，尝试使用普通矩形
                try
                {
                    graphics.DrawRectangle(pen, bounds.X, bounds.Y, bounds.Width, bounds.Height);
                }
                catch
                {
                    // 忽略所有最终异常
                }
            }
        }

        /// <summary>
        /// 填充圆角矩形
        /// </summary>
        /// <param name="graphics">Graphics对象</param>
        /// <param name="brush">填充画刷</param>
        /// <param name="bounds">矩形范围</param>
        /// <param name="radius">圆角半径</param>
        public static void FillRoundedRectangle(this Graphics graphics, Brush brush, RectangleF bounds, float radius)
        {
            if (graphics == null || brush == null)
                return;

            // 如果没有圆角，直接填充普通矩形
            if (radius <= 0)
            {
                graphics.FillRectangle(brush, bounds);
                return;
            }

            try
            {
                using (GraphicsPath path = CreateRoundedRectanglePath(bounds, radius))
                {
                    graphics.FillPath(brush, path);
                }
            }
            catch
            {
                // 如果圆角矩形填充失败，尝试使用普通矩形
                try
                {
                    graphics.FillRectangle(brush, bounds);
                }
                catch
                {
                    // 忽略所有最终异常
                }
            }
        }

        /// <summary>
        /// 绘制胶囊形状（两端为半圆的矩形）
        /// </summary>
        public static void DrawCapsule(this Graphics graphics, Pen pen, RectangleF bounds)
        {
            if (graphics == null || pen == null)
                return;

            float radius = Math.Min(bounds.Width, bounds.Height) / 2;
            DrawRoundedRectangle(graphics, pen, bounds, radius);
        }

        /// <summary>
        /// 填充胶囊形状（两端为半圆的矩形）
        /// </summary>
        public static void FillCapsule(this Graphics graphics, Brush brush, RectangleF bounds)
        {
            if (graphics == null || brush == null)
                return;

            float radius = Math.Min(bounds.Width, bounds.Height) / 2;
            FillRoundedRectangle(graphics, brush, bounds, radius);
        }

        /// <summary>
        /// 创建圆角矩形路径
        /// </summary>
        private static GraphicsPath CreateRoundedRectanglePath(RectangleF bounds, float radius)
        {
            // 确保半径有效
            radius = Math.Min(radius, Math.Min(bounds.Width / 2, bounds.Height / 2));
            
            // 如果半径为0，直接返回普通矩形路径
            if (radius <= 0)
            {
                GraphicsPath rectPath1 = new GraphicsPath();
                rectPath1.AddRectangle(bounds);
                return rectPath1;
            }

            // 正常处理圆角矩形
            float diameter = radius * 2;
            SizeF size = new SizeF(diameter, diameter);
            RectangleF arc = new RectangleF(bounds.Location, size);
            GraphicsPath roundPath = new GraphicsPath();

            try
            {
                // 左上角
                roundPath.AddArc(arc, 180, 90);

                // 右上角
                arc.X = bounds.Right - diameter;
                roundPath.AddArc(arc, 270, 90);

                // 右下角
                arc.Y = bounds.Bottom - diameter;
                roundPath.AddArc(arc, 0, 90);

                // 左下角
                arc.X = bounds.Left;
                roundPath.AddArc(arc, 90, 90);

                roundPath.CloseFigure();
                return roundPath;
            }
            catch
            {
                // 如果创建圆角路径失败，返回普通矩形路径
                GraphicsPath rectPath2 = new GraphicsPath();
                rectPath2.AddRectangle(bounds);
                return rectPath2;
            }
        }
        
        /// <summary>
        /// 绘制带有亮度变化的渐变效果
        /// </summary>
        public static void DrawGlossyRectangle(this Graphics graphics, RectangleF bounds, Color baseColor, float radius = 0)
        {
            if (graphics == null)
                return;
                
            try
            {
                // 创建主色渐变
                using (LinearGradientBrush mainBrush = new LinearGradientBrush(
                    bounds,
                    Color.FromArgb(230, baseColor),
                    Color.FromArgb(180, baseColor),
                    LinearGradientMode.Vertical))
                {
                    graphics.FillRoundedRectangle(mainBrush, bounds, radius);
                }
                
                // 添加顶部高光效果
                RectangleF highlightRect = bounds;
                highlightRect.Height = bounds.Height * 0.4f;
                using (LinearGradientBrush highlightBrush = new LinearGradientBrush(
                    highlightRect,
                    Color.FromArgb(120, Color.White),
                    Color.FromArgb(0, Color.White),
                    LinearGradientMode.Vertical))
                {
                    graphics.FillRoundedRectangle(highlightBrush, highlightRect, radius);
                }
            }
            catch
            {
                // 如果渐变效果失败，使用纯色填充
                try
                {
                    using (SolidBrush brush = new SolidBrush(baseColor))
                    {
                        graphics.FillRoundedRectangle(brush, bounds, radius);
                    }
                }
                catch
                {
                    // 忽略所有最终异常
                }
            }
        }
    }
} 