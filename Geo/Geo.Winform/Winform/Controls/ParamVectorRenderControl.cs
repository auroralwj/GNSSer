//2014.12.12, czs, create, 定位结果绘图，查看残差，均方根等,提取为控件
//2016.10.04, czs, edit in hongqing, 取消与GnssResult的绑定，增加平差向量选项
//2016.10.04, czs, edit  in hongqing, 采用Storage作为数据
//2017.02.27, czs, edit in hongqing, 增加时间坐标显示
//2017.03.22，czs, edit in hongqing, 增加边界控制，多线程绘图支持
//2019.09.12, czs edit in hmx, 增加索引判断，略过无索引数据，避免崩溃

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Text;
using System.Windows.Forms;
using System.Drawing.Drawing2D;
using System.Windows.Forms.DataVisualization.Charting; 
using Gnsser; 
using Geo.Coordinates;
using Geo.Referencing;
using Geo.Algorithm.Adjust;
using Geo.Utils;
using Geo;
using Geo.Times;
using Geo.Algorithm;

namespace Geo.Winform.Controls
{
    /// <summary>
    /// 定位结果绘图，查看残差，均方根等。AdjustVectorRenderControl
    /// </summary>
    public partial class ParamVectorRenderControl : UserControl
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public ParamVectorRenderControl()
        {
            InitializeComponent();
             
            this.comboBox_chartType.DataSource = Geo.Utils.EnumUtil.GetList<SeriesChartType>();
            SeriesChartType = SeriesChartType.Point;
            this.comboBox_chartType.SelectedItem = SeriesChartType.Point;
            
        }

        #region 属性

        bool IsMaxChartWindow { get { return checkBox_isMaxWindow.Checked; } }
        int BorderWidth { get { return int.Parse(this.textBox2BorderWidth.Text); } }
        int MarkerSize { get { return int.Parse(this.textBoxMarkerSize.Text); } }
        SeriesChartType SeriesChartType { get; set; }
        /// <summary>
        /// 若X轴为时间，则设置为时间间隔
        /// </summary>
        public bool IsTakeXAsTimeIf { get { return checkBox_isTakeXAsTime.Checked; } }
        /// <summary>
        /// 待绘参数名称
        /// </summary>
        protected List<string> ParamNames { get; private set; }
        /// <summary>
        /// 数据表。
        /// </summary>
        public ObjectTableStorage TableTextStorage { get; protected set; }

        /// <summary>
        /// 绘图起始历元编号
        /// </summary>
        public int StartIndex { get { return Int32.Parse(textBox_startEpoch.Text); } }
        /// <summary>
        /// 绘图截止历元编号
        /// </summary>
        public int DataCount { get { return Int32.Parse(textBox_Count.Text); } }
        /// <summary>
        /// 绘图参数个数，默认为3个。
        /// </summary>
        public int ParamCount { get { return int.Parse(textBox_paramCount.Text); } }
        /// <summary>
        /// 起始参数编号，从0开始
        /// </summary>
        public int StartParamIndex { get { return int.Parse(textBox_startParamIndex.Text); }protected set { textBox_startParamIndex.Text = value + ""; } }
        /// <summary>
        /// 结束参数编号
        /// </summary>
        public int EndParamIndex { get { return StartParamIndex + ParamCount; } }
        /// <summary>
        /// 指定的Y值范围
        /// </summary>
        public EnableFloatSpan IndicatedYSpan { get { return enabledFloatSpanControl_YSpan.GetEnabledValue(); } }
        #endregion

        #region 绘制
        /// <summary>
        /// 设置参数名称列表
        /// </summary>
        /// <param name="TableTextStorage"></param>
        public void SetTableTextStorage(ObjectTableStorage TableTextStorage, bool updateUi = true)
        {
            this.TableTextStorage = TableTextStorage;
            var paramNames = TableTextStorage.ParamNames;
            SetParamNames(paramNames, updateUi);
        }
        /// <summary>
        /// 设置参数名称
        /// </summary>
        /// <param name="isUpdateUi"></param>
        /// <param name="paramNames"></param>
        public void SetParamNames(List<string> paramNames, bool isUpdateUi  =true)
        {
            this.ParamNames = paramNames;
            if (!isUpdateUi) { return; }

            if (this.Created)
            {
                this.Invoke(new Action(() =>
                {
                    this.arrayCheckBoxControl1.Init<string>(this.ParamNames);
                }));
            }
            else { this.arrayCheckBoxControl1.Init<string>(this.ParamNames); }
           
        }


        /// <summary>
        /// 绘图中心
        /// </summary>
        public Vector CenterVector
        {
            get
            {
                int count = GetShowingParamsNames().Count;
                Vector vector = new Vector(count);
                Vector inputVector = Vector.Parse(this.textBox_differ.Text);
                vector.SetSubVector(inputVector);
                return vector;
            }
        }
        #endregion

        string currentTabName;
        static object nameGetLocker = new object();
        /// <summary>
        /// 显示待显示的参数名称
        /// </summary>
        /// <returns></returns>
        public List<string> GetShowingParamsNames()
        {
            //  this.Invoke(new Action(()=>{ 
            var manualName = tabPage_ManualSelect.Name;
            if (String.Compare(currentTabName, manualName) == 0)
            {
                var names = this.arrayCheckBoxControl1.GetSelected<string>();
                if (names == null)
                {
                    names = ParamNames;
                }
                return names;
            }
            else
            {
                var startIndex = StartParamIndex < ParamNames.Count ? StartParamIndex : ParamNames.Count;
                var count = Math.Min(ParamCount, ParamNames.Count - startIndex);
                return ParamNames.GetRange(StartParamIndex, count);
            }//}} 
        }

        /// <summary>
        /// 绘制估计值 
        /// </summary>
        public void DrawParamLines()
        { 
            DrawTable(this.TableTextStorage);
        }
         /// <summary>
        /// 直接绘制图表，不再更新界面元素。
        /// </summary>
        /// <param name="table"></param>
        public void DrawTable(ObjectTableStorage table)
        {
         //   if(this.TableTextStorage == null)
            {
                this.SetTableTextStorage(table);
            }

            if (table == null || table.RowCount == 0) return;
            var isTakeXAsTimeIf = IsTakeXAsTimeIf;
            Vector centerVector = CenterVector; ;
            var firstType = table.GetIndexValue(0).GetType();
            var isIndexGeoTime = (firstType == typeof(Geo.Times.Time));
            //  var isIndexDateTime = (firstType == typeof(DateTime));
            bool isIndexTime = isIndexGeoTime;//|| isIndexDateTime;

            var names = GetShowingParamsNames();
            if (names.Count == 0) { Geo.Utils.FormUtil.ShowWarningMessageBox("请选中参数后再试！"); return; }
            Dictionary<string, Series> dic = new Dictionary<string, Series>();
            var span = enabledFloatSpanControl_filterVal.GetEnabledValue();

            double maxVal = double.MinValue;
            double minVal = double.MaxValue;
            if (isIndexTime && isTakeXAsTimeIf)//X轴为时间
            {
                int paramIndex = 0;
                foreach (var name in names)
                {
                    Series seriesX = new Series(name);
                    seriesX.ChartType = SeriesChartType;// SeriesChartType.Point;
                    seriesX.YValueType = ChartValueType.Double;
                    seriesX.XValueType = ChartValueType.DateTime;
                    seriesX.MarkerSize = MarkerSize;
                    seriesX.BorderWidth = BorderWidth;
                    seriesX.ToolTip = "#SERIESNAME: #VALX, #VALY, #AXISLABEL";//#LEGENDTEXT // https://msdn.microsoft.com/en-us/library/dd207017.aspx
                                                                              //seriesX.IsXValueIndexed = true;
                                                                              //seriesX.ChartType = SeriesChartType.Line;
                    dic[name] = seriesX;

                    var colValues = table.GetNumeralColDic<Time>(name, StartIndex, DataCount, true);
                    int rowIndex = 0;
                    foreach (var item in colValues)
                    {
                        var val = item.Value;

                        if (span.Enabled && !span.Value.Contains(val)) { continue; }
                        var drawVal = centerVector.Count > paramIndex ? val - centerVector[paramIndex] : val;

                        minVal = Math.Min(minVal, drawVal);
                        maxVal = Math.Max(maxVal, drawVal);

                        if (isIndexTime && isTakeXAsTimeIf)
                        {
                            var time = (Time)item.Key;
                            seriesX.Points.AddXY(time.DateTime, drawVal);
                        }
                        else //if (Geo.Utils.DoubleUtil.IsValid(key)) //自动忽略非数据//起始 
                        {
                            seriesX.Points.AddXY(rowIndex, drawVal);
                        }
                        rowIndex++;
                    }
                paramIndex++;
                }
            }
            else
            {
                var colValues = table.GetVectors(names, StartIndex, DataCount, false, Double.NaN);
                var indexes = table.GetIndexValues(true);
                foreach (var name in names)
                {
                    Series seriesX = new Series(name);
                    seriesX.ChartType = SeriesChartType;// SeriesChartType.Point;
                    seriesX.YValueType = ChartValueType.Double;
                    seriesX.XValueType = ChartValueType.Double;
                    seriesX.MarkerSize = MarkerSize;
                    seriesX.BorderWidth = BorderWidth;
                    seriesX.ToolTip = "#SERIESNAME: #VALX, #VALY, #AXISLABEL";//#LEGENDTEXT // https://msdn.microsoft.com/en-us/library/dd207017.aspx
                                                                              //seriesX.IsXValueIndexed = true;
                                                                              //seriesX.ChartType = SeriesChartType.Line;
                    dic[name] = seriesX;
                }

                int paramIndex = 0;
                foreach (var result in colValues)//避免集合修改无法遍历
                {
                    var seriesX = dic[result.Key];
                    int graphIndex = 1;//图形显示从1开始
                    int dataIndex = StartIndex - 1;
                    foreach (var val in result.Value)
                    {
                        dataIndex++;

                        if (Geo.Utils.DoubleUtil.IsValid(val))
                        {
                            if (span.Enabled && !span.Value.Contains(val)) { continue; }

                            var drawVal = centerVector.Count > paramIndex ? val - centerVector[paramIndex] : val;

                            minVal = Math.Min(minVal, drawVal);
                            maxVal = Math.Max(maxVal, drawVal);
                            if (indexes.Count > (dataIndex))
                            {
                                seriesX.Points.AddXY(indexes[dataIndex], drawVal);
                            }
                            graphIndex++;
                        }
                    }
                    paramIndex++;
                }
            }
            ShowCharForm(table, dic, maxVal, minVal);
        }

        /// <summary>
        /// 显示图片。
        /// </summary>
        /// <param name="table"></param>
        /// <param name="dic"></param>
        /// <param name="maxVal"></param>
        /// <param name="minVal"></param>
        private void ShowCharForm(ObjectTableStorage table, Dictionary<string, Series> dic, double maxVal, double minVal)
        {
            List<Series> list = new List<Series>(dic.Values);
            var form = new Geo.Winform.CommonChartForm(list.ToArray());

            SetYSpan(maxVal, minVal, form);

            form.Text = table.Name + " 列×行: " + list.Count + "×" + table.RowCount;
            if (IsMaxChartWindow) { form.WindowState = FormWindowState.Maximized; }
            form.Show();
        }

        /// <summary>
        /// 设置Y轴数值显示范围
        /// </summary>
        /// <param name="maxVal"></param>
        /// <param name="minVal"></param>
        /// <param name="form"></param>
        private void SetYSpan(double maxVal, double minVal, Geo.Winform.CommonChartForm form)
        {
            if (checkBox_autoYSpan.Checked)
            {
                return;
            }

            //最大最小边界控制
            if (IndicatedYSpan.Enabled)
            {
                form.Chart.ChartAreas[0].AxisY.Minimum = IndicatedYSpan.Value.Start;
                form.Chart.ChartAreas[0].AxisY.Maximum = IndicatedYSpan.Value.End;
            }
            var Yspan = maxVal - minVal;
            var digital = Yspan > 10 || Yspan ==0 ? 0 : (int)(Math.Log10(1.0 / Yspan)) + 1;//小数部分显示控制
            var autoMargin = 1.0 / Math.Pow(10, digital);
            if (enabledFloatControl_downMargin.GetEnabledValue().Enabled)
            {
                form.Chart.ChartAreas[0].AxisY.Minimum = Math.Round(minVal - enabledFloatControl_downMargin.Value, digital);
            }
            else
            {
                form.Chart.ChartAreas[0].AxisY.Minimum = Math.Round(minVal - autoMargin, digital);
            }

            if (this.enabledFloatControl_upMargin.GetEnabledValue().Enabled)
            {
                form.Chart.ChartAreas[0].AxisY.Maximum = Math.Round(maxVal + enabledFloatControl_upMargin.Value, digital);
            }
            else
            {
                form.Chart.ChartAreas[0].AxisY.Maximum = Math.Round(maxVal + autoMargin, digital);
            }
        }
        
        /// <summary>
        /// 设置显示的参数范围
        /// </summary>
        /// <param name="from"></param>
        /// <param name="count"></param>
        public void SetParamIndexRange(int from, int count)
        {
            this.textBox_paramCount.Text = count + "";
            this.textBox_startParamIndex.Text = from + "";
        }

        private void tabControl_Selected(object sender, TabControlEventArgs e)
        {
            currentTabName = tabControl.SelectedTab.Name;
        }

        private void comboBox_chartType_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.SeriesChartType = (SeriesChartType)comboBox_chartType.SelectedItem;
        }
    }
}