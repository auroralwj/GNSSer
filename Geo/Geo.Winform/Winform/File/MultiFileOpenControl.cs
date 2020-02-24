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
    public partial class MultiFileOpenControl : UserControl
    {
        public MultiFileOpenControl()
        {
            InitializeComponent();
        }

        public event EventHandler FilePathSet;

        private void button_setPath_Click(object sender, EventArgs e)
        {
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                this.textBox_filepath.Lines = openFileDialog1.FileNames;
                if (FilePathSet != null) FilePathSet(sender, e);
            }
        }
        #region 基本属性
        /// <summary>
        /// 文件读取对话框
        /// </summary>
        public OpenFileDialog OpenFileDialog { get { return openFileDialog1; } }
        /// <summary>
        /// 是否多选文件
        /// </summary>
        public bool MultiSelect { get { return openFileDialog1.Multiselect; } set { openFileDialog1.Multiselect = value; } }

        /// <summary>
        /// 文件名称标签名称
        /// </summary>
        public string LabelName { get { return this.label_fileName.Text; } set { this.label_fileName.Text = value; } }

        /// <summary>
        /// 文件路径
        /// </summary>
        public string[] FilePaths { get { return this.textBox_filepath.Lines; } set { this.textBox_filepath.Lines = value; } }

        /// <summary>
        /// 文件过滤器
        /// </summary>
        public string Filter { get { return openFileDialog1.Filter; } set { openFileDialog1.Filter = value; } }
        #endregion
         
    }
}
