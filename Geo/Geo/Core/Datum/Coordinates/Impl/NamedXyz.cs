//2016.02.02, czs, created, �������Ƶ�����
//2017.03.22, double, add in zhengzhou, NamedXyzAndTime


using System;
using System.Collections.Generic;
using System.Text;
using Geo.Utils;
using Geo.Common;
using System.IO;
using Geo;
using Geo.Algorithm.Adjust;
using Geo.Coordinates;
using Geo.IO;
using Geo.Referencing;
using Geo.Times;
using Geo.Utils; 

namespace Geo.Coordinates
{ 
    /// <summary>
    /// ����Ȩֵ�����Ƶ�����
    /// </summary>
    public class NamedRmsXyz : NamedValue<RmsedXYZ>
    { 
        /// <summary>
        /// ���캯��
        /// </summary>
        public NamedRmsXyz()  { }
        /// <summary>
        /// ���캯��
        /// </summary>
        /// <param name="name"></param>
        /// <param name="xyz"></param>
        public NamedRmsXyz(string name, RmsedXYZ xyz) : base(name, xyz) { }
        /// <summary>
        /// X
        /// </summary>
        public double X { get { return Value.Value.X; } }
        /// <summary>
        /// Y
        /// </summary>
        public double Y { get { return Value.Value.Y; } }
        /// <summary>
        /// Z
        /// </summary>
        public double Z { get { return Value.Value.Z; } }
        /// <summary>
        /// �Ƚ�
        /// </summary>
        /// <param name="listA"></param>
        /// <param name="listB"></param>
        /// <returns></returns>
        public static List<NamedRmsXyz> Compare(List<NamedRmsXyz> listA, List<NamedRmsXyz> listB)
        {
            List<NamedRmsXyz> list = new List<NamedRmsXyz>();
            foreach (var item in listA)
            {
                NamedRmsXyz site = listB.Find(m => string.Equals(m.Name.Trim(), item.Name.Trim(), StringComparison.CurrentCultureIgnoreCase));
                if (site != null)
                {
                    list.Add(item.Compare(site));
                }
            }
            return list;
             
        }
        /// <summary>
        /// �Ƚ�
        /// </summary>
        /// <param name="site"></param>
        /// <returns></returns>
        public NamedRmsXyz Compare(NamedRmsXyz site)
        {
            NamedRmsXyz namedXyz = new NamedRmsXyz()
            {
                Name = site.Name,
              //  Value = this.Value.Value - site.Value.Value
            };

            return namedXyz;
        }

        /// <summary>
        /// �������
        /// </summary>
        /// <param name="convertedXy"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        public static ObjectTableStorage BuildTable(List<NamedRmsXyz> convertedXy, string name)
        {
            ObjectTableStorage resultTable = new ObjectTableStorage(name);
            foreach (var item in convertedXy)
            {
                resultTable.NewRow();
                resultTable.AddItem(GeoParamNames.Name, item.Name);
                resultTable.AddItem(GeoParamNames.X, item.X);
                resultTable.AddItem(GeoParamNames.Y, item.Y);
                resultTable.AddItem(GeoParamNames.Z, item.Z);
                resultTable.AddItem(GeoParamNames.RmsX, item.Value.Rms.X);
                resultTable.AddItem(GeoParamNames.RmsY, item.Value.Rms.Y);
                resultTable.AddItem(GeoParamNames.RmsZ, item.Value.Rms.Z);
            }
            return resultTable;
        }
    }
    /// <summary>
    /// ����һ����������࣬������ʾNEU�����ƫ�
    /// </summary>
    public class NamedXyzEnu : NamedXyz
    {
        /// <summary>
        /// ������
        /// </summary>
        public double N { get; set; }
        /// <summary>
        /// ������
        /// </summary>
        public double E { get; set; }
        /// <summary>
        /// �Ϸ���
        /// </summary>
        public double U { get; set; }

        public override double Len { get { return Math.Sqrt(X * X + Y * Y + Z * Z); } }

        
        

        /// <summary>
        /// ����ת����ȡ
        /// </summary>
        /// <param name="name"></param>
        /// <param name="localXyz"></param>
        /// <param name="siteXyz"></param>
        /// <returns></returns>
        public static NamedXyzEnu Get(string name, XYZ localXyz, XYZ siteXyz)
        {
            var enu = Geo.Coordinates.CoordTransformer.LocaXyzToEnu(localXyz, siteXyz);
            return new NamedXyzEnu() { Name = name, Value = localXyz, E = enu.E, N = enu.N, U = enu.U };
        } 
    }
    /// <summary>
    /// ���캯��
    /// </summary>
    public class NamedXy : NamedValue<XY>
    {/// <summary>
     /// ���캯��
     /// </summary>
        public NamedXy() { }
        /// <summary>
        /// ���캯��
        /// </summary>
        /// <param name="name"></param>
        /// <param name="xy"></param>
        public NamedXy(string name, XY xy) : base(name, xy) { }
        /// <summary>
        /// X
        /// </summary>
        public double X { get { return Value.X; } }
        /// <summary>
        /// Y
        /// </summary>
        public double Y { get { return Value.Y; } } 
        /// <summary>
        /// ���ȣ�����
        /// </summary>
        public virtual double Len { get { return Math.Sqrt(X * X + Y * Y); } }
        /// <summary>
        /// �������
        /// </summary>
        /// <param name="convertedXy"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        public static ObjectTableStorage BuildTable(List<NamedXy> convertedXy, string name)
        {
            ObjectTableStorage resultTable = new ObjectTableStorage(name);
            foreach (var item in convertedXy)
            {
                resultTable.NewRow();
                resultTable.AddItem("Name", item.Name);
                resultTable.AddItem("X", item.X);
                resultTable.AddItem("Y", item.Y);
            }
            return resultTable;
        }
    }

    
    /// <summary>
    /// XYZ �����ļ���ȡ
    /// </summary>
    public class NamedXyz : NamedValue<XYZ>
    {
        /// <summary>
        /// ���캯��
        /// </summary>
        public NamedXyz()  { }
        /// <summary>
        /// ���캯��
        /// </summary>
        /// <param name="name"></param>
        /// <param name="xyz"></param>
        public NamedXyz(string name, XYZ xyz) : base(name, xyz) { }
        /// <summary>
        /// X
        /// </summary>
        public double X { get { return Value.X; } }
        /// <summary>
        /// Y
        /// </summary>
        public double Y { get { return Value.Y; } }
        /// <summary>
        /// Z
        /// </summary>
        public double Z { get { return Value.Z; } }
        /// <summary>
        /// ���ȣ�����
        /// </summary>
        public virtual double Len { get { return Math.Sqrt( X*X + Y*Y + Z*Z ); } }
        /// <summary>
        /// �Ƚ�
        /// </summary>
        /// <param name="listA"></param>
        /// <param name="listB"></param>
        /// <returns></returns>
        public static List<NamedXyz> Compare(List<NamedXyz> listA, List<NamedXyz> listB, bool enableCutNameLen=false, int NameLen=4)
        {
            List<NamedXyz> list = new List<NamedXyz>();
            foreach (var item in listA)
            {
                NamedXyz site = listB.Find(m => string.Equals(
                    GetCuttedName( m.Name.Trim(),enableCutNameLen, NameLen)
                    , GetCuttedName(item.Name.Trim(), enableCutNameLen, NameLen),
                    StringComparison.CurrentCultureIgnoreCase));
                if (site != null)
                {
                    list.Add(item.Compare(site));
                }
            }
            return list;             
        }

        public static string GetCuttedName(string name , bool enableCutNameLen = false, int NameLen = 4)
        {
            if (enableCutNameLen)
            {
                return Geo.Utils.StringUtil.SubString(name,0, NameLen);
            }
            return name;
        }

        /// <summary>
        /// �Ƚ�
        /// </summary>
        /// <param name="site"></param>
        /// <returns></returns>
        public NamedXyz Compare(NamedXyz site)
        {
            NamedXyz namedXyz = new NamedXyz()
            {
                Name = site.Name,
                Value = this.Value - site.Value
            };

            return namedXyz;
        }
        /// <summary>
        /// ����һ��
        /// </summary>
        /// <param name="line"></param>
        /// <param name="splitter"></param>
        /// <returns></returns>
        public static NamedXyz Parse(string line, char [] splitter = null)
        {
            if (splitter == null) { splitter = new char[] { ' ', ',', '\t' }; }
            var values = line.Split(splitter, StringSplitOptions.RemoveEmptyEntries);
            if (values.Length >= 4)
            {
                return Parse(values);
            }

            NamedXyz xyz = new NamedXyz()
            {
                Name = "",
                Value = new XYZ(
                    double.Parse(values[0]),
                    double.Parse(values[1]),
                    double.Parse(values[2]))

            };
            return xyz;
        }

        /// <summary>
        /// ��ȡ�������ļ�����һ��Ϊͷ������������Ϊ��tab�����ָ�����֣����У�ͷ����name��Ϊ���֣���һ����x��Ϊx������Ϊy��z��
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public static List<NamedXyz> ReadNamedXyz(string path)
        {
            List<NamedXyz> listA = new List<NamedXyz>();
            using (StreamReader reader = new StreamReader(new FileStream(path, FileMode.Open)))
            {
                int nameIndex = -1;
                int xIndex = -1;
                int yIndex = -1;
                int zIndex = -1;
                string line = reader.ReadLine();
                //��һ��Ϊͷ��
                string[] titles = StringUtil.SplitByTab(line);
                int i = 0;
                foreach (var item in titles)
                {
                    var title = item.ToLower();
                    if (title.Contains("name") && nameIndex == -1) { nameIndex = i; }
                    else if (title.Contains("x") && xIndex == -1) xIndex = i;
                    else if (title.Contains("y") && yIndex == -1) yIndex = i;
                    else if (title.Contains("z") && zIndex == -1) zIndex = i;
                    //����Ԫ�Ƚϣ�����ԪΪ����
                    else if(title.Contains("epoch") && nameIndex == -1) { nameIndex = i; }

                    i++;
                }

                while ((line = reader.ReadLine()) != null)
                {
                    if (String.IsNullOrWhiteSpace(line)) { continue; }

                    string[] values = StringUtil.SplitByTab(line);
              
                    NamedXyz xyz = Parse(values, nameIndex, xIndex, yIndex, zIndex); 
                    listA.Add(xyz);
                }
                return listA;
            }
        }
        /// <summary>
        /// ����
        /// </summary>
        /// <param name="values"></param>
        /// <param name="nameIndex"></param>
        /// <param name="xIndex"></param>
        /// <param name="yIndex"></param>
        /// <param name="zIndex"></param>
        /// <returns></returns>
        public static NamedXyz Parse(string[] values, int nameIndex=0, int xIndex=1, int yIndex=2, int zIndex=3)
        {
            var z = 0.0;
            if(zIndex != -1)
            {
                z = double.Parse(values[zIndex]);
            }
            NamedXyz xyz = new NamedXyz()
            {
                Name = values[nameIndex],
                Value = new XYZ(
                    double.Parse(values[xIndex]),
                    double.Parse(values[yIndex]),
                    z)

            };
            return xyz;
        }
        /// <summary>
        /// ��ȡ�������ļ�����һ��Ϊͷ������������Ϊ��tab�����ָ�����֣����У�ͷ����name��Ϊ���֣���һ����x��Ϊx������Ϊy��z��
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public static List<NamedXyz> ReadNamedXyztxt(string path)
        {
            List<NamedXyz> listA = new List<NamedXyz>();
            using (StreamReader reader = new StreamReader(new FileStream(path, FileMode.Open)))
            {
                int nameIndex = -1;
                int xIndex = -1;
                int yIndex = -1;
                int zIndex = -1;
                string line = reader.ReadLine();
                //��һ��Ϊͷ��
                string[] titles = StringUtil.SplitByBlank(line);
                int i = 0;
                foreach (var item in titles)
                {
                    var title = item.ToLower();
                    if (title.Contains("name") && nameIndex == -1) nameIndex = i;
                    else if (title.Contains("x") && xIndex == -1) xIndex = i;
                    else if (title.Contains("y") && yIndex == -1) yIndex = i;
                    else if (title.Contains("z") && zIndex == -1) zIndex = i;

                    i++;
                }

                while ((line = reader.ReadLine()) != null)
                {
                    if (String.IsNullOrWhiteSpace(line)) { continue; }

                    string[] values = StringUtil.SplitByBlank(line);
                    NamedXyz xyz = new NamedXyz()
                    {
                        Name = values[nameIndex],
                        Value = new XYZ(
                            double.Parse(values[xIndex]),
                            double.Parse(values[yIndex]),
                            double.Parse(values[zIndex]))

                    };
                    listA.Add(xyz);
                }
                reader.Close();
            }
            return listA;
        }
        public static List<NamedXyz> CompareWithListB(List<NamedXyz> listA, List<NamedXyz> listB)
        {
            List<NamedXyz> list = new List<NamedXyz>();
            foreach (var item in listA)
            {
                NamedXyz site = listB.Find(m => string.Equals(m.Name.Trim(), item.Name.Trim(), StringComparison.CurrentCultureIgnoreCase));
                if (site != null)
                {
                    list.Add((site));
                }
            }
            return list;
        }
         
        /// <summary>
        /// �������
        /// </summary>
        /// <param name="convertedXy"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        public static ObjectTableStorage BuildTable(List<NamedXyz> convertedXy, string name)
        {
            ObjectTableStorage resultTable = new ObjectTableStorage(name);
            foreach (var item in convertedXy)
            {
                resultTable.NewRow();
                resultTable.AddItem("Name", item.Name);
                resultTable.AddItem("X", item.X);
                resultTable.AddItem("Y", item.Y);
                resultTable.AddItem("Z", item.Z);
            }
            return resultTable;
        }
        /// <summary>
        /// ���
        /// </summary>
        /// <param name="result"></param>
        /// <param name="allToBeConvert"></param>
        /// <returns></returns>
        public static List<NamedXyz> Differ(List<NamedXyz> result, List<NamedXyz> allToBeConvert)
        {
            List<NamedXyz> residuals = new List<NamedXyz>();
            foreach (var item in result)
            {
                var before = allToBeConvert.Find(m => m.Name == item.Name);
                if(before == null) { continue; }
                var Differ = item.Value - before.Value;
                residuals.Add(new NamedXyz(item.Name, Differ));
            }
            return residuals;
        }
    }
    #region �����ʱ�����бȽ�
    public class NamedXyzAndTime : NamedXyz
    {
        public string dayOfWeek { get; set; }
        private static string GetDayOfWeekFromObservationFileName(string filename)
        {
            string year = "20" + filename.Substring(9, 2);
            string day = filename.Substring(4, 3);
            string yearAndDay = year + day;
            Time time = Time.ParseYearDayString(yearAndDay);
            string dayOfWeek = time.GetGpsWeekAndDay().ToString();
            return dayOfWeek;
        }
        public static List<NamedXyzAndTime> ReadNamedXyz(string path)
        {
            List<NamedXyzAndTime> listA = new List<NamedXyzAndTime>();
            using (StreamReader reader = new StreamReader(new FileStream(path, FileMode.Open)))
            {
                int nameIndex = -1;
                int xIndex = -1;
                int yIndex = -1;
                int zIndex = -1;
                int fileIndex = -1;
                string line = reader.ReadLine();
                //��һ��Ϊͷ��
                string[] titles = StringUtil.SplitByTab(line);
                int i = 0;
                foreach (var item in titles)
                {
                    var title = item.ToLower();
                    if (title.Contains("name") && nameIndex == -1) nameIndex = i;
                    else if (title.Contains("x") && xIndex == -1) xIndex = i;
                    else if (title.Contains("y") && yIndex == -1) yIndex = i;
                    else if (title.Contains("z") && zIndex == -1) zIndex = i;
                    else if (title.Contains("epoch") && fileIndex == -1) fileIndex = i;
                    i++;
                }

                if(nameIndex == -1) { nameIndex = fileIndex; }

                while ((line = reader.ReadLine()) != null)
                {
                    if (String.IsNullOrWhiteSpace(line)) { continue; }

                    string[] values = StringUtil.SplitByTab(line);
                    NamedXyzAndTime xyz = new NamedXyzAndTime()
                    {
                        Name = values[nameIndex],
                        Value = new XYZ(
                            double.Parse(values[xIndex]),
                            double.Parse(values[yIndex]),
                            double.Parse(values[zIndex])),
                        dayOfWeek = GetDayOfWeekFromObservationFileName(values[fileIndex])

                    };
                    listA.Add(xyz);
                }
                return listA;
            }
        }
    }
    /// <summary>
    /// ����һ����������࣬������ʾNEU�����ƫ�
    /// </summary>
    public class NamedXyzEnuAndTime : NamedXyzEnu
    {
        /// <summary>
        /// ������
        /// </summary>
        public double N { get; set; }
        /// <summary>
        /// ������
        /// </summary>
        public double E { get; set; }
        /// <summary>
        /// �Ϸ���
        /// </summary>
        public double U { get; set; }

        public double Len { get { return Math.Sqrt(X * X + Y * Y + Z * Z); } }
        public string DayOfWeek { get; set; }
        /// <summary>
        /// ����ת����ȡ
        /// </summary>
        /// <param name="name"></param>
        /// <param name="localXyz"></param>
        /// <param name="siteXyz"></param>
        /// <returns></returns>
        public static NamedXyzEnuAndTime Get(string name, string dayofWeek,XYZ localXyz, XYZ siteXyz)
        {
            var enu = Geo.Coordinates.CoordTransformer.LocaXyzToEnu(localXyz, siteXyz);
            return new NamedXyzEnuAndTime() { Name = name, DayOfWeek = dayofWeek,Value = localXyz, E = enu.E, N = enu.N, U = enu.U };
        }
    }
    #endregion
}