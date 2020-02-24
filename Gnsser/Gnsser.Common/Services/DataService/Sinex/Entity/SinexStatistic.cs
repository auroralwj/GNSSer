using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Gnsser.Data.Sinex
{

    public class StatisticLabel
    {
        public static string CODE_MEASUREMENTS_SIGMA = "CODE MEASUREMENTS SIGMA";
        public static string NUMBER_OF_DEGREES_OF_FREEDOM = "NUMBER OF DEGREES OF FREEDOM";
        public static string NUMBER_OF_OBSERVATIONS = "NUMBER OF OBSERVATIONS";
        public static string NUMBER_OF_UNKNOWNS = "NUMBER OF UNKNOWNS";
        public static string PHASE_MEASUREMENTS_SIGMA = "PHASE MEASUREMENTS SIGMA";
        public static string SAMPLING_INTERVAL_SECONDS = "SAMPLING INTERVAL (SECONDS)";
        public static string SQUARE_SUM_OF_RESIDUALS_VTPV = "SQUARE SUM OF RESIDUALS (VTPV)";
        public static string VARIANCE_FACTOR = "VARIANCE FACTOR";
        public static string WEIGHTED_SQUARE_SUM_OF_O_C = "WEIGHTED SQUARE SUM OF O-C";

    }
    /// <summary>
    /// 统计信息
    /// </summary>
    public class SinexStatistic
    {
        /// <summary>
        /// of observations used in the adjustment.
        /// 观测值数量。
        /// </summary>
        public double NumberOfObservations { get; set; }
        /// <summary>
        /// 未知数数量。
        /// of unknowns solved in the adjustment
        /// </summary>
        public double NumberOfUnknown { get; set; }
        /// <summary>
        /// Interval in fraction between successives observations.
        /// 抽样间隔（秒）。
        /// </summary>
        public double SamplingIntervalSeconds { get; set; }

        /// <summary>
        /// SQUARE SUM OF RESIDUALS (VTPV)'
        /// Sum of squares of residuals.  (V'PV); 
        /// V - resid. vector; 残差向量
        /// P - weight matrix 权矩阵
        /// 残差平方和(V'PV)。
        /// </summary>
        public double SquareSumOfResidualsVTPV { get; set; }

        /// <summary>
        /// 'PHASE MEASUREMENTS SIGMA'
        /// Sigma used for the phase measurements.
        /// 相位测量Sigma
        /// </summary>
        public double PhaseMeasurementsSigma { get; set; }

        /// <summary>
        /// 'CODE MEASUREMENTS SIGMA'
        /// Sigma used for the obsCodeode (pseudo-range) measurements.
        /// 伪距测量Sigma
        /// </summary>
        public double CodeMeasurementsSigma { get; set; }

        /// <summary>
        /// 'NUMBER OF DEGREES OF FREEDOM' 
        /// # of observations minus the
        /// # of unknowns (df)
        /// 自由度数量。
        /// </summary>
        public double NumberOfDegreesOfFreedom { get; set; }

        /// <summary>
        /// 'VARIANCE FACTOR' 
        /// Sum of squares of residuals divided by the degrees of freedom (V'PV/df). 
        /// Equivalent to Chi-squared/df. 
        /// 残差平方和除以自由度。单位权方差因子？
        /// </summary>
        public double VarianceOfUnitWeight { get; set; }

        /// <summary>
        /// 'WEIGHTED SQUARE SUM OF O-C' 
        /// Sum of squares of the vector 
        /// 'observed minus computed': (o-c)'P(o-c) 
        /// with P - weigth matrix
        /// 观测值减计算值加权平方和
        /// </summary>
        public double WeightedSqureSumOfOMinusC { get; set; }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine(String.Format("观测值数量:{0}", this.NumberOfObservations));
            sb.AppendLine(String.Format("自由度数量:{0}", this.NumberOfDegreesOfFreedom));
            sb.AppendLine(String.Format("未知数数量:{0}", this.NumberOfUnknown));
            sb.AppendLine(String.Format("采样间隔（秒）:{0}", this.SamplingIntervalSeconds));
            sb.AppendLine(String.Format("残差平方和除以自由度，即单位权方差因子:{0}", this.VarianceOfUnitWeight));
            sb.AppendLine(String.Format("伪距测量Sigma（精度？）:{0}", this.CodeMeasurementsSigma));
            sb.AppendLine(String.Format("相位测量Sigma（精度？）:{0}", this.PhaseMeasurementsSigma));
            sb.AppendLine(String.Format("残差平方和(V'PV):{0}", this.SquareSumOfResidualsVTPV));
            sb.AppendLine(String.Format("观测值减计算值(O-C)加权平方和(l'Pl):{0}", this.WeightedSqureSumOfOMinusC));

            return sb.ToString();
        }
        public List<SolutionStatistic> GetSolutionStatistics() { return GetSolutionStatistics(this); }

        public static List<SolutionStatistic> GetSolutionStatistics(SinexStatistic sta)
        {
            List<SolutionStatistic> list = new List<SolutionStatistic>();
            list.Add(new SolutionStatistic()
            {
                Name = StatisticLabel.CODE_MEASUREMENTS_SIGMA,
                Val = sta.CodeMeasurementsSigma
            });
            list.Add(new SolutionStatistic()
            {
                Name = StatisticLabel.NUMBER_OF_DEGREES_OF_FREEDOM,
                Val = sta.NumberOfDegreesOfFreedom
            });
            list.Add(new SolutionStatistic()
            {
                Name = StatisticLabel.NUMBER_OF_OBSERVATIONS,
                Val = sta.NumberOfObservations
            });
            list.Add(new SolutionStatistic()
            {
                Name = StatisticLabel.NUMBER_OF_UNKNOWNS,
                Val = sta.NumberOfUnknown
            });
            list.Add(new SolutionStatistic()
            {
                Name = StatisticLabel.PHASE_MEASUREMENTS_SIGMA,
                Val = sta.PhaseMeasurementsSigma
            });
            list.Add(new SolutionStatistic()
            {
                Name = StatisticLabel.SAMPLING_INTERVAL_SECONDS,
                Val = sta.SamplingIntervalSeconds
            });
            list.Add(new SolutionStatistic()
            {
                Name = StatisticLabel.SQUARE_SUM_OF_RESIDUALS_VTPV,
                Val = sta.SquareSumOfResidualsVTPV
            });
            list.Add(new SolutionStatistic()
            {
                Name = StatisticLabel.VARIANCE_FACTOR,
                Val = sta.VarianceOfUnitWeight
            });
            list.Add(new SolutionStatistic()
            {
                Name = StatisticLabel.WEIGHTED_SQUARE_SUM_OF_O_C,
                Val = sta.WeightedSqureSumOfOMinusC
            });
            return list;
        }



        /// <summary>
        /// 合并两个统计信息产生一个新的。
        /// 本方法假定两文件观测内容不重复，且独立不相关。
        /// </summary>
        /// <param name="other"></param>
        public SinexStatistic Merge(SinexStatistic other)
        {
            return Merge(this, other);
        }

        /// <summary>
        /// 合并两个统计信息产生一个新的。
        /// 本方法假定两文件观测内容不重复，且独立不相关。
        /// 单位权方差 = （单位权方差A * 自由度A + 单位权方差B * 自由度B）/（自由度A + 自由度B）
        /// </summary>
        /// <param name="one"></param>
        /// <param name="other"></param>
        /// <returns></returns>
        public static SinexStatistic Merge(SinexStatistic one, SinexStatistic other)
        {
            return new SinexStatistic()
            {
                NumberOfObservations = one.NumberOfObservations + other.NumberOfObservations,
                NumberOfUnknown = one.NumberOfUnknown + other.NumberOfUnknown,
                NumberOfDegreesOfFreedom = one.NumberOfDegreesOfFreedom + other.NumberOfDegreesOfFreedom,
                VarianceOfUnitWeight = (one.VarianceOfUnitWeight * one.NumberOfDegreesOfFreedom
                    + other.VarianceOfUnitWeight * other.NumberOfDegreesOfFreedom)
                    / (one.NumberOfDegreesOfFreedom + other.NumberOfDegreesOfFreedom),
                //以下处理方式尚未明确正确与否
                CodeMeasurementsSigma = one.CodeMeasurementsSigma,
                WeightedSqureSumOfOMinusC = one.WeightedSqureSumOfOMinusC,
                PhaseMeasurementsSigma = one.PhaseMeasurementsSigma,
                SamplingIntervalSeconds = one.SamplingIntervalSeconds,
                SquareSumOfResidualsVTPV = one.SquareSumOfResidualsVTPV,
                //PhaseMeasurementsSigma =
                //    (one.PhaseMeasurementsSigma * one.NumberOfDegreesOfFreedom 
                //    + other.PhaseMeasurementsSigma * other.NumberOfDegreesOfFreedom)
                //    / (one.NumberOfDegreesOfFreedom + other.NumberOfDegreesOfFreedom),
            };
        }
        /// <summary>
        /// 单位权方差的合并。
        /// </summary>
        /// <param name="varianceA"></param>
        /// <param name="extraObsA"></param>
        /// <param name="varianceB"></param>
        /// <param name="extraObsB"></param>
        /// <returns></returns>
        public static double GetNewVarianceFactor(double varianceA, double extraObsA, double varianceB, double extraObsB)
        {
            return (varianceA * extraObsA + varianceB * extraObsB)/(extraObsA+ extraObsB);
        }
    }
}
