using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Globalization;
using System.Security.Cryptography;

namespace Geo.Winform
{
    public class MD5
    {
        ///MD5加密
        public string MD5Encrypt(string pToEncrypt, string sKey)
        {
            DESCryptoServiceProvider md5 = new DESCryptoServiceProvider();
            byte[] inputByteArray = Encoding.UTF8.GetBytes(pToEncrypt);
            md5.Key = ASCIIEncoding.ASCII.GetBytes(sKey);
            md5.IV = ASCIIEncoding.ASCII.GetBytes(sKey);
            MemoryStream ms = new MemoryStream();
            CryptoStream cs = new CryptoStream(ms, md5.CreateEncryptor(), CryptoStreamMode.Write);
            cs.Write(inputByteArray, 0, inputByteArray.Length);
            cs.FlushFinalBlock();
            StringBuilder ret = new StringBuilder();
            foreach (byte b in ms.ToArray())
            {
                ret.AppendFormat("{0:X2}", b);
            }
            ret.ToString();
            return ret.ToString();
        }

        ///MD5解密
        public string MD5Decrypt(string pToDecrypt, string sKey)
        {
            DESCryptoServiceProvider md5 = new DESCryptoServiceProvider();

            byte[] inputByteArray = new byte[pToDecrypt.Length / 2];
            for (int x = 0; x < pToDecrypt.Length / 2; x++)
            {
                int i = (Convert.ToInt32(pToDecrypt.Substring(x * 2, 2), 16));
                inputByteArray[x] = (byte)i;
            }

            md5.Key = ASCIIEncoding.ASCII.GetBytes(sKey);
            md5.IV = ASCIIEncoding.ASCII.GetBytes(sKey);
            MemoryStream ms = new MemoryStream();
            CryptoStream cs = new CryptoStream(ms, md5.CreateDecryptor(), CryptoStreamMode.Write);
            cs.Write(inputByteArray, 0, inputByteArray.Length);
            cs.FlushFinalBlock();

            StringBuilder ret = new StringBuilder();

            return System.Text.Encoding.UTF8.GetString(ms.ToArray());
        }

        // 创建Key
        public string GenerateKey()
        {
            DESCryptoServiceProvider desCrypto = (DESCryptoServiceProvider)DESCryptoServiceProvider.Create();
            return ASCIIEncoding.ASCII.GetString(desCrypto.Key);
        }
        // 加密字符串
        public string EncryptString(string sInputString, string sKey)
        {
            byte[] data = Encoding.UTF8.GetBytes(sInputString);
            DESCryptoServiceProvider md5 = new DESCryptoServiceProvider();
            md5.Key = ASCIIEncoding.ASCII.GetBytes(sKey);
            md5.IV = ASCIIEncoding.ASCII.GetBytes(sKey);
            ICryptoTransform desencrypt = md5.CreateEncryptor();
            byte[] result = desencrypt.TransformFinalBlock(data, 0, data.Length);
            return BitConverter.ToString(result);
        }
        // 解密字符串
        public string DecryptString(string sInputString, string sKey)
        {
            string[] sInput = sInputString.Split("-".ToCharArray());
            byte[] data = new byte[sInput.Length];
            for (int i = 0; i < sInput.Length; i++)
            {
                data[i] = byte.Parse(sInput[i], NumberStyles.HexNumber);
            }
            DESCryptoServiceProvider md5 = new DESCryptoServiceProvider();
            md5.Key = ASCIIEncoding.ASCII.GetBytes(sKey);
            md5.IV = ASCIIEncoding.ASCII.GetBytes(sKey);
            ICryptoTransform desencrypt = md5.CreateDecryptor();
            byte[] result = desencrypt.TransformFinalBlock(data, 0, data.Length);
            return Encoding.UTF8.GetString(result);
        }
    }
    class Test
    {
        static void Main2()
        {
            bool sy = true;
            char a;
            MD5 md5 = new MD5();
            string key = md5.GenerateKey();
            string s0 = " microsoft ";
            string s1 = md5.EncryptString(s0, key);
            string s2 = md5.DecryptString(s1, key);
            Console.WriteLine("原串: [{0}]", s0);
            Console.WriteLine("加密: [{0}]", s1);
            Console.WriteLine("解密: [{0}]", s2);
            while (sy)
            {
                Console.WriteLine("是否输入字符串？ Y/N");
                a = Convert.ToChar(Console.ReadLine());
                if (a == 'Y' || a == 'y')
                {
                    sy = true;
                    Console.WriteLine(" 请输入所要验证的字符串 ");
                    s0 = Console.ReadLine();
                    s1 = md5.EncryptString(s0, key);
                    s2 = md5.DecryptString(s1, key);
                    Console.WriteLine("原串: [{0}]", s0);
                    Console.WriteLine("加密: [{0}]", s1);
                    Console.WriteLine("解密: [{0}]", s2);
                }
                else
                    if (a == 'N' || a == 'n')
                        sy = false;
                    else
                        Console.WriteLine(" 您一定输错了，请重新输入 ！");
            }
        }
    }

}
