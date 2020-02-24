using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using  Geo.Coordinates;
using Geo.Utils;

namespace Gnsser.Data.Sinex
{
    /// <summary>
    /// Sinex Block 工厂。
    /// </summary>
    public static class SinexReader
    {


        public static List<SinexFile> Read(string[] pathes, bool skipMatrix = false)
        {
            List<SinexFile> files = new List<SinexFile>();
            foreach (var item in pathes)
            {
                files.Add(Read(item));

            }
            return files;
        }

        /// <summary>
        /// 读取解析 Sinex 文件。
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        public static SinexFile ParseText(string sinexText, string name = "SinexName", bool skipMatrix = false)
        {
            Stream stream = new MemoryStream(ASCIIEncoding.ASCII.GetBytes(sinexText));
            return Read(stream, name, skipMatrix);
        }

        /// <summary>
        /// 读取解析 Sinex 文件。
        /// </summary>
        /// <param name="path"></param>
        /// <param name="skipMatrix"></param>
        /// <returns></returns>
        public static SinexFile Read(string path, bool skipMatrix = false)
        {
            Stream stream = new FileStream(path, FileMode.Open);
            string name = Path.GetFileNameWithoutExtension(path);
            return Read(stream, name, skipMatrix);
        }
        /// <summary>
        ///  读取解析 Sinex 文件。
        /// </summary>
        /// <param name="stream"></param>
        /// <param name="skipMatrix"></param>
        /// <returns></returns>
        public static SinexFile Read(Stream stream, string name = "SinexName", bool skipMatrix = false)
        {
            SinexFile file = new SinexFile();
            file.Name = name;
            using (StreamReader reader = new StreamReader(stream))
            {
                string line = "";
                //第一行
                line = reader.ReadLine();
                file.Header = SinexFileHeader.Read(line);

                //第二行后
                while ((line = reader.ReadLine()) != null)
                {
                    if (line.StartsWith(CharDefinition.COMMENT)) continue;//注释
                    if (line.StartsWith(CharDefinition.TITLE_START))//+
                    {
                        string title = line.Substring(1).Trim();
                        switch (title)
                        {
                            case BlockTitle.FILE_REFERENCE: file.FileReferBlock = FileReferBlock.Read(reader); break;
                            case BlockTitle.FILE_COMMENT: file.FileCommentBlock = BlockLineFactory.Read<FileComment>(reader, title); break;
                            case BlockTitle.INPUT_HISTORY: file.InputHistoryBlock = BlockLineFactory.Read<InputHistory>(reader, title); break;
                            case BlockTitle.INPUT_FILES: file.InputFilesBlock = BlockLineFactory.Read<InputFile>(reader, title); break;
                            case BlockTitle.INPUT_ACKNOWLEDGEMENTS: file.FileAcknowledgementBlock = BlockLineFactory.Read<FileAcknowledgement>(reader, title); break;
                            case BlockTitle.SATELLITE_ID: file.SatelliteIdBlock = BlockLineFactory.Read<SatelliteId>(reader, title); break;
                            case BlockTitle.SATELLITE_PHASE_CENTER: file.SatellitePhaseCenterBlock = BlockLineFactory.Read<SatellitePhaseCenter>(reader, title); break;
                            case BlockTitle.SITE_ID: file.SiteIdBlock = BlockLineFactory.Read<SiteId>(reader, title); break;
                            case BlockTitle.SITE_RECEIVER: file.SiteReceiverBlock = BlockLineFactory.Read<SiteReceiver>(reader, title); break;
                            case BlockTitle.SITE_ANTENNA: file.SiteAntennaBlock = BlockLineFactory.Read<SiteAntenna>(reader, title); break;
                            case BlockTitle.SITE_GPS_PHASE_CENTER: file.SiteGpsPhaseCenterBlock = BlockLineFactory.Read<SiteGpsPhaseCenter>(reader, title); break;
                            case BlockTitle.SITE_ECCENTRICITY: file.SiteEccentricityBlock = BlockLineFactory.Read<SiteEccentricity>(reader, title); break;
                            case BlockTitle.SOLUTION_STATISTICS: file.SolutionStattisticsBlock = BlockLineFactory.Read<SolutionStatistic>(reader, title); break;
                            case BlockTitle.SOLUTION_EPOCHS: file.SolutionEpochBlock = BlockLineFactory.Read<SolutionEpoch>(reader, title); break;
                            case BlockTitle.SOLUTION_ESTIMATE: file.SolutionEstimateBlock = BlockLineFactory.Read<SolutionValue>(reader, title); break;
                            case BlockTitle.SOLUTION_APRIORI: file.SolutionAprioriBlock = BlockLineFactory.Read<SolutionValue>(reader, title); break;
                            case BlockTitle.SOLUTION_MATRIX_ESTIMATE_U_COVA:
                            case BlockTitle.SOLUTION_MATRIX_ESTIMATE_L_COVA: if (!skipMatrix) file.SolutionMatrixEstimateCova = (SolutionMatrixBlock)BlockLineFactory.Read<SolutionMatrixBlock, MatrixLine>(reader, title); break;
                            case BlockTitle.SOLUTION_MATRIX_APRIORI_U_COVA:
                            case BlockTitle.SOLUTION_MATRIX_APRIORI_L_COVA: if (!skipMatrix) file.SolutionMatrixAprioriCova = (SolutionMatrixBlock)BlockLineFactory.Read<SolutionMatrixBlock, MatrixLine>(reader, title); break;
                            case BlockTitle.SOLUTION_NORMAL_EQUATION_VECTOR: if (!skipMatrix) file.SolutionNormalEquationVectorBlock = BlockLineFactory.Read<SolutionNomalEquationVector>(reader, title); break;
                            case BlockTitle.SOLUTION_NORMAL_EQUATION_MATRIX_U:
                            case BlockTitle.SOLUTION_NORMAL_EQUATION_MATRIX_L: if (!skipMatrix) file.SolutionNormalEquationMatrix = (SolutionMatrixBlock)BlockLineFactory.Read<SolutionMatrixBlock, MatrixLine>(reader, title); break;

                            default:
                                break;
                        }
                    }
                }
            }
            return file;
        }

    
    }
}
