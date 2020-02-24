using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Windows.Forms;
using System.Drawing;
using System.Drawing.Imaging;

using Microsoft.Win32;

namespace Geo.Utils
{
    /// <summary>
    /// ͼ����
    /// </summary>
    public class ImageUtil
    {

        // /// <summary>
        // /// //���������ʱ�ļ����µ��ļ�
        // /// </summary>
        //public static void TryClearTempDir()
        // {
        //     try
        //     {
        //         if (Directory.Exists(Winform.Setting.AppTempDataDir))
        //         {
        //             Directory.Delete(Winform.Setting.AppTempDataDir, true);
        //             Directory.CreateDirectory(Winform.Setting.AppTempDataDir);
        //         }
        //     }
        //     catch { }
        // }   
        /// <summary>
        /// ��ȡ����ͼƬ����������ָ���Ĵ�С��
        /// </summary>
        /// <returns></returns>
        public static Image GetDiskSmallPhoto()
        {
            Image image = GetDiskImage();
            if (image == null) return null;

            int maxSide = 150;
            int oldMaxSide = Math.Max(image.Size.Height, image.Size.Width);

            double zoom = oldMaxSide * 1.0 / maxSide;
            int newWidth = (int)(image.Size.Width / zoom);
            int newHeight = (int)(image.Size.Height / zoom);

            return new Bitmap(image, newWidth, newHeight);
        }

        /// <summary>
        ///  To show the photo in a propert aboutSize.
        /// </summary>
        /// <param name="bytes"></param>
        /// <returns></returns>
        public static Image GetSmallPhoto(Byte[] bytes)
        {
            if (bytes == null) return null;

            MemoryStream stream = new MemoryStream(bytes);
            Bitmap bitMap = new Bitmap(stream);
            Image image = bitMap.GetThumbnailImage(120, 150, null, IntPtr.Zero);
            return image;
        }

        /// <summary>
        /// ��ȡͼƬ�Ķ��������ݡ�
        /// </summary>
        /// <param name="image"></param>
        /// <returns></returns>
        public static byte[] GetImageBytes(Image image)
        {
            byte[] photo = null;
            if (image != null)
            {
                MemoryStream stream = new MemoryStream();
                image.Save(stream, System.Drawing.Imaging.ImageFormat.Jpeg);
                stream.Position = 0;
                photo = new byte[stream.Length];
                stream.Read(photo, 0, photo.Length);
                stream.Flush();
                stream.Close();
            }
            return photo;
        }
        /**
         * Open a file dilag and select a photo.
         */
        public static Image GetDiskImage()
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Multiselect = false;
            openFileDialog.Title = "��ѡ��ͼƬ";
            openFileDialog.Filter = "ͼƬ�ļ�(*.jpg)|*.jpg|png��ʽ(*.png)|*.png|gif��ʽ(*.gif)|*.gif|�����ļ�(*.*)|*.*";
            openFileDialog.FileName = "";

            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    Stream stream = openFileDialog.OpenFile();
                    Image showImage = new Bitmap(stream);
                    return showImage;
                }
                catch (Exception ex)
                {
                    MessageBox.Show("��ȡͼƬ����" + ex.Message);
                }
            }
            return null;
        }

        /// <summary>
        /// get the default photo bytes
        /// </summary>
        /// <returns></returns>
        //public static byte[] GetDefaultPhotoBytes()
        //{
        //    //picture.
        //    //  string picPath = @"C:\Documents and Settings\All Users\Documents\My Pictures\ʾ��ͼƬ\Sunset.jpg";
        //    string picPath = ImageManager.DefaultPhotoPath;

        //    //save to bytes;
        //    Stream stream = null;
        //    try
        //    {
        //        stream = new FileStream(picPath, FileMode.Open);
        //    }
        //    catch (Exception ex)
        //    {
        //        MessageBox.Show("Ĭ��ͷ��װ�ش���" + ex.Message);
        //    }
        //    return StreamToBytes(stream);
        //}

        /// <summary>
        /// stream to bytes.
        /// </summary>
        /// <param name="stream"></param>
        /// <returns></returns>
        public static byte[] StreamToBytes(Stream stream)
        {
            if (stream == null) return null;

            int length = (int)stream.Length;
            byte[] bytes = new byte[length];
            stream.Position = 0;
            stream.Read(bytes, 0, length);
            stream.Close();
            return bytes;
        }


        /**
         * turn image into bytes.
         */
        public static byte[] ImageToBytes(Image image)
        {
            MemoryStream stream = new MemoryStream();
            image.Save(stream, ImageFormat.Jpeg);
            stream.Position = 0;
            int length = (int)stream.Length;
            byte[] imageBytes = new byte[length];
            stream.Read(imageBytes, 0, length);
            stream.Close();

            return imageBytes;
        }

        /**
         * To show the photo in a propert aboutSize.
         */
        public static Image GetSmallPhoto(Byte[] bytes, int width, int height)
        {
            if (bytes == null) return null;

            MemoryStream stream = new MemoryStream(bytes);
            Bitmap bitMap = new Bitmap(stream);
            Image image = bitMap.GetThumbnailImage(width, height, null, IntPtr.Zero);
            //new Bitmap(image, new Size(width, height));
            return image;
        }

        public static byte[] GetFileBytes(string filePath)
        {
            return File.ReadAllBytes(filePath);
        }

        public static Stream BytesToStream(Byte[] bytes)
        {
            return new MemoryStream(bytes);
        }

        /// <summary>
        /// ������������ת��ΪͼƬ��
        /// </summary>
        /// <param name="bytes"></param>
        /// <returns></returns>
        public static Image BytesToImage(byte[] bytes)
        {
            Bitmap image = null;
            try
            {
                MemoryStream stream = new MemoryStream(bytes);
                image = new Bitmap(stream);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ĭ��ͷ��װ�ش���" + ex.Message);
            }
            return image;
        }
    }
}
