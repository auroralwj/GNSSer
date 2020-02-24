//2017.07.25, czs, create in hongqing, 字符串界面 
//2019.01.19, czs, create in hmx, XYZ 坐标界面 

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Geo.Coordinates;

namespace Geo.Winform.Controls
{
    /// <summary>
    /// XY 坐标界面 
    /// </summary>
    public partial class NamedXyzControl : NamedStringControl
    {
       /// <summary>
       /// 构造函数
       /// </summary>
        public NamedXyzControl()
        {
            InitializeComponent();
            this.Title = "XYZ：";
        }

        /// <summary>
        /// 获取值
        /// </summary>
        /// <returns></returns>
        public XYZ GetXyz()
        {
            return XYZ.Parse(this.GetValue());
        } 

        /// <summary>
        /// 设置值
        /// </summary>
        /// <param name="xy"></param>
        public void SetXyz(XYZ xy)
        {
            this.SetValue( xy.ToString("G12", "\t")); 
        }


    }
}
