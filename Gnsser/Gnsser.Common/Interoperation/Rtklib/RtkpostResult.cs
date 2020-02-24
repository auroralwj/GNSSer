//2018.10.12, czs, create in hmx, 解析 Rtk 定位的计算结果

using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using Geo.Utils;
using Geo.Coordinates;
using Geo.Common;
using Geo.Times;
using Geo;
using Geo.Coordinates;

namespace Gnsser.Interoperation
{
    /// <summary>
    /// 解析 Rtkrcv 实时定位的计算结果
    /// </summary>
    public class RtkpostResult
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public RtkpostResult()
        {
            this.Items = new List<RtkpostResultItem>();
            Comments = new List<string>();
        }
        /// <summary>
        /// 集合。
        /// </summary>
        public List<RtkpostResultItem> Items { get; set; }
        /// <summary>
        /// 注释
        /// </summary>
        public List<string> Comments { get; set; }


        public List<NamedXyz> GetNamedXyzs()
        {
            var xyzs = new List<NamedXyz>();

            foreach (var item in Items)
            {
                xyzs.Add(new NamedXyz(item.Time.ToString(), new XYZ(item.Xyz.X, item.Xyz.Y, item.Xyz.Z)));
            }
            return xyzs;
        }

        public ObjectTableStorage ToTable(string name="RtkLib 结果")
        {
            ObjectTableStorage table = new ObjectTableStorage(name);
            foreach (var item in Items)
            {
                table.NewRow();
                table.AddItem("Epoch", item.Time);
                table.AddItem("X", item.Xyz.X);
                table.AddItem("Y", item.Xyz.Y);
                table.AddItem("Z", item.Xyz.Z);
                table.AddItem("RmsX", item.XyzRms.X);
                table.AddItem("RmsY", item.XyzRms.Y);
                table.AddItem("RmsZ", item.XyzRms.Z);
                table.AddItem("RktSolveType", item.RktSolveType);
                table.AddItem("SatCount", item.SatCount);
            }

            return table;
        }


        /// <summary>
        /// 加载
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public static RtkpostResult Load(string path)
        {
            return Parse( File.ReadAllLines(path));
        }
        /// <summary>
        /// 解析计算结果
        /// </summary>
        /// <param name="text">结果文本</param>
        /// <returns></returns>
        public static RtkpostResult Parse(string text)
        {
            string[] lines = text.Split(new char[] { '\n' });
            return Parse(lines);
        }

        /// <summary>
        /// 解析计算结果。
        /// </summary>
        /// <param name="lines">结果行</param>
        /// <returns></returns>
        public static RtkpostResult Parse(string[] lines)
        {
            var  result = new RtkpostResult();

            foreach (var item in lines)
            {
                if (String.IsNullOrEmpty(item)) continue;
                var line = item.Trim();
                if (line.StartsWith("%"))
                {
                    result.Comments.Add(line);
                    continue;
                }

                result.Items.Add(RtkpostResultItem.Parse(line));
            }

            return result;
        }
    }

    /// <summary>
    /// 计算类型
    /// </summary>
    public enum RktSolveType
    {
        Fix=1,
        Float =2,
        Sbas=3,
        DGps=4,
        Single=5,
        Ppp=6
    }


    /// <summary>
    /// 一行代表的对象
    /// </summary>
    public class RtkpostResultItem
    {
        /// <summary>
        /// 时间
        /// </summary>
        public Time Time { get; set; }
        /// <summary>
        /// 坐标
        /// </summary>
        public XYZ Xyz { get; set; }
        /// <summary>
        /// 坐标
        /// </summary>
        public XYZ XyzRms { get; set; }
        /// <summary>
        /// 坐标
        /// </summary>
        public GeoCoord GeoCoord { get; set; }
        /// <summary>
        /// 是否固定解
        /// </summary>
        public RktSolveType RktSolveType { get; set; }
        /// <summary>
        /// 卫星数量
        /// </summary>
        public int SatCount { get; set; }


        /// <summary>
        /// 解析实时计算行结果。相隔一个字符串为内部，2个为外部。
        /// </summary>
        /// <param name="line"></param>
        /// <returns></returns>
        public static RtkpostResultItem Parse(string line)
        {
            var item = new RtkpostResultItem();

            //line = line.Replace("  ", "\t");
            //string[] items = line.Split(new char[] { '\t' }, StringSplitOptions.RemoveEmptyEntries);

            var dateString = Geo.Utils.StringUtil.SubString(line, 0, 23);
            var xyzString = Geo.Utils.StringUtil.SubString(line, 24, 44);
            var typeNum = Geo.Utils.StringUtil.SubString(line, 69, 3).Trim();
            var satNum = Geo.Utils.StringUtil.SubString(line, 73, 3).Trim();
            var xyzRmsString = Geo.Utils.StringUtil.SubString(line, 77, 26);

            item.Time = Time.Parse(dateString);
            item.Xyz = XYZ.Parse(xyzString);
            item.XyzRms = XYZ.Parse(xyzRmsString);
            item.RktSolveType = (RktSolveType)int.Parse( typeNum );//Q=1:fix,2:float,3:sbas,4:dgps,5:single,6:ppp
            item.SatCount = int.Parse(satNum); 

            return item;
        }
    }
}
