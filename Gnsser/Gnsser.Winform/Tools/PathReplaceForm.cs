using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;

namespace Gnsser.Winform
{
    public partial class PathReplaceForm : Form
    {
        public PathReplaceForm()
        {
            InitializeComponent();
            enumRadioControl_type.Init<System.IO.SearchOption>();

            namedStringControl_extension.SetValue("*.*");
        }

        private void button_ok_Click(object sender, EventArgs e)
        {
            string key = this.namedStringControl_key.GetValue();
            string extension = namedStringControl_extension.GetValue();
            SearchOption option = enumRadioControl_type.GetCurrent<SearchOption>();
            string[] pathes = this.fileOpenControl_pathes.GetFilePathes(extension, option);
            if(pathes.Length == 0)
            {
                MessageBox.Show("路径为空!");
                return;
            }
            foreach (var path in pathes)
            {
                string source = path;
                string dest = path.Replace(key, "");
                string folder = Path.GetDirectoryName(source);
                string fileName = Path.GetFileName(dest);
                if (fileName.StartsWith(".")) {
                    fileName = fileName.TrimStart('.');
                    dest = Path.Combine(folder, fileName);
                }
                if(String.Compare(source, dest, true) == 0) { continue; }
                File.Move(source, dest);
            }
            string dir = Path.GetDirectoryName(pathes[0]);

            Geo.Utils.FormUtil.ShowOkAndOpenDirectory(dir);

            //MessageBox.Show("Done!");


        }
    }
}
