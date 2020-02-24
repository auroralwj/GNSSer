//2018.05.20, czs, create in HMX,  数据多项式平滑器

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Geo;
using Geo.IO;

namespace Gnsser
{
    /// <summary>
    /// 数据多项式平滑管理器
    /// </summary>
    public class SmoothValueBuilderManager : BaseDictionary<string, SmoothValueBuilder>
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public SmoothValueBuilderManager(int windowSize)
        {
            this.WindowSize = windowSize;
        }
        /// <summary>
        /// 最大窗口大小，单位：历元 次
        /// </summary>
        public int WindowSize { get; set; }
        /// <summary>
        /// 创建
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public override SmoothValueBuilder Create(string key)
        {
            return new SmoothValueBuilder(WindowSize, key );
        }
    }

    /// <summary>
    /// 数据多项式平滑器。
    /// </summary>
    public class SmoothValueBuilder : AbstractBuilder<double>, Geo.Namable
    {
        Log log = new Log(typeof(SmoothValueBuilder));

        /// <summary>
        ///  数据多项式平滑器
        /// </summary>
        /// <param name="maxEpochCount"></param>
        /// <param name="name"></param>
        public SmoothValueBuilder(int maxEpochCount, string name)
        { 
            this.Name = name;
            NumeralWindowData = new NumeralWindowData(maxEpochCount);
            this.Order = 2;
        }

        #region  属性

        /// <summary>
        /// 名称
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 阶次
        /// </summary>
        public int Order { get; set; }
        /// <summary>
        /// 多项式拟合伪距方法。
        /// </summary>
        private  NumeralWindowData NumeralWindowData { get; set; } 
         /// <summary>
         /// 当前原始数据
         /// </summary>
        public double CurrentRaw { get; set; }
        #endregion

        #region  设值 

        /// <summary>
        /// 设置原始原始伪距,载波伪距
        /// </summary>
        /// <param name="rawRange"></param> 
        /// <returns></returns>
        public SmoothValueBuilder SetRawValue(double rawRange)
        {
            //通过是否相等，判断是否重复设值
            if (CurrentRaw  == rawRange)
            {
                return this;
            }

            this.NumeralWindowData.Add(rawRange); 
            this.CurrentRaw = rawRange; 
            return this;
        }
        /// <summary>
        /// 是否重置，如果发生周跳。
        /// </summary>
        /// <param name="IsReset"></param>
        /// <returns></returns>
        public SmoothValueBuilder SetReset(bool IsReset)
        {
            if (IsReset)
            { 
                this.NumeralWindowData.Clear();
            };
            return this;
        }

        #endregion

        /// <summary>
        /// 采用窗口进行平滑
        /// </summary>
        /// <returns></returns>
        public double GetSmoothedRange()
        {
            if (NumeralWindowData.Count == 0)
            {
                throw new Exception("are you kidding? you must put one value first.");
            }

            if (NumeralWindowData.Count < this.Order + 1)
            {
                return this.CurrentRaw;
            }

            var val = NumeralWindowData.GetLsPolyFit(Order).GetY( this.NumeralWindowData.Count-1);

            double differ = val - CurrentRaw;

            return val;
        }

        /// <summary>
        /// 构建
        /// </summary>
        /// <returns></returns>
        public override double Build()
        { 
            return GetSmoothedRange(); 
        } 

        /// <summary>
        /// 重设。
        /// </summary>
        public void Reset()
        {
            NumeralWindowData.Clear();
        }
    }
}