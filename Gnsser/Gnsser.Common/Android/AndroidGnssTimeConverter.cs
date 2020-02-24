
//2017.08.02, czs, create in hongqing, AndroidGnssTimeCaculator

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Geo;
using Geo.Times;
using Gnsser.Times;
using Geo.IO;
using System.IO;


namespace Gnsser
{
    /// <summary>
    /// 对整型数据分解，避免出错
    /// </summary>
    public class LongPaser
    {
        public LongPaser(long  lon)
        {
            this.Value = lon;
            this.IsPositive = lon > 0;
        }

        /// <summary>
        /// 长整型
        /// </summary>
        public long Value { get; set; }
        /// <summary>
        /// 是否为正
        /// </summary>
        public bool IsPositive { get;set; }
        /// <summary>
        /// 字符串
        /// </summary>
        public string ValueString { get { return Value.ToString(); } }
        /// <summary>
        /// 长度
        /// </summary>
        public int Length { get { return ValueString.Length; } }

        /// <summary>
        /// 获取双精度浮点数。
        /// </summary>
        /// <param name="fractionPosition"></param>
        /// <returns></returns>
        public double GetDouble(int fractionPosition)
        {
            var tail = GetTail(fractionPosition);

            var header = ( GetHeader(fractionPosition));
           
            double val = header + tail * Math.Pow(10, -fractionPosition);
           
            return val;
        }

        /// <summary>
        /// 获取末尾
        /// </summary>
        /// <param name="count"></param>
        /// <returns></returns>
        public long GetTail(int count)
        {
            var val = long.Parse(ValueString.Substring(Length - count, count));
            if (!IsPositive)
            {
                val = -val;
            }
            return val;
        }

        /// <summary>
        /// 获取末尾
        /// </summary>
        /// <param name="tailCount"></param>
        /// <returns></returns>
        public long GetHeader(int tailCount)
        {
            return long.Parse(ValueString.Substring(0, Length - tailCount));
        }
     
        /// <summary>
        /// 截断
        /// </summary>
        /// <param name="startIndex"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        public long GetSub(int startIndex, int count)
        {
            return long.Parse(ValueString.Substring(startIndex, count));
        }
    }


    /**
     * 单位默认为秒，若非，则以单位尾注，如Nanos，以GPS时间作为转换基准。
     */
    public class AndroidGnssTimeConverter
    { 
        public const int SecondsOfWeek = 604800;
        public const long E9Long = 1000000000L;
        public const double E9Double = 1000000000.0D;
        public const int LIGHT_SPEED = 299792458;
        public const long NanoSecondsOfWeek = SecondsOfWeek * E9Long;
        public const int SecondsOfDay = 86400;
        public const long NanoSecondsOfDay = SecondsOfDay * E9Long;

        /// <summary>
        ///  GNSS时间系统周期，单位秒，GPS为周秒，Glonass为日秒
        /// </summary>
        int GnssPeriod { get; set; }
        public SatelliteType SatelliteType { get; set; }
        /// <summary>
        /// 卫星系统起始时间与GPST偏差，单位秒，采用加号改正不同GNSS系统时间
        /// </summary>
        double OffsetOfSatSysTimeToGpsT { get; set; }
        #region 原始数据
        /// <summary>
        /// 当前时间(UTC)与GPS起始时间之差（StartOfGPST - CurrentUTC），为负数,单位，纳秒
        /// </summary>
        public long CurrentFullBias { get; set; }
        /// <summary>
        /// 当前时间,递增的时间标签
        /// </summary>
        public long TimeNanos { get; set; }
        /// <summary>
        /// 时间偏差,时间偏差的次纳秒部分
        /// </summary>
        public double BiasNanos { get; set; }
        /// <summary>
        /// 时间偏离,测量时间偏差
        /// </summary>
        public double TimeOffsetNanos { get; set; }
        public AndroidGnssTimeConverter SetFullBias(long fullBiasNanos) { this.CurrentFullBias = fullBiasNanos; return this; }

        public AndroidGnssTimeConverter SetTimeNanos(long TimeNanos) { this.TimeNanos = TimeNanos; return this; }

        public AndroidGnssTimeConverter SeBiasNanos(double BiasNanos) { this.BiasNanos = BiasNanos; return this; }

        public AndroidGnssTimeConverter SetTimeOffsetNanos(double TimeOffsetNanos) { this.TimeOffsetNanos = TimeOffsetNanos; return this; }

        #endregion
        /// <summary>
        /// 历元是否有效。
        /// </summary>
        public bool IsEpochValid { get; set; }
        /// <summary>
        /// 第一次输入。
        /// </summary>
        public long FirstFullBias { get; set; }
        /// <summary>
        /// 当前时间GPS周数量
        /// </summary>
        public int GpsWeekNumber { get; set; }
        /// <summary>
        /// 纳秒表示的GPS周数量
        /// </summary>
        long GpsWeekNumberNanos { get; set; }
        /// <summary>
        /// 第一次记录时间,UTC。
        /// </summary>
        public Time FirstEpochOfUtc { get; set; }
        /// <summary>
        /// 第一次记录时间,GPST。
        /// </summary>
        public Time FirstEpochOfGpst { get; set; }
        /// <summary>
        /// 本GPS周起始时间。由第一次输入的时间偏差决定。
        /// </summary>
        public Time GpsWeekStartTimeUtc { get; set; }
        /// <summary>
        /// 是否从第一个历元开始记录时间。
        /// </summary>
        public bool IsFromFirstEpoch { get; set; }
        
        Log log = new Log(typeof(AndroidGnssTimeConverter));

        /// <summary>
        ///   GNSS时间计算构造函数，
        /// </summary>
        /// <param name="fullBiasBetweenUtcToStartOfGpstNanos">当前时间(UTC)与GPS起始时间之差（StartOfGPST - CurrentUTC），为负数,单位纳秒，第一次观测记录时间。</param>
        public AndroidGnssTimeConverter(long fullBiasBetweenUtcToStartOfGpstNanos, bool IsFromFirstEpoch)
        {
            this.IsFromFirstEpoch = IsFromFirstEpoch;
            this.FirstFullBias = fullBiasBetweenUtcToStartOfGpstNanos;
            this.GpsWeekNumber = (int)Math.Floor(-fullBiasBetweenUtcToStartOfGpstNanos / E9Double / SecondsOfWeek);
            this.GpsWeekNumberNanos = this.GpsWeekNumber * SecondsOfWeek * E9Long;
            this.GpsWeekStartTimeUtc = Time.StartOfGpsT + GpsWeekNumber * SecondsOfWeek;
            double fullBiasSeconds = - (fullBiasBetweenUtcToStartOfGpstNanos * 1E-9);
          

            LongPaser LongPaser = new LongPaser(fullBiasBetweenUtcToStartOfGpstNanos);
            var second =  -1.0 * LongPaser.GetDouble(9);
            var integ = -1 * LongPaser.GetHeader(9);
            var fraction = -1.0 * LongPaser.GetTail(9) * 1E-9;

            var secTime = new SecondTime(integ, fraction);
            var newTime = Time.StartOfGpsT.TickTime + secTime;
            var newGeoTime = new Time(newTime);

            var time = Time.StartOfGpsT + fullBiasSeconds;
            this.FirstEpochOfUtc = time + LeapSecond.Instance.GetLeapSecondBetween(GpsWeekStartTimeUtc.DateTime, time.DateTime);//已经包含跳秒，实质为UTC时间，如果该周内无闰秒的话。
            // local estimate of GPS time = TimeNanos - (FullBiasNanos + BiasNanos) 
            this.FirstEpochOfGpst = FirstEpochOfUtc - LeapSecond.Instance.GetLeapSecondFromGpsT(FirstEpochOfUtc.DateTime);

            this.IsEpochValid = true;
            //时间检核
            if (FirstEpochOfUtc < Time.StartOfBdT || FirstEpochOfUtc > Time.UtcNow)
            {
                log.Error("时间可能错误，忽略本历元，可以采用逐历元计算尝试 " + FirstEpochOfUtc);
                this.IsEpochValid = false;
            }
        }
      

        /// <summary>
        /// 置卫星系统类型，决定时间起始，周期和偏移量。
        /// </summary>
        /// <param name="mSatType"></param>
        public void SetSatelliteType(SatelliteType mSatType)
        {
            if (this.SatelliteType != mSatType)
            {
                this.SatelliteType = mSatType;
                switch (mSatType)
                {
                    case SatelliteType.G:
                        this.GnssPeriod = SecondsOfWeek;
                        this.OffsetOfSatSysTimeToGpsT = 0;
                        break;
                    case SatelliteType.R:
                        this.GnssPeriod = SecondsOfDay;
                        this.OffsetOfSatSysTimeToGpsT = -3600 * 3; //UTC 起始点差提取了3小时
                        this.OffsetOfSatSysTimeToGpsT += LeapSecond.Instance.GetLeapSecondFromGpsT(FirstEpochOfUtc.DateTime); //归算到UTC
                        break;
                    case SatelliteType.C:
                        this.GnssPeriod = SecondsOfWeek;
                        this.OffsetOfSatSysTimeToGpsT = LeapSecond.Instance.GetLeapSecondFromGpsT(FirstEpochOfUtc.DateTime);// - LeapSecond.BeTweenBdTAndGpsT;//归算到GPST
                        break;
                    default:
                        this.GnssPeriod = SecondsOfWeek;
                        this.OffsetOfSatSysTimeToGpsT = 0;
                        break;
                }
            }
                
        }
        /// <summary>
        /// 获取与起算时间偏差的距离偏差。
        /// </summary>
        /// <returns></returns>
        public double GetDifferDistance()
        {
            long curentDifferOfFullBiasNanos = 0L;
            if (!IsFromFirstEpoch) { curentDifferOfFullBiasNanos = CurrentFullBias - FirstFullBias; }
            
            double timeErrorDistance = GnssConst.LIGHT_SPEED * (curentDifferOfFullBiasNanos * 1E-9);
            return timeErrorDistance;
        }

        /// <summary>
        ///  计算信号传输时间，时间差  
        /// </summary>
        /// <param name="TimeNanos">接收机当前时间标签</param>
        /// <param name="biasNanos">时间偏差，FullBiasToGpstOfFirstEpochNanos的小数部分</param>
        /// <param name="timeOffsetNanos"> 当前卫星的测量时刻相相对于历元时刻的偏差</param>
        /// <param name="receivedSvTimeNanos">测量时刻卫星的时间，为改卫星系统时间，对于GPS，是周内秒，俄罗斯是天内秒，接下来需要转换为GPS时间 </param>
        /// <returns></returns>
        public double GetTransmissionTime(double receivedSvTimeNanos)
        {
            double receiverGpsTimeNanos = GetReceiverGpsWeekSecondNanos();             

            return GetTransmissionTime(receiverGpsTimeNanos, receivedSvTimeNanos);
        }
        /// <summary>
        /// 计算信号传输时间
        /// </summary>
        /// <param name="receivedSvTimeNanos"></param>
        /// <param name="receiverGpsTimeNanos"></param>
        /// <returns></returns>
        public double GetTransmissionTime(double receiverGpsTimeNanos, double receivedSvTimeNanos)
        {
            double receivedSvGpsTimeNanos = GetSatGpsTimeNanos(receivedSvTimeNanos);
            double differ = (receiverGpsTimeNanos - receivedSvGpsTimeNanos) / E9Double;

            double result = Geo.Utils.DoubleUtil.RollTo(differ, GnssPeriod);
            return result;
        }
        /// <summary>
        /// 获取卫星测量的GPS时间。
        /// </summary>
        /// <param name="receivedSvTimeNanos">卫星测量时间（来自于卫星，不同系统不同时间？？？）</param>
        /// <returns></returns>
        private double GetSatGpsTimeNanos(double receivedSvTimeNanos)
        {
            double receivedSvGpsTimeNanos = receivedSvTimeNanos + OffsetOfSatSysTimeToGpsT * E9Double;//转换为GPS时间纳秒时间 
            return Geo.Utils.DoubleUtil.RollTo(receivedSvGpsTimeNanos, GnssPeriod * E9Double); ;
        }
        /// <summary>
        /// 获取接收机记录的本地GPS时间（周内秒部分），是一个近似，以构造函数设置的 FullBiasBetweenUtcToStartOfGpstNanos 为基准计算。
        /// </summary>
        /// <param name="TimeNanos">递增的时间标签</param>
        /// <param name="biasNanos">时间偏差的次纳秒部分</param>
        /// <param name="timeOffsetNanos">测量时间偏差</param>
        /// <returns></returns>
        public double GetReceiverGpsTimeNanos(long TimeNanos, double biasNanos, double timeOffsetNanos)
        {
            long differWeekNanos = 0L;
            if (IsFromFirstEpoch) { differWeekNanos = FirstFullBias + GpsWeekNumberNanos; }//减去固定的GPS周时间纳秒，变成起始时间的周内纳秒，减少计算量
            else { differWeekNanos = CurrentFullBias + GpsWeekNumberNanos; }
            long localTime = TimeNanos - differWeekNanos;//再变成double，避免舍入误差
            double receiverGpsTimeNanos = localTime - biasNanos - timeOffsetNanos;
            return receiverGpsTimeNanos;
        }
        /// <summary>
        /// 获取接收机记录的本地GPS时间（周内秒部分），是一个近似，以构造函数设置的 FullBiasBetweenUtcToStartOfGpstNanos 为基准计算。
        /// </summary>
        /// <param name="TimeNanos">递增的时间标签</param>
        /// <param name="BiasNanos">时间偏差的次纳秒部分</param>
        /// <param name="TimeOffsetNanos">测量时间偏差</param>
        /// <returns></returns>
        public double GetReceiverGpsWeekSecondNanos()
        {
            long differWeekNanos = 0L;
            if (IsFromFirstEpoch) { differWeekNanos = FirstFullBias + GpsWeekNumberNanos; }//减去固定的GPS周时间纳秒，变成起始时间的周内纳秒，减少计算量
            else { differWeekNanos = CurrentFullBias + GpsWeekNumberNanos; }
            long localTime = TimeNanos - differWeekNanos;//再变成double，避免舍入误差
            double receiverGpsTimeNanos = localTime - BiasNanos - TimeOffsetNanos;
            return receiverGpsTimeNanos;
        }
        /// <summary>
        /// 获取接收机的UTC时间。
        /// </summary>
        /// <param name="TimeNanos"></param>
        /// <param name="biasNanos"></param>
        /// <param name="timeOffsetNanos"></param>
        /// <returns></returns>
        public Time GetReceiverUtcTime(long TimeNanos, double biasNanos, double timeOffsetNanos)
        {
            long localTime = TimeNanos - FirstFullBias;//先把数变小,再变成double，避免舍入误差
            double receiverGpsTimeNanos = localTime - biasNanos - timeOffsetNanos;

            return Time.StartOfGpsT + receiverGpsTimeNanos / E9Double;
        }
        /// <summary>
        /// 获取接收机的UTC时间。
        /// </summary> 
        /// <returns></returns>
        public Time GetReceiverUtcTime()
        {
            long localTime = TimeNanos - FirstFullBias;//先把数变小,再变成double，避免舍入误差
            if (IsFromFirstEpoch) { localTime = TimeNanos - FirstFullBias; }//减去固定的GPS周时间纳秒，变成起始时间的周内纳秒，减少计算量
            else { localTime = TimeNanos - CurrentFullBias; }


            double receiverGpsTimeNanos = localTime - BiasNanos - TimeOffsetNanos;

            return Time.StartOfGpsT + receiverGpsTimeNanos / E9Double;
        }

        public Time GetGpsTime(long fullBiasNanos)
        { 
            return Time.StartOfGpsT + fullBiasNanos / E9Double;
        }
        /// <summary>
        /// 获取接收机的UTC时间。
        /// </summary>
        /// <param name="TimeNanos"></param>
        /// <param name="biasNanos"></param>
        /// <param name="timeOffsetNanos"></param>
        /// <returns></returns>
        public Time GetReceiverGpsTime(long TimeNanos, double biasNanos, double timeOffsetNanos, long FullBiasBetweenUtcToStartOfGpstNanos)
        {
            long localTime = TimeNanos - FullBiasBetweenUtcToStartOfGpstNanos;//先把数变小,再变成double，避免舍入误差
            double receiverGpsTimeNanos = localTime - biasNanos - timeOffsetNanos;
            return Time.StartOfGpsT + receiverGpsTimeNanos / E9Double;
        }
        /// <summary>
        /// 获取接收机的UTC时间。
        /// </summary>
        /// <param name="TimeNanos"></param>
        /// <param name="biasNanos"></param>
        /// <param name="timeOffsetNanos"></param>
        /// <param name="FullBiasBetweenUtcToStartOfGpstNanos">当前UTC时间到GPST起始偏差，纳秒</param>
        /// <returns></returns>
        public Time GetReceiverUtcTime(long TimeNanos, double biasNanos, double timeOffsetNanos, long FullBiasBetweenUtcToStartOfGpstNanos)
        {
            long localTime = TimeNanos - FullBiasBetweenUtcToStartOfGpstNanos;//先把数变小,再变成double，避免舍入误差
            double receiverGpsTimeNanos = localTime - biasNanos - timeOffsetNanos;
            return Time.StartOfGpsT + receiverGpsTimeNanos / E9Double;
        }
        /// <summary>
        /// 获取接收机的UTC时间。
        /// </summary>
        /// <param name="TimeNanos"></param>
        /// <param name="biasNanos"></param>
        /// <param name="timeOffsetNanos"></param>
        /// <param name="FullBiasToStartOfGpstNanos">当前GPS时间到GPST起始的偏差，纳秒</param>
        /// <returns></returns>
        public Time GetReceiverUtcTimeFromGpst(long TimeNanos, double biasNanos, double timeOffsetNanos, long FullBiasToStartOfGpstNanos)
        {
            var gpsTime = GetReceiverGpsTime(TimeNanos, biasNanos, timeOffsetNanos, FullBiasToStartOfGpstNanos);
            Time utcTime = gpsTime - LeapSecond.Instance.GetLeapSecondFromGpsT(gpsTime.DateTime);//可能在跳秒时刻差几秒
            if (utcTime.Hour == 0 && utcTime.Day == 1)//如果GPS时间为2017.01.01.0:0:0，则计算差为27秒，实际应该26s，会多计算一秒，此时需要重新计算一次。
            {
                utcTime = gpsTime - LeapSecond.Instance.GetLeapSecondFromGpsT(utcTime.DateTime);
            }
            return utcTime;
        }

    }

}