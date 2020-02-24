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
    public partial class IonoDcbViewerForm : Form
    {
        public IonoDcbViewerForm()
        {
            InitializeComponent();
        }

        private void button_view_Click(object sender, EventArgs e)
        {
            bool isValOrRms = checkBox_valOrRMS.Checked;
            var name = namedStringControl1.GetValue();
            var timePeriod = timePeriodControl1.TimePeriod;
            var strs = name.Split(new char[] { ';', ',' }, StringSplitOptions.RemoveEmptyEntries);

            var ionoService = GlobalIgsGridIonoDcbService.Instance;

            ObjectTableStorage table = new ObjectTableStorage();

            for (var time = timePeriod.StartDateTime; time < timePeriod.EndDateTime; time  += TimeSpan.FromDays(1)  )
            {
                table.NewRow();
                table.AddItem("Epoch", time);
                foreach (var item in strs)
                {
                    if(item.Length == 3 && SatelliteNumber.IsPrn(item))
                    {
                        var prn = SatelliteNumber.Parse(item);
                        var val = ionoService.GetDcb(new Time(time), prn);
                        if (isValOrRms)
                        { 
                            table.AddItem(prn, val.Value);
                        }
                        else
                        {
                            table.AddItem(prn, val.Rms);
                        }
                    }
                    else
                    {
                        var val = ionoService.GetDcb(new Time(time), item);
                        if (isValOrRms)
                        {
                            table.AddItem(item, val.Value);
                        }
                        else
                        {
                            table.AddItem(item, val.Rms);
                        }
                    }
                }
               
            }


            objectTableControl1.DataBind(table);
        }

        private void IonoDcbViewerForm_Load(object sender, EventArgs e)
        {
            namedStringControl1.SetValue("G01,G02,G03,G04,G05,G06,G07,G08,G09,G10,algo,ajac");
            timePeriodControl1.SetTimePerid(new TimePeriod(DateTime.Now - TimeSpan.FromDays(100), DateTime.Now - TimeSpan.FromDays(20)));
        }
    }
}
