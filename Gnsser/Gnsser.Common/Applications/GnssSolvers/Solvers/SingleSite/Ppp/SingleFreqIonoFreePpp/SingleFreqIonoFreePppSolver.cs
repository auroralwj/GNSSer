//2017.09.14, czs, create in hongqing, 单频消电离层组合

using System;
using System.Collections.Generic;
using System.Text;
using Geo.Algorithm;
using Geo;
using Geo.Utils;
using Geo.Common;
using Gnsser.Domain;
using Geo.Algorithm.Adjust;
using Geo.Coordinates;
using Gnsser.Data.Rinex;
using Gnsser.Service;
using Gnsser.Checkers;

namespace Gnsser.Service
{ 
    /// <summary>
    /// 单频消电离层组合
    /// </summary>
    public class SingleFreqIonoFreePppSolver : SingleSiteGnssSolver
    {
        #region 构造函数 
        
        /// <summary>
        ///  单频消电离层组合 
        /// </summary>
        /// <param name="DataSourceContext"></param>
        /// <param name="PositionOption"></param>
        public SingleFreqIonoFreePppSolver(DataSourceContext DataSourceContext, GnssProcessOption PositionOption)
            : base(DataSourceContext, PositionOption)
        {
            this.Name = "单频消电离层组合";
            this.MatrixBuilder = new SingleFreqIonoFreePppMatrixBuilder(PositionOption);
            PrevResults = new DicWindowData<int, SingleSiteGnssResult>(10);
        }

        #endregion

        public XYZ LastValidXyzRms { get; set; }
        /// <summary>
        /// 成功的结果。
        /// </summary>
        public DicWindowData<int, SingleSiteGnssResult> PrevResults { get; set; }

        /// <summary>
        /// 当前执行编号
        /// </summary>
        public int CurrentIndex { get; set; }

        public override SingleSiteGnssResult CaculateKalmanFilter(EpochInformation epochInfo, SingleSiteGnssResult lastResult)
        {
            CurrentIndex++;

            var result = base.CaculateKalmanFilter(epochInfo, lastResult);

            if (CurrentProduct != null)
            {
                if (XYZ.IsValueValid(CurrentProduct.EstimatedXyzRms))
                {
                    LastValidXyzRms = CurrentProduct.EstimatedXyzRms;
                }
            }

            result = CheckOrRecaculate(epochInfo, result);

            if (!XYZ.IsValueValid(result.EstimatedXyzRms))
            {
                log.Error("RMS 检核未通过 " + result.EstimatedXyzRms);
                return null;
            }


            //if (LastValidXyzRms != null
            //    && CurrentIndex > 10 //先让其浪一会儿
            //    && (result.EstimatedXyzRms.Length > LastValidXyzRms.Length * 50))
            //{
            //    log.Error("RMS 检核未通过 " + result.EstimatedXyzRms);
            //    return null;
            //}
            //ok
            PrevResults.Add(CurrentIndex, result);

            return result;
        }
        /// <summary>
        /// 如果有效，则直接返回，如果无效则重置先验值重新计算。
        /// </summary>
        /// <param name="epochInfo"></param>
        /// <param name="result"></param>
        /// <returns></returns>
        private SingleSiteGnssResult CheckOrRecaculate(EpochInformation epochInfo, SingleSiteGnssResult result)
        {
            if (Geo.Algorithm.Vector.IsValid(result.ResultMatrix.StdOfEstimatedParam))
            {
                return result;
            }

            var lastOkIndex = PrevResults.LastKey;
            var differIndex = CurrentIndex - lastOkIndex;

            var newAprioriIndex = PrevResults.LastKey - differIndex;
            while (newAprioriIndex > 0)
            {
                if (PrevResults.Contains(newAprioriIndex))
                {
                    var product = PrevResults[newAprioriIndex];
                    this.SetProduct(product);

                    if (differIndex > 5)
                    {
                        //this.MatrixBuilder.IsResetCovaOfApriori = true; 
                    }

                    break;
                }
                newAprioriIndex--;
            }

            return base.CaculateKalmanFilter(epochInfo, this.CurrentProduct); 
        }
        /// <summary>
        /// 生成结果
        /// </summary>
        /// <returns></returns>
        public override SingleSiteGnssResult BuildResult()
        {
            return new SingleFreqIonoFreePppResult(this.CurrentMaterial, Adjustment, this.MatrixBuilder.GnssParamNameBuilder);
        }
    }

    public class DicWindowData<TKey, TValue> : BaseDictionary<TKey, TValue>
    {
        public DicWindowData(int WindowSize)
        {
            this.WindowSize = WindowSize;
        }
        /// <summary>
        /// 窗口大小
        /// </summary>
        public int WindowSize { get; set; }
    }

}
