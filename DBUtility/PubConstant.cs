using System;
using System.Configuration;
using System.IO;
using Microsoft.Win32;

namespace DBUtility
{

    public class PubConstant
    {



        /// <summary>
        /// 获取连接字符串
        /// </summary>
        public static string BOSConnectionString
        {
            get
            {
                string temp = GetRegistData().Substring(10, 1);

                if (temp == "C" || temp == "c")
                {
                    return "Driver={Easysoft IB6 ODBC};Server=localhost;Database=LocalHost:c:\\Office\\db\\Office.gdb;Uid=SYSDBA;Pwd=masterkey";
                }
                else
                {
                    return "Driver={Easysoft IB6 ODBC};Server=localhost;Database=LocalHost:d:\\Office\\db\\Office.gdb;Uid=SYSDBA;Pwd=masterkey";
                }

                //return "Driver={Easysoft IB6 ODBC};Server=10.81.7.8;Database=10.81.7.8:c:\\Office\\db\\Office.gdb;Uid=SYSDBA;Pwd=masterkey";
            }
        }

        public static string ConnectionString
        {
            get
            {
                string dataDir = AppDomain.CurrentDomain.BaseDirectory + "DataBase.db";
                string _connectionString = string.Format("Data Source={0};Initial Catalog=sqlite;Integrated Security=True;Max Pool Size=10", dataDir);
                return _connectionString;
            }
        }

        private static string GetRegistData()
        {
            try
            {
                string registData;
                RegistryKey hkml = Registry.LocalMachine;
                RegistryKey software = hkml.OpenSubKey("SOFTWARE", true);
                RegistryKey aimdir = software.OpenSubKey("Positive", true);
                RegistryKey subaimdir1 = aimdir.OpenSubKey("DATABASES", true);
                RegistryKey subaimdir2 = subaimdir1.OpenSubKey("OFFICE", true);
                registData = subaimdir2.GetValue("DatabaseName").ToString();
                return registData;
            }
            catch (Exception e)
            {
                WriteLog("错误：" + e.Message);
                return null;
            }


        }






        #region 日志
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
