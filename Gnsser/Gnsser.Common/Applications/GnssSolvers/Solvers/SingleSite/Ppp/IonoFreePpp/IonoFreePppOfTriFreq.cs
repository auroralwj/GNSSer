//2016.10.14 double create in hongqing,基于IonoFreePpp

using System;
using System.Collections.Generic;
using System.Text;
using Geo.Algorithm;
using Geo;
using Geo.Utils;
using Geo.Common;
using Gnsser.Domain;
using Geo.Algorithm.Adjust;
using Geo.Coordinates;
using Gnsser.Data.Rinex;
using Gnsser.Service;
using Gnsser.Checkers;
using Geo.Algorithm;
using Geo.Times;
using Gnsser.Filter;

namespace Gnsser.Service
{
    //包含4类参数，测站位置（x,y,z），钟差（Cdt），对流程天顶距延迟(zpd)和以m为单位的模糊度（N）。

    /// PPP 计算核心方法。 Kalmam滤波。
    /// 观测量的顺序是先伪距观测量，后载波观测量，观测量的总数为卫星数量的两倍。
    /// 参数数量为卫星数量加5,卫星数量对应各个模糊度，5为3个位置量xyz，1个接收机钟差量，1个对流程湿分量。
    /// <summary>
    /// 精密单点定位。
    /// 此处采用观测值残差向量计算。
    /// 条件：必须是三频数据，且观测卫星数量大于5个。
    /// 参考：Jan Kouba and Pierre Héroux. GPS Precise Point Positioning Using IGS Orbit Products[J].2000,sep
    /// </summary>
    public class IonoFreePppOfTriFreq : SingleSiteGnssSolver
    {
        #region 构造函数
     
        /// <summary>
        /// 最简化的构造函数，可以多个定位器同时使用的参数，而不必多次读取
        /// </summary>
        /// <param name="DataSourceContext"></param>
        /// <param name="PositionOption"></param>
        public IonoFreePppOfTriFreq(DataSourceContext DataSourceContext, GnssProcessOption PositionOption)
            : base(DataSourceContext, PositionOption)
        {
            this.Name = "三频无电离层组合PPP";
            this.BaseParamCount = 5;
            this.MatrixBuilder = new IonoFreePppOfTriFreqMatrixBuilder(PositionOption);
        }
        #endregion

        /// <summary>
        /// 生成结果
        /// </summary>
        /// <returns></returns>
        public override SingleSiteGnssResult BuildResult()
        {
            int a = 9;
            if (this.CurrentMaterial.ReceiverTime == Time.Parse("2013 01 01 11 36 00"))
                a = 0;
            return new PppResult(this.CurrentMaterial, Adjustment, this.MatrixBuilder.GnssParamNameBuilder);
        }
    }
}