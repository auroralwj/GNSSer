//2018.12.05, czs, edit in hmx, 莱卡LGO文件

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Geo.Algorithm;
using Geo.Times;
using Geo.Coordinates;

namespace Gnsser.Data
{
    //https://surveyequipment.com/assets/index/download/id/221/

    /// <summary>
    /// There are a maximum of 4 header lines possible. The first two header lines are compulsory, the last two are 
    /// optional for the input file but are always included in the output file unless the option to omit them has been selected.
    /// The sequence of the header lines has to be strictly observed. 
    /// </summary>
    public class LgoAscHeader
    {
        public LgoAscHeader()
        {
            Unit = "m";
            CoordinateType = "Cartesian";
            CoordinateType = "WGS 1984";
        }

        /// <summary>
        /// m: for meter,        
        /// fts: for U.S.survey foot, 
        /// fti: for international foot
        /// </summary>
        public string Unit { get; set; }
        /// <summary>
        /// Coordinate type
        /// </summary>
        public string CoordinateType { get; set; }
        /// <summary>
        /// Reference ellipsoid
        /// </summary>
        public string ReferenceEllipsoid { get; set; }
        /// <summary>
        /// Projection set
        /// </summary>
        public string ProjectionSet { get; set; } 
    }

    /// <summary>
    /// Each point must contain the line with the coordinate information. In addition, more information like thematical 
    /// coding and the variance covariance information can be attached with additional lines.These lines have to follow
    /// immediately the first line of the point and coordinate information. 
    /// </summary>
    public class LgoAscPoint
    {
        public LgoAscPoint()
        {
            CoordinateType = "12";
        }
        /// <summary>
        /// 历元
        /// </summary>
        public Time Epoch { get; set; }
        public string Name { get; set; }

        public XYZ XYZ { get; set; }
        
        public string GeoidSeparationN  { get; set; }

        public string CoordinateClass  { get; set; }

        public string CoordinateQuality { get; set; }

        public string CoordinateType { get; set; }
        public Matrix CovaMatrix { get; internal set; }
        /// <summary>
        /// 椭球差
        /// </summary>
        public LgoAscElementsOfAbsoluteErrorEllipse ErrorEllipse { get; set; }
    }


    /// <summary>
    /// 基线
    /// </summary>
    public class LgoAscBaseLine
    {
        public LgoAscBaseLine(EstimatedBaseline baseline)
        {
            this.Baseline = baseline;
            ErrorEllipse = new LgoAscElementsOfAbsoluteErrorEllipse();
            AntennaBiasOfRov = new HeightOffset();
            AntennaBiasOfRef = new HeightOffset();
        }

        public LgoAscBaseLine()
        {
            Baseline = new  EstimatedBaseline();
            ErrorEllipse = new LgoAscElementsOfAbsoluteErrorEllipse();
            AntennaBiasOfRov = new HeightOffset();
            AntennaBiasOfRef = new HeightOffset();
        }
        /// <summary>
        /// 名称
        /// </summary>
        public GnssBaseLineName LineName { get => Baseline.BaseLineName;  set=> Baseline.BaseLineName = value; }
        /// <summary>
        /// 历元
        /// </summary>
        public Time Epoch { get => Baseline.Epoch; set => Baseline.Epoch = value; }
        /// <summary>
        /// 参考站天线偏差
        /// </summary>
        public HeightOffset AntennaBiasOfRef { get; set; }
        /// <summary>
        /// 流动站天线偏差
        /// </summary>
        public HeightOffset AntennaBiasOfRov { get; set; }
        /// <summary>
        /// 基线数据
        /// </summary>
        public EstimatedBaseline Baseline { get; set; } 
        /// <summary>
        /// 椭球差
        /// </summary>
        public LgoAscElementsOfAbsoluteErrorEllipse ErrorEllipse { get; set; } 
    }
    /// <summary>
    /// 高度和偏差
    /// </summary>
    public class HeightOffset
    {
        public double Height { set; get; }
        public double Offset { set; get; }
    }

    public class LgoAscElementsOfAbsoluteErrorEllipse
    {
        public double SemiMajor { get; set; }
        public double SemiMinor { get; set; }
        public double OrientationInRadians { get; set; }
        public double Height { get; set; }
    }

    








    /*
     * This format may be used to import coordinates into LGO. It may also be used to write coordinates into an ASCII file 
        from LGO. The same format may be used to exchange baseline vectors. See SKI ASCII Baseline Vector format for 
        further information
     */
    /// <summary>
    /// LGO 标识信息
    /// </summary>
    public class LgoAscLable 
    {
        /// <summary>
        /// Projection set
        /// </summary>
        public const string ProjectionSet = "Projection set";
        /// <summary>
        /// Reference ellipsoid
        /// </summary>
        public const string ReferenceEllipsoid = "Reference ellipsoid";

        /// <summary>
        /// Coordinate type
        /// </summary>
        public const string CoordinateType = "Coordinate type";
        /// <summary>
        /// Unit
        /// </summary>
        public const string Unit = "Unit";
        /// <summary>
        /// Header lines
        /// </summary>
        public const string HeaderLines = "@%";
        /// <summary>
        /// Point and coordinate information
        /// </summary>
        public const string PointAndCoordinateInformation = "@#";
        /// <summary>
        /// Convergence angle and scale factor information
        /// </summary>
        public const string ConvergenceAngleAndScaleFactorInformation = "@$";
        /// <summary>
        /// Variance-Covariance information
        /// </summary>
        public const string VarianceCovarianceOfPoint = "@&";
        /// <summary>
        /// Error Ellipse (absolute)
        /// </summary>
        public const string ErrorEllipseAbsolute = "@E";
        /// <summary>
        ///  Reliability 
        /// </summary>
        public const string  Reliability = "@R";
        /// <summary>
        ///  Code 
        /// </summary>
        public const string Code = "@1";
        /// <summary>
        /// Code Description
        /// </summary>
        public const string CodeDescription = "@2";
        /// <summary>
        /// Code Group
        /// </summary>
        public const string CodeGroup = "@3";
        /// <summary>
        /// Attribute
        /// </summary>
        public const string Attribute = "@A";
        /// <summary>
        ///  Annotations
        /// </summary>
        public const string Annotations = "@4";
        /// <summary>
        /// Free Code
        /// </summary>
        public const string FreeCode = "@F";
        /// <summary>
        /// Free Code Description
        /// </summary>
        public const string FreeCodeDescription = "@G";
        /// <summary>
        /// Free Code Information Record
        /// </summary>
        public const string FreeCodeInformationRecord = "@H";
        /// <summary>
        /// Field Note
        /// </summary>
        public const string FieldNo = "@N";


        //This format is used when baseline vectors are required (e.g. as input into an adjustment program).
        //The SKI ASCII  Baseline Vector format is an extension of the SKI ASCII Point Coordinate format


        /// <summary>
        /// Individual baseline information (Reference point of baseline and its coordinates)
        /// </summary>
        public const string ReferencePointOfBaseline  = "@+";
        /// <summary>
        /// Baseline vector components
        /// </summary>
        public const string BaselineVectorComponents = "@-";
        /// <summary>
        /// Variance-covariance information for baseline vector
        /// </summary>
        public const string VarianceCovarianceOfBaseLine = "@=";
        /// <summary>
        /// Reference antenna height and offset
        /// </summary>
        public const string ReferenceAntennaHeightAndOffset = "@:";
        /// <summary>
        /// Rover antenna height and offset
        /// </summary>
        public const string RoverAntennaHeightAndOffset = "@;";
        /// <summary>
        /// Date and time of first common epoch
        /// </summary>
        public const string DateTimeOfFirstCommonEpoch = "@*";

        /**
         * @E Error Ellipse (relative).
         * Note: In the SKI ASCII Point Coordinate format the error ellipses records accompany 
         * the point info and refer to absolute error ellipses.
         * Here they accompany the baseline info and therefor refer to relative error ellips
         */

    }
}
