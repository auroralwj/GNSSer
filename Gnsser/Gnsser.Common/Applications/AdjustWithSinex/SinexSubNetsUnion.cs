//2013.03.28， czs ,  Created.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using Geo.Coordinates;
using Geo.Utils;
using Geo.Algorithm.Adjust;
using Gnsser.Data.Sinex;

namespace Gnsser.Service
{ 
    /// <summary>
    /// 两个具有公共点的子网进行联合整体平差。
    /// </summary>
    public  class SinexSubNetsUnion
    {
        public SinexSubNetsUnion(SinexFile fileA, SinexFile fileB)
        {
            this.fileA = fileA;
            this.fileB = fileB;

            double[][] apriori = Adjust(fileA, fileB);
            BuildResultSinex(fileA, fileB, apriori);
        }

        /// <summary>
        /// 构建参数平差观测方程，并进行平差计算。
        /// </summary>
        /// <param name="fileA"></param>
        /// <param name="fileB"></param>
        /// <returns></returns>
        private double[][] Adjust(SinexFile fileA, SinexFile fileB)
        {
            //检查一下是否只包含坐标，如否，则清理。
            if (!fileA.IsOnlyEstimateCoordValue) fileA.CleanNonCoordSolutionValue();
            if (!fileB.IsOnlyEstimateCoordValue) fileB.CleanNonCoordSolutionValue();

            double[][] A = SinexSubNetsUnion.GetCoeffMatrixOfParams(fileA, fileB);
            double[][] Q = SinexSubNetsUnion.GetCovaMatrixOfObs(fileA, fileB);
            double[][] obsMinusApriori = SinexSubNetsUnion.GetObsMinusApriori(fileA, fileB);
            var pa = new ParamAdjuster();

            p = pa.Run(new AdjustObsMatrix( A, obsMinusApriori, Q));

            double[][] apriori = GetApriori(fileA, fileB);


            //MatrixUtil.SaveToText(A, @"C:\A.txt");
            //MatrixUtil.SaveToText(Q, @"C:\Q.txt");
            //MatrixUtil.SaveToText(obsMinusApriori, @"C:\l.txt");
            //MatrixUtil.SaveToText(apriori, @"C:\D.txt");

            xyzs = GetXyzs( MatrixUtil.GetPlus(p.Estimated.OneDimArray, apriori));

            geoCoords = new List<GeoCoord>();
            foreach (var item in xyzs)
            {
                geoCoords.Add(CoordTransformer.XyzToGeoCoord(item));
            }

            result = SinexMerger.EmergeBasic(fileA, fileB);
            return apriori;
        }

        public override string ToString()
        {
            if(p!=null) return p.ToString();
            return "SinexSubNetsUnion does not executed.";
        }
        /// <summary>
        /// 依据输入和平差信息，构建平差结果Sinex文件
        /// </summary>
        /// <param name="fileA"></param>
        /// <param name="fileB"></param>
        /// <param name="apriori"></param>
        private void BuildResultSinex(SinexFile fileA, SinexFile fileB, double[][] apriori)
        {
            //统计信息
            SinexStatistic stat = SinexStatistic.Merge(fileA.GetStatistic(), fileB.GetStatistic());
            stat.NumberOfDegreesOfFreedom = p.Freedom;
            stat.NumberOfObservations = p.ObsMatrix.Observation.Count;
            stat.NumberOfUnknown = p.ParamCount;
            stat.VarianceOfUnitWeight = p.VarianceOfUnitWeight;
            result.SolutionStattisticsBlock.Items = stat.GetSolutionStatistics();
            //先验值
            result.SolutionAprioriBlock.Items.AddRange(fileA.SolutionAprioriBlock.Items);
            foreach (var item in fileB.SolutionAprioriBlock.Items)
            {
                if (!result.SolutionAprioriBlock.Items.Contains(item)) result.SolutionAprioriBlock.Items.Add(item);
            }

            //测站估值
            result.SolutionEstimateBlock.Items.AddRange(fileA.SolutionEstimateBlock.Items);
            foreach (var item in fileB.SolutionEstimateBlock.Items)
            {
                if (!result.SolutionEstimateBlock.Items.Contains(item)) result.SolutionEstimateBlock.Items.Add(item);
            }
            int i = 0;
            foreach (var item in result.SolutionEstimateBlock.Items)
            {
                item.ParameterValue = apriori[i][0];
                item.StdDev = p.CovaOfEstimatedParam[i,i];
                i++;
            }
            //矩阵
            result.SolutionMatrixEstimateCova.Items = SinexMatrixConvertor.GetMatrixLines(p.CovaOfEstimatedParam.Array);
        }
        

        SinexFile fileA, fileB;
        SinexFile result;

        public SinexFile ResultSinexFile { get { return result; } }

        List<GeoCoord> geoCoords;
        List<XYZ> xyzs;
        public List<GeoCoord> GetGeoCoords()
        {
            return geoCoords;
        }
        public List<XYZ> GetXyzs()
        {
            return xyzs;
        } 

        AdjustResultMatrix p;        
        
        /// <summary>
        /// 从矩阵向量中提取坐标值，要求矩阵的首行列必须是XYZ顺序排序。
        /// </summary>
        /// <param name="matrix"></param>
        /// <returns></returns>
        public static List<XYZ> GetXyzs(double[][] matrix)
        {
            List<XYZ> xyzs = new List<XYZ>();
            if (matrix.Length % 3 != 0) throw new ArgumentException("列向量长度必须为3的整数倍");
            for (int i = 0; i < matrix.Length; )
            {
                xyzs.Add(new XYZ(matrix[i++][0], matrix[i++][0], matrix[i++][0]));
            }
            return xyzs;
        }

        /// <summary>
        /// 未知参数的先验值。是一个列向量.
        /// </summary>
        /// <param name="fileA"></param>
        /// <param name="fileB"></param>
        /// <returns></returns>
        public static double[][] GetApriori(SinexFile fileA, SinexFile fileB)
        { 

            int row = SinexFile.GetDistinctSiteCodes(fileA, fileB).Count * 3;
            double[][] array = MatrixUtil.Create(row, 1);

            int j = 0;
            foreach (var item in fileA.SolutionAprioriBlock.Items)
            {
                array[j++][0] = item.ParameterValue;
            }

            List<string> sames = SinexFile.GetSameSiteCodes(fileA, fileB);
            foreach (var item in fileB.SolutionAprioriBlock.Items)
            {
                if (!sames.Contains(item.SiteCode))
                {
                    array[j++][0] = item.ParameterValue;
                }
            }
            return array;
        }

        /// <summary>
        /// 获取矩阵l，为观测值减去先验值，是一个列向量。
        /// </summary>
        /// <param name="fileA"></param>
        /// <param name="fileB"></param>
        /// <returns></returns>
        public static double[][] GetObsMinusApriori(SinexFile fileA, SinexFile fileB)
        {
            int paraCountA = fileA.EstimateParamCount;
            int row = paraCountA + fileB.EstimateParamCount;
            double[][] array = MatrixUtil.Create(row, 1);

            double[] estiamtedA = fileA.GetEstimateVector();
            double[] aprioriA = fileA.GetAprioriVector();
            double[] estiamtedB = fileB.GetEstimateVector();
            double[] aprioriB = fileB.GetAprioriVector();

            for (int i = 0; i < paraCountA; i++) 
                array[i][0] = estiamtedA[i] - aprioriA[i];

            for (int i = paraCountA; i < paraCountA + estiamtedB.Length; i++)  
                array[i][0] = estiamtedB[i - paraCountA] - aprioriB[i - paraCountA];

            return array;
        }

        /// <summary>
        /// 提取观测量的权逆阵。是否只提取对角阵？？
        /// </summary>
        /// <param name="fileA"></param>
        /// <param name="fileB"></param>
        /// <returns></returns>
        public static double[][] GetCovaMatrixOfObs(SinexFile fileA, SinexFile fileB)
        {
            int paramCountA = fileA.EstimateParamCount;
            int row = paramCountA + fileB.EstimateParamCount;
            double[][] array =  MatrixUtil.Create(row);

            //fileA直接设置。
            //这里应该定权！
            SinexStatistic statisticA = fileA.GetStatistic();
            SinexStatistic statisticB = fileB.GetStatistic();

            SinexStatistic statisticNew = SinexStatistic.Merge(statisticA, statisticB);
            double varFactorA = statisticNew.VarianceOfUnitWeight / statisticA.VarianceOfUnitWeight;
            double varFactorB = statisticNew.VarianceOfUnitWeight / statisticB.VarianceOfUnitWeight;

            double[][] matrixA = fileA.GetEstimateCovaMatrix();
            double[][] matrixB = fileB.GetEstimateCovaMatrix();

            MatrixUtil.Multiply(matrixA, varFactorA);
            MatrixUtil.Multiply(matrixB, varFactorB);

            MatrixUtil.SetSubMatrix(array, matrixA);
            MatrixUtil.SetSubMatrix(array, matrixB, paramCountA, paramCountA);

            return array;
        }

        /// <summary>
        /// 根据两个Sinex文件，提取系数阵。就是参数平差的参数系数阵。
        /// Get coe Of Params Matrix Of Params
        /// </summary>
        /// <param name="fileA"></param>
        /// <param name="fileB"></param>
        /// <returns></returns>
        public static double[][] GetCoeffMatrixOfParams(SinexFile fileA, SinexFile fileB)
        {
            List<string> paramCodes = SinexFile.GetDistinctSiteCodes(fileA, fileB);
            List<string> codesA = fileA.GetSiteCods();
            List<string> codesB = fileB.GetSiteCods();
            List<int> sameParamIndexesInB = new List<int>();//B文件中具有相同测站的代码的索引号。
            for (int i = 0; i < codesB.Count; i++)
                if (codesA.Contains(codesB[i])) sameParamIndexesInB.Add(i);
             
            int paramCountA = fileA.EstimateParamCount;//文件A的参数，一个测站有三个参数X,Y,Z
            int row = paramCountA + fileB.EstimateParamCount;//观测数量包含A和B
            int col = paramCodes.Count * 3;//新参数数量（除去了A,B共有参数）
            double[][] array = MatrixUtil.Create(row, col);

            int indentCount = 0;//有多少参数相同，则缩进多少位置
            for (int i = 0; i < paramCountA; i++) array[i][i] = 1;//先A
            for (int i = paramCountA; i < row; i++)//再B
            {
                int siteIndexInB = (i - paramCountA) / 3;//测站名在B中的索引
                if (sameParamIndexesInB.Contains(siteIndexInB))
                {
                    int colInA = codesA.IndexOf(codesB[siteIndexInB]) * 3;//在A中的索引
                    array[i][colInA] = 1;
                    array[++i][colInA + 1] = 1;
                    array[++i][colInA + 2] = 1;

                    indentCount += 3;//一个测站缩进三单位
                }
                else  array[i][i - indentCount] = 1; 
            }

            return array;
        }
         

    }
}
