using System;
namespace Geo.Referencing
{
    /// <summary>
    /// 布尔莎七参数
    /// </summary>
   public interface IBursaTransParams
    {
        /// <summary>
        /// 平移参数，单位米。
        /// </summary>
        double Dx { get; set; }
        /// <summary>
        /// 平移参数，单位米。
        /// </summary>
        double Dy { get; set; }
        /// <summary>
        /// 平移参数，单位米。
        /// </summary>
        double Dz { get; set; }
        /// <summary>
        /// 旋转参数
        /// </summary>
        double Ex { get; set; }
        /// <summary>
        /// 旋转参数 
        /// </summary>
        double Ey { get; set; }
        /// <summary>
        /// 旋转参数
        /// </summary>
        double Ez { get; set; }
        /// <summary>
        /// 尺度因子，单位微米。
        /// </summary>
        double Scale_ppm { get; set; }
        /// <summary>
        /// 尺度因子，单位米。
        /// </summary>
        double Scale_m { get; set; }
        /// <summary>
        /// 描述。
        /// </summary>
        string Discription { get; set; }

       /// <summary>
       /// 七参数的反算参数。
       /// </summary>
       /// <returns></returns>
        IBursaTransParams GetInverse();

        double[] GetAffineTransform();
        double[] Transform(double [] coord);
    }
}
