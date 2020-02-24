//2018.07.07, czs, create in HMX, 数值条件


using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Geo.Winform
{
    /// <summary>
    /// 数值条件
    /// </summary>
    public partial class NumeralConditionControl : UserControl
    {
        /// <summary>
        /// 数值条件
        /// </summary>
        public NumeralConditionControl()
        {
            InitializeComponent();
        }

        private void NumeralConditionControl_Load(object sender, EventArgs e)
        {
            bindingSource_condition.DataSource = Enum.GetNames(typeof(NumeralConditionOperator));
        }

        /// <summary>
        /// 数值条件
        /// </summary>
        public NumeralCondition NumeralCondition
        {
            get
            {
                double val1 = double.Parse(textBox_value1.Text);
                double val2 = double.Parse(textBox_value2.Text);
                NumeralConditionOperator conditionOperator = (NumeralConditionOperator)Enum.Parse(typeof(NumeralConditionOperator), bindingSource_condition.Current.ToString());

                return new NumeralCondition(conditionOperator, val1, val2);
            } 
        }

        /// <summary>
        /// 设置
        /// </summary>
        /// <param name="condition"></param>
        public void SetNumeralCondition(NumeralCondition condition)
        {
            textBox_value1.Text = condition.FirstRefferValue.ToString();
            textBox_value2.Text = condition.SecondRefferValue.ToString();
            comboBox_match.SelectedItem = condition.ConditionOperator.ToString();
        }

        /// <summary>
        /// 获取值
        /// </summary>
        /// <returns></returns>
        public NumeralCondition GetValue()
        {
            return NumeralCondition;
        }

        /// <summary>
        /// 设置值
        /// </summary>
        /// <param name="val"></param>
        public void SetValue(NumeralCondition val)
        {
            SetNumeralCondition(val);
        }

        private void bindingSource_condition_CurrentChanged(object sender, EventArgs e)
        {
            var conditionOperator = (NumeralConditionOperator)Enum.Parse(typeof(NumeralConditionOperator), bindingSource_condition.Current.ToString());
            var isDual = NumeralCondition.IsDualValueCondition(conditionOperator);
             
            textBox_value2.Visible = isDual; 
        } 
    }
}
