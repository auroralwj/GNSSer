//2014.10.28, czs, create in numu, 矩阵头文件

using System;
using System.Text;
using System.IO;
using System.Collections.Generic;
using Geo.Utils;

namespace Geo.Algorithm
{
    /// <summary>
    /// 矩阵头文件。
    /// </summary>
    public class MatrixHeader 
    {
        /// <summary>
        /// 构造函数。
        /// </summary> 
        public MatrixHeader(MatrixType MatrixType, string Name = "Name")
        {
            this.MatrixType = MatrixType;
            this.Name = Name;
            Comments = new List<string>();
            RowNames = new List<string>();
            ColNames = new List<string>();
        }
        /// <summary>
        /// 头文件注释
        /// </summary>
        public List<string> Comments { get; set; }
        /// <summary>
        /// 行名称
        /// </summary>
        public List<string> RowNames { get; set; }
        /// <summary>
        /// 列名称
        /// </summary>
        public List<string> ColNames { get; set; }
        /// <summary>
        /// 行数量
        /// </summary>
        public int RowCount { get; set; }
        /// <summary>
        /// 列数量
        /// </summary>
        public int ColCount { get; set; }
        /// <summary>
        /// 矩阵数据数量。
        /// </summary>
        public int ContentCount { get; set; }
        /// <summary>
        /// 矩阵存储类型。
        /// </summary>
        public MatrixType MatrixType { get; set; }
        /// <summary>
        /// 矩阵存储版本
        /// </summary>
        public int Version { get; set; }
        /// <summary>
        /// 矩阵的名称，请使用英文或拼音，中文可能出错。
        /// </summary>
        public String Name { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreationTime { get; set; }
        /// <summary>
        /// 创建者，创建机构。
        /// </summary>
        public String Creator { get; set; }

        public static MatrixHeader GetDefaultHeader(IMatrix matrix)
        {
            MatrixHeader header = new MatrixHeader(matrix.MatrixType,matrix.Name)
            {
                CreationTime = DateTime.Now,
                Creator = "Gnsser",
                Version = 1,
                ContentCount = matrix.ItemCount,
                RowCount = matrix.RowCount,
                ColCount = matrix.ColCount,
                ColNames = matrix.ColNames,
                RowNames = matrix.RowNames,
            }; 
            return header;
        }
    }
}
