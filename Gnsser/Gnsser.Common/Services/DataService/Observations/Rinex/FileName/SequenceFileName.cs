//2014.12.27, czs, create in namu, 序列文件。连续的文件


using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Text;

namespace Gnsser.Data.Rinex
{ 

    /// <summary>
    /// 序列文件。连续的文件。
    /// </summary>
    public class SequenceFileName
    {
        /// <summary>
        /// 构造函数。
        /// </summary>
        /// <param name="First">初始化序列</param>
        public SequenceFileName(FileName First)
        {
            this.First = First;
        }
        /// <summary>
        /// 测站名称
        /// </summary>
        public string StationName { get; set; }
        /// <summary>
        /// 第一个文件名称
        /// </summary>
        public FileName First { get; set; }

        /// <summary>
        /// 获取一个连续的文件序列
        /// </summary>
        /// <param name="dayCount">天数。</param>
        /// <returns></returns>
        public List<FileName> GetFileNames(int dayCount)
        {
            List<FileName> list = new List<FileName>();
           // colName.Add(First);
            var tmp = FileName.Parse(First.FileNameString);
            for (int i = 0; i < dayCount; i++)
            {
                list.Add(tmp);
                tmp = new FileName(tmp);//new object 
            }

            return list;
        }
    }

}
