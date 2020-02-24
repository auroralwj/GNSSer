//2018.11.12, czs, edit in hmx, 更新为 1.4 版本

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Diagnostics;
using Geo.Utils;

namespace Gnsser.Winform
{
    public partial class NoticeForm : Form
    {
        public NoticeForm()
        {
            InitializeComponent();

            richTextBox_info.Clear();
            RichTextBoxUtil.AppendFormatedLine(richTextBox_info,
                "欢迎使用 GNSS 数据并行处理软件 —— GNSSer（GNSS Data Paralell Processer） v 1.4\r\n", Color.FromArgb(100,100,250),  3);

            StringBuilder sb = new StringBuilder();   

            int i = 1;
            sb.Clear();
            RichTextBoxUtil.AppendFormatedLine(richTextBox_info, "简介：", Color.Black, 2);
            sb.Append("  " + (i++) + ". ");
            sb.AppendLine("GNSSer 是一个中国国产科研型GNSS数据处理软件，系统从底层开发，具有完全自主的知识产权和多项首创技术，我们秉承“技术至上”的理念，不断探索、专研和发展；");
            sb.Append("  " + (i++) + ". ");
            sb.AppendLine("GNSSer 致力于 全球导航卫星系统（GNSS） 高精度数据处理算法与应用研究；");
            sb.Append("  " + (i++) + ". ");
            sb.AppendLine("GNSSer 的目标是 提供高精度、高效率、高度自动化、界面友好、性能稳定的 GNSS 数据处理解决方案，并提供云模式的 GNSS 计算服务；");
            sb.Append("  " + (i++) + ". ");
            sb.AppendLine("GNSSer 的一个特点是大规模 GNSS 数据的分布式计算和并行算法。");
            sb.AppendLine();
            richTextBox_info.AppendText(sb.ToString());

            i = 1;
            sb.Clear();
            RichTextBoxUtil.AppendFormatedLine(richTextBox_info, "使用须知：", Color.Black, 2);
            sb.Append("  " + (i++) + ". ");
            sb.AppendLine("GNSSer 桌面公开版 提供免费的使用，任何人、任何组织、任何党派、都可以在非商业模式下，自由 使用、复制、拷贝 本版本软件；");
            sb.Append("  " + (i++) + ". ");
            sb.AppendLine("未经许可，不可以将 GNSSer 的代码、组件、安装包，进行破解、反编译、嵌入系统、重新打包等" +
                "，GNSSer 保留版权和法律追究的权利；");
            sb.Append("  " + (i++) + ". ");
            sb.AppendLine("GNSSer 欢迎以本软件为基础发表论文和著述，请引用我们公开发表的论文，论文目录将更新到网站上；");
            sb.Append("  " + (i++) + ". ");
            sb.AppendLine("GNSSer 欢迎任何形式的合作、开发与应用；");
            sb.AppendLine();
            richTextBox_info.AppendText(sb.ToString());
                         
            sb.Clear();
            RichTextBoxUtil.AppendFormatedLine(richTextBox_info, "GNSSer 1.x 标签：", Color.Black, 2); 
            sb.AppendLine("自主研发，高精度，多系统，并行计算，AnyInfo，界面友好");
            sb.AppendLine();
            richTextBox_info.AppendText(sb.ToString());

            i = 1;
            sb.Clear();
            RichTextBoxUtil.AppendFormatedLine(richTextBox_info, "注意：", Color.Black, 2);
            sb.Append("  " + (i++) + ". ");
            sb.AppendLine("GNSSer 仍然在不断发展中，后继版本的变化可能会很大！");
            sb.Append("  " + (i++) + ". ");
            sb.AppendLine("如果在软件使用过程中，出现了任何异常、错误、Bug等，请将状况反映到我们邮箱或网站；");
            sb.AppendLine();
            richTextBox_info.AppendText(sb.ToString());

            sb.Clear();
            RichTextBoxUtil.AppendFormatedLine(richTextBox_info, "致谢：", Color.Black, 2); 
            sb.AppendLine("GNSSer 的研发中，参考比较了大量论文、书籍和已有成熟软件的算法和数据格式，以及对结果进行了比较，在此表示感谢，包括 Bernese、GAMIT、GPSTk、RTKLIB等；");

            richTextBox_info.AppendText(sb.ToString());


            sb.Clear();
            sb.AppendLine();
            sb.AppendLine("邮箱: gnsser@163.com");
            sb.AppendLine("网站: www.gnsser.com");
            sb.AppendLine("GNSSer 开发组");
            sb.AppendLine("2018 年 11 月 12 日 ");

            var info = sb.ToString();
            this.richTextBox_info.AppendText(info);
            this.richTextBox_info.SelectionStart = 0;
        }

        private void NoticeForm_MouseClick(object sender, MouseEventArgs e)
        {

        }

        private void richTextBox_info_LinkClicked(object sender, LinkClickedEventArgs e)
        {
            try
            {
                var url = e.LinkText;
              
                Process.Start(url);
            }
            catch (Exception ex)
            {
                MessageBox.Show("无法访问网络！" + ex.Message);
            }
        }


    }
}
