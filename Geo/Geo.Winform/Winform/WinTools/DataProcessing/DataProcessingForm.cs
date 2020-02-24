using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;  
using Geo.Coordinates;  
using Geo.Referencing;
using Geo.Algorithm.Adjust;
using Geo.Utils;
using Geo.Times;  
using Geo.IO;

namespace Geo.WinTools
{
    /// <summary>
    /// 数据输入，输出。
    /// </summary>
    public partial class DataProcessingForm : Form
    {
        ILog log = Log.GetLog(typeof(DataProcessingForm));
        /// <summary>
        /// 数据处理
        /// </summary>
        public DataProcessingForm()
        {
            InitializeComponent();
        }

        LogWriter LogWriter = LogWriter.Instance;

        void LogWriter_MsgProduced(string msg, LogType LogType, Type msgProducer)
        {
            //if (IsShowProcessInfo)// && LogType != Geo.IO.LogType.Debug)
            {
                var info = LogType.ToString() + "\t" + msg;// +"\t" + msgProducer.Name;
                ShowInfo(info);
            }
        }
        private void button_run_Click(object sender, EventArgs e)
        {
            this.button_run.Enabled = false;
            Run();
            this.button_run.Enabled = true;
        }
        /// <summary>
        /// 运行
        /// </summary>
        protected virtual void Run()
        {
            throw new NotImplementedException();
        }
        /// <summary>
        /// 数据列表
        /// </summary>
        public List<double> Values { get { return new List<double>(IndexedValues.Values); } }

        /// <summary>
        /// 具有检索的数据
        /// </summary>
        public Dictionary<double, double> IndexedValues
        {
            get
            {
                try
                {

                    var lines = InputLines;
                    if (HasIndexColumn)
                    {
                        List<double[]> doubles = Utils.DoubleUtil.ParseTable(lines);
                        return DoubleUtil.ToDic(doubles);
                    }
                    else
                    {
                        Dictionary<double, double> dic = new Dictionary<double, double>();
                        int len = lines.Length;
                        for (int i = 0; i < len; i++)
                        {
                            dic[i] = DoubleUtil.TryParse(StringUtil.TrimBlank(lines[i]));
                        }
                        return dic;
                    }
                }
                catch (Exception ex) { Geo.Utils.FormUtil.ShowErrorMessageBox("数据解析错误！" + ex.Message); return new Dictionary<double, double>(); }
            }
        }

        /// <summary>
        /// 是否具有列编号
        /// </summary>
        public bool HasIndexColumn { get { return this.checkBox_hasRowId.Checked; } }

        /// <summary>
        /// 输入行。
        /// </summary>
        public string[] InputLines { get { return richTextBox_input.Lines; } set { richTextBox_input.Lines = value; } }
        /// <summary>
        /// 输出行。
        /// </summary>
        public string[] OutputLines { get { return richTextBox_output.Lines; } set { richTextBox_output.Lines = value; } }
        /// <summary>
        /// 文本输出
        /// </summary>
        public string OutputText { get { return richTextBox_output.Text; } set { richTextBox_output.Text = value; } }


        /// <summary>
        /// 显示结果，并激活窗口
        /// </summary>
        /// <param name="msg"></param>
        protected void ShowResult(string msg)
        {
            //if (this.IsClosed) { return; }
            this.Invoke(new Action(delegate()
            {
                this.tabControl1.SelectedTab = this.tabPage_result;
            }));
            FormUtil.ShowNotice(this.richTextBox_output, msg);
        }
        private void DataProcessingForm_Load(object sender, EventArgs e)
        {
            LogWriter.MsgProduced += LogWriter_MsgProduced;
        }


        /// <summary>
        /// 在输出框显示信息。
        /// </summary>
        /// <param name="msg"></param>
        protected void ShowInfo(string msg)
        {
            //if (this.IsClosed || !IsShowInfo) { return; }
            FormUtil.InsertLineWithTimeToTextBox(this.richTextBoxControl_info, msg);
        }

        private void LogListeningForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            LogWriter.MsgProduced -= LogWriter_MsgProduced;
            //IsClosed = true;
        }

        private void button_draw_Click(object sender, EventArgs e)
        {
            try
            {

                if (this.HasIndexColumn)
                {
                    List<double[]> doubles = Utils.DoubleUtil.ParseTable(this.InputLines);
                    List<double[]> outdoubles = Utils.DoubleUtil.ParseTable(this.OutputLines);
                    Dictionary<Double, Double> dic = DoubleUtil.ToDic(doubles);
                    var inputs = DoubleUtil.ToDic(doubles);
                    var outputs = DoubleUtil.ToDic(outdoubles);


                    List<Dictionary<Double, Double>> lists = new List<Dictionary<double, double>>();

                    List<string> names = new List<string>();
                    if (inputs != null && inputs.Count > 0)
                    {
                        lists.Add(inputs);
                        names.Add("Input");
                    }
                    if (outputs != null && outputs.Count > 0)
                    {
                        lists.Add(outputs);
                        names.Add("Output");
                    }

                    if (lists.Count > 0)
                    {
                        var form = new Geo.Winform.CommonChartForm(lists, names);
                        form.Text = "Data";
                        form.Show();
                    }
                }
                else
                {
                    var inputs = (Utils.DoubleUtil.ParseLines(this.InputLines));
                    var outputs = (Utils.DoubleUtil.ParseLines(this.OutputLines));
                    List<IEnumerable<Double>> lists = new List<IEnumerable<double>>();
                    List<string> names = new List<string>();
                    if (inputs != null && inputs.Length > 0)
                    {
                        lists.Add(inputs);
                        names.Add("Input");
                    }
                    if (outputs != null && outputs.Length > 0)
                    {
                        lists.Add(outputs);
                        names.Add("Output");
                    }

                    if (lists.Count > 0)
                    {
                        var form = new Geo.Winform.CommonChartForm(lists, names);
                        form.Text = "Data";
                        form.Show();
                    }
                }
            }
            catch (Exception ex) { Geo.Utils.FormUtil.ShowErrorMessageBox("绘图发生错误！" + ex.Message); }
        }

        private void button_setInputToFraction_Click(object sender, EventArgs e)
        {
            if (HasIndexColumn)
            {
                Dictionary<double, double> data = new Dictionary<double, double>();
                var oldValues = IndexedValues;
                int firstInt = (int)(oldValues.ElementAt(0).Value);
                foreach (var item in oldValues)
                {
                    data[item.Key] = item.Value - firstInt;
                }
                this.InputLines = Utils.DoubleUtil.ToStringLines(data).ToArray();
            }
            else
            {
                if (Values.Count == 0) { return; }
                var oldValues = Values;
                int firstInt = (int)(oldValues.ElementAt(0));
                var data = new List<double>();
                foreach (var item in oldValues)
                {
                    data.Add(item - firstInt);
                }
                this.InputLines = Utils.DoubleUtil.ToStringLines(data).ToArray();
            }
        }
    }
}