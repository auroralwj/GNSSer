using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.IO;
using System.Windows.Forms;

using Gnsser.Data.Sinex;
using Gnsser.Service;

using Geo.Utils;
using Geo.Coordinates;
using AnyInfo;
using Geo.Algorithm.Adjust;

namespace Gnsser.Winform
{
    public partial class AmbizapForm : Form
    {
        public AmbizapForm()
        {
            InitializeComponent();
        }

        public event ShowLayerHandler showPointLayer;
        private void button_setPppPathes_Click(object sender, EventArgs e) { if (this.openFileDialog1.ShowDialog() == System.Windows.Forms.DialogResult.OK) this.textBox_PathesOfPPP.Lines = this.openFileDialog1.FileNames; }
        private void button_setBaseLinePathes_Click(object sender, EventArgs e) { if (this.openFileDialog1.ShowDialog() == System.Windows.Forms.DialogResult.OK) this.textBox_pathesOfBaseLine.Lines = this.openFileDialog1.FileNames; }

        private void button_read_Click(object sender, EventArgs e) { Read(); }

        /// <summary>
        /// Ambizap算法的实现。
        /// 选择两个点的PPP计算结果，及其基线计算结果。
        /// </summary>
        private void Read()
        {
            string pointNameA = null, pointNameB = null;
            //---------------------------------------------------数据读取//---------------------------------------------------
            //只提取两个点的位置信息及其基线。
            //读取PPP结果
            string[] pppfilePathes = this.textBox_PathesOfPPP.Lines;
            List<SinexFile> pppsinexFiles = SinexReader.Read(pppfilePathes);
            List<SinexSiteDetail> pppSitesA = new List<SinexSiteDetail>();
            List<SinexSiteDetail> pppSitesB = new List<SinexSiteDetail>();
            foreach (var file in pppsinexFiles)
            {
                List<SinexSiteDetail> sites = file.GetSinexSites();
                foreach (var item2 in sites)
                {
                    if (pointNameA == null) pointNameA = item2.Name;
                    if (pointNameB == null && pointNameA != item2.Name) pointNameB = item2.Name;

                    if (item2.Name == pointNameA)
                        pppSitesA.Add(item2);
                    if (item2.Name == pointNameB)
                        pppSitesB.Add(item2);
                }
            }
            if (pointNameA == null || pointNameB == null)
            {
                throw new ArgumentException("起算数据不足。");
            }

            //读取基线结果
            //只需要一条基线就行了。
            string[] baseLineFilePathes = this.textBox_pathesOfBaseLine.Lines;
            List<SinexFile> baseLineSinexFiles = SinexReader.Read(baseLineFilePathes);
            List<SinexSiteDetail> baseLineSites = new List<SinexSiteDetail>();
            foreach (var file in baseLineSinexFiles)
            {
                List<SinexSiteDetail> sites = file.GetSinexSites();
                foreach (var item2 in sites)
                {
                    if ((item2.Name == pointNameA || item2.Name == pointNameB) && (baseLineSites.Find(m => m.Name == item2.Name) == null))
                        baseLineSites.Add(item2);
                    if (baseLineSites.Count == 2)
                        break;
                }
                if (baseLineSites.Count == 2)
                    break;
            }
            //组建基线 
            string name = baseLineSites[0].Name + "-" + baseLineSites[1].Name;
            BaseLine line = new BaseLine(name, baseLineSites[1].EstimateXYZ - baseLineSites[0].EstimateXYZ);

            //---------------------------------------------------------平差计算--------------------------------------------
            //以第一个坐标为近似值
            XYZ aprioriA = pppSitesA[0].EstimateXYZ;
            XYZ aprioriB = pppSitesB[0].EstimateXYZ;
            //构建矩阵，具有约束条件的参数平差中，误差方程和约束方程的未知数相同。 
            //参数有6个x0,y0,z0,x1,y1,z1,条件有三个 x1-x0 = a, y1-y0 = b, z1-z0 = c
            double[][] coeff_error = MatrixUtil.Create((pppSitesA.Count + pppSitesB.Count) * 3, 6);
            double[][] indentity3x3 = MatrixUtil.CreateIdentity(3);
            for (int i = 0; i < pppSitesA.Count; i++) MatrixUtil.SetSubMatrix(coeff_error, indentity3x3, i * 3, 0);
            for (int i = 0; i < pppSitesB.Count; i++) MatrixUtil.SetSubMatrix(coeff_error, indentity3x3, (pppSitesA.Count + i) * 3, 3);

            //obsMinusApriori
            double[] obsMinusApriori_error = new double[(pppSitesA.Count + pppSitesB.Count) * 3];
            for (int i = 0; i < pppSitesA.Count; i++)
            {
                int rowIndex = i * 3;
                obsMinusApriori_error[rowIndex + 0] = pppSitesA[i].EstimateXYZ.X - aprioriA.X;
                obsMinusApriori_error[rowIndex + 1] = pppSitesA[i].EstimateXYZ.Y - aprioriA.Y;
                obsMinusApriori_error[rowIndex + 2] = pppSitesA[i].EstimateXYZ.Z - aprioriA.Z;
            }
            for (int i = 0; i < pppSitesB.Count; i++)
            {
                int rowIndex = (pppSitesA.Count + i) * 3;
                obsMinusApriori_error[rowIndex + 0] = pppSitesB[i].EstimateXYZ.X - aprioriB.X;
                obsMinusApriori_error[rowIndex + 1] = pppSitesB[i].EstimateXYZ.Y - aprioriB.Y;
                obsMinusApriori_error[rowIndex + 2] = pppSitesB[i].EstimateXYZ.Z - aprioriB.Z;
            }

            //Coeff_Condition,条件有三个 x0-x1 - a = 0,
            //条件方程：c x + w  = 0. => c deltaX + c X0 + w = 0
            double[][] coeff_condition = MatrixUtil.Create(3, 6);
            for (int index = 0; index < 3; index++)
            {
                coeff_condition[index][index + 0] = -1;
                coeff_condition[index][index + 3] = 1;
            }
            //ConstVector_Condition  c deltaX  - W = 0,  W = w - c X0 
            double[] obsMinusApriori_condition = new double[3];
            XYZ aprioriVector = aprioriB - aprioriA;
            obsMinusApriori_condition[0] = line.Vector.X - aprioriVector.X;
            obsMinusApriori_condition[1] = line.Vector.Y - aprioriVector.Y;
            obsMinusApriori_condition[2] = line.Vector.Z - aprioriVector.Z;

            ParamAdjustmentWithCondition adjust = new ParamAdjustmentWithCondition(coeff_error, obsMinusApriori_error, coeff_condition, obsMinusApriori_condition);
            double[][] param = adjust.Solve();
            
            //---------------------------------------------------------结果输出--------------------------------------------
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("计算完毕");

            XYZ estA = aprioriA + new XYZ(adjust.ParamVector);
            XYZ estB = aprioriB + new XYZ(adjust.ParamVector[3], adjust.ParamVector[4], adjust.ParamVector[5]);
            sb.AppendLine("近似值A：" + aprioriA);
            sb.AppendLine("平差值A：" + estA);
            sb.AppendLine("近似值B：" + aprioriB);
            sb.AppendLine("平差值B：" + estB); 
            sb.AppendLine("条件向量 A->B：" + line.Vector);
            sb.AppendLine("平差向量 A->B：" + (estB - estA));

            sb.AppendLine("------------------------------------参数------------------------------------");
            sb.AppendLine(MatrixUtil.GetFormatedText(param));
            double[][] param2 = adjust.Solve_ToBeCheck();
            sb.AppendLine("param2");
            sb.AppendLine(MatrixUtil.GetFormatedText(param2));


            sb.AppendLine("误差方程系数阵");
            sb.AppendLine(MatrixUtil.GetFormatedText(coeff_error));
            sb.AppendLine("------------------------------------obsMinusApriori------------------------------------");
            sb.AppendLine(MatrixUtil.GetFormatedText(MatrixUtil.Create(obsMinusApriori_error)));
            sb.AppendLine("------------------------------------Coeff_Condition------------------------------------");
            sb.AppendLine(MatrixUtil.GetFormatedText(coeff_condition));
            sb.AppendLine("------------------------------------ConstVector_Condition------------------------------------");
            sb.AppendLine(MatrixUtil.GetFormatedText(MatrixUtil.Create(obsMinusApriori_condition)));
            sb.AppendLine("------------------------------------误差方程法方程------------------------------------");
            sb.AppendLine(MatrixUtil.GetFormatedText(adjust.Normal_error));
            sb.AppendLine("------------------------------------条件方程法方程------------------------------------");
            sb.AppendLine(MatrixUtil.GetFormatedText(adjust.Normal_condition));
            sb.AppendLine("------------------------------------误差方程右手边------------------------------------");
            sb.AppendLine(MatrixUtil.GetFormatedText(adjust.RightHandSide_error));

            this.textBox_info.Text = sb.ToString();
            
            //----------------------------------------------------地图准备//----------------------------------------------------
            points.Clear();
            GeoCoord c1 = CoordTransformer.XyzToGeoCoord(aprioriA);
            GeoCoord c2 = CoordTransformer.XyzToGeoCoord(aprioriB);
            GeoCoord c3 = CoordTransformer.XyzToGeoCoord(estA);
            GeoCoord c4 = CoordTransformer.XyzToGeoCoord(estB);
            points.Add(new AnyInfo.Geometries.Point(c1.Lon,c1.Lat, "aprA"));
            points.Add(new AnyInfo.Geometries.Point(c2.Lon,c2.Lat, "aprB"));
            points.Add(new AnyInfo.Geometries.Point(c3.Lon,c3.Lat, "estA"));
            points.Add(new AnyInfo.Geometries.Point(c4.Lon,c4.Lat, "estB")); 
        }
        List<AnyInfo.Geometries.Point> points = new List<AnyInfo.Geometries.Point>(); 

        private void button_showOnMap_Click(object sender, EventArgs e)
        {
            if (showPointLayer != null && points != null)
            { 
                Layer layer = LayerFactory.CreatePointLayer(points);                
                showPointLayer(layer);
            }
        }

        private void button_saveTofile_Click(object sender, EventArgs e)
        {
            if (saveFileDialog1.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                File.WriteAllText(saveFileDialog1.FileName, this.textBox_info.Text);
                FormUtil.ShowIfOpenDirMessageBox(saveFileDialog1.FileName);
            }
        }

        private void button_outSolve_Click(object sender, EventArgs e)
        {
            //基于GPSTk输出的.OUT文件
            //  OutRead();//约束条件
           OutReadWithoutCondition();//无约束条件
        }

        /// <summary>
        /// Ambizap算法的实现。
        /// 选择两个点的PPP计算结果，及其基线计算结果。
        /// </summary>
        
        private void OutRead()
        {
            //引入的IGS站及其名称
            string[] pubSite = { "ARTU","DAEJ","GUAO",
                                   "IRKT","NRIL","POL2",
                                   "TIXI","TNML","ULAB","YAKT" };

            //读取IGS站z
            List<Geo.Coordinates.XYZ> igsSite = new List<Geo.Coordinates.XYZ>(); 
            igsSite = SinexUtil.GetEstXYZ(pubSite);
            int mIGS = pubSite.Length;
            //---------------------------------------------------数据读取//---------------------------------------------------
            //只提取点的位置信息 。
            //读取PPP结果
            string[] pppfilePathes = this.textBox_PathesOfPPP.Lines;
            int m = pppfilePathes.Length;
            for (int i = 0; i < pppfilePathes.Length; i++)
            {
                string strname = pppfilePathes[i].Substring(pppfilePathes[i].LastIndexOf("\\") + 1, pppfilePathes[i].LastIndexOf(".") - pppfilePathes[i].LastIndexOf("\\") - 1);
                string strSite = strname.Substring(5, 4);
                for (int j = 0; j < mIGS; j++)
                {
                    if (strSite == pubSite[j])//如果包含IGS站的PPP解，则需要剔除掉。
                    { m -= 1; }
                }
            }

            int mP = pppfilePathes.Length;
            double[][] Nppp = new double[3 * m][];// 3 * m);//法矩阵,PPP解构成的法方程系数矩阵
            double[][] Uppp = new double[3 * m][];//, 1);//PPP解构成的法方程常数项
            for (int i = 0; i < 3 * m; i++) { Nppp[i] = new double[3 * m]; Uppp[i] = new double[1]; }
            string[] site = new string[m];//测站名称
            int markOfSite = 0;
            for (int i = 0; i < mP; i++)
            {
                string strname = pppfilePathes[i].Substring(pppfilePathes[i].LastIndexOf("\\") + 1, pppfilePathes[i].LastIndexOf(".") - pppfilePathes[i].LastIndexOf("\\") - 1);
                string strSite = strname.Substring(5, 4);
                bool isIGS = false;
                for (int j = 0; j < mIGS; j++)
                {
                    if (strSite == pubSite[j])//如果包含IGS站的PPP解，则需要剔除掉。
                    {
                        isIGS = true;
                        break;
                    }
                }
                if (isIGS == true)
                { continue; }
                site[markOfSite] = strSite;

                StreamReader sr = new StreamReader(pppfilePathes[i]);
                string read = sr.ReadLine();

                string read1 = sr.ReadLine();
                string read2 = sr.ReadLine();
                string read3 = sr.ReadLine();
                string read4 = sr.ReadLine();
                string read5 = sr.ReadLine();
                string[] cha5 = read5.Split(' ');
                double m0 = Convert.ToDouble(cha5[5]);

                string[] cha = read.Split(' ');
                double x = Convert.ToDouble(cha[0]);
                double y = Convert.ToDouble(cha[2]);
                double z = Convert.ToDouble(cha[4]);
                Uppp[3 * markOfSite + 0][0] = x;
                Uppp[3 * markOfSite + 1][0] = y;
                Uppp[3 * markOfSite + 2][0] = z;//常数项l
                double mx = Convert.ToDouble(cha[6]);
                double my = Convert.ToDouble(cha[8]);
                double mz = Convert.ToDouble(cha[10]);

                string[] cha1 = read1.Split(' ');
                Nppp[3 * markOfSite + 0][3 * markOfSite + 0] = Convert.ToDouble(cha1[0]) * (m0 * m0);

                string[] cha2 = read2.Split(' ');
                Nppp[3 * markOfSite + 1][3 * markOfSite + 0] = Convert.ToDouble(cha2[0]) * (m0 * m0);
                Nppp[3 * markOfSite + 1][3 * markOfSite + 1] = Convert.ToDouble(cha2[2]) * (m0 * m0);

                string[] cha3 = read3.Split(' ');
                Nppp[3 * markOfSite + 2][3 * markOfSite + 0] = Convert.ToDouble(cha3[0]) * (m0 * m0);
                Nppp[3 * markOfSite + 2][3 * markOfSite + 1] = Convert.ToDouble(cha3[2]) * (m0 * m0);
                Nppp[3 * markOfSite + 2][3 * markOfSite + 2] = Convert.ToDouble(cha3[4]) * (m0 * m0);
                //对称
                Nppp[3 * markOfSite + 0][3 * markOfSite + 2] = Nppp[3 * markOfSite + 2][3 * markOfSite + 0];
                Nppp[3 * markOfSite + 0][3 * markOfSite + 1] = Nppp[3 * markOfSite + 1][3 * markOfSite + 0];
                Nppp[3 * markOfSite + 1][3 * markOfSite + 2] = Nppp[3 * markOfSite + 2][3 * markOfSite + 1];

                sr.Close();
                markOfSite += 1;
            }

            Geo.Algorithm.ArrayMatrix mNppp = new Geo.Algorithm.ArrayMatrix(Nppp);//N=ATPA=P
            mNppp = mNppp.Inverse;
            Geo.Algorithm.ArrayMatrix mUppp = new Geo.Algorithm.ArrayMatrix(Uppp);
            mUppp = mNppp * mUppp;//U=ATPL

            //读取基线结果

            string[] baseLineFilePathes = this.textBox_pathesOfBaseLine.Lines;

            int mb = baseLineFilePathes.Length;
            int count = 0;
            double limitDet = 2;//m
            for (int i = 0; i < mb; i++)
            {
                StreamReader sr = new StreamReader(baseLineFilePathes[i]);
                //
                string read = sr.ReadLine();

                read = sr.ReadLine();


                read = sr.ReadLine();

                string read1 = sr.ReadLine();

                string read2 = sr.ReadLine();

                string read3 = sr.ReadLine();


                string read4 = sr.ReadLine();
                string read5 = sr.ReadLine();
                string[] cha5 = read5.Split(' ');
                double m0 = Convert.ToDouble(cha5[5]);
                if (m0 <= limitDet)
                { count += 1; }
              
                sr.Close();
            }

            //


            double[][] Asnx = new double[3 * count][];// 3 * m);//系数矩阵,独立基线解构成的观测方程系数矩阵，先生成系数矩阵
            double[][] Psnx = new double[3 * count][];// 3 * m);//系数矩阵,独立基线解构成的观测方程系数矩阵，先生成系数矩阵
            double[][] Lsnx = new double[3 * count][];//, 1);//独立基线解构成的法方程常数项
            for (int i = 0; i < 3 * count; i++) { Asnx[i] = new double[3 * m]; Psnx[i] = new double[3 * count]; Lsnx[i] = new double[1]; }
            int iii = 0;
            for (int i = 0; i < mb; i++)
            {
                StreamReader sr = new StreamReader(baseLineFilePathes[i]);
                //
                string read0 = sr.ReadLine();
                string read00 = sr.ReadLine();
                string read000 = sr.ReadLine();
                string read1 = sr.ReadLine();
                string read2 = sr.ReadLine();
                string read3 = sr.ReadLine();
                string read4 = sr.ReadLine();
                string read5 = sr.ReadLine();
                string[] cha5 = read5.Split(' ');
                double m0 = Convert.ToDouble(cha5[5]);

                if (m0 > limitDet)
                {
                    sr.Close();
                    //break;
                    continue;
                }

                string[] cha0 = read0.Split('=');
                string refSsite = cha0[1].Trim();//终点

                cha0 = read00.Split('=');
                string rovSsite = cha0[1].Trim();//起点
                int ii = 0; int jj = 0;

                string[] cha = read000.Split(' ');
                double dx = Convert.ToDouble(cha[0]);
                double dy = Convert.ToDouble(cha[2]);
                double dz = Convert.ToDouble(cha[4]);
                double mdx = Convert.ToDouble(cha[6]);
                double mdy = Convert.ToDouble(cha[8]);
                double mdz = Convert.ToDouble(cha[10]);
                Lsnx[3 * iii + 0][0] = dx;
                Lsnx[3 * iii + 1][0] = dy;
                Lsnx[3 * iii + 2][0] = dz;

                for (int k = 0; k < igsSite.Count; k++)
                {
                    if (igsSite[k].Site.ToUpper() == refSsite.ToUpper())//如果是IGS站，则将IGS发布坐标作为已知值，引入约束。
                    {
                        //终点
                        Lsnx[3 * iii + 0][0] -= igsSite[k].X;
                        Lsnx[3 * iii + 1][0] -= igsSite[k].Y;
                        Lsnx[3 * iii + 2][0] -= igsSite[k].Z;
                    }

                    if (igsSite[k].Site.ToUpper() == rovSsite.ToUpper())//如果是IGS站，则将IGS发布坐标作为已知值，引入约束。
                    {
                        //起点
                        Lsnx[3 * iii + 0][0] += igsSite[k].X;
                        Lsnx[3 * iii + 1][0] += igsSite[k].Y;
                        Lsnx[3 * iii + 2][0] += igsSite[k].Z;
                    }
                }
                //

                for (int j = 0; j < m; j++)
                {
                    if (site[j].ToUpper() == refSsite.ToUpper())
                    {
                        jj = j;//终点
                        Asnx[3 * iii + 0][3 * jj + 0] = 1;
                        Asnx[3 * iii + 1][3 * jj + 1] = 1;
                        Asnx[3 * iii + 2][3 * jj + 2] = 1;
                    }
                    if (site[j].ToUpper() == rovSsite.ToUpper())
                    {
                        ii = j;//起点
                        //
                        Asnx[3 * iii + 0][3 * ii + 0] = -1;
                        Asnx[3 * iii + 1][3 * ii + 1] = -1;
                        Asnx[3 * iii + 2][3 * ii + 2] = -1;

                    }
                }

                string[] cha1 = read1.Split(' ');
                Psnx[3 * iii + 0][3 * iii + 0] = Convert.ToDouble(cha1[0]) * (m0 * m0);
                if (Psnx[3 * iii + 0][3 * iii + 0] == 0)
                {
                    throw new Exception("该基线数据错误！");
                }

                string[] cha2 = read2.Split(' ');
                Psnx[3 * iii + 1][3 * iii + 0] = Convert.ToDouble(cha2[0]) * (m0 * m0);
                Psnx[3 * iii + 1][3 * iii + 1] = Convert.ToDouble(cha2[2]) * (m0 * m0);
                if (Psnx[3 * iii + 1][3 * iii + 1] == 0)
                {
                    throw new Exception("该基线数据错误！");
                }

                string[] cha3 = read3.Split(' ');
                Psnx[3 * iii + 2][3 * iii + 0] = Convert.ToDouble(cha3[0]) * (m0 * m0);
                Psnx[3 * iii + 2][3 * iii + 1] = Convert.ToDouble(cha3[2]) * (m0 * m0);
                Psnx[3 * iii + 2][3 * iii + 2] = Convert.ToDouble(cha3[4]) * (m0 * m0);
                if (Psnx[3 * iii + 2][3 * iii + 2] == 0)
                {
                    throw new Exception("该基线数据错误！");
                }
                //对称
                Psnx[3 * iii + 0][3 * iii + 2] = Psnx[3 * iii + 2][3 * iii + 0];
                Psnx[3 * iii + 0][3 * iii + 1] = Psnx[3 * iii + 1][3 * iii + 0];
                Psnx[3 * iii + 1][3 * iii + 2] = Psnx[3 * iii + 2][3 * iii + 1];


                sr.Close();
                iii += 1;
            }
            Geo.Algorithm.ArrayMatrix mAsnx = new Geo.Algorithm.ArrayMatrix(Asnx);//N=ATPA=P
            Geo.Algorithm.ArrayMatrix mLsnx = new Geo.Algorithm.ArrayMatrix(Lsnx);
            Geo.Algorithm.ArrayMatrix mPsnx = new Geo.Algorithm.ArrayMatrix(Psnx);

            Geo.Algorithm.ArrayMatrix mNsnx = null;//独立基线解构成的法方程系数矩阵
            Geo.Algorithm.ArrayMatrix mUsnx = null;//独立基线解构成的法方程常数项
            mPsnx = mPsnx.Inverse;
            mNsnx = mAsnx.Transpose() * mPsnx;//N=ATP
            mUsnx = mNsnx * mLsnx;//U=ATPL
            mNsnx = mNsnx * mAsnx;//N=ATPA


            Geo.Algorithm.ArrayMatrix N = mNppp + mNsnx;
            Geo.Algorithm.ArrayMatrix U = mUppp + mUsnx;

            Geo.Algorithm.ArrayMatrix Qx = N.Inverse;
            Geo.Algorithm.ArrayMatrix X = Qx * U;
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < m; i++)
            {
                for (int j = 0; j < X.Columns; j++)
                {
                    sb.AppendLine(X[3 * i + 0, j].ToString() + " " + X[3 * i + 1, j].ToString() + " " + X[3 * i + 2, j].ToString());
                }
            }
            for (int i = 0; i < m; i++)
            {
                sb.AppendLine((Qx[3 * i + 0, 3 * i + 0]).ToString() + " " + (Qx[3 * i + 1, 3 * i + 1]).ToString() + " " + (Qx[3 * i + 2, 3 * i + 2]).ToString());
            }
            this.textBox_info.Text = sb.ToString();

            //----------------------------------------------------地图准备//----------------------------------------------------
            points.Clear();
            for (int i = 0; i < m; i++)
            {
                XYZ xyz = new XYZ(X[3 * i + 0, 0], X[3 * i + 1, 0], X[3 * i + 2, 0]);
                //points.Add(new AnyInfo.Geometries.Point(CoordTransformer.XyzToGeoCoord(aprioriA), "aprA"));
                //points.Add(new AnyInfo.Geometries.Point(CoordTransformer.XyzToGeoCoord(aprioriB), "aprB"));
                points.Add(new AnyInfo.Geometries.Point(CoordTransformer.XyzToGeoCoord(xyz), "estX"));
            }     
        }


        private void OutReadWithoutCondition()
        {
            //引入的IGS站及其名称
            string[] pubSite = { "" };

            //读取IGS站z
            List<Geo.Coordinates.XYZ> igsSite =            SinexUtil.GetEstXYZ(pubSite);
            int mIGS = pubSite.Length;
            //---------------------------------------------------数据读取//---------------------------------------------------
            //只提取点的位置信息 。
            //读取PPP结果
            string[] pppfilePathes = this.textBox_PathesOfPPP.Lines;
            int m = pppfilePathes.Length;
            for (int i = 0; i < pppfilePathes.Length; i++)
            {
                string strname = pppfilePathes[i].Substring(pppfilePathes[i].LastIndexOf("\\") + 1, pppfilePathes[i].LastIndexOf(".") - pppfilePathes[i].LastIndexOf("\\") - 1);
                string strSite = strname.Substring(5, 4);
                for (int j = 0; j < mIGS; j++)
                {
                    if (strSite == pubSite[j])//如果包含IGS站的PPP解，则需要剔除掉。
                    { m -= 1; }
                }
            }

            int mP = pppfilePathes.Length;
            double[][] Nppp = new double[3 * m][];// 3 * m);//法矩阵,PPP解构成的法方程系数矩阵
            double[][] Uppp = new double[3 * m][];//, 1);//PPP解构成的法方程常数项
            for (int i = 0; i < 3 * m; i++) { Nppp[i] = new double[3 * m]; Uppp[i] = new double[1]; }
            string[] site = new string[m];//测站名称
            int markOfSite = 0;
            for (int i = 0; i < mP; i++)
            {
                string strname = pppfilePathes[i].Substring(pppfilePathes[i].LastIndexOf("\\") + 1, pppfilePathes[i].LastIndexOf(".") - pppfilePathes[i].LastIndexOf("\\") - 1);
                string strSite = strname.Substring(5, 4);
                bool isIGS = false;
                for (int j = 0; j < mIGS; j++)
                {
                    if (strSite == pubSite[j])//如果包含IGS站的PPP解，则需要剔除掉。
                    {
                        isIGS = true;
                        break;
                    }
                }
                if (isIGS == true)
                { continue; }
                site[markOfSite] = strSite;

                StreamReader sr = new StreamReader(pppfilePathes[i]);
                string read = sr.ReadLine();

                string read1 = sr.ReadLine();
                string read2 = sr.ReadLine();
                string read3 = sr.ReadLine();
                string read4 = sr.ReadLine();
                string read5 = sr.ReadLine();
                string[] cha5 = read5.Split(' ');
                double m0 = Convert.ToDouble(cha5[5]);

                string[] cha = read.Split(' ');
                double x = Convert.ToDouble(cha[0]);
                double y = Convert.ToDouble(cha[2]);
                double z = Convert.ToDouble(cha[4]);
                Uppp[3 * markOfSite + 0][0] = x;
                Uppp[3 * markOfSite + 1][0] = y;
                Uppp[3 * markOfSite + 2][0] = z;//常数项l
                double mx = Convert.ToDouble(cha[6]);
                double my = Convert.ToDouble(cha[8]);
                double mz = Convert.ToDouble(cha[10]);

                string[] cha1 = read1.Split(' ');
                Nppp[3 * markOfSite + 0][3 * markOfSite + 0] = Convert.ToDouble(cha1[0]);// *(m0 * m0);

                string[] cha2 = read2.Split(' ');
                Nppp[3 * markOfSite + 1][3 * markOfSite + 0] = Convert.ToDouble(cha2[0]);//* (m0 * m0);
                Nppp[3 * markOfSite + 1][3 * markOfSite + 1] = Convert.ToDouble(cha2[2]);// * (m0 * m0);

                string[] cha3 = read3.Split(' ');
                Nppp[3 * markOfSite + 2][3 * markOfSite + 0] = Convert.ToDouble(cha3[0]);// * (m0 * m0);
                Nppp[3 * markOfSite + 2][3 * markOfSite + 1] = Convert.ToDouble(cha3[2]);// * (m0 * m0);
                Nppp[3 * markOfSite + 2][3 * markOfSite + 2] = Convert.ToDouble(cha3[4]);//  * (m0 * m0);
                //对称
                Nppp[3 * markOfSite + 0][3 * markOfSite + 2] = Nppp[3 * markOfSite + 2][3 * markOfSite + 0];
                Nppp[3 * markOfSite + 0][3 * markOfSite + 1] = Nppp[3 * markOfSite + 1][3 * markOfSite + 0];
                Nppp[3 * markOfSite + 1][3 * markOfSite + 2] = Nppp[3 * markOfSite + 2][3 * markOfSite + 1];

                sr.Close();
                markOfSite += 1;
            }

            Geo.Algorithm.ArrayMatrix mNppp = new Geo.Algorithm.ArrayMatrix(Nppp);//N=ATPA=P
            Geo.Algorithm.ArrayMatrix mPppp = new Geo.Algorithm.ArrayMatrix(Nppp);//tmp
            mPppp = mPppp.Inverse;
            mNppp = mNppp.Inverse;
            Geo.Algorithm.ArrayMatrix mUppp = new Geo.Algorithm.ArrayMatrix(Uppp);
            Geo.Algorithm.ArrayMatrix mlppp = new Geo.Algorithm.ArrayMatrix(Uppp);//tmp
            mUppp = mNppp * mUppp;//U=ATPL

            //读取基线结果

            string[] baseLineFilePathes = this.textBox_pathesOfBaseLine.Lines;

            int mb = baseLineFilePathes.Length;
            int count = 0;
            double limitDet = 0.01;//m
            for (int i = 0; i < mb; i++)
            {
                StreamReader sr = new StreamReader(baseLineFilePathes[i]);
                //
                string read = sr.ReadLine();

                read = sr.ReadLine();


                read = sr.ReadLine();

                string read1 = sr.ReadLine();

                string read2 = sr.ReadLine();

                string read3 = sr.ReadLine();


                string read4 = sr.ReadLine();
                string read5 = sr.ReadLine();
                string[] cha5 = read5.Split(' ');
                double m0 = Convert.ToDouble(cha5[5]);
                if (m0 <= limitDet)
                { count += 1; }
              
                sr.Close();
            }

            //


            double[][] Asnx = new double[3 * count][];// 3 * m);//系数矩阵,独立基线解构成的观测方程系数矩阵，先生成系数矩阵
            double[][] Psnx = new double[3 * count][];// 3 * m);//系数矩阵,独立基线解构成的观测方程系数矩阵，先生成系数矩阵
            double[][] Lsnx = new double[3 * count][];//, 1);//独立基线解构成的法方程常数项
            for (int i = 0; i < 3 * count; i++) { Asnx[i] = new double[3 * m]; Psnx[i] = new double[3 * count]; Lsnx[i] = new double[1]; }
            int iii = 0;
            for (int i = 0; i < mb; i++)
            {
                StreamReader sr = new StreamReader(baseLineFilePathes[i]);
                //
                string read0 = sr.ReadLine();
                string read00 = sr.ReadLine();
                string read000 = sr.ReadLine();
                string read1 = sr.ReadLine();
                string read2 = sr.ReadLine();
                string read3 = sr.ReadLine();
                string read4 = sr.ReadLine();
                string read5 = sr.ReadLine();
                string[] cha5 = read5.Split(' ');
                double m0 = Convert.ToDouble(cha5[5]);

                if (m0 > limitDet)
                {
                    sr.Close();
                    //break;
                    continue;
                }

                string[] cha0 = read0.Split('=');
                string refSsite = cha0[1].Trim();//终点

                cha0 = read00.Split('=');
                string rovSsite = cha0[1].Trim();//起点
                int ii = 0; int jj = 0;

                string[] cha = read000.Split(' ');
                double dx = Convert.ToDouble(cha[0]);
                double dy = Convert.ToDouble(cha[2]);
                double dz = Convert.ToDouble(cha[4]);
                double mdx = Convert.ToDouble(cha[6]);
                double mdy = Convert.ToDouble(cha[8]);
                double mdz = Convert.ToDouble(cha[10]);
                Lsnx[3 * iii + 0][0] = dx;
                Lsnx[3 * iii + 1][0] = dy;
                Lsnx[3 * iii + 2][0] = dz;

                for (int k = 0; k < igsSite.Count; k++)
                {
                    if (igsSite[k].Site.ToUpper() == refSsite.ToUpper())//如果是IGS站，则将IGS发布坐标作为已知值，引入约束。
                    {
                        //终点
                        Lsnx[3 * iii + 0][0] -= igsSite[k].X;
                        Lsnx[3 * iii + 1][0] -= igsSite[k].Y;
                        Lsnx[3 * iii + 2][0] -= igsSite[k].Z;
                    }

                    if (igsSite[k].Site.ToUpper() == rovSsite.ToUpper())//如果是IGS站，则将IGS发布坐标作为已知值，引入约束。
                    {
                        //起点
                        Lsnx[3 * iii + 0][0] += igsSite[k].X;
                        Lsnx[3 * iii + 1][0] += igsSite[k].Y;
                        Lsnx[3 * iii + 2][0] += igsSite[k].Z;
                    }
                }
                //

                for (int j = 0; j < m; j++)
                {
                    if (site[j].ToUpper() == refSsite.ToUpper())
                    {
                        jj = j;//终点
                        Asnx[3 * iii + 0][3 * jj + 0] = 1;
                        Asnx[3 * iii + 1][3 * jj + 1] = 1;
                        Asnx[3 * iii + 2][3 * jj + 2] = 1;
                    }
                    if (site[j].ToUpper() == rovSsite.ToUpper())
                    {
                        ii = j;//起点
                        //
                        Asnx[3 * iii + 0][3 * ii + 0] = -1;
                        Asnx[3 * iii + 1][3 * ii + 1] = -1;
                        Asnx[3 * iii + 2][3 * ii + 2] = -1;

                    }
                }

                string[] cha1 = read1.Split(' ');
                Psnx[3 * iii + 0][3 * iii + 0] = Convert.ToDouble(cha1[0]);// * (m0 * m0);
                if (Psnx[3 * iii + 0][3 * iii + 0] == 0)
                {
                    throw new Exception("该基线数据错误！");
                }

                string[] cha2 = read2.Split(' ');
                Psnx[3 * iii + 1][3 * iii + 0] = Convert.ToDouble(cha2[0]);// *(m0 * m0);
                Psnx[3 * iii + 1][3 * iii + 1] = Convert.ToDouble(cha2[2]);// * (m0 * m0);
                if (Psnx[3 * iii + 1][3 * iii + 1] == 0)
                {
                    throw new Exception("该基线数据错误！");
                }

                string[] cha3 = read3.Split(' ');
                Psnx[3 * iii + 2][3 * iii + 0] = Convert.ToDouble(cha3[0]);// * (m0 * m0);
                Psnx[3 * iii + 2][3 * iii + 1] = Convert.ToDouble(cha3[2]);// * (m0 * m0);
                Psnx[3 * iii + 2][3 * iii + 2] = Convert.ToDouble(cha3[4]);// * (m0 * m0);
                if (Psnx[3 * iii + 2][3 * iii + 2] == 0)
                {
                    throw new Exception("该基线数据错误！");
                }
                //对称
                Psnx[3 * iii + 0][3 * iii + 2] = Psnx[3 * iii + 2][3 * iii + 0];
                Psnx[3 * iii + 0][3 * iii + 1] = Psnx[3 * iii + 1][3 * iii + 0];
                Psnx[3 * iii + 1][3 * iii + 2] = Psnx[3 * iii + 2][3 * iii + 1];


                sr.Close();
                iii += 1;
            }
            Geo.Algorithm.ArrayMatrix mAsnx = new Geo.Algorithm.ArrayMatrix(Asnx);//N=ATPA=P
            Geo.Algorithm.ArrayMatrix mLsnx = new Geo.Algorithm.ArrayMatrix(Lsnx);
            Geo.Algorithm.ArrayMatrix mPsnx = new Geo.Algorithm.ArrayMatrix(Psnx);

            Geo.Algorithm.ArrayMatrix mNsnx = null;//独立基线解构成的法方程系数矩阵
            Geo.Algorithm.ArrayMatrix mUsnx = null;//独立基线解构成的法方程常数项
            mPsnx = mPsnx.Inverse;
            mNsnx = mAsnx.Transpose() * mPsnx;//N=ATP
            mUsnx = mNsnx * mLsnx;//U=ATPL
            mNsnx = mNsnx * mAsnx;//N=ATPA


            Geo.Algorithm.ArrayMatrix N = mNppp + mNsnx;
            Geo.Algorithm.ArrayMatrix U = mUppp + mUsnx;

            Geo.Algorithm.ArrayMatrix Qx = N.Inverse;
            Geo.Algorithm.ArrayMatrix X = Qx * U;//约束后的估值

            Geo.Algorithm.ArrayMatrix V1 = X - mlppp;
            Geo.Algorithm.ArrayMatrix V2 = mAsnx * X - mLsnx;
            Geo.Algorithm.ArrayMatrix VTPV = V1.Transpose() * mPppp * V1;

            Geo.Algorithm.ArrayMatrix VTPV1 = V2.Transpose() * mPsnx * V2;

            VTPV = VTPV + V2.Transpose() * mPsnx * V2;

            double det0 = VTPV[0, 0] / (count * 3);

            det0 = Math.Sqrt(det0);

            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < m; i++)
            {
                for (int j = 0; j < X.Columns; j++)
                {
                    sb.AppendLine(site[i]+" "+ X[3 * i + 0, j].ToString() + " " + X[3 * i + 1, j].ToString() + " " + X[3 * i + 2, j].ToString());
                }
            }
            for (int i = 0; i < m; i++)
            {
                sb.AppendLine((det0* Math.Sqrt( Qx[3 * i + 0, 3 * i + 0])).ToString() + " " + (det0* Math.Sqrt(Qx[3 * i + 1, 3 * i + 1])).ToString() + " " + (det0* Math.Sqrt(Qx[3 * i + 2, 3 * i + 2])).ToString());
            }
            this.textBox_info.Text = sb.ToString();

            //----------------------------------------------------地图准备//----------------------------------------------------
            points.Clear();
            for (int i = 0; i < m; i++)
            {
                XYZ xyz = new XYZ(X[3 * i + 0, 0], X[3 * i + 1, 0], X[3 * i + 2, 0]);
                //points.Add(new AnyInfo.Geometries.Point(CoordTransformer.XyzToGeoCoord(aprioriA), "aprA"));
                //points.Add(new AnyInfo.Geometries.Point(CoordTransformer.XyzToGeoCoord(aprioriB), "aprB"));
                points.Add(new AnyInfo.Geometries.Point(CoordTransformer.XyzToGeoCoord(xyz), "estX"));
            }

        }
    }
}
