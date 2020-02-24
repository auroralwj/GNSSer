//2017.06.16, czs, create in hongqing, 数组手动排序

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
    /// 选择文本
    /// </summary>
    public partial class SortingNamesForm : Form
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public SortingNamesForm()
        {
            InitializeComponent();
        }
        /// <summary>
        /// 以数组初始化
        /// </summary>
        /// <param name="strs"></param>
        public SortingNamesForm(string[] strs)
        {
            InitializeComponent();
            this.checkedListBox1.Items.AddRange(strs);
            button_selectAll_Click(null, null);
        }
        /// <summary>
        /// 以数组初始化,将老的排列靠前，且选中。
        /// 
        /// </summary>
        /// <param name="strs"></param>
        public SortingNamesForm(List<String> allNames, string[] oldOrderedNames)
        {
            InitializeComponent();
           var  okNames =    Geo.Utils.ArrayUtil.GetNamesInOldOrders(  allNames, oldOrderedNames);

           this.checkedListBox1.Items.AddRange(okNames.ToArray());
           List<string> old = new List<string>(oldOrderedNames);

           for (int i = 0; i <  this.checkedListBox1.Items.Count; i++)
           {
               var item = this.checkedListBox1.Items[i];
                if(old.Contains(item.ToString())){
                    checkedListBox1.SetItemChecked(i, true);
                } 
            } 
        } 

        /// <summary>
        /// 排序后的名称列表
        /// </summary>
        public List<string> OrderedNames { get; set; }
        /// <summary>
        /// 已选排序后的
        /// </summary>
        public List<string> CheckedOrderedNames { get; set; }
        private void okbutton1_Click(object sender, EventArgs e)
        {
            if (this.checkedListBox1.CheckedItems.Count < 1)
            {
                MessageBox.Show("请选择项目。");
                return;
            }
            SaveToList();
            this.DialogResult = DialogResult.OK;
            Close();
        }

        private void SaveToList()
        {
            OrderedNames = new List<string>();
            foreach (var item in checkedListBox1.Items)
            {
                OrderedNames.Add(item.ToString());
            }
            CheckedOrderedNames = new List<string>();
            foreach (var item in checkedListBox1.CheckedItems)
            {
                CheckedOrderedNames.Add(item.ToString());
            } 
        }
        private void button_top_Click(object sender, EventArgs e)
        {
            if (this.checkedListBox1.CheckedItems.Count < 1)
            {
                MessageBox.Show("请选择待排序项目。");
                return;
            }
            SaveToList();
            var selecteds = GetSelectedNames();

            this.checkedListBox1.Items.Clear();
            this.checkedListBox1.Items.AddRange(selecteds.ToArray());

            foreach (var item in OrderedNames)
            {
                if (selecteds.Contains(item)) { continue; }
                this.checkedListBox1.Items.Add(item);
            }

            RecoverSelects(selecteds);
        }

        private void RecoverSelects(List<string> selecteds)
        {
            foreach (var item in selecteds)
            {
                var index = this.checkedListBox1.Items.IndexOf(item);
                checkedListBox1.SetItemChecked(index, true);
            }
        }

        private void button_up_Click(object sender, EventArgs e)
        {
            if (this.checkedListBox1.CheckedItems.Count < 1)
            {
                MessageBox.Show("请选择待排序项目。");
                return;
            }
            SaveToList();
            var selecteds = GetSelectedNames();
            foreach (var item in selecteds)
            {
                var oldIndex = OrderedNames.IndexOf(item);
                if(oldIndex ==0){continue;}//第一个不用调了
                var newIndex = oldIndex - 1;//上有已选，不用调了
                if( selecteds.Contains( OrderedNames[newIndex])){continue;}

                OrderedNames.RemoveAt(oldIndex);
                OrderedNames.Insert(newIndex, item);
            } 

            this.checkedListBox1.Items.Clear();
            this.checkedListBox1.Items.AddRange(OrderedNames.ToArray());

            RecoverSelects(selecteds);
        }

        private void button_down_Click(object sender, EventArgs e)
        {
            if (this.checkedListBox1.CheckedItems.Count < 1)
            {
                MessageBox.Show("请选择待排序项目。");
                return;
            }
            SaveToList();
            var selecteds = GetSelectedNames();
            selecteds.Reverse();
            foreach (var item in selecteds)
            {
                var oldIndex = OrderedNames.IndexOf(item);
                if (oldIndex == OrderedNames.Count -1) { continue; }//最后一个不用调了
                var newIndex = oldIndex + 1;//下有已选，不用调了
                if (selecteds.Contains(OrderedNames[newIndex])) { continue; }

                OrderedNames.RemoveAt(oldIndex);
                OrderedNames.Insert(newIndex, item);
            }

            this.checkedListBox1.Items.Clear();
            this.checkedListBox1.Items.AddRange(OrderedNames.ToArray());
            RecoverSelects(selecteds);
        }

        private void button_last_Click(object sender, EventArgs e)
        {
            if (this.checkedListBox1.CheckedItems.Count < 1)
            {
                MessageBox.Show("请选择待排序项目。");
                return;
            }
            SaveToList();
            var selecteds = GetSelectedNames();

            this.checkedListBox1.Items.Clear();
            foreach (var item in OrderedNames)
            {
                if (selecteds.Contains(item)) { continue; }
                this.checkedListBox1.Items.Add(item);
            }
            this.checkedListBox1.Items.AddRange(selecteds.ToArray());

            RecoverSelects(selecteds);
        }

        private List<string> GetSelectedNames()
        {
            var selecteds = new List<string>();
            foreach (object item in this.checkedListBox1.CheckedItems)
            {
                selecteds.Add(item.ToString());
            }
            return selecteds;
        }

        private void button_selectAll_Click(object sender, EventArgs e)
        {
            int count = this.checkedListBox1.Items.Count;
            for (int i = 0; i < count; i++)
            {
                if (!this.checkedListBox1.GetItemChecked(i))
                {
                    this.checkedListBox1.SetItemChecked(i, true);
                }
            }
        }

        private void button_reverseSelect_Click(object sender, EventArgs e)
        {
            int count = this.checkedListBox1.Items.Count;
            for (int i = 0; i < count; i++)
            {
                if (this.checkedListBox1.GetItemChecked(i))
                {
                    this.checkedListBox1.SetItemChecked(i, false);
                }
                else
                {
                    this.checkedListBox1.SetItemChecked(i, true);
                }
            }

        }

        private void SortingNamesForm_Load(object sender, EventArgs e)
        {

        }
    }
}