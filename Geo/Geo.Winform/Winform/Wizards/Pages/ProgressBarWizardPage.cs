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
using Geo.Winform.Controls;

namespace Geo.Winform.Wizards
{
    /// <summary>
    /// 选择文件页面控件
    /// </summary>
    public partial class ProgressBarWizardPage : UserControl, IWizardPage
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public ProgressBarWizardPage()
        {
            InitializeComponent(); 
            this.Name = "执行进度";
        }
         
        /// <summary>
        /// 内容
        /// </summary>
        public UserControl Content    {  get { return this; }    }

        /// <summary>
        /// 单选
        /// </summary>
        public ProgressBarComponent ProgressBarComponent { get { return this.progressBarComponent1; } }

        /// <summary>
        /// 初始化
        /// </summary>
        /// <typeparam name="TEnum"></typeparam>
        /// <param name="name"></param>
        /// <param name="IsCheckOne"></param>
        public void Init(string name, int fileCount)
        {
            this.Name = name;
            Init(fileCount);
        }

        public void Init(int fileCount)
        {
            this.ProgressBarComponent.InitProcess(fileCount);
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
