//2019.03.05, czs, create in hongqing, 批量矩阵方程计算器

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Geo.Algorithm
{
    /// <summary>
    /// 批量矩阵方程计算器
    /// </summary>
    public class MultiMatrixEquationComputer
    {
        /// <summary>
        /// 默认构造函数
        /// </summary>
        public MultiMatrixEquationComputer()
        {
            MatrixEquations = new List<MatrixEquation>();
        }
        /// <summary>
        /// 后缀名称
        /// </summary>
        public string PostFixName { get; set; }
        /// <summary>
        /// 原始矩阵
        /// </summary>
        List<MatrixEquation> MatrixEquations { get; set; }
        /// <summary>
        /// 增加方程
        /// </summary>
        /// <param name="obj"></param>
        public void AddSubMatrix(MatrixEquation obj)
        {
            MatrixEquations.Add(obj);
        }

        public MatrixEquation Build()
        {
            //统计列参数
            List<string> paramNames = GetParamNames();
            //按照此顺序重新构建方程左手边
            foreach (var eqa in MatrixEquations)
            {
                eqa.ExpandObsParams(paramNames);
            }
            //分别计算
            MatrixEquation added = null;
            foreach (var item in MatrixEquations)
            {
                if(added == null)
                {
                    added = item.NormalEquation;
                }
                else
                {
                    added += item.NormalEquation;
                }
            }
            added.Name = "NormalIterOf"+ MatrixEquations.Count + "EqsOf" + PostFixName;
            return added;
        }

        private List<string> GetParamNames()
        {
            List<string> paramNames = new List<string>();
            foreach (var eq in MatrixEquations)
            {
                foreach (var name in eq.ParamNames)
                {
                    if (paramNames.Contains(name)) { continue; }
                    paramNames.Add(name);
                }
            }

            return paramNames;
        }
    }
}
