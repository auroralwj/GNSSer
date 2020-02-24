using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
//using Geo.Utils;

namespace Geo.Winform
{
    public partial class ButtonControl : UserControl
    {
        public ButtonControl()
        {
            InitializeComponent();
        }
        string path = "";
        public ButtonControl(string name, string path, Image image)
        {
            InitializeComponent();
            this.path = path;
            this.label1.Text = name;
            if(image != null)
            this.button1.Image = image;
        }
        private void control_Click(object sender, EventArgs e)
        {
            Geo.Utils.FileUtil.OpenFile(path);
        }

        private void ButtonControl_MouseLeave(object sender, EventArgs e)
        {
            this.label1.ForeColor = System.Drawing.SystemColors.WindowText;
            this.BackColor = System.Drawing.SystemColors.GradientInactiveCaption;
            this.Refresh();
        }

        private void ButtonControl_MouseEnter(object sender, EventArgs e)
        {
            this.label1.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.BackColor = System.Drawing.SystemColors.GradientActiveCaption;
            this.Refresh();
        }
    }
}
