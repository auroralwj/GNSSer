using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

using System.IO;
using System.Xml;
 
using System.Diagnostics;//加入，使用进程类，创建独立进程

namespace Geo.Winform
{

    public partial class ShortcutCollectorForm : Form
    {
        public static string PROGRAM_DIR = Application.StartupPath + @"\Data\Collector";
        public static string IMG_DIR = PROGRAM_DIR + "\\Icons";

        int subProgramCount = 0;
        List<ButtonControl> btnControlList = new List<ButtonControl>();


        public ShortcutCollectorForm()
        {
            InitializeComponent();

            refresh();
        }

        public void refresh()
        {
            btnControlList.Clear();
            //读取并添加菜单
            initButtons();
            this.Refresh();
        }

        //初始化按钮
        private void initButtons()
        {
            string xmlPath = PROGRAM_DIR + "\\Data.xml";

            //如果不存在则创建一个，体现出程序的健壮性。
            if(!Directory.Exists(IMG_DIR)) Directory.CreateDirectory(IMG_DIR);

            if (!File.Exists(xmlPath))
            {
                CreateEmptyDataXmlFile(xmlPath);
            }

            XmlDocument doc = new XmlDocument();
            doc.Load(xmlPath);
            XmlNodeList nodeList = doc.SelectNodes("./programs/program");
            subProgramCount = nodeList.Count;

            foreach (XmlNode n in nodeList)
            {
                XmlNode node = n.SelectSingleNode("./name");
                string programName = node.InnerText.Trim();
                string programPath = n.SelectSingleNode("./path").InnerText.Trim();

                node = n.SelectSingleNode("./image");
                Bitmap pic = null;
                if (node != null)
                {
                    string programIconPath = node.InnerText.Trim();
                    string path = IMG_DIR + "\\" + programIconPath;
                    FileInfo fileInfo = new FileInfo(path);

                    if (fileInfo.Exists)
                    {
                          pic = new Bitmap(path);                        
                    }
                }
                addOneButtonControl(programName, programPath, pic);
                addOneToolStripMenuItem(programName, programPath);
            }          

            //布局菜单
           // layoutButton();
            layoutButtonControls();
        }

        private static void CreateEmptyDataXmlFile(string path)
        {
            string content = "<?xml version=\"1.0\" encoding=\"utf-8\" ?> " + "\r\n";
            content += "<programs>" + "\r\n";
            content += "</programs>" + "\r\n";
            using (StreamWriter sw = new StreamWriter(path, false, Encoding.UTF8))
            {
                sw.Write(content);
            }
        }

        private void addOneToolStripMenuItem(string programName, string programPath)
        {
            ToolStripMenuItem menuItem = new ToolStripMenuItem();
            menuItem.Name = programName;
            menuItem.Size = new System.Drawing.Size(152, 22);
            menuItem.Text = programName;
            menuItem.Tag = programPath;
            menuItem.Click += new System.EventHandler(openFileToolStripItem_Click);

            //保存，显示
            this.程序PToolStripMenuItem.DropDownItems.Add( menuItem );
        }

        //根据信息添加一个菜单到form上。
        private void addOneButtonControl(string programName,string programPath,Image pic)
        {
            ButtonControl btnCon = new ButtonControl(programName, programPath, pic);

            this.btnControlList.Add(btnCon);
            this.Controls.Add(btnCon);
        }

        //布局菜单
        private void layoutButtonControls()
        {
            if (btnControlList.Count == 0) return;
            int btnColumnCount = this.Width / (btnControlList[0].Width + 120);
            int columnIndex = 0;
            int rowIndex = 0;
            foreach (ButtonControl btn in btnControlList)
            {
                //button postion            
                if (btnColumnCount < columnIndex + 1)
                {
                    columnIndex = 0;
                    rowIndex++;
                }
                int x = 20 + (btn.Width +120) * columnIndex;
                int y = 90 + (btn.Height +5)* rowIndex;
                btn.Location = new System.Drawing.Point(x, y);
                columnIndex++;
            }
        }

        private void openFileToolStripItem_Click(object sender, EventArgs e)
        {
            ToolStripItem btn = (ToolStripItem)sender;
            string programPath = (string)btn.Tag;

            Geo.Utils.FileUtil.OpenFile(programPath);
        }

        //布局菜单
        private void ShortcutCollector_Resize(object sender, EventArgs e)
        {
            layoutButtonControls();
        }


        private void 增加子程序ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            AddSubProgramForm form = new AddSubProgramForm();
            if (form.ShowDialog() == DialogResult.OK)
            {
                refresh();
            }
            
        }


        private void 数据文件ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string path = PROGRAM_DIR + "\\data.xml";
            Process.Start("notepad.exe", path);
        }

        private void 打开主程序位置ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Geo.Utils.FileUtil.OpenFile(Application.StartupPath);
        }

 
        private void 刷新toolStripButton1_Click(object sender, EventArgs e)
        {
            refresh();
        }
    }
}