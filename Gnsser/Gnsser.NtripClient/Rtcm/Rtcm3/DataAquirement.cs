//2015.02.04, czs, create in pengzhou, 处理 RTCM 3.X 版本的数据读取。

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Geo.Utils;

namespace Gnsser.Ntrip.Rtcm
{
    /// <summary>
    /// 读取器处理数据状态
    /// </summary>
    public enum DataPositionState
    {
        /// <summary>
        /// 有待搜索报文头部
        /// </summary>
        FindingPreamble,//finds      
        /// <summary>
        /// 内容数据不足
        /// </summary>
        ParsingContentData,//parse
        /// <summary>
        /// 等待校验码
        /// </summary>
        CheckingCrc,//check
    }
    
    /// <summary>
    /// 下个数据处理需求。
    /// </summary>
    public class DataAquirement
    {
        /// <summary>
        /// 构造函数。
        /// </summary>
        /// <param name="ByteCount">需要读取的字节数量</param>
        /// <param name="PositionState">位置状态</param>
        public DataAquirement(int ByteCount, Rtcm.DataPositionState PositionState)
        {
            this.ByteCount = ByteCount;
            this.PositionState = PositionState;
        } 
        /// <summary>
        /// 一次预读取字符数量 ，满足后才激发。
        /// </summary>
        public int ByteCount { get; set; }
        /// <summary>
        /// 下一个处理状态
        /// </summary>
        public Rtcm.DataPositionState PositionState { get; set; }

        public override string ToString()
        {
            return PositionState.ToString() + ByteCount;
        }

        public override bool Equals(object obj)
        {
            var other = obj as DataAquirement;
            if (other == null) return false;


            return other.PositionState == this.PositionState;
        }

        public override int GetHashCode()
        {
            return PositionState.GetHashCode();
        }
    }
}
