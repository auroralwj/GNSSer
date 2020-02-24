//2018.04.19， czs, create, hmx, 可以快速删除行列的矩阵

using System;
using Geo.Algorithm;
using Geo.Common;
using Geo.Utils; 
using System.Collections.Generic; 
using System.Threading.Tasks;
using Geo.IO;

namespace Geo.Algorithm
{
    /// <summary>
    /// 可以快速删除行列的矩阵，快速改变行列。
    /// 一般只在改变时使用，一旦完成，就转换成其它矩阵以提高效率。
    /// </summary>
    public class ChangeableMatrix : AbstractMatrix, IMatrix
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="m"></param>
        public ChangeableMatrix(Matrix m):this(m.Array, m.RowNames, m.ColNames)
        {

        }
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="m"></param>
        public ChangeableMatrix(ArrayMatrix m):this(m.Array, m.RowNames, m.ColNames)
        {

        }
        /// <summary>
        /// 初始化 
        /// </summary>
        /// <param name="rowCount">行数</param>
        /// <param name="colCount">列数</param>
        public ChangeableMatrix(double[][] data, List<string> rowNames, List<string> colNames) : base(MatrixType.ConstMatrix)
        {
            this.RowNames = new List<string>(rowNames);
            this.ColNames = new List<string>(colNames);
            this.Data = new List<List<double>>();
            int rowCount = RowCount;
            int colCount = ColCount;
            for (int i = 0; i < rowCount; i++)
            {
                var rowVector = new List<double>();
                var row = data[i];
                for (int j = 0; j < colCount; j++)
                {
                    rowVector.Add(row[j]);
                }
                this.Data.Add(rowVector);
            }
        }
        /// <summary>
        /// 核心存储
        /// </summary>
         public List<List<double>> Data { get; set; }

        /// <summary>
        /// 返回数组
        /// </summary>
        public override  double[][] Array
        {
            get {
                var array = new double[RowCount][];// MatrixUtil.Create(RowCount, ColCount, 0);
                int rowCount = RowCount;
                int colCount = ColCount;
                for (int i = 0; i < rowCount; i++)
                {
                    var rowVector = new double[colCount];
                    var row = Data[i];
                    for (int j = 0; j < colCount; j++)
                    {
                        rowVector[j] = row[j];
                    }
                    array[i] = rowVector;
                }
                return array;
            }
        }

        /// <summary>
        /// 所有元素总和。指有效的内容表示数，矩阵内容必须的数。
        /// </summary>
        public override int ItemCount { get { return ColCount * RowCount; } }
          
        public new int ColCount { get { return this.ColNames.Count;   } }

        public new int RowCount { get { return this.RowNames.Count; } }
         
        public override bool IsSymmetric
        {
            get { return IsSquare; }
        }

        public override double this[int i, int j]
        {
            get
            {
                return Data[i][j];
            }
            set
            {
                Data[i][j] = value;
            }
        }

        public override Vector GetRow(int index)
        {
           return new Vector(Data[index]);
        }

        public override Vector GetCol(int index)
        {
            Vector vector = new Vector(this.RowCount);
            for (int i = 0; i < this.RowCount; i++)
            {
                vector[i] = Data[i][index];
            }
            return vector;
        }
         

        public override IMatrix Transposition
        {
            get
            {
                int rowCount = RowCount;
                int colCount = ColCount;
                ArrayMatrix X = new ArrayMatrix(colCount, rowCount);
                double[][] x = X.Array;

                for (int i = 0; i < rowCount; i++)
                {
                    var row = Data[i];
                    for (int j = 0; j < colCount; j++)
                    {
                        x[j][i] = row[j];
                    }
                }

                return X;
            }
        }
        #region 独有改变算法


        /// <summary>
        /// 对行和列进行重新组织
        /// </summary>
        /// <param name="newRowNames"></param>
        /// <param name="newColNames"></param>
        /// <param name="defaultNewVal"></param>
        public void ArrangeRowCols(List<string> newRowNames, List<string> newColNames, double defaultNewVal = 0)
        {
            ArrangeRows(newRowNames, defaultNewVal);
            ArrangeCols(newColNames, defaultNewVal);
        }

        /// <summary>
        /// 对行和列进行重新组织
        /// </summary>
        /// <param name="newNames"></param>
        /// <param name="defaultNewVal"></param>
        public void ArrangeRowCols(List<string> newNames, double defaultNewVal = 0)
        {
            ArrangeRows(newNames, defaultNewVal);
            ArrangeCols(newNames, defaultNewVal);
        }


        /// <summary>
        /// 按照新的名称，排列组织数组
        /// </summary>
        /// <param name="newRowNames"></param>
        /// <param name="defaultNewVal"></param>
        public void ArrangeRows(List<string> newRowNames, double defaultNewVal = 0)
        {
            //首先，在老矩阵中删除新矩阵没有的行或列
            var rowNamesTobeRemoved = Geo.Utils.ListUtil.GetExcept<string>(RowNames, newRowNames); 
            RemoveRow(rowNamesTobeRemoved);

            int irow = 0;
            foreach (var name in newRowNames)
            {
                if (RowNames.Contains(name))//若包含
                {
                    var index = (RowNames.IndexOf(name));
                    if (index != irow)//但非同一行
                    {
                        ChangeRow(irow, index);//交换之
                    }
                }
                else
                {
                    InsertRow(irow, name, defaultNewVal);
                }
                irow++;
            }
        }

        /// <summary>
        /// 按照新的名称，排列组织数组
        /// </summary>
        /// <param name="newColNames"></param>
        /// <param name="defaultNewVal"></param>
        public void ArrangeCols(List<string> newColNames, double defaultNewVal = 0)
        {
            //首先，在老矩阵中删除新矩阵没有的行或列
            var colNamesTobeRemoved = Geo.Utils.ListUtil.GetExcept<string>(ColNames, newColNames); 
            RemoveCol(colNamesTobeRemoved);

            int icol = 0;
            foreach (var name in newColNames)
            {
                if (ColNames.Contains(name))//若包含
                {
                    var index = (ColNames.IndexOf(name));
                    if (index != icol)//但非同一行
                    {
                        ChangeCol(icol, index);//交换之
                    }
                }
                else
                {
                    InsertCol(icol, name, defaultNewVal);
                }
                icol++;
            }
        }


        /// <summary>
        /// 交换行
        /// </summary>
        /// <param name="index1"></param>
        /// <param name="index2"></param>
        public void ChangeRow(int index1, int index2)
        {
            var from = this.Data[index1];
            var to = this.Data[index2];
            this.Data[index2] = from;
            this.Data[index1] = to;
            //update names
            var temp = this.RowNames[index2];
            this.RowNames[index2] = this.RowNames[index1];
            this.RowNames[index1] = temp;
        }

        /// <summary>
        /// 交换列
        /// </summary>
        /// <param name="index1"></param>
        /// <param name="index2"></param>
        public void ChangeCol(int index1, int index2)
        {
            var from = this.GetCol(index1);
            var to = this.GetCol(index2);
            this.SetCol(index1, to);
            this.SetCol(index2, from);

            //update names
            var temp = this.ColNames[index2];
            this.ColNames[index2] = this.ColNames[index1];
            this.ColNames[index1] = temp;
        }



        /// <summary>
        /// 同时插入一行和一列，适用于方阵。
        /// </summary>
        /// <param name="index"></param>
        /// <param name="name"></param>
        /// <param name="val"></param>
        public void InsertRowCol(int index, string name, double val = 0)
        {
            InsertRow(index, name, val);
            InsertCol(index, name, val);
        }
        /// <summary>
        /// 传指定位置插入列
        /// </summary>
        /// <param name="index"></param>
        /// <param name="name"></param>
        /// <param name="val"></param>
        public void InsertCol(int index, string name, double val = 0)
        {
            var news = new List<double>();
            int len = RowCount;
            for (int i = 0; i < len; i++)
            {
                news.Add(val);
            }
            InsertCol(index, name, news);
        }
        /// <summary>
        /// 传指定位置插入列
        /// </summary>
        /// <param name="index"></param>
        /// <param name="name"></param>
        /// <param name="newRow"></param>
        public void InsertCol(int index, string name, List<double> newRow)
        {
            int i = 0;
            foreach (var item in Data)
            {
                item.Insert(index, newRow[i]);
                i++;
            } 
            this.ColNames.Insert(index, name);
        }
        /// <summary>
        /// 传指定位置插入行
        /// </summary>
        /// <param name="index"></param>
        /// <param name="name"></param>
        /// <param name="val"></param>
        public void InsertRow(int index, string name, double val = 0)
        {
            var newRow = new List<double>();
            int colCount = ColCount;
            for (int i = 0; i < colCount; i++)
            {
                newRow.Add(val);
            }
            InsertRow(index, name, newRow);
        }
        /// <summary>
        /// 传指定位置插入行
        /// </summary>
        /// <param name="index"></param>
        /// <param name="name"></param>
        /// <param name="newRow"></param>
        public void InsertRow(int index, string name, List<double> newRow)
        {
            Data.Insert(index, newRow);
            this.RowNames.Insert(index, name);
        }
        /// <summary>
        /// 移除行
        /// </summary>
        /// <param name="names"></param>
        public void RemoveRow(IEnumerable<string> names)
        {
            foreach (var name in names)
            {
                RemoveRow(name);
            }
        }
        /// <summary>
        /// 移除列
        /// </summary>
        /// <param name="names"></param>
        public void RemoveCol(IEnumerable<string> names)
        {
            foreach (var name in names)
            {
                RemoveCol(name);
            }
        }
        /// <summary>
        /// 移除行
        /// </summary>
        /// <param name="name"></param>
        public void RemoveRow(string name)
        {
            int index = RowNames.IndexOf(name);
            RemoveRow(index);
        }
        /// <summary>
        /// 移除列
        /// </summary>
        /// <param name="name"></param>
        public void RemoveCol(string name)
        {
            int index = ColNames.IndexOf(name);
            RemoveCol(index); 
        }
        /// <summary>
        /// 移除列
        /// </summary>
        /// <param name="index"></param>
        public void RemoveCol(int index)
        {
            foreach (var item in Data)
            {
                item.RemoveAt(index);
            }
            ColNames.RemoveAt(index);
        }
        /// <summary>
        /// 移除行
        /// </summary>
        /// <param name="index"></param>
        public void RemoveRow(int index)
        {
            Data.RemoveAt(index);
            RowNames.RemoveAt(index);
        }

        #endregion
        /// <summary>
        /// 转换成更为通用的二维数组矩阵。
        /// </summary>
        /// <returns></returns>
        public ArrayMatrix ToArrayMatrix()
        {
            return new ArrayMatrix(this.Array) { ColNames = ColNames, RowNames = RowNames };
        }


        public override IMatrix Pow(double power)
        {
            return null;
        }

        public override IMatrix Clone()
        {
            return null;
        }

        public override IMatrix SubMatrix(int fromIndex, int count)
        {
            throw new NotImplementedException();
        }
    }
}
