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
    public partial class AttributeBox : TitledBox
    {
        public AttributeBox()
        {
            InitializeComponent();
        }
        public void SetObject<T>(T t, bool useDisplayAttribute= true)
        {
            this.bindingSource1.DataSource = Geo.Utils.ObjectUtil.GetAttributes(t, useDisplayAttribute);
        }
        public void SetObject(Object t)
        {
            this.bindingSource1.DataSource = Geo.Utils.ObjectUtil.GetAttributes(t);
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
        /// DataGridView 的数据源{Name，Value}
        /// </summary>
        public System.Windows.Forms.BindingSource BindingSource
        {
            get { return bindingSource1; }
            set { bindingSource1 = value; }
        }
    }
}
