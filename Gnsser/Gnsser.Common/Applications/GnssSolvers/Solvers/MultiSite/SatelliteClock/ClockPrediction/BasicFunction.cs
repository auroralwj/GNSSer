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
    /// 基本函数
    /// </summary>
    public class BasicFunctionModel
    {
        /// <summary>
        /// 预报误差
        /// </summary>
        public ArrayMatrix PredictedError { get; set; }
        /// <summary>
        /// 拟合误差
        /// </summary>
         public ArrayMatrix PolyError { get; set; }
        /// <summary>
        /// 预报数据
        /// </summary>
        public ArrayMatrix PredictedData { get; set; }
        public double nextFitValue { get; set; }

        #region  计算残差值的最大值，最小值，平均值以及极差值
        /// <summary>
        /// 预报误差值的最小值
        /// </summary>
        public double min { get { return PredictedError.MinValue; }  }
        /// <summary>
        /// 预报误差值的最大值
        /// </summary>
        public double max { get { return PredictedError.MaxValue; }  }
        /// <summary>
        /// 预报误差值的平均值
        /// </summary>
        public double mean { get { return PredictedError.MeanValue; }  }
        /// <summary>
        /// 预报误差值的极差值
        /// </summary>
        public double range { get { return max - min; }  }
        #endregion
        /// <summary>
        /// 拟合中误差
        /// </summary>
        public double PolyRms { get; set; }
        /// <summary>
        /// 预报中误差
        /// </summary>
        public double Predicted3hRms { get; set; }
        public double Predicted6hRms { get; set; }
        public double Predicted12hRms { get; set; }
        public double Predicted24hRms { get; set; }
        public double PredictedRms { get; set; }

        #region 一次多项式模型
        /// <summary>
        /// 获取抗差一次多项式模型的结果
        /// </summary>
        /// <param name="ModelData"></param>
        /// <param name="PredictedLength"></param>
        /// <param name="IntervalSecond"></param>
        /// <returns></returns>
        public static ArrayMatrix GetRobustLinearPolynomialX(ArrayMatrix ModelData, int PredictedLength, int IntervalSecond)
        {
            int N = ModelData.Rows + PredictedLength;
            ArrayMatrix RobustLinearPolynomialX = new ArrayMatrix(N, 1);
            ArrayMatrix A = new ArrayMatrix(ModelData.Rows, 2);
            double t;
            for (int i = 0; i < ModelData.Rows; i++)
            {
                t = i * IntervalSecond;
                A[i, 0] = 1;
                A[i, 1] = t;
            }
            double a1 = 0; double a2 = 0;

            ArrayMatrix X = (A.Transpose() * A).Inverse * (A.Transpose() * ModelData);
            a1 = X[0, 0];
            a2 = X[1, 0];

            double diff1 = 1; double diff2 = 1;
            ArrayMatrix Error = new ArrayMatrix(ModelData.Rows, 1);
            while (diff1 > 1E-14 || diff2 > 1E-14)
            {
                for (int i = 0; i < ModelData.Rows; i++)
                {
                    Error[i, 0] = Math.Abs(a1 + a2 * IntervalSecond * i-ModelData[i,0]);
                }
                ArrayMatrix P=new ArrayMatrix (ModelData.Rows,ModelData.Rows);
                double rms = GetRMS(Error, ModelData.Rows);
                double k0 = 2.5 * rms;
                double k1 = 3.5 * rms;
                for (int i = 0; i < ModelData.Rows; i++)
                {
                    if (Error[i, 0] < k0)
                        P[i, i] = 1.0;
                    else if (Error[i, 0] >= k0 && Error[i, 0] < k1)
                        P[i, i] =Math.Pow( (k1 - Error[i, 0]) / (k1 - k0),  2) * k0 / (Error[i, 0]);
                    if (Error[i, 0] >= k1)
                        P[i, i] = 0;
                }
                X = (A.Transpose() * P * A).Inverse * (A.Transpose() * P * ModelData);
                diff1 = Math.Abs(X[0, 0] - a1);
                diff2 = Math.Abs(X[1, 0] - a2);
                a1 = X[0, 0];
                a2 = X[1, 0];
            }

            RobustLinearPolynomialX[0, 0] = ModelData[0, 0];
            for (int i = 0; i < N; i++)
            {
                RobustLinearPolynomialX[i, 0] = a1 + a2 * IntervalSecond * i;
            }

            return RobustLinearPolynomialX;
        }
        /// <summary>
        /// 获取一次多项式模型的结果
        /// </summary>
        /// <param name="ModelData"></param>
        /// <param name="PredictedLength"></param>
        /// <param name="IntervalSecond"></param>
        /// <returns></returns>
        public static ArrayMatrix GetLinearPolynomialX(ArrayMatrix ModelData, int PredictedLength, int IntervalSecond)
        {
            int N = ModelData.Rows + PredictedLength;
            ArrayMatrix LinearPolynomialX = new ArrayMatrix(N, 1);
            ArrayMatrix A = new ArrayMatrix(ModelData.Rows, 2);
            double t;
            for (int i = 0; i < ModelData.Rows; i++)
            {
                t = i * IntervalSecond;
                A[i, 0] = 1;
                A[i, 1] = t;
            }
            ArrayMatrix X = (A.Transpose() * A).Inverse * (A.Transpose() * ModelData);
            double a1 = X[0, 0];
            double a2 = X[1, 0];
            LinearPolynomialX[0, 0] = ModelData[0, 0];
            for (int i = 0; i < N; i++)
            {
                LinearPolynomialX[i, 0] = a1 + a2 * IntervalSecond * i;
            }

            return LinearPolynomialX;
        }
        #endregion

        #region 二次多项式模型
        /// <summary>
        /// 获取二次多项式模型的结果
        /// </summary>
        /// <param name="ModelData"></param>
        /// <param name="PredictedLength"></param>
        /// <param name="IntervalSecond"></param>
        /// <returns></returns>
        public static ArrayMatrix GetQuadraticPolynomialX(ArrayMatrix ModelData, int PredictedLength, int IntervalSecond)
        {
            int N = ModelData.Rows + PredictedLength;
            ArrayMatrix QuadraticPolynomialX = new ArrayMatrix(N, 1);
            ArrayMatrix A = new ArrayMatrix(ModelData.Rows, 3);
            double t;
            for (int i = 0; i < ModelData.Rows; i++)
            {
                if (ModelData[i, 0] == 0)
                    continue;
                t = i * IntervalSecond;
                A[i, 0] = 1;
                A[i, 1] = t;
                A[i, 2] = t * t;
            }
            ArrayMatrix X = (A.Transpose() * A).Inverse * (A.Transpose() * ModelData);
            double a1 = X[0, 0];
            double a2 = X[1, 0];
            double a3 = X[2, 0];
            QuadraticPolynomialX[0, 0] = ModelData[0, 0];
            for (int i = 0; i < N; i++)
            {
                QuadraticPolynomialX[i, 0] = a1 + a2 * IntervalSecond * i + a3 * IntervalSecond * i * IntervalSecond * i;
            }

            return QuadraticPolynomialX;
        }
        /// <summary>
        /// 获取抗差二次多项式模型的结果
        /// </summary>
        /// <param name="ModelData"></param>
        /// <param name="PredictedLength"></param>
        /// <param name="IntervalSecond"></param>
        /// <returns></returns>
        public static ArrayMatrix GetRobustQuadraticPolynomialX(ArrayMatrix ModelData, int PredictedLength, int IntervalSecond)
        {
            int N = ModelData.Rows + PredictedLength;
            ArrayMatrix QuadraticPolynomialX = new ArrayMatrix(N, 1);
            ArrayMatrix A = new ArrayMatrix(ModelData.Rows, 3);
            double t;
            for (int i = 0; i < ModelData.Rows; i++)
            {
                t = i * IntervalSecond;
                A[i, 0] = 1;
                A[i, 1] = t;
                A[i, 2] = t * t;
            }
            ArrayMatrix X = (A.Transpose() * A).Inverse * (A.Transpose() * ModelData);
            double a1 = X[0, 0];
            double a2 = X[1, 0];
            double a3 = X[2, 0];


            double diff1 = 1; double diff2 = 1; double diff3 = 1;
            ArrayMatrix Error = new ArrayMatrix(ModelData.Rows, 1);
            while (diff1 > 1E-14 || diff2 > 1E-14 || diff3 > 1E-14)
            {
                for (int i = 0; i < ModelData.Rows; i++)
                {
                    Error[i, 0] = Math.Abs(a1 + a2 * IntervalSecond * i + a3 * IntervalSecond * i * IntervalSecond * i - ModelData[i, 0]);
                }
                ArrayMatrix P = new ArrayMatrix(ModelData.Rows, ModelData.Rows);
                double rms = GetRMS(Error, ModelData.Rows);
                double k0 = 2.5 * rms;
                double k1 = 3.5 * rms;
                for (int i = 0; i < ModelData.Rows; i++)
                {
                    if (Error[i, 0] < k0)
                        P[i, i] = 1.0;
                    else if (Error[i, 0] >= k0 && Error[i, 0] < k1)
                        P[i, i] = Math.Pow((k1 - Error[i, 0]) / (k1 - k0), 2) * k0 / (Error[i, 0]);
                    if (Error[i, 0] >= k1)
                        P[i, i] = 0;
                }
                X = (A.Transpose() * P * A).Inverse * (A.Transpose() * P * ModelData);
                diff1 = Math.Abs(X[0, 0] - a1);
                diff2 = Math.Abs(X[1, 0] - a2);
                diff3 = Math.Abs(X[3, 0] - a2);
                a1 = X[0, 0];
                a2 = X[1, 0];
                a3 = X[2, 0];
            }


            QuadraticPolynomialX[0, 0] = ModelData[0, 0];
            for (int i = 0; i < N; i++)
            {
                QuadraticPolynomialX[i, 0] = a1 + a2 * IntervalSecond * i + a3 * IntervalSecond * i * IntervalSecond * i;
            }

            return QuadraticPolynomialX;
        }
        /// <summary>
        /// 获取二次多项式加1个主要周期项模型的结果
        /// </summary>
        /// <param name="ModelData"></param>
        /// <param name="PredictedLength"></param>
        /// <param name="IntervalSecond"></param>
        /// <returns></returns>
        public static ArrayMatrix GetQuadraticPolynomialT1X(ArrayMatrix ModelData, int PredictedLength, int IntervalSecond)
        {
            double f1 = 1.0 / (12 * 3600);
            int N = ModelData.Rows + PredictedLength;
            ArrayMatrix QuadraticPolynomialX = new ArrayMatrix(N, 1);
            ArrayMatrix A = new ArrayMatrix(ModelData.Rows, 5);
            double t;
            for (int i = 0; i < ModelData.Rows; i++)
            {
                t = i * IntervalSecond;
                A[i, 0] = 1;
                A[i, 1] = t;
                A[i, 2] = t * t;

                A[i, 3] = Math.Sin(2 * Math.PI * f1 * t);
                A[i, 4] = Math.Cos(2 * Math.PI * f1 * t);
            }
            ArrayMatrix X = (A.Transpose() * A).Inverse * (A.Transpose() * ModelData);
            double a1 = X[0, 0];
            double a2 = X[1, 0];
            double a3 = X[2, 0];
            QuadraticPolynomialX[0, 0] = ModelData[0, 0];
            for (int i = 0; i < N; i++)
            {
                t = i * IntervalSecond;
                QuadraticPolynomialX[i, 0] = a1 + a2 * IntervalSecond * i + a3 * IntervalSecond * i * IntervalSecond * i + X[3, 0] * Math.Sin(2 * Math.PI * f1 * t) + X[4, 0] * Math.Cos(2 * Math.PI * f1 * t);
            }

            return QuadraticPolynomialX;
        }
        public List<string> MEO =new  List<string>();
        /// <summary>
        /// 获取二次多项式加2个主要周期项模型的结果
        /// </summary>
        /// <param name="ModelData"></param>
        /// <param name="PredictedLength"></param>
        /// <param name="IntervalSecond"></param>
        /// <returns></returns>
        public static ArrayMatrix GetQuadraticPolynomialT2X(ArrayMatrix ModelData, int PredictedLength, int IntervalSecond, SatelliteNumber prn)
        {
            double f1 = 0; double f2 = 0;
            if (prn.SatelliteType.ToString().Equals("G"))
            { f1 = 1.0 / (12 * 3600);  f2 = 1.0 / (6 * 3600); }
            else if (prn.SatelliteType.ToString().Equals("C"))
            {
                if (prn.PRN <= 10 )
                { f1 = 1.0 / (12 * 3600); f2 = 1.0 / (24 * 3600); }
                else { f1 = 1.0 / (12 * 3600 + 55 * 60); f2 = 1.0 / (6 * 3600 + 26 * 60); }
            }
            int N = ModelData.Rows + PredictedLength;
            ArrayMatrix QuadraticPolynomialX = new ArrayMatrix(N, 1);
            ArrayMatrix A = new ArrayMatrix(ModelData.Rows, 7);
            double t;
            for (int i = 0; i < ModelData.Rows; i++)
            {
                t = i * IntervalSecond;
                A[i, 0] = 1;
                A[i, 1] = t;
                A[i, 2] = t * t;

                A[i, 3] = Math.Sin(2 * Math.PI * f1 * t);
                A[i, 4] = Math.Cos(2 * Math.PI * f1 * t);
                A[i, 5] = Math.Sin(2 * Math.PI * f2 * t);
                A[i, 6] = Math.Cos(2 * Math.PI * f2 * t);
            }
            ArrayMatrix X = (A.Transpose() * A).Inverse * (A.Transpose() * ModelData);
            //double a1 = X[0, 0];
            //double a2 = X[1, 0];
            //double a3 = X[2, 0];
            //QuadraticPolynomialX[0, 0] = ModelData[0, 0];
            for (int i = 0; i < N; i++)
            {
                t = i * IntervalSecond;
                QuadraticPolynomialX[i, 0] = X[0, 0] + X[1, 0] * IntervalSecond * i + X[2, 0] * IntervalSecond * i * IntervalSecond * i + X[3, 0] * Math.Sin(2 * Math.PI * f1 * t) + X[4, 0] * Math.Cos(2 * Math.PI * f1 * t) + X[5, 0] * Math.Sin(2 * Math.PI * f2 * t) + X[6, 0] * Math.Cos(2 * Math.PI * f2 * t);
            }

            return QuadraticPolynomialX;
        }

        /// <summary>
        /// 获取二次多项式加3个主要周期项模型的结果
        /// </summary>
        /// <param name="ModelData"></param>
        /// <param name="PredictedLength"></param>
        /// <param name="IntervalSecond"></param>
        /// <returns></returns>
        public static ArrayMatrix GetQuadraticPolynomialT3X(ArrayMatrix ModelData, int PredictedLength, int IntervalSecond)
        {
            double f1 = 1.0 / (12 * 3600); double f2 = 1.0 / (6 * 3600);
            double f3 = 1.0 / (4 * 3600); 
            int N = ModelData.Rows + PredictedLength;
            ArrayMatrix QuadraticPolynomialX = new ArrayMatrix(N, 1);
            ArrayMatrix A = new ArrayMatrix(ModelData.Rows, 9);
            double t;
            for (int i = 0; i < ModelData.Rows; i++)
            {
                t = i * IntervalSecond;
                A[i, 0] = 1;
                A[i, 1] = t;
                A[i, 2] = t * t;

                A[i, 3] = Math.Sin(2 * Math.PI * f1 * t);
                A[i, 4] = Math.Cos(2 * Math.PI * f1 * t);
                A[i, 5] = Math.Sin(2 * Math.PI * f2 * t);
                A[i, 6] = Math.Cos(2 * Math.PI * f2 * t);
                A[i, 7] = Math.Sin(2 * Math.PI * f3 * t);
                A[i, 8] = Math.Cos(2 * Math.PI * f3 * t);
            }
            ArrayMatrix X = (A.Transpose() * A).Inverse * (A.Transpose() * ModelData);
            double a1 = X[0, 0];
            double a2 = X[1, 0];
            double a3 = X[2, 0];
            QuadraticPolynomialX[0, 0] = ModelData[0, 0];
            for (int i = 0; i < N; i++)
            {
                t = i * IntervalSecond;
                QuadraticPolynomialX[i, 0] = a1 + a2 * IntervalSecond * i + a3 * IntervalSecond * i * IntervalSecond * i + X[3, 0] * Math.Sin(2 * Math.PI * f1 * t) + X[4, 0] * Math.Cos(2 * Math.PI * f1 * t) + X[5, 0] * Math.Sin(2 * Math.PI * f2 * t) + X[6, 0] * Math.Cos(2 * Math.PI * f2 * t) + X[7, 0] * Math.Sin(2 * Math.PI * f3 * t) + X[8, 0] * Math.Cos(2 * Math.PI * f3 * t);
            }

            return QuadraticPolynomialX;
        }
        /// <summary>
        /// 获取二次多项式加4个主要周期项模型的结果
        /// </summary>
        /// <param name="ModelData"></param>
        /// <param name="PredictedLength"></param>
        /// <param name="IntervalSecond"></param>
        /// <returns></returns>
        public static ArrayMatrix GetQuadraticPolynomialT4X(ArrayMatrix ModelData, int PredictedLength, int IntervalSecond)
        {
            double f1 = 1.0 / (12 * 3600); double f2 = 1.0 / (6 * 3600);
            double f3 = 1.0 / (4 * 3600); double f4 = 1.0 / (3 * 3600);
            int N = ModelData.Rows + PredictedLength;
            ArrayMatrix QuadraticPolynomialX = new ArrayMatrix(N, 1);
            ArrayMatrix A = new ArrayMatrix(ModelData.Rows, 11);
            double t;
            for (int i = 0; i < ModelData.Rows; i++)
            {
                t = i * IntervalSecond;
                A[i, 0] = 1;
                A[i, 1] = t;
                A[i, 2] = t * t;

                A[i, 3] = Math.Sin(2 * Math.PI * f1 * t);
                A[i, 4] = Math.Cos(2 * Math.PI * f1 * t);
                A[i, 5] = Math.Sin(2 * Math.PI * f2 * t);
                A[i, 6] = Math.Cos(2 * Math.PI * f2 * t);
                A[i, 7] = Math.Sin(2 * Math.PI * f3 * t);
                A[i, 8] = Math.Cos(2 * Math.PI * f3 * t);
                A[i, 9] = Math.Sin(2 * Math.PI * f4 * t);
                A[i, 10] = Math.Cos(2 * Math.PI * f4 * t);
            }
            ArrayMatrix X = (A.Transpose() * A).Inverse * (A.Transpose() * ModelData);
            double a1 = X[0, 0];
            double a2 = X[1, 0];
            double a3 = X[2, 0];
            QuadraticPolynomialX[0, 0] = ModelData[0, 0];
            for (int i = 0; i < N; i++)
            {
                t = i * IntervalSecond;
                QuadraticPolynomialX[i, 0] = a1 + a2 * IntervalSecond * i + a3 * IntervalSecond * i * IntervalSecond * i + X[3, 0] * Math.Sin(2 * Math.PI * f1 * t) + X[4, 0] * Math.Cos(2 * Math.PI * f1 * t) + X[5, 0] * Math.Sin(2 * Math.PI * f2 * t) + X[6, 0] * Math.Cos(2 * Math.PI * f2 * t) + X[7, 0] * Math.Sin(2 * Math.PI * f3 * t) + X[8, 0] * Math.Cos(2 * Math.PI * f3 * t) + X[9, 0] * Math.Sin(2 * Math.PI * f4 * t) + X[10, 0] * Math.Cos(2 * Math.PI * f4 * t);
            }

            return QuadraticPolynomialX;
        }        
        #endregion

        #region 灰色模型
        /// <summary>
        /// 灰色模型
        /// </summary>
        /// <param name="ModelData"></param>
        /// <param name="PredictedLength"></param>
        /// <returns></returns>
        public static ArrayMatrix GetGreyModelX(ArrayMatrix ModelData, int PredictedLength)
        {
            int N = ModelData.Rows + PredictedLength;
            ArrayMatrix GreyModelX = new ArrayMatrix(N, 1);
            ArrayMatrix GreyModel1 = new ArrayMatrix(ModelData.Rows, 1);
            ArrayMatrix L1 = new ArrayMatrix(ModelData.Rows , 1);
            ArrayMatrix L = new ArrayMatrix(ModelData.Rows - 1, 1);
            double MaxValueofModelData = Math.Abs(ModelData.MaxValue);
            if (ModelData.MinValue < 0)
                for (int i = 0; i < ModelData.Rows; i++)
                { GreyModel1[i, 0] = ModelData[i, 0] + MaxValueofModelData * 10; }//;
            else GreyModel1 = ModelData;
            L1[0, 0] = GreyModel1[0, 0];
            for (int i = 1; i < ModelData.Rows; i++)
            {
                L1[i, 0] = L1[i - 1, 0] + GreyModel1[i, 0];
                L[i - 1, 0] = GreyModel1[i, 0];
            }
            
            ArrayMatrix A = new ArrayMatrix(ModelData.Rows-1, 2);
            for (int i = 0; i < ModelData.Rows-1; i++)
            {
                A[i, 0] = -(L1[i, 0] + L1[i + 1, 0]) / 2.0;
                A[i, 1] = 1;
            }
            if (A[0, 0] * A[1, 1] - A[0, 1] * A[1, 0] == 0)
                return GreyModelX;
            ArrayMatrix X = (A.Transpose() * A).Inverse * (A.Transpose() * L);
            double a = X[0, 0];
            double u = X[1, 0];
            GreyModelX[0, 0] = GreyModel1[0, 0];
            for (int i = 1; i < N; i++)
            {
                GreyModelX[i, 0] = (1.0 - Math.Exp(a)) * (GreyModelX[0, 0] - u / a) * Math.Exp(-a * i);
            }
            if (ModelData.MinValue < 0)
                for (int i = 0; i < GreyModelX.Rows; i++) { GreyModelX[i, 0] = (GreyModelX[i, 0] - MaxValueofModelData * 10); }//
            return GreyModelX;
        }
        /// <summary>
        /// 抗差灰色模型
        /// </summary>
        /// <param name="ModelData"></param>
        /// <param name="PredictedLength"></param>
        /// <returns></returns>
        public static ArrayMatrix GetRobustGreyModelX(ArrayMatrix ModelData, int PredictedLength)
        {
            int N = ModelData.Rows + PredictedLength;
            ArrayMatrix RobustGreyModelX = new ArrayMatrix(N, 1);
            ArrayMatrix GreyModel1 = new ArrayMatrix(ModelData.Rows, 1);
            ArrayMatrix L1 = new ArrayMatrix(ModelData.Rows, 1);
            ArrayMatrix L = new ArrayMatrix(ModelData.Rows - 1, 1);

            for (int i = 0; i < ModelData.Rows; i++) { GreyModel1[i, 0] = ModelData[i, 0] + ModelData.MaxValue * 10; }//;
            L1[0, 0] = GreyModel1[0, 0];
            for (int i = 1; i < ModelData.Rows; i++)
            {
                L1[i, 0] = L1[i - 1, 0] + GreyModel1[i, 0];
                L[i - 1, 0] = GreyModel1[i, 0];
            }

            ArrayMatrix A = new ArrayMatrix(ModelData.Rows - 1, 2);
            for (int i = 0; i < ModelData.Rows - 1; i++)
            {
                A[i, 0] = -(L1[i, 0] + L1[i + 1, 0]) / 2.0;
                A[i, 1] = 1;
            }
            if (A[0, 0] * A[1, 1] - A[0, 1] * A[1, 0] == 0)
                return RobustGreyModelX;
            ArrayMatrix X = (A.Transpose() * A).Inverse * (A.Transpose() * L);
            double a = X[0, 0];
            double u = X[1, 0];
            double a1 = 0;
            double u1 = 0;

            RobustGreyModelX[0, 0] = GreyModel1[0, 0];
            for (int i = 1; i < N; i++)
            {
                RobustGreyModelX[i, 0] = (1.0 - Math.Exp(a)) * (RobustGreyModelX[0, 0] - u / a) * Math.Exp(-a * i);
            }
            double difference1 = 1.0; double difference2 = 1.0;
            double k0 = 0;
            double k1 = 0;
            double dk=0;
            double SSE1 = 0;
            ArrayMatrix dPloy = new ArrayMatrix(ModelData.Rows, 1);
            ArrayMatrix P = ArrayMatrix.EyeMatrix(ModelData.Rows-1,1);
            while (difference1 > 1E-9 || difference2 > 1E-9)
            {
                a = X[0, 0];
                u = X[1, 0];
                for (int i = 0; i < dPloy.Rows; i++)
                    dPloy[i,0] = RobustGreyModelX[i,0] - GreyModel1[i,0];
                SSE1 = Math.Sqrt((dPloy.Transpose()*dPloy)[0, 0] / (ModelData.Rows * 1.0));
                k0 = 2.5 * SSE1;
                k1 = 3.5 * SSE1;
                dk = k1 - k0;
                for (int i = 0; i < dPloy.Rows; i++)
                {
                    if (Math.Abs(dPloy[i, 0]) > k1)
                        P[i, i] = 0;
                    else if (Math.Abs(dPloy[i, 0]) > k0 && Math.Abs(dPloy[i, 0]) < k1)
                        P[i, i] = (k1 - Math.Abs(dPloy[i+1, 0])) * (k1 - Math.Abs(dPloy[i+1, 0])) * k0 / (dk * dk * Math.Abs(dPloy[i+1, 0]));
                }
                X = (A.Transpose() * P * A).Inverse * (A.Transpose() * P * L);
                a1 = X[0, 0];
                u1 = X[1, 0];
                for (int i = 1; i < N; i++)
                {
                    RobustGreyModelX[i, 0] = (1.0 - Math.Exp(a1)) * (RobustGreyModelX[0, 0] - u1 / a1) * Math.Exp(-a1 * i);
                }
                difference1 = Math.Abs(a1 - a);
                difference2 = Math.Abs(u1 - u);
            }
            for (int i = 0; i < RobustGreyModelX.Rows; i++) { RobustGreyModelX[i, 0] = (RobustGreyModelX[i, 0] - ModelData.MaxValue * 10); }//
            return RobustGreyModelX;
        }
        #endregion

        #region 钟差建模数据的Allan方差、Hardamard方差解算
        /// <summary>
        /// 求解钟差建模数据的Allan方差
        /// </summary>
        /// <param name="IntervalSecond"></param>
        /// <param name="N"></param>
        /// <param name="ModelData"></param>
        /// <returns></returns>
        public static ArrayMatrix GetAllanDevariation(int IntervalSecond, int N, ArrayMatrix ModelData)
        {
            ArrayMatrix AllanDevariation=new ArrayMatrix (N,1);
            for (int i = 2; i <= (N - 1) / 2.0 ; i++)
            {
                int a = (int)Math.Floor((double)N / i) + 1;
                int IntervalTime = i * IntervalSecond;
                double sum=0;
                
                for (int j = 0; j < a - 2; j++)
                {
                    sum += ModelData[j + 2 * i,0] - 2 * ModelData[j + i,0] + ModelData[j,0];
                }
                AllanDevariation[i - 2,0] = (1.0 / (2 * (a - 2) * IntervalTime * IntervalTime)) * sum;
            }
            return AllanDevariation;
        }
        /// <summary>
        /// 根据Allan方差求解状态方程对应的协方差函数
        /// </summary>
        /// <param name="N"></param>
        /// <param name="T"></param>
        /// <param name="L"></param>
        /// <returns></returns>
        public static ArrayMatrix GetCovarianceByAllanDerivation(int N, ArrayMatrix Time, ArrayMatrix AllanDevariantion)
        {
            double a = (N - 1) / 2.0 - 1;
            double[][] T = Time.Array;
            int a1 = (int)Math.Floor(a);
            ArrayMatrix A = new ArrayMatrix(a1, 4);
            for (int i = 1; i < (N - 1) / 2.0 - 1; i++)
            {
                A[i - 1, 0] = 3 * (1 / (T[i - 1][0] * T[i - 1][0]));
                A[i - 1, 1] = 1 / T[i - 1][0];
                A[i - 1, 2] = T[i - 1][0] / 3;
                A[i - 1, 3] = T[i - 1][0] * T[i - 1][0] * T[i - 1][0] / 20;
            }
            ArrayMatrix Q = new ArrayMatrix(4, 4);
            Q = ((A.Transpose() * A).Inverse) * (A.Transpose() * AllanDevariantion);
            return Q;
        }
        /// <summary>
        /// 求解hardamard方差
        /// </summary>
        /// <param name="IntervalSecond"></param>
        /// <param name="N"></param>
        /// <param name="ModelData"></param>
        /// <returns></returns>
        public static ArrayMatrix GetHardamardDevariation(int IntervalSecond, int N, ArrayMatrix ModelData)
        {
            ArrayMatrix HardamardDevariation = new ArrayMatrix(N, 1);
            for (int i = 1; i <= (N - 1) / 3.0; i++)
            {
                int IntervalTime = i * IntervalSecond;
                double sum = 0;
                for (int j = 0; j <= N - 3 * i; j++)
                {
                    sum += ModelData[j + 3 * i, 0] - 3 * ModelData[j + 2 * i, 0] - ModelData[j, 0];
                }
                HardamardDevariation[i - 1, 0] = (1.0 / (6 * (N - 3 * i) * IntervalTime * IntervalTime)) * sum;
            }
            return HardamardDevariation;
        }
        /// <summary>
        /// 求解钟差建模数据的Hardamard协方差矩阵
        /// </summary>
        /// <param name="N"></param>
        /// <param name="Time"></param>
        /// <param name="AllanDevariantion"></param>
        /// <returns></returns>
        public static ArrayMatrix GetCovarianceByHardamardDevariation(int N, ArrayMatrix Time, ArrayMatrix AllanDevariantion)
        {
            double a = (N - 1) / 2.0 - 1;
            double[][] T = Time.Array;
            int a1 = (int)Math.Floor(a);
            ArrayMatrix A = new ArrayMatrix(a1, 4);
            for (int i = 1; i < (N - 1) / 2.0 - 1; i++)
            {
                A[i - 1, 0] = 3 * (1 / (T[i - 1][0] * T[i - 1][0]));
                A[i - 1, 1] = 1 / T[i - 1][0];
                A[i - 1, 2] = T[i - 1][0] / 3;
                A[i - 1, 3] = T[i - 1][0] * T[i - 1][0] * T[i - 1][0] / 20;
            }
            ArrayMatrix Q = new ArrayMatrix(4, 4);
            Q = ((A.Transpose() * A).Inverse) * (A.Transpose() * AllanDevariantion);
            return Q;
        }
        #endregion
        /// <summary>
        /// 获得钟差对应的时间矩阵
        /// </summary>
        /// <param name="N"></param>
        /// <param name="IntervalSecond"></param>
        /// <returns></returns>
        public static ArrayMatrix  GetTimeMatrix(int N,int IntervalSecond)
        {
            ArrayMatrix T = new ArrayMatrix((int)((N-1.0)/2.0+1),1);
            for(int i=0;i<(N-1)/2.0-1;i++)
            {
                T[i,0]=IntervalSecond*(i+1);
            }
            return T;
        }
        #region Kalman滤波解算及其递推解
        /// <summary>
        /// 求解初始的噪声协方差矩阵
        /// </summary>
        /// <param name="q1"></param>
        /// <param name="q2"></param>
        /// <param name="q3"></param>
        /// <param name="IntervalSecond"></param>
        /// <returns></returns>
        public static ArrayMatrix GetInitialCovarianceW( double q1, double q2, double q3, int IntervalSecond)
        {
            ArrayMatrix InitialCovarianceW = new ArrayMatrix(3, 3);
            double[][] InitialCovarianceW1 = InitialCovarianceW.Array;
            InitialCovarianceW1[0][0] = q1 * IntervalSecond + (q2 * IntervalSecond * IntervalSecond * IntervalSecond) / 3.0 + (q3 * IntervalSecond * IntervalSecond * IntervalSecond * IntervalSecond * IntervalSecond) / 20.0;
            InitialCovarianceW1[0][1] = (q2 * IntervalSecond * IntervalSecond) / 2.0 + (q3 * IntervalSecond * IntervalSecond * IntervalSecond * IntervalSecond) / 8.0;
            InitialCovarianceW1[0][2] = (q3 * IntervalSecond * IntervalSecond * IntervalSecond) / 6.0;
            InitialCovarianceW1[1][0] = (q2 * IntervalSecond * IntervalSecond) / 2.0 + (q3 * IntervalSecond * IntervalSecond * IntervalSecond * IntervalSecond) / 8.0;
            InitialCovarianceW1[1][1] = q2 * IntervalSecond + (q3 * IntervalSecond * IntervalSecond * IntervalSecond) / 3.0;
            InitialCovarianceW1[1][2] = (q3 * IntervalSecond * IntervalSecond ) / 2.0;
            InitialCovarianceW1[2][0] = (q3 * IntervalSecond * IntervalSecond * IntervalSecond) / 6.0;
            InitialCovarianceW1[2][1] = (q2 * IntervalSecond * IntervalSecond) / 2.0;
            InitialCovarianceW1[2][2] = q3 * IntervalSecond ;
            return InitialCovarianceW;
        }
        /// <summary>
        /// 返回转移矩阵
        /// </summary>
        /// <param name="IntervalSeconds">数据采样间隔</param>
        /// <returns></returns>
        public static ArrayMatrix GetTransformMatrix(int IntervalSeconds)
        {
            ArrayMatrix TransformMatrix=new ArrayMatrix(3,3);
            double [][]TransformMatrix1=TransformMatrix.Array;
            
            TransformMatrix1[0][0] = 1;
            TransformMatrix1[0][1] = IntervalSeconds;
            TransformMatrix1[0][2] = (IntervalSeconds * IntervalSeconds) / 2.0;
            TransformMatrix1[1][0] = 0;
            TransformMatrix1[1][1] = 1;
            TransformMatrix1[1][2] = IntervalSeconds;
            TransformMatrix1[2][0] =0;
            TransformMatrix1[2][1] = 0;
            TransformMatrix1[2][2] = 1;

            return TransformMatrix;
        }
        
        /// <summary>
        /// 利用Kalman滤波进行求解
        /// </summary>
        /// <param name="N"></param>
        /// <param name="InitialX"></param>
        /// <param name="CovarianceV"></param>
        /// <param name="InitialCovarianceX"></param>
        /// <param name="InitialCovarianceW"></param>
        /// <param name="L"></param>
        /// <param name="Q"></param>
        /// <returns></returns>
        public static ArrayMatrix GetKalmanFilter(int N, ArrayMatrix InitialX, ArrayMatrix CovarianceV, ArrayMatrix InitialCovarianceX, ArrayMatrix InitialCovarianceW, ArrayMatrix L, ArrayMatrix Q)
        {
            ArrayMatrix X = new ArrayMatrix(3, 1);
            ArrayMatrix CovEstimateX = new ArrayMatrix(3, 3);
            ArrayMatrix CovarianceX = new ArrayMatrix(3, 3);
            ArrayMatrix EstimateX = new ArrayMatrix(3, 1);
            ArrayMatrix J = new ArrayMatrix(3, 1);
            ArrayMatrix A = new ArrayMatrix(1, 3);
            A[0, 0] = 1; A[0, 1] = 0; A[0, 2] = 0;
            for (int i = 0; i < N; i++)
            {
                EstimateX = Q * InitialX;
                CovEstimateX = Q * InitialCovarianceX * Q.Transpose() + InitialCovarianceW;
                J = (CovEstimateX * A.Transpose() * (A * CovEstimateX * A.Transpose() + CovarianceV)).Inverse;
                ArrayMatrix m = A * EstimateX;
                X = EstimateX + J * (L[i,0] - m[0,0]);
                CovarianceX = (ArrayMatrix.EyeMatrix(3, 3) - J * A) * CovEstimateX;//在Matrix中添加了单位矩阵的生成
                InitialX = X;
                InitialCovarianceX = CovarianceX; 
            }
            return X;
        }
        /// <summary>
        /// 钟差数据的递推Kalman滤波解算
        /// </summary>
        /// <param name="N"></param>
        /// <param name="InitialX"></param>
        /// <param name="CovarianceV"></param>
        /// <param name="InitialCovarianceX"></param>
        /// <param name="InitialCovarianceW"></param>
        /// <param name="L"></param>
        /// <param name="Q"></param>
        /// <returns></returns>
        public static ArrayMatrix GetKalmanFilterRecursion(int N, ArrayMatrix InitialX, ArrayMatrix CovarianceV, ArrayMatrix InitialCovarianceX, ArrayMatrix InitialCovarianceW, ArrayMatrix L, ArrayMatrix Q)
        {
            ArrayMatrix X = new ArrayMatrix(3, 1);
            ArrayMatrix CovEstimateX = new ArrayMatrix(3, 3);
            ArrayMatrix CovarianceX = new ArrayMatrix(3, 3);
            ArrayMatrix EstimateX = new ArrayMatrix(3, 1);
            ArrayMatrix J = new ArrayMatrix(3, 1);
            ArrayMatrix A = new ArrayMatrix(1, 3);
            A[0, 0] = 1; A[0, 1] = 0; A[0, 2] = 0;
            for (int i = 0; i < N; i++)
            {
                EstimateX = Q * InitialX;
                CovEstimateX = Q * InitialCovarianceX * Q.Transpose() + InitialCovarianceW;
                J = (CovEstimateX * A.Transpose() * (A * CovEstimateX * A.Transpose() + CovarianceV)).Inverse;
                ArrayMatrix m = A * EstimateX;
                X = EstimateX + J * (L[i, 0] - m[0, 0]);
                CovarianceX = (ArrayMatrix.EyeMatrix(3, 1) - J * A) * CovEstimateX;//在Matrix中添加了单位矩阵的生成
                InitialCovarianceW = ArrayMatrix.EyeMatrix(3, 0.5) * InitialCovarianceW + ArrayMatrix.EyeMatrix(3, 0.5) * (X - Q * InitialX) * (X - Q * InitialX).Transpose();
                InitialX = X;
                InitialCovarianceX = CovarianceX;
            }
            return X;
        }
        #endregion
        public static ArrayMatrix GetA5(int N,int IntervalSecond, ArrayMatrix ModelData)
        {
            
            ArrayMatrix A5 = new ArrayMatrix(N, 3);
            //Matrix a1=Matrix.OneMatrix(N);//在Matrix中添加了一个生成一列全为1的数组
            //Matrix a2 = new Matrix(N, 1);
            //Matrix a3 = new Matrix(N, 1);
            for (int i = 0; i < N;i++ )
            {
                A5[i, 0] = 1;
                A5[i, 1] = i * IntervalSecond;
                A5[i, 2] = i * IntervalSecond * i * IntervalSecond;
            }
            
            return A5;
        }
        /// <summary>
        /// 获取模型的拟合值
        /// </summary>
        /// <param name="ModelX"></param>
        /// <param name="N"></param>
        /// <returns></returns>
        public static ArrayMatrix GetModelPolyX(ArrayMatrix ModelX, int N)
        {
            ArrayMatrix PolyX = new ArrayMatrix(N, 1);
            for (int i = 0; i < N; i++)
            {
                PolyX[i, 0] = ModelX[i, 0];
            }
            return PolyX;
        }
        /// <summary>
        /// 获取模型的预报值
        /// </summary>
        /// <param name="ModelX"></param>
        /// <param name="N"></param>
        /// <returns></returns>
        public static ArrayMatrix GetModelPredictedX(ArrayMatrix ModelX, int N)
        {
            ArrayMatrix PredictedX = new ArrayMatrix(ModelX.Rows - N, 1);
            for (int i = 0; i < ModelX.Rows - N; i++)
            {
                PredictedX[i, 0] = ModelX[N + i, 0];
            }
            return PredictedX;
        }
        #region 钟差数据的预报数据及其对应的真实数据
        /// <summary>
        /// 获取模型的预报值
        /// </summary>
        /// <param name="x0"></param>
        /// <param name="x1"></param>
        /// <param name="x2"></param>
        /// <param name="IntervalSecond"></param>
        /// <param name="PredictedLength"></param> 
        /// <returns></returns>
        public static ArrayMatrix GetPredictedData(double x0, double x1, double x2, int IntervalSecond, int PredictedLength)
        {
            ArrayMatrix PredictedData = new ArrayMatrix(PredictedLength, 1);
            for (int i = 0; i < PredictedLength; i++)
            {
                PredictedData[i, 0] = x0 + x1 * (i + 1) * IntervalSecond + x2 * (i + 1) * IntervalSecond * (i + 1) * IntervalSecond;
            }
            return PredictedData;
        }
        /// <summary>
        /// 获取预报值对应的真值
        /// </summary>
        /// <param name="CompareData"></param>
        /// <param name="PredictedLength"></param>
        /// <returns></returns>
        public static ArrayMatrix GetPredictedRealData(ArrayMatrix CompareData, int PredictedLength)
        {
            ArrayMatrix PredictedRealData = new ArrayMatrix(PredictedLength, 1);
            for (int i = 0; i < PredictedLength; i++)
            {
                PredictedRealData[i, 0] = CompareData[i, 0];
            }
            return PredictedRealData;
        }
        #endregion
        #region 精度评定
        /// <summary>
        /// 获取预报误差，若真值缺失，则记为0
        /// </summary>
        /// <param name="CompareData"></param>
        /// <param name="PredictedLength"></param>
        public void GetPredictedError(ArrayMatrix CompareData, int PredictedLength)
        {
            PredictedError = PredictedData - CompareData;

            for (int i = 0; i < PredictedLength; i++)
            {
                if (CompareData[i, 0] == 0)
                    PredictedError[i, 0] = 0;
            }
        }
        /// <summary>
        /// 求解残差的中误差值
        /// </summary>
        /// <param name="error"></param>
        /// <param name="Length"></param>
        /// <returns></returns>
        public static double GetRMS(ArrayMatrix error,int Length)
        {
            double RMS = 0;
            for (int i = 0; i < Length; i++)
                RMS += error[i, 0] * error[i, 0];
            return Math.Sqrt(RMS/Length);
        }
        #endregion
        
        
    }
    
}

