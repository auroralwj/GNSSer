//2017.10.01, czs, create in hongqing, ���ֲ���

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
    ///���ֹ��˴���
    /// </summary>
    public partial class RadioNumeralFilterForm : Form
    {
        /// <summary>
        /// ���캯��
        /// </summary>
        /// <param name="initVal"></param>
        public RadioNumeralFilterForm(double initVal = 0)
        {
            InitializeComponent();
            enumRadioControl1.Init<NumeralOperationType>(true);
        }
        /// <summary>
        /// �Ƚ�
        /// </summary>
        public NumeralOperationType NumeralOperationType { get; set; }
        /// ��ѡ
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