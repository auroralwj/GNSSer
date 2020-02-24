//2015.10.20, czs, create in hongqing, 对象编辑器
//2015.10.23, czs, edit in hongqign, 提取非泛型IEntityEditForm
using System;

namespace Geo.Winform
{    /// <summary>
    /// 对象编辑器。
    /// </summary> 
    public interface IEntityEditForm
    {
        /// <summary>
        /// 对象到UI
        /// </summary>
        void EntityToUi();
        /// <summary>
        /// UI到对象
        /// </summary>
        void UiToEntity();
    }
    /// <summary>
    /// 对象编辑器。
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface IEntityEditForm<T> : IEntityEditForm
    {
        /// <summary>
        /// 对象
        /// </summary>
        T Entity { get; set; } 
    }
}
