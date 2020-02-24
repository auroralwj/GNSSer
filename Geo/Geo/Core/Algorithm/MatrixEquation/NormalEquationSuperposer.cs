//2018.04.18, czs, created in hmx, 法方程叠加器


using System;
using System.Collections.Generic;
using System.Text;
using Geo.Algorithm;
using Geo.Utils;
using Geo.IO;
using Geo.Algorithm.Adjust;

//平差：在有多余观测的基础上，根据一组含有误差的观测值，
//依一定的数学模型，按某种平差准则，求出未知量的最优估值，并进行精度评定。
namespace Geo.Algorithm
{
    /// <summary>
    /// 法方程实时叠加器，适合于导航计算。 
    /// 需要一定的积累数据，计算固定参数，然后再一边修正固定参数，一边输出易变（状态）参数。
    /// 为了保证能用，第一个也输出，只不过精度有限。
    /// </summary>
    public class NormalEquationSuperposer 
    {
        Log log = new Log(typeof(NormalEquationSuperposer));

        /// <summary>
        /// 构造函数
        /// </summary>
        public NormalEquationSuperposer() {
            NormalEquations = new List<MatrixEquation>();
        }
        /// <summary>
        /// 用于探测参数是否改变
        /// </summary>
        public bool IsParamNameChanged { get; set; }
        /// <summary>
        /// 当前叠加结果
        /// </summary>
        public MatrixEquation CurrentAccumulated { get; set; }
        /// <summary>
        /// 最后的法方程
        /// </summary>
        public MatrixEquation LastEquation { get; set; }

        /// <summary>
        /// 历史法方程。成对的 N U 矩阵
        /// </summary>
        public List<MatrixEquation> NormalEquations { set; get; }
        /// <summary>
        /// 法方程数量
        /// </summary>
        public int Count { get => NormalEquations.Count; }

        /// <summary>
        /// 添加一个
        /// </summary>
        /// <param name="N"></param>
        /// <param name="U"></param>
        public void Add(Matrix N, Matrix U)
        {
            Add(new MatrixEquation(N, U));
        }

        /// <summary>
        /// 添加一个
        /// </summary>
        /// <param name="pairNU"></param>
        public void Add(Pair<Matrix> pairNU)
        {
            Add(new MatrixEquation(pairNU.First, pairNU.Second)); 
        }

        /// <summary>
        /// 添加一个
        /// </summary>
        /// <param name="newNormal"></param>
        public void Add(MatrixEquation newNormal)
        {
            if (newNormal == LastEquation || newNormal.Equals(LastEquation))
            {
                log.Warn("重复添加，已忽略。");
                return;
            }
            this.LastEquation = newNormal;

            NormalEquations.Add(newNormal);

            //添加同时也计算
            if (CurrentAccumulated == null)
            {
                CurrentAccumulated = newNormal;
            }
            else
            {                               
                CurrentAccumulated = CurrentAccumulated.AddNormal( newNormal);
            }

        }
         

        #region 计算 
        /// <summary>
        /// 获取叠加后法方程的结果
        /// </summary>
        /// <returns></returns>
        public  WeightedVector GetEstimated()
        {
            if (CurrentAccumulated == null)
            {
                return null;
            } 
            var result = CurrentAccumulated.GetEstimated();
            if (false)//验证
            {
                var result2 =  Superposition(NormalEquations);
                int ii = 0;
            }

            return result;
        }
        /// <summary>
        /// 直接计算
        /// </summary>
        /// <param name="PairNUs"></param>
        /// <returns></returns>
        public static WeightedVector Superposition(List<MatrixEquation> PairNUs)
        {
            //step 2： 求固定参数 
            MatrixEquation eq = null; 
            foreach (var normal in PairNUs)
            { 
                if(eq == null) { eq = normal; continue; }

                eq += normal;
            } 

            return eq.GetEstimated();
        }
         
        #endregion

    }
}
