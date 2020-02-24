//2018.06.04, czs, edit in hmx, 增加单站单星多历元GNSS计算

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
using Gnsser.Data;
using Geo;

namespace Gnsser.Service
{
    /// <summary>
    /// 电离层延迟变化量的计算
    /// </summary>
    public class IonoDeltaSolveMatrixBuilder : SingleSiteSinglePeriodSatGnssMatrixBuilder 
        //: SimpleBaseGnssMatrixBuilder<BaseSinglePeriodSatGnssResult, PeriodSatellite>
    {
        /// <summary>
        /// 先构造，再设置历元值。
        /// </summary>
        /// <param name="option">解算选项</param> 
        public IonoDeltaSolveMatrixBuilder(
            GnssProcessOption option)
            : base(option)
        {
            ParamNameBuilder = new IonoDeltaSolveParamNameBuilder(option);      
        }

        #region 全局基础属性 
        /// <summary>
        /// 选定的卫星
        /// </summary>
        public SatelliteNumber Prn { get; set; }
        /// <summary>
        /// 参数数量
        /// </summary>
        public override int ParamCount { get { return 2; } }


        /// <summary>
        /// 参数列表
        /// </summary>
        public override List<string> ParamNames { get { return ParamNameBuilder.Build(); } }
        #endregion

        public override void Build()
        {
            if(Prn  != this.CurrentMaterial.First.Prn)
            {
                log.Info("当前卫星已改变，由 " + Prn + " 变成 " + this.CurrentMaterial.First.Prn);
                this.Prn = this.CurrentMaterial.First.Prn;
            }
            base.Build();
        }

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
                Vector L = new Vector(CurrentMaterial.Count);
                var ms = this.CurrentMaterial;
                int i = 0;
                foreach (var m in ms)
                {

                    double P1 = m.FrequenceA.PseudoRange.Value;
                    double P2 = m.FrequenceB.PseudoRange.Value;
                    double L1 = m.FrequenceA.PhaseRange.Value;
                    double L2 = m.FrequenceB.PhaseRange.Value;

                    double dL = (L1 - L2);
                    double dP = (P2 - P1);

                    double B = dL - dP;
                    double l = P1 - L1;// + B;

                   L[i] = l; 
                    i++;
                }
                return L;
            }
        }

        /// <summary>
        /// 观测量的权逆阵，一个对角阵。按照先伪距，后载波的顺序排列。
        /// </summary> 
        /// <returns></returns>
        protected IMatrix BulidInverseWeightOfObs()
        {
            var inverseWeight = Matrix.CreateIdentity(this.CurrentMaterial.Count); 
            int i = 0;                                                                   
            foreach (var item in this.CurrentMaterial)
            {
                inverseWeight[i, i] = SatWeightProvider.GetInverseWeightOfRange(item);
                i++;
            }
            return (inverseWeight);
        }

        #endregion

        #region 公共矩阵生成

        /// <summary>
        /// 参数平差系数阵 A。前一半行数为伪距观测量，后一半为载波观测量。
        /// </summary> 
        /// <returns></returns>
        public override Matrix Coefficient
        {
            get
            {

                var first = this.CurrentMaterial.First.ReceiverTime;
               
                int rowCount = this.CurrentMaterial.Count;
                int colCount = 2;
                Matrix A = new Matrix(rowCount, colCount);
                int i = 0;
                foreach (var item in this.CurrentMaterial)
                {
                    A[i, 0] = 1;
                    A[i, 1] = 2 * (item.ReceiverTime - first);
                    i++;
                } 

                return (A);
            }
        }
         
        /// <summary>
        /// 只有四个参数且不变。
        /// </summary>
        public override bool IsParamsChanged { get { return false; } }

        /// <summary>
        /// 状态转移矩阵
        /// </summary>
        public override WeightedMatrix Transfer {
            get
            {

                Matrix matrix = Matrix.CreateIdentity(ParamCount);
                Matrix moise = Matrix.CreateIdentity(ParamCount) * 100;
                if (PreviousProduct != null)
                {

                    matrix[0, 0] = 100;
                    matrix[1, 1] = 100;

                    moise[0, 0] = 100;
                    moise[1, 1] = 100;//硬件延迟稳定
                }
                else
                {
                    matrix[0, 0] = 1;
                    matrix[1, 1] = 1;

                    moise[0, 0] = 1;
                    moise[1, 1] = 1;//硬件延迟稳定

                }

                WeightedMatrix weightedMatrix = new WeightedMatrix(matrix, moise);

                return weightedMatrix;
            }
            protected set => base.Transfer = value;
        }


        #endregion

    }//end class
}