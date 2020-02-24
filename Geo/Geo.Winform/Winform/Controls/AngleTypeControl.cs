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
    public partial class AngleTypeControl : UserControl
    {
        public AngleTypeControl()
        {
            InitializeComponent(); 
        }

        private void checkBox_isRad_CheckedChanged(object sender, EventArgs e)
        {
            this.panel1.Visible = (AngleUnit == Coordinates.AngleUnit.Degree);
        }

        public double GetDegree(double angle)
        {
            if (this.AngleUnit == AngleUnit.Radian)
            {
                angle = Geo.Coordinates.AngularConvert.RadToDeg(angle);
            }
            else
            {
                if (this.DegreeFormat == AngleUnit.DMS_S)
                {
                    angle = Geo.Coordinates.AngularConvert.Dms_sToDeg(angle);
                }
            }
            return angle;
        }

        /// <summary>
        /// 角度单位类型
        /// </summary>
        public AngleUnit AngleUnit
        {
            get
            {
                if (this.checkBox_isDeg.Checked) return Coordinates.AngleUnit.Degree;
                return Coordinates.AngleUnit.Radian;
            }
        }

        public AngleUnit DegreeFormat
        {
            get
            {
                if (this.radioButton_degree.Checked)
                {
                    return Coordinates.AngleUnit.Degree;
                }
                if (this.radioButton_dms_s.Checked)
                {
                    return Coordinates.AngleUnit.DMS_S;
                }
                throw new NotImplementedException("算法尚未更新！");
            }
        }
    }
}
