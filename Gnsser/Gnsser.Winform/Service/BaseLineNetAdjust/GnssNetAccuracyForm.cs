//2018.11.29, czs, create in hmx, GNSS网精度查询

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Gnsser.Winform
{
    public partial class GnssNetAccuracyForm : Form
    {
        public GnssNetAccuracyForm()
        {
            InitializeComponent();
        }

        private void button_run_Click(object sender, EventArgs e)
        {
            var receiverType = this.enumRadioControl_gnssReceiverType.GetCurrent<GnssReveiverType>();
            var gnssGrade = this.enumRadioControl_gnssGrade.GetCurrent<GnssGradeType>();
            var distance = this.namedFloatControl_distance.GetValue();
            var receiverVersion = this.namedStringControl_reveicerVertion.GetValue();
            var gradeToleranceMiniMeter = GnssReveiverNominalAccuracy.GetGnssGradeToleranceMeter(gnssGrade, distance) * 1000;
            var receiverTolerance = GnssReveiverNominalAccuracy.GetReceiverToleranceError(receiverType, distance, receiverVersion);

            //手动输入
            var levelFixed = this.namedFloatControl_fixedErrorLevel.GetValue();
            var verticalFixed = this.namedFloatControl_fixedErrorVertical.GetValue();
            var levelCoeef = this.namedFloatControl_levelCoefOfProprotion.GetValue();
            var verticalCoeef = this.namedFloatControl_verticalCoefOfProprotion.GetValue();

            var levelTolerance = GnssReveiverNominalAccuracy.GetToleranceErrorMilliMeter(levelFixed, levelCoeef, distance);
            var verticalTolerance = GnssReveiverNominalAccuracy.GetToleranceErrorMilliMeter(verticalFixed, verticalCoeef, distance);

            StringBuilder sb = new StringBuilder();
            sb.AppendLine("GNSS 网等级：" + gnssGrade + ", 精度限差[mm]：" + gradeToleranceMiniMeter);
            sb.AppendLine();
            sb.AppendLine("接收机 ：" + receiverType + " " + receiverVersion + ", 精度限差(水平，垂直)[mm]：" + receiverTolerance);
            sb.AppendLine();
            sb.AppendLine("手动输入，精度限差(水平，垂直)[mm]：" + levelTolerance  + ", "+ verticalTolerance);

            this.richTextBoxControl_result.Text = sb.ToString();
        }

        private void GnssNetAccuracyForm_Load(object sender, EventArgs e)
        {
            enumRadioControl_gnssReceiverType.Init<GnssReveiverType>();
            enumRadioControl_gnssGrade.Init<GnssGradeType>();
        }
    }
}
