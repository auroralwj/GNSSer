//2015.01.10, czs, create in namu, RINEX 观测值数据源游走器/遍历器。

using System;
using System.Collections.Generic;
using System.Text;
using Geo;
using Gnsser.Domain;

namespace Gnsser.Data.Rinex
{
    /// <summary>
    /// RINEX 观测值数据源游走器/遍历器。
    /// </summary>
    public class ObsDataSourceWalker : EnumerableWalker<EpochInformation>
    {
        /// <summary>
        /// 数据遍历处理器，构造函数。
        /// </summary>
        /// <param name="data"></param>
        public ObsDataSourceWalker(IEnumerable<EpochInformation> data)
            : base(data)
        { 
        } 
    }

    /// <summary>
    /// RINEX 观测值数据源游走器/遍历器。
    /// </summary>
    public class BaseLineObsDataSourceWalker : TwoEnumerableWalker<EpochInformation>
    { 
        /// <summary>
        /// 数据遍历处理器，构造函数。
        /// </summary>
        /// <param name="satData"></param>
        public BaseLineObsDataSourceWalker(ISingleSiteObsStream dataA, ISingleSiteObsStream dataB)
            : base(dataA, dataB)
        { 
        }

        /// <summary>
        /// walk
        /// </summary>
        public override void Walk()
        {
            ISingleSiteObsStream dataSourceB = this.EnumerableDataB as ISingleSiteObsStream;
            foreach (var obsA in this.EnumerableDataA)
            {
                var obsB = dataSourceB.Get(obsA.ReceiverTime, 1);

                var obj = obsA;
                var objB = obsA;
                if (!ProcessorChain.Revise(ref obj, ref objB))
                {
                    //throw new Exception(ProcessorChain.Message);
                }
                
            }
        }
    }

}
