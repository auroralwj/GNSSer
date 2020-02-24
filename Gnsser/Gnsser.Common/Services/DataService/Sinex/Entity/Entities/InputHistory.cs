using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using Geo.Coordinates;
using Gnsser.Times;
using Geo.Utils;
using Geo.Times; 

namespace Gnsser.Data.Sinex
{

    /// <summary>
    /// 历史记录。
    /// +INPUT/HISTORY
    /// *_Version_ Cre __Creation__ Own _Data_start_ _Data_end___ TProduct Param S Type_
    /// +GLK 1.06 SIO 12:365:84606 SIO 12:365:00000 12:365:86370 P 00732 2 S E A
    /// </summary>
    public class InputHistory : IBlockItem
    {
        public string FileCode { get; set; }
        public string DocumentType { get; set; }
        public Double FormatVersion { get; set; }
        public string CreationAgencyCode { get; set; }
        public Time CreationTime { get; set; }
        public string DataProvidingAgencyCode { get; set; }
        public Time StartTime { get; set; }
        public Time EndTime { get; set; }
        public string ObservationTechnique { get; set; }
        public int NumberOfEstimates { get; set; }
        public string ConstraintCode { get; set; }
        public string[] SolutionContents { get; set; }

        public override bool Equals(object obj)
        {
            InputHistory site = obj as InputHistory;
            return site == null ? false : FileCode.Equals(site.FileCode);
        }

        public override int GetHashCode()
        {
            return FileCode.GetHashCode();
        }
        public override string ToString()
        {
            string line = " ";
            line += FileCode;
            line += DocumentType;
            line += " " + FormatVersion.ToString("0.00");
            line += " " + CreationAgencyCode;
            line += " " + CreationTime.ToYdsString();
            line += " " + StringUtil.FillSpace(DataProvidingAgencyCode, 3);
            line += " " + StartTime.ToYdsString();
            line += " " + EndTime.ToYdsString();
            line += " " + ObservationTechnique;
            line += " " + StringUtil.FillZeroLeft(NumberOfEstimates, 5);
            line += " " + ConstraintCode;

            foreach (var item in SolutionContents)
            {
                line += " " + item;
            }
            return line;
        }

        public  IBlockItem Init(string line)
        {
            // InputHistory b = new InputHistory();
            this.FileCode = line.Substring(1, 1);
            this.DocumentType = line.Substring(2, 3);
            this.FormatVersion = Double.Parse(line.Substring(6, 4));
            this.CreationAgencyCode = line.Substring(11, 3);
            this.CreationTime = Time.ParseYds(line.Substring(15, 12));
            this.DataProvidingAgencyCode = line.Substring(28, 3);
            this.StartTime = Time.ParseYds(line.Substring(32, 12));
            this.EndTime = Time.ParseYds(line.Substring(45, 12));
            this.ObservationTechnique = line.Substring(58, 1);
            this.NumberOfEstimates = int.Parse(line.Substring(60, 5));
            this.ConstraintCode = line.Substring(66, 1);
            this.SolutionContents = line.Substring(68).Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);

            return this;
        }

        public static InputHistory ParseLine(string line)
        {
            InputHistory b = new InputHistory();
            b.FileCode = line.Substring(1, 1);
            b.DocumentType = line.Substring(2, 3);
            b.FormatVersion = Double.Parse(line.Substring(6, 4));
            b.CreationAgencyCode = line.Substring(11, 3);
            b.CreationTime = Time.ParseYds(line.Substring(15, 12));
            b.DataProvidingAgencyCode = line.Substring(28, 3);
            b.StartTime = Time.ParseYds(line.Substring(32, 12));
            b.EndTime = Time.ParseYds(line.Substring(45, 12));
            b.ObservationTechnique = line.Substring(58, 1);
            b.NumberOfEstimates = int.Parse(line.Substring(60, 5));
            b.ConstraintCode = line.Substring(66, 1);
            b.SolutionContents = line.Substring(68).Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);

            return b;
        }

    }

}
