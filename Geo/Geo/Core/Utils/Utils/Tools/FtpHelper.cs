//2015.11.13, czs, create in hongqing, FTP�����࣬Դ�����ϣ������޸�


using System;
using System.Collections.Generic;
using System.Text;
using System.Net;
using System.IO;
using System.Net.NetworkInformation;
using System.Runtime.InteropServices;
using System.Text.RegularExpressions;  
using System.Globalization; 
using System.Threading;



namespace Geo.Utils
{
    /// <summary>
    /// FTP �����ࡣ
    /// </summary>
     public class FtpHelper
    {

         public FtpHelper(string servername, string username = "Anonymous", string password = "Passaword")
        {
            this.path = servername;
            this.username = username;
            this.password = password;
        }


        //��������
        private string path { get; set; }    //Ŀ��·��
        private string ftpip { get; set; }     //ftp IP��ַ
        private string username { get; set; }   //ftp�û���
        private string password { get; set; }    //ftp����


        /// <summary>
        /// ��ȡftp������ļ����ļ���
        /// </summary>
        /// <param name="dir"></param>
        /// <returns></returns>
        public  string[] GetFileList(string dir)
        {
            string[] downloadFiles;
            StringBuilder result = new StringBuilder();
            FtpWebRequest request;
            try
            {
                request = (FtpWebRequest)FtpWebRequest.Create(new Uri(path));
                request.UseBinary = true;
                request.Credentials = new NetworkCredential(username, password);//�����û���������
                request.Method = WebRequestMethods.Ftp.ListDirectory;
                request.UseBinary = true;

                WebResponse response = request.GetResponse();
                StreamReader reader = new StreamReader(response.GetResponseStream());

                string line = reader.ReadLine();
                while (line != null)
                {
                    result.Append(line);
                    result.Append("\n");
                    Console.WriteLine(line);
                    line = reader.ReadLine();
                }
                // to remove the trailing '\n'
                result.Remove(result.ToString().LastIndexOf('\n'), 1);
                reader.Close();
                response.Close();
                return result.ToString().Split('\n');
            }
            catch (Exception ex)
            {
                Console.WriteLine("��ȡftp������ļ����ļ��У�" + ex.Message);
                downloadFiles = null;
                return downloadFiles;
            }
        }

        /// <summary>
        /// ��ȡ�ļ���С
        /// </summary>
        /// <param name="file">ip�������µ����·��</param>
        /// <returns>�ļ���С</returns>
        public  int GetFileSize(string file)
        {
            StringBuilder result = new StringBuilder();
            FtpWebRequest request;
            try
            {
                request = (FtpWebRequest)FtpWebRequest.Create(new Uri(path + file));
                request.UseBinary = true;
                request.Credentials = new NetworkCredential(username, password);//�����û���������
                request.Method = WebRequestMethods.Ftp.GetFileSize;

                int dataLength = (int)request.GetResponse().ContentLength;

                return dataLength;
            }
            catch (Exception ex)
            {
                Console.WriteLine("��ȡ�ļ���С����" + ex.Message);
                throw ex;
                return -1;
            }
        }

        /// <summary>
        /// �ļ��ϴ�
        /// </summary>
        /// <param name="filePath">ԭ·��������·���������ļ���</param>
        /// <param name="objPath">Ŀ���ļ��У��������µ����·�� ����Ϊ��Ŀ¼</param>
        public  void FileUpLoad(string filePath, string objPath = "")
        {
            try
            {
                string url = path;
                if (objPath != "")
                    url += objPath + "/";
                try
                {

                    FtpWebRequest reqFTP = null;
                    //���ϴ����ļ� ��ȫ·����
                    try
                    {
                        FileInfo fileInfo = new FileInfo(filePath);
                        using (FileStream fs = fileInfo.OpenRead())
                        {
                            long length = fs.Length;
                            reqFTP = (FtpWebRequest)FtpWebRequest.Create(new Uri(url + fileInfo.Name));

                            //�������ӵ�FTP���ʺ�����
                            reqFTP.Credentials = new NetworkCredential(username, password);
                            //����������ɺ��Ƿ񱣳�����
                            reqFTP.KeepAlive = false;
                            //ָ��ִ������
                            reqFTP.Method = WebRequestMethods.Ftp.UploadFile;
                            //ָ�����ݴ�������
                            reqFTP.UseBinary = true;

                            using (Stream stream = reqFTP.GetRequestStream())
                            {
                                //���û����С
                                int BufferLength = 5120;
                                byte[] b = new byte[BufferLength];
                                int i;
                                while ((i = fs.Read(b, 0, BufferLength)) > 0)
                                {
                                    stream.Write(b, 0, i);
                                }
                                Console.WriteLine("�ϴ��ļ��ɹ�");
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("�ϴ��ļ�ʧ�ܴ���Ϊ" + ex.Message);
                        throw ex;
                    }
                    finally
                    {

                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine("�ϴ��ļ�ʧ�ܴ���Ϊ" + ex.Message);
                    throw ex;
                }
                finally
                {

                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("�ϴ��ļ�ʧ�ܴ���Ϊ" + ex.Message);
                throw ex;
            }
        }

        /// <summary>
        /// ɾ���ļ�
        /// </summary>
        /// <param name="fileName">�������µ����·�� �����ļ���</param>
        public  void DeleteFileName(string fileName)
        {
            try
            {
                FileInfo fileInf = new FileInfo(ftpip + "" + fileName);
                string uri = path + fileName;
                FtpWebRequest reqFTP = (FtpWebRequest)FtpWebRequest.Create(new Uri(uri));
                // ָ�����ݴ�������
                reqFTP.UseBinary = true;
                // ftp�û���������
                reqFTP.Credentials = new NetworkCredential(username, password);
                // Ĭ��Ϊtrue�����Ӳ��ᱻ�ر�
                // ��һ������֮��ִ��
                reqFTP.KeepAlive = false;
                // ָ��ִ��ʲô����
                reqFTP.Method = WebRequestMethods.Ftp.DeleteFile;
                FtpWebResponse response = (FtpWebResponse)reqFTP.GetResponse();
                response.Close();
            }
            catch (Exception ex)
            {
                Console.WriteLine("ɾ���ļ�����" + ex.Message);
                throw ex;
            }
        }

        /// <summary>
        /// �½�Ŀ¼ ��һ�������ȴ���
        /// </summary>
        /// <param name="dirName">�������µ����·��</param>
        public  void MakeDir(string dirName)
        {
            try
            {
                string uri = path + dirName;
                FtpWebRequest reqFTP = (FtpWebRequest)FtpWebRequest.Create(new Uri(uri));
                // ָ�����ݴ�������
                reqFTP.UseBinary = true;
                // ftp�û���������
                reqFTP.Credentials = new NetworkCredential(username, password);
                reqFTP.Method = WebRequestMethods.Ftp.MakeDirectory;
                FtpWebResponse response = (FtpWebResponse)reqFTP.GetResponse();
                response.Close();
            }
            catch (Exception ex)
            {
                Console.WriteLine("����Ŀ¼����" + ex.Message);
                throw ex;
            }
        }

        /// <summary>
        /// ɾ��Ŀ¼ ��һ�������ȴ���
        /// </summary>
        /// <param name="dirName">�������µ����·��</param>
        public  void DelDir(string dirName)
        {
            try
            {
                string uri = path + dirName;
                FtpWebRequest reqFTP = (FtpWebRequest)FtpWebRequest.Create(new Uri(uri));
                // ftp�û���������
                reqFTP.Credentials = new NetworkCredential(username, password);
                reqFTP.Method = WebRequestMethods.Ftp.RemoveDirectory;
                FtpWebResponse response = (FtpWebResponse)reqFTP.GetResponse();
                response.Close();
            }
            catch (Exception ex)
            {
                Console.WriteLine("ɾ��Ŀ¼����" + ex.Message);
                throw ex;
            }
        }

        /// <summary>
        /// ��ftp�������ϻ���ļ����б�
        /// </summary>
        /// <param name="RequedstPath">�������µ����·��</param>
        /// <returns></returns>
        public  List<string> GetDirctory(string RequedstPath)
        {
            List<string> strs = new List<string>();
            try
            {
                string uri = path + RequedstPath;   //Ŀ��·�� pathΪ��������ַ
                FtpWebRequest reqFTP = (FtpWebRequest)FtpWebRequest.Create(new Uri(uri));
                // ftp�û���������
                reqFTP.Credentials = new NetworkCredential(username, password);
                reqFTP.Method = WebRequestMethods.Ftp.ListDirectoryDetails;
                WebResponse response = reqFTP.GetResponse();
                StreamReader reader = new StreamReader(response.GetResponseStream());//�����ļ���

                string line = reader.ReadLine();
                while (line != null)
                {
                    if (line.Contains("<DIR>"))
                    {
                        string msg = line.Substring(line.LastIndexOf("<DIR>") + 5).Trim();
                        strs.Add(msg);
                    }
                    line = reader.ReadLine();
                }
                reader.Close();
                response.Close();
                return strs;
            }
            catch (Exception ex)
            {
                Console.WriteLine("��ȡĿ¼����" + ex.Message); 
                throw ex;
            }
            return strs;
        }

        /// <summary>
        /// ��ftp�������ϻ���ļ��б�
        /// ��λ�ȡĳһĿ¼�µ��ļ����ļ����б�
        /// ����FtpWebRequest��ֻ�ṩ��WebRequestMethods.Ftp.ListDirectory��ʽ��WebRequestMethods.Ftp.ListDirectoryDetails��ʽ��
        /// ���������ȡ�����ǰ����ļ��б���ļ����б����Ϣ�������ǵ���ֻ����ĳһ�ࡣ
        /// Ϊ��������Ҫ������ȡ��Ϣ���ص㡣�������֣������ļ��л��С�<DIR>����һ����ļ�û�С�
        /// �������ǿ��Ը�����������֡�һ�·ֱ��ǻ�ȡ�ļ��б���ļ����б�Ĵ��룺
        /// ��ȡ�ļ��У�
        /// </summary>
        /// <param name="RequedstPath">�������µ����·��</param>
        /// <returns></returns>
        public  List<string> GetFile(string RequedstPath)
        {
            List<string> strs = new List<string>();
            try
            {
                string uri = path + RequedstPath;   //Ŀ��·�� pathΪ��������ַ
                FtpWebRequest reqFTP = (FtpWebRequest)FtpWebRequest.Create(new Uri(uri));
                // ftp�û���������
                reqFTP.Credentials = new NetworkCredential(username, password);
                reqFTP.Method = WebRequestMethods.Ftp.ListDirectoryDetails;
                WebResponse response = reqFTP.GetResponse();
                StreamReader reader = new StreamReader(response.GetResponseStream());//�����ļ���

                string line = reader.ReadLine();
                while (line != null)
                {
                    if (!line.Contains("<DIR>"))
                    {
                        string msg = line.Substring(39).Trim();
                        strs.Add(msg);
                    }
                    line = reader.ReadLine();
                }
                reader.Close();
                response.Close();
                return strs;
            }
            catch (Exception ex)
            { 
                Console.WriteLine("��ȡ�ļ�����" + ex.Message);
                throw ex;
            }
            return strs;
        }

    }
}