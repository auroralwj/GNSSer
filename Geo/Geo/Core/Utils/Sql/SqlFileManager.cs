//2012.01.02, czs, create, SQL Server 数据库大文件上传、下载操作支持类。
using System;
using System.Collections.Generic;
using System.Text;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Xml;
using System.Diagnostics;
using System.IO;

namespace Geo.Utils
{
    /// <summary>
    /// 用于通知进度条文件上传或下载的进度。
    /// </summary>
    /// <param name="progressValue"></param>
    public delegate void DbFileProcessChangedEventHandler(long progressValue);

    /// <summary>
    /// SQL Server 数据库大文件上传、下载操作支持类。
    /// 初始化此类时必须指明详细到数据库/数据表/数据列/单元格，其中数据表的主键为Int类型， 事务支持由SqlCommand调入。
    /// 本类文件只有上传，下载，而没有编辑和删除功能。
    /// 如果出错，则蹦出异常，由调用程序处理。
    /// Author:czs
    /// Version:1.0
    /// Time:2012.01.02
    /// </summary>
    public class SqlFileManager
    {
        private int bufferSize = 1024 * 1024;
        /// <summary>
        /// 缓存大小
        /// </summary>
        public int BufferSize
        {
            get { return bufferSize; }
            set { bufferSize = value; }
        } 
        /// <summary>
        /// 如果要启用事务支持，必须传入此数据。
        /// </summary>
        public SqlCommand SqlCommand { get; set; }

        /// <summary>
        /// 文件表名称
        /// </summary>
        public string TableName { get; set; }
        /// <summary>
        /// 文件列名称
        /// </summary>
        public string ColumeName { get; set; }
        /// <summary>
        /// 主键名称
        /// </summary>
        public string IdColumeName { get; set; }

        private long uploadFileLength;
        /// <summary>
        /// 上传文件的大小
        /// </summary>
        public long UploadFileLength
        {
            get { return uploadFileLength; }
        }

        public event DbFileProcessChangedEventHandler  filePercessChanged; 

        public SqlFileManager() { }
        public SqlFileManager(String tableName, string columeName, string idColumeName)
        {
            this.TableName = tableName;
            this.ColumeName = columeName;
            this.IdColumeName = idColumeName;
        }
        public SqlFileManager(String tableName, string columeName, string idColumeName, SqlCommand sqlCommand)
        {
            this.TableName = tableName;
            this.ColumeName = columeName;
            this.IdColumeName = idColumeName;
            this.SqlCommand = sqlCommand;
        }

        #region 下载
        /// <summary>
        /// 从数据库表的BLOB列读取二进制数据块，并写入文件中
        /// </summary>
        /// <param name="reader">已打开的SqlDataReader</param>
        /// <param name="column">BLOB列的序列</param>
        /// <param name="filePath">文件路径</param>
        public void DownloadFile( string filePath, int fileId)
        {
            FileStream outStream = new FileStream(filePath, FileMode.OpenOrCreate, FileAccess.Write);
            DownloadFile(outStream, fileId );
        }
        /// <summary>
        /// 从数据库表的BLOB列读取二进制数据块，并写入输出流
        /// </summary>
        /// <param name="outStream">输出流</param>
        /// <param name="fileId"></param>
        public void DownloadFile(Stream outStream, int fileId)
        {
            string sql = "SELECT " + ColumeName + " FROM " + TableName + " WHERE " + IdColumeName + " = " + fileId;
            SqlCommand.CommandText = sql;

            SqlDataReader reader = SqlCommand.ExecuteReader(CommandBehavior.SequentialAccess);

            if (reader.Read())
            {
                BinaryWriter writer = new BinaryWriter(outStream);
                int column = 0;
                long startIndex = 0;
                byte[] buffer = new byte[this.BufferSize];

                long retSize = reader.GetBytes(column, startIndex, buffer, 0, this.BufferSize);

                while (retSize == this.BufferSize)
                {
                    writer.Write(buffer);
                    writer.Flush();

                    startIndex += retSize;
                    retSize = reader.GetBytes(column, startIndex, buffer, 0, this.BufferSize);
                    //触发事件，通知委托
                    if (filePercessChanged != null)  filePercessChanged(startIndex); 
                }
                //触发事件，通知委托
                if (filePercessChanged != null) filePercessChanged(startIndex + retSize); 

                writer.Write(buffer, 0, (int)retSize);
                writer.Flush();
               
                writer.Close();
                outStream.Close();
                reader.Close();
            }
        }

        #endregion

        #region 上传
        #region 支持SQL 2005的增量上传
        /// <summary>
        /// Up load file 采用WRITE方法增量更新。
        /// 需要SQL Server 2005及其以上版本支持。
        /// 对应文件列类型应为 varbinary(MAX)
        /// </summary>
        /// <param name="filePath"></param>
        /// <param name="fileId"></param>
        /// <param name="progressBar"></param>
        /// <returns></returns>
        public void UploadFileAsVarBinarryMax( string filePath, int fileId)
        {
            //newst function but not suplly in SQL 2000
            FileStream stream = new FileStream(filePath, FileMode.Open, FileAccess.Read);
            UploadFileAsVarBinarryMax( stream, fileId);
        }

        /// <summary>
        ///Up load file 采用WRITE方法增量更新。
        /// 需要SQL Server 2005及其以上版本支持
        ///  对应文件列类型应为 varbinary(MAX)
        /// </summary>
        /// <param name="fileId"></param>
        /// <param name="inputStream">输入（数据库）流</param>
        /// <param name="msg"></param>
        /// <returns></returns>
        public void  UploadFileAsVarBinarryMax( Stream inputStream,int fileId)
        {
            long dataLen = inputStream.Length;//文件流总长度
            this.uploadFileLength = dataLen;

            BinaryReader reader = new BinaryReader(inputStream);

            long offset = 0;

            bool isFirst = true;
            while (offset < dataLen)
            {
                byte[] packet = reader.ReadBytes(BufferSize);//读取缓存
                //增量加入数据库
                AppendFile(packet, fileId, isFirst);

                isFirst = false;
                offset += packet.Length;
                packet = null;//清空内存。
                //触发事件，通知委托
                if (filePercessChanged != null)  filePercessChanged(offset); 
            }

            //触发事件，通知委托
            if (filePercessChanged != null) filePercessChanged(dataLen); 

            reader.Close();
            inputStream.Close();
        }

        /// <summary>
        /// 增量更新指定文件列。
        ///  对应文件列类型应为 varbinary(MAX)
        /// Sql server 2005
        /// </summary>
        /// <param name="fileBytes"></param>
        /// <param name="fileId"></param>
        /// <returns></returns>
        private void AppendFile(byte[] fileBytes, int fileId, bool isFirst)
        {
            if (fileBytes == null) fileBytes = new byte[0];

            //column_name { .WRITE ( expression , @Offset , @Length )
            //wirite(@file ,@Offset , @Length)  (0, null)全部替换之,(null,0)为尾加。
            string func;//= ".WRITE(@file, null, 0)";
            if (isFirst) func = ".WRITE(@file" + index + ", 0, null)"; //第一次 (0, null)全部替换之。
            else func = ".WRITE(@file" + index + ", null, 0)";//非第一次，(null,0)为尾加。

            String sql = "UPDATE " + TableName + " SET " + ColumeName + func + " WHERE " + IdColumeName + " = " + fileId;

            SqlCommand.CommandText = sql;

            SqlCommand.Parameters.Add("@file" + index + "", SqlDbType.Image, fileBytes.Length).Value = fileBytes;

            SqlCommand.ExecuteNonQuery();
            index++;
            if (index == long.MaxValue) index = 0;
        }
        //由于变量在同一批次中只能用一次，所以要加编号予以区别。
        static long index = 0;
        #endregion

        #region 支持SQL 2000的增量上传
        /// <summary>
        /// 支持SQL 2000的文件上传
        /// 到 SQL 2008也支持。但以后可能会废弃，见微软官方文档。
        /// 对应文件列类型应为 Image<br />
        /// http://msdn.microsoft.com/zh-cn/library/ms189466.aspx
        /// 后续版本的 Microsoft SQL Server 将删除该功能。
        /// 请避免在新的开发工作中使用该功能，并着手修改当前还在使用该功能的应用程序。
        /// 请改用大值数据类型和 UPDATE 语句的 .WRITE 子句
        /// </summary>
        /// <param name="filePath"></param>
        /// <param name="fileId"></param>
        /// <param name="progressBar"></param>
        /// <returns></returns>
        public void UploadFileAsImage(string filePath, int fileId)
        {
            FileStream inputStream = new FileStream(filePath, FileMode.Open, FileAccess.Read);
            UploadFileAsImage( inputStream, fileId);
        }
        /// <summary>
        ///  支持SQL 2000的文件上传
        ///   如果出错，则蹦出异常，由调用程序处理
        ///   对应文件列类型应为 Image<br />
        /// http://msdn.microsoft.com/zh-cn/library/ms189466.aspx
        /// 后续版本的 Microsoft SQL Server 将删除该功能。
        /// 请避免在新的开发工作中使用该功能，并着手修改当前还在使用该功能的应用程序。
        /// 请改用大值数据类型和 UPDATE 语句的 .WRITE 子句
        /// </summary>
        /// <param name="inputStream"></param>
        /// <param name="fileId"></param>
        /// <param name="msg"></param>
        /// <returns></returns>
        public void UploadFileAsImage(FileStream inputStream, int fileId)
        {
            //在数据表中获取存取文件的指针，该指针指向待插入数据表中的单元格。需要预先存入一个指示数据，如0x0，然后覆盖之。
            string sql = "SELECT @Pointer0 = TEXTPTR(" + ColumeName + ") FROM " + TableName + " WHERE " + IdColumeName + " = @index0";
            this.SqlCommand.CommandText = sql;
            SqlParameter ptrParm = SqlCommand.Parameters.Add("@Pointer0", SqlDbType.Binary, 16);
            ptrParm.Direction = ParameterDirection.Output;

            SqlCommand.Parameters.Add("@index0", SqlDbType.Int, 10).Value = fileId;

            SqlCommand.ExecuteNonQuery();

            byte[] pt = (byte[])ptrParm.Value;

            UploadFileAsImage(inputStream, pt);
        }

        /// <summary>
        /// MSDN 例子,支持 SQL 2000。
        /// 如果出错，则蹦出异常，由调用程序处理。
        /// 对应文件列类型应为 Image<br />
        /// http://msdn.microsoft.com/zh-cn/library/ms189466.aspx
        /// 后续版本的 Microsoft SQL Server 将删除该功能。
        /// 请避免在新的开发工作中使用该功能，并着手修改当前还在使用该功能的应用程序。
        /// 请改用大值数据类型和 UPDATE 语句的 .WRITE 子句
        /// </summary>
        /// <param name="inputStream">输入数据库的流</param>
        /// <param name="pointer">TextPtr，数据库BLOB列的指针</param>
        /// <param name="connection"></param>
        private void UploadFileAsImage(FileStream inputStream, byte[] pointer)
        {
            this.uploadFileLength = inputStream.Length;//文件流总长度

            SqlCommand.CommandText = "UPDATETEXT " + TableName + "." + ColumeName + " @Pointer1 @Offset1 0 @Bytes1";
            SqlParameter ptrParm = SqlCommand.Parameters.Add("@Pointer1", SqlDbType.Binary, 16);
            ptrParm.Value = pointer;
            SqlParameter fileParm = SqlCommand.Parameters.Add("@Bytes1", SqlDbType.Image, this.BufferSize);
            SqlParameter offsetParm = SqlCommand.Parameters.Add("@Offset1", SqlDbType.Int);
            offsetParm.Value = 0;

            BinaryReader br = new BinaryReader(inputStream);

            byte[] buffer = br.ReadBytes(this.BufferSize);
            long offset_ctr = 0;

            while (buffer.Length > 0)
            {
                fileParm.Value = buffer;
                SqlCommand.ExecuteNonQuery();

                offset_ctr += this.BufferSize;
                offsetParm.Value = offset_ctr;
                buffer = br.ReadBytes(this.BufferSize);

                //触发事件，通知委托
                if (filePercessChanged != null) filePercessChanged(offset_ctr); 
            }

            //触发事件，通知委托
            if (filePercessChanged != null) filePercessChanged(uploadFileLength); 

            br.Close();
            inputStream.Close();
        }
        #endregion
        #endregion

    }
}
