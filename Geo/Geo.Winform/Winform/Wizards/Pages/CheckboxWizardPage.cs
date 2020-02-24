//2017.09.01, czs, create in hongqing, 多选框页面控件

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
    /// 多选框页面控件
    /// </summary>
    public partial class CheckboxWizardPage : UserControl, IWizardPage
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public CheckboxWizardPage()
        {
            InitializeComponent(); 
            this.Name = "选择";
        }
         
        /// <summary>
        /// 内容
        /// </summary>
        public UserControl Content    {  get { return this; }    }

        /// <summary>
        /// 单选
        /// </summary>
        public Geo.Winform.EnumCheckBoxControl GetEnumCheckBoxControl { get { return this.enumCheckBoxControl1; } }

        /// <summary>
        /// 初始化
        /// </summary>
        /// <typeparam name="TEnum"></typeparam>
        /// <param name="name"></param>
        /// <param name="IsCheckOne"></param>
        public void Init<TEnum>(string name)
        {
            this.Name = name;
            this.enumCheckBoxControl1.Init<TEnum>();
        }

        /// <summary>
        /// 当前类型
        /// </summary>
        /// <typeparam name="TEnum"></typeparam>
        /// <returns></returns>
        public List<TEnum> GetSelected<TEnum>()
        {
            return  this.enumCheckBoxControl1.GetSelected<TEnum>();
        }

        /// <summary>
        /// 当前类型
        /// </summary>
        /// <typeparam name="TEnum"></typeparam>
        /// <returns></returns>
        public void SetCurrent<TEnum>(TEnum val)
        {
            this.enumCheckBoxControl1.Select<TEnum>(val);
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
            get { return this.enumCheckBoxControl1.SelectedCount > 0; }
        }

        public string ValidationMessage
        {
            get { return "请必须选择一个再离开！"; }
        }
         

        private void SelectFilePageControl_Load(object sender, EventArgs e)
        { 
        }
    }
}
