//2015.01.07, czs, create in namu, 相对于前一均方差的最大倍数
//2017.09.15, czs, edit in hongqing, 修改结平差果检核策略，只和上一个STD比较，不再积累求平均
//2018.11.06, czs, edit in hmx, 进行了简单的修改

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Gnsser.Domain;
using Gnsser.Excepts;
using Gnsser.Service; 
using Geo.Algorithm.Adjust;

namespace Gnsser.Checkers
{
    /// <summary>
    /// 均方差/标准差检核。
    /// 去除均方差陡峭的平差结果。
    /// </summary>
    public class CliffyStdChecker : GnssResultChecker
    {
        /// <summary>
        /// 标准差检核构造函数。
        /// </summary>
        /// <param name="maxTimes">相对于前均方差平均值的最大倍数</param>
        /// <param name="GreenPass">绿色通信证，小于此直接放行</param>
        public CliffyStdChecker(double maxTimes = 10.0, double GreenPass = 0.5)
        {
            this.MaxTimes = maxTimes;
            this.GreenPass = GreenPass;
            this.Index = 0;
            this.MaxBreakCount = 3;

        }
        /// <summary>
        /// 相对于前一均方差的最大倍数
        /// </summary>
        public double MaxTimes { get; set; }
        /// <summary>
        /// 上一次有效的STD
        /// </summary>
        public double PrevValidStd { get; set; }
        /// <summary>
        /// 记录比较编号，当断裂发生后，重新纪录结果。
        /// </summary>
        public int Index { get; set; }
        /// <summary>
        /// 上一次有效的编号
        /// </summary>
        public int LastValidIndex { get; set; }
        /// <summary>
        /// 最大的断裂数量
        /// </summary>
        public int MaxBreakCount { get; set; }

        /// <summary>
        /// 绿色通行证
        /// </summary>
        public double GreenPass { get; set; }
        /// <summary>
        /// 当前最大允许
        /// </summary>
        public double CurrentMaxAlllowed { get => PrevValidStd * MaxTimes; }
        /// <summary>
        /// 检核是否满足要求
        /// </summary>
        /// <param name="adjust"></param>
        public override bool Check(BaseGnssResult adjust)
        {
            Index++;
            var infoHeader = adjust.Name + ", " + adjust.Material.ReceiverTime + ", ";

            var std = adjust.ResultMatrix.StdDev;
            if (std == 0) { log.Error(infoHeader +"消息，标准差为 " + std); return true; } // 不必判断
            if (!Geo.Utils.DoubleUtil.IsValid(std)) { log.Error(infoHeader + "检核不通过，标准差数值无效 " + std); return false; }

            //首次，或断裂后的首次，直接放行
            if (PrevValidStd == 0 || (Index - LastValidIndex > MaxBreakCount)) { 
                UpdatePassedValue(std);
                return true;
            }

            bool isTrue = std < GreenPass || (std <= CurrentMaxAlllowed);
            if (isTrue)
            {
                UpdatePassedValue(std);
            }
            else
            {
                var msg = infoHeader + adjust.Material.ReceiverTime + ", 标准差超限，当前为 " + std.ToString("0.000") + ", 上一有效值为：" + PrevValidStd.ToString("0.000") + "，最大允许倍数为 " + MaxTimes;
                log.Error(msg);
                Exception = new SatCountException(msg);
            }
            return isTrue;
        }

        private void UpdatePassedValue(double std)
        {
            this.PrevValidStd = std;
            this.LastValidIndex = Index;
        }

    }
}
