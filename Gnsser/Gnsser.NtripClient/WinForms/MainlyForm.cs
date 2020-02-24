using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Gnsser.Ntrip.WinForms
{
    public partial class MainlyForm : Form
    {
        public MainlyForm()
        {
            InitializeComponent();
        }
         

        private void button2_Click(object sender, EventArgs e)
        {
            var form = new DataReceiverForm();
            form.WindowState = FormWindowState.Maximized;
            form.Show();

        }

        private void button3_Click(object sender, EventArgs e)
        {
            var form = new SourceTableForm();
            form.WindowState = FormWindowState.Maximized;
            form.Show();

        }

        private void button_readLocal_Click(object sender, EventArgs e)
        {
            var form = new LocalDataReceiverForm();
            form.WindowState = FormWindowState.Maximized;
            form.Show();

        }
    }
}
