using System;
//2014.05.24, czs, created 

using System.Collections.Generic;
using System.Linq;
using System.Text;
using  Geo.Common;

namespace Geo.Referencing
{
    /// <summary>
    /// 站心坐标系。以测站为原点的坐标系。
    /// </summary>
    public class TopocentricCs : CoordinateSystem
    {
        
    }

    /// <summary>
    /// 站心直角坐标系。
    /// </summary>
    public class TopocentricRectangularCs : TopocentricCs, ICartesianCs
    {

        public bool IsRightHand { get; set; }

        /// <summary>
        /// 站心直角坐标系,左手系
        /// </summary>
        public static TopocentricRectangularCs TopocentricRectCs
        {
            get
            {
                return new TopocentricRectangularCs()
                {
                     Name = "站心直角坐标系（左手系）",
                      IsRightHand = false,
                     Axes = new List<IAxis>()
                     {
                       new Axis("U", Direction.Up),
                       new Axis("N", new Orientation(Direction.Other,"指向椭球短半轴")),
                       new Axis("E", new Orientation(Direction.Other,"垂直UN，组成左手系"))
                     }
                };
            }

        }


    }
    /// <summary>
    /// 站心极坐标系。
    /// </summary>
    public class TopocentricPolarCs : TopocentricCs
    { 

    }



}
