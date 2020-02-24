//2017.10.14, czs, create in hongiqng, 大地坐标集合

using System;
using System.Collections.Generic;
using System.Text;
using Geo.Referencing;
using Geo.Algorithm;
using Geo.Utils;

namespace Geo.Coordinates
{
    /// <summary>
    /// 大地坐标集合
    /// </summary>
    public class GeoCoords
    {
        /// <summary>
        /// 大地坐标集合
        /// </summary>
        /// <param name="coords"></param>
        public GeoCoords(List<GeoCoord> coords)
        {
            this.Coords = coords;
            double MaxLon = double.MinValue;
            double MaxLat = double.MinValue;
            double MinLon = double.MaxValue;
            double MinLat = double.MaxValue;
            double minHeight = double.MaxValue;
            double maxHeight = double.MinValue;
            //LatInterval = double.MaxValue;
            //LonInterval = double.MaxValue;
            List<double> LatList = new List<double>();
            List<double> LonList = new List<double>();
            foreach (var item in this.Coords)
            {
                if (MaxLon < item.Lon) { MaxLon = item.Lon; }
                if (MaxLat < item.Lat) { MaxLat = item.Lat; }
                if (MinLon > item.Lon) { MinLon = item.Lon; }
                if (MinLat > item.Lat) { MinLat = item.Lat; }
                if (maxHeight < item.Height) { maxHeight = item.Height; }
                if (minHeight > item.Height) { minHeight = item.Height; }
                if (!LatList.Contains(item.Lat)) { LatList.Add(item.Lat); }
                if (!LonList.Contains(item.Lon)) { LonList.Add(item.Lon); }
            }
            this.LatSpan = new NumerialSegment(MinLat, MaxLat);
            this.LonSpan = new NumerialSegment(MinLon, MaxLon);
            this.HeightSpan = new NumerialSegment(minHeight, maxHeight);
            LonList.Sort();
            LonInterval = Math.Abs(LonList[1] - LonList[0]);
            LatList.Sort();
            LatInterval = Math.Abs(LatList[1] - LatList[0]);
        }

        public double LatInterval { get; set; }
        public double LonInterval { get; set; }
        public List<GeoCoord> Coords { get; set; }

        public XY CoordFrom { get { return new XY(LonSpan.Start, LatSpan.Start); } }

        public NumerialSegment LatSpan { get; set; }
        public NumerialSegment LonSpan { get; set; }
        public NumerialSegment HeightSpan { get; set; }
        /// <summary>
        /// 范围大小
        /// </summary>
        public XY Size { get { return new XY(LonSpan.Span, LatSpan.Span); } }
    }
}