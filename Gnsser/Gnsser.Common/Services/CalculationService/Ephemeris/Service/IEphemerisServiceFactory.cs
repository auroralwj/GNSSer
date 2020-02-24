using System;
namespace Gnsser
{

    /// <summary>
    /// 星历数据工厂接口
    /// </summary>
    public interface IEphemerisServiceFactory
    {
        /// <summary>
        /// 从文件获取。
        /// </summary>
        /// <param name="filePath"></param>
        /// <returns></returns>
        IFileEphemerisService CreateFromFile(string filePath);
    }
}
