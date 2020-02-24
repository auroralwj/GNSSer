//2017.12.10, czs, edit in hongqing, 单独提出结果检核

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
    public partial class ResultCheckOptionPage : BaseGnssProcessOptionPage
    {
        public ResultCheckOptionPage( )
        {
            InitializeComponent();

            this.Name = "结果检核"; 
        }


        public override void UiToEntity()
        {
            base.UiToEntity();

            Option.IsResultCheckEnabled = this.checkBox_IsResultCheckEnabled.Checked;
            Option.MaxErrorTimesOfPostResdual = this.namedFloatControl1MaxErrorTimesOfPostResdual.GetValue();
            Option.IsResidualCheckEnabled = checkBox1IsResidualCheckEnabled.Checked;
            Option.MaxLoopCount = int.Parse(this.textBox_maxLoopCount.Text); 
            //Option.DataSourceOption.IsDownloadingSurplurseIgsProducts = this.checkBox1DownloadingSurplusIgsProduct.Checked;
 
            Option.MaxStdDev = Double.Parse(this.textBox_maxStdDev.Text);

            Option.MaxMeanStdTimes =  Double.Parse( this.textBoxMaxMeanStdTimes.Text); 

            Option.ExemptedStdDev = this.namedFloatControlExemptedStdDev.GetValue();
        }

        public override void EntityToUi()
        {
            base.UiToEntity();
             
            this.checkBox_IsResultCheckEnabled.Checked = Option.IsResultCheckEnabled;
            this.namedFloatControl1MaxErrorTimesOfPostResdual.SetValue(Option.MaxErrorTimesOfPostResdual);
            checkBox1IsResidualCheckEnabled.Checked = Option.IsResidualCheckEnabled;
            this.textBox_maxLoopCount.Text = Option.MaxLoopCount + "";
            this.textBoxMaxMeanStdTimes.Text = Option.MaxMeanStdTimes + "";
            this.textBox_maxStdDev.Text = Option.MaxStdDev + "";

            this.namedFloatControlExemptedStdDev.SetValue(Option.ExemptedStdDev);
        }
          
        private void GnssOptionForm_Load(object sender, EventArgs e)
        {
           
        } 

    }
}