//2015.10.09, czs, create in  xi'an hongqing, 周跳探测
//2015.10.09, czs, refactor in 彭州, 重构为通用文件输入输出执行类
//2015.10.28, czs, create in hongqing, 增加具有版本参数的操作

using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Geo.IO;
using Geo;

namespace Geo.IO
{
    /// <summary>
    /// 指定了版本的执行操作
    /// </summary>
    public abstract class AbstractVersionedIoOperation : AbstractVersionedIoOperation<VersionedIoParam>
    {
        ///// <summary>
        ///// 获取读取器
        ///// </summary>
        ///// <returns></returns>
        //protected override LineFileReader<VersionedIoParam> GetParamFileReader()
        //{
        //    return new LineFileReader<VersionedIoParam>(this.OperationInfo.ParamFilePath);
        //}
    }
    /// <summary>
    /// 具有版本参数的操作
    /// </summary>
    public abstract class AbstractVersionedIoOperation<TParam> : AbstractIoOperation<TParam>
        where TParam : VersionedIoParam
    {
        //protected override LineFileReader<TParam> GetParamFileReader()
        //{
        //    return base.GetParamFileReader();
        //}

        ///// <summary>
        ///// 执行
        ///// </summary>
        ///// <returns></returns>
        //public override bool Do()
        //{
        //    var reader = GetParamFileReader();// new VersionedIoParamReader(this.OperationInfo.ParamFilePath);
        //    foreach (var key in reader)
        //    {
        //        CurrentParam = (TParam)key;

        //        var inPath = key.InputPath;
        //        var outPath = key.OutputPath;

        //        var files = Geo.Utils.FileUtil.GetFiles(inPath, FileExtension);
        //        foreach (var file in files)
        //        {
        //            var outFile = BuildOutputFilePath(outPath, file);
        //            CheckOrExecute(file, outFile, key.IsOverwrite);
        //        }

        //        var Message = "已执行 " + key.InputPath + " 到 " + key.OutputPath;
        //        this.OnStatedMessageProduced(StatedMessage.GetProcessed(Message));
        //    }
        //    return true;
        //}
    }
}