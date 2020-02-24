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
    /// 灰色模型
    /// </summary>
    public class GreyModel : BasicFunctionModel
    {
        /// <summary>
        /// 计算
        /// </summary>
        /// <param name="ModelData"></param>
        /// <param name="CompareData"></param>
        /// <param name="IntervalSeconds"></param>
        public  void Calculate(ArrayMatrix ModelData,ArrayMatrix CompareData,int IntervalSeconds)
        {

            int ModelLength = ModelData.RowCount;//设定所用建模数据长度
            int PredictedLength = CompareData.RowCount;//设定所用 
            

            ArrayMatrix GreyModelX = GetGreyModelX(ModelData, PredictedLength);
            ArrayMatrix GreyModelPolyX = GetModelPolyX(GreyModelX, ModelLength);
            PolyError = GreyModelPolyX - ModelData;
            PolyRms = GetRMS(PolyError, ModelLength);
            PredictedData = GetModelPredictedX(GreyModelX, ModelLength);
            GetPredictedError(CompareData, PredictedLength);

            PredictedRms = GetRMS(PredictedError, PredictedLength)*1E9;
            Predicted3hRms = GetRMS(PredictedError, 3 * 3600 / IntervalSeconds) * 1E9;
            Predicted6hRms = GetRMS(PredictedError, 6 * 3600 / IntervalSeconds) * 1E9;
            Predicted12hRms = GetRMS(PredictedError, 12 * 3600 / IntervalSeconds) * 1E9;
            Predicted24hRms = GetRMS(PredictedError, 24 * 3600 / IntervalSeconds) * 1E9;

        }
        /// <summary>
        /// 用于周跳探测的灰色模型
        /// </summary>
        /// <param name="modeldata"></param>
        /// <param name="IntervalSeconds"></param>
        public void Calculate(List<double> modeldata, double IntervalSeconds)
        {
            ArrayMatrix ModelData = new ArrayMatrix(modeldata.Count - 1, 1);
            for (int i = 0; i < modeldata.Count - 1; i++)
                ModelData[i, 0] = modeldata[i + 1] - modeldata[i];
            int ModelLength = ModelData.RowCount;//设定所用建模数据长度

            //Matrix PredictedData = new Matrix(PredictedLength, 1);//预报数据
            ArrayMatrix GreyModelX = GetGreyModelX(ModelData, 1);
            ArrayMatrix GreyModelPolyX = GetModelPolyX(GreyModelX, ModelLength);
            PolyError = GreyModelPolyX - ModelData;
            ArrayMatrix a = (PolyError.Transpose() * PolyError);
            PolyRms = Math.Sqrt(a[0, 0] / (ModelLength - 3));
            PredictedData = GetModelPredictedX(GreyModelX, ModelLength);
            nextFitValue = PredictedData[0, 0];
        }
    }
    /// <summary>
    /// 灰色模型
    /// </summary>
    public class RevisedGreyModel : BasicFunctionModel
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

            #region 选取建模数据最后5个历元，对最后一个历元的钟差值进行拟合
            ArrayMatrix QPModelData = new ArrayMatrix(5, 1);
            for (int i = 0; i < 5; i++)
            {
                QPModelData[4 - i, 0] = ModelData[ModelLength - i, 0];

            }
            ArrayMatrix QPModelDataX = GetQuadraticPolynomialX(QPModelData, 1, IntervalSeconds);

            ModelData[ModelLength, 0] = QPModelDataX[4, 0];
            #endregion


            //Matrix PredictedData = new Matrix(PredictedLength, 1);//预报数据
            ArrayMatrix GreyModelX = GetGreyModelX(ModelData, PredictedLength);
            ArrayMatrix GreyModelPolyX = GetModelPolyX(GreyModelX, ModelLength);
            PolyError = GreyModelPolyX - ModelData;
            PolyRms = GetRMS(PolyError, ModelLength);
            PredictedData = GetModelPredictedX(GreyModelX, ModelLength);
            PredictedError = PredictedData - CompareData;
            PredictedRms = GetRMS(PredictedError, PredictedLength) * 1E9;
            Predicted3hRms = GetRMS(PredictedError, 3 * 3600 / IntervalSeconds) * 1E9;
            Predicted6hRms = GetRMS(PredictedError, 6 * 3600 / IntervalSeconds) * 1E9;
            Predicted12hRms = GetRMS(PredictedError, 12 * 3600 / IntervalSeconds) * 1E9;
            Predicted24hRms = GetRMS(PredictedError, 24 * 3600 / IntervalSeconds) * 1E9;

        }
    }
    
}

