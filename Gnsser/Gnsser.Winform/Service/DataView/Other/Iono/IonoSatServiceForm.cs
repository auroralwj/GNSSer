//2018.06.05, czs, edit in hmx, 自动读取坐标

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Gnsser.Data;
using Gnsser.Service;
using Geo.Utils;
using Gnsser;
using Geo.Coordinates;
using Geo.Times;
using Geo;
using Geo.IO;
using Geo.Draw;

namespace Gnsser.Winform
{
    public partial class IonoSatServiceForm : Form
    {
        public IonoSatServiceForm()
        {
            InitializeComponent();
        }

        private void button_view_Click(object sender, EventArgs e)
        {
            double cutOff = namedFloatControl_satCutOff.GetValue();
            bool isRangeOrTec = checkBox_isRangeOrTec.Checked;
            var prnsString = namedStringControl_prn.GetValue();
            var step = timeLoopControl1.TimeStepControl.GetValue();
            var timePeriod = timeLoopControl1.TimePeriodControl.TimePeriod;
            var prns = SatelliteNumber.ParsePRNsBySplliter(prnsString, new char[] { ',', '，', ';' });
            IIonoService ionoService = null;

            IonoSerivceType type = enumRadioControl_IonoSerivceType.GetCurrent<Gnsser.IonoSerivceType>();

            switch (type)
            {
                case IonoSerivceType.IgsGrid:
                    ionoService = GlobalIgsGridIonoService.Instance;
                    break;
                case IonoSerivceType.SphericalHarmonics:
                    ionoService = GlobalCodeHarmoIonoService.Instance;
                    break;
                case IonoSerivceType.GpsKlobuchar:
                    ionoService = GlobalKlobucharIonoService.Instance;
                    break;
                default:
                    break;
            } 

            var ephService = GlobalIgsEphemerisService.Instance;
            var siteXyz = XYZ.Parse(namedStringControl_coord.GetValue());

            double freqValue = Frequence.GpsL1.Value;
            if (!checkBox_freq.Checked)
            {
                freqValue = Frequence.GpsL2.Value;
            }


            ObjectTableStorage table = new ObjectTableStorage();

            for (var time = timePeriod.Start; time < timePeriod.End; time += TimeSpan.FromMinutes(step))
            {
                table.NewRow();
                table.AddItem("Epoch", time);
                foreach (var prn in prns)
                {
                    var eph = ephService.Get(prn, time);
                     if(eph == null)
                    {
                        continue;
                    }
                    var satXyz = eph.XYZ;

                    var polar = CoordTransformer.XyzToGeoPolar(satXyz, siteXyz, AngleUnit.Degree);
                    if (cutOff > polar.Elevation)
                    {
                        continue;
                    }


                    if (isRangeOrTec)
                    {
                        var range = ionoService.GetSlopeDelayRange(time, siteXyz, satXyz, freqValue);

                        table.AddItem(prn, range);
                    }
                    else
                    {
                        var tec = ionoService.GetSlope(time, siteXyz, satXyz);


                        table.AddItem(prn, tec.Value);
                    }
                }

            }


            objectTableControl1.DataBind(table);
        }
         

        private void IonoDcbViewerForm_Load(object sender, EventArgs e)
        {
            namedStringControl_coord.SetValue("4033469.9897    23673.0251  4924301.3837");

            var gpsPrns = Geo.Utils.StringUtil.ToString(SatelliteNumber.GpsPrns, ",");

            namedStringControl_prn.SetValue(gpsPrns); 
            timeLoopControl1.TimePeriodControl.SetTimePerid( Setting.SampleSession);
            timeLoopControl1.TimeStepControl.SetValue(0.5);

            enumRadioControl_IonoSerivceType.Init<Gnsser.IonoSerivceType>(true);
        }

        private void button_coordSet_Click(object sender, EventArgs e)
        {
            CoordSelectForm form = new CoordSelectForm();
            if(form.ShowDialog() == DialogResult.OK)
            {
                this.namedStringControl_coord.SetValue(form.XYZ.ToString());
            }
        }
    }
}
