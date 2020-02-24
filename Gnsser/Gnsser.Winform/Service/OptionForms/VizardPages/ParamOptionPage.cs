//2019.03.04, czs, create in hongqing, 参数面板

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Gnsser.Service;
using Gnsser.Times;
using Geo.Times;
using Geo.Coordinates;
using Geo.Algorithm;
using Geo.Winform.Wizards;
 

namespace Gnsser.Winform
{
    public partial class ParamOptionPage : BaseGnssProcessOptionPage
    {
        public ParamOptionPage()
        {
            InitializeComponent();

            this.Name = "参数"; 
        }


        public override void UiToEntity()
        {
            base.UiToEntity();
            this.Option.IsSiteNameIncluded = this.checkBox_IsSiteNameIncluded.Checked;
        }
        public override void EntityToUi()
        {
            base.UiToEntity();
            this.checkBox_IsSiteNameIncluded.Checked =this.Option.IsSiteNameIncluded;

        }

        private void GnssOptionForm_Load(object sender, EventArgs e)
        { 
        } 

    }
}