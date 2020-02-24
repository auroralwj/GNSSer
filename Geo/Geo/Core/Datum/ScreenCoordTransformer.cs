

using System;
using System.Collections.Generic;
using System.Text;

namespace Geo.Coordinates
{
    /// <summary>
    /// ����ת������
    /// �˴�ʹ�õ�ת������Ϊ ProjNet����Ҫ��߶�ҲҪ��ֵ������ת�����ϴ��л�����ϸ�о�֮��2010.11.13
    /// ����տ��� Google map  �� Google Earch ֮���ת��Ҳ��� ��ʮ�ף���������һ�������⡣
    /// 
    /// Ϊ���ٶ�ζ�ȡ��ī���еĸ�ֵ�����÷Ǿ�̬�ࡣ
    /// </summary>
    public class ScreenCoordTransformer : CoordConsts
    { 
       
        /// <summary>
        /// ī���������ΰ���߳�����λ���ס�
        /// </summary>
       // public const double HalfMercatrorLength = 20037508.342789244;//256;// 

        //public const double EarthRadius = 6378137;
        ////�ǶȻ���
        //public const double DegToRadMultiplier = 0.017453292519943295769236907684886;
        //public const double MinToRadMultiplier = 0.0002908882086657215961539484614;
        //public const double SecToRadMultiplier = 4.8481368110953599358991410233333e-6;
        //public const double RadToDegMultiplier = 57.295779513082320876798154814105;
        //public const double RadToMinMultiplier = 3437.746770784939252607889288846;
        //public const double RadToSecMultiplier = 206264.80624709635515647335733076;
        //public const double PI = 3.1415926535897932384626433832795;
        //public const double OneQuaterPI = 0.78539816339744830961566084581988;
        //
        /// <summary>
        /// ī����ͶӰ��ά������
        /// </summary>
        public const double MaxLat = 85.083986522047383;//(26.840153,85.083984)
      //  public static XY TileLeftTop_InMecator = new XY(HalfMercatrorLength, 20025468.3427892);


        /// <summary>
        /// ���ŵȼ���Ӧ����Ƭ�������þ�̬�洢����ÿ�μ��㡣
        /// </summary>
        static long[] tileSideCounts = new long[] { 
            1	,
            2	,
            4	,
            8	,
            16	,
            32	,
            64	,
            128	,
            256	,
            512	,
            1024	,
            2048	,
            4096	,
            8192	,
            16384	,
            32768	,
            65536	,
            131072	,
            262144	,
            524288	,
            1048576	,
            2097152	,
            4194304	,
            8388608	,
            16777216	,
            33554432	,
            67108864	,
            134217728	,
            268435456	,
            536870912	,
            1073741824	,
            2147483648	,
            4294967296	,
            8589934592	,
            17179869184	,
            34359738368	,
            68719476736	};

        /// <summary>
        /// һ�����ض�Ӧ�Ķ�����ֻ�����ھ��ȡ�
        /// </summary>
        static double[] Resolutions_DegPerPiex = new double[]{
                1.40625, 
                0.703125, 
                0.3515625, 
                0.17578125, 
                0.087890625, 
                0.0439453125,
                0.02197265625, 
                0.010986328125, 
                0.0054931640625, 
                0.00274658203125,
                0.001373291015625, 
                0.0006866455078125, 
                0.00034332275390625,
                0.000171661376953125, 
                0.0000858306884765625, 
                0.00004291534423828125,
                0.000021457672119140625,
                0.0000107288360595703125,
                0.00000536441802978515625,
                0.000002682209014892578125,
                0.0000013411045074462890625,
                0.00000067055225372314453125,
                0.000000335276126861572265625,
                0.0000001676380634307861328125,
                0.00000008381903171539306640625,
                0.000000041909515857696533203125,
                0.0000000209547579288482666015625,
                0.00000001047737896442413330078125,
                5.238689482212066650390625e-9,
                2.6193447411060333251953125e-9,
                1.30967237055301666259765625e-9,
                6.54836185276508331298828125e-10

        };
        /// <summary>
        /// һ�����ض�Ӧ�������������뾶Ϊ6371Km��ֻ�����ھ��ȡ�
        /// </summary>
        static double[] Resolutions_MeterPerPiex = new double[]{
                151057.25883789111488487170562169,  
                75528.629418945557442435852810844,
                75528.62941894550000000000 	,
                37764.31470947270000000000 	,
                18882.15735473640000000000 	,
                9441.07867736819000000000 	,
                4720.53933868409000000000 	,
                2360.26966934205000000000 	,
                1180.13483467102000000000 	,
                590.06741733551200000000 	,
                295.03370866775600000000 	,
                147.51685433387800000000 	,
                73.75842716693900000000 	,
                36.87921358346950000000 	,
                18.43960679173470000000 	,
                9.21980339586737000000 	,
                4.60990169793368000000 	,
                2.30495084896684000000 	,
                1.15247542448342000000 	,
                0.57623771224171100000 	,
                0.28811885612085500000 	,
                0.14405942806042800000 	,
                0.07202971403021380000 	,
                0.03601485701510690000 	,
                0.01800742850755350000 	,
                0.00900371425377673000 	,
                0.00450185712688836000 	,
                0.00225092856344418000 	,
                0.00112546428172209000 	,
                0.00056273214086104600 	,
                0.00028136607043052300 	,
                0.00014068303521526100 	,
                0.00007034151760763070 	,
                0.00003517075880381530 	,
                0.00001758537940190770 	,
                0.00000879268970095384 	,
                0.00000439634485047692 	,
                0.00000219817242523846 	,
                0.00000109908621261923 	,
                0.00000054954310630962 	,
                0.00000027477155315481 	,
                0.00000013738577657740 	 
        };


        public ScreenCoordTransformer()
        {
        }

        /// <summary>
        /// �ɴ�������γ�Ȼ�ȡī����ͶӰ��Y���ꡣԭʼ��ʽ��
        /// </summary>
        /// <param name="lat_deg"></param>
        /// <returns></returns>
        public static double GetMercatorY(double lat_deg)
        {
            double lat_rad = lat_deg * DegToRadMultiplier;
            double y = EarthRadius * Math.Log(Math.Tan(OneQuaterPI + lat_rad / 2.0));
            return y;
        }

        #region Mercator WGS84 Convert
        MercatorProjection mecator = new MercatorProjection(HalfMercatrorLength *2);

        /// <summary>
        /// ��γ������ת��Ϊī����ͶӰ���ꡣû��Y�򾭶ȷ����С���ơ�
        /// </summary>
        /// <param name="lonLat"></param>
        /// <returns></returns>
        public XY GetMercatorXyFromLonLat(LonLat lonLat)
        {
            if (lonLat.Lat > MaxLat) lonLat.Lat = MaxLat;
            if (lonLat.Lat < -MaxLat) lonLat.Lat = -MaxLat;


            XY xy = mecator.LonLatToMecatorXy(lonLat);
            return xy;
        }

        /// <summary>
        /// ī����ͶӰ����ת��Ϊ��γ�����ꡣû��Y�򾭶ȷ����С���ơ�
        /// </summary>
        /// <param name="xy"></param>
        /// <returns></returns>
        public LonLat GetLonLatFromMercatorXy(XY xy)
        {
            LonLat lonLat = mecator.MecatorXyToLonLat(xy);
            return lonLat;
        }

 
        #endregion

        #region ScreenXY Mercator Convert
        
        /// <summary>
        /// ī��������ת��Ϊ��Ļ���ꡣֻ�Ƿ�����ͬ��
        /// </summary>
        /// <param name="mecatorXy">��תī��������</param>
        /// <param name="centerXy">��ʾ��������ī��������</param>
        /// <param name="screenSize">��Ļ��С</param>
        /// <param name="zoom">���ż���</param>
        /// <returns></returns>
        public XY GetScreenPointFromMercatorXy(XY mecatorXy, XY centerXy, XY screenCenter, int zoom)
        {
            //ī���������α����ж��ٸ���Ƭ
            long sideCount = tileSideCounts[zoom];//(int)Math.Pow(2.0, zoom);//��
            double meterPerPiex = HalfMercatrorLength * 2.0 / sideCount / 256.0;//��ֵ ��/���أ�һ���ش����ף�

            double x = (mecatorXy.X - centerXy.X) / meterPerPiex + screenCenter.X;
            double y = (mecatorXy.Y - centerXy.Y) / meterPerPiex + screenCenter.Y;
            return new XY( x,  y);
        }

        /// <summary>
        /// ī����XY����Ļ����ֻ��������ת�ͷ����Ĺ�ϵ��
        /// </summary>
        /// <param name="mecatorXy"></param>
        /// <param name="zoom"></param>
        /// <returns></returns>
        public System.Drawing.Point GetScreenPointFromMercatorXy(XY mecatorXy, int zoom)
        {
            //ī���������α����ж��ٸ���Ƭ
            long sideCount = tileSideCounts[zoom];//(int)Math.Pow(2.0, zoom);//��
            double meterPerPiex = HalfMercatrorLength * 2.0 / sideCount / 256.0;//��ֵ ��/���أ�һ���ش����ף�

            double x = (mecatorXy.X ) / meterPerPiex;
            double y = (HalfMercatrorLength  *2 - mecatorXy.Y) / meterPerPiex;
            return new System.Drawing.Point((int)x, (int)y);
        }
      
        public System.Drawing.Point GetScreenPointFromLonLat(LonLat lonLat, int zoom)
        {
            XY xy = GetMercatorXyFromLonLat(lonLat);
            return GetScreenPointFromMercatorXy(xy, zoom);
        }
        /// <summary>
        /// ��Ļ����ת��Ϊī�������ꡣ
        /// </summary>
        /// <param name="location"></param>
        /// <param name="centerXy"></param>
        /// <param name="screenSize"></param>
        /// <param name="zoom"></param>
        /// <returns></returns>
        public XY GetMecatorXyFromScreenXy(System.Drawing.Point location, XY centerXy, XY screenCenter, int zoom)
        {
            long sideCount = tileSideCounts[zoom];//(int)Math.Pow(2.0, zoom);//��
            double miPerPiex = HalfMercatrorLength * 2.0 / sideCount / 256.0;//��ֵ ��/����


            double x = (location.X - screenCenter.X) * miPerPiex + centerXy.X;
            double y = (location.Y - screenCenter.Y) * miPerPiex + centerXy.Y;
            return new XY(x, y);
        }
        #endregion

        #region Google Tile index


        /// <summary>
        /// ��γ��ת��ΪGoogle��Ƭ��š�
        /// </summary>
        /// <param name="lonLat"></param>
        /// <param name="zoom"></param>
        /// <returns></returns>
        public XY GetGoogleTileNumberFromLonLat(LonLat lonLat,  int zoom)
        {
            return  mecator.LonLatToTileXy(lonLat, zoom);
        }
 

        /// <summary>
        /// ��Google��Ƭ���ת��ΪMercater���ꡣ
        /// </summary>
        /// <param name="tileXy"></param>
        /// <param name="zoom"></param>
        /// <returns></returns>
        public XY GetMercatorXyFromGoogleTileNumber(XY tileXy,   int zoom)
        {
          LonLat lonlat = mecator.TileXyToLonLat(tileXy, zoom);
          return new XY(lonlat.Lon, lonlat.Lat);
        }      

        /// <summary>
        /// Google��Ƭ���ת��Ϊ��γ�ȡ�
        /// </summary>
        /// <param name="tileXy"></param>
        /// <param name="zoom"></param>
        /// <returns></returns>
        public LonLat GetLonLatFromGoogleTileNumber(XY tileXy, int zoom)
        {
            return  mecator.TileXyToLonLat(tileXy, zoom);
        }

        #endregion


        #region ��γ�� ��Ļ����
        /// <summary>
        /// ��γ������ת��Ϊ��Ļ���ꡣ
        /// </summary>
        /// <param name="lonLat"></param>
        /// <param name="center"></param>
        /// <param name="screenSize"></param>
        /// <param name="zoom"></param>
        /// <returns></returns>
        public XY GetScreenXyFromLonLat(LonLat lonLat, LonLat center, XY screenCenter, int zoom)
        {
            XY mecatorXy = GetMercatorXyFromLonLat(lonLat);
            XY centerXy = GetMercatorXyFromLonLat(center);
            XY pt = GetScreenPointFromMercatorXy(mecatorXy, centerXy, screenCenter, zoom);
            return pt;
        }
        /// <summary>
        /// ��Ļ����ת��Ϊ��γ�����ꡣ
        /// </summary>
        /// <param name="location"></param>
        /// <param name="center"></param>
        /// <param name="screenSize"></param>
        /// <param name="zoom"></param>
        /// <returns></returns>
        public LonLat GetLonLatFromScreenXy(System.Drawing.Point location, LonLat center, System.Drawing.Size screenSize, int zoom)
        {
            XY screenCenter = new XY(screenSize.Width / 2, screenSize.Height / 2);
            return GetLonLatFromScreenXy(location, center, screenCenter, zoom);
        }

        public LonLat GetLonLatFromScreenXy(System.Drawing.Point location, LonLat center, XY screenCenter, int zoom)
        {
            XY centerXy = GetMercatorXyFromLonLat(center);
            XY mercator = GetMecatorXyFromScreenXy(location, centerXy, screenCenter, zoom);
            LonLat lonLat = GetLonLatFromMercatorXy(mercator);
            return lonLat;
        }
        #endregion


    }
}
