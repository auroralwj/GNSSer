using System;
using System.Collections.Generic;
using System.Text;
 

namespace Geo.Coordinates
{

    /// <summary>
    /// �ռ�ֱ������ϵ�е��˶�״̬������λ�ú��ٶ�
    /// </summary>
    public class MotionState
    {
        /// <summary>
        /// ���캯��
        /// </summary>
        public MotionState()
        {
            Position = new XYZ();
            Velocity = new XYZ();
        }
        /// <summary>
        /// ���캯��
        /// </summary>
        /// <param name="pos"></param>
        /// <param name="vel"></param>
        public MotionState(XYZ pos, XYZ vel)
        {
            Position = pos;
            Velocity = vel;
        }
        /// <summary>
        /// λ��
        /// </summary>
        public XYZ Position { get; protected set; }
        /// <summary>
        /// �ٶ�
        /// </summary>
        public XYZ Velocity { get; protected set; }

        public MotionState RotateX(double radians)
        {
            return new MotionState(Position.RotateX(radians), Velocity.RotateX(radians));
        }
        public MotionState RotateY(double radians)
        {
            return new MotionState(Position.RotateY(radians), Velocity.RotateY(radians));
        }
        public MotionState RotateZ(double radians)
        {
            return new MotionState(Position.RotateZ(radians), Velocity.RotateZ(radians));
        }

        /// <summary>
        /// Scale the position vector by a factor.
        /// </summary>
        public void ScalePosVector(double factor)
        {
            Position.Mul(factor);
        }

        /// <summary>
        /// Scale the velocity vector by a factor.
        /// </summary>
        public void ScaleVelVector(double factor)
        {
            Velocity.Mul(factor);
        }
        /// <summary>
        /// ����
        /// </summary>
        /// <param name="left">���</param>
        /// <param name="right">�ұ�</param>
        /// <returns></returns>
        public static MotionState operator -(MotionState left, MotionState right) { return new MotionState(left.Position - right.Position, left.Velocity - right.Velocity); }

        /// <summary>
        /// �ӷ�
        /// </summary>
        /// <param name="left">���</param>
        /// <param name="right">�ұ�</param>
        /// <returns></returns>
        public static MotionState operator +(MotionState left, MotionState right) { return new MotionState(left.Position + right.Position, left.Velocity + right.Velocity); }

        /// <summary>
        /// Returns a string representation of the coordinate and 
        /// velocity XYZ values.
        /// </summary>
        /// <returns>The formatted string.</returns>
        public override string ToString()
        {
            return string.Format("m:({0:F3}, {1:F3}, {2:F3}) m/s:({3:F3}, {4:F3}, {5:F3})",
                                 Position.X, Position.Y, Position.Z,
                                 Velocity.X, Velocity.Y, Velocity.Z);
        }
    }




}
