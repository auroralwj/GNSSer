//2017.09.26, czs, edit in hongqing, 增加DataTable、InstantValueStorage绘图，绘图缩放功能
//2018.08.22, czs, edit in hmx, 增加绘图选项
//2019.01.03, czs, edit in hmx, 增加可隐藏菜单选项，增加可新窗口打开功能

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.IO;
using System.Collections;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;
using Geo.IO;

namespace Geo.Winform
{
    /// <summary>
    /// 折线绘图
    /// </summary>
    public partial class CommonChartForm : Form
    {
        Log log = new Log(typeof(CommonChartForm));
        /// <summary>
        /// 默认构造函数
        /// </summary>
        public CommonChartForm()
        {
            InitializeComponent();
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="lists"></param>
        /// <param name="name"></param>
        public CommonChartForm(List<IEnumerable<Double>> lists, List<string> name)
        {
            InitializeComponent();
            Series[] ss = new Series[lists.Count];
            Int32 i = 0;
            foreach (IEnumerable<Double> item in lists)
            {
                Series s = BuildSeries(item, name[i]);

                ss[i] = s;
                i++;
            }
            this.Init(ss);
        }
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="lists"></param>
        /// <param name="name"></param>
        public CommonChartForm(List<Dictionary<Double, Double>> lists, List<string> name)
        {
            InitializeComponent();
            Series[] ss = new Series[lists.Count];
            Int32 i = 0;
            foreach (var item in lists)
            {
                Series s = BuildSeries(item, name[i]);

                ss[i] = s;
                i++;
            }
            this.Init(ss);
        }
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="lists"></param>
        /// <param name="name"></param>
        public CommonChartForm(IEnumerable<Double> lists, string name = "S")
        {
            InitializeComponent();
            Series s = BuildSeries(lists, name);
            this.Init(s);
        }
        /// <summary>
        /// 输入序列
        /// </summary>
        /// <param name="series"></param>
        public CommonChartForm(params Series[] series)
        {
            InitializeComponent();

            Init(series);
        }
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="Points"></param>
        /// <param name="name"></param>
        public CommonChartForm(List<DataPoint> Points, string name = "S") : this(Points.ToArray(), name) { }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="Points"></param>
        /// <param name="name"></param>
        public CommonChartForm(DataPoint[] Points, string name = "S")
        {
            InitializeComponent();
            this.Points = Points;

            Series series = new Series(name);
            series.ChartType = SeriesChartType.Line;
            series.YValueType = ChartValueType.Double;
            series.XValueType = ChartValueType.Double;

            foreach (var item in Points)
            {
                series.Points.Add(item);
            }

            Init(series, series);
        }

        /// <summary>
        /// 输入序列
        /// </summary>
        /// <param name="InstantValueStorage"></param>
        public CommonChartForm(InstantValueStorage InstantValueStorage)
        {
            InitializeComponent();

            Dictionary<string, Series> dic = new Dictionary<string, Series>();
            List<string> names = new List<string>(InstantValueStorage.Keys);
            names.Sort();
            foreach (var name in names)
            {
                Series seriesX = new Series(name);
                seriesX.ChartType = SeriesChartType.Point;
                seriesX.XValueType = ChartValueType.DateTime;
                //   seriesX.YValueType = ChartValueType.String;
                seriesX.MarkerSize = 10;
                seriesX.BorderWidth = 10;
                //  seriesX.MarkerBorderWidth = 4;
                dic[name] = seriesX;

                int number = Geo.Utils.IntUtil.TryGetInt(name);
                foreach (var item in InstantValueStorage[name])
                {
                    //seriesX.Points.Add(new DataPoint(key.DateTime.TimeOfDay.TotalSeconds, number));                    
                    DataPoint data = new DataPoint();
                    data.SetValueXY(item.DateTime, number);
                    seriesX.Points.Add(data);
                }
            }

            Init(dic.Values);
        }
        /// <summary>
        /// 输入序列
        /// </summary>
        /// <param name="table"></param>
        /// <param name="SeriesChartType"></param>
        public CommonChartForm(DataTable table, SeriesChartType SeriesChartType = SeriesChartType.Line)
        {
            InitializeComponent();

            if (table == null || table.Rows.Count == 0) return;
            ChartValueType XValueType = ChartValueType.Double;
            List<Object> indexes = new List<object>();
            var indexColName = "";
            var firstVal = table.Rows[0][0];
            if (firstVal is DateTime)
            {
                indexColName = table.Columns[0].ColumnName;
                foreach (DataRow row in table.Rows)
                {
                    indexes.Add((DateTime)(row[indexColName]));
                }
                XValueType = ChartValueType.DateTime;
            }
            else if (Geo.Utils.DateTimeUtil.TryParse(firstVal.ToString()) != DateTime.MinValue)
            {
                indexColName = table.Columns[0].ColumnName;
                foreach (DataRow item in table.Rows)
                {
                    indexes.Add(Geo.Utils.DateTimeUtil.TryParse(item[indexColName]));
                }
                XValueType = ChartValueType.DateTime;
            }
            else
            {
                indexColName = "编号ID";
                int i = 1;
                foreach (var item in table.Rows)
                {
                    indexes.Add(i++);
                }
            }

            // var prevObj = indexes[0];
            //   bool isIndexTime = (prevObj.GetType() == typeof(DateTime));
            Dictionary<string, Series> dic = new Dictionary<string, Series>();
            //var indexColName = table.GetIndexColName();
            foreach (DataColumn name in table.Columns)
            {
                if (name.ColumnName == indexColName) { continue; }
                Series seriesX = new Series(name.ColumnName);
                seriesX.ChartType = SeriesChartType;// SeriesChartType.Point;
                seriesX.YValueType = ChartValueType.Double;
                seriesX.XValueType = XValueType;
                seriesX.MarkerSize = 5;
                seriesX.BorderWidth = 5;
                seriesX.ToolTip = "#SERIESNAME: #VALX, #VALY";//#LEGENDTEXT // https://msdn.microsoft.com/en-us/library/dd207017.aspx
                dic[name.ColumnName] = seriesX;
            }

            int paramIndex = 0;
            foreach (var item in dic) { var series = item.Value; }
            //  var indexColValues = table.GetIndexValues();
            foreach (var result in dic)//避免集合修改无法遍历
            {
                if (result.Key == indexColName) { continue; }

                var seriesX = dic[result.Key];
                int graphIndex = 0;
                int dataIndex = -1;
                foreach (DataRow row in table.Rows)
                {
                    dataIndex++;
                    var obj = row[result.Key];
                    var val = Geo.Utils.DoubleUtil.TryParse(obj, double.NaN);

                    if (val is Double && Geo.Utils.DoubleUtil.IsValid((Double)val))
                    {
                        seriesX.Points.AddXY(indexes[graphIndex], val);

                    }
                    graphIndex++;
                }
                paramIndex++;
            }

            Init(dic.Values);
        }


        /// <summary>
        /// 输入序列
        /// </summary>
        /// <param name="table"></param>
        /// <param name="SeriesChartType">X轴图形类型</param>
        public CommonChartForm(ObjectTableStorage table, SeriesChartType SeriesChartType = SeriesChartType.Line)
        {
            InitializeComponent();
            this.Text = table.Name;

            if (table == null || table.RowCount == 0) return;
            var vectors = table.GetVectors(0, 100000, false, Double.NaN);
            var indexes = table.GetIndexValues();
            
            var first = indexes[0];
            bool isIndexTime = (first.GetType() == typeof(DateTime)) || (first.GetType() == typeof(Geo.Times.Time));
            Dictionary<string, Series> dic = new Dictionary<string, Series>();
            var indexColName = table.GetIndexColName();

            foreach (var name in table.ParamNames)
            {
                if (name == indexColName) { continue; }
                Series seriesX = new Series(name);
                seriesX.ChartType = SeriesChartType;// SeriesChartType.Point;
                seriesX.YValueType = ChartValueType.Double;
                seriesX.XValueType = isIndexTime ? ChartValueType.DateTime : ChartValueType.Double;
                seriesX.MarkerSize = 6;
                seriesX.BorderWidth = 6;
                seriesX.ToolTip = "#SERIESNAME: #VALX, #VALY";//#LEGENDTEXT // https://msdn.microsoft.com/en-us/library/dd207017.aspx
                dic[name] = seriesX;
            }

            int paramIndex = 0;
            foreach (var item in dic) { var series = item.Value; }
            var indexColValues = table.GetIndexValues();
            foreach (var result in vectors)//避免集合修改无法遍历
            {
                if (result.Key == indexColName) { continue; }

                var seriesX = dic[result.Key];
                int graphIndex = 1;//图形显示从1开始
                int dataIndex = -1;
                foreach (var val in result.Value)
                {
                    dataIndex++;

                    if (Geo.Utils.DoubleUtil.IsValid(val))
                    {
                        var index = indexColValues[dataIndex];
                        if (index is Geo.Times.Time)
                        {
                            seriesX.Points.AddXY(((Geo.Times.Time)index).DateTime, val);
                        }
                        else
                        {
                            seriesX.Points.AddXY(index, val);
                        }

                        graphIndex++;
                    }
                }
                paramIndex++;
            }

            Init(dic.Values);
        }

        /// <summary>
        /// 点
        /// </summary>
        public DataPoint[] Points { get; set; }
        /// <summary>
        /// 图，可以设置样式
        /// </summary>
        public Chart Chart { get { return chart1; } set { chart1 = value; } }
        /// <summary>
        /// 初始化
        /// </summary>
        /// <param name="series"></param>
        public void Init(IEnumerable<Series> series)
        {
            Init(series.ToArray());
        }
        /// <summary>
        /// 初始化
        /// </summary>
        /// <param name="series"></param>
        public void Init(params Series[] series)
        {
            if (series.Length == 0) { return; }

            this.chart1.BeginInit();
            this.chart1.MouseWheel += chart1_MouseWheel;
            //this.chart1.ChartAreas[0].AxisX.IsStartedFromZero = false;
            //chart1.ChartAreas[0].AxisX.IntervalOffset = -80.00D;

            // Zoom into the X axis
            var chartArea = chart1.ChartAreas[0];

            chartArea.AxisY.IsStartedFromZero = false;
            //chartArea.AxisY.IntervalOffset = -80.00D;  
            this.chart1.Series.Clear();
            foreach (var item in series) { this.chart1.Series.Add(item); }
            chart1.ImeMode = ImeMode.Inherit;
         
            this.chart1.Legends[0].Position.Auto = true;

            if (false && series.Length >= 5)
            {
                this.chart1.Legends[0].Docking = Docking.Bottom;
                this.chart1.Legends[0].Alignment = StringAlignment.Center;
                chartArea.AxisX.LabelStyle.Angle = 90;

            }
            var ser = series[0];
            if (ser.XValueType == ChartValueType.DateTime)
            { 
                chartArea.AxisX.LabelStyle.Format = Geo.Utils.ChartUtil.GetTimeLableFormat(ser);
            }
            FontFamily fontFamily = FontSettingOption.DefaultFontFamily;

            //chartArea.AxisX.ScaleView.Zoom(2, 3);
            // Enable range selection and zooming end user interface
            chartArea.CursorX.IsUserEnabled = true;
            chartArea.CursorX.AutoScroll = true;
            chartArea.CursorX.IsUserSelectionEnabled = true;
            chartArea.AxisX.ScaleView.Zoomable = true;
            chartArea.AxisX.IsLabelAutoFit = true;
            chartArea.AxisX.MajorTickMark.Enabled = true;
            chartArea.AxisX.IsMarginVisible = false;
            //坐标轴设置
            chartArea.AxisX.LineColor = Color.Black;
            chartArea.AxisX.LineWidth = 1;
            //chartArea.AxisX.MajorGrid = new Grid();
            chartArea.AxisY.LineColor = Color.Black;
            chartArea.AxisY.LineWidth = 1;
            chartArea.AxisX.TitleFont = new Font(fontFamily, 14);
            chartArea.AxisY.TitleFont = new Font(fontFamily, 14);
            chartArea.AxisX.LabelStyle.Font = new Font(fontFamily, 12);
            chartArea.AxisY.LabelStyle.Font = new Font(fontFamily, 12);
            chart1.Legends[0].Font = new Font(fontFamily, 12);

            chartArea.AxisX.ScrollBar.IsPositionedInside = false;// true;//将滚动内嵌到坐标轴中           
            chartArea.AxisX.ScrollBar.Size = 10; // 设置滚动条的大小
            // 设置滚动条的按钮的风格，下面代码是将所有滚动条上的按钮都显示出来
            chartArea.AxisX.ScrollBar.ButtonStyle = ScrollBarButtonStyles.All;
            chartArea.AxisX.ScrollBar.Enabled = true;
            // 设置自动放大与缩小的最小量
            // chartArea.AxisX.ScaleView.SmallScrollSize = double.NaN;
            // chartArea.AxisX.ScaleView.SmallScrollMinSize = 2;
            //设置Y轴允许拖动放大  
            chartArea.CursorY.AutoScroll = true;
            chartArea.CursorY.IsUserEnabled = true;
            chartArea.CursorY.IsUserSelectionEnabled = true;
            chartArea.AxisY.ScrollBar.IsPositionedInside = false;// true;
            chartArea.AxisY.ScrollBar.ButtonStyle = ScrollBarButtonStyles.All;
            chartArea.AxisY.IsLabelAutoFit = true;
            chartArea.AxisY.ScaleView.Zoomable = true;
            chartArea.AxisY.ScrollBar.Enabled = true;

            chartArea.CursorX.LineColor = Color.Black;
            chartArea.CursorX.LineWidth = 1;

            chartArea.CursorY.LineColor = Color.Black;
            chartArea.CursorY.LineWidth = 1;

            chartArea.CursorX.IsUserEnabled = true;               //设置坐标轴可以用鼠标点击放大，可以看到更小的刻度
            chartArea.CursorX.IsUserSelectionEnabled = true; //用户可以选择从那里放大
            chartArea.CursorY.IsUserEnabled = true;               //设置坐标轴可以用鼠标点击放大，可以看到更小的刻度
            chartArea.CursorY.IsUserSelectionEnabled = true; //用户可以选择从那里放大

            //chart1.ViewZoomAutoLinkage = true;
            //chart1.ViewZoomYFix = 1;

            //   chart1.Zoom(3, 5, chart1.Series[0]);
            this.chart1.EndInit();
        } 


        //鼠标滚轮事件(移动/缩放)
        private void chart1_MouseWheel(object sender, MouseEventArgs e)
        {
            var chartArea = chart1.ChartAreas[0];
            //按住Alt，缩放 X
            if (Control.ModifierKeys == Keys.Alt)
            {
                if (e.Delta > 0) { ZoomInX(); }
                else { ZoomOutX(); }
            }
            //按住Shift，缩放 Y
            else if (Control.ModifierKeys == Keys.Shift)
            {
                if (e.Delta > 0) { ZoomInY(); }
                else { ZoomOutY(); }
            }
            //按住Ctrl，滚动 Y
            else if (Control.ModifierKeys == Keys.Control)
            {
                if (e.Delta > 0) { MoveUp(); }
                else { MoveLeft(); }
            }
            //不按Ctrl，滚动 X
            else
            {
                if (e.Delta > 0) { MoveLeft(); }
                else { MoveRight(); }
            }
        }


        private void 复制绘图ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                using (MemoryStream ms = new MemoryStream())
                {
                    this.chart1.SaveImage(ms, ChartImageFormat.Png);
                    Bitmap m = new Bitmap(ms);

                    //复制到粘贴板
                    Clipboard.SetImage(m);
                }
            }
            catch (Exception ex)
            {
                Geo.Utils.FormUtil.ShowErrorMessageBox("复制绘图失败！" + ex.Message);
            }
        }


        private static Series BuildSeries(Dictionary<Double, Double> dic, string name)
        {
            Series s = new Series(name);
            s.ChartType = SeriesChartType.Point;
            s.YValueType = ChartValueType.Double;
            s.XValueType = ChartValueType.Double;
            int i = 0;

            foreach (var list in dic)
            {
                DataPoint p = new DataPoint(list.Key, list.Value);
                s.Points.Add(p);
                i++;
            }
            return s;
        }
        private static Series BuildSeries(IEnumerable<Double> lists, string name)
        {
            Series s = new Series(name);
            s.ChartType = SeriesChartType.Point;
            s.YValueType = ChartValueType.Double;
            s.XValueType = ChartValueType.Double;
            int i = 0;

            foreach (var list in lists)
            {
                DataPoint p = new DataPoint(i, list);
                s.Points.Add(p);
                i++;
            }
            return s;
        }

        #region Y
        private void buttonZoomInY_Click(object sender, EventArgs e) { ZoomInY(); }
        private void buttonZoomOutY_Click(object sender, EventArgs e) { ZoomOutY(); }
        private void buttonUpY_Click(object sender, EventArgs e) { MoveUp(); }
        private void buttonDouwnY_Click(object sender, EventArgs e) { MoveDown(); }
        private void button_resetY_Click(object sender, EventArgs e)
        {
            var ScaleView = this.chart1.ChartAreas[0].AxisY.ScaleView;
            ScaleView.ZoomReset();
        }
        #endregion

        #region  X
        private void buttonZoomXIn_Click(object sender, EventArgs e) { ZoomInX(); }
        private void buttonZoomXOut_Click(object sender, EventArgs e) { ZoomOutX(); }
        private void button_upX_Click(object sender, EventArgs e) { MoveRight(); }
        private void button_downX_Click(object sender, EventArgs e) { MoveLeft(); }
        private void button_resetX_Click(object sender, EventArgs e)
        {
            var ScaleView = this.chart1.ChartAreas[0].AxisX.ScaleView;
            //CheckZoomable(ScaleView);
            ScaleView.ZoomReset();
        }
        #endregion

        #region 工具
        private void ZoomInX() { Zoom(true, true); }
        private void ZoomOutX() { Zoom(true, false); }
        private void ZoomOutY() { Zoom(false, false); }
        private void ZoomInY() { Zoom(false, true); }
        private void MoveRight() { MoveAxis(true, true); }
        private void MoveLeft() { MoveAxis(true, false); }
        private void MoveUp() { MoveAxis(false, true); }
        private void MoveDown() { MoveAxis(false, false); }
        /// <summary>
        /// 坐标轴平移
        /// </summary>
        /// <param name="isAxisXOrY"></param>
        /// <param name="isGreaterOrSmaller"></param>
        private void MoveAxis(bool isAxisXOrY, bool isGreaterOrSmaller)
        {
            var ScaleView = this.chart1.ChartAreas[0].AxisY.ScaleView;
            if (isAxisXOrY) { ScaleView = this.chart1.ChartAreas[0].AxisX.ScaleView; }
            CheckZoomable(ScaleView);
            double size = 1;
            if (Geo.Utils.DoubleUtil.IsValid(ScaleView.Size))
            {
                size = ScaleView.Size / 4;
            }
            if (isGreaterOrSmaller)
            {
                ScaleView.Scroll(ScaleView.Position - size);
            }
            else
            {
                ScaleView.Scroll(ScaleView.Position + size);
            }
        }
        /// <summary>
        /// 缩放工具方法
        /// </summary>
        /// <param name="isAxisXOrY"></param>
        /// <param name="inOrOut"></param>
        private void Zoom(bool isAxisXOrY, bool inOrOut)
        {
            var ScaleView = this.chart1.ChartAreas[0].AxisY.ScaleView;
            if (isAxisXOrY) { ScaleView = this.chart1.ChartAreas[0].AxisX.ScaleView; }
            CheckZoomable(ScaleView);
            if (inOrOut)
            {
                ScaleView.Size -= ScaleView.Size / 4;
            }
            else
            {
                ScaleView.Size += ScaleView.Size / 4;
            }
        }

        /// <summary>
        /// 检查是否可以调整。确保可以调整。
        /// </summary>
        /// <param name="ScaleView"></param>
        private static void CheckZoomable(AxisScaleView ScaleView)
        {
            if (!ScaleView.IsZoomed)
            {
                ScaleView.Zoom(ScaleView.ViewMinimum, ScaleView.ViewMaximum * 0.999999999);
            }
        }
        #endregion

        private void chart1_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        {
            switch (e.KeyData)
            {
                case Keys.Up: MoveUp(); break;
                case Keys.Down: MoveDown(); break;
                case Keys.Left: MoveLeft(); break;
                case Keys.Right: MoveRight(); break;
                default: break;
            }
        }

        private void CommonChartForm_Load(object sender, EventArgs e)
        {
            if (Setting.VersionType != VersionType.Development)
            {
                this.button_setting.Visible = false;
            }
            //this.button_openNewTableWihFormat.Visible = false;
            //button_simpleSet
        }
        public ChartSettingOption Option { get; set; }
        private void button_setting_Click(object sender, EventArgs e)
        {
            CheckOrInitOption();

            ChartSettingForm form = new ChartSettingForm(Option);
            form.ApplyAction += Form_ApplyAction;
            form.Show();
            //if (form.ShowDialog() == DialogResult.OK)
            //{
            //    Option = form.Option;

            //    ApplyOptionFormat(Option);
            //}
        }

        private void CheckOrInitOption()
        {
            if (Option == null)
            {
                var legend = chart1.Legends[0];
                var chart = chart1.ChartAreas[0];
                Option = new ChartSettingOption(chart1);
            }
        }

        private void Form_ApplyAction(ChartSettingOption obj)
        {
            Option = obj;
            Option.ApplyOptionFormat(chart1);
        }

        private void button_openNewTableWihFormat_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = Setting.TextTableFileFilter;
            openFileDialog.Multiselect = true;

            CheckOrInitOption();

            if (openFileDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                string[] files = openFileDialog.FileNames;
                foreach (var filePath in files)
                {
                    var reader = new ObjectTableReader(filePath, Encoding.Default);
                    var table = reader.Read();//.GetDataTable();  
                    var fileName = System.IO.Path.GetFileName(filePath);
                    var colNames = table.ParamNames.ToArray();
                    foreach (var item in colNames)
                    {
                        if (!Option.SeriesSettingOptions.ContainsKey(item))
                        {
                            table.RemoveCol(item);
                        }
                    }
                    if(table.ColCount ==0 || table.RowCount == 0)
                    {
                        Geo.Utils.FormUtil.ShowWarningMessageBox("载入表格格式不匹配！" + filePath);
                        continue;
                    }
                    var form = new CommonChartForm(table) { Text = fileName };
                    form.Text = filePath;
                    form.Option = this.Option;
                    form.Option.ApplyOptionFormat(chart1);
                    form.Show();
                    //  form.Show();
                }
            }


        }
        bool LegendMoveFlag;
        private void chart1_KeyPress(object sender, KeyPressEventArgs e)
        {
        }
        private void chart1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.L)
            {
                LegendMoveFlag = true;
                log.Info("触发图例移动功能，请用鼠标左键在目的位置点击。");
            }
        }
        private void chart1_MouseUp(object sender, MouseEventArgs e)
        {
            this.chart1.Focus();
            if (LegendMoveFlag)
            {
                var lengend = this.chart1.Legends[0];
                LegendMoveFlag = false;
                try
                {
                    var pt = e.Location;// this.chart1.PointToClient(e.Location);

                    lengend.Position.X = pt.X * 100 / this.chart1.Width;//设置x坐标.
                    lengend.Position.Y = pt.Y * 100 / this.chart1.Height;//设置y坐标. 
                }
                catch (Exception ex)
                {
                    lengend.Position.X = 1;
                    lengend.Position.Y = 1;

                    Geo.Utils.FormUtil.ShowErrorMessageBox(e.Location + ", " + ex.Message);
                }
            }
        }

        private void chart1_MouseDown(object sender, MouseEventArgs e)
        {
        }

        private void chart1_MouseMove(object sender, MouseEventArgs e)
        {

        }

        private void 移动图例LToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.LegendMoveFlag = true;
            log.Info("触发图例移动功能，请用鼠标左键在目的位置点击。");
        }

        private void 绘图另存为SToolStripMenuItem_Click(object sender, EventArgs e)
        {
            saveFileDialog1.FileName = this.Text;

            if (saveFileDialog1.ShowDialog() == DialogResult.OK)
            {
                var extension = Path.GetExtension(saveFileDialog1.FileName).ToLower();
                ChartImageFormat chartImage = ChartImageFormat.Bmp;
                if (extension.Contains("png"))
                {
                    chartImage = ChartImageFormat.Png;
                }
                if (extension.Contains("jpg"))
                {
                    chartImage = ChartImageFormat.Jpeg;
                }
                this.chart1.SaveImage(saveFileDialog1.FileName, chartImage);
                Geo.Utils.FormUtil.ShowOkAndOpenDirectory( Path.GetDirectoryName( saveFileDialog1.FileName));
            }
        }

        private void button_simpleSet_Click(object sender, EventArgs e)
        {
            CheckOrInitOption();

            SimpleChartSettingForm form = new SimpleChartSettingForm(Option);
            form.ApplyAction += Form_ApplyAction;
            form.Show();
            //if (form.ShowDialog() == DialogResult.OK)
            //{
            //    Option = form.Option;

            //    ApplyOptionFormat(Option);
            //}
        }

        private void 新窗口打开绘图NToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                CommonChartForm form = new CommonChartForm();
                form.Text = this.Text;
                form.Init(this.chart1.Series);
                form.Show();
            }
            catch (Exception ex)
            {
                Geo.Utils.FormUtil.ShowErrorMessageBox(ex.Message + " 只能新打开一次！");
            }

        }

        private void 隐藏打开按钮面板VToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.panel1.Visible = !this.panel1.Visible;
        }

        private void CommonChartForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            //若是以图打开，则必须加上，
            this.chart1.Series.Clear();
        }
    }
}