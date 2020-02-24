//2017.05.10, lly, add in zz,GT2 模型
//2017.06.28, czs, edit in hongqing, 重构，速度优化


using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using Gnsser.Service;
using Geo.Algorithm;
using Geo.Coordinates;
using Gnsser.Data.Rinex; 
using System.Collections.Concurrent;
using Geo.Utils;
using Gnsser;
using Geo;
using Geo.Common;
using Gnsser.Times;
using Geo.Times;

namespace Gnsser.Data
{
    public class Gpt2DataService1Degree : FileBasedService<Gpt2Res1Degree>
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="gpt2FilePath"></param>
        public Gpt2DataService1Degree(FileOption gpt2FilePath)
            : base(gpt2FilePath)
        {
            gpt2File1Degree = new Gpt2FileReader1Degree(this.Option.FilePath).Read();

            Data = new ConcurrentDictionary<string, Gpt2Res1Degree>();
        }
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="gpt2FilePath"></param>
        public Gpt2DataService1Degree(string  gpt2FilePath)
            : base(gpt2FilePath)
        {
            gpt2File1Degree = new Gpt2FileReader1Degree(this.Option.FilePath).Read();

            Data = new ConcurrentDictionary<string, Gpt2Res1Degree>();
        }


        //string path = "D:\\[VSPro]\\Gnsser2016.12.01[西安试算后分发版]+PCO PCV\\Gnsser\\Gnsser.Winform\\bin\\Debug\\Data\\GNSS\\Common\\gpt2_5.grd";
        static object locker = new object();
        /// <summary>
        /// 获取
        /// </summary>
        /// <param name="time"></param>
        /// <param name="geoCoord"></param>
        /// <returns></returns>
        public Gpt2Res1Degree Acquire(Time time, GeoCoord geoCoord)
        {
            double lat = geoCoord.Lat;
            double lon = geoCoord.Lon;
            double height = geoCoord.Height;

            var key = BuildKey(time, geoCoord);
            if (!Data.ContainsKey(key))
            {
                this.gpt2Res1Degree = gpt2File1Degree.GetGridInfo_1Degree(time, lat, lon, height);
                Data[key] = (gpt2Res1Degree);
                return Data[key];
            }

            gpt2Res1Degree = Data[key];
            return gpt2Res1Degree;

        }

        string BuildKey(Time time, GeoCoord coord)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(time.ToDateString()).Append(",").Append(coord.GetUniqueKey(1));
            return sb.ToString();
            //return + "," + coord.GetUniqueKey(1);
        }

        Gpt2File1Degree gpt2File1Degree;

        Gpt2Res1Degree gpt2Res1Degree = null;


        public ConcurrentDictionary<string, Gpt2Res1Degree> Data { get; set; }


        /// <summary>
        /// 数量。
        /// </summary>
        public int Count { get { return Data.Count; } }

        /// <summary>
        /// GetEnumerator
        /// </summary>
        /// <returns></returns>
        public IEnumerator<Gpt2Res1Degree> GetEnumerator()
        {
            return Data.Values.GetEnumerator();
        }

        //System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        //{
        //    return this.GetEnumerator();
        //}

        public void Clear()
        {
            Data.Clear();
        }

        public void Dispose()
        {
            Clear();
        }
    }
}
