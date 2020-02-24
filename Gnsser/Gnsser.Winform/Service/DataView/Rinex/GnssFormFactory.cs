using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Windows.Forms;
using AnyInfo;

namespace Gnsser.Winform
{
    public static class GnssFormFactory
    {
        /// <summary>
        /// open one OpenFileDialog to select rinex O files, and create one anyinfo point layer;
        /// </summary>
        /// <param name="t"></param>
        /// <returns></returns>
        public static Layer OpenAndShowOFileOnMap(string title)
        {
            Layer layer = null;
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = Setting.RinexOFileFilter;
            openFileDialog.Multiselect = true;

            if (openFileDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                string[] files = openFileDialog.FileNames;
                List<AnyInfo.Geometries.Point> lonlats = new List<AnyInfo.Geometries.Point>();


                Geo.IO.InputFileManager inputFileManager = new Geo.IO.InputFileManager(Setting.TempDirectory);

                foreach (var item in files)
                {
                    var local = inputFileManager.GetLocalFilePath(item, "*.*O;*.rnx","*.*");
                    Data.Rinex.RinexObsFileHeader header = new Data.Rinex.RinexObsFileReader(local).GetHeader();
                    if (header.ApproxXyz == null) continue;

                    Geo.Coordinates.GeoCoord lonlat = Geo.Coordinates.CoordTransformer.XyzToGeoCoord(header.ApproxXyz);
                    lonlats.Add(new AnyInfo.Geometries.Point(lonlat, Path.GetFileNameWithoutExtension(item).Substring(0, 4)) { Name = header.MarkerName });
                    header = null;//释放资源。
                }
                layer = LayerFactory.CreatePointLayer(lonlats, title);
            }
            return layer;
        }

    }
}
