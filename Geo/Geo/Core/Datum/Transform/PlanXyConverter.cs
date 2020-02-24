//2019.01.12, czs, create in hmx, 增加平面坐标转换

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Geo.Referencing;
using Geo.Algorithm.Adjust;
using Geo.Algorithm;

namespace Geo.Coordinates
{
    
    /// <summary>
    /// 平面坐标4参数转换
    /// </summary>
    public class PlainXyConvertParam
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public PlainXyConvertParam() { }
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="Offset"></param>
        /// <param name="ScaleFactorPpm"></param>
        /// <param name="RotationAngleSecond"></param>
        public PlainXyConvertParam(XY Offset, double ScaleFactorPpm, double RotationAngleSecond) {
            this.Offset = Offset;
            this.ScaleFactorPpm = ScaleFactorPpm;
            this.RotationAngleSecond = RotationAngleSecond;
        }


        /// <summary>
        /// 平移参数 (x0, y0), 米
        /// </summary> 
        public XY Offset { get; set; }
        /// <summary>
        /// 尺度因子lambda， ppm
        /// </summary>
        public double ScaleFactorPpm { get; set; }
        /// <summary>
        /// 尺度因子lambda
        /// </summary>
        public double ScaleFactor => ScaleFactorPpm / 1e6;
        /// <summary>
        /// 旋转角 theta, 秒
        /// </summary>
        public double RotationAngleSecond { get; set; }
        /// <summary>
        /// 旋转角 theta, 弧度
        /// </summary>
        public double RotationAngleRad => RotationAngleSecond / 3600.0 * Geo.CoordConsts.DegToRadMultiplier;
        /// <summary>
        /// 参数名称
        /// </summary>
        public static List<string> ParamNames => new List<string>() { "X0", "Y0", "Lambda", "Theta" };
    }

    /// <summary>
    /// 平面坐标转换
    /// </summary>
    public class PlanXyConverter
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="xyConvertParam"></param>
        public PlanXyConverter(PlainXyConvertParam xyConvertParam)
        {
            this.PlanXyConvertParam = xyConvertParam;
        }
        /// <summary>
        /// 平面坐标4参数转换
        /// </summary>
        public PlainXyConvertParam PlanXyConvertParam { get; set; }
        /// <summary>
        /// 转换
        /// </summary>
        /// <param name="old"></param>
        /// <returns></returns>
        public List<XY> Convert(List<XY> old)
        {
            var result = new List<XY>();
            foreach (var item in old)
            {
                XY newCoord = Convert(item);
                result.Add(newCoord);
            }
            return result;
        }
        /// <summary>
        /// 转换
        /// </summary>
        /// <param name="old"></param>
        /// <returns></returns>
        public List<NamedXy> Convert(List<NamedXy> old)
        {
            var result = new List<NamedXy>();
            foreach (var item in old)
            {
                XY newCoord = Convert(item.Value);
                result.Add(new NamedXy(item.Name, newCoord));
            }
            return result;
        }
        /// <summary>
        /// 转换
        /// </summary>
        /// <param name="old"></param>
        /// <returns></returns>
        public XY Convert(XY old)
        {
            var newX = PlanXyConvertParam.Offset.X + (1 + PlanXyConvertParam.ScaleFactor) * old.X + PlanXyConvertParam.RotationAngleRad * old.Y;
            var newY = PlanXyConvertParam.Offset.Y + (1 + PlanXyConvertParam.ScaleFactor) * old.Y - PlanXyConvertParam.RotationAngleRad * old.X;

            return new XY(newX, newY);
        }  
    }

    /// <summary>
    /// 平面坐标参数转换，老系统向新系统转换。
    /// </summary>
    public class PlainXyConvertParamEstimator
    {
        /// <summary>
        /// 构造函数，以老系统数量为基准
        /// </summary>
        /// <param name="oldsOrTobeConvet"></param>
        /// <param name="newsOrTarget"></param>
        public PlainXyConvertParamEstimator(List<XY> oldsOrTobeConvet, List<XY> newsOrTarget)
        {
            this.OldOrTobeConvertingXys = oldsOrTobeConvet;
            this.NewOrTargedXys = newsOrTarget; 
        }
        /// <summary>
        /// 构造函数，以老系统数量为基准
        /// </summary>
        /// <param name="oldOrTobeConvertingXys"></param>
        /// <param name="newOrTargedXys"></param>
        public PlainXyConvertParamEstimator(List<NamedXy> oldOrTobeConvertingXys, List<NamedXy> newOrTargedXys)
        {
            this.OldOrTobeConvertingXys = new List<XY>();
            foreach (var item in oldOrTobeConvertingXys)
            {
                OldOrTobeConvertingXys.Add(item.Value);
            }
            this.NewOrTargedXys = new List<XY>();
            foreach (var item in newOrTargedXys)
            {
                NewOrTargedXys.Add(item.Value);
            }
        }
        /// <summary>
        /// 老系统坐标，待转换的坐标
        /// </summary>
        List<XY> OldOrTobeConvertingXys { get; set; }
        /// <summary>
        /// 新系统坐标，目标系统坐标
        /// </summary>
        List<XY> NewOrTargedXys { get; set; }
        /// <summary>
        /// 坐标数量
        /// </summary>
        public int CoordCount => OldOrTobeConvertingXys.Count;
        /// <summary>
        /// 观测值数量
        /// </summary>
        public int ObsCount => CoordCount * 2;
        /// <summary>
        /// 结果矩阵
        /// </summary>
        public AdjustResultMatrix ResultMatrix { get; set; }
        /// <summary>
        /// 参数估计
        /// </summary>
        /// <returns></returns>
        public PlainXyConvertParam Estimate()
        {  
            var obsVector = new Vector(ObsCount);
            //构建观测向量
            for (int i = 0; i < CoordCount; i++)
            {
                int index = 2 * i;
                var oldXy = OldOrTobeConvertingXys[i];
                var newXy = NewOrTargedXys[i];
                obsVector[index + 0] = newXy.X - oldXy.X;
                obsVector[index + 1] = newXy.Y - oldXy.Y; 
            }
            //构建系数阵
            Matrix coeffOfParam = new Matrix(ObsCount, 4);
            for (int i = 0; i < CoordCount; i++)
            {
                int index = 2 * i;
                var oldXy = OldOrTobeConvertingXys[i];
                //尺度因子，
                coeffOfParam[index + 0, 0] = 1;
                coeffOfParam[index + 1, 1] = 1;
                //转换参数，弧度
                coeffOfParam[index + 0, 2] = oldXy.X;
                coeffOfParam[index + 0, 3] = oldXy.Y;
                coeffOfParam[index + 1, 2] = oldXy.Y;
                coeffOfParam[index + 1, 3] = -oldXy.X; 
            }
             

            AdjustObsMatrix obsMatrix = new AdjustObsMatrix();
            obsMatrix.SetCoefficient(coeffOfParam).SetObservation(obsVector);
            var adjuster = new ParamAdjuster();
            this.ResultMatrix = adjuster.Run(obsMatrix);
            var ested = ResultMatrix.Estimated;

            var result = new PlainXyConvertParam(new XY(ested[0], ested[1]), ested[2] * 1e6, ested[3] * CoordConsts.RadToSecMultiplier);
            return result;
        }

    }


}
