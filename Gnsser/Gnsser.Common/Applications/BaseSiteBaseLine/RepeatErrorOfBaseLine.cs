//2019.01.15, czs, create in hmx, 复测基线较差

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Geo;
using Geo.Times;
using Gnsser.Times;
using System.IO;
using Gnsser.Data.Rinex;
using Geo.Coordinates;

namespace Gnsser
{  
   
    /// <summary>
    /// 
    /// </summary>
    public class RepeatErrorOfBaseLineManager : BaseDictionary<SiteObsBaseline, PeriodRepeatErrorOfBaseLine>
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="GnssReveiverNominalAccuracy"></param>
        public  RepeatErrorOfBaseLineManager(GnssReveiverNominalAccuracy GnssReveiverNominalAccuracy)
        {
            this.GnssReveiverNominalAccuracy= GnssReveiverNominalAccuracy;
        }
        /// <summary>
        /// 接收机标称禁精度
        /// </summary>
        public GnssReveiverNominalAccuracy GnssReveiverNominalAccuracy { get; set; }
        public override PeriodRepeatErrorOfBaseLine Create(SiteObsBaseline key)
        {
            return new PeriodRepeatErrorOfBaseLine(key, GnssReveiverNominalAccuracy);
        }
        /// <summary>
        /// 获取所有的时段，并从前到后排序。
        /// </summary>
        /// <returns></returns>
        public List<TimePeriod> GetAlTrimNetPeriodInOrder()
        {
            List<TimePeriod> list = new List<TimePeriod>();
            foreach (var kv in this.KeyValues)
            {
                list.Add(kv.Key.NetPeriod); 
            }
            var result = list.Distinct().ToList();
            result.Sort();

            return result;
        }
        /// <summary>
        /// 编号
        /// </summary>
        /// <returns></returns>
        public BaseDictionary<TimePeriod, int> GetAllPeriodWithOrderNumber()
        {
            BaseDictionary<TimePeriod, int> result = new BaseDictionary<TimePeriod, int>();
            List<TimePeriod> list = GetAlTrimNetPeriodInOrder();
            if (list == null || list.Count == 0) { return result; }
            int i = 1;
            foreach (var item in list)
            {
                result[item] = i++;
            } 
            return result;
        }

        /// <summary>
        /// 对象表
        /// </summary>
        /// <returns></returns>
        public ObjectTableStorage GetObjectTable()
        {
            ObjectTableStorage table = new ObjectTableStorage("复测基线较差");
            var orderedPeriod = this.Keys;
            orderedPeriod.Sort();//排序
            BaseDictionary<TimePeriod, int> periodNums = GetAllPeriodWithOrderNumber();
            int baseLineIndex = 1;
            foreach (var key in orderedPeriod)
            {
                var periodError = this[key];  
                foreach (var row in periodError.KeyValues)
                {
                    table.NewRow();
                    table.AddItem("基线号", baseLineIndex++);
                    table.AddItem("时段", key.NetPeriod.ToDefualtPathString());
                    table.AddItem("比较时段", row.Key.ToDefualtPathString()); 

                    var periodNum = periodNums.GetOrCreate(key.NetPeriod);
                    var toPeriodNum = periodNums.GetOrCreate(row.Key);
                    table.AddItem("时段号", periodNum);
                    table.AddItem("比较时段号", toPeriodNum);

                    Dictionary<string, object> objRow = row.Value.GetObjectRow();
                    table.AddItem(objRow);
                }
            }
            return table;
        }
    }

    /// <summary>
    /// 复测基线较差
    /// </summary>
    public class PeriodRepeatErrorOfBaseLine : BaseDictionary<TimePeriod, QualityOfRepeatError>
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="LineName"></param>
        /// <param name="GnssReveiverNominalAccuracy"></param> 
        public PeriodRepeatErrorOfBaseLine(
            SiteObsBaseline LineName,
            GnssReveiverNominalAccuracy GnssReveiverNominalAccuracy
            )
        {
            this.GnssReveiverNominalAccuracy = GnssReveiverNominalAccuracy;
            this.BaseLine = LineName;
        } 
        /// <summary>
        /// 接收机标称禁精度
        /// </summary>
        public GnssReveiverNominalAccuracy GnssReveiverNominalAccuracy { get; set; }
        /// <summary>
        /// 基线
        /// </summary>
        public List<SiteObsBaseline> DifferPeriodSameNameLines { get; set; }
        /// <summary>
        /// 基线名称
        /// </summary>
        public SiteObsBaseline BaseLine { get; set; }

        #region 便捷属性
        /// <summary>
        /// 长度
        /// </summary>
        public double LineLength => BaseLine.GetLength();
        #endregion

        /// <summary>
        /// 初始化
        /// </summary>
        /// <param name="DifferPeriodSameNameLines"></param>
        public void Init(List<SiteObsBaseline> DifferPeriodSameNameLines)
        {
            this.DifferPeriodSameNameLines = DifferPeriodSameNameLines;
            if (this.BaseLine.EstimatedResult == null) { return; }

            foreach (var line in DifferPeriodSameNameLines)
            {
                if(line == this.BaseLine || line.EstimatedResult == null ) { continue; }

                var error = line.EstimatedResult.EstimatedVectorRmsedXYZ - BaseLine.EstimatedResult.EstimatedVectorRmsedXYZ;

                this[line.NetPeriod] = new QualityOfRepeatError((EstimatedBaseline)line.EstimatedResult, error, GnssReveiverNominalAccuracy);
            } 
        }

        /// <summary>
        /// 对象表
        /// </summary>
        /// <returns></returns>
        public ObjectTableStorage GetObjectTable()
        {
            ObjectTableStorage table = new ObjectTableStorage("复测基线较差");
            foreach (var row in this.KeyValues)
            {
                Dictionary<string, object> objRow = row.Value.GetObjectRow();
          
                table.NewRow();
                table.AddItem(objRow);
            }
            return table;
        }

    }
}
