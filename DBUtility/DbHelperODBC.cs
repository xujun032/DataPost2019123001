using System;
using System.Collections;
using System.Collections.Specialized;
using System.Data;
using System.Data.OleDb;
using System.Configuration;
using System.Data.Odbc;
using System.IO;

namespace DBUtility
{
    /// <summary>
    /// 数据访问基础类(基于ODBC) 
    /// 可以用户可以修改满足自己项目的需要。
    /// Copyright (C) Maticsoft
    /// </summary>
    public abstract class DbHelperODBC
    {
     
        //数据库连接字符串(web.config来配置)，可以动态更改connectionString支持多数据库.		
        public static string connectionString = PubConstant.BOSConnectionString;
        public DbHelperODBC()
        {

        }


        #region 公用方法
       
        public static int GetMaxID(string FieldName, string TableName)
        {
            string strsql = "select max(" + FieldName + ")+1 from " + TableName;
            object obj = GetSingle(strsql);
            if (obj == null)
            {
                return 1;
            }
            else
            {
                return int.Parse(obj.ToString());
            }
        }
        public static bool Exists(string strSql)
        {
            object obj = GetSingle(strSql);
            int cmdresult;
            if ((Object.Equals(obj, null)) || (Object.Equals(obj, System.DBNull.Value)))
            {
                cmdresult = 0;
            }
            else
            {
                cmdresult = int.Parse(obj.ToString());
            }
            if (cmdresult == 0)
            {
                return false;
            }
            else
            {
                return true;
            }
        }
        public static bool Exists(string strSql, params OdbcParameter[] cmdParms)
        {
   
            object obj = GetSingle(strSql, cmdParms);
            int cmdresult;
            if ((Object.Equals(obj, null)) || (Object.Equals(obj, System.DBNull.Value)))
            {
                cmdresult = 0;
            }
            else
            {
                cmdresult = int.Parse(obj.ToString());
            }
            if (cmdresult == 0)
            {
                return false;
            }
            else
            {
                return true;
            }
        }
        
        #endregion

        #region  执行简单SQL语句

        /// <summary>
        /// 执行SQL语句，返回影响的记录数
        /// </summary>
        /// <param name="SQLString">SQL语句</param>
        /// <returns>影响的记录数</returns>
        public static int ExecuteSql(string SQLString)
        {
            using (OdbcConnection connection = new OdbcConnection(connectionString))
            {
                using (OdbcCommand cmd = new OdbcCommand(SQLString, connection))
                {
                    try
                    {
                        connection.Open();
                        int rows = cmd.ExecuteNonQuery();
                        connection.Close();
                        return rows;
                    }
                    catch (System.Data.Odbc.OdbcException E)
                    {
                        connection.Close();
                        throw new Exception(E.Message);
                    }
                }
            }
        }

        /// <summary>
        /// 执行多条SQL语句，实现数据库事务。
        /// </summary>
        /// <param name="SQLStringList">多条SQL语句</param>		
        public static void ExecuteSqlTran(ArrayList SQLStringList)
        {
            using (OdbcConnection conn = new OdbcConnection(connectionString))
            {
                conn.Open();
                OdbcCommand cmd = new OdbcCommand();
                cmd.Connection = conn;
                OdbcTransaction tx = conn.BeginTransaction();
                cmd.Transaction = tx;
                try
                {
                    for (int n = 0; n < SQLStringList.Count; n++)
                    {
                        string strsql = SQLStringList[n].ToString();
                        if (strsql.Trim().Length > 1)
                        {
                            cmd.CommandText = strsql;
                            cmd.ExecuteNonQuery();
                            conn.Close();
                        }
                    }
                    tx.Commit();
                }
                catch (System.Data.Odbc.OdbcException E)
                {
                    tx.Rollback();
                    throw new Exception(E.Message);
                }
            }
        }
        /// <summary>
        /// 执行带一个存储过程参数的的SQL语句。
        /// </summary>
        /// <param name="SQLString">SQL语句</param>
        /// <param name="content">参数内容,比如一个字段是格式复杂的文章，有特殊符号，可以通过这个方式添加</param>
        /// <returns>影响的记录数</returns>
        public static int ExecuteSql(string SQLString, string content)
        {
            using (OdbcConnection connection = new OdbcConnection(connectionString))
            {
                OdbcCommand cmd = new OdbcCommand(SQLString, connection);
                System.Data.Odbc.OdbcParameter myParameter = new System.Data.Odbc.OdbcParameter("@content", OdbcType.VarChar);
                
                myParameter.Value = content;
                cmd.Parameters.Add(myParameter);
                try
                {
                    connection.Open();
                    int rows = cmd.ExecuteNonQuery();
                    connection.Close();
                    return rows;
                }
                catch (System.Data.Odbc.OdbcException E)
                {
                    throw new Exception(E.Message);
                }
                finally
                {
                    cmd.Dispose();
                    connection.Close();
                }
            }
        }
        /// <summary>
        /// 向数据库里插入图像格式的字段(和上面情况类似的另一种实例)
        /// </summary>
        /// <param name="strSQL">SQL语句</param>
        /// <param name="fs">图像字节,数据库的字段类型为image的情况</param>
        /// <returns>影响的记录数</returns>
        public static int ExecuteSqlInsertImg(string strSQL, byte[] fs)
        {
            using (OdbcConnection connection = new OdbcConnection(connectionString))
            {
                OdbcCommand cmd = new OdbcCommand(strSQL, connection);
                System.Data.Odbc.OdbcParameter myParameter = new System.Data.Odbc.OdbcParameter("@fs", OleDbType.Binary);

                myParameter.Value = fs;
                cmd.Parameters.Add(myParameter);
                try
                {
                    connection.Open();
                    int rows = cmd.ExecuteNonQuery();
                    connection.Close();
                    return rows;
                }
                catch (System.Data.Odbc.OdbcException E)
                {
                    throw new Exception(E.Message);
                }
                finally
                {
                    cmd.Dispose();
                    connection.Close();
                }
            }
        }

        /// <summary>
        /// 执行一条计算查询结果语句，返回查询结果（object）。
        /// </summary>
        /// <param name="SQLString">计算查询结果语句</param>
        /// <returns>查询结果（object）</returns>
        public static object GetSingle(string SQLString)
        {
            using (OdbcConnection connection = new OdbcConnection(connectionString))
            {
                using (OdbcCommand cmd = new OdbcCommand(SQLString, connection))
                {
                    try
                    {
                        connection.Open();
                        object obj = cmd.ExecuteScalar();
                        if ((Object.Equals(obj, null)) || (Object.Equals(obj, System.DBNull.Value)))
                        {
                            connection.Close();
                            return null;
                        }
                        else
                        {
                            connection.Close();
                            return obj;
                        }
                    }
                    catch (System.Data.Odbc.OdbcException e)
                    {
                        connection.Close();
                        WriteLog("错误："+e.Message);
                        throw new Exception(e.Message);
                    }
                }
            }
        }
        /// <summary>
        /// 执行查询语句，返回OleDbDataReader
        /// </summary>
        /// <param name="strSQL">查询语句</param>
        /// <returns>OleDbDataReader</returns>
        public static OdbcDataReader ExecuteReader(string strSQL)
        {
            OdbcConnection connection = new OdbcConnection(connectionString);
            OdbcCommand cmd = new OdbcCommand(strSQL, connection);
            try
            {
                connection.Open();

                OdbcDataReader myReader = cmd.ExecuteReader();
                connection.Close();
                return myReader;
            }
            catch (System.Data.Odbc.OdbcException e)
            {
                throw new Exception(e.Message);
            }

        }
        /// <summary>
        /// 执行查询语句，返回DataSet
        /// </summary>
        /// <param name="SQLString">查询语句</param>
        /// <returns>DataSet</returns>
        public static DataSet Query(string SQLString)
        {
            using (OdbcConnection connection = new OdbcConnection(connectionString))
            {
                DataSet ds = new DataSet();
                try
                {
                    connection.Open();
                    OdbcDataAdapter command = new OdbcDataAdapter(SQLString, connection);
                    command.Fill(ds, "ds");
                    connection.Close();
                }
                catch (System.Data.Odbc.OdbcException ex)
                {
                    WriteLog(ex.Message);
                    connection.Close();
                    throw new Exception(ex.Message);
                }
                return ds;
            }
        }


        #endregion

        #region 执行带参数的SQL语句

        /// <summary>
        /// 执行SQL语句，返回影响的记录数
        /// </summary>
        /// <param name="SQLString">SQL语句</param>
        /// <returns>影响的记录数</returns>
        public static int ExecuteSql(string SQLString, params OdbcParameter[] cmdParms)
        {
            using (OdbcConnection connection = new OdbcConnection(connectionString))
            {
                using (OdbcCommand cmd = new OdbcCommand())
                {
                    try
                    {
                        PrepareCommand(cmd, connection, null, SQLString, cmdParms);
                        int rows = cmd.ExecuteNonQuery();
                        cmd.Parameters.Clear();
                        connection.Close();
                        return rows;
                    }
                    catch (System.Data.Odbc.OdbcException E)
                    {
                        throw new Exception(E.Message);
                    }
                }
            }
        }


        /// <summary>
        /// 执行多条SQL语句，实现数据库事务。
        /// </summary>
        /// <param name="SQLStringList">SQL语句的哈希表（key为sql语句，value是该语句的OleDbParameter[]）</param>
        public static void ExecuteSqlTran(Hashtable SQLStringList)
        {
            using (OdbcConnection conn = new OdbcConnection(connectionString))
            {
                conn.Open();
                using (OdbcTransaction trans = conn.BeginTransaction())
                {
                    OdbcCommand cmd = new OdbcCommand();
                    try
                    {
                        //循环
                        foreach (DictionaryEntry myDE in SQLStringList)
                        {
                            string cmdText = myDE.Key.ToString();
                            OdbcParameter[] cmdParms = (OdbcParameter[])myDE.Value;
                            PrepareCommand(cmd, conn, trans, cmdText, cmdParms);
                            int val = cmd.ExecuteNonQuery();
                            cmd.Parameters.Clear();

                            trans.Commit();
                            conn.Close();
                        }
                    }
                    catch
                    {
                        trans.Rollback();
                        throw;
                    }
                }
            }
        }


        /// <summary>
        /// 执行一条计算查询结果语句，返回查询结果（object）。
        /// </summary>
        /// <param name="SQLString">计算查询结果语句</param>
        /// <returns>查询结果（object）</returns>
        public static object GetSingle(string SQLString, params OdbcParameter[] cmdParms)
        {
            using (OdbcConnection connection = new OdbcConnection(connectionString))
            {
                using (OdbcCommand cmd = new OdbcCommand())
                {
                    try
                    {
                        PrepareCommand(cmd, connection, null, SQLString, cmdParms);
                        object obj = cmd.ExecuteScalar();
                        cmd.Parameters.Clear();
                        if ((Object.Equals(obj, null)) || (Object.Equals(obj, System.DBNull.Value)))
                        {
                            connection.Close();
                            return null;
                        }
                        else
                        {
                            connection.Close();
                            return obj;
                        }
                    }
                    catch (System.Data.Odbc.OdbcException e)
                    {
                        connection.Close();
                        throw new Exception(e.Message);
                    }
                }
            }
        }

        /// <summary>
        /// 执行查询语句，返回OleDbDataReader
        /// </summary>
        /// <param name="strSQL">查询语句</param>
        /// <returns>OleDbDataReader</returns>
        public static OdbcDataReader ExecuteReader(string SQLString, params OdbcParameter[] cmdParms)
        {
            OdbcConnection connection = new OdbcConnection(connectionString);
            OdbcCommand cmd = new OdbcCommand();
            try
            {
                PrepareCommand(cmd, connection, null, SQLString, cmdParms);
                OdbcDataReader myReader = cmd.ExecuteReader();
                cmd.Parameters.Clear();
                connection.Close();
                return myReader;
            }
            catch (System.Data.Odbc.OdbcException e)
            {
                throw new Exception(e.Message);
            }

        }

        /// <summary>
        /// 执行查询语句，返回DataSet
        /// </summary>
        /// <param name="SQLString">查询语句</param>
        /// <returns>DataSet</returns>
        public static DataSet Query(string SQLString, params OdbcParameter[] cmdParms)
        {
            using (OdbcConnection connection = new OdbcConnection(connectionString))
            {
                OdbcCommand cmd = new OdbcCommand();
                PrepareCommand(cmd, connection, null, SQLString, cmdParms);
                using (OdbcDataAdapter da = new OdbcDataAdapter(cmd))
                {
                    DataSet ds = new DataSet();
                    try
                    {
                        da.Fill(ds, "ds");
                        cmd.Parameters.Clear();
                        connection.Close();
                    }
                    catch (System.Data.Odbc.OdbcException ex)
                    {
                        throw new Exception(ex.Message);
                    }
                    return ds;
                }
            }
        }


        private static void PrepareCommand(OdbcCommand cmd, OdbcConnection conn, OdbcTransaction trans, string cmdText, OdbcParameter[] cmdParms)
        {
            if (conn.State != ConnectionState.Open)
                conn.Open();
            cmd.Connection = conn;
            cmd.CommandText = cmdText;
            if (trans != null)
                cmd.Transaction = trans;
            cmd.CommandType = CommandType.Text;//cmdType;
            if (cmdParms != null)
            {
                foreach (OdbcParameter parm in cmdParms)
                    cmd.Parameters.Add(parm);
            }
        }


        public static void UpdateAccess(DataTable temp)
        {
            OdbcConnection con = new OdbcConnection(connectionString);
            try
            {
                con.Open();
                OdbcDataAdapter Bada = new OdbcDataAdapter("SELECT *  FROM PRICEMODHDR where 1 =2", con);//建立一个DataAdapter对象
                OdbcCommandBuilder cb = new OdbcCommandBuilder(Bada);//这里的CommandBuilder对象一定不要忘了,一般就是写在DataAdapter定义的后面
                cb.QuotePrefix = "[";
                cb.QuoteSuffix = "]";
                DataSet ds = new DataSet();//建立DataSet对象
                Bada.Fill(ds, "PRICEMODHDR");//填充DataSet
                foreach (DataRow tempRow in temp.Rows)
                {
                    DataRow dr = ds.Tables["PRICEMODHDR"].NewRow();
                    dr.ItemArray = tempRow.ItemArray;//行复制
                    ds.Tables["PRICEMODHDR"].Rows.Add(dr);
                }
                Bada.Update(ds, "PRICEMODHDR");//用DataAdapter的Update()方法进行数据库的更新
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
              
            }
            finally
            {
                con.Close();
            }
        }

        #endregion


        #region 临时日志


        public static void WriteLog(string Text)
        {
            string path = System.Windows.Forms.Application.StartupPath + "\\" + "log";
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
            string Path = path + "\\" + DateTime.Now.ToString("yyyyMMdd") + ".log";
            WriteLogFile(Path, Text);
        }
        public static bool WriteLogFile(string FileName, string message)
        {
            string logText = string.Format("{0} .........  {1}", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss:ffff"), message);
            if (FileName == null || FileName == "")
                return false;

            if (message == null)
                message = "null";

            try
            {
                if (!File.Exists(FileName))
                {
                    ///创建日志文件
                    StreamWriter sr = File.CreateText(FileName);
                    sr.Close();
                }
                System.IO.FileInfo fileInfo = new System.IO.FileInfo(FileName);
                if (fileInfo.Length / (1024 * 1024) > 2)//fa.File.Exists(FileName))
                {
                    string newfilename = DateTime.Now.ToString().Replace(":", "_");
                    newfilename = FileName.Replace(".log", "") + "_" + DateTime.Now.ToString("yyyyMMddHHmmssffff") + fileInfo.Extension;
                    fileInfo.MoveTo(newfilename);
                    ///创建日志文件
                    StreamWriter sr = File.CreateText(FileName);
                    //sr.WriteLine(DateTime.Now.ToString() + "   " + message);
                    sr.WriteLine(logText);
                    sr.Close();
                }
                else
                {
                    ///如果日志文件已经存在，则直接写入日志文件
                    StreamWriter sr = File.AppendText(FileName);
                    // sr.WriteLine("[" + DateTime.Now.ToString() + "] " + message);
                    sr.WriteLine(logText);
                    sr.Close();
                }
                return true;
            }
            catch
            {

            }
            return false;
        }
        #endregion 
    }
}
