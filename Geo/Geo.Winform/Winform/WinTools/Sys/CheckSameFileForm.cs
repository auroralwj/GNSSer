using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Text;
using System.Windows.Forms;

namespace Geo.Winform
{
    /// <summary>
    /// 检查相同文件
    /// </summary>
    public partial class CheckSameFileForm : Form
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public CheckSameFileForm()
        {
            InitializeComponent();
        }

        private void button_setPathA_Click(object sender, EventArgs e)
        {
            if (folderBrowserDialog1.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                this.textBox_pathA.Text = folderBrowserDialog1.SelectedPath;
        }

        private void button_setPathB_Click(object sender, EventArgs e)
        {
            if (folderBrowserDialog1.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                this.textBox_pathB.Text = folderBrowserDialog1.SelectedPath;
        }

        private void button_compareDir_Click(object sender, EventArgs e)
        {
            string dirA = this.textBox_pathA.Text;
            string dirB = this.textBox_pathB.Text;
            this.textBox_result.Text = "开始执行...";
            DirComparison dirComparison = new DirComparison(dirA, dirB);


            this.textBox_result.Text = "复制文件...";
            if (checkBox_copyDir1ToDir2.Checked) dirComparison.CopyDifferInAToB();

            this.textBox_result.Text = dirComparison.ToString();
        }

        private void button_compareFile_Click(object sender, EventArgs e)
        {
            string dirA = this.textBox_pathA.Text;
            string dirB = this.textBox_pathB.Text;
             
            FileComparison fileComparison = new FileComparison(dirA, dirB);
            this.textBox_result.Text = "复制文件...";
            if (checkBox_copyFile1ToFile2.Checked) fileComparison.CopyDifferInAToB();
             
            this.textBox_result.Text = fileComparison.ToString();
        } 
    }

    /// <summary>
    /// 目录比较
    /// </summary>
    public class DirComparison
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="dirA"></param>
        /// <param name="dirB"></param>
        public DirComparison(string dirA, string dirB)
        {
            this.DirA = dirA;
            this.DirB = dirB;
            ListA = DirComparison.Parse(dirA);
            ListB = DirComparison.Parse(dirB);

            DiffersInA = new List<DirItem>();
            foreach (DirItem item in ListA)
            {
                if (!ListB.Contains(item)) DiffersInA.Add(item);
            }
            DiffersInB = new List<DirItem>();
            foreach (DirItem item in ListB)
            {
                if (!ListA.Contains(item)) DiffersInB.Add(item);
            }

        }
        /// <summary>
        /// 目录A
        /// </summary>
        public string DirA { get; set; }
        /// <summary>
        /// 目录B
        /// </summary>
        public string DirB { get; set; } 
        /// <summary>
        /// 列表A
        /// </summary>
        public List<DirItem> ListA { get; set; }
        /// <summary>
        /// 列表B
        /// </summary>
        public List<DirItem> ListB { get; set; }
        /// <summary>
        /// A中不同
        /// </summary>
        public List<DirItem> DiffersInA { get; set; }
        /// <summary>
        /// B中不同
        /// </summary>
        public List<DirItem> DiffersInB { get; set; }

        /// <summary>
        /// 将A中不同复制到B
        /// </summary>
        public void CopyDifferInAToB()
        {
            foreach (var item in DiffersInA)
            {
                string to =DirB + item.HalfaPath;
                CopyDirectory(item.FullPath, to);
            }
        }
        /// <summary>
        /// 字符串
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            //显示结果
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("目录 " + this.DirA + " 下有 " + this.ListA.Count + " 个目录，有 " + this.DiffersInA.Count + " 个不同的目录");
           
            foreach (var item in this.DiffersInA)
            {
                sb.AppendLine(item.FullPath);
            } 
            
            sb.AppendLine("目录 " + this.DirB + " 下有 " + this.ListB.Count + " 个目录，有 " + this.DiffersInB.Count + " 个不同的目录");
             foreach (var item in this.DiffersInB)
            {
                sb.AppendLine(item.FullPath);
            }
            return sb.ToString();
        }
        /// <summary>
        /// 过去列表
        /// </summary>
        /// <param name="dir"></param>
        /// <returns></returns>
        public static List<DirItem> Parse(string dir)
        {
            string[] files = System.IO.Directory.GetDirectories(dir, "*", SearchOption.AllDirectories);
            List<DirItem> fileList = new List<DirItem>();
            foreach (string item in files)
            {
                fileList.Add(new DirItem(dir, item));
            }
            return fileList;
        }
        /// <summary>
        /// 复制目录
        /// </summary>
        /// <param name="from"></param>
        /// <param name="to"></param>
        public static  void CopyDirectory(string from, string to)
        {
            if (!Directory.Exists(to)) Directory.CreateDirectory(to);
            string [] files = Directory.GetFiles(from, "*.*", SearchOption.AllDirectories);
            foreach (string item in files)
            {
                string dest = to + item.Replace(from, "");
                string dir = Path.GetDirectoryName(dest);
                if (!Directory.Exists(dir)) Directory.CreateDirectory(dir);

                File.Copy(item, dest);
            }
        }
    }

    /// <summary>
    /// 文件夹比较项目。
    /// </summary>
    public class DirItem
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public DirItem() { }
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="absDirectory"></param>
        /// <param name="fullPath"></param>
        public DirItem(string directory, string fullPath)
        {
            this.FullPath = fullPath;
            this.Directory = directory;
            this.HalfaPath = fullPath.Replace(directory, "");
        }
        /// <summary>
        /// 全路径
        /// </summary>
        public string FullPath { get; set; }
        /// <summary>
        /// 目录
        /// </summary>
        public string Directory { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string HalfaPath { get; set; }
        /// <summary>
        /// 字符串
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return FullPath.ToString();
        }
        /// <summary>
        /// 哈希
        /// </summary>
        /// <returns></returns>
        public override int GetHashCode()
        {
            return HalfaPath.GetHashCode();
        }
        /// <summary>
        /// 是否相等
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public override bool Equals(object obj)
        {
            DirItem o = obj as DirItem;
            if (o == null) return false;

            return HalfaPath.Equals(o.HalfaPath);
        } 
    }


    /// <summary>
    /// 文件比较
    /// </summary>
    public class FileComparison
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="dirA"></param>
        /// <param name="dirB"></param>
        public FileComparison(string dirA, string dirB)
        {
            this.DirA = dirA;
            this.DirB = dirB;
            ListA = FileComparison.Parse(dirA);
            ListB = FileComparison.Parse(dirB);

            DiffersInA = new List<FileItem>();
            foreach (FileItem item in ListA)
            {
                if (!ListB.Contains(item)) DiffersInA.Add(item);
            }
            DiffersInB = new List<FileItem>();
            foreach (FileItem item in ListB)
            {
                if (!ListA.Contains(item)) DiffersInB.Add(item);
            }
        }
        /// <summary>
        /// 目录A
        /// </summary>
        public string DirA { get; set; }
        /// <summary>
        /// 目录B
        /// </summary>
        public string DirB { get; set; }
        /// <summary>
        /// 列表A
        /// </summary>
        public List<FileItem> ListA { get; set; }
        /// <summary>
        /// 文件集合B
        /// </summary>
        public List<FileItem> ListB { get; set; }
        /// <summary>
        /// A中不同
        /// </summary>
        public List<FileItem> DiffersInA { get; set; }
        /// <summary>
        /// B中不同
        /// </summary>
        public List<FileItem> DiffersInB { get; set; }

        /// <summary>
        /// 转换为字符串
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            //显示结果
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("文件夹 " + this.DirA + " 下有 " + this.ListA.Count + " 个文件, " + this.DiffersInA.Count + " 个不同的文件");

            foreach (var item in this.DiffersInA)
            {
                sb.AppendLine(item.FullPath);
            }

            sb.AppendLine("文件夹 " + this.DirB + " 下有 " + this.ListB.Count + " 个文件, " + this.DiffersInB.Count + " 个不同的文件");
            foreach (var item in this.DiffersInB)
            {
                sb.AppendLine(item.FullPath);
            }
            return sb.ToString();
        }
        /// <summary>
        /// 将目录下所有文件读取解析为文档
        /// </summary>
        /// <param name="dir"></param>
        /// <returns></returns>
        public static List<FileItem> Parse(string dir)
        {
            string[] files = System.IO.Directory.GetFiles(dir, "*.*", SearchOption.AllDirectories);
            List<FileItem> fileList = new List<FileItem>();
            foreach (string item in files)
            {
                fileList.Add(new FileItem(dir, item));
            }
            return fileList;
        }
        /// <summary>
        /// 复制A中不同的到目录B中
        /// </summary>
        public void CopyDifferInAToB()
        {
            foreach (var item in DiffersInA)
            {
                CopyFile(item.FullPath, DirB+ item.HalfaPath);
            }
        }
        /// <summary>
        /// 复制
        /// </summary>
        /// <param name="from"></param>
        /// <param name="to"></param>
        public void CopyFile(string from, string to)
        {
            string dir = Path.GetDirectoryName(to);
            if (!Directory.Exists(dir)) Directory.CreateDirectory(dir);

            File.Copy(from, to);
        }
    }

    /// <summary>
    /// 文件比较项目。
    /// </summary>
    public class FileItem : DirItem
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public FileItem() { }
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="absDirectory"></param>
        /// <param name="fullPath"></param>
        public FileItem(string directory, string fullPath)
            : base(directory, fullPath)
        {
            this.FileName = Path.GetFileName(fullPath);
        }
        /// <summary>
        /// 名称
        /// </summary>
        public string FileName { get; set; }
        /// <summary>
        /// 文本
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return FullPath.ToString();
        }
        /// <summary>
        /// 哈希
        /// </summary>
        /// <returns></returns>
        public override int GetHashCode()
        {
            return HalfaPath.GetHashCode();
        }
        /// <summary>
        /// 是否相等
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public override bool Equals(object obj)
        {
            FileItem o = obj as FileItem;
            if (o == null) return false;

            return HalfaPath.Equals(o.HalfaPath);
        }
        /// <summary>
        /// 获取所有的文件
        /// </summary>
        /// <param name="dir"></param>
        /// <returns></returns>
        public static List<FileItem> Parse(string dir)
        {
            string[] files = System.IO.Directory.GetFiles(dir, "*.*", SearchOption.AllDirectories);
            List<FileItem> fileList = new List<FileItem>();
            foreach (string item in files)
            {
                fileList.Add(new FileItem(dir, item));
            }
            return fileList;
        }
        /// <summary>
        /// 获取两个文件夹中不同的文件列表
        /// </summary>
        /// <param name="dirA"></param>
        /// <param name="dirB"></param>
        /// <returns></returns>
        public static List<FileItem> GetDiffers(string dirA, string dirB)
        {
            List<FileItem> listA = FileItem.Parse(dirA);
            List<FileItem> listB = FileItem.Parse(dirB);

            List<FileItem> differs = FileItem.Parse(dirB);
            foreach (FileItem item in listA)
            {
                if (!listB.Contains(item)) differs.Add(item);
            }
            return differs;
        }
    }
}



    



