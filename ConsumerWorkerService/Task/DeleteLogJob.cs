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
                   HandReadyInvoiceMsg();
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

        //流水提交到发票--正常流程
        public void HandReadyInvoiceMsg()
        {
            try
            {
                LoggerHelper.WriteInvoceLog("======================================");
                LoggerHelper.WriteInvoceLog("开始执行===交易流水到发票系统===定时任务！");
                 string dir = AppContext.BaseDirectory + "/" + "ReadyInvoiceMsg";
               // string dir = "C:\\1";
                DirectoryInfo d = new DirectoryInfo(dir);
                FileInfo[] files = d.GetFiles();//文件
                FanoutMessageConsume Consume = new FanoutMessageConsume();
                List<ResParameter> ResultList = new List<ResParameter>();
                foreach (FileInfo f in files)
                {
                    string ExceptionMsg = System.IO.File.ReadAllText(dir + "/" + f.Name);
                    ResParameter res = MyObject(ExceptionMsg, f);                   
                    FileInfo fs = new FileInfo(dir + "/" + f.Name);
                    if (res.GUID == null)
                    {
                        string newfile = AppContext.BaseDirectory + "/" + "InvoiceErrorMsg" + "/" + f.Name;
                        fs.CopyTo(newfile);
                    }
                    else
                    {
                        ResultList.Add(res);
                    }
                    fs.Delete();
                }
                if (ResultList.Count >50)
                {
                    int numb = 50;
                    int size = (int)Math.Ceiling((decimal)ResultList.Count / numb);
                    for (int i = 1; i <= size; i++)
                    {
                        List<ResParameter> EndResultList = ResultList.Skip((i - 1) * numb).Take(numb).ToList();
                        if (Consume.PostInvoiceList(EndResultList))
                        {
                            LoggerHelper.WriteInvoceLog("执行完毕===交易流水到发票系统===定时任务");
                        }
                    }
                }
                else
                {
                    if (Consume.PostInvoiceList(ResultList))
                    {
                        LoggerHelper.WriteInvoceLog("执行完毕===交易流水到发票系统===定时任务");
                    }
                }
            }
            catch (Exception ex)
            {
                LoggerHelper.WriteInvoceLog("发送发票消息失败：" + ex.Message);
            }
        }

        public ResParameter MyObject(string ExceptionMsg, FileInfo f)
        {
            ResParameter res=new ResParameter();
            try
            {
                res= ExceptionMsg.ToObject<ResParameter>();
            }
            catch (Exception ex)
            {
                LoggerHelper.WriteInvoceLog("数据格式错误：" + ex.Message);
            }
            return res;
        }
    }
}
