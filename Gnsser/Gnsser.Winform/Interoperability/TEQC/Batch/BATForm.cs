//2014.12.27, lh, create in 郑州, TEQC 互操作

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Threading.Tasks;
using System.Windows.Forms.DataVisualization.Charting.Data;
using System.Windows.Forms.DataVisualization.Charting;

//using Gnsser.Rinex;
using Gnsser;
using Geo.Coordinates;
using Geo.Referencing;
using System.Diagnostics;
using Gnsser.Interoperation.Teqc;

namespace Gnsser.Winform
{
    public partial class BATForm : Form
    {
        public BATForm()
        {
            InitializeComponent();
        }

        public class QCresult
        {
            public string Sites { get; set; }
            public string First_epoch { get; set; }
            public string Last_epoch { get; set; }
            public string Hrs { get; set; }
            public string Dt { get; set; }
            public string Expt { get; set; }
            public string Have { get; set; }
            public string Percent { get; set; }
            public string Mp1 { get; set; }
            public string Mp2 { get; set; }
            public string O_slps { get; set; }

        }
        public List<QCresult> Items = new List<QCresult>();
        public List<QCresult> Failure = new List<QCresult>();
        string destDir = "";
        Stopwatch stopwatch = new Stopwatch();

        Dictionary<string, string> pathes = new Dictionary<string, string>();

        private void button_setPath_Click(object sender, EventArgs e)
        {
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                this.bindingSource1.DataSource = null;
                pathes.Clear();
                string msg = "";
                foreach (string p in openFileDialog1.FileNames)
                {
                    string fileName = Path.GetFileName(p);
                    if (!pathes.ContainsKey(fileName)) pathes.Add(fileName, p);
                    else msg += fileName + "," + "\r\n";
                }
                if (msg != "")
                {
                    MessageBox.Show("以下文件没有添加成功，原因是已经包含了同名称的文件。\r\n" + msg);
                }

                this.treeView_rinex.Nodes.Clear();
                this.treeView_rinex.LabelEdit = true;
                TreeNode gen = new TreeNode();
                gen.Text = "Rinex观测文件";
                this.treeView_rinex.Nodes.Add(gen);
                foreach (KeyValuePair<string, string> kv in pathes)
                {
                    TreeNode zi = new TreeNode();
                    zi.Text = kv.Key;
                    gen.Nodes.Add(zi);
                }
                this.treeView_rinex.Nodes[0].Expand();
            }
        }

        private void button_BAT_Click(object sender, EventArgs e)
        {
            this.Items.Clear();
            this.Failure.Clear();
            stopwatch.Stop();
            stopwatch.Start();
            Geo.Utils.FormUtil.InitProgressBar(this.progressBar1, pathes.Count);

            foreach (KeyValuePair<string, string> kv in pathes)
            {
                string paths = kv.Value;
                destDir = paths.Substring(0, paths.LastIndexOf("\\"));
                TeqcFunctionCaller call = new TeqcFunctionCaller(TeqcSet.TeqcPath, TeqcFunction.QualityChecking);
                string result = call.Run(paths)[0];
                QCresult item = new QCresult();
                QCresult fail = new QCresult();

                //当后台cmd输出流不正常时，说明该测站O文件QC失败！
                if (!result.Contains("SUM ") || ((result.Contains("SUM ")) && result.Substring(result.IndexOf("SUM ") + 63, 4).Replace(" ", "") == "n/a"))
                {
                    fail.Sites = paths.Substring(paths.LastIndexOf("\\") + 1, 4);
                    fail.First_epoch = @"失败";
                    fail.Last_epoch = @"失败";
                    fail.Hrs = @"失败";
                    fail.Dt = @"失败";
                    fail.Expt = @"失败";
                    fail.Have = @"失败";
                    fail.Percent = @"失败";
                    fail.Mp1 = @"失败";
                    fail.Mp2 = @"失败";
                    fail.O_slps = @"失败";
                    this.Failure.Add(fail);
                }

                else
                {
                    item.Sites = paths.Substring(paths.LastIndexOf("\\") + 1, 4);
                    item.First_epoch = result.Substring(result.IndexOf("SUM ") + 4, 2) + @"-"
                        + result.Substring(result.IndexOf("SUM ") + 7, 2).Replace(" ", "")
                        + @"-" + result.Substring(result.IndexOf("SUM ") + 10, 2).Replace(" ", "")
                        + result.Substring(result.IndexOf("SUM ") + 12, 6);
                    item.Last_epoch = result.Substring(result.IndexOf("SUM ") + 19, 2) + @"-"
                        + result.Substring(result.IndexOf("SUM ") + 22, 2).Replace(" ", "")
                        + @"-" + result.Substring(result.IndexOf("SUM ") + 25, 2).Replace(" ", "")
                        + result.Substring(result.IndexOf("SUM ") + 27, 6);
                    item.Hrs = result.Substring(result.IndexOf("SUM ") + 34, 5).Replace(" ", "");
                    item.Dt = result.Substring(result.IndexOf("SUM ") + 41, 2).Replace(" ", "");
                    item.Expt = result.Substring(result.IndexOf("SUM ") + 45, 5).Replace(" ", "");
                    item.Have = result.Substring(result.IndexOf("SUM ") + 52, 5).Replace(" ", "");
                    item.Percent = result.Substring(result.IndexOf("SUM ") + 58, 3).Replace(" ", "");
                    item.Mp1 = result.Substring(result.IndexOf("SUM ") + 63, 4).Replace(" ", "");
                    item.Mp2 = result.Substring(result.IndexOf("SUM ") + 69, 4).Replace(" ", "");
                    item.O_slps = result.Substring(result.IndexOf("SUM ") + 74, 6).Replace(" ", "");
                    this.Items.Add(item);
                }

                this.progressBar1.PerformStep();
                this.progressBar1.Update();
                this.Update();

            }
            string msg = "转换完毕，耗时 " + stopwatch.Elapsed.ToString();

            stopwatch.Reset();

            msg += "\r\n是否打开？";

            Geo.Utils.FormUtil.ShowIfOpenDirMessageBox(destDir, msg);
        }

        private void button_screening_Click(object sender, EventArgs e)
        {
            BATScreeningForm f = new BATScreeningForm();
            if (f.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                this.dataGridView1.Rows.Clear();
                string[] dr = new string[12];
                int j = 0;
                for (int i = 0; i < this.Items.Count; i++)
                {
                    dr[1] = this.Items[i].Sites;
                    dr[2] = this.Items[i].First_epoch;
                    dr[3] = this.Items[i].Last_epoch;
                    dr[4] = this.Items[i].Hrs;
                    dr[5] = this.Items[i].Dt;
                    dr[6] = this.Items[i].Expt;
                    dr[7] = this.Items[i].Have;
                    dr[8] = this.Items[i].Percent;
                    dr[9] = this.Items[i].Mp1;
                    dr[10] = this.Items[i].Mp2;
                    dr[11] = this.Items[i].O_slps;
                    if (!BATScreeningParameter.invert_selection)
                    {
                        if (BATScreeningParameter.percent_selection)
                        {
                            if (Convert.ToDouble(dr[8]) < BATScreeningParameter.Percent ||
                               Convert.ToDouble(dr[9]) > BATScreeningParameter.Mp1 ||
                               Convert.ToDouble(dr[10]) > BATScreeningParameter.Mp2 ||
                               Convert.ToDouble(dr[11]) < BATScreeningParameter.O_slps)
                            {
                                j = j + 1;
                                dr[0] = j.ToString();
                                this.dataGridView1.Rows.Add(dr);
                            }
                        }
                        else
                            if (Convert.ToDouble(dr[9]) > BATScreeningParameter.Mp1 ||
                               Convert.ToDouble(dr[10]) > BATScreeningParameter.Mp2 ||
                               Convert.ToDouble(dr[11]) < BATScreeningParameter.O_slps)
                            {
                                j = j + 1;
                                dr[0] = j.ToString();
                                this.dataGridView1.Rows.Add(dr);
                            }
                    }
                    else
                    {
                        if (BATScreeningParameter.percent_selection)
                        {
                            if (Convert.ToDouble(dr[8]) >= BATScreeningParameter.Percent &&
                               Convert.ToDouble(dr[9]) <= BATScreeningParameter.Mp1 &&
                               Convert.ToDouble(dr[10]) <= BATScreeningParameter.Mp2 &&
                               Convert.ToDouble(dr[11]) >= BATScreeningParameter.O_slps)
                            {
                                j = j + 1;
                                dr[0] = j.ToString();
                                this.dataGridView1.Rows.Add(dr);
                            }
                        }
                        else
                            if (Convert.ToDouble(dr[9]) <= BATScreeningParameter.Mp1 &&
                               Convert.ToDouble(dr[10]) <= BATScreeningParameter.Mp2 &&
                               Convert.ToDouble(dr[11]) >= BATScreeningParameter.O_slps)
                            {
                                j = j + 1;
                                dr[0] = j.ToString();
                                this.dataGridView1.Rows.Add(dr);
                            }
                    }
                }
            }
        }

        private void button_clear_Click(object sender, EventArgs e)
        {
            this.treeView_rinex.Nodes.Clear();
            this.dataGridView1.Rows.Clear();
            this.chart_line.Series.Clear();
            this.chart_point.Series.Clear();
            Geo.Utils.FormUtil.InitProgressBar(this.progressBar1, pathes.Count);
        }

        private void button_show_Click(object sender, EventArgs e)
        {
            this.dataGridView1.Rows.Clear();

            //方法一：通过与DataTable绑定，直接显示Ppk内容，故this..dataGridView1的Column则无需定义！！
            //DataTable pk = new DataTable();

            //pk.Columns.Add("Sites"); pk.Columns.Add("First_epoch"); pk.Columns.Add("Last_epoch"); pk.Columns.Add("Hrs"); pk.Columns.Add("Dt");
            //pk.Columns.Add("Expt"); pk.Columns.Add("Have"); pk.Columns.Add("Percent"); pk.Columns.Add("Mp1"); pk.Columns.Add("Mp2");
            //pk.Columns.Add("O_slps");
            //for (int time = 0; time < this.Items.Count; time++)
            //{
            //    DataRow dr = pk.NewRow();
            //    dr["Sites"] = this.Items[time].Sites;
            //    dr["First_epoch"] = this.Items[time].First_epoch;
            //    dr["Last_epoch"] = this.Items[time].Last_epoch;
            //    dr["Hrs"] = this.Items[time].Hrs;
            //    dr["Dt"] = this.Items[time].Dt;
            //    dr["Expt"] = this.Items[time].Expt;
            //    dr["Have"] = this.Items[time].Have;
            //    dr["Percent"] = this.Items[time].Percent;
            //    dr["Mp1"] = this.Items[time].Mp1;
            //    dr["Mp2"] = this.Items[time].Mp2;
            //    dr["O_slps"] = this.Items[time].O_slps;
            //    pk.Rows.Add(dr);
            //} 
            //this.dataGridView1.DataSource = pk;


            //方法二：直接用this.dataGridView1.Rows.Add(数组)在定义好的Column中添加多个行
            string[] dr = new string[12];
            for (int i = 0; i < this.Items.Count; i++)
            {
                dr[0] = (i + 1).ToString();
                dr[1] = this.Items[i].Sites;
                dr[2] = this.Items[i].First_epoch;
                dr[3] = this.Items[i].Last_epoch;
                dr[4] = this.Items[i].Hrs;
                dr[5] = this.Items[i].Dt;
                dr[6] = this.Items[i].Expt;
                dr[7] = this.Items[i].Have;
                dr[8] = this.Items[i].Percent;
                dr[9] = this.Items[i].Mp1;
                dr[10] = this.Items[i].Mp2;
                dr[11] = this.Items[i].O_slps;
                this.dataGridView1.Rows.Add(dr);
            }

            for (int i = 0; i < this.Failure.Count; i++)
            {
                dr[0] = (i + 1).ToString();
                dr[1] = this.Failure[i].Sites;
                dr[2] = this.Failure[i].First_epoch;
                dr[3] = this.Failure[i].Last_epoch;
                dr[4] = this.Failure[i].Hrs;
                dr[5] = this.Failure[i].Dt;
                dr[6] = this.Failure[i].Expt;
                dr[7] = this.Failure[i].Have;
                dr[8] = this.Failure[i].Percent;
                dr[9] = this.Failure[i].Mp1;
                dr[10] = this.Failure[i].Mp2;
                dr[11] = this.Failure[i].O_slps;
                this.dataGridView1.Rows.Add(dr);
            }
        }

        private void dataGridView1_CellClick(object sender, System.Windows.Forms.DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {


                string s = dataGridView1.Rows[e.RowIndex].Cells[1].Value.ToString();
                for (int i = 0; i < this.treeView_rinex.Nodes[0].Nodes.Count; i++)
                {
                    if (this.treeView_rinex.Nodes[0].Nodes[i].ToString().Substring(10, 4) == s)
                        this.treeView_rinex.Nodes[0].Nodes[i].Checked = true;
                    else
                        this.treeView_rinex.Nodes[0].Nodes[i].Checked = false;
                }
            }
        }

        private void button_plot_Click(object sender, EventArgs e)
        {
            BATSeriesList.BATSeries = new List<Series>();

            //判断绘图文件类型，包括是否绘制周跳图
            bool Ifslip = false;
            //清除chart控件中的当前区域，再次点击绘图时坐标轴范围停留在上次绘图，因为暂未找到使得ChartAreas[0].AxisY.Maximum为Auto的方法！
            this.chart_line.Series.Clear(); this.chart_line.ChartAreas.Clear(); ChartArea chartArea1 = new ChartArea();
            this.chart_point.Series.Clear(); this.chart_point.ChartAreas.Clear(); ChartArea chartArea2 = new ChartArea();
            this.chart_line.ChartAreas.Add(chartArea1);
            this.chart_point.ChartAreas.Add(chartArea2);
            chart_line.Titles.Clear();
            chart_point.Titles.Clear();

            string plotfile_type = "空";
            if (radioButton_ele.Checked) { checkBox1.Checked = false; checkBox2.Checked = false; plotfile_type = "ele"; this.tabControl1.SelectedIndex = 0; }
            if (radioButton_azi.Checked) { checkBox1.Checked = false; checkBox2.Checked = false; plotfile_type = "azi"; this.tabControl1.SelectedIndex = 0; }
            if (radioButton_mp1mp2.Checked)
            {
                this.tabControl1.SelectedIndex = 0;
                if (checkBox1.Checked && (!checkBox2.Checked)) plotfile_type = "mp1";
                else if (!checkBox1.Checked && (checkBox2.Checked)) plotfile_type = "mp2";
                else { MessageBox.Show("请选中选项1或选项2！"); plotfile_type = "空"; }
            }
            if (radioButton_ioniod.Checked)
            {
                this.tabControl1.SelectedIndex = 0;
                if (checkBox1.Checked && (!checkBox2.Checked)) plotfile_type = "ion";
                else if (!checkBox1.Checked && (checkBox2.Checked)) plotfile_type = "iod";
                else { MessageBox.Show("请选中选项1或选项2！"); plotfile_type = "空"; }
            }
            if (radioButton_sn1sn2.Checked)
            {
                this.tabControl1.SelectedIndex = 0;
                if (checkBox1.Checked && (!checkBox2.Checked)) plotfile_type = "sn1";
                else if (!checkBox1.Checked && (checkBox2.Checked)) plotfile_type = "sn2";
                else { MessageBox.Show("请选中选项1或选项2！"); plotfile_type = "空"; }
            }
            if (radioButton_cycleslips.Checked) { Ifslip = true; plotfile_type = "ion"; this.tabControl1.SelectedIndex = 1; }



            /////////////////////////////////////////////////////////////////
            //以下为绘图部分，由于时间原因暂时图方便没有写成绘图类，Sorry！//
            /////////////////////////////////////////////////////////////////
            int j = 0; string filename_without_extention = "";
            for (int i = 0; i < this.treeView_rinex.Nodes[0].Nodes.Count; i++)
                if (this.treeView_rinex.Nodes[0].Nodes[i].Checked == true)
                {
                    filename_without_extention = this.treeView_rinex.Nodes[0].Nodes[i].ToString().Substring(10, this.treeView_rinex.Nodes[0].Nodes[i].ToString().IndexOf(".") - 10);
                    j++;
                }
            if (j == 0) MessageBox.Show("Rinex文件未选中，请在列表复选框内重新确认！");
            if (j > 1) MessageBox.Show("所选Rinex文件不只一个，请在列表复选框内重新确认！");
            if (j == 1)
            {
                if (plotfile_type == "空")
                {
                    this.chart_line.Series.Clear();
                    this.chart_point.Series.Clear();
                }
                //以下为数据库存储全部卫星观测值信息，没读到的卫星对应的元素转为字符后是""而不是null，绘图时选择从数据库提取，这样做
                //貌似多此一举，也可在过程中直接纳入绘图元素，但考虑到绘制多路径天空图，故一试（存入数据库经验证不到一秒钟）！
                //pk_ele.Rows[time][j]的行列序号均从0开始！
                DataTable pk = new DataTable();
                pk.Columns.Add("Epochs"); pk.Columns.Add("SV01"); pk.Columns.Add("SV02"); pk.Columns.Add("SV03"); pk.Columns.Add("SV04");
                pk.Columns.Add("SV05"); pk.Columns.Add("SV06"); pk.Columns.Add("SV07"); pk.Columns.Add("SV08"); pk.Columns.Add("SV09");
                pk.Columns.Add("SV10"); pk.Columns.Add("SV11"); pk.Columns.Add("SV12"); pk.Columns.Add("SV13"); pk.Columns.Add("SV14");
                pk.Columns.Add("SV15"); pk.Columns.Add("SV16"); pk.Columns.Add("SV17"); pk.Columns.Add("SV18"); pk.Columns.Add("SV19");
                pk.Columns.Add("SV20"); pk.Columns.Add("SV21"); pk.Columns.Add("SV22"); pk.Columns.Add("SV23"); pk.Columns.Add("SV24");
                pk.Columns.Add("SV25"); pk.Columns.Add("SV26"); pk.Columns.Add("SV27"); pk.Columns.Add("SV28"); pk.Columns.Add("SV29");
                pk.Columns.Add("SV30"); pk.Columns.Add("SV31"); pk.Columns.Add("SV32");

                string path_plotfile = destDir + "\\" + filename_without_extention + @"." + plotfile_type;
                if (File.Exists(path_plotfile))
                {
                    string[] readText = File.ReadAllLines(path_plotfile);
                    int k = 0, i0 = 4;
                    if (readText[0] == "COMPACT2") i0 = 3;

                    //保证只读取GPS结果且去除字符“G”
                    if (readText[i0].Contains("R")) readText[i0] = readText[i0].Substring(0, readText[i0].IndexOf("R") - 1);
                    if (readText[i0].Contains("G")) readText[i0] = readText[i0].Replace("G", "");
                    string[] sat_list = readText[i0].Substring(3, readText[i0].Length - 3).ToString().Split(' ');
                    int sat_num = sat_list.Count();

                    //////////////////////////
                    //以下为前8种文件绘图！//
                    /////////////////////////
                    if (Ifslip == false)
                    {
                        for (int i = i0 + 1; i < readText.Length; i++)
                        {
                            if (readText[i] != null)
                            {
                                k = k + 1;
                                DataRow dr = pk.NewRow();
                                dr["Epochs"] = k.ToString();
                                for (int kk = 0; kk < sat_num; kk++)
                                    dr["SV" + sat_list[kk]] = readText[i].Substring(kk * 10, 8).Trim();
                                pk.Rows.Add(dr);
                                i = i + 1;
                            }
                            if (i == readText.Length) break;
                            while (readText[i] == " 0")
                            {
                                DataRow dr = pk.NewRow();
                                k = k + 1;
                                dr["Epochs"] = k.ToString();
                                pk.Rows.Add(dr);
                                i = i + 1;
                            }
                            if (readText[i] != "-1")
                            {
                                //保证只读取GPS结果且去除字符“G”
                                if (readText[i].Contains("R")) readText[i] = readText[i].Substring(0, readText[i].IndexOf("R") - 1);
                                if (readText[i].Contains("G")) readText[i] = readText[i].Replace("G", "");
                                sat_list = readText[i].Substring(3, readText[i].Length - 3).ToString().Split(' ');
                                sat_num = sat_list.Count();
                            }
                        }

                        for (int i = 1; i <= 32; i++)
                        {
                            Series series = new Series();
                            if (i < 10) series.Name = "SV0" + i.ToString();
                            else series.Name = "SV" + i.ToString();
                            for (int s = 0; s < pk.Rows.Count; s++)
                            {
                                if (pk.Rows[s][i].ToString() != "")
                                    series.Points.AddXY(Convert.ToInt32(s + 1), Convert.ToDouble(pk.Rows[s][i].ToString()));
                            }
                            series.ChartType = SeriesChartType.Line;
                            //series.EmptyPointStyle = 
                            this.chart_line.Series.Add(series);
                            BATSeriesList.BATSeries.Add(series);
                        }
                        this.chart_line.ChartAreas[0].AxisX.MajorGrid.Interval = 500;
                        this.chart_line.ChartAreas[0].AxisX.MajorGrid.LineDashStyle = ChartDashStyle.Dash;
                        this.chart_line.ChartAreas[0].AxisY.MajorGrid.LineDashStyle = ChartDashStyle.Dash;
                        this.chart_line.ChartAreas[0].AxisX.Minimum = 0;
                        this.chart_line.ChartAreas[0].AxisX.Title = "观测历元";

                        if (plotfile_type == "ele") { chart_line.Titles.Add("卫星高度角图"); this.chart_line.ChartAreas[0].AxisY.Title = "高度角"; this.chart_line.ChartAreas[0].AxisY.Maximum = 90; }
                        else if (plotfile_type == "azi") { chart_line.Titles.Add("卫星方位角图"); this.chart_line.ChartAreas[0].AxisY.Title = "方位角"; }
                        else if (plotfile_type == "mp1") { chart_line.Titles.Add("L1多路径效应图"); this.chart_line.ChartAreas[0].AxisY.Title = "多路径误差/m"; }
                        else if (plotfile_type == "mp2") { chart_line.Titles.Add("L2多路径效应图"); this.chart_line.ChartAreas[0].AxisY.Title = "多路径误差/m"; }
                        else if (plotfile_type == "ion") { chart_line.Titles.Add("电离层延迟误差图"); this.chart_line.ChartAreas[0].AxisY.Title = "电离层延迟"; }
                        else if (plotfile_type == "iod") { chart_line.Titles.Add("电离层延迟变化率图"); this.chart_line.ChartAreas[0].AxisY.Title = "电离层延迟变化率"; }
                        else if (plotfile_type == "sn1") { chart_line.Titles.Add("信噪比SN1图"); this.chart_line.ChartAreas[0].AxisY.Title = "信噪比"; }
                        else if (plotfile_type == "sn2") { chart_line.Titles.Add("信噪比SN2图"); this.chart_line.ChartAreas[0].AxisY.Title = "信噪比"; }

                        chart_line.Titles[0].Font = new Font("宋体", 12, FontStyle.Regular);
                        chart_line.ChartAreas[0].AxisX.TitleFont = new Font("宋体", 12, FontStyle.Regular);
                        chart_line.ChartAreas[0].AxisY.TitleFont = new Font("宋体", 12, FontStyle.Regular);
                        //chart_line.ChartAreas[0].AxisX.Maximum = pk.Rows.Count;
                    }


                    //////////////////////////
                    //以下为周跳绘图！//
                    /////////////////////////
                    else
                    {
                        for (int i = i0 + 1; i < readText.Length; i++)
                        {
                            if (readText[i] != null)
                            {
                                k = k + 1;
                                DataRow dr = pk.NewRow();
                                dr["Epochs"] = k.ToString();
                                for (int kk = 0; kk < sat_num; kk++)
                                {
                                    if (readText[i].Substring(kk * 10, 9).Contains("I"))
                                        dr["SV" + sat_list[kk]] = sat_list[kk];
                                }
                                pk.Rows.Add(dr);
                                i = i + 1;
                            }
                            if (i == readText.Length) break;
                            while (readText[i] == " 0")
                            {
                                DataRow dr = pk.NewRow();
                                k = k + 1;
                                dr["Epochs"] = k.ToString();
                                pk.Rows.Add(dr);
                                i = i + 1;
                            }
                            if (readText[i] != "-1")
                            {
                                //保证只读取GPS结果且去除字符“G”
                                if (readText[i].Contains("R")) readText[i] = readText[i].Substring(0, readText[i].IndexOf("R") - 1);
                                if (readText[i].Contains("G")) readText[i] = readText[i].Replace("G", "");
                                sat_list = readText[i].Substring(3, readText[i].Length - 3).ToString().Split(' ');
                                sat_num = sat_list.Count();
                            }
                        }

                        for (int i = 1; i <= 32; i++)
                        {
                            Series series = new Series();
                            if (i < 10) series.Name = "SV0" + i.ToString();
                            else series.Name = "SV" + i.ToString();
                            for (int s = 0; s < pk.Rows.Count; s++)
                            {
                                if (pk.Rows[s][i].ToString() != "")
                                    series.Points.AddXY(Convert.ToInt32(s + 1), i);
                            }

                            series.ChartType = SeriesChartType.Point;
                            series.MarkerStyle = MarkerStyle.Triangle;
                            //series.MarkerColor = Color.Red;
                            series.MarkerSize = 12;
                            this.chart_point.Series.Add(series);
                            this.chart_point.ChartAreas[0].AxisX.MajorGrid.Interval = 500;
                            //this.chart_point.ChartAreas[0].AxisX.MajorGrid.;
                            this.chart_point.ChartAreas[0].AxisY.MajorGrid.Interval = 1;
                            this.chart_point.ChartAreas[0].AxisY.Interval = 1;


                            this.chart_point.ChartAreas[0].AxisX.MajorGrid.LineDashStyle = ChartDashStyle.Dash;
                            this.chart_point.ChartAreas[0].AxisY.MajorGrid.LineDashStyle = ChartDashStyle.Dash;
                            BATSeriesList.BATSeries.Add(series);
                        }
                        chart_point.ChartAreas[0].AxisX.Minimum = 0;
                        //chart_point.ChartAreas[0].AxisX.Maximum = pk.Rows.Count;
                        chart_point.ChartAreas[0].AxisY.Minimum = 0;
                        chart_point.ChartAreas[0].AxisY.Maximum = 32;

                        chart_point.Titles.Add("周跳探测图");
                        chart_point.Titles[0].Font = new Font("宋体", 16, FontStyle.Regular);
                        chart_point.ChartAreas[0].AxisX.Title = "观测历元";
                        chart_point.ChartAreas[0].AxisX.TitleFont = new Font("宋体", 12, FontStyle.Regular);
                        chart_point.ChartAreas[0].AxisY.Title = "卫星序列";
                        chart_point.ChartAreas[0].AxisY.TitleFont = new Font("宋体", 12, FontStyle.Regular);
                    }
                }

                else MessageBox.Show("绘图失败，请检查该类型文件是否存在！");

            }
        }

        private void button_lines_first_Click(object sender, EventArgs e)
        {
            this.chart_line.Series.Clear();
            this.chart_line.Series.Add(BATSeriesList.BATSeries[0]);
        }

        private void button_lines_before_Click(object sender, EventArgs e)
        {
            string now;
            if (this.chart_line.Series[0].Name == "SV01")
            {
                this.chart_line.Series.Clear();
                this.chart_line.Series.Add(BATSeriesList.BATSeries[31]);
            }
            else
            {
                now = this.chart_line.Series[0].Name.ToString().Substring(2, 2);
                this.chart_line.Series.Clear();
                if (now.Substring(0, 1) == "0")
                    this.chart_line.Series.Add(BATSeriesList.BATSeries[Convert.ToInt32(now.Substring(1, 1)) - 2]);
                else
                    this.chart_line.Series.Add(BATSeriesList.BATSeries[Convert.ToInt32(now.ToString()) - 2]);
            }
        }

        private void button_lines_next_Click(object sender, EventArgs e)
        {
            string now;
            if (this.chart_line.Series[0].Name == "SV32")
            {
                this.chart_line.Series.Clear();
                this.chart_line.Series.Add(BATSeriesList.BATSeries[0]);
            }
            else
            {
                now = this.chart_line.Series[0].Name.ToString().Substring(2, 2);
                this.chart_line.Series.Clear();
                if (now.Substring(0, 1) == "0")
                    this.chart_line.Series.Add(BATSeriesList.BATSeries[Convert.ToInt32(now.Substring(1, 1))]);
                else
                    this.chart_line.Series.Add(BATSeriesList.BATSeries[Convert.ToInt32(now.ToString())]);
            }
        }

        private void button_lines_all_Click(object sender, EventArgs e)
        {
            this.chart_line.Series.Clear();
            for (int i = 0; i < 32; i++)
                this.chart_line.Series.Add(BATSeriesList.BATSeries[i]);
        }

        private void button_lines_last_Click(object sender, EventArgs e)
        {
            this.chart_line.Series.Clear();
            this.chart_line.Series.Add(BATSeriesList.BATSeries[31]);
        }

        private void button_points_first_Click(object sender, EventArgs e)
        {
            this.chart_point.Series.Clear();
            this.chart_point.Series.Add(BATSeriesList.BATSeries[0]);
        }

        private void button_points_before_Click(object sender, EventArgs e)
        {
            string now;
            if (this.chart_point.Series[0].Name == "SV01")
            {
                this.chart_point.Series.Clear();
                this.chart_point.Series.Add(BATSeriesList.BATSeries[31]);
            }
            else
            {
                now = this.chart_point.Series[0].Name.ToString().Substring(2, 2);
                this.chart_point.Series.Clear();
                if (now.Substring(0, 1) == "0")
                    this.chart_point.Series.Add(BATSeriesList.BATSeries[Convert.ToInt32(now.Substring(1, 1)) - 2]);
                else
                    this.chart_point.Series.Add(BATSeriesList.BATSeries[Convert.ToInt32(now.ToString()) - 2]);
            }
        }

        private void button_points_next_Click(object sender, EventArgs e)
        {
            string now;
            if (this.chart_point.Series[0].Name == "SV32")
            {
                this.chart_point.Series.Clear();
                this.chart_point.Series.Add(BATSeriesList.BATSeries[0]);
            }
            else
            {
                now = this.chart_point.Series[0].Name.ToString().Substring(2, 2);
                this.chart_point.Series.Clear();
                if (now.Substring(0, 1) == "0")
                    this.chart_point.Series.Add(BATSeriesList.BATSeries[Convert.ToInt32(now.Substring(1, 1))]);
                else
                    this.chart_point.Series.Add(BATSeriesList.BATSeries[Convert.ToInt32(now.ToString())]);
            }
        }

        private void button_points_all_Click(object sender, EventArgs e)
        {
            this.chart_point.Series.Clear();
            for (int i = 0; i < 32; i++)
                this.chart_point.Series.Add(BATSeriesList.BATSeries[i]);
        }

        private void button_points_last_Click(object sender, EventArgs e)
        {
            this.chart_point.Series.Clear();
            this.chart_point.Series.Add(BATSeriesList.BATSeries[31]);
        }

        private void button_save_Click(object sender, EventArgs e)
        {
            SaveFileDialog sfd = new SaveFileDialog();
            sfd.Filter = "JPEG文件|*.jpg|BMP文件|*.bmp|PNG文件|*.png";
            if (sfd.ShowDialog() == DialogResult.OK)
            {
                if (this.tabControl1.SelectedIndex == 0)
                {
                    if (sfd.FilterIndex == 1) chart_line.SaveImage(sfd.FileName, System.Drawing.Imaging.ImageFormat.Jpeg);
                    else if (sfd.FilterIndex == 2) chart_line.SaveImage(sfd.FileName, System.Drawing.Imaging.ImageFormat.Bmp);
                    else chart_line.SaveImage(sfd.FileName, System.Drawing.Imaging.ImageFormat.Png);
                }
                else
                {
                    if (sfd.FilterIndex == 1) chart_point.SaveImage(sfd.FileName, System.Drawing.Imaging.ImageFormat.Jpeg);
                    else if (sfd.FilterIndex == 2) chart_point.SaveImage(sfd.FileName, System.Drawing.Imaging.ImageFormat.Bmp);
                    else chart_point.SaveImage(sfd.FileName, System.Drawing.Imaging.ImageFormat.Png);
                }
            }
        }

        private void checkBox1_MouseClick(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            if (checkBox1.Checked) this.checkBox2.Checked = false;
        }

        private void checkBox2_MouseClick(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            if (checkBox2.Checked) this.checkBox1.Checked = false;
        }

        /// <summary>
        /// 在chart控件的GetToolTipText事件中对当前鼠标对应点的周跳信息进行判断，
        /// 程序中0、1表示顺序，类似{0:F3}中F3为float型,且格式必须为dp.XValue和dp.YValues[0]！
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void chart_line_GetToolTipText(object sender, ToolTipEventArgs e)
        {
            //不知为何鼠标悬停时未显示坐标，可能跟图像为曲线图有关
            if (e.HitTestResult.ChartElementType == ChartElementType.DataPoint)
            {
                int i = e.HitTestResult.PointIndex;
                DataPoint dp = e.HitTestResult.Series.Points[i];
                e.Text = string.Format("观测历元：{0}\r\n卫星观测值：{1:F}", dp.XValue, dp.YValues[0]);
            }
        }

        private void chart_point_GetToolTipText(object sender, ToolTipEventArgs e)
        {
            if (e.HitTestResult.ChartElementType == ChartElementType.DataPoint)
            {
                int i = e.HitTestResult.PointIndex;
                DataPoint dp = e.HitTestResult.Series.Points[i];
                if (dp.YValues[0] < 10) e.Text = string.Format("卫星SV0{0}在第{1}个历元处发生周跳！", Convert.ToInt32(dp.YValues[0]), dp.XValue);
                else e.Text = string.Format("卫星SV{0}在第{1}个历元处发生周跳！", Convert.ToInt32(dp.YValues[0]), dp.XValue);
            }
        }

        private void radioButton_ele_MouseClick(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            if (this.radioButton_ele.Checked)
            {
                this.tabControl1.SelectedIndex = 0;
                if (this.checkBox1.Checked || this.checkBox2.Checked)
                {
                    this.checkBox1.Checked = false;
                    this.checkBox2.Checked = false;
                }
            }
        }

        private void radioButton_azi_MouseClick(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            if (this.radioButton_azi.Checked)
            {
                this.tabControl1.SelectedIndex = 0;
                if (this.checkBox1.Checked || this.checkBox2.Checked)
                {
                    this.checkBox1.Checked = false;
                    this.checkBox2.Checked = false;
                }
            }
        }

        private void radioButton_mp1mp2_MouseClick(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            this.tabControl1.SelectedIndex = 0;
        }

        private void radioButton_ioniod_MouseClick(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            this.tabControl1.SelectedIndex = 0;
        }

        private void radioButton_sn1sn2_MouseClick(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            this.tabControl1.SelectedIndex = 0;
        }
    }
}
