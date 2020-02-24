//2015.06.03, czs, 

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;

namespace Geo.Winform
{
    public delegate bool FileOpenReadyEventHander(Controls.FileOpenControl fileOpenControl);

    public partial class FileOpenForm : Form
    {
        public event FileOpenReadyEventHander FileOpenReady;
        public FileOpenForm()
        {
            InitializeComponent();
        }
        public FileOpenForm(string Filter)
        {
            InitializeComponent();
            this.Filter = Filter;
        }

        public string Filter { get { return this.fileOpenControl1.Filter; } set {   this.fileOpenControl1.Filter= value; }}
        /// <summary>
        /// 文件打开控件
        /// </summary>
        public Controls.FileOpenControl FileOpenControl { get { return this.fileOpenControl1; } }

        private void button_read_Click(object sender, EventArgs e)
        {
            if (!File.Exists(FileOpenControl.FilePath))
            {
                Geo.Utils.FormUtil.ShowWarningMessageBox("路径不存在，请设置后再试。");
                return;
            }

            bool result = true;
            if (FileOpenReady != null)
            {
                result = FileOpenReady(this.FileOpenControl);
            }

            if (result)
            {
                this.DialogResult = System.Windows.Forms.DialogResult.OK;
                this.Close();
            }
            else
            {
                this.DialogResult = System.Windows.Forms.DialogResult.No;
            }
        }

        private void button_cancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.Close();
        }
    }
}
