using System;
using Gnsser.Times;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.IO;
using System.Text;
using System.Windows.Forms;
using Gnsser.Data.Rinex;
using Gnsser;
using Geo.Coordinates;
using Geo.Referencing;
using AnyInfo;
using Geo.Algorithm;
using Geo.Times; 

namespace Gnsser.Winform
{
    /// <summary>
    /// 拟合星历服务。
    /// </summary>
    public partial class NaviFileConvertForm : Form
    {
        public NaviFileConvertForm()
        {
            InitializeComponent();
        }

        private void button_getPath_Click(object sender, EventArgs e) { if (this.openFileDialog_nav.ShowDialog() == DialogResult.OK)  this.textBox_Path.Text = openFileDialog_nav.FileName; }
        SingleParamNavFileEphService navFile;
        private void button_read_Click(object sender, EventArgs e)
        {
            string path = this.textBox_Path.Text;
            if (!File.Exists(path))
            {
                Geo.Utils.FormUtil.ShowWarningMessageBox("文件不存在 " + path);
                return;
            }
            this.textBox_source.Text = File.ReadAllText(path);

            ParamNavFileReader reader = new ParamNavFileReader(path);
            navFile = new SingleParamNavFileEphService( reader.ReadGnssNavFlie());

            this.bindingSource1.DataSource = navFile.Gets();
            bindingSource_prn.DataSource = navFile.Prns;

            //设置时间间隔
            this.dateTimePicker_from.Value = navFile.TimePeriod.Start.DateTime;
            this.dateTimePicker_to.Value = navFile.TimePeriod.End.DateTime;

            //MessageBox.Show("已经成功导入！");
        }


        DateTime timeFrom;
        DateTime timeTo; 
         SatelliteNumber prn;
        private void button_show_Click(object sender, EventArgs e)
        {
            timeFrom = this.dateTimePicker_from.Value;
            timeTo = this.dateTimePicker_to.Value;

            prn = (SatelliteNumber) this.comboBox_prn.SelectedItem ;
            var range = navFile.NavFile.GetEphemerisParams(prn,
                 Time.Parse(timeFrom),
                 Time.Parse(timeTo));

            navFile.NavFile = new ParamNavFile() { Header = navFile.NavFile.Header };          
            navFile.NavFile.Add(range);
        }


        private void button_convert_Click(object sender, EventArgs e)
        {
            this.textBox_converted.Text = ParamNavFileWriter.BuidRinexV3String(navFile.NavFile);
        }

        private void NaviFileConvertForm_Load(object sender, EventArgs e)
        {
            this.textBox_Path.Text = Setting.GnsserConfig.SampleNFile;
        }

        private void button_saveTo_Click(object sender, EventArgs e)
        {
            Geo.Utils.FormUtil.ShowFormSaveTextFileAndIfOpenFolder(this.textBox_converted.Text);
        }


    }
}
