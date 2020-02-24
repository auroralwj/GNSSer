//2018.12.05, czs, create in hmx, LgoBaselineFileForm  莱卡

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Geo;
using Geo.IO;
using System.IO;
using Geo.Coordinates;
using Geo.Times;
using Geo.Algorithm;
using Geo.Algorithm.Adjust;
using Gnsser.Data;

namespace Gnsser.Winform
{
    public partial class LgoBaselineFileForm : Form, IShowLayer
    {
        Log log = new Log(typeof(LgoBaselineFileForm));

        public event ShowLayerHandler ShowLayer;

        public LgoBaselineFileForm()
        {
            InitializeComponent();
        }

        BaseLineNetManager BaseLineNet { get; set; }

        string [] BaseLinePathes=>this.fileOpenControl_baseline.FilePathes;


        private void BaselineNetClosureErrorForm_Load(object sender, EventArgs e)
        {
            this.fileOpenControl_baseline.Filter =  Setting.BaseLineFileFilterOfLgo;
            if (!Directory.Exists(Setting.TempDirectory)) { return; }
            var files = Directory.GetFiles(Setting.TempDirectory, "*" + Setting.BaseLineFileOfLgoExtension);
            if (files != null && files.Length > 0)
            {
                this.fileOpenControl_baseline.FilePathes = files;
            }
        }

        private void enumRadioControl_adjustType_EnumItemSelected(string arg1, bool arg2)
        { 
        }

        private void fileOpenControl_baseline_FilePathSetted(object sender, EventArgs e)
        {
        }

        private void button_showOnMap_Click(object sender, EventArgs e)
        {
            if (ShowLayer != null && BaseLineNet != null)
            {
                List<AnyInfo.Geometries.Point> pts = new List<AnyInfo.Geometries.Point>();
                int netIndex = 0;
                List<string> addedNames = new List<string>();
                foreach (var kv in BaseLineNet.KeyValues)
                {
                    foreach (var line in kv.Value )
                    {
                        var name = netIndex + "-" + line.BaseLineName.RovName;
                        if (!addedNames.Contains(name))
                        {
                            pts.Add(new AnyInfo.Geometries.Point(line.EstimatedGeoCoordOfRov,null, name));
                            addedNames.Add(name);
                        }
                        name = netIndex + "-" + line.BaseLineName.RefName;
                        if (!addedNames.Contains(name))
                        {
                            var geoCoord = CoordTransformer.XyzToGeoCoord(line.ApproxXyzOfRef);
                            pts.Add(new AnyInfo.Geometries.Point(geoCoord, null, name));
                            addedNames.Add(name);
                        }
                    }
                    netIndex++;
                }
                AnyInfo.Layer layer = AnyInfo.LayerFactory.CreatePointLayer(pts);
                ShowLayer(layer);
            }
        }
        MultiEpochLgoAscBaseLineFile BaseLineFile { get; set; }
        private void button_read_Click(object sender, EventArgs e)
        {
            var span = namedFloatControl_periodSpanMinutes.GetValue();
            BaseLineNet = new BaseLineNetManager();
            int netIndex = 0;
            foreach (var path in BaseLinePathes)
            {
                LgoAscBaseLineFileReader reader = new LgoAscBaseLineFileReader(path);
                BaseLineFile = reader.Read() ;
                var mgr = BaseLineFile.GetBaseLineNetManager();  
                BaseLineNet.Add(mgr);
                netIndex++; 
            }

            objectTableControl_sites.DataBind(BaseLineNet.GetSiteTable());
            this.objectTableControl_rawData.DataBind(BaseLineNet.GetLineTable());

        }

        private void button_write_Click(object sender, EventArgs e)
        {
            if(BaseLineFile == null)
            {
                Geo.Utils.FormUtil.ShowWarningMessageBox("请先读取！");
                return;
            }
            var outpath = Path.Combine(Setting.TempDirectory,  "Out_"+ BaseLineFile.Name);

            LgoAscBaseLineFileWriter writer = new LgoAscBaseLineFileWriter(outpath);
            writer.Write(BaseLineFile);

            Geo.Utils.FormUtil.ShowIfOpenDirMessageBox(Setting.TempDirectory);
        }
    }
}
