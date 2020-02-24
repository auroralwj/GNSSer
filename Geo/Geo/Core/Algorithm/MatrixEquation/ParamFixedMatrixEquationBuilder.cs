//2019.02.28, czs, create in hongqing, 带入消除已知固定参数，返回新的方程

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Geo.Algorithm.Adjust;

namespace Geo.Algorithm
{
    /// <summary>
    /// 带入消除已知固定参数，返回新的方程
    /// </summary>
    public class ParamFixedMatrixEquationBuilder
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public ParamFixedMatrixEquationBuilder(PeriodRmsedNumeralStoarge PeriodRmsedNumeralStoarge)
        {
            this.FixedParamService = PeriodRmsedNumeralStoarge;
        }
        /// <summary>
        /// 固定参数服务器
        /// </summary>
        public PeriodRmsedNumeralStoarge FixedParamService { get; set; }
        /// <summary>
        /// 构建消除固定参数后的新方程。
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public MatrixEquation Build(MatrixEquation input)
        {
            var oldParams = input.ParamNames;
            var oldCoeff = input.LeftSide;
            var oldObs = input.RightSide;
            var fixedParams = GetFixedParams(oldParams);
            var fixeParamList = fixedParams.Keys.ToList();
            Vector fixedVector = new Vector(fixedParams);

            //提取新阵
            List<string> newParams = Geo.Utils.ListUtil.GetExcept<string>(oldParams, fixeParamList);
            SparseMatrix newCoef = BuildNewCoeff(oldCoeff, newParams);
            SparseMatrix fixedParamCoef = BuildNewCoeff(oldCoeff, fixeParamList);

            Vector fixedObs = new Matrix(fixedParamCoef) * fixedVector;
            Vector newObs =  oldObs.Col(0) -fixedObs;
            WeightedVector newRight = new WeightedVector(newObs, input.QofU);
            MatrixEquation result = new MatrixEquation(newCoef, newRight);


            return result;
        }
         /// <summary>
         /// 根据参数名称获取新参数。
         /// </summary>
         /// <param name="oldCoeff"></param>
         /// <param name="newParams"></param>
         /// <returns></returns>
        private static SparseMatrix BuildNewCoeff( Matrix oldCoeff, List<string> newParams)
        {
            SparseMatrix newCoef = new SparseMatrix(newParams.Count, newParams.Count)
            {
                RowNames = oldCoeff.RowNames,
                ColNames = newParams
            };

            foreach (var row in oldCoeff.RowNames)//行不变
            {
                foreach (var col in newParams)
                {
                    var val = oldCoeff[row, col];
                    newCoef.Add(row, col, val);
                }
            }
            return newCoef;
        }
        /// <summary>
        /// 根据提供的固定参数服务，提取固定参数值。
        /// </summary>
        /// <param name="oldParams"></param>
        /// <returns></returns>
        private Dictionary<string, double> GetFixedParams(List<string> oldParams)
        {
            Dictionary<string, double> fixedParams = new Dictionary<string, double>();
            foreach (var item in oldParams)
            {
                var vals = FixedParamService.Get(item);
                if (vals == null || vals.Count == 0)
                {
                    continue;
                }
                fixedParams[item] = vals.First().Value;
            }
            return fixedParams;
        }
    } 
}
