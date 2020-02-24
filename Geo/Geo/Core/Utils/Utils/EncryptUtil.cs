using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Security.Cryptography;

namespace Geo.Utils
{
    /// <summary>
    /// 自定义简单加密程序，用于保护密码。
    /// </summary>
    public class EncryptUtil
    {
        /// <summary>
        /// 加密
        /// </summary>
        /// <param name="normal"></param>
        /// <returns></returns>
        public static string Encrypt(string normal)
        {
            //return normal;

            return EncryptUtil.EncryptDES(normal, "Geodata2");
            ////按照字加密
            //char[] chars = normal.ToCharArray();
            ////首先颠倒顺序,逆序
            //List<char> colName = new List<char>(chars);
            //colName.Reverse();
            ////+5
            //for (int i = 0; i < colName.Count; i++)
            //{
            //    colName[i] = (char)(colName[i] + i);
            //}
            //return new string(colName.ToArray());             
        }

        /// <summary>
        /// 解密,如果是非Base64，则直接返回字符串。
        /// </summary>
        /// <param name="normal"></param>
        /// <returns></returns>
        public static string Decrypt(string encode)
        {
            return EncryptUtil.DecryptDES(encode, "Geodata2");

            //char[] chars = encode.ToCharArray();
            ////首先颠倒顺序,逆序
            //List<char> colName = new List<char>(chars);
            ////+5
            //for (int colNum = 0; colNum < colName.Count; colNum++)
            //{
            //    colName[colNum] = (char)(colName[colNum] - colNum);
            //}
            //colName.Reverse();
            //return new string(colName.ToArray());      
        }

        //默认密钥向量 
        private static byte[] Keys = { 0x12, 0x34, 0x56, 0x78, 0x90, 0xAB, 0xCD, 0xEF };
        /// <summary> 
        /// DES加密字符串 
        /// </summary> 
        /// <param name="encryptString">待加密的字符串</param> 
        /// <param name="encryptKey">加密密钥,要求为8位</param> 
        /// <returns>加密成功返回加密后的字符串，失败返回源串</returns> 
        public static string EncryptDES(string encryptString, string encryptKey)
        {
            try
            {
                byte[] rgbKey = Encoding.UTF8.GetBytes(encryptKey.Substring(0, 8));
                byte[] rgbIV = Keys;
                byte[] inputByteArray = Encoding.UTF8.GetBytes(encryptString);
                DESCryptoServiceProvider dCSP = new DESCryptoServiceProvider();
                MemoryStream mStream = new MemoryStream();
                CryptoStream cStream = new CryptoStream(mStream, dCSP.CreateEncryptor(rgbKey, rgbIV), CryptoStreamMode.Write);
                cStream.Write(inputByteArray, 0, inputByteArray.Length);
                cStream.FlushFinalBlock();
                return Convert.ToBase64String(mStream.ToArray());
            }
            catch
            {
                return encryptString;
            }
        }

        /// <summary> 
        /// DES解密字符串 
        /// </summary> 
        /// <param name="decryptString">待解密的字符串</param> 
        /// <param name="decryptKey">解密密钥,要求为8位,和加密密钥相同</param> 
        /// <returns>解密成功返回解密后的字符串，失败返源串</returns> 
        public static string DecryptDES(string decryptString, string decryptKey)
        {
            try
            {
                byte[] rgbKey = Encoding.UTF8.GetBytes(decryptKey);
                byte[] rgbIV = Keys;
                byte[] inputByteArray = Convert.FromBase64String(decryptString);
                DESCryptoServiceProvider DCSP = new DESCryptoServiceProvider();
                MemoryStream mStream = new MemoryStream();
                CryptoStream cStream = new CryptoStream(mStream, DCSP.CreateDecryptor(rgbKey, rgbIV), CryptoStreamMode.Write);
                cStream.Write(inputByteArray, 0, inputByteArray.Length);
                cStream.FlushFinalBlock();
                return Encoding.UTF8.GetString(mStream.ToArray());
            }
            catch
            {
                return decryptString;
            }
        }
    }
}
