//2014.12.07, czs, create in jinxinliangmao shaungliao, 
//2018.11.02, czs, edit in hmx, 增加点位名称显示

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AnyInfo;
using AnyInfo.Features;
using AnyInfo.Geometries;
using Gnsser.Times;
using Gnsser.Service;
using Geo.Coordinates;

namespace Gnsser.Winform
{
    /// <summary>
    /// 定位结果 AnyInfo 图层生成器
    /// </summary>
    public class PostionResultLayerBuilder : Geo.IBuilder<Layer>
    {
        public PostionResultLayerBuilder(List<BaseGnssResult> _results, int startEpoch)
        {
            if (_results == null || _results.Count == 0) throw new Exception("输入结果为空。");
            if (startEpoch >= _results.Count) throw new Exception("起始历元编号过大！总结果数 " + _results.Count + "，起始历元编号：" + startEpoch);

            this.start = startEpoch;

            this._results = new List<NamedRmsXyz>();
            foreach (var item in _results.ToArray())
            {
                if(item is SingleSiteGnssResult){
                    var  result = item as SingleSiteGnssResult;
                   this._results.Add(new NamedRmsXyz(result.Name, new RmsedXYZ(result.EstimatedXyz, result.EstimatedXyzRms)));                    
                }
                
            }
            dicData = new Dictionary<string, XYZ>();
        }
        public PostionResultLayerBuilder(List<NamedRmsXyz> _results, int startEpoch)
        {
            if (_results == null || _results.Count == 0) throw new Exception("输入结果为空。");
            if (startEpoch >= _results.Count) throw new Exception("起始历元编号过大！总结果数 " + _results.Count + "，起始历元编号：" + startEpoch);

            this.start = startEpoch;
            this._results = _results;
            dicData = new Dictionary<string, XYZ>();
        }

        int start;
        List<NamedRmsXyz> _results { get; set; }
        Dictionary<string, XYZ> dicData { get; set; }

        /// <summary>
        /// 添加一个现实点。
        /// </summary>
        /// <param name="xyz"></param>
        /// <param name="name"></param>
        public void AddPt(XYZ xyz, string name)
        {
            dicData.Add(name, xyz);
        }


        /// <summary>
        /// 建立图层
        /// </summary>
        /// <returns></returns>
        public Layer Build()
        { 
            int index = 0;
            List<Point> lonLats = new List<Point>();
            foreach (var item in _results.ToArray())
            {
                index++;
                if (index > start)
                {
                   var single = item;
                   var geoCoord = Geo.Coordinates.CoordTransformer.XyzToGeoCoord(item.Value.Value);
                    var pt = new Point(geoCoord, index.ToString(), item.Name);
                   lonLats.Add(pt); 
                }
            }

            Layer layer = LayerFactory.CreatePointLayer(lonLats);

            foreach (var item in dicData)
            {
                CreatePtFeature(layer, item.Value, item.Key);
            }

            layer.FeatureSource.BuildIndexing();
            layer.UseLayerStyle = false;
            return (layer);
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
