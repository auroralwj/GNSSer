//2015.02.07, czs, create in pengzhou, 字节转运站，用于接收网络数据。

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Geo.Utils
{
     

    /// <summary>
    /// 字节顺序存储结构。编号低位为先传入的数据。后续增加的数据追加到后面。
    /// 是一个字节数组队列。
    /// </summary>
    public class ByteSequence : BaseQueue<byte>
    {
        /// <summary>
        /// 构造函数。
        /// </summary>
        public ByteSequence()
        { 
        }

      

        #region 字节反转工具

        /// <summary>
        /// 字节内部顺序对调
        /// </summary>
        /// <param name="bytes"></param>
        /// <returns></returns>
        public static List<byte> Inverse(List<byte> bytes)
        {
            return new List<byte>(Inverse(bytes.ToArray()));
        }
        /// <summary>
        /// 将字节反转。字节内部顺序对调
        /// </summary>
        /// <param name="bytes">待反转的字节</param>
        /// <returns></returns>
        public static byte[] Inverse(byte[] bytes)
        {
            int len = bytes.Length;
            byte[] newBytes = new byte[len];
            for (int i = 0; i < len; i++)
            {
                newBytes[i] = Inverse(bytes[i]);
            }
            return newBytes;
        }

        /// <summary>
        /// 将字节反转。
        /// </summary>
        /// <param name="aByte">待反转的字节</param>
        /// <returns></returns>
        public static byte Inverse(byte aByte)
        {
            return (byte)Inverse(aByte, 8);
        }
        /// <summary>
        /// 反转二进制序列。最多可以处理32位。
        /// </summary>
        /// <param name="bits">二进制序列</param>
        /// <param name="ch">二进制长度</param>
        /// <returns></returns>
        public static int Inverse(int bits, int ch = 8)
        {
            int value = 0;
            // 交换bit0和bit7，bit1和bit6，类推
            for (int i = 1; i < (ch + 1); i++)
            {
                if ((bits & 1) == 1) //若最低位为1
                    value |= 1 << (ch - i); //将1左移到指定位置，进行或计算，将对应的高位设为1
                //右移一位，将要处理的搬到最低位
                bits >>= 1;
            }
            return value;
        }
        #endregion

        public  void EnQuence(byte[] inBytes)
        {
            this.Queue.AddRange(inBytes);
        }

        public  List<byte> DeQueue(int p)
        {
            var minCount = Math.Min(Queue.Count, p);
            var list = new List<byte>(this.Queue.GetRange(0, minCount));
            this.Queue.RemoveRange(0, minCount);
            return list;
        }
    }
}