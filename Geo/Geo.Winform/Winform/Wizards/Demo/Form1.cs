using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Geo.Winform.Wizards;

namespace Geo.Winform.Demo
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            WizardHostForm host = new WizardHostForm();
            host.Text = "My Wizards";
            host.WizardCompleted += new WizardCompletedEventHandler(host_WizardCompleted);
            host.WizardPages.Add(1, new Page1());
            host.WizardPages.Add(2, new Page2());
            host.WizardPages.Add(3, new Page3());
            host.LoadWizard();
            host.ShowDialog();
        }

        void host_WizardCompleted()
        {
            MessageBox.Show("Done!"); //obviously you'd do something else in a real app...
            textBox1.Text = "You finished. Whoopdeedoo!";
        }
    }
}
