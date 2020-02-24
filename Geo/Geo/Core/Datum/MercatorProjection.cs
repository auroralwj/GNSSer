using System;
using System.Collections.Generic;
using System.Text;

namespace Geo.Coordinates
{ 

    /// <summary>
    /// Provide Coordinate Convertion Of Google Maps between LonLat and Tile number.
    /// </summary>
    public class MercatorProjection
    {
        //Ħ����ȡֵ��Χ��������Ħ����ͶӰ�ĵ�λ���˴�����ȡ�ף��������ϴ󣬲����ڼ��㡣
        //��������ֻ����Ϊ��γ�ȡ���Ƭ���ꡢ��Ļ�����ת�����ߣ�ֻ�����ֵ�����ˡ����������˲�ͬ����ο�ϵ�������鷳��
        double MERCATOR_RANGE = 20037508.342789244 * 2;// 256; 

        XY pixelOrigin;//������ʼ��
        double pixelsPerLonDegree;//����һ�ȶ�Ӧһ��λĦ����
        double pixelsPerLonRadian;//����һ���ȶ�Ӧһ��λĦ����
        /// <summary>
        /// ���캯��
        /// </summary>
        public MercatorProjection()
        {
            this.pixelOrigin = new XY(MERCATOR_RANGE / 2, MERCATOR_RANGE / 2);//��Ħ����ƽ�����Ŀ�ʼ��
            this.pixelsPerLonDegree = MERCATOR_RANGE / 360;
            this.pixelsPerLonRadian = MERCATOR_RANGE / (2 * CoordConsts.PI);
        }
        /// <summary>
        /// ���캯��
        /// </summary>
        /// <param name="mecatorRange"></param>
        public MercatorProjection(double mecatorRange)
        {
            this.MERCATOR_RANGE = mecatorRange;
            this.pixelOrigin = new XY(MERCATOR_RANGE / 2, MERCATOR_RANGE / 2);//��Ħ����ƽ�����Ŀ�ʼ��
            this.pixelsPerLonDegree = MERCATOR_RANGE / 360;
            this.pixelsPerLonRadian = MERCATOR_RANGE / (2 * CoordConsts.PI);
        }
        /// <summary>
        /// ����
        /// </summary>
        /// <param name="zoom"></param>
        public void Test(int zoom)
        {
            LonLat lonLat = new LonLat( -87.6500523, 41.850033);

            string latlngStr = "Chicago, IL" + "<br />" + "LatLng: " + lonLat.Lat + " , " + lonLat.Lon + "<br />";

            MercatorProjection projection = new MercatorProjection();
            //�������꣨Mecator��
            XY worldCoordinate = projection.LonLatToMecatorXy(lonLat);
            string worldCoordStr = "World Coordinate: " + worldCoordinate.X + " , " + worldCoordinate.Y;

            //��������
            XY pixelCoordinate = new XY(worldCoordinate.X * Math.Pow(2, zoom), worldCoordinate.Y * Math.Pow(2, zoom));


            string pixelCoordStr = "<br />Pixel Coordinate: " + Math.Floor(pixelCoordinate.X) + " , " + Math.Floor(pixelCoordinate.Y);

            //��Ƭ���ꡣ
            XY tileCoordinate = new XY(Math.Floor(pixelCoordinate.X / MERCATOR_RANGE), Math.Floor(pixelCoordinate.Y / MERCATOR_RANGE));
            string tileCoordStr = "<br />Tile Coordinate: " + tileCoordinate.X + " , " + tileCoordinate.Y + " at Zoom Level: " + zoom;


        }


        /// <summary>
        /// �õ�Google Tile ��š�
        /// </summary>
        /// <param name="lonLat"></param>
        /// <returns></returns>
        public XY LonLatToTileXy(LonLat lonLat, int zoom)
        {
            XY worldCoordinate = LonLatToMecatorXy(lonLat);
            //��������
            double temp = Math.Pow(2, zoom);
            XY pixelCoordinate = new XY(worldCoordinate.X * temp, worldCoordinate.Y *temp);
            //��Ƭ���ꡣ
            XY tileCoordinate = new XY(Math.Floor(pixelCoordinate.X / MERCATOR_RANGE), Math.Floor(pixelCoordinate.Y / MERCATOR_RANGE));
            return tileCoordinate;
        }

        /// <summary>
        /// ��Ƭ���� To LonLat
        /// </summary>
        /// <param name="tileXy"></param>
        /// <param name="zoom"></param>
        /// <returns></returns>
        public LonLat TileXyToLonLat(XY tileXy, int zoom)
        {
            //��������
            XY pixelCoordinate = new XY(tileXy.X * MERCATOR_RANGE, tileXy.Y * MERCATOR_RANGE);
            double temp =  Math.Pow(2, zoom);
            XY mecatorXy = new XY(pixelCoordinate.X / temp, pixelCoordinate.Y / temp);
           return  MecatorXyToLonLat( mecatorXy);
        }

        /// <summary>
        /// World Coordinate
        /// </summary>
        /// <param name="lonLat"></param>
        /// <returns></returns>
        public XY LonLatToMecatorXy(LonLat lonLat)
        { 
            XY point = new XY();
            XY origin = this.pixelOrigin;
            point.X = origin.X + lonLat.Lon * this.pixelsPerLonDegree;
            // NOTE(appleton): Truncating to 0.9999 effectively limits latitude to
            // 89.189.  This is about a third of a tile past the edge of the world tile.
            double siny = Bound(Math.Sin(DegreesToRadians(lonLat.Lat)), -0.9999, 0.9999);
            point.Y = origin.Y + 0.5 * Math.Log((1 + siny) / (1 - siny)) * -this.pixelsPerLonRadian;
            return point;
        }

        /// <summary>
        /// MecatorXyToLonLat
        /// </summary>
        /// <param name="mecatorXy"></param>
        /// <returns></returns>
        public LonLat MecatorXyToLonLat(XY mecatorXy)
        {
            XY origin = this.pixelOrigin;
            double lng = (mecatorXy.X - origin.X) / this.pixelsPerLonDegree;
            double latRadians = (mecatorXy.Y - origin.Y) / -this.pixelsPerLonRadian;
            double lat = RadiansToDegrees(2 * Math.Atan(Math.Exp(latRadians)) - CoordConsts.PI / 2);
            return new LonLat(lng, lat);
        }

        /// <summary>
        /// ʹ���ֲ��������ޡ�
        /// </summary>
        /// <param name="value"></param>
        /// <param name="opt_min"></param>
        /// <param name="opt_max"></param>
        /// <returns></returns>
        double Bound(double value, double opt_min, double opt_max)
        {
            value = Math.Max(value, opt_min);
            value = Math.Min(value, opt_max);
            return value;
        }
        /// <summary>
        /// ת��
        /// </summary>
        /// <param name="deg"></param>
        /// <returns></returns>
        public double DegreesToRadians(double deg)
        {
            return deg * (CoordConsts.PI / 180);
        }
        /// <summary>
        /// ת��
        /// </summary>
        /// <param name="rad"></param>
        /// <returns></returns>
        public double RadiansToDegrees(double rad)
        {
            return rad / (CoordConsts.PI / 180);
        }

    }
}
