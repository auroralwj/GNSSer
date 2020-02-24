//2019.01.16, czs, create in hmx, 4参数平面转换界面

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Geo.Common;
using Geo.Coordinates;
using Geo.Referencing;
using Geo.Times;

namespace Geo.Winform.Controls
{
    /// <summary>
    /// 4参数平面转换
    /// </summary>
    public partial class PlainXyTransParamControl : UserControl
    {
        /// <summary>
        /// 4参数平面转换
        /// </summary>
        public PlainXyTransParamControl()
        {
            InitializeComponent();
        }
        /// <summary>
        /// 参数
        /// </summary>
        /// <returns></returns>
        public PlainXyConvertParam GetValue()
        {
            var rotateSec = namedFloatControl_rotateAngleSec.GetValue();
            var xy = this.namedXyControl1.GetXy();
            var scalePpm = namedFloatControl_scaleFactor.GetValue();
            return new PlainXyConvertParam(xy, scalePpm, rotateSec);
        }
        /// <summary>
        /// 参数
        /// </summary>
        /// <returns></returns>
        public void SetValue(PlainXyConvertParam convertParam)
        {
            namedFloatControl_rotateAngleSec.SetValue(convertParam.RotationAngleSecond);
            this.namedXyControl1.SetXy(convertParam.Offset);
            namedFloatControl_scaleFactor.SetValue(convertParam.ScaleFactorPpm);
        }
    }
}
