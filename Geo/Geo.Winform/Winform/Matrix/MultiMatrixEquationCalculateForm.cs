//2019.03.05, czs, create in hongiqng, 多矩阵方程计算

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
using Geo.IO;

namespace Geo.Winform
{
    /// <summary>
    /// 多矩阵方程计算
    /// </summary>
    public partial class MultiMatrixEquationCalculateForm : Form
    {
        ILog log = new Log(typeof(MultiMatrixEquationCalculateForm));
        /// <summary>
        /// 构造函数
        /// </summary>
        public MultiMatrixEquationCalculateForm()
        {
            InitializeComponent();
            this.fileOpenControl_filePath.Filter = Setting.MatrixEquationFileFilter;
        }

        MultiMatrixEquationComputer EquationComposer { get; set; }
        public string OutputDirectory => directorySelectionControl1.Path;

        private void DataBind(MatrixEquation mat)
        {
           Entity = mat;




            //具有固定参数，则生成打开
            if (this.HasOrEnableFixedParam)
            {
                var fixedStorage = this.GetFixedParamStorage();
                if (fixedStorage != null)
                {
                    ParamFixedMatrixEquationBuilder equationBuilder = new ParamFixedMatrixEquationBuilder(fixedStorage);
                    var newEq = equationBuilder.Build(Entity);

                    MatrixEquationForm form = new MatrixEquationForm();
                    form.DataBind(newEq);
                    form.Show();
                }
            }










            this.richTextBoxControl_left.Text = Entity.LeftSide.ToReadableText();
            StringBuilder sb = new StringBuilder();
            sb.AppendLine(Entity.RightSide.ToReadableText());

            if (Entity.HasWeightOfRightSide && Entity.QofU.RowCount < 100)
            {
                sb.AppendLine("右边权逆阵");
                sb.AppendLine(Entity.QofU.ToReadableText());
            }

            this.richTextBoxControl_right.Text = sb.ToString();

            //输出
            var path = Path.Combine(OutputDirectory, mat.Name + Setting.TextMatrixEquationFileExtension);
            WriteToTextFile(path);


            //法方程与观测残差
            sb = new StringBuilder();
            var normal = Entity.NormalEquation;
            sb.AppendLine("法方程文本");
            sb.AppendLine(normal.ToReadableText());

            sb.AppendLine("参数计算结果");
            var est = normal.GetEstimated();
            sb.AppendLine(est.ToReadableText());

            var table = est.GetObjectTable();
            this.objectTableControl1.DataBind(table);
            //写结果
            var paramPath = Path.Combine(OutputDirectory, "EstimatedParamOf" + Entity.Name + Setting.ParamFileExtension);
            ObjectTableWriter.Write(table, paramPath);


            sb.AppendLine("观测残差");
            sb.AppendLine(normal.GetResidual().ToReadableText());

            richTextBoxControl_normal.Text = sb.ToString();

        }

        private void button_saveAsText_Click(object sender, EventArgs e)
        {
            if (Entity == null) { Geo.Utils.FormUtil.ShowWarningMessageBox("请先读取或生成矩阵再试。"); return; }
            SaveFileDialog dlg = new SaveFileDialog();
            dlg.FileName = "NormalIteration";
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

        /// <summary>
        /// 是否启用或具有固定参数文件
        /// </summary>
        bool HasOrEnableFixedParam => this.checkBox_enbaleFixParam.Checked && File.Exists(this.fileOpenControl_fixedParam.FilePath);
        /// <summary>
        /// 获取模糊度存储
        /// </summary>
        /// <returns></returns>
        public PeriodRmsedNumeralStoarge GetFixedParamStorage()
        {
            var path = this.fileOpenControl_fixedParam.FilePath;
            if (!File.Exists(path))
            {
                return null;
            }
            PeriodRmsedNumeralStoarge result = PeriodRmsedNumeralStoarge.Read(path);
            return result;
        }
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
            var start = DateTime.Now;

            var isSingleFile = this.checkBox_singleFIle.Checked;

            EquationComposer = new MultiMatrixEquationComputer();
            if (!isSingleFile)//多文件
            {
                var pathes = this.fileOpenControl_filePath.FilePathes;
                EquationComposer.PostFixName = Path.GetFileNameWithoutExtension(pathes[0]) + "s";
                foreach (var path in pathes)
                {
                    MatrixEquationReader reader = new MatrixEquationReader(path);

                    var obj = reader.Read();

                    EquationComposer.AddSubMatrix(obj);
                }
            }
            else//单文件
            {
                var path = this.fileOpenControl_filePath.FilePath;
                EquationComposer.PostFixName = Path.GetFileNameWithoutExtension(path);

                var matrixReader = new TextMatrixEquationsReader(path);
                var mat = matrixReader.Read();
                mat.AddNumbersToObs();

                foreach (var obj in mat)
                {
                    EquationComposer.AddSubMatrix(obj); 
                } 
            }

            var result = EquationComposer.Build();

            DataBind(result);

            var span = DateTime.Now - start;
            log.Info("耗时：" + span.TotalSeconds.ToString("0.000") + " s = " + span);

            Geo.Utils.FormUtil.ShowOkAndOpenDirectory( this.OutputDirectory);
        }

        private void checkBox_singleFIle_CheckedChanged(object sender, EventArgs e)
        {
            fileOpenControl_filePath.IsMultiSelect = !checkBox_singleFIle.Checked;
            if (fileOpenControl_filePath.IsMultiSelect)
            {
                fileOpenControl_filePath.Height = 75;
            }
            else
            {
                fileOpenControl_filePath.Height = 25;
            }
        }

        private void fileOpenControl_filePath_FilePathSetted(object sender, EventArgs e)
        {
            directorySelectionControl1.Path = Path.GetDirectoryName(fileOpenControl_filePath.FilePath);
        }

        private void MultiMatrixEquationCalculateForm_Load(object sender, EventArgs e)
        {
            fileOpenControl_fixedParam.Filter = Setting.AmbiguityFileFilter;

        }
    }
}
