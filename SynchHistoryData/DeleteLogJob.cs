using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Threading;
using System.Threading.Tasks;
using System.IO;
using Message.Client.MessageManage.Consume;
using ConsumerWorkerService.MessageManage.Model;
using System.Collections.Generic;
using System.Linq;
using Common.RabbitMQ;
using System.Data;

namespace ConsumerWorkerService
{

    public class Bos2HosMessage
    {



        public string Type { get; set; }
        public string SiteId { get; set; }

        public DataTable Data { get; set; }
    }
    /// <summary>
    /// 简单的定时任务执行
    /// </summary>
    public class DeleteLogJob : BackgroundService
    {
        public ILogger _logger;
        public DeleteLogJob(ILogger<DeleteLogJob> logger)
        {
            this._logger = logger;
        }
        static DateTime date1;
        static DateTime date2;
   
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            try
            {
                LoggerHelper.WriteLog("删除日志服务：启动");
                _logger.LogInformation(DateTime.Now.ToString() + "删除日志服务：启动"); 
                while (!stoppingToken.IsCancellationRequested)
                {
                    LoggerHelper.WriteLog("开始执行删除日志任务");
                    string path = AppContext.BaseDirectory + "/" + "log";
                    LoggerHelper.WriteLog(path);
                    if (Directory.Exists(path))
                    {
                        LoggerHelper.WriteLog("OK");
                        date1 = DateTime.Now.AddDays(-10);
                        date2 = DateTime.Now.AddDays(-60);
                        delete(path);
                    }
                    LoggerHelper.WriteLog("已经执行删除日志任务");
                    _logger.LogInformation(DateTime.Now.ToString() + " 已经执行删除日志任务");
                    await Task.Delay(60000, stoppingToken);
                }

                _logger.LogInformation(DateTime.Now.ToString() + "删除日志服务：停止");
                LoggerHelper.WriteLog("删除日志服务：停止");
            }
            catch (Exception ex)
            {
                if (!stoppingToken.IsCancellationRequested)
                {
                    LoggerHelper.WriteLog("删除日志服务：异常" + ex.Message + ex.StackTrace);
                    _logger.LogInformation(DateTime.Now.ToString() + "删除日志服务：异常" + ex.Message + ex.StackTrace);
                }
                else
                {
                    LoggerHelper.WriteLog("删除日志服务：停止");
                    _logger.LogInformation(DateTime.Now.ToString() + "删除日志服务：停止");
                }
            }
        }
        private static void delete(String dir)
        {
            DirectoryInfo d = new DirectoryInfo(dir);
            FileInfo[] files = d.GetFiles();//文件
            DirectoryInfo[] directs = d.GetDirectories();//文件夹
            foreach (FileInfo f in files)
            {
                FileInfo fs = new FileInfo(dir + "/" + f.Name);
                DateTime dates = Convert.ToDateTime(fs.LastWriteTime);
                if (dates <= date1)
                {
                    fs.Delete();
                    LoggerHelper.WriteLog("删除文件："+ fs.FullName);
                }
            }
        }
    }
}
