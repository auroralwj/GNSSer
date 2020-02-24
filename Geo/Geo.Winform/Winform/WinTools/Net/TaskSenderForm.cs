using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Net.Sockets;
using System.Net;
using System.IO;
using System.Threading;
using System.Windows.Forms;

namespace Geo.WinTools
{
    /// <summary>
    /// 任务发送
    /// </summary>
    public partial class TaskSenderForm : Form
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public TaskSenderForm()
        {
            InitializeComponent();
        }

        BinaryReader br;
        BinaryWriter bw;
        TcpClient client;

        private void button_send_Click(object sender, EventArgs e)
        {
            string ip = this.textBox_ip.Text;
            int port = int.Parse( this.textBox_port.Text);
            string cmd = this.textBox_cmd.Text;

            try
            {
                client = new TcpClient(ip, port);
                NetworkStream stream = client.GetStream();
                br = new BinaryReader(stream);
                bw = new BinaryWriter(stream);


                bw.Write(cmd);

                bw.Flush();
                //BinaryWriter.Close();

                string str = br.ReadString();

                MessageBox.Show(str);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void button_cancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
