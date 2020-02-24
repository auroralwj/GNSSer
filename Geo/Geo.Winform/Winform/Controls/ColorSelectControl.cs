//2018.08.22, czs , create in hmx, 颜色

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Geo.Winform
{
    public partial class ColorSelectControl : UserControl
    {
        public ColorSelectControl()
        {
            InitializeComponent();
        }

        public Color GetValue()
        {
            var color = this.label_Color.BackColor; 
            byte alpha = byte.Parse(this.textBox_alfa.Text);
            return color = Color.FromArgb(alpha, color); 
        }

        public void SetValue(Color color)
        {
             this.label_Color.BackColor = color;
            this.textBox_alfa.Text = color.A.ToString();
        }

        private void label_Color_Click(object sender, EventArgs e)
        {
            if (colorDialog1.ShowDialog() == DialogResult.OK)
                this.label_Color.BackColor = colorDialog1.Color;
        }
    }
}
