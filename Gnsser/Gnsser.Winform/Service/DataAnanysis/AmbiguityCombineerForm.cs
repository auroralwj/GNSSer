//2018.09.28, czs, create in hmx, 模糊度合成器

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.IO;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Geo;


namespace Gnsser.Winform
{
    public partial class AmbiguityCombineerForm : Form
    {
        public AmbiguityCombineerForm()
        {
            InitializeComponent();
            fileOpenControlA.Filter = Setting.AmbiguityFileFilter;
            fileOpenControlB.Filter = Setting.AmbiguityFileFilter;
        }

        private void button_combine_Click(object sender, EventArgs e)
        {
            var pathA = this.fileOpenControlA.FilePath;
            var pathB = this.fileOpenControlB.FilePath;
            if(!File.Exists(pathA) || !File.Exists(pathB))
            {
                Geo.Utils.FormUtil.ShowWarningMessageBox("请选择模糊度文件后再试！");
                return;
            }

            var ambiA = PeriodRmsedNumeralStoarge.Read(pathA);
            var ambiB = PeriodRmsedNumeralStoarge.Read(pathB);

            var result = PeriodRmsedNumeralStoarge.Combine(ambiA, ambiB);

            objectTableControlA.DataBind(ambiA.ToTable());
            objectTableControlB.DataBind(ambiB.ToTable());
            objectTableControlC.DataBind(result.ToTable());

            var outputPath = Path.Combine(Setting.TempDirectory, "CombinedOf" + Path.GetFileNameWithoutExtension(pathA) + "And" + Path.GetFileNameWithoutExtension(pathB) + Setting.AmbiguityFileExtension);
            var table = result.ToTable();
            var writer = new ObjectTableWriter(outputPath);
            writer.Write(table);
            writer.Close();
            Geo.Utils.FormUtil.ShowOkAndOpenDirectory(Setting.TempDirectory);
        }

        private void AmbiguityCombineerForm_Load(object sender, EventArgs e)
        {

        }
    }
}
