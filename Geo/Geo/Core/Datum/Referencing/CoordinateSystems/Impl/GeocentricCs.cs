//2014.05.24, czs, created 

using System;
using System.Collections.Generic;
using System.Text;
using Geo.Common;
using System.Globalization;

namespace Geo.Referencing
{
    
    /// <summary>
    /// ������ά����ϵ��
    /// </summary>
    public class GeocentricCs : CoordinateSystem
    { 
        /// <summary>
        /// ���캯��
        /// </summary>
        public GeocentricCs():base() { }
        /// <summary>
        /// ���캯��
        /// </summary>
        /// <param name="PrimeMeridian">��������</param>
        /// <param name="HorizontalDatum">��׼</param>
        /// <param name="LinearUnit">�߶ȼ�����λ</param>
        /// <param name="axes">������</param>
        /// <param name="name">����ϵͳ����</param>
        /// <param name="id">����ϵͳ���</param>
        public GeocentricCs( 
            PrimeMeridian PrimeMeridian, 
            HorizontalDatum HorizontalDatum,
            LinearUnit LinearUnit,
            List<IAxis> axes,
            string name = null,
            string id = null)
            :base(axes,  name,  id)
        {
            this.HorizontalDatum = HorizontalDatum;
            this.HorizontalDatum = HorizontalDatum;
            this.LinearUnit = LinearUnit;
        } 
        /// <summary>
        /// Returns the HorizontalDatum. The horizontal datum is used to determine where
        /// the centre of the Earth is considered to be. All coordinate points will be 
        /// measured from the centre of the Earth, and not the surface.
        /// </summary>
        public HorizontalDatum HorizontalDatum { get; set; }
        /// <summary>
        /// ����������Լ�����λ.
        /// </summary>
        public LinearUnit LinearUnit { get; set; }
        /// <summary>
        /// ��ʼ�����ߡ�
        /// </summary>
        public PrimeMeridian PrimeMeridian { get; set; }

        #region Predefined geographic coordinate systems

        /// <summary>
        /// Creates a geocentric coordinate system based on the WGS84 ellipsoid, suitable for GPS measurements
        /// </summary>
        public static GeocentricCs WGS84
        {
            get
            {
                return new CoordinateSystemFactory().CreateGeocentricCs("WGS84 Geocentric",
                    HorizontalDatum.WGS84, LinearUnit.Metre,
                    PrimeMeridian.Greenwich);
            }
        }

        #endregion

        /// <summary>
        /// ��ϣ��
        /// </summary>
        /// <returns></returns>
        public override int GetHashCode()
        {
            return HorizontalDatum.GetHashCode() * 9 + PrimeMeridian.GetHashCode() * 13;
        }

        /// <summary>
        /// �Ƿ����
        /// </summary>
        /// <param name="obj"></param>
        /// <returns>True if equal</returns>
        public override bool Equals(object obj)
        {
            if (!(obj is GeocentricCs))
                return false;
            GeocentricCs gcc = obj as GeocentricCs;
            return gcc.HorizontalDatum.Equals(this.HorizontalDatum) &&
                gcc.LinearUnit.Equals(this.LinearUnit) &&
                gcc.PrimeMeridian.Equals(this.PrimeMeridian);
        }

    }

     
}
