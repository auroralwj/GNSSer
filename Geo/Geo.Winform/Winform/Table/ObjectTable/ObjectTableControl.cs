//2017.10.03, czs, create in hongqing, 表对象查看和操作UI
//2018.07.15, czs, edit in HMX, 增加多项式拟合等

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms; 
using System.Drawing.Drawing2D;
using System.Windows.Forms.DataVisualization.Charting;
using Geo.Utils; 
using Gnsser;
using Geo.Coordinates;
using Geo.Times;
using Geo; 
using Geo.IO;
using Geo.Draw;
using Geo.Algorithm;

namespace Geo.Winform
{
    /// <summary>
    /// 表对象查看和操作UI
    /// </summary>
    public partial class ObjectTableControl : UserControl
    {
        Log log = new Log(typeof(ObjectTableControl));
        /// <summary>
        /// 构造函数
        /// </summary>
        public ObjectTableControl()
        {
            InitializeComponent();
        }

        /// <summary>
        /// 当前表
        /// </summary>
        public ObjectTableStorage TableObjectStorage { get; set; }
        /// <summary>
        /// 数据表
        /// </summary>
        public System.Windows.Forms.DataGridView DataGridView { get { return dataGridView1; } }

        #region 调用方法
        /// <summary>
        /// 显示信息
        /// </summary>
        /// <param name="info"></param>
        public void ShowInfo(string info)
        {
            this.Invoke(new Action(() =>
            {
                toolStripLabel_info.Text = info;
            }));
        }
        /// <summary>
        /// 数据绑定
        /// </summary>
        /// <param name="table"></param>
        public void DataBind(ObjectTableStorage table)
        {
            var title = table.Name + "， 共 " + table.ColCount + "列 × " + table.RowCount + "行";
            log.Debug("绑定显示 " + title);
   
            this.TableObjectStorage = table;
            //this.Invoke(new Action(() =>//采用Invoke将出错
            //{
                this.dataGridView1.DataSource = table.GetDataTable();
                toolStripLabel_info.Text = title;
                bindingSource1.DataSource = this.dataGridView1.DataSource;
                bindingNavigator1.BindingSource = bindingSource1;
            //}));
        }
        private void dataGridView1_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (e.Value is Time)
            {
                e.Value = e.Value.ToString();
            }
            if (e.Value is DateTime)
            {
                e.Value = ((DateTime)e.Value).ToString("yyyy-MM-dd HH:mm:ss");
            }

        }
        #endregion
        /// <summary>
        /// 清空
        /// </summary>
        public void Clear()
        {
            this.dataGridView1.DataSource = null;
        }

        #region 操作

        private void 设置选中单元值SToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (TableObjectStorage == null || TableObjectStorage.RowCount == 0) { Geo.Utils.FormUtil.ShowOkMessageBox("数据表为空！"); return; }

            double val = 0;
            if (Geo.Utils.FormUtil.ShowInputNumeralForm("设置数值：", out val, 0))
            {
                int fromRowIndex, formColIndex, toRowIndex, toColIndex;
                Geo.Utils.DataGridViewUtil.GetSelectedIndexRange(DataGridView, out fromRowIndex, out formColIndex, out toRowIndex, out toColIndex);

                TableObjectStorage.SetCellValues(val, fromRowIndex, formColIndex, toRowIndex, toColIndex);
                this.DataBind(TableObjectStorage);
            }
        }
        private void 所有列减去首位有效数ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (TableObjectStorage == null) { Geo.Utils.FormUtil.ShowOkMessageBox("数据表为空！"); return; }
            var table = TableObjectStorage.GetTableAllColMinusFirstValid();
            this.DataBind(table);
        }

        private void 所有列减去末位有效数ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (TableObjectStorage == null) { Geo.Utils.FormUtil.ShowOkMessageBox("数据表为空！"); return; }
            var table = TableObjectStorage.GetTableAllColMinusLastValid();
            this.DataBind(table);
        }

        private void 同列前后差分一次DToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (TableObjectStorage == null) { Geo.Utils.FormUtil.ShowOkMessageBox("数据表为空！"); return; }
            var table = TableObjectStorage.GetTableDifferPrevValue();
            this.DataBind(table);
        }

        private void 移除指定列RToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (TableObjectStorage == null) { Geo.Utils.FormUtil.ShowOkMessageBox("数据表为空！"); return; }

            var list = Geo.Utils.FormUtil.OpenFormSelectTitles(TableObjectStorage.ParamNames, false);
            if (list.Count < 1) return;

            TableObjectStorage.RemoveCols(list);
            this.DataBind(TableObjectStorage);
        }

        private void 移除选定行RToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (TableObjectStorage == null) { Geo.Utils.FormUtil.ShowOkMessageBox("数据表为空！"); return; }
            DataGridView DataGridView = this.dataGridView1;
            var indexName = TableObjectStorage.GetIndexColName();
            //检查是否选中列
            if (DataGridView.SelectedRows == null || DataGridView.SelectedRows.Count < 1)
            {
                MessageBox.Show("请选择要移除的行！");
                return;
            }

            List<object> indexes = new List<object>();
            foreach (DataGridViewRow row in DataGridView.SelectedRows)
            {
                var indexObj = row.Cells[indexName].Value;
                indexes.Add(indexObj);
            }
            TableObjectStorage.RemoveRows(indexes);


            //排序后以下算法可能失败
            //List<int> indexes = new List<int>();
            //foreach (DataGridViewRow row in DataGridView.SelectedRows)
            //{ 
            //    var index = DataGridView.Rows.IndexOf(row);
            //    indexes.Add(index);
            //} 
            //TableObjectStorage.RemoveRows(indexes);

            this.DataBind(TableObjectStorage);
        }

        private void 移除行当某列满足ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (TableObjectStorage == null) { Geo.Utils.FormUtil.ShowOkMessageBox("数据表为空！"); return; }

            var form = new NumeralFilterForm();

            if (form.ShowDialog() != System.Windows.Forms.DialogResult.OK) return;

            TableObjectStorage.RemoveRowsWithFilter(form.InputedValue, form.CompareOperator);
            this.DataBind(TableObjectStorage);

        }

        private void 清空所选数据CToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (TableObjectStorage == null) { Geo.Utils.FormUtil.ShowOkMessageBox("数据表为空！"); return; }
            int fromRowIndex, formColIndex, toRowIndex, toColIndex;
            Geo.Utils.DataGridViewUtil.GetSelectedIndexRange(DataGridView, out fromRowIndex, out formColIndex, out toRowIndex, out toColIndex);

            TableObjectStorage.RemoveCells(fromRowIndex, formColIndex, toRowIndex, toColIndex);
            this.DataBind(TableObjectStorage);
        }

        private void 移除行当前指定列满足ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (TableObjectStorage == null) { Geo.Utils.FormUtil.ShowOkMessageBox("数据表为空！"); return; }

            var colName = Geo.Utils.FormUtil.OpenFormSelectOne(TableObjectStorage.ParamNames);
            if (String.IsNullOrWhiteSpace(colName)) return;

            var form = new NumeralFilterForm();

            if (form.ShowDialog() != System.Windows.Forms.DialogResult.OK) return;

            TableObjectStorage.RemoveRowsWithFilter(colName, form.InputedValue, form.CompareOperator);
            this.DataBind(TableObjectStorage);
        }

        private void 操作指定列IToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (TableObjectStorage == null) { Geo.Utils.FormUtil.ShowOkMessageBox("数据表为空！"); return; }

            var list = Geo.Utils.FormUtil.OpenFormSelectOne(TableObjectStorage.ParamNames);
            if (String.IsNullOrWhiteSpace(list)) return;
            var colName = list;
            var form = new RadioNumeralFilterForm();

            if (form.ShowDialog() != System.Windows.Forms.DialogResult.OK) return;

            this.TableObjectStorage.UpdateColumnBy(colName, form.InputedValue, form.NumeralOperationType);
            this.DataBind(TableObjectStorage);

        }


        private void 更新所有数OToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (TableObjectStorage == null) { Geo.Utils.FormUtil.ShowOkMessageBox("数据表为空！"); return; }

            var form = new RadioNumeralFilterForm();

            if (form.ShowDialog() != System.Windows.Forms.DialogResult.OK) return;

            this.TableObjectStorage.UpdateAllBy(form.InputedValue, form.NumeralOperationType);
            this.DataBind(TableObjectStorage);

        }

        private void 所有行操作指定列ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (TableObjectStorage == null) { Geo.Utils.FormUtil.ShowOkMessageBox("数据表为空！"); return; }
            NumeralOperationType NumeralOperationType;
            var result = Geo.Utils.FormUtil.OpenFormSelectOne<NumeralOperationType>(out NumeralOperationType);
            if (!result) { return; }

            var colName = Geo.Utils.FormUtil.OpenFormSelectOne(TableObjectStorage.ParamNames);
            if (String.IsNullOrWhiteSpace(colName)) return;

            TableObjectStorage.UpdateAllByOperateCol(colName, NumeralOperationType);
            this.DataBind(TableObjectStorage);
        }
        private void 设置索引列KToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (TableObjectStorage == null) { Geo.Utils.FormUtil.ShowOkMessageBox("数据表为空！"); return; }

            var colName = Geo.Utils.FormUtil.OpenFormSelectOne(TableObjectStorage.ParamNames);
            if (String.IsNullOrWhiteSpace(colName)) return;

            TableObjectStorage.IndexColName = colName;
            this.DataBind(TableObjectStorage);
        }

        private void 数值条件搜索VToolStripMenuItem_Click(object sender, EventArgs e)
        {
            NumeralSearchForm form = new NumeralSearchForm();
            if (form.ShowDialog() == DialogResult.OK)
            {
                var condition = form.ConnectedCondition;
                var newTable = TableObjectStorage.Search(condition, true);

                TableObjectViewForm openForm = new TableObjectViewForm(newTable);
                openForm.Show();
                int ii = 0;
            }
        }

        private void 所有列均值对齐AToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (TableObjectStorage == null || TableObjectStorage.RowCount == 0) { Geo.Utils.FormUtil.ShowOkMessageBox("数据表为空！"); return; }

            ObjectTableStorage newTable2 = TableObjectStorage.GetTableAllColMinusFirstValid();
            var newTable = ObjectTableUtil.GetAlignedTable(newTable2);
            this.DataBind(newTable);

            //Geo.Winform.TableObjectViewForm form = new Geo.Winform.TableObjectViewForm(newTable);
            //form.Show();
        }
        #endregion

        #region 绘图

        private void 绘图DToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                //var tables = dataGridView1.DataSource as DataTable;
                //if (tables == null) { return; }
                if (TableObjectStorage == null) { Geo.Utils.FormUtil.ShowOkMessageBox("数据表为空！"); return; }
                var chartForm = new Geo.Winform.CommonChartForm(TableObjectStorage, System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Point);
                chartForm.Text = "" + TableObjectStorage.Name;
                chartForm.Show();
            }
            catch (Exception ex)
            {
                Geo.Utils.FormUtil.ShowErrorMessageBox(ex.Message);
            }
        }

        private void 绘制选择行SToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var indexName = this.TableObjectStorage.GetIndexColName(); ;
            Geo.Utils.DataGridViewUtil.SelectColsAndDraw(dataGridView1, indexName, TableObjectStorage.Name);
        }

        private void 设置绘图选项后绘图SToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (TableObjectStorage == null) return;
            DrawObjectOptionForm form = new DrawObjectOptionForm(this.TableObjectStorage);
            form.Show();
        }
        private void 快速频率分析后绘图AToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (TableObjectStorage == null) { Geo.Utils.FormUtil.ShowOkMessageBox("数据表为空！"); return; }

            var colName = Geo.Utils.FormUtil.OpenFormSelectOne(TableObjectStorage.ParamNames);
            if (String.IsNullOrWhiteSpace(colName)) return;
            var vector = TableObjectStorage.GetVector(colName, 0, int.MaxValue, true);

            var valCount = 10;
            var rmsedVal = Geo.Utils.DoubleUtil.GetAverageWithRms(vector);
            Dictionary<double, int> frequence = Geo.Utils.DoubleUtil.GetDataFrequence(vector, valCount);

            ShowFrequencyTableAndChart(colName, rmsedVal, frequence);
        }

        private void 指定起始和步长频率绘图IToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (TableObjectStorage == null) { Geo.Utils.FormUtil.ShowOkMessageBox("数据表为空！"); return; }

            var colName = Geo.Utils.FormUtil.OpenFormSelectOne(TableObjectStorage.ParamNames);
            if (String.IsNullOrWhiteSpace(colName)) return;
            var vector = TableObjectStorage.GetVector(colName, 0, int.MaxValue, true);

            var rmsedVal = Geo.Utils.DoubleUtil.GetAverageWithRms(vector);
            //    999 
            NumerialPair numerialPair = null;
            var max = vector[vector.Count - 1];
            var minVal = Geo.Utils.DoubleUtil.Min(vector);
            var maxVal = Geo.Utils.DoubleUtil.Max(vector);
            var len = (maxVal - minVal) / 10.0;
            if (Geo.Utils.FormUtil.ShowTwoValueInputForm(new StringPair("起始数据：", "步 长："), out numerialPair, new NumerialPair(minVal, len)))
            {
                Dictionary<double, int> frequence = Geo.Utils.DoubleUtil.GetDataFrequence(vector, numerialPair.First, numerialPair.Second);
                ShowFrequencyTableAndChart(colName, rmsedVal, frequence);
            }
        }
        #region 频率绘图细节
        private void ShowFrequencyTableAndChart(string colName, RmsedNumeral rmsedVal, Dictionary<double, int> frequence)
        {
            BuildAndShowFrequencyTable(rmsedVal, frequence);
            BuildAndShowFrequenceChart(colName, rmsedVal, frequence);
        }

        private static void BuildAndShowFrequenceChart(string colName, RmsedNumeral rmsedVal, Dictionary<double, int> frequence)
        {
            //绘图
            Dictionary<string, Series> dic = new Dictionary<string, Series>();
            var colunmSeries = new Series("Count");
            var NormalDistribution = new Series("NormalDistribution");
            dic[colunmSeries.Name] = colunmSeries;
            dic[NormalDistribution.Name] = NormalDistribution;
            colunmSeries.ChartType = SeriesChartType.Column;
            NormalDistribution.ChartType = SeriesChartType.Spline;
            NormalDistribution.YAxisType = AxisType.Secondary;

            foreach (var kv in frequence)
            {
                var normalDis = Geo.Utils.MathUtil.GetNormalDistributionValue(kv.Key, rmsedVal.Value, rmsedVal.Rms);

                NormalDistribution.Points.Add(new DataPoint(kv.Key, normalDis));
                colunmSeries.Points.Add(new DataPoint(kv.Key, kv.Value));
            }
            List<Series> list = new List<Series>(dic.Values);
            Geo.Winform.CommonChartForm form2 = new Geo.Winform.CommonChartForm(list.ToArray());
            form2.Chart.ChartAreas[0].AxisY.Minimum = 0;
            form2.Chart.ChartAreas[0].AxisY2.Minimum = 0;
            form2.Text = "Frequency distribution of " + colName 
                + " Total " + frequence.Count
                + ", u=" + rmsedVal.Value.ToString("G5")
                + ", δ=" + rmsedVal.Rms.ToString("G5")
                ;
            form2.Show();
        }

        private void BuildAndShowFrequencyTable(RmsedNumeral rmsedVal, Dictionary<double, int> frequence)
        {
            var name = "频率-" + frequence.Count + "-" + TableObjectStorage.Name
                + ", u=" + rmsedVal.Value.ToString("G5")
                + ", δ=" + rmsedVal.Rms.ToString("G5");
            ObjectTableStorage table = new ObjectTableStorage(name);
            foreach (var kv in frequence)
            {
                var normalDis = Geo.Utils.MathUtil.GetNormalDistributionValue(kv.Key, rmsedVal.Value, rmsedVal.Rms);

                table.NewRow();
                table.AddItem("Value", kv.Key);
                table.AddItem("Count", kv.Value);
                table.AddItem("NormalDistribution", normalDis);
            }
            TableObjectViewForm form = new TableObjectViewForm(table);
            form.Show();
        }
        #endregion
        #endregion

        #region 查看去粗差数据
        private void 查看拟合去粗差先设置再查看ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (TableObjectStorage == null || TableObjectStorage.RowCount == 0) { Geo.Utils.FormUtil.ShowOkMessageBox("数据表为空！"); return; }

            NumerialPair numerialPair = null;
            if (Geo.Utils.FormUtil.ShowTwoValueInputForm(new StringPair("窗口数量(数字)：", "误差倍数(数字)："), out numerialPair, new NumerialPair(6, 8)))
            {
                var table = ObjectTableUtil.GetTableOfNoFitGrossError(TableObjectStorage, (int)numerialPair.First, numerialPair.Second);
                TableObjectViewForm form = new TableObjectViewForm(table);
                form.Show();
            }
        }

        private void 获取去粗差数据SToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ShowNoFitErrorTalbeForm(8);
        }


        private void 查看去粗差拟合2倍中误差SToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ShowNoFitErrorTalbeForm(2);
        }

        private void 查看去粗差拟合5倍中误差SToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ShowNoFitErrorTalbeForm(5);
        }
        private void 查看拟合去粗差10倍中误差SToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ShowNoFitErrorTalbeForm(10);
        }

        private void 查看拟合去粗差12倍中误差SToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ShowNoFitErrorTalbeForm(12);

        }

        private void ShowNoFitErrorTalbeForm(int errorTimes, int window = 6)
        {
            if (TableObjectStorage == null || TableObjectStorage.RowCount == 0) { Geo.Utils.FormUtil.ShowOkMessageBox("数据表为空！"); return; }
            var table = ObjectTableUtil.GetTableOfNoFitGrossError(TableObjectStorage, window, errorTimes);
            TableObjectViewForm form = new TableObjectViewForm(table);
            form.Show();
        }

        private void 查看去粗差平均3倍中误差SToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ShowAveErrorFiledTableForm(3);
        }

        private void 查看去粗差平均2倍中误差SToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ShowAveErrorFiledTableForm(2);
        }
        private void 查看平均去粗差5倍中误差SToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ShowAveErrorFiledTableForm(5);
        }


        private void 查看平均去粗差先设置SToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (TableObjectStorage == null || TableObjectStorage.RowCount == 0) { Geo.Utils.FormUtil.ShowOkMessageBox("数据表为空！"); return; }

            NumerialPair numerialPair = null;
            if (Geo.Utils.FormUtil.ShowTwoValueInputForm(new StringPair("窗口数量：", "误差倍数："), out numerialPair, new NumerialPair(6, 3)))
            {
                var table = ObjectTableUtil.GetTableOfNoAveGrossError(TableObjectStorage, (int)numerialPair.First, numerialPair.Second);
                TableObjectViewForm form = new TableObjectViewForm(table);
                form.Show();
            }
        }

        private void 查看平均去粗差10倍中误差SToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ShowAveErrorFiledTableForm(10);
        }

        private void ShowAveErrorFiledTableForm(int errorTimes, int window = 6)
        {
            if (TableObjectStorage == null || TableObjectStorage.RowCount == 0) { Geo.Utils.FormUtil.ShowOkMessageBox("数据表为空！"); return; }
            var table = ObjectTableUtil.GetTableOfNoAveGrossError(TableObjectStorage, window, errorTimes);
            TableObjectViewForm form = new TableObjectViewForm(table);
            form.Show();
        }
        #endregion

        #region 多项式拟合
        private void 拟合求偏差FToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (TableObjectStorage == null || TableObjectStorage.RowCount == 0) { Geo.Utils.FormUtil.ShowOkMessageBox("数据表为空！"); return; }

            NumerialPair numerialPair = null;
            if (Geo.Utils.FormUtil.ShowTwoValueInputForm(new StringPair("窗口数量(数字)：", "拟合次数(数字)："), out numerialPair, new NumerialPair(12, 2)))
            {
                var table = ObjectTableUtil.GetTableOfFitError(TableObjectStorage, (int)numerialPair.First, (int)numerialPair.Second);
                TableObjectViewForm form = new TableObjectViewForm(table);
                form.Show();
            }
        }

        private void 整列多次拟合对比MToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (TableObjectStorage == null || TableObjectStorage.RowCount == 0) { Geo.Utils.FormUtil.ShowOkMessageBox("数据表为空！"); return; }

            StringPair numerialPair = null;
            if (Geo.Utils.FormUtil.ShowTwoStringInputForm(new StringPair("最大断裂数(数字)：", "拟合次数(数组)："), out numerialPair, new StringPair("5", "1,2")))
            {
                var list = Geo.Utils.ListUtil.ParseToInts(numerialPair.Second);
                List<FitDataValueType> showFitDataValueTypes;
                if (!Geo.Utils.FormUtil.ShowAndSelectEnumsForm(out showFitDataValueTypes, new List<FitDataValueType>() { FitDataValueType.Result })) { return; }
                var table = ObjectTableUtil.GetPolyfitColTableWithDiffOrders(TableObjectStorage, list, showFitDataValueTypes, int.Parse(numerialPair.First));
                TableObjectViewForm form = new TableObjectViewForm(table);
                form.Show();
            }
        }

        private void 重叠分段滑动拟合SToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (TableObjectStorage == null || TableObjectStorage.RowCount == 0) { Geo.Utils.FormUtil.ShowOkMessageBox("数据表为空！"); return; }

            StringPair numerialPair = null;
            if (Geo.Utils.FormUtil.ShowTwoStringInputForm(new StringPair("窗口数量(数字)：", "拟合次数(数组)："), out numerialPair, new StringPair("20", "1,2")))
            {
                List<FitDataValueType> showFitDataValueTypes;
                if (!Geo.Utils.FormUtil.ShowAndSelectEnumsForm(out showFitDataValueTypes, new List<FitDataValueType>() { FitDataValueType.Result })) { return; }

                var list = Geo.Utils.ListUtil.ParseToInts(numerialPair.Second);
                var table = ObjectTableUtil.GetPolyFittedTable(TableObjectStorage, list, int.Parse(numerialPair.First), PolyfitType.OverlapedWindow, showFitDataValueTypes);
                TableObjectViewForm form = new TableObjectViewForm(table);
                form.Show();
            }
        }

        private void 常规分段拟合DToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (TableObjectStorage == null || TableObjectStorage.RowCount == 0) { Geo.Utils.FormUtil.ShowOkMessageBox("数据表为空！"); return; }

            StringPair numerialPair = null;
            if (Geo.Utils.FormUtil.ShowTwoStringInputForm(new StringPair("窗口数量(数字)：", "拟合次数(数组)："), out numerialPair, new StringPair("20", "1,2")))
            {
                var list = Geo.Utils.ListUtil.ParseToInts(numerialPair.Second);

                List<FitDataValueType> showFitDataValueTypes;
                if (!Geo.Utils.FormUtil.ShowAndSelectEnumsForm(out showFitDataValueTypes, new List<FitDataValueType>() { FitDataValueType.Result })) { return; }

                var table = ObjectTableUtil.GetPolyFittedTable(TableObjectStorage, list, int.Parse(numerialPair.First), PolyfitType.IndependentWindow, showFitDataValueTypes);
                TableObjectViewForm form = new TableObjectViewForm(table);
                form.Show();
            }

        }
        private void 滑动窗口多次拟合对比WToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (TableObjectStorage == null || TableObjectStorage.RowCount == 0) { Geo.Utils.FormUtil.ShowOkMessageBox("数据表为空！"); return; }

            StringPair numerialPair = null;
            if (Geo.Utils.FormUtil.ShowTwoStringInputForm(new StringPair("窗口数量(数字)：", "拟合次数(数组)："), out numerialPair, new StringPair("20", "1,2")))
            {
                var list = Geo.Utils.ListUtil.ParseToInts(numerialPair.Second);
                List<FitDataValueType> showFitDataValueTypes;
                if (!Geo.Utils.FormUtil.ShowAndSelectEnumsForm(out showFitDataValueTypes, new List<FitDataValueType>() { FitDataValueType.Result })) { return; }

                var table = ObjectTableUtil.GetPolyFittedTableWithMoveWindowAndDifferOrders(TableObjectStorage, list, int.Parse(numerialPair.First), showFitDataValueTypes);
                TableObjectViewForm form = new TableObjectViewForm(table);
                form.Show();
            }
        }
        private void 滑动窗口拟合数值列FToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (TableObjectStorage == null || TableObjectStorage.RowCount == 0) { Geo.Utils.FormUtil.ShowOkMessageBox("数据表为空！"); return; }

            NumerialPair numerialPair = null;
            if (Geo.Utils.FormUtil.ShowTwoValueInputForm(new StringPair("窗口数量(数字)：", "拟合次数(数字)："), out numerialPair, new NumerialPair(12, 2)))
            {
                List<FitDataValueType> showFitDataValueTypes;
                if (!Geo.Utils.FormUtil.ShowAndSelectEnumsForm(out showFitDataValueTypes, new List<FitDataValueType>() { FitDataValueType.Result })) { return; }

                var table = ObjectTableUtil.GetPolyFittedTableWithMoveWindow(TableObjectStorage, showFitDataValueTypes, (int)numerialPair.First, (int)numerialPair.Second);
                TableObjectViewForm form = new TableObjectViewForm(table);
                form.Show();
            }
        }
        private void 整列拟合WToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (TableObjectStorage == null || TableObjectStorage.RowCount == 0) { Geo.Utils.FormUtil.ShowOkMessageBox("数据表为空！"); return; }

            double times = 2;
            if (Geo.Utils.FormUtil.ShowInputNumeralForm("拟合次数(数字)：", out times, 2))
            {
                List<FitDataValueType> showFitDataValueTypes;
                if (!Geo.Utils.FormUtil.ShowAndSelectEnumsForm(out showFitDataValueTypes, new List<FitDataValueType>() { FitDataValueType.Result })) { return; }

                ObjectTableStorage newTable = ObjectTableUtil.GetPolyfitColTable(TableObjectStorage, (int)times, showFitDataValueTypes);

                TableObjectViewForm form = new TableObjectViewForm(newTable);
                form.Show();
            }
        }


        private void 多项式拟合精度评估AToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (TableObjectStorage == null) { Geo.Utils.FormUtil.ShowOkMessageBox("数据表为空！"); return; }

            var itemPair = Geo.Utils.FormUtil.OpenFormSelectPair(TableObjectStorage.ParamNames, TableObjectStorage.ParamNames, "待拟合列", "选择真值列(无则选拟合列)");
            if (itemPair == null) { return; }

            var colName = itemPair.First;
            var TrueColName = itemPair.Second;
            PolyfitType polyType = PolyfitType.MovingWindow;

            Geo.Utils.FormUtil.OpenFormSelectOne<PolyfitType>(out polyType);

            StringPair numerialPair = null;
            if (Geo.Utils.FormUtil.ShowTwoStringInputForm(new StringPair("窗口数量(数组)：", "拟合次数(数组)："), out numerialPair, new StringPair("20,40", "1,2")))
            {
                var fitCounts = Geo.Utils.ListUtil.ParseToInts(numerialPair.First);
                var orders = Geo.Utils.ListUtil.ParseToInts(numerialPair.Second);

                if (polyType == PolyfitType.WholeData)
                {
                    fitCounts = new List<int>() { TableObjectStorage.RowCount };
                }
                var table = ObjectTableUtil.PolyfitAccuracyEvaluation(TableObjectStorage, colName, fitCounts, polyType, orders, TrueColName);
                TableObjectViewForm form = new TableObjectViewForm(table);
                form.Show();
            }
        }

        private void 滑动窗口拟合指定列DToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (TableObjectStorage == null || TableObjectStorage.RowCount == 0) { Geo.Utils.FormUtil.ShowOkMessageBox("数据表为空！"); return; }

            var names = Geo.Utils.FormUtil.OpenFormSelectTitles(TableObjectStorage.ParamNames, false);
            if (names.Count < 1) return;

            NumerialPair numerialPair = null;
            if (Geo.Utils.FormUtil.ShowTwoValueInputForm(new StringPair("窗口数量(数字)：", "拟合次数(数组)："), out numerialPair, new NumerialPair(12, 2)))
            {
                List<FitDataValueType> showFitDataValueTypes;
                if (!Geo.Utils.FormUtil.ShowAndSelectEnumsForm(out showFitDataValueTypes, new List<FitDataValueType>() { FitDataValueType.Result })) { return; }
                var table = ObjectTableUtil.GetPolyFittedTableWithMoveWindow(TableObjectStorage, showFitDataValueTypes, (int)numerialPair.First, (int)numerialPair.Second, names);
                TableObjectViewForm form = new TableObjectViewForm(table);
                form.Show();
            }
        }
        private void 滑动开窗加权平均LToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (TableObjectStorage == null || TableObjectStorage.RowCount == 0) { Geo.Utils.FormUtil.ShowOkMessageBox("数据表为空！"); return; }


            NumerialPair numerialPair = null;
            if (Geo.Utils.FormUtil.ShowTwoValueInputForm(new StringPair("窗口数量：", "缓存(小于0则为窗口一半)："), out numerialPair, new NumerialPair(60, 30)))
            {
                List<FitDataValueType> showFitDataValueTypes;
                if (!Geo.Utils.FormUtil.ShowAndSelectEnumsForm(out showFitDataValueTypes, new List<FitDataValueType>() { FitDataValueType.Result })) { return; }
                var table = ObjectTableUtil.GetAdaptiveLinearFitTable(TableObjectStorage, (int)numerialPair.First, showFitDataValueTypes, (int)numerialPair.Second, null, 5);
                TableObjectViewForm form = new TableObjectViewForm(table);
                form.Show();
            }
        }
        #endregion

        #region 统计分析

        private void 求列平均AToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (TableObjectStorage == null) { Geo.Utils.FormUtil.ShowOkMessageBox("数据表为空！"); return; }

            var table = TableObjectStorage.GetAbsAveragesWithStdDevTable();
            TableObjectViewForm form = new TableObjectViewForm(table);
            form.Show();
        }

        private void 求列平均VToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (TableObjectStorage == null) { Geo.Utils.FormUtil.ShowOkMessageBox("数据表为空！"); return; }

            var table = TableObjectStorage.GetAveragesWithStdDevTable();
            TableObjectViewForm form = new TableObjectViewForm(table);
            form.Show();
        }

        private void 数据表统计信息SToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (TableObjectStorage == null || TableObjectStorage.RowCount == 0) { Geo.Utils.FormUtil.ShowOkMessageBox("数据表为空！"); return; }

            TableObjectViewForm form = new TableObjectViewForm(ObjectTableUtil.GetStatisticsTable(this.TableObjectStorage));
            form.Show();
        }

        private void 求有效数平均值AToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (TableObjectStorage == null || TableObjectStorage.RowCount == 0) { Geo.Utils.FormUtil.ShowOkMessageBox("数据表为空！"); return; }

            var aves = TableObjectStorage.GetAverageWithRmse();
            ObjectTableStorage table = new ObjectTableStorage(TableObjectStorage.Name + "_平均数与中误差");
            table.NewRow();
            table.AddItem("Average", aves[0]);
            table.AddItem("RMSE", aves[1]);
            table.AddItem("Count", aves[2]);

            TableObjectViewForm form = new TableObjectViewForm(table);
            form.Show();
        }
        #endregion

        #region 误差剔除

        private void 去除3倍中误差后求有效数平均值QToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (TableObjectStorage == null || TableObjectStorage.RowCount == 0) { Geo.Utils.FormUtil.ShowOkMessageBox("数据表为空！"); return; }

            var aves = TableObjectStorage.GetAverageWithRmse();

            var newTable = TableObjectStorage.RemoveCellWith(val => Math.Abs(val - aves[0]) > 3 * aves[1]);

            TableObjectViewForm form = new TableObjectViewForm(newTable);
            form.Show();

        }

        private void 去除指定倍数中误差求平均值IToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (TableObjectStorage == null || TableObjectStorage.RowCount == 0) { Geo.Utils.FormUtil.ShowOkMessageBox("数据表为空！"); return; }

            double times = 3;
            if (Geo.Utils.FormUtil.ShowInputNumeralForm("倍数：", out times, 3))
            {
                var aves = TableObjectStorage.GetAverageWithRmse();

                var newTable = TableObjectStorage.RemoveCellWith(val => Math.Abs(val - aves[0]) > times * aves[1]);

                TableObjectViewForm form = new TableObjectViewForm(newTable);
                form.Show();
            }
        }
        #endregion

        #region 新增
        private void 新窗口打开NToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (TableObjectStorage == null || TableObjectStorage.RowCount == 0) { Geo.Utils.FormUtil.ShowOkMessageBox("数据表为空！"); return; }

            TableObjectViewForm form = new TableObjectViewForm(this.TableObjectStorage);
            form.Show();
        }
        private void 新增白噪声列NToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (TableObjectStorage == null) { Geo.Utils.FormUtil.ShowOkMessageBox("数据表为空！"); return; }

            TableObjectStorage.AddNewWhiteNoiseCol();
            this.DataBind(TableObjectStorage);
        }
        #endregion

        #region 另存为

        private void word导出ToolStripMenuItem_Click(object sender, EventArgs e) { ReportUtil.SaveToWord(this.dataGridView1); }

        private void excel导出ToolStripMenuItem_Click(object sender, EventArgs e) { ReportUtil.SaveToExcel(this.dataGridView1); }

        private void 存为文本文件txtxlsSToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.SaveAs();
        }
        internal void SaveAs() { SaveAs(TableObjectStorage); }
        /// <summary>
        /// 另存为
        /// </summary>
        /// <param name="TableObjectStorage"></param>
        public static void SaveAs(ObjectTableStorage TableObjectStorage)
        {
            if (TableObjectStorage == null) { Geo.Utils.FormUtil.ShowOkMessageBox("数据表为空！"); return; }
            SaveFileDialog dlg = new SaveFileDialog();
            dlg.Filter = Setting.TextTableFileFilter;
            dlg.FileName = TableObjectStorage.Name;
            if (dlg.ShowDialog() == DialogResult.OK)
            {
                ObjectTableWriter writer = new ObjectTableWriter(dlg.FileName, Encoding.Default);
                writer.Write(TableObjectStorage);
                Geo.Utils.FormUtil.ShowOkAndOpenDirectory(System.IO.Path.GetDirectoryName(dlg.FileName));
            }
        }
        #endregion

        private void 计算中误差标准差的估值RToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (TableObjectStorage == null) { Geo.Utils.FormUtil.ShowOkMessageBox("数据表为空！"); return; }

            var itemPair = Geo.Utils.FormUtil.OpenFormSelectPair(TableObjectStorage.ParamNames, TableObjectStorage.ParamNames, "计算列(观测值、估值等)", "真值列");
            if (itemPair == null) { return; }

            var colName = itemPair.First;
            var TrueColName = itemPair.Second;

            //下面进行精度计算
            var storage = TableObjectStorage.Clone();
            storage.UpdateAllByMinusCol(TrueColName);

            var table = ObjectTableUtil.GetResidualRmseTable(storage);
            TableObjectViewForm form = new TableObjectViewForm(table);
            form.Show();

        }

        private void 计算所有列作为残差的中误差AToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (TableObjectStorage == null) { Geo.Utils.FormUtil.ShowOkMessageBox("数据表为空！"); return; }

            var table = ObjectTableUtil.GetResidualRmseTable(TableObjectStorage);
            TableObjectViewForm form = new TableObjectViewForm(table);
            form.Show();

        }

        private void 重叠窗口拟合指定列OToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (TableObjectStorage == null || TableObjectStorage.RowCount == 0) { Geo.Utils.FormUtil.ShowOkMessageBox("数据表为空！"); return; }

            var names = Geo.Utils.FormUtil.OpenFormSelectTitles(TableObjectStorage.ParamNames, false);
            if (names.Count < 1) return;

            NumerialPair numerialPair = null;
            if (Geo.Utils.FormUtil.ShowTwoValueInputForm(new StringPair("窗口数量(数字)：", "拟合次数："), out numerialPair, new NumerialPair(60, 1)))
            {
                List<FitDataValueType> showFitDataValueTypes;
                if (!Geo.Utils.FormUtil.ShowAndSelectEnumsForm(out showFitDataValueTypes, new List<FitDataValueType>() { FitDataValueType.Result })) { return; }

                var table = ObjectTableUtil.GetPolyFittedTable(TableObjectStorage,
                    new List<int>() { (int)numerialPair.Second },//fit count
                    (int)numerialPair.First,//order 
                     PolyfitType.OverlapedWindow,
                     showFitDataValueTypes,
                     5, names);
                TableObjectViewForm form = new TableObjectViewForm(table);
                form.Show();
            }

        }

        private void 输入行数据操作RToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (TableObjectStorage == null) { Geo.Utils.FormUtil.ShowOkMessageBox("数据表为空！"); return; }

            try
            {
                string txt;
                if (!Geo.Utils.FormUtil.ShowTextInputForm(out txt, "Prn Value\r\nG01 2000\r\n G02  2000", "第一行为列名称,一行对应一行，第一列为索引名称"))
                {
                    return;
                }

                var lines = txt.Split(new char[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries);

                var form = new OperationTypeSelectForm();
                if (form.ShowDialog() != System.Windows.Forms.DialogResult.OK) return;

                var title = lines[0].Split(new char[] { ' ', '\t', ',', ';', ':' }, StringSplitOptions.RemoveEmptyEntries);
                string valName = title[1];
                int i = -1;
                foreach (var line in lines)
                {
                    i++;
                    if (i == 0) { continue; }

                    var strs = line.Split(new char[] { ' ', '\t', ',', ';', ':' }, StringSplitOptions.RemoveEmptyEntries);
                    if (strs.Length < 2) continue;
                    var index = strs[0];
                    var val = Geo.Utils.DoubleUtil.TryParse(strs[1]);

                    var row = this.TableObjectStorage.GetRow(index);
                    if (row == null || !row.ContainsKey(valName)) { continue; }
                    var numeralOperationType = form.NumeralOperationType;
                    var obj = row[valName];
                    if (!Geo.Utils.ObjectUtil.IsNumerial(obj)) { continue; }

                    var old = Geo.Utils.ObjectUtil.GetNumeral(obj);

                    switch (numeralOperationType)
                    {
                        case NumeralOperationType.加:
                            row[valName] = old + val;
                            break;
                        case NumeralOperationType.减:
                            row[valName] = old - val;
                            break;
                        case NumeralOperationType.乘:
                            row[valName] = old * val;
                            break;
                        case NumeralOperationType.除:
                            row[valName] = old / val;
                            break;
                        default: break;
                    }
                }
                this.DataBind(TableObjectStorage);
            }
            catch (Exception ex)
            {
                Geo.Utils.FormUtil.ShowErrorMessageBox("糟糕！出错了： " + ex.Message);
            }
        }
        private void 输入列数据操作LToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (TableObjectStorage == null) { Geo.Utils.FormUtil.ShowOkMessageBox("数据表为空！"); return; }

            try
            {
                string txt;
                if (!Geo.Utils.FormUtil.ShowTextInputForm(out txt, "G01 2000\r\n G02  2000", "一行对应一列，第一位名称"))
                {
                    return;
                }

                var lines = txt.Split(new char[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries);

                var form = new OperationTypeSelectForm();
                if (form.ShowDialog() != System.Windows.Forms.DialogResult.OK) return;

                foreach (var line in lines)
                {
                    var strs = line.Split(new char[] { ' ', '\t', ',', ';', ':' }, StringSplitOptions.RemoveEmptyEntries);
                    if (strs.Length < 2) continue;
                    var title = strs[0];
                    var val = Geo.Utils.DoubleUtil.TryParse(strs[1]);

                    if (!TableObjectStorage.ParamNames.Contains(title)) { continue; }

                    this.TableObjectStorage.UpdateColumnBy(title, val, form.NumeralOperationType);
                }
                this.DataBind(TableObjectStorage);
            }
            catch (Exception ex)
            {
                Geo.Utils.FormUtil.ShowErrorMessageBox("糟糕！出错了： " + ex.Message);
            }
        }

        private void 所有只保留正小数FToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (TableObjectStorage == null) { Geo.Utils.FormUtil.ShowOkMessageBox("数据表为空！"); return; }
            var table = TableObjectStorage.GetTable("正小数表_" + TableObjectStorage.Name, (val) => Geo.Utils.DoubleUtil.GetPositiveFraction(val));
            this.DataBind(table);
        }
        private void 所有只保留四舍五入小数RToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (TableObjectStorage == null) { Geo.Utils.FormUtil.ShowOkMessageBox("数据表为空！"); return; }
            var table = TableObjectStorage.GetTable("四舍五入小数表_" + TableObjectStorage.Name, (val) => Geo.Utils.DoubleUtil.GetRoundFraction(val));
            this.DataBind(table);

        }

        private void 所有数值四舍五入RToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (TableObjectStorage == null) { Geo.Utils.FormUtil.ShowOkMessageBox("数据表为空！"); return; }
            var table = TableObjectStorage.GetRoundTable();
            this.DataBind(table);
        }

        private void 移除所有空列CToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (TableObjectStorage == null) { Geo.Utils.FormUtil.ShowOkMessageBox("数据表为空！"); return; }

            double numerialPair;
            if (Geo.Utils.FormUtil.ShowInputNumeralForm("与所有行的最小比例：", out numerialPair, 0.95))
            {
                TableObjectStorage.RemoveAllEmptyCols(numerialPair);
                this.DataBind(TableObjectStorage);
            }

        }
        private void 移除所有空行NToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (TableObjectStorage == null) { Geo.Utils.FormUtil.ShowOkMessageBox("数据表为空！"); return; }
            TableObjectStorage.RemoveAllEmptyRows();
            this.DataBind(TableObjectStorage);
        }

        private void 各列小数归算到一个区间AToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (TableObjectStorage == null) { Geo.Utils.FormUtil.ShowOkMessageBox("数据表为空！"); return; }
            var table = TableObjectStorage.GetPeriodPipeFilterTable(1, 0.5);
            this.TableObjectStorage = table;
            this.DataBind(TableObjectStorage);
        }

        private void 插入数字索引列IToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (TableObjectStorage == null) { Geo.Utils.FormUtil.ShowOkMessageBox("数据表为空！"); return; }
            TableObjectStorage.InsertIndexCol();
            this.DataBind(TableObjectStorage);

        }

        private void 滑动平均去粗差EToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (TableObjectStorage == null || TableObjectStorage.RowCount == 0) { Geo.Utils.FormUtil.ShowOkMessageBox("数据表为空！"); return; }

            NumerialPair numerialPair = null;
            if (Geo.Utils.FormUtil.ShowTwoValueInputForm(new StringPair("窗口数量：", "粗差倍数："), out numerialPair, new NumerialPair(100000, 3)))
            {
                var table = ObjectTableUtil.GetTableOfNoAveGrossError(TableObjectStorage, (int)numerialPair.First, numerialPair.Second);
                TableObjectViewForm form = new TableObjectViewForm(table);
                form.Show();
            }
        }

        private void 循环滑动平均去粗差LToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (TableObjectStorage == null || TableObjectStorage.RowCount == 0) { Geo.Utils.FormUtil.ShowOkMessageBox("数据表为空！"); return; }

            NumerialPair numerialPair = null;
            if (Geo.Utils.FormUtil.ShowTwoValueInputForm(new StringPair("窗口数量：", "粗差倍数："), out numerialPair, new NumerialPair(100000, 3)))
            {
                var table = ObjectTableUtil.GetTableOfNoAveGrossErrorLoopUtil(TableObjectStorage, (int)numerialPair.First, numerialPair.Second);
                TableObjectViewForm form = new TableObjectViewForm(table);
                form.Show();
            }

        }
        private void 移除各列各段前X行RToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (TableObjectStorage == null || TableObjectStorage.RowCount == 0) { Geo.Utils.FormUtil.ShowOkMessageBox("数据表为空！"); return; }
            NumerialPair numerialPair = null;
            if (Geo.Utils.FormUtil.ShowTwoValueInputForm(new StringPair("移除行数：", "分段内允许断裂数："), out numerialPair, new NumerialPair(10, 2)))
            {
                TableObjectStorage.RemoveStartRowOfEachSegment((int)numerialPair.First, (int)numerialPair.Second);
                this.DataBind(TableObjectStorage);
            }
        }

        private void 各列分段求平均SToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (TableObjectStorage == null || TableObjectStorage.RowCount == 0) { Geo.Utils.FormUtil.ShowOkMessageBox("数据表为空！"); return; }
            NumerialPair numerialPair = null;
            if (Geo.Utils.FormUtil.ShowTwoValueInputForm(new StringPair("忽略少于数量：", "分段内允许断裂数："), out numerialPair, new NumerialPair(10, 2)))
            {
                var table = ObjectTableUtil.GetAverageTableOfEachSegments(TableObjectStorage, (int)numerialPair.First, (int)numerialPair.Second);
                TableObjectViewForm form = new TableObjectViewForm(table);
                form.Show();
            }
        }
        private void 各列分段求平均详表SToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (TableObjectStorage == null || TableObjectStorage.RowCount == 0) { Geo.Utils.FormUtil.ShowOkMessageBox("数据表为空！"); return; }
            NumerialTriple numerialPair = null;
            if (Geo.Utils.FormUtil.ShowThreeValueInputForm(new StringTriple("忽略少于数量：", "分段内允许断裂数：","最大允许跳跃："), out numerialPair, new NumerialTriple(10, 2, 2)))
            {
                var table = ObjectTableUtil.GetGroupedAverageTable(TableObjectStorage, (int)numerialPair.First, (int)numerialPair.Second, (double)numerialPair.Third);
                TableObjectViewForm form = new TableObjectViewForm(table);
                form.Show();
            }
        }

        private void 求行平均RToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (TableObjectStorage == null) { Geo.Utils.FormUtil.ShowOkMessageBox("数据表为空！"); return; }

            var table = TableObjectStorage.GetRowAveragesWithStdDevTable();
            TableObjectViewForm form = new TableObjectViewForm(table);
            form.Show();
        }

        private void 提取值表适用加权数字VToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (TableObjectStorage == null) { Geo.Utils.FormUtil.ShowOkMessageBox("数据表为空！"); return; }

            var table = TableObjectStorage.GetValueTable();
            TableObjectViewForm form = new TableObjectViewForm(table);
            form.Show();

        }

        private void 提取RMS表使用加权数字RToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (TableObjectStorage == null) { Geo.Utils.FormUtil.ShowOkMessageBox("数据表为空！"); return; }

            var table = TableObjectStorage.GetRmsValueTable();
            TableObjectViewForm form = new TableObjectViewForm(table);
            form.Show();

        }

        private void 行抗差加权平均KToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (TableObjectStorage == null) { Geo.Utils.FormUtil.ShowOkMessageBox("数据表为空！"); return; }

            var table = TableObjectStorage.GetRowWeightedAveragesWithStdDevTable(3);
            TableObjectViewForm form = new TableObjectViewForm(table);
            form.Show();
        }

        private void 行平均后序贯平差AToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (TableObjectStorage == null) { Geo.Utils.FormUtil.ShowOkMessageBox("数据表为空！"); return; }

            var table = TableObjectStorage.GetRowAveragesAndSquentialAdjustTable(3);
            TableObjectViewForm form = new TableObjectViewForm(table);
            form.Show();
        }

        private void 匹配指定列CToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (TableObjectStorage == null) { Geo.Utils.FormUtil.ShowOkMessageBox("数据表为空！"); return; }

            var colName = Geo.Utils.FormUtil.OpenFormSelectOne(TableObjectStorage.ParamNames);
            if (String.IsNullOrWhiteSpace(colName)) return;

            StringConditionOperator oper = StringConditionOperator.Contains;
            if (!Geo.Utils.FormUtil.ShowAndSelectEnumRadioForm<StringConditionOperator>(out oper, oper)) { return; }

            string [] referVal = null;
            if (!Geo.Utils.FormUtil.ShowInputLineForm("请以行输入",out referVal)) { return; }


            ConnectedStrCondition connectedStrCondition = null;
            foreach (var item in referVal)
            {
                if(connectedStrCondition == null)
                {
                    connectedStrCondition = new ConnectedStrCondition(new StringCondition(oper, item));
                    continue;
                }
                connectedStrCondition.AddConditon( new ConnectedStringCondition( ConditionConnectOperator.Or, new StringCondition(oper, item)));
            } 

            var newTable = ObjectTableStorage.FilterStringCol(TableObjectStorage, colName, connectedStrCondition, true);

            TableObjectViewForm openForm = new TableObjectViewForm(newTable);
            openForm.Show();
        }

        private void 数据频率分析FToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            if (TableObjectStorage == null) { Geo.Utils.FormUtil.ShowOkMessageBox("数据表为空！"); return; }

            NumerialPair numerialPair = null;
            if (Geo.Utils.FormUtil.ShowTwoValueInputForm(new StringPair("起始数据：", "步 长："), out numerialPair, new NumerialPair(0, 5)))
            {
                var table = TableObjectStorage.GetFrequenceTable(numerialPair.First, numerialPair.Second);
                TableObjectViewForm form = new TableObjectViewForm(table);
                form.Show();
            }
        }

        private void 提取新采样率表格EToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (TableObjectStorage == null) { Geo.Utils.FormUtil.ShowOkMessageBox("数据表为空！"); return; }

            double interval=5;
            if (Geo.Utils.FormUtil.ShowInputNumeralForm("采样率(提取间隔)：", out interval, interval))
            {
                var table = TableObjectStorage.GetNewSamplingTable((int)interval);
                TableObjectViewForm form = new TableObjectViewForm(table);
                form.Show();
            }
        }
    }
}
