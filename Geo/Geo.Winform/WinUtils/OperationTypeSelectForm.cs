//2018.08.14, czs, create in hmx, ���ֲ���

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
    ///��������
    /// </summary>
    public partial class OperationTypeSelectForm : Form
    {
        /// <summary>
        /// ���캯��
        /// </summary>
        /// <param name="initVal"></param>
        public OperationTypeSelectForm(NumeralOperationType initVal = NumeralOperationType.��)
        {
            InitializeComponent();
            enumRadioControl1.Init<NumeralOperationType>(true);
            enumRadioControl1.SetCurrent(initVal);
        }
        /// <summary>
        /// �Ƚ�
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