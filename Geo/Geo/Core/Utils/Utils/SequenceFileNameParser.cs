//2014.12.27, czs, create in namu, 序列文件名称解析器

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace Geo.Utils
{
    /// <summary>
    /// 序列文件名称解析器。主要用于拼接观测文件，如多日多文件的观测文件放在一起，自动区牌靴和分为不同的分组。
    /// 只处理统一类型的文件（不处理后缀名）
    /// 1.判断输入是否为序列，
    /// 2.对不同序列进行分组
    /// </summary>
    public class SequenceFileNameParser
    {

        public SequenceFileNameParser(string [] fileNames)
        {
            List<string> originalNames = new List<string>(fileNames);

            List<string> suffixes = new List<string>();
            List<string> nameOnly = new List<string>();
            foreach (var item in originalNames)
            {
                suffixes.Add(Path.GetExtension(item));
                var name = Path.GetFileNameWithoutExtension(item);
                if(!nameOnly.Contains(name))  nameOnly.Add(name);
            }

            nameOnly.Sort();

            Dictionary<string, List<string>> nameGroups = new Dictionary<string, List<string>>();
            int i = 0;
            int number = 0;
            foreach (var item in nameOnly)
            {
                if (i == 0) nameGroups[item] = new List<string>();




                i++;
            }



        }

        //public static int GetLastNumber 


        /// <summary>
        /// 文件名称
        /// </summary>
        public List<List<string>> FileNames { get; set; }

    }
}
