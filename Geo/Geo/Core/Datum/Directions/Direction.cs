using System;
using System.Collections.Generic;
using System.Text;

namespace Geo.Coordinates
{
    /// <summary>
    /// ����8������
    /// </summary>
    public enum Direction
    {
        /// <summary>
        /// δ֪
        /// </summary>
        UnKnown,
        /// <summary>
        /// ��
        /// </summary>
        East,
        /// <summary>
        /// ��
        /// </summary>
        South,
        /// <summary>
        /// ��
        /// </summary>
        West,
        /// <summary>
        /// ��
        /// </summary>
        North,
        /// <summary>
        /// ����
        /// </summary>
        SouthEast,
        /// <summary>
        /// ����
        /// </summary>
        SouthWest,
        /// <summary>
        /// ����
        /// </summary>
        NorthEast,
        /// <summary>
        /// ����
        /// </summary>
        NorthWest
    }
    /// <summary>
    /// ���߷������ơ�
    /// </summary>
    public enum DrivenDirection
    {
        /// <summary>
        /// ��ǰ
        /// </summary>
        Ahead,
        /// <summary>
        /// ���
        /// </summary>
        Back,
        /// <summary>
        /// ����
        /// </summary>
        Right,
        /// <summary>
        /// ����
        /// </summary>
        Left
    }
}
