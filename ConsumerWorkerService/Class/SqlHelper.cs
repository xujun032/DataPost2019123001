using Microsoft.Extensions.Configuration;
using SqlSugar;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace ConsumerWorkerService
{
    public class SqlHelper
    {
        public SqlSugarClient db;

        static IConfiguration configure = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json").Build();
        private static readonly string _connectionstring = configure["DBSetting:ConnectString"];
        // public BaseHelper(string connectionString)
        public SqlHelper()
        {
            db = new SqlSugarClient(
                   new ConnectionConfig()
                   {
                       ConnectionString = _connectionstring,
                       DbType = DbType.MySql,//设置数据库类型
                       IsAutoCloseConnection = true,//自动释放数据务，如果存在事务，在事务结束后释放
                       InitKeyType = InitKeyType.Attribute //从实体特性中读取主键自增列信息
                   });

            //用来打印Sql方便你调式    
            db.Aop.OnLogExecuting = (sql, pars) =>
            {
                //Console.WriteLine(sql + "\r\n" +
                //db.Utilities.SerializeObject(pars.ToDictionary(it => it.ParameterName, it => it.Value)));
                ////WriteLog(sql + "\r\n" +
                ////db.Utilities.SerializeObject(pars.ToDictionary(it => it.ParameterName, it => it.Value)));
                //Console.WriteLine();
            };
        }
        public SqlSugarClient GetDb()
        {
            return db;
        }

        public void BeginTran()
        {
            db.BeginTran();
        }

        public void CommitTran()
        {
            db.CommitTran();
        }

        public void RollbackTran()
        {
            db.RollbackTran();
        }
        public bool InsertInto<T>(T obj) where T : class, new()
        {
            return db.Insertable(obj).ExecuteCommandIdentityIntoEntity();
        }



        public int UpdateInfo<T>(Expression<Func<T, bool>> set, Expression<Func<T, bool>> where) where T : class, new()
        {
            return db.Updateable<T>().SetColumns(set).Where(where).ExecuteCommand();
        }

        public int UpdateInfo<T>(T obj) where T : class, new()
        {
            return db.Updateable<T>(obj).ExecuteCommand();
        }

        public T QueryInfo<T>(Expression<Func<T, bool>> where) where T : class, new()
        {
            return db.Queryable<T>().First(where);
        }

     
        public int  DeleteInfo<T>(T obj) where T : class, new()
        {

          
            return db.Deleteable<T>(obj).ExecuteCommand();
        }


        public int DeleteInfo<T>(Expression<Func<T, bool>> where) where T : class, new()
        {
            return db.Deleteable<T>().Where(where).ExecuteCommand();//删除等于1的
        }
        public List<T> SqlQueryInfo<T>(string sql) where T : class, new()
        {
            return db.SqlQueryable<T>(sql).ToList();
        }

        public bool IsExit<T>(Expression<Func<T, bool>> where) where T : class, new()
        {
            return db.Queryable<T>().Any(where);
        }
        #region 日志
        public void WriteText(string Text)
        {
            string logText = string.Format("{0}   {1}" + Environment.NewLine, DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss:ffff"), Text);
            string path = AppContext.BaseDirectory + "/" + "log";
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
            string Path = path + "/" + DateTime.Now.ToString("yyyyMMdd") + ".log";
            WriteLogFile(Path, Text);
        }
        public static void WriteLog(string Text)
        {
            string path = AppContext.BaseDirectory + "/" + "log";
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
            string Path = path + "/" + DateTime.Now.ToString("yyyyMMdd") + ".log";
            WriteLogFile(Path, Text);
        }
        public static bool WriteLogFile(string FileName, string message)
        {
            string logText = string.Format("{0} .........  {1}", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss:ffff"), message);
            if (FileName == null || FileName == "")
                return false;

            if (message == null)
                message = "null";

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

        public static bool WrireErrorJson(string message)
        {
            string path = AppContext.BaseDirectory + "/" + "ErrorMsg";
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
            string Path = path + "/" + DateTime.Now.ToString("yyyyMMdd") + ".log";
            string logText = message;
            string FileName = path + "/" + DateTime.Now.ToString("yyyyMMddHHmmssffff") + ".json";
            if (FileName == null || FileName == "")
                return false;

            if (message == null)
                message = "null";
            if (!File.Exists(FileName))
            {
                ///创建文件
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
        #endregion 
    }
}
