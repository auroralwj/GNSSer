//2018.07.07, czs, create in HMX, 数值条件

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
    /// 数值条件
    /// </summary>
    public partial class NumeralSearchForm : Form
    {
        /// <summary>
        /// 数值条件
        /// </summary>
        public NumeralSearchForm()
        {
            InitializeComponent();
            this.ControlList = new List<ConnectedNumeralConditionControl>();
            ControlItemHeight = 33;
        }
        int ControlItemHeight = 33;
        /// <summary>
        /// 最后结果
        /// </summary>
        public ConnectedNumCondition ConnectedCondition { get; private set; }
        /// <summary>
        /// 列表
        /// </summary>
        private List<ConnectedNumeralConditionControl> ControlList { get; set; }

        private void NumeralSearchForm_Load(object sender, EventArgs e)
        {

        }

        private void button_add_Click(object sender, EventArgs e)
        {
            var control = BuildControl(ControlList.Count);
            this.Controls.Add(control);
            ControlList.Add(control);
            this.Height += ControlItemHeight;
        }

        private void button_del_Click(object sender, EventArgs e)
        {
            if(ControlList.Count == 0)
            {
                return;
            }
            var last = ControlList[ControlList.Count - 1];
            this.Controls.Remove(last);
            ControlList.Remove(last);
            this.Height -= ControlItemHeight;
        }

        private void button_ok_Click(object sender, EventArgs e)
        {
            this.ConnectedCondition = GetValue();
            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private void button_cancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }

        private ConnectedNumeralConditionControl BuildControl(int index)
        {
            ConnectedNumeralConditionControl control = new ConnectedNumeralConditionControl();
            int y = 62 + ControlItemHeight * index;
            control.Location = new System.Drawing.Point(23, y);
            control.Name = "control" + index;
            control.Size = new System.Drawing.Size(554, 33);
            control.TabIndex = index;
            return control;
        }

        /// <summary>
        /// 获取值
        /// </summary>
        /// <returns></returns>
        public ConnectedNumCondition GetValue()
        {
            var baseCondition = numeralConditionControl1.GetValue();
            ConnectedNumCondition connectedCondition = new ConnectedNumCondition(baseCondition);
            foreach (var item in ControlList)
            {
                connectedCondition.AddConditon(item.GetValue());
            }
            return connectedCondition;
        }
    }
}
