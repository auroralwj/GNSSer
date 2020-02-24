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
using Geo.IO;

namespace Geo.Winform
{
    public partial class MatrixViewerForm : Form
    {
        Log log = new Log(typeof(MatrixViewerForm));
        public MatrixViewerForm()
        {
            InitializeComponent();
            this.fileOpenControl1.Filter = Setting.MatrixTableFileFilter;
        }

        #region 其它测试
        private void 二进制矩阵读写试验ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DateTime from = DateTime.Now;
            int order = 100;
            double[][] matrix = MatrixUtil.CreateRandom(order);
            //double[][] matrix = MatrixUtil.CreateIdentity(order);
            TimeSpan create = DateTime.Now - from;

            from = DateTime.Now;
            string path = @"C:\matrix.bmat";
            MatrixUtil.SaveToBinary(matrix, path);

            //TimeSpan write =  DateTime.Now - from;

            //from = DateTime.Now;
            //double[][] newMatrix = MatrixUtil.FromBinary(path);
            //TimeSpan read = DateTime.Now - from;
            //string msg = ""
            //    + "阶次：" + order + "\r\n"
            //    + "创建：" + create.TotalSeconds +"\r\n"
            //    + "写入：" + write.TotalSeconds + "\r\n"
            //    + "读取：" + read.TotalSeconds + "\r\n";
            //MessageBox.Show(msg
            //    );

            //稀疏矩阵

            Geo.Algorithm.SparseMatrix sM = new Geo.Algorithm.SparseMatrix(matrix);
            path = @"C:\matrix.sbmat";
            sM.ToBinary(path);

            Geo.Algorithm.SparseMatrix s = Geo.Algorithm.SparseMatrix.FromBinary(path);
            double[][] newSmMtirix = s.GetMatrix();
            bool equal = MatrixUtil.IsEqual(newSmMtirix, matrix);


            //MatrixUtil.GetFormatedText(newMatrix);
        }

        private void 文本矩阵读取试验ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DateTime from = DateTime.Now;
            int order = 98;
            double[][] matrix = MatrixUtil.CreateRandom(order);
            TimeSpan create = DateTime.Now - from;

            from = DateTime.Now;
            string path = @"C:\matrix.txt";
            MatrixUtil.SaveToText(matrix, path);

            TimeSpan write = DateTime.Now - from;

            from = DateTime.Now;
            double[][] newMatrix = MatrixUtil.ReadFromText(path);
            TimeSpan read = DateTime.Now - from;
            string msg = ""
                + "是否相等：" + MatrixUtil.IsEqual(newMatrix, matrix)
                + "阶次：" + order + "\r\n"
                + "创建：" + create.TotalSeconds + "\r\n"
                + "写入：" + write.TotalSeconds + "\r\n"
                + "读取：" + read.TotalSeconds + "\r\n";
            MessageBox.Show(msg);
        }
        #endregion
         

        private void button_randomGen_Click(object sender, EventArgs e)
        {
            DateTime from = DateTime.Now;
            int row = this.namedIntControl_row.GetValue();
            int col = this.namedIntControl_col.GetValue();

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

            var span = DateTime.Now - from;
            log.Info("耗时：" + span.TotalSeconds  + " s ," + span);

            DataBind(mat);
        }
        private void DataBind(IMatrix mat)
        { 
            if(mat is Matrix)
            {
                Matrix = mat as Matrix;
            }
            else
            {
                Matrix = new Matrix(mat);
            }
            this.richTextBoxControl1.Text = Matrix.ToReadableText();
            this.objectTableControl1.DataBind(Matrix.GetObectTable());
        }

        private void button_saveAsText_Click(object sender, EventArgs e)
        {
            if (Matrix == null) { Geo.Utils.FormUtil.ShowWarningMessageBox("请先读取或生成矩阵再试。"); return; }
            SaveFileDialog dlg = new SaveFileDialog();
            dlg.FileName = "matrix";
            dlg.Filter = Setting.TextMatrixFileFilter;
            if (dlg.ShowDialog() == DialogResult.OK)
            {
                var path = dlg.FileName;
                var writer = new TextMatrixWriter(path);
                writer.Write(Matrix);
                writer.Close();


                Geo.Utils.FormUtil.ShowOkAndOpenDirectory(Path.GetDirectoryName(path));
            }
        }

        public Matrix Matrix { get; set; }

        private void button_saveAsBinary_Click(object sender, EventArgs e)
        {
            if(Matrix == null) { Geo.Utils.FormUtil.ShowWarningMessageBox("请先读取或生成矩阵再试。"); return; }
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
            }
        }

        private void button_read_Click(object sender, EventArgs e)
        {
            var path = this.fileOpenControl1.FilePath;
            if (!File.Exists(path))
            {
                Geo.Utils.FormUtil.ShowWarningMessageBox("请选择文件后再试。");
                return;
            }
            if (path.ToLower().EndsWith(Setting.BinaryMatrixFileExtension.ToLower()))//二进制
            {
                BinaryMatrixReader matrixReader = new BinaryMatrixReader(path);
                var mat = matrixReader.Read();
                this.DataBind(mat);
            }
            else
            {
                TextMatrixReader matrixReader = new TextMatrixReader(path);
                var mat = matrixReader.Read();
                this.DataBind(mat);
            }
        }

        private void MatrixViewerForm_Load(object sender, EventArgs e)
        {
            namedStringControl_prefName.SetValue("a");
        }
    }
}
