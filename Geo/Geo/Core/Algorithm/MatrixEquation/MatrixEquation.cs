//2018.04.18, czs, created in hmx, 法方程
//2019.02.15, czs, edit in hongqing, 修改为  MatrixEquation


using System;
using System.Collections.Generic;
using System.Text;
using Geo.Algorithm;
using Geo.Utils;
using Geo.IO;
using Geo.Algorithm.Adjust;

namespace Geo.Algorithm
{
    /// <summary>
    /// 法方程 NX=U
    /// </summary>
    public class MatrixEquation
    {
        Log log = new Log(typeof(MatrixEquation));

        #region 构造函数
        /// <summary>
        /// 构造函数
        /// </summary>
        public MatrixEquation()
        {
        }
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="N"></param>
        /// <param name="U"></param>
        /// <param name="name"></param>
        public MatrixEquation(Matrix N, Matrix U, string name = null) : this()
        {
            this.Name = name;
            this.N = N;
            this.U = U;
        }
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="N"></param>
        /// <param name="U"></param>
        /// <param name="QofU"></param>
        /// <param name="name"></param>
        public MatrixEquation(Matrix N, Matrix U, Matrix QofU, string name = null) : this()
        {
            this.Name = name;
            this.N = N;
            this.QofU = QofU;
            this.U = U;
        }
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="N"></param>
        /// <param name="U"></param>
        /// <param name="name"></param>
        public MatrixEquation(IMatrix N, Vector U, string name = null) : this()
        {
            if (N is Matrix)
            {
                this.N = N as Matrix;
            }
            else
            {
                this.N = new Matrix(N);
            }
            this.Name = name;
            this.U = new Matrix((IVector)U, true);
            if (U is WeightedVector)
            {
                this.QofU = (U as WeightedVector).InverseWeight;
            }
        }
        #endregion

        #region 属性

        #region 核心属性
        /// <summary>
        /// 名称，区别其他方程。
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 法方程左手边
        /// </summary>
        public Matrix N { get; set; }
        /// <summary>
        /// 法方程右手边
        /// </summary>
        public Matrix U { get; set; }
        /// <summary>
        /// 权逆阵
        /// </summary>
        public Matrix QofU { get; set; }
        #endregion

        #region 便捷属性
        /// <summary>
        /// 方程是否可以求解
        /// </summary>
        public bool IsSolvable { get => LeftSide.RowCount >= LeftSide.ColCount; }
        /// <summary>
        /// 最大阶数，比较各个大小返回最大。
        /// </summary>
        public int MaxSize => Math.Max(this.LeftSide.ColCount, this.LeftSide.RowCount);
        /// <summary>
        /// 参数名称
        /// </summary>
        public List<string> ParamNames { get => N.ColNames; }
        /// <summary>
        /// 方程左手边
        /// </summary>
        public Matrix LeftSide { get => N; set => N = value; }
        /// <summary>
        /// 方程右手边
        /// </summary>
        public Matrix RightSide { get => U; set => U = value; }
        /// <summary>
        /// 方程右手边权值
        /// </summary>
        public Matrix InverseWeightOfRightSide { get => QofU; set => QofU = value; }
        /// <summary>
        /// 是否具有权，观测层面。
        /// </summary>
        public bool HasWeightOfRightSide => InverseWeightOfRightSide != null;
        /// <summary>
        /// 左边名称
        /// </summary>
        public string LeftSideName => MatrixEquationNameBuiler.GetLeftSideName(Name);
        /// <summary>
        /// 右边名称
        /// </summary>
        public string RightSideName => MatrixEquationNameBuiler.GetRightSideName(Name);
        /// <summary>
        /// 右边名称
        /// </summary>
        public string InverseWeightNameOfRightSide => MatrixEquationNameBuiler.GetInverseWeightNameOfRightSide(Name);
        #endregion
        #endregion

        /// <summary>
        /// 将观测值参数进行编号，参数值不变。
        /// 这样避免多历元参数合并错误。
        /// </summary>
        /// <param name="number"></param>
        public void SetObsNameNumber(int number)
        {
            var names = LeftSide.RowNames;
            List<string> newNames = new List<string>();
            foreach (var item in names)
            {
                newNames.Add(item + number);
            }
            this.LeftSide.RowNames = newNames;
            this.RightSide.RowNames = newNames;
            if (HasWeightOfRightSide)
            {
                this.QofU.RowNames = newNames;
                this.QofU.ColNames = newNames;
            }
        }
        /// <summary>
        /// 扩展观测方程参数，只扩展参数，不足以0补齐
        /// </summary>
        /// <param name="newParams"></param>
        public void ExpandObsParams(List<string> newParams)
        {
            this.LeftSide = this.LeftSide.ExpandCols(newParams);
        }
        /// <summary>
        /// 扩展法方程参数，不足以0补齐
        /// </summary>
        /// <param name="newParams"></param>
        public void ExpandNormalParams(List<string> newParams)
        {
            this.LeftSide = this.LeftSide.Expand(newParams, newParams);
            this.RightSide = this.RightSide.ExpandCols(newParams);
            if (HasWeightOfRightSide)
            {
                this.QofU = this.QofU.Expand(newParams, newParams);
            }
        }
        /// <summary>
        /// 扩展法方程参数，不足以0补齐
        /// </summary>
        /// <param name="newParams"></param>
        public MatrixEquation GetParamExpandedEquation(List<string> newParams)
        {
            var LeftSide = this.LeftSide.Expand(newParams, newParams);
            var RightSide = this.RightSide.ExpandCols(newParams);
            Matrix QofU = null;
            if (HasWeightOfRightSide)
            {
                QofU = this.QofU.Expand(newParams, newParams);
            }

            MatrixEquation result = new MatrixEquation(LeftSide, RightSide, QofU, Name);
            return result;
        }

        #region  计算值
        /// <summary>
        /// 向量列右边
        /// </summary>
        public Vector RightVector => new Vector(U, true);
        /// <summary>
        /// 是否是法方程
        /// </summary>
        public bool IsNormal => N.IsSymmetric;

        /// <summary>
        /// 法方程。如果本身是法方程，则直接返回本书。
        /// </summary>
        public MatrixEquation NormalEquation
        {
            get
            {
                if (this.IsNormal)
                {
                    return this;
                }
                var A = N;
                var L = this.U;
                IMatrix P = null;
                if (HasWeightOfRightSide) { P = QofU.GetInverse(); }
                else { P = DiagonalMatrix.GetIdentity(A.RowCount); }
                var apt = A.Trans * P;
                var n = new Matrix(SymmetricMatrix.Parse( apt * A));
                n.ColNames = A.ColNames;
                n.RowNames = A.ColNames;

                var u = apt * L;
                u.RowNames = A.ColNames;
                return new MatrixEquation(n, u, this.Name);
            }
        }

        /// <summary>
        /// 参数估值
        /// </summary>
        public WeightedVector EstVector { get => GetEstimated(); }
        /// <summary>
        /// 方程观测残差，只有观测方程才有残差
        /// </summary>
        public WeightedVector ResidualVector { get => GetResidual(); }

        /// <summary>
        /// 方程观测残差，只有观测方程才有残差，如果是法方程在为 0.
        /// </summary>
        /// <returns></returns>
        public WeightedVector GetResidual()
        {
            Matrix L = this.RightSide;
            Matrix A = this.LeftSide;
            var obsNames = A.RowNames;

            var normal = this.NormalEquation;

            WeightedVector est = normal.GetEstimated();
            Matrix X = new Matrix((IVector)est);
            Matrix Qx = est.InverseWeight;
            
            Matrix dL = A * X - L;
            dL.RowNames = obsNames;

            Matrix p =  ( A * Qx * A.Transposition).GetSymmetric();
            p.ColNames = obsNames;
            p.RowNames = obsNames;

            return new WeightedVector(dL, p) { ParamNames = obsNames }; 
        }

        /// <summary>
        /// 获取平差结果
        /// </summary>
        /// <returns></returns>
        public WeightedVector GetEstimated()
        {
            List<string> names = null;
            if (ParamNames != null)
            {
                names = new List<string>(ParamNames);
            }
            Matrix inverN = N.Inversion;
            Matrix x = inverN * U;
            x.RowNames = names;
            x.ColNames = new List<string>() { "Names" };
            Matrix Qx = inverN;
            Qx.ColNames = names;
            Qx.RowNames = names;

            WeightedVector est = new WeightedVector(x, Qx) { ParamNames = names };
            return est;
        }
        #endregion

        #region IO转换
        /// <summary>
        /// 读取文本
        /// </summary>
        /// <returns></returns>
        public string ToReadableText()
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine(LeftSideName);
            sb.Append(LeftSide.ToReadableText());
            sb.AppendLine(RightSideName);
            sb.Append(RightSide.ToReadableText());
            if (HasWeightOfRightSide)
            {
                sb.AppendLine(InverseWeightNameOfRightSide);
                sb.Append(InverseWeightOfRightSide.ToReadableText());
            }

            if (IsNormal)
            {
                var est = this.GetEstimated();
                sb.AppendLine("Result");
                sb.Append(est.ToFormatedText());
            }
            return sb.ToString();
        }

        public ObjectTableManager GetObjectTables()
        {
            ObjectTableManager ressult = new ObjectTableManager();
            ressult.AddTable(LeftSideName, LeftSide.GetObectTable());
            ressult.AddTable(RightSideName, RightSide.GetObectTable());
            if (HasWeightOfRightSide)
            {
                ressult.AddTable(InverseWeightNameOfRightSide, InverseWeightOfRightSide.GetObectTable());
            }
            return ressult;
        }
        /// <summary>
        /// 字符串
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return N.ToString() + " " + U.ToString();
        }
        #endregion


        /// <summary>
        /// 法方程叠加，支持不同的参数叠加。
        /// </summary>
        /// <returns></returns>
        public MatrixEquation AddNormal(MatrixEquation newNormal)
        {
            if (!IsNormal) { throw new Exception("只支持法方程叠加！"); }
            var newColNames = Geo.Utils.ListUtil.GetAll(this.ParamNames, newNormal.ParamNames);
            var expanded = this.GetParamExpandedEquation(newColNames);
            var expandedNew =  newNormal.GetParamExpandedEquation(newColNames);

            var result = expanded + expandedNew;
            return result;
        }

        /// <summary>
        /// 法方程叠加。只支持参数相同叠加，但是不做判断，只比较维数大小。如果自动化处理，请使用AddNormal方法。
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        public static MatrixEquation operator +(MatrixEquation left, MatrixEquation right)
        {
            if (left == null) return right;
            if (right == null) return left;

            if (left.ParamNames != null && right.ParamNames != null)
            {
                if (left.ParamNames.Count != right.ParamNames.Count)
                {
                    throw new Exception("指定了参数名称，但是参数数量并不相等！请同步后再来！");
                }
                //为了加快速度
                //if (!Geo.Utils.ListUtil.IsEqual(left.ParamNames, right.ParamNames))
                //{
                //    throw new Exception("指定了参数名称，但是参数名称不相等！请同步后再来！");
                //}
            }

            if (left.N.ColCount != right.N.ColCount
                || left.N.RowCount != right.N.RowCount)
            {
                throw new Exception("法方程左手边维度不同！请同步后再来！");
            }

            if (left.U.ColCount != right.U.ColCount
                || left.U.RowCount != right.U.RowCount)
            {
                throw new Exception("法方程右手边维度不同！请同步后再来！");
            }

            var newN = left.N + right.N;
            var newU = left.U + right.U;
            newN.ColNames = right.N.ColNames;
            newN.RowNames = right.N.RowNames;
            newU.ColNames = right.U.ColNames;
            newU.RowNames = right.U.RowNames;

            return new MatrixEquation(newN, newU);
        }

    } 
}
