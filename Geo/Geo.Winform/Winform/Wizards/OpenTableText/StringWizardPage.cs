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
    public partial class StringWizardPage : UserControl, IWizardPage
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public StringWizardPage()
        {
            InitializeComponent(); 
            this.Name = "单行输入";
        }
         
        /// <summary>
        /// 内容
        /// </summary>
        public UserControl Content    {  get { return this; }    }
        /// <summary>
        ///  "提示：" + str
        /// </summary>
        /// <param name="str"></param>
        public void SetNotice(string str) { label_notice.Text = "提示：" + str; }
        /// <summary>
        /// 单选
        /// </summary>
        public String Value { get { return this.namedStringControl1.GetValue(); } }
        /// <summary>
        /// 单选
        /// </summary>
        public String [] Lines { get { return this.namedStringControl1.GetLines(); } }

        /// <summary>
        /// 初始化
        /// </summary>
        /// <param name="name"></param>
        /// <typeparam name="currentVal"></typeparam>
        /// <typeparam name="notice"></typeparam>
        /// <param name="isMultiPath"></param>
        public void Init(string name, string val, string notice, bool isMultiPath = false)
        {
            this.Name = name;
            SetNotice(notice);
            namedStringControl1.Init(name, val, isMultiPath);
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
