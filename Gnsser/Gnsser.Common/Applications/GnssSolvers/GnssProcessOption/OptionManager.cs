//2018.03.20, czs, create in hmx, 选项与文件
//2018.03.20, czs, edit in hmx, 存储核心改为Config，利于文件存储和交互


using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using Gnsser.Data.Rinex;
using Gnsser.Domain;
using Geo.Times;
using Geo.Coordinates;
using Gnsser;
using Gnsser.Core;
using Geo;
using Geo.IO;
using Gnsser.Service;
using Geo.Algorithm;
using Geo.Algorithm.Adjust;

namespace Gnsser
{  
    /// <summary>
    /// 选项值转换器
    /// </summary>
    public class OptionManager
    {
        /// <summary>
        /// 默认构造函数
        /// </summary>
        public OptionManager()
        {

        }
        /// <summary>
        /// 写入路径
        /// </summary>
        /// <param name="option"></param>
        /// <param name="path"></param>
        public void Write(GnssProcessOption option, String path)
        {
            OptionConfigWriter objectConfigWriter = new OptionConfigWriter(path);
            objectConfigWriter.Write(option.Data);
        }

        /// <summary>
        /// 读取配置
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public GnssProcessOption Read(String path)
        {
            FileStream stream = new FileStream(path, FileMode.Open);
            string name = System.IO.Path.GetFileName(path);
            return Read(stream, name);
        }
        /// <summary>
        /// 读取配置
        /// </summary>
        /// <param name="name"></param>
        /// <param name="gnssOptionText"></param>
        /// <returns></returns>
        public GnssProcessOption Read(String gnssOptionText, string name)
        {
            MemoryStream memory = new MemoryStream();
            StreamWriter writer = new StreamWriter(memory);
            writer.Write(gnssOptionText);
            writer.Flush();
            memory.Position = 0;
            return Read(memory , name);
        }
        /// <summary>
        /// 读取数据流
        /// </summary>
        /// <param name="stream"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        public  GnssProcessOption Read(Stream stream, string name)
        {
            //new OptionManager().Read(optPath)

            OptionConfigReader reader = new OptionConfigReader(stream);
            var config = reader.Read();
            config.Name = name;
            return new GnssProcessOption(config);
        }

        /// <summary>
        /// 是否是枚举类型
        /// </summary>
        /// <param name="optionName"></param>
        /// <returns></returns>
        public static bool IsEnumType(OptionName optionName)
        {
            var type = GetNameTypeDic()[optionName];
            switch (type)
            {
                case OptionParamType.String:
                   return false;
                case OptionParamType.Double:
                   return false;
                case OptionParamType.Int:
                   return false;
                case OptionParamType.Bool:
                   return false;
                case OptionParamType.DateTime:
                   return false;
                case OptionParamType.XYZ:
                   return false;
                case OptionParamType.List_SatelliteType:
                   return false;
                case OptionParamType.List_String:
                   return false;
                case OptionParamType.SatelliteNumber:
                   return false;
                case OptionParamType.Dictionary_CycleSlipDetectorType_Bool:
                   return false;
                case OptionParamType.WeightedVector:
                   return false;
                case OptionParamType.PositionType:
                   return true;
                case OptionParamType.ProcessType:
                   return true;
                case OptionParamType.RinexObsFileFormatType:
                   return true;
                case OptionParamType.AdjustmentType:
                   return true;
                case OptionParamType.GnssSolverType:
                   return true;
                case OptionParamType.RangeType:
                   return true;
                case OptionParamType.SatObsDataType:
                   return true;
                case OptionParamType.SatApproxDataType:
                   return true;
                case OptionParamType.CaculateType:
                   return true;
                default:
                    break;
            }
            return false;
        }


        public static String ObjToString(Object obj)
        {
            if (obj == null) return String.Empty;

            var type = GetType(obj);
            switch (type)
            {
                case OptionParamType.SatelliteNumber:
                    return obj.ToString();
                    break;
                case OptionParamType.String:
                    return (string)obj;
                    break;
                case OptionParamType.Double:
                    return obj.ToString();
                    break;
                case OptionParamType.Int:
                    return obj.ToString();
                    break;
                case OptionParamType.Bool:
                    return obj.ToString();
                    break;
                case OptionParamType.DateTime:
                    return ((DateTime)obj).ToString("yyyy-MM-dd HH:mm:ss");
                    break;
                case OptionParamType.PositionType:
                    return obj.ToString();
                    break;
                case OptionParamType.ProcessType:
                    return obj.ToString();
                    break;
                case OptionParamType.RinexObsFileFormatType:
                    return obj.ToString();
                    break;
                case OptionParamType.AdjustmentType:
                    return obj.ToString();
                    break;
                case OptionParamType.GnssSolverType:
                    return obj.ToString();
                    break;
                case OptionParamType.XYZ:
                    return obj.ToString();
                    break;
                case OptionParamType.RangeType:
                    return obj.ToString();
                    break;
                case OptionParamType.SatObsDataType:
                    return obj.ToString();
                    break;
                case OptionParamType.SatApproxDataType:
                    return obj.ToString();
                    break;
                case OptionParamType.List_SatelliteType:
                    var list = (List<SatelliteType>)obj;
                    return ListToString(list);
                    break;
                case OptionParamType.List_String:
                    var list2 = (List<String>)obj;
                    return ListToString(list2);
                    break;
                case OptionParamType.CaculateType:
                    return obj.ToString();
                    break;
                case OptionParamType.Dictionary_CycleSlipDetectorType_Bool:
                    var dic = (Dictionary<CycleSlipDetectorType, bool>)(obj);
                    return DicToString(dic);
                    break;
                case OptionParamType.WeightedVector:
                    return obj.ToString();
                    break;
                default:
                    return obj.ToString();
                    break;
            }
            throw new Exception("解析错误，赶快联系开发人员 gnsser@163.com");
        }

        private static string ListToString<T>(List<T> list)
        {
            StringBuilder sb = new StringBuilder();
            int i = 0;
            foreach (var item in list)
            {
                if (i != 0) { sb.Append(","); }
                sb.Append(item);
                i++;
            }

            return sb.ToString();
        }

        private static string DicToString(Dictionary<CycleSlipDetectorType, bool> dic)
        {
            StringBuilder sb = new StringBuilder();
            int i = 0;
            foreach (var item in dic)
            {
                if (i != 0) { sb.Append(";"); }
                sb.Append(item.Key + ":" + item.Value);
                i++;
            }

            return sb.ToString();
        }
        /// <summary>
        /// 获取数据类型的系统 Type
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public static Type GetSysType(OptionName name)
        {
            OptionParamType keyType = GetTypeByKey(name);
            Type type = typeof(String) ;
            switch (keyType)
            {
                case OptionParamType.String:
                    type = typeof(String);
                    break;
                case OptionParamType.Double:
                    type = typeof(Double);
                    break;
                case OptionParamType.Int:
                    type = typeof(int);
                    break;
                case OptionParamType.Bool:
                    type = typeof(bool);
                    break;
                case OptionParamType.DateTime:
                    type = typeof(DateTime);
                    break;
                case OptionParamType.PositionType:
                    type = typeof(PositionType);
                    break;
                case OptionParamType.ProcessType:
                    type = typeof(ProcessType);
                    break;
                case OptionParamType.RinexObsFileFormatType:
                    type = typeof(RinexObsFileFormatType);
                    break;
                case OptionParamType.AdjustmentType:
                    type = typeof(AdjustmentType);
                    break;
                case OptionParamType.GnssSolverType:
                    type = typeof(GnssSolverType);
                    break;
                case OptionParamType.XYZ:
                    type = typeof(XYZ);
                    break;
                case OptionParamType.RangeType:
                    type = typeof(RangeType);
                    break;
                case OptionParamType.SatObsDataType:
                    type = typeof(SatObsDataType);
                    break;
                case OptionParamType.SatApproxDataType:
                    type = typeof(SatApproxDataType);
                    break;
                case OptionParamType.SatelliteNumber:
                    type = typeof(SatelliteNumber);
                    break;
                case OptionParamType.List_SatelliteType:
                    type = typeof(List<SatelliteType>);
                    break;
                case OptionParamType.List_String:
                    type = typeof(List<String>);
                    break;
                case OptionParamType.CaculateType:
                    type = typeof(CaculateType);
                    break;
                case OptionParamType.Dictionary_CycleSlipDetectorType_Bool:
                    type = typeof(Dictionary<CycleSlipDetectorType, bool>);
                    break;
                case OptionParamType.WeightedVector:
                    type = typeof(WeightedVector);
                    break;
                default:
                    break;
            }
            return type;
        }
        /// <summary>
        /// 获取类型OptionParamType
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public static OptionParamType GetTypeByKey(OptionName name)
        {
            return GetNameTypeDic()[name];
        }
        /// <summary>
        /// 通过对象获取Option类型OptionParamType
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static OptionParamType GetType(Object obj)
        {
            if (obj is Double || obj is float)
            {
                return OptionParamType.Double;
            }
            if (obj is DateTime)
            {
                return OptionParamType.DateTime;
            }
            if (obj is Int32 || obj is Int16 || obj is Int64)
            {
                return OptionParamType.Double;
            }
            if (obj is bool)
            {
                return OptionParamType.Bool;
            }
            if (obj is String)
            {
                return OptionParamType.String;
            }
            if (obj is PositionType)
            {
                return OptionParamType.PositionType;
            }

            if (obj is ProcessType)
            {
                return OptionParamType.ProcessType;
            }

            if (obj is RinexObsFileFormatType)
            {
                return OptionParamType.RinexObsFileFormatType;
            }

            if (obj is AdjustmentType)
            {
                return OptionParamType.AdjustmentType;
            }

            if (obj is GnssSolverType)
            {
                return OptionParamType.GnssSolverType;
            }

            if (obj is XYZ)
            {
                return OptionParamType.XYZ;
            }

            if (obj is RangeType)
            {
                return OptionParamType.RangeType;
            }

            if (obj is SatObsDataType)
            {
                return OptionParamType.SatObsDataType;
            }

            if (obj is SatApproxDataType)
            {
                return OptionParamType.SatApproxDataType;
            }

            if (obj is List<SatelliteType>)
            {
                return OptionParamType.List_SatelliteType;
            }
            if (obj is List<String>)
            {
                return OptionParamType.List_String;
            }

            if (obj is CaculateType)
            {
                return OptionParamType.CaculateType;
            }

            if (obj is Dictionary<CycleSlipDetectorType, bool>)
            {
                return OptionParamType.Dictionary_CycleSlipDetectorType_Bool;
            }

            if (obj is WeightedVector)
            {
                return OptionParamType.WeightedVector;
            }
            if (obj is SatelliteNumber)
            {
                return OptionParamType.SatelliteNumber;
            }

            throw new Exception("解析错误，赶快联系开发人员 gnsser@163.com");
            return OptionParamType.String;
        }

        /// <summary>
        /// 通过对象获取Option类型OptionParamType
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static OptionParamType GetTypeBySysType(Type type)
        {
            if (type == typeof(Double) || type == typeof(float)) {
                return OptionParamType.Double;
            }
            if (type == typeof(DateTime)) {
                return OptionParamType.DateTime; }
            if (type == typeof(Int32) || type == typeof(Int16) || type == typeof(Int64)) {
                return OptionParamType.Double;
            }
            if (type == typeof(bool)) {
                return OptionParamType.Bool;
            }
            if (type == typeof(String)) {
                return OptionParamType.String;
            }
            if (type == typeof(PositionType)) {
                return OptionParamType.PositionType;
            }

            if (type == typeof(ProcessType)) {
                return OptionParamType.ProcessType;
            }

            if (type == typeof(RinexObsFileFormatType)) {
                return OptionParamType.RinexObsFileFormatType;
            }

            if (type == typeof(AdjustmentType)) {
                return OptionParamType.AdjustmentType;
            }

            if (type == typeof(GnssSolverType)) {
                return OptionParamType.GnssSolverType;
            }

            if (type == typeof(XYZ)) {
                return OptionParamType.XYZ;
            }

            if (type == typeof(RangeType)) { return OptionParamType.RangeType;
            }

            if (type == typeof(SatObsDataType)) {
                return OptionParamType.SatObsDataType;
            }

            if (type == typeof(SatApproxDataType)) {
                return OptionParamType.SatApproxDataType;
            }

            if (type == typeof(List<SatelliteType>)) {
                return OptionParamType.List_SatelliteType;
            }
            if (type == typeof(List<String>)) {
                return OptionParamType.List_String;
            }

            if (type == typeof(CaculateType)) {
                return OptionParamType.CaculateType;
            }

            if (type == typeof(Dictionary<CycleSlipDetectorType, bool>)) {
                return OptionParamType.Dictionary_CycleSlipDetectorType_Bool;
            }

            if (type == typeof(WeightedVector)) {
                return OptionParamType.WeightedVector;
            }
            if (type == typeof(SatelliteNumber)) {
                return OptionParamType.SatelliteNumber;
            }

            throw new Exception("解析错误，赶快联系开发人员 gnsser@163.com");
            return OptionParamType.String;
        }
        /// <summary>
        /// 解析参数名称
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public static OptionName ParseKey(string name)
        {
            return Geo.Utils.EnumUtil.Parse<OptionName>(name);
        }
        /// <summary>
        /// 解析参数值
        /// </summary>
        /// <param name="strVal"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        public static Object ParseValue(String strVal, OptionName name)
        {
            OptionParamType type = GetNameTypeDic()[name];

            return ParseValue(strVal, type);
        }

        /// <summary>
        /// 解析值
        /// </summary>
        /// <param name="strVal"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        public static Object ParseValue(String strVal, OptionParamType type)
        {
            Object obj = null;
            switch (type)
            {
                case OptionParamType.String:
                    obj = strVal;
                    break;
                case OptionParamType.SatelliteNumber:
                    obj = SatelliteNumber.Parse(strVal);
                    break;
                case OptionParamType.Double:
                    if (String.IsNullOrEmpty(strVal)) { obj = 0; }
                    else obj = Double.Parse(strVal);
                    break;
                case OptionParamType.Int:
                    if (String.IsNullOrEmpty(strVal)) { obj = 0; }
                    else obj = Int32.Parse(strVal);
                    break;
                case OptionParamType.Bool:
                    if (String.IsNullOrEmpty(strVal) || strVal == "0") { obj = false; }
                    else { obj = Boolean.Parse(strVal); } 
                    break;
                case OptionParamType.DateTime:
                    if (String.IsNullOrEmpty(strVal)) { obj = Time.MinValue.DateTime; }
                    else obj = DateTime.Parse(strVal);
                    break;
                case OptionParamType.PositionType:
                    if (String.IsNullOrEmpty(strVal)) { obj = PositionType.静态定位; }
                    else obj = (PositionType)Enum.Parse(typeof(PositionType), strVal);
                    break;
                case OptionParamType.ProcessType:
                    if (String.IsNullOrEmpty(strVal)) { obj = ProcessType.仅计算; }
                    else obj = (ProcessType)Enum.Parse(typeof(ProcessType), strVal);
                    break;
                case OptionParamType.RinexObsFileFormatType:
                    if (String.IsNullOrEmpty(strVal)) { obj = RinexObsFileFormatType.单站单历元; }
                    else obj = (RinexObsFileFormatType)Enum.Parse(typeof(RinexObsFileFormatType), strVal);
                    break;
                case OptionParamType.AdjustmentType:
                    if (String.IsNullOrEmpty(strVal)) { obj = AdjustmentType.卡尔曼滤波; }
                    else obj = (AdjustmentType)Enum.Parse(typeof(AdjustmentType), strVal);
                    break;
                case OptionParamType.GnssSolverType:
                    if (String.IsNullOrEmpty(strVal)) { obj = GnssSolverType.动态伪距定位; }
                    else obj = (GnssSolverType)Enum.Parse(typeof(GnssSolverType), strVal);
                    break;
                case OptionParamType.XYZ:
                    if (String.IsNullOrEmpty(strVal)) { obj = XYZ.Zero; }
                    else obj = XYZ.Parse(strVal);
                    break;
                case OptionParamType.RangeType:
                    if (String.IsNullOrEmpty(strVal)) { obj = RangeType.IonoFreeRangeOfAB; }
                    else obj = (RangeType)Enum.Parse(typeof(RangeType), strVal);
                    break;
                case OptionParamType.SatObsDataType:
                    if (String.IsNullOrEmpty(strVal)) { obj = SatObsDataType.IonoFreeRange; }
                    else obj = (SatObsDataType)Enum.Parse(typeof(SatObsDataType), strVal);
                    break;
                case OptionParamType.SatApproxDataType:
                    if (String.IsNullOrEmpty(strVal)) { obj = SatApproxDataType.ApproxPseudoRangeA; }
                    else obj = (SatApproxDataType)Enum.Parse(typeof(SatApproxDataType), strVal);
                    break;
                case OptionParamType.List_SatelliteType:
                    if (String.IsNullOrEmpty(strVal)) { obj = new List<SatelliteType>() { SatelliteType.G }; }
                    else
                    {
                        List<SatelliteType> list = ParseEnumList<SatelliteType>(strVal);
                        obj = list;
                    }
                    break;
                case OptionParamType.List_String:
                    if (String.IsNullOrEmpty(strVal)) { obj = new List<String>() {   }; }
                    else
                    {
                        List<String> list = ParseList<String>(strVal);
                        obj = list;
                    }
                    break;
                case OptionParamType.CaculateType:
                    if (String.IsNullOrEmpty(strVal)) { obj = CaculateType.Filter; }
                    else obj = (CaculateType)Enum.Parse(typeof(CaculateType), strVal);
                    break;
                case OptionParamType.Dictionary_CycleSlipDetectorType_Bool:
                    if (String.IsNullOrEmpty(strVal)) { obj = new Dictionary<CycleSlipDetectorType, bool>() ; }
                    else
                    {
                        Dictionary<CycleSlipDetectorType, bool> dic = ParseBoolDic<CycleSlipDetectorType>(strVal);
                        obj = dic;
                    }
                    break;
                case OptionParamType.WeightedVector:
                    if (String.IsNullOrEmpty(strVal)) { obj = WeightedVector.GetZeroVector(0); ; }
                    else
                    {
                        WeightedVector vector = ParseWeitedVector(strVal);
                        obj = vector;
                    }
                    break;
                default:
                    throw new Exception("还没有识别这个对象，赶快告诉开发人员！gnsser@163.com");
                    break;
            }
            return obj;
        }

        private static WeightedVector ParseWeitedVector(string strVal)
        {
            if (strVal.Length < 3) { return WeightedVector.GetZeroVector(1); }

            List<double> vals = new List<double>();
            List<double> rmses = new List<double>();
            var array = Geo.Utils.StringUtil.Split(strVal, ';', true);
            foreach (var item in array)
            {
                var arr = Geo.Utils.StringUtil.Split(item, ':', true);
                var first = arr[0].Trim();
                var second = arr[1].Trim();

                var val = Double.Parse(first);
                var rms = Double.Parse(second);
                vals.Add(val);
                rmses.Add(rms);
            }
            WeightedVector vector = new WeightedVector(vals.ToArray(), rmses.ToArray());
            return vector;
        }

        private static Dictionary<TEnumType, bool> ParseBoolDic<TEnumType>(string strVal)
        {
            Dictionary<TEnumType, bool> dic = new Dictionary<TEnumType, bool>();
            var array = Geo.Utils.StringUtil.Split(strVal, ';', true);
            foreach (var item in array)
            {
                var arr = Geo.Utils.StringUtil.Split(item, ':', true);
                var keyStr = arr[0].Trim();
                var valStr = arr[1].Trim();

                var key = (TEnumType)Enum.Parse(typeof(TEnumType), keyStr);
                var val = bool.Parse(valStr);
                dic[key] = val;
            }

            return dic;
        }

        private static List<TEnumType> ParseEnumList<TEnumType>(string strVal)
        {
            List<TEnumType> list = new List<TEnumType>();
            var array = Geo.Utils.StringUtil.Split(strVal, ',', true);
            foreach (var item in array)
            {
                var ty = (TEnumType)Enum.Parse(typeof(TEnumType), item);
                list.Add(ty);
            }

            return list;
        }
        private static List<TType> ParseList<TType>(string strVal)
        {
            var paramType = GetTypeBySysType(typeof(TType));

            List<TType> list = new List<TType>();
            var array = Geo.Utils.StringUtil.Split(strVal, ',', true);
            foreach (var item in array)
            {
                var ty = (TType)ParseValue(item, paramType);
                list.Add(ty);
            }

            return list;
        } 

        static Dictionary<OptionName, OptionParamType> nameTypeDic;
        /// <summary>
        /// 名字与数据类型对应。
        /// </summary>
        /// <returns></returns>
         public  static Dictionary<OptionName, OptionParamType> GetNameTypeDic()
        {
            if (nameTypeDic == null) { nameTypeDic = BuildDictionary(); }
            return nameTypeDic;
        }

        /// <summary>
        /// 构建变量字典
        /// </summary>
        /// <returns></returns>
        public static Dictionary<OptionName, OptionParamType> BuildDictionary()
        {
            Dictionary<OptionName, OptionParamType> data = new Dictionary<OptionName, OptionParamType>();

            var collection = Enum.GetNames(typeof(OptionName));

            foreach (var str in collection)
            {
                OptionName name = (OptionName)Enum.Parse(typeof(OptionName), str);

                switch (name)
                {
                    case OptionName.Version:
                        data.Add(name, OptionParamType.Double);
                        break;
                    case OptionName.Author:
                        data.Add(name, OptionParamType.String);
                        break;
                    case OptionName.CreationTime:
                        data.Add(name, OptionParamType.DateTime);
                        break;
                    case OptionName.Name:
                        data.Add(name, OptionParamType.String);
                        break;
                    case OptionName.OrdinalAndReverseCount:
                        data.Add(name, OptionParamType.Int);
                        break;
                    case OptionName.IsReversedDataSource:
                        data.Add(name, OptionParamType.Bool);
                        break;
                    case OptionName.PositionType:
                        data.Add(name, OptionParamType.PositionType);
                        break;
                    case OptionName.ProcessType:
                        data.Add(name, OptionParamType.ProcessType); break;
                    case OptionName.AdjustmentType:
                        data.Add(name, OptionParamType.AdjustmentType); break;
                    case OptionName.RinexObsFileFormatType:
                        data.Add(name, OptionParamType.RinexObsFileFormatType); break;
                    case OptionName.OutputRinexVersion:
                        data.Add(name, OptionParamType.Double); break;
                    case OptionName.GnssSolverType:
                        data.Add(name, OptionParamType.GnssSolverType); break;
                    case OptionName.StartIndex:
                        data.Add(name, OptionParamType.Int); break;
                    case OptionName.CaculateCount:
                        data.Add(name, OptionParamType.Int); break;
                    case OptionName.IsApproxXyzRequired:
                        data.Add(name, OptionParamType.Bool); break;
                    case OptionName.InitApproxXyzRms:
                        data.Add(name, OptionParamType.XYZ); break;
                    case OptionName.InitApproxXyz:
                        data.Add(name, OptionParamType.XYZ); break;
                    case OptionName.IsIndicatingApproxXyz:
                        data.Add(name, OptionParamType.Bool); break;
                    case OptionName.IsIndicatingApproxXyzRms:
                        data.Add(name, OptionParamType.Bool); break;
                    case OptionName.IsUpdateStationInfo:
                        data.Add(name, OptionParamType.Bool); break;
                    case OptionName.IsLengthPhaseValue:
                        data.Add(name, OptionParamType.Bool); break;
                    case OptionName.IsObsDataRequired:
                        data.Add(name, OptionParamType.Bool); break;
                    case OptionName.IsIndicatingClockFile:
                        data.Add(name, OptionParamType.Bool); break;
                    case OptionName.IsP2C2Enabled:
                        data.Add(name, OptionParamType.Bool); break;
                    case OptionName.IonoGridFilePath:
                        data.Add(name, OptionParamType.String); break;
                    case OptionName.IsIndicatingGridIonoFile:
                        data.Add(name, OptionParamType.Bool); break;
                    case OptionName.EpochIonoParamFilePath:
                        data.Add(name, OptionParamType.String); break;
                    case OptionName.IsIndicatingEphemerisFile:
                        data.Add(name, OptionParamType.Bool); break;
                    case OptionName.IsIndicatingCoordFile:
                        data.Add(name, OptionParamType.Bool); break;
                    case OptionName.ClockFilePath:
                        data.Add(name, OptionParamType.String); break;
                    case OptionName.EphemerisFilePath:
                        data.Add(name, OptionParamType.String); break;
                    case OptionName.NavIonoModelPath:
                        data.Add(name, OptionParamType.String); break;
                    case OptionName.CoordFilePath:
                        data.Add(name, OptionParamType.String); break;
                    case OptionName.TropAugmentFilePath:
                        data.Add(name, OptionParamType.String); break;
                    case OptionName.StationInfoPath:
                        data.Add(name, OptionParamType.String); break;
                    case OptionName.IsPreciseClockFileRequired:
                        data.Add(name, OptionParamType.Bool); break;
                    case OptionName.IsEphemerisRequired:
                        data.Add(name, OptionParamType.Bool); break;
                    case OptionName.IsPreciseEphemerisFileRequired:
                        data.Add(name, OptionParamType.Bool); break;
                    case OptionName.IsAntennaFileRequired:
                        data.Add(name, OptionParamType.Bool); break;
                    case OptionName.IsSatStateFileRequired:
                        data.Add(name, OptionParamType.Bool); break;
                    case OptionName.IsSatInfoFileRequired:
                        data.Add(name, OptionParamType.Bool); break;
                    case OptionName.IsOceanLoadingFileRequired:
                        data.Add(name, OptionParamType.Bool); break;
                    case OptionName.IsDCBFileRequired:
                        data.Add(name, OptionParamType.Bool); break;
                    case OptionName.IsVMF1FileRequired:
                        data.Add(name, OptionParamType.Bool); break;
                    case OptionName.Isgpt2FileRequired:
                        data.Add(name, OptionParamType.Bool); break;
                    case OptionName.IsErpFileRequired:
                        data.Add(name, OptionParamType.Bool); break;
                    case OptionName.IsIgsIonoFileRequired:
                        data.Add(name, OptionParamType.Bool); break;
                    case OptionName.IsOpenReportWhenCompleted:
                        data.Add(name, OptionParamType.Bool); break;
                    case OptionName.IsIonoCorretionRequired:
                        data.Add(name, OptionParamType.Bool); break;
                    case OptionName.IsRangeValueRequired:
                        data.Add(name, OptionParamType.Bool); break;
                    case OptionName.IsPhaseValueRequired:
                        data.Add(name, OptionParamType.Bool); break;
                    case OptionName.IsUpdateEstimatePostition:
                        data.Add(name, OptionParamType.Bool); break;
                    case OptionName.IsRemoveOrDisableNotPassedSat:
                        data.Add(name, OptionParamType.Bool); break;
                    case OptionName.IsRemoveSmallPartSat:
                        data.Add(name, OptionParamType.Bool); break;
                    case OptionName.IsExcludeMalfunctioningSat:
                        data.Add(name, OptionParamType.Bool); break;
                    case OptionName.IsDisableEclipsedSat:
                        data.Add(name, OptionParamType.Bool); break;
                    case OptionName.IsEnableBufferCs:
                        data.Add(name, OptionParamType.Bool); break;
                    case OptionName.IsEnableRealTimeCs:
                        data.Add(name, OptionParamType.Bool); break;
                    case OptionName.MaxErrorTimesOfBufferCs:
                        data.Add(name, OptionParamType.Double); break;
                    case OptionName.DifferTimesOfBufferCs:
                        data.Add(name, OptionParamType.Int); break;
                    case OptionName.PolyFitOrderOfBufferCs:
                        data.Add(name, OptionParamType.Int); break;
                    case OptionName.IgnoreCsedOfBufferCs:
                        data.Add(name, OptionParamType.Bool); break;
                    case OptionName.MinWindowSizeOfCs:
                        data.Add(name, OptionParamType.Int); break;
                    case OptionName.IsUsingRecordedCycleSlipInfo:
                        data.Add(name, OptionParamType.Bool); break;
                    case OptionName.MaxDifferValueOfMwCs:
                        data.Add(name, OptionParamType.Double); break;
                    case OptionName.MaxRmsTimesOfLsPolyCs:
                        data.Add(name, OptionParamType.Double); break;
                    case OptionName.MaxValueDifferOfHigherDifferCs:
                        data.Add(name, OptionParamType.Double); break;
                    case OptionName.MaxBreakingEpochCount://历元秒
                        data.Add(name, OptionParamType.Double); break;
                    case OptionName.IsCycleSlipDetectionRequired:
                        data.Add(name, OptionParamType.Bool); break;
                    case OptionName.IsCycleSlipReparationRequired:
                        data.Add(name, OptionParamType.Bool); break;
                    case OptionName.IsOutputCycleSlipFile:
                        data.Add(name, OptionParamType.Bool); break;
                    case OptionName.IsAliningPhaseWithRange:
                        data.Add(name, OptionParamType.Bool); break;
                    case OptionName.IsDopplerShiftRequired:
                        data.Add(name, OptionParamType.Bool); break;
                    case OptionName.IsRequireSameSats:
                        data.Add(name, OptionParamType.Bool); break;
                    case OptionName.IsAllowMissingEpochSite:
                        data.Add(name, OptionParamType.Bool); break;
                    case OptionName.MinAllowedRange:
                        data.Add(name, OptionParamType.Double); break;
                    case OptionName.MaxAllowedRange:
                        data.Add(name, OptionParamType.Double); break;
                    case OptionName.OutputDirectory:
                        data.Add(name, OptionParamType.String); break;
                    case OptionName.IsOutputSinex:
                        data.Add(name, OptionParamType.Bool); break;
                    case OptionName.IsOutputSummery:
                        data.Add(name, OptionParamType.Bool); break;
                    case OptionName.OutputBufferCount:
                        data.Add(name, OptionParamType.Int); break;
                    case OptionName.BufferSize:
                        data.Add(name, OptionParamType.Int); break; 
                    case OptionName.MaxEpochSpan:
                        data.Add(name, OptionParamType.Double); break;
                    case OptionName.MinContinuouObsCount:
                        data.Add(name, OptionParamType.Int); break;
                    case OptionName.MinSatCount:
                        data.Add(name, OptionParamType.Int); break;
                    case OptionName.MinFrequenceCount:
                        data.Add(name, OptionParamType.Int); break;
                    case OptionName.RangeType:
                        data.Add(name, OptionParamType.RangeType); break;
                    case OptionName.ObsDataType:
                        data.Add(name, OptionParamType.SatObsDataType); break;
                    case OptionName.ApproxDataType:
                        data.Add(name, OptionParamType.SatApproxDataType); break;
                    case OptionName.SatelliteTypes:
                        data.Add(name, OptionParamType.List_SatelliteType); break;
                    case OptionName.CaculateType:
                        data.Add(name, OptionParamType.CaculateType); break;
                    case OptionName.RejectGrossError:
                        data.Add(name, OptionParamType.Bool); break;
                    case OptionName.EnableClockService:
                        data.Add(name, OptionParamType.Bool); break;
                    case OptionName.MaxStdDev:
                        data.Add(name, OptionParamType.Double); break;
                    case OptionName.IsPreciseOrbit:
                        data.Add(name, OptionParamType.Bool); break;
                    case OptionName.VertAngleCut:
                        data.Add(name, OptionParamType.Double); break;
                    case OptionName.FilterCourceError:
                        data.Add(name, OptionParamType.Bool); break;
                    case OptionName.MaxLoopCount:
                        data.Add(name, OptionParamType.Int); break;
                    case OptionName.EnableLoop:
                        data.Add(name, OptionParamType.Bool); break;
                    case OptionName.MaxMeanStdTimes:
                        data.Add(name, OptionParamType.Double); break;
                    case OptionName.IsReverseCycleSlipeRevise:
                        data.Add(name, OptionParamType.Bool); break;
                    case OptionName.PhaseCovaProportionToRange:
                        data.Add(name, OptionParamType.Double); break;
                    case OptionName.StdDevOfSysTimeRandomWalkModel:
                        data.Add(name, OptionParamType.Double); break;
                    case OptionName.StdDevOfRandomWalkModel:
                        data.Add(name, OptionParamType.Double); break;
                    case OptionName.StdDevOfPhaseModel:
                        data.Add(name, OptionParamType.Double); break;
                    case OptionName.StdDevOfCycledPhaseModel:
                        data.Add(name, OptionParamType.Double); break;
                    case OptionName.StdDevOfIonoRandomWalkModel:
                        data.Add(name, OptionParamType.Double); break;
                    case OptionName.StdDevOfStaticTransferModel:
                        data.Add(name, OptionParamType.Double); break;
                    case OptionName.StdDevOfTropoRandomWalkModel:
                        data.Add(name, OptionParamType.Double); break;
                    case OptionName.StdDevOfRevClockWhiteNoiseModel:
                        data.Add(name, OptionParamType.Double); break;
                    case OptionName.IsSetApproxXyzWithCoordService:
                        data.Add(name, OptionParamType.Bool); break;
                    case OptionName.MutliEpochSameSatCount:
                        data.Add(name, OptionParamType.Int); break;
                    case OptionName.MultiEpochCount:
                        data.Add(name, OptionParamType.Int); break;
                    case OptionName.IsOutputResult:
                        data.Add(name, OptionParamType.Bool); break;
                    case OptionName.IsOutputEpochSatInfo:
                        data.Add(name, OptionParamType.Bool); break;
                    case OptionName.IsOutputAdjust:
                        data.Add(name, OptionParamType.Bool); break;
                    case OptionName.IsOutputIono:
                        data.Add(name, OptionParamType.Bool); break;
                    case OptionName.IsOutputWetTrop:
                        data.Add(name, OptionParamType.Bool); break;
                    case OptionName.IsOutputEpochResult:
                        data.Add(name, OptionParamType.Bool); break;
                    case OptionName.MinSuccesiveEphemerisCount:
                        data.Add(name, OptionParamType.Int); break;
                    case OptionName.IsSwitchWhenEphemerisNull:
                        data.Add(name, OptionParamType.Bool); break;
                    case OptionName.MinDistanceOfLongBaseLine:
                        data.Add(name, OptionParamType.Double); break;
                    case OptionName.MaxDistanceOfShortBaseLine:
                        data.Add(name, OptionParamType.Double); break;
                    case OptionName.CycleSlipDetectSwitcher:
                        data.Add(name, OptionParamType.Dictionary_CycleSlipDetectorType_Bool); break;
                    case OptionName.IsResidualCheckEnabled:
                        data.Add(name, OptionParamType.Bool); break;
                    case OptionName.IsPromoteTransWhenResultValueBreak:
                        data.Add(name, OptionParamType.Bool); break;
                    case OptionName.IsObsCorrectionRequired:
                        data.Add(name, OptionParamType.Bool); break;
                    case OptionName.IsApproxModelCorrectionRequired:
                        data.Add(name, OptionParamType.Bool); break;
                    case OptionName.IsDcbCorrectionRequired:
                        data.Add(name, OptionParamType.Bool); break;
                    case OptionName.IsReceiverAntSiteBiasCorrectionRequired:
                        data.Add(name, OptionParamType.Bool); break;
                    case OptionName.IsOceanTideCorrectionRequired:
                        data.Add(name, OptionParamType.Bool); break;
                    case OptionName.IsSolidTideCorrectionRequired:
                        data.Add(name, OptionParamType.Bool); break;
                    case OptionName.IsPoleTideCorrectionRequired:
                        data.Add(name, OptionParamType.Bool); break;
                    case OptionName.IsSatClockBiasCorrectionRequired:
                        data.Add(name, OptionParamType.Bool); break;
                    case OptionName.IsTropCorrectionRequired:
                        data.Add(name, OptionParamType.Bool); break;
                    case OptionName.IsGravitationalDelayCorrectionRequired:
                        data.Add(name, OptionParamType.Bool); break;
                    case OptionName.IsSatAntPcoCorrectionRequired:
                        data.Add(name, OptionParamType.Bool); break;
                    case OptionName.IsRecAntPcoCorrectionRequired:
                        data.Add(name, OptionParamType.Bool); break;
                    case OptionName.IsRecAntPcvCorrectionRequired:
                        data.Add(name, OptionParamType.Bool); break;
                    case OptionName.IsPhaseWindUpCorrectionRequired:
                        data.Add(name, OptionParamType.Bool); break;
                    case OptionName.IsSiteCorrectionsRequired:
                        data.Add(name, OptionParamType.Bool); break;
                    case OptionName.IsRangeCorrectionsRequired:
                        data.Add(name, OptionParamType.Bool); break;
                    case OptionName.IsFrequencyCorrectionsRequired:
                        data.Add(name, OptionParamType.Bool); break;
                    case OptionName.IsEpochIonoFileRequired:
                        data.Add(name, OptionParamType.Bool); break;
                    case OptionName.IsNavIonoModelCorrectionRequired:
                        data.Add(name, OptionParamType.Bool); break;
                    case OptionName.IsTropAugmentEnabled:
                        data.Add(name, OptionParamType.Bool); break;
                    case OptionName.Isgpt2File1DegreeRequired:
                        data.Add(name, OptionParamType.Bool); break;
                    case OptionName.IsSameSatRequired:
                        data.Add(name, OptionParamType.Bool); break;
                    case OptionName.IsBaseSatelliteRequried:
                        data.Add(name, OptionParamType.Bool); break;
                    case OptionName.IsFixingAmbiguity:
                        data.Add(name, OptionParamType.Bool); break;
                    case OptionName.IsFixingCoord:
                        data.Add(name, OptionParamType.Bool); break;
                    case OptionName.IsSiteCoordServiceRequired:
                        data.Add(name, OptionParamType.Bool); break;
                    case OptionName.IsIndicatingStationInfoFile:
                        data.Add(name, OptionParamType.Bool); break;
                    case OptionName.IsStationInfoRequired:
                        data.Add(name, OptionParamType.Bool); break;
                    case OptionName.IsEnableInitApriori:
                        data.Add(name, OptionParamType.Bool); break;
                    case OptionName.InitApriori:
                        data.Add(name, OptionParamType.WeightedVector); break;
                    case OptionName.IsEnableNgaEphemerisSource:
                        data.Add(name, OptionParamType.Bool); break;
                    case OptionName.IsUniqueEphSource:
                        data.Add(name, OptionParamType.Bool); break;
                    case OptionName.IndicatedSourceCode:
                        data.Add(name, OptionParamType.String); break;
                    case OptionName.ObsFiles:
                        data.Add(name, OptionParamType.List_String); break;
                    case OptionName.IndicatedPrn:
                        data.Add(name, OptionParamType.SatelliteNumber); break;
                    case OptionName.IsIndicatedPrn:
                        data.Add(name, OptionParamType.Bool); break;
                    default:
                        data.Add(name, OptionParamType.String); break;
                }

            }
            return data;
        }

        internal static object GetDefaultValue(OptionName name)
        {
            var type = GetNameTypeDic()[name];


            Object obj = null;
            switch (type)
            {
                case OptionParamType.String:
                    obj = "";
                    break;
                case OptionParamType.List_String:
                    obj = "";
                    break;
                case OptionParamType.Double:
                    obj = 0;
                    break;
                case OptionParamType.Int:
                    obj = 0;
                    break;
                case OptionParamType.Bool:
                    obj = false;
                    break;
                case OptionParamType.SatelliteNumber:
                    obj = new SatelliteNumber(1, SatelliteType.G);
                    break;
                case OptionParamType.DateTime:
                    obj = Time.MinValue.DateTime;
                    break;
                case OptionParamType.PositionType:
                    obj = PositionType.静态定位;
                    break;
                case OptionParamType.ProcessType:
                    obj = ProcessType.仅计算;
                    break;
                case OptionParamType.RinexObsFileFormatType:
                    obj = RinexObsFileFormatType.单站单历元;
                    break;
                case OptionParamType.AdjustmentType:
                    obj = AdjustmentType.卡尔曼滤波;
                    break;
                case OptionParamType.GnssSolverType:
                    obj = GnssSolverType.无电离层组合PPP;
                    break;
                case OptionParamType.XYZ:
                    obj = XYZ.Zero;
                    break;
                case OptionParamType.RangeType:
                    obj = RangeType.IonoFreeRangeOfAB;
                    break;
                case OptionParamType.SatObsDataType:
                    obj = SatObsDataType.IonoFreePhaseRange;
                    break;
                case OptionParamType.SatApproxDataType:
                    obj = SatApproxDataType.ApproxPseudoRangeA;
                    break;
                case OptionParamType.List_SatelliteType:
                    obj = new List<SatelliteType>() { SatelliteType.G };
                    break;
                case OptionParamType.CaculateType:
                    obj = CaculateType.Filter;
                    break;
                case OptionParamType.Dictionary_CycleSlipDetectorType_Bool:
                    Dictionary<CycleSlipDetectorType, bool> dic = new Dictionary<CycleSlipDetectorType, bool>();
                    dic[CycleSlipDetectorType.LI组合] = true;
                    dic[CycleSlipDetectorType.MW组合] = true;
                    obj = dic;

                    break;
                case OptionParamType.WeightedVector:
                    obj = null;
                    break;
                default:
                    throw new Exception("还没有识别这个对象，赶快告诉开发人员！gnsser@163.com");
                    break;
            }
            return obj;
        }
    }



     
}
