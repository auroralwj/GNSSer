using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Gnsser.Interoperation.Teqc;

namespace Gnsser.Winform
{
    public partial class QualityCheckForm : Form
    {
        public QualityCheckForm()
        {
            InitializeComponent();
        }

        private void button_selectFiles_Click(object sender, EventArgs e)
        {
            if (openFileDialog1.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                this.textBox_infiles.Lines = openFileDialog1.FileNames;
            }
        }

        private void button_run_Click(object sender, EventArgs e)
        {
            try
            {
                //check
                string[] files = this.textBox_infiles.Lines;
                if (files.Length == 0) throw new ArgumentNullException("输入文件不可为空。");

                TeqcFunctionCaller call = new TeqcFunctionCaller(Setting.GnsserConfig.TeqcPath, TeqcFunction.QualityChecking);
                this.textBox_result.Text = call.Run(files)[0];
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void QualityCheckForm_Load(object sender, EventArgs e)
        {
            this.textBox_infiles.Text = Setting.GnsserConfig.SampleOFile;
        }
    }
}
