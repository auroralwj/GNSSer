//2015.06.06, czs, create in namu, 对象选择窗口

using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Geo.Winform
{
    /// <summary>
    /// 对象选择窗口
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public partial class ObjectSelectingForm<T> : Form
    {
        public ObjectSelectingForm()
        {
            InitializeComponent();  
        }

        public ObjectSelectingForm(List<T> dataSource, List<T> selecteds = null)
        {
            InitializeComponent();
            dataGridView.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            DataBind(dataSource, selecteds);
        }

        public List<EnbaledObject<T>> EnbaledObjects { get; set; }

        List<T> InitSelectedObjects { get; set; }
        /// <summary>
        /// 已经选择的对象列表
        /// </summary>
        public List<T> SelectedObjects { get; private set; }

        public void DataBind(List<T> dataSource, List<T> selecteds = null)
        {
            SetInitSelecteds(selecteds);
            SetSource(dataSource);

            this.dataGridView.DataSource = EnbaledObjects;
        }

        private void SetSource(List<T> dataSource)
        {
            foreach (var item in dataSource)
            {
                bool enabled = InitSelectedObjects.Contains(item);

                EnbaledObjects.Add(new EnbaledObject<T> { Name = item.ToString(), Object = item, Enabled = enabled });
            }
        }

        private void SetInitSelecteds(List<T> selecteds)
        {
            EnbaledObjects = new List<EnbaledObject<T>>();
            InitSelectedObjects = new List<T>();
            if (selecteds != null)
            {
                foreach (var item in selecteds)
                {
                    InitSelectedObjects.Add(item);
                }
            }
        }

        private void button_ok_Click(object sender, EventArgs e)
        {
            SetSelctedObjects();

            this.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.Close();
        }

        private void SetSelctedObjects()
        { 
            SelectedObjects = new List<T>();
            foreach (var item in this.dataGridView.DataSource as List<EnbaledObject<T>>)
            {
                if (item.Enabled)
                    SelectedObjects.Add(item.Object);
            }
        }

        private void button_cancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.Close();
        }

        private void dataGridView_proj_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
        }

        private void button_selectAll_Click(object sender, EventArgs e)
        {
            foreach (var item in EnbaledObjects)
            {
                item.Enabled = true;
            }
            DataBind();
        }

        private void button_cancelall_Click(object sender, EventArgs e)
        { 
            foreach (var item in EnbaledObjects)
            {
                item.Enabled = false;
            }
            DataBind();
        }

        private void button_inverse_Click(object sender, EventArgs e)
        {
            foreach (var item in EnbaledObjects)
            {
                item.Enabled = !item.Enabled;
            }
            DataBind();
        }

        private void DataBind()
        {
            this.dataGridView.DataSource = null;
            this.dataGridView.DataSource = EnbaledObjects;
        }

        private void dataGridView_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            SetSelctedObjects();
            this.label1.Text = "已选择：" + SelectedObjects.Count + " 个对象";

        }

    }


    public class EnbaledObject<T>
    {
         [DisplayName("选择")]
        public bool Enabled { get; set; }
        [DisplayName("名称")]
        public string Name { get; set; }

        [DisplayName("对象")]
        public T Object { get; set; }
    }
}
