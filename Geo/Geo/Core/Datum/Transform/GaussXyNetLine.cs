//2019.01.12, czs, create in hmx, 高斯平面坐标基线向量网

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Geo.Referencing;
using Geo.Algorithm.Adjust;
using Geo.Algorithm;

namespace Geo.Coordinates
{
    
    /// <summary>
    /// 高斯平面坐标基线向量网
    /// </summary>
    public class GaussXyNetLine : BaseDictionary<string, GaussPoint>
    { 
        public GaussXyNetLine( )
        {
        }

        /// <summary>
        /// 所有的点位表格
        /// </summary> 
        /// <returns></returns>
        public ObjectTableStorage GetPointTable()
        { 
            ObjectTableStorage table = new ObjectTableStorage();
            int i = 1;
            foreach (var line in this.Values)
            {
                table.NewRow();
                table.AddItem("Num", i++);
                table.AddItem(line.GetObjectRow());
            }
            return table;
        }

        /// <summary>
        /// 所有的向量表格
        /// </summary> 
        /// <returns></returns>
        public ObjectTableStorage GetAllLineTable()
        {
            List<GaussXyVectorLine> lines = GetAllLines();
            ObjectTableStorage table = new ObjectTableStorage();
            int i = 1;
            foreach (var line in lines)
            {
                table.NewRow();
                table.AddItem("Num", i++);
                table.AddItem(line.GetObjectRow());
            }
            return table;
        }

        /// <summary>
        /// 获取所有基线
        /// </summary> 
        /// <returns></returns>
        public List<GaussXyVectorLine>  GetAllLines( )
        {
            var vals = this.Values;
            var keys = this.Keys;
            var length = this.Count;
            List<GaussXyVectorLine> result = new List<GaussXyVectorLine>();
            for (int i = 0; i < length; i++)
            {
                var startName = keys[i];
                var startXy = vals[i];
                 

                for (int j = 0; j < i; j++)
                {
                    if(i == j) { continue; }
                    var endName = keys[j];
                    var endXy = vals[j];
                    var line = new GaussXyVectorLine(startXy, endXy);
                    result.Add( line); 
                }
            }
            return result;
        }

        /// <summary>
        /// 解析表格
        /// </summary>
        /// <param name="table"></param>
        /// <param name="OrinalLonDeg"></param>
        /// <param name="AveGeoHeight"></param>
        /// <param name="YConst"></param>
        /// <returns></returns>
        public static GaussXyNetLine Parse(ObjectTableStorage table, double OrinalLonDeg, double AveGeoHeight, double YConst)
        {
            GaussXyNetLine result = new GaussXyNetLine();
            foreach (var item in table.BufferedValues)
            {
                string name = null;
                XY xy = new XY();
                foreach (var item2 in item)
                {
                    if (item2.Key.ToUpper().Contains("NAME")) { name = item2.Value.ToString(); }
                    if (xy.X == 0 && (item2.Key.ToUpper().Contains("X"))) { xy.X = Geo.Utils.ObjectUtil.GetNumeral(item2.Value); }
                    if (xy.Y == 0 && (item2.Key.ToUpper().Contains("Y"))) { xy.Y = Geo.Utils.ObjectUtil.GetNumeral(item2.Value); }
                }
                var lonlatStart = Geo.Coordinates.GeodeticUtils.GaussXyToLonLat(xy, AveGeoHeight, OrinalLonDeg, YConst);
                GaussPoint gaussPoint = new GaussPoint()
                {
                    Name = name,
                    XY = xy,
                    LonLat = lonlatStart,
                    AveGeoHeight = AveGeoHeight,
                };
                result[name] = gaussPoint;
            }
            return result;
        }

    }

    /// <summary>
    /// 高斯坐标
    /// </summary>
    public class GaussPoint : IObjectRow
    {
        public GaussPoint() { }
        public GaussPoint(string Name)
        {
            this.Name = Name;
        }
        /// <summary>
        /// 名称
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 坐标
        /// </summary>
        public XY XY { get; set; }
        /// <summary>
        /// 经纬度
        /// </summary>
        public LonLat LonLat { get; set; }
        /// <summary>
        /// 平均大地高，膨胀椭球法
        /// </summary>
        public double AveGeoHeight { get; set; }


        /// <summary>
        /// 行数据
        /// </summary>
        /// <returns></returns>
        public Dictionary<string, object> GetObjectRow()
        {
            Dictionary<string, object> row = new Dictionary<string, object>();
            row["Name"] = Name;
            row["X"] = XY.X;
            row["Y"] = XY.Y;
            row["LonDMS"] = new DMS( this.LonLat.Lon);
            row["LatDMS"] = new DMS(this.LonLat.Lat);
            row["AveGeoHeight"] = AveGeoHeight;
            row["Lon"] = this.LonLat.Lon;
            row["Lat"] = this.LonLat.Lat;
             
           return row; 
        }



        public override bool Equals(object obj)
        {
            var o = obj as GaussPoint;
            if(o == null) { return false; }

            return o.Equals(this.Name);
        }
        public override int GetHashCode()
        {
            return Name.GetHashCode();
        }
        public override string ToString()
        {
            return Name;
        }
    }

    /// <summary>
    /// 高斯XY向量
    /// </summary>
    public class GaussXyVectorLine
    { 
        public GaussXyVectorLine(GaussPoint start, GaussPoint end)
        {
            LineName = new VectorLineName(start.Name, end.Name); 
            this.Start = start;
            this.End = end;
        }
        /// <summary>
        /// 基线向量名称
        /// </summary>
        public VectorLineName LineName { get; set; }
        /// <summary>
        /// 起始点
        /// </summary>
        public GaussPoint Start { get; set; }
        /// <summary>
        /// 结束
        /// </summary>
        public GaussPoint End { get; set; }
        /// <summary>
        /// 起始坐标
        /// </summary>
        public XY YyOfStart => Start.XY;
        /// <summary>
        /// 结束坐标
        /// </summary>
        public XY YyOfEnd => End.XY;
        /// <summary>
        /// 坐标向量
        /// </summary>
        public XY VectorXy => this.YyOfEnd - YyOfStart;
        /// <summary>
        /// 长度
        /// </summary>
        public double Length => VectorXy.Norm;
        /// <summary>
        /// 平面方位角 度
        /// </summary>
        public double Azimuth => Geo.Coordinates.CoordTransformer.GetAzimuthAngleInLeftHandXyCoordSystem(VectorXy);
        /// <summary>
        /// 平面方位角
        /// </summary>
        public DMS AzimuthDMS => new DMS(Azimuth, AngleUnit.Degree);
        /// <summary>
        /// 大地线长
        /// </summary>
        public double GeodeticLength => Geo.Coordinates.GeodeticUtils.BesselGeodeticLine(Start.LonLat, End.LonLat, Start.AveGeoHeight);

        /// <summary>
        /// 大地方位角
        /// </summary>
        public double GeodeticAzimuth =>  Geo.Coordinates.GeodeticUtils.BesselAzimuthAngle(Start.LonLat, End.LonLat);
        /// <summary>
        /// 大地方位角
        /// </summary>
        public DMS GeodeticAzimuthDMS=> new DMS(GeodeticAzimuth, AngleUnit.Degree);
          
        /// <summary>
        /// 反转
        /// </summary>
        public GaussXyVectorLine Reversed => new GaussXyVectorLine(End, Start);

        /// <summary>
        /// 行数据
        /// </summary>
        /// <returns></returns>
        public Dictionary<string, object> GetObjectRow()
        {
            Dictionary<string, object> row = new Dictionary<string, object>();
            row["Name"] = LineName;
            row["Length"] = Length; 
            row["GeodeticLength"] = GeodeticLength;
            row["AzimuthDMS)"] = AzimuthDMS;
            row["GeodeticAzimuthDMS"] = GeodeticAzimuthDMS;

            var differLen = (GeodeticLength - Length) * 100; 
            row["长度差(厘米)"] = differLen;

            var differAngle = (GeodeticAzimuth - Azimuth) * 3600;
            row["角度差(秒)"] = differAngle;


            row["GeodeticAzimuth"] = GeodeticAzimuth;
            row["Azimuth"] = Azimuth;

            return row;

        }


    }
}
