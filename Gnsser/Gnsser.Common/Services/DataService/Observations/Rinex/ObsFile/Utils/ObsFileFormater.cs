//2013, czs, create, 为Bernese而生。
//2015.05.11, czs, edit in namu, 整理合并原格式化方法。
//2015.05.22,cy, 数据格式转换，剔除观测时段不够的数据

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using Geo.Utils;

namespace Gnsser.Data.Rinex
{
    /// <summary>
    /// O文件选格式化器。
    /// 2013年创建
    /// </summary>
    public class ObsFileFormater
    {
        /// <summary>
        /// 格式化O文件,如果修改了，则返回true。并将其移动到指定的目录。
        /// 标准化文件内容供Bernese5.0调用。
        /// 包括：1）标准化 MarkerName 为四个字的英文
        /// </summary>
        /// <param name="text"></param>
        public static bool Format(string path, string destDir = null)
        {
            bool needRewrite = false;
            StringBuilder sb = new StringBuilder();
            using (StreamReader r = new StreamReader(path))
            {
                string line = null;
                while ((line = r.ReadLine()) != null)
                {
                    if (!needRewrite && line.Contains(RinexHeaderLabel.END_OF_HEADER)) break;

                    //名称控制在 4 个
                    if (line.Contains(RinexHeaderLabel.MARKER_NAME))
                    {
                        if (line.Substring(0, 60).Trim().Length > 4)
                        {
                            string name = Path.GetFileName(path).Substring(0, 4);
                            string newLine = StringUtil.FillSpace(name, 60)
                                + RinexHeaderLabel.MARKER_NAME;
                            line = newLine;
                            // sb.AppendLine(newLine);
                            needRewrite = true;
                        }
                    }
                    ////天线只取前些字母
                    //if (line.Contains(HeaderLabel.ANT_NUM_TYPE))
                    //{
                    //    string AntennaNumber = line.Substring(0, 20);
                    //    string[] AntennaTypes = line.Substring(20, 20).Trim().Split(new char[] { ' ' },
                    //          StringSplitOptions.RemoveEmptyEntries);
                    //    if (AntennaTypes.Length > 1)
                    //    {

                    //        line = StringUtil.FillSpace(AntennaNumber, 20)
                    //            + StringUtil.FillSpace(AntennaTypes[0], 20);
                    //        line = StringUtil.FillSpace(line, 60) + "ANT # / TYPE";
                    //        needRewrite = true;
                    //    }
                    //}

                    sb.AppendLine(line);
                }
            }
            if (needRewrite)
            {
                string dest = path;
                if (destDir != null) dest = Path.Combine(destDir, Path.GetFileName(path));
                File.WriteAllText(dest, sb.ToString());
            }

            return needRewrite;
        }
        
        /// <summary>
        /// 本方法只实现了判断是否包含指定的天线类型。
        /// 标准化文件内容供Bernese5.0调用。
        /// 包括：1）标准化 MarkerName 为四个字的英文；2）去掉天线类型空格后的内容。
        /// </summary>
        /// <param name="text"></param>
        public static bool UpperAntTypeName(string path, string[] antnnas)
        {  
            using (StreamReader r = new StreamReader(path))
            {
                string line = null;
                while ((line = r.ReadLine()) != null)
                {
                    if (line.Contains(RinexHeaderLabel.ANT_NUM_TYPE))
                    {
                        foreach (var item in antnnas)
                        {
                            if (line.ToUpper().Contains(item.ToUpper())) return true;
                        }
                        return false;
                    } 
                }
            }

            return false;
        }
    }
}
