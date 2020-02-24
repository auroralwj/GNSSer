using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Gnsser.Winform.Controls
{
    public partial class TimePeriodUserControl : UserControl
    {
        public TimePeriodUserControl()
        {
            InitializeComponent();
        }
        /// <summary>
        /// 获取
        /// </summary>
        public Geo.Times.TimePeriod TimePeriod { get { return  this.timePeriodControl1.TimePeriod; } }

        private void button_loadFromObsFile_Click(object sender, EventArgs e)
        {
            OpenFileDialog dlg = new OpenFileDialog();
            dlg.Filter = Setting.RinexOFileFilter;
            if (dlg.ShowDialog() == DialogResult.OK)
            {
                var path = dlg.FileName;

                var header = Gnsser.Data.Rinex.RinexObsFileReader.ReadHeader(path);
                this.timePeriodControl1.SetTimePerid(header.TimePeriod);
            }
        }
    }
}
