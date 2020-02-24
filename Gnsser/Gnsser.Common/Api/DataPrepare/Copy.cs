//2015.09.30, czs, create in K879西安到宝鸡列车上, 复制

using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Geo.IO;
using Geo;

namespace Gnsser.Api
{
    /// <summary>
    /// 复制文件
    /// </summary>
    public class Copy : ParamBasedOperation<IoParam>
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public Copy()
        {
        }

      
        /// <summary>
        /// 执行
        /// </summary>
        /// <returns></returns>
        public override bool Do()
        {
            var reader = new IoParamReader(this.OperationInfo.ParamFilePath);
            foreach (var item in reader)
            {
                this.CurrentParam = item;
                if (File.Exists(item.OutputPath))
                {
                    if (item.IsOverwrite)
                    {
                        var Message = "文件(夹)已存在，即将覆盖 " + item.OutputPath;
                        this.OnStatedMessageProduced(StatedMessage.GetProcessing(Message));
                    }
                    else
                    {
                        var Message = "文件(夹)已存在，复制操作取消 " + item.OutputPath;
                        this.OnStatedMessageProduced(StatedMessage.GetProcessing(Message));
                    }
                }

                if (Geo.Utils.FileUtil.CopyFileOrDirectory(item.InputPath, item.OutputPath, item.IsOverwrite))
                { 
                    var Message = "已复制 " + item.InputPath + " 到 " + item.OutputPath;
                    this.OnStatedMessageProduced(StatedMessage.GetProcessing(Message));
                }
            }
            return true;
        }
    }
}
