//2016.12.12 double create in hongqing, 对称矩阵的并行求逆,基于崔阳师兄以前的版本


using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Geo.Algorithm;
using Geo.Algorithm;
 

namespace Geo.Algorithm.Adjust
{
    public class ParallelInverseOfSymmetric:Geo.Algorithm.Adjust.AdjustResultMatrix
    {
        public IMatrix invA;
        private IMatrix orignA;
        private int coreNum;//计算机的物理核数，为2的倍数，如2、4、8、16、32等
        public ParallelInverseOfSymmetric(IMatrix A)
        {
            //A是对称正定矩阵
            if (!A.IsSquare || !A.IsSymmetric)
            { throw new Exception("矩阵不是是对称的方阵！"); }
            //
            this.orignA = A;
            //this.orignA = new Matrix(8, 8);
            //for (int i = 0; i < 8; i++)
            //    for (int j = 0; j < 8; j++)
            //    {
            //        if (i == j) orignA[i, j] = 2;
            //        else orignA[i, j] = 1;
            //    }
            coreNum = 4;
            inverse();
        }


        private cyMatrix cA;
        private cyMatrix cA1;
        private cyMatrix cA2;
        private IMatrix[] DiagM;
        private IMatrix[] DiagQ;
        private IMatrix cA1Q21;
        private IMatrix cA1Q22;
        private IMatrix Q21;
        private IMatrix Q22;
        public void inverse()
        {
            //
            int log = (int)Math.Log(coreNum, 2);//返回指定数字的对数
            //总共经过log级的分解完成并行计算
            if (log == 2)//4核
            {
                DateTime start = DateTime.Now;
                var span = DateTime.Now - start;
                //1级分裂
                cA = new cyMatrix(orignA, 2, 2);
                //start = DateTime.Now;
                //2级分裂
                cA1 = new cyMatrix(cA.blockMatrix1[0], 2, 2);
                cA2 = new cyMatrix(cA.blockMatrix1[2], 2, 2);
                DiagM = new SymmetricMatrix[coreNum];
                DiagM[0] = cA1.blockMatrix1[0].Clone();
                DiagM[1] = cA1.blockMatrix1[2].Clone();
                DiagM[2] = cA2.blockMatrix1[0].Clone();
                DiagM[3] = cA2.blockMatrix1[2].Clone();
                Parallel.For(0, coreNum, (int i) =>
                {
                    DiagM[i] = DiagM[i].GetInverse();//.Inverse;
                });
                //var sssss = s11.Multiply(DiagM[0]);
                //并行求对角线上的块矩阵
                DiagQ = new SymmetricMatrix[coreNum];
                //通过使用任务来对代码进行并行化
                //创建任务
                var t0 = new Task(() => GenerateDiagQ0());
                var t1 = new Task(() => GenerateDiagQ1());
                var t2 = new Task(() => GenerateDiagQ2());
                var t3 = new Task(() => GenerateDiagQ3());
                t0.Start(); t1.Start(); t2.Start(); t3.Start();
                Task.WaitAll(t0, t1, t2, t3);//等待所有任务的完成

                
                //求逆
                Parallel.For(0, coreNum, (int i) =>
                {
                    if (i != 1) { DiagQ[i] = DiagQ[i].GetInverse();}
                    
                });

                #region 比下面这个多5ms，故省去
                //start = DateTime.Now; var b0 = new Task(() => GenenateBlock0());
                //var b1 = new Task(() => GenenateBlock1());
                //var b2 = new Task(() => GenenateBlock2());
                //var b3 = new Task(() => GenenateBlock3());
                //b0.Start(); b1.Start(); b2.Start(); b3.Start();
                //Task.WaitAll(b0, b1, b2, b3);//等待所有任务的完成
                //span = DateTime.Now - start;
                //Console.WriteLine(span.TotalMilliseconds + "ms融合3----");
                #endregion

                //GenenateBlockQ22();
                
                var c0 = new Task(() => GenenateBlockQ21());
                var c1 = new Task(() => GenenateBlockQ22());
                var c2 = new Task(() => GenenateBlock2());
                c0.Start(); c1.Start(); c2.Start(); Task.WaitAll(c0,c1, c2); 

                cA1.blockMatrix1[0] = DiagQ[0];
                cA1.blockMatrix1[1] = cA1Q21;
                cA1.blockMatrix1[2] = cA1Q22;
                cA2.blockMatrix1[0] = DiagQ[2];
                cA2.blockMatrix1[2] = DiagQ[3];


                DiagM[0] = cA1.uniteBlockOfSymmetric(cA1, 1);
                DiagM[1] = cA2.uniteBlockOfSymmetric(cA2, 1);
                //var sss = DiagM[0].Multiply(cA.blockMatrix1[0]);
                var nt0 = new Task(() => GeneratenewDiagQ0());
                var nt1 = new Task(() => GeneratenewDiagQ1());
                nt0.Start();
                nt1.Start();
                Task.WaitAll(nt0, nt1);//等待所有任务的完成
                //Parallel.For(0, 2, (int i) =>
                //{
                //    DiagQ[i] = DiagQ[i].GetInverse();//.Inverse;
                //});
                DiagQ[0] = DiagQ[0].GetInverse();

                #region 比下面这个直接按顺序计算多15ms，故省去
                //start = DateTime.Now;
                //var nb0 = new Task(() => GeneratenewBlock0());
                //var nb1 = new Task(() => GeneratenewBlock1());
                //nb0.Start(); nb1.Start();
                //Task.WaitAll(nb0, nb1);//等待所有任务的完成
                //span = DateTime.Now - start;
                //Console.WriteLine(span.TotalMilliseconds + "ms融合30000");
                #endregion


                var task0 = new Task(() => GeneratenewBlockQ22());
                var task1 = new Task(() => GeneratenewBlockQ21());
                task0.Start(); task1.Start();
                Task.WaitAll(task0, task1);

                cA.blockMatrix1[0] = DiagQ[0];
                cA.blockMatrix1[1] = Q21;
                cA.blockMatrix1[2] = Q22;

                invA = cA.uniteBlockOfSymmetric(cA, 1);
                //var sssss0 = invA.Multiply(orignA);
            }
        }

       

        

        private void GenerateDiagQ0()
        {
            var blockMatrix00 = cA1.blockMatrix1[0] as SymmetricMatrix;
            DiagQ[0] = blockMatrix00.Minus(new SymmetricMatrix(cA1.blockMatrix1[1].Transposition.Multiply(DiagM[1]).Multiply(cA1.blockMatrix1[1]).Array));//, DiagM[1]));// cA1.blockMatrix[0][1] * DiagM[1] * cA1.blockMatrix[1][0];
        }
        private void GenerateDiagQ1()
        {
            var blockMatrix11 = cA1.blockMatrix1[2] as SymmetricMatrix;
            DiagQ[1] = blockMatrix11.Minus(new SymmetricMatrix(cA1.blockMatrix1[1].Multiply(DiagM[0]).Multiply(cA1.blockMatrix1[1].Transposition).Array));// cA1.blockMatrix[1][0] * DiagM[0] * cA1.blockMatrix[0][1];
        }
        private void GenerateDiagQ2()
        {
            var blockMatrix00 = cA2.blockMatrix1[0] as SymmetricMatrix;
            DiagQ[2] = blockMatrix00.Minus(new SymmetricMatrix(cA2.blockMatrix1[1].Transposition.Multiply(DiagM[3]).Multiply(cA2.blockMatrix1[1]).Array));// cA2.blockMatrix[0][1] * DiagM[3] * cA2.blockMatrix[1][0];
        }
        private void GenerateDiagQ3()
        {
            var blockMatrix11 = cA2.blockMatrix1[2] as SymmetricMatrix;
            DiagQ[3] = blockMatrix11.Minus(new SymmetricMatrix(cA2.blockMatrix1[1].Multiply(DiagM[2]).Multiply(cA2.blockMatrix1[1].Transposition).Array));// cA2.blockMatrix[1][0] * DiagM[2] * cA2.blockMatrix[0][1];
        }
        
        private void GenenateBlockQ21()
        {
            cA1Q21 = DiagM[1].Multiply(-1).Multiply(cA1.blockMatrix1[1]).Multiply(DiagQ[0]);
        }
        private void GenenateBlockQ22()
        {
            cA1Q22 = DiagM[1].Plus(DiagM[1].Multiply(cA1.blockMatrix1[1]).Multiply(DiagQ[0]).Multiply(cA1.blockMatrix1[1].Transposition).Multiply(DiagM[1]));
        }
        private void GenenateBlock2()
        {
            cA2.blockMatrix1[1] = DiagM[3].Multiply(-1).Multiply(cA2.blockMatrix1[1]).Multiply(DiagQ[2]);
        }

        private void GeneratenewDiagQ0()
        {
            var blockMatrix00 = cA.blockMatrix1[0] as SymmetricMatrix;
            DiagQ[0] = blockMatrix00.Minus(new SymmetricMatrix(cA.blockMatrix1[1].Transposition.Multiply(DiagM[1]).Multiply(cA.blockMatrix1[1]).Array));// cA.blockMatrix[0][1] * DiagM[1] * cA.blockMatrix[1][0];
        }
        private void GeneratenewDiagQ1()
        {
            var blockMatrix11 = cA.blockMatrix1[2] as SymmetricMatrix;
            DiagQ[1] = blockMatrix11.Minus(new SymmetricMatrix(cA.blockMatrix1[1].Multiply(DiagM[0]).Multiply(cA.blockMatrix1[1].Transposition).Array));// cA.blockMatrix[1][0] * DiagM[0] * cA.blockMatrix[0][1];
        }

        
        private void GeneratenewBlockQ21()
        {
            Q21 = DiagM[1].Multiply(-1).Multiply(cA.blockMatrix1[1]).Multiply(DiagQ[0]);
        }
        private void GeneratenewBlockQ22()
        {
            Q22 = DiagM[1].Plus(DiagM[1].Multiply(cA.blockMatrix1[1]).Multiply(DiagQ[0]).Multiply(cA.blockMatrix1[1].Transposition).Multiply(DiagM[1]));
        }
         
    }


    public class ParallelInverseOfSymmetric1 : Geo.Algorithm.Adjust.AdjustResultMatrix
    {
        
        public ParallelInverseOfSymmetric1(IMatrix A,int DiagRowCount)
        {
            //A是对称正定矩阵
            if (!A.IsSquare || !A.IsSymmetric)
            { throw new Exception("矩阵不是是对称的方阵！"); }
            this.OriginA = A;
            this.DiagRowCount = DiagRowCount;
            inverse();
        }
        #region 参数
        public int DiagRowCount { get; set; }
        public SymmetricMatrix SymmetricMatrix { get; set; }
        public DiagonalMatrix DiagonalMatrix { get; set; }
        public IMatrix OriginA { get; set; }
        public SymmetricMatrix invA { get; set; }
        #endregion
        public void inverse()
        {
            block(OriginA, DiagRowCount);
            //ParallelInverseOfSymmetric2 pppp = new ParallelInverseOfSymmetric2(SymmetricMatrix);
            //var aa = pppp.invA.Multiply(SymmetricMatrix);
            //invA = uniteBlockOfSymmetric(DiagonalMatrix, pppp.invA);


            DiagonalMatrix = DiagonalMatrix.Inverse();
            invA = uniteBlockOfSymmetric(DiagonalMatrix, SymmetricMatrix.Inverse());
            //var sss = OriginA.Multiply(invA);
        }
        
        private  void block(IMatrix Matrix, int DiagRowCount)
        {
            int row = Matrix.RowCount;
            int countOfSymmetric = row - DiagRowCount;
            SymmetricMatrix = new SymmetricMatrix(countOfSymmetric);
            double[] SymmetricMatrixVector = SymmetricMatrix.Vector;
            DiagonalMatrix = new DiagonalMatrix(DiagRowCount,1.0);
            double[] DiagonalMatrixVector = DiagonalMatrix.Vector;
            double[][] MatrixArray = Matrix.Array;
            for (int i = 0; i < DiagRowCount; i++)
            {
                double[] MatrixArrayVector = MatrixArray[i];
                DiagonalMatrixVector[i] = MatrixArrayVector[i];
            }
            for (int i = 0; i < countOfSymmetric; i++)
            {
                int rowOfSymmetrix = i + DiagRowCount;
                double[] MatrixArrayVector = MatrixArray[rowOfSymmetrix];
                SymmetrixData(DiagRowCount, SymmetricMatrixVector, i, MatrixArrayVector);
            }
 
        }

        private static void SymmetrixData(int DiagRowCount, double[] SymmetricMatrixVector, int i, double[] MatrixArrayVector)
        {
            for (int j = 0; j <= i; j++)
            {
                int colOfSymmetrix = j + DiagRowCount;
                SymmetricMatrixVector[i * (i + 1)/2 + j] = MatrixArrayVector[colOfSymmetrix];
            }
        }
        private SymmetricMatrix uniteBlockOfSymmetric(IMatrix Diagonal, IMatrix Symmetric)
        {
            var DiagonalMatrix = Diagonal as DiagonalMatrix;
            int countOfDiag = DiagonalMatrix.RowCount;
            double[] DiagonalLAVector = DiagonalMatrix.Vector;
            int countOfSymmetric = Symmetric.RowCount;
            int totalCount = countOfDiag + countOfSymmetric;
            double[]result=new double[totalCount*(totalCount+1)/2];
            double [][]SymmetricArray=Symmetric.Array;
            for (int i = 0; i < countOfDiag;i++ ) 
            {
                result[i*(i+3)/2]=DiagonalLAVector[i];
            }
            for(int i=0;i<countOfSymmetric;i++)
            {
                double[] SymmetricArrayVector = SymmetricArray[i];
                GetSymmetricData(countOfDiag, result, i, SymmetricArrayVector);
            }
            return new SymmetricMatrix(result);
        }

        private static void GetSymmetricData(int DiagRowCount, double[] result, int i, double[] SymmetricArrayVector)
        {
            int index = i + DiagRowCount;
            for (int j = 0; j <= i; j++)
            {
                int index1 = j + DiagRowCount;
                result[index * (index + 1) / 2 + index1] = SymmetricArrayVector[j];
            }
        } 
    }

    public class ParallelInverseOfSymmetric2 : Geo.Algorithm.Adjust.AdjustResultMatrix
    {
        public IMatrix invA;
        private IMatrix orignA;
        private int coreNum;//计算机的物理核数，为2的倍数，如2、4、8、16、32等
        public ParallelInverseOfSymmetric2(IMatrix A)
        {
            //A是对称正定矩阵
            if (!A.IsSquare || !A.IsSymmetric)
            { throw new Exception("矩阵不是是对称的方阵！"); }
            //
            this.orignA = A;
            //this.orignA = new Matrix(8, 8);
            //for (int i = 0; i < 8; i++)
            //    for (int j = 0; j < 8; j++)
            //    {
            //        if (i == j) orignA[i, j] = 2;
            //        else orignA[i, j] = 1;
            //    }
            coreNum = 4;
            inverse();
        }


        private cyMatrix cA;
        private cyMatrix cA1;
        private cyMatrix cA2;
        private IMatrix[] DiagM;
        private IMatrix[] DiagQ;



        private SymmetricMatrix Q11;
        private IMatrix Q21;
        private IMatrix Q22;
        public void inverse()
        {
            
                DateTime start = DateTime.Now;
                var span = DateTime.Now - start;
                //1级分裂
                cA = new cyMatrix(orignA, 2, 2);

                SymmetricMatrix q22 = new SymmetricMatrix (cA.blockMatrix1[2].Clone().Array);
                IMatrix q21 = cA.blockMatrix1[1].Clone();
                SymmetricMatrix Q = q22.Inverse();

                SymmetricMatrix q11 = new SymmetricMatrix(cA.blockMatrix1[0].Minus(q21.Transposition.Multiply(Q).Multiply(q21)).Array);
                Q11 = q11.Inverse();//.GetInverse();
                var task0 = new Task(() => GetQ21(q21, Q));
                var task1 = new Task(() => GetQ22(q21, Q));
                task0.Start(); task1.Start();

                Task.WaitAll(task0, task1);

                cA.blockMatrix1[0] = Q11;
                cA.blockMatrix1[1] = Q21;
                cA.blockMatrix1[2] = Q22;
                invA = cA.uniteBlockOfSymmetric(cA, 1);
                var sssss0 = invA.Multiply(orignA);

                #region hide
                ////start = DateTime.Now;
                ////2级分裂
                //cA1 = new cyMatrix(cA.blockMatrix1[0], 2, 2);
                //cA2 = new cyMatrix(cA.blockMatrix1[2], 2, 2);
                //DiagM = new SymmetricMatrix[coreNum];
                //DiagM[0] = cA1.blockMatrix1[0].Clone();
                //DiagM[1] = cA1.blockMatrix1[2].Clone();
                //DiagM[2] = cA2.blockMatrix1[0].Clone();
                //DiagM[3] = cA2.blockMatrix1[2].Clone();
                ////var s11 = DiagM[0].Clone();
                //Parallel.For(0, coreNum, (int i) =>
                //{
                //    DiagM[i] = DiagM[i].GetInverse();//.Inverse;
                //});
                ////var sssss = s11.Multiply(DiagM[0]);
                ////并行求对角线上的块矩阵
                //DiagQ = new SymmetricMatrix[coreNum];
                ////通过使用任务来对代码进行并行化
                ////创建任务
                //var t0 = new Task(() => GenerateDiagQ0());
                //var t1 = new Task(() => GenerateDiagQ1());
                //var t2 = new Task(() => GenerateDiagQ2());
                //var t3 = new Task(() => GenerateDiagQ3());
                //t0.Start(); t1.Start(); t2.Start(); t3.Start();
                //Task.WaitAll(t0, t1, t2, t3);//等待所有任务的完成

                ////var q0 = DiagQ[0].Clone();
                ////var q1 = DiagQ[1].Clone();
                ////var q2 = DiagQ[2].Clone();
                ////var q3 = DiagQ[3].Clone();

                ////求逆
                //Parallel.For(0, coreNum, (int i) =>
                //{
                //    DiagQ[i] = DiagQ[i].GetInverse();//.Inverse;
                //});

                ////var r0 = q0.Multiply(DiagQ[0]);
                ////var r1 = q1.Multiply(DiagQ[1]);
                ////var r2 = q2.Multiply(DiagQ[2]);
                ////var r3 = q3.Multiply(DiagQ[3]);

                //#region 比下面这个多5ms，故省去
                ////start = DateTime.Now; var b0 = new Task(() => GenenateBlock0());
                ////var b1 = new Task(() => GenenateBlock1());
                ////var b2 = new Task(() => GenenateBlock2());
                ////var b3 = new Task(() => GenenateBlock3());
                ////b0.Start(); b1.Start(); b2.Start(); b3.Start();
                ////Task.WaitAll(b0, b1, b2, b3);//等待所有任务的完成
                ////span = DateTime.Now - start;
                ////Console.WriteLine(span.TotalMilliseconds + "ms融合3----");
                //#endregion

                //var xxx = DiagM[1].Plus(DiagM[1].Multiply(cA1.blockMatrix1[1]).Multiply(DiagQ[0]).Multiply(cA1.blockMatrix1[1].Transposition).Multiply(DiagM[1]));
                //var xxx2 = DiagM[3].Plus(DiagM[3].Multiply(cA2.blockMatrix1[1]).Multiply(DiagQ[2]).Multiply(cA2.blockMatrix1[1].Transposition).Multiply(DiagM[3]));

                //var c0 = new Task(() => GenenateBlock0());
                //var c2 = new Task(() => GenenateBlock2());
                //c0.Start(); c2.Start(); Task.WaitAll(c0, c2);

                //cA1.blockMatrix1[0] = DiagQ[0];
                ////cA1.blockMatrix1[2] = DiagQ[1];

                //cA2.blockMatrix1[0] = DiagQ[2];
                ////cA2.blockMatrix1[2] = DiagQ[3];

                ////var ooo = cA1.blockMatrix1[2].Minus(xxx);

                ////DiagM[0] = cA1.uniteBlockOfSymmetric(cA1,1);
                ////DiagM[1] = cA2.uniteBlockOfSymmetric(cA2,1);

                ////var ss = DiagM[0].Multiply(cA.blockMatrix1[0]);
                ////var ss1 = DiagM[1].Multiply(cA.blockMatrix1[2]);
                //cA1.blockMatrix1[2] = xxx;
                //cA2.blockMatrix1[2] = xxx2;

                //DiagM[0] = cA1.uniteBlockOfSymmetric(cA1, 1);
                //DiagM[1] = cA2.uniteBlockOfSymmetric(cA2, 1);
                //var sss = DiagM[0].Multiply(cA.blockMatrix1[0]);
                //var nt0 = new Task(() => GeneratenewDiagQ0());
                //var nt1 = new Task(() => GeneratenewDiagQ1());
                //nt0.Start();
                //nt1.Start();
                //Task.WaitAll(nt0, nt1);//等待所有任务的完成
                //Parallel.For(0, 2, (int i) =>
                //{
                //    DiagQ[i] = DiagQ[i].GetInverse();//.Inverse;
                //});


                //#region 比下面这个直接按顺序计算多15ms，故省去
                ////start = DateTime.Now;
                ////var nb0 = new Task(() => GeneratenewBlock0());
                ////var nb1 = new Task(() => GeneratenewBlock1());
                ////nb0.Start(); nb1.Start();
                ////Task.WaitAll(nb0, nb1);//等待所有任务的完成
                ////span = DateTime.Now - start;
                ////Console.WriteLine(span.TotalMilliseconds + "ms融合30000");
                //#endregion
                //var xxxx = DiagM[1].Plus(DiagM[1].Multiply(cA.blockMatrix1[1]).Multiply(DiagQ[0]).Multiply(cA.blockMatrix1[1].Transposition).Multiply(DiagM[1]));
                //GeneratenewBlock0();

                //cA.blockMatrix1[0] = DiagQ[0];
                ////cA.blockMatrix1[2] = DiagQ[1];
                ////invA = cA.uniteBlockOfSymmetric(cA, 1);
                ////var ssss = invA.Multiply(orignA);

                ////var oooo = cA1.blockMatrix1[2].Minus(xxx);
                //cA.blockMatrix1[2] = xxxx;
                //invA = cA.uniteBlockOfSymmetric(cA, 1);
                ////var sssss0 = invA.Multiply(orignA);
                #endregion
            
        }

        private void GetQ22(IMatrix q21, SymmetricMatrix Q)
        {
            Q22 = Q.Plus(Q.Multiply(q21).Multiply(Q11).Multiply(q21.Transposition).Multiply(Q));
        }

        private void GetQ21(IMatrix q21, SymmetricMatrix Q)
        {
            Q21 = Q.Multiply(q21).Multiply(Q11).Multiply(-1);
        }

        private void GenerateDiagQ0()
        {
            var blockMatrix00 = cA1.blockMatrix1[0] as SymmetricMatrix;
            DiagQ[0] = blockMatrix00.Minus(new SymmetricMatrix(cA1.blockMatrix1[1].Transposition.Multiply(DiagM[1]).Multiply(cA1.blockMatrix1[1]).Array));//, DiagM[1]));// cA1.blockMatrix[0][1] * DiagM[1] * cA1.blockMatrix[1][0];
        }
        private void GenerateDiagQ1()
        {
            var blockMatrix11 = cA1.blockMatrix1[2] as SymmetricMatrix;
            DiagQ[1] = blockMatrix11.Minus(new SymmetricMatrix(cA1.blockMatrix1[1].Multiply(DiagM[0]).Multiply(cA1.blockMatrix1[1].Transposition).Array));// cA1.blockMatrix[1][0] * DiagM[0] * cA1.blockMatrix[0][1];
        }
        private void GenerateDiagQ2()
        {
            var blockMatrix00 = cA2.blockMatrix1[0] as SymmetricMatrix;
            DiagQ[2] = blockMatrix00.Minus(new SymmetricMatrix(cA2.blockMatrix1[1].Transposition.Multiply(DiagM[3]).Multiply(cA2.blockMatrix1[1]).Array));// cA2.blockMatrix[0][1] * DiagM[3] * cA2.blockMatrix[1][0];
        }
        private void GenerateDiagQ3()
        {
            var blockMatrix11 = cA2.blockMatrix1[2] as SymmetricMatrix;
            DiagQ[3] = blockMatrix11.Minus(new SymmetricMatrix(cA2.blockMatrix1[1].Multiply(DiagM[2]).Multiply(cA2.blockMatrix1[1].Transposition).Array));// cA2.blockMatrix[1][0] * DiagM[2] * cA2.blockMatrix[0][1];
        }

        private void GenenateBlock0()
        {
            cA1.blockMatrix1[1] = DiagM[1].Multiply(-1).Multiply(cA1.blockMatrix1[1]).Multiply(DiagQ[0]);
        }

        private void GenenateBlock2()
        {
            cA2.blockMatrix1[1] = DiagM[3].Multiply(-1).Multiply(cA2.blockMatrix1[1]).Multiply(DiagQ[2]);
        }

        private void GeneratenewDiagQ0()
        {
            var blockMatrix00 = cA.blockMatrix1[0] as SymmetricMatrix;
            DiagQ[0] = blockMatrix00.Minus(new SymmetricMatrix(cA.blockMatrix1[1].Transposition.Multiply(DiagM[1]).Multiply(cA.blockMatrix1[1]).Array));// cA.blockMatrix[0][1] * DiagM[1] * cA.blockMatrix[1][0];
        }
        private void GeneratenewDiagQ1()
        {
            var blockMatrix11 = cA.blockMatrix1[2] as SymmetricMatrix;
            DiagQ[1] = blockMatrix11.Minus(new SymmetricMatrix(cA.blockMatrix1[1].Multiply(DiagM[0]).Multiply(cA.blockMatrix1[1].Transposition).Array));// cA.blockMatrix[1][0] * DiagM[0] * cA.blockMatrix[0][1];
        }


        private void GeneratenewBlock0()
        {
            cA.blockMatrix1[1] = DiagM[1].Multiply(-1).Multiply(cA.blockMatrix1[1]).Multiply(DiagQ[0]);
        }
         
    }
}
