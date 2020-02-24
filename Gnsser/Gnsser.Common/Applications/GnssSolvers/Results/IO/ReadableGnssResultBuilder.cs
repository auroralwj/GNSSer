//2018.08.03, czs, create in hmx, 可读的 GNSS 定位文本信息生成器
//2018.11.12, czs, edit in hmx, 精简提取

using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading;
using Gnsser;
using Gnsser.Domain;
using Gnsser.Service; 
using Gnsser.Data.Rinex;
using Geo.Utils;
using Geo.Coordinates;
using Geo.Referencing;
using Geo.Algorithm.Adjust;
using Geo.Common;
using Geo;
using Geo.Algorithm;
using Geo.IO;

namespace Gnsser
{
    /// <summary>
    /// 可读的 GNSS 定位文本信息生成器
    /// </summary>
    public class ReadableGnssResultBuilder : AbstractBuilder<string, SimpleGnssResult>
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public ReadableGnssResultBuilder()
        {

        }

        SimpleGnssResult CurrentGnssResult { get; set; }

        /// <summary>
        /// 生成信息
        /// </summary>
        /// <returns></returns>
        public override string Build(SimpleGnssResult lastResult)
        {
            this.CurrentGnssResult = lastResult;

            StringBuilder sb = new StringBuilder(); 
            sb.AppendLine(lastResult.Name + "\t历元：\t" + lastResult.ReceiverTime);

            if (lastResult is IReadable)
            {
                sb.AppendLine((lastResult as IReadable).ToReadableText());
            }
            else if (lastResult is IWithEstimatedBaseline)
            {
                var differResult = lastResult as IWithEstimatedBaseline;
                sb.AppendLine(differResult.GetEstimatedBaseline().ToReadableText());
            }
            else  if (lastResult is IWithEstimatedBaselines)
            {
                var differResult = lastResult as IWithEstimatedBaselines;
                sb.AppendLine(differResult.GetEstimatedBaselines().ToReadableText());
            }

            if (lastResult.ParamAccuracyInfos != null)
            {
                sb.AppendLine("参数收敛时间与精度估计：");
                sb.AppendLine(lastResult.ParamAccuracyInfos.ToReadableText());
            }

            return sb.ToString();
        }
    }
}
