//2017.10, lly, create in zz, 对流层服务
//2017.11.10, czs, edit in hongqing, 服务重构合并

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using Geo.Coordinates;
using Gnsser;
using Gnsser.Times;
using Geo.Times;
using Gnsser.Data;
using Geo;
using Geo.IO;

namespace Gnsser
{
    /// <summary>
    /// 对流层服务
    /// </summary>
    public class TropAugService : IService
    {
        Log log = new Log(typeof(TropAugService)); 
        /// <summary>
        /// 默认构造函数
        /// </summary>
        /// <param name="path"></param>
        public TropAugService(string path)
        {
            this.Name = Path.GetFileName( path);
            TropAugmentValues = new TropAugmentInfoReader(path).read();
        }
        List<TropAugmentValue> TropAugmentValues { get; set; }
        /// <summary>
        /// 路径
        /// </summary>
        public string Name { get; set; }

        public double Correction(Time time)
        {
            double res = 0.0;

            int index = (int)Math.Floor(time.SecondsOfDay / 300);
            //foreach(var item in TropAugmentValues)
            //{
                 
            //    if (item.time >= time && (item.time.SecondsOfDay - time.SecondsOfDay) < 300)
            //    {
            //        res = item.zwd;
            //        continue;
            //    }
            //}
            if (TropAugmentValues.Count > index)
            { 
                res = TropAugmentValues[index].zwd;
            }
            else
            {
                log.Warn("对流层服务 没有数据");
            }

            return res;
        }
    }
}
