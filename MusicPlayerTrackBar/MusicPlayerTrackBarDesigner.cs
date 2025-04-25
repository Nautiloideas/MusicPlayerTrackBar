using System;
using System.Collections;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Drawing;
using System.Windows.Forms;
using System.Windows.Forms.Design;

namespace MusicPlayerTrackBar
{
    /// <summary>
    /// MusicPlayerTrackBar控件的设计器
    /// </summary>
    internal class MusicPlayerTrackBarDesigner : ControlDesigner
    {
        public override void Initialize(IComponent component)
        {
            base.Initialize(component);
            
            // 启用设计时选择和调整大小
            AutoResizeHandles = true;
        }

        protected override void PreFilterProperties(IDictionary properties)
        {
            base.PreFilterProperties(properties);
            
            // 隐藏不需要在属性窗口显示的属性
            properties.Remove("ForeColor");
            properties.Remove("ImeMode");
            properties.Remove("Padding");
            properties.Remove("RightToLeft");
        }
    }
} 