//2014.12.27, czs, create in namu, 文件名称分类

using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Text;

namespace Gnsser.Data.Rinex
{
    /// <summary>
    /// 文件名称分类。前提是文件都在一个目录下。
    /// </summary>
    public class FileNameClassifier
    {
        /// <summary>
        /// 文件名称分类 构造函数。
        /// </summary>
        /// <param name="obsPathes"></param>
        public FileNameClassifier(string[] obsPathes)
        {
            this.FilePathes = new List<string>(obsPathes);

            //目录
            this.Directory = Path.GetDirectoryName(obsPathes[0]);
            this.FileCount = obsPathes.Length;

            //转换
            List<FileName> fileNames = new List<FileName>();
            foreach (var item in obsPathes)
            {
                var name = FileName.Parse(Path.GetFileName(item));
                if (!fileNames.Contains(name))
                    fileNames.Add(name);
            }

            //分类
            this.ClassifiedNames = new Dictionary<string, List<FileName>>();
            foreach (var item in fileNames)
            {
                if (!ClassifiedNames.ContainsKey(item.StationName)) this.ClassifiedNames[item.StationName] = new List<FileName>();

                this.ClassifiedNames[item.StationName].Add(item);
            }
            //排序
            foreach (var item in this.ClassifiedNames)
            {
                item.Value.Sort();
            }
            //按照连续的文件分类
            this.SequenceFileNames = new Dictionary<FileName, List<FileName>>();
            foreach (var kv in this.ClassifiedNames)
            {
                List<FileName> originNames = kv.Value;
                //第一次提取
                List<FileName> sequence =  ExtractFirstSequence(originNames);
                
                //迭代
               while (sequence.Count != originNames.Count) //没有处理完毕
                {
                    originNames = originNames.GetRange(sequence.Count, originNames.Count - sequence.Count);
                    sequence = ExtractFirstSequence(originNames);
                }
            }
        }

        /// <summary>
        /// 文件目录。
        /// </summary>
        public string Directory { get; set; }

        /// <summary>
        /// 文件数量
        /// </summary>
        public int FileCount { get; set; }

        /// <summary>
        /// 提取第一个序列
        /// </summary>
        /// <param name="originNames">初始名称列表</param>
        /// <returns></returns>
        private List<FileName> ExtractFirstSequence(List<FileName> originNames)
        {
            var first = originNames[0];
            if (originNames.Count == 1)
            {
                this.SequenceFileNames[first] = new List<FileName>();
                this.SequenceFileNames[first].Add(first);
            }
            else
            {
                SequenceFileName sequence = new SequenceFileName(first);
                var list = sequence.GetFileNames(originNames.Count);
                var sames = GetSameNames(originNames, list);
                this.SequenceFileNames[first] = sames;
            }
            return this.SequenceFileNames[first];
        }

        /// <summary>
        /// 返回从第一个开始的相同部分。遇到不同的则截止。
        /// </summary>
        /// <param name="namesA">列表A</param>
        /// <param name="namesB">列表B</param>
        /// <returns></returns>
        public static List<FileName> GetSameNames(List<FileName> namesA, List<FileName> namesB)
        {
            List<FileName> sames = new List<FileName>();
            int count = Math.Min(namesA.Count, namesB.Count);
            for (int i = 0; i < count; i++)
            {
                if(namesB[i].Equals(namesA[i])) sames.Add(namesA[i]);
                else break;
            }
            return sames;
        }
        /// <summary>
        /// 原始输入路径
        /// </summary>
        public List<string> FilePathes { get; set; }

        /// <summary>
        /// 按照测站分类，不在乎文件的连续性。
        /// </summary>
        public Dictionary<string, List<FileName>> ClassifiedNames { get; set; }
        /// <summary>
        /// 序列名称
        /// </summary>
        public Dictionary<FileName, List<FileName>> SequenceFileNames { get; set; }
        /// <summary>
        /// 序列路径
        /// </summary>
        public Dictionary<FileName, List<string>> SequenceFilePaths
        {
            get
            {
                Dictionary<FileName, List<string>> pathesDic = new Dictionary<FileName, List<string>>();

                String[] pathLines = new string[FileCount]; 
                foreach (var kv in SequenceFileNames)
                {
                    pathesDic[kv.Key] = new List<string>(); 
                    foreach (var name in kv.Value)
                    {
                        string path = Path.Combine(Directory, name.FileNameString);
                        pathesDic[kv.Key].Add(path);  
                    }
                }
                return pathesDic;
            }
        }


        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            foreach (var kv in SequenceFileNames)
            {
                int i = 0;
                foreach (var name in kv.Value)
                {
                    string path = Path.Combine(Directory, name.FileNameString);
                    if (i != 0) sb.Append("\t");
                    sb.AppendLine(path);
                    i++;
                }
            }
            return sb.ToString();
        }
        public  string [] ToStepedLines()
        {
            String[] pathLines = new string[FileCount];
            int allIndex = 0;
            foreach (var kv in SequenceFileNames)
            {
                int i = 0;
                foreach (var name in kv.Value)
                {
                    string path = Path.Combine(Directory, name.FileNameString);
                    if (i != 0) path = ("\t") + path;
                   pathLines[allIndex]= (path);
                    i++;
                    allIndex++;
                }
            }
            return pathLines;
        }
    } 
}
