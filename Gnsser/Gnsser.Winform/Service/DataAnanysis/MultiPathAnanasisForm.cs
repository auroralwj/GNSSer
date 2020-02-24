//2019.01.01, czs, create in hmx, 多路径效应

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.IO;
using System.Threading.Tasks;
using System.Windows.Forms;
using Gnsser.Data;
using Geo.Times;
using Geo.IO;
using Geo;
using Gnsser.Data.Rinex;
using Geo.Winform;

namespace Gnsser.Winform
{
    public partial class MultiPathAnanasisForm : Form
    {
        public MultiPathAnanasisForm()
        {
            InitializeComponent();
            fileOpenControl1_fiels.Filter = Setting.RinexOFileFilter;
        }
        double AngleCut => this.namedFloatControl_angleCut.GetValue(); 

        private void button_run_Click(object sender, EventArgs e)
        {
            var fileNames = fileOpenControl1_fiels.FilePathes;
            var angleCut = AngleCut; 
            var satType = gnssSystemSelectControl1.SatelliteType;
            int index = 0;
            foreach (var filePath in fileNames)
            { 
               var table =  BuildMpTable(angleCut, filePath, satType);
                if(index == 0)
                {
                    objectTableControl1.DataBind(table);
                }

                if (table == null || table.RowCount == 0) { Geo.Utils.FormUtil.ShowOkMessageBox("数据表为空！"); continue; }

                TableObjectViewForm form = new TableObjectViewForm(table);
                form.Show();
                index++;
            }
        }
        /// <summary>
        ///构建一个文件的表格
        /// </summary>
        /// <param name="angleCut"></param>
        /// <param name="filePath"></param>
        /// <param name="satType"></param>
        /// <returns></returns>
        private ObjectTableStorage BuildMpTable(double angleCut, string filePath, SatelliteType satType)
        {
            var name = Path.GetFileName(filePath);
            MultiPathSolver multiPathSolver = new MultiPathSolver(satType);
            var stream = new RinexFileObsDataSource(filePath);
            ObjectTableStorage table = new ObjectTableStorage("MPOf" + name);
            foreach (var epoch in stream)
            {
                epoch.RemoveOtherGnssSystem(satType);
                if(epoch.Count == 0)
                {                    
                    continue;
                }

                table.NewRow();
                table.AddItem("Epoch", epoch.ReceiverTime);

                foreach (var sat in epoch)
                {
                    var eph = GlobalIgsEphemerisService.Instance.Get(sat.Prn, sat.ReceiverTime);
                    if (eph != null)
                    {
                        var polar = Geo.Coordinates.CoordTransformer.XyzToGeoPolar(eph.XYZ, epoch.SiteInfo.ApproxXyz);
                        if (polar.Elevation < angleCut) { continue; }
                    }


                    //var L1 = sat.FrequenceA.PhaseRange.RawPhaseValue;
                    //var L2 = sat.FrequenceB.PhaseRange.RawPhaseValue;
                    var L1 = sat.FrequenceA.PhaseRange.Value;
                    var L2 = sat.FrequenceB.PhaseRange.Value;
                    var P1 = sat.FrequenceA.PseudoRange.Value;
                    var P2 = sat.FrequenceB.PseudoRange.Value;

                    if(L1 == 0 
                        || L2 == 0
                        || P1 == 0
                        || P2 == 0
                        )
                    {
                        continue;
                    }

                    var mp1 = multiPathSolver.GetMpA(P1, L1, L2);
                    var mp2 = multiPathSolver.GetMpB(P2, L1, L2);
                    table.AddItem(sat.Prn + "_MP1", mp1);
                    table.AddItem(sat.Prn + "_MP2", mp2);
                }
            }
            return table;
        }

        private void ObsPeriodDividerForm_Load(object sender, EventArgs e)
        {
            this.fileOpenControl1_fiels.FilePath = Setting.GnsserConfig.SampleOFileA;
            gnssSystemSelectControl1.SetSatelliteType(SatelliteType.G);
        }
    }

    /// <summary>
    /// 多路径误差计算器
    /// </summary>
    public class MultiPathSolver
    {

        public MultiPathSolver(SatelliteType SatelliteType)
        {
            this.SatelliteType = SatelliteType;
            FrequenceA = Frequence.GetFrequenceA(SatelliteType);
            FrequenceB = Frequence.GetFrequenceB(SatelliteType);
            CoeefOfIono = Math.Pow(FrequenceA.Value / FrequenceB.Value, 2.0);
        }
        public SatelliteType SatelliteType { get; set; }
        /// <summary>
        /// 电离层系数
        /// </summary>
        public double CoeefOfIono { get; set; }
        /// <summary>
        /// 第一频率
        /// </summary>
        public Frequence FrequenceA { get; set; }
        /// <summary>
        /// 第二频率
        /// </summary>
        public Frequence FrequenceB { get; set; }
        /// <summary>
        /// 伪距 1 多路径 效应，含伪距残差，忽略了载波的影响 
        /// </summary>
        /// <param name="P1"></param>
        /// <param name="L1">距离</param>
        /// <param name="L2"></param>
        /// <returns></returns>
        public double GetMpA(double P1, double L1, double L2)
        { 
            var temp = 1 +  2.0 / (CoeefOfIono - 1);
            var result = P1 -  temp * L1 +  (temp -1) * L2;//表达式一致，系数不同
            return result;
        }
        /// <summary>
        /// 伪距 2 多路径 效应 ，含伪距残差，忽略了载波的影响 
        /// </summary>
        /// <param name="P1"></param>
        /// <param name="L1">距离</param>
        /// <param name="L2"></param>
        /// <returns></returns>
        public double GetMpB(double P2, double L1, double L2)
        { 
            var temp = 2.0 * CoeefOfIono / (CoeefOfIono - 1);
            var result = P2 - temp * L1 + (temp - 1) * L2;//表达式一致，系数不同
            return result;
        }
    }
}