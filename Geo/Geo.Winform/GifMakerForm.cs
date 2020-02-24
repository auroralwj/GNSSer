using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using Geo.Utils;
using Geo.IO;
using System.Drawing.Imaging;

namespace Geo
{
    public partial class GifMakerForm : Form
    {
        Log log = new Log(typeof(GifMakerForm));

        public GifMakerForm()
        {
            InitializeComponent();

            this.fileOpenControl_input.Filter = Geo.Setting.ImageFilter;
        }

        private void button_run_Click(object sender, EventArgs e0)
        {
            button_run.Enabled = false;
            var start = DateTime.Now;

            var pngfiles = this.fileOpenControl_input.FilePathes;
            var giffile = this.fileOutputControl_outGif.FilePath;

            bool isRepeat = this.checkBox_repeat.Checked;
            var interval = namedIntControl_delayMs.Value;

            AnimatedGifEncoder e = new AnimatedGifEncoder();
            e.Start(giffile);

            //每帧播放时间
            e.SetDelay(interval);

            //-1：不重复，0：重复
            e.SetRepeat(isRepeat ? 0 : -1);
            foreach (var path in pngfiles)
            {
                e.AddFrame(Image.FromFile(path));
            }

            e.Finish();
            var span = DateTime.Now - start;
            log.Info("处理完毕，耗时 ：" + span.TotalSeconds .ToString("0.00")+ " 秒 = " + span.ToString());

            Geo.Utils.FormUtil.ShowOkAndOpenDirectory(giffile);

            button_run.Enabled = true;
        }


        /// <summary>
        /// 把Gif文件转成Png文件，放在directory目录下
        /// </summary>
        /// <param name="file"></param>
        /// <param name="directory"></param>
        /// <returns></returns>
        public static void GifToPngs(string giffile, string directory)
        {
            GifDecoder gifDecoder = new GifDecoder();
            directory += "\\";
            if (!Directory.Exists(directory))
            {
                Directory.CreateDirectory(directory);
            }
            //读取
            gifDecoder.Read(giffile);
            for (int i = 0, count = gifDecoder.GetFrameCount(); i < count; i++)
            {
                Image frame = gifDecoder.GetFrame(i);  // frame i
                frame.Save(directory + "\\" + i.ToString("d2") + ".png", ImageFormat.Png);
                //转成jpg
                //frame.Save(directory + "\\" + i.ToString("d2") + ".jpg", ImageFormat.Jpeg);
            }
        }
 

//2、把多张Png文件转成Gif文件

        /// <summary>
        /// 把directory文件夹里的png文件生成为gif文件，放在giffile
        /// </summary>
        /// <param name="directory">png文件夹</param>
        /// <param name="giffile">gif保存路径</param>
        /// <param name="time">每帧的时间/ms</param>
        /// <param name="repeat">是否重复</param>
        public static void PngsToGif(string directory, string giffile, int time, bool repeat)
        {
            //一般文件名按顺序排
            string[] pngfiles = Directory.GetFileSystemEntries(directory, "*.png");

            AnimatedGifEncoder e = new AnimatedGifEncoder();
            e.Start(giffile);

            //每帧播放时间
            e.SetDelay(500);

            //-1：不重复，0：重复
            e.SetRepeat(repeat ? 0 : -1);
            for (int i = 0, count = pngfiles.Length; i < count; i++)
            {
                e.AddFrame(Image.FromFile(pngfiles[i]));
            }
            e.Finish();
        }

        private void fileOpenControl1_FilePathSetted(object sender, EventArgs e)
        {
            if(String.IsNullOrWhiteSpace(fileOutputControl_outGif.FilePath))
            {
                var name = Path.GetFileName(fileOpenControl_input.FirstDirectory);
                var output = Path.Combine(fileOpenControl_input.FirstDirectory, name + ".gif");

                this.fileOutputControl_outGif.FilePath = output;
            }
        }

        private void GifMakerForm_Load(object sender, EventArgs e)
        {

        }
    }
}
