//2015.10.23, czs, edit in hongqign, 对象编辑器

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Geo.Utils;

namespace Geo.Winform
{
    /// <summary>
    /// 具有显示
    /// </summary>
    public partial class DisplayPropertyEditForm : Form, IEntityEditForm
    {
        /// <summary>
        /// 新建
        /// </summary>
        public DisplayPropertyEditForm()
        {
            InitializeComponent(); 
        }
         
        /// <summary>
        /// 修改，编辑
        /// </summary>
        /// <param name="Entity"></param>
        public DisplayPropertyEditForm(Object Entity)
        {
            InitializeComponent();
            Init(Entity);
        }

        public Object Entity { get; set; }

        public List<AttributeItem> Attributes;

        /// <summary>
        /// 对象初始化
        /// </summary>
        /// <param name="obj"></param>
        public void Init(Object obj)
        {
            this.Entity = obj;
            EntityToUi();
        }

        public void Init<T>()
        {
            this.Entity = default(T);
            EntityToUi();
        }

        private void button_ok_Click(object sender, EventArgs e)
        {
            this.UiToEntity();

            this.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.Close();
        }

        private void button_cancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.Close();
        }

        public void EntityToUi()
        { 
            this.Attributes = Geo.Utils.ObjectUtil.GetAttributes(Entity, true);
            this.dataGridView1.DataSource = Attributes;              
        }

        public void UiToEntity()
        {
            Type type = Entity.GetType();
            foreach (var item in Attributes)
            {
                var property = type.GetProperty(item.PropertyName);
                var ptype = property.PropertyType;
                object obj = item.Value;//default string
             
                if (ptype == typeof(int))
                {
                    obj = Int32.Parse(item.Value); 
                }
                else if (ptype == typeof(Double))
                {
                    obj = Double.Parse(item.Value);  
                }
                else if (ptype == typeof(Boolean))
                {
                    obj = Boolean.Parse(item.Value);  
                }

                property.SetValue(Entity, obj, null);
            }
        }

    }
}
