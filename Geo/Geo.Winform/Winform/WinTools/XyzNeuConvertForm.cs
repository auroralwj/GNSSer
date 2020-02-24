//2018.06.23, czs, create in HMX, 地心坐标转换为本地北东天

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Geo.Coordinates;

namespace Geo.Winform.Tools
{
    /// <summary>
    /// 地心坐标转换为本地北东天
    /// </summary>
    public partial class XyzNeuConvertForm : Form
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public XyzNeuConvertForm()
        {
            InitializeComponent();
        }

        private void button_exchangeXyz_Click(object sender, EventArgs e)
        {
            var temp = richTextBox_originEcefXyz.Text;
            richTextBox_originEcefXyz.Text = richTextBox_targetEcefXyz.Text;
            richTextBox_targetEcefXyz.Text = temp; 
        }

        private void button_xyzToNeu_Click(object sender, EventArgs e)
        {
            try
            {
                List<XYZ> originEcefXyz = GetOriginEcefXyz();
                List<XYZ> targetEcefXyz = GetTargetEcefXyz();

                ObjectTableManager manager = new ObjectTableManager();
                var table = manager.AddTable("ENU");
                int count = originEcefXyz.Count;
                for (int i = 0; i < count; i++)
                {
                    var oXyz = originEcefXyz[i];
                    var tXyz = targetEcefXyz[i];
                    var enu = CoordTransformer.XyzToEnu(tXyz, oXyz);

                    table.NewRow();
                    table.AddItem("Index", i);
                    table.AddItem("E", enu.E);
                    table.AddItem("N", enu.N);
                    table.AddItem("U", enu.U);
                }

                this.objectTableControl1.DataBind(table);
            }catch(Exception ex)
            {
                Geo.Utils.FormUtil.ShowErrorMessageBox(ex.Message);
            }
        }
 

        public List<XYZ> GetOriginEcefXyz()
        {
            var strs = this.richTextBox_originEcefXyz.Lines;
            List<XYZ> list = new List<XYZ>();
            foreach (string item in strs)
            {
                if (String.IsNullOrWhiteSpace(item)) { continue; }
                list.Add(XYZ.Parse(item));
            }

            return list;
        }

        public List<XYZ> GetTargetEcefXyz()
        {
            var strs = this.richTextBox_targetEcefXyz.Lines;
            List<XYZ> list = new List<XYZ>();
            foreach (string item in strs)
            {
                if (String.IsNullOrWhiteSpace(item)) { continue; }
                list.Add(XYZ.Parse(item));
            }

            return list;
        }
    }
}
