//2014.05.24, czs, created 

using System;
using System.Collections.Generic;
using System.Text;
using Geo.Common;
using System.Globalization;

namespace Geo.Referencing
{
    /// <summary>
    /// ����ϵͳ���������ʴ��ڵĿռ�λ�ã����꣩�Ĳ���ϵ��ͨ�������ض���׼���������ʽ��ʵ�֡�
    /// Ϊ�˷�����д����������ΪCS��
    /// </summary>
    [Serializable]
    public class CoordinateSystem : IdentifiedObject, ICoordinateSystem
    {
        /// <summary>
        /// ����ϵͳ��Ĭ�Ϲ��캯��
        /// </summary>
        public CoordinateSystem()
            : this(new List<IAxis>())
        { }
        /// <summary>
        /// ���캯����
        /// </summary>
        /// <param name="axes">������</param>
        /// <param name="name">����ϵͳ����</param>
        /// <param name="id">����ϵͳ���</param>
        public CoordinateSystem(IEnumerable<IAxis> axes, string name = null, string id = null)
            : base(id, name)
        {
            this.Axes = new List<IAxis>(axes);

            //������͸�ֵ
            this.CoordinateType = Referencing.CoordinateType.Other;
            if (this.Contains(Ordinate.X, Ordinate.Y))
            {
                this.CoordinateType = CoordinateType.XY;
                if (Contains(Ordinate.Z))
                    this.CoordinateType = CoordinateType.XYZ;
            }
            else if (this.Contains(Ordinate.Lon, Ordinate.Lat))
            {
                this.CoordinateType = CoordinateType.LonLat;
                if (Contains(Ordinate.Height))
                    this.CoordinateType = CoordinateType.LonLatHeight;
            }
        }

        /// <summary>
        /// ���������������͡�
        /// </summary>
        public CoordinateType CoordinateType { get; protected set; }

        /// <summary>
        /// �����������
        /// </summary>
        /// <param name="axisIndex">��ţ���0��ʼ</param>
        /// <returns></returns>
        public IAxis this[int axisIndex] { get { return Axes[axisIndex]; } set { Axes[axisIndex] = value; } }

        /// <summary>
        /// ����ϵͳά����
        /// </summary>
        public int Dimension { get { return this.Axes.Count; } }

        /// <summary>
        /// �����Ἧ�ϡ�
        /// </summary>
        public List<IAxis> Axes { get; set; }

        /// <summary>
        /// �Ƿ����ָ�������ᡣ
        /// </summary>
        /// <param name="ordinate">������</param>
        /// <returns>��������</returns>
        public bool Contains(Ordinate ordinate)
        {
            return Axes.Exists(m => m.Ordinate == ordinate);
        }
        /// <summary>
        /// �Ƿ����ָ�������ᡣ
        /// </summary>
        /// <param name="ordinates">������</param>
        /// <returns>��������</returns>
        public bool Contains(params Ordinate[] ordinates)
        {
            foreach (var item in ordinates)
            {
                if (!this.Contains(item)) return false;
            }
            return true;
        }
        /// <summary>
        /// ����ϵͳ�Ƿ���ȣ���Ҫ�Ƚ������ᡣ
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public override bool Equals(object obj)
        {
            CoordinateSystem cs = obj as CoordinateSystem;
            if (cs == null) return false;

            if (Dimension != cs.Dimension) return false;
            for (int i = 0; i < Dimension; i++)
            {
                if (!Axes[i].Equals(cs.Axes[i])) return false;
            }

            return true;
        }
        /// <summary>
        /// ��ϣ��
        /// </summary>
        /// <returns></returns>
        public override int GetHashCode()
        {
            int code = 0;
            for (int i = 0; i < Dimension; i++)
            {
                code += Axes[i].GetHashCode() * (i + 1) * 3;
            }
            return code;
        }
        /// <summary>
        /// Ĭ���Զ��Ÿ������ַ�����
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            string str = "";
            if (Name != null) str += Name;
            else str = CoordinateType + "";
            return str;
        }

        #region �������ӿ�ʵ��
        /// <summary>
        /// ����ѭ�����ʵ�ö����
        /// </summary>
        /// <returns></returns>
        public IEnumerator<IAxis> GetEnumerator()
        {
            return Axes.GetEnumerator();
        }
        /// <summary>
        /// ����ѭ�����ʵ�ö����
        /// </summary>
        /// <returns></returns>
        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return Axes.GetEnumerator();
        }
        #endregion

        #region ��������ϵͳ
        /// <summary>
        /// ������ֱ�Ϊ X Y �� 2 ά����ϵͳ�� �߶�Ϊ�ס�
        /// </summary>
        public static CoordinateSystem XyCs = new CoordinateSystem(
                    new List<IAxis>() { Axis.X, Axis.Y },
                    "XyCs");
        /// <summary>
        /// ������ֱ�Ϊ X Y Z ����ά����ϵͳ�� �߶�Ϊ�ס�
        /// </summary>
        public static CoordinateSystem XyzCs = new CoordinateSystem(
                    new List<IAxis>() { Axis.X, Axis.Y, Axis.Z },
                    "XyzCs");
        /// <summary>
        /// ������ֱ�Ϊ Lon Lat �� 2 ά����ϵͳ�� �߶�Ϊ�ȡ�
        /// </summary>
        public static CoordinateSystem LonLatCs = new CoordinateSystem(
                    new List<IAxis>() { Axis.Lon, Axis.Lat },
                    "LonLatCS");
        /// <summary>
        /// ������ֱ�Ϊ Lon Lat Heigt �� 3 ά����ϵͳ�� Lon Lat �߶�Ϊ��, Height Ϊ�ס�
        /// </summary>
        public static CoordinateSystem GeodeticCs = new CoordinateSystem(
                    new List<IAxis>() { Axis.Lon, Axis.Lat, Axis.Height },
                    "GeodeticCS");
        /// <summary>
        /// ��������
        /// </summary>
        public static CoordinateSystem SphereCs = new CoordinateSystem(
                    new List<IAxis>() { Axis.Lon, Axis.Lat, Axis.Radius },
                    "SphereCS");
        /// <summary>
        /// վ������ NEU
        /// </summary>
        public static CoordinateSystem NeuCs = new CoordinateSystem(
                    new List<IAxis>() { Axis.North, Axis.East, Axis.Up },
                    "NeuCS");
        public static CoordinateSystem EnuCs = new CoordinateSystem(
                    new List<IAxis>() { Axis.East, Axis.North, Axis.Up },
                    "EnuCs");
        /// <summary>
        /// ��վ���� HEN
        /// </summary>
        public static CoordinateSystem HenCs = new CoordinateSystem(
                    new List<IAxis>() { Axis.Height, Axis.East, Axis.North },
                    "HenCS");
        /// <summary>
        /// RadiusAzimuthZenithAngle
        /// </summary>
        public static CoordinateSystem PolorCs = new CoordinateSystem(
                    new List<IAxis>() { Axis.Radius, Axis.Azimuth, Axis.ElevatAngle },
                    "PolorCs");
        /// <summary>
        /// RadiusAzimuth
        /// </summary>
        public static CoordinateSystem PlanePolorCs = new CoordinateSystem(
                    new List<IAxis>() { Axis.Radius, Axis.Azimuth },
                    "PlanePolorCs");
        #endregion


    }
}