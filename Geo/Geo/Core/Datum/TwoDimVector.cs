//2014.10.02， czs, create, 海鲁吐嘎查 通辽, 创建向量接口，为后继实现自我的、快速的、大规模的矩阵计算做准备
//2014.10.08， czs, edit in hailutu, 将核心变量数组改成了列表，这样可以方便更改维数。

using System;
using System.Text;
using System.Collections.Generic;
using Geo.Common;
using Geo.Referencing;

namespace Geo.Coordinates
{
    /// <summary>
    /// 二维坐标向量
    /// </summary>
    [Serializable]
    public class TwoDimVector : Geo.Algorithm.Vector
    {
        
        #region 构造函数
        /// <summary>
        /// 构建一个二维向量
        /// </summary>
        /// <param name="prevObj">第一元素</param>
        /// <param name="second">第二元素</param>
        public TwoDimVector(double first, double second) : this(new double[] { first, second }) { }
        /// <summary>
        /// 构建一个三维向量
        /// </summary>
        /// <param name="prevObj">第一元素</param>
        /// <param name="second">第二元素</param>
        /// <param name="third">第3元素</param>
        public TwoDimVector(double first, double second, double third) : this(new double[] { first, second, third }) { }
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="dimension">维数</param>
        /// <param name="initVal">初始数据</param>
        public TwoDimVector(int dimension, double initVal = 0) : base(dimension, initVal) { }
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="vector">一维数组</param>
        public TwoDimVector(double[] vector): base(vector)
        { 
        }
        #endregion 

        /// <summary>
        /// 坐标系的象限。象限从 1 开始。右手空间直角坐标系。象限逆时针顺序。
        /// </summary>
        public int Quadrant
        {
            get
            {
                if (this[0] >= 0 && this[1] >= 0) return 1;
                if (this[0] <= 0 && this[1] >= 0) return 2;
                if (this[0] <= 0 && this[1] <= 0) return 3;
                if (this[0] >= 0 && this[1] <= 0) return 4;
                return 0;
            }
        }
        /// <summary>
        ///以 第二个 轴为起点相对于坐标原点顺时针的方位角 0-360。
        ///右手直角坐标系。
        ///X lon Y  lat
        /// </summary> 
        /// <returns></returns>
        public double GetAzimuth(bool isDegreeOrRad  =true)
        {
            double angle = Math.Asin(this[0] / this.Radius());
            int quadrant = Quadrant;
            if (quadrant == 2)
            {
                angle = CoordConsts.TwoPI + angle;
            }
            if (quadrant == 3)
            {
                angle = CoordConsts.PI - angle;
            }
            if (quadrant == 4)
            {
                angle = CoordConsts.PI - angle;
            }

            if (isDegreeOrRad)
                angle = CoordConsts.RadToDegMultiplier * (angle);

            return angle;
        } 
    }
     
}
