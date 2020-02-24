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
    public class Move : ParamBasedOperation<IoParam>
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public Move()
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
                        this.StatedMessage = StatedMessage.Processing;
                        this.StatedMessage.Message = "文件(夹)已存在，即将覆盖 " + item.OutputPath;
                        this.OnStatedMessageProduced();
                    }
                    else
                    {
                        this.StatedMessage = StatedMessage.Processing;
                        this.StatedMessage.Message = "文件(夹)已存在，移动操作取消 " + item.OutputPath;
                        this.OnStatedMessageProduced();
                    }
                }

                if (Geo.Utils.FileUtil.MoveFileOrDirectory(item.InputPath, item.OutputPath, item.IsOverwrite))
                {
                    this.StatedMessage = StatedMessage.Processing;
                    this.StatedMessage.Message = "已移动 " + item.InputPath + " 到 " + item.OutputPath;
                    this.OnStatedMessageProduced(); 
                }

            }
            return true;
        }
    }
}
