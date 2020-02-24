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
    /// 矩阵方程命名器
    /// </summary>
    public class MatrixEquationNameBuiler
    {
        public MatrixEquationNameBuiler()
        {

        }
        /// <summary>
        /// 左边
        /// </summary>
        public const string L = "L";
        /// <summary>
        /// 右边
        /// </summary>
        public const string R = "R";
        /// <summary>
        /// 名称分隔符
        /// </summary>
        public const string Q = "Q";
        /// <summary>
        /// 名称分隔符
        /// </summary>
        public const string NameSpliter = "_Of_";
        /// <summary>
        /// 提取名称
        /// </summary>
        /// <param name="nameOfAnySide"></param>
        /// <returns></returns>
        public static string GetName(string nameOfAnySide)
        {
            if (nameOfAnySide.Contains(NameSpliter))
            {
                return nameOfAnySide.Substring(nameOfAnySide.IndexOf(NameSpliter) + NameSpliter.Length);
            }
            return "";
        }

        /// <summary>
        /// 左边名称
        /// </summary>
        public static string GetLeftSideName(string Name)
        {
            if (String.IsNullOrWhiteSpace(Name)) { return L; }
            return L + NameSpliter + Name;
        }
        /// <summary>
        /// 右边名称
        /// </summary>
        public static string GetRightSideName(string Name)
        {
            if (String.IsNullOrWhiteSpace(Name)) { return R; }
            return R + NameSpliter + Name;
        }
        /// <summary>
        /// 右边名称
        /// </summary>
        public static string GetInverseWeightNameOfRightSide(string Name)
        {
            if (String.IsNullOrWhiteSpace(Name)) { return Q; }
            return Q + NameSpliter + Name;
        }
    }
}
