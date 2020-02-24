//2018.11.02, czs, create in HMX, 双差网解定位


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
using Gnsser.Filter;
using Gnsser.Checkers;
using Geo.Common;
using Geo;


namespace Gnsser.Service
{
    /// <summary>
    /// 双差网解定位。具有模糊度。。
    /// </summary>
    public class NetDoubleDifferPositionResult : MultiSiteGnssResult, IFixableParamResult,IWithEstimatedBaselines
    {

        /// <summary>
        /// 双差网解定位。具有模糊度。
        /// </summary>
        /// <param name="mInfo">历元观测信息</param>
        /// <param name="Adjustment"></param>
        /// <param name="positioner"></param>
        /// <param name="baseSatPrn"></param>
        /// <param name="baseParamCount"></param>
        public NetDoubleDifferPositionResult(
            MultiSiteEpochInfo mInfo,
            AdjustResultMatrix Adjustment,
             GnssParamNameBuilder positioner,
            SatelliteNumber baseSatPrn,
            int baseParamCount = 5
            )
            : base(mInfo, Adjustment, positioner)
        {
            //设置第一个流动站坐标改正数
            int i = 0;
            var estmated = this.ResultMatrix.Estimated;
            foreach (var paramName in estmated.ParamNames)
            {
                if (paramName.Contains(Gnsser.ParamNames.Dx))
                {
                    double x = estmated[i];
                    double y = estmated[i+1];
                    double z = estmated[i+2];
                    this.XyzCorrection = new XYZ(x, y, z);
                    break;
                }
            }
        }
        BaseLineNet  EstimatedBaselines;
        /// <summary>
        /// 提取基线结果
        /// </summary>
        /// <returns></returns>
        public BaseLineNet  GetEstimatedBaselines()
        {
            if (EstimatedBaselines != null) { return EstimatedBaselines; }

            var baseLineResult = new BaseLineNet ();
            var nameBuider = (NetDoubleDifferPositionParamNameBuilder)this.NameBuilder;
            var baseSiteName = this.MaterialObj.BaseSiteName;
            var refSite = this.MaterialObj.BaseEpochInfo;
            foreach (var site in this.MaterialObj)
            {
                var siteName = site.SiteName;
                if(String.Equals(siteName, baseSiteName, StringComparison.CurrentCultureIgnoreCase)) { continue; }

                RmsedXYZ rmsedXYZ = ExtractBaseline(nameBuider, siteName);
                var cova = ExtractCovaMatrix(nameBuider, siteName);
                baseLineResult.Add( new EstimatedBaseline(refSite, site, rmsedXYZ, cova, this.ResultMatrix.StdDev) {
                    ResultType = this.ResultMatrix .ResultType,
                    GnssSolverType = this.NameBuilder.Option.GnssSolverType
                });
            }
            EstimatedBaselines = baseLineResult;

            return baseLineResult;
        }
        /// <summary>
        /// 提取基线协方差
        /// </summary>
        /// <param name="nameBuider"></param>
        /// <param name="siteName"></param>
        /// <returns></returns>
        public Matrix ExtractCovaMatrix(NetDoubleDifferPositionParamNameBuilder nameBuider, string siteName)
        {
            var total = this.ResultMatrix.CovaOfEstimatedParam;
            var siteXName = nameBuider.GetSiteDx(siteName);

            var indexOfX = this.ResultMatrix.ParamNames.IndexOf(siteXName);
            Matrix cova = new Matrix(new SymmetricMatrix(3));
            cova[0, 0] = total[indexOfX, indexOfX];
            cova[0, 1] = total[indexOfX, indexOfX + 1];
            cova[0, 2] = total[indexOfX, indexOfX + 2];
            cova[1, 1] = total[indexOfX + 1, indexOfX + 1];
            cova[1, 2] = total[indexOfX + 1, indexOfX + 2];
            cova[2, 2] = total[indexOfX + 2, indexOfX + 2];
            return cova;
        }


        /// <summary>
        /// 提取基线
        /// </summary>
        /// <param name="nameBuider"></param>
        /// <param name="siteName"></param>
        /// <returns></returns>
        private RmsedXYZ ExtractBaseline(NetDoubleDifferPositionParamNameBuilder nameBuider, string siteName)
        {
            var siteXyzNames = nameBuider.GetSiteDxyz(siteName);
            RmsedXYZ rmsedXYZ = new RmsedXYZ();
            int i = 0;
            foreach (var siteXyzName in siteXyzNames)
            {
                var rmsVal = this.ResultMatrix.Estimated.Get(siteXyzName);
                if (i == 0)
                {
                    rmsedXYZ.Value.X = rmsVal.Value;
                    rmsedXYZ.Rms.X = rmsVal.Rms;
                }
                if (i == 1)
                {
                    rmsedXYZ.Value.Y = rmsVal.Value;
                    rmsedXYZ.Rms.Y = rmsVal.Rms;
                }
                if (i == 2)
                {
                    rmsedXYZ.Value.Z = rmsVal.Value;
                    rmsedXYZ.Rms.Z = rmsVal.Rms;
                }
                i++;
            }
            return rmsedXYZ;
        }

        public WeightedVector FixedParams { get; set; }

        public WeightedVector GetFixableVectorInUnit()
        {
            this.ResultMatrix.Estimated.ParamNames = this.ResultMatrix.ParamNames;

            var ambiParamNames = new List<string>();
            foreach (var name in ResultMatrix.ParamNames)
            {
                //if (HasUnstablePrn(name)) { continue; }

                if (name.Contains(Gnsser.ParamNames.DoubleDifferAmbiguity))
                { ambiParamNames.Add(name); }
            }
            var estVector = this.ResultMatrix.Estimated.GetWeightedVector(ambiParamNames);

            return estVector;
        }

        /// <summary>
        /// 此将用于输出
        /// </summary>
        /// <returns></returns>
        public override string GetTabTitles()
        {
            BaseLineNet net = GetEstimatedBaselines();
            StringBuilder sb = new StringBuilder();
            sb.Append(net.First.GetTabTitles());
            return sb.ToString();
        }
        /// <summary>
        /// 此将用于输出
        /// </summary>
        /// <returns></returns>
        public override string GetTabValues()
        {
            StringBuilder sb = new StringBuilder();

            BaseLineNet net = GetEstimatedBaselines();
            foreach (var item in net)
            {

                sb.AppendLine(item.GetTabValues());
            }
            return sb.ToString();
        }
    } 

}
