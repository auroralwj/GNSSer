//2016.04.26 double edit in xi'an train station 修改精简程序
//2016.12.05 double edit in hongqing, 增加了基于一次差分的抗差灰色模型和基于修正一次差分的抗差灰色模型

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
    /// 抗差灰色模型
    /// </summary>
    public class RobustGreyModel : BasicFunctionModel
    {
        /// <summary>
        /// 计算
        /// </summary>
        /// <param name="ModelData"></param>
        /// <param name="CompareData"></param>
        /// <param name="IntervalSeconds"></param>
        public void Calculate(ArrayMatrix ModelData, ArrayMatrix CompareData,int IntervalSeconds)
        {
            int ModelLength = ModelData.RowCount;//设定所用建模数据长度
            int PredictedLength = CompareData.RowCount;//设定所用 

            ArrayMatrix GreyModelX = GetRobustGreyModelX(ModelData, PredictedLength);
            ArrayMatrix GreyModelPolyX = GetModelPolyX(GreyModelX, ModelLength);
            PolyError = GreyModelPolyX - ModelData;
            PolyRms = GetRMS(PolyError, ModelLength);
            PredictedData = GetModelPredictedX(GreyModelX, ModelLength);
            GetPredictedError(CompareData, PredictedLength);

            PredictedRms = GetRMS(PredictedError, PredictedLength) * 1E9;
            Predicted3hRms = GetRMS(PredictedError, 3 * 3600 / IntervalSeconds) * 1E9;
            Predicted6hRms = GetRMS(PredictedError, 6 * 3600 / IntervalSeconds) * 1E9;
            Predicted12hRms = GetRMS(PredictedError, 12 * 3600 / IntervalSeconds) * 1E9;
            Predicted24hRms = GetRMS(PredictedError, 24 * 3600 / IntervalSeconds) * 1E9;

        }
    }
    /// <summary>
    /// 基于一次差分的抗差灰色模型
    /// </summary>
    public class RobustDGreyModel : BasicFunctionModel
    {
        /// <summary>
        /// 计算
        /// </summary>
        /// <param name="ModelData"></param>
        /// <param name="CompareData"></param>
        /// <param name="IntervalSeconds"></param>
        public void Calculate(ArrayMatrix ModelData, ArrayMatrix CompareData,int IntervalSeconds)
        {
            int ModelLength = ModelData.RowCount - 1;//设定所用建模数据长度
            int PredictedLength = CompareData.RowCount;//设定所用

            ArrayMatrix modelData = new ArrayMatrix(ModelLength, 1);
            for (int i = 0; i < ModelLength; i++)
            {
                modelData[i, 0] = ModelData[i + 1, 0] - ModelData[i, 0];
            }

            ArrayMatrix RobustDGreyModelX = GetRobustGreyModelX(modelData, PredictedLength);
            ArrayMatrix RobustGreyModelX = new ArrayMatrix(ModelLength + PredictedLength + 1, 1);

            RobustGreyModelX[0, 0] = ModelData[0, 0];

            for (int i = 0; i < ModelLength + PredictedLength; i++)
            {
                RobustGreyModelX[i + 1, 0] = RobustGreyModelX[i, 0] + RobustDGreyModelX[i, 0];
            }

            ArrayMatrix PolyX = GetModelPolyX(RobustGreyModelX, ModelLength + 1);
            PolyError = PolyX - ModelData;
            PolyRms = GetRMS(PolyError, ModelLength);
            PredictedData = GetModelPredictedX(RobustGreyModelX, ModelLength + 1);
            GetPredictedError(CompareData, PredictedLength);

            PredictedRms = GetRMS(PredictedError, PredictedLength) * 1E9;
            Predicted3hRms = GetRMS(PredictedError, 3 * 3600 / IntervalSeconds) * 1E9;
            Predicted6hRms = GetRMS(PredictedError, 6 * 3600 / IntervalSeconds) * 1E9;
            Predicted12hRms = GetRMS(PredictedError, 12 * 3600 / IntervalSeconds) * 1E9;
            Predicted24hRms = GetRMS(PredictedError, 24 * 3600 / IntervalSeconds) * 1E9;
        }
    }

    /// <summary>
    /// 基于修正一次差分的抗差灰色模型（对建模使用的最后一个历元值进行修正，降低最后一个历元出现异常时对预报结果的影响）
    /// </summary>
    public class RevisedRobustDGreyModel : BasicFunctionModel
    {
        /// <summary>
        /// 计算
        /// </summary>
        /// <param name="ModelData"></param>
        /// <param name="CompareData"></param>
        /// <param name="IntervalSeconds"></param>
        public void Calculate(ArrayMatrix ModelData, ArrayMatrix CompareData,int IntervalSeconds)
        {
            int ModelLength = ModelData.RowCount - 1;//设定所用建模数据长度
            int PredictedLength = CompareData.RowCount;//设定所用

            #region 选取建模数据最后5个历元，对最后一个历元的钟差值进行拟合
            ArrayMatrix QPModelData = new ArrayMatrix(5, 1);
            for (int i = 0; i < 5; i++)
            {
                QPModelData[4 - i, 0] = ModelData[ModelLength - i, 0];

            }
            ArrayMatrix QPModelDataX = GetQuadraticPolynomialX(QPModelData, 1, IntervalSeconds);

            ModelData[ModelLength, 0] = QPModelDataX[4, 0];
            #endregion


            ArrayMatrix modelData = new ArrayMatrix(ModelLength, 1);
            for (int i = 0; i < ModelLength; i++)
            {
                modelData[i, 0] = ModelData[i + 1, 0] - ModelData[i, 0];
            }

            ArrayMatrix RobustDGreyModelX = GetRobustGreyModelX(modelData, PredictedLength);
            ArrayMatrix RobustGreyModelX = new ArrayMatrix(ModelLength + PredictedLength + 1, 1);

            RobustGreyModelX[0, 0] = ModelData[0, 0];

            for (int i = 0; i < ModelLength + PredictedLength; i++)
            {
                RobustGreyModelX[i + 1, 0] = RobustGreyModelX[i, 0] + RobustDGreyModelX[i, 0];
            }

            ArrayMatrix PolyX = GetModelPolyX(RobustGreyModelX, ModelLength + 1);
            PolyError = PolyX - ModelData;
            PolyRms = GetRMS(PolyError, ModelLength);
            PredictedData = GetModelPredictedX(RobustGreyModelX, ModelLength + 1);
            GetPredictedError(CompareData, PredictedLength);

            PredictedRms = GetRMS(PredictedError, PredictedLength) * 1E9;
            Predicted3hRms = GetRMS(PredictedError, 3 * 3600 / IntervalSeconds) * 1E9;
            Predicted6hRms = GetRMS(PredictedError, 6 * 3600 / IntervalSeconds) * 1E9;
            Predicted12hRms = GetRMS(PredictedError, 12 * 3600 / IntervalSeconds) * 1E9;
            Predicted24hRms = GetRMS(PredictedError, 24 * 3600 / IntervalSeconds) * 1E9;
        }
    }

}

