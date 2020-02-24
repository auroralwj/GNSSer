//2017.07.13, czs, edit in hongqing, 进行了梳理，增加注释和异常处理。

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Geo.Coordinates;
using System.IO;

namespace Gnsser.Winform
{
    public partial class CombinationXyzForm : Form
    {
        public CombinationXyzForm()
        {
            InitializeComponent();
        }

        private void button_run_Click(object sender, EventArgs e)
        {
            try
            {

                var files = this.fileOpenControlA.FilePathes;
                var output = this.fileOutputControl1.FilePath;
                using (var writer = new GnsserXyzCoordWriter(output))
                {
                    writer.WriteHeaderLine();

                    List<NamedXyz> coords = new List<NamedXyz>();
                    foreach (var item in files)
                    {
                        var reader = new GnsserXyzCoordReader(item);
                        var all = reader.ReadAll();
                        foreach (var item1 in all)
                        {
                            writer.Write(item1);
                        }
                    }
                    writer.Close();
                }
                Geo.Utils.FormUtil.ShowIfOpenDirMessageBox(output);
            }
            catch (Exception ex)
            {
                Geo.Utils.FormUtil.ShowWarningMessageBox("抱歉不支持这种格式，" + ex.Message);
            }
        }

        private void fileOpenControlA_FilePathSetted(object sender, EventArgs e)
        {
            if (String.IsNullOrWhiteSpace(fileOutputControl1.FilePath))
            {
                fileOutputControl1.FilePath = Path.Combine(Path.GetDirectoryName(this.fileOpenControlA.FirstPath), "CombinedGnsserCoord.xls");
            }
        }
    }
}
