//2017.10.14, czs, create in hongqing, 颜色转换器

using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;

namespace Geo.Draw
{
     /// <summary>
    /// 增加中间渐变颜色
    /// </summary>
    public class ThreeStepColorBuilder : AbstractBuilder<Color, double>, Geo.Draw.INumeralColorBuilder
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="Span"></param>
        /// <param name="ColorFrom"></param>
        /// <param name="ColorTo"></param>
        public ThreeStepColorBuilder(NumerialSegment Span, Color ColorFrom, Color ColorMiddle, Color ColorMiddleB, Color ColorTo)
        {
            var smallSpan = Span.Span / 3.0;

            FistStep = Span.Start + smallSpan;
            SecondStep = Span.Start + smallSpan * 2;
            var segA = new NumerialSegment(Span.Start, Span.Start + smallSpan);
            var segB = new NumerialSegment(Span.Start + smallSpan, Span.Start + smallSpan *2);
            var segC = new NumerialSegment(Span.Start + smallSpan * 2, Span.End);
            ColorBuilderA = new ColorBuilder(segA, ColorFrom, ColorMiddle);
            ColorBuilderB = new ColorBuilder(segB, ColorMiddle, ColorMiddleB);
            ColorBuilderC = new ColorBuilder(segC, ColorMiddleB, ColorTo);
        }

        #region 属性
        /// <summary>
        /// 第一段颜色构造器
        /// </summary>
        public ColorBuilder ColorBuilderA { get; set; }
        /// <summary>
        /// 第二段颜色构造器
        /// </summary>
        public ColorBuilder ColorBuilderB { get; set; }
        /// <summary>
        /// 颜色三段构造器
        /// </summary>
        public ColorBuilder ColorBuilderC { get; set; }
        /// <summary>
        /// 第一段值
        /// </summary>
        public double FistStep { get; set; }
        /// <summary>
        /// 第二段值
        /// </summary>
        public double SecondStep { get; set; }
        #endregion

        /// <summary>
        /// 构建
        /// </summary>
        /// <param name="material"></param>
        /// <returns></returns>
        public override Color Build(double material)
        {
            if (material < FistStep) {
                return ColorBuilderA.Build(material);
            }
            else if (material < SecondStep)
            {
                return ColorBuilderB.Build(material);
            }
            return ColorBuilderC.Build(material); 
        }
    }

    /// <summary>
    /// 增加中间渐变颜色
    /// </summary>
    public class TwoStepColorBuilder : AbstractBuilder<Color, double>, INumeralColorBuilder
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="Span"></param>
        /// <param name="ColorFrom"></param>
        /// <param name="ColorTo"></param>
        public TwoStepColorBuilder(NumerialSegment Span, Color ColorFrom, Color ColorMiddle, Color ColorTo)
        {
            MidleValue = Span.Median;
            var segA = new NumerialSegment(Span.Start, MidleValue);
            var segB = new NumerialSegment(MidleValue, Span.End);
            ColorBuilderA = new ColorBuilder(segA, ColorFrom, ColorMiddle);
            ColorBuilderB = new ColorBuilder(segB, ColorMiddle, ColorTo);
        }

        #region 属性
        /// <summary>
        /// 第一段颜色生成器
        /// </summary>
        public ColorBuilder ColorBuilderA { get; set; }
        /// <summary>
        /// 第二段颜色生成器
        /// </summary>
        public ColorBuilder ColorBuilderB { get; set; }
        /// <summary>
        /// 中间值
        /// </summary>
        public double MidleValue { get; set; }
        #endregion

        /// <summary>
        /// 生成
        /// </summary>
        /// <param name="material"></param>
        /// <returns></returns>
        public override Color Build(double material)
        {
            if (material > MidleValue) {
                return ColorBuilderB.Build(material); 
            }
            return ColorBuilderA.Build(material);
        }
    }

    /// <summary>
    /// 颜色构建器.一个范围，替换为int32.
    /// </summary>
    public class ColorBuilder : AbstractBuilder<Color, double>, INumeralColorBuilder
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="Span"></param>
        /// <param name="ColorFrom"></param>
        /// <param name="ColorTo"></param>
        public ColorBuilder(NumerialSegment Span, Color ColorFrom, Color ColorTo)
        {
            this.ColorFrom = ColorFrom;
            this.ColorTo = ColorTo;
            int argbFrom = ColorFrom.ToArgb();
            int argbTo = ColorTo.ToArgb(); 

            this.Span = Span;
            this.Alfa = 255;
            this.RedFrom = argbFrom >> 16 & 0XFF;
            this.GreenFrom = argbFrom >> 8 & 0XFF;
            this.BlueFrom = argbFrom & 0XFF;

            int redTo = argbTo >> 16 & 0XFF;
            int greenTo = argbTo >> 8 & 0XFF;
            int blueTo = argbTo & 0XFF;

            RedIngredient = (redTo - RedFrom) / this.Span.Span;
            BlueIngredient = (blueTo - BlueFrom) / this.Span.Span;
            GreenIngredient = (greenTo - GreenFrom) / this.Span.Span;
        }

        #region 属性
        /// <summary>
        /// 透明值 1-255
        /// </summary>
        public int Alfa { get; set; }
        /// <summary>
        /// 红色起始
        /// </summary>
        public int RedFrom { get; set; }
        /// <summary>
        /// 绿色起始
        /// </summary>
        public int GreenFrom { get; set; }
        /// <summary>
        /// 绿色起始
        /// </summary>
        public int BlueFrom { get; set; }
        /// <summary>
        /// 外部数据范围。
        /// </summary>
        public NumerialSegment Span { get; set; }
        /// <summary>
        /// 起始颜色
        /// </summary>
        public Color ColorFrom { get; set; }
        /// <summary>
        /// 目标颜色
        /// </summary>
        public Color ColorTo { get; set; }
        /// <summary>
        /// 红色增量
        /// </summary>
        public double RedIngredient { get; set; }
        /// <summary>
        /// 蓝色增量
        /// </summary>
        public double BlueIngredient { get; set; }
        /// <summary>
        /// 绿色增量
        /// </summary>
        public double GreenIngredient { get; set; }
        #endregion

        /// <summary>
        /// 生成
        /// </summary>
        /// <param name="val"></param>
        /// <returns></returns>
        public override Color Build(double val)
        {
            val = val - Span.Start;

            int red = Check0To255((int)Math.Round((val * RedIngredient) + RedFrom));
            int green = Check0To255((int)Math.Round((val * GreenIngredient) + GreenFrom));
            int blue = Check0To255((int)Math.Round((val * BlueIngredient) + BlueFrom));
            return Color.FromArgb(Alfa, red, green, blue);
        }

        int Check0To255(int colorNum)
        {
            if (colorNum < 0) { 
                return 0;
            }
            if (colorNum > 255) { 
                return 255; 
            }
            return colorNum;
        }

    }
}