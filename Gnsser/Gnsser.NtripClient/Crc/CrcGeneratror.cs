//2015.02， czs , create in pengzhou, CRC校验，正序

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gnsser.Ntrip
{
    /// <summary>
    /// 正序，似乎只有我才是对的。
    /// </summary>
    public class CrcGeneratror
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="gPloy">简化多项式，不需要高位</param>
        /// <param name="polyCount"></param>
        public CrcGeneratror(int gPloy = 0x1021, int polyCount = 24)
        {
            this.GenPloy = gPloy;
            this.PolyCount = polyCount;
            this.MaxCrcNumber = this.GetMax(polyCount);
            ComputeCrcTable();
        }
        private int[] crcTable = new int[256];
        /// <summary>
        ///  生成多项式
        /// </summary>
        private int GenPloy { get; set; }
        public int PolyCount { get; set; }

        public int MaxCrcNumber { get; set; }

        /// <summary>
        /// 计算单个8位字节的CRC编码。
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        private int GetCrcValueOfKey(int key)
        {
            int crcValue = 0;
            int shifted = key << ( PolyCount - 8);//向左移位,左边首位对齐，留出CRC位为0.
          
            for (int i = 7; i >=0; i--)//8 次
            {
                var firsCrcBit = ((shifted ^ crcValue) >> (PolyCount - 1)) & 0x1;
                if (firsCrcBit == 1)//首位是否为1，为1，则按位异或
                {
                    crcValue = (crcValue << 1) ^ GenPloy;
                }
                else
                {
                    crcValue <<= 1; // 首位为0，直接左移
                }

                shifted <<= 1;
            }
            crcValue = crcValue & MaxCrcNumber; // 取低位的值，为该字节的CRC编码。
            return crcValue;

          //  for (int i32 = 0; i32 < 256; i32++ )
          //  {


          //      int  nData32 = ( i32 << 24 );
          //      int  nAccum32 = 0;
          //  for (int j32 = 0; j32 < 8; j32++ )
          //  {
          //  if (( ( nData32 ^ nAccum32 ) & 0x80000000) == 1 )
          //  nAccum32 = ( nAccum32 << 1 ) ^ cnCRC_32;
          //  else
          //  nAccum32 <<= 1;
          //  nData32 <<= 1;


          //  }
          //  Table_CRC32[i32] = nAccum32;
          //  }


        }

        /// <summary>
        /// 生成0 - 255对应的CRC16校验码
        /// </summary>
        private void ComputeCrcTable()
        {
            for (int i = 0; i < 256; i++)
            {
                crcTable[i] = GetCrcValueOfKey(i);
            }
        }
        /// <summary>
        /// 计算数组的CRC。
        /// </summary>
        /// <param name="satData">字节数组</param>
        /// <returns></returns>
        public int GetCrc(byte[] data)
        {
            int crc = 0;//校验码（寄存器）初始化为0.
            int length = data.Length;
            for (int i = 0; i < length; i++)
            {
                //描述：
                 //寄存器左移出1字节，右边补0；
                 //移出的字节与待测信息字节进行XOR，根据结果值查表，得表值
                 //表值与寄存器进行XOR
                  
                //计算查表法的检索
                var tobeMovedOut = (crc >> (PolyCount - 8)) & 0xFF; //获取即将左移出的高8位字节， 
                crc = (crc << 8) & this.MaxCrcNumber;//左移八位,将移出的字节去掉

                var index = (tobeMovedOut ^ data[i]);//移出的字节与待测信息字节进行XOR，根据结果值查表，得表值
                                                     // 若是第一次，则是原数据本身 

                crc = crc ^ crcTable[index];//表值与寄存器进行XOR
                //对于第一次执行，则是移入了一个字节。
            }
            crc = crc & this.MaxCrcNumber;
            return  crc;
        }

        private int GetMax(int bitWidth)
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

            return Convert.ToInt32(str, 16);
        }
    }

    public class CodecUtil
    {
        static CrcGeneratror crc16 = new CrcGeneratror();
        private CodecUtil()
        {
        }
        public static byte[] short2bytes(short s)
        {
            byte[] bytes = new byte[2];
            for (int i = 1; i >= 0; i--)
            {
                bytes[i] = (byte)(s % 256);
                s >>= 8;
            }
            return bytes;
        }
        public static short bytes2short(byte[] bytes)
        {
            short s = (short)(bytes[1] & 0xFF);
            s |= (short)((bytes[0] << 8) & 0xFF00);
            return s;
        }
        /*
         * 获取crc校验的byte形式
         */
        public static byte[] crc16Bytes(byte[] data)
        {
            return short2bytes(crc16Short(data));
        }
        /*
         * 获取crc校验的short形式
         */
        public static short crc16Short(byte[] data)
        {
            return (short)crc16.GetCrc(data);
        }
        public static void main(String[] args)
        {
            byte[] test = new byte[] { 0, 1, 2, 3, 4 };
            byte[] crc = crc16Bytes(test);
            byte[] testc = new byte[test.Length + 2];
            for (int i = 0; i < test.Length; i++)
            {
                testc[i] = test[i];
            }
            testc[test.Length] = crc[0];
            testc[test.Length + 1] = crc[1];
            // System.out.println(crc16Short(testc));
        }
    }
}
