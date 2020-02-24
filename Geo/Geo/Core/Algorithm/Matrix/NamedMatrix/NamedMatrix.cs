// 2014.09.10, czs, create in 海鲁吐， 具有名称的矩阵

using System;
using System.Collections.Generic;
using System.Text;  using Geo.Algorithm; 
using Geo.Algorithm.Adjust;  
using Geo.Utils; 
using Geo.Common;


namespace Geo.Algorithm
{ 
    /// <summary>
    /// 命名的矩阵。通常用于矩阵行里变换。
    /// </summary>
    public class NamedMatrix
    {
        /// <summary>
        /// 构造函数。
        /// </summary>
        /// <param name="paramNames">矩阵对应行列的名称</param>
        /// <param name="matrix">矩阵</param>
        public NamedMatrix(List<string> paramNames, double[][] matrix)
        {
            dict = new Dictionary<string, NamedVector>();
            int oldLen = matrix.Length;
            for (int i = 0; i < oldLen; i++)
            {
                string name = paramNames[i];
                NamedVector cov = new NamedVector(name);
                for (int j = 0; j < oldLen; j++)
                {
                    cov.SetValue(paramNames[j], matrix[i][j]);
                }
                dict.Add(name, cov);
            }
        }
        #region 核心变量、属性
        Dictionary<string, NamedVector> dict;
        #endregion
        /// <summary>
        /// 根据新的名称列表，生成新的矩阵。
        /// </summary>
        /// <param name="newParamNames">新矩阵行列名称</param>
        /// <param name="DefaultValue">默认非对角线数值</param>
        /// <param name="DefaultDiagonalValue">默认对角线数值的平方根</param>
        /// <returns></returns>
        public double[][] GetNewMatrix(List<string> newParamNames, double defaultVal, double DefaultDiagonalValue)
        {
            int newLen = newParamNames.Count;
            double[][] newMatrix = MatrixUtil.CreateDiagonal(newLen, 0);
            for (int i = 0; i < newLen; i++)
            {
                string name = newParamNames[i];
                NamedVector sat = null;
                if (dict.ContainsKey(name))
                {
                    sat = dict[name];
                }
                else
                {
                    sat = new NamedVector(name, defaultVal, DefaultDiagonalValue);
                }
                for (int j = 0; j < newLen; j++)
                {
                    newMatrix[i][j] = sat.GetValue(newParamNames[j]);
                }
            }
            return newMatrix;
        }
        /// <summary>
        /// 根据新的名称列表，生成新的矩阵。
        /// </summary>
        /// <param name="newParamNames">新矩阵行列名称</param>
        /// <param name="oldParamNames">旧矩阵行列名称</param>
        /// <param name="oldMatrix">旧矩阵</param>
        /// <returns></returns>
        public static IMatrix GetNewMatrix(List<string> newParamNames, List<string> oldParamNames, double[][] oldMatrix, Double defaultVal = 0, double DefaultDiagonalValue = 1e10)
        {
            if(newParamNames.Count == 0)
            {
                return new Matrix(0);
            }
            NamedMatrix matrix = new NamedMatrix(oldParamNames, oldMatrix);
            double[][] matriResult = matrix.GetNewMatrix(newParamNames, defaultVal, DefaultDiagonalValue);
            return new ArrayMatrix(matriResult);
        }
        /// <summary>
        /// 根据新的名称列表，生成新的矩阵。
        /// </summary>
        /// <param name="newParamNames">新矩阵行列名称</param>
        /// <param name="oldParamNames">旧矩阵行列名称</param>
        /// <param name="oldMatrix">旧矩阵</param>
        /// <returns></returns>
        public static IMatrix GetSymmetricInOrder(List<string> newParamNames, List<string> oldParamNames, double[][] oldMatrix, Double defaultVal = 0, double DefaultDiagonalValue = 1e10)
        {
            NamedMatrix matrix = new NamedMatrix(oldParamNames, oldMatrix);
            double[][] matriResult = matrix.GetNewMatrix(newParamNames, defaultVal, DefaultDiagonalValue);
            return new  SymmetricMatrix(matriResult);
        }
         
    }

}