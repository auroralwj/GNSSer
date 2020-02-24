//2015.10.07, create in xi'an, 追加参数到Gnsser参数文件

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
    /// 追加参数到Gnsser参数文件
    /// </summary>
    public class AppendStringToLine : ParamBasedOperation<AppendStringToLineParam>
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public AppendStringToLine()
        {
        }

      
        /// <summary>
        /// 执行
        /// </summary>
        /// <returns></returns>
        public override bool Do()
        {
            var reader = new AppendStringToLineParamReader(this.OperationInfo.ParamFilePath);
            throw new NotImplementedException();

            foreach (var item in reader)
            {
                this.CurrentParam = item;
               // Geo.Utils.FileUtil.AppendStringToLineFileOrDirectory(key.FileToAppdend, key.Content, key.IsOverwrite);
                var lines = File.ReadAllLines(item.FileToAppdend);
                lines[0] = lines[0] + ", " + item.Content;
                File.WriteAllLines(item.FileToAppdend, lines);

                this.StatedMessage = StatedMessage.Processing;
                this.StatedMessage.Message = "追加参数 " + item.Content + " 到 " +item.FileToAppdend ;
                this.OnStatedMessageProduced();  
            }
            return true;
        }
    }
}
