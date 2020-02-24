using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Geo.Winform.Controls;
using Geo.Utils;

namespace Geo.WinTools
{
    /// <summary>
    /// 文件执行器
    /// </summary>
    public partial class FileExecutingForm : Form
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public FileExecutingForm()
        {
            InitializeComponent();
        }

        private void button_run_Click(object sender, EventArgs e)
        {
            if (!File.Exists(InputPath))
            {
                MessageBox.Show("输入文件不存在！"); return;
            }
            if (OutputPath == "")
            {
                MessageBox.Show("请设置输出文件"); return;
            }

            this.EnableButtom = false;

            Run(InputPath, OutputPath);
 
            this.EnableButtom = true; 
        }
        /// <summary>
        /// 启用按钮
        /// </summary>
        public bool EnableButtom { get { return this.button_run.Enabled; } set { this.button_run.Enabled = value; } }
        /// <summary>
        /// 打开文件控件
        /// </summary>
        protected FileOpenControl FileOpenControl { get { return fileOpenControl1; } }
        /// <summary>
        /// 文件输出控件
        /// </summary>
        protected FileOutputControl FileOutputControl { get { return fileOutputControl1; } }
        /// <summary>
        /// 输入路径
        /// </summary>
        public string InputPath { get { return fileOpenControl1.FilePath.Trim(); } }
        /// <summary>
        /// 输出路径
        /// </summary>
        public string OutputPath { get { return fileOutputControl1.FilePath.Trim(); } }

        /// <summary>
        /// 是否忽略第一列
        /// </summary>
        protected bool IsIgnoreFirstCol
        {
            get
            {
                return checkBox_ignoreFirstCol.Checked;
            }
        }
        /// <summary>
        /// 是否忽略第一行
        /// </summary>
        protected bool IsIgnoreFirstRow
        {
            get
            {
                return checkBox_ignoreFirstRow.Checked;
            }
        }
        /// <summary>
        /// 显示信息
        /// </summary>
        /// <param name="info"></param>
        public void ShowInfo(string info)
        {
            this.label_info.Text = info;
        }
        #region 读取文件
        /// <summary>
        /// 返回指定的数据列
        /// </summary>
        /// <param name="colIndex">列号</param>
        /// <param name="table">列号</param>
        /// <returns></returns>
        public List<double> GetInputDoubleCol(int colIndex, List<double[]> table)
        {
            List<double> col = new List<double>(); 
            foreach (var item in table)
            {
                col.Add(item[colIndex]);
            }
            return col;
        }
        /// <summary>
        /// 以列形式返回
        /// </summary>
        /// <returns></returns>
        public List<List<double>> GetInputDoubleCols()
        {
            List<List<double>> cols = new List<List<double>>();
            List<double[]> table = GetInputDoubleRows();
            int colCount = table[0].Length;
            for (int i = 0; i < colCount; i++)
            {
                cols.Add(GetInputDoubleCol(i, table));
            } 
            return cols;
        }

        /// <summary>
        /// 读取双精度数组集合。只返回数据部分，可设置是否忽略首行首列。
        /// </summary>
        /// <returns></returns>
        public List<double[]> GetInputDoubleRows()
        {
            string[] inputs = GetInputLines();
            return DoubleUtil.ParseTable(inputs, IsIgnoreFirstRow, IsIgnoreFirstCol); 
        }


        /// <summary>
        /// 获取行列表，每一行为一个双精度数字。
        /// </summary>
        /// <returns></returns>
        public List<Double> GetInputDoubles()
        {
            string[] inputs = GetInputLines();
            int length = inputs.Length;
            List<Double> doubles = new List<double>();
            for (int i = 0; i < length; i++)
            {
                if (inputs[i].Trim() == "") continue;
                doubles.Add(Double.Parse(inputs[i]));
            }
            return doubles;
        }

        /// <summary>
        /// 输入文件的行。原始行，不做任何更改。
        /// </summary>
        /// <returns></returns>
        public string[] GetInputLines()
        {
            string[] inputs = this.FileOpenControl.ReadAllLines();
            return inputs;
        }
        #endregion

        /// <summary>
        /// 附加到文件
        /// </summary>
        /// <param name="text"></param>
        protected void AppendToOutFile(string text)
        {
            File.AppendAllText(OutputPath, text, Encoding.Default);
        }
        /// <summary>
        /// 追加
        /// </summary>
        /// <param name="text"></param>
        protected void AppendLineToOutFile(string text)
        {
            AppendToOutFile(text + "\r\n");
        }
        /// <summary>
        /// 删除输出文件。
        /// </summary>
        protected void TryDeleteOutputFile()
        {
            try
            {
                if (File.Exists(OutputPath)) File.Delete(OutputPath);
            }
            catch (Exception ex)
            {
                MessageBox.Show("删除文件时错误，"+ OutputPath + ". " + ex.Message);
            }
        }
        /// <summary>
        /// 运行
        /// </summary>
        /// <param name="input"></param>
        /// <param name="output"></param>
        public virtual void Run(string input, string output)
        {

        }

        private void fileOpenControl1_FilePathSetted(object sender, EventArgs e)
        {
            if( String.IsNullOrWhiteSpace( fileOutputControl1.FilePath))
            {
                var inPath = fileOpenControl1.FilePath;              
                var fileName = "执行后_" + Path.GetFileName(inPath);
                var dir = Path.GetDirectoryName(inPath);
                fileOutputControl1.FilePath = Path.Combine(dir, fileName); 
            }

        }
    }
}
