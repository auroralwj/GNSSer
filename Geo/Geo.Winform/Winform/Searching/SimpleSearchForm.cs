using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Geo.Winform
{
    public partial class SimpleSearchForm : Form
    {
        public SimpleSearchForm()
        {
            InitializeComponent();
        }
        public SimpleSearchForm(SimpleSearchCondition SimpleSearchCondition)
        {
            InitializeComponent();
            this.SimpleSearchCondition = SimpleSearchCondition; 
        }

        SimpleSearchCondition con;
        public SimpleSearchCondition SimpleSearchCondition
        {
            get { return con; }
            set
            {
                con = value;
                SetUI(value);
            }
        }

        private void button_ok_Click(object sender, EventArgs e)
        {
            this.con = FetchFromUI();
            this.DialogResult = System.Windows.Forms.DialogResult.OK;
        }

        private void SetUI(Winform.SimpleSearchCondition value)
        {
            this.radioButton_include.Checked = value.IsIncludingOrNot;
            this.radioButton_exclude.Checked = !value.IsIncludingOrNot;
            this.radioButton_fuzzy.Checked = value.IsFuzzyMathching;
            this.radioButton_exclude.Checked = !value.IsFuzzyMathching;
            this.textBox_word.Text = value.Word;
        }

        private Winform.SimpleSearchCondition FetchFromUI()
        {
            SimpleSearchCondition con = new SimpleSearchCondition()
            {
                IsFuzzyMathching = this.radioButton_fuzzy.Checked,
                IsIncludingOrNot = this.radioButton_include.Checked,
                Word = this.textBox_word.Text.Trim()
            };
            return con;
        }
    }

    /// <summary>
    /// 简单查询条件。
    /// </summary>
    public class SimpleSearchCondition
    {
        /// <summary>
        /// 关键字
        /// </summary>
        public string Word { get; set; }
        /// <summary>
        /// 包含匹配，还是包含不匹配
        /// </summary>
        public bool IsIncludingOrNot { get; set; }
        /// <summary>
        /// 是否模糊查询
        /// </summary>
        public bool IsFuzzyMathching { get; set; }
    }


}
