using System;
using Geo.Coordinates;
using Geo.Utils;
using Geo.Times;

namespace Geo.Coordinates
{ 
   /// <summary>
   /// Encapsulates an Earth Centered Inertial coordinate and 
   /// associated time.
   /// </summary>
    public class TimedMotionState : MotionState
   {
      #region Construction

      /// <summary>
      /// Creates an instance of the class with the given position, velocity, and time.
      /// </summary>
      /// <param name="pos">The position vector.</param>
      /// <param name="vel">The velocity vector.</param>
      /// <param name="date">The time associated with the position.</param>
      public TimedMotionState(XYZ pos, XYZ vel, Julian date)
         : base(pos, vel)
      {
         Date = date;
      }

      #endregion

      #region Properties

      /// <summary>
      /// The time associated with the ECI coordinates.
      /// </summary>
      public Julian Date { get; protected set; }

      #endregion
   }
}
