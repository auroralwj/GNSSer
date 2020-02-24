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
    /// 具有连接条件的
    /// </summary>
    public partial class ConnectedNumeralConditionControl : UserControl
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public ConnectedNumeralConditionControl()
        {
            InitializeComponent();
        }
        /// <summary>
        /// 取值
        /// </summary>
        /// <returns></returns>
        public ConnectedNumeralCondition GetValue()
        {
            return ConnectedCondition;
        }
        /// <summary>
        /// 连接条件
        /// </summary>
        public ConnectedNumeralCondition ConnectedCondition
        {
            get
            {
                ConditionConnectOperator connect = ConditionConnectOperator.And;
                var coText = this.comboBox_conect.Text;
                if (coText == "或")
                {
                    connect = ConditionConnectOperator.Or;
                }
                return new ConnectedNumeralCondition(connect, this.numeralConditionControl1.NumeralCondition);
            } 
        }
        /// <summary>
        /// 设置
        /// </summary>
        /// <param name="ConnectedCondition"></param>
        public void SetConnectedNumeralCondition( ConnectedNumeralCondition ConnectedCondition)
        {
                this.numeralConditionControl1.SetNumeralCondition((NumeralCondition)ConnectedCondition.Condition);
                if (ConnectedCondition.ConnectOperator == ConditionConnectOperator.And)
                    this.comboBox_conect.Text = "且";
                else this.comboBox_conect.Text = "或";

        }
        /// <summary>
        /// 设置值
        /// </summary>
        /// <param name="val"></param>
        public void SetValue(ConnectedNumeralCondition val)
        {
            SetConnectedNumeralCondition(val);
        }
    }
}
