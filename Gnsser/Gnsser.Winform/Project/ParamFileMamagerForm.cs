//2015.11.10, czs, create in 彭州, 参数文件管理窗口

using System;
using System.IO;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Gnsser.Api;
using Geo.Winform;
using Geo;

namespace Gnsser.Winform
{

    public partial class ParamFileMamagerForm : Form, IWithMainForm
    {
        public ParamFileMamagerForm()
        {
            InitializeComponent();
            Init();
        }

        private void Init()
        {
            var project = Setting.GnsserConfig.CurrentProject;
            ParamDirectory = project.GetAbsPath(project.ParamDirectory);

            this.ParamFiles = Directory.GetFiles(ParamDirectory, "*.param", SearchOption.TopDirectoryOnly);

            this.listView1.Items.Clear();

            foreach (var file in ParamFiles)
            {
                var name = Path.GetFileName(file);
                List<string> showItems = new List<string>();
                showItems.Add(name);
                showItems.Add("1");
                ListViewItem item = new ListViewItem(showItems.ToArray());
                item.Tag = file;
                this.listView1.Items.Add(item);
            }
        }
    
        public IMainForm MainForm { get; set; } 
        /// <summary>
        /// 工程项目
        /// </summary>
        public string ParamDirectory { get; set; }

        string [] ParamFiles { get; set; } 
         
         
        private void listView1_SelectedIndexChanged(object sender, EventArgs e)
        { 
            foreach (ListViewItem item in this.listView1.SelectedItems)
            { 
                //只设置第一个。
                OperationInfo info = new OperationInfo("Temp.gof", (string)item.Tag);
                this.paramEditControl1.SetOperationInfo(info);
                break;
            }  
        }


        private void toolStripButton_openOutSide_Click(object sender, EventArgs e)
        { 
            var obj = Geo.Utils.ListViewUtil.GetSelectedObject<string>(this.listView1);
            if (obj == null)
            {
                MessageBox.Show("请选中后再试！");
                return;
            }
            var path = Setting.GnsserConfig.CurrentProject.GetAbsPath(obj);
            if (System.IO.File.Exists(path))
            {
                Geo.Utils.FileUtil.OpenFile(path);
            }
            else
            {
                MessageBox.Show("文件不存在，请检查路径。" + path);
            }
        }

        private void toolStripButtonDelete_Click(object sender, EventArgs e)
        {
            var obj = Geo.Utils.ListViewUtil.GetSelectedObject<string>(this.listView1);
            if (obj == null)
            {
                MessageBox.Show("请选中后再试！");
                return;
            }
            var path = Setting.GnsserConfig.CurrentProject.GetAbsPath(obj);
            if (System.IO.File.Exists(path))
            {
                Geo.Utils.FileUtil.TryDeleteFileOrDirectory(path);
                Init();
            }
            else
            {
                MessageBox.Show("文件不存在，请检查路径。" + path);
            }
        }

        private void toolStripButtonRefresh_Click(object sender, EventArgs e)
        {
            Init();
        }

        private void 外部打开OToolStripButton_Click(object sender, EventArgs e)
        {
            var project = Setting.GnsserConfig.CurrentProject;
            ParamDirectory = project.GetAbsPath(project.ParamDirectory);

            Geo.Utils.FileUtil.OpenDirectory(ParamDirectory);
        }

    }
}
