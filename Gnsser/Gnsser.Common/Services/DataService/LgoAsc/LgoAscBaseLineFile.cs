//2018.12.05, czs, edit in hmx, 莱卡LGO文件

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Geo;
using Geo.Times;

namespace Gnsser.Data
{
    /// <summary>
    ///基线转换器
    /// https://surveyequipment.com/assets/index/download/id/221/ 
    /// </summary>
    public class BaseLineFileConverter
    {
        /// <summary>
        /// 构建
        /// </summary>
        /// <param name="net"></param>
        /// <returns></returns>
        public EpochLgoAscBaseLine Build(BaseLineNet net)
        {
            EpochLgoAscBaseLine file = new EpochLgoAscBaseLine();
            if(!String.IsNullOrWhiteSpace(net.Name))
            {
                file.Name = net.Name;
            }
            else
            {
                file.Name = "新建文件";
            }
            foreach (var item in net.KeyValues)
            {
                file.BaseLines.Add(item.Key, new LgoAscBaseLine(item.Value));
            }

            return file;
        }
    } 

    /// <summary>
    /// 多历元
    /// </summary>
    public class  MultiEpochLgoAscBaseLineFile : BaseDictionary<Time, EpochLgoAscBaseLine>
    {
        public MultiEpochLgoAscBaseLineFile()
        {

        }
        /// <summary>
        /// 头部文件
        /// </summary>
        public LgoAscHeader Header { get; set; }


        public override EpochLgoAscBaseLine Create(Time key)
        {
            return new EpochLgoAscBaseLine();
        }
        /// <summary>
        /// 多时段
        /// </summary>
        /// <returns></returns>
        public BaseLineNetManager GetBaseLineNetManager()
        {
            BaseLineNetManager result = new BaseLineNetManager();

            foreach (var item in this.KeyValues)
            {
                var period = new BufferedTimePeriod(item.Key, item.Key);
                result[period] = item.Value.GetBaseLineNet();
            }
            return result;
        }

        /// <summary>
        /// 提取网
        /// </summary>
        /// <returns></returns>
        public List<EstimatedBaseline> GetEstimatedBaselines()
        {
            List<EstimatedBaseline> list = new List<EstimatedBaseline>();
            foreach (var kv in this.KeyValues)
            {
                foreach (var item in kv.Value.BaseLines)
                {
                    list.Add(item.Value.Baseline);
                }
            }
            return list;
        }
         
    }

    /// <summary>
    /// 文件
    /// </summary>
    public class EpochLgoAscBaseLine 
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public EpochLgoAscBaseLine()
        {
            Coords = new Dictionary<string, LgoAscPoint>();
            BaseLines = new Dictionary<GnssBaseLineName, LgoAscBaseLine>();
        }
        /// <summary>
        /// 名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 坐标信息集合
        /// </summary>
        public Dictionary<string, LgoAscPoint> Coords { get; set; }
        /// <summary>
        /// 基线信息集合
        /// </summary>
        public Dictionary<GnssBaseLineName, LgoAscBaseLine> BaseLines { get; set; }

        public void Set(LgoAscBaseLine line  )
        { 
            this.BaseLines[line.LineName] = line;
        }

        /// <summary>
        /// 提取网
        /// </summary>
        /// <returns></returns>
        public BaseLineNet GetBaseLineNet()
        {
            BaseLineNet net = new BaseLineNet();
            foreach (var item in BaseLines)
            {
                net.Add(item.Value.Baseline);
            }
            net.Init();
            return net;
        }
      

    }
}
