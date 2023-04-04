using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace ConsumerWorkerService
{
    public static partial class LoggerHelper
    {

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

        public static void WriteInvoiceText(string Text)
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
        public static void WriteInvoceLog(string Text)
        {
            string path = AppContext.BaseDirectory + "/" + "log";
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
            string Path = path + "/" + "PostInvoice_" + DateTime.Now.ToString("yyyyMMdd") + ".log";
            WriteInvoiceLogFile(Path, Text);
        }

        public static void WriteISynchLog(string Text)
        {
            string path = AppContext.BaseDirectory + "/" + "log";
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
            string Path = path + "/" + "Synch_" + DateTime.Now.ToString("yyyyMMdd") + ".log";
            WriteSynchLogFile(Path, Text);
        }


        public static bool WriteSynchLogFile(string FileName, string message)
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

        public static bool WriteInvoiceLogFile(string FileName, string message)
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

        public static bool WrireInvoiceErrorJson(string message)
        {
            string path = AppContext.BaseDirectory + "/" + "InvoiceErrorMsg";
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
            if (!File.Exists(FileName))
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
                StreamWriter sr = File.AppendText(FileName);
                // sr.WriteLine("[" + DateTime.Now.ToString() + "] " + message);
                sr.WriteLine(logText);
                sr.Close();
            }
            return true;

        }



        public static  void WriteText(string Text)
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
                sr.WriteLine(logText);
                sr.Close();
            }
            return true;
        }
    }
}
