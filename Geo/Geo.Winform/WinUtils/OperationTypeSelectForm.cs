//2018.08.14, czs, create in hmx, 数字操作

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
    ///操作类型
    /// </summary>
    public partial class OperationTypeSelectForm : Form
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="initVal"></param>
        public OperationTypeSelectForm(NumeralOperationType initVal = NumeralOperationType.加)
        {
            InitializeComponent();
            enumRadioControl1.Init<NumeralOperationType>(true);
            enumRadioControl1.SetCurrent(initVal);
        }
        /// <summary>
        /// 比较
        /// </summary>
        public NumeralOperationType NumeralOperationType { get; set; } 

    
        private void okbutton1_Click(object sender, EventArgs e)
        {         
            this.NumeralOperationType = enumRadioControl1.GetCurrent<NumeralOperationType>();
       
            this.DialogResult = DialogResult.OK;
            Close();
        }
          
         

        private void SelectingColForm_Load(object sender, EventArgs e)
        {

        }
    }
}