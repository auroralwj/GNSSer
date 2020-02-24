//2015.09.30, czs, create in K385宝鸡到成都列车上, 删除

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
    public class Delete : ParamBasedOperation<InputParam>
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public Delete()
        {
        }

      
        /// <summary>
        /// 执行
        /// </summary>
        /// <returns></returns>
        public override bool Do()
        {
            var reader = new InputParamReader(this.OperationInfo.ParamFilePath);
            foreach (var item in reader)
            {
                this.CurrentParam = item;
                Geo.Utils.FileUtil.TryDeleteFileOrDirectory(item.InputPath);

                this.StatedMessage = StatedMessage.Processing;
                this.StatedMessage.Message = "删除 " + item.InputPath ;
                this.OnStatedMessageProduced();
            }
            return true;
        }
    }
}
