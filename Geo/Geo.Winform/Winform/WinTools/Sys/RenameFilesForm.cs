using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.IO;
using System.Windows.Forms;

namespace Geo.WinTools.Sys
{
    /// <summary>
    /// 重命名文件
    /// </summary>
    public partial class RenameFilesForm : Form
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public RenameFilesForm()
        {
            InitializeComponent();
        }

        private void button_setdirpath_Click(object sender, EventArgs e)
        {
            if (this.folderBrowserDialog1.ShowDialog() == DialogResult.OK)
            {
                this.textBox_dirPath.Text = this.folderBrowserDialog1.SelectedPath;
            }
        }

        private void button_RenameSuffix_Click(object sender, EventArgs e)
        {            

            string dirPath = this.textBox_dirPath.Text;
            string sourceSuffix = this.comboBox_sourseSuffix.Text.Trim();
            string targetSuffix = this.comboBox_targetSuffix.Text.Trim();

            if (sourceSuffix == targetSuffix)
            {
                MessageBox.Show("源后缀名和目标后缀名相同!");
                return;
            }
            List<string> dirList = GetAllSubDirs(dirPath);
            dirList.Add(dirPath);

            this.progressBar1.Maximum = dirList.Count;
            this.progressBar1.Minimum = 0;
            this.progressBar1.Step = 1;
            this.progressBar1.Value = this.progressBar1.Minimum;

            long count = 0L;
            foreach (string dir in dirList)
            {   
                this.label_process_info.Text = "正在将 " + dir + "文件夹下的 " + sourceSuffix + " 文件后缀改为 " + targetSuffix;
                this.label_process_info.Refresh();

                count +=  RenameFilesInOneDir(dir, sourceSuffix, targetSuffix);

              this.progressBar1.PerformStep();
                this.progressBar1.Refresh();
            }
            this.label_process_info.Text = "Done!";
            MessageBox.Show("操做完毕！共 " + dirList.Count + "个文件夹，重命名" + count + "个文件。");
        }



        private void button_renameName_Click(object sender, EventArgs e)
        {
            string dirPath = this.textBox_dirPath.Text;
            string prefixName = this.textBox_filePrefixName.Text.Trim();
            string spaceMark = this.comboBox_spaceMark.Text.Trim();
            prefixName = prefixName + spaceMark;

            List<string> dirList = GetAllSubDirs(dirPath);
            dirList.Add(dirPath);

            this.progressBar1.Maximum = dirList.Count;
            this.progressBar1.Minimum = 0;
            this.progressBar1.Step = 1;
            this.progressBar1.Value = this.progressBar1.Minimum;

            long count = 0L;
            foreach (string dir in dirList)
            {
                string[] filenNames = Directory.GetFiles(dir);
                foreach (string fileName in filenNames)
                {
                    File.Move(fileName, dirPath + "\\" + prefixName + (++count) + "." + Path.GetExtension(fileName));
                    count++;
                }
                this.label_process_info.Text = "正在将修改 " + dir + "文件夹下的文件名。";
                this.label_process_info.Refresh();
                this.progressBar1.PerformStep();
                this.progressBar1.Refresh();
            }
            this.label_process_info.Text = "Done!";
            MessageBox.Show("操做完毕！共 " + dirList.Count + "个文件夹，重命名" + count + "个文件。");
        }

        private void filePrefixName_TextChanged(object sender, EventArgs e)
        {
            this.label_example.Text = this.textBox_filePrefixName.Text.Trim() + 
                this.comboBox_spaceMark.Text.Trim() + "1.后缀";
        }
　　    /// <summary>
        /// 获取所有子文件。
        /// </summary>
        /// <param name="dirPath"></param>
        /// <returns></returns>
        private List<string> GetAllSubDirs(string dirPath)
        {
            List<string> dirList = new List<string>(); 
            string[] dirs = Directory.GetDirectories(dirPath);
            dirList.AddRange(dirs);//current absDirectory pathes.
            foreach (string dir in dirs)
            {
                dirList.AddRange(GetAllSubDirs(dir));
            }
            return dirList;
        }

        private  int RenameFilesInOneDir(string dirPath, string sourceSuffix, string targetSuffix)
        {
            string[] filenNames = Directory.GetFiles(dirPath);
            int count = 0;
            foreach (string fileName in filenNames)
            {
                string sourseSuffix = Path.GetExtension(fileName).ToLower();
                if (sourseSuffix.Equals(sourceSuffix.ToLower()))
                {
                    string destPath = dirPath + "\\" + Path.GetFileNameWithoutExtension(fileName) + targetSuffix;
                    if( this.checkBox_replaceExist.Checked && File.Exists(destPath) ) 
                        File.Delete(destPath);
                    File.Move(fileName, destPath);
                    count++;
                }
            }
            return count;
        }
    }
}