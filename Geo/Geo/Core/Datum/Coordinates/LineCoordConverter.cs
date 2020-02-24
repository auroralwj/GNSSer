//2017.10.13, czs, create in hongqing, 线性坐标数据转换器

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using Geo.Coordinates;


namespace Geo.Coordinates
{  
    /// <summary>
    /// 线性坐标数据转换器
    /// </summary>
    public class LineCoordConverter
    {
        /// <summary>
        /// 线性坐标转换器
        /// </summary>
        /// <param name="oldSartVal">老坐标的起始位置，计算时需要减去</param>
        public LineCoordConverter(double oldSartVal = 0)
        {
            this.Factor = 1;
            this.OldStartValue = oldSartVal; 
        }
        /// <summary>
        /// 线性坐标转换器
        /// </summary>
        /// <param name="fromScale"></param>
        /// <param name="toScale"></param>
        /// <param name="oldSartVal">老坐标的起始位置，计算时需要减去</param>
        public LineCoordConverter(double fromScale, double toScale,double oldSartVal = 0)
        {
            this.OldStartValue = oldSartVal;
            this.Factor = toScale/fromScale ;
        }
        /// <summary>
        /// 线性坐标转换器
        /// </summary>
        /// <param name="fromScale"></param>
        /// <param name="toScale"></param> 
        public LineCoordConverter(NumerialSegment fromScale, double toScale)
        {
            this.OldStartValue = fromScale.Start;                ;
            this.Factor = toScale / fromScale.Span;
        }
        /// <summary>
        /// 起始数据
        /// </summary>
        public double OldStartValue { get; set; }

        /// <summary>
        /// 乘法因子
        /// </summary>
        public double Factor { get; set; }
        /// <summary>
        /// 获取新值
        /// </summary>
        /// <param name="old"></param>
        /// <returns></returns>
        public double GetNew(double old) { return (old - OldStartValue )* Factor; }
    }
}
