﻿//2016.04.26 double edit in xi'an train station 修改精简程序

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Geo.Algorithm.Adjust;
using Geo.Algorithm;
using Geo.Utils;

namespace Gnsser
{
    /// <summary>
    /// 使用递推的哈达码方差的Kalman模型
    /// </summary>
    public class KalmanRecursionHardamard : BasicFunctionModel
    {
        /// <summary>
        /// 计算
        /// </summary>
        /// <param name="ModelData"></param>
        /// <param name="CompareData"></param>
        /// <param name="IntervalSeconds"></param>
        public void Calculate(ArrayMatrix ModelData, ArrayMatrix CompareData, int IntervalSeconds)
        {
            int ModelLength = ModelData.RowCount;
            int PredictedLength = CompareData.RowCount;
            ArrayMatrix InitialX = new ArrayMatrix(3, 1);//参数解算初值
            ArrayMatrix HardamardDevariation = GetHardamardDevariation(IntervalSeconds, ModelLength, ModelData);//根据已知的钟差数据解算Allan方差

            double a = (ModelLength - 1) / 2.0 - 1;
            int a1 = (int)Math.Floor(a);

            ArrayMatrix TimeMatrix = GetTimeMatrix(ModelLength, IntervalSeconds);
            ArrayMatrix CovarianceByHardamardDevariation = GetCovarianceByHardamardDevariation(ModelLength, TimeMatrix, HardamardDevariation);
            double q0 = CovarianceByHardamardDevariation[0, 0]; double q1 = CovarianceByHardamardDevariation[1, 0]; double q2 = CovarianceByHardamardDevariation[2, 0]; double q3 = CovarianceByHardamardDevariation[3, 0];
            ArrayMatrix TransformMatrix = GetTransformMatrix(IntervalSeconds);
            ArrayMatrix InitialCovarianceW = GetInitialCovarianceW(q1, q2, q3, IntervalSeconds);
            ArrayMatrix CovarianceV = new ArrayMatrix(1, 1);
            CovarianceV[0, 0] = q0;

            ArrayMatrix A5 = GetA5(ModelLength, IntervalSeconds, ModelData);
            InitialX = (A5.Transpose() * A5).Inverse * (A5.Transpose() * ModelData);
            ArrayMatrix InitialCovarianceX = (A5 * A5.Transpose()).Inverse;
            ArrayMatrix X = GetKalmanFilterRecursion(ModelLength, InitialX, CovarianceV, InitialCovarianceX, InitialCovarianceW, ModelData, CovarianceByHardamardDevariation);

            ArrayMatrix PredictedData = new ArrayMatrix(PredictedLength, 1);//预报数据
            double x0 = X[0, 0]; double x1 = X[1, 0]; double x2 = X[2, 0];
            PredictedData = GetPredictedData(x0, x1, x2, IntervalSeconds, PredictedLength);
            GetPredictedError(CompareData, PredictedLength);

            PredictedRms = GetRMS(PredictedError, PredictedLength)* 1E9;
            Predicted3hRms = GetRMS(PredictedError, 3 * 3600 / IntervalSeconds) * 1E9;
            Predicted6hRms = GetRMS(PredictedError, 6 * 3600 / IntervalSeconds) * 1E9;
            Predicted12hRms = GetRMS(PredictedError, 12 * 3600 / IntervalSeconds) * 1E9;
            Predicted24hRms = GetRMS(PredictedError, 24 * 3600 / IntervalSeconds) * 1E9;
        }
        

    }
    
}

