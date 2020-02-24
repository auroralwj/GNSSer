//2015.09.26, czs, create in xi'an hongqing, API开始了! 下载文件。
//2016.11.28, czs, edit in hongqing, 自适应可以下载目录

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
    /// FTP 文件下载器
    /// </summary>
    public class FtpDownload : ParamBasedOperation<FtpParam>
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public FtpDownload()
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
                    this.StatedMessage.Message = "正在下载 " + item.InputPath + " 到 " + item.OutputPath;
                    this.OnStatedMessageProduced();
                     
                    //执行下载操作
                    Geo.Utils.NetUtil.DownloadFtpDirecotryOrFile(item.InputPath, item.Extension, item.OutputPath, item.UserName, item.Password, item.IsOverwrite);
                }
                catch (Exception ex)
                { 
                    this.StatedMessage = StatedMessage.Faild;
                    this.StatedMessage.Message = "下载 " + item.InputPath + " 出错 " + ex.Message;
                    this.OnStatedMessageProduced();
                }
            }
            return true;
        }
    }
}
