using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Gnsser;

namespace Gnsser.Winform
{
    public partial class SingleSiteGnssSolverTypeSelectionControl : Geo.Winform.Controls.EnumSelectControl<SingleSiteGnssSolverType>
    {
        public SingleSiteGnssSolverTypeSelectionControl()
        {
            InitializeComponent();
            this.CurrentdType = SingleSiteGnssSolverType.无电离层组合PPP;
            this.Text = "单站 GNSS 计算器";

            if(Setting.VersionType !=  Geo.VersionType.Development)
            foreach (var item in this.RadioButtons)
            {
                var type = (GnssSolverType)Enum.Parse(typeof(GnssSolverType), item.Text);
                if (type == GnssSolverType.最简伪距定位
                    || type == GnssSolverType.无电离层组合PPP
                    )
                {
                    continue;
                }
                item.Enabled = false;
                this.flowLayoutPanel1.Controls.Remove(item);
            }
        }



    }




}
