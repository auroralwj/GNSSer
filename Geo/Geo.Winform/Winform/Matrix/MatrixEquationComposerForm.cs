//2019.02.20, czs, create in hongiqng, 矩阵查看器

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Geo;
using Geo.Utils;
using Geo.Winform;
using Geo.Winform.Demo;
using Geo.Winform.Tools;
using Geo.Winform.Wizards;
using Geo.WinTools; 
using System.Configuration;
using System.IO;
using Geo.Algorithm;

namespace Geo.Winform
{
    public partial class MatrixEquationComposerForm : Form
    {
        public MatrixEquationComposerForm()
        {
            InitializeComponent();
            this.fileOpenControl_filePath.Filter = Setting.MatrixEquationFileFilter;
        }

        MatrixEquationComposer EquationComposer;
        /// <summary>
        /// 输出目录
        /// </summary>
        public string OutputDirectory => directorySelectionControl1.Path;
        /// <summary>
        /// 每行是否独立观测
        /// </summary>
        public bool IsIndepdentObs => checkBox_independentObsOfEach.Checked;

        private void DataBind(MatrixEquation mat)
        {
           Entity = mat;

            this.richTextBoxControl_left.Text = Entity.LeftSide.ToReadableText();
            StringBuilder sb = new StringBuilder();
            sb.AppendLine(Entity.RightSide.ToReadableText());

            if (Entity.HasWeightOfRightSide && Entity.QofU.RowCount < 100)
            {
                sb.AppendLine("右边权逆阵");
                sb.AppendLine(Entity.QofU.ToReadableText());
            }

            this.richTextBoxControl_right.Text = sb.ToString();

            var path = Path.Combine(OutputDirectory, "Combied" + Setting.TextMatrixEquationFileExtension);
            WriteToTextFile(path);
            Geo.Utils.FormUtil.ShowOkAndOpenDirectory(Path.GetDirectoryName(path));
        }

        private void button_saveAsText_Click(object sender, EventArgs e)
        {
            if (Entity == null) { Geo.Utils.FormUtil.ShowWarningMessageBox("请先读取或生成矩阵再试。"); return; }
            SaveFileDialog dlg = new SaveFileDialog();
            dlg.FileName = "Combied";
            dlg.Filter = Setting.TextMatrixEquationFileFilter;
            if (dlg.ShowDialog() == DialogResult.OK)
            {
                var path = dlg.FileName;
                WriteToTextFile(path); 
                Geo.Utils.FormUtil.ShowOkAndOpenDirectory(Path.GetDirectoryName(path));
            }
        }

        private void WriteToTextFile(string path)
        {
            var writer = new TextMatrixEquationWriter(path);
            writer.Write(Entity);
            writer.Close();
        }

        /// <summary>
        /// 矩阵方程
        /// </summary>
        public MatrixEquation Entity { get; set; }

        private void button_saveAsBinary_Click(object sender, EventArgs e)
        {
           /* if(Matrix == null) { Geo.Utils.FormUtil.ShowWarningMessageBox("请先读取或生成矩阵再试。"); return; }
            SaveFileDialog dlg = new SaveFileDialog();
            dlg.Filter = Setting.BinaryMatrixFileFilter;
            dlg.FileName = "matrix";
            if (dlg.ShowDialog() == DialogResult.OK)
            {
                var path = dlg.FileName;
                var writer = new BinaryMatrixWriter(path);
                writer.Write(Matrix);
                writer.Close();
                Geo.Utils.FormUtil.ShowOkAndOpenDirectory(Path.GetDirectoryName(path));
            }*/
        }

        private void button_run_Click(object sender, EventArgs e)
        {
            var isMultiEqsInOneFile = this.checkBox_multiEquation.Checked;

            EquationComposer = new MatrixEquationComposer();

            if (!isMultiEqsInOneFile)//多文件叠加第一个
            {
                var pathes = this.fileOpenControl_filePath.FilePathes;
                foreach (var path in pathes)
                {
                    MatrixEquationReader reader = new MatrixEquationReader(path);

                    var obj = reader.Read();

                    EquationComposer.AddSubMatrix(obj);
                } 
            }
            else//单文件或多文件叠加所有
            {
                var pathes = this.fileOpenControl_filePath.FilePathes;

                int i = 0;

                foreach (var path in pathes)
                { 
                    var matrixReader = new TextMatrixEquationsReader(path);
                    var mat = matrixReader.Read();

                     i = mat.AddNumbersToObs(i);

                    foreach (var obj in mat)
                    {
                        EquationComposer.AddSubMatrix(obj);
                    } 
                } 
            }

            var result = EquationComposer.Build();

            DataBind(result);

            //var path = this.fileOpenControl1.FilePath;
            //if (!File.Exists(path))
            //{
            //    Geo.Utils.FormUtil.ShowWarningMessageBox("请选择文件后再试。");
            //    return;
            //}
            //if (path.ToLower().EndsWith(Setting.BinaryMatrixFileExtension.ToLower()))//二进制
            //{
            //    BinaryMatrixReader matrixReader = new BinaryMatrixReader(path);
            //    var mat = matrixReader.Read();
            //    this.DataBind(mat);
            //}
            //else
            //{
            //    TextMatrixReader matrixReader = new TextMatrixReader(path);
            //    var mat = matrixReader.Read();
            //    this.DataBind(mat);
            //}
        }

        private void checkBox_singleFIle_CheckedChanged(object sender, EventArgs e)
        {
            //fileOpenControl_filePath.IsMultiSelect = !checkBox_multiEquation.Checked;
            //if (fileOpenControl_filePath.IsMultiSelect)
            //{
            //    fileOpenControl_filePath.Height = 75;
            //}
            //else
            //{
            //    fileOpenControl_filePath.Height = 25;
            //}
        }

        private void fileOpenControl_filePath_FilePathSetted(object sender, EventArgs e)
        {
            directorySelectionControl1.Path = Path.GetDirectoryName(fileOpenControl_filePath.FilePath);
        }
    }
}
