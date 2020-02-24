//2017.07.24, czs, create in hongqing, 单选框页面控件

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
    /// 单选框页面控件
    /// </summary>
    public partial class EnumRadioWizardPage : UserControl, IWizardPage
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public EnumRadioWizardPage()
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
        public  Geo.Winform.EnumRadioControl GetRatioControl { get { return this.enumRadioControl1; } }

        /// <summary>
        /// 初始化
        /// </summary>
        /// <typeparam name="TEnum"></typeparam>
        /// <param name="name"></param>
        /// <param name="IsCheckOne"></param>
        public void Init<TEnum>(string name, bool IsCheckOne = true)
        {
            this.Name = name;
            this.enumRadioControl1.Init<TEnum>(IsCheckOne);
        }

        /// <summary>
        /// 当前类型
        /// </summary>
        /// <typeparam name="TEnum"></typeparam>
        /// <returns></returns>
        public TEnum GetCurrent<TEnum>()
        {
            return (TEnum)Enum.Parse(typeof(TEnum), this.enumRadioControl1.CurrentText);
        }

        /// <summary>
        /// 当前类型
        /// </summary>
        /// <typeparam name="TEnum"></typeparam>
        /// <returns></returns>
        public void SetCurrent<TEnum>(TEnum val, bool isEnableOther=true)
        {
            this.enumRadioControl1.Select<TEnum>(val, isEnableOther);
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
