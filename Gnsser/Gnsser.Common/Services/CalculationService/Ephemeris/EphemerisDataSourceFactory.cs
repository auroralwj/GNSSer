using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Text;
using Gnsser.Data.Rinex;

namespace Gnsser.Data
{


    /// <summary>
    /// 星历文件类型。
    /// </summary>
    public enum FileEphemerisType
    {
        /// <summary>
        /// 未知类型
        /// </summary>
        Unkown,
        /// <summary>
        /// GPS 导航文件
        /// </summary>
        GpsNFile,
        /// <summary>
        /// 格洛纳斯
        /// </summary>
        Glonass,
        /// <summary>
        /// 北斗
        /// </summary>
        Compass,
        /// <summary>
        /// 伽利略
        /// </summary>
        Galileo,
        /// <summary>
        /// 星历
        /// </summary>
        Sp3,
        /// <summary>
        /// 混合
        /// </summary>
        Mixed
    }

    //2016.11.17, czs , create in hongqing, 星历文件服务池
    /// <summary>
    /// 星历文件服务池。
    /// </summary>
    public class FileEphemerisServicePool : Geo.BaseConcurrentDictionary<string, FileEphemerisService>
    {
        static FileEphemerisServicePool instance = new FileEphemerisServicePool();

        public static FileEphemerisServicePool Instance { get { return instance; } } 
    }


    /// <summary>
    /// 以文件为星历数据源工厂
    /// </summary>
    public  class EphemerisDataSourceFactory : Gnsser.IEphemerisServiceFactory
    {
        static FileEphemerisServicePool FileEphemerisServicePool = FileEphemerisServicePool.Instance;
        /// <summary>
        /// 从文件判断。
        /// </summary>
        /// <param name="filePath"></param>
        /// <returns></returns>
        public IFileEphemerisService CreateFromFile(string filePath)
        {
            return EphemerisDataSourceFactory.Create(filePath);
        }


        /// <summary>
        /// 自动判断路径文件类型，进行星历数据源类的初始化。
        /// </summary>
        /// <param name="naviFilePath"></param>
        /// <param name="fileType"></param>
        /// <param name="IsAvailableOnly"></param>
        /// <param name="MinSequentialSatCount"></param>
        /// <param name="MaxBreakingCount"></param>
        /// <param name="ephInterOrder"></param>
        /// <returns></returns>
        public static FileEphemerisService  Create(string naviFilePath, 
            FileEphemerisType fileType = FileEphemerisType.Unkown, 
            bool IsAvailableOnly =true, 
            int MinSequentialSatCount = 11,
            int MaxBreakingCount = 5,
            int ephInterOrder = 10
            )
        {
            if (!File.Exists(naviFilePath) || new FileInfo(naviFilePath).Length == 0)
            {
                return null;
            }

            var fileName = Path.GetFileName(naviFilePath);
            if (FileEphemerisServicePool.Contains(fileName))
            {
                return FileEphemerisServicePool[fileName];
            }  

            //如果没有手动指定，则自动判断，大多数情况下是可行的。
            if(fileType == FileEphemerisType.Unkown)
                  fileType = GetFileEphemerisTypeFromPath(naviFilePath);

            FileEphemerisService ephemerisFile = null;
           
            switch (fileType)
            {
                case FileEphemerisType.Mixed:
                    var reader2 = new MixedNavFileReader(naviFilePath);
                    ephemerisFile = new MixedNavFileEphService(reader2.ReadGnssNavFlie());
                    break;
                case FileEphemerisType.Compass:
                case FileEphemerisType.Galileo:
                case FileEphemerisType.GpsNFile:       
                        ParamNavFileReader reader = new ParamNavFileReader(naviFilePath);          
                        ephemerisFile = new SingleParamNavFileEphService( reader.ReadGnssNavFlie());
                    break;
                case FileEphemerisType.Glonass:
                    ephemerisFile = new SingleGlonassNavFileEphService( new GlonassNaviFileReader(naviFilePath).Read());
                    break;
                case FileEphemerisType.Sp3:
                    //MinSequentialSatCount = 11, int MaxBreakingCount = 5
                    ephemerisFile = new SingleSp3FileEphService( naviFilePath, MinSequentialSatCount, MaxBreakingCount, IsAvailableOnly, ephInterOrder); break;
                default: break;
            }
            FileEphemerisServicePool[fileName] = ephemerisFile;

            return ephemerisFile;
        }

        /// <summary>
        /// 分析文件路径最后一个字符，判断导航文件的类型。
        /// </summary>
        /// <param name="filePath"></param>
        /// <returns></returns>
        public static FileEphemerisType GetFileEphemerisTypeFromPath(string filePath)
        {
            FileEphemerisType type = FileEphemerisType.Unkown;
            char lastChar = filePath.ToUpper()[filePath.Length - 1];
            switch (lastChar)
            {
                case 'P':
                    type = FileEphemerisType.Mixed;
                    break;
                case 'N':
                    type = FileEphemerisType.GpsNFile;
                    break;
                case 'R':
                case 'C':
                    type = FileEphemerisType.Compass;
                    break;
                case 'G':
                    type = FileEphemerisType.Glonass;
                    break;
                case '3': //sp3
                case 'H': //eph
                    type = FileEphemerisType.Sp3;
                    break;
                default: break;
            }
            return type;
        }
    }
}
