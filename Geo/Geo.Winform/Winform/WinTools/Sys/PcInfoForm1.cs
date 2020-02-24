using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace Geo.WinTools.Sys
{
    /// <summary>
    /// 当前计算机信息
    /// </summary>
    public partial class PcInfoForm1 : Form
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public PcInfoForm1()
        {
            InitializeComponent();

            this.label_computerName.Text = System.Environment.MachineName;
            this.label_osVersion.Text = System.Environment.OSVersion.VersionString;
            this.label_sysDir.Text = System.Environment.SystemDirectory;

            this.label_cpu.Text = "";
            System.Management.ManagementClass mc = new System.Management.ManagementClass("win32_processor");
            System.Management.ManagementObjectCollection moc = mc.GetInstances();
            foreach (System.Management.ManagementObject mo in moc)
            {
                this.label_cpu.Text += mo["processorid"].ToString();
            }



        }
    }
}