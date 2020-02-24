//2017.07.25, czs, create in hongqing, 字符串界面 
//2019.01.12, czs, create in hmx, XY 坐标界面 

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
    public partial class NamedXyControl : NamedStringControl
    {
       /// <summary>
       /// 构造函数
       /// </summary>
        public NamedXyControl()
        {
            InitializeComponent();
            this.Title = "XY：";
        }

        /// <summary>
        /// 获取值
        /// </summary>
        /// <returns></returns>
        public XY GetXy()
        {
            return XY.Parse(this.GetValue());
        } 

        /// <summary>
        /// 设置值
        /// </summary>
        /// <param name="xy"></param>
        public void SetXy(XY xy)
        {
            this.SetValue( xy + ""); 
        }


    }
}
