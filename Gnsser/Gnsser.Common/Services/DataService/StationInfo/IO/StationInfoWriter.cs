//2016.12.27 czs & cuiyang, created,  ��վ�ļ�д����

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


namespace Gnsser.Data
{
    /// <summary>
    /// �����ļ�д����
    /// </summary>
    public class StationInfoWriter : LineFileWriter<StationInfo>
    { /// <summary>
        /// ���캯��
        /// </summary>
        /// <param name="gofFilePath"></param>
        /// <param name="metaFilePath"></param>
        public StationInfoWriter(string gofFilePath, string metaFilePath = null)
            : base(gofFilePath, metaFilePath, Encoding.ASCII)
        {
            //Ĭ��·���� ProjectDirectory/Script/GofFileName.gof
            this.BaseDirectory = (Path.GetDirectoryName(gofFilePath));
        }
        /// <summary>
        /// ���캯��
        /// </summary>
        /// <param name="gofFilePath"></param>
        /// <param name="Gmetadata"></param>
        public StationInfoWriter(string gofFilePath, Gmetadata Gmetadata)
            : base(gofFilePath, Gmetadata, Encoding.ASCII)
        {
            //Ĭ��·���� ProjectDirectory/Script/GofFileName.gof
            this.BaseDirectory = (Path.GetDirectoryName(gofFilePath));
        }
        /// <summary>
        /// ���캯��������������ʼ��
        /// </summary>
        /// <param name="stream"></param>
        /// <param name="Gmetadata"></param>
        public StationInfoWriter(Stream stream, Gmetadata Gmetadata)
            : base(stream, Gmetadata, Encoding.ASCII)
        {
        }

        public override void FinalInit()
        {
            base.FinalInit();
            this.IsItemWidthFixed = true;
            ItemLengthes = new List<int>()
            {
                 7,18,19,19,9,8,8,8,20,20,20,20
            };
        }

        /// <summary>
        /// ����תΪ�ַ���
        /// </summary>
        /// <param name="val"></param>
        /// <returns></returns>
        public override string PropertyToString(object val)
        {
            if (val == null) { return ""; }

            if (val.GetType() == typeof(Time))
            {
                var time = (Time)val;
                return time.ToYearDayHourMinuteSecondString();
            } 
            return base.PropertyToString(val);
        }

    } 
}