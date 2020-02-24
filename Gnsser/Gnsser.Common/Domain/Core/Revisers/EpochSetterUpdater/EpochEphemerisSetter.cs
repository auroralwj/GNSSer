//2014.09.15, czs, create, 设置卫星星历。
//2014.10.12, czs, edit in hailutu, 对星历赋值进行了重新设计，分解为几个不同的子算法
//2015.02.08, 崔阳, 卫星钟差和精密星历若同时存在，则不可分割
//2017.08.06, czs, edit in hongqing, 代码整理，面向对象重构
//2018.06.11, lly, edit in zz, 去掉 SatAntOffEphemerisReviser

using System;
using System.IO;
using System.Collections.Generic;
using Gnsser.Domain;
using System.Text;
using Geo.Coordinates;
using Gnsser.Data.Rinex;
using Geo.Algorithm;
using Geo.Algorithm.Adjust;
using Gnsser.Service;
using Geo.Utils;
using Gnsser.Checkers;
using Geo.Common;
using Gnsser.Times;
using Gnsser.Data;
using Geo.Times;
using Gnsser.Correction;

namespace Gnsser
{
    /// <summary>
    /// 设置卫星星历。
    /// 请在本类前，先执行观测值的有效性检查与过滤。
    /// 在本类后，执行星历检核与过滤。
    /// 为了获得精准的卫星位置，需要精准的接收机时间，即需要计算出接收机钟差。 
    /// </summary>
    public class EpochEphemerisSetter : EpochInfoReviser
    {

        /// <summary>
        /// 构造函数。设置卫星星历和钟差，请在本类前执行 观测值的有效性检查与过滤。
        /// 
        /// </summary>
        public EpochEphemerisSetter(DataSourceContext DataSourceContext, GnssProcessOption Option)
        {
            this.Context = DataSourceContext;
            this.EphemerisService = DataSourceContext.EphemerisService;// EphemerisService;
            this.SecondEphemerisService = DataSourceContext.SecondEphemerisService;
            this.ClockDataSource = DataSourceContext.SimpleClockService;// SimpleClockService;
            this.Option = Option;
            this.Name = "星历赋值器";
        }
        /// <summary>
        /// 选项
        /// </summary>
        public GnssProcessOption Option { get; set; }
        /// <summary>
        /// 星历数据源
        /// </summary>
        public IEphemerisService EphemerisService { get; set; }
        public IEphemerisService SecondEphemerisService { get; set; }
        /// <summary>
        /// 上下文资源
        /// </summary>
        public DataSourceContext Context { get; set; }

        /// <summary>
        /// 钟差数据源。
        /// </summary>
        public ISimpleClockService ClockDataSource { get; set; }
        List<IEphemerisReviser> EphemerisProcessors { get; set; }
        //避免重复通知，只通知一次
        bool IsNoticedClockService = false;
        bool IsNoticedCoordZero = false;

        /// <summary>
        /// 星历矫正
        /// </summary>
        /// <param name="info"></param>
        /// <returns></returns>
        public override bool Revise(ref EpochInformation info)
        {
            BuildEphemerisReviser(info);

            foreach (var sat in info)
            { 






                EmissionEphemerisRolver solver = null;
                //if (info.Time.Correction == 0 
                //    || info.SiteInfo.EstimatedXyzRms.Length > 100 
                //    || Math.Abs(info.Time.Correction) > 86400) //没有平差，或坐标初值不准确的情况下,直接采用伪距估算卫星发射时刻
                {
                    solver = new EmissionEphemerisRolverWithRange(EphemerisService, Context, sat);
                }
                //else //坐标初值已经算出，采用精确坐标计算卫星发射时刻
                //{
                //    solver = new EmissionEphemerisRolverWithCoord(service, DataSouceProvider, sat);
                //}
                 
                IEphemeris eph = solver.Get();//第一次获取
                if (eph == null)
                { //采用第二星历服务
                    if (SecondEphemerisService != null)
                    {
                        solver = new EmissionEphemerisRolverWithRange(SecondEphemerisService, Context, sat);
                        eph = solver.Get();//第2次获取
                    }
                    if (eph == null)
                    {
                        continue;
                    }
                }
                //查看差多少，如果少于1cm，则极速取消，2018.10.06， hmx， czs
                eph = Revise(eph); //测试校正，差了100多米啊！！！
                sat.Ephemeris = eph; //第一次赋值
                

                //update and once more //第二次计算
                eph =  solver.Get();
                if (eph == null)
                {
                    continue;
                }
                eph = Revise(eph); 
                if (false)//测试校正，差了0.1m ！！！
                {
                    var diff = eph.XYZ - sat.Ephemeris.XYZ;
                    int i = 0;
                }
                sat.Ephemeris = eph;

                //eph = solver.Get();//第3次计算
                //if (eph == null) continue;
                //eph = Revise(eph); 

                //if (true)//测试校正，差了0.0m ！！！
                //{
                //    var diff = eph.XYZ - sat.Ephemeris.XYZ;
                //    int i = 0;
                //}
            }
            return true;
        }

        /// <summary>
        /// 只对星历进行矫正，不再赋值
        /// </summary>
        /// <param name="info"></param>
        public void ReviseEphemerisOnly(EpochInformation info)
        {
            BuildEphemerisReviser(info);

            foreach (var sat in info)
            {
                Revise(sat.Ephemeris);
            }
        }

        private void BuildEphemerisReviser(EpochInformation info)
        {
            XYZ receiverPos = info.SiteInfo.EstimatedXyz;
            //星历修正器
            this.EphemerisProcessors = new List<IEphemerisReviser>();
            if (Context.HasClockService) { this.EphemerisProcessors.Add(new ClockEphemerisReviser(Context.SimpleClockService)); }//SimpleClockService
            else if (!IsNoticedClockService) { log.Warn("没有钟差服务！无法对星历进行二次矫正。"); IsNoticedClockService = true; }
              //修改到xy方向上
            if ( receiverPos != null && !receiverPos.IsZero) { this.EphemerisProcessors.Add(new EarthSagnacEphemerisReviser(receiverPos)); }  //地球自转改正，
            else if (!IsNoticedCoordZero) { log.Warn("接收机坐标为 0，无法进行地球自转改正！无法对星历进行二次矫正。"); IsNoticedCoordZero = true; }

            this.EphemerisProcessors.Add(new RelativeEphemerisReviser());    // 计算相对论改正，添加到钟上
        }

        public IEphemeris Revise(IEphemeris eph)
        {
            foreach (var item in EphemerisProcessors)
            {
                item.Revise(ref eph);
            }
            return eph;
        }
    }
      

}
