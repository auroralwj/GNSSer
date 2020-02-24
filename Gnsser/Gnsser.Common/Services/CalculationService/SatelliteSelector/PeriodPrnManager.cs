//2017.03.09, czs, create in hongqing, 分时段卫星编号选择器


using System;
using System.Collections.Generic;
using System.Linq;
using Geo;
using Geo.IO;
using Geo.Common;
using Geo.Coordinates;

using Geo.Times;
using Gnsser.Core;
using Gnsser.Domain;
using System.Text;
using System.IO;

namespace Gnsser
{
   


    /// <summary>
    /// 分时段卫星编号 管理器
    /// </summary>
    public class PeriodPrnManager : TimeSquentialValue<PeriodPrn, SatelliteNumber>
    {
        /// <summary>
        ///读取
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public static PeriodPrnManager ReadFromFile(string path)
        {
            var reader = new PeriodPrnReader(path);
            var all = reader.ReadAll();
            PeriodPrnManager mgr = new PeriodPrnManager();
            foreach (var item in all)
            {
                mgr.Add(item);
            }
            return mgr;
        }
        /// <summary>
        /// 写入
        /// </summary>
        /// <param name="path"></param>
        public void WriteToFile(string path)
        {
            using (var writer = new PeriodPrnWriter(path))
            {
                foreach (var item in this)
                {
                    writer.Write(item);
                }
            }
        }
        /// <summary>
        /// 返回文本
        /// </summary>
        public string GetText()
        {
            using (MemoryStream stream = new MemoryStream())
            {
                var writer = new PeriodPrnWriter(stream);  
                foreach (var item in this)
                {
                    writer.Write(item);
                } 
                writer.Flush();
                stream.Position = 0;
                StreamReader reader = new StreamReader(stream);
                return  reader.ReadToEnd();
            }
        }


    }
    
    /// <summary>
    /// 文件写入
    /// </summary>
    public class PeriodPrnWriter : LineFileWriter<PeriodPrn>
    {

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="filePath">数据文件路径。</param>
        public PeriodPrnWriter(Stream filePath)
            : base(filePath, null)
        {

        }
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="filePath">数据文件路径。</param>
        public PeriodPrnWriter(string filePath)
            : base(filePath)
        {

        }


    }

    /// <summary>
    /// 文件读取
    /// </summary>
    public class PeriodPrnReader : LineFileReader<PeriodPrn>
    {        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="filePath">数据文件路径。</param>
        /// <param name="metaFilePathOrDirectory">元数据文件或者元数据文件存放的目录。若不指定，则自动寻找文件目录下的元数据，若没有则调用默认的元数据，若还没有则报错。</param>
        public PeriodPrnReader(string filePath, string metaFilePathOrDirectory = null)
            : base(filePath, metaFilePathOrDirectory)
        { 
        } 
        public override PeriodPrn Parse(string[] items)
        {
           var span =  TimePeriod.Parse(items[0]);
           var prn = SatelliteNumber.Parse(items[1]);

           return new PeriodPrn(span, prn);
        }
    }


    /// <summary>
    /// 分时段卫星编号
    /// </summary>
    public class PeriodPrn : TimePeriodValue<SatelliteNumber>, IOrderedProperty    
    {
        /// <summary>
        /// 默认构造函数 
        /// </summary>
        public PeriodPrn():this(null,SatelliteNumber.Default)
        {

        }
        
        /// <summary>
        /// 默认构造函数
        /// </summary>
        /// <param name="perid"></param>
        /// <param name="val"></param>
        public PeriodPrn(TimePeriod perid, SatelliteNumber val)
            : base(perid, val)
        {
            OrderedProperties = new List<string>() { "TimePeriod", "Value" };
        }

        public List<string> OrderedProperties
        {
            get;
            set;
        }

        public List<Geo.IO.ValueProperty> Properties { get; set; }
    }
}
