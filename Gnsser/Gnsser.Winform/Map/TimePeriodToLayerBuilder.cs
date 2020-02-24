//2015.10.19, czs, create in Train of 成都到西安, 在AnyInfo地图上显示观测数据的时段信息

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AnyInfo;
using AnyInfo.Features;
using AnyInfo.Geometries;
using Gnsser.Times;
using Geo.Times;
using Gnsser.Service;
using Gnsser.Service;
using Gnsser.Domain;
using Gnsser;
using AnyInfo.Styles;
using Geo.Coordinates;
using System.Drawing;

namespace Gnsser.Winform
{
    /// <summary>
    /// 在AnyInfo地图上显示观测数据的时段信息
    /// </summary>
    public class TimePeriodToLayerBuilder : Geo.IBuilder<Layer>
    {
        public TimePeriodToLayerBuilder( SatPeriodInfoManager SatPeriodMarker)
        {
            this.SatPeriodInfoManager = SatPeriodMarker;
        }

        SatPeriodInfoManager SatPeriodInfoManager { get; set; }
        /// <summary>
        /// 建立图层
        /// </summary>
        /// <returns></returns>
        public Layer Build()
        {
            List<LineString> lines = new List<LineString>();
            var peroid  = SatPeriodInfoManager.TimePeriod;
            var spanSec = peroid.Span;
            var spanIn180Deg = spanSec / 180.0;
            var startTime = peroid.Start;
            var endTime = peroid.End;

            double lat = -60;
            double lon = 0;

            int i = 0;
            foreach (var sat in SatPeriodInfoManager.Data)
            {
              
                foreach (var p in sat.Value)
                {
                    var pts = new List<AnyInfo.Geometries.Point>();
                    var from = (p.Start - startTime) / spanIn180Deg;
                    var to = (p.End - startTime) / spanIn180Deg;

                    var ptFrom = new AnyInfo.Geometries.Point(from, lat, i + "_from", sat.Key + "");
                    var ptTo = new AnyInfo.Geometries.Point(to, lat, i + "_to", sat.Key + "");
                    pts.Add(ptFrom);
                    pts.Add(ptTo);

                    var line = new LineString(pts, sat.Key + "_" + p.ToTimeString());

                    lines.Add(line);
                }

                lat = lat + 1;//纬度每次递增 
                i++;
            }

           // Layer layer = LayerFactory.CreateLineLayer(lines);

            //foreach (var path in dicData)
            //    CreatePtFeature(layer, path.Value, path.Key);


            //Create SimpleFeatureType
            SimpleFeatureType featureType = SimpleFeatureTypeFactory.GetDefaultLineStringFeatureType();

            //Create Geometry
            Dictionary<string, SimpleFeature> featureDic = new Dictionary<string, SimpleFeature>();
            

            IEnvelope envelope = null;
            Color color = Color.AliceBlue;
            int j = 0;
            foreach (LineString lineString in lines)
            {
                var style = new LineStyle();
                style.Width = 5;
                style.Color = GetColor(j++, lines.Count);

                // AnyInfo.Geometries.Point point = new AnyInfo.Geometries.Point(lonLat.Lon, lonLat.Lat);
                SimpleFeature feature =LayerFactory. CreateLineStringFeature(featureType, lineString, style);
                featureDic.Add(feature.Id, feature);

                if (envelope == null) envelope = lineString.Box;
                else envelope = envelope.Expands(lineString.Box);
            }

            //Create Feature Collection
            FeatureCollection<SimpleFeatureType, SimpleFeature> featureCollection =
                new FeatureCollection<SimpleFeatureType, SimpleFeature>(featureType, featureDic);

            Layer layer = new Layer("时段绘图", featureCollection, envelope);
            return layer; 


            //layer.FeatureSource.BuildIndexing();
            //layer.UseLayerStyle = false;
            //return (layer);
        }

        public Color GetColor(int i, int totalCount)
        {
            var all = 768;
            var raw = 1.0* i/ totalCount * all;

            var val = (int)(raw);

            if (val < 256)
            {
                return Color.FromArgb(val, 0, 0);
            }
            if (val > 255 && val < 512)
            {
                  return Color.FromArgb(0, val -256, 0);
            }
            return Color.FromArgb(0,0, val - 512);

        }
        /// <summary>
        /// 创建点图元素
        /// </summary>
        /// <param name="layer">图层</param>
        /// <param name="xyz">坐标</param>
        /// <param name="name">名称</param>
        /// <returns></returns>
        private static SimpleFeature CreatePtFeature(Layer layer, XYZ xyz, string name)
        {
            AnyInfo.Geometries.Point p = new AnyInfo.Geometries.Point(CoordTransformer.XyzToGeoCoord(xyz));
            p.Name = name;
            p.Id = name + new Random().Next(10000);

            SimpleFeatureBuilder b = new SimpleFeatureBuilder(layer.FeatureSource.FeatureCollection.Schema);
            b.SetGeometry(p);
            b.SetName(p.Name);
            b.SetFeatureId(p.Id);
            b.SetStyle(new AnyInfo.Styles.PointStyle() { Color = System.Drawing.Color.Red, Diameter = 12 });
            SimpleFeature sf = b.Build();

            layer.FeatureSource.FeatureCollection.Add(sf);
            layer.Extent.Expands(sf.Envelope);
            return sf;
        }
    }
}
