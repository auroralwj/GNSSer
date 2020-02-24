using System;
using Gnsser.Service;

namespace Gnsser
{
    /// <summary>
    ///  星历以文件形式存储。 一个类的实例代表一个文件。通常只能计算文件有效范围内的新历信息。
    /// </summary>
    public interface IFileEphemerisService : IEphemerisService
    {

    }
    /// <summary>
    /// 以网络服务形式表达的星历信息。
    /// </summary>
    public interface INetEphemerisService : IEphemerisService
    {

    }


    /// <summary>
    /// 以数据库存储的星历信息。
    /// </summary>
    public interface IDatabaseEphemerisService : IEphemerisService
    {


    }
     
}
