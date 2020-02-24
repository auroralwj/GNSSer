//2019.02.20, czs, create in hongiqng, 矩阵方程查看器

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
    /// 矩阵方程
    /// </summary>
    public partial class MatrixEquationForm : Form
    {
        Log log = new Log(typeof(MatrixEquationForm));
        /// <summary>
        /// 构造函数
        /// </summary>
        public MatrixEquationForm()
        {
            InitializeComponent();
            this.fileOpenControl_eqPath.Filter = Setting.MatrixEquationFileFilter;
        }

        /// <summary>
        /// 矩阵方程
        /// </summary>
        public MatrixEquation Entity { get; set; }
        /// <summary>
        /// 矩阵方程
        /// </summary>
        public MatrixEquationManager Entities { get; set; }
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
        /// <summary>
        /// 最大阶数
        /// </summary>
        int MaxOrderToShow => namedIntControl_notShowOrder.GetValue();

        public void DataBind(MatrixEquation mat)
        {
            Entity = (mat);
             
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

            StringBuilder sb = new StringBuilder();
            if (Entity.MaxSize > MaxOrderToShow)
            {
                sb.Append("超出最大显示阶数：" + Entity.MaxSize);
            }
            else
            {
                //左右方程
                this.richTextBoxControl_left.Text = Entity.LeftSide.ToReadableText();
                this.richTextBoxControl_right.Text = Entity.RightSide.ToReadableText();

                if (Entity.HasWeightOfRightSide)
                {
                    richTextBoxControl_weightOfU.Text = Entity.QofU.ToReadableText();
                }

                sb.AppendLine("方程文本");
                sb.AppendLine(Entity.ToReadableText()); 
            }

            //法方程与观测残差
            var normal = Entity.NormalEquation;
            sb.AppendLine("法方程文本");
            sb.AppendLine(normal.ToReadableText());

            sb.AppendLine("参数计算结果");
            var est = normal.GetEstimated();
            sb.AppendLine(est.ToReadableText());

            var table = est.GetObjectTable();
            this.objectTableControl_result.DataBind(table);
            //写结果
            var paramPath = Path.Combine(OutputDirectory, "EstimatedParamOf" + Entity.Name + Setting.ParamFileExtension);
            ObjectTableWriter.Write(table, paramPath);


            sb.AppendLine("观测残差");
            sb.AppendLine(normal.GetResidual().ToReadableText());
             

            this.richTextBoxControl_result.Text = sb.ToString();
        }
        /// <summary>
        /// 输出目录
        /// </summary>
        string OutputDirectory
        {
            get
            {
                if (string.IsNullOrWhiteSpace(this.directorySelectionControl1.Path))
                {
                    this.directorySelectionControl1.Path = Setting.TempDirectory;
                }
                Geo.Utils.FileUtil.CheckOrCreateDirectory(this.directorySelectionControl1.Path);
                return this.directorySelectionControl1.Path;
            }
        }
        private void DataBind(MatrixEquationManager mat)
        {
            this.Entities = mat;
            Entity = mat.First;
            this.richTextBoxControl_result.Text = mat.ToReadableText();
            var mormal = mat.GetNormalEquations();

            this.objectTableControl_obs.DataBind(mat.GetObsTable());
            this.objectTableControl_result.DataBind(mormal.GetResultTable());
            this.objectTableControl_residual.DataBind(mat.GetResidualTable());
            this.richTextBoxControl_normalEq.Text = mormal.ToReadableText();
        }

        private void button_saveAsText_Click(object sender, EventArgs e)
        {
            if (Entity == null) { Geo.Utils.FormUtil.ShowWarningMessageBox("请先读取或生成矩阵再试。"); return; }
            SaveFileDialog dlg = new SaveFileDialog();
            dlg.FileName = "equation";
            dlg.Filter = Setting.TextMatrixEquationFileFilter;
            if (dlg.ShowDialog() == DialogResult.OK)
            {
                var path = dlg.FileName;
                var writer = new TextMatrixEquationWriter(path);
                writer.Write(Entity);
                writer.Close();


                Geo.Utils.FormUtil.ShowOkAndOpenDirectory(Path.GetDirectoryName(path));
            }
        }

        private void button_saveAsBinary_Click(object sender, EventArgs e)
        {
            if(Entity == null) { Geo.Utils.FormUtil.ShowWarningMessageBox("请先读取或生成矩阵再试。"); return; }
            SaveFileDialog dlg = new SaveFileDialog();
            dlg.Filter = Setting.BinaryMatrixEquationFileExtension;
            dlg.FileName = "equation";
            if (dlg.ShowDialog() == DialogResult.OK)
            {
                var path = dlg.FileName;
                var writer = new TextMatrixEquationWriter(path);
                writer.Write(Entity);
                writer.Close();
                Geo.Utils.FormUtil.ShowOkAndOpenDirectory(Path.GetDirectoryName(path));
            }
        }

        private void button_read_Click(object sender, EventArgs e)
        {
            DateTime start = DateTime.Now;

            var isMulti = this.checkBox_isMulti.Checked;
            var path = this.fileOpenControl_eqPath.FilePath;
            if (!File.Exists(path))
            {
                Geo.Utils.FormUtil.ShowWarningMessageBox("请选择文件后再试。");
                return;
            }
            if (path.ToLower().EndsWith(Setting.BinaryMatrixEquationFileExtension.ToLower()))//二进制
            {
                BinaryMatrixReader matrixReader = new BinaryMatrixReader(path);
                var mat = matrixReader.Read();
                //this.DataBind(mat);
            }
            else
            {
                if (isMulti)
                {
                    var matrixReader = new TextMatrixEquationsReader(path);
                    var mat = matrixReader.Read();
                    this.DataBind(mat); 
                }
                else
                {
                    var matrixReader = new TextMatrixEquationReader(path);
                    var mat = matrixReader.Read();
                    this.DataBind(mat);
                }
            }

            var span = DateTime.Now - start;
            log.Info("计算完毕，耗时：" + span.TotalSeconds.ToString("0.00") + "s = " +  span.ToString());
        }
         
        private void button_randomGen_Click(object sender, EventArgs e)
        {
            DateTime from = DateTime.Now;
            int order = 100;
            int row = this.namedIntControl_row.GetValue();
            int col = this.namedIntControl_col.GetValue();
            var name = namedStringControl_name.GetValue();

            double[][] matrix = MatrixUtil.CreateRandom(row, col);
            Matrix mat = new Matrix(matrix);
            var prefix = namedStringControl_prefName.GetValue();
            List<string> rowNames = new List<string>();
            for (int i = 0; i < row; i++)
            {
                rowNames.Add(prefix + i.ToString("000"));
            }
            List<string> colNames = new List<string>();
            for (int i = 0; i < col; i++)
            {
                colNames.Add(prefix + i.ToString("000"));
            }

            mat.RowNames = rowNames;
            mat.ColNames = colNames;


            double[][] rigth = MatrixUtil.CreateRandom(row, 1);
            Matrix rmat = new Matrix(rigth);
            rmat.RowNames = rowNames;
            Matrix QofU = Matrix.CreateIdentity(row);
            QofU.ColNames = rowNames;
            QofU.RowNames = rowNames;

            var equa = new MatrixEquation(mat, rmat) { Name = name, QofU  = QofU }; 
            DataBind(equa);
        }
 

        private void MatrixEquationForm_Load(object sender, EventArgs e)
        {
            fileOpenControl_fixedParam.Filter = Setting.AmbiguityFileFilter;
            namedStringControl_prefName.SetValue("p");
            namedStringControl_name.SetValue("Name");
        }

        private void checkBox_enbaleFixParam_CheckedChanged(object sender, EventArgs e)
        {
            this.fileOpenControl_fixedParam.Enabled = this.checkBox_enbaleFixParam.Checked;
        }

        private void fileOpenControl_eqPath_FilePathSetted(object sender, EventArgs e)
        {
            directorySelectionControl1.Path = Path.GetDirectoryName(this.fileOpenControl_eqPath.FilePath);
        }
    }
}
