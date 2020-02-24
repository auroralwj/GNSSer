//2017.08.11, czs, edit in hongqing, 单独提出
//2018.10.13, czs, edit in hmx, 进行了剥离

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
    public partial class StreamOptionPage : BaseGnssProcessOptionPage
    {
        public StreamOptionPage( )
        {
            InitializeComponent();

            this.Name = "计算流程"; 
        }


        public override void UiToEntity()
        {
            base.UiToEntity();

            Option.TopSpeedModel = this.checkBox1TopSpeedModel.Checked;
            Option.IsOnlySameParam = this.checkBox1IsOnlySameParam.Checked;

            Option.BufferSize = int.Parse(this.textBox_bufferSize.Text);

            Option.CaculateCount = int.Parse(this.textBox_caculateCount.Text);
            Option.StartIndex = int.Parse(this.textBox_startEpoch.Text); 


            Option.IsReversedDataSource = checkBox_isReversed.Checked;
            Option.IsClearOutBufferWhenReversing = this.checkBox1IsClearOutBufferWhenReversing.Checked;

            Option.OrdinalAndReverseCount = this.namedIntControlOrdinalAndReverseCount.GetValue();
            Option.ExtraStreamLoopCount = this.namedIntControl1ExtraStreamLoopCount.GetValue();

        }

        public override void EntityToUi()
        {
            base.UiToEntity();
             
            this.checkBox1TopSpeedModel.Checked = Option.TopSpeedModel;
            this.checkBox1IsOnlySameParam.Checked = Option.IsOnlySameParam;
            this.checkBox1IsClearOutBufferWhenReversing.Checked = Option.IsClearOutBufferWhenReversing;
            this.textBox_bufferSize.Text = Option.BufferSize + "";
            this.textBox_caculateCount.Text = Option.CaculateCount + "";
            this.textBox_startEpoch.Text = Option.StartIndex + "";
         

            checkBox_isReversed.Checked = Option.IsReversedDataSource;

           this.namedIntControlOrdinalAndReverseCount.SetValue(Option.OrdinalAndReverseCount);
            this.namedIntControl1ExtraStreamLoopCount.SetValue( Option.ExtraStreamLoopCount);

        }
          
        private void GnssOptionForm_Load(object sender, EventArgs e)
        {
           
        } 

    }
}