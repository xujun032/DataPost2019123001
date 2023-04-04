using Common.RabbitMQ;
using ConsumerWorkerService;
using ConsumerWorkerService.MessageManage.Model;
using SqlSugar;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace Message.Client.MessageManage.Consume
{
    public class FanoutMessageConsume : IMessageConsume
    {
        public void Consume(string message)
        {

            WriteLog("获得消息：" + Regex.Match(message, ".*?(?=Data)").Value);
            Console.WriteLine("Get the message:  " + message);
            ResParameter Result = message.ToObject<ResParameter>();
            switch (Result.Type)
            {
                case "Till":
                    SaveTill(Result);
                    break;
                case "SiteInfo":
                    SaveSiteInfo(Result);
                    break;
                case "SITEDAYBATCH":
                    SaveDayBatchData(Result);
                    break;
            }
        }


        #region 不同类型的数据入库操作

        /// <summary>
        /// 交易流水入库
        /// </summary>
        /// <param name="Result"></param>
        public void SaveTill(ResParameter Result)
        {
            var db = new SqlHelper();
            TILL tillEntitys = new TILL();
            List<TILL> TILL_LIST = new List<TILL>();
            List<TILLITEM> TILLITEM_LIST = new List<TILLITEM>();
            List<TILL_PMNT> TILL_PMNT_LIST = new List<TILL_PMNT>();
            List<TILL_PROMO> TILL_PROMO_LIST = new List<TILL_PROMO>();
            List<TILLITEM_PMNT> TILLITEM_PMNT_LIST = new List<TILLITEM_PMNT>();
            List<TILLITEM_PROMO> TILLITEM_PROMO_LIST = new List<TILLITEM_PROMO>();

            try
            {
                TILL_LIST = Result.Data.Till;
                db.BeginTran();
                foreach (var till in TILL_LIST)
                {
                    db.InsertInto<TILL>(CreateTILLModel(till, Result.SiteId));
                }
                TILLITEM_LIST = Result.Data.TILLITEM;
                foreach (var tillitem in TILLITEM_LIST)
                {
                    Report_TILL ReportTILL = new Report_TILL();
                    ReportTILL.ID = Guid.NewGuid().ToString();
                    ReportTILL.SITEID = Result.SiteId;
                    ReportTILL.SITENAME = ReportTILL.SITEID;
                    ReportTILL.POS_ID = tillitem.POS_ID;
                    TILL Till = TILL_LIST.FirstOrDefault(t => t.TILLNUM == tillitem.TILLNUM);
                    ReportTILL.WTR = Till.WTR;
                    ReportTILL.TILLNUM = tillitem.TILLNUM;
                    ReportTILL.DAY_BATCH_DATE = tillitem.DAY_BATCH_DATE;
                    ReportTILL.POSTED = tillitem.POSTED;
                    ReportTILL.BARCODE = tillitem.BARCODE;
                    ReportTILL.DEPT = tillitem.DEPT.Substring(0, 4);
                    ReportTILL.ITEMNAME = tillitem.ITEMNAME.Replace("?", "");
                    ReportTILL.STDPRICE = tillitem.STDPRICE;
                    ReportTILL.SALEQTY = tillitem.SALEQTY;
                    if (tillitem.ISFUEL == "T")
                    {
                        ReportTILL.RETAIL_AMOUNT = tillitem.PUMP_TOTAL;
                        ReportTILL.DISCOUNT_AMOUNT = Math.Round(Convert.ToDouble(tillitem.PUMP_TOTAL - tillitem.TOTAL), 2);
                    }
                    else
                    {
                        ReportTILL.RETAIL_AMOUNT = (tillitem.STDPRICE * tillitem.SALEQTY);
                        ReportTILL.DISCOUNT_AMOUNT = Math.Round(Convert.ToDouble((tillitem.STDPRICE * tillitem.SALEQTY) - tillitem.TOTAL), 2);
                    }
                    ReportTILL.TOTAL = tillitem.TOTAL;
                    ReportTILL.STATUSTYPE = tillitem.STATUSTYPE;

                    if (!string.IsNullOrEmpty(Till.CUST_MAGKEY))
                    {
                        ReportTILL.CUST_MAGKEY = Till.CUST_MAGKEY;
                        ReportTILL.ISVIP = "T";
                    }
                    else
                    {
                        ReportTILL.ISVIP = "F";
                    }
                    ReportTILL.REMARK1 = tillitem.ISFUEL;
                    ReportTILL.CREATEDATE = Till.CREATEDATE;
                    db.InsertInto<Report_TILL>(ReportTILL);
                    db.InsertInto<TILLITEM>(CreateTILLITEMModel(tillitem, Result.SiteId));
                }

                TILL_PMNT_LIST = Result.Data.TILL_PMNT;
                foreach (var tillpmnt in TILL_PMNT_LIST)
                {
                    db.InsertInto<TILL_PMNT>(CreateTILL_PMNTModel(tillpmnt, Result.SiteId));
                }
                TILL_PROMO_LIST = Result.Data.TILL_PROMO;
                foreach (var tillpromo in TILL_PROMO_LIST)
                {
                    db.InsertInto<TILL_PROMO>(CreateTILL_PROMOModel(tillpromo, Result.SiteId));
                }
                TILLITEM_PMNT_LIST = Result.Data.TILLITEM_PMNT;
                foreach (var tillitempmnt in TILLITEM_PMNT_LIST)
                {
                    db.InsertInto<TILLITEM_PMNT>(CreateTILLITEM_PMNTModel(tillitempmnt, Result.SiteId));
                }
                TILLITEM_PROMO_LIST = Result.Data.TILLITEM_PROMO;
                foreach (var tillitempromo in TILLITEM_PROMO_LIST)
                {
                    db.InsertInto<TILLITEM_PROMO>(CreateTILLITEM_PROMOModel(tillitempromo, Result.SiteId));
                }

                db.CommitTran();
                WriteLog("原始交易流水数据已入库：" + Result.GUID);
            }
            catch (Exception ex)
            {
                WriteLog("原始交易流水消费错误：" + ex.Message);
                db.RollbackTran();
            }
        }

        public TILL CreateTILLModel(TILL till, string siteid)
        {
            till.ID = Guid.NewGuid().ToString();
            till.SITEID = siteid;
            till.CREATEDATE = DateTime.Now;
            return till;
        }

        public TILLITEM CreateTILLITEMModel(TILLITEM till, string siteid)
        {
            till.ID = Guid.NewGuid().ToString();
            till.SITEID = siteid;
            till.CREATEDATE = DateTime.Now;
            return till;
        }

        public TILL_PMNT CreateTILL_PMNTModel(TILL_PMNT till, string siteid)
        {
            till.ID = Guid.NewGuid().ToString();
            till.SITEID = siteid;
            till.CREATEDATE = DateTime.Now;
            return till;
        }

        public TILL_PROMO CreateTILL_PROMOModel(TILL_PROMO till, string siteid)
        {
            till.ID = Guid.NewGuid().ToString();
            till.SITEID = siteid;
            till.CREATEDATE = DateTime.Now;
            return till;
        }
        public TILLITEM_PMNT CreateTILLITEM_PMNTModel(TILLITEM_PMNT till, string siteid)
        {
            till.ID = Guid.NewGuid().ToString();
            till.SITEID = siteid;
            till.CREATEDATE = DateTime.Now;
            return till;
        }

        public TILLITEM_PROMO CreateTILLITEM_PROMOModel(TILLITEM_PROMO till, string siteid)
        {
            till.ID = Guid.NewGuid().ToString();
            till.SITEID = siteid;
            till.CREATEDATE = DateTime.Now;
            return till;
        }


        public void SaveSiteInfo(ResParameter Result)
        {
            try
            {
                var db = new SqlHelper();
                SITEINFO siteinfo = new SITEINFO();
                siteinfo.ID = Guid.NewGuid().ToString();
                siteinfo.SITEID = Result.Data.SiteID;
                siteinfo.SITENAME = Result.Data.SiteName;
                siteinfo.CREATEDATE = DateTime.Now;
                if (db.InsertInto<SITEINFO>(siteinfo))
                {
                    WriteLog("新站点注册成功:" + siteinfo.SITEID + "    " + siteinfo.SITENAME);
                }
            }
            catch (Exception ex)
            {
                WriteLog("错误：" + ex.Message);
            }
        }
        /// <summary>
        /// 保存营业日信息
        /// </summary>
        /// <param name="till"></param>
        /// <returns></returns>
        public void SaveDayBatchData(ResParameter Result)
        {
            var db = new SqlHelper();
            try
            {
                List<SITEDAYBATCH> SITEDAYBATCH = new List<SITEDAYBATCH>();
                SITEDAYBATCH = Result.Data.SITEDAYBATCH;
                db.BeginTran();
                foreach (var daybatch in SITEDAYBATCH)
                {
                    SITEDAYBATCH SITEBATCH = db.QueryInfo<SITEDAYBATCH>(t => t.SITEID == Result.SiteId && t.DAY_BATCH_DATE == daybatch.DAY_BATCH_DATE);
                    if (SITEBATCH != null)
                    {
                        daybatch.SITENAME = SITEBATCH.SITENAME;
                        db.UpdateInfo<SITEDAYBATCH>(UpdateDayBatchModel(daybatch, SITEBATCH.ID, Result.SiteId));
                    }
                    //判断是否存在
                    else
                    {
                        db.InsertInto<SITEDAYBATCH>(CreateDayBatchModel(daybatch, Result.SiteId));
                    }
    
                }
                db.CommitTran();
                WriteLog("营业日班次信息入库成功：" + Result.GUID);
                UpdateFuelSummaryReport(SITEDAYBATCH);
                UpdateDrySummaryReport(SITEDAYBATCH);
            }
            catch (Exception ex)
            {
                WriteLog("营业日班次信息错误：" + ex.Message);
                db.RollbackTran();
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
                    string sql1 = string.Format("SELECT TI.BARCODE,TI.ITEMNAME,SUM(SALEQTY) AS QTY,SUM((T.TICKET)) AS FUELTIMES FROM  TILLITEM TI ,TILL T WHERE   T.TILLNUM = TI.TILLNUM AND TI.ISFUEL = 'T' AND T.DAY_BATCH_DATE='{0}' AND T.SITEID={1}  AND TI.DAY_BATCH_DATE='{2}' GROUP BY TI.BARCODE", daybatch.DAY_BATCH_DATE.ToString("yyyy-MM-dd"), daybatch.SITEID, daybatch.DAY_BATCH_DATE.ToString("yyyy-MM-dd"));
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
                        string sql2 = string.Format("SELECT TI.BARCODE,SUM(SALEQTY) AS VIPQTY FROM  TILLITEM TI ,TILL T WHERE   T.TILLNUM = TI.TILLNUM AND TI.ISFUEL = 'T' AND T.DAY_BATCH_DATE='{0}' AND T.CUST_MAGKEY>0 AND TI.BARCODE={1} AND TI.SITEID={2}  AND TI.DAY_BATCH_DATE='{3}' GROUP BY TI.BARCODE", daybatch.DAY_BATCH_DATE.ToString("yyyy-MM-dd"), report_PMNTSummaryFuel.BARCODE, daybatch.SITEID, daybatch.DAY_BATCH_DATE.ToString("yyyy-MM-dd"));
                        List<Report_PMNT1> vipqty = slavedb.SqlQueryInfo<Report_PMNT1>(sql2);
                        if (vipqty != null && vipqty.Count > 0)
                            report_PMNTSummaryFuel.VIPQTY = Math.Round(Convert.ToDouble(vipqty[0].VIPQTY), 2);
                        else
                            report_PMNTSummaryFuel.VIPQTY = 0;
                        string sql3 = string.Format("SELECT TI.BARCODE,SUM(TP.QTY) AS B2CQTY FROM  TILLITEM TI ,TILL T,TILLITEM_PMNT TP WHERE   T.TILLNUM = TI.TILLNUM AND TP.TILLNUM=TI.TILLNUM  AND TI.ISFUEL = 'T' AND T.DAY_BATCH_DATE='{0}' AND TP.PMNTEXTREF=15 AND TI.BARCODE={1} AND TI.SITEID={2}  AND TP.DAY_BATCH_DATE='{3}' GROUP BY TI.BARCODE", daybatch.DAY_BATCH_DATE.ToString("yyyy-MM-dd"), report_PMNTSummaryFuel.BARCODE, daybatch.SITEID, daybatch.DAY_BATCH_DATE.ToString("yyyy-MM-dd"));
                        List<Report_PMNT2> B2CQTY = slavedb.SqlQueryInfo<Report_PMNT2>(sql3);
                        if (B2CQTY != null && B2CQTY.Count > 0)
                            report_PMNTSummaryFuel.B2CQTY = Math.Round(Convert.ToDouble(B2CQTY[0].B2CQTY), 2);
                        else
                            report_PMNTSummaryFuel.B2CQTY = 0;
                        report_PMNTSummaryFuel.VIPQTY1 = report_PMNTSummaryFuel.VIPQTY - report_PMNTSummaryFuel.B2CQTY;
                        string sql4 = string.Format("SELECT TI.BARCODE,SUM(TP.QTY) AS B2BQTY FROM  TILLITEM TI ,TILL T,TILLITEM_PMNT TP WHERE   T.TILLNUM = TI.TILLNUM AND TP.TILLNUM=TI.TILLNUM  AND TI.ISFUEL = 'T' AND T.DAY_BATCH_DATE='{0}' AND TP.PMNTEXTREF=18 AND TI.BARCODE={1} AND TI.SITEID={2} AND TI.DAY_BATCH_DATE='{2}'  AND TI.DAY_BATCH_DATE='{3}' GROUP BY TI.BARCODE", daybatch.DAY_BATCH_DATE.ToString("yyyy-MM-dd"), report_PMNTSummaryFuel.BARCODE, daybatch.SITEID, daybatch.DAY_BATCH_DATE.ToString("yyyy-MM-dd"), daybatch.DAY_BATCH_DATE.ToString("yyyy-MM-dd"));
                        List<Report_PMNT3> B2BQTY = slavedb.SqlQueryInfo<Report_PMNT3>(sql4);
                        if (B2BQTY != null && B2BQTY.Count > 0)
                            report_PMNTSummaryFuel.B2BQTY = Math.Round(Convert.ToDouble(B2BQTY[0].B2BQTY), 2);
                        else
                            report_PMNTSummaryFuel.B2BQTY = 0;
                        string sql5 = string.Format("SELECT TI.BARCODE,SUM(TP.QTY) AS OneKeyQTY FROM  TILLITEM TI ,TILL T,TILLITEM_PMNT TP  WHERE   T.TILLNUM = TI.TILLNUM AND TP.TILLNUM=TI.TILLNUM  AND TI.ISFUEL = 'T' AND T.DAY_BATCH_DATE='{0}' AND TI.DAY_BATCH_DATE='{1}'  AND TP.DAY_BATCH_DATE='{2}' AND TI.BARCODE={3} AND TI.SITEID={4} AND  (TP.PMNT_NAME LIKE'%室外%' OR TP.PMNT_NAME LIKE'%小程序%') GROUP BY TI.BARCODE", daybatch.DAY_BATCH_DATE.ToString("yyyy-MM-dd"), daybatch.DAY_BATCH_DATE.ToString("yyyy-MM-dd"), daybatch.DAY_BATCH_DATE.ToString("yyyy-MM-dd"), report_PMNTSummaryFuel.BARCODE, daybatch.SITEID);
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
                    }
                }
                WriteLog("完成油品支付汇总生成，站点编码：" + SITEDAYBATCH[0].SITEID );
            }
            catch (Exception ex)
            {
                WriteLog("生成油品支付方式汇总错误：" + ex.Message);
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
                    string sql = string.Format("SELECT LEFT(TI.DEPT,4)AS DEPT1,SUM(TOTAL) AS QTY,SUM((T.TICKET)) AS DRYTIMES FROM  TILLITEM TI ,TILL T WHERE   T.TILLNUM = TI.TILLNUM AND TI.ISFUEL = 'F' AND T.DAY_BATCH_DATE='{0}' AND T.SITEID={1} AND TI.DAY_BATCH_DATE='{0}' GROUP BY DEPT1", daybatch.DAY_BATCH_DATE.ToString("yyyy-MM-dd"),daybatch.SITEID, daybatch.DAY_BATCH_DATE.ToString("yyyy-MM-dd"));
                    List <Report_SummaryDry> SummaryDry = slavedb.SqlQueryInfo<Report_SummaryDry>(sql);
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
                }
                WriteLog("完成非油品汇总生成，站点编码：" + SITEDAYBATCH[0].SITEID);
            }
            catch (Exception ex)
            {
                WriteLog("生成非油品汇总错误：" + ex.Message);
            }

        }

        /// <summary>
        /// 营业日更新字段处理
        /// </summary>
        /// <param name="till"></param>
        /// <param name="siteid"></param>
        /// <returns></returns>
        public SITEDAYBATCH UpdateDayBatchModel(SITEDAYBATCH till, string id, string siteid)
        {
            till.ID = id;
            till.SITEID = siteid;
            till.CREATEDATE = DateTime.Now;
            return till;
        }
        /// <summary>
        /// 营业日插入字段处理
        /// </summary>
        /// <param name="till"></param>
        /// <param name="siteid"></param>
        /// <returns></returns>
        public SITEDAYBATCH CreateDayBatchModel(SITEDAYBATCH till, string siteid)
        {
            till.ID = Guid.NewGuid().ToString();
            till.SITEID = siteid;
            till.CREATEDATE = DateTime.Now;
            return SetDAYBATCHColumn(till);

        }

        public SITEDAYBATCH SetDAYBATCHColumn(SITEDAYBATCH till)
        {
            var db = new SqlHelper();
            SITEINFO SITEINFO = db.QueryInfo<SITEINFO>(t => t.SITEID == till.SITEID);
            if (!string.IsNullOrEmpty(SITEINFO.SITENAME))
                till.SITENAME = SITEINFO.SITENAME;
            return till;
        }
        #endregion

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
