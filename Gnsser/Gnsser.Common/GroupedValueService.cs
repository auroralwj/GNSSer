//2019.03.03, czs, create in 洪庆，分组服务

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Geo;
using Geo.Algorithm.Adjust;

namespace Gnsser
{
    /// <summary>
    /// 参数服务
    /// </summary>
    public class ParamValueService : ObjectTableBasedService
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="table"></param>
        public ParamValueService(ObjectTableStorage table) : base(table)
        {
            Data = new Dictionary<string, RmsedNumeral>();
            foreach (var item in table.BufferedValues)
            {
                var name = item["Name"].ToString();
                var val = (double)item["Value"];
                var rms = (double)item["Rms"];
                Data[name] = new RmsedNumeral(val, rms);
            }
        }

        /// <summary>
        /// 参数
        /// </summary>
        public List<string> ParamNames => new List<string>(Data.Keys);
        /// <summary>
        /// 数据
        /// </summary>
        public Dictionary<string, RmsedNumeral> Data { get; set; }
        /// <summary>
        /// 向量
        /// </summary>
        /// <returns></returns>
        public WeightedVector GetWeightedVector() => new WeightedVector(Data);
    }

}
