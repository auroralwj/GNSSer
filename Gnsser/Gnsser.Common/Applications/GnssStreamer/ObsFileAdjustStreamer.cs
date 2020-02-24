//2016.08.20, czs, create in 福建永安, 宽项计算器，多站观测数据遍历器
//2016.08.29, czs, edit in 西安洪庆, 重构多站观测数据遍历器
//2016.11.19，czs, refact in hongqing, 提取更通用的观测文件数据流
//2018.07.29, czs, edit in HMX, 修改通用GNSS数据流执行器
//2018.11.05, czs, edit in hmx, 提取平差数据流处理器


using System;
using System.IO;
using System.Text;
using System.Collections.Generic;
using Geo;
using Geo.Algorithm;
using Geo.Coordinates;
using Geo.Algorithm.Adjust;
using Gnsser.Times;
using Gnsser.Data;
using Gnsser.Data.Rinex;
using Gnsser.Domain;
using Gnsser.Service;
using Gnsser.Correction;
using Geo.Times;
using Geo.IO;
using Gnsser; 
using Geo.Referencing; 
using Geo.Utils; 
using Gnsser.Checkers;

namespace Gnsser
{
    /// <summary>
    /// GNSS 平差数据流处理器
    /// </summary>
    /// <typeparam name="TMaterial"></typeparam>
    /// <typeparam name="TResult"></typeparam>
    public abstract class ObsFileAdjustStreamer<TMaterial, TResult> : ObsFileProcessStreamer<TMaterial, TResult> where TMaterial : ISiteSatObsInfo
        where TResult : AdjustmentResult
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public ObsFileAdjustStreamer()
        { 
        }
        /// <summary>
        /// 检核结果。
        /// </summary>
        /// <param name="newResult"></param>
        /// <returns></returns>
        protected override bool CheckResult(TResult newResult)
        {          
            if (this.Option.IsOnlySameParam && this.CurrentGnssResult != null)
            {
                if (this.CurrentGnssResult.ResultMatrix.ParamCount != newResult.ResultMatrix.ParamCount
                    || Geo.Utils.ListUtil.GetDifferences(this.CurrentGnssResult.ResultMatrix.ParamNames, newResult.ResultMatrix.ParamNames).Count > 0
                    )
                {
                    this.IsCancel = true;
                    log.Warn("出现了不同参数，由于设定了只允许相同参数，计算中断！" + Geo.Utils.StringUtil.ToString(Geo.Utils.ListUtil.GetDifferences(this.CurrentGnssResult.ResultMatrix.ParamNames, newResult.ResultMatrix.ParamNames)));
                }
                else if (this.CurrentGnssResult.ResultMatrix.SecondParamCount != newResult.ResultMatrix.SecondParamCount
                   || Geo.Utils.ListUtil.GetDifferences(this.CurrentGnssResult.ResultMatrix.SecondParamNames, newResult.ResultMatrix.SecondParamNames).Count > 0
                   )
                {
                    this.IsCancel = true;
                    log.Warn("出现了不同参数，由于设定了只允许相同参数，计算中断！" + Geo.Utils.StringUtil.ToString(Geo.Utils.ListUtil.GetDifferences(this.CurrentGnssResult.ResultMatrix.SecondParamNames, newResult.ResultMatrix.SecondParamNames)));
                }
            }

            return true;
        }
    }

}