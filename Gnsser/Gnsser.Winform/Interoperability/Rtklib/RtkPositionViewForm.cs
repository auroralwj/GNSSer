//2018.10.12, czs, create in hmx, RTKLIB坐标文件查看

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.IO;
using Geo.IO;
using System.Windows.Forms;
using Gnsser.Interoperation;

namespace Gnsser.Winform
{
    public partial class RtkPositionViewForm : Form
    {
        public RtkPositionViewForm()
        {
            InitializeComponent();

            fileOpenControl1.Filter = "Rtklib 结果|*.pos|txt|*.txt|*.*|*.*"; 
        }
         
         
        private void RtkPositionViewForm_Load(object sender, EventArgs e)
        {

        }

        private void button_load_Click(object sender, EventArgs e)
        {
            var path = this.fileOpenControl1.FilePath;
            if (!File.Exists(path))
            {
                Geo.Utils.FormUtil.ShowWarningMessageBox("文件不存在！" + path);
                return;
            }
            
            RtkpostResult result = RtkpostResult.Load(path);

            richTextBoxControl2.Text = Geo.Utils.StringUtil.ToLineString(result.Comments);
            objectTableControl1.DataBind(result.ToTable(Path.GetFileName(path)));
        }
    }
}
