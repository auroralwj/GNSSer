using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using Geo.Coordinates;
using Geo.Utils;

namespace Gnsser.Data.Sinex
{
    /// <summary>
    ///+SATELLITE/PHASE_CENTER
    ///G063 5 1.5613 0.3940 0.0000 0 0.0000 0.0000 0.0000 IGS08_1717 A E
    /// </summary>
    public class SatellitePhaseCenter : IBlockItem
    {
        public string SatelliteCode { get; set; }
        public string FrequencyCodeA { get; set; }
        public XYZ PhaseCenterOffsetA { get; set; }
        public string FrequencyCodeB { get; set; }
        public XYZ PhaseCenterOffsetB { get; set; }
        public string AntennaCalibrationModel { get; set; }
        public string PcvType { get; set; }
        public string PcvModelApplication { get; set; }

        public override bool Equals(object obj)
        {
            SatellitePhaseCenter site = obj as SatellitePhaseCenter;
            return site == null ? false : SatelliteCode.Equals(site.SatelliteCode);
        }

        public override int GetHashCode()
        {
            return SatelliteCode.GetHashCode();
        }
        public override string ToString()
        {
            string line = "";
            line += " " + StringUtil.FillSpace(SatelliteCode, 4);
            line += " " + StringUtil.FillSpace(FrequencyCodeA, 1);
            line += " " + PhaseCenterOffsetA.ToSnxString();
            line += " " + FrequencyCodeB;
            line += " " + PhaseCenterOffsetB.ToSnxString();
            line += " " + StringUtil.FillSpace(AntennaCalibrationModel, 10);
            line += " " + PcvType;
            line += " " + PcvModelApplication;

            return line;
        }

        public static SatellitePhaseCenter ParseLine(string line)
        {
            SatellitePhaseCenter b = new SatellitePhaseCenter();
            b.SatelliteCode = line.Substring(1, 4);
            b.FrequencyCodeA = line.Substring(6, 1);
            b.PhaseCenterOffsetA = XYZ.ParseSnx(line.Substring(8, 20));
            b.FrequencyCodeB = line.Substring(29, 1);
            b.PhaseCenterOffsetB = XYZ.ParseSnx(line.Substring(31, 20));
            b.AntennaCalibrationModel = line.Substring(52, 10);
            b.PcvType = line.Substring(63, 1);
            b.PcvModelApplication = line.Substring(65, 1);

            return b;
        }

        public  IBlockItem Init(string line)
        {
            this.SatelliteCode = line.Substring(1, 4);
            this.FrequencyCodeA = line.Substring(6, 1);
            this.PhaseCenterOffsetA = XYZ.ParseSnx(line.Substring(8, 20));
            this.FrequencyCodeB = line.Substring(29, 1);
            this.PhaseCenterOffsetB = XYZ.ParseSnx(line.Substring(31, 20));
            this.AntennaCalibrationModel = line.Substring(52, 10);
            this.PcvType = line.Substring(63, 1);
            this.PcvModelApplication = line.Substring(65, 1);

            return this;     
        }
    }
}
