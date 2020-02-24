using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
//using Geo.Coordinates;

namespace Geo.Algorithm.Adjust
{
    /// <summary>
    /// 定义平差基本信息，如总点数，未知点数，点编号等
    /// </summary>
    public class AdjustBasicInformation
    {
        /// <summary>
        /// 文件数，同步区数，向量组总数
        /// </summary>
        public int FileNumber { get; set; }
        /// <summary>
        /// 总点数
        /// </summary>
        public int TotalPointNumber { get; set; }
        /// <summary>
        /// 基线向量总数
        /// </summary>
        public int TotalBaselineNumber { get; set; }
        /// <summary>
        /// 未知点数=总点数-已知点数
        /// </summary>
        public int UnknowPointnumber { get; set; }
        /// <summary>
        /// 点名指针数组,存储所有的点，不能遗漏也不能重复，根据点的位置进行系数矩阵的建立
        /// </summary>
        public List<string> TotalPointName = new List<string>();//
        /// <summary>
        /// 已知点数组
        /// </summary>
        public List<string> KnownPointName = new List<string>();

        ///// <summary>
        ///// 基线向量起点编号
        ///// </summary>
        //public List<int[]> totalbeginOfBaselineList = new List<int[]>();
        ///// <summary>
        ///// 基线向量终点编号
        ///// </summary>
        //public List<int[]> totalendOfBaselineList = new List<int[]>();

        /// <summary>
        /// 点坐标近似值
        /// 坐标数组，三个坐标构成一个XYZ，顺序与点号一一对应
        /// </summary>
        public List<Vector> PointsXYZ = new List<Vector>();
    }
}
