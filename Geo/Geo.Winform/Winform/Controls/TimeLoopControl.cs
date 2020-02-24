//2017.07.21, czs, create in hongqing, 创建时段界面
//2017.10.09, czs, edit in hongqing, 增加事件步长控件获取

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Geo.Winform.Controls
{
    /// <summary>
    /// 事件遍历控件
    /// </summary>
    public partial class TimeLoopControl : UserControl
    {
        /// <summary>
        /// 初始构造函数
        /// </summary>
        public TimeLoopControl()
        {
            InitializeComponent();
        }
        /// <summary>
        /// 事件控件
        /// </summary>
        public TimePeriodControl TimePeriodControl { get { return this.timePeriodControl1; } }
        /// <summary>
        /// 事件步长控件
        /// </summary>
        public NamedFloatControl TimeStepControl { get { return namedFloatControl_time; } }
        /// <summary>
        /// 设置间隔单位
        /// </summary>
        /// <param name="unit"></param>
        public void SetStepUnit(string unit = "分")
        {
            this.TimeStepControl.Title = "间隔(" + unit + ")"; ;
        }

        /// <summary>
        /// 获取时间遍历器
        /// </summary>
        /// <returns></returns>
        public TimeLooper GetTimeLooper()
        {
            double timeStep = namedFloatControl_time.GetValue() * 60.0; //seconds
            var timePeriod = this.timePeriodControl1.TimePeriod;
            return new TimeLooper(timePeriod, timeStep);
        }
    }
}
