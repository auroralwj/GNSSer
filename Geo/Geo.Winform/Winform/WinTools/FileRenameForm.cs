//2016, czs , create in hongqing, 重命名工具


using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;

namespace Gnsser.Winform
{
    public partial class FileRenameForm : Form
    {
        public FileRenameForm()
        {
            InitializeComponent();
        }

        private void button_ok_Click(object sender, EventArgs e)
        {
            var dir = directorySelectionControl1.Path;

            var files = Directory.GetFiles(dir, "*.*", SearchOption.AllDirectories);
            int i = 1;
            foreach (var item in files)
            {
                i++;
                var folder = Path.GetDirectoryName(item);
                var extension = Path.GetExtension(item);
                var path = Path.Combine(folder,i+""  + extension );
                File.Move(item, path);
            }
            MessageBox.Show("OK");
           
        }
         
    }
}
