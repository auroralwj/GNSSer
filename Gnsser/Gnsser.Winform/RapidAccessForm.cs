using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Gnsser.Winform
{
    public partial class RapidAccessForm : Form
    {
        public RapidAccessForm()
        {
            InitializeComponent();
        }

        private void button_pointPosVizard_Click(object sender, EventArgs e)
        {
            new PointPositionVizardForm().ShowDialog();
        }

        private void button_baseLineSolve_Click(object sender, EventArgs e)
        {
            var form = new BaselineSolverVizardForm();
         //   form.Parent = this.Parent;
            form.Show();
            //new BaselineSolverVizardForm().ShowDialog();
        }
    }
}
