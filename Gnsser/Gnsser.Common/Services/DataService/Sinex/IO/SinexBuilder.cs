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
    public class SinexBuilder
    {

      //  SinexFile SinexFile { get; set; }
        /// <summary>
        /// 转换为文本
        /// </summary>
        /// <param name="SinexFile"></param>
        /// <returns></returns>
        public static string ToSinex(SinexFile SinexFile)
        {
            string divLine = "*-------------------------------------------------------------------------------";
            StringBuilder sb = new StringBuilder();
            //line 1
            sb.AppendLine(SinexFile.Header.ToString());
            sb.AppendLine(divLine);
            if (SinexFile.FileReferBlock != null)
            {
                sb.AppendLine(SinexFile.FileReferBlock.ToString());
                sb.AppendLine(divLine);
            }
            if (SinexFile.FileCommentBlock != null && SinexFile.FileCommentBlock.HasItems)
            {
                sb.AppendLine(SinexFile.FileCommentBlock.ToString());
                sb.AppendLine(divLine);
            } if (SinexFile.InputHistoryBlock != null && SinexFile.InputHistoryBlock.HasItems)
            {
                sb.AppendLine(SinexFile.InputHistoryBlock.ToString());
                sb.AppendLine(divLine);
            } if (SinexFile.InputFilesBlock != null && SinexFile.InputFilesBlock.HasItems)
            {
                sb.AppendLine(SinexFile.InputFilesBlock.ToString());
                sb.AppendLine(divLine);
            } if (SinexFile.FileAcknowledgementBlock != null && SinexFile.FileAcknowledgementBlock.HasItems)
            {
                sb.AppendLine(SinexFile.FileAcknowledgementBlock.ToString());
                sb.AppendLine(divLine);
            } if (SinexFile.SolutionStattisticsBlock != null && SinexFile.SolutionStattisticsBlock.HasItems)
            {
                sb.AppendLine(SinexFile.SolutionStattisticsBlock.ToString());
                sb.AppendLine(divLine);
            } if (SinexFile.SatelliteIdBlock != null && SinexFile.SatelliteIdBlock.HasItems)
            {
                sb.AppendLine(SinexFile.SatelliteIdBlock.ToString());
                sb.AppendLine(divLine);
            } if (SinexFile.SatellitePhaseCenterBlock != null && SinexFile.SatellitePhaseCenterBlock.HasItems)
            {
                sb.AppendLine(SinexFile.SatellitePhaseCenterBlock.ToString());
                sb.AppendLine(divLine);
            } if (SinexFile.SiteIdBlock != null && SinexFile.SiteIdBlock.HasItems)
            {
                sb.AppendLine(SinexFile.SiteIdBlock.ToString());
                sb.AppendLine(divLine);
            } if (SinexFile.SiteReceiverBlock != null && SinexFile.SiteReceiverBlock.HasItems)
            {
                sb.AppendLine(SinexFile.SiteReceiverBlock.ToString());
                sb.AppendLine(divLine);
            } if (SinexFile.SiteAntennaBlock != null && SinexFile.SiteAntennaBlock.HasItems)
            {
                sb.AppendLine(SinexFile.SiteAntennaBlock.ToString());
                sb.AppendLine(divLine);
            } if (SinexFile.SiteGpsPhaseCenterBlock != null && SinexFile.SiteGpsPhaseCenterBlock.HasItems)
            {
                sb.AppendLine(SinexFile.SiteGpsPhaseCenterBlock.ToString());
                sb.AppendLine(divLine);
            } if (SinexFile.SiteEccentricityBlock != null && SinexFile.SiteEccentricityBlock.HasItems)
            {
                sb.AppendLine(SinexFile.SiteEccentricityBlock.ToString());
                sb.AppendLine(divLine);
            } if (SinexFile.SolutionEpochBlock != null && SinexFile.SolutionEpochBlock.HasItems)
            {
                sb.AppendLine(SinexFile.SolutionEpochBlock.ToString());
                sb.AppendLine(divLine);
            } if (SinexFile.SolutionAprioriBlock != null && SinexFile.SolutionAprioriBlock.HasItems)
            {
                sb.AppendLine(SinexFile.SolutionAprioriBlock.ToString());
                sb.AppendLine(divLine);
            } if (SinexFile.SolutionEstimateBlock != null && SinexFile.SolutionEstimateBlock.HasItems)
            {
                sb.AppendLine(SinexFile.SolutionEstimateBlock.ToString());
                sb.AppendLine(divLine);
            }
            if (SinexFile.SolutionNormalEquationVectorBlock != null && SinexFile.SolutionNormalEquationVectorBlock.HasItems)
            {
                sb.AppendLine(SinexFile.SolutionNormalEquationVectorBlock.ToString());
                sb.AppendLine(divLine);
            }
            if (SinexFile.SolutionMatrixEstimateCova != null && SinexFile.SolutionMatrixEstimateCova.HasItems)
            {
                sb.AppendLine(SinexFile.SolutionMatrixEstimateCova.ToString(BlockTitle.SOLUTION_MATRIX_ESTIMATE_L_COVA));
                sb.AppendLine(divLine);
            }
            if (SinexFile.SolutionMatrixAprioriCova != null && SinexFile.SolutionMatrixAprioriCova.HasItems)
            {
                sb.AppendLine(SinexFile.SolutionMatrixAprioriCova.ToString(BlockTitle.SOLUTION_MATRIX_APRIORI_L_COVA));
                sb.AppendLine(divLine);
            }

            sb.AppendLine("%ENDSNX");
            return sb.ToString();
        }

    
    
    }
}
