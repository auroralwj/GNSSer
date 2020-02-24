//2017.08.01, czs, create in hongqing, 单文件多表数据打开向导


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
    /// 单文件多表数据打开向导
    /// </summary>
    public class MultiTableTextOpenVizardForm : WizardForm
    {
        Log log = new Log(typeof(MultiTableTextOpenVizardForm ));
        public MultiTableTextOpenVizardForm()
        {
            InitializeComponent();
            FinalStepText = "执行";
            HeaderMarkersWizardPage = new StringWizardPage();
            HeaderMarkersWizardPage.Init("头部标记符号", "#", "按行输入分隔符，一行一个", true);


            StringWizardPage = new StringWizardPage();
            StringWizardPage.Init("分隔符号", ",\r\n\t\r\n;", "按行输入分隔符，一行一个" , true);

            FileOpenWizardPage = new FileOpenWizardPage();
            FileOpenWizardPage.Init("打开文件", Setting.TextTableFileFilter);

            CheckboxWizardPage = new Geo.Winform.Wizards.CheckboxWizardPage();
            CheckboxWizardPage.Init<AioFileOpenType>("文件打开动作");

            //ProgressBarWizardPage.Init("执行进度", 1);
            int index = 1;
            var WizardPages = new WizardPageCollection();
            WizardPages.Add(index++, HeaderMarkersWizardPage);
            WizardPages.Add(index++, StringWizardPage);
            WizardPages.Add(index++, FileOpenWizardPage);
            WizardPages.Add(index++, CheckboxWizardPage); 


            this.Init(WizardPages);
        }

        private enum AioFileOpenType
        {
            转换为单表并保存,
            在新窗口中查看分析
        }

        StringWizardPage HeaderMarkersWizardPage;
        StringWizardPage StringWizardPage;
        FileOpenWizardPage FileOpenWizardPage;
        CheckboxWizardPage CheckboxWizardPage;
                 
        protected override void OnWizardPageLocationChanged(WizardPageLocationChangedEventArgs e)
        {     
            base.OnWizardPageLocationChanged(e);
        }

        protected override void OnWizardCompleted()
        {
            base.OnWizardCompleted();
            this.Enabled = false;
            var filePath = FileOpenWizardPage.FilePath;
            var spliter = StringWizardPage.Lines;
            var AioFileOpenTypes = CheckboxWizardPage.GetSelected<AioFileOpenType>();

            var reader = new ObjectTableManagerReader(filePath, Encoding.Default);
            reader.Spliters = spliter;
            reader.HeaderMarkers = HeaderMarkersWizardPage.Lines;
            var tables = reader.Read();//.GetDataTable();  
            var fileName = System.IO.Path.GetFileName(filePath);

            if (AioFileOpenTypes.Contains(AioFileOpenType.转换为单表并保存))
            {
                tables.WriteAllToFileAndCloseStream();
            }


            if (AioFileOpenTypes.Contains(AioFileOpenType.在新窗口中查看分析))
            {
                foreach (var table in tables)
                {
                    var form = new Geo.Winform.TableObjectViewForm(table) { Text = table.Name };
                    form.Show();
                }
            }

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
            // MultiTableTextOpenVizardForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.ClientSize = new System.Drawing.Size(605, 423);
            this.Name = "MultiTableTextOpenVizardForm";
            this.ShowFirstButton = true;
            this.ShowLastButton = true;
            this.Text = "单文件多表数据打开向导";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.SinexImportVizardForm_FormClosing);
            this.ResumeLayout(false);

        }

        private void SinexImportVizardForm_FormClosing(object sender, FormClosingEventArgs e)
        { 
        } 
    }

}
