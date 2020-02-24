//2015.12.06, czs, create in hongqing,提供工具向导起始页

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Geo.Winform.Demo;

namespace Geo.Winform.Wizards
{
    public partial class BaseStartWizardForm : Form
    {
        public BaseStartWizardForm()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            WizardPageCollection WizardPageCollection = new Geo.Winform.Wizards.WizardPageCollection();
           // WizardPageCollection.Add(1, new SelectFilePageControl());
            WizardPageCollection.Add(1, new Page1());
            WizardPageCollection.Add(2, new Page2());
            WizardPageCollection.Add(3, new Page3());

            var host = new WizardForm(WizardPageCollection);


            host.Text = "My Wizards";
            host.WizardCompleted += new WizardCompletedEventHandler(host_WizardCompleted);

            host.LoadWizard();
            host.ShowDialog();
        }

        void host_WizardCompleted()
        {
            MessageBox.Show("Done!"); //obviously you'd do something else in a real app...
            textBox1.Text = "开始执行啦!!!!To be done!";
        }
    }
}
