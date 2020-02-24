using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms; 
using Geo.Algorithm.Adjust;

namespace Geo.WinTools
{
    /// <summary>
    /// 粗差探测窗口
    /// </summary>
    public partial class GrossDetectingForm : Form
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public GrossDetectingForm()
        {
            InitializeComponent();
        }

        private void button_run_Click(object sender, EventArgs e)
        {
            try
            {
                double times = double.Parse(textBox_rms.Text);
                bool isLoop = checkBox_loopDetect.Checked;
                string[] strs = this.textBox_infiles.Lines;
                //string[] strs = str.Split(new char[] { ' ', ',', ';', '\n', '\t', '\r' }, StringSplitOptions.RemoveEmptyEntries);
                double[] errors = Geo.Utils.DoubleUtil.ParseLines(strs);// new double[strs.Length];
                //int i = 0;
                //foreach (var key in strs)
                //{
                //    errors[i] = Int32.Parse(key);
                //    i++;
                //}

                Dictionary<int, double> grosses = DetectAndOutput(times, errors);
                while (isLoop && grosses.Count > 0)
                {
                    List<double> list = GrossErrorFinder.GetSmartData(errors, grosses);

                    errors = list.ToArray(); 

                    grosses = DetectAndOutput(times, errors);
                }

                List<double> list3 =GrossErrorFinder. GetSmartData(errors, grosses);

                StringBuilder sb = new StringBuilder();
                foreach (var item in list3)
                {
                    sb.AppendLine(item+"");
                }
                this.textBox_result.Text = sb.ToString();

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }


        private Dictionary<int, double> DetectAndOutput(double times, double[] errors)
        {
            Geo.Algorithm.Adjust.GrossErrorFinder finder = new Geo.Algorithm.Adjust.GrossErrorFinder(times);         

            Dictionary<int, double> grosses = finder.Find(errors);

            StringBuilder sb = new StringBuilder();
            sb.AppendLine("RMS:" + finder.Rms.ToString("0.000000"));
            sb.AppendLine("阈值（限差）:" + finder.LimitError.ToString("0.000000"));
            foreach (var item in grosses)
            {
                sb.AppendLine(item.Key + "\t" + item.Value);
            }
            this.textBox_gross.Text += sb.ToString();
            return grosses;
        }

        private void QualityCheckForm_Load(object sender, EventArgs e)
        {
           // this.textBox_infiles.Text = Setting.SampleOFile;
        }

        private void button_fileProcess_Click(object sender, EventArgs e)
        {
            Geo.WinTools.FileGrossDetectingForm form = new Geo.WinTools.FileGrossDetectingForm();
            form.ShowDialog();
        }
    }
}
