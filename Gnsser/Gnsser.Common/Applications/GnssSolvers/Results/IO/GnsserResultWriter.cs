//2014.12.25, czs, create in 洪庆, 结果输出文件

using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading;
using Gnsser;
using Gnsser.Domain;
using Gnsser.Service; 
using Gnsser.Data.Rinex;
using Geo.Utils;
using Geo.Coordinates;
using Geo.Referencing;
using Geo.Algorithm.Adjust;
using Geo.Common;
using Geo;
using Geo.Algorithm;

using Geo.IO;

namespace Gnsser
{
    /// <summary>
    /// GNSSer 结果文件写入器。
    /// </summary>
    public abstract class GnsserResultWriter<TProduct>
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="directory"></param>
        public GnsserResultWriter(string directory)
        {
            this.BaseDirectory = directory;
            this.IsOutputSummery = false;
            this.IsOutputSinex = false;
        }
        /// <summary>
        /// 基本的输出目录
        /// </summary>
        public string BaseDirectory { get; set; }
        /// <summary>
        /// 是否输出标准SINEX文件
        /// </summary>
        public bool IsOutputSinex { get; set; }
        /// <summary>
        /// 是否输出概略文件
        /// </summary>
        public bool IsOutputSummery { get; set; }
        /// <summary>
        /// 写产品
        /// </summary>
        /// <param name="product"></param>
        public abstract void Write(TProduct product);

    }

    /// <summary>
    /// 定位写入器
    /// </summary>
    public class GnsserPointPositionResultWriter : GnsserResultWriter<SingleSiteGnssResult>
    {      
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="directory"></param>
        public GnsserPointPositionResultWriter(string directory, GnssProcessOption Option)
            : base(directory)
        {
            this.Option = Option;
        }
        /// <summary>
        /// 配置
        /// </summary>
        public GnssProcessOption Option { get; set; }

        /// <summary>
        /// 写入文件
        /// </summary>
        /// <param name="product"></param>
        public override void Write(SingleSiteGnssResult product)
        {
            if (product == null) { return; }

            if (Option.IsOutputSinex && product.HasEstimatedXyz)
            { 
                var sinexPath = Path.Combine( BaseDirectory, product.Name + ".SNX");
                WriteToSinex(sinexPath, product);
            }

            if (Option.IsOutputSummery)
            {
                var summeryPath = Path.Combine(BaseDirectory, product.Name + ".SUMMERY");
                WriteSummeryFile(summeryPath, product);
            }
        }


        #region 写元数据文件
        private static void WriteSummeryFile(string summeryPath, SingleSiteGnssResult product)
        {
            Dictionary<string, string> dic = BuildMetaInfo(product);

            StringBuilder sb = new StringBuilder();
            foreach (var item in dic)
            {
                sb.Append(Geo.Utils.StringUtil.FillSpaceRight(item.Key, 10));
                sb.Append("\t:\t");
                sb.Append(item.Value);
                sb.AppendLine();
            }

            File.WriteAllText(summeryPath, sb.ToString(), Encoding.Default);
        }

        private static Dictionary<string, string> BuildMetaInfo(BaseGnssResult product)
        {
            Dictionary<string, string> dic = new Dictionary<string, string>();

            //var positioner = product.GnssSolver;
            //if (positioner != null)
            //{
            //    if (positioner != null && positioner.DataSourceContext.ObservationDataSource != null)
            //    {
            //        keyDic.Add("数据源", positioner.DataSourceContext.ObservationDataSource.Name);
            //    }

            //    keyDic.Add("初始坐标", product.ApproxXyz.ToString());
            //    keyDic.Add("星历", positioner.DataSourceContext.EphemerisService.Name);
            //    if (positioner.DataSourceContext.HasClockService)
            //    {
            //        keyDic.Add("钟差", positioner.DataSourceContext.ClockService.Name);
            //    }
            //    else
            //    {
            //        keyDic.Add("钟差", "无钟差文件，计算收敛时间受影响");
            //    }
            //    if (positioner.DataSourceContext.HasErpService)
            //    {
            //        keyDic.Add("地球自转", positioner.DataSourceContext.ErpDataService.Name);
            //    }
            //    else
            //    {
            //        keyDic.Add("地球自转", "无数据，计算精度受少量影响");
            //    }

            //    //显示天线信息 
            //    if (product.Material.SiteInfo.Antenna != null)
            //    {
            //        var ant = product.Material.SiteInfo.Antenna;
            //        keyDic.Add("接收机天线", ant.AntennaType + ", " + ant.AntennaSerial);
            //    }
            //    else
            //    {
            //        keyDic.Add("接收机天线", "无！将影响计算结果！！");
            //    }

            //    var option = positioner.Option;
            //    if (option != null)
            //    {
            //        keyDic.Add("计算方法", option.CaculateType.ToString());
            //        //keyDic.Add("起始历元数", option.DelayCount.ToString());
            //        keyDic.Add("高度截止角", option.VertAngleCut.ToString());
            //        keyDic.Add("缓存大小", option.BufferSize.ToString());
            //        keyDic.Add("最小连续历元数", option.MinContinuouObsCount.ToString());
            //        keyDic.Add("最大遍历循环数", option.MaxLoopCount.ToString());
            //        keyDic.Add("处理系统类型", String.Format(new EnumerableFormatProvider(), "{0}", option.SatelliteTypes));
            //    }
            //}
            return dic;
        }
        #endregion

        /// <summary>
        /// 写到文件
        /// </summary>
        /// <param name="path"></param>
        /// <param name="key"></param>
        public static void WriteToSinex(string path, SingleSiteGnssResult item)
        {
            File.WriteAllText(path, ResultSinexBuillder.Build(item).ToString(), Encoding.Default);
        }

    }
}
