
//2015.02， czs , create in pengzhou, CRC校验，反序

using System; 
using System.Collections.Generic; using System.Text; using System.IO;
namespace Gnsser.Ntrip
{
    /// <summary>
    /// 调用方法： 好像是逆序。
    ///  CRC32Cls CRC = new CRC32Cls();
    ///textBox2.Text = String.Format("{0:X8}", CRC.GetCRC32Str(textBox1.Text));
    /// </summary>
    public class CrcSovler
    {
        public CrcSovler(uint poly = 0xEDB88320, int PolyCount = 32)
        {
            this.Poly = poly;
             
            this.PolyCount = PolyCount;
            this.MaxCrcNumber = this.GetMax(PolyCount);
            GetCRC32Table();
        }
        private int[] crcTable = new int[256];
        /// <summary>
        ///  生成多项式
        /// </summary>
        private int GenPloy { get; set; }
        public int PolyCount { get; set; }

        public uint MaxCrcNumber { get; set; }

        public uint Poly { get; set; }
        protected ulong[] Crc32Table;         //生成CRC32码表 
        public void GetCRC32Table()
        {
            ulong Crc;
            Crc32Table = new ulong[256]; int i, j;
            for (i = 0; i < 256; i++)
            {
                Crc = (ulong)i;
                for (j = 8; j > 0; j--)
                {
                    if ((Crc & 1) == 1)
                        Crc = (Crc >> 1) ^ Poly;
                    else
                        Crc >>= 1;
                }
                Crc32Table[i] = Crc;
            }
        }
        //获取字符串的CRC32校验值 
        public ulong GetCRC32Str(string sInputString)
        {
            //生成码表             GetCRC32Table(); 
            byte[] buffer = System.Text.ASCIIEncoding.ASCII.GetBytes(sInputString);
            return GetCrc(buffer);
        }

        public ulong GetCrc(byte[] buffer)
        {
            ulong value = MaxCrcNumber;
            int len = buffer.Length;
            for (int i = 0; i < len; i++)
            {
                value = (value >> 8) ^ Crc32Table[(value & 0xFF) ^ buffer[i]];
            }
            return value ^ MaxCrcNumber;
        }
        private uint GetMax(int bitWidth)
        {
            string str = "0xF";
            switch (bitWidth)
            {
                case 8: str = "0xFF"; break;
                case 16: str = "0xFFFF"; break;
                case 24: str = "0xFFFFFF"; break;
                case 32: str = "0xFFFFFFFF"; break;
                default: throw new ArgumentException("不支持");
            }

            return Convert.ToUInt32(str, 16);
        }
    }
}