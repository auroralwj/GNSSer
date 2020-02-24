using System;
namespace Gnsser.Winform
{
    /// <summary>
    /// 具有显示图层的事件。
    /// </summary>
    public interface IShowLayer
    {
        event Gnsser.ShowLayerHandler ShowLayer;
    }
}
