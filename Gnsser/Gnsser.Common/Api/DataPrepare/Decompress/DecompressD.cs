//2015.10.01, czs, create in K385宝鸡到成都列车上, 解压

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
    public class DecompressD : ParamBasedOperation<DecompressParam>
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public DecompressD()
        {
        }

      
        /// <summary>
        /// 执行
        /// </summary>
        /// <returns></returns>
        public override bool Do()
        {
            var reader = new DecompressParamReader(this.OperationInfo.ParamFilePath);
            foreach (var item in reader)
            {
                this.CurrentParam = item;
                Geo.Utils.DecompressRinexer.DecompressD(item.InputPath, item.OutputPath, item.IsDeleteSource, item.IsOverwrite);


                this.StatedMessage = StatedMessage.Processing;
                this.StatedMessage.Message = "解压 " + item.InputPath + " 到 " + item.OutputPath;
                this.OnStatedMessageProduced();
            }
            return true;
        }
    }
}
