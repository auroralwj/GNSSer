//2014.09.14, czs, create, 观测量，Gnsser核心模型！

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Geo.Correction;
using Gnsser.Correction;
using Geo;

namespace Gnsser.Domain
{
    /// <summary>
    /// 观测量，包含载波、伪距、多普勒，观测值得组合观测值等。
    /// 观测量由观测值和其改正组数成。
    /// </summary>
    public class Observation : DetailedCorrectableNumeral, IObservation, IValueClone<Observation>, IUpdateValueWithCorrections
    {
        /// <summary>
        /// 观测量构造函数。
        /// </summary>
        /// <param name="val">数值</param> 
        /// <param name="ObservationCode"> RINEX 码类型</param>
        public Observation(double val,  ObservationCode ObservationCode)
            : base(val)
        {
            this.GnssCodeType = ObservationTypeHelper.GetGnssCodeType(ObservationCode);
            this.ObservationCode = ObservationCode;
        }

        /// <summary>
        /// GNSSer 波、码的类型。
        /// </summary>
        public GnssCodeType GnssCodeType { get; set; }
        /// <summary>
        /// RINEX 波、码的类型。
        /// </summary>
        public ObservationCode ObservationCode { get; set; }
        /// <summary>
        /// 增加改正
        /// </summary>
        /// <param name="name"></param>
        /// <param name="val"></param>
        public void AddCorrection(CorrectionNames name, double val)
        {
            Corrections.Add(name.ToString(), val);
        }
        /// <summary>
        /// 增加改正
        /// </summary>
        /// <param name="name"></param>
        /// <param name="val"></param>
        public void SetCorrection(CorrectionNames name, double val)
        {
            Corrections[name.ToString()] =  val;
        }

        /// <summary>
        /// 观测值改正名称
        /// </summary>
        public List<CorrectionNames> CorrectionNames
        {
            get
            {
                List<CorrectionNames> names = new List<CorrectionNames>();
                foreach (var item in this.Corrections)
                {
                    var enumName = (CorrectionNames)Enum.Parse(typeof(CorrectionNames), item.Key);
                    names.Add(enumName);
                }
                return names;
            }
        }

        /// <summary>
        /// 数值克隆
        /// </summary>
        /// <returns></returns>
        public virtual Observation ValueClone()
        {
            return  new Observation(this.Value, this.ObservationCode); 
        }
        /// <summary>
        /// 将改正数更新到数值，并清空改正数。
        /// </summary>
        public void UpdateValueWithCorrections()
        {
            if(this.Corrections.Count == 0) { return; }

            this.Value = CorrectedValue;
            this.ClearCorrections();
        }
    }

    /// <summary>
    /// 将改正数更新到数值，并清空改正数。
    /// </summary>
    public interface IUpdateValueWithCorrections
    {
        /// <summary>
        /// 将改正数更新到数值，并清空改正数。
        /// </summary>
        void UpdateValueWithCorrections();
    }
}