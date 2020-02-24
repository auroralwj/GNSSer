using System;
using System.Collections.Generic;

using System.Text;
using Gnsser.Domain;
using Geo.Coordinates;
using Gnsser.Data.Rinex;
using Geo.Algorithm;
using Gnsser.Data.Sinex;
using  Geo.Algorithm.Adjust;

namespace Gnsser.Service
{
   
    /// <summary>
    /// 伪距单点定位结果。
    /// </summary>
    public class ResultSinexBuillder
    { 
        /// <summary>
        /// 单点定位结果转化为Sinex文件
        /// </summary>
        /// <returns></returns>
        public static SinexFile Build( SingleSiteGnssResult result)
        {
            SinexFile sinex = new SinexFile("Gnsser");
            sinex.SolutionEpochBlock.Items.Add(new SolutionEpoch()
            {
                DateStart =result. ReceiverTime,
                DateEnd = result.ReceiverTime,
                DateMean = result.ReceiverTime,
                ObservationCode = "P",
                PointCode = "A",
                SiteCode = result.SiteInfo.SiteName,
                SolutionID = "0001"
            });
            SinexStatistic stat = new SinexStatistic()
            {
                NumberOfUnknown = result.ResultMatrix.ParamCount,
                NumberOfObservations = result.ResultMatrix.ObsMatrix.Observation.Count,
                VarianceOfUnitWeight = result.ResultMatrix.VarianceOfUnitWeight,
                NumberOfDegreesOfFreedom = result.ResultMatrix.Freedom
                   
            };
            sinex.SolutionStattisticsBlock.Items = stat.GetSolutionStatistics();

            GeoCoord approxGeo = CoordTransformer.XyzToGeoCoord(result.ApproxXyz);
             sinex.SiteIdBlock.Items.Add(new SiteId()
            {
                SiteCode = result.SiteInfo.SiteName,
                PointCode = "A",
                UniqueMonumentId = result.SiteInfo.MarkerNumber,
                ApproximateHeight = approxGeo.Height,
                ApproximateLatitude = approxGeo.Lat,
                ApproximateLongitude = approxGeo.Lon,
                ObservationCode = "P",
                GeoCoord = result.GeoCoord,
                StationDescription = "Single Point"
            } );
            int index = 0;
            sinex.SolutionEstimateBlock.Items.AddRange(new SolutionValue[]{ 
                new SolutionValue()
            {
                Index = 1 + index++,
                ParameterType = "STAX",
                ParameterValue =result. EstimatedXyz.X,
                SiteCode = result.SiteInfo.SiteName,
                RefEpoch = result.ReceiverTime,
                PointCode = "A",
                ParameterUnits = "m",
                ConstraintCode = "2",
                StdDev = Math.Sqrt(result.ResultMatrix.CovaOfEstimatedParam[index-1,index-1]),
                SolutionID = "0001"
            },   new SolutionValue()
            {
                 Index = 1 + index++,
                ParameterType = "STAY",
                ParameterValue =result. EstimatedXyz.Y,
                SiteCode = result.SiteInfo.SiteName,
                RefEpoch =result. ReceiverTime,
                PointCode = "A",
                ParameterUnits = "m",
                ConstraintCode = "2",
                StdDev = Math.Sqrt(result.ResultMatrix.CovaOfEstimatedParam[index-1,index-1]),
                SolutionID = "0001"
            },   new SolutionValue()
            {
               Index = 1 + index++,
                ParameterType = "STAZ",
                ParameterValue =result. EstimatedXyz.Z,
                SiteCode = result. SiteInfo.SiteName,
                RefEpoch =result. ReceiverTime,
                PointCode = "A",
                ParameterUnits = "m",
                ConstraintCode = "2",
                StdDev = Math.Sqrt(result.ResultMatrix.CovaOfEstimatedParam[index-1,index-1]),
                SolutionID = "0001"
            },   
            new SolutionValue()
            {
                Index = 1 + index++,
                ParameterType = "RBIAS",
                ParameterValue =  result.RcvClkErrDistance,
                SiteCode = result.SiteInfo.SiteName,
                RefEpoch =result. ReceiverTime,
                PointCode = "A",
                ParameterUnits = "m",
                ConstraintCode = "2",
                StdDev = Math.Sqrt(result.ResultMatrix.CovaOfEstimatedParam[index-1,index-1]),
                SolutionID = "0001"
            }            
            });

            //  sinex.
            sinex.SolutionMatrixEstimateCova.Items = SinexMatrixConvertor.GetMatrixLines(result.ResultMatrix.CovaOfEstimatedParam.Array);

            return sinex;
        }
    }
}
