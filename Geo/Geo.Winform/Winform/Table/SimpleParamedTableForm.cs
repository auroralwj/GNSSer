//2015.06.03, czs , create in namu,表数据查看器

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Geo.Winform
{
    public partial class SimpleParamedTableForm<T> : Form 
        where T:class
    {
        public SimpleParamedTableForm( )
        {
            InitializeComponent();
            this.TalbeControl.Init<T>(false); 
        }

        public SimpleParamedTableForm(bool useDisplayName, object datasource)
        {
            InitializeComponent();
            this.TalbeControl.Init<T>(useDisplayName);
            this.TalbeControl.DataBind(datasource);
        }

        public SimpleTableControl TalbeControl { get { return this.simpleTableControl1; } }
    }
}
