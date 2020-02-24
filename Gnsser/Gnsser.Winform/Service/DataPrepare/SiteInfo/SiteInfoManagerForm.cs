//2017.07.25, czs, create in honqging, 测站信息维护窗口

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Geo.IO;
using Geo.Algorithm.Adjust;
using Geo.Utils;
using Geo.Common;
using Geo.Coordinates; 
using Gnsser.Service; 
using Gnsser.Data;
using Gnsser.Times;
using Gnsser.Data.Rinex;
using Geo; 
using Geo.Times;
using Gnsser.Data.Sinex;
using Geo.Winform;

namespace Gnsser.Winform
{
    public partial class SiteInfoManagerForm : Form
    {
        Log log = new Log(typeof(SiteInfoManagerForm)); 
        public SiteInfoManagerForm()
        {
            InitializeComponent();
        }

        public string CurrentPath { get; set; }

        private void 打开OToolStripButton_Click(object sender, EventArgs e)
        {
            if (openFileDialog1.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                CurrentPath = openFileDialog1.FileName;
                ReadFile(CurrentPath);
            }
        }

        private void 新建NToolStripButton_Click(object sender, EventArgs e)
        {
            var form = new ObjectEditForm<StationInfo>();          
            if (form.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                 var o= form.UiToObj();

                 var all = bindingSource1.DataSource as List<StationInfo>;
                 if (all == null) { return; }
                 
                 all.Add(o);
                 DataBind(all);
            } 
        }

        private void toolStripButton1saveas_Click(object sender, EventArgs e)
        {
            if (saveFileDialog1.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            { 
                Save(saveFileDialog1.FileName);
            }

        }
        private void 保存SToolStripButton_Click(object sender, EventArgs e)
        {
            Save(CurrentPath);
        }

        private void Save(string CurrentPath)
        {
            StationInfoWriter writer = new StationInfoWriter(CurrentPath);
            var collection = this.bindingSource1.DataSource as List<StationInfo>;
            writer.WriteCommentLine(StaionInfoService.reader.Comments);
            foreach (var item in collection)
            {
                writer.Write(item);
            }
            writer.Close();
            writer.Dispose();
            Geo.Utils.FormUtil.ShowOkAndOpenFile(CurrentPath);
        }

        private void toolStripButton编辑_Click(object sender, EventArgs e)
        {
            var current = this.bindingSource1.Current as StationInfo;
            if (current == null) { return; }

            var form = new ObjectEditForm<StationInfo>();
            form.ObjToUi(current);
            if(form.ShowDialog() == System.Windows.Forms.DialogResult.OK){
                form.UiToObj(current);
                this.dataGridView1.Refresh();
            }
           
        }

        private void toolStripButton删除_Click(object sender, EventArgs e)
        {
            var selected = Geo.Utils.DataGridViewUtil.GetSelectedObjects<StationInfo>(this.dataGridView1); 
            var all = bindingSource1.DataSource as List<StationInfo>;
            if (selected == null || all == null) { return; } 
            all.RemoveAll(m => selected.Contains(m));
            DataBind(all);
        }

        private void toolStripButton搜索_Click(object sender, EventArgs e)
        {

            SimpleSearchForm form = new SimpleSearchForm();
            if (form.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
               var conditons =  form.SimpleSearchCondition;
               var all = bindingSource1.DataSource as List<StationInfo>;
               List<StationInfo> result = all;

               if (conditons.IsFuzzyMathching)
               {
                   result = all.FindAll(m => m.SiteName.Contains(conditons.Word) || m.StationName.Contains(conditons.Word));
               }
               else
               {
                   result = all.FindAll(m => String.Equals(conditons.Word, m.SiteName, StringComparison.CurrentCultureIgnoreCase) || String.Equals(conditons.Word, m.StationName, StringComparison.CurrentCultureIgnoreCase));
               }
               if (!conditons.IsIncludingOrNot)
               {
                   result = all.FindAll(m => !result.Contains(m));
               } 
                DataBind(result); 
            }
             
        }

        private void SiteInfoManagerForm_Load(object sender, EventArgs e)
        {
            CurrentPath = Setting.GnsserConfig.StationInfoPath;
            ReadFile(CurrentPath);
        }
        StaionInfoService StaionInfoService;
        private void ReadFile(string path)
        {
            if (Geo.Utils.FileUtil.Exists(path))
            {
                StaionInfoService = new StaionInfoService(path);
                var list =  StaionInfoService.GetAll();
                DataBind(list);
            }
            else
            {
                log.Info("文件不存在： " + path);
            }
        }

        private void DataBind(List<StationInfo> list)
        {
            this.bindingSource1.DataSource = null;
            this.bindingSource1.DataSource = list;
        }

    }
}
