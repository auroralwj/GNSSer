//2017.07.24, czs, create in hongqing, 选择文件页面控件

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms; 
using Geo.Winform.Wizards;

namespace Gnsser.Winform
{
    public partial class SelectRinexFileWizardPage : UserControl, IWizardPage
    {
        public SelectRinexFileWizardPage()
        {
            InitializeComponent();

            fileOpenControl_o.Filter = Setting.RinexOFileFilter;
            this.Name = "选择文件";
        }

        public string NFilePath { get { return fileOpenControl_n.FilePath; } }
        public string[] FilePathes { get { return fileOpenControl_o.FilePathes; } }
        public string FilePath{ get { return fileOpenControl_o.FilePath; } }

        public UserControl Content
        {
            get { return this; }
        }

        public new void LoadPage()
        {
          //  throw new NotImplementedException();
        }

        public void Save()
        {
            //throw new NotImplementedException();
        }

        public void Cancel()
        {
            //throw new NotImplementedException();
        }

        public bool IsBusy
        {
            get { return false; }
        }

        public bool PageValid
        {
            get { return true; }
        }

        public string ValidationMessage
        {
            get { throw new NotImplementedException(); }
        }

        /// <summary>
        /// 是否是多文件
        /// </summary>
        /// <param name="trueOrFalse"></param>
        public SelectRinexFileWizardPage SetIsMultiFile(bool trueOrFalse)
        {
            this.fileOpenControl_o.IsMultiSelect = trueOrFalse;
            if (this.fileOpenControl_o.IsMultiSelect)
            {
                this.fileOpenControl_o.Height = 22*4;
            }
            else
            {
                this.fileOpenControl_o.Height = 22;
            }
            return this;
        }

        private void checkBox_isMultiFile_CheckedChanged(object sender, EventArgs e)
        {
            var trueOrFalse = this.checkBox_isMultiFile.Checked;
            SetIsMultiFile(trueOrFalse);
        }
 
            
        private void SelectFilePageControl_Load(object sender, EventArgs e)
        {
            directorySelectionControl1.Path = Setting.TempDirectory;
            fileOpenControl_n.Visible = checkBox_indicateEph.Checked;
            fileOpenControl_o.FilePath = Setting.GnsserConfig.SampleOFile + "\r\n" + Setting.GnsserConfig.SampleOFileB;
        }

        private void checkBox_indicateEph_CheckedChanged(object sender, EventArgs e)
        {
            fileOpenControl_n.Visible = checkBox_indicateEph.Checked;


        }

        private void fileOpenControl_n_Load(object sender, EventArgs e)
        {

        }
    }
}
