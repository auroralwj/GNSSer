//2017.10.31, czs, edit in hongqing, 增加批量向量查看。

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Geo;
using Geo.Coordinates;

namespace Gnsser.Winform
{
    public partial class VectorInfoForm : Form
    {
        public VectorInfoForm()
        {
            InitializeComponent();
        }

        private void button_run_Click(object sender, EventArgs e)
        {
            try
            {
                var lines = this.textBox_aXyz.Lines; 

                NamedXyz baseXyz = null;
                List<NamedXyz> rovXyzs = new List<NamedXyz>();
                int i = 1;
                foreach (var line in lines)
                {
                    if (String.IsNullOrWhiteSpace(line)) { continue; }

                    if (baseXyz == null)
                    {
                        baseXyz = NamedXyz.Parse(line);
                        if (String.IsNullOrWhiteSpace(baseXyz.Name)) { baseXyz .Name= i + ""; }
                        continue;
                    }

                    var rovXyz = NamedXyz.Parse(line);
                    if (String.IsNullOrWhiteSpace(rovXyz.Name)) { rovXyz.Name = i + ""; }
                    rovXyzs.Add(rovXyz);
                    i++;
                }

                StringBuilder sb = new StringBuilder();
                i = 1;
                foreach (var rovXyz in rovXyzs)
                {
                    XYZ vector = rovXyz.Value - baseXyz.Value;
                    var baseGeoCoord = Geo.Coordinates.CoordTransformer.XyzToGeoCoord(baseXyz.Value);

                    var neu = Geo.Coordinates.CoordTransformer.XyzToNeu(vector, baseGeoCoord);
                    var polar = Geo.Coordinates.CoordTransformer.NeuToPolar(neu);

                    sb.AppendLine(" " + i + "  --------------------  ");
                    var vecName = baseXyz.Name + "->" + rovXyz.Name;
                    sb.AppendLine(vecName);
                    sb.AppendLine("Vector:\t" + vector.ToString());
                    sb.AppendLine("NEU:\t" + neu.ToString());

                    sb.AppendLine("Height Differ:\t" + polar.Height.ToString());
                    sb.AppendLine("Plain Distance:\t" + polar.PlainRange.ToString());
                    sb.AppendLine("Length:\t" + vector.Length);

                    i++;
                }



                this.textBox_result.Text = sb.ToString();
            }
            catch (Exception ex)
            {
                Geo.Utils.FormUtil.ShowWarningMessageBox("出错了，多半是数据格式问题，请仔细检查！" + ex.Message);
            }

        }
    }
}
