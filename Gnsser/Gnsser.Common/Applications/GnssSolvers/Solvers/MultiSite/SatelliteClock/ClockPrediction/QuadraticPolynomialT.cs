//2016.04.26 double create on the train of xi'an-Zhengzhou 
//2016.12.05 double edit in hongqing, 整合代码

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
    /// QPT1模型
    /// </summary>
    public class QuadraticPolynomialT1 : BasicFunctionModel
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

            ArrayMatrix PredictedData = new ArrayMatrix(PredictedLength, 1);//预报数据
            ArrayMatrix QPlT1X = GetQuadraticPolynomialT1X(ModelData, PredictedLength, IntervalSeconds);
            ArrayMatrix QPT1PolyX = GetModelPolyX(QPlT1X, ModelLength);
            PolyError = QPT1PolyX - ModelData;
            PolyRms = GetRMS(PolyError, ModelLength);
            GetPredictedError(CompareData, PredictedLength);

            PredictedRms = GetRMS(PredictedError, PredictedLength)* 1E9;
            Predicted3hRms = GetRMS(PredictedError, 3 * 3600 / IntervalSeconds) * 1E9;
            Predicted6hRms = GetRMS(PredictedError, 6 * 3600 / IntervalSeconds) * 1E9;
            Predicted12hRms = GetRMS(PredictedError, 12 * 3600 / IntervalSeconds) * 1E9;
            Predicted24hRms = GetRMS(PredictedError, 24 * 3600 / IntervalSeconds) * 1E9;
        }
    }

    /// <summary>
    /// QPT2模型
    /// </summary>
    public class QuadraticPolynomialT2 : BasicFunctionModel
    {
        /// <summary>
        /// 计算
        /// </summary>
        /// <param name="ModelData"></param>
        /// <param name="CompareData"></param>
        /// <param name="IntervalSeconds"></param>
        public void Calculate(ArrayMatrix ModelData, ArrayMatrix CompareData, int IntervalSeconds, SatelliteNumber prn)
        {
            int ModelLength = ModelData.RowCount;//设定所用建模数据长度
            int PredictedLength = CompareData.RowCount;//设定所用

            ArrayMatrix QPT2X = GetQuadraticPolynomialT2X(ModelData, PredictedLength, IntervalSeconds, prn);
            ArrayMatrix QPT2PolyX = GetModelPolyX(QPT2X, ModelLength);
            PolyError = QPT2PolyX - ModelData;
            PolyRms = GetRMS(PolyError, ModelLength);
            PredictedData = GetModelPredictedX(QPT2X, ModelLength);
            GetPredictedError(CompareData, PredictedLength);

            PredictedRms = GetRMS(PredictedError, PredictedLength) * 1E9;
            Predicted3hRms = GetRMS(PredictedError, 3 * 3600 / IntervalSeconds) * 1E9;
            Predicted6hRms = GetRMS(PredictedError, 6 * 3600 / IntervalSeconds) * 1E9;
            Predicted12hRms = GetRMS(PredictedError, 12 * 3600 / IntervalSeconds) * 1E9;
            Predicted24hRms = GetRMS(PredictedError, 24 * 3600 / IntervalSeconds) * 1E9;
        }
    }
    /// <summary>
    /// QPT3模型
    /// </summary>
    public class QuadraticPolynomialT3 : BasicFunctionModel
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

            ArrayMatrix QPlT3X = GetQuadraticPolynomialT3X(ModelData, PredictedLength, IntervalSeconds);
            ArrayMatrix QPT3PolyX = GetModelPolyX(QPlT3X, ModelLength);
            PolyError = QPT3PolyX - ModelData;
            PolyRms = GetRMS(PolyError, ModelLength);
            PredictedData = GetModelPredictedX(QPlT3X, ModelLength);
            GetPredictedError(CompareData, PredictedLength);

            PredictedRms = GetRMS(PredictedError, PredictedLength)* 1E9;
            Predicted3hRms = GetRMS(PredictedError, 3 * 3600 / IntervalSeconds) * 1E9;
            Predicted6hRms = GetRMS(PredictedError, 6 * 3600 / IntervalSeconds) * 1E9;
            Predicted12hRms = GetRMS(PredictedError, 12 * 3600 / IntervalSeconds) * 1E9;
            Predicted24hRms = GetRMS(PredictedError, 24 * 3600 / IntervalSeconds) * 1E9;
        }
    }
    /// <summary>
    /// QPT4模型
    /// </summary>
    public class QuadraticPolynomialT4 : BasicFunctionModel
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

            ArrayMatrix QPT4X = GetQuadraticPolynomialT4X(ModelData, PredictedLength, IntervalSeconds);
            ArrayMatrix QPT4PolyX = GetModelPolyX(QPT4X, ModelLength);
            PolyError = QPT4PolyX - ModelData;
            PolyRms = GetRMS(PolyError, ModelLength);
            PredictedData = GetModelPredictedX(QPT4X, ModelLength);
            GetPredictedError(CompareData, PredictedLength);

            PredictedRms = GetRMS(PredictedError, PredictedLength)* 1E9;
            Predicted3hRms = GetRMS(PredictedError, 3 * 3600 / IntervalSeconds) * 1E9;
            Predicted6hRms = GetRMS(PredictedError, 6 * 3600 / IntervalSeconds) * 1E9;
            Predicted12hRms = GetRMS(PredictedError, 12 * 3600 / IntervalSeconds) * 1E9;
            Predicted24hRms = GetRMS(PredictedError, 24 * 3600 / IntervalSeconds) * 1E9;
        }
    }    
}

