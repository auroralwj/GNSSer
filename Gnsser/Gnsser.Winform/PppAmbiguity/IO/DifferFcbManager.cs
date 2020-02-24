//2016.08.12, czs, create in fujian yong'an, 基于无电离层组合星间单差模糊度的宽窄项
//2016.10.19, czs, edit in hongqing, 修改为字典模式

using System;
using System.IO;
using System.Text;
using System.Collections.Generic;
using Geo;
using Geo.Algorithm;
using Geo.Coordinates;
using Geo.Algorithm.Adjust;
using Geo.Algorithm;
using Gnsser.Times;
using Gnsser.Data;
using Gnsser.Data.Rinex;
using Gnsser.Domain;
using Gnsser.Service;
using Gnsser.Correction;
using Geo.Times;
using Geo.IO;
using Gnsser;
using Geo;
using Gnsser.Data;
using Gnsser.Checkers;
using Geo.Referencing;
using Geo.Utils;
using System.IO;

namespace Gnsser
{
    /// <summary>
    /// 基于无电离层组合星间单差模糊度的宽窄项。 
    /// 一个管理器对应一个类。
    /// 计算产品的存储、调用、输出。
    /// </summary>
    public class DifferFcbManager : BaseDictionary<string, BaseList< DifferFcbOfSatDcbItem>>
    {
        /// <summary>
        /// 构造函数。
        /// </summary>
        public DifferFcbManager()
        {

        }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="path"></param>
        public DifferFcbManager(string path)
        {
            this.FilePath = path;
            Load(path);
        }
        /// <summary>
        ///构造函数
        /// </summary>
        public DifferFcbManager(string direcotry, Time time)
        {
            var NameBuilder = new YearDayFileNameBuilder(direcotry, ".sddcb");
            this.FilePath = NameBuilder.Build();
            Load(FilePath);
        }

        #region 属性 
        /// <summary>
        /// 文件路径
        /// </summary>
        public string FilePath { get; set; }

        #endregion

        #region 方法
        /// <summary>
        /// 从文件加载
        /// </summary>
        /// <param name="path"></param>
        public void Load(string path)
        {
            DifferFcbOfSatDcbReader reader = new DifferFcbOfSatDcbReader(path);
            foreach (var item in reader)
            {
                this.Add(item);
            }
        }

        /// <summary>
        /// 创建一个新的。
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public override Geo.BaseList<DifferFcbOfSatDcbItem> Create(string key)
        {
            return new Geo.BaseList<DifferFcbOfSatDcbItem>();
        }

        /// <summary>
        /// 添加
        /// </summary>
        /// <param name="key"></param>
        public void Add(DifferFcbOfSatDcbItem item) { this.GetOrCreate(item.Key).Add(item); }           

   
        #region  IO
        static object locker = new object();
        public void WriteToFile(string path)
        {
            lock (locker)
            using (DifferFcbOfSatDcbWriter writer = new DifferFcbOfSatDcbWriter(path, System.IO.FileMode.Append))
            {
                foreach (var item in this)
                {
                    writer.Write(item); 
                }
            }
        }
        /// <summary>
        /// 读取
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public static DifferFcbManager Read(string path)
        {
            return new DifferFcbManager(path);
        }
        #endregion
        #endregion
    }

}