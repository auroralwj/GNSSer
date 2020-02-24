//2017.10.03, czs, create in hongqing, 绘图窗口工具

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;

namespace Geo.Winform
{
   /// <summary>
    /// 绘图窗口工具
   /// </summary>
    public partial class DrawObjectOptionForm : Form
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="pathes"></param>
        public DrawObjectOptionForm(string [] pathes)
        {
            InitializeComponent();
            Tables = new List<ObjectTableStorage>();
            foreach (var item in pathes)
            { 
                if (!File.Exists(item))    { continue; }
                 var table = ObjectTableReader.Read(item);
                Tables.Add(table);
            }
            paramVectorRenderControl1.SetTableTextStorage(Tables[0]);

        }
        List<ObjectTableStorage > Tables { get; set; }
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="TableObjectStorage"></param>
        public DrawObjectOptionForm(ObjectTableStorage TableObjectStorage)
        {
            InitializeComponent();
            Tables = new List<ObjectTableStorage>();
            Tables.Add(TableObjectStorage);
            paramVectorRenderControl1.SetTableTextStorage(TableObjectStorage);
        }

        private void button_draw_Click(object sender, EventArgs e)
        {
            //paramVectorRenderControl1.SetTableTextStorage(TableObjectStorage);
            foreach (var item in Tables)
            {
                paramVectorRenderControl1.SetTableTextStorage(item);
                paramVectorRenderControl1.DrawParamLines();
            }

            this.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.Close();
        }

        private void button_cancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.Close();

        }
    }
}
