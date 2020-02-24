//2014.11.07, czs, edit in namu, 提取接口，不限于 XY

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
    /// Box, Bound, Envelope, ViewPort.
    /// </summary>
    [Serializable]
    public class Envelope : Box<XY>, IEnvelope
    {
        #region constructor
        /// <summary>
        /// 默认构造函数。
        /// </summary>
        public Envelope() { }
        /// <summary>
        /// 以一个点初始化。
        /// </summary>
        /// <param name="xy"></param>
        public Envelope(XY xy):base(xy)
        { 
        }
        /// <summary>
        /// 以四个参数初始化
        /// </summary>
        /// <param name="minX"></param>
        /// <param name="minY"></param>
        /// <param name="maxX"></param>
        /// <param name="maxY"></param>
        public Envelope(double minX, double minY, double maxX, double maxY)
            : base(minX,   minY,   maxX,   maxY)
        { 
        }
        /// <summary>
        /// 以中心点初始化
        /// </summary>
        /// <param name="centerXy"></param>
        /// <param name="width"></param>
        /// <param name="height"></param>
        public Envelope(Geo.Algorithm.Vector centerXy, double width, double height)
            : base(  new XY(centerXy.OneDimArray), width, height)
        { 
        }
        /// <summary>
        /// 以中心点初始化
        /// </summary>
        /// <param name="centerXy"></param>
        /// <param name="width"></param>
        /// <param name="height"></param>
        public Envelope(XY centerXy, double width, double height)
            : base(centerXy, width, height)
        { 
        }

        /// <summary>
        /// 自动提取XY的最大小X、Y值生成一个Envelope。
        /// </summary>
        /// <param name="xy1"></param>
        /// <param name="xy2"></param>
        public Envelope(XY xy1, XY xy2)
            : base(xy1, xy2)
        {
        }
        /// <summary>
        /// 盒子构造函数
        /// </summary>
        /// <param name="xy1"></param>
        /// <param name="xy2"></param>
        public Envelope(Vector xy1, Vector xy2)
            : base(xy1, xy2)
        {
        }

        /// <summary>
        /// 盒子构造函数
        /// </summary>
        /// <param name="xy1"></param>
        /// <param name="xy2"></param>
        public Envelope(XYZ xy1, XYZ xy2)
            : base(xy1, xy2)
        { 
        }
         
          
        #endregion 

        #region property
        public double MaxX { get { return this.MaxHorizontal; } }
        public double MaxY { get { return this.MaxVertical; } }
        public double MinX { get { return this.MinHorizontal; } }
        public double MinY { get { return this.MinVertical; } }
        public  IEnvelope Expands(IEnvelope bbox)
        {
            if (bbox == null)
                return (Envelope)Clone();
            else
                return new Envelope(Math.Min(MinHorizontal, bbox.MinHorizontal), Math.Min(MinVertical, bbox.MinVertical),
                                       Math.Max(MaxHorizontal, bbox.MaxHorizontal), Math.Max(MaxVertical, bbox.MaxVertical));
        }

        public IEnvelope And(IEnvelope other)
        {
            if (this.IntersectsWith(other))
            {
                double minX = DoubleUtil.Max(this.MinHorizontal, other.MinHorizontal);
                double minY = DoubleUtil.Max(this.MinVertical, other.MinVertical);
                double maxX = DoubleUtil.Min(this.MaxHorizontal, other.MaxHorizontal);
                double maxY = DoubleUtil.Min(this.MaxVertical, other.MaxVertical);
                return new Envelope(minX, minY, maxX, maxY);
            }
            return null;
        }

        public override XY Center { get { return new XY((MinHorizontal + MaxHorizontal) / 2, (MinVertical + MaxVertical) / 2); } }
        public override XY LeftTop { get { return new XY(MinHorizontal, MaxVertical); } }
        public override XY LeftBottom { get { return new XY(MinHorizontal, MinVertical); } }
        public override XY RightTop { get { return new XY(MaxHorizontal, MaxVertical); } }
        public override XY RightBottom { get { return new XY(MaxHorizontal, MinVertical); } } 
        #endregion

        #region toperlogy 
         
        public static Envelope None
        {
            get
            {
                return new Envelope(Double.NaN, Double.NaN, Double.NaN, Double.NaN);
            }
        }
        public static Envelope Zero
        {
            get
            {
                return new Envelope(0, 0, 0, 0);
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
            return "Envelope [maxX=" + MaxHorizontal.ToString("0.#####") + ", maxY=" + MaxVertical.ToString("0.#####") + ", minX=" + MinHorizontal.ToString("0.#####")
                    + ", minY=" + MinVertical.ToString("0.#####") + "]";
        }
        /// <summary>
        /// 持久化为XML
        /// </summary>
        /// <returns></returns>
        public string ToXmlStub()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("<Envelope>");
            sb.Append("<MinX>" + MinHorizontal.ToString("0.0000000") + "</MinX>");
            sb.Append("<MinY>" + MinVertical.ToString("0.0000000") + "</MinY>");
            sb.Append("<MaxX>" + MaxHorizontal.ToString("0.0000000") + "</MaxX>");
            sb.Append("<MaxY>" + MaxVertical.ToString("0.0000000") + "</MaxY>"); 
            sb.Append("</Envelope>");
            return sb.ToString();
        }
        /// <summary>
        /// <Envelope><MinX>116.0000000</MinX><MinY>39.0000000</MinY><MaxX>116.0000000</MaxX><MaxY>40.0000000</MaxY></Envelope>
        /// </summary>
        /// <param name="xmlString"></param>
        /// <returns></returns>
        public static  Envelope ParseXmlStub(String xmlString)
        {
            XmlDocument doc = new XmlDocument();
            doc.LoadXml(xmlString);

           // string minXStr = 
            double minX = Double.Parse(doc.SelectSingleNode("./Envelope/MinX").InnerText);
            double minY = Double.Parse(doc.SelectSingleNode("./Envelope/MinY").InnerText);
            double maxX = Double.Parse(doc.SelectSingleNode("./Envelope/MaxX").InnerText);
            double maxY = Double.Parse(doc.SelectSingleNode("./Envelope/MaxY").InnerText);
            return new Envelope(minX, minY, maxX, maxY);
        }
        #endregion       
    }
}
