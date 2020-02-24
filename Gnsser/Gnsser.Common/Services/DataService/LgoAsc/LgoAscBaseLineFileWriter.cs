//2018.12.05, czs, edit in hmx, 莱卡LGO文件

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using Geo.Coordinates;
using Geo.Algorithm;
using Geo.Times;
using System.Threading.Tasks;

namespace Gnsser.Data
{
    //https://surveyequipment.com/assets/index/download/id/221/
 
    /// <summary>
    /// 文件
    /// </summary>
    public class LgoAscBaseLineFileWriter
    {

        public LgoAscBaseLineFileWriter(String filePath)
        {
            this.FilePath = filePath;
        }
        /// <summary>
        /// 文件路径
        /// </summary>
        public string FilePath { get; set; }

        public void Write(MultiEpochLgoAscBaseLineFile files)
        {
            StringBuilder sb = new StringBuilder();
            var txt = BuidFileString(files);

            using (StreamWriter writer = new StreamWriter(new FileStream(FilePath, FileMode.Create), Encoding.Default))
            {
                writer.Write(txt);
            }
        }
        public void Write(List<EpochLgoAscBaseLine> files)
        {
            StringBuilder sb = new StringBuilder();
            var txt = BuidFileString(files);

            using (StreamWriter writer = new StreamWriter(new FileStream(FilePath, FileMode.Create), Encoding.Default))
            {
                writer.Write(txt);
            }
        }
        public void Write(EpochLgoAscBaseLine file)
        {
            StringBuilder sb = new StringBuilder();
            var txt = BuidFileString(file);

            using (StreamWriter writer = new StreamWriter(new FileStream(FilePath, FileMode.Create), Encoding.Default))
            {
                writer.Write(txt);
            }
        }

        public string BuidFileString(MultiEpochLgoAscBaseLineFile files)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(GetHeaderString());
            foreach (var file in files)
            {
                sb.Append(BuildContent(file));
            }

            return sb.ToString();
        }
        public string BuidFileString(List<EpochLgoAscBaseLine> files)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(GetHeaderString());
            foreach (var file in files)
            {
                sb.Append(BuildContent(file));
            }

            return sb.ToString();
        }

        public string BuidFileString(EpochLgoAscBaseLine file)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(GetHeaderString());
            sb.Append(BuildContent(file));

            return sb.ToString();
        }

        private StringBuilder BuildContent(EpochLgoAscBaseLine file)
        {
            StringBuilder sb = new StringBuilder();
            var net = file.GetBaseLineNet();

            foreach (var kv in file.BaseLines)
            {
                var line = kv.Value;
                var item = kv.Value.Baseline;
                var xyz = item.ApproxXyzOfRov;
                //流动站坐标
                sb.AppendLine("@#"
                        + Geo.Utils.StringUtil.FillSpaceRight(item.BaseLineName.RovName, 16)
                        + " "
                        + ToString(item.ApproxXyzOfRov, 15, 4)
                        + "            MEAS"
                        + "    " + item.EstimatedRmsXyzOfRov.StdDev.Length.ToString("G5") + "   12");
                sb.AppendLine("@&" + ToString(item.CovaMatrix, item.StdDev));
                sb.AppendLine("@E  " + ToString(line.ErrorEllipse));

                //基线内容
                sb.AppendLine("@+"
                  + Geo.Utils.StringUtil.FillSpaceRight(item.BaseLineName.RefName, 16)
                  + " "
                  + ToString(item.ApproxXyzOfRef, 15, 4));
                sb.AppendLine("@-"
                  + Geo.Utils.StringUtil.FillSpaceRight(item.BaseLineName.RovName, 16)
                  + " "
                  + ToString(item.EstimatedVector, 15, 4));

                sb.AppendLine("@=" + ToString(item.CovaMatrix, item.StdDev));
                sb.AppendLine("@:" + " "
                   + Geo.Utils.StringUtil.FillSpaceLeft(line.AntennaBiasOfRef.Height, 14)
                   + " " + Geo.Utils.StringUtil.FillSpaceLeft(line.AntennaBiasOfRef.Offset, 14)
                   );
                sb.AppendLine("@;" + " "
                   + Geo.Utils.StringUtil.FillSpaceLeft(line.AntennaBiasOfRov.Height, 14)
                   + " " + Geo.Utils.StringUtil.FillSpaceLeft(line.AntennaBiasOfRov.Offset, 14)
                   );
                sb.AppendLine("@*" + ToString(item.Epoch));
                sb.AppendLine("@E  " + ToString(line.ErrorEllipse));
                //                    @:         1.4580         0.0000
                //@; 1.4110         0.0000
                //@*13.11.2018 04:23:42 
            }
            return sb;
        }

        public void WriteOld(EpochLgoAscBaseLine file)
        {
            StringBuilder sb = new StringBuilder();


            using (StreamWriter writer = new StreamWriter(new FileStream(FilePath, FileMode.Create), Encoding.Default))
            {
                WriterHeader(writer);
                var net = file.GetBaseLineNet();

                foreach (var kv in file.BaseLines)
                {
                    var line = kv.Value;
                    var item = kv.Value.Baseline;
                    var xyz = item.ApproxXyzOfRov;
                    //流动站坐标
                    writer.WriteLine("@#"
                        + Geo.Utils.StringUtil.FillSpaceRight(item.BaseLineName.RovName, 16)
                        + " "
                        + ToString(item.ApproxXyzOfRov, 15, 4)
                        + "            MEAS"
                        + "    " + item.EstimatedRmsXyzOfRov.StdDev.Length.ToString("G5") + "   12");
                    writer.WriteLine("@&" + ToString(item.CovaMatrix, item.StdDev));
                    writer.WriteLine("@E  " + ToString(line.ErrorEllipse));

                    //基线内容
                    writer.WriteLine("@+"
                       + Geo.Utils.StringUtil.FillSpaceRight(item.BaseLineName.RefName, 16)
                       + " "
                       + ToString(item.ApproxXyzOfRef, 15, 4));
                    writer.WriteLine("@-"
                       + Geo.Utils.StringUtil.FillSpaceRight(item.BaseLineName.RovName, 16)
                       + " "
                       + ToString(item.EstimatedVector, 15, 4));

                    writer.WriteLine("@=" + ToString(item.CovaMatrix, item.StdDev));
                    writer.WriteLine("@:" + " "
                        + Geo.Utils.StringUtil.FillSpaceLeft(line.AntennaBiasOfRef.Height, 14)
                        + " " + Geo.Utils.StringUtil.FillSpaceLeft(line.AntennaBiasOfRef.Offset, 14)
                        );
                    writer.WriteLine("@;" + " "
                        + Geo.Utils.StringUtil.FillSpaceLeft(line.AntennaBiasOfRov.Height, 14)
                        + " " + Geo.Utils.StringUtil.FillSpaceLeft(line.AntennaBiasOfRov.Offset, 14)
                        );
                    writer.WriteLine("@*" + ToString(item.Epoch));
                    writer.WriteLine("@E  " + ToString(line.ErrorEllipse));
                    //                    @:         1.4580         0.0000
                    //@; 1.4110         0.0000
                    //@*13.11.2018 04:23:42 
                }
            }
        }

        private string ToString(Time time)
        {
            StringBuilder sb = new StringBuilder(10);
            sb
                .Append(time.Day.ToString("00"))
                .Append(".")
                .Append(time.Month.ToString("00"))
                .Append(".")
                .Append(time.Year)
                .Append(" ")
                .Append(time.Hour.ToString("00"))
                .Append(":")
                .Append(time.Minute.ToString("00"))
                .Append(":")
                .Append(time.Second.ToString("00"));
            return sb.ToString();
        }
        public static string ToString(LgoAscElementsOfAbsoluteErrorEllipse elipse)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(" ");
            sb.Append(ToString(elipse.SemiMajor, 12, 8));
            sb.Append(" ");
            sb.Append(ToString(elipse.SemiMinor, 12, 8));
            sb.Append(" ");
            sb.Append(ToString(elipse.OrientationInRadians, 12, 8));
            sb.Append(" ");
            sb.Append(ToString(elipse.Height, 12, 8));
            return sb.ToString();

        } 
        public static string ToString(Matrix coaraMatrix, double delta0)
        {
            StringBuilder sb = new StringBuilder();
            double mm0 = delta0 * delta0;
            sb.Append(ToString(delta0, 10, 5));
            sb.Append(" ");
           sb.Append(ToString(coaraMatrix[0, 0] / mm0, 17, 12));
            sb.Append(" ");
            sb.Append(ToString(coaraMatrix[0, 1] / mm0, 17, 12));
            sb.Append(" ");
            sb.Append(ToString(coaraMatrix[0, 2] / mm0, 17, 12));
            sb.Append(" ");
            sb.Append(ToString(coaraMatrix[1, 1] / mm0, 17, 12));
            sb.Append(" ");
            sb.Append(ToString(coaraMatrix[1, 2] / mm0, 17, 12));
            sb.Append(" ");
            sb.Append(ToString(coaraMatrix[2, 2] / mm0, 17, 12));

            return sb.ToString();

        }
        public static string ToString(double val, int itemWidth, int fractionCount)
        {
            string format = "0.";
            for (int i = 0; i < fractionCount; i++)
            {
                format += "0";
            }
            return Geo.Utils.StringUtil.FillSpaceLeft(val.ToString(format), itemWidth);
        }
            /// <summary>
            /// 转换为字符串,保证有个空格
            /// </summary>
            /// <param name="xyz"></param>
            /// <param name="itemWidth"></param>
            /// <param name="fractionCount"></param>
            /// <returns></returns>
        public static string ToString(XYZ xyz, int itemWidth, int fractionCount)
        {
            string format = "0.";
            for (int i = 0; i < fractionCount; i++)
            {
                format += "0";
            }
            StringBuilder sb = new StringBuilder();
            sb.Append(Geo.Utils.StringUtil.FillSpaceLeft(xyz.X.ToString(format), itemWidth));
            sb.Append(" ");
            sb.Append(Geo.Utils.StringUtil.FillSpaceLeft(xyz.Y.ToString(format), itemWidth));
            sb.Append(" ");
            sb.Append(Geo.Utils.StringUtil.FillSpaceLeft(xyz.Z.ToString(format), itemWidth));
            return sb.ToString();
        }

        private static void WriterHeader(StreamWriter writer)
        {
            writer.WriteLine("@%Unit:                m");
            writer.WriteLine("@%Coordinate type:     Cartesian");
            writer.WriteLine("@%Reference ellipsoid: WGS 1984");
        }
        private string GetHeaderString()
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("@%Unit:                m");
            sb.AppendLine("@%Coordinate type:     Cartesian");
            sb.AppendLine("@%Reference ellipsoid: WGS 1984");
            return sb.ToString();
        }
    }
}
