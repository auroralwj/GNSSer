//2014.10.03, czs, edit, 卫星轨道计算暂存

using System;
using System.Globalization;
using Geo.Coordinates; 
using Geo.Utils;
using Geo.Times;


namespace Geo.Coordinates
{
     /// <summary>
   /// Class to encapsulate topo-centric coordinates.
   ///1.站心坐标
   /// 2.以观测站为原点或天球中心的天体坐标
   /// </summary>
   public class TopoCoord : Polar
   {
      #region Properties
        

      /// <summary>
      /// The range rate, in kilometers per second. 
      /// A negative value means "towards observer".
      /// </summary>
      public double RangeRate { get; set; }

      #endregion

      #region Construction

      /// <summary>
      /// Creates a new instance of the class from the given components.
      /// </summary>
      /// <param name="radAz">Azimuth, in radians.</param>
      /// <param name="radEl">Elevation, in radians.</param>
      /// <param name="range">Range, in kilometers.</param>
      /// <param name="rangeRate">Range rate, in kilometers per second. A negative
      /// range rate means "towards the observer".</param>
      public TopoCoord(double radAz, double radEl, double range, double rangeRate)
      {
         Azimuth   = radAz;
         Elevation = radEl;
         Range     = range;
         RangeRate = rangeRate;
      }

      #endregion
   } 
}