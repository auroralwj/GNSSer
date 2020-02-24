//2016.12.01, czs, create in hongqing, 绘制坐标
//2016.12.04, czs, edit in hongqing, 基本完成绘制坐标
//2018.12.14，czs, edit in hmx, 实现鼠标选择，删除功能

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Drawing.Imaging;
using Geo.Times;

namespace Geo.Draw
{
    /// <summary>
    /// 绘图，绘制表格数据是否有数据。
    /// </summary>
    public partial class EpochChartControl : UserControl
    {
        /// <summary>
        /// 默认构造函数
        /// </summary>
        public EpochChartControl()
        {
            InitializeComponent();

            Origin = new Point(50, 70);
            this.UserToScreenCoordConverter = new Geo.Coordinates.ScreenCoordConverter(this.pictureBox1.Size);
            UserChartGraphics = new UserChartGraphics(this.UserToScreenCoordConverter);

            this.pictureBox1.MouseWheel += pictureBox1_MouseWheel;
        }
        /// <summary>
        /// 是否允许删除
        /// </summary>
        public bool EnableDelete { get => this.删除所选DToolStripMenuItem.Visible; set => this.删除所选DToolStripMenuItem.Visible = value; }

        /// <summary>
        /// 通过鼠标滚轮实现图的缩放
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void pictureBox1_MouseWheel(object sender, MouseEventArgs e)
        {
            var location = UserToScreenCoordConverter.GetUserCoord(e.Location);
            int userX = location.X;
            var epoch = GetEpcohNumber(userX); 
            int toStartEpochCount = epoch - MinEpoch;
            int toEndEpochCount = MaxEpoch - epoch;

            if (e.Delta > 0)
            {
                toStartEpochCount = toStartEpochCount / 2;
                toEndEpochCount = toEndEpochCount / 2; 
            }
            else
            {
                toStartEpochCount = toStartEpochCount * 2;
                toEndEpochCount = toEndEpochCount * 2; 
            }


            this.MinEpoch = epoch - toStartEpochCount;
            this.MaxEpoch = epoch + toEndEpochCount;
            //合法性设置
            if (this.MinEpoch < 0) { this.MinEpoch = 0; }
            if (this.MaxEpoch > this.DataTable.Rows.Count) { this.MaxEpoch = this.DataTable.Rows.Count; }
            if (this.MaxEpoch == 0) { this.MaxEpoch = 1; }
            if (this.MinEpoch >= MaxEpoch) { this.MinEpoch = MaxEpoch - 1; }

            Draw();
        }
        #region 数据绑定
        /// <summary>
        /// 绘制表格，默认第一列为编号
        /// </summary>
        /// <param name="table"></param>
        public void SetTable(ObjectTableStorage table)
        {
            var tb = table.GetDataTable("表格", true);
            SetTable(tb);
        }
        /// <summary>
        /// 绘制表格，默认第一列为编号
        /// </summary> 
        /// <param name="tb"></param>
        public void SetTable(System.Data.DataTable tb)
        {
            this.DataTable = tb;
            MinEpoch = 0;
            MaxEpoch = tb.Rows.Count;//.RowCount;

            this.SatCount = tb.Columns.Count - 1;
        }

        /// <summary>
        /// 由时段数据构建历元表数据
        /// </summary>
        /// <param name="timeperiods"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        public static ObjectTableStorage BuildEpochTable(Dictionary<string, List<TimePeriod>> timeperiods, string name)
        {
            ObjectTableStorage table = new ObjectTableStorage(name);
            Time start = Time.MaxValue;
            Time end = Time.MinValue;
            foreach (var list in timeperiods)
            {
                var max = TimePeriod.MaxTime(list.Value);
                var min = TimePeriod.MinTime(list.Value);
                if (max > end) { end = max; }
                if (min < start) { start = min; }
            }
            //绘制一千条
            double interval = (end - start) / 1000.0;// 1 * 60;//min
            for (Time i = start; i <= end; i += interval)
            {
                table.NewRow();
                table.AddItem("_Epoch", i);//保证在第1位置

                foreach (var kvs in timeperiods)
                {
                    foreach (var item in kvs.Value)
                    {
                        if (item.Contains(i))
                        {
                            table.AddItem(kvs.Key, i);
                        }
                    }
                }
            }

            return table;
        }
        #endregion

        /// <summary>
        /// 执行数据删除操作
        /// </summary>
        public event Action<Dictionary<string, IntSegment>> DataDeleting;
        #region  属性
        /// <summary>
        /// 用户坐标向屏幕转换。
        /// </summary>
        Geo.Coordinates.ScreenCoordConverter UserToScreenCoordConverter { get; set; }
        UserChartGraphics UserChartGraphics { get; set; }
        /// <summary>
        /// 绘图控件
        /// </summary>
        protected PictureBox PictureBox { get { return pictureBox1; } }
        /// <summary>
        /// 数据表
        /// </summary>
        DataTable DataTable { get; set; }
        /// <summary>
        /// 卫星数量，行带数量。除去了表格中的第一行，认为是时间等编号。
        /// </summary>
        public int SatCount { get; set; }
        /// <summary>
        /// 坐标原点位置
        /// </summary>
        public Point Origin { get; set; }
        /// <summary>
        /// 最小绘图历元
        /// </summary>
        public int MinEpoch { get; set; }
        /// <summary>
        /// 最大绘图历元
        /// </summary>
        public int MaxEpoch { get; set; }
        /// <summary>
        /// 中心绘图历元
        /// </summary>
        public int CenterEpoch { get { return (MaxEpoch + MinEpoch) / 2; } }
        /// <summary>
        /// 绘图历元数量
        /// </summary>
        public int EpochCount { get { return Math.Abs(MaxEpoch - MinEpoch); } }
        /// <summary>
        /// 坐标区域高度，即绘图区域
        /// </summary>
        int CoordHeight { get { return UserToScreenCoordConverter.ChartSize.Height - Origin.Y; } }
        /// <summary>
        /// 坐标区域宽度
        /// </summary>
        int CoordWidth {get{ return UserToScreenCoordConverter.ChartSize.Width - Origin.X;}}
        #endregion

        #region 绘图细节
        /// <summary>
        /// 绘制图标，第一列为编号
        /// </summary>
        private void DrawTable()
        {
            if (DataTable == null) { return; }
            DrawBelts();
            DrawYLabels();
            DrawXLables();
        }
        /// <summary>
        /// 绘制内容带
        /// </summary>
        private void DrawBelts()
        {
            int intervalOfSat = 2;

            double epochCountPerPixe = this.EpochCount * 1.0 / CoordWidth;
            double satCountPerPixe = this.SatCount * 1.0 / CoordHeight;

            int beltHeight = CoordHeight / SatCount - intervalOfSat;//减一，为留空格好看
            int drawingBeltHeight = Math.Min(beltHeight, 6);
            Pen penL1 = new Pen(Color.Blue, 2);
            Pen penL2 = new Pen(Color.FromArgb(100, 200, 50), 2);

            for (int i = 1; i <= SatCount; i++)
            {
                int y = GetYCoord(i - 1);//扣除epoch，从1开始
                List<int> xCoord = new List<int>();
                int lastX = 0;//避免重复加入
                for (int epoch = MinEpoch; epoch < MaxEpoch; epoch++)
                {
                    var row = DataTable.Rows[epoch];

                    if (!row.IsNull(i))
                    {
                        var x = GetXCoord(epoch);

                        if (lastX == x) { continue; }

                        xCoord.Add(x);

                        lastX = x;
                    }
                }
                Pen pen = null;
                if (i % 2 == 0) { pen = penL1; }
                else { pen = penL2; }

                UserChartGraphics.DarwBelt(pen, y, xCoord.ToArray(), drawingBeltHeight);
            }
        }
        /// <summary>
        /// 绘制Y轴标签
        /// </summary>
        private void DrawYLabels()
        {
            //绘制Y轴标签，纵轴，卫星号   
            int minYDiffer = 10;
            int prevYPixe = -minYDiffer;
            for (int i = 1; i <= SatCount; i++)
            {
                int y = GetYCoord(i - 1);//扣除epoch，从1开始

                int x = Origin.X / 2;
                var pos = new Point(x, y); ;
                if (y - prevYPixe >= minYDiffer)
                {
                    UserChartGraphics.DrawLabel(DataTable.Columns[i].ToString(), pos);

                    prevYPixe = y;
                }
            }
        }

        /// <summary>
        /// 绘制X轴标签
        /// </summary>
        private void DrawXLables()
        {
            //绘制历元标签 
            int prevX = -50;
            int minXDiffer = 20;
            for (int epcoh = MinEpoch; epcoh < MaxEpoch; epcoh++)
            {
                int y = Origin.Y / 2;
                int x = GetXCoord(epcoh);
                var pos = new Point(x, y);
                if ((pos.X - prevX) >= minXDiffer)
                {
                    prevX = pos.X;

                    var xlabel = GetIndexLabelX(epcoh);

                    UserChartGraphics.DrawLabel(xlabel, pos, -90);
                }
            }
        }
        /// <summary>
        /// 获取用户 Y 坐标,，右手笛卡尔坐标
        /// </summary>
        /// <param name="satIndex">从0开始</param>
        /// <returns></returns>
        protected int GetYCoord(int satIndex)
        {
            return (int)(Origin.Y + satIndex * HeightPerSat + HeightPerSat / 2);
        }
        /// <summary>
        /// 获取卫星编号,卫星将纵坐标分区。
        /// </summary>
        /// <param name="yCoord"></param>
        /// <returns></returns>
        public int GetSatIndex(int yCoord)
        {
            double satIndex = (yCoord - Origin.Y) / HeightPerSat;
            return (int)Math.Round(satIndex);
        }
        /// <summary>
        /// 获取一个卫星对应的区域高度,单位像素
        /// </summary>
        public double HeightPerSat { get => 1.0 * CoordHeight / SatCount; }
        /// <summary>
        /// 根据历元数量，获取X的坐标
        /// </summary>
        /// <param name="epoch"></param>
        /// <returns></returns>
        private int GetXCoord( int epoch)
        { 
            var x = (int)(Origin.X + 1.0 * (epoch - MinEpoch) / EpochCount * CoordWidth);
            return x;
        }
        /// <summary>
        /// 计算历元数量
        /// </summary>
        /// <param name="xCoord"></param>
        /// <returns></returns>
        public int GetEpcohNumber(int xCoord)
        {
            var epoch = (xCoord - Origin.X) * 1.0 * EpochCount / CoordWidth + MinEpoch;
            if( epoch < 0) { epoch = 0; }
            return (int)epoch;
        }
        /// <summary>
        /// 获取检索 X 的标签。
        /// </summary>
        /// <param name="i"></param>
        /// <returns></returns>
        private string GetIndexLabelX(int i)
        {
            var xLabelObj = DataTable.Rows[i][0];
            var xlabel = xLabelObj.ToString();
            if (xLabelObj is Geo.Times.Time)
            {
                xlabel = ((Geo.Times.Time)xLabelObj).DateTime.ToString("HH:mm:ss");
            }
            return xlabel;
        } 

        /// <summary>
        /// 绘图
        /// </summary>
        public void Draw()
        {
            if (DataTable == null) { return; }

            var count = GetMaxYLabelCount();
            var labelLen = count * 8;
            this.Origin = new Point(labelLen, 60);

            var size = pictureBox1.Size;
            if (size.Width == 0 || size.Height == 0) { return; }

            Bitmap bmp = new Bitmap(size.Width, pictureBox1.Size.Height); ;
            Graphics g = Graphics.FromImage(bmp);

            this.pictureBox1.Image = bmp;

            g.Clear(Color.White);
            UserChartGraphics.Graphics = g;

            DrawCoordAixs();

            DrawCoordGrid();

            //绘制图表
            DrawTable();
        }

        public int GetMaxYLabelCount()
        {
            int i = 1;
            foreach (DataColumn item in DataTable.Columns)
            {
                i = Math.Max(i, item.ToString().Length);
            }
            return i;
        }


        /// <summary>
        /// 绘制坐标轴
        /// </summary>
        private void DrawCoordAixs()
        {
            Pen penBoder = new Pen(Color.Black, 2);

            int width = this.Width;
            int height = this.Height;
            int minX = Origin.X;
            int minY = Origin.Y;
            int maxX = width;
            int maxY = height;

            //绘制坐标
            var verticalLine = new Point[]{
                new Point(minX, minY),
                new Point(minX, maxY )};
            var horizonLine = new Point[]{
                new Point(minX, minY), new Point(maxX, minY)
            };

            UserChartGraphics.DrawLine(penBoder, new Point(minX, minY), new Point(minX, maxY));
            UserChartGraphics.DrawLine(penBoder, new Point(minX, minY), new Point(maxX, minY));
        }
        /// <summary>
        /// 绘制坐标窗格
        /// </summary>
        private void DrawCoordGrid()
        {
            Pen penGrid = new Pen(Color.BurlyWood, 1);
            penGrid.DashStyle = System.Drawing.Drawing2D.DashStyle.Dash;

            //绘制窗格
            int rowCount = SatCount == 0 ? 30 : SatCount;
            int colCount = EpochCount > 60 ? 60 : EpochCount;
            colCount = colCount == 0 ? 60 : colCount;

            UserChartGraphics.DrawGrid(penGrid, rowCount, colCount,
                   Origin, UserToScreenCoordConverter.RightTop);
        }

        private void ChartControl_SizeChanged(object sender, EventArgs e)
        {
            UserToScreenCoordConverter.ChartSize = pictureBox1.Size;
            Draw();
        }
        #endregion

        private void 复制图像CToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Clipboard.SetDataObject(pictureBox1.Image);
        }

        private void 重置RToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (this.DataTable == null) { return; }
            this.MinEpoch = 0;
            this.MaxEpoch = this.DataTable.Rows.Count;

            this.Draw();
        }

        #region  鼠标绘图
        bool IsCatchStart = false;
        Point DownPoint = Point.Empty;
        /// <summary>
        /// 选择的矩形框
        /// </summary>
        Rectangle SelectedRectangle { get; set; }
        private void pictureBox1_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)// 
            {
                if (!IsCatchStart)
                {
                    IsCatchStart = true;
                    DownPoint = e.Location;
                }
            }
        }

        private void pictureBox1_MouseUp(object sender, MouseEventArgs e)
        {
            //区域选择Features
            if (e.Button == MouseButtons.Left)
            {
                IsCatchStart = false;
                //Size size = GetSizeFromStartPoint(e);
                //SelectedRectangle = new Rectangle(e.Location, size);
            }
        }

        private void pictureBox1_MouseMove(object sender, MouseEventArgs e)
        {
            if (IsCatchStart)
            {
                //Bitmap destmap = (Bitmap)originBmp.Clone();
                Bitmap destmap = (Bitmap)this.pictureBox1.Image.Clone();
                var newPt = e.Location;
                var g = Graphics.FromImage(destmap);

                Pen pen = new Pen(Color.Gray, 1);

                Size size = GetSizeFromStartPoint(e);

                Point leftTop = GetLeftTop(newPt);

                this.SelectedRectangle = new Rectangle(leftTop, size);
                g.DrawRectangle(pen, SelectedRectangle);                
                g.FillRectangle(new SolidBrush(Color.FromArgb(125, Color.OrangeRed)), SelectedRectangle);
                g.Dispose();
                pen.Dispose();

                //双缓冲，避免闪烁
                Graphics g1 = this.pictureBox1.CreateGraphics();
                g1.DrawImage(destmap, new PointF(0, 0));
                g1.Dispose();
                destmap.Dispose();
            }
        }

        private Size GetSizeFromStartPoint(MouseEventArgs e)
        {
            return new Size(Math.Abs(e.X - DownPoint.X), Math.Abs(e.Y - DownPoint.Y));
        }

        //获取左上角
        private Point GetLeftTop(Point newPt)
        {
            var leftTop = DownPoint;
            if (newPt.X < DownPoint.X)
            {
                leftTop.X = newPt.X;
            }
            if (newPt.Y < DownPoint.Y)
            {
                leftTop.Y = newPt.Y;
            }

            return leftTop;
        }
        #endregion

        private void 删除所选DToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (SelectedRectangle.IsEmpty)
            {
                Geo.Utils.FormUtil.ShowWarningMessageBox("请先用鼠标选择后再试！");return;
            }
            if(DataDeleting != null)
            {
                Dictionary<string, IntSegment> result = GetSelectedData();
                if(result ==null || result.Count == 0) { return; }
                IntSegment first = result.Values.First();
                StringBuilder sb = new StringBuilder();
                sb.Append("确认删除");
                sb.Append("历元");
                var start = this.DataTable.Rows[first.Start][0];
                var end = this.DataTable.Rows[first.End][0];
                sb.Append(start + "->" + end);
                sb.Append(" 的 ");
                sb.Append(Geo.Utils.StringUtil.ToString(result.Keys));
                sb.Append(", 历元数 " + first.Span);// 
               // sb.Append(", 历元数 " + Geo.Utils.StringUtil.ToString(result.Values.First()));

                if (Geo.Utils.FormUtil.ShowYesNoMessageBox(sb.ToString()) == DialogResult.Yes)
                {
                    DataDeleting(result);
                }
            }
        }

        private Dictionary<string, IntSegment> GetSelectedData()
        {
            Dictionary<string, IntSegment> data = new Dictionary<string, IntSegment>();
            
            var leftTop = this.UserToScreenCoordConverter.GetUserCoord(SelectedRectangle.Location);
            var rightDown = this.UserToScreenCoordConverter.GetUserCoord(SelectedRectangle.Location + new Size(SelectedRectangle.Width, SelectedRectangle.Height));

            //计算纵坐标包括的标签
            int startSatIndex = GetSatIndex(rightDown.Y);
            int endSatIndex = GetSatIndex(leftTop.Y);
            if(startSatIndex < 0) { endSatIndex = 0; }
            if(endSatIndex >= DataTable.Columns.Count) { endSatIndex = DataTable.Columns.Count - 1; }


            int startEpochIndex = GetEpcohNumber( leftTop.X);
            int endEpochIndex = GetEpcohNumber( rightDown.X);
            if (startEpochIndex < 0) { startEpochIndex = 0; }
            if (endEpochIndex >= DataTable.Rows.Count) { endEpochIndex = DataTable.Rows.Count - 1; }
            var epochSpan = new IntSegment(startEpochIndex, endEpochIndex);

            Dictionary<string, IntSegment> restult = new Dictionary<string, IntSegment>();
            for (int i =startSatIndex  ; i < endSatIndex; i++)
            {
                var prn = this.DataTable.Columns[i+1].ColumnName;//扣除第一检索列
                restult[prn] = epochSpan;
            }

            return restult;
        }
    }
}