using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.IO;
using System.Windows.Forms;

//using Geo.Utils;
using System.Xml;

namespace Geo.Winform
{
    public partial class AddSubProgramForm : Form
    {
        public AddSubProgramForm()
        {
            InitializeComponent();
        }
        private static string programDir = ShortcutCollectorForm.PROGRAM_DIR;
         /// <summary>
        /// 查找要添加的程序
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void browseFilebutton1_Click(object sender, EventArgs e)
        {
            if (this.openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                this.pathtextBox1.Text =   this.openFileDialog1.FileName;             
            }
        }


        private void addbutton1_Click(object sender, EventArgs e)
        {
            string path = this.pathtextBox1.Text.Trim();
            if (path.Length > 1)
            {
                string fileNameNoExt = Path.GetFileNameWithoutExtension(path);
                string imgPath = Path.GetFileNameWithoutExtension(path) + ".jpg";


                //get image icon and save to IMG_DIR
                Bitmap bitMap = Geo.Utils.ExtractIcon.GetIcon(openFileDialog1.FileName, false).ToBitmap();
                string iconPath = ShortcutCollectorForm.IMG_DIR + "\\" + imgPath;
                if (!File.Exists(iconPath)) bitMap.Save(iconPath);

                WriteToXml(fileNameNoExt, path, imgPath);

                this.DialogResult = DialogResult.OK;
                this.Close();
            }

        }

        /// <summary>
        /// 写入配置文件
        /// </summary>
        /// <param name="name">程序的名称</param>
        /// <param name="path">程序的路径</param>
        /// <param name="imgPath">程序图标名称</param>
        public static void WriteToXml(string name, string path, string imgPath)
        {
            XmlDocument xmlDoc = new XmlDocument();
            string xmlFile = programDir + "\\data.xml";
            xmlDoc.Load(xmlFile);

            XmlElement programEle =  xmlDoc.CreateElement("program");
            XmlElement nameEle = xmlDoc.CreateElement("name");
            nameEle.InnerText = name;

            XmlElement pathEle = xmlDoc.CreateElement("path");
            pathEle.InnerText = path;

            XmlElement imageEle = xmlDoc.CreateElement("image");
            imageEle.InnerText = imgPath;

            programEle.AppendChild(nameEle);
            programEle.AppendChild(pathEle);
            programEle.AppendChild(imageEle);         
           

            XmlNode topNode = xmlDoc.SelectSingleNode("./programs");
            topNode.AppendChild(programEle);

            xmlDoc.Save(xmlFile);
        }

    }
}