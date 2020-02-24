//2017.02.09, czs, create in hongqing, 新宽巷数据处理。Between Satellite Difference


using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Threading.Tasks;
using System.IO;
using System.Text;
using System.Windows.Forms;
using Gnsser;
using Gnsser.Times;
using Gnsser.Data;
using Gnsser.Domain;
using Gnsser.Data.Rinex;
using Geo.Coordinates;
using Geo.Referencing;
using Geo.Algorithm.Adjust;
using Geo.Utils;
using Geo.Times;
using Gnsser.Service;
using Geo.IO;
using Geo;
using Gnsser.Checkers;

namespace Gnsser.Winform
{
    public partial class WideLaneOfBsdSolverForm : MultiFileStreamerForm
    {
        public WideLaneOfBsdSolverForm()
        {
            InitializeComponent();

            baseSatSelectingControl1.EnableBaseSat = true;
            baseSatSelectingControl1.SetSatelliteType(SatelliteType.G);

            //SetExtendTabPageCount(0, 0);
            SetEnableMultiSysSelection(false);
            SetEnableDetailSettingButton(false);
        }


        #region 属性
        /// <summary>
        /// 基准卫星
        /// </summary>
        SatelliteNumber BasePrn { get { return baseSatSelectingControl1.SelectedPrn; } }

        #endregion

        protected override void DetailSetting()
        {
        }

        /// <summary>
        /// 解析输入路径
        /// </summary>
        protected override List<string> ParseInputPathes(string[] inputPathes)
        {
            return new List<string>(inputPathes);
        }

        protected override void Run(string[] inputPathes)
        {
            var SmoothedMwValue = ObjectTableManager.Read(inputPathes);
            WideLaneOfBsdSolver solver = null;
            var espan = enabledTimePeriodControl1.GetEnabledValue();
            if (espan.Enabled)
            {
                var span = espan.Value;
                SmoothedMwValue = SmoothedMwValue.GetSub(span.StartDateTime, span.EndDateTime);
            }

            solver = new WideLaneOfBsdSolver(SmoothedMwValue, BasePrn, namedIntControl_minSiteCount.Value, this.namedIntControl_minEpoch.Value, OutputDirectory);
            solver.Run();

            this.BindTableA(solver.FractionValueTables.First);
            //输出
            if (this.checkBox_outputInt.Checked) { solver.IntValueTables.WriteAllToFileAndClearBuffer(); }
            if (this.checkBox_outputFraction.Checked) { solver.FractionValueTables.WriteAllToFileAndClearBuffer(solver.FractionValueTables.First); }
            if (this.checkBox_outputSumery.Checked) { solver.SummeryTables.WriteAllToFileAndClearBuffer(); }
        }

        private void WideLaneSolverForm_Load(object sender, EventArgs e)
        {
            fileOpenControl_inputPathes.FilePath = "";
        }
    }
}
