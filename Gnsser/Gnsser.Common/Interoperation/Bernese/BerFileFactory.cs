using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Gnsser.Interoperation.Bernese
{
    /// <summary>
    /// 文件类型
    /// </summary>
    public enum BerFileType
    {
        /// <summary>
        /// 测站文件
        /// </summary>
        STA, 
        /// <summary>
        /// ABB
        /// </summary>
        ABB,
        /// <summary>
        /// CRD
        /// </summary>
        CRD,
        /// <summary>
        /// VEL
        /// </summary>
        VEL
    }

    /// <summary>
    /// 工厂
    /// </summary>
    public static class BerFileFactory
    {     
        /// <summary>
        /// 创建
        /// </summary>
        /// <param name="oDir"></param>
        /// <param name="berFileType"></param>
        /// <returns></returns>
        public static IBerFile Create(string oDir, BerFileType berFileType )
        {
            IBerFile o = null;
            switch (berFileType)
            {
                case BerFileType.ABB:
                    o = AbbFile.CreateFromODir(oDir);
                    break;
                case BerFileType.STA:
                    o = StaFile.CreateFromODir(oDir);
                    break;
                case BerFileType.CRD:
                    o = CrdFile.CreateFromODir(oDir);
                    break;
                case BerFileType.VEL:
                    o = VelFile.CreateFromODir(oDir);
                    break; 
                default: break;
            }
            return o;
        }
        /// <summary>
        /// 合并
        /// </summary>
        /// <param name="textA"></param>
        /// <param name="textB"></param>
        /// <param name="berFileType"></param>
        /// <returns></returns>
        public static IBerFile Merge(string textA, string textB, BerFileType berFileType)
        {
          IBerFile a = null,  b = null;
            switch (berFileType)
            {
                case BerFileType.ABB:
                    a = AbbFile.ParseText(textA);
                    b = AbbFile.ParseText(textB); 
                    break;
                case BerFileType.STA:
                    a = StaFile.ParseText(textA);
                    b = StaFile.ParseText(textB);                     
                    break;
                case BerFileType.CRD:
                    a = CrdFile.ParseText(textA);
                    b = CrdFile.ParseText(textB); 
                    break;
                case BerFileType.VEL:
                    a = VelFile.ParseText(textA);
                    b = VelFile.ParseText(textB); 
                    break;
                default: break;
            }
            return Merge(a, b, berFileType);
        }  
        /// <summary>
        /// 合并
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <param name="berFileType"></param>
        /// <returns></returns>
        public static IBerFile Merge(IBerFile a, IBerFile b, BerFileType berFileType)
        {
            IBerFile o = null;
            switch (berFileType)
            {
                case BerFileType.ABB:
                    o = AbbFile.Merger(a as AbbFile, b as AbbFile);
                    break;
                case BerFileType.STA:
                    o = StaFile.Merger(a as StaFile, b as StaFile);
                    break;
                case BerFileType.CRD:
                    o = CrdFile.Merger(a as CrdFile, b as CrdFile);
                    break;
                case BerFileType.VEL:
                    o = VelFile.Merger(a as VelFile, b as VelFile);
                    break; 
                default: break;
            }
            return o;
        }
    }


}
