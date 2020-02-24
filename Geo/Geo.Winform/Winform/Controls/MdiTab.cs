using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Drawing;

namespace Geo.Winform.Controls
{   
 

    /// <summary>
    /// Tab 面板。
    /// 用法：
    /// this.mdiTab1.MdiParent = this;
    /// this.mdiTab1.OpenMdiChild(form); 
    /// </summary>
    public class MdiTab :  System.Windows.Forms.ToolStrip
    { 
        public int SelectedTabIndex { get; set; }

        public MdiTab()// : base() 
        {
            InitializeComponent();

            this.LayoutStyle = ToolStripLayoutStyle.Flow;
            this.GripStyle = ToolStripGripStyle.Hidden;
            this.MdiParent = this.Parent as Form;

            this.toolStripMenuItem_close.Click += new EventHandler(toolStripMenuItem_close_Click);
            this.toolStripMenuItem_closeOthers.Click += new EventHandler(toolStripMenuItem_closeOthers_Click);
        }
        void toolStripMenuItem_closeOthers_Click(object sender, EventArgs e)
        {
            List<Form> toBeClosed = new List<Form>();

            foreach (ToolStripItem item in this.Items) 
                toBeClosed.Add(item.Tag as Form); 

            toBeClosed.RemoveAt(SelectedTabIndex);

            foreach (Form item in toBeClosed) item.Close();
        }

        void toolStripMenuItem_close_Click(object sender, EventArgs e)
        {
            Form form = this.Items[SelectedTabIndex].Tag as Form;
            form.Close();   
        }

        private ContextMenuStrip contextMenuStrip1;
        private System.ComponentModel.IContainer components;
        private ToolStripMenuItem toolStripMenuItem_close;
        private ToolStripMenuItem toolStripMenuItem_closeOthers;

        Form mdiParent;

        public Form MdiParent
        {
            get { return mdiParent; }
            set { mdiParent = value;  
            }
        }
        /// <summary>
        /// 如果已经有该类型的对象，则打开，并返回null，如果没有，则从默认构造函数创建，并返回该对象。
        /// </summary>
        /// <param name="title"></param>
        /// <param name="formType"></param>
        public Form OpenMidForm(string title, Type formType)
        {
            Form form = null;
            if (!Activate(title))
            {
                System.Reflection.ConstructorInfo ct = formType.GetConstructor(Type.EmptyTypes);
                if (ct == null) return null;

                form = (ct.Invoke(null) as Form);
                form.Text = title;
                OpenMdiChild(form);
            }
            return form;
        }

        /// <summary>
        /// 打开
        /// </summary>
        /// <param name="form"></param>
        public void OpenMdiChild(Form form)
        {             
            bool suc = Activate(form);

            if (suc) {
                //      form.Dispose(); 
                return;
            }

            form.MdiParent = MdiParent;
            form.WindowState = FormWindowState.Maximized;
            form.Show();

            ToolStripButton item = new ToolStripButton(form.Text, null);
            item.Margin = new System.Windows.Forms.Padding(5, 5, 5, 0);
            item.Padding = new System.Windows.Forms.Padding(10, 5, 10, 0);
            
            item.Tag = form;
            item.DoubleClickEnabled = true;
            item.Click += new EventHandler(item_Click);
            item.DoubleClick += new EventHandler(item_DoubleClick);
            //Form 信息反馈
            form.FormClosed += new FormClosedEventHandler(form_FormClosed); 
            form.Activated += new EventHandler(form_Activated);
            this.Items.Add(item);
            
            Activate(item);
        }

        //窗口激活，同步更新面板菜单。
        void form_Activated(object sender, EventArgs e)
        {
            Form form = sender as Form;
            Activate(form);
        } 

        void form_FormClosed(object sender, FormClosedEventArgs e)
        {
            Form form = sender as Form;
            foreach (ToolStripItem it in this.Items)
            {
                if (it.Text == form.Text)
                {
                    this.Items.Remove(it);
                    it.Dispose();
                    return;
                }
            }

            foreach (Form f in MdiParent.MdiChildren)
            {
                if (f.Focused && f != form) OpenMdiChild(form);
            }

        }

        /// <summary>
        /// 将指定的表单激活，如果没有该表单则返回false。
        /// </summary>
        /// <param name="form"></param>
        /// <returns></returns>
        public bool Activate(Form form) {   return Activate(form.Text);  }
        /// <summary>
        /// 将指定名称的表单激活
        /// </summary>
        /// <param name="title"></param>
        /// <returns></returns>
        public bool Activate(string title)
        {
            bool suc = false;
            for (int i=0; i<  this.Items.Count;i++)  {
                ToolStripItem it = this.Items[i];
                if (it.Text == title)
                { 
                    Activate(it);
                    return true;
                }
            }
            return suc;
        }
        /// <summary>
        /// 获取窗口对象，失败为null
        /// </summary>
        /// <param name="title"></param>
        /// <returns></returns>
        public Form GetForm(string title)
        { 
            for (int i = 0; i < this.Items.Count; i++)
            {
                ToolStripItem it = this.Items[i];
                if (it.Text == title)
                { 
                    return it.Tag as Form;
                }
            }
            return null;

        }

        /// <summary>
        /// 包含
        /// </summary>
        /// <param name="title"></param>
        /// <returns></returns>
        public bool Contains(string title)
        {
            bool suc = false;
            for (int i = 0; i < this.Items.Count; i++)
            {
                ToolStripItem it = this.Items[i];
                if (it.Text == title)
                { 
                    suc = true;
                    return true;
                }
            }
            return suc;

        }
        public void Close(string title)
        {
            foreach (Form f in MdiParent.MdiChildren)
            {
                if (String.Equals(f.Text, title, StringComparison.CurrentCultureIgnoreCase))
                {
                    f.Close(); 
                }
            }
        }

        /// <summary>
        /// 激活按钮绑定的窗口。窗口获得焦点。
        /// </summary>
        /// <param name="key"></param>
        private void Activate(ToolStripItem item)
        {
            //设置为未选中
            foreach (ToolStripItem it in this.Items)
            {
                it.BackColor = Color.FromArgb(222, 230, 250);
                item.Font = new System.Drawing.Font(item.Font.FontFamily, item.Font.Size, FontStyle.Regular);
            }
            
            Form form = item.Tag as Form;
            if(!form.ContainsFocus)
            form.Activate();

            SelectedTabIndex = this.Items.IndexOf(item);

            item.BackColor = Color.FromArgb(150, 160, 250);
            item.Font = new System.Drawing.Font(item.Font.FontFamily, item.Font.Size, FontStyle.Bold);
        }

        void item_DoubleClick(object sender, EventArgs e)
        {
            ToolStripButton item = sender as ToolStripButton;
            Form form = item.Tag as Form;
            if(form !=null && !form.IsDisposed)    form.Close();

            this.Items.Remove(item);
            item.Dispose();
           
            //foreach (Form f in MdiParent.MdiChildren)
            //{
            //    if (f.Focused && f != form) OpenMdiChild(f);
            //}
        }
        void item_Click(object sender, EventArgs e)
        {
            ToolStripButton item = sender as ToolStripButton;
            Form form = item.Tag as Form;
            form.Activate();
            Activate(item);
        }

        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.toolStripMenuItem_close = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem_closeOthers = new System.Windows.Forms.ToolStripMenuItem();
            this.contextMenuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripMenuItem_close,
            this.toolStripMenuItem_closeOthers});
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.Size = new System.Drawing.Size(137, 48);
            // 
            // toolStripMenuItem_close
            // 
            this.toolStripMenuItem_close.Name = "toolStripMenuItem_close";
            this.toolStripMenuItem_close.Size = new System.Drawing.Size(136, 22);
            this.toolStripMenuItem_close.Text = "关闭(&C)";
            // 
            // toolStripMenuItem_closeOthers
            // 
            this.toolStripMenuItem_closeOthers.Name = "toolStripMenuItem_closeOthers";
            this.toolStripMenuItem_closeOthers.Size = new System.Drawing.Size(136, 22);
            this.toolStripMenuItem_closeOthers.Text = "关闭其它(&O)";
            // 
            // MdiTab
            // 
            this.ContextMenuStrip = this.contextMenuStrip1;
            this.Size = new System.Drawing.Size(100, 23);
            this.contextMenuStrip1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

    }
}
