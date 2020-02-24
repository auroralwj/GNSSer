//2018.04.26, lly, create in zz, 对流层比较

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using Geo.Times;
using Geo.Utils;

namespace Gnsser.Winform.Tools
{
    public partial class TropProductsCompForm : Form
    {
        public TropProductsCompForm()
        {
            InitializeComponent();
        }

        //Dictionary<Time, double> ZTDs_IGS = new Dictionary<Time, double>();
        //Dictionary<Time, double> ZTDs_GNSSer = new Dictionary<Time, double>();
        //Dictionary<Time, double> ZTDs_difference = new Dictionary<Time, double>();

        List<ZTDs> ZTDs_IGS = new List<ZTDs>();
        List<ZTDs> ZTDs_GNSSer = new List<ZTDs>();
        List<ZTDs> ZTDs_difference = new List<ZTDs>();

        public List<ZTDs> Jiexi(StreamReader reader)
        {

            List<ZTDs> ZTDs_IGS1 = new List<ZTDs>();
            string line = reader.ReadLine();
            while ((line = reader.ReadLine()) != null)
            {
                if (line != "-TROP/SOLUTION" && line != "%=ENDTRO")
                {
                    ZTDs tmpZTD = new ZTDs();
                    tmpZTD.time = Time.ParseYds(line.Substring(6, 12));
                    tmpZTD.ZTD = double.Parse(line.Substring(19, 6)) / 1000;
                    ZTDs_IGS1.Add(tmpZTD);
                }                
            }
            return ZTDs_IGS1;             
        }
        private void readandcompare_Click(object sender, EventArgs e)
        {
            string pathA = fileOpenControl1.FilePath;
            string pathB = fileOpenControl2.FilePath;

            //读取IGS的zpd产品
            using (StreamReader reader = new StreamReader(pathA))
            {
                string line = null;
                line = reader.ReadLine();

                while((line = reader.ReadLine())!= null)
                {
                    if (line == "+TROP/SOLUTION")
                    {
                        ZTDs_IGS = Jiexi(reader);

                    }
                    else
                    {
                        //continue;
                    }
                }                
            }
            //读取GNSSer的zpd产品
            using (StreamReader reader = new StreamReader(pathB))
            {
                string line = null;
                line = reader.ReadLine();

                while ((line = reader.ReadLine()) != null)
                {
                    string[] values = SplitByBlank(line);
                    ZTDs tmpZTD = new ZTDs();
                    tmpZTD.time = Time.Parse(values[0]);
                    tmpZTD.ZTD = double.Parse(values[1]);
                    ZTDs_GNSSer.Add(tmpZTD);

                }
            }

            //比较
            for (int i = 0; i < ZTDs_GNSSer.Count; i++)
            {
                Time currentTime_IGS = ZTDs_IGS[i].time;
                double currentZTD_IGS = ZTDs_IGS[i].ZTD;

                Time currentTime_GNSSer = ZTDs_IGS[i].time;
                double currentZTD_GNSSer = ZTDs_GNSSer[i].ZTD;

                ZTDs tmpZTD_difference = new ZTDs();

                double difference = 0;
                if(currentTime_GNSSer.Equals(currentTime_IGS))
                {
                    tmpZTD_difference.time = currentTime_IGS;
                    tmpZTD_difference.ZTD = currentZTD_IGS - currentZTD_GNSSer;
                }


                ZTDs_difference.Add(tmpZTD_difference);
            }
            this.bindingSource1.DataSource = ZTDs_difference;

        }

        public class ZTDs
        {
            public Time time { get; set; }
            public double ZTD { get; set; }
        }
        public static string[] SplitByBlank(string line)
        {
            return line.Split(new char[] {  '\t' }, StringSplitOptions.RemoveEmptyEntries);
        }

    }
}
