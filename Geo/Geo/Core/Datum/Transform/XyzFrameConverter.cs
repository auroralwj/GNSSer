//2019.01.19, czs, create in hmx, XYZ坐标参考框架转换

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Geo.Referencing;
using Geo.Algorithm.Adjust;
using Geo.Algorithm;
using Geo.IO;

namespace Geo.Coordinates
{
    
    /// <summary>
    /// 坐标7参数转换
    /// </summary>
    public class XyzFrameConvertParam
    { 
        /// <summary>
        /// 默认构造函数
        /// </summary>
        /// <param name="Offset"></param>
        /// <param name="RotationAngleSecond"></param>
        /// <param name="ScaleFactorPpm"></param>
        public XyzFrameConvertParam(XYZ Offset, XYZ RotationAngleSecond ,double ScaleFactorPpm)
        {
            this.Offset = Offset;
            this.RotationAngleSecond = RotationAngleSecond;
            this.ScaleFactorPpm = ScaleFactorPpm; 
        }
        /// <summary>
        /// 平移参数 (x0, y0, z0)
        /// </summary> 
        public XYZ Offset { get; set; }
        /// <summary>
        /// 尺度因子lambda，ppm
        /// </summary>
        public double ScaleFactorPpm { get; set; }
        /// <summary>
        /// 旋转角 theta,  秒
        /// </summary>
        public XYZ RotationAngleSecond { get; set; }
        /// <summary>
        /// 旋转角，弧度
        /// </summary>
        public XYZ RotationAngleRad => RotationAngleSecond * CoordConsts.SecToRadMultiplier;

        /// <summary>
        /// 平移参数 
        /// </summary>
        public double Dx => Offset.X;
        /// <summary>
        /// 平移参数 
        /// </summary>
        public double Dy => Offset.Y;
        /// <summary>
        /// 平移参数 
        /// </summary>
        public double Dz => Offset.Z;
        /// <summary>
        /// 原始单位，非ppm
        /// </summary>
        public double m => ScaleFactorPpm * 1e-6;
        /// <summary>
        /// 旋转参数，弧度
        /// </summary>
        public double RxRad => RotationAngleRad.X;
        /// <summary>
        /// 旋转参数，弧度
        /// </summary>
        public double RyRad => RotationAngleRad.Y;
        /// <summary>
        /// 旋转参数，弧度
        /// </summary>
        public double RzRad => RotationAngleRad.Z;
        /// <summary>
        /// 参数名称
        /// </summary>
        public static List<string> ParamNames => new List<string>() { "X0", "Y0", "Z0", "Lambda", "ThetaX", "ThetaY", "ThetaZ" };
    }

    /// <summary>
    /// XYZ坐标参考框架转换
    /// </summary>
    public class XyzFrameConverter
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="xyConvertParam"></param>
        public XyzFrameConverter(XyzFrameConvertParam xyConvertParam)
        {
            this.ConvertParam = xyConvertParam;
        }
        /// <summary>
        /// 平面坐标4参数转换
        /// </summary>
        public XyzFrameConvertParam ConvertParam { get; set; }
        /// <summary>
        /// 转换
        /// </summary>
        /// <param name="old"></param>
        /// <returns></returns>
        public List<XYZ> Convert(List<XYZ> old)
        {
            var result = new List<XYZ>();
            foreach (var item in old)
            {
                XYZ newCoord = Convert(item);
                result.Add(newCoord);
            }
            return result;
        }
        /// <summary>
        /// 转换
        /// </summary>
        /// <param name="old"></param>
        /// <returns></returns>
        public List<NamedXyz> Convert(List<NamedXyz> old)
        {
            var result = new List<NamedXyz>();
            foreach (var item in old)
            {
                XYZ newCoord = Convert(item.Value);
                result.Add(new NamedXyz(item.Name, newCoord));
            }
            return result;
        }
        /// <summary>
        /// 转换
        /// </summary>
        /// <param name="old"></param>
        /// <returns></returns>
        public XYZ Convert(XYZ  old)
        { 
            return CoordinateTransform(old, ConvertParam);
        }

        public static XYZ CoordinateTransform(Geo.Coordinates.IXYZ old, XyzFrameConvertParam p)
        {
            double X, Y, Z;
            X = p.Dx + old.X * (1 + p.m) + (old.Y * p.RzRad - old.Z * p.RyRad);
            Y = p.Dy + old.Y * (1 + p.m) + (old.Z * p.RxRad - old.X * p.RzRad);
            Z = p.Dz + old.Z * (1 + p.m) + (old.X * p.RyRad - old.Y * p.RxRad);

            return new XYZ(X, Y, Z);
        }
    }

    /// <summary>
    /// 坐标参数转换，老系统向新系统转换。
    /// </summary>
    public class XyzFrameConvertParamEstimator
    {
        Log log = new Log(typeof(XyzFrameConvertParamEstimator)); 
        /// <summary>
        /// 构造函数，以老系统数量为基准
        /// </summary>
        /// <param name="oldsOrTobeConvet"></param>
        /// <param name="newsOrTarget"></param>
        public XyzFrameConvertParamEstimator(List<XYZ> oldsOrTobeConvet, List<XYZ> newsOrTarget)
        {
            this.OldOrTobeConvertingXys = oldsOrTobeConvet;
            this.NewOrTargedXys = newsOrTarget;
        }
        /// <summary>
        /// 构造函数，以老系统数量为基准
        /// </summary>
        /// <param name="oldOrTobeConvertingXys"></param>
        /// <param name="newOrTargedXys"></param>
        public XyzFrameConvertParamEstimator(List<NamedXyz> oldOrTobeConvertingXys, List<NamedXyz> newOrTargedXys)
        {
            this.OldOrTobeConvertingXys = new List<XYZ>();
            foreach (var item in oldOrTobeConvertingXys)
            {
                OldOrTobeConvertingXys.Add(item.Value);
            }
            this.NewOrTargedXys = new List<XYZ>();
            foreach (var item in newOrTargedXys)
            {
                NewOrTargedXys.Add(item.Value);
            }
        }
        /// <summary>
        /// 老系统坐标，待转换的坐标
        /// </summary>
        List<XYZ> OldOrTobeConvertingXys { get; set; }
        /// <summary>
        /// 新系统坐标，目标系统坐标
        /// </summary>
        List<XYZ> NewOrTargedXys { get; set; }
        /// <summary>
        /// 坐标数量
        /// </summary>
        public int CoordCount => OldOrTobeConvertingXys.Count;
        /// <summary>
        /// 观测值数量
        /// </summary>
        public int ObsCount => CoordCount * 3;
        /// <summary>
        /// 结果矩阵
        /// </summary>
        public AdjustResultMatrix ResultMatrix { get; set; }
        /// <summary>
        /// 参数估计
        /// </summary>
        /// <returns></returns>
        public XyzFrameConvertParam Estimate()
        {
            var obsVector = new Vector(ObsCount);
            //构建观测向量
            for (int i = 0; i < CoordCount; i++)
            {
                int index = 3 * i;
                var oldXy = OldOrTobeConvertingXys[i];
                var newXy = NewOrTargedXys[i];
                obsVector[index + 0] = newXy.X - oldXy.X;
                obsVector[index + 1] = newXy.Y - oldXy.Y;
                obsVector[index + 2] = newXy.Z - oldXy.Z;
            }
            //构建系数阵
            Matrix coeffOfParam = new Matrix(ObsCount, 7);
            for (int i = 0; i < CoordCount; i++)
            {
                int index = 3 * i;
                var oldXy = OldOrTobeConvertingXys[i];
                //平移参数XYZ，0-2，单位：m
                coeffOfParam[index + 0, 0] = 1;
                coeffOfParam[index + 1, 1] = 1;
                coeffOfParam[index + 2, 2] = 1;
                //尺度参数，3，单位：ppm，求出的直接为 ppm
                double factor = 1.0;// 1e-6;
                coeffOfParam[index + 0, 3] = oldXy.X * factor;
                coeffOfParam[index + 1, 3] = oldXy.Y * factor;
                coeffOfParam[index + 2, 3] = oldXy.Z * factor;

                //旋转参数，4-6, 单位：s,需要转换为rad
                factor = 1.0;// CoordConsts.SecToRadMultiplier;
                coeffOfParam[index + 0, 5] = -oldXy.Z * factor;
                coeffOfParam[index + 0, 6] = oldXy.Y * factor;
                coeffOfParam[index + 1, 4] = oldXy.Z * factor;
                coeffOfParam[index + 1, 6] = -oldXy.X * factor;
                coeffOfParam[index + 2, 4] = -oldXy.Y * factor;
                coeffOfParam[index + 2, 5] = oldXy.X * factor;
            }
            AdjustObsMatrix obsMatrix = new AdjustObsMatrix();
            obsMatrix.SetCoefficient(coeffOfParam).SetObservation(obsVector);
            var adjuster = new ParamAdjuster();
            this.ResultMatrix = adjuster.Run(obsMatrix);
            var ested = ResultMatrix.Estimated;

            var result = new XyzFrameConvertParam(
                new XYZ(ested[0], ested[1], ested[2]),//meter
                new XYZ(ested[4], ested[5], ested[6]) * CoordConsts.RadToSecMultiplier,//second
                ested[3] * 1e6 //ppm
                );

            //输出精度信息
            var RMSVector = ResultMatrix.StdOfEstimatedParam;
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("单位权中误差：" + this.ResultMatrix.StdDev.ToString("G6"));
            sb.Append("平移参数(m)：" + result.Offset);
            sb.AppendLine(", 标准差：" + RMSVector[0].ToString("G6") + ", " + RMSVector[1].ToString("G6") + ", " + RMSVector[2].ToString("G6"));
            sb.Append("旋转参数(s)：" + result.RotationAngleSecond);
            sb.AppendLine(", 标准差：" + RMSVector[4].ToString("G6") + ", " + RMSVector[5].ToString("G6") + ", " + RMSVector[6].ToString("G6"));
            sb.Append("尺度参数(ppm)：" + result.ScaleFactorPpm.ToString("G6"));
            sb.AppendLine(", 标准差：" + RMSVector[3].ToString("G6"));

            log.Info(sb.ToString());

            return result;
        }

    }


}
