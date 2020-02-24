//2017.07.31, czs, create in hongqing, 单行输入页面控件

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms; 
using Geo.Winform.Wizards;

namespace Geo.Winform.Wizards
{
    /// <summary>
    /// 单行输入页面控件
    /// </summary>
    public partial class FileOpenWizardPage : UserControl, IWizardPage
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public FileOpenWizardPage()
        {
            InitializeComponent(); 
            this.Name = "选择文件路径";
        }
         
        /// <summary>
        /// 内容
        /// </summary>
        public UserControl Content    {  get { return this; }    }

        public String[] FilePathes { get { return this.fileOpenControl1.FilePathes; } }
        public String FilePath { get { return this.fileOpenControl1.FilePath; } }

        /// <summary>
        /// 初始化
        /// </summary>
        /// <typeparam name="TEnum"></typeparam>
        /// <param name="name"></param>
        /// <param name="IsCheckOne"></param>
        public void Init(string name, string filter)
        {
            this.Name = name;
            this.fileOpenControl1.LabelName = name;
            this.fileOpenControl1.Filter = (filter);
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
         

        private void SelectFilePageControl_Load(object sender, EventArgs e)
        { 
        }
    }
}
