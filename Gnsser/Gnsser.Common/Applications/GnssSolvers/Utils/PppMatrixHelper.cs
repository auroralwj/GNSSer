//2016.03.21, czs, edit in hongqing, 精密单点定位辅助类
//2017.05.10, lly, edit in zz, 增加多系统方法。
//2017.09.10, czs, create in hongqing, 基于参数名称的初始先验信息构造器

using System;
using Gnsser.Domain;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Geo.Coordinates;
using Gnsser.Data.Rinex;
using Geo.Algorithm;
using Geo.Algorithm.Adjust;
using Gnsser.Service;
using Geo.Utils;
using Geo.Algorithm;
using Geo;
using Gnsser;

namespace Gnsser.Service
{ 



   
    /// <summary>
    /// 首次方差
    /// </summary>
    [Obsolete("请使用 InitAprioriParamBuilder")]
    public class PppMatrixHelper
    {
        /// 构建一个首次方差参数顺序：xyz dt ,没有对流层
        /// </summary>
        /// <param name="rowCol"></param>
        /// <param name="xyzRms"></param>
        /// <param name="isSiteFixed"></param>
        /// <returns></returns>
        public static WeightedVector GetInitTropAugCombinedAprioriParam(int Totalsystem, int rowCol, XYZ xyzRms, bool isSiteFixed = false)
        {
            //   int rowCol = EpochData.NumOfUnknowns; 
            DiagonalMatrix initCova = new DiagonalMatrix(rowCol);
            double[] initCovaVector = initCova.Vector;
            //double[][] initCova = MatrixUtil.Create(rowCol, rowCol);
            //Fill the initialErrorCovariance matrix
            int i = 0;
            if (!isSiteFixed)
            {
                //Second, the coordinates
                for (i = 0; i < 3; i++)
                {
                    if (!xyzRms.Equals(XYZ.Zero))
                        initCovaVector[i] = Math.Pow(xyzRms[i], 2); //(1 m)  ^*2
                    else
                        initCovaVector[i] = 10000.0; //(100 m)  ^*2
                }
            }

            //Third, the receiver clock
            initCovaVector[3] = 9.0e10;    //(300 km) ^*2
            //First, the 系统时间偏差，没有对流层
            for (i = 4; i < 4 - 1 + Totalsystem; i++)
            {
                initCovaVector[i] = 0.25;
            }

            //Finally, the phase biases
            for (i = 4 - 1 + Totalsystem; i < rowCol; i++)
            {
                // initCova[time][time] = 1.0e100;   //(20000 km) ^*2
                //initCova[i][i] = 4.0e14;   //(20000 km) ^*2
                initCovaVector[i] = 4.0e14;   //已对齐相位，应该和伪距精度相同
            }
            return new WeightedVector(new Vector(rowCol), initCova);
        }


        

        /// <summary>
        /// 构建一个首次方差参数顺序：xyz 
        /// </summary>
        /// <param name="rowCol">行列数量</param>
        /// <param name="baseParamCount">基础参数数量</param>
        /// <param name="xyzRms">初始坐标RMS</param>
        /// <returns></returns>
        public static WeightedVector GetInitDoubleAprioriParam(int rowCol, int baseParamCount, XYZ xyzRms)
        {
            //   int rowCol = EpochData.NumOfUnknowns; 
            DiagonalMatrix initCova =new DiagonalMatrix(rowCol);
            double[] initCovaVector = initCova.Vector;
            //Fill the initialErrorCovariance matrix

            //Second, the coordinates
            for (int i = 0; i < 3; i++)
            {
                if (!xyzRms.Equals(XYZ.Zero))
                    initCovaVector[i] = Math.Pow(xyzRms[i], 2); //(1 m)  ^*2
                else
                    initCovaVector[i] = 10000.0; //(100 m)  ^*2
            }

            ////Third, the receiver clock
            //initCova[3][3] = 9.0e10;    //(300 km) ^*2
            if (baseParamCount == 4)
            {
                //First, the zenital wet tropospheric delay
                initCova[3, 3] = 0.25;    //(0.5 m) ^*2
            }
            if (baseParamCount == 5)
            {
                //First, the zenital wet tropospheric delay
                initCova[3, 3] = 0.25;    //(0.5 m) ^*2

                //First, the zenital wet tropospheric delay
                initCova[4, 4] = 0.25;    //(0.5 m) ^*2
            }
            ////First, the ionosphere delay
            //initCova[5][5] = 0.25;    //(0.5 m) ^*2


            //Finally, the phase biases
            for (int i = baseParamCount; i < rowCol; i++)
            {
                // initCova[time][time] = 1.0e100;   //(20000 km) ^*2
                initCovaVector[i] = 4.0e14;   //(20000 km) ^*2
            }
            return new WeightedVector(new Vector(rowCol), initCova);
        }
        
    }

}
