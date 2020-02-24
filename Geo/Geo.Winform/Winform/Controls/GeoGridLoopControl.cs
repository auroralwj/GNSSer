//2017.07.21, czs, create in hongqing, 创建时段界面
//2017.10.09, czs, edit in hongqing, 增加控件获取属性


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
    /// 地理格网控件
    /// </summary>
    public partial class GeoGridLoopControl : UserControl
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public GeoGridLoopControl()
        {
            InitializeComponent();
        }
        /// <summary>
        /// 精度跨度
        /// </summary>
        public FloatSpanControl LonSpanControl { get { return floatSpanControl_lon; } }
        /// <summary>
        /// 纬度跨度
        /// </summary>
        public FloatSpanControl LatSpanControl { get { return floatSpanControl_lat; } }
        /// <summary>
        /// 精度步长控件，单位分
        /// </summary>
        public NamedFloatControl LonStepControl { get { return namedFloatControl_lonStep; } }
        /// <summary>
        /// 纬度步长控件，单位分
        /// </summary>
        public NamedFloatControl LatStepControl { get { return namedFloatControl_latStep; } }
        /// <summary>
        /// 设置步长，单位分
        /// </summary>
        /// <param name="lonStepMin"></param>
        /// <param name="latStepMin"></param>
        public void SetStep(double lonStepMin, double latStepMin)
        {
            LonStepControl.SetValue(lonStepMin);
            LatStepControl.SetValue(latStepMin);
        }
        /// <summary>
        /// 获取格网遍历器
        /// </summary>
        /// <returns></returns>
        public GeoGridLooper GetGridLooper()
        {
            NumerialSegment LonSpan = LonSpanControl.GetValue();
            NumerialSegment LatSpan = LatSpanControl.GetValue();
            double LonStep = this.LonStepControl.Value / 60.0;
            double LatStep = this.LatStepControl.Value / 60.0;

            bool isLonDesc = this.checkBox_lonOrder.Checked;
            bool isLatDesc = this.checkBox_latOrder.Checked;

            bool isLatFirst = this.checkBox_latFirst.Checked;
            return new GeoGridLooper(LonSpan, LonStep, LatSpan, LatStep, isLonDesc, isLatDesc, isLatFirst);
        }
    }
}
