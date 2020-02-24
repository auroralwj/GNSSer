//2018.11.06, czs, create in hmx, 差分 MW 值生成查看

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Geo;
using Gnsser.Correction;
using Gnsser.Data.Rinex;

namespace Gnsser.Winform
{
    public partial class MwDifferTableBuilderForm : Form
    {
        public MwDifferTableBuilderForm()
        {
            InitializeComponent();
            arrayCheckBoxControl_prn.Init(SatelliteNumber.DefaultGpsPrns);
            fileOpenControl1.Filter = Setting.RinexOFileFilter;
            fileOpenControl1.FilePathes = new string[] { Gnsser.Setting.GnsserConfig.SampleOFileA, Gnsser.Setting.GnsserConfig.SampleOFileB };
        }

        private void button_read_Click(object sender, EventArgs e)
        {
            var pathes = fileOpenControl1.FilePathes;
            ObjectTableStorage table = new ObjectTableStorage("MW原始值"); 
            var DataSource = new MultiSiteObsStream(pathes, BaseSiteSelectType.GeoCenter, true);
            var checkedPrns = arrayCheckBoxControl_prn.GetSelected<SatelliteNumber>();
            if(checkedPrns.Count == 0) { return; }

            var DcbRangeCorrector = new DcbRangeCorrector(GlobalDataSourceService.Instance.DcbDataService, false); 
            foreach (var mEpoch in DataSource)
            {
                bool isIndexAdded = false;
                foreach (var epoch in mEpoch)
                {
                    foreach (var sat in epoch)
                    {
                        //C1 改为 P1
                        DcbRangeCorrector.Correct(sat);

                        var prn = sat.Prn;
                        if (checkedPrns.Contains(prn))
                        {
                            if (!isIndexAdded)
                            {
                                table.NewRow();
                                table.AddItem("Epoch", mEpoch.ReceiverTime);
                                isIndexAdded = true;
                            }
                            table.AddItem(epoch.SiteName + "_" + prn, sat.MwCycle);
                        }
                    }
                }
            }
            objectTableControl1.DataBind(table);
        }

        private void fileOpenControl1_FilePathSetted(object sender, EventArgs e)
        {
            var path = this.fileOpenControl1.FilePath;
            if (File.Exists(path))
            {
                var file = Gnsser.Data.Rinex.RinexObsFileReader.Read(path);
                arrayCheckBoxControl_prn.Init(file.GetPrns());
            }
        }
    }
}
