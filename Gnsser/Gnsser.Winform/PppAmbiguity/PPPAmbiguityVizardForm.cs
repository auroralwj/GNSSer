using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.IO;
using System.Windows.Forms;
using Gnsser.Winform;
using Geo.Winform;
using Geo.IO;
using Geo.Data;
using Geo;

namespace Gnsser.Winform
{
    public partial class PPPAmbiguityVizardForm : Form
    {
        public PPPAmbiguityVizardForm()
        {
            InitializeComponent();
        }


        private void button_multiSite_Click(object sender, EventArgs e)
        {
            var form = new NarrowLineSolverForm();
            form.Show();
            form.WindowState = FormWindowState.Maximized;
        }

        private void button_viewWcb_Click(object sender, EventArgs e)
        {
            var form = new WideLaneGpsBiasViewerForm();
            form.Show();
            form.WindowState = FormWindowState.Maximized;

        }

        private void button1_Click(object sender, EventArgs e)
        {
            var form = new OFileFormaterForm();
            form.Show();
            form.WindowState = FormWindowState.Maximized;

        }

        private void button_coordCompare_Click(object sender, EventArgs e)
        {
            var form = new XyzCompareForm();
            form.Show();
            form.WindowState = FormWindowState.Maximized;
        }

        private void button_netSolveWm_Click(object sender, EventArgs e)
        {
            var form = new MwWideLaneSolverForm();
            form.Show();
            form.WindowState = FormWindowState.Maximized;

        }

        private void buttonNewMethod_Click(object sender, EventArgs e)
        {
            var form = new MultiPeriodNarrowLaneOfBsdSolverForm();
            form.Show();
            form.WindowState = FormWindowState.Maximized;
        }

        private void buttonWmNew_Click(object sender, EventArgs e)
        {
            var form = new WideLaneOfBsdSolverForm();
            form.Show();
            form.WindowState = FormWindowState.Maximized;
        }

        private void button_elevate_Click(object sender, EventArgs e)
        {
            var form = new SatEevationSolverForm();
            form.Show();
            form.WindowState = FormWindowState.Maximized;
        }

        private void button_ppp_Click(object sender, EventArgs e)
        {
            var form = new SingleGnssFileSolveForm();
            form.Show();
            form.WindowState = FormWindowState.Maximized;

        }

        private void button_viewAndDrawTable_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Geo表格文件(*.txt;*.xls)|*.txt;*.xls|所有文件(*.*)|*.*";
            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                var path = openFileDialog.FileName;

                ObjectTableReader reader = new ObjectTableReader(path, Encoding.Default);

                var table = reader.Read();//.GetDataTable(); 
                var fileName = Path.GetFileName(path);
                new TableObjectViewForm(table) { Text = fileName }.Show();
            }
        }

        private void button_bsdSolver_Click(object sender, EventArgs e)
        {
            var form = new MultiPeriodBsdProductSolverForm();
            form.Show();
            form.WindowState = FormWindowState.Maximized; 
        }

        private void button_mwExtructor_Click(object sender, EventArgs e)
        { 
            var form = new MwFractionTableBuilderForm();
            form.Show();
            form.WindowState = FormWindowState.Maximized; 
        }

        private void button_totalWLOfBSD_Click(object sender, EventArgs e)
        {
            var form = new MultiPeriodlWideLaneOfBsdSolverForm();
            form.Show();
            form.WindowState = FormWindowState.Maximized; 
        }

        private void buttonSatelliteSelectorForm_Click(object sender, EventArgs e)
        {
            var form = new SatelliteSelectorForm();
            form.Show();
            form.WindowState = FormWindowState.Maximized; 

        }
    }
}