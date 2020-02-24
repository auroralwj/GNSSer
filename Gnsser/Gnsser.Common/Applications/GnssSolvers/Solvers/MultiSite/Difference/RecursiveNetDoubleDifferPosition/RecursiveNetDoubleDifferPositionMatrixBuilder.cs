//2018.11.02, czs, create in HMX, 双差网解定位
//2018.11.07, czs, edit in hmx, 递归最小二乘双差网解定位

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
    /// 双差网解定位
    /// </summary>
    public class RecursiveNetDoubleDifferPositionMatrixBuilder : NetDoubleDifferPositionMatrixBuilder// BasePositionMatrixBuilder
    {
        /// <summary>
        /// 无电离层双差矩阵构造器
        /// </summary>
        /// <param name="option"></param>
        /// <param name="baseParamCount"></param>
        public RecursiveNetDoubleDifferPositionMatrixBuilder(
            GnssProcessOption option, int baseParamCount)
            : base(option, baseParamCount)
        {
            this.ParamNameBuilder = new RecursiveNetDoubleDifferPositionParamNameBuilder(option);
            this.SecondParamNameBuilder = new RecursiveNetDoubleDifferPositionSecondParamNameBuilder(option);
        } 
        
        
        #region 公共矩阵生成

        #region 系数矩阵生成 

        /// <summary>
        /// 参数平差系数阵 A。前一半行数为伪距观测量，后一半为载波观测量。构建为稀疏矩阵
        /// </summary>
        public override Matrix Coefficient
        {
            get
            {
                var paramNameBuilder = (RecursiveNetDoubleDifferPositionParamNameBuilder)this.ParamNameBuilder;

                var A = new Matrix(ObsCount, ParamCount);
                if (ObsCount < ParamCount)
                {
                    log.Warn("小心：观测方程少于参数数量： " + ObsCount + " < " + ParamCount + ", " + this.CurrentMaterial);
                }
                //相位模糊度系数，可以为米和周，按照设置文件决定。
                double coeefOfPhase = CheckAndGetCoeefOfPhase();

                int rangeRowCount = this.ObsCount / 2;
                int rangeRow = 0;//行的索引号，对应观测方程行
                int colIndex = 0;//列索引，对应参数编号
                                 //第一次为伪距，第二次为载波

                var baseSite = this.CurrentMaterial[BaseSiteName];
                var baseZpdname = paramNameBuilder.GetSiteWetTropZpdName(BaseSiteName);
                int colIndexOfBaseZpdname = this.ParamNames.IndexOf(baseZpdname); //基准站对流层位置相同，但是各颗卫星系数（映射函数）不同

                foreach (var site in CurrentMaterial)
                {
                    var siteName = site.SiteName;
                    if (siteName == BaseSiteName) { continue; }

                    //1.测站相关 
                    //基准站对流层
                    var name = paramNameBuilder.GetSiteWetTropZpdName(siteName);
                    int colIndexOfZpd = this.ParamNames.IndexOf(name);
                    var refSiteRefSat = baseSite[CurrentBasePrn];//基准站的当前星，用于获取对流层映射函数值

                    //当前站的基准卫星
                    var rovSiteRefSat = site[CurrentBasePrn];
                    var baseVector = rovSiteRefSat.EstmatedVector;

                    //2.卫星相关，或站星相关
                    foreach (var sat in site.EnabledSats)
                    {
                        var prn = sat.Prn;
                        if (prn == CurrentBasePrn) { continue; }

                        int phaseRow = rangeRow + rangeRowCount;
                        var vector = sat.EstmatedVector;

                        //测站坐标
                        var satXyzNames = paramNameBuilder.GetSiteDxyz(site.SiteName);
                        colIndex = this.ParamNames.IndexOf(satXyzNames[0]);
                        A[rangeRow, colIndex + 0] = -(vector.CosX - baseVector.CosX);
                        A[rangeRow, colIndex + 1] = -(vector.CosY - baseVector.CosY);
                        A[rangeRow, colIndex + 2] = -(vector.CosZ - baseVector.CosZ);
                        A[phaseRow, colIndex + 0] = A[rangeRow, colIndex + 0];
                        A[phaseRow, colIndex + 1] = A[rangeRow, colIndex + 1];
                        A[phaseRow, colIndex + 2] = A[rangeRow, colIndex + 2];

                        if (IsEstimateTropWetZpd)
                        {
                            //基准测站对流层湿延迟
                            var baseMapDiffer = baseSite[prn].WetMap - refSiteRefSat.WetMap;
                            A[rangeRow, colIndexOfBaseZpdname] = -baseMapDiffer;
                            A[phaseRow, colIndexOfBaseZpdname] = -baseMapDiffer;

                            //流动站对流层湿延迟天顶距
                            var zpdName = paramNameBuilder.GetSiteWetTropZpdName(site);
                            colIndex = this.ParamNames.IndexOf(zpdName);
                            var rovMapDiffer = sat.WetMap - rovSiteRefSat.WetMap;
                            A[rangeRow, colIndex] = rovMapDiffer;
                            A[phaseRow, colIndex] = rovMapDiffer;
                        }

                        ////载波相位模糊度，只有相位才有此参数
                        //var ambi = paramNameBuilder.GetDoubleDifferAmbiParamName(siteName, sat.Prn);
                        //colIndex = this.ParamNames.IndexOf(ambi);
                        //A[phaseRow, colIndex] = coeefOfPhase;

                        rangeRow++;
                    }
                }
                A.ColNames = ParamNames;
                A.RowNames = BuildObsNames();
                return A;
            }
        }

        /// <summary>
        /// 参数平差系数阵 A。前一半行数为伪距观测量，后一半为载波观测量。构建为稀疏矩阵
        /// </summary>
        public override Matrix SecondCoefficient
        {
            get
            {
                var paramNameBuilder = (RecursiveNetDoubleDifferPositionSecondParamNameBuilder)this.SecondParamNameBuilder;

                var A = new Matrix(ObsCount, SecondParamCount);
                if (ObsCount < SecondParamCount)
                {
                    log.Warn("小心：观测方程少于参数数量： " + ObsCount + " < " + SecondParamCount + ", " + this.CurrentMaterial);
                }
                //相位模糊度系数，可以为米和周，按照设置文件决定。
                double coeefOfPhase = CheckAndGetCoeefOfPhase();

                int rangeRowCount = this.ObsCount / 2;
                int rangeRow = 0;//行的索引号，对应观测方程行
                int colIndex = 0;//列索引，对应参数编号
                                 //第一次为伪距，第二次为载波

                var baseSite = this.CurrentMaterial[BaseSiteName];
                var baseZpdname = paramNameBuilder.GetSiteWetTropZpdName(BaseSiteName);
                int colIndexOfBaseZpdname = this.ParamNames.IndexOf(baseZpdname); //基准站对流层位置相同，但是各颗卫星系数（映射函数）不同

                foreach (var site in CurrentMaterial)
                {
                    var siteName = site.SiteName;
                    if (siteName == BaseSiteName) { continue; }

                    //1.测站相关 
                    //基准站对流层
                    var name = paramNameBuilder.GetSiteWetTropZpdName(siteName);
                    //int colIndexOfZpd = this.ParamNames.IndexOf(name);
                    //var refSiteRefSat = baseSite[CurrentBasePrn];//基准站的当前星，用于获取对流层映射函数值

                    //当前站的基准卫星
                    //var rovSiteRefSat = site[CurrentBasePrn];
                    //var baseVector = rovSiteRefSat.EstmatedVector;

                    //2.卫星相关，或站星相关
                    foreach (var sat in site.EnabledSats)
                    {
                        var prn = sat.Prn;
                        if (prn == CurrentBasePrn) { continue; }

                        int phaseRow = rangeRow + rangeRowCount;
                        var vector = sat.EstmatedVector;

                        //测站坐标
                        //var satXyzNames = paramNameBuilder.GetSiteDxyz(site.SiteName);
                        //colIndex = this.ParamNames.IndexOf(satXyzNames[0]);
                        //A[rangeRow, colIndex + 0] = -(vector.CosX - baseVector.CosX);
                        //A[rangeRow, colIndex + 1] = -(vector.CosY - baseVector.CosY);
                        //A[rangeRow, colIndex + 2] = -(vector.CosZ - baseVector.CosZ);
                        //A[phaseRow, colIndex + 0] = A[rangeRow, colIndex + 0];
                        //A[phaseRow, colIndex + 1] = A[rangeRow, colIndex + 1];
                        //A[phaseRow, colIndex + 2] = A[rangeRow, colIndex + 2];

                        //if (IsEstimateTropWetZpd)
                        //{
                        //    //基准测站对流层湿延迟
                        //    var baseMapDiffer = baseSite[prn].WetMap - refSiteRefSat.WetMap;
                        //    A[rangeRow, colIndexOfBaseZpdname] = -baseMapDiffer;
                        //    A[phaseRow, colIndexOfBaseZpdname] = -baseMapDiffer;

                        //    //流动站对流层湿延迟天顶距
                        //    var zpdName = paramNameBuilder.GetSiteWetTropZpdName(site);
                        //    colIndex = this.ParamNames.IndexOf(zpdName);
                        //    var rovMapDiffer = sat.WetMap - rovSiteRefSat.WetMap;
                        //    A[rangeRow, colIndex] = rovMapDiffer;
                        //    A[phaseRow, colIndex] = rovMapDiffer;
                        //}

                        //载波相位模糊度，只有相位才有此参数
                        var ambi = paramNameBuilder.GetDoubleDifferAmbiParamName(siteName, sat.Prn);
                        colIndex = this.SecondParamNames.IndexOf(ambi);
                        A[phaseRow, colIndex] = coeefOfPhase;

                        rangeRow++;
                    }
                }
                A.ColNames = SecondParamNames;
                A.RowNames = BuildObsNames();
                return A;
            }
        }


        #endregion

        #endregion
    }//end class
}
