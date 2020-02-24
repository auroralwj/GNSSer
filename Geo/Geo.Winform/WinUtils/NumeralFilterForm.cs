//2017.09.30, czs, create in hongqing, 数字过滤

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace Geo.Utils
{

    /// <summary>
    ///数字过滤窗口
    /// </summary>
    public partial class NumeralFilterForm : Form
    { 
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="strs"></param>
        public NumeralFilterForm(double initVal  =0 )
        {
            InitializeComponent();
            enumRadioControl1.Init<NumeralCompareOperator>(true);
        }
        /// <summary>
        /// 比较
        /// </summary>
        public NumeralCompareOperator CompareOperator  { get; set; }
        /// 所选
        /// </summary>
        public double InputedValue { get; set; }

    
        private void okbutton1_Click(object sender, EventArgs e)
        {        
            InputedValue = namedFloatControl1.GetValue();
            this.CompareOperator = enumRadioControl1.GetCurrent<NumeralCompareOperator>();
       
            this.DialogResult = DialogResult.OK;
            Close();
        }
          
         

        private void SelectingColForm_Load(object sender, EventArgs e)
        {

        }
    }
}