//using System;
// using System.Collections.Generic;
// using System.IO;
// using System.IO.Packaging;

//namespace Geo.Utils
//{
//    /// <summary>
//    /// 系统自带ZIP压缩工具
//    /// </summary>
//    public class ZipUtil
//    {
//        private const long BUFFER_SIZE = 4096;

//        /// <summary>
//        /// 压缩文件
//        /// </summary>
//        /// <param name="fileNames"></param>
//        /// <param name="zipFileName"></param>
//        public static void CompressFiles(List<string> fileNames, string zipFileName)
//        {
//            foreach (string file in fileNames)
//            {
//                CompressFile(zipFileName, file);
//            }
//        }

//        /// <summary>
//        /// 追加 文件
//        /// </summary>
//        /// <param name="zipFilename"></param>
//        /// <param name="fileToAdd"></param>
//        public static void CompressFile(string zipFilename, string fileToAdd)
//        {
//            using (Package zip = System.IO.Packaging.Package.Open(zipFilename, FileMode.OpenOrCreate))
//            {
//                string destFilename = ".\\" + Path.GetFileName(fileToAdd);
//                Uri uri = PackUriHelper.CreatePartUri(new Uri(destFilename, UriKind.Relative));
//                if (zip.PartExists(uri))
//                {
//                    zip.DeletePart(uri);
//                }
//                PackagePart part = zip.CreatePart(uri, "", CompressionOption.Normal);
//                using (FileStream fileStream = new FileStream(fileToAdd, FileMode.Open, FileAccess.Read))
//                {
//                    using (Stream dest = part.GetStream())
//                    {
//                        CopyStream(fileStream, dest);
//                    }
//                }
//            }
//        }

//        /// <summary>
//        /// 压缩文件
//        /// </summary>
//        /// <param name="zipFilename"></param>
//        /// <param name="outPath"></param>
//        public static void DecompressFile(string zipFilename, string outPath)
//        {
//            using (Package zip = System.IO.Packaging.Package.Open(zipFilename, FileMode.Open))
//            {
//                foreach (PackagePart part in zip.GetParts())
//                {
//                    string outFileName = Path.Combine(outPath, part.Uri.OriginalString.Substring(1));
//                    using (System.IO.FileStream outFileStream = new System.IO.FileStream(outFileName, FileMode.Create))
//                    {
//                        using (Stream inFileStream = part.GetStream())
//                        {
//                            CopyStream(inFileStream, outFileStream);
//                        }
//                    }
//                }
//            }
//        }

//        /// <summary>
//        /// 复制
//        /// </summary>
//        /// <param name="inputStream"></param>
//        /// <param name="outputStream"></param>
//        private static void CopyStream(System.IO.Stream inputStream, System.IO.Stream outputStream)
//        {
//            long bufferSize = inputStream.Length < BUFFER_SIZE ? inputStream.Length : BUFFER_SIZE;
//            byte[] buffer = new byte[bufferSize];
//            int bytesRead = 0;
//            long bytesWritten = 0;
//            while ((bytesRead = inputStream.Read(buffer, 0, buffer.Length)) != 0)
//            {
//                outputStream.Write(buffer, 0, bytesRead);
//                bytesWritten += bufferSize;
//            }
//        }
//    }
//}