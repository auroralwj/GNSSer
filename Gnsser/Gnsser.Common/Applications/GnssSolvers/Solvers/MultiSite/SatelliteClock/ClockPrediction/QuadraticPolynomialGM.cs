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
    /// QPGM模型
    /// </summary>
    public class QuadraticPolynomialGM : BasicFunctionModel
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
            ArrayMatrix QuadraticPolynomialX = GetQuadraticPolynomialX(ModelData, PredictedLength, IntervalSeconds);
            ArrayMatrix QP_PolyX = GetModelPolyX(QuadraticPolynomialX, ModelLength);
            ArrayMatrix QP_PolyError = QP_PolyX - ModelData;
            ArrayMatrix QP_PolyError1 = new ArrayMatrix(48, 1);
            ArrayMatrix ModelData1 = new ArrayMatrix(48, 1);
            for (int i = 0; i < QP_PolyError1.RowCount; i++)
            {
                QP_PolyError1[i, 0] = QP_PolyError[ModelLength - (48 - i), 0];
                ModelData1[i, 0] = ModelData[ModelLength - (48 - i), 0];
            }
            ArrayMatrix GreyModelofDX = GetGreyModelX(QP_PolyError1, PredictedLength);
            ArrayMatrix GreyModelPolyofDX = GetModelPolyX(GreyModelofDX, ModelLength);
            ArrayMatrix QPGM= new ArrayMatrix(48 + PredictedLength, 1);
            ArrayMatrix QuadraticPolynomialX1 = new ArrayMatrix(48 + PredictedLength, 1);
            for(int i = 0; i < 48 + PredictedLength; i++)
                QPGM[i, 0] = QuadraticPolynomialX[ModelLength-48+i  , 0] + GreyModelofDX[i, 0];
            
            ArrayMatrix QPGM_PolyX = GetModelPolyX(QPGM, 48);
            PolyError = QPGM_PolyX - ModelData1;
            PolyRms = GetRMS(PolyError, 48);
            PredictedData = GetModelPredictedX(QPGM, 48);
            GetPredictedError(CompareData, PredictedLength);

            PredictedRms = GetRMS(PredictedError, PredictedLength)*1E9;
            Predicted3hRms = GetRMS(PredictedError, 3 * 3600 / IntervalSeconds) * 1E9;
            Predicted6hRms = GetRMS(PredictedError, 6 * 3600 / IntervalSeconds) * 1E9;
            Predicted12hRms = GetRMS(PredictedError, 12 * 3600 / IntervalSeconds) * 1E9;
            Predicted24hRms = GetRMS(PredictedError, 24 * 3600 / IntervalSeconds) * 1E9;
            
        }
        
    }
    
}

