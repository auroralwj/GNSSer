//2019.01.19, czs, create in hmx, 7 参数 转换界面

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
    ///  7 参数转换
    /// </summary>
    public partial class XyzFrameTrans7ParamControl : UserControl
    {
        /// <summary>
        /// 7 参数转换
        /// </summary>
        public XyzFrameTrans7ParamControl()
        {
            InitializeComponent();
            namedXyzControl_offset.SetXyz(XYZ.Zero);
            namedXyzControl_rotateAngleSec.SetXyz(XYZ.Zero);
        }
        /// <summary>
        /// 参数
        /// </summary>
        /// <returns></returns>
        public XyzFrameConvertParam GetValue()
        {
            var rotateSec = namedXyzControl_rotateAngleSec.GetXyz();
            var xy = this.namedXyzControl_offset.GetXyz();
            var scaleppm = namedFloatControl_scaleFactor.GetValue();
            return new XyzFrameConvertParam(xy, rotateSec, scaleppm);
        }
        /// <summary>
        /// 参数
        /// </summary>
        /// <returns></returns>
        public void SetValue(XyzFrameConvertParam convertParam)
        {
            namedXyzControl_rotateAngleSec.SetXyz(convertParam.RotationAngleSecond);
            this.namedXyzControl_offset.SetXyz(convertParam.Offset);
            namedFloatControl_scaleFactor.SetValue(convertParam.ScaleFactorPpm);
        }
    }
}
