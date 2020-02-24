//2017.07.24, czs, create in hongqing, 运行状态信息显示器

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
    public partial class RunningStateForm : LogListenerForm
    {
        public RunningStateForm()
        {
            InitializeComponent();
        }

        protected override void ShowInfo(string info)
        {
            Geo.Utils.FormUtil.InsertLineWithTimeToTextBox( this.richTextBoxControl_output, info);
        } 


        private void RunningStateForm_FormClosing(object sender, FormClosingEventArgs e)
        {

        }
    }
}
