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
    public partial class SelectEphFilePageControl : UserControl, IWizardPage
    {
        public SelectEphFilePageControl()
        {
            InitializeComponent();

            fileOpenControl_o.Filter = Setting.RinexNFileFilter;
            this.Name = "选择文件";
        }

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

        private void checkBox_isMultiFile_CheckedChanged(object sender, EventArgs e)
        {
            this.fileOpenControl_o.IsMultiSelect = this.checkBox_isMultiFile.Checked;
            if (this.fileOpenControl_o.IsMultiSelect)
            {
                this.fileOpenControl_o.Height *= 3;
            }
            else
            {
                this.fileOpenControl_o.Height /= 3;
            }
        }

        private void SelectFilePageControl_Load(object sender, EventArgs e)
        {
            directorySelectionControl1.Path = Setting.TempDirectory;
        }
    }
}
