using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Geo.Winform
{
    public partial class TitledTableBox : TitledBox
    {
        public TitledTableBox()
        {
            InitializeComponent();
        }
        /// <summary>
        /// 表
        /// </summary>
        public System.Windows.Forms.DataGridView DataGridView
        {
            get { return dataGridView1; }
            set { dataGridView1 = value; }
        }
        /// <summary>
        /// DataGridView 的数据源
        /// </summary>
        public System.Windows.Forms.BindingSource BindingSource
        {
            get { return bindingSource1; }
            set { bindingSource1 = value; }
        }
    }
}
