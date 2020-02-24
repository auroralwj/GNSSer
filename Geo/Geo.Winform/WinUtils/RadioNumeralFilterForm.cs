//2017.10.01, czs, create in hongqing, 数字操作

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
    public partial class RadioNumeralFilterForm : Form
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="initVal"></param>
        public RadioNumeralFilterForm(double initVal = 0)
        {
            InitializeComponent();
            enumRadioControl1.Init<NumeralOperationType>(true);
        }
        /// <summary>
        /// 比较
        /// </summary>
        public NumeralOperationType NumeralOperationType { get; set; }
        /// 所选
        /// </summary>
        public double InputedValue { get; set; }

    
        private void okbutton1_Click(object sender, EventArgs e)
        {        
            InputedValue = namedFloatControl1.GetValue();
            this.NumeralOperationType = enumRadioControl1.GetCurrent<NumeralOperationType>();
       
            this.DialogResult = DialogResult.OK;
            Close();
        }
          
         

        private void SelectingColForm_Load(object sender, EventArgs e)
        {

        }
    }
}