using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using ConsumerWorkerService.MessageManage.Model;
using EasyNetQ;
using Message.Client;
using Message.Client.MessageManage.Consume;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace ConsumerWorkerService
{
    public class Worker : BackgroundService
    {
        private readonly ILogger<Worker> _logger;
        static IConfiguration configure = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory())
.AddJsonFile("appsettings.json").Build();

        private static readonly string _token = configure["InvoiceSettings:token"];
        private static readonly string _url = configure["InvoiceSettings:URL"];

        public Worker(ILogger<Worker> logger)
        {
            _logger = logger;
        }


        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {

            try
            {
                while (!stoppingToken.IsCancellationRequested)
                {
          
                    //HandMQErrorMsg();
                    if (configure["TaskConfig:CreateReport"] == "true")
                    {
                        List<SITEDAYBATCH> List = GETSITEDAYBATCH();
                        if (List != null)
                        {
                            UpdateFuelSummaryReport(List);
                            UpdateDrySummaryReport(List);
                            UpdatePostVoidReport(List);
                        }
                    }
                    await Task.Delay(1000 * 60 * 30, stoppingToken);
                }
            }
            catch (Exception ex)
            {
                if (!stoppingToken.IsCancellationRequested)
                {
                    LoggerHelper.WriteInvoceLog("服务异常" + ex.Message + ex.StackTrace);
                    _logger.LogInformation(DateTime.Now.ToString() + "服务异常" + ex.Message + ex.StackTrace);
                }
                else
                {
                    LoggerHelper.WriteInvoceLog("服务停止");
                    _logger.LogInformation(DateTime.Now.ToString() + "服务停止");
                }
            }
        }


        /// <summary>
        /// MQ异常数据处理
        /// </summary>
        public void HandMQErrorMsg()
        {
            string dir = AppContext.BaseDirectory + "/" + "ErrorMsg";
            DirectoryInfo d = new DirectoryInfo(dir);
            FileInfo[] files = d.GetFiles();//文件
            List<ResParameter> ResultList = new List<ResParameter>();
            foreach (FileInfo f in files)
            {
                string ExceptionMsg = System.IO.File.ReadAllText(dir + "/" + f.Name);
                FanoutMessageConsume FanoutMessageConsume = new FanoutMessageConsume();
                FanoutMessageConsume.ExceptionConsume(ExceptionMsg);
                FileInfo fs = new FileInfo(dir + "/" + f.Name);
                fs.Delete();
            }
        }




        #region 报表生成

        public List<SITEDAYBATCH> GETSITEDAYBATCH()
        {
            try
            {
                var db = new SqlHelper();
                var slavedb = new SlaveSqlHelper();
                List<SITEDAYBATCH> List = new List<SITEDAYBATCH>();
                List = slavedb.SqlQueryInfo<SITEDAYBATCH>("SELECT * FROM SITEDAYBATCH   WHERE CREATEDATE > DATE_SUB(NOW(),INTERVAL  1 HOUR)");
                if (List != null && List.Count > 0)
                    return List;
                else
                    return null;
            }

            catch (Exception ex)
            {
                LoggerHelper.WriteLog("报表生成错误，获取营业日信息错误：" + ex.Message);
                throw ex;
            }
        }

        /// <summary>
        /// 汇总油品汇总表
        /// </summary>
        public void UpdateFuelSummaryReport(List<SITEDAYBATCH> SITEDAYBATCH)
        {

            var db = new SqlHelper();
            var slavedb = new SlaveSqlHelper();
            try
            {
                foreach (var daybatch in SITEDAYBATCH)
                {
                    LoggerHelper.WriteLog("开始油品支付汇总生成，站点编码：" + daybatch.SITEID);
                    string sql1 = string.Format("SELECT TI.BARCODE,TI.ITEMNAME,SUM(SALEQTY) AS QTY,SUM((T.TICKET)) AS FUELTIMES FROM  TILLITEM TI ,TILL T WHERE   T.TILLNUM = TI.TILLNUM AND TI.ISFUEL = 'T' AND T.DAY_BATCH_DATE='{0}' AND T.SITEID='{1}'  AND TI.DAY_BATCH_DATE='{2}' AND T.STATUSTYPE IN (1,15) GROUP BY TI.BARCODE", daybatch.DAY_BATCH_DATE.ToString("yyyy-MM-dd"), daybatch.SITEID, daybatch.DAY_BATCH_DATE.ToString("yyyy-MM-dd"));
                    List<Report_PMNT> tillitem = slavedb.SqlQueryInfo<Report_PMNT>(sql1);
                    foreach (var item in tillitem)
                    {
                        Report_PMNTSummaryFuel report_PMNTSummaryFuel = new Report_PMNTSummaryFuel();
                        report_PMNTSummaryFuel.ID = Guid.NewGuid().ToString();
                        report_PMNTSummaryFuel.SITEID = daybatch.SITEID;
                        report_PMNTSummaryFuel.SITENAME = daybatch.SITEID;
                        report_PMNTSummaryFuel.DAY_BATCH_DATE = daybatch.DAY_BATCH_DATE;
                        report_PMNTSummaryFuel.BARCODE = item.BARCODE;
                        report_PMNTSummaryFuel.ITEMNAME = item.ITEMNAME.Replace("?", "");
                        report_PMNTSummaryFuel.QTY = item.QTY.ToDouble(2);
                        report_PMNTSummaryFuel.STATUS = daybatch.STATUS;
                        report_PMNTSummaryFuel.FUELTIMES = item.FUELTIMES;
                        string sql2 = string.Format("SELECT TI.BARCODE,SUM(SALEQTY) AS VIPQTY FROM  TILLITEM TI ,TILL T WHERE   T.TILLNUM = TI.TILLNUM AND TI.ISFUEL = 'T' AND T.DAY_BATCH_DATE='{0}' AND T.CUST_MAGKEY>0 AND TI.BARCODE={1} AND TI.SITEID='{2}'  AND TI.DAY_BATCH_DATE='{3}' 	AND T.STATUSTYPE IN (1,15) GROUP BY TI.BARCODE", daybatch.DAY_BATCH_DATE.ToString("yyyy-MM-dd"), report_PMNTSummaryFuel.BARCODE, daybatch.SITEID, daybatch.DAY_BATCH_DATE.ToString("yyyy-MM-dd"));
                        List<Report_PMNT1> vipqty = slavedb.SqlQueryInfo<Report_PMNT1>(sql2);
                        if (vipqty != null && vipqty.Count > 0)
                            report_PMNTSummaryFuel.VIPQTY = Math.Round(Convert.ToDouble(vipqty[0].VIPQTY), 2);
                        else
                            report_PMNTSummaryFuel.VIPQTY = 0;
                        string sql3 = string.Format("SELECT TI.BARCODE,SUM(TP.QTY) AS B2CQTY FROM  TILLITEM TI ,TILL T,TILLITEM_PMNT TP WHERE   T.TILLNUM = TI.TILLNUM AND TP.TILLNUM=TI.TILLNUM  AND TI.ISFUEL = 'T' AND T.DAY_BATCH_DATE='{0}' AND TP.PMNTEXTREF=15 AND TI.BARCODE={1} AND TI.SITEID='{2}'  AND TP.DAY_BATCH_DATE='{3}' 	AND T.STATUSTYPE IN (1,15)  GROUP BY TI.BARCODE", daybatch.DAY_BATCH_DATE.ToString("yyyy-MM-dd"), report_PMNTSummaryFuel.BARCODE, daybatch.SITEID, daybatch.DAY_BATCH_DATE.ToString("yyyy-MM-dd"));
                        List<Report_PMNT2> B2CQTY = slavedb.SqlQueryInfo<Report_PMNT2>(sql3);
                        if (B2CQTY != null && B2CQTY.Count > 0)
                            report_PMNTSummaryFuel.B2CQTY = Math.Round(Convert.ToDouble(B2CQTY[0].B2CQTY), 2);
                        else
                            report_PMNTSummaryFuel.B2CQTY = 0;
                        report_PMNTSummaryFuel.VIPQTY1 = (report_PMNTSummaryFuel.VIPQTY - report_PMNTSummaryFuel.B2CQTY).ToDouble(2);
                        string sql4 = string.Format("SELECT TI.BARCODE,SUM(TP.QTY) AS B2BQTY FROM  TILLITEM TI ,TILL T,TILLITEM_PMNT TP WHERE   T.TILLNUM = TI.TILLNUM AND TP.TILLNUM=TI.TILLNUM  AND TI.ISFUEL = 'T' AND T.DAY_BATCH_DATE='{0}' AND TP.PMNTEXTREF=18 AND TI.BARCODE={1} AND TI.SITEID='{2}' AND TI.DAY_BATCH_DATE='{2}'  AND TI.DAY_BATCH_DATE='{3}' 	AND T.STATUSTYPE IN (1,15) GROUP BY TI.BARCODE", daybatch.DAY_BATCH_DATE.ToString("yyyy-MM-dd"), report_PMNTSummaryFuel.BARCODE, daybatch.SITEID, daybatch.DAY_BATCH_DATE.ToString("yyyy-MM-dd"), daybatch.DAY_BATCH_DATE.ToString("yyyy-MM-dd"));
                        List<Report_PMNT3> B2BQTY = slavedb.SqlQueryInfo<Report_PMNT3>(sql4);
                        if (B2BQTY != null && B2BQTY.Count > 0)
                            report_PMNTSummaryFuel.B2BQTY = Math.Round(Convert.ToDouble(B2BQTY[0].B2BQTY), 2);
                        else
                            report_PMNTSummaryFuel.B2BQTY = 0;
                        string sql5 = string.Format("SELECT TI.BARCODE,SUM(TP.QTY) AS OneKeyQTY FROM  TILLITEM TI ,TILL T,TILLITEM_PMNT TP  WHERE   T.TILLNUM = TI.TILLNUM AND TP.TILLNUM=TI.TILLNUM  AND TI.ISFUEL = 'T' AND T.DAY_BATCH_DATE='{0}' AND TI.DAY_BATCH_DATE='{1}'  AND TP.DAY_BATCH_DATE='{2}' AND TI.BARCODE={3} AND TI.SITEID='{4}' AND  (TP.PMNT_NAME LIKE'%室外%' OR TP.PMNT_NAME LIKE'%小程序%') 	AND T.STATUSTYPE IN (1,15) GROUP BY TI.BARCODE", daybatch.DAY_BATCH_DATE.ToString("yyyy-MM-dd"), daybatch.DAY_BATCH_DATE.ToString("yyyy-MM-dd"), daybatch.DAY_BATCH_DATE.ToString("yyyy-MM-dd"), report_PMNTSummaryFuel.BARCODE, daybatch.SITEID);
                        List<Report_PMNT4> OneKeyQTY = slavedb.SqlQueryInfo<Report_PMNT4>(sql5);
                        if (OneKeyQTY != null && OneKeyQTY.Count > 0)
                            report_PMNTSummaryFuel.OneKeyQTY = Math.Round(Convert.ToDouble(OneKeyQTY[0].OneKeyQTY), 2);
                        else
                            report_PMNTSummaryFuel.OneKeyQTY = 0;
                        report_PMNTSummaryFuel.CREATEDATE = DateTime.Now;
                        Report_PMNTSummaryFuel PMNTSummaryFuel = slavedb.QueryInfo<Report_PMNTSummaryFuel>(t => t.SITEID == daybatch.SITEID && t.DAY_BATCH_DATE == daybatch.DAY_BATCH_DATE && t.BARCODE == report_PMNTSummaryFuel.BARCODE);
                        if (PMNTSummaryFuel != null)
                        {
                            report_PMNTSummaryFuel.ID = PMNTSummaryFuel.ID;
                            db.UpdateInfo<Report_PMNTSummaryFuel>(report_PMNTSummaryFuel);
                        }
                        //判断是否存在
                        else
                        {
                            db.InsertInto<Report_PMNTSummaryFuel>(report_PMNTSummaryFuel);
                        }
                        LoggerHelper.WriteLog("完成油品支付汇总生成，站点编码：" + daybatch.SITEID);
                    }
                }

            }
            catch (Exception ex)
            {
                LoggerHelper.WriteLog("生成油品支付方式汇总错误：" + ex.Message);
                throw ex;
            }

        }

        //汇总撤销报表
        public void UpdatePostVoidReport(List<SITEDAYBATCH> SITEDAYBATCH)
        {
            var db = new SqlHelper();
            var slavedb = new SlaveSqlHelper();
            try
            {
                foreach (var daybatch in SITEDAYBATCH)
                {
                    LoggerHelper.WriteLog("开始撤销次数汇总生成，站点编码：" + daybatch.SITEID);
                    string sql = string.Format("SELECT SITEID,DAY_BATCH_DATE,COUNT(*) AS TIMES FROM TILL WHERE STATUSTYPE IN (15,4) AND DAY_BATCH_DATE='{0}'  AND SITEID='{1}' GROUP BY SITEID ORDER BY COUNT(*)  DESC", daybatch.DAY_BATCH_DATE.ToString("yyyy-MM-dd"), daybatch.SITEID);
                    List<Report_PostVoidTemp> Report_PostVoidTemp = slavedb.SqlQueryInfo<Report_PostVoidTemp>(sql);
                    if (Report_PostVoidTemp != null && Report_PostVoidTemp.Count > 0)
                    {
                        foreach (var item in Report_PostVoidTemp)
                        {
                            Report_PostVoid Report_PostVoid = new Report_PostVoid();
                            Report_PostVoid.ID = Guid.NewGuid().ToString();
                            Report_PostVoid.SITEID = item.SITEID;
                            Report_PostVoid.SITENAME = item.SITEID;
                            Report_PostVoid.DAY_BATCH_DATE = item.DAY_BATCH_DATE;
                            Report_PostVoid.TIMES = item.TIMES;
                            Report_PostVoid.CREATEDATE = DateTime.Now;
                            Report_PostVoid Report_PostVoid1 = slavedb.QueryInfo<Report_PostVoid>(t => t.SITEID == daybatch.SITEID && t.DAY_BATCH_DATE == item.DAY_BATCH_DATE);
                            if (Report_PostVoid1 != null)
                            {
                                Report_PostVoid.ID = Report_PostVoid1.ID;
                                db.UpdateInfo<Report_PostVoid>(Report_PostVoid);
                            }
                            //判断是否存在
                            else
                            {
                                db.InsertInto<Report_PostVoid>(Report_PostVoid);
                            }
                        }
                    }
                    LoggerHelper.WriteLog("完成撤销次数汇总生成，站点编码：" + daybatch.SITEID);
                }

            }
            catch (Exception ex)
            {
                LoggerHelper.WriteLog("完成撤销次数汇总生成：" + ex.Message);
                throw ex;
            }
        }


        /// <summary>
        /// 汇总非油品汇总表
        /// </summary>
        public void UpdateDrySummaryReport(List<SITEDAYBATCH> SITEDAYBATCH)
        {
            var db = new SqlHelper();
            var slavedb = new SlaveSqlHelper();
            try
            {
                foreach (var daybatch in SITEDAYBATCH)
                {
                    LoggerHelper.WriteLog("开始非油品支付汇总生成，站点编码：" + daybatch.SITEID);
                    string sql = string.Format("SELECT LEFT(TI.DEPT,4)AS DEPT1,SUM(TOTAL) AS QTY,SUM((T.TICKET)) AS DRYTIMES FROM  TILLITEM TI ,TILL T WHERE   T.TILLNUM = TI.TILLNUM AND TI.ISFUEL = 'F' AND T.DAY_BATCH_DATE='{0}' AND T.SITEID='{1}' AND TI.DAY_BATCH_DATE='{0}' GROUP BY DEPT1", daybatch.DAY_BATCH_DATE.ToString("yyyy-MM-dd"), daybatch.SITEID, daybatch.DAY_BATCH_DATE.ToString("yyyy-MM-dd"));
                    List<Report_SummaryDry> SummaryDry = slavedb.SqlQueryInfo<Report_SummaryDry>(sql);
                    if (SummaryDry != null && SummaryDry.Count > 0)
                    {
                        foreach (var item in SummaryDry)
                        {
                            Report_SummaryDry Report_SummaryDry = new Report_SummaryDry();
                            Report_SummaryDry.ID = Guid.NewGuid().ToString();
                            Report_SummaryDry.SITEID = daybatch.SITEID;
                            Report_SummaryDry.SITENAME = daybatch.SITEID;
                            Report_SummaryDry.STATUS = daybatch.STATUS;
                            Report_SummaryDry.DAY_BATCH_DATE = daybatch.DAY_BATCH_DATE;
                            Report_SummaryDry.DRYTIMES = item.DRYTIMES;
                            Report_SummaryDry.DEPT1 = item.DEPT1;
                            Report_SummaryDry.QTY = Math.Round(Convert.ToDouble(item.QTY), 2);
                            Report_SummaryDry.CREATEDATE = DateTime.Now;
                            Report_SummaryDry Report_SummaryDry1 = slavedb.QueryInfo<Report_SummaryDry>(t => t.SITEID == daybatch.SITEID && t.DAY_BATCH_DATE == daybatch.DAY_BATCH_DATE && t.DEPT1 == Report_SummaryDry.DEPT1);
                            if (Report_SummaryDry1 != null)
                            {
                                Report_SummaryDry.ID = Report_SummaryDry1.ID;
                                db.UpdateInfo<Report_SummaryDry>(Report_SummaryDry);
                            }
                            //判断是否存在
                            else
                            {
                                db.InsertInto<Report_SummaryDry>(Report_SummaryDry);
                            }
                        }
                    }
                    LoggerHelper.WriteLog("完成非油品汇总生成，站点编码：" + daybatch.SITEID);
                }

            }
            catch (Exception ex)
            {
                LoggerHelper.WriteLog("生成非油品汇总错误：" + ex.Message);
                throw ex;
            }

        }

        #endregion



    }
}
