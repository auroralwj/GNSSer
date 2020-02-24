//2014.07.01, czs, create in TianJing, 提取接口，不限于 LonLat

using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using System.Diagnostics;
using Geo.Coordinates;
using Geo.Utils;
using Geo.Algorithm;


namespace Geo.Coordinates
{
    /// <summary>
    /// 无所不在的盒子、边界、窗口、矩形。
    /// 以 （X， Y）为坐标，笛卡尔右手坐标系，Y为竖轴指向上对应高度，X为横轴指向右对应宽度。
    /// Box, Bound, LonLatEnvelope, ViewPort.
    /// </summary>
    [Serializable]
    public class LonLatEnvelope : Box<LonLat>, ILonLatEnvelope
    {
        #region constructor
        /// <summary>
        /// 默认构造函数。
        /// </summary>
        public LonLatEnvelope() { }
        /// <summary>
        /// 以一个点初始化。
        /// </summary>
        /// <param name="xy"></param>
        public LonLatEnvelope(LonLat xy):base(xy)
        { 
        }
        /// <summary>
        /// 以四个参数初始化
        /// </summary>
        /// <param name="minX"></param>
        /// <param name="minY"></param>
        /// <param name="maxX"></param>
        /// <param name="maxY"></param>
        public LonLatEnvelope(double minX, double minY, double maxX, double maxY)
            : base(minX,   minY,   maxX,   maxY)
        { 
        }
        /// <summary>
        /// 以中心点初始化
        /// </summary>
        /// <param name="centerXy"></param>
        /// <param name="width"></param>
        /// <param name="height"></param>
        public LonLatEnvelope(LonLat centerXy, double width, double height)
            : base(centerXy, width, height)
        { 
        }
        /// <summary>
        /// 以中心点初始化
        /// </summary>
        /// <param name="centerXy"></param>
        /// <param name="distance"></param> 
        public LonLatEnvelope(LonLat centerXy, double distance)
            : base(centerXy, distance, distance)
        { 
        }

        /// <summary>
        /// 自动提取LonLat的最大小X、Y值生成一个LonLatEnvelope。
        /// </summary>
        /// <param name="xy1"></param>
        /// <param name="xy2"></param>
        public LonLatEnvelope(LonLat xy1, LonLat xy2)
            : base(xy1, xy2)
        {
        }
        public LonLatEnvelope(Vector xy1, Vector xy2)
            : base(xy1, xy2)
        {
        }


        public LonLatEnvelope(XYZ xy1, XYZ xy2)
            : base(xy1, xy2)
        { 
        }
         
          
        #endregion 

        #region property
        public double MaxX { get { return this.MaxHorizontal; } }
        public double MaxY { get { return this.MaxVertical; } }
        public double MinX { get { return this.MinHorizontal; } }
        public double MinY { get { return this.MinVertical; } }
        public  ILonLatEnvelope Expands(ILonLatEnvelope bbox)
        {
            if (bbox == null)
                return (LonLatEnvelope)Clone();
            else
                return new LonLatEnvelope(Math.Min(MinHorizontal, bbox.MinHorizontal), Math.Min(MinVertical, bbox.MinVertical),
                                       Math.Max(MaxHorizontal, bbox.MaxHorizontal), Math.Max(MaxVertical, bbox.MaxVertical));
        }

        public ILonLatEnvelope And(ILonLatEnvelope other)
        {
            if (this.IntersectsWith(other))
            {
                double minX = DoubleUtil.Max(this.MinHorizontal, other.MinHorizontal);
                double minY = DoubleUtil.Max(this.MinVertical, other.MinVertical);
                double maxX = DoubleUtil.Min(this.MaxHorizontal, other.MaxHorizontal);
                double maxY = DoubleUtil.Min(this.MaxVertical, other.MaxVertical);
                return new LonLatEnvelope(minX, minY, maxX, maxY);
            }
            return null;
        }

        public override LonLat Center { get { return new LonLat((MinHorizontal + MaxHorizontal) / 2, (MinVertical + MaxVertical) / 2); } }
        public override LonLat LeftTop { get { return new LonLat(MinHorizontal, MaxVertical); } }
        public override LonLat LeftBottom { get { return new LonLat(MinHorizontal, MinVertical); } }
        public override LonLat RightTop { get { return new LonLat(MaxHorizontal, MaxVertical); } }
        public override LonLat RightBottom { get { return new LonLat(MaxHorizontal, MinVertical); } } 
        #endregion

        #region toperlogy 
         
        public static LonLatEnvelope None
        {
            get
            {
                return new LonLatEnvelope(Double.NaN, Double.NaN, Double.NaN, Double.NaN);
            }
        }
        public static LonLatEnvelope Zero
        {
            get
            {
                return new LonLatEnvelope(0, 0, 0, 0);
            }
        }
        #endregion

        #region override 

        /// <summary>
        /// 自定义的格式化输出。
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return "LonLatEnvelope [maxX=" + MaxHorizontal.ToString("0.#####") + ", maxY=" + MaxVertical.ToString("0.#####") + ", minX=" + MinHorizontal.ToString("0.#####")
                    + ", minY=" + MinVertical.ToString("0.#####") + "]";
        }
        /// <summary>
        /// 持久化为XML
        /// </summary>
        /// <returns></returns>
        public string ToXmlStub()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("<LonLatEnvelope>");
            sb.Append("<MinX>" + MinHorizontal.ToString("0.0000000") + "</MinX>");
            sb.Append("<MinY>" + MinVertical.ToString("0.0000000") + "</MinY>");
            sb.Append("<MaxX>" + MaxHorizontal.ToString("0.0000000") + "</MaxX>");
            sb.Append("<MaxY>" + MaxVertical.ToString("0.0000000") + "</MaxY>"); 
            sb.Append("</LonLatEnvelope>");
            return sb.ToString();
        }
        /// <summary>
        /// <LonLatEnvelope><MinX>116.0000000</MinX><MinY>39.0000000</MinY><MaxX>116.0000000</MaxX><MaxY>40.0000000</MaxY></LonLatEnvelope>
        /// </summary>
        /// <param name="xmlString"></param>
        /// <returns></returns>
        public static  LonLatEnvelope ParseXmlStub(String xmlString)
        {
            XmlDocument doc = new XmlDocument();
            doc.LoadXml(xmlString);

           // string minXStr = 
            double minX = Double.Parse(doc.SelectSingleNode("./LonLatEnvelope/MinX").InnerText);
            double minY = Double.Parse(doc.SelectSingleNode("./LonLatEnvelope/MinY").InnerText);
            double maxX = Double.Parse(doc.SelectSingleNode("./LonLatEnvelope/MaxX").InnerText);
            double maxY = Double.Parse(doc.SelectSingleNode("./LonLatEnvelope/MaxY").InnerText);
            return new LonLatEnvelope(minX, minY, maxX, maxY);
        }
        #endregion       
    }
}
