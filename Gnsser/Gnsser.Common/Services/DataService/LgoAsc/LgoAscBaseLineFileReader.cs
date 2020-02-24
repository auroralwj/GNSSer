//2018.12.05, czs, edit in hmx, 莱卡LGO文件

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using Geo.Algorithm;
using System.Threading.Tasks;
using Geo.Times;
using Geo.Coordinates;

namespace Gnsser.Data
{
    //https://surveyequipment.com/assets/index/download/id/221/
 
    /// <summary>
    /// 文件
    /// </summary>
    public class LgoAscBaseLineFileReader 
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="path"></param>
        public LgoAscBaseLineFileReader(string path)
        {
            this.FilePath = path;
        }

        /// <summary>
        /// 文件路径
        /// </summary>
        public string FilePath { get; set; }

        /// <summary>
        /// 读取文件
        /// </summary>
        /// <returns></returns>
        public MultiEpochLgoAscBaseLineFile Read()
        {
            var  file = new MultiEpochLgoAscBaseLineFile();
            file.Name = Path.GetFileName(FilePath);
            file.Header = LoadHeader();

            string line = null;
            using (StreamReader reader = new StreamReader(FilePath))
            {
                LgoAscPoint currentPoint = null;
                LgoAscBaseLine currentLine = null;
                EpochLgoAscBaseLine epochLgoAscBaseLine = null;
                bool isInPoint = true;
                while ((line = reader.ReadLine()) != null)
                {
                    var label = line.Substring(0, 2);
                    if (label == LgoAscLable.HeaderLines) { continue; }//跳过头部

                    var content = Geo.Utils.StringUtil.SubString(line, 2);
                    switch (label)
                    {
                        case LgoAscLable.PointAndCoordinateInformation:
                            {
                                currentPoint = ParsePoint(content); 
                                isInPoint = true;
                            }
                            break;
                        case LgoAscLable.ReferencePointOfBaseline:
                            { 
                                var xyz = ParseNamedXyz(content);
                                //由于第一次无法将名称读完，因此只能读完后再存储
                                currentLine = new LgoAscBaseLine();
                                currentLine.LineName.RefName = xyz.Name;
                                currentLine.Baseline.ApproxXyzOfRef = xyz.Value;

                                isInPoint = false;
                            }
                            break;
                        case LgoAscLable.BaselineVectorComponents:
                            {
                                var xyz = ParseNamedXyz(content);
                                currentLine.Baseline.BaseLineName.RovName = xyz.Name;

                                currentLine.Baseline.EstimatedVectorRmsedXYZ.Value = xyz.Value;
                                //流动站坐标位于基线上方
                                if(currentPoint!=null && currentPoint.Name == xyz.Name)
                                {
                                    currentLine.Baseline.ApproxXyzOfRov = currentPoint.XYZ;
                                }
                                else
                                {
                                    currentLine.Baseline.ApproxXyzOfRov = currentLine.Baseline.ApproxXyzOfRef + xyz.Value;
                                } 

                                currentLine.Baseline.BaseLineName.Init();

                               isInPoint = false;
                            }
                            break;
                        case LgoAscLable.VarianceCovarianceOfPoint:
                            {
                                double sigma0;
                                var cova = ParseCovaMatrix(content, out sigma0);
                                currentPoint.CovaMatrix = new Matrix(cova);
                            }
                            break;
                        case LgoAscLable.VarianceCovarianceOfBaseLine:
                            {
                                double sigma0;
                                var cova = ParseCovaMatrix(content, out sigma0);
                                currentLine.Baseline.StdDev = sigma0;
                                currentLine.Baseline.CovaMatrix = new Matrix(cova);
                                currentLine.Baseline.EstimatedVectorRmsedXYZ.Rms = new XYZ(Math.Sqrt(cova[0, 0]), Math.Sqrt(cova[1, 1]), Math.Sqrt(cova[2, 2]));
                            }
                            break; 
                        case LgoAscLable.ErrorEllipseAbsolute:
                            {
                                var result = ParseErrorEllipse(content);
                                if (isInPoint)
                                {
                                    currentPoint.ErrorEllipse = result;
                                }
                                else
                                {
                                    currentLine.ErrorEllipse = result;
                                }
                            }
                            break;
                        case LgoAscLable.DateTimeOfFirstCommonEpoch:
                            {
                                currentLine.Epoch = ParseEpoch(content);

                                file.GetOrCreate(currentLine.Epoch).Set(currentLine); 
                            }
                            break;
                        case LgoAscLable.ReferenceAntennaHeightAndOffset:
                            {
                                currentLine.AntennaBiasOfRef = ParseHeightOffset(content);
                            }
                            break;
                        case LgoAscLable.RoverAntennaHeightAndOffset:
                            {
                                currentLine.AntennaBiasOfRov = ParseHeightOffset(content);
                            }
                            break;
                        default:
                            break;
                    }
                }
                //最后保存最后的次基线 
                var epochFile = file.GetOrCreate(currentLine.Epoch);
                epochFile.Set(currentLine);
            }

            return file;
        }
         

        /// <summary>
        /// 偏移
        /// </summary>
        /// <param name="content"></param>
        /// <returns></returns>
        public static HeightOffset ParseHeightOffset(string content)
        {
            var items = Geo.Utils.StringUtil.ParseDoubles(content, new char[] { ' ' });
            return new HeightOffset()
            {
                Height = items[0],
                Offset = items[1]
            };
        }

        /// <summary>
        /// 历元解析
        /// </summary>
        /// <param name="content"></param>
        /// <returns></returns>
        public static Time ParseEpoch(string content)
        {
            Time epoch = Time.Now;
            try
            {
                var items = content.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                var dates = items[0].Split('.');
                var time = DateTime.Parse(items[1]);
                var date = new DateTime(Convert.ToInt32(dates[2]), Convert.ToInt32(dates[1]), Convert.ToInt32(dates[0]));
                var result = date + time.TimeOfDay;
                return new Time(result);
            }
            catch (Exception ex3)
            {
                try
                {
                    return new Time(DateTime.Parse(content));
                }
                catch (Exception ex1)
                {
                    try
                    {
                        return Time.Parse(content);
                    }
                    catch (Exception ex2)
                    {

                    }
                }
            }

            return epoch;
        }

        /// <summary>
        /// Elements of absolute error ellipse 
        /// </summary>
        /// <param name="content"></param>
        /// <returns></returns>
        public static LgoAscElementsOfAbsoluteErrorEllipse ParseErrorEllipse(string content)
        {
            var items = Geo.Utils.StringUtil.ParseDoubles(content, new char[] { ' ' });
            var result = new LgoAscElementsOfAbsoluteErrorEllipse()
            {
                SemiMajor = items[0],
                SemiMinor = items[1],
                OrientationInRadians = items[2],
                Height = items[3],
            };
            return result;
        }

        /// <summary>
        /// 对称矩阵，第一个为单位权中误差， 3*3 .
        /// 转换为方差阵返回。
        /// </summary>
        /// <param name="content"></param>
        /// <returns></returns>
        public static SymmetricMatrix ParseCovaMatrix(string content, out double signa0)
        {
            var items = content.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
            var m0 = Geo.Utils.StringUtil.ParseDouble(items[0]);// * 1e2; // 单位是否是cm米（和）？？？？
            var mm0 =  m0 * m0;
            signa0 = m0;

            SymmetricMatrix cova = new SymmetricMatrix(3);
            cova[0, 0] = Geo.Utils.StringUtil.ParseDouble(items[1]) * mm0;
            cova[0, 1] = Geo.Utils.StringUtil.ParseDouble(items[2]) * mm0;
            cova[0, 2] = Geo.Utils.StringUtil.ParseDouble(items[3]) * mm0;
            cova[1, 1] = Geo.Utils.StringUtil.ParseDouble(items[4]) * mm0;
            cova[1, 2] = Geo.Utils.StringUtil.ParseDouble(items[5]) * mm0;
            cova[2, 2] = Geo.Utils.StringUtil.ParseDouble(items[6]) * mm0;
            
            return cova;
        } 

        /// <summary>
        /// 解析坐标
        /// </summary>
        /// <param name="content"></param>
        /// <returns></returns>
        public static LgoAscPoint ParsePoint(string content)
        {
            NamedXyz namedXyz = ParseNamedXyz(content);

            LgoAscPoint result = new LgoAscPoint()
            {
                Name = namedXyz.Name,
                XYZ = namedXyz.Value,
                // CoordinateType = strs[6]

            };
            return result;
        }
        /// <summary>
        /// 4 conponents
        /// </summary>
        /// <param name="content"></param>
        /// <returns></returns>
        private static NamedXyz ParseNamedXyz(string content)
        { 
            var strs = Geo.Utils.StringUtil.Split(content, new char[] { ' ' }, true);
            var name = strs[0];
            var xyz = new XYZ(Convert.ToDouble(strs[1]), Convert.ToDouble(strs[2]), Convert.ToDouble(strs[3]));
            var namedXyz = new NamedXyz(name, xyz);
            return namedXyz;
        }

        /// <summary>
        /// 载入头部信息
        /// </summary>
        /// <returns></returns>
        public LgoAscHeader LoadHeader()
        {
            string line = null;
            LgoAscHeader header = new LgoAscHeader();
            using (StreamReader reader = new StreamReader(FilePath))
            {
                while ((line = reader.ReadLine()) != null)
                {
                    var label = line.Substring(0, 2);
                    if (label != LgoAscLable.HeaderLines) { break; }
                    var content = Geo.Utils.StringUtil.SubString(line, 2);
                    var spliter = ":";
                    var contents = content.Split(new string[] { spliter }, StringSplitOptions.RemoveEmptyEntries);

                    if (contents.Length < 2) { continue; }

                    var name = contents[0];
                    var val = contents[1];

                    if (name == LgoAscLable.Unit) { header.Unit = val; }
                    if (name == LgoAscLable.CoordinateType) { header.CoordinateType = val; }
                    if (name == LgoAscLable.ReferenceEllipsoid) { header.ReferenceEllipsoid = val; }
                    if (name == LgoAscLable.ProjectionSet) { header.ProjectionSet = val; }
                }

                return header;
            }
        }
    }
}
