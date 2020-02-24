// 2014.09.15, czs, create, 误差赋值器
// 2014.09.19, czs, edit in 海鲁吐,   将接收机改正转化为各个卫星分量的距离改正
// 2014.09.20, czs, edit in 海鲁吐,   增加卫星频率改正
// 2014.09.21， cy, edit in zz, 调校各种改正
// 2014.09.24，czs, edit in halutu, 1.重构为责任链列表，以免构造函数无限扩张；2.将所有的改正数采用加号“+”相加。
// 2014.11.30, czs, edit in jinxinliaomao shuangliao,   观测值改正器。直接改正观测值，与模型改正器对应。

using System;
using System.Collections.Generic;
using Gnsser.Domain;
using System.Text;
using Geo.Coordinates;
using Gnsser.Data.Rinex;
using Geo.Algorithm;
using Geo.Algorithm.Adjust;
using Gnsser.Service;
using Geo.Utils;
using Gnsser.Checkers;
using Geo.Common;
using Gnsser.Correction;
using Geo.Correction;

namespace Gnsser
{
    /// <summary>
    /// 改正数处理器.
    /// 观测值改正器。直接改正观测值，与模型改正器对应。
    /// </summary>
    public class ObsCorrectionSetter : EpochInfoReviser
    {
        /// <summary>
        /// 改正数处理器构造函数。
        /// </summary>
        public ObsCorrectionSetter()
        {
            this.CorrectorChains = new List<ICorrector>();
        }

        /// <summary>
        /// 改正链表列表。
        /// </summary>
        List<ICorrector> CorrectorChains { get; set; }

        /// <summary>
        /// 添加一个改正链表。
        /// 注意：添加的类型必须在 Process 中判断并处理，否则添加进来不会起作用的。
        /// </summary>
        /// <param name="chain">改正链表</param>
        public void AddCorrectorChain(ICorrector chain)
        {
            CorrectorChains.Add(chain);
        }

        public override bool Revise(ref EpochInformation info)
        {
            //依次对每个责任链进行解析
            foreach (var chain in CorrectorChains)
            { 
                #region  通用距离改正，对所有的距离改正有效 
                if (chain is RangeCorrectionReviser)
                {
                    RangeCorrectionReviser rangeCorrectorChain = chain as RangeCorrectionReviser;
                    foreach (var sat in info)//分别对指定卫星进行改正
                    {
                        rangeCorrectorChain.Correct(sat); 
                    }
                }
                #endregion 
            }

            return info.EnabledPrns.Count > 0;
        }
   
    }
}
