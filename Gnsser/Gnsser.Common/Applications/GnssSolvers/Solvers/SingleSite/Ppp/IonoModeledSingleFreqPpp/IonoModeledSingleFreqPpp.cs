//2017.10.12, czs, create in hongqing, 电离层模型改正单频PPP

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

namespace Gnsser.Service
{
    //包含4类参数，测站位置（x,y,z），钟差（Cdt），对流程天顶距延迟(zpd)和非整的整周模糊度（N）。

    /// <summary>
    /// 精密单点定位。电离层模型改正单频PPP
    /// 此处采用观测值残差向量计算。
    /// 条件：必须是双频观测，且观测卫星数量大于5个。
    /// 参考：Jan Kouba and Pierre Héroux. GPS Precise Point Positioning Using IGS Orbit Products[J].2000,sep
    /// </summary>
    public class IonoModeledSingleFreqPpp : SingleSiteGnssSolver
    {
        #region 构造函数 
        
        /// <summary>
        /// 电离层模型改正单频PPP，可以多个定位器同时使用的参数，而不必多次读取
        /// </summary>
        /// <param name="DataSourceContext"></param>
        /// <param name="PositionOption"></param>
        public IonoModeledSingleFreqPpp(DataSourceContext DataSourceContext, GnssProcessOption PositionOption)
            : base(DataSourceContext, PositionOption)
        {
            this.Name = "电离层建模单频PPP";
            this.BaseParamCount = 6;

            this.MatrixBuilder = new IonoModeledSingleFreqPppMatrixBuilder(PositionOption);
        }

        #endregion

        /// <summary>
        /// 生成结果
        /// </summary>
        /// <returns></returns>
        public override SingleSiteGnssResult BuildResult()
        {
            //return new PppResult(this.CurrentMaterial, Adjustment, this.MatrixBuilder.GnssParamNameBuilder);
            return new IonoModeledSingleFreqPppResult(this.CurrentMaterial, Adjustment, this.MatrixBuilder.GnssParamNameBuilder);
        }
    }
}
