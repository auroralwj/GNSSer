//2014.08.29, czs, edit, 行了继承设计

using System;
using System.Collections.Generic;
using System.Text;
using Geo.Coordinates;
using Gnsser.Data.Rinex;
using Geo.Algorithm;
using Gnsser.Data.Sinex;
using Gnsser.Domain;
using Geo.Algorithm;
using Geo.Algorithm.Adjust;


namespace Gnsser.Service
{
    /// <summary>
    /// 模糊度固定，仅对GPS系统
    /// </summary>
    public class Ppp_AR
    {
        public double MIN_ARC_GAP = 300.0; //min arc gap(s)
        public double CONST_AMB = 0.001;  //constraint to fixed ambiguity
        public double THRES_RES = 0.3;  //threashold of residuals test (m)
        public double LOG_PI = 1.14472988584940017;  //log(pi)
        public double SQRT2 = 1.41421356237309510;  //sqrt(2)


        //以下参数需人工设置
        //measurements errors(1-sigma)
        double err1 = 0.003;    //err[1] errphase m
        double err2 = 0.003;   //err[2]  errphaseel m
        double err3 = 0;       //err[3] errphasebl m/10km
        double err4 = 10;      //err[4] errdoppler Hz

        double std0 = 30;     //std[0] stdbias m
        double std1 = 0.03;   //std[1] stdiono m
        double std2 = 0.3;    //std[2] stdtrpp m



        double eratio0 = 100.0; //eratio[0]   code_phase_error_ratio_L1
        double eratio1 = 100.0; //eratio[1]  code_phase_error_ratio_L2

        double thresar0 = 3;  //thresar[0] arthres  min_ration_to_fix_ambiguity
        //double min_ration_to_fix_ambiguity = 3;
        double thresar1 = 0.9999;  //min_confidence_to_fix_amb
        //double min_confidence_to_fix_amb = 0.9999;
        double thresar2 = 0.2;//max_FCB_to_fix_amb
        //double max_FCB_to_fix_amb = 0.2;


        /// <summary>
        /// 默认构造函数
        /// </summary>
        public Ppp_AR()
        { }

        /// <summary>
        /// 精密单点定位结果构造函数。
        /// </summary>
        /// <param name="receiverInfo">接收信息</param>
        /// <param name="Adjustment">平差</param>
        /// <param name="paramNames">参数名称</param>
        public int Process(EpochInformation receiverInfo, AdjustResultMatrix Adjustment)
        {
            int m = 0, stat = 0;
            int n = receiverInfo.EnabledPrns.Count;

            int[] NW = new int[n * n];
            int[] sat1 = new int[n * n];
            int[] sat2 = new int[n * n];


            //average LC
            ComputeAverage_LC(receiverInfo);

            //fix wide-lane ambiguity 
            for (int i = 0; i < n - 1; i++)
            {
                for (int j = i + 1; j < n; j++)
                {


                    //test already fixed
                    if (ambc[receiverInfo[i].Prn].flags[receiverInfo[j].Prn] != 0 &&
                       ambc[receiverInfo[j].Prn].flags[receiverInfo[i].Prn] != 0)
                    {
                        continue;
                    }


                    sat1[m] = receiverInfo[i].Prn.PRN;  //只考虑GPS的固定
                    sat2[m] = receiverInfo[j].Prn.PRN;

                    if (fix_amb_WL(receiverInfo, sat1[m], sat2[m], ref NW[m]) == 1) m++;
                }

            }


            //fix narrow-lane ambiguity
            //AR mode: PPP_AR (目前实现的)
            stat = fxi_amb_NL_ROUND(receiverInfo, Adjustment, sat1, sat2, NW, m);
            //AR mode: PPP_AR ILS




            return stat;
        }

        //wide_lane_GPS_satellite_bias, 应该通过文件读取进来，读取Wide_lane_GPS_satellite_biais.wsb
        public double[] wlbias = new double[] { 
             0.078,-0.306,-0.673,-0.331,-0.641,
            -0.919,-0.046,-0.541,-0.332,-0.865,
            -0.385,-0.396,-0.911,-0.124,-0.211,
            -0.196,-0.332,-0.390,-0.239,0.262,
            -0.648,-1.084,-0.973,0.094,-0.907,
            -0.115,-0.649,-0.487,-0.092,-0.850,
            -0.164,-0.134,0.000,0.000,0.000,
             0.000,0.000,0.000,0.000,0.000};

        /// <summary>
        /// fix wide-lane ambiguity
        /// </summary>
        /// <param name="receiverInfo"></param>
        /// <param name="satN1"></param>
        /// <param name="satN2"></param>
        /// <param name="NW"></param>
        /// <returns></returns>
        private int fix_amb_WL(EpochInformation receiverInfo, int satN1, int satN2, ref int NW)
        {
            double BW, vW, lam_WL = lam_LC(1, -1, 0);

            SatelliteNumber sat1 = new SatelliteNumber();
            SatelliteNumber sat2 = new SatelliteNumber();
            sat1.PRN = satN1;
            sat1.SatelliteType = SatelliteType.G;
            sat2.PRN = satN2;
            sat2.SatelliteType = SatelliteType.G;


            ambc_t amb1 = ambc[sat1];
            ambc_t amb2 = ambc[sat2];

            //根据Ge的论文，为避免估计偏差，不采纳观测时间小于20分钟的
            if (amb1.n[0] <= 40 || amb2.n[0] <= 40)
            {
                return 0;
            }



            //wide-lane ambiguity(以周为单位）
            BW = (amb1.LC[0] - amb2.LC[0]) / lam_WL + wlbias[sat1.PRN - 1] - wlbias[sat2.PRN - 1];



            //就近取整
            NW = (int)Math.Floor(BW + 0.5);

            double NW0 = Math.Round(BW);
            if (NW0 != NW)
            {
                //
            }


            //variance of wide-lane ambiguity
            vW = (amb1.LCv[0] / amb1.n[0] + amb2.LCv[0] / amb2.n[0]) / (lam_WL * lam_WL);



            //validation of integer wide-lane ambiguity
            if (Math.Abs(NW - BW) <= thresar2 && conffunc(NW, BW, Math.Sqrt(vW)) >= thresar1)
            {
                return 1;
            }
            else
            {
                return 0;
            }
        }


        private int fxi_amb_NL_ROUND(EpochInformation receiverInfo, AdjustResultMatrix adjustment, int[] satN1, int[] satN2, int[] NW, int n)
        {
            if (n <= 0) return 0;
            double C1, C2, B1, v1, BC, v, vc;
            double[] NC, var;
            double lam_NL = lam_LC(1, 1, 0);
            double lam1, lam2;
            int i, j, k, m = 0, N1, stat;

            lam1 = receiverInfo.First.FrequenceA.Frequence.WaveLength;
            lam2 = receiverInfo.First.FrequenceB.Frequence.WaveLength;

            C1 = (lam2 * lam2) / (lam2 * lam2 - lam1 * lam1);
            C2 = -(lam1 * lam1) / (lam2 * lam2 - lam1 * lam1);

            NC = new double[n]; var = new double[n];

            Vector vector = new Vector(adjustment.Estimated.OneDimArray);
            double[][] P = adjustment.Estimated.InverseWeight.Array;
            //for (int time = 5; time < length; time++)
            //{
            //    SatelliteNumber satelliteType = receiverInfo[time - 5].Prn;
            //    double val = vector[time];
            //    AmbiguityDic.Add(satelliteType, val);

            //}


            for (i = 0; i < n; i++)
            {
                SatelliteNumber sat1 = new SatelliteNumber();
                SatelliteNumber sat2 = new SatelliteNumber();
                sat1.PRN = satN1[i];
                sat1.SatelliteType = SatelliteType.G;
                sat2.PRN = satN2[i];
                sat2.SatelliteType = SatelliteType.G;


                j = receiverInfo.EnabledPrns.IndexOf(sat1); //模糊度参数的索引
                k = receiverInfo.EnabledPrns.IndexOf(sat2); //模糊度参数的索引


                //narrow-lane ambiguity
                B1 = (vector[j + 5] - vector[k + 5] + C2 * lam2 * NW[i]) / lam_NL;

                N1 = (int)Math.Floor(B1 + 0.5);

                double N10 = Math.Round(B1);
                if (N10 != N1)
                {
                    //
                }



                //variance of narrow-lane ambiguity
                var[m] = P[j + 5][j + 5] + P[k + 5][k + 5] - 2.0 * P[k + 5][j + 5];
                v1 = var[m] / (lam_NL * lam_NL);


                //validation of narrow-lane ambiguity
                if (Math.Abs(N1 - B1) > thresar2 || conffunc(N1, B1, Math.Sqrt(v1)) < thresar1)
                {
                    continue;
                }

                //iono-free ambiguity (m) 固定后的无电离层模糊度，单位 米
                BC = C1 * lam1 * N1 + C2 * lam2 * (N1 - NW[i]);


                //check residuals
                v = adjustment.PostfitResidual[receiverInfo.EnabledSatCount + j] - adjustment.PostfitResidual[receiverInfo.EnabledSatCount + k];
                //residuals of carrier-phase (m)???
                double v0 = adjustment.ObsMatrix.Observation[receiverInfo.EnabledSatCount + j] - adjustment.ObsMatrix.Observation[receiverInfo.EnabledSatCount + k];

                if (v0 != v)
                {

                }
                vc = v + (BC - (vector[j + 5] - vector[k + 5]));
                if (Math.Abs(vc) > THRES_RES) continue;

                satN1[m] = satN1[i];
                satN2[m] = satN2[i];

                NC[m] = BC;
                m++;

            }


            //select fixed ambiguities by dependancy check
            m = sel_amb(satN1, satN2, NC, var, m);



            if (m >= 3)
            {
                double factor0 = 0.0;

                //fixed solution 
                WeightedVector Estimated = fix_sol(receiverInfo, adjustment, satN1, satN2, NC, m, ref factor0);

                double factor = adjustment.VarianceOfUnitWeight;

                if (factor0 < factor)
                {
                }
                else
                {
                }


                int nx = adjustment.Estimated.Count; //参数个数  n*1

                //set solution
                for (i = 0; i < nx; i++)
                {
                    adjustment.Estimated[i] = Estimated[i];
                    for (j = 0; j < nx; j++)
                    {
                        adjustment.Estimated.InverseWeight[i, j] = adjustment.Estimated.InverseWeight[j, i] = Estimated.InverseWeight[i, j];
                    }
                }



                return 1;
            }
            else
                return 0;

        }

        private WeightedVector fix_sol(EpochInformation receiverInfo, AdjustResultMatrix adjustment, int[] satN1, int[] satN2, double[] NC, int n, ref double factor0)
        {
            int i, j, k;
            double[] v = new double[n];  //m * 1

            int nx = adjustment.Estimated.Count; //参数个数  n*1

            double[][] H = new double[nx][]; for (i = 0; i < nx; i++) H[i] = new double[n];  //n * m
            double[][] R = new double[n][]; for (i = 0; i < n; i++) R[i] = new double[n];   //m * m


            SatelliteNumber sat1 = new SatelliteNumber();
            SatelliteNumber sat2 = new SatelliteNumber();


            //constraints to fixed ambiguities
            for (i = 0; i < n; i++)
            {

                sat1.PRN = satN1[i];
                sat1.SatelliteType = SatelliteType.G;
                sat2.PRN = satN2[i];
                sat2.SatelliteType = SatelliteType.G;

                j = receiverInfo.EnabledPrns.IndexOf(sat1); //模糊度参数的索引
                k = receiverInfo.EnabledPrns.IndexOf(sat2); //模糊度参数的索引

                v[i] = NC[i] - (adjustment.Estimated.OneDimArray[j + 5] - adjustment.Estimated.OneDimArray[k + 5]);
                // v[time] = NC[time] - (adjustment.Estimated[j + 5] - adjustment.Estimated[k + 5]);
                H[j + 5][i] = 1.0;
                H[k + 5][i] = -1.0;
                R[i][i] = CONST_AMB * CONST_AMB;
            }


            //update states with constraints
            WeightedVector Estimated = filter(adjustment.Estimated.OneDimArray, adjustment.Estimated.InverseWeight.Array, H, v, R, ref factor0);


            //set flags
            for (i = 0; i < n; i++)
            {
                sat1.PRN = satN1[i];
                sat1.SatelliteType = SatelliteType.G;
                sat2.PRN = satN2[i];
                sat2.SatelliteType = SatelliteType.G;

                ambc[sat1].flags[sat2] = 1;
                ambc[sat2].flags[sat1] = 1;
            }

            return Estimated;
        }


        /// <summary>
        /// update states with constraints
        /// </summary>
        /// <param name="x">states vector (n * 1)</param>
        /// <param name="P">covariance matrix of states (n * n)</param>
        /// <param name="H">transpose of design matrix (n*m)</param>
        /// <param name="v">innovation (measurement -model ) (m *1 )</param>
        /// <param name="R">covariance matrix of measurement error (m * m)</param>
        /// <returns></returns>
        private WeightedVector filter(double[] x, double[][] P, double[][] H, double[] v, double[][] R, ref double factor0)
        {
            Vector tx = new Vector(x);
            Vector tv = new Vector(v);

            IMatrix P_ = new ArrayMatrix(P);
            IMatrix H_ = new ArrayMatrix(H);
            IMatrix R_ = new ArrayMatrix(R);

            IMatrix x_ = new ArrayMatrix(tx);
            IMatrix v_ = new ArrayMatrix(tv);

            IMatrix H_T = H_.Transposition;



            //IMatrix P_o = R_.GetInverse();
            //IMatrix P1 = P_.GetInverse();

            //IMatrix invTemp = (H_.Multiply(P_o).Multiply(H_T)).Plus(P1);

            //// Compute the a posteriori error covariance matrix
            //IMatrix CovaOfEstParam = invTemp.GetInverse();

            //IMatrix EstParam = CovaOfEstParam.Multiply(H_.Multiply(P_o).Multiply(v_).Plus(P1.Multiply(x_)));


            IMatrix QL = R_.Plus(H_T.Multiply(P_).Multiply(H_));
            IMatrix J = (P_.Multiply(H_)).Multiply(QL.GetInverse());
            IMatrix dX = J.Multiply(v_);
            IMatrix QdX = J.Multiply(H_T).Multiply(P_);

            IMatrix newX = x_.Minus(dX); // CovaOfEstParam.Multiply(AT.Multiply(P_o).Multiply(observation).Plus(P1.Multiply(PredictParam)));

            IMatrix newP = P_.Minus(QdX);

            IMatrix V = H_T.Multiply(dX).Minus(v_);
            IMatrix VTPV = V.Transposition.Multiply(R_.GetInverse()).Multiply(V).Plus((dX.Transposition).Multiply(P_.GetInverse()).Multiply(dX));

            //double det02 = VTPV[0, 0] / control.RowCount;
            //double det0 = Math.Sqrt(det02);

            factor0 = VTPV[0, 0] / H_T.RowCount;



            //IMatrix AT = control.Transposition;
            //IMatrix P_o = covaOfModelNoice.GetInverse();
            //IMatrix P1 = CovaOfPredictParam.GetInverse();
            //IMatrix invTemp = AT.Multiply(P_o).Multiply(control).Plus(P1);

            //// Compute the a posteriori error covariance matrix
            //IMatrix CovaOfEstParam = invTemp.GetInverse();
            //IMatrix EstParam = CovaOfEstParam.Multiply(AT.Multiply(P_o).Multiply(observation).Plus(P1.Multiply(PredictParam)));


            IMatrix Tmp = (H_T.Multiply(P_)).Multiply(H_).Plus(R_);


            IMatrix K = P_.Multiply(H_).Multiply(Tmp.GetInverse());

            IMatrix xp = x_.Plus(K.Multiply(v_)); // CovaOfEstParam.Multiply(AT.Multiply(P_o).Multiply(observation).Plus(P1.Multiply(PredictParam)));

            IMatrix Pp = P_.Minus(K.Multiply(H_T).Multiply(P_));



            WeightedVector Estimated = new WeightedVector(xp, Pp);


            WeightedVector Estimated0 = new WeightedVector(newX, newP);

            //  return Estimated; //OK
            return Estimated0; //OK

        }





        /// <summary>
        /// select fixed ambiguities
        /// </summary>
        /// <param name="sat1"></param>
        /// <param name="sat2"></param>
        /// <param name="N"></param>
        /// <param name="var"></param>
        /// <param name="n"></param>
        /// <returns></returns>
        private int sel_amb(int[] sat1, int[] sat2, double[] N, double[] var, int n)
        {
            int i, j, max_flag = 0;
            int[] flgs = new int[GnssConst.MaxSat];  //卫星编号PRN的大小对应的位置为该卫星的状态标记

            //sort by variance
            for (i = 0; i < n; i++)
            {
                for (j = 1; j < n - i; j++)
                {
                    if (var[j] >= var[j - 1]) continue;
                    int tmp1 = sat1[j]; sat1[j] = sat1[j - 1]; sat1[j - 1] = tmp1;
                    int tmp2 = sat2[j]; sat2[j] = sat2[j - 1]; sat2[j - 1] = tmp2;
                    double tmp3 = N[j]; N[j] = N[j - 1]; N[j - 1] = tmp3;
                    double tmp4 = var[j]; var[j] = var[j - 1]; var[j - 1] = tmp4;
                }
            }

            //select linearly independent satellite pair
            for (i = j = 0; i < n; i++)
            {
                if (is_depend(sat1[i], sat2[i], flgs, ref max_flag) != 1) continue;
                sat1[j] = sat1[i];
                sat2[j] = sat2[i];
                N[j] = N[i];
                var[j] = var[i];
                j++;
            }

            return j;
        }

        //linear dependency check
        private int is_depend(int sat1, int sat2, int[] flgs, ref int max_flg)
        {
            int i;
            if (flgs[sat1 - 1] == 0 && flgs[sat2 - 1] == 0)
            {
                flgs[sat1 - 1] = flgs[sat2 - 1] = ++(max_flg);
            }
            else if (flgs[sat1 - 1] == 0 && flgs[sat2 - 1] != 0)
            {
                flgs[sat1 - 1] = flgs[sat2 - 1];
            }
            else if (flgs[sat1 - 1] != 0 && flgs[sat2 - 1] == 0)
            {
                flgs[sat2 - 1] = flgs[sat1 - 1];
            }
            else if (flgs[sat1 - 1] > flgs[sat2 - 1])
            {
                for (i = 0; i < GnssConst.MaxSat; i++) if (flgs[i] == flgs[sat2 - 1]) flgs[i] = flgs[sat1 - 1];
            }
            else if (flgs[sat1 - 1] < flgs[sat2 - 1])
            {
                for (i = 0; i < GnssConst.MaxSat; i++) if (flgs[i] == flgs[sat1 - 1]) flgs[i] = flgs[sat2 - 1];
            }
            else return 0; //linear dependent 线性相关
            return 1;
        } 


        /// <summary>
        /// average LC
        /// </summary>
        /// <param name="receiverInfo"></param>
        private void ComputeAverage_LC(EpochInformation receiverInfo)
        {
            //average LC
            ambc_t amb = new ambc_t();

            double LC1, LC2, LC3, var1, var2, var3, sig;


            foreach (var sat in receiverInfo)//分别对指定卫星进行改正
            {

                SatelliteNumber prn = sat.Prn;
                if (sat.Prn.SatelliteType != SatelliteType.G) continue;

                LC1 = sat.Combinations.MwRangeCombination.Value; //双频

                //if (sat.FrequenceC != null)
                //{
                //    double LC11 = sat.CombinationBuilder.WideBandL1L2PhaseComb.Value - sat.CombinationBuilder.WideBandP1P2PhaseComb.Value;

                //    LC2 = sat.CombinationBuilder.WideBandL2L5PhaseComb.Value - sat.CombinationBuilder.WideBandP2P5PhaseComb.Value;

                //    LC3 = sat.CombinationBuilder.WideBandL1L2L5PhaseComb.Value - sat.CombinationBuilder.WideBandP1P2PhaseComb.Value;

                //}
                //else
                //{
                double freqA = sat.FrequenceA.Frequence.Value;
                double freqB = sat.FrequenceB.Frequence.Value;




                double valA = sat.FrequenceA.PhaseRange.CorrectedValue;
                double valB = sat.FrequenceB.PhaseRange.CorrectedValue;


                double value_lc = (freqA * valA - freqB * valB) / (freqA - freqB);


                valA = sat.FrequenceA.PseudoRange.CorrectedValue;
                valB = sat.FrequenceB.PseudoRange.CorrectedValue;
                double value_pc = (freqA * valA + freqB * valB) / (freqA + freqB);

                double LC11 = value_lc - value_pc;

                //    LC2 = 0;
                //    LC3 = 0;
                //}

                double elev = sat.Polar.Elevation * 0.0174532925199432957692369; //  (PI/180)  AstronomicalFunctions.D2R;
                double sinElev = Math.Sin(elev);
                sig = Math.Sqrt(err1 * err1 + (err2 * err2) / (sinElev * sinElev));

                //measurement noise variance(m)
                var1 = var_LC(1, 1, 0, sig * eratio0);
                //var2 = var_LC(0, 1, 1, sig * eratio0);
                //var3 = var_LC(1, 1, 0, sig * eratio0);



                if (!ambc.ContainsKey(prn)) { ambc.Add(prn, new ambc_t()); }

                amb = ambc[prn]; amb.prn = prn;

                if (sat.IsUnstable || amb.n[0] == 0.0 || Math.Abs(amb.epoch[0] - sat.RecevingTime) > MIN_ARC_GAP)
                {
                    amb.n[0] = amb.n[1] = amb.n[2] = 0.0;
                    amb.LC[0] = amb.LC[1] = amb.LC[2] = 0.0;
                    amb.LCv[0] = amb.LCv[1] = amb.LCv[2] = 0.0;
                    amb.fixcnt = 0;
                    // for (int j = 0; j < GnssConst.MaxSat; j++) amb.flags[j] = 0;  
                    foreach (var sat0 in receiverInfo)//分别对指定卫星进行改正
                    {
                        if (sat0 != sat && !amb.flags.ContainsKey(sat0.Prn)) amb.flags.Add(sat0.Prn, 0);
                        if (sat0 != sat && amb.flags.ContainsKey(sat0.Prn)) amb.flags[sat0.Prn] = 0;
                    }
                }

                // for (int j = 0; j < GnssConst.MaxSat; j++) amb.flags[j] = 0;  
                foreach (var sat0 in receiverInfo)//分别对指定卫星进行改正
                {
                    if (sat0 != sat && !amb.flags.ContainsKey(sat0.Prn)) amb.flags.Add(sat0.Prn, 0);
                }

                //averaging
                if (LC11 != null)
                {
                    amb.n[0] += 1.0;
                    amb.LC[0] += (LC11 - amb.LC[0]) / amb.n[0];
                    amb.LCv[0] += (var1 - amb.LCv[0]) / amb.n[0];

                }

                //if (LC2 != null)
                //{
                //    amb.n[1] += 1.0;
                //    amb.LC[1] += (LC2 - amb.LC[1]) / amb.n[1];
                //    amb.LCv[1] += (var2 - amb.LCv[1]) / amb.n[1];

                //}

                //if (LC3 != null)
                //{
                //    amb.n[2] += 1.0;
                //    amb.LC[2] += (LC3 - amb.LC[2]) / amb.n[2];
                //    amb.LCv[2] += (var3 - amb.LCv[2]) / amb.n[2];

                //}


                amb.epoch[0] = sat.RecevingTime;

                ambc[prn] = amb;
            }
        }


        /// <summary>
        /// 天顶距延迟（zenith path delay，zpd)
        /// </summary>
        public double WetTropoFactor { get; set; }




        /// <summary>
        /// confidence function of integer ambiguity
        /// </summary>
        /// <param name="N"></param>
        /// <param name="B"></param>
        /// <param name="sig"></param>
        /// <returns></returns>
        public double conffunc(int N, double B, double sig)
        {
            double x, p = 1.0;
            int i;
            x = Math.Abs(B - N);
            for (i = 1; i < 8; i++)
            {
                p -= f_erfc((i - x) / (SQRT2 * sig)) - f_erfc((i + x) / (SQRT2 * sig));
            }
            return p;
        }

        private double f_erfc(double x)
        {
            return x >= 0.0 ? q_gamma(0.5, x * x, LOG_PI / 2.0) : 1.0 + p_gamma(0.5, x * x, LOG_PI / 2.0);

        }


        //complementaty error function
        private double q_gamma(double a, double x, double log_gamma_a)
        {
            double y, w, la = 1.0, lb = x + 1.0 - a, lc;

            int i;

            if (x < a + 1.0) return 1.0 - p_gamma(a, x, log_gamma_a);
            w = Math.Exp(-x + a * Math.Log(x) - log_gamma_a);
            y = w / lb;
            for (i = 2; i < 100; i++)
            {
                lc = ((i - 1 - a) * (lb - la) + (i + x) * lb) / i;
                la = lb; lb = lc;
                w *= (i - 1 - a) / i;
                y += w / la / lb;
                if (Math.Abs(w / la / lb) < 1E-15) break;
            }

            return y;
        }
        private double p_gamma(double a, double x, double log_gamma_a)
        {
            double y, w;
            int i;
            if (x == 0) return 0.0;
            if (x >= a + 1.0) return 1.0 - q_gamma(a, x, log_gamma_a);
            y = w = Math.Exp(a * Math.Log(x) - x - log_gamma_a) / a;

            for (i = 1; i < 100; i++)
            {
                w *= x / (a + i);
                y += w;
                if (Math.Abs(w) < 1E-15) break;
            }
            return y;
        }


        /// <summary>
        /// noise variance of LC(m)
        /// </summary>
        /// <param name="time"></param>
        /// <param name="j"></param>
        /// <param name="k"></param>
        /// <param name="sig"></param>
        /// <returns></returns>
        private double var_LC(int i, int j, int k, double sig)
        {
            double f1 = GnssConst.GPS_FREQUENCY_L1;
            double f2 = GnssConst.GPS_FREQUENCY_L2;
            double f5 = GnssConst.GPS_FREQUENCY_L5;
            return (i * f1 * i * f1 + j * f2 * j * f2 + k * f5 * k * f5) * (sig * sig) / ((i * f1 + j * f2 + k * f5) * (i * f1 + j * f2 + k * f5));
        }

        /// <summary>
        /// wave length of LC (m)
        /// </summary>
        /// <param name="time"></param>
        /// <param name="j"></param>
        /// <param name="k"></param>
        /// <returns></returns>
        private double lam_LC(int i, int j, int k)
        {
            double f1 = GnssConst.GPS_FREQUENCY_L1;
            double f2 = GnssConst.GPS_FREQUENCY_L2;
            double f5 = GnssConst.GPS_FREQUENCY_L5;
            return GnssConst.LIGHT_SPEED / (i * f1 + j * f2 + k * f5);
        }


        /// <summary>
        /// ambiguity control type
        /// </summary>
        public class ambc_t
        {
            public Geo.Times.Time[] epoch = new Geo.Times.Time[4]; //last epoch
            public int fixcnt = 0;  //fix counter


            //public int[] flags = new int[GnssConst.MaxSat];     //fix flags , 0 没有固定， 1 固定

            public SatelliteNumber prn;

            public Dictionary<SatelliteNumber, int> flags = new Dictionary<SatelliteNumber, int>();//fix flags

            public double[] n = new double[4];     // number of epochs
            public double[] LC = new double[4];    //linear combination average
            public double[] LCv = new double[4];   //linear combination variance
        }


        /// <summary>
        /// ambiguity control
        /// </summary>
        //  public ambc_t[] ambc = new ambc_t[GnssConst.MaxSat];
        public SortedDictionary<SatelliteNumber, ambc_t> ambc = new SortedDictionary<SatelliteNumber, ambc_t>();


        /// <summary>
        /// number of continuous fixes of ambiguity
        /// </summary>
        public int nfix { get; set; }

        /// <summary>
        /// number of float states
        /// </summary>
        public int nx { get; set; }
        /// <summary>
        /// number of fixed states
        /// </summary>
        public int na { get; set; }

        /// <summary>
        /// time difference between current and previous(s)
        /// </summary>
        public double tt { get; set; }

    }



}
