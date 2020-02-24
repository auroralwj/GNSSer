
//2017.03.16, czs, create in hongqing, 界面数据


using System;

namespace Geo.Winform.Controls
{

    /// <summary>
    /// 命名的数据
    /// </summary>
    /// <typeparam name="TValue"></typeparam>
    public interface IEnableNamedValue<TValue> : INamedValue<TValue>
    {
        bool Enabled { get; set; }
    }



    /// <summary>
    /// 命名的数据
    /// </summary>
    /// <typeparam name="TValue"></typeparam>
    public interface INamedValue<TValue>
    {
        /// <summary>
        /// 获取
        /// </summary>
        /// <returns></returns>
        TValue GetValue();
        void Init(string title, TValue val = default(TValue));
        /// <summary>
        /// 设置
        /// </summary>
        /// <param name="val"></param>
        void SetValue(TValue val);
        /// <summary>
        /// 名称
        /// </summary>
        string Title { get; set; }
        /// <summary>
        /// 数值
        /// </summary>
        TValue Value { get; set; }
    }
    /// <summary>
    /// 浮点数
    /// </summary>
    public interface INamedFloat : INamedValue<double>
    {
    }
    /// <summary>
    /// 整数
    /// </summary>
    public interface INamedInt : INamedValue<int>
    {
    }
}
