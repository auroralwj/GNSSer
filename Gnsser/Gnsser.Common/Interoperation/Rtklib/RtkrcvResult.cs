//2015.03.30, czs, create in hailutu, 解析 Rtkrcv 实时定位的计算结果

using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using Geo.Utils;
using Geo.Coordinates;
using Geo.Common;

namespace Gnsser.Interoperation
{
    /// <summary>
    /// 解析 Rtkrcv 实时定位的计算结果
    /// </summary>
    public class RtkrcvResult
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public RtkrcvResult()
        {
            this.Items = new List<RtkrcvResultItem>();
        }
        /// <summary>
        /// 集合。
        /// </summary>
        public List<RtkrcvResultItem> Items { get; set; }

        /// <summary>
        /// 解析计算结果
        /// </summary>
        /// <param name="text">结果文本</param>
        /// <returns></returns>
        public static RtkrcvResult Parse(string text)
        {
            string[] lines = text.Split(new char[] { '\n' });
            return Parse(lines);
        }

        /// <summary>
        /// 解析计算结果。
        /// </summary>
        /// <param name="lines">结果行</param>
        /// <returns></returns>
        public static RtkrcvResult Parse(string[] lines)
        {
            RtkrcvResult result = new RtkrcvResult();

            foreach (var item in lines)
            {
                if (String.IsNullOrEmpty(item)) continue;
                var line = item.Trim();
                if (line.StartsWith("%"))
                {
                    continue;
                }

                result.Items.Add(RtkrcvResultItem.Parse(line));
            }

            return result;
        }
    }

    /// <summary>
    /// 一行代表的对象
    /// </summary>
    public class RtkrcvResultItem
    {
        /// <summary>
        /// 时间
        /// </summary>
        public DateTime Time { get; set; }
        /// <summary>
        /// 坐标
        /// </summary>
        public double[] Coords { get; set; }

        /// <summary>
        /// 解析实时计算行结果。相隔一个字符串为内部，2个为外部。
        /// </summary>
        /// <param name="line"></param>
        /// <returns></returns>
        public static RtkrcvResultItem Parse(string line)
        {
            line = line.Replace("  ", "\t");
            string[] items = line.Split(new char[] { '\t' }, StringSplitOptions.RemoveEmptyEntries);
            var dateString = items[0];

            string one = items[1];
            string two = (items[2]);
            string three = (items[3]);

            RtkrcvResultItem item = new RtkrcvResultItem();
            item.Time = DateTime.Parse(dateString);

            double lon = 0;
            double lat = 0;
            double height = 0;
            if (one.Trim().Contains(' '))//查看坐标内部是否有空格，如果有，则是度分秒格式。
            {
                lat = DMS.Parse(one).Degrees;
                lon = DMS.Parse(two).Degrees;
                height = Double.Parse(three); 
            }
            else
            { //查看是否是XYZ坐标，如果是，则需要转换
                //通过离地心距离判断
                double oneDouble = Double.Parse(one);
                double twoDouble = Double.Parse(two);
                double threeDouble = Double.Parse(three);
                //经纬度如(180， 180， 8848) 到地心的距离小于6000 000（地球下400 000米） 则认为是经纬度，不是米
                if (Math.Sqrt(oneDouble * oneDouble + twoDouble * twoDouble + threeDouble * threeDouble) < 6000000)
                {
                    lat = oneDouble;
                    lon = twoDouble;
                    height = threeDouble;
                }
                else
                {
                    var geoCoord = CoordTransformer.XyzToGeoCoord(new XYZ(oneDouble, twoDouble, threeDouble));
                    lon = geoCoord.Lon;
                    lat = geoCoord.Lat;
                    height = geoCoord.Height;
                }
            }

            item.Coords = new double[] { lon, lat,  height };
            return item;
        }
    }
}
