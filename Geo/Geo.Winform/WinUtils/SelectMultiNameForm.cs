//2018.08.11, czs,  edit in hmx, 修改

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Linq;
using System.Windows.Forms;

namespace Geo.Utils
{
    /// <summary>
    /// 选择文本
    /// </summary>
    public partial class SelectMultiNameForm : Form
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public SelectMultiNameForm()
        {
            InitializeComponent();
        }
        /// <summary>
        /// 以数组初始化
        /// </summary>
        /// <param name="strs"></param>
        /// <param name="isSelectAll"></param>
        public SelectMultiNameForm(string[] strs, bool isSelectAll = true)
        {
            InitializeComponent();
            AllNames = new List<string>(strs);
            Init(strs);
            if (isSelectAll) { selectAllbutton1_Click(null, null); }
        }
        /// <summary>
        /// 所有的
        /// </summary>
        public List<string> AllNames { get; set; }
        /// <summary>
        /// 初始化
        /// </summary>
        /// <param name="strs"></param>
        public void Init(string[] strs)
        {
            Clear();
            this.checkedListBox1.Items.AddRange(strs);
        }

        private void Clear()
        {
            this.checkedListBox1.Items.Clear();
        }

        /// <summary>
        /// 设置所选
        /// </summary>
        /// <param name="strs"></param>
        public void SetSelected(IEnumerable<string> strs)
        {
            int count = this.checkedListBox1.Items.Count;
            for (int i = 0; i < count; i++)
            {
                var text = this.checkedListBox1.Items[i].ToString();
                if (strs.Contains(text))
                {
                    this.checkedListBox1.SetItemChecked(i, true);
                }
                else
                {
                    this.checkedListBox1.SetItemChecked(i, false);
                }
            }

        }

         
        bool isVertical = true;
        /// <summary>
        /// 是否列显示
        /// </summary>
        public bool IsVertical
        {
            get { return isVertical; }
            set { isVertical = value; }
        }

        /// <summary>
        /// 名称列表
        /// </summary>
        public List<string> SelectedNames { get; set; }

        private void okbutton1_Click(object sender, EventArgs e)
        {
            SelectedNames = new List<string>();
            if (this.checkedListBox1.CheckedItems.Count < 1)
            {
                MessageBox.Show("选择列不能为空。");
                return;
            }
            foreach (object item in this.checkedListBox1.CheckedItems)
            {
                SelectedNames.Add(item.ToString());
            }
            if (!this.checkBox1.Checked)
            {
                isVertical = false;
            }
            this.DialogResult = DialogResult.OK;
            Close();
        }

        /// <summary>
        /// 将所有的选中。
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void selectAllbutton1_Click(object sender, EventArgs e)
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

        //清除所有选择
        private void cancelAllbutton1_Click(object sender, EventArgs e)
        {
            int count = this.checkedListBox1.Items.Count;
            for (int i = 0; i < count; i++)
            {
                if (this.checkedListBox1.GetItemChecked(i))
                {
                    this.checkedListBox1.SetItemChecked(i, false);
                }
            }
        }

        //反选
        private void invertSelectbutton1_Click(object sender, EventArgs e)
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

        private void SelectingColForm_Load(object sender, EventArgs e)
        {

        }

        private void button_filter_Click(object sender, EventArgs e)
        {            
            string keyword;
            if (Geo.Utils.FormUtil.ShowInputForm("请输入过滤关键字","D", out keyword))
            {    
                int j = 0;
                var list = new List<object>( );
                foreach (var item in this.checkedListBox1.Items)
                {
                    list.Add(item);
                    j++;
                }

                int count = this.checkedListBox1.Items.Count;
                for (int i = 0; i < count; i++)
                { 
                    var text = list[i].ToString();
                    if (text.Contains(keyword))
                    {
                        this.checkedListBox1.SetItemChecked(i, true);
                    }
                }
            }
        }

        private void button_filterExclude_Click(object sender, EventArgs e)
        {
            string keyword;
            if (Geo.Utils.FormUtil.ShowInputForm("请输入过滤关键字", "L", out keyword))
            {
                int j = 0;
                var list = new List<object>();
                foreach (var item in this.checkedListBox1.Items)
                {
                    list.Add(item);
                    j++;
                }

                int count = this.checkedListBox1.Items.Count;
                for (int i = 0; i < count; i++)
                {
                    var text = list[i].ToString();
                    if (text.Contains(keyword))
                    {
                        this.checkedListBox1.SetItemChecked(i, false);
                    }
                }
            }

        }

        private void button_sort_Click(object sender, EventArgs e)
        {
            AllNames.Sort();
            Init(AllNames.ToArray());
        }
    }
}