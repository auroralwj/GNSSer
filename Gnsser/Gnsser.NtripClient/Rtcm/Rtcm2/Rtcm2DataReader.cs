//2015.02.04, czs, create in pengzhou, 处理 RTCM 2.X 版本的数据读取。

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gnsser.Ntrip.Rtcm
{
    /// <summary>
    /// 处理 RTCM 2.X 版本的数据读取。
    /// </summary>
     public  class Rtcm2DataReader
    {

         public Rtcm2DataReader()
         {
            
         }
         string prevRemained = "";//上一次处理剩下的

         List<string> tailReversedBinStrs = new List<string>();//上一次处理剩下的
         /// <summary>
         /// 接收数据流
         /// </summary>
         /// <param name="inbytes"></param>
         public void Receive(byte[] inbytes)
         {
             //下面进行解码工作

             //下面进行解码工作

             //(1) 字节扫描，过滤掉无效数据
             List<byte> filteredBytes = FilterExtraBytes(inbytes);
             //数量是否足够
             //(2) 字节滚动,且组成 rtcm 字
             //由于UART 为异步通信,优先发送或接收低位数据,接收到RTCM 字节后必须进行“字节滚动”, 1-6,2-5,3-4. 
             List<string> reversedBinStrs = ReverseLowerSixBitOrder(filteredBytes);
             //上次余下的
             if (tailReversedBinStrs.Count != 0)
             {
                 reversedBinStrs.InsertRange(0, tailReversedBinStrs);
             }
             //遍历查找每一个是否匹配
             int headerIndex = SearchHeaderV2(reversedBinStrs);
             if (headerIndex != -1)
             {
                 //
                 var prevSix = reversedBinStrs[headerIndex - 1];
                 //移除前面的无效数据
                 reversedBinStrs.RemoveRange(0, headerIndex);

                 //转换为30位一组（5个8位字节（取6位））的列表。
                 List<string> rtcmWords = GetRtcm2BinWords(ref reversedBinStrs);

                 //剩下的不足一字的。用于下一次？
                 tailReversedBinStrs.AddRange(reversedBinStrs);

                 //是否取补码
                 string headerString = rtcmWords[0];
                 if (prevSix.EndsWith("1"))
                 {
                     headerString = GetComplementCode(headerString.Substring(0, 24)) + headerString.Substring(24);
                 }


                 //解析
                 Gnsser.Ntrip.Rtcm.Rtcm2Header1 header1 = Gnsser.Ntrip.Rtcm.Rtcm2Header1.Parse(headerString);
                 //29,30校验码
                 bool bit29 = prevSix[4].ToString() == "1";
                 bool bit30 = prevSix[5].ToString() == "1";

                 if (header1.Check(bit29, bit30))
                 {
                    // MessageBox.Show("测试1通过啦！！！");
                     if (rtcmWords.Count > 1)
                     {
                         headerString = rtcmWords[1];
                         prevSix = rtcmWords[0].Substring(24);
                         if (prevSix.EndsWith("1"))
                         {
                             headerString = GetComplementCode(headerString.Substring(0, 24)) + headerString.Substring(24);
                         }

                       var header2 = Gnsser.Ntrip.Rtcm.Rtcm2Header2.Parse(rtcmWords[1]);

                         if (header2.Check(bit29, bit30))
                         {
                           //  MessageBox.Show("测试2通过啦！！！");
                         }
                     }
                    // Gnsser.Ntrip.Rtcm.RtcmHeader header = new RtcmHeader();
                 }

             }
         }

         #region 工具函数
         /// <summary>
         /// 查找是否包含头部，返回头部所在的下标。
         /// </summary>
         /// <param name="reversedBinStrs"></param>
         /// <returns></returns>
         private static int SearchHeaderV2(List<string> reversedBinStrs)
         {
             int headerIndex = -1;
             for (int i = 0; i < reversedBinStrs.Count - 5; i++)
             {
                 var prevSix = reversedBinStrs[i];
                 // 拼接24个，好取补码
                 var current24 = reversedBinStrs[i + 1] + reversedBinStrs[i + 2] + reversedBinStrs[i + 3] + reversedBinStrs[i + 4];
                 //是否取补码
                 //判断是否取补码
                 //(3) 字节取补码
                 //按上述步骤处理连续5 个RTCM 字节后,将每个字节的低6 位连接起来,
                 //得到一个完整的RTCM 字。与GPS导航电文类似,若前一个字的最后一个bit d30 为1 ,
                 //则当前字的前24 位d1 ～ d24 需要取补码;若d30 为0 ,则当前字保持不变。
                 if (prevSix.EndsWith("1"))
                 {
                     current24 = GetComplementCode(current24);
                 }

                 if (current24.StartsWith(Rtcm2Header1.Preamble_2))
                 {
                     //找到了。//拼接一个 30 位的字节测试，是否是头部 
                     var headString = current24 + reversedBinStrs[i + 5];
                     Gnsser.Ntrip.Rtcm.Rtcm2Header1 header1 = Gnsser.Ntrip.Rtcm.Rtcm2Header1.Parse(headString);
                     //29,30校验码
                     bool bit29 = prevSix[4].ToString() == "1";
                     bool bit30 = prevSix[5].ToString() == "1";

                     if (header1.Check(bit29, bit30))
                     {
                         headerIndex = i + 1;
                         break;
                     }
                 }
             }
             return headerIndex;
         }

         /// <summary>
         /// 取补码
         /// </summary>
         /// <param name="str"></param>
         /// <returns></returns>
         private static string GetComplementCode(string str)
         {
             StringBuilder sb = new StringBuilder();
             foreach (var c in str)
             {
                 if (c + "" == "0") sb.Append("1");
                 else sb.Append("0");
             }

             var result = sb.ToString();
             return result;
         }


         /// <summary>
         /// 从第一个开始， 按照 5 个一分组，组成rtcm二进制字节。
         /// </summary>
         /// <param name="sixBits">6 位二进制集合,生成一个将删除前5个</param>
         /// <returns></returns>
         private static List<string> GetRtcm2BinWords(ref List<string> sixBits)
         {
             List<string> rtcmWords = new List<string>();
             while (sixBits.Count >= 5)
             {
                 StringBuilder rtcmWordSb = new StringBuilder();
                 for (int j = 0; j < 5; j++)//每 5 个 6 位的二进制组成 30 位的一个 rtcm 字节。
                 {
                     rtcmWordSb.Append(sixBits[j]);
                 }
                 var rtcmWord = rtcmWordSb.ToString();
                 rtcmWords.Add(rtcmWord);
                 //移除
                 sixBits.RemoveRange(0, 5);
             }
             return rtcmWords;
         }
         /// <summary>
         /// 将8位的二进制字节的后6位顺序调换，6-1,5-2,4-3.返回字符串表示的二进制数。
         /// </summary>
         /// <param name="bytes">待转换的字节，前两位（7,8位）将被忽略</param>
         /// <returns></returns>
         private static List<string> ReverseLowerSixBitOrder(List<byte> bytes)
         {
             List<string> reversedBinStrs = new List<string>();
             foreach (var bt in bytes)
             {
                 var reversedBinString = ReverseLastSixBit(bt);
                 reversedBinStrs.Add(reversedBinString);
             }
             return reversedBinStrs;
         }

         /// <summary>
         /// 将8位的二进制字节的后6位顺序调换，6-1,5-2,4-3.返回字符串表示的二进制数。
         /// </summary>
         /// <param name="bt">字节</param>
         /// <returns></returns>
         private static string ReverseLastSixBit(byte bt)
         {
             var bit = Convert.ToString(bt, 2);//十进制转为二进制字符串
             while (bit.Length < 6)
             {
                 bit = "0" + bit;
             }
             var sixChar = bit.Substring(bit.Length - 6);
             var reverseddBinString = Reverse(sixChar);
             return reverseddBinString;
         }

         /// <summary>
         /// 过滤掉不在范围内的数。
         /// </summary>
         /// <param name="inBytes">输入集合</param>
         /// <param name="largerThanExclude">大于此数（不含）</param>
         /// <param name="smallerThanExclude">小于此数（不含）</param>
         /// <returns></returns>
         private static List<byte> FilterExtraBytes(byte[] inBytes, int largerThanExclude = 63, int smallerThanExclude = 128)
         {
             // RTCM 电文传输时,通常只有低6 位是有效位,7 、8 位是填充位,7 位置“1”,8 位置“0”。
             //所以,接收到的字节值只有在64～127 之间,才是有效的,否则就要删除掉。 
             List<byte> filteredBytes = new List<byte>();
             foreach (var item in inBytes)
             {
                 if (item > largerThanExclude && item < smallerThanExclude)
                     filteredBytes.Add(item);
             }
             return filteredBytes;
         }

         /// <summary>
         /// 字符串顺序调换，第一位和最后一位调换。相当于反向排序。
         /// </summary>
         /// <param name="str">字符串</param>
         /// <returns></returns>
         private static String Reverse(string str)
         {
             StringBuilder sb = new StringBuilder();
             for (int i = str.Length - 1; i >= 0; i--)
             {
                 sb.Append(str[i]);
             }
             return sb.ToString();
         }
         #endregion


    }
}
