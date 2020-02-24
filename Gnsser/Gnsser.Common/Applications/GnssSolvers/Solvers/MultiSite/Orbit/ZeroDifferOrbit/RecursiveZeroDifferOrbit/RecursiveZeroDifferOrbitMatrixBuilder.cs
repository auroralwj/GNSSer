//2018.10.26, czs, create in hmx, 递归算法实现

using System;
using System.Collections.Generic;
using System.Text;
using Geo.Algorithm;
using Geo.Utils;
using Geo.Common;
using Geo.Coordinates;
using Geo.Algorithm.Adjust;
using Gnsser.Times;
using Gnsser.Domain;
using Gnsser.Service;
using Gnsser.Data.Rinex;
using Gnsser.Checkers;
using Gnsser.Models;
using Geo.Times;
using Geo;

namespace Gnsser.Service
{
    /// <summary>
    /// 矩阵生成类。
    /// </summary>
    public class RecursiveZeroDifferOrbitMatrixBuilder : ZeroDifferOrbitMatrixBuilder
    {
        /// <summary>
        /// 构造函数。
        /// </summary> 
        /// <param name="option">解算选项</param> 
        public RecursiveZeroDifferOrbitMatrixBuilder(
             GnssProcessOption option)
            : base(option)
        {
            ParamNameBuilder = new RecursiveZeroDifferOrbitMutabletParamNameBuilder(option);//option中包含了几个系统      
            this.SecondParamNameBuilder = new RecursiveZeroDifferOrbitConstParamNameBuilder(option);
        }         

        #region 公共矩阵生成
        /// <summary>
        /// 固定参数数量,模糊度与对流层湿延迟天顶距。
        /// </summary>
        public int FixedParamCount { get => this.SecondParamNames.Count; }// SiteSatCount + SiteCount; }
        /// <summary>
        /// 易变参数数量
        /// </summary>
        public override int ParamCount => base.ParamCount;

        /// <summary>
        /// 参数平差系数阵 A。前一半行数为伪距观测量，后一半为载波观测量。构建为稀疏矩阵
        /// </summary>
        public override Matrix Coefficient
        {
            get
            {
                var paramNameBuilder = (RecursiveZeroDifferOrbitMutabletParamNameBuilder)this.ParamNameBuilder;               
                var A = new Matrix(ObsCount, ParamCount);

                int rangeRow = 0;//行的索引号，对应观测方程行
                int colIndex = 0;//列索引，对应参数编号
                //第一次为伪距，第二次为载波
                foreach (var site in CurrentMaterial)
                {
                    var siteName = site.SiteName;
                    //1.测站相关
                    //测站接收机钟差
                    var cdtr = paramNameBuilder.GetReceiverClockParamName(siteName);
                    int colIndexOfCdtr = this.ParamNames.IndexOf(cdtr);

                    //2.卫星相关，或站星相关
                    foreach (var sat in site.EnabledSats)
                    {
                        int phaseRow = rangeRow + SiteSatCount;
                        var prn = sat.Prn;
                        var vector = sat.EstmatedVector;
                        //测站接收机钟差
                        A[rangeRow, colIndexOfCdtr] = 1;
                        A[phaseRow, colIndexOfCdtr] = 1;

                        //卫星坐标
                        var satXyzNames = paramNameBuilder.GetSatDxyz(prn);
                        colIndex = this.ParamNames.IndexOf(satXyzNames[0]);
                        A[rangeRow, colIndex + 0] = vector.CosX;
                        A[rangeRow, colIndex + 1] = vector.CosY;
                        A[rangeRow, colIndex + 2] = vector.CosZ;
                        A[phaseRow, colIndex + 0] = A[rangeRow, colIndex + 0];
                        A[phaseRow, colIndex + 1] = A[rangeRow, colIndex + 1];
                        A[phaseRow, colIndex + 2] = A[rangeRow, colIndex + 2];

                        //卫星钟差
                        var cdts = paramNameBuilder.GetSatClockParamName(prn);
                        colIndex = this.ParamNames.IndexOf(cdts);
                        A[rangeRow, colIndex] = -1;
                        A[phaseRow, colIndex] = -1;

                        //对流层湿延迟天顶距
                        //var zpd = paramNameBuilder.GetReceiverWetTropParamName(site);
                        //colIndex = this.ParamNames.IndexOf(zpd);
                        //A[rangeRow, colIndex] = sat.WetMap;
                        //A[phaseRow, colIndex] = sat.WetMap;

                        ////载波相位模糊度，只有相位才有此参数
                        //var ambi = paramNameBuilder.GetSiteSatAmbiguityParamName(sat);
                        //colIndex = this.ParamNames.IndexOf(ambi);
                        //A[phaseRow, colIndex] = 1;

                        rangeRow++;
                    }
                }
                A.ColNames = ParamNames;
                A.RowNames = GetObsNames();
                return A;
            }
        }


        /// <summary>
        /// 固定参数（如模糊度）平差系数阵 B 。模糊度参数。
        /// 随着卫星数量的改变，参数将改变。
        /// </summary> 
        /// <returns></returns>
        public override Matrix SecondCoefficient
        {
            get
            {
                var paramNameBuilder = (RecursiveZeroDifferOrbitConstParamNameBuilder)this.SecondParamNameBuilder;
                var A = new Matrix(ObsCount, FixedParamCount);
                

                int rangeRow = 0;//行的索引号，对应观测方程行
                int colIndex = 0;//列索引，对应参数编号
                //第一次为伪距，第二次为载波
                foreach (var site in CurrentMaterial)
                {
                    var siteName = site.SiteName;

                    //2.卫星相关，或站星相关
                    foreach (var sat in site.EnabledSats)
                    {
                        int phaseRow = rangeRow + SiteSatCount;
                        var prn = sat.Prn;
                        var vector = sat.EstmatedVector;

                        //对流层湿延迟天顶距
                        var zpd = paramNameBuilder.GetSiteWetTropZpdName(site);
                        colIndex = this.SecondParamNames.IndexOf(zpd);
                        A[rangeRow, colIndex] = sat.WetMap;
                        A[phaseRow, colIndex] = sat.WetMap;

                        //载波相位模糊度，只有相位才有此参数
                        var ambi = paramNameBuilder.GetSiteSatAmbiguityParamName(sat);
                        colIndex = this.SecondParamNames.IndexOf(ambi);
                        A[phaseRow, colIndex] = 1;

                        rangeRow++;
                    }
                }
                A.ColNames = SecondParamNames;
                A.RowNames = GetObsNames();
                return A;
            }
        }
        #endregion


    }//end class
}