//2016.07.27, czs, create in 福建 永安, 枚举生成界面
//2016.08.04, czs, edit in 福建 永安, 修改自 Ratiobutton 重命名委托
//2016.10.05, czs, edit in honqing, 重构为通用数组选择器。
//2019.03.15, czs, edit in hongqing, 界面最大显示数设置为 1000

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Geo.IO;

namespace Geo.Winform
{
     

    /// <summary>
    /// 枚举生成界面
    /// </summary>
    public partial class ArrayCheckBoxControl : UserControl
    {
        Log log = new Log(typeof(ArrayCheckBoxControl));
        /// <summary>
        /// 默认构造函数
        /// </summary>
        public ArrayCheckBoxControl()
        {
            InitializeComponent();
        }
        /// <summary>
        /// 已经选择枚举项目。
        /// </summary>
        public event Action<string, bool> EnumItemSelected;
        /// <summary>
        /// 标题
        /// </summary>
        public string Title
        {
            get { return this.groupBox1.Text; }
            set { this.groupBox1.Text = value; }
        }
        /// <summary>
        /// 初始化
        /// </summary>
        /// <typeparam name="T"></typeparam>
        public void Init<T>(IEnumerable<T> objs )
        {
            var olds = this.GetSelected<T>();

            this.flowLayoutPanel1.Controls.Clear();
             
            int i = 0;
            foreach (var name in objs.ToArray())
            {
                 BuildBtnAndAddToControl(name);
                //if (i == 0) { btn.Checked = true; }
                i++;
            }

            this.Select<T>(olds);
        }
        /// <summary>
        /// 添加一个
        /// </summary>
        /// <param name="txt"></param>
        public void BuildBtnAndAddToControl(object tag)
        {
            if (this.flowLayoutPanel1.Controls.Count > 1000)
            {
                log.Warn("已超出界面最大支持显示数 " + this.flowLayoutPanel1.Controls.Count);
                return;
            }
            string txt = tag.ToString();
            CheckBox btn = new CheckBox();
            btn.Text = txt;
            btn.Name = txt;
            btn.Tag = tag;
            btn.AutoSize = true;
            btn.CheckedChanged += btn_CheckedChanged;
            this.flowLayoutPanel1.Controls.Add(btn); 
        }

        void btn_CheckedChanged(object sender, EventArgs e)
        {
            CheckBox btn = sender as CheckBox;
            if (EnumItemSelected!=null)
            {
                EnumItemSelected(btn.Text, btn.Checked);
            }
        }
        /// <summary>
        /// 当前类型
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public List<T> GetSelected<T>()
        {
            List<T> list = new List<T>();
            foreach (var item in CurrentCheckBoxes)
            {
                var e = (T)item.Tag;
                list.Add(e);
            }
            return list;
        }
        /// <summary>
        /// 使其选择上
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="val"></param>
        public void Select<T>(T val)
        {
            foreach (var item in this.flowLayoutPanel1.Controls)
            {
                if (item is CheckBox)
                {
                    var btn = item as CheckBox;
                    if (btn.Text == val + "") { if(!btn.Checked) btn.Checked = true; break ; }
                }
            }
        }

        public void Select<T>(List<T> objs)
        {
            foreach (var item in objs)
            {
                this.Select<T>(item);
            }
        }

        /// <summary>
        /// 当前选择
        /// </summary>
        public List<string> CurrentTexts
        {
            get
            {
                List<string> list = new List<string>();
                foreach (var item in this.flowLayoutPanel1.Controls)
                {
                    if (item is CheckBox)
                    {
                        var btn = item as CheckBox;
                        if (btn.Checked) { list.Add(btn.Text); }
                    }
                }
                return list;
            }
        }
        /// <summary>
        /// 当前选择
        /// </summary>
        public List<CheckBox> CurrentCheckBoxes
        {
            get
            {
                List<CheckBox> list = new List<CheckBox>();
                foreach (var item in this.flowLayoutPanel1.Controls)
                {
                    if (item is CheckBox)
                    {
                        var btn = item as CheckBox;
                        if (btn.Checked) { list.Add(btn); }
                    }
                }
                return list;
            }
        }

        private void EnumRadioControl_Load(object sender, EventArgs e)
        {
           
        }

        private void 全选AToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var checkState = true;
            SetChecked(checkState);

        }

        private void SetChecked(bool checkState)
        {
            foreach (var item in this.flowLayoutPanel1.Controls)
            {
                if (item is CheckBox)
                {
                    var btn = item as CheckBox;
                    btn.Checked = checkState;
                }
            }
        }

        private void SetChecked(bool checkState, string key)
        {
            foreach (var item in this.flowLayoutPanel1.Controls)
            {
                if (item is CheckBox)
                {
                    var btn = item as CheckBox;
                    if (btn.Text.Contains(key))
                    { 
                        btn.Checked = checkState;
                    }
                }
            }
        }

        private void 全部清除CToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SetChecked(false);
        }

        private void 反选RToolStripMenuItem_Click(object sender, EventArgs e)
        {
            foreach (var item in this.flowLayoutPanel1.Controls)
            {
                if (item is CheckBox)
                {
                    var btn = item as CheckBox;
                    btn.Checked =!btn.Checked;
                }
            }

        }

        private void 选择关键字SToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var key = "";
            if(Geo.Utils.FormUtil.ShowInputForm(out key))
            { 
                SetChecked(true, key);
            } 
        }

        private void 清除关键字KToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var key = "";
            if (Geo.Utils.FormUtil.ShowInputForm(out key))
            {
                SetChecked(false, key);
            }
        }
    }
}
