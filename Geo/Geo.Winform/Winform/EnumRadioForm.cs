//2018.08.11, czs, create in hmx, 枚举选项

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Geo.Winform
{
    /// <summary>
    /// 枚举选项
    /// </summary>
    public partial class EnumRadioForm : Form
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public EnumRadioForm()
        {
            InitializeComponent();
        }

        /// <summary>
        /// 设置
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="IsCheckOne"></param>
        public void Init<T>(bool IsCheckOne = true)
        {
            this.enumRadioControl1.Init<T>(IsCheckOne);
        }
        /// <summary>
        /// 获取
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public T GetCurrent<T>()
        {
            return this.enumRadioControl1.GetCurrent<T>();
        }

        /// <summary>
        /// 获取
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public void SetCurrent<T>(T val)
        {
            this.enumRadioControl1.SetCurrent<T>(val);
        }

        /// <summary>
        /// 当前所选
        /// </summary>
        public string CurrentText { get; set; }

        private void button_ok_Click(object sender, EventArgs e)
        {
            this.CurrentText = this.enumRadioControl1.CurrentText;
            this.DialogResult = DialogResult.OK;
        }

        private void button_cancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
        }
    }
}
