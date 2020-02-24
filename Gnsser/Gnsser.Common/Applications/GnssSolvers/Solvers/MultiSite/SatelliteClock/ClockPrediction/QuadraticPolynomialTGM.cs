//2016.04.26 double edit in xi'an train station 修改精简程序
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
    /// QPT2GM模型
    /// </summary>
    public class QuadraticPolynomialT2GM : BasicFunctionModel
    {
        /// <summary>
        /// 计算
        /// </summary>
        /// <param name="ModelData"></param>
        /// <param name="CompareData"></param>
        /// <param name="IntervalSeconds"></param>
        public void Calculate(ArrayMatrix ModelData, ArrayMatrix CompareData, int IntervalSeconds,SatelliteNumber prn)
        {
            int ModelLength = ModelData.RowCount;//设定所用建模数据长度
            int PredictedLength = CompareData.RowCount;//设定所用

            ArrayMatrix PredictedData = new ArrayMatrix(PredictedLength, 1);//预报数据
            ArrayMatrix QuadraticPolynomialT2X = GetQuadraticPolynomialT2X(ModelData, PredictedLength, IntervalSeconds, prn);
            ArrayMatrix QPT2_PolyX = GetModelPolyX(QuadraticPolynomialT2X, ModelLength);
            ArrayMatrix QPT2_PolyError = QPT2_PolyX - ModelData;
            double a = GetRMS(GetModelPredictedX(QuadraticPolynomialT2X, ModelLength) - CompareData, PredictedLength) * 1E9;
            ArrayMatrix GreyModelofDX = GetGreyModelX(QPT2_PolyError, PredictedLength);
            ArrayMatrix GreyModelPolyofDX = GetModelPolyX(GreyModelofDX, ModelLength);

            ArrayMatrix QPT2GM = QuadraticPolynomialT2X + GreyModelofDX;

            ArrayMatrix QPT2GM_PolyX = GetModelPolyX(QPT2GM, ModelLength);
            PolyError = QPT2GM_PolyX - ModelData;
            PolyRms = GetRMS(PolyError, ModelLength);
            PredictedData = GetModelPredictedX(QPT2GM, ModelLength);
            GetPredictedError(CompareData, PredictedLength);

            PredictedRms = GetRMS(PredictedError, PredictedLength)*1E9;
            Predicted3hRms = GetRMS(PredictedError, 3 * 3600 / IntervalSeconds) * 1E9;
            Predicted6hRms = GetRMS(PredictedError, 6 * 3600 / IntervalSeconds) * 1E9;
            Predicted12hRms = GetRMS(PredictedError, 12 * 3600 / IntervalSeconds) * 1E9;
            Predicted24hRms = GetRMS(PredictedError, 24 * 3600 / IntervalSeconds) * 1E9;
        } 
    }
    /// <summary>
    /// QPT4GM模型
    /// </summary>
    public class QuadraticPolynomialT4GM : BasicFunctionModel
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
            ArrayMatrix QuadraticPolynomialT4X = GetQuadraticPolynomialT4X(ModelData, PredictedLength, IntervalSeconds);
            ArrayMatrix QPT4_PolyX = GetModelPolyX(QuadraticPolynomialT4X, ModelLength);
            ArrayMatrix QPT4_PolyError = QPT4_PolyX - ModelData;

            ArrayMatrix GreyModelofDX = GetGreyModelX(QPT4_PolyError, ModelLength);
            ArrayMatrix GreyModelPolyofDX = GetModelPolyX(GreyModelofDX, ModelLength);

            ArrayMatrix QPT4GM = QuadraticPolynomialT4X + GreyModelofDX;

            ArrayMatrix QPT4GM_PolyX = GetModelPolyX(QPT4GM, ModelLength);
            PolyError = QPT4GM_PolyX - ModelData;
            PolyRms = GetRMS(PolyError, ModelLength);
            PredictedData = GetModelPredictedX(QPT4GM, ModelLength);
            GetPredictedError(CompareData, PredictedLength);

            PredictedRms = GetRMS(PredictedError, PredictedLength) * 1E9;
            Predicted3hRms = GetRMS(PredictedError, 3 * 3600 / IntervalSeconds) * 1E9;
            Predicted6hRms = GetRMS(PredictedError, 6 * 3600 / IntervalSeconds) * 1E9;
            Predicted12hRms = GetRMS(PredictedError, 12 * 3600 / IntervalSeconds) * 1E9;
            Predicted24hRms = GetRMS(PredictedError, 24 * 3600 / IntervalSeconds) * 1E9;
        }
    }
}

