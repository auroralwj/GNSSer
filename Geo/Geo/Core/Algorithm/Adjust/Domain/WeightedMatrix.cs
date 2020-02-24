//2017.09.02, czs, create in hongqing, 加权矩阵，每个元素对应一个方差

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Geo.Algorithm;
using Geo.Utils;

namespace Geo.Algorithm.Adjust
{
    /// <summary>
    /// 加权矩阵，每个元素对应一个方差
    /// </summary>
    public class WeightedMatrix : Matrix
    {
        #region 构造函数 
        /// <summary>
        /// 具有权值的数据向量
        /// </summary>
        /// <param name="matrix">数据向量</param>
        /// <param name="inverseWeight">权逆阵,如果为null，则为单位阵</param> 
        public WeightedMatrix(double[][] matrix, double[][] inverseWeight = null)
            : base(matrix) 
        {
            SetInverseWeight(  new Matrix(inverseWeight));
        }

        public WeightedMatrix(IMatrix matrix, IMatrix inverseWeight = null)
            : base(matrix)
        {
            SetInverseWeight(inverseWeight);
        }
        /// <summary>
        /// 具有权值的数据向量
        /// </summary>
        /// <param name="vector">数据以矩阵形式初始化对角阵</param>
        /// <param name="inverseWeight">权逆阵,如果为null，则为单位阵</param>
        public WeightedMatrix(IVector vector, IMatrix inverseWeight = null)
            : base(vector)
        {  
            SetInverseWeight(inverseWeight);
        } 

        public void SetInverseWeight(IMatrix inverseWeight)
        {

            if (inverseWeight != null)
            {
                if (!inverseWeight.IsSquare) throw new ArgumentException("必须传入方阵！");
                if (this.RowCount != inverseWeight.ColCount) throw new ArgumentException("参数个数与权逆阵维数必须相等！");
            }
            else { inverseWeight = new DiagonalMatrix(this.RowCount, 1); }
            this.InverseWeight = inverseWeight;
        }
        #endregion 
        #region  核心变量 

        /// <summary>
        /// 参数的权逆阵（协因数阵） Inverse Weight Matrix（Cofactor Matrix ）of Some Vector。
        /// 协因数阵。InverseWeight=Weight^(-1)
        /// 法方程系数阵的逆阵为未知参数向量的权逆阵.
        /// ??此处应该为残差的权逆阵？？
        /// </summary> 
        public IMatrix InverseWeight { get; protected set; } 
        #endregion

        #region 计算属性
        /// <summary>
        /// 是否具有权值。
        /// </summary>
        public bool IsWeighted { get { return InverseWeight != null && InverseWeight.ColCount == this.RowCount; } }
         
        /// <summary>
        /// 权阵
        /// </summary>
        public IMatrix Weights { get { return  InverseWeight.GetInverse(); } } 

        #endregion
 
        public void Dispose()
        {
            base.Dispose();
            this.InverseWeight = null;  
        }

        #region  IO
        /// <summary>
        /// 提供可读String类型返回
        /// </summary>
        /// <returns></returns>
        public override string ToReadableText(string splitter = ",")
        {
            StringBuilder sb = new StringBuilder();
            var vecStr = base.ToReadableText(splitter);
            var matStr = new Matrix(this.InverseWeight).ToReadableText(splitter);

            sb.AppendLine(vecStr);
            sb.AppendLine(matStr);

            return sb.ToString();
        }

        /// <summary>
        /// 解析
        /// </summary>
        /// <param name="text"></param>
        /// <param name="splitter"></param>
        /// <returns></returns>
        public new static WeightedMatrix Parse(string text, string[] splitter = null)
        {
            string[] lines = text.Split(new char[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries);

            return Parse(lines, splitter);
        }
        /// <summary>
        /// 解析行
        /// </summary>
        /// <param name="splitter"></param>
        /// <param name="lines"></param>
        /// <returns></returns>
        public new static WeightedMatrix Parse(string[] lines, string[] splitter = null)
        {
            StringBuilder sb = new StringBuilder();
            string transText = null;
            string noiseText = null;
            foreach (var item in lines)
            {
                if (String.IsNullOrWhiteSpace(item))
                {
                    continue;
                }
                if (item.Contains("×") && sb.Length > 0)
                {
                    transText = sb.ToString();
                    sb.Clear(); 
                }

                sb.AppendLine(item);
            }
            noiseText = sb.ToString();


            var trans = Matrix.Parse(transText);
            var noise = Matrix.Parse(noiseText);

            return new WeightedMatrix(trans, noise);
        }

        #endregion

    }

}