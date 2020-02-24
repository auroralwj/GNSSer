//2016.04.26 double edit in xi'an train station 修改精简程序

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
    /// 二次多项式模型
    /// </summary>
    public class QuadraticPolynomialModel : BasicFunctionModel
    {
        /// <summary>
        /// 计算
        /// </summary>
        /// <param name="ModelData"></param>
        /// <param name="CompareData"></param>
        /// <param name="IntervalSeconds"></param>
        public void Calculate(ArrayMatrix ModelData, ArrayMatrix CompareData, int IntervalSeconds)
        {
            int ModelLength = ModelData.RowCount;//设定所用建模数据长度
            int PredictedLength = CompareData.RowCount;//设定所用

            ArrayMatrix QuadraticPolynomialX = GetQuadraticPolynomialX(ModelData, PredictedLength, IntervalSeconds);
            ArrayMatrix QP_PolyX = GetModelPolyX(QuadraticPolynomialX, ModelLength);
            PolyError = QP_PolyX - ModelData;
            PolyRms = GetRMS(PolyError, ModelLength);
            PredictedData = GetModelPredictedX(QuadraticPolynomialX, ModelLength);
            GetPredictedError(CompareData, PredictedLength);

            PredictedRms = GetRMS(PredictedError, PredictedLength) * 1E9;
            Predicted3hRms = GetRMS(PredictedError, 3 * 3600 / IntervalSeconds) * 1E9;
            Predicted6hRms = GetRMS(PredictedError, 6 * 3600 / IntervalSeconds) * 1E9;
            Predicted12hRms = GetRMS(PredictedError, 12 * 3600 / IntervalSeconds) * 1E9;
            Predicted24hRms = GetRMS(PredictedError, 24 * 3600 / IntervalSeconds) * 1E9;
        }

        
        public void FillingCalculate(ArrayMatrix ModelData, int IntervalSeconds)
        {
            int ModelLength = ModelData.RowCount;//设定所用建模数据长度
            
            ArrayMatrix QuadraticPolynomialX = GetQuadraticPolynomialX(ModelData, 1, IntervalSeconds);
            ArrayMatrix QP_PolyX = GetModelPolyX(QuadraticPolynomialX, ModelLength);
            for (int i = 0; i < ModelLength; i++)
            {
                if (ModelData[i, 0] == 0)
                    ModelData[i, 0] = QP_PolyX[i, 0];
            }              
        }
    }
    /// <summary>
    /// 一次多项式模型
    /// </summary>
    public class LinearPolynomialModel : BasicFunctionModel
    {
        /// <summary>
        /// 计算
        /// </summary>
        /// <param name="ModelData"></param>
        /// <param name="CompareData"></param>
        /// <param name="IntervalSeconds"></param>
        public void Calculate(ArrayMatrix ModelData, ArrayMatrix CompareData, int IntervalSeconds)
        {
            int ModelLength = ModelData.RowCount;//设定所用建模数据长度
            int PredictedLength = CompareData.RowCount;//设定所用

            ArrayMatrix QuadraticPolynomialX = GetLinearPolynomialX(ModelData, PredictedLength, IntervalSeconds);
            ArrayMatrix QP_PolyX = GetModelPolyX(QuadraticPolynomialX, ModelLength);
            PolyError = QP_PolyX - ModelData;
            PolyRms = GetRMS(PolyError, ModelLength);
            PredictedData = GetModelPredictedX(QuadraticPolynomialX, ModelLength);
            GetPredictedError(CompareData, PredictedLength);

            PredictedRms = GetRMS(PredictedError, PredictedLength) * 1E9;
            Predicted3hRms = GetRMS(PredictedError, 3 * 3600 / IntervalSeconds) * 1E9;
            Predicted6hRms = GetRMS(PredictedError, 6 * 3600 / IntervalSeconds) * 1E9;
            Predicted12hRms = GetRMS(PredictedError, 12 * 3600 / IntervalSeconds) * 1E9;
            Predicted24hRms = GetRMS(PredictedError, 24 * 3600 / IntervalSeconds) * 1E9;
        }
    }
    /// <summary>
    /// 基于一次差分的一次多项式模型
    /// </summary>
    public class DLinearPolynomialModel : BasicFunctionModel
    {
        /// <summary>
        /// 计算
        /// </summary>
        /// <param name="ModelData"></param>
        /// <param name="CompareData"></param>
        /// <param name="IntervalSeconds"></param>
        public void Calculate(ArrayMatrix ModelData, ArrayMatrix CompareData, int IntervalSeconds)
        {
            int ModelLength = ModelData.RowCount - 1;//设定所用建模数据长度
            int PredictedLength = CompareData.RowCount;//设定所用

            ArrayMatrix modelData = new ArrayMatrix(ModelLength, 1);
            for (int i = 0; i < ModelLength; i++)
            {
                modelData[i, 0] = ModelData[i + 1, 0] - ModelData[i, 0];
            }

            ArrayMatrix LinearPolynomialX = GetLinearPolynomialX(modelData, PredictedLength, IntervalSeconds);
            ArrayMatrix DLinearPolynomialX = new ArrayMatrix(ModelLength + PredictedLength + 1, 1);

            DLinearPolynomialX[0, 0] = ModelData[0, 0];

            for (int i = 0; i < ModelLength + PredictedLength; i++)
            {
                DLinearPolynomialX[i + 1, 0] = DLinearPolynomialX[i, 0] + LinearPolynomialX[i, 0];
            }

            ArrayMatrix PolyX = GetModelPolyX(DLinearPolynomialX, ModelLength + 1);
            PolyError = PolyX - ModelData;
            PolyRms = GetRMS(PolyError, ModelLength);
            PredictedData = GetModelPredictedX(DLinearPolynomialX, ModelLength + 1);
            GetPredictedError(CompareData, PredictedLength);

            PredictedRms = GetRMS(PredictedError, PredictedLength) * 1E9;
            Predicted3hRms = GetRMS(PredictedError, 3 * 3600 / IntervalSeconds) * 1E9;
            Predicted6hRms = GetRMS(PredictedError, 6 * 3600 / IntervalSeconds) * 1E9;
            Predicted12hRms = GetRMS(PredictedError, 12 * 3600 / IntervalSeconds) * 1E9;
            Predicted24hRms = GetRMS(PredictedError, 24 * 3600 / IntervalSeconds) * 1E9;
        }
    }
}

