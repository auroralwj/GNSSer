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
using Geo.Algorithm;
using Geo.Utils;
using Gnsser;
using Geo;
using Geo.Common;
using Gnsser.Times;
using Geo.Times;

namespace Gnsser.Data
{
    public class Gpt2DataService : FileBasedService<Gpt2Res>
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="gpt2FilePath"></param>
        public Gpt2DataService(FileOption gpt2FilePath)
            : base(gpt2FilePath)
        {
            Gpt2File = new Gpt2FileReader(this.Option.FilePath).Read();

            Data = new Dictionary<string, Gpt2Res>();
        }
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="gpt2FilePath"></param>
        public Gpt2DataService(string  gpt2FilePath)
            : base(gpt2FilePath)
        {
            Gpt2File = new Gpt2FileReader(this.Option.FilePath).Read();

            Data = new Dictionary<string, Gpt2Res>();
        }


        //string path = "D:\\[VSPro]\\Gnsser2016.12.01[西安试算后分发版]+PCO PCV\\Gnsser\\Gnsser.Winform\\bin\\Debug\\Data\\GNSS\\Common\\gpt2_5.grd";
        static object locker = new object();
        /// <summary>
        /// 获取
        /// </summary>
        /// <param name="time"></param>
        /// <param name="geoCoord"></param>
        /// <returns></returns>
        public Gpt2Res Acquire(Time time, GeoCoord geoCoord)
        {
            double lat = geoCoord.Lat;
            double lon = geoCoord.Lon;
            double height = geoCoord.Height;

            var key = BuildKey(time, geoCoord);
            if (!Data.ContainsKey(key))
            {
                this.gpt2Res = Gpt2File.GetGridInfo(time, lat, lon, height);
                Data[key] = (gpt2Res);
                return Data[key];
            }

            gpt2Res = Data[key];
            return gpt2Res;

        }

        string BuildKey(Time time, GeoCoord coord)
        {
            return time.ToDateString() + "" + coord.GetUniqueKey(1);
        }

        Gpt2File Gpt2File;

        Gpt2Res gpt2Res = null;


        public Dictionary<string, Gpt2Res> Data { get; set; }


        /// <summary>
        /// 数量。
        /// </summary>
        public int Count { get { return Data.Count; } }

        /// <summary>
        /// GetEnumerator
        /// </summary>
        /// <returns></returns>
        public IEnumerator<Gpt2Res> GetEnumerator()
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
