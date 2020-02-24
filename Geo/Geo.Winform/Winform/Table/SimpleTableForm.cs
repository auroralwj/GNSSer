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
    public partial class SimpleTableForm : Form  
    {
        public SimpleTableForm( )
        {
            InitializeComponent(); 
        }
         

        public void Init<T>(bool useDisplayName = false)
           where T : class
        {
            this.TalbeControl.Init<T>(useDisplayName);
        }

        public void DataBind(object datasource)
        { 
            this.TalbeControl.DataBind(datasource);
        }

        public SimpleTableControl TalbeControl { get { return this.simpleTableControl1; } }
    }
}
