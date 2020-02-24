//2017.10.16, czs, create in hongqing, 提取通用接口

using System;

namespace Geo.Draw
{
    /// <summary>
    /// 数字转换为颜色。
    /// </summary>
    public interface INumeralColorBuilder : IBuilder<System.Drawing.Color, double>
    {
        
    }
}
