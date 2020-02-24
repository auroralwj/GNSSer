using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Drawing.Imaging;
using System.Threading;
using System.Runtime.InteropServices;

namespace Geo.WinTools.Images
{
    /// <summary>
    /// 截图小工具
    /// </summary>
    public partial class ScreenImageForm1 : Form
    {

        CopyScreen screen;
        /// <summary>
        /// 构造函数
        /// </summary>
        public ScreenImageForm1()
        {
            InitializeComponent();
            screen = new CopyScreen();
            screen.GetScreenImage  += new  CopyScreen.GetImage(s_GetScreenImage);

        }
        private void button_GetScreenPic_Click(object sender, EventArgs e)
        {

            //获得当前屏幕大小
            Rectangle rect = new Rectangle();
            rect = Screen.GetWorkingArea(this);
            //创建一个以当前屏幕为模板的图像。
            Graphics g1 = this.CreateGraphics();
            Bitmap myImage = new Bitmap(rect.Width, rect.Height);
            Graphics g2 = Graphics.FromImage(myImage);
            IntPtr dc1 = g1.GetHdc();
            IntPtr dc2 = g2.GetHdc();

            BitBlt(dc2, 0, 0, rect.Width, rect.Height, dc1, 0, 0, 13369376);
            g1.ReleaseHdc(dc1);
            g2.ReleaseHdc(dc2);
            string path = this.textBox_dir.Text + "\\" +this.textBox_name.Text + "." + this.comboBox1.Text;
            myImage.Save(path, GetImageFormat());


            this.WindowState = FormWindowState.Normal;
            MessageBox.Show(@"截取图像保存在：" + path);

        }

        //声明一个Windows API 函数。
        [System.Runtime.InteropServices.DllImportAttribute("gdi32.dll")]
        private static extern bool BitBlt(
            IntPtr hdcDest,
            int nXDest,
            int nYDest,
            int nWidth,
            int nHeight,
            IntPtr hdcSrc,
            int nXSrc,
            int nYSrc,
            System.Int32 dwRop//光栅处理数值,负责颜色等等？
            );

        private void button_SetFolder_Click(object sender, EventArgs e)
        {
            if (this.folderBrowserDialog1.ShowDialog() == DialogResult.OK)
            {
                this.textBox_dir.Text = folderBrowserDialog1.SelectedPath;
            }
        }

        private void comboBox1_KeyDown(object sender, KeyEventArgs e)
        {
            e.Handled = true;
        }

        private ImageFormat GetImageFormat()
        {
            switch (this.comboBox1.Text)
            {
                case "JPG": return ImageFormat.Jpeg; 
                case "PNG": return ImageFormat.Png; 
                case "GIF": return ImageFormat.Gif; 
                case "ICO": return ImageFormat.Icon; 
                case "BMP": return ImageFormat.Bmp; 
                default : return ImageFormat.Jpeg;
            }
        }

        private void button_GetScreen_Click(object sender, EventArgs e)
        {

           this.WindowState = FormWindowState.Minimized;
           //this.Visible = false;
           this.Refresh(); 
           
            //获得当前屏幕大小
            Rectangle rect = new Rectangle();
            rect = Screen.GetBounds(this);
            Bitmap myImage = new Bitmap(rect.Width, rect.Height);
            //创建一个以当前屏幕为模板的图像。 
            Graphics g2 = Graphics.FromImage(myImage);

            g2.CopyFromScreen(new Point(0, 0), new Point(0, 0), rect.Size);
            
            this.Visible = true;
            this.WindowState = FormWindowState.Normal;
            this.Update();

            if (checkBox_save.Checked)
            {
                string path = this.textBox_dir.Text + "\\" + this.textBox_name.Text + "." + this.comboBox1.Text;
                myImage.Save(path, GetImageFormat());
                MessageBox.Show(@"截取图像保存在：" + path);
            }

            Clipboard.SetImage(myImage);
            if(!checkBox_hideMessageBox.Checked)
                MessageBox.Show(@"截取图像已复制到粘贴板");
        }
        private void button_mouse_Click(object sender, EventArgs e)
        {
           
            screen.GerScreenFormRectangle();


          //  MessageBox.Show("还没实现！");

           // this.WindowState = FormWindowState.Minimized;
           // this.Refresh();

           // this.MouseUp +=new MouseEventHandler(ScreenImageForm1_MouseUp);

           //Cursor cursor =  Cursor.Current;

           //this.Visible = true;
           // this.WindowState = FormWindowState.Normal;
           // this.Update();


        }
        private void ScreenImageForm1_MouseUp(object sender, MouseEventArgs e)
        {

           Rectangle rect =  Cursor.Clip;
           MessageBox.Show(rect.ToString());
        }
        void s_GetScreenImage(Image p_Image)
        {
            pictureBox1.Image = p_Image;
            Clipboard.SetImage(p_Image);
            if (!checkBox_hideMessageBox.Checked)
                MessageBox.Show(@"截取图像已复制到粘贴板");
        }

        //[DllImport("user32.dll", CharSet = CharSet.Auto)]
        //public static extern IntPtr GetDesktopWindow();


    }
}