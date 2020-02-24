//2014.07.02, Cui Yang, created, 质量控制
//2015.04.02, Cui Yang, 该类应启用，保证数据的连续性

using System;
using System.Collections.Generic;
using System.Linq;
using Gnsser.Domain;
using System.Text;
using Gnsser.Times;
using Gnsser.Service;
using Geo.Coordinates;
using Gnsser.Data.Rinex;
using Gnsser;
using Geo.Times; 

namespace Gnsser.Filter
{

    /// <summary>
    /// keep track of satellite arcs caused by cycle slips
    /// </summary>
    public class SatArcMarkerFilter:EpochInfoReviser
    {
        /// <summary>
        /// Cycle slip flag to be watched. 查找某频率是否发生周跳的标识
        /// </summary>
        FrequenceType WatchCsFlag { get; set; }

        /// <summary>
        ///flag indicating if unstable satellites will be deleted.
        /// </summary>
        private bool DeleteUnstableSat { get; set; }

        /// <summary>
        /// number of fraction since arc change that a satellite will be considered as unstable.
        /// </summary>
        private double UnstablePeriod { get; set; }

        /// <summary>
        /// Dictionary holding information regarding every satellite
        /// </summary>
        private Dictionary<SatelliteNumber, int> SatArcDic = new Dictionary<SatelliteNumber, int>();
       
        /// <summary>
        /// Dictionary holding information about epoch of last arc change
        /// </summary>
        private Dictionary<SatelliteNumber, Time> SatArcChangeDic = new Dictionary<SatelliteNumber, Time>();
       
        /// <summary>
        /// Dictionary keeping track if this satellite is new or not
        /// </summary>
        private Dictionary<SatelliteNumber, bool> SatIsNewDic = new Dictionary<SatelliteNumber, bool>();



        /// <summary>
        /// Default constructor.
        /// </summary>
        public SatArcMarkerFilter()
        {
            WatchCsFlag = FrequenceType.A; //L1 频率的周跳标识
            DeleteUnstableSat = false;
            UnstablePeriod = 32.0;
        }

      
        /// <summary>
        /// Common constructor
        /// </summary>
        /// <param name="watchFlag">cycle slip flag to be watched，默认的是L1频率的周跳标识</param>
        /// <param name="delUnstableSats">whether unstable satellites will be deleted.</param>
        /// <param name="unstableTime">unmber of fraction since last arc change that a satellite will be considered as unstable</param>
        public SatArcMarkerFilter(FrequenceType watchFlag, bool delUnstableSats, double unstableTime)
        {
            WatchCsFlag = watchFlag; //默认的是L1频率的周跳标识
            DeleteUnstableSat = delUnstableSats;
            UnstablePeriod = unstableTime;  //unstableTime value
        }

        /// <summary>
        /// SatArcMarkerFilter核心
        /// </summary>
        /// <param name="gData"></param>
        /// <returns></returns>
        public override bool Revise(ref EpochInformation gData)
        {           
            //   Time epoch = gData.CorrectedTime;
            Time epoch = gData.ReceiverTime;
            
            List<SatelliteNumber> satRejectedSet = new List<SatelliteNumber>();
            
            double flag = 0.0;
                        
            // Loop through all the satellites
            foreach (var sat in gData)
            {
                SatelliteNumber prn = sat.Prn;

                try
                {
                    //try to extract to CS flag value
                    flag = sat[WatchCsFlag].IsCycleSliped?0:1;
                }
                catch
                {
                    //if flag is missing, then schedule this satellite from removal
                    satRejectedSet.Add(prn);
                    continue;
                }
                               
                //check if satellite currently has entries
                if (!SatArcDic.ContainsKey(prn))
                { 
                    //if it doesn't have an entry, insert one
                    SatArcDic.Add(prn, 0);
                    SatArcChangeDic.Add(prn, Time.StartOfMjd);

                    //this is a new satellite
                    SatIsNewDic.Add(prn, true);
                }
                
                //check if this satellite inside unstable period
                bool insideUnstable = false;
                if ((double)(Math.Abs(epoch - SatArcChangeDic[prn])) <= UnstablePeriod) { insideUnstable = true; }
                
                //satellites can be new only once, and having at least once a flag > 0.0 outside 'unstablePeriod' will make them old.
                if (SatIsNewDic[prn] && !insideUnstable && flag <= 0.0)
                {
                    SatIsNewDic[prn] = false;
                }
            
                //check if there was a cycle slip
                if (flag > 0.0)
                { 
                    //increment the vale of satArc
                    SatArcDic[prn] += 1;

                    //update arc chage epoch
                    SatArcChangeDic[prn] = epoch;

                    //if delete unstable satellites, we must do it also when arc changes, but only if this SV is not new
                    if (DeleteUnstableSat && !SatIsNewDic[prn])
                    {
                        //if flag is missing, then schedule this satellite from removal
                        satRejectedSet.Add(prn);
                    }
                }
                //Test if delete unstable satellites. Only do it if satellite is NOT new and we are inside unstable period
                if (insideUnstable && DeleteUnstableSat && !SatIsNewDic[prn])
                {                   
                    satRejectedSet.Add(prn);
                }
               //insert satellite arc number
                sat.ArcMarker = SatArcDic[prn];
            }

            //Remove satellites with missing satData
            gData.Disable(satRejectedSet);
            return true;
        }

     
    }
}
