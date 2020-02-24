using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Geo.Winform
{
    public partial class TitledBox : UserControl
    {
        public TitledBox()
        {
            InitializeComponent();            
        } 
        public TitledBox(string title)
        {
            InitializeComponent();
            this.label_title.Text = title;
        } 
        /// <summary>
        /// 盒子标题
        /// </summary>
        public string Tilte
        {
            get { return this.label_title.Text; }
            set  {  this.label_title.Text = value;  }
        }
    }
}
