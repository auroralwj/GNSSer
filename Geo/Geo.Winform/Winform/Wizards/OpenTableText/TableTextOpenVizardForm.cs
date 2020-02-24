//2017.07.31, czs, create in hongqing, 文本表数据打开向导


using System;
using System.Collections.Generic;
using System.Linq;
using System.Text; 
using System.ComponentModel;
using System.Data;
using System.Drawing; 
using System.Windows.Forms; 
using Geo.Winform.Demo;
using Geo.Winform.Wizards;
using Geodesy.Winform;
using Geo.Coordinates;
using Geo;
using Geo.Algorithm;
using Geo.IO;
using System.Net;  
using Gnsser;


namespace Gnsser.Winform
{
    /// <summary>
    /// 文本表数据打开向导
    /// </summary>
    public class TableTextOpenVizardForm : WizardForm
    {
        Log log = new Log(typeof(TableTextOpenVizardForm));
        /// <summary>
        /// 构造函数
        /// </summary>
        public TableTextOpenVizardForm()
        {
            InitializeComponent();
            FinalStepText = "执行";
            StringWizardPage = new StringWizardPage();
            StringWizardPage.Init("分隔符号", ",\r\n\t\r\n;", "按行输入分隔符，一行一个" , true);

            FileOpenWizardPage = new FileOpenWizardPage();
            FileOpenWizardPage.Init("打开文件", Setting.TextTableFileFilter);
             
            //ProgressBarWizardPage.Init("执行进度", 1);

            var WizardPages = new WizardPageCollection();
            WizardPages.Add(1, StringWizardPage);
            WizardPages.Add(2, FileOpenWizardPage); 


            this.Init(WizardPages);
        }         

        StringWizardPage StringWizardPage;
        
        FileOpenWizardPage FileOpenWizardPage;
                 
        protected override void OnWizardPageLocationChanged(WizardPageLocationChangedEventArgs e)
        {     
            base.OnWizardPageLocationChanged(e);
        }


        public void ReadAio()
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Glog文本表格文件(*.txt;*.txt.xls;*.txt;*.xls)|*.glog;*.txt.xls;*.txt;*.xls|所有文件(*.*)|*.*";
            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                var path = openFileDialog.FileName;
                var reader = new ObjectTableManagerReader(path, Encoding.Default);
                reader.Spliters = new string[] { ",", "\t" };
                reader.HeaderMarkers = new string[] { "#" };
                var tables = reader.Read();//.GetDataTable();  
                var fileName = System.IO.Path.GetFileName(path);

                foreach (var table in tables)
                {
                    //var form = new Geo.Winform.DataTableViewForm(table) { Text = table.Name };
                    //form.Show();
                    var name = table.Name;
                  //  OpenMidForm(new DataTableViewForm(table));
                }
            }
        }
          

        protected override void OnWizardCompleted()
        {
            base.OnWizardCompleted();
            this.Enabled = false;
            var filePath = FileOpenWizardPage.FilePath;
            var spliter = StringWizardPage.Lines;
            var reader = new ObjectTableReader(filePath, Encoding.Default);
            reader.Spliters = spliter;
            var table = reader.Read();//.GetDataTable();  
            var fileName = System.IO.Path.GetFileName(filePath);

            var form = new Geo.Winform.TableObjectViewForm(table) { Text = fileName };
            form.Show();

            this.Invoke(new Action(delegate()
            {
                this.DialogResult = System.Windows.Forms.DialogResult.OK;
                this.Close();
            }));      

          //  OpenMidForm();
        }         


        private void InitializeComponent()
        { 
            this.SuspendLayout();
           // 
            // PointPositionVizardForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.ClientSize = new System.Drawing.Size(605, 423);
            this.Name = "TableTextOpenVizardForm";
            this.ShowFirstButton = true;
            this.ShowLastButton = true;
            this.Text = "文本表数据打开向导";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.SinexImportVizardForm_FormClosing);
            this.ResumeLayout(false);

        }

        private void SinexImportVizardForm_FormClosing(object sender, FormClosingEventArgs e)
        { 
        } 
    }
}
