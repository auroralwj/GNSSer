//2014.05.24, czs, created 

using System;
using System.Collections.Generic;
using System.Text;
using Geo.Common;
using System.Globalization;

namespace Geo.Referencing
{
    /// <summary>
    /// �������Ķ�ά��ˮƽ����ϵ������ͶӰ���ƽ�����꣬Ҳ��Ϊ�������꣨Grid Coordinate��
    /// </summary>
    public class HorizontalCs : CoordinateSystem
    {
        /// <summary>
        /// ���캯��
        /// </summary>
        public HorizontalCs() { }
        /// <summary>
        /// ���캯��
        /// </summary>
        /// <param name="HorizontalDatum">ƽ���׼</param>
        /// <param name="axes">������</param>
        /// <param name="name">����ϵͳ����</param>
        /// <param name="id">����ϵͳ���</param>
        public HorizontalCs(HorizontalDatum HorizontalDatum, List<IAxis> axes, string name = null, string id = null)
            :base(axes,  name,  id)
        {
            this.HorizontalDatum = HorizontalDatum;
        } 

        /// <summary>
        /// ˮƽ��׼��
        /// </summary>
       public HorizontalDatum HorizontalDatum { get; set; }
    }
 

}
