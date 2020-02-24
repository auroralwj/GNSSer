//2018.10.24, czs, create in hmx, 非差轨道确定


using System;
using System.Collections.Generic;
using System.Linq;
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
using System.Threading.Tasks;

namespace Gnsser
{
    /// <summary>
    /// 非差轨道确定
    /// </summary>
    public class ZeroDifferOrbitMatrixBuilder : MultiSiteMatrixBuilder
    {
        /// <summary>
        /// 非差轨道确定 构造函数。
        /// </summary>
        /// <param name="option">解算选项</param>
        public ZeroDifferOrbitMatrixBuilder(
            GnssProcessOption option)
            : base(option)
        {
            this.ParamNameBuilder = new ZeroDifferOrbitParamNameBuilder(option);
        }


        #region 全局基础属性 
        /// <summary>
        /// 观测数量
        /// </summary>
        public override int ObsCount { get { return SiteSatCount * 2; } }
        /// <summary>
        /// 测站卫星射线数量
        /// </summary>
        public int SiteSatCount
        {
            get
            {
                int satCount = 0;
                foreach (var EpochInfo in CurrentMaterial)
                {
                    satCount += EpochInfo.EnabledSatCount;
                }
                return satCount;
            }
        }
        /// <summary>
        /// 测站数量
        /// </summary>
        public int SiteCount { get => this.CurrentMaterial.Count; }
        #endregion

        #region 创建观测信息

        /// <summary>
        /// 具有权值的观测值。
        /// </summary> 
        public override WeightedVector Observation
        {
            get
            {
                IMatrix inverseWeightOfObs = BulidInverseWeightOfObs();
                WeightedVector deltaObs = new WeightedVector(ObservationVector, inverseWeightOfObs);
                return deltaObs;
            }
        }

        /// <summary>
        /// 观测值。
        /// 自由项 l，观测值减去先验值或估计值。
        /// 常数项，观测误差方程的常数项,或称自由项
        /// </summary>
        public virtual IVector ObservationVector
        {
            get
            {
                var siteSatCount = this.SiteSatCount;
                Vector L = new Vector(siteSatCount * 2);
                int rangeRow = 0;
                foreach (var site in CurrentMaterial)//loop site
                {
                    Vector rangeVector = site.GetAdjustVector(SatObsDataType.IonoFreeRange);
                    Vector phaseVector = null;
                    if (this.Option.IsAliningPhaseWithRange)
                    {
                        phaseVector = site.GetAdjustVector(SatObsDataType.AlignedIonoFreePhaseRange, true);
                    }
                    else
                    {
                        phaseVector = site.GetAdjustVector(SatObsDataType.IonoFreePhaseRange, true);
                    }
                    int index = 0;
                    foreach (var sat in site.EnabledSats)//loop sat
                    {
                        var prn = sat.Prn;
                        int phaseRow = rangeRow + siteSatCount;
                        L[rangeRow] = rangeVector[index];
                        L[phaseRow] = phaseVector[index];

                        rangeRow++;
                        index++;
                    }
                }
                L.ParamNames = GetObsNames();
                return L;
            }
        }
       
        /// <summary>
        /// 观测量的权逆阵，一个对角阵。按照先伪距，后载波的顺序排列。
        /// 标准差由常数确定，载波比伪距标准差通常是 1:100，其方差比是 1:10000.
        /// 1.0/140*140=5.10e-5
        /// </summary>  
        /// <returns></returns>
        public IMatrix BulidInverseWeightOfObs()
        {
            int siteSatCount = this.SiteSatCount;
            DiagonalMatrix inverseWeight = new DiagonalMatrix(siteSatCount * 2);
            double[] inverseWeightVector = inverseWeight.Vector;
            double invFactorOfRange = 1;
            double invFactorOfPhase = Option.PhaseCovaProportionToRange;

            int rangeRow = 0;
            foreach (var site in this.CurrentMaterial)
            {
                foreach (var sat in site.EnabledSats)
                { 
                    int phaseRow = rangeRow + siteSatCount;
                    double inverseWeightOfSat = SatWeightProvider.GetInverseWeightOfRange(sat);
                    inverseWeight[rangeRow] = inverseWeightOfSat * invFactorOfRange;
                    inverseWeight[phaseRow] = inverseWeightOfSat * invFactorOfPhase;
                    rangeRow++;
                }
            }
            return inverseWeight;
        }
        /// <summary>
        /// 观测名称
        /// </summary>
        /// <returns></returns>
        public List<string> GetObsNames()
        {
            var names = new string[ObsCount];
            int rangeRow = 0;
            var siteSatCount = this.SiteSatCount;
            foreach (var site in CurrentMaterial)
            {
                var siteName = site.SiteName;
                foreach (var sat in site.EnabledSats)
                {
                    var prn = sat.Prn;
                    int phaseRow = rangeRow + siteSatCount;
                    names[rangeRow] = siteName + "_" + prn + "_P";
                    names[phaseRow] = siteName + "_" + prn + "_L";
                    rangeRow++;
                }

            }
            return new List<string>(names);
        }

        #endregion

        #region 系数矩阵生成 

        /// <summary>
        /// 参数平差系数阵 A。前一半行数为伪距观测量，后一半为载波观测量。构建为稀疏矩阵
        /// </summary>
        public override Matrix Coefficient
        {
            get
            {
                var paramNameBuilder = (ZeroDifferOrbitParamNameBuilder)this.ParamNameBuilder;

                var A = new Matrix(ObsCount, ParamCount);  
                if(ObsCount < ParamCount)
                {
                    log.Warn("小心：观测方程少于参数数量： " + ObsCount + " < " + ParamCount + ", " + this.CurrentMaterial);
                }

                int rangeRow = 0;//行的索引号，对应观测方程行
                int colIndex = 0;//列索引，对应参数编号
                //第一次为伪距，第二次为载波
                foreach (var site in CurrentMaterial)
                {
                    var siteName = site.SiteName;
                    //1.测站相关
                    //测站接收机钟差
                    var cdtr = paramNameBuilder.GetReceiverClockParamName(siteName);
                    int  colIndexOfCdtr = this.ParamNames.IndexOf(cdtr);
                    
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
                        var zpd = paramNameBuilder.GetSiteWetTropZpdName(site);
                        colIndex = this.ParamNames.IndexOf(zpd);
                        A[rangeRow, colIndex] = sat.WetMap;
                        A[phaseRow, colIndex] = sat.WetMap;

                        //载波相位模糊度，只有相位才有此参数
                        var ambi = paramNameBuilder.GetSiteSatAmbiguityParamName(sat);
                        colIndex = this.ParamNames.IndexOf(ambi);
                        A[phaseRow, colIndex] = 1;
                         
                       rangeRow++;
                    }
                }
                A.ColNames = ParamNames;
                A.RowNames = GetObsNames();
                return A;
            }
        }

        #endregion
    }
}
