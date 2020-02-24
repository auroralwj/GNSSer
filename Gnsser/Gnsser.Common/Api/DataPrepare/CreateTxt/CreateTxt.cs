//2015.10.07, czs, create in 安康到西安临客K8182, 文本文件生成器

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
    /// 删除
    /// </summary>
    public class CreateTxt : ParamBasedOperation<CreateTxtParam>
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public CreateTxt()
        {
        }

      
        /// <summary>
        /// 执行
        /// </summary>
        /// <returns></returns>
        public override bool Do()
        {
            var reader = new CreateTxtParamReader(this.OperationInfo.ParamFilePath);
            foreach (var item in reader)
            {
                this.CurrentParam = item;
                var saveDir = Path.GetDirectoryName(item.OutputPath); 
                Geo.Utils.FileUtil.CheckOrCreateDirectory(saveDir);

                if (File.Exists(item.OutputPath))
                { 
                    this.StatedMessage = StatedMessage.Processing;
                    this.StatedMessage.Message = "文件已存在，即将覆盖 " + item.OutputPath;
                    this.OnStatedMessageProduced();
                }

                File.WriteAllText(item.OutputPath, item.Content,Encoding.UTF8);


                this.StatedMessage = StatedMessage.Processing;
                this.StatedMessage.Message = "已创建文本到 " + item.OutputPath;
                this.OnStatedMessageProduced();
            }
            return true;
        }
    }
}
