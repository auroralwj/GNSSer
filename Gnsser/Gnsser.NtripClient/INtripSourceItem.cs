using System;
namespace Gnsser.Ntrip
{
    public interface INtripSourceItem
    {
        /// <summary>
        /// 类型
        /// </summary>
        SourceType SourceType { get; }
    }
}
