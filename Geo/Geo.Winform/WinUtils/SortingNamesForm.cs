//2017.06.16, czs, create in hongqing, �����ֶ�����

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
    /// ѡ���ı�
    /// </summary>
    public partial class SortingNamesForm : Form
    {
        /// <summary>
        /// ���캯��
        /// </summary>
        public SortingNamesForm()
        {
            InitializeComponent();
        }
        /// <summary>
        /// �������ʼ��
        /// </summary>
        /// <param name="strs"></param>
        public SortingNamesForm(string[] strs)
        {
            InitializeComponent();
            this.checkedListBox1.Items.AddRange(strs);
            button_selectAll_Click(null, null);
        }
        /// <summary>
        /// �������ʼ��,���ϵ����п�ǰ����ѡ�С�
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
        /// �����������б�
        /// </summary>
        public List<string> OrderedNames { get; set; }
        /// <summary>
        /// ��ѡ������
        /// </summary>
        public List<string> CheckedOrderedNames { get; set; }
        private void okbutton1_Click(object sender, EventArgs e)
        {
            if (this.checkedListBox1.CheckedItems.Count < 1)
            {
                MessageBox.Show("��ѡ����Ŀ��");
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
                MessageBox.Show("��ѡ���������Ŀ��");
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
                MessageBox.Show("��ѡ���������Ŀ��");
                return;
            }
            SaveToList();
            var selecteds = GetSelectedNames();
            foreach (var item in selecteds)
            {
                var oldIndex = OrderedNames.IndexOf(item);
                if(oldIndex ==0){continue;}//��һ�����õ���
                var newIndex = oldIndex - 1;//������ѡ�����õ���
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
                MessageBox.Show("��ѡ���������Ŀ��");
                return;
            }
            SaveToList();
            var selecteds = GetSelectedNames();
            selecteds.Reverse();
            foreach (var item in selecteds)
            {
                var oldIndex = OrderedNames.IndexOf(item);
                if (oldIndex == OrderedNames.Count -1) { continue; }//���һ�����õ���
                var newIndex = oldIndex + 1;//������ѡ�����õ���
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
                MessageBox.Show("��ѡ���������Ŀ��");
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