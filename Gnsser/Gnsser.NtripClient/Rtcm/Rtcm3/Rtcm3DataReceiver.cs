//2015.02.04, czs, create in pengzhou, 处理 RTCM 3.X 版本的数据读取。
//2017.04.22,, czs, edit in hongqing, 进行了封装

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Geo.Utils;

namespace Gnsser.Ntrip.Rtcm
{
    /// <summary>
    /// 接收原始数据，并判断内容或头部数据，拼接后通知解析器解析。
    /// 是通用数据接收器，与测站无关。
    /// </summary>
    public class Rtcm3DataReceiver
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public Rtcm3DataReceiver()
        {
            this.DataAquirement = new DataAquirement(50, DataPositionState.FindingPreamble);
            this.ByteSequence = new ByteSequence();
        }

        /// <summary>
        /// 数据接收事件
        /// </summary>
        public event Action<List<byte>> ContentReceived;

        #region 属性
        /// <summary>
        /// 组合字节
        /// </summary>
        ByteSequence ByteSequence { get; set; }
        /// <summary>
        /// 上次接收剩余
        /// </summary>
        List<Byte> PrevRemainedBytes { get; set; }
        /// <summary>
        /// 当前帧
        /// </summary>
        public Rtcm3Frame CurrentFrame { get; set; }
        /// <summary>
        /// 封装了下个数据处理需求
        /// </summary>
        public DataAquirement DataAquirement { get; set; }
        /// <summary>
        /// 报文头部标识，11010011
        /// </summary>
        public const byte PREAMBLE_VALUE = 0XD3;
        /// <summary>
        /// 反转后的报文头部标识，11001011
        /// </summary>
        public const byte REVERSE_PREAMBLE_VALUE = 0XCB;
        #endregion

        /// <summary>
        /// 接收数据小块,进行解码和检查工作
        /// </summary>
        /// <param name="inBytes"></param>
        public void Receive(byte[] inBytes)
        {
            bool isReverse = false;
            byte preamble = PREAMBLE_VALUE;
            if (isReverse)//这句话的作用是啥？
            {
                inBytes = ByteSequence.Inverse(inBytes);
                preamble = REVERSE_PREAMBLE_VALUE;
            }

            //1）转换成二进制字符串,该数据在各个过程中将被改变 
            ByteSequence.EnQuence(inBytes);
            //拼接上一过程留下的数据
            InsertAndEmpytyPreRemainedData();

            while (this.DataAquirement.ByteCount <= ByteSequence.Count)
            {
                var InputBytes = ByteSequence.DeQueue(this.DataAquirement.ByteCount);

                switch (this.DataAquirement.PositionState)
                {
                    case DataPositionState.FindingPreamble:
                        int index = -1;
                        for (int i = 0; i < InputBytes.Count; i++)
                        {
                            if (InputBytes[i] == preamble)
                            {
                                index = i;
                                break;
                            }
                        }
                        //1.1 没有找到，则返回后7位作为下一字符的开头，继续搜索
                        if (index == -1)
                        {
                            this.PrevRemainedBytes = null;
                        }
                        //1.2 找到了，则解析，并转入下一个过程
                        else
                        {
                            this.CurrentFrame = new Rtcm3Frame(preamble);  //说明是新开始的头部 

                            //如果后继字节不够，则重新来过
                            if (InputBytes.Count < index + 3)
                            {
                                //处理完一个帧啦，下一步进行第二帧处理
                                this.PrevRemainedBytes = InputBytes.GetRange(index, InputBytes.Count - index);
                                this.DataAquirement.PositionState = DataPositionState.FindingPreamble;
                                this.DataAquirement.ByteCount = 30;//一次预读取字符数量 bre
                                break;
                            }
                            ProcessPresamble(InputBytes, index);
                        }
                        break;
                    case DataPositionState.ParsingContentData://5）已经找到了报文头部，接下来进行处理
                        this.CurrentFrame.AddContent(InputBytes);
                        this.DataAquirement.PositionState = Rtcm.DataPositionState.CheckingCrc;
                        this.DataAquirement.ByteCount = 3;//一次预读取字符数量 
                        break;
                    case DataPositionState.CheckingCrc:
                        CheckCrs(isReverse, InputBytes);
                        break;
                    default: break;
                }

                InsertAndEmpytyPreRemainedData();
            }
        }
        #region 处理细节
        /// <summary>
        /// 处理报文头部
        /// </summary>
        /// <param name="InputBytes"></param>
        /// <param name="index"></param>
        private void ProcessPresamble(List<byte> InputBytes, int index)
        {
            int nextIndex = index + 1;
            var nexBytes = InputBytes.GetRange(nextIndex, InputBytes.Count - nextIndex);

            //-----6位保留字，      //如果不足8位则补充
            var bit = Convert.ToString(nexBytes[0], 2);
            while (bit.Length < 8)
            {
                bit = "0" + bit;
            }
            var Reserved = bit.Substring(0, 6);
            this.CurrentFrame.Reserved = Convert.ToByte(Reserved, 2);

            //------接下来是10位表示的内容的字节数量。
            //10位数量，2位来自于前面
            string nextTopTwo = bit.Substring(6, 2);
            //如果不足8位则补充
            var tailEight = Convert.ToString(nexBytes[1], 2);
            while (tailEight.Length < 8)
            {
                tailEight = "0" + tailEight;
            }
            var MessageLengthBinStr = nextTopTwo + tailEight;
            this.CurrentFrame.MessageLength = Convert.ToInt32(MessageLengthBinStr, 2);

            this.CurrentFrame.AddContent(nexBytes.GetRange(0, 2));
            var nextIndex2 = 2;
            this.PrevRemainedBytes = nexBytes.GetRange(nextIndex2, nexBytes.Count - nextIndex2);
            //------下一个步骤是数据读取
            this.DataAquirement.PositionState = Rtcm.DataPositionState.ParsingContentData;
            this.DataAquirement.ByteCount = this.CurrentFrame.MessageLength;
        }
        /// <summary>
        /// 检查CRS
        /// </summary>
        /// <param name="isReverse"></param>
        /// <param name="InputBytes"></param>
        private void CheckCrs(bool isReverse, List<byte> InputBytes)
        {
            //􀝃􀯜 = 1 for i = 0, 1, 3, 4, 5, 6, 7, 10, 11, 14, 17, 18, 23, 24
            //binnary : 1100001100100110011111011 = 0x1864CFB , 0x864CFB
            //reverse  : 1101111100110010011000011 = 0x1BE64C3 , 0xBE64C3
            int crcCode = (InputBytes[0] << 16) + (InputBytes[1] << 8) + InputBytes[2];
            this.CurrentFrame.CrcCode = crcCode;

            byte[] data = this.CurrentFrame.Data.ToArray();

            var list = new List<byte>(data);
            // colName.Reverse();
            //satData = colName.GetRange(colName.Count -10, 10).ToArray();


            uint genPoly = 0x864CFB;
            uint rawGenPoly = 0x1864CFB;
            if (isReverse)
            {
                genPoly = 0xBE64C3;
                rawGenPoly = 0x1BE64C3;
            }
            // uint genPoly = 0x8005;
            int crcCount = 24;

            var genCrc0 = new CrcSovler(genPoly, crcCount).GetCrc(data);
            var genCrc1 = CrcUtil.CRC24(data, genPoly);
            var genCrc2 = ByteCrcGenerator.GetCrcByteByByte(data, data.Length * 8, rawGenPoly, crcCount);
            var genCrc3 = new CrcGeneratror((int)genPoly, crcCount).GetCrc(data);

            bool checkingResult = (crcCode == genCrc3);

            //如果检验失败，则归还部分数据，继续搜索
            if (!checkingResult)
            {
                var remained = this.CurrentFrame.Content;
                this.PrevRemainedBytes = remained;
            }
            else//OK
            {
                int a;//是不是不需要这个变量？这个函数中没有使用该变量
                //throw new Exception("天啊！终于成功了！");
                if (ContentReceived != null) { ContentReceived(this.CurrentFrame.Messages); }                
            }

            //处理完一个帧啦，下一步进行第二帧处理
            this.DataAquirement.PositionState = DataPositionState.FindingPreamble;
            this.DataAquirement.ByteCount = 30;//一次预读取字符数量 
        }
        /// <summary>
        /// 将上一次留下的字符串插入到库中。
        /// </summary>
        private void InsertAndEmpytyPreRemainedData()
        {
            if (PrevRemainedBytes != null && PrevRemainedBytes.Count > 0)
            {
                //插入到队列最前端
                this.ByteSequence.Insert(0, PrevRemainedBytes.ToArray());
                PrevRemainedBytes = null;
            }
        }
        #endregion
    }
}
