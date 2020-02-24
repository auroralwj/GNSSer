using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Geo.Utils;

namespace Gnsser.Data.Sinex
{
    /// <summary>
    /// Sinex 文件合并器。
    /// </summary>
    public class SinexMerger
    {
        /// <summary>
        /// 合并两个文件的基础信息，如测站名、接收机、天线等信息。
        /// 协方差、平差结果等需要计算后添加进来。
        /// 只有两个文件都有的信息块才会被合并。
        /// </summary>
        /// <param name="fileA"></param>
        /// <param name="fileB"></param>
        /// <returns></returns>
        public static SinexFile EmergeBasic(SinexFile fileA, SinexFile fileB)
        {
            //创建 Sinex 结果文件
            SinexFile result = new SinexFile("Gnsser");
            SinexMerger.MergeBlock(result.SiteIdBlock, fileA.SiteIdBlock, fileB.SiteIdBlock);
            SinexMerger.MergeBlock(result.SiteReceiverBlock, fileA.SiteReceiverBlock, fileB.SiteReceiverBlock);
            SinexMerger.MergeBlock(result.SiteAntennaBlock, fileA.SiteAntennaBlock, fileB.SiteAntennaBlock);
            SinexMerger.MergeBlock(result.SiteEccentricityBlock, fileA.SiteEccentricityBlock, fileB.SiteEccentricityBlock);
            SinexMerger.MergeBlock(result.SatelliteIdBlock, fileA.SatelliteIdBlock, fileB.SatelliteIdBlock);
            SinexMerger.MergeBlock(result.SatellitePhaseCenterBlock, fileA.SatellitePhaseCenterBlock, fileB.SatellitePhaseCenterBlock);
            SinexMerger.MergeBlock(result.InputHistoryBlock, fileA.InputHistoryBlock, fileB.InputHistoryBlock);
            SinexMerger.MergeBlock(result.InputFilesBlock, fileA.InputFilesBlock, fileB.InputFilesBlock);
            SinexMerger.MergeBlock(result.FileCommentBlock, fileA.FileCommentBlock, fileB.FileCommentBlock);
            SinexMerger.MergeBlock(result.FileAcknowledgementBlock, fileA.FileAcknowledgementBlock, fileB.FileAcknowledgementBlock);
            SinexMerger.MergeBlock(result.InputHistoryBlock, fileA.InputHistoryBlock, fileB.InputHistoryBlock);
            SinexMerger.MergeBlock(result.SiteGpsPhaseCenterBlock, fileA.SiteGpsPhaseCenterBlock, fileB.SiteGpsPhaseCenterBlock);
            SinexMerger.MergeBlock(result.SolutionEpochBlock, fileA.SolutionEpochBlock, fileB.SolutionEpochBlock);

            return result;
        }

        /// <summary>
        /// 两个文件合并，本方法只适合计算站点不重复,且不相关的情况。
        /// 信息只是简单的叠加。只有两个文件都有的信息块才会被合并。
        /// </summary>
        /// <param name="fileB"></param>
        /// <param name="eraseNonCoord"> 是否清理非坐标的值和对应矩阵</param>
        public static SinexFile Merge(SinexFile fileA, SinexFile fileB, bool eraseNonCoord = true)
        {
            if (eraseNonCoord)
            {
                fileA.CleanNonCoordSolutionValue();
                fileB.CleanNonCoordSolutionValue();
            }

            SinexFile newFile = EmergeBasic(fileA, fileB);

            //合并解算结果，不更新标准差
            SinexMerger.MergeSolutionValue(newFile.SolutionEstimateBlock, fileA.SolutionEstimateBlock, fileB.SolutionEstimateBlock);
            SinexMerger.MergeSolutionValue(newFile.SolutionAprioriBlock, fileA.SolutionAprioriBlock, fileB.SolutionAprioriBlock);

            //统计数据的合并, 需要做一些叠加工作。
            SinexStatistic statisticA = fileA.GetStatistic();
            SinexStatistic statisticB = fileB.GetStatistic();

            if (fileA.SolutionStattisticsBlock != null && fileB.SolutionStattisticsBlock != null)
            {
                newFile.SolutionStattisticsBlock.Items = fileA.GetStatistic().Merge(fileB.GetStatistic()).GetSolutionStatistics();
            }
            SinexStatistic statisticNew = newFile.GetStatistic();
            double varFactorA = statisticNew.VarianceOfUnitWeight / statisticA.VarianceOfUnitWeight;
            double varFactorB = statisticNew.VarianceOfUnitWeight / statisticB.VarianceOfUnitWeight;


            //合并矩阵，协方差矩阵,前面已经判断并重新布置matrix了，此处只管合并。
            MergeSolutionMatrix(newFile.SolutionMatrixEstimateCova, fileA.SolutionMatrixEstimateCova, varFactorA, fileB.SolutionMatrixEstimateCova, varFactorB, newFile.SolutionEstimateBlock);
            MergeSolutionMatrix(newFile.SolutionMatrixAprioriCova, fileA.SolutionMatrixAprioriCova, varFactorA, fileB.SolutionMatrixAprioriCova, varFactorB, newFile.SolutionAprioriBlock);

            return newFile;
        }

        /// <summary>
        /// 合并协方差阵，并更新标准差。
        /// </summary>
        /// <param name="merge"></param>
        /// <param name="solutionMatrixBlockA"></param>
        /// <param name="varFactorA"></param>
        /// <param name="solutionMatrixBlockB"></param>
        /// <param name="varFactorB"></param>
        /// <param name="SolutionValueBlockMerged"></param>
        public static void MergeSolutionMatrix(
            SolutionMatrixBlock merge,
            SolutionMatrixBlock solutionMatrixBlockA,
            double varFactorA,
            SolutionMatrixBlock solutionMatrixBlockB,
            double varFactorB,
            ICollectionBlock<SolutionValue> SolutionValueBlockMerged)
        {
            if (solutionMatrixBlockA != null && solutionMatrixBlockA.Items.Count != 0)
            {
                merge.Items =
                    SinexMatrixConvertor.Merge(
                    solutionMatrixBlockA.Items, varFactorA,
                    solutionMatrixBlockB.Items, varFactorB);

                UpdateStdDev(merge, SolutionValueBlockMerged);
            }
        }

        /// <summary>
        /// 更新标准差
        /// </summary>
        /// <param name="merge"></param>
        /// <param name="SolutionValueBlockMerged"></param>
        public static void UpdateStdDev(SolutionMatrixBlock merge, ICollectionBlock<SolutionValue> SolutionValueBlockMerged)
        {
            //更新标准差
            double[] steDevs = SinexMatrixConvertor.GetStdDevs(merge.Items);
            int i = 0;
            foreach (var item in SolutionValueBlockMerged.Items)
            {
                item.StdDev = steDevs[i];
                i++;
            }
        }

        /// <summary>
        /// 清除非坐标的信息。同时清理对应的矩阵。
        /// </summary>
        /// <param name="solutionValues"></param>
        /// <param name="matrixBlock"></param>
        public static void CleanNonCoordSolutionValue(ICollectionBlock<SolutionValue> solutionValues, SolutionMatrixBlock matrixBlock)
        {
            if (solutionValues == null || solutionValues.Items == null || solutionValues.Items.Count == 0) return;

            List<int> tobeRemoveIndexes = new List<int>();
            int index = 0;
            foreach (var item in solutionValues.Items)
            {
                if (item.ParameterType != ParameterType.STAX
                    && item.ParameterType != ParameterType.STAY
                    && item.ParameterType != ParameterType.STAZ)
                    tobeRemoveIndexes.Add(index);
                index++;
            }

            for (int i = tobeRemoveIndexes.Count - 1; i >= 0; i--)
            {
                solutionValues.Items.RemoveAt(tobeRemoveIndexes[i]);
            }

            //清理对应的矩阵
            if (matrixBlock == null || matrixBlock.Items.Count == 0) return;
            double[][] matrix = SinexMatrixConvertor.GetMatrix(matrixBlock.Items);
            double[][] cleanedMatrix = MatrixUtil.ShrinkMatrix(matrix, tobeRemoveIndexes);

            //更新
            matrixBlock.Items = SinexMatrixConvertor.GetMatrixLines(cleanedMatrix);
        }
        /// <summary>
        /// 将a和b合并到merged里面，先合并a，在合并b，若b中有相同的则剔除。
        /// </summary>
        /// <typeparam name="TProduct"></typeparam>
        /// <param name="merged"></param>
        /// <param name="a"></param>
        /// <param name="b"></param>
        public static void MergeBlock<T>(ICollectionBlock<T> merged, ICollectionBlock<T> a, ICollectionBlock<T> b) where T : IBlockItem, new()
        {
            if (a == null || b == null) return;
            merged.Items.Clear();
            merged.Items.AddRange(a.Items);
            foreach (var item in b.Items)
            {
                if (!merged.Items.Contains(item)) merged.Items.Add(item);
            }
        }
        /// <summary>
        /// /合并解算结果，更新编号，但不更新标准差
        /// </summary>
        /// <param name="SolutionValueBlockMerged"></param>
        /// <param name="SolutionValueBlockA"></param>
        /// <param name="SolutionValueBlockB"></param>
        public static void MergeSolutionValue(ICollectionBlock<SolutionValue> SolutionValueBlockMerged, ICollectionBlock<SolutionValue> SolutionValueBlockA, ICollectionBlock<SolutionValue> SolutionValueBlockB)
        {
            if (SolutionValueBlockA != null && SolutionValueBlockB != null)
            {
                SolutionValueBlockMerged.Items.Clear();
                SolutionValueBlockMerged.Items.AddRange(SolutionValueBlockA.Items);
                foreach (var item in SolutionValueBlockB.Items)
                {
                    if (!SolutionValueBlockMerged.Items.Contains(item))
                    {
                        item.Index = SolutionValueBlockMerged.Items.Count + 1;
                        SolutionValueBlockMerged.Items.Add(item);
                    }
                }
            }
        }

      

    }
}
