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
    public static class SinexFactory
    {
        public static ICollectionBlock<SiteGpsPhaseCenter> CreateSiteGpsPhaseCenterBlock(List<SiteGpsPhaseCenter> list = null) { return CreateBlock<SiteGpsPhaseCenter>(BlockTitle.SITE_GPS_PHASE_CENTER, list); }
        public static ICollectionBlock<SiteEccentricity> CreateSiteEccentricityBlock(List<SiteEccentricity> list = null) { return CreateBlock<SiteEccentricity>(BlockTitle.SITE_ECCENTRICITY, list); }
        public static ICollectionBlock<SiteAntenna> CreateSiteAntennaBlock(List<SiteAntenna> list = null) { return CreateBlock<SiteAntenna>(BlockTitle.SITE_ANTENNA, list); }
        public static ICollectionBlock<SatellitePhaseCenter> CreateSatellitePhaseCenterBlock(List<SatellitePhaseCenter> list = null) { return CreateBlock<SatellitePhaseCenter>(BlockTitle.SATELLITE_PHASE_CENTER, list); }
        public static ICollectionBlock<SatelliteId> CreateSatelliteIdBlock(List<SatelliteId> list = null) { return CreateBlock<SatelliteId>(BlockTitle.SATELLITE_ID, list); }
        public static ICollectionBlock<InputHistory> CreateInputHistoryBlock(List<InputHistory> list = null) { return CreateBlock<InputHistory>(BlockTitle.INPUT_HISTORY, list); }
        public static ICollectionBlock<SiteId> CreateSiteIdBlock(List<SiteId> list = null) { return CreateBlock<SiteId>(BlockTitle.SITE_ID, list); }
        public static ICollectionBlock<SiteReceiver> CreateSiteReceiverBlock(List<SiteReceiver> list = null) { return CreateBlock<SiteReceiver>(BlockTitle.SITE_RECEIVER, list); }

        public static ICollectionBlock<SolutionValue> CreateSolutionAprioriBlock(List<SolutionValue> list = null) { return CreateBlock<SolutionValue>(BlockTitle.SOLUTION_APRIORI, list); }
        public static ICollectionBlock<SolutionValue> CreateSolutionEstimateBlock(List<SolutionValue> list = null) { return CreateBlock<SolutionValue>(BlockTitle.SOLUTION_ESTIMATE, list); }
        public static ICollectionBlock<SolutionEpoch> CreateSolutionEpochBlock(List<SolutionEpoch> list = null) { return CreateBlock<SolutionEpoch>(BlockTitle.SOLUTION_EPOCHS, list); }
        public static ICollectionBlock<SolutionNomalEquationVector> CreateSolutionNormalEquationVectorBlock(List<SolutionNomalEquationVector> list = null) { return CreateBlock<SolutionNomalEquationVector>(BlockTitle.SOLUTION_NORMAL_EQUATION_VECTOR, list); }
        public static ICollectionBlock<SolutionStatistic> CreateSolutionStattisticsBlock(List<SolutionStatistic> list = null) { return CreateBlock<SolutionStatistic>(BlockTitle.SOLUTION_STATISTICS, list); }
       
        public static SolutionMatrixBlock CreateSolutionMatrixEstimateCova(List<MatrixLine> list = null) {  if (list == null) list = new List<MatrixLine>();return new SolutionMatrixBlock() { Label = BlockTitle.SOLUTION_MATRIX_ESTIMATE_L_COVA, Items = list }; }     
        public static SolutionMatrixBlock CreateSolutionMatrixAprioriCova(List<MatrixLine> list = null) { if (list == null) list = new List<MatrixLine>();return new SolutionMatrixBlock() {  Label = BlockTitle.SOLUTION_MATRIX_APRIORI_L_COVA, Items = list  }; }
        public static SolutionMatrixBlock CreateSolutionMatrixEstimateCova(double[][] matrix) { return CreateSolutionMatrixEstimateCova(SinexMatrixConvertor.GetMatrixLines(matrix)); }
        public static SolutionMatrixBlock CreateSolutionMatrixAprioriCova(double[][] matrix) { return CreateSolutionMatrixAprioriCova(SinexMatrixConvertor.GetMatrixLines(matrix)); }
   
        

        public static ICollectionBlock<T> CreateBlock<T>(string blockName, List<T> list = null) where T : IBlockItem, new() { if (list == null) list = new List<T>(); return new CollectionBlock<T>(blockName) { Items = list }; }
    }
}
