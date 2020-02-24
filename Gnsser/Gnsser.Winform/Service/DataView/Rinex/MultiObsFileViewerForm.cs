//2018.06.06, czs, create in hmx, 批量观测文件查看器

using Gnsser.Times;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using Geo.Utils;
using System.Text;
using System.IO;
using System.Windows.Forms;
using Gnsser.Data.Rinex;
using Gnsser;
using Geo.Coordinates; 
using Geo.Times;
using Geo;
using Gnsser.Domain;
using Geo.Draw;
using Gnsser.Data;
using Geo.IO;
using AnyInfo;

namespace Gnsser.Winform
{
    /// <summary>
    /// 批量观测文件查看器
    /// </summary>
    public partial class MultiObsFileViewerForm : Form, Gnsser.Winform.IShowLayer
    {
        Log log = new Log(typeof(MultiObsFileViewerForm));
        public MultiObsFileViewerForm()
        {
            InitializeComponent();
        }
        #region 属性
        /// <summary>
        /// 地图显示
        /// </summary>
        public event ShowLayerHandler ShowLayer;  
        List<RinexObsFileHeader> Headers { get; set; }

        #endregion

        private void button_read_Click(object sender, EventArgs e)
        {
            if (fileOpenControl1.FilePathes.Length == 0)
            {
                Geo.Utils.FormUtil.ShowWarningMessageBox("文件不存在！");
                return;
            }
              Headers = ReadFileHeaders(fileOpenControl1.FilePathes);

            ObjectTableStorage table = new ObjectTableStorage();

            foreach (var header in Headers)
            {
                table.NewRow();
                table.AddItem("Name", header.FileName);
                table.AddItem("StartEpoch", header.StartTime); 
                table.AddItem("ObsCodes", header.GetReadableObsCodes());
                var geocoord = CoordTransformer.XyzToGeoCoord(header.ApproxXyz);
                table.AddItem("GeoCoord", geocoord);
                table.AddItem("Interval", header.Interval);
                table.AddItem("Version", header.Version);
                table.AddItem("ReceiverType", header.SiteInfo.ReceiverType);
                table.AddItem("ReceiverNumber", header.SiteInfo.ReceiverNumber);
                table.AddItem("AntennaType", header.SiteInfo.AntennaType);
                table.AddItem("AntennaNumber", header.SiteInfo.AntennaNumber);
                table.AddItem("ApproxXyz", header.ApproxXyz);
                table.AddItem("EndEpoch(MayBe)", header.EndTime);
            }

            BindDataSource(table);

        }
         
        /// <summary>
        /// 数据绑定
        /// </summary>
        /// <param name="table"></param>
        private void BindDataSource(ObjectTableStorage table)
        {
            this.objectTableControl1.DataBind(table);
        }

        private void ObsFileViewerForm_Load(object sender, EventArgs e) {
            this.fileOpenControl1.FilePathes = new string[]
            {
                Setting.GnsserConfig.SampleOFileA,
                Setting.GnsserConfig.SampleOFileB
            };
        }
             
        /// <summary>
        /// 读取数据
        /// </summary>
        private List<RinexObsFileHeader> ReadFileHeaders(string[] filePathes)
        {
            List<RinexObsFileHeader> headers = new List<RinexObsFileHeader>();
            foreach (var obsPath in filePathes)
            {
                RinexObsFileHeader header = null;
                RinexObsFileReader obsFileReader;
                string lastChar = Geo.Utils.StringUtil.GetLastChar(obsPath);
                string lastChar3 = Geo.Utils.StringUtil.GetLastChar(obsPath, 3);
                string lastChar5 = Geo.Utils.StringUtil.GetLastChar(obsPath, 5);
                if (String.Equals(lastChar, "o", StringComparison.CurrentCultureIgnoreCase) || String.Equals(lastChar3, "rnx", StringComparison.CurrentCultureIgnoreCase))
                {
                    obsFileReader = new RinexObsFileReader(obsPath);
                    header = obsFileReader.GetHeader();
                }

                if (String.Equals(lastChar, "z", StringComparison.CurrentCultureIgnoreCase)
                    || String.Equals(lastChar3, "crx", StringComparison.CurrentCultureIgnoreCase)
                    || String.Equals(lastChar5, "crx.gz", StringComparison.CurrentCultureIgnoreCase)
                    )
                {
                    Geo.IO.InputFileManager inputFileManager = new Geo.IO.InputFileManager(Setting.TempDirectory);
                    var obsPathes = inputFileManager.GetLocalFilePath(obsPath, "*.*o;*.rnx", "*.*");

                    obsFileReader = new RinexObsFileReader(obsPathes);
                    header = obsFileReader.GetHeader();
                }
                if(header ==null)
                {
                   log.Error("不支持输入文件格式！" + obsPath);
                }
                else
                {
                    headers.Add(header);
                    log.Info("成功读入：" + obsPath);
                } 
            }   
            return headers;
        }

         

        private void button_drawOnMap_Click(object sender, EventArgs e)
        {
            if (ShowLayer != null && Headers != null)
            { 
                List<AnyInfo.Geometries.Point> lonlats = GetPoints(Headers); 
                Layer layer = LayerFactory.CreatePointLayer(lonlats);
                ShowLayer(layer);
            }

        }

        public List<AnyInfo.Geometries.Point> GetPoints(List<RinexObsFileHeader> Headers)
        {
            List<AnyInfo.Geometries.Point> coords = new List<AnyInfo.Geometries.Point>();
            int i = 0;
            foreach (var item in Headers)
            {
                var xyz = item.ApproxXyz;
                 var geocoord = CoordTransformer.XyzToGeoCoord(xyz);
                coords.Add(new AnyInfo.Geometries.Point(geocoord, i+"", item.MarkerName));

                i++;
            }
            return coords;
        }
         
    }
}