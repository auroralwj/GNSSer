
//2015.02， czs , create in pengzhou, CRC校验，反序

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gnsser.Ntrip
{
    public sealed class CrcUtil
    { 

        private static ushort[] CRC16Table = null;
        private static uint[] CRC32Table = null;
        private static uint[] CRC24Table = null;
        private static ushort crc16GenPoly = 0x8408;
       // private static uint crc24GenPoly = 0x1BE64C3;//0X1864CFB;//ntrip3 ???????
        private static uint crc24GenPoly = 0X864CFB;//0X864CFB;//ntrip3 ???????
        private static uint crc32GenPoly = 0xEDB88320;
        private static void MakeCRC16Table()
        {
            if (CRC16Table != null) return;
            CRC16Table = new ushort[256];
            for (ushort i = 0; i < 256; i++)
            {
                ushort vCRC = i;
                for (int j = 0; j < 8; j++)
                    if (vCRC % 2 == 0)
                        vCRC = (ushort)(vCRC >> 1);
                    else vCRC = (ushort)((vCRC >> 1) ^ crc16GenPoly);
                CRC16Table[i] = vCRC;
            }
        }

        private static void MakeCRC24Table()
        {
           // if (CRC24Table != null) return;
            CRC24Table = new uint[256];
            for (uint i = 0; i < 256; i++)
            {
                uint vCRC = i;
                for (int j = 0; j < 8; j++)
                    if (vCRC % 2 == 0)//如果低位为0，则右移一位取本身
                    { vCRC = (uint)(vCRC >> 1); }
                    else//否则，右移一位，取异或，位除法取余
                    {
                        vCRC = (uint)((vCRC >> 1) ^ crc24GenPoly);
                    }
                CRC24Table[i] = vCRC;
            }
        }

        private static void MakeCRC32Table()
        {
            if (CRC32Table != null) return;
            CRC32Table = new uint[256];
            for (uint i = 0; i < 256; i++)
            {
                uint vCRC = i;
                for (int j = 0; j < 8; j++)
                    if (vCRC % 2 == 0)
                        vCRC = (uint)(vCRC >> 1);
                    else vCRC = (uint)((vCRC >> 1) ^ 0xEDB88320);
                CRC32Table[i] = vCRC;
            }
        }

        public static ushort UpdateCRC16(byte AByte, ushort ASeed)
        {
            return (ushort)(CRC16Table[(ASeed & 0x000000FF) ^ AByte] ^ (ASeed >> 8));
        }
        public static uint UpdateCRC24(byte AByte, uint ASeed)
        {
            return (uint)(CRC24Table[(ASeed & 0x000000FF) ^ AByte] ^ (ASeed >> 8));
        }
        public static uint UpdateCRC32(byte AByte, uint ASeed)
        {
            return (uint)(CRC32Table[(ASeed & 0x000000FF) ^ AByte] ^ (ASeed >> 8));
        }
        #region 16
        public static ushort CRC16(byte[] ABytes)
        {
            MakeCRC16Table();
            ushort Result = 0xFFFF;
            foreach (byte vByte in ABytes)
                Result = UpdateCRC16(vByte, Result);
            return (ushort)(~Result);
        }
        public static ushort CRC16(string AString, Encoding AEncoding)
        {
            return CRC16(AEncoding.GetBytes(AString));
        }
        public static ushort CRC16(string AString)
        {
            return CRC16(AString, Encoding.UTF8);
        }
        #endregion
        #region 32
        public static uint CRC32(byte[] ABytes)
        {
            MakeCRC32Table();
            uint Result = 0xFFFFFFFF;
            foreach (byte vByte in ABytes)
                Result = UpdateCRC32(vByte, Result);
            return (uint)(~Result);
        }
        public static uint CRC32(string AString, Encoding AEncoding)
        {
            return CRC32(AEncoding.GetBytes(AString));
        }
        public static uint CRC32(string AString)
        {
            return CRC32(AString, Encoding.UTF8);
        }
        #endregion
        #region 24
        /// <summary>
        /// 好像进行的是逆序
        /// </summary>
        /// <param name="ABytes"></param>
        /// <param name="crc24GenPoly"></param>
        /// <returns></returns>
        public static uint CRC24(byte[] ABytes, uint crc24GenPoly = 0xBE64C3)
        {
            CrcUtil.crc24GenPoly = crc24GenPoly;
            MakeCRC24Table();
            uint Result = 0xFFFFFF;
            for (int i = 0; i < ABytes.Length; i++)
            {
                Result = UpdateCRC24(ABytes[i], Result);
            } 
                
            return (uint)(~Result) & 0xFFFFFF;//只取后24位
        }
        public static uint CRC24(string AString, Encoding AEncoding)
        {
            return CRC24(AEncoding.GetBytes(AString), 0xBE64C3);
        }
        public static uint CRC24(string AString)
        {
            return CRC24(AString, Encoding.UTF8);
        }
        #endregion
    }
}
