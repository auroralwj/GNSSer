//2019.01.10, czs, create in hmx, 椭球参数输入

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Geo.Referencing;

namespace Geo.Winform
{
    /// <summary>
    /// 构造函数
    /// </summary>
    public partial class EllipsoidSelectControl : UserControl
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public EllipsoidSelectControl()
        {
            InitializeComponent();

            enumRadioControl_ellipsoidType.Init<EllipsoidType>();
        }
        /// <summary>
        /// 椭球
        /// </summary>
        public Ellipsoid Ellipsoid
        {
            get
            {
                if (IsInputParam)
                {
                    double semiMajor = Double.Parse(this.textBox_semiMajor.Text);
                    double flatten = Double.Parse(this.textBox_flattonOrAnt.Text); 
                    return  new Referencing.Ellipsoid(semiMajor, flatten);
                }
                var type = this.enumRadioControl_ellipsoidType.GetCurrent<EllipsoidType>();
                return Ellipsoid.GetEllipsoid(type);
            }
        }

        /// <summary>
        /// 设置椭球
        /// </summary>
        /// <param name="ellipsoidType"></param>
        public void SetEllipsoidType(EllipsoidType ellipsoidType)
        {
            this.enumRadioControl_ellipsoidType.SetCurrent<EllipsoidType>(ellipsoidType);
        }

        bool IsInputParam => checkBox_inputParam.Checked;

        private void checkBox_inputParam_CheckedChanged(object sender, EventArgs e)
        {
            this.panel_ellipsiodParam.Visible = IsInputParam;
            enumRadioControl_ellipsoidType.Visible = !IsInputParam;
        }

        private void EllipsoidSelectControl_Load(object sender, EventArgs e)
        {
            enumRadioControl_ellipsoidType.Init<Geo.Referencing.EllipsoidType>();
            this.panel_ellipsiodParam.Visible = IsInputParam;
        }
    }
}
