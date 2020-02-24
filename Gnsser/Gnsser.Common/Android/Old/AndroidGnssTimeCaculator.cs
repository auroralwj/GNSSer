
//2017.08.02, czs, create in hongqing, AndroidGnssTimeCaculator

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Geo;
using Geo.Times;
using Gnsser.Times;
using System.IO;


namespace Gnsser
{

/**
 * 单位默认为秒，若非，则以单位尾注，如Nanos，以GPS时间作为转换基准。
 */
public class AndroidGnssTimeCaculator {
 public static int LeapSecondFromGpsT=18;
    public static  int SecondsOfWeek = 604800;
    public static  long E9Long = 1000000000L;
    public static  double E9Double =  1000000000.0D;
    public static  int LIGHT_SPEED = 299792458;
    public static  long NanoSecondsOfWeek = SecondsOfWeek * E9Long;
    public static  int SecondsOfDay = 86400;
    public static  long NanoSecondsOfDay = SecondsOfDay* E9Long;

    /** 周期，单位秒*/
    int mPeriod;
    public SatelliteType mSatType;
    /** 卫星系统起始时间与UTC偏差，单位秒，采用加号改正系统时间*/
    double mOffsetToGpsT = 0;
    /** 接收机开始测量的本地时间，归算到GPST, 单位纳秒*/
    private LongFraction localGpsTimeNaro;

    /**
     * GNSS时间计算构造函数，
     * @param TimeNanos 当前系统时间，计时器，守时器，通常从开机0开始
     * @param mFullBiasNanos 当前时间与GPS起始时间之差
     */
    public AndroidGnssTimeCaculator( long TimeNanos, LongFraction mFullBiasNanos) :  this(new LongFraction(TimeNanos).minus( mFullBiasNanos) ) {
        // local estimate of GPS time = TimeNanos - (FullBiasNanos + BiasNanos) 
    }

    /**
     * GNSS时间计算构造函数
     * @param localGpsTimeNaro 接收机开始测量的本地时间，归算到GPST, 单位纳秒
     */
    public AndroidGnssTimeCaculator(LongFraction localGpsTimeNaros) {
        //if (localGpsTimeNaro == null)
            localGpsTimeNaro = localGpsTimeNaros;
        //else localGpsTimeNaro.Fraction = localGpsTimeNaros.Fraction;
    }

    /**
     * 设置卫星系统类型，决定时间起始，周期和偏移量。
     * @param mSatType
     */
    public void setSatelliteType(SatelliteType mSatType) {
        if( this.mSatType != mSatType) {
            this.mSatType = mSatType;
            this.mPeriod = SecondsOfWeek;
            if (mSatType == SatelliteType.R) {
                this.mPeriod = SecondsOfDay;
                this.mOffsetToGpsT = -3600 * 3; //UTC 起始点差提取了3小时
                this.mOffsetToGpsT += LeapSecondFromGpsT;//归算到GPST
            }
        }
    }

    /**
     * 计算具体卫星的测量时间。measurement time = TimeNanos + TimeOffsetNanos
     * @param timeOffsetNanos 当前卫星的测量时刻相相对于历元时刻的偏差
     * @return
     */
    public  LongFraction getMeasurementGpsTimeNanos(double timeOffsetNanos){
        return this.getLocalGpsTimeNaro().plus(timeOffsetNanos);
    }

    /**
     * 计算时间差
     * @param timeOffsetNanos 当前卫星的测量时刻相相对于历元时刻的偏差
     * @param receivedSvTimeNanos 测量时刻卫星的时间，为改卫星系统时间，接下来需要转换为GPS时间
     * @return
     */
    public  double getTimeDiffer(double timeOffsetNanos, double receivedSvTimeNanos){
        double receivedSvGpsTimeNanos = receivedSvTimeNanos + mOffsetToGpsT * E9Double;//转换为GPS时间
        double valSeconds = this.getMeasurementGpsTimeNanos(timeOffsetNanos).minus(receivedSvGpsTimeNanos).getValue() / E9Double;
        return Geo.Utils. DoubleUtil.RollTo(valSeconds, mPeriod);
    }

    /// <summary>
    /// 本地接收机GPS时间
    /// </summary>
    /// <returns></returns>
    public LongFraction getLocalGpsTimeNaro() {
        return localGpsTimeNaro;
    }

    public void setLocalGpsTimeNaro(LongFraction localGpsTimeNaros) {
        localGpsTimeNaro = localGpsTimeNaros;
    }

}


/**
 * 2017/7/31,czs, create in hongqing, new class.
 */

public class LongFraction {
    public long Long;
    public double Fraction;

    public LongFraction(long aLong, double fraction) {
        Long = aLong;
        Fraction = fraction;
    }
    public LongFraction(double fraction) {
        Fraction = fraction;
    }
    public LongFraction(long aLong) {
        Long = aLong;
    }

    public  LongFraction plus(LongFraction other){
        return new LongFraction(this.Long + other.Long, this.Fraction + other.Fraction);
    }
    public  LongFraction plus(long other){
        return new LongFraction(this.Long + other, this.Fraction);
    }
    public  LongFraction plus(double other){
        return new LongFraction(this.Long, this.Fraction +  other);
    }
    public  LongFraction minus(LongFraction other){
        return new LongFraction(this.Long - other.Long, this.Fraction - other.Fraction);
    }
    public  LongFraction minus(long other){
        return new LongFraction(this.Long - other, this.Fraction);
    }
    public  LongFraction minus(double other){
        return new LongFraction(this.Long, this.Fraction - other);
    }

    public  double getValue(){
        return this.Long + this.Fraction;
    }
	 
    public override String ToString() {
        return "LongFraction{" +
                "Long=" + Long +
                ", Fraction=" + Fraction +
                '}';
    }
	  
}

}