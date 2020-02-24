//2014.05.24, czs, created 

using System;
using System.Collections.Generic;
using System.Text;
using Geo.Common;
using System.Globalization;

namespace Geo.Referencing
{
    
    /// <summary>
    /// ��������ϵͳ�����׼�Ƚ����⡣
    /// </summary>
    /// <remarks>In general, a local coordinate system cannot be related to other coordinate 
    /// systems. However, if two objects supporting this interface have the same dimension, 
    /// axes, units and datum then client code is permitted to assume that the two coordinate
    /// systems are identical. This allows several datasets from a common source (e.g. a CAD
    /// system) to be overlaid. In addition, some implementations of the Coordinate 
    /// Transformation (CT) package may have a mechanism for correlating local datums. (E.g. 
    /// from a database of transformations, which is created and maintained from real-world 
    /// measurements.)
    /// </remarks>
    public class LocalCs : CoordinateSystem
    {
         /// <summary>
        /// ���캯��
        /// </summary>
        public LocalCs() :base() {  }
        /// <summary>
        /// ���캯��
        /// </summary>
        /// <param name="LocalDatum">��׼</param>
        /// <param name="axes">������</param>
        /// <param name="name">����ϵͳ����</param>
        /// <param name="id">����ϵͳ���</param>
        /// <param name="abbrev">����ϵͳ���</param>
        public LocalCs(LocalDatum LocalDatum, List<IAxis> axes, string name = null, string id = null)
            :base(axes,  name,  id)
        {
            this.LocalDatum = LocalDatum;
        } 
        /// <summary>
        ///  ���ػ�׼
        /// </summary>
        LocalDatum LocalDatum { get; set; }
    }

}
