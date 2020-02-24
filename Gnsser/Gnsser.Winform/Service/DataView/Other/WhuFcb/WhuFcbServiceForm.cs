//2018.09.17, czs, create in hmx, 武大FCB服务

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.IO;
using System.Windows.Forms;
using Gnsser.Data.Rinex;
using Geo.Coordinates;
using AnyInfo;
using Geo.Times;
using Gnsser.Data;
using System.Drawing.Drawing2D;
using System.Windows.Forms.DataVisualization.Charting;
using Gnsser;
using Geo.Referencing;
using Geo.Algorithm.Adjust;
using Geo.Utils;
using Geo;
using Geo.Algorithm;
using Gnsser;
using Gnsser.Service;

namespace Gnsser.Winform
{
    public partial class WhuFcbServiceForm : Form
    {
        public WhuFcbServiceForm()
        {
            InitializeComponent();
        }
         

        FcbFile FcbFile { get; set; }
        FcbDataService FcbDataService { get; set; }

        private void button_multiShow_Click(object sender, EventArgs e)
        {
            var service = GetFcbDataService();
            var timeperiod = this.timePeriodControl1.TimePeriod;
            SatelliteNumber basePrn = (SatelliteNumber)this.bindingSource_basePrn.Current;              

            ObjectTableStorage table = new ObjectTableStorage("NarrowFcbOfBsd");
            var Interval = TimeSpan.FromMinutes(15);
            for (var time = timeperiod.Start; time <= timeperiod.End; time += Interval)
            {
                bool isNullRow = true;
                foreach (var prn in SatelliteNumber.DefaultGpsPrns)
                {
                    var bsdFrac = service.GetNLFcbOfBsdValue(time, prn, basePrn);
                    if (bsdFrac != null)
                    {
                        if (isNullRow)
                        {
                            table.NewRow();
                            table.AddItem("Epoch", time);
                        }

                        table.AddItem(prn.ToString(), bsdFrac);
                        isNullRow = false;
                    }
                }
            }
            this.objectTableControl1.DataBind(table);
        }

        private void IgsFcbViewerForm_Load(object sender, EventArgs e)
        {
            bindingSource_basePrn.DataSource = SatelliteNumber.DefaultGpsPrns;
            this.directorySelectionControl1.Path = @"E:\";
        }
          
        private void button_vewSingleEpoch_Click(object sender, EventArgs e)
        {           
           var service =  GetFcbDataService();

            var epoch = new Time( namedTimeControl1.GetValue());
            SatelliteNumber basePrn = (SatelliteNumber)this.bindingSource_basePrn.Current;

            var bsdFrac = service.GetNLFcbOfBsd(epoch, basePrn);
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("NL");
            sb.AppendLine(epoch.ToString());
            if (bsdFrac != null)
            {
                foreach (var item in bsdFrac)
                {
                    sb.AppendLine(item.Key + ":\t  " + item.Value);
                }
            }
            else
            {
                sb.AppendLine("No Data!!");
            }

            richTextBoxControl1.Text = sb.ToString();
        }

        private FcbDataService GetFcbDataService()
        {
            if (FcbDataService == null)
            {
                var dcbDir = directorySelectionControl1.Path;
                FcbDataService = new FcbDataService(dcbDir);
            }
            return FcbDataService;
        }


        private void button_viewSingleWL_Click(object sender, EventArgs e)
        {
            var service = GetFcbDataService();

            var epoch = new Time(namedTimeControl1.GetValue());
            SatelliteNumber basePrn = (SatelliteNumber)this.bindingSource_basePrn.Current;

            var bsdFrac = service.GetWLFcbOfBsd(epoch, basePrn);
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("WL");
            sb.AppendLine(epoch.ToString());
            foreach (var item in bsdFrac.KeyValues)
            {
                sb.AppendLine(item.Key + ":\t  " + item.Value);
            }
            if(bsdFrac.Count == 0)
            {
                sb.AppendLine("No Data!!!");
            }
            richTextBoxControl1.Text = sb.ToString();
        }

        private void button_multiWLView_Click(object sender, EventArgs e)
        {
            var service = GetFcbDataService();
            var timeperiod = this.timePeriodControl1.TimePeriod;
            SatelliteNumber basePrn = (SatelliteNumber)this.bindingSource_basePrn.Current;

            ObjectTableStorage table = new ObjectTableStorage("WideFcbOfBsd");
            var Interval = TimeSpan.FromDays(1);
            for (var time = timeperiod.Start; time <= timeperiod.End; time += Interval)
            {
                var bsdFrac = service.GetWLFcbOfBsd(time, basePrn);
                if (bsdFrac != null && bsdFrac.Count > 0)
                {
                    table.NewRow();
                    table.AddItem("Epoch", time);

                    foreach (var kv in bsdFrac.KeyValues)
                    {
                        table.AddItem(kv.Key.ToString(), kv.Value);
                    }
                }

            }
            this.objectTableControl1.DataBind(table);
        }

        private void button_saveToGnsserFcb_Click(object sender, EventArgs e)
        {
            var service = GetFcbDataService();
            var timeperiod = this.timePeriodControl1.TimePeriod;
            SatelliteNumber basePrn = (SatelliteNumber)this.bindingSource_basePrn.Current;
            var Interval = TimeSpan.FromMinutes(15);


            var toPath = Path.Combine(Setting.TempDirectory, "WhuNLFcbToGNSSer" + Gnsser.Setting.FcbExtension);
            using (FcbOfUpdWriter writer = new FcbOfUpdWriter(toPath))
            {

                for (var time = timeperiod.Start; time <= timeperiod.End; time += Interval)
                {
                    FcbOfUpd fcbOfUpd = new FcbOfUpd(basePrn, time, false);
                    foreach (var prn in SatelliteNumber.DefaultGpsPrns)
                    {

                        var bsdFrac = service.GetNLFcbOfBsdValue(time, prn, basePrn);
                        if (bsdFrac != null)
                        {
                            fcbOfUpd.Add(prn, bsdFrac);
                        }
                    }

                    if (fcbOfUpd.DataCount > 0)
                    {
                        writer.Write(fcbOfUpd);
                    }
                }
            }
            Geo.Utils.FormUtil.ShowOkAndOpenDirectory(Setting.TempDirectory);
        }
        private void button_saveWLToGNSSer_Click(object sender, EventArgs e)
        {
            var service = GetFcbDataService();
            var timeperiod = this.timePeriodControl1.TimePeriod;
            SatelliteNumber basePrn = (SatelliteNumber)this.bindingSource_basePrn.Current;

            var toPath = Path.Combine(Setting.TempDirectory, "WhuWLFcbToGNSSer" + Gnsser.Setting.FcbExtension);
            using (FcbOfUpdWriter writer = new FcbOfUpdWriter(toPath))
            {
                var Interval = TimeSpan.FromDays(1);
                for (var time = timeperiod.Start.Date; time <= timeperiod.End; time += Interval)
                {
                    var bsdFrac = service.GetWLFcbOfBsd(time, basePrn);
                    if (bsdFrac != null && bsdFrac.Count > 0)
                    {
                        FcbOfUpd fcbOfUpd = new FcbOfUpd(basePrn, time, true);
                        foreach (var kv in bsdFrac.KeyValues)
                        {
                            fcbOfUpd.Add(kv.Key, kv.Value);
                        }
                        if (fcbOfUpd.DataCount > 0)
                        {
                            writer.Write(fcbOfUpd);
                        }
                    }
                }
            }
            Geo.Utils.FormUtil.ShowOkAndOpenDirectory(Setting.TempDirectory);
        }
    }
}

