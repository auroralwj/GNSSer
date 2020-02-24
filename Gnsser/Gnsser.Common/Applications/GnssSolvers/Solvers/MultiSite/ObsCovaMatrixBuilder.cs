//2018.12.23, czs, create in ryd, 单独成文件类

using System;
using System.Collections.Generic;
using System.Text;
using Geo.Algorithm;
using Gnsser.Domain;
using Gnsser.Service;
using Gnsser.Data.Rinex;
using Gnsser.Checkers;
using Geo.Utils; 
using Geo.Coordinates;
using Geo.Algorithm.Adjust;
using Geo.Times;

namespace Gnsser
{
    /// <summary>
    /// 基础的定位矩阵生成器
    /// </summary>
    public  class ObsCovaMatrixBuilder
    {
        public ObsCovaMatrixBuilder(ISatWeightProvider SatWeightProvider)
        {
            this.SatWeightProvider = SatWeightProvider;
        }
      

        /// <summary>
        /// 卫星定权
        /// </summary>
        public ISatWeightProvider SatWeightProvider { get; set; }

        /// <summary>
        /// 多历元多站双差
        /// </summary>
        /// <param name="periodMaterial"></param>
        /// <param name="BaseSiteName"></param>
        /// <param name="CurrentBasePrn"></param>
        /// <param name="invFactorOfPhase"></param>
        /// <param name="phaseOnly"></param>
        /// <param name="isDualFreq"></param>
        /// <returns></returns>
        public Matrix BulidDoubleInverseWeightOfObs(MultiSitePeriodInfo periodMaterial,
         string BaseSiteName, SatelliteNumber CurrentBasePrn,
          double invFactorOfPhase, bool phaseOnly, bool isDualFreq = false)
        {
            var SiteCount = periodMaterial[0].Count;
            var EpochCount = periodMaterial.Count;
            var EnabledSatCount = periodMaterial.EnabledSatCount;
            int satCount = EnabledSatCount;
            int epochObsCount =  (satCount-1);//一个历元对应的参数数量
            int totalObsCount = (SiteCount-1) * epochObsCount * (EpochCount); //原始非差观测数，only载波
            if (!phaseOnly)//包含载波，翻翻
            {
                totalObsCount *= 2;
                epochObsCount *= 2;
            }
            if (isDualFreq)//包含2个频率，翻翻
            {
                totalObsCount *= 2;
                epochObsCount *= 2;
            }
            Matrix result = new Matrix(totalObsCount);
            for (int i = 0; i < EpochCount; i++)
            {
                var epochMaterial = periodMaterial[i];
                var subDiag = BulidDoubleInverseWeightOfObs(epochMaterial, BaseSiteName, CurrentBasePrn, invFactorOfPhase, phaseOnly, isDualFreq);
                var startRowCol = epochObsCount * i;
                result.SetSub(subDiag, startRowCol, startRowCol);
            }
            return result; 
        }

        /// <summary>
        /// 构建双差观测矩阵协方差
        /// </summary>
        /// <param name="epochMaterial"></param>
        /// <param name="CurrentBasePrn"></param>
        /// <param name="BaseSiteName"></param>
        /// <param name="invFactorOfPhase"></param>
        /// <param name="phaseOnly">是否只用载波，否则载波加伪距</param>
        /// <param name="isDualFreq">是否双频</param>
        /// <returns></returns>
        public Matrix BulidDoubleInverseWeightOfObs(MultiSiteEpochInfo epochMaterial,
           string BaseSiteName, SatelliteNumber CurrentBasePrn,  
            double invFactorOfPhase, bool phaseOnly, bool isDualFreq = false)
        {
            DiagonalMatrix primevalCova = null;
            if (phaseOnly)
            {
                primevalCova = BuildPrmevalPhaseOnlyObsCovaMatrix(epochMaterial,BaseSiteName, CurrentBasePrn,  invFactorOfPhase);
            }
            else
            {
                primevalCova = BuildPrmevalRangeAndPhaseObsCovaMatrix(epochMaterial, BaseSiteName, CurrentBasePrn, invFactorOfPhase);
            }

            Matrix undiffInverseWeigth = new Matrix(primevalCova);
            DifferCoeffBuilder differCoeffBuilder = new DifferCoeffBuilder(epochMaterial, BaseSiteName, CurrentBasePrn, phaseOnly);
            Matrix sigleCoeff = differCoeffBuilder.BuildDifferCoeefOfBetweenSats(); //构建星间单差系数阵的转换矩阵
            //单差协因数阵
            var singleDifferCova = sigleCoeff * undiffInverseWeigth * sigleCoeff.Transposition;

            //组建双差系数阵
            Matrix doubleCoeff = differCoeffBuilder.BuildDifferCoeefBetweenSites();

            //误差传播定律
            var doubleDifferCova = doubleCoeff * singleDifferCova * doubleCoeff.Transposition;

            if (isDualFreq)
            {
                //二倍而已
                var ObsCount = doubleDifferCova.RowCount * 2;
                Matrix result = new Matrix(ObsCount, ObsCount);
                result.SetSub(doubleDifferCova);
                result.SetSub(doubleDifferCova, doubleDifferCova.RowCount, doubleDifferCova.ColCount);

                return result;
            }
            else
            {
                return doubleDifferCova;
            }
        }
        #region  只是载波

        /// <summary>
        /// 生成多测站单历元原始纯载波的观测权逆阵，将基准站放在第一，基准星放在第一。
        /// 注意：此处没有伪距观测值。
        /// </summary>
        protected DiagonalMatrix BuildPrmevalPhaseOnlyObsCovaMatrix(MultiSiteEpochInfo epochMaterial,           
            string BaseSiteName, SatelliteNumber CurrentBasePrn,
            double invFactorOfPhase)
        {
            //invFactorOfPhase = 1;//只有单类型观测量

            int siteCount = epochMaterial.Count;
            int satCount = epochMaterial.EnabledSatCount;
            int totalObsCount = siteCount * satCount; //原始非差观测数，伪距加载波

            var enabledPrns = epochMaterial.EnabledPrns;
            //首先建立非差观测值的权逆阵，这里前第一个是ref站，每个站的第一星是ref星，与观测值保持一致
            DiagonalMatrix primevalCova = new DiagonalMatrix(totalObsCount); 
            siteCount = epochMaterial.Count;
            int row = 0;
            EpochInformation baseSite = epochMaterial[BaseSiteName];
            //第一颗星
            var refSat = baseSite[CurrentBasePrn];
            double inverseWeightOfSat = SatWeightProvider.GetInverseWeightOfRange(refSat);
            primevalCova[row, row] = inverseWeightOfSat * invFactorOfPhase;
            row++;
            //第一个站
            foreach (var prn in enabledPrns)
            {
                if (prn == CurrentBasePrn) { continue; }
                var sat = baseSite[prn];
                inverseWeightOfSat = SatWeightProvider.GetInverseWeightOfRange(sat);
                primevalCova[row, row] = inverseWeightOfSat * invFactorOfPhase;
                row++;
            }
            //其它站
            foreach (var site in epochMaterial)//所有观测值的权都要保留
            {
                var siteName = site.SiteName;
                if (String.Equals(siteName, BaseSiteName, StringComparison.CurrentCultureIgnoreCase)) { continue; }
                //第一颗星
                var baseSat = baseSite[CurrentBasePrn];
                inverseWeightOfSat = SatWeightProvider.GetInverseWeightOfRange(baseSat);
                primevalCova[row, row] = inverseWeightOfSat * invFactorOfPhase;
                row++;

                foreach (var sat in site.EnabledSats)
                {
                    if (sat.Prn == CurrentBasePrn) { continue; }

                    inverseWeightOfSat = SatWeightProvider.GetInverseWeightOfRange(sat);
                    primevalCova[row, row] = inverseWeightOfSat * invFactorOfPhase;
                    row++;
                }
            }
            return primevalCova;
        }
        #endregion

        #region 载波加伪距




        /// <summary>
        /// 生成多测站单历元原始纯载波的观测权逆阵，将基准站放在第一，基准星放在第一。
        /// 注意：伪距观测值排在前面，载波在后面。
        /// </summary>
        public DiagonalMatrix BuildPrmevalRangeAndPhaseObsCovaMatrix(MultiSiteEpochInfo epochMaterial,  string BaseSiteName,
            SatelliteNumber CurrentBasePrn,   double invFactorOfPhase  )
        {
            var enabledPrns = epochMaterial.EnabledPrns;
            int siteCount = epochMaterial.Count; 
            int satCount = epochMaterial.EnabledSatCount;
            int totalObsCount = siteCount * satCount * 2; //原始非差观测数，伪距加载波 

            //首先建立非差观测值的权逆阵，这里前第一个是ref站，每个站的第一星是ref星，与观测值保持一致
            DiagonalMatrix primevalCova = new DiagonalMatrix(totalObsCount);  
            int rangeRowCount = totalObsCount / 2;
            int rangeRow = 0;
            int phaseRow = rangeRow + rangeRowCount;
            EpochInformation baseSite = epochMaterial[BaseSiteName];
            //第一颗星
            var refSat = baseSite[CurrentBasePrn];
            double inverseWeightOfSat = SatWeightProvider.GetInverseWeightOfRange(refSat);
            primevalCova[rangeRow, rangeRow] = inverseWeightOfSat;
            primevalCova[phaseRow, phaseRow] = inverseWeightOfSat * invFactorOfPhase;
            rangeRow++;
            //第一个站
            foreach (var prn in enabledPrns)
            {
                if (prn == CurrentBasePrn) { continue; }
                var sat = baseSite[prn];
                phaseRow = rangeRow + rangeRowCount;
                inverseWeightOfSat = SatWeightProvider.GetInverseWeightOfRange(sat);
                primevalCova[rangeRow, rangeRow] = inverseWeightOfSat;
                primevalCova[phaseRow, phaseRow] = inverseWeightOfSat * invFactorOfPhase;
                rangeRow++;
            }
            //其它站
            foreach (var site in epochMaterial)//所有观测值的权都要保留
            {
                var siteName = site.SiteName;
                if (String.Equals(siteName, BaseSiteName, StringComparison.CurrentCultureIgnoreCase)) { continue; }
                //第一颗星
                var baseSat = baseSite[CurrentBasePrn];
                inverseWeightOfSat = SatWeightProvider.GetInverseWeightOfRange(baseSat);
                phaseRow = rangeRow + rangeRowCount;
                primevalCova[rangeRow, rangeRow] = inverseWeightOfSat;
                primevalCova[phaseRow, phaseRow] = inverseWeightOfSat * invFactorOfPhase;
                rangeRow++;

                foreach (var sat in site.EnabledSats)
                {
                    if (sat.Prn == CurrentBasePrn) { continue; }

                    phaseRow = rangeRow + rangeRowCount;
                    inverseWeightOfSat = SatWeightProvider.GetInverseWeightOfRange(sat);
                    primevalCova[rangeRow, rangeRow] = inverseWeightOfSat;
                    primevalCova[phaseRow, phaseRow] = inverseWeightOfSat * invFactorOfPhase;
                    rangeRow++;
                }
            }
            return primevalCova;
        }
        #endregion
    }
}