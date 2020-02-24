//2014.12.13, czs, create in namu, 枚举类型选择器

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Geo.Winform.Controls
{
    /// <summary>
    /// 将枚举类型封装在分组框内。
    /// </summary>
    /// <typeparam name="TEnum">枚举类型</typeparam>
    public partial class EnumSelectControl<TEnum> : UserControl, IEnumSelector<TEnum> 
    {
        public EnumSelectControl()
        {
            InitializeComponent();
            this._Default = default(TEnum);
            Init();
        }
        /// <summary>
        /// 单选按钮
        /// </summary>
        protected List<RadioButton> RadioButtons { get; set; }
        /// <summary>
        /// 选择事件
        /// </summary>
        public event Action<TEnum> RadioSelected;

        public void Init()
        {
            this.groupBox1.SuspendLayout();
            this.flowLayoutPanel1.SuspendLayout();
            this.SuspendLayout();

            RadioButtons = new List<RadioButton>();
            var values = Enum.GetValues(typeof(TEnum));
            foreach (var item in values)
            {
                RadioButton radioButton = new RadioButton();
                radioButton.CheckedChanged += RadioButton_CheckedChanged;
                radioButton.Text = item.ToString();
                radioButton.AutoSize = true;
                radioButton.Tag = item;

                if (CurrentdType.Equals((TEnum)item)) radioButton.Checked = true;

                this.flowLayoutPanel1.Controls.Add(radioButton);
                RadioButtons.Add(radioButton);
            }

            this.groupBox1.ResumeLayout(false);
            this.flowLayoutPanel1.ResumeLayout(false);
            this.flowLayoutPanel1.PerformLayout();
            this.ResumeLayout(false);
        }

        private void RadioButton_CheckedChanged(object sender, EventArgs e)
        {
            if (RadioSelected == null) { return; }

            RadioButton RadioButton = (RadioButton)sender;
            if (RadioButton.Checked)
            {
                var type = (TEnum)Enum.Parse(typeof(TEnum), RadioButton.Text);
                RadioSelected(type);
            } 
        }

        /// <summary>
        /// this.groupBox1.Text;
        /// </summary>
        public override string Text { get { return  this.groupBox1.Text; } set{ this.groupBox1.Text = value;}}
        TEnum _Default;
        /// <summary>
        /// 默认选择项
        /// </summary>
        public TEnum CurrentdType
        {
            get
            {
                foreach (var item in RadioButtons)
                {
                    if (item.Checked) return (TEnum)item.Tag;
                }
                return _Default;
            }
            set
            {
                this._Default = value;
                foreach (var item in RadioButtons)
                {
                    TEnum  enu = (TEnum)item.Tag;
                    if (enu.Equals(value))
                    {
                        item.Checked = true;
                        break;
                    }
                } 
            }
        }

        private void EnumSelectControl_Load(object sender, EventArgs e)
        { 
           // Init();
        }

    }
}
