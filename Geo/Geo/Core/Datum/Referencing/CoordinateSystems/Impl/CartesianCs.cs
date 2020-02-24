//2014.05.24, czs, created 

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using  Geo.Common;

namespace Geo.Referencing
{
    /// <summary>
    /// 笛卡尔（Cartesian）坐标系是直角坐标系和斜角坐标系的统称.
    /// 坐标轴度量值相同。
    /// 此处为坐标轴相互垂直的直角坐标系。
    /// </summary>
    public class CartesianCs : CoordinateSystem, ICartesianCs
    {
        /// <summary>
        /// 左右手坐标系。
        /// 在空间直角坐标系中，让右手拇指指向x轴的正方向，食指指向y轴的正方向，如果中指能指向z轴的正方向，则称这个坐标系为右手直角坐标系．
        /// 同理左手直角坐标系。
        /// </summary>
        public bool IsRightHand { get; protected set; }

        #region 常用
        /// <summary>
        /// 空间直角坐标系
        /// </summary>
        public static CartesianCs SpaceRectangularCs {
            get
            {
                return new CartesianCs()
                {
                     IsRightHand = true,
                     Name = "右手空间直角坐标系",
                     Abbreviation="XYZ",
                     Axes = new List<IAxis>()
                     {
                        new Axis("X"),
                        new Axis("Y"),
                        new Axis("Z")
                     }    
                };
            }
        }
        #endregion

    }   
}
