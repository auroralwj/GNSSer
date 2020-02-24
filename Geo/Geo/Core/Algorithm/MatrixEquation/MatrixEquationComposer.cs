//2019.02.14, czs, create in hongqing, 矩阵方程组合类

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Geo.Algorithm
{
    /// <summary>
    /// 矩阵方程组合类
    /// </summary>
    public class MatrixEquationComposer
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public MatrixEquationComposer()
        {
            CoeffMatrixComposer = new CoeffMatrixComposer();
            QofRightVectorComposer = new  CoeffMatrixComposer();
        }
        /// <summary>
        /// 系数阵叠加器
        /// </summary>
        CoeffMatrixComposer CoeffMatrixComposer { get; set; }
        /// <summary>
        /// 权阵叠加器
        /// </summary>
        CoeffMatrixComposer QofRightVectorComposer { get; set; }
        /// <summary>
        /// 累加后的左乘矩阵
        /// </summary>
        public SparseMatrix LeftMatrix => CoeffMatrixComposer.CoeffMatrix;
        /// <summary>
        /// 权逆阵,也是一个系数阵
        /// </summary>
        public SparseMatrix QofRightVector => QofRightVectorComposer.CoeffMatrix;
        /// <summary>
        /// 右边向量
        /// </summary> 
        public Vector RightVector { get; private set; }
        
        /// <summary>
        /// 构建
        /// </summary>
        /// <returns></returns>
        public MatrixEquation Build()
        {
            var meq = new MatrixEquation(new Matrix(LeftMatrix), new Matrix(RightVector), "ComEq");

            if(QofRightVector != null) {
                meq.QofU = new Matrix(QofRightVector);
            }
            return meq;
        }

        /// <summary>
        /// 添加
        /// </summary>
        /// <param name="equation"></param>
        public void AddSubMatrix(MatrixEquation equation )
        {
            //左边
            CoeffMatrixComposer.Add(equation.LeftSide);


            //右边
            if (RightVector == null) { RightVector = equation.RightVector; }
            else
            {
                RightVector.AddRange(equation.RightVector);
            }

            if (equation.HasWeightOfRightSide)
            {
                QofRightVectorComposer.Add(equation.InverseWeightOfRightSide);
            }
        }
    }


    /// <summary>
    /// 系数阵组合器
    /// </summary>
    public class CoeffMatrixComposer
    {
        /// <summary>
        /// 默认构造函数
        /// </summary>
        public CoeffMatrixComposer()
        {

        }

        /// <summary>
        /// 累加后的系数矩阵
        /// </summary>
        public SparseMatrix CoeffMatrix { get; set; }
        
        /// <summary>
        /// 行名称
        /// </summary>
        public List<string> RowNames => CoeffMatrix.RowNames;

        /// <summary>
        /// 列名称
        /// </summary>
        public  List<string> ColNames => CoeffMatrix.ColNames;
        /// <summary>
        /// 添加一个
        /// </summary>
        /// <param name="matrix"></param>
        public void Add(Matrix matrix)
        {
            if (CoeffMatrix == null)
            {
                CoeffMatrix = new SparseMatrix(matrix);
                return;
            }

            //增加
            int rowLen = matrix.RowCount;
            int colLen = matrix.ColCount;

            if (matrix.MatrixType == MatrixType.Diagonal)
            {
                for (int i = 0; i < rowLen; i++)
                {
                    var item = matrix[i, i];
                    if (item == 0) { continue; }

                    this.CoeffMatrix.Add(matrix.RowNames[i], matrix.ColNames[i], item);
                }
            }
            else
            {
                for (int i = 0; i < rowLen; i++)
                {
                    for (int j = 0; j < colLen; j++)
                    {
                        var item = matrix[i, j];
                        if (item == 0) { continue; }

                        this.CoeffMatrix.Add(matrix.RowNames[i], matrix.ColNames[j], item);
                    }
                }
            }
        }
    }


}
