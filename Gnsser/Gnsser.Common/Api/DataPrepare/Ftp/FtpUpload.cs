//2016.11.28, czs, create in hongqing, FTP 文件上传

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
    ///  FTP 文件上传
    /// </summary>
    public class FtpUpload : ParamBasedOperation<FtpParam>
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public FtpUpload()
        {
        }

      
        /// <summary>
        /// 执行
        /// </summary>
        /// <returns></returns>
        public override bool Do()
        {
            var reader = new FtpParamReader(this.OperationInfo.ParamFilePath);
            foreach (var item in reader)
            {
                this.CurrentParam = item;
                try
                {
                    this.StatedMessage = StatedMessage.Processing;
                    this.StatedMessage.Message = "正在上传 " + item.InputPath + " 到 " + item.OutputPath;
                    this.OnStatedMessageProduced();
                     
                    //执行下载操作
                    Geo.Utils.NetUtil.FtpUpload(item.InputPath, item.OutputPath, item.UserName, item.Password);
                }
                catch (Exception ex)
                { 
                    this.StatedMessage = StatedMessage.Faild;
                    this.StatedMessage.Message = "上传 " + item.InputPath + " 出错 " + ex.Message;
                    this.OnStatedMessageProduced();
                }
            }
            return true;
        }
    }
}
