using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Net;
using System.Net.NetworkInformation;
using System.Runtime.InteropServices;
using System.Text.RegularExpressions;


namespace Geo.WinTools.Net
{
    /// <summary>
    /// ��ȡIP
    /// </summary>
    public partial class GetIpForm1 : Form
    {
        /// <summary>
        /// ���캯��
        /// </summary>
        public GetIpForm1()
        {
            InitializeComponent();
        }

        private void buttonOk_Click(object sender, EventArgs e)
        {
            try
            {
                string hostNmeOrAddress = this.textBox_hostNameOrAddress.Text;
                IPAddress[] ipas = Dns.GetHostAddresses(hostNmeOrAddress);

                string ips = "";
                foreach (IPAddress ip in ipas)
                {
                    ips += ip.ToString();
                }
                this.textBoxOutput.Text = ips;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "��ʾ", MessageBoxButtons.OK, MessageBoxIcon.Information);
                 
            }

        }
    }
}