// 2014.09.26, czs, create, 用于计算无多余数据源，无初始坐标，无多余改正的数据的“三无”数据。
//2017.09.04, kyc, edit in zz,  动态伪距定位
//2017.09.05, czs, edit in hongqing, 整理代码，动态伪距定位
//2018.05.29, czs, eit in HMX, 移除载波平滑伪距算法，统一放到改正数里面

using System;
using System.Collections.Generic;
using Gnsser.Domain;
using System.Text;
using Geo.Coordinates;
using Gnsser.Data.Rinex;
using Geo.Algorithm;
using Geo.Algorithm.Adjust;
using Gnsser.Times;
using Gnsser.Service;
using Geo.Utils;
using Gnsser.Checkers;
using Geo.Times;
using Geo.Algorithm;

namespace Gnsser.Service
{
    /// <summary>
    /// 伪距单点定位的矩阵生成器。适用于参数平差、卡尔曼滤波等。
    /// </summary>
    public class DynamicRangeMatrixBuilder : SingleSiteGnssMatrixBuilder
    {
        #region 构造函数
        /// <summary>
        /// 伪距单点定位矩阵生成器，构造函数。
        /// </summary>
        /// <param name="model">解算选项</param>
        public DynamicRangeMatrixBuilder(GnssProcessOption model)
            : base(model)
        {
            this.IonosphereBuilderManager = new IonophereBuilderManager(); 
        }

        #endregion

        #region 全局基础属性
        /// <summary>
        /// 电离层延迟计算
        /// </summary>
        public IonophereBuilderManager IonosphereBuilderManager { get; set; }
         
 
        /// <summary>
        /// 参数数量
        /// </summary>
        public override int ParamCount { get { return 8; } }//kyc
        /// <summary>
        /// 观测数量
        /// </summary>
        public override int ObsCount { get { return CurrentMaterial.EnabledSatCount; } }
        /// <summary>
        /// 参数列表
        /// </summary>
        public override List<string> ParamNames { get { return Gnsser.ParamNames.DxyzClkAndV; } }
        /// <summary>
        /// 名称
        /// </summary>
        /// <returns></returns>
        public override List<string> BuildParamNames() { return ParamNames; }
        #endregion


        #region 通用矩阵，误差方程系数阵，设计矩阵。

        /// <summary>
        /// 误差方程系数阵，设计矩阵。
        /// 有多少颗卫星就有多少个观测量，只有4个参数，X,Y,Z和接收机钟差等效距离偏差。
        /// 动态有8个参数,kyc
        /// </summary> 
        public override Matrix Coefficient
        {
            get
            {
                Matrix A = new Matrix(ObsCount, ParamCount);

                int satIndex = 0;
                foreach (var sat in CurrentMaterial.EnabledSats)
                {
                    XYZ satToStaVector = sat.Ephemeris.XYZ - this.CurrentMaterial.SiteInfo.EstimatedXyz;

                    A[satIndex, 0] = -satToStaVector.CosX;
                    A[satIndex, 1] = -satToStaVector.CosY;
                    A[satIndex, 2] = -satToStaVector.CosZ;
                    A[satIndex, 3] = -1.0;////*** 接收机 钟差改正 in units of meters
                    A[satIndex, 4] = 0.0;//VX//kyc
                    A[satIndex, 5] = 0.0;//VY
                    A[satIndex, 6] = 0.0;//VZ
                    A[satIndex, 7] = 0.0;//钟漂

                    satIndex++;
                }
                return A;
            }
        }
        #endregion


        #region 参数先验值，先验值设置为 0 ，主要需要其权逆阵

        /// <summary>
        /// 先验参数
        /// </summary> 
        protected override WeightedVector CreateInitAprioriParam()
        {
            double[] residualVec = new double[ParamCount];//近似差为 0 
            double[][] inverseWeight = MatrixUtil.CreateIdentity(ParamCount);
            inverseWeight[3][3] = 100000.0;//钟差
            inverseWeight[7][7] = 5e-12;//kyc:钟漂,谱哥说给个小的值
            if (Option.IsIndicatingApproxXyzRms)
            {
                int i = 0;
                for (i = 0; i < 3; i++)
                {
                    var item = Option.InitApproxXyzRms[i];
                    inverseWeight[i][i] = item * item;
                }
                for (int j = 4; j < 7; j++)//kyc
                {
                    inverseWeight[j][j] = 10000;
                }
            }
            else if (CurrentMaterial.SiteInfo.EstimatedXyz.IsZero)
            {
                for (int i = 0; i < 3; i++) { inverseWeight[i][i] = 1e10; }
                for (int j = 4; j < 7; j++) { inverseWeight[j][j] = 1e10; }//kyc
            }

            else
            {
                for (int i = 0; i < 3; i++) { inverseWeight[i][i] = 1000; }
                for (int j = 4; j < 7; j++) { inverseWeight[j][j] = 1000; }//kyc
            }
            return new WeightedVector(residualVec, inverseWeight); 
        }

        #endregion

        #region 观测值，观测值残差


        /// <summary>
        /// 残差
        /// </summary>
        public override WeightedVector Observation
        {
            get
            {
                Vector obs = new Vector(this.ObsCount);
                Vector app = new Vector(this.ObsCount);
                int i = 0;
                foreach (var sat in this.CurrentMaterial.EnabledSats)
                {
                    //是否指定观测类型
                    var obsObj = sat.GetDataValue(Option.ObsDataType);
                    if (obsObj != null && obsObj.Value != 0)
                    {
                        obs[i] = obsObj.CorrectedValue;
                    }
                    else//最低定位条件，确保获取一个定位结果。
                    {
                        obs[i] = sat.AvailablePseudoRange.CorrectedValue; //近似值 
                    }
                    obs.ParamNames[i] = sat.Prn + "";

                    i++;
                }
                var vector = new AdjustVector(obs, app);

                var cova = BulidInverseWeightOfObs();

                return new WeightedVector(vector, cova);
            }
        }

        /// <summary>
        /// 观测量的权逆阵，一个对角阵。
        /// </summary> 
        /// <returns></returns>
        protected IMatrix BulidInverseWeightOfObs()
        {
            double[][] inverseWeight = Geo.Utils.MatrixUtil.Create(EnabledSatCount);

            double invFactorOfRange = 10;//

            int i = 0;
            foreach (var prn in CurrentMaterial.EnabledPrns)// 一颗卫星1行 
            {
                EpochSatellite e = this.CurrentMaterial[prn];
                double inverseWeightOfSat = 1;
                if (!e.SiteInfo.ApproxXyz.IsZero)//高度角有关，如果测站坐标为 0，则高度角计算错误。
                {
                    inverseWeightOfSat = SatWeightProvider.GetInverseWeightOfRange(e);
                } 

                inverseWeight[i][i] = inverseWeightOfSat * invFactorOfRange;

                i++;
            }
            return new ArrayMatrix(inverseWeight);
        }

        /// <summary>
        ///  自由项D，B0等等。则参数平差中，满足满足 l = L - (AX0 + D).
        ///  此处，FreeVector = (AX0 + D)。
        /// </summary>
        public override Vector FreeVector
        {
            get
            {
                Vector app = new Vector(this.ObsCount);
                int i = 0;
                foreach (var sat in this.CurrentMaterial.EnabledSats)
                {
                    var obsObj = sat.GetDataValue(Option.ApproxDataType);
                    if (obsObj != null && obsObj.Value != 0)
                    {
                        app[i] = obsObj.CorrectedValue;
                    }
                    else//最低定位条件，确保获取一个定位结果。
                    {
                        app[i] = sat.AvailableApproxPseudoRange.CorrectedValue; //近似值 
                    }
                    app.ParamNames[i] = sat.Prn + "";

                    i++;
                }
                return app;
            }
        }
        #endregion

        /// <summary>
        /// kyc:为动态定位改转移矩阵及相应权
        /// </summary>
        public override WeightedMatrix Transfer
        {
            get
            {
                var epoch = this.CurrentMaterial;
                double delta_t = epoch.ObsInfo.Interval;
                double[][] trans = CreateTrans(delta_t);

                double[][] model = MatrixUtil.CreateIdentity(ParamCount);
                for (int i = 0; i < 8; i++)
                {
                    var item = this.ParamStateTransferModelManager.GetOrCreate(ParamNames[i]);
                    item.Init(this.CurrentMaterial);
                }
                //kyc: 照着杨元喜书上改的协方差矩阵
                double[] a = new double[3] { 1e10, 1e10, 1e10 };//前后历元相关长度的倒数,越大说明相关性越小
                DiagonalMatrix S22 = new DiagonalMatrix(3);
                DiagonalMatrix S23 = new DiagonalMatrix(3);
                DiagonalMatrix S33 = new DiagonalMatrix(3);
                DiagonalMatrix q1 = new DiagonalMatrix(3);//位置和钟差参数系统噪声的谱密度矩阵
                DiagonalMatrix q2 = new DiagonalMatrix(3);//速度和钟漂参数系统噪声的谱密度矩阵
                for (int i = 0; i < 3; i++)
                {
                    S22.Vector[i] = (-3 + 2 * a[i] * delta_t + 4 * Math.Exp((-a[i] * delta_t)) - Math.Exp((-2 * a[i] * delta_t))) / (2.0 * Math.Pow(a[i], 3));
                    S23.Vector[i] = (1 - 2 * Math.Exp((-a[i] * delta_t)) + Math.Exp((-2 * a[i] * delta_t))) / (2.0 * Math.Pow(a[i], 2));
                    S33.Vector[i] = (1 - Math.Exp((-2 * a[i] * delta_t))) / (2.0 * a[i]);

                }
                double spectralDensityOfLocation = 17;

                q1.Vector[0] = spectralDensityOfLocation;
                q1.Vector[1] = spectralDensityOfLocation;
                q1.Vector[2] = spectralDensityOfLocation;
                //q1.Vector[3] = 1e-9 * delta_t;//乱设的,钟差参数的谱密度是多少???
                double spectralDensityOfSpeed = 0.0001;
                q2.Vector[0] = spectralDensityOfSpeed;
                q2.Vector[1] = spectralDensityOfSpeed;
                q2.Vector[2] = spectralDensityOfSpeed;
                //q2.Vector[3] = 1e-4 * delta_t;//乱设的,钟漂参数的谱密度是多少???
                IMatrix W1 = S22.Multiply(q2).Plus(q1);
                IMatrix W2 = S23.Multiply(q2);
                IMatrix W3 = S33.Multiply(q2);
                for (int i = 0; i < 3; i++)
                {
                    model[i][i] = W1.Array[i][i];
                    model[i][i + 3] = W2.Array[i][i];
                }
                for (int j = 0; j < 3; j++)
                {
                    model[j + 4][j + 4] = W3.Array[j][j];
                    model[j + 4][j] = W2.Array[j][j];
                }
                model[3][3] = 1e8;//钟差过程噪声:平方米
                model[7][7] = 1e1;

                return new WeightedMatrix(trans, model) { ColNames = this.ParamNames, RowNames = this.ParamNames };
            }
        }

        private double[][] CreateTrans(double deltaT)
        {
            double[][] trans = MatrixUtil.CreateIdentity(ParamCount);
            //右上角的对角阵
            for (int j = 0; j < 4; j++)
            {
                trans[j][j + 4] = deltaT;
            }

            return trans;
        }
    }
}