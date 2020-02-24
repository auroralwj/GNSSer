using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Windows.Forms;
using Gnsser.Data.Sinex;
using Geo.Utils;
using Geo.Coordinates;
using AnyInfo;
using Geo.Algorithm;


namespace Gnsser.Winform
{
    /// <summary>
    /// Gnss网平差计算
    /// </summary>
    public partial class NetAdjustment: Form
    {
        /// <summary>
        /// 显示图层
        /// </summary>
        public event ShowLayerHandler showPointLayer;

        public NetAdjustment()
        {
            InitializeComponent();
        }

        List<GeoCoord> GeoCoords { get; set; }

        private void button_showOnMap_Click(object sender, EventArgs e)
        {
            GeoCoords = new System.Collections.Generic.List<GeoCoord>();
            foreach (var item in AllPointsApproXyz)
            {
                var geoCoord = CoordTransformer.XyzToGeoCoord(item.Value);
                GeoCoords.Add(geoCoord);
            }

            if (showPointLayer != null && GeoCoords != null)
            {
                List<AnyInfo.Geometries.Point> lonlats = new List<AnyInfo.Geometries.Point>();
                foreach (GeoCoord g in GeoCoords)
                {
                    lonlats.Add(new AnyInfo.Geometries.Point(g, "1"));
                }
                Layer layer = LayerFactory.CreatePointLayer(lonlats);
                showPointLayer(layer);
            }
        }

        private void button_saveTofile_Click(object sender, EventArgs e)
        {
            //    if (saveFileDialog1.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            //    {
            //        File.WriteAllText(saveFileDialog1.FileName, this.textBox_info.Text);

            //        FormUtil.ShowIfOpenDirMessageBox(saveFileDialog1.FileName);
            //    }
        }


        /// <summary>
        /// 已知点列表信息
        /// </summary>
        public Dictionary<string, XYZ> KonwnPointsXyz { get; set; }
        /// <summary>
        /// 所有已知点的名称,大写存储
        /// </summary>
        public List<string> KnownPointName { get; set; }

        ///  
        /// 公共/连接点列表信息
        /// </summary>
        public Dictionary<string, XYZ> CommonPointsXyz { get; set; }
        /// <summary>
        /// 所有公共/连接点列表信息的名称,大写存储
        /// </summary>
        public List<string> CommonPointName { get; set; }

        /// <summary>
        /// 所有点的坐标信息，未知的为近似值，已知点仍为基线时的近似值，
        /// </summary>
        public Dictionary<string, XYZ> AllPointsApproXyz { get; set; }
        /// <summary>
        /// 所有点的名称,大写存储
        /// </summary>
       public List<string> AllPointName { get; set; }
        /// <summary>
       /// 所有观测值基线残差信息
        /// </summary>
       public Dictionary<string, XYZ> BaselineInfo { get; set; } 
        /// <summary>
        /// 所有观测值的方差-协方差矩阵
        /// </summary>
       public Dictionary<string, SymmetricMatrix> BaselineInverseOfWegihtInfo { get; set; }

        /// <summary>
        /// 系数矩阵
        /// </summary>
       public ArrayMatrix CoeffOfParam { get; set; }

       // /// <summary>
       ///// 所有基线观测向量的起点的序列，提高平差效率
       // /// </summary>
       //public int[] BeginPoint { get; set; }
       // /// <summary>
       ///// 所有基线观测向量的终点的序列，提高平差效率
       // /// </summary>
       //public int[] EndPoint { get; set; }

       ///// <summary>
       ///// 所有基线观测向量，提高平差效率
       ///// </summary>
       //public double[] ObsL { get; set; }
       ///// <summary>
       ///// 所有观测向量的权逆阵，下三角存放，提高平差效率
       ///// </summary>
       //public double[] InverseWeightOfObsL { get; set; }

       /// <summary>
       /// 所有基线观测向量
       /// /// </summary>
          public ArrayMatrix Observation { get; set; }
          /// <summary>
          /// 所有观测向量的权逆阵
          /// </summary>
          public ArrayMatrix InverseOfWeight { get; set; }

          public IMatrix ATPA { get; set; }

          public IMatrix ATPL { get; set; }

        /// <summary>
        /// 更新后的估计坐标值=
        /// </summary>
          public Dictionary<string, XYZ> AllPointsEstimateXyz { get; set; }


        private void button_Calculate_Click(object sender, EventArgs e)
        {
            //读取基线信息
            Int();

            //根据基线信息建立观测方程和法方程,此时是条件数不足的，无法解
            AdjustMatrixBuilder();

            #region 根据所选网平差方法，对法方程增加不同的约束条件
            string outName = "平差文件";
            if (this.radioButton_fixedConstraintAdj.Checked) //固定点平差
            {
                outName = "固定点法平差文件";
                foreach (var item in KonwnPointsXyz)
                {
                    string name = item.Key.ToUpper();
                    XYZ Xyz = item.Value;
                    int index = AllPointName.IndexOf(name);
                    if (!AllPointsApproXyz.ContainsKey(name))
                    {
                        //
                    }
                    this.ATPA[3 * index + 0, 3 * index + 0] += 1.0e30;
                    this.ATPA[3 * index + 1, 3 * index + 1] += 1.0e30;
                    this.ATPA[3 * index + 2, 3 * index + 2] += 1.0e30;
                }        
            }
            else if (this.radioButton_freeNetAdj.Checked) //自由网约束平差
            {
                outName = "自由网松约束法平差文件";
                double constrainValue = 1.0 / this.AllPointName.Count;
                for (int i = 0; i < ATPA.RowCount; i++)
                { 
                    ATPA[i, i] += constrainValue; 
                }
            }
                
            else if (this.radioButton_minConstraintAdj.Checked) //最小约束条件平差 见 Bernese 5.2 Doc P221. Gamit 叫相似变换？？？
            {
                outName = "最小约束条件法平差文件";
                int count = AllPointName.Count;
                ArrayMatrix B = B = new ArrayMatrix(count * 3, 7);
                ArrayMatrix h = new ArrayMatrix(count * 3, 1);
                ArrayMatrix Ph = new ArrayMatrix(count * 3, count * 3);
                int k = 0;
                foreach (var item in KonwnPointsXyz)
                {
                    string staName = item.Key;
                    XYZ xyz = item.Value; //真值

                    int index =AllPointName.IndexOf(staName);

                    B[3 * index + 0, 0] = 1; B[3 * index + 0, 1] = 0; B[3 * index + 0, 2] = 0; B[3 * index + 0, 3] = 0; B[3 * index + 0, 4] = -xyz.Z; B[3 * index + 0, 5] = xyz.Y; B[3 * index + 0, 6] = xyz.X;
                    B[3 * index + 1, 0] = 0; B[3 * index + 1, 1] = 1; B[3 * index + 1, 2] = 0; B[3 * index + 1, 3] = xyz.Z; B[3 * index + 1, 4] = 0; B[3 * index + 1, 5] = -xyz.X; B[3 * index + 1, 6] = xyz.Y;
                    B[3 * index + 2, 0] = 0; B[3 * index + 2, 1] = 0; B[3 * index + 2, 2] = 1; B[3 * index + 2, 3] = -xyz.Y; B[3 * index + 2, 4] = xyz.X; B[3 * index + 2, 5] = 0; B[3 * index + 2, 6] = xyz.Z;

                    h[3 * index + 0, 0] = xyz.X - AllPointsApproXyz[staName].X;
                    h[3 * index + 1, 0] = xyz.Y - AllPointsApproXyz[staName].Y;
                    h[3 * index + 2, 0] = xyz.Z - AllPointsApproXyz[staName].Z;

                    Ph[3 * index + 0, 3 * index + 0] = 10e10;
                    Ph[3 * index + 1, 3 * index + 1] = 10e10;
                    Ph[3 * index + 2, 3 * index + 2] = 10e10;

                    k++;
                }
                IMatrix tmp = (B.Transposition).Multiply(B); //7*7
                IMatrix H = (B.Transposition); //tmp.GetInverse().Multiply(B.Transposition); //7*3n
              //  IMatrix H = tmp.GetInverse().Multiply(B.Transposition); //7*3n
                
                IMatrix lb = H.Multiply(h); //7*1
                IMatrix Nb = H.Transposition.Multiply(H); //3n*3n
                IMatrix Ub = H.Transposition.Multiply(lb);//3n*1

                this.ATPA = this.ATPA.Plus(Nb);
                this.ATPL = this.ATPL.Plus(Ub);
            }
            else if (this.radioButton_Robust.Checked)
            { 
            
            }
            else if (this.radioButton_bayesRobust.Checked)
            {

            }

            #endregion

            //  Geo.Algorithm.IMatrix QX = (N.GetInverse());

            Geo.Algorithm.IMatrix QX = (ATPA).GetInverse();

            Geo.Algorithm.IMatrix X = QX.Multiply(ATPL);
            //Geo.Algorithm.IMatrix X = QX.Multiply(U);

          

            //输出结果
            #region
            string savePath = "D:\\" + outName + ".txt";
            FileInfo aFile = new FileInfo(savePath);
            StreamWriter sw = aFile.CreateText();
            System.Globalization.NumberFormatInfo GN = new System.Globalization.CultureInfo("zh-CN", false).NumberFormat;
            GN.NumberDecimalDigits = 6;//小数点6位
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("计算完毕！结果保存在：" + savePath);

            sw.WriteLine("基准点坐标信息：");
            foreach(var item in KnownPointName)
            {
                sw.Write(item); sw.Write("\t"); sw.Write(KonwnPointsXyz[item].ToSnxString()); sw.Write("\n");
            }
            sw.WriteLine("所有测站坐标的估计值：");
            for (int i = 0; i < this.AllPointName.Count; i++)
            {
                string name = AllPointName[i];
                sw.Write(name);
                sw.Write("\t");
                sw.Write((AllPointsApproXyz[name].X + X[3 * i + 0, 0]).ToString("0.0000", GN));//("N", GN));//
                sw.Write("\t");
                sw.Write((AllPointsApproXyz[name].Y + X[3 * i + 1, 0]).ToString("0.0000", GN));//("N", GN));//
                sw.Write("\t");
                sw.Write((AllPointsApproXyz[name].Z + X[3 * i + 2, 0]).ToString("0.0000", GN));//("N", GN));//
                sw.Write("\n");
            }

            for (int i = 0; i < QX.RowCount; i++)
            {
                for (int j = 0; j <= i; j++)
                {
                    sw.Write(QX[i, j].ToString("N", GN));
                    sw.Write("\t");
                }
                sw.Write("\n");
            }

            sw.Close();
            #endregion

            this.textBox_info.Text = sb.ToString();
            
        }
        /// <summary>
        /// 构建基线网平差观测方程的信息,得到法方程的系数ATPA和常数ATPL
        /// </summary>
        private void AdjustMatrixBuilder()
        {
            int row = BaselineInfo.Count * 3;
            int column = AllPointsApproXyz.Count * 3;
            CoeffOfParam = new ArrayMatrix(row, column);
            Observation = new ArrayMatrix(row, 1);
            InverseOfWeight = new ArrayMatrix(row, row);
            //BeginPoint = new int[row];
            //EndPoint = new int[row];

            int i = 0;
            foreach (var item in BaselineInfo)
            {
                string[] name = item.Key.Split('-');
                string refName = name[0].ToUpper();
                string rovName = name[1].ToUpper();

                int refIndex = AllPointName.IndexOf(refName);
                int rovIndex = AllPointName.IndexOf(rovName);
                //系数项
                //BeginPoint[i] = refIndex;
                //EndPoint[i] = rovIndex;
                CoeffOfParam[3 * i + 0, 3 * refIndex + 0] = -1; CoeffOfParam[3 * i + 0, 3 * rovIndex + 0] = 1;
                CoeffOfParam[3 * i + 1, 3 * refIndex + 1] = -1; CoeffOfParam[3 * i + 1, 3 * rovIndex + 1] = 1;
                CoeffOfParam[3 * i + 2, 3 * refIndex + 2] = -1; CoeffOfParam[3 * i + 2, 3 * rovIndex + 2] = 1;
                //常数项
                //ObsL[3 * i + 0] = key.Value.X;
                //ObsL[3 * i + 1] = key.Value.Y;
                //ObsL[3 * i + 2] = key.Value.Z;
                Observation[3 * i + 0, 0] = item.Value.X;
                Observation[3 * i + 1, 0] = item.Value.Y;
                Observation[3 * i + 2, 0] = item.Value.Z;
                
                if (item.Value.X >= 0.06)
                {
                    item.Value.X = 0.06 / 2;
                }
                if (item.Value.Y >= 0.06)
                {
                    item.Value.Y = 0.06 / 2;
                }
                if (item.Value.Z >= 0.06)
                {
                    item.Value.Z = 0.06 / 2;
                }


                //权逆阵
                InverseOfWeight[3 * i + 0, 3 * i + 0] = BaselineInverseOfWegihtInfo[item.Key].Array[0][0]; InverseOfWeight[3 * i + 0, 3 * i + 1] = BaselineInverseOfWegihtInfo[item.Key].Array[1][0]; InverseOfWeight[3 * i + 0, 3 * i + 2] = BaselineInverseOfWegihtInfo[item.Key].Array[2][0];
                InverseOfWeight[3 * i + 1, 3 * i + 0] = BaselineInverseOfWegihtInfo[item.Key].Array[1][0]; InverseOfWeight[3 * i + 1, 3 * i + 1] = BaselineInverseOfWegihtInfo[item.Key].Array[1][1]; InverseOfWeight[3 * i + 1, 3 * i + 2] = BaselineInverseOfWegihtInfo[item.Key].Array[2][1];
                InverseOfWeight[3 * i + 2, 3 * i + 0] = BaselineInverseOfWegihtInfo[item.Key].Array[2][0]; InverseOfWeight[3 * i + 2, 3 * i + 1] = BaselineInverseOfWegihtInfo[item.Key].Array[2][1]; InverseOfWeight[3 * i + 2, 3 * i + 2] = BaselineInverseOfWegihtInfo[item.Key].Array[2][2];

                //
                i++;
            }

            IMatrix Weight = InverseOfWeight.GetInverse();
            IMatrix Tmp = CoeffOfParam.Transposition;//.Multiply(Weight);

            this.ATPA = Tmp.Multiply(CoeffOfParam);
            this.ATPL = Tmp.Multiply(Observation);



            ////提取存放下三角
            //int ni = InverseOfWeight.RowCount;
            //InverseWeightOfObsL = new double[(ni + 1) * ni / 2];
            //int sk = 0;
            //for (int s1 = 0; s1 < ni; s1++)
            //{
            //    for (int s2 = 0; s2 <= s1; s2++)
            //    {
            //        InverseWeightOfObsL[sk] = InverseOfWeight[s1, s2];
            //        sk++;
            //    }
            //}

        }


        /// <summary>
        /// 读取基线向量观测文件和SNX文件，获取基线相关信息
        /// </summary>
        private void Int()
        {
            List<string> allKnownPointName = new System.Collections.Generic.List<string>(); //所指定的已知点，但这些已知点可能SNX文件中没有，不是全部采用
            KnownPointName = new System.Collections.Generic.List<string>(); //实际采用的已知点，根据SNX文件读取坐标值
            KonwnPointsXyz = new System.Collections.Generic.Dictionary<string, XYZ>();
            CommonPointName = new System.Collections.Generic.List<string>();
            CommonPointsXyz = new System.Collections.Generic.Dictionary<string, XYZ>();
            AllPointsApproXyz = new System.Collections.Generic.Dictionary<string, XYZ>();//所有点的坐标，已知点为真值，未知的为近似值
            AllPointName = new System.Collections.Generic.List<string>();
            BaselineInfo = new System.Collections.Generic.Dictionary<string, XYZ>(); //存放观测值基线残差信息
            BaselineInverseOfWegihtInfo = new System.Collections.Generic.Dictionary<string, SymmetricMatrix>();//存放观测值的协方差阵

            var coordNamePath = this.fileOpenControl2_KnownPointName.FilePath;
            StreamReader sr = new StreamReader(coordNamePath);
            string strLine=sr.ReadLine();
            while ((strLine) != null && strLine != "")
            {
                string[] names = strLine.Split(new char[] { ' ', ',', ';', '\t' });
                foreach(var item in names)
                { if (!allKnownPointName.Contains(item)) { allKnownPointName.Add(item.ToUpper()); } }
                strLine = sr.ReadLine();
            }
            sr.Close();
            var coordPath = this.fileOpenControl1.FilePath;
            if (coordPath == null || !Path.GetExtension(coordPath).Contains(".snx"))
            { throw new Exception("请指定坐标基准文件SNX"); }

            SinexFile SinexFile = new Data.Sinex.SinexFile(coordPath);
            SinexFile fileA = SinexReader.Read(coordPath, true);
            List<NamedXyz> listSnx = fileA.GetSiteEstimatedCoords();
            Dictionary<string, XYZ> SnxXyz = new System.Collections.Generic.Dictionary<string, XYZ>();//snx文件的坐标值
          //  var colName = SinexFile.GetSinexSites();
            foreach (var item in listSnx)
            {
                SnxXyz.Add(item.Name.ToUpper().Trim(), item.Value);
            }

            foreach (var item in allKnownPointName)
            {
                if (SnxXyz.ContainsKey(item))
                { 
                    KonwnPointsXyz.Add(item, SnxXyz[item]);
                    KnownPointName.Add(item);
                }
                 
            }

            var baselinefiles = this.fileOpenControl2baselinefiles.FilePathes;
            foreach (var path in baselinefiles)
            {
                using (StreamReader reader = new StreamReader(new FileStream(path, FileMode.Open)))
                {
                    int nameIndex = -1;
                    int detXIndex = -1; int detYIndex = -1; int detZIndex = -1;
                    int QxxIndex = -1; int QxyIndex = -1; int QyyIndex = -1; int QxzIndex = -1; int QyzIndex = -1; int QzzIndex = -1;
                    int RefApproXIndex = -1; int RefApproYIndex = -1; int RefApproZIndex = -1;
                    int RovApproXIndex = -1; int RovApproYIndex = -1; int RovApproZIndex = -1;
                    string line = reader.ReadLine();
                    //第一行为头部
                    string[] titles = StringUtil.SplitByTab(line);
                    int i = 0;
                    foreach (var item in titles)
                    {
                        var title = item.ToLower();
                        if (title.Contains("name") && nameIndex == -1) nameIndex = i;
                        else if (title.Contains("estvectorx") && detXIndex == -1) detXIndex = i;
                        else if (title.Contains("estvectory") && detYIndex == -1) detYIndex = i;
                        else if (title.Contains("estvectorz") && detZIndex == -1) detZIndex = i;
                        else if (title.Contains("qxx") && QxxIndex == -1) QxxIndex = i;
                        else if (title.Contains("qxy") && QxyIndex == -1) QxyIndex = i;
                        else if (title.Contains("qyy") && QyyIndex == -1) QyyIndex = i;
                        else if (title.Contains("qxz") && QxzIndex == -1) QxzIndex = i;
                        else if (title.Contains("qyz") && QyzIndex == -1) QyzIndex = i;
                        else if (title.Contains("qzz") && QzzIndex == -1) QzzIndex = i;
                        else if (title.Contains("refapprox") && RefApproXIndex == -1) RefApproXIndex = i;
                        else if (title.Contains("refapproy") && RefApproYIndex == -1) RefApproYIndex = i;
                        else if (title.Contains("refapproz") && RefApproZIndex == -1) RefApproZIndex = i;
                        else if (title.Contains("rovapprox") && RovApproXIndex == -1) RovApproXIndex = i;
                        else if (title.Contains("rovapproy") && RovApproYIndex == -1) RovApproYIndex = i;
                        else if (title.Contains("rovapproz") && RovApproZIndex == -1) RovApproZIndex = i;
                        i++;
                    }

                    while ((line = reader.ReadLine()) != null)
                    {
                        if (String.IsNullOrWhiteSpace(line)) { continue; }
                        string[] values = StringUtil.SplitByTab(line);

                        string baselineName = values[nameIndex];
                        string[] Names = baselineName.Split('-');

                        string RefName = Names[0].Substring(0, 4).ToUpper();
                        string RovName = Names[1].Substring(0, 4).ToUpper();
                        

                        XYZ baselineValue = new XYZ(double.Parse(values[detXIndex]), double.Parse(values[detYIndex]), double.Parse(values[detZIndex]));
                        double[] qx = new double[6] { double.Parse(values[QxxIndex]) + 10e-10, double.Parse(values[QxyIndex]), double.Parse(values[QyyIndex]) + 10e-10, double.Parse(values[QxzIndex]), double.Parse(values[QyzIndex]), double.Parse(values[QzzIndex]) + 10e-10 };
                        SymmetricMatrix Q = new SymmetricMatrix(qx);

                        XYZ RefApproXYZ = new XYZ(double.Parse(values[RefApproXIndex]), double.Parse(values[RefApproYIndex]), double.Parse(values[RefApproZIndex]));
                        XYZ RovApproXYZ = new XYZ(double.Parse(values[RovApproXIndex]), double.Parse(values[RovApproYIndex]), double.Parse(values[RovApproZIndex]));

                        //观测值常数项l=L(基线向量）-(流动站近似坐标-基准站近似坐标)
                        //参考站
                        if (RefName == "SHAO" && RovName == "ZJZS")
                        {
                            //
                        }
                        if (SnxXyz.ContainsKey(RefName))
                        {
                            if (!CommonPointsXyz.ContainsKey(RefName)) CommonPointsXyz.Add(RefName, SnxXyz[RefName]);
                            //if (!AllPointsApproXyz.ContainsKey(RefName)) { AllPointsApproXyz.Add(RefName, RefApproXYZ); }
                            if (!AllPointsApproXyz.ContainsKey(RefName))
                            {
                                AllPointsApproXyz.Add(RefName,RefApproXYZ);// SnxXyz[RefName]);
                                AllPointName.Add(RefName);
                            }

                            if ((CommonPointsXyz[RefName] - AllPointsApproXyz[RefName]).Length > 1.0) //暂时防止同名点情况
                            {
                                CommonPointsXyz.Remove(RefName);
                                AllPointsApproXyz[RefName] = RefApproXYZ;
                                baselineValue += AllPointsApproXyz[RefName];
                            }
                            else
                            {
                                baselineValue += CommonPointsXyz[RefName];//基线观测值减去基线近似值
                               // baselineValue += AllPointsApproXyz[RefName];
                            }
                        }
                        else
                        {
                            if (!AllPointsApproXyz.ContainsKey(RefName))
                            {
                                AllPointsApproXyz.Add(RefName, RefApproXYZ);
                                AllPointName.Add(RefName);
                            }
                            baselineValue += AllPointsApproXyz[RefName];//基线观测值减去基线近似值
                        }
                        //流动站
                        if (SnxXyz.ContainsKey(RovName))
                        {
                            if (!CommonPointsXyz.ContainsKey(RovName)) CommonPointsXyz.Add(RovName, SnxXyz[RovName]);
                            //if (!AllPointsApproXyz.ContainsKey(RovName)) { AllPointsApproXyz.Add(RovName, RovApproXYZ); }
                            if (!AllPointsApproXyz.ContainsKey(RovName))
                            {
                                AllPointsApproXyz.Add(RovName,RovApproXYZ);// SnxXyz[RovName]);
                                AllPointName.Add(RovName);
                            }

                            if ((CommonPointsXyz[RovName] - AllPointsApproXyz[RovName]).Length > 1.0) //暂时防止同名点情况
                            {
                                CommonPointsXyz.Remove(RovName);
                                AllPointsApproXyz[RovName] = RovApproXYZ;
                                baselineValue -= AllPointsApproXyz[RovName];
                            }
                            else
                            {
                                baselineValue -= CommonPointsXyz[RovName];
                               // baselineValue -= AllPointsApproXyz[RovName];
                            }
                        }
                        else
                        {
                            if (!AllPointsApproXyz.ContainsKey(RovName))
                            {
                                AllPointsApproXyz.Add(RovName, RovApproXYZ);
                                AllPointName.Add(RovName);
                            }
                            baselineValue -= AllPointsApproXyz[RovName]; //基线观测值减去基线近似值
                        }
                       //// //基线观测值减去基线近似值
                       ////// baselineValue -= (AllPointsApproXyz[RovName] - AllPointsApproXyz[RefName]);

                        //不同基线文件中可能存在同名基线，需根据基线文件再唯一标示
                        BaselineInfo.Add(RefName + "-" + RovName + "-" + Path.GetFileName(path), baselineValue);
                        BaselineInverseOfWegihtInfo.Add(RefName + "-" + RovName + "-" + Path.GetFileName(path), Q);
                    }
                }//当前基线文件遍历完
            }//所有基线文件遍历完

            //已知点的向量
            foreach(var item in KonwnPointsXyz)
            {
                if (!AllPointsApproXyz.ContainsKey(item.Key))
                {
                    AllPointsApproXyz.Add(item.Key, item.Value);
                    AllPointName.Add(item.Key);
                }
                string knownName = null;
                double length = 10e14;
                foreach (var com in CommonPointsXyz)
                {
                    if(com.Key!=item.Key)
                    {
                        double tmp=(com.Value-item.Value).Length;
                        if(tmp<length)
                        {
                            length = tmp;
                            knownName = com.Key;
                        }
                    }                  
                }             
                XYZ baseline = (CommonPointsXyz[knownName] - KonwnPointsXyz[item.Key]) - (AllPointsApproXyz[knownName] - AllPointsApproXyz[item.Key]);
                //不同基线文件中可能存在同名基线，需根据基线文件再唯一标示
                BaselineInfo.Add(item.Key + "-" + knownName + "-" + "基准基线", baseline);
                SymmetricMatrix Q = new SymmetricMatrix(3, 0); Q[0, 0] = Q[1, 1] = Q[2, 2] = 10e-10;
                BaselineInverseOfWegihtInfo.Add(item.Key + "-" + knownName + "-" + "基准基线", Q);
            }

            ////关系后续矩阵方程建立的编号，得小心改
            //foreach (var key in AllPointsApproXyz)
            //{
            //    AllPointName.Add(key.Key);
            //}
        }

        private void fileOpenControl2baselinefiles_Load(object sender, EventArgs e)
        {

        }

      


    }
}
