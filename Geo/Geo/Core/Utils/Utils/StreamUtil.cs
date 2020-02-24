using System;
using System.Collections.Generic;
using System.Text;
using System.Net;
using System.IO;
using System.Net.NetworkInformation;
using System.Runtime.InteropServices;
using System.Text.RegularExpressions;



namespace Geo.Utils
{
    /// <summary>
    /// ���繤��
    /// </summary>
    public static class StreamUtil
    {
        static Geo.IO.ILog log = Geo.IO.Log.GetLog(typeof(NetUtil));

        /// <summary>
        /// ���������浽�ļ�
        /// </summary>
        /// <param name="filePath"></param>
        /// <param name="inputStream"></param>
        /// <param name="bufferSize"></param>
        public static void StreamToLocalFile(string filePath, Stream inputStream, int bufferSize = 1024)
        {
            var outputStream = new FileStream(filePath, FileMode.Create, FileAccess.Write);

            byte[] buffer = new byte[bufferSize];
            int readedSize = 0;
            while ((readedSize = inputStream.Read(buffer, 0, bufferSize)) > 0)
            {
                outputStream.Write(buffer, 0, readedSize);
                outputStream.Flush();
            }
            outputStream.Close();
        } 

        public static bool HttpDownload(string urlpath, string savePath, bool throwException = false)
        {
            HttpWebRequest request = (HttpWebRequest)FtpWebRequest.Create(urlpath); 
            try
            {
                //���Ŀ¼�Ĵ����ԣ�������
                var saveDir = Path.GetDirectoryName(savePath); 
                Geo.Utils.FileUtil.CheckOrCreateDirectory(saveDir);

                HttpWebResponse respose = (HttpWebResponse)request.GetResponse();
                using (Stream ftpStream = respose.GetResponseStream())
                {
                    using (FileStream fileStream = new FileStream(savePath, FileMode.Create))
                    {
                        int size = 512;
                        byte[] buffer = new byte[size];
                        int count = 0;
                        while ((count = ftpStream.Read(buffer, 0, size)) > 0)
                        {
                            fileStream.Write(buffer, 0, count);
                            fileStream.Flush();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                if (throwException) { throw ex; }

                return false;
            }
            return true;
        }
         

    }
}
