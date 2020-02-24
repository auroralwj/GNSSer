//2017.10.19, czs, create in hongqing, 电离层历元参数改正服务
//2018.08.05, czs, edit in hmx, 增加拟合数量参数

using System;
using System.IO;
using System.Collections.Generic;
using System.Text;
using Geo.Algorithm;
using Geo.Algorithm.Adjust;
using Geo.Utils;
using Geo.Common;
using Geo.Coordinates;
using Gnsser.Service;
using Gnsser.Data;
using Gnsser.Times;
using Gnsser.Data.Rinex;
using System.Linq;
using Geo.IO;
using Geo;
using Geo.Times;
using Gnsser.Data.Sinex;

namespace Gnsser
{
      /// <summary>
    /// 电离层历元参数改正服务
    /// </summary>
    public class IonoEpochParamService : IService
    {
        /// <summary>
        /// 电离层历元参数改正服务
        /// </summary>
        /// <param name="filePath"></param>
        public IonoEpochParamService(string filePath)
        {
            Name = Path.GetFileName(filePath);
            this.FilePath = filePath;
            ObjectTableReader reader = new ObjectTableReader(this.FilePath);
            this.Table = reader.Read();
            this.Data = this.Table.GetTwoKeyDictionary("Epoch");
            var first = this.Data.KeyAs.First();
            var end = this.Data.KeyAs.Last();
            this.TimePeriod = new BufferedTimePeriod(first, end, 30 * 60);
            this.IsAverageOrFit = true;
            this.FitCount = 4;
        }

        /// <summary>
        /// 时间范围
        /// </summary>
        public BufferedTimePeriod TimePeriod { get; set; }

        /// <summary>
        /// 文件路径
        /// </summary>
        public string FilePath { get; set; }
        /// <summary>
        /// 用窗口平均值或用拟合值
        /// </summary>
        public bool IsAverageOrFit { get; set; }
        /// <summary>
        /// 名称
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 拟合数量
        /// </summary>
        public int FitCount { get; set; }

        /// <summary>
        /// 数据表
        /// </summary>
        public ObjectTableStorage Table { get; set; }
        TwoKeyDictionary<Time, string, double> Data { get; set; }
        /// <summary>
        /// 返回一个距离历元最近的结果。
        /// </summary>
        /// <param name="prn"></param>
        /// <param name="epoch">需要精确的时刻</param>
        /// <returns></returns>
        public double Get(SatelliteNumber prn, Time epoch)
        {
            if (Data != null )
            {
                if (Data.ContainsKeyBA(prn.ToString(), epoch)) { return Data[epoch, prn.ToString()]; }

                if (!Data.ContainsKeyB(prn.ToString())) { return 0; }

                if (!TimePeriod.Contains(epoch)) {
                    return 0; 
                }

                var vals = Data.GetValuesByKeyB(prn.ToString());

                //获取最近几个
                var dic = Geo.Utils.DoubleUtil.GetNearst(vals, m => m.SecondsOfWeek, epoch, FitCount);
                if (IsAverageOrFit)
                {
                    var ave = dic.Values.Average();
                    return ave;
                }
                else
                {
                    LsPolyFit fit = new LsPolyFit(2);
                    fit.InitAndFitParams<Time>(dic, m => m.SecondsOfWeek);
                    var y = fit.GetY(epoch.SecondsOfWeek);
                    return y;
                } 
            }
            return 0;
        }
    }
}
