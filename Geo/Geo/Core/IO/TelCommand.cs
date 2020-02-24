//2014.05.20, cy, edit, 修改了头部信息


using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data; 
using System.Linq;
using System.Text;
using System.Net.Sockets;
using System.Net;
using System.IO;
using System.Threading;

namespace Geo.Common
{
    /// <summary>
    /// 远程。
    /// </summary>
    public class TelCommand
    {
        /// <summary>
        /// 二进制读取
        /// </summary>
        public BinaryReader BinaryReader { get; set; }
        /// <summary>
        /// 二进制写 
        /// </summary>
        public BinaryWriter BinaryWriter { get; set; }
        /// <summary>
        /// 客户端连接
        /// </summary>
        public TcpClient TcpClient { get; set; }
        /// <summary>
        /// 命令字符串
        /// </summary>
        public string CmdStr { get; set; }
         /// <summary>
         /// 本地入口
         /// </summary>
        public string LocalEndPoint { get; set; }
        /// <summary>
        /// 构造器
        /// </summary>
        /// <param name="client"></param>
        public TelCommand(TcpClient client)
        {
            this.TcpClient = client;
            this.LocalEndPoint = this.TcpClient.Client.LocalEndPoint.ToString();

            NetworkStream stream = client.GetStream();
            this.BinaryReader = new BinaryReader(stream);
            this.BinaryWriter = new BinaryWriter(stream);

            this.CmdStr = BinaryReader.ReadString();

            this.BinaryWriter.Write("OK");
            this.BinaryWriter.Flush();
            //BinaryWriter.Close();
        }
        /// <summary>
        /// 关闭资源。
        /// </summary>
        public void Close()
        {
            this.BinaryReader.Close();
            this.BinaryWriter.Close();
            this.TcpClient.Close();
        }
    }
}
