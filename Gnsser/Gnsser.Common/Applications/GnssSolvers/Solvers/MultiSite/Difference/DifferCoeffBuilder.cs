//2018.11.02, czs, create in HMX, 双差网解定位
//2018.11.09, czs, create in HMX, 差分系数左乘线性变换矩阵构建器

using System;
using System.Collections.Generic;
using System.Text;
using Geo.Algorithm; 
using Geo.Utils;
using Geo.Common;
using Geo.Coordinates; 
using Geo.Algorithm.Adjust;
using Geo.Times;
using Gnsser;
using Gnsser.Times;
using Gnsser.Service;
using Gnsser.Domain;
using Gnsser.Checkers;
using Gnsser.Models;
using Gnsser.Data.Rinex;

namespace Gnsser.Service
{

    /// <summary>
    /// 差分系数左乘线性变换矩阵构建器。
    /// </summary>
    public class DifferCoeffBuilder
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="CurrentMaterial"></param>
        /// <param name="BaseSiteName"></param>
        /// <param name="CurrentBasePrn"></param>
        public DifferCoeffBuilder(MultiSiteEpochInfo CurrentMaterial, string BaseSiteName, SatelliteNumber CurrentBasePrn, bool isSingleObs=false)
        {
            this.CurrentMaterial = CurrentMaterial;
            this.CurrentBasePrn = CurrentBasePrn;
            this.isSingleObs = isSingleObs;
            this.BaseSiteName = BaseSiteName;
        }
        public bool isSingleObs { get; set; }
        /// <summary>
        /// 原料
        /// </summary>
        public MultiSiteEpochInfo CurrentMaterial { get; set; }
        /// <summary>
        /// 基准站名称
        /// </summary>
        public string BaseSiteName { get; set; }
        /// <summary>
        /// 当前基准星
        /// </summary>
        public SatelliteNumber CurrentBasePrn { get; set; }
        /// <summary>
        /// 站间差分左乘系数阵.
        /// 基准站将被差分掉。
        /// </summary>
        /// <returns></returns>
        public Matrix BuildDifferCoeefBetweenSites()
        {
            if (isSingleObs)
            {
                return BuildDifferCoeefBetweenSitesSingleObs(CurrentMaterial, BaseSiteName, CurrentBasePrn);
            }
            return BuildDifferCoeefBetweenSites(CurrentMaterial, BaseSiteName, CurrentBasePrn);
        }
        /// <summary>
        ///  在原始数据上构建星间差分左乘系数阵。参考星放在各站第一位置，参考站放在第一区块，各站卫星与该站的基准卫星作差分。
        /// </summary>
        /// <returns></returns>
        public Matrix BuildDifferCoeefOfBetweenSats()
        {
            if (isSingleObs)
            {
                return BuildDifferCoeefOfBetweenSatsSingleObs(CurrentMaterial, BaseSiteName, CurrentBasePrn);
            }
            return BuildDifferCoeefOfBetweenSats(CurrentMaterial, BaseSiteName, CurrentBasePrn);
        }
         


        #region  静态方法，载波only， 或单观测值
        /// <summary>
        /// 站间差分左乘系数阵.
        /// 基准站将被差分掉。
        /// </summary>
        /// <param name="CurrentMaterial"></param>
        /// <param name="BaseSiteName"></param>
        /// <param name="CurrentBasePrn">基准星， 非必需，若是在单差基础上做双差，则需要指定</param>
        /// <param name="isIgnoreBasPrn">若是在单差基础上做双差，则需要指定true</param>
        /// <returns></returns>
        protected static Matrix BuildDifferCoeefBetweenSitesSingleObs(MultiSiteEpochInfo CurrentMaterial,
            string BaseSiteName, SatelliteNumber CurrentBasePrn, bool isIgnoreBasPrn = true)
        {
            int siteCount = CurrentMaterial.Count;
            int satCount = CurrentMaterial.EnabledSatCount;
            var rangeRowCount = (satCount - 1) * (siteCount - 1);
            int rangeColCount = (satCount - 1) * (siteCount);//伪距双差数量
            Matrix doubleCoeff = new Matrix(rangeRowCount, rangeColCount);//单差系数阵
                                                                                  //第一个站也被差分掉啦    
            var rangeRow = 0;
            var siteIndex = 0;
            foreach (var site in CurrentMaterial)//所有观测值的权都要保留
            {
                var siteName = site.SiteName;
                if (String.Equals(siteName, BaseSiteName, StringComparison.CurrentCultureIgnoreCase)) { continue; }

                int satIndex = 0;
                foreach (var prn in CurrentMaterial.EnabledPrns)
                {
                    if (isIgnoreBasPrn && prn == CurrentBasePrn) { continue; }

                    var sat = site[prn]; 
                    int col = satIndex;
                    doubleCoeff[rangeRow, col] = -1;        //参考星
                    col = rangeColCount + satIndex; 

                    col = (siteIndex + 1) * (satCount - 1) + satIndex;
                    doubleCoeff[rangeRow, col] = 1;        //流动星  
                    rangeRow++;
                    satIndex++;
                }
                siteIndex++;
            }

            return doubleCoeff;
        }

        /// <summary>
        /// 在原始数据上构建星间差分左乘系数阵。参考星放在各站第一位置，参考站放在第一区块，各站卫星与该站的基准卫星作差分。
        /// </summary>
        /// <param name="CurrentMaterial"></param>
        /// <param name="BaseSiteName"></param>
        /// <param name="CurrentBasePrn"></param> 
        /// <returns></returns>
        protected static Matrix BuildDifferCoeefOfBetweenSatsSingleObs(MultiSiteEpochInfo CurrentMaterial,
            string BaseSiteName, SatelliteNumber CurrentBasePrn)
        {
            int siteCount = CurrentMaterial.Count;
            int satCount = CurrentMaterial.EnabledSatCount;

            EpochInformation baseSite = CurrentMaterial[BaseSiteName];
            var rangeRowCount = (satCount - 1) * siteCount; //单差所有站少一个基准星
            int rowCount = (satCount - 1) * siteCount ;
            int colCount = (satCount) * siteCount ;
            Matrix sigleCoeff = new Matrix(rowCount, colCount);
            //第一个站    
            var rangeRow = 0;
            foreach (var prn in CurrentMaterial.EnabledPrns)
            {
                if (prn == CurrentBasePrn) { continue; }

                var sat = baseSite[prn]; 
                int col = 0;
                sigleCoeff[rangeRow, col] = -1;        //参考星
                col = siteCount * satCount; 

                col = rangeRow + 1;//补偿基准星的位置+1
                sigleCoeff[rangeRow, col] = 1;        //流动星 
                rangeRow++;
            }
            //其它站
            var siteIndex = 1;
            foreach (var site in CurrentMaterial)//所有观测值的权都要保留
            {
                var siteName = site.SiteName;
                if (String.Equals(siteName, BaseSiteName, StringComparison.CurrentCultureIgnoreCase)) { continue; }

                int satIndex = 1; //0 给了参考星
                foreach (var sat in site.EnabledSats)
                {
                    if (sat.Prn == CurrentBasePrn) { continue; }

                    //第一颗星是参考星 
                    int col = siteIndex * satCount;
                    sigleCoeff[rangeRow, col] = -1;        //参考星
                    col = siteCount * satCount + col; 

                    col = siteIndex * satCount + satIndex; 
                    sigleCoeff[rangeRow, col] = 1;                   //流动星 

                    rangeRow++;
                    satIndex++;
                }
                siteIndex++;
            }
            return sigleCoeff;
        }

        #endregion





        #region  静态方法，载波加伪距
        /// <summary>
        /// 站间差分左乘系数阵.
        /// 基准站将被差分掉。
        /// </summary>
        /// <param name="CurrentMaterial"></param>
        /// <param name="BaseSiteName"></param>
        /// <param name="CurrentBasePrn">基准星， 非必需，若是在单差基础上做双差，则需要指定</param>
        /// <param name="isIgnoreBasPrn">若是在单差基础上做双差，则需要指定true</param>
        /// <returns></returns>
        protected static Matrix BuildDifferCoeefBetweenSites(MultiSiteEpochInfo CurrentMaterial,
            string BaseSiteName, SatelliteNumber CurrentBasePrn, bool isIgnoreBasPrn = true)
        {
            int siteCount = CurrentMaterial.Count;
            int satCount = CurrentMaterial.EnabledSatCount;
            var rangeRowCount = (satCount - 1) * (siteCount - 1);
            int rangeColCount = (satCount - 1) * (siteCount);//伪距双差数量
            Matrix doubleCoeff = new Matrix(rangeRowCount * 2, rangeColCount * 2);//单差系数阵
                                                                                  //第一个站也被差分掉啦    
            var rangeRow = 0;
            var siteIndex = 0;
            foreach (var site in CurrentMaterial)//所有观测值的权都要保留
            {
                var siteName = site.SiteName;
                if (String.Equals(siteName, BaseSiteName, StringComparison.CurrentCultureIgnoreCase)) { continue; }

                int satIndex = 0;
                foreach (var prn in CurrentMaterial.EnabledPrns)
                {
                    if (isIgnoreBasPrn && prn == CurrentBasePrn) { continue; }

                    var sat = site[prn];
                    var phaseRow = rangeRow + rangeRowCount;
                    int col = satIndex;
                    doubleCoeff[rangeRow, col] = -1;        //参考星
                    col = rangeColCount + satIndex;
                    doubleCoeff[phaseRow, col] = -1;         //参考星

                    col = (siteIndex + 1) * (satCount - 1) + satIndex;
                    doubleCoeff[rangeRow, col] = 1;        //流动星
                    col = rangeColCount + (siteIndex + 1) * (satCount - 1) + satIndex;
                    doubleCoeff[phaseRow, col] = 1;        //流动星
                    rangeRow++;
                    satIndex++;
                }
                siteIndex++;
            }

            return doubleCoeff;
        }

        /// <summary>
        /// 在原始数据上构建星间差分左乘系数阵。参考星放在各站第一位置，参考站放在第一区块，各站卫星与该站的基准卫星作差分。
        /// </summary>
        /// <param name="CurrentMaterial"></param>
        /// <param name="BaseSiteName"></param>
        /// <param name="CurrentBasePrn"></param> 
        /// <returns></returns>
        protected static Matrix BuildDifferCoeefOfBetweenSats(MultiSiteEpochInfo CurrentMaterial,
            string BaseSiteName, SatelliteNumber CurrentBasePrn)
        {
            int siteCount = CurrentMaterial.Count;
            int satCount = CurrentMaterial.EnabledSatCount;

            EpochInformation baseSite = CurrentMaterial[BaseSiteName];
            var rangeRowCount = (satCount - 1) * siteCount; //单差所有站少一个基准星
            int rowCount = (satCount - 1) * siteCount * 2;
            int colCount = (satCount) * siteCount * 2;
            Matrix sigleCoeff = new Matrix(rowCount, colCount);
            //第一个站    
            var rangeRow = 0;
            foreach (var prn in CurrentMaterial.EnabledPrns)
            {
                if (prn == CurrentBasePrn) { continue; }

                var sat = baseSite[prn];
                var phaseRow = rangeRow + rangeRowCount;
                int col = 0;
                sigleCoeff[rangeRow, col] = -1;        //参考星
                col = siteCount * satCount;
                sigleCoeff[phaseRow, col] = -1;         //参考星

                col = rangeRow + 1;//补偿基准星的位置+1
                sigleCoeff[rangeRow, col] = 1;        //流动星
                col = siteCount * satCount + col;
                sigleCoeff[phaseRow, col] = 1;       //流动星
                rangeRow++;
            }
            //其它站
            var siteIndex = 1;
            foreach (var site in CurrentMaterial)//所有观测值的权都要保留
            {
                var siteName = site.SiteName;
                if (String.Equals(siteName, BaseSiteName, StringComparison.CurrentCultureIgnoreCase)) { continue; }

                int satIndex = 1; //0 给了参考星
                foreach (var sat in site.EnabledSats)
                {
                    if (sat.Prn == CurrentBasePrn) { continue; }

                    //第一颗星是参考星
                    var phaseRow = rangeRow + rangeRowCount;
                    int col = siteIndex * satCount;
                    sigleCoeff[rangeRow, col] = -1;        //参考星
                    col = siteCount * satCount + col;
                    sigleCoeff[phaseRow, col] = -1;        //参考星

                    col = siteIndex * satCount + satIndex;
                    phaseRow = rangeRow + rangeRowCount;
                    sigleCoeff[rangeRow, col] = 1;                   //流动星
                    col = siteCount * satCount + col;
                    sigleCoeff[phaseRow, col] = 1;        //流动星

                    rangeRow++;
                    satIndex++;
                }
                siteIndex++;
            }
            return sigleCoeff;
        }

        #endregion

    }
}