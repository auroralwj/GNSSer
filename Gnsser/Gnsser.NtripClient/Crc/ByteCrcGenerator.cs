//2015.02， czs , create in pengzhou, CRC校验，正序

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gnsser.Ntrip
{
    /// <summary>
    /// 好像还需要修改 2015.02.17
    /// </summary>
    public class ByteCrcGenerator
    {





        public void main()
        {
            char i;
            int j, crc;
            //--------------------------------------下面循环求出CRC
            for (j = 0; j < 256; j++)
            {         //8位数据00~FF
                crc = 0;                          //每个数据的16位的CRC
                for (i = (char)0x80; i != 0; i >>= 1)
                { //对8位数据从高位到低位进行判断
                    if ((crc & 0x8000) != 0)
                    {
                        crc <<= 1;
                        crc ^= 0x1021;
                    }
                    else crc = crc << 1;
                    if ((j & i) != 0) crc ^= 0x1021;
                }
                //--------------------------------------计算完毕，下面进行显示
                //if (j % 4 == 0)  printf("\n");
                //if (j % 32 == 0)      system("pause");
                //printf("0x%02X: 0x%04x,  ", j, crc & 0xFFFF);
            }
        }

        int Reflect(int refer, char ch)
        {
            int value = 0;
            // 交换bit0和bit7，bit1和bit6，类推
            for (int i = 1; i < (ch + 1); i++)
            {
                if ((refer & 1) == 1)
                    value |= 1 << (ch - i);
                refer >>= 1;
            }
            return value;
        }

 
        /// <summary>
        /// 通用CRC 计算程序.这是最原始的计算方法。
        /// </summary>
        /// <param name="POLY">生成项，需要含有最高位的"1"，如果 CRC 32比特，则多项式是33比特</param>
        /// <param name="crcBitNumber">crc是32比特</param>
        /// <param name="satData">待测数据，为字串"1234"</param>
        /// <param name="dataBitNumber">数据是32比特</param>
        public static long GetCrcByteByByte(byte[] data, int dataBitNumber = 32, long POLY = 0x104C11DB7, int crcBitNumber = 32)
        {
            long MaxCrc = 1;
            for (int i = 0; i < crcBitNumber -1; i++)
            {
                MaxCrc = MaxCrc << 1;
                MaxCrc = MaxCrc | 1;
            }

           var AugmentedList = new List<byte>(data);
           var crcByteCount = crcBitNumber / 8;
            for (int i = 0; i < crcByteCount; i++)
			{
                AugmentedList.Add(0);
			}

            var AugmentedData = AugmentedList.ToArray();

            long regi = 0x0; // load the register with zero bits
            // augment the satData by appending W zero bits to the end of it.
            //按CRC计算的定义，加入32个比特0，以容纳32比特的CRC；
            //这样共有64比特待测数据，从第33比特开始做除法，就要做64－33＋1＝32次XOR
            //satData <<= crcBitNumber;
            // we do it bit after bit
            int allLen = dataBitNumber + crcBitNumber;
            //处理64 次（32 比特待测数据＋32 比特扩展0），前32次是加载数据
            for (int currentDataBitIndex = 0; currentDataBitIndex < allLen; ++currentDataBitIndex)
            {
                // test the highest bit which will be poped later.
                var highestBit = GetBit(regi, crcBitNumber);
                if (highestBit == 0x1) { regi = regi ^ POLY; }

                // shift the register
                regi <<= 1;

                //获取下一个待计算的数据位 // reading the next bit of the augmented satData//加载待测数据1比特到tmp中，tmp只有1比特 
                long  lowerstBit  = GetBit(AugmentedData, currentDataBitIndex);  

                regi |= lowerstBit; //这1比特加载到寄存器中
            }
            var highestBit2 = GetBit(regi, crcBitNumber);
            if (highestBit2 == 0x1) 
                 { regi = regi ^ POLY; } //做最后一次XOR

            //这时， regi中的值就是CRC
            var result = regi & MaxCrc;
            return result;
        }


        public static long GetCrc(  int data  , int bitlen, int POLY = 0x13, int polyLen = 24)
        {
            //网上的程序经修改
         //  ; //生成项，13H＝10011，这样CRC是4比特
            //待测数据是35BH，12比特，注意，数据不是16比特
            int regi = 0x0000; // load the register with zero bits
            // augment the satData by appending W(4) zero bits to the end of it.
            //按CRC计算的定义，待测数据后加入4个比特0，以容纳4比特的CRC；
            //这样共有16比特待测数据，从第5比特开始做除法，就要做16－5＋1＝12次XOR
            data <<= 4;
            // we do it bit after bit
            int allLen = bitlen + polyLen;
            for (int cur_bit = 15; cur_bit >= 0; --cur_bit) //处理16次，前4次实际上只是加载数据
            {
                // test the highest bit which will be poped later.
                /// in fact, the 5th bit from right is the hightest bit here
                if (((regi >> 4) & 0x0001) == 0x1) regi = regi ^ POLY;
                regi <<= 1; // shift the register
                // reading the next bit of the augmented satData
                int tmp = (data >> cur_bit) & 0x0001; //加载待测数据1比特到tmp中，tmp只有1比特
                regi |= tmp; //这1比特加载到寄存器中
            }
            if (((regi >> 4) & 0x0001) == 0x1) regi = regi ^ POLY; //做最后一次XOR
            //这时， regi中的值就是CRC
            return regi;
        }


        #region 原始备份
        /// <summary>
        /// 通用CRC 计算程序.这是最原始的计算方法。
        /// </summary>
        /// <param name="POLY">生成项，需要含有最高位的"1"，这样CRC是32比特</param>
        /// <param name="crcBitNumber">crc是32比特</param>
        /// <param name="satData">待测数据，为字串"1234"</param>
        /// <param name="dataBitNumber">数据是32比特</param>
        public static long GetCrc( long data = 0x31323334,  int dataBitNumber = 32,long POLY = 0x104C11DB7, int crcBitNumber = 32)
        {   
            long regi = 0x0; // load the register with zero bits
            // augment the satData by appending W zero bits to the end of it.
            //按CRC计算的定义，加入32个比特0，以容纳32比特的CRC；
            //这样共有64比特待测数据，从第33比特开始做除法，就要做64－33＋1＝32次XOR
            data <<= crcBitNumber;
            // we do it bit after bit
            for (int cur_bit = dataBitNumber + crcBitNumber - 1; cur_bit >= 0; --cur_bit) //处理64 次（32 比特待测数据＋32 比特扩展0），前32次是加载数据
            {
                // test the highest bit which will be poped later.
                /// in fact, the 5th bit from right is the hightest bit here
                if (((regi >> crcBitNumber) & 0x0001) == 0x1) regi = regi ^ POLY;
                regi <<= 1; // shift the register
                // reading the next bit of the augmented satData
                long tmp = (data >> cur_bit) & 0x0001; //加载待测数据1比特到tmp中，tmp只有1比特
                regi |= tmp; //这1比特加载到寄存器中
            }
            if (((regi >> crcBitNumber) & 0x0001) == 0x1) regi = regi ^ POLY; //做最后一次XOR
            //这时， regi中的值就是CRC
            return regi;
        }
        #endregion

        /// <summary>
        /// 获取指定编号的位数据。
        /// </summary>
        /// <param name="bytes">用于存储二进制数据位，注意：一个byte从高位到低位的编号顺序为7-0.</param>
        /// <param name="index">二进制数据的编号</param>
        /// <returns></returns>
        static public int GetBit(byte[] bytes, int index)
        {
            int arrayIndex = index / 8;
            int byteIndex = index % 8;
            if (arrayIndex >= bytes.Length) throw new IndexOutOfRangeException();

            byte bt = bytes[arrayIndex];
            //注意：一个byte从高位到低位的编号顺序为7-0.
            // int bit = (bt >> (byteIndex)) & 0x1;
            int bit = GetBit(bt, byteIndex);

            if (arrayIndex == 0)
            {
                int a = 0;//用于调试
            }
            return bit;
        }

        /// <summary>
        /// 获取指定的bit位。
        /// </summary>
        /// <param name="ObsDataSource">bit数据源</param>
        /// <param name="index">检索编号，从低位到高位，从0开始，从右到左</param>
        /// <returns></returns>
        public static long GetBit(long source, int index)
        {
            return ((source >> index) & 0x0001);
        }
        public static int GetBit(int source, int index)
        {
            return ((source >> index) & 0x0001);
        }
    }
}
