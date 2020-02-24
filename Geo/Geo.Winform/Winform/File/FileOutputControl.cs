using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.IO;
using System.Text;
using System.Windows.Forms;

namespace Geo.Winform.Controls
{
    /// <summary>
    /// 打开文件控件
    /// </summary>
    public partial class FileOutputControl : UserControl
    {
        public FileOutputControl()
        {
            InitializeComponent();
        }

        private void button_setPath_Click(object sender, EventArgs e)
        {
            if (this.saveFileDialog1.ShowDialog() == DialogResult.OK)
                this.textBox_filepath.Text = saveFileDialog1.FileName;
        }
        #region 基本属性
        /// <summary>
        /// 文件名称标签名称
        /// </summary>
        public string LabelName { get { return this.label_fileName.Text; } set { this.label_fileName.Text = value; } }

        /// <summary>
        /// 文件路径
        /// </summary>
        public string FilePath { get { return this.textBox_filepath.Text; } set { this.textBox_filepath.Text = value; } }

        /// <summary>
        /// 文件过滤器
        /// </summary>
        public string Filter { get { return saveFileDialog1.Filter; } set { saveFileDialog1.Filter = value; } }
        #endregion

        #region 扩展方法
        public void ReadAllLines(string[] lines)
        {
            File.WriteAllLines(FilePath, lines);
        }
        public void WriteAllText(string text)
        {
            File.WriteAllText(FilePath, text);
        } 
        public FileStream OpenWrite()
        {
            return File.OpenWrite(FilePath);
        } 
        #endregion
    }
}
