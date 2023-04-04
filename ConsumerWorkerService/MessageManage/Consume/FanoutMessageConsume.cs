using Common.RabbitMQ;
using ConsumerWorkerService;
using ConsumerWorkerService.MessageManage.Model;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using SqlSugar;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Message.Client.MessageManage.Consume
{
    public class FanoutMessageConsume : IMessageConsume
    {

        static IConfiguration configure = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory())
       .AddJsonFile("appsettings.json").Build();
        private static readonly string _token = configure["InvoiceSettings:token"];
        private static readonly string _url = configure["InvoiceSettings:URL"];
        IdWorker idworker = new IdWorker(1);

        public void Consume(string message)
        {
            LoggerHelper.WriteLog("获得消息：" + message);
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
                case "RepeatTill":
                    RepeatSaveTill(Result);
                    break;
            }
        }

        public void ExceptionConsume(string message)
        {
            LoggerHelper.WriteLog("处理异常消息：" + message);
            Console.WriteLine("获得MQ异常队列消息  ：" + message);
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
                    LoggerHelper.WriteLog("异常的班次信息不做处理");
                    break;
            }
        }

        public bool ExceptionPostInvoice(string mess)
        {
            var message = HttpManager.HttpPostAsync(_url, mess, "application / json", 30, null);

            LoggerHelper.WriteLog(mess);
            InvoiceReturnMsg msg = message.Result.ToObject<InvoiceReturnMsg>();
            if (msg.msg == "000000")
            {
                LoggerHelper.WriteLog("发票系统重新同步交易流水成功：");
                return true;
            }
            else
            {
                LoggerHelper.WriteLog("发票系统重新同步交易流水失败：     ");
                return false;
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
                ReportTILL.DEPT = tillitem.DEPT.Length > 4 ? tillitem.DEPT.Substring(0, 4) : "0";
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
                //2021-04-08修改STATUSTYPE需要取TILL表的字段
                foreach (var _till in TILL_LIST)
                {
                    if (ReportTILL.TILLNUM == _till.TILLNUM && ReportTILL.SITEID == _till.SITEID)
                    {
                        ReportTILL.STATUSTYPE = _till.STATUSTYPE;
                        ReportTILL.REMARK2 = _till.LINK_TILLNUM.ToString();
                    }
                }


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
            LoggerHelper.WriteLog("原始交易流水数据已入库!    " + "站点编码：" + Result.Data.Till[0].SITEID + "   交易号：" + Result.Data.Till[0].TILLNUM + "   GUID:" + Result.GUID);

            if (configure["TaskConfig:Invoiced"] == "true")
            {
                if (Result.Data.TILL_PMNT.Exists(t => t.PMNTEXTREF != null))
                {
                    WriteInvoiceJson1(Result.ToJson().ToString());
                }
                //  PostInvoice(Result);
            }
        }

        public void RepeatSaveTill(ResParameter Result)
        {
            var db = new SqlHelper();
            TILL tillEntitys = new TILL();
            List<TILL> TILL_LIST = new List<TILL>();
            List<TILLITEM> TILLITEM_LIST = new List<TILLITEM>();
            List<TILL_PMNT> TILL_PMNT_LIST = new List<TILL_PMNT>();
            List<TILL_PROMO> TILL_PROMO_LIST = new List<TILL_PROMO>();
            List<TILLITEM_PMNT> TILLITEM_PMNT_LIST = new List<TILLITEM_PMNT>();
            List<TILLITEM_PROMO> TILLITEM_PROMO_LIST = new List<TILLITEM_PROMO>();
            TILL_LIST = Result.Data.Till;
            //  db.BeginTran();
            foreach (var till in TILL_LIST)
            {
                string dd = till.DAY_BATCH_DATE.GetValueOrDefault().ToString("yyyy-MM-dd");

                TILL till1 = db.QueryInfo<TILL>(t => t.SITEID == Result.SiteId && t.TILLNUM == till.TILLNUM && t.DAY_BATCH_DATE.ToString() == dd);
                if (till1 != null)
                {

                    int i = db.DeleteInfo<TILL>(t => t.SITEID == till1.SITEID && t.TILLNUM == till1.TILLNUM && t.DAY_BATCH_DATE.ToString() == dd);
                    int j = db.DeleteInfo<Report_TILL>(t => t.SITEID == till1.SITEID && t.TILLNUM == till1.TILLNUM && t.DAY_BATCH_DATE.ToString() == dd);
                }
            }
            foreach (var till in TILL_LIST)
            {
                db.InsertInto<TILL>(CreateTILLModel(till, Result.SiteId));
            }
            TILLITEM_LIST = Result.Data.TILLITEM;
            foreach (var tillitem in TILLITEM_LIST)
            {
                string dd = tillitem.DAY_BATCH_DATE.GetValueOrDefault().ToString("yyyy-MM-dd");
                TILLITEM till1 = db.QueryInfo<TILLITEM>(t => t.SITEID == Result.SiteId && t.TILLNUM == tillitem.TILLNUM && t.DAY_BATCH_DATE.ToString() == dd);
                if (till1 != null)
                {

                    int i = db.DeleteInfo<TILLITEM>(t => t.SITEID == Result.SiteId && t.TILLNUM == tillitem.TILLNUM && t.DAY_BATCH_DATE.ToString() == dd);
                }
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
                string dd = tillpmnt.DAY_BATCH_DATE.GetValueOrDefault().ToString("yyyy-MM-dd");
                TILLITEM_PMNT till1 = db.QueryInfo<TILLITEM_PMNT>(t => t.SITEID == Result.SiteId && t.TILLNUM == tillpmnt.TILLNUM && t.DAY_BATCH_DATE.ToString() == dd);
                if (till1 != null)
                {

                    int i = db.DeleteInfo<TILLITEM_PMNT>(t => t.SITEID == Result.SiteId && t.TILLNUM == tillpmnt.TILLNUM && t.DAY_BATCH_DATE.ToString() == dd);
                }
                db.InsertInto<TILL_PMNT>(CreateTILL_PMNTModel(tillpmnt, Result.SiteId));
            }
            TILL_PROMO_LIST = Result.Data.TILL_PROMO;
            foreach (var tillpromo in TILL_PROMO_LIST)
            {
                string dd = tillpromo.DAY_BATCH_DATE.GetValueOrDefault().ToString("yyyy-MM-dd");
                TILL_PROMO till1 = db.QueryInfo<TILL_PROMO>(t => t.SITEID == Result.SiteId && t.TILLNUM == tillpromo.TILLNUM && t.DAY_BATCH_DATE.ToString() == dd);
                if (till1 != null)
                {

                    db.DeleteInfo<TILL_PROMO>(t => t.SITEID == Result.SiteId && t.TILLNUM == tillpromo.TILLNUM && t.DAY_BATCH_DATE.ToString() == dd);
                }
                db.InsertInto<TILL_PROMO>(CreateTILL_PROMOModel(tillpromo, Result.SiteId));
            }
            TILLITEM_PMNT_LIST = Result.Data.TILLITEM_PMNT;
            foreach (var tillitempmnt in TILLITEM_PMNT_LIST)
            {
                string dd = tillitempmnt.DAY_BATCH_DATE.GetValueOrDefault().ToString("yyyy-MM-dd");
                TILLITEM_PMNT till1 = db.QueryInfo<TILLITEM_PMNT>(t => t.SITEID == Result.SiteId && t.TILLNUM == tillitempmnt.TILLNUM && t.DAY_BATCH_DATE.ToString() == dd);
                if (till1 != null)
                {

                    int i = db.DeleteInfo<TILLITEM_PMNT>(t => t.SITEID == Result.SiteId && t.TILLNUM == tillitempmnt.TILLNUM && t.DAY_BATCH_DATE.ToString() == dd);
                }
                db.InsertInto<TILLITEM_PMNT>(CreateTILLITEM_PMNTModel(tillitempmnt, Result.SiteId));
            }
            TILLITEM_PROMO_LIST = Result.Data.TILLITEM_PROMO;
            foreach (var tillitempromo in TILLITEM_PROMO_LIST)
            {
                string dd = tillitempromo.DAY_BATCH_DATE.GetValueOrDefault().ToString("yyyy-MM-dd");
                TILLITEM_PROMO till1 = db.QueryInfo<TILLITEM_PROMO>(t => t.SITEID == Result.SiteId && t.TILLNUM == tillitempromo.TILLNUM && t.DAY_BATCH_DATE.ToString() == dd);
                if (till1 != null)
                {

                    int i = db.DeleteInfo<TILLITEM_PROMO>(t => t.SITEID == Result.SiteId && t.TILLNUM == tillitempromo.TILLNUM && t.DAY_BATCH_DATE.ToString() == dd);
                }

                db.InsertInto<TILLITEM_PROMO>(CreateTILLITEM_PROMOModel(tillitempromo, Result.SiteId));
            }
            //  db.CommitTran();
            LoggerHelper.WriteLog("原始交易流水数据已入库!    " + "站点编码：" + Result.Data.Till[0].SITEID + "   交易号：" + Result.Data.Till[0].TILLNUM + "   GUID:" + Result.GUID);

   

            if (configure["TaskConfig:Invoiced"] == "true")
            {
                if (Result.Data.TILL_PMNT.Exists(t => t.PMNTEXTREF != null))
                {
                    LoggerHelper.WriteLog("开始写发票文件！");
                    WriteInvoiceJson1(Result.ToJson().ToString());
                    LoggerHelper.WriteLog("结束写发票文件！");
                }
                //  PostInvoice(Result);
            }
        }
        /// <summary>
        /// /流水保存
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
        public static bool WriteInvoiceJson1(string message)
        {
            string path = AppContext.BaseDirectory + "ReadyInvoiceMsg";
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
                LoggerHelper.WriteLog(path);
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
        #region 
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
        #endregion 
        public void SaveSiteInfo(ResParameter Result)
        {
            //try
            //{
            var db = new SqlHelper();
            SITEINFO siteinfo = new SITEINFO();
            siteinfo.ID = Guid.NewGuid().ToString();
            siteinfo.SITEID = Result.Data.SiteID;
            siteinfo.SITENAME = Result.Data.SiteName;
            siteinfo.CREATEDATE = DateTime.Now;
            SiteInfo SiteInfo = new SiteInfo();
            SiteInfo.SiteID = siteinfo.SITEID;
            SiteInfo.SiteName = siteinfo.SITENAME;
            if (db.InsertInto<SITEINFO>(siteinfo))
            {
                LoggerHelper.WriteLog("新站点注册成功:" + siteinfo.SITEID + "    " + siteinfo.SITENAME);
                //var message = HttpManager.HttpPostAsync(_url + "SiteInfo", SiteInfo.ToJson(), "application / json", 30, null);
                //InvoiceReturnMsg msg = message.Result.ToObject<InvoiceReturnMsg>();
                //if (msg.msg == "000000")
                //{
                //    WriteLog("与发票系统油站信息主数据同步成功：");
                //}
                //else
                //{
                //    WriteLog("与发票系统油站信息主数据同步失败：     " + msg.result);
                //}
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
        }




        /// <summary>
        /// 汇总油品汇总表
        /// </summary>
        public void UpdateFuelSummaryReport(List<SITEDAYBATCH> SITEDAYBATCH)
        {
            var db = new SqlHelper();
            var slavedb = new SlaveSqlHelper();
            //try
            //{
            foreach (var daybatch in SITEDAYBATCH)
            {
                string sql1 = string.Format("SELECT TI.BARCODE,TI.ITEMNAME,SUM(SALEQTY) AS QTY,SUM((T.TICKET)) AS FUELTIMES FROM  TILLITEM TI ,TILL T WHERE   T.TILLNUM = TI.TILLNUM AND TI.ISFUEL = 'T' AND T.DAY_BATCH_DATE='{0}' AND T.SITEID='{1}'  AND TI.DAY_BATCH_DATE='{2}' GROUP BY TI.BARCODE", daybatch.DAY_BATCH_DATE.ToString("yyyy-MM-dd"), daybatch.SITEID, daybatch.DAY_BATCH_DATE.ToString("yyyy-MM-dd"));
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
                    string sql2 = string.Format("SELECT TI.BARCODE,SUM(SALEQTY) AS VIPQTY FROM  TILLITEM TI ,TILL T WHERE   T.TILLNUM = TI.TILLNUM AND TI.ISFUEL = 'T' AND T.DAY_BATCH_DATE='{0}' AND T.CUST_MAGKEY>0 AND TI.BARCODE={1} AND TI.SITEID='{2}'  AND TI.DAY_BATCH_DATE='{3}' GROUP BY TI.BARCODE", daybatch.DAY_BATCH_DATE.ToString("yyyy-MM-dd"), report_PMNTSummaryFuel.BARCODE, daybatch.SITEID, daybatch.DAY_BATCH_DATE.ToString("yyyy-MM-dd"));
                    List<Report_PMNT1> vipqty = slavedb.SqlQueryInfo<Report_PMNT1>(sql2);
                    if (vipqty != null && vipqty.Count > 0)
                        report_PMNTSummaryFuel.VIPQTY = Math.Round(Convert.ToDouble(vipqty[0].VIPQTY), 2);
                    else
                        report_PMNTSummaryFuel.VIPQTY = 0;
                    string sql3 = string.Format("SELECT TI.BARCODE,SUM(TP.QTY) AS B2CQTY FROM  TILLITEM TI ,TILL T,TILLITEM_PMNT TP WHERE   T.TILLNUM = TI.TILLNUM AND TP.TILLNUM=TI.TILLNUM  AND TI.ISFUEL = 'T' AND T.DAY_BATCH_DATE='{0}' AND TP.PMNTEXTREF=15 AND TI.BARCODE={1} AND TI.SITEID='{2}'  AND TP.DAY_BATCH_DATE='{3}' GROUP BY TI.BARCODE", daybatch.DAY_BATCH_DATE.ToString("yyyy-MM-dd"), report_PMNTSummaryFuel.BARCODE, daybatch.SITEID, daybatch.DAY_BATCH_DATE.ToString("yyyy-MM-dd"));
                    List<Report_PMNT2> B2CQTY = slavedb.SqlQueryInfo<Report_PMNT2>(sql3);
                    if (B2CQTY != null && B2CQTY.Count > 0)
                        report_PMNTSummaryFuel.B2CQTY = Math.Round(Convert.ToDouble(B2CQTY[0].B2CQTY), 2);
                    else
                        report_PMNTSummaryFuel.B2CQTY = 0;
                    report_PMNTSummaryFuel.VIPQTY1 = report_PMNTSummaryFuel.VIPQTY - report_PMNTSummaryFuel.B2CQTY;
                    string sql4 = string.Format("SELECT TI.BARCODE,SUM(TP.QTY) AS B2BQTY FROM  TILLITEM TI ,TILL T,TILLITEM_PMNT TP WHERE   T.TILLNUM = TI.TILLNUM AND TP.TILLNUM=TI.TILLNUM  AND TI.ISFUEL = 'T' AND T.DAY_BATCH_DATE='{0}' AND TP.PMNTEXTREF=18 AND TI.BARCODE={1} AND TI.SITEID='{2}' AND TI.DAY_BATCH_DATE='{2}'  AND TI.DAY_BATCH_DATE='{3}' GROUP BY TI.BARCODE", daybatch.DAY_BATCH_DATE.ToString("yyyy-MM-dd"), report_PMNTSummaryFuel.BARCODE, daybatch.SITEID, daybatch.DAY_BATCH_DATE.ToString("yyyy-MM-dd"), daybatch.DAY_BATCH_DATE.ToString("yyyy-MM-dd"));
                    List<Report_PMNT3> B2BQTY = slavedb.SqlQueryInfo<Report_PMNT3>(sql4);
                    if (B2BQTY != null && B2BQTY.Count > 0)
                        report_PMNTSummaryFuel.B2BQTY = Math.Round(Convert.ToDouble(B2BQTY[0].B2BQTY), 2);
                    else
                        report_PMNTSummaryFuel.B2BQTY = 0;
                    string sql5 = string.Format("SELECT TI.BARCODE,SUM(TP.QTY) AS OneKeyQTY FROM  TILLITEM TI ,TILL T,TILLITEM_PMNT TP  WHERE   T.TILLNUM = TI.TILLNUM AND TP.TILLNUM=TI.TILLNUM  AND TI.ISFUEL = 'T' AND T.DAY_BATCH_DATE='{0}' AND TI.DAY_BATCH_DATE='{1}'  AND TP.DAY_BATCH_DATE='{2}' AND TI.BARCODE={3} AND TI.SITEID='{4}' AND  (TP.PMNT_NAME LIKE'%室外%' OR TP.PMNT_NAME LIKE'%小程序%') GROUP BY TI.BARCODE", daybatch.DAY_BATCH_DATE.ToString("yyyy-MM-dd"), daybatch.DAY_BATCH_DATE.ToString("yyyy-MM-dd"), daybatch.DAY_BATCH_DATE.ToString("yyyy-MM-dd"), report_PMNTSummaryFuel.BARCODE, daybatch.SITEID);
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
            LoggerHelper.WriteLog("完成油品支付汇总生成，站点编码：" + SITEDAYBATCH[0].SITEID);
            //}
            //catch (Exception ex)
            //{
            //    WriteLog("生成油品支付方式汇总错误：" + ex.Message);
            //    throw ex;
            //}

        }




        /// <summary>
        /// 汇总非油品汇总表
        /// </summary>
        public void UpdateDrySummaryReport(List<SITEDAYBATCH> SITEDAYBATCH)
        {
            var db = new SqlHelper();
            var slavedb = new SlaveSqlHelper();
            //try
            //{
            foreach (var daybatch in SITEDAYBATCH)
            {
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
            }
            LoggerHelper.WriteLog("完成非油品汇总生成，站点编码：" + SITEDAYBATCH[0].SITEID);
            //}
            //catch (Exception ex)
            //{
            //    WriteLog("生成非油品汇总错误：" + ex.Message);
            //    throw ex;
            //}

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
            if (SITEINFO != null)
            {
                if (!string.IsNullOrEmpty(SITEINFO.SITENAME))
                    till.SITENAME = SITEINFO.SITENAME;
            }

            return till;
        }



        #endregion

        #region HB提交发票系统

        /// <summary>
        /// 发送到发票系统的条数
        /// </summary>
        public string ReadyInvoiceCount = "";
        public string InvoiceTillNumber = "";
        public bool PostInvoiceList(List<ResParameter> ResultList)
        {

            string InvoiceTill = GetInvoiceListTill(ResultList).ToJson();
            LoggerHelper.WriteInvoceLog(InvoiceTillNumber);
            LoggerHelper.WriteInvoceLog(InvoiceTill);
            try
            {
                var message = HttpManager.HttpPostAsync(_url, InvoiceTill, "application / json", 5000, null);
                InvoiceReturnMsg msg = message.Result.ToObject<InvoiceReturnMsg>();
                if (msg.msg == "000000")
                {
                    string datamsg = string.Format("发票系统同步交易流水全部成功,发送{0}条，成功{1}条", ReadyInvoiceCount, ReadyInvoiceCount);
                    LoggerHelper.WriteInvoceLog(datamsg);
                    return true;
                }
                else
                {
                    string datamsg = string.Format("发票系统同步交易流水失败,发送{0}条，失败{1}条", ReadyInvoiceCount, msg.result.data.Count);
                    LoggerHelper.WriteInvoceLog(datamsg);
                    SaveInvoiceErrorMsg(msg, InvoiceTill);
                    return false;
                }
            }
            catch (Exception ex)
            {
                #region"如异常则全部存入数据库"
                var db = new SqlHelper();
                db.BeginTran();
                InvoiceTill invoiceTill = InvoiceTill.ToObject<InvoiceTill>();
                foreach (var item in invoiceTill.SaleDataList)
                {
                    InvoiceErrorMsg invoiceErrorMsg = new InvoiceErrorMsg();
                    invoiceErrorMsg.ID = Guid.NewGuid().ToString();
                    invoiceErrorMsg.SITEID = item.Site_Number;
                    invoiceErrorMsg.SITENAME = item.Site_Number;
                    invoiceErrorMsg.POSTDATE = Convert.ToDateTime((item.Date + " " + item.Time));
                    invoiceErrorMsg.REQUESTID = item.Transaction_ID;
                    invoiceErrorMsg.TILLNUM = Convert.ToInt32(item.POS_Transaction_Number);
                    invoiceErrorMsg.CREATEDATE = DateTime.Now;
                    invoiceErrorMsg.STATUS = 0;
                    invoiceErrorMsg.SENDDATA = ConvertJsonString(item.ToJson());
                    invoiceErrorMsg.MESSAGE = "";
                    db.InsertInto<InvoiceErrorMsg>(invoiceErrorMsg);
                }
                db.CommitTran();
                LoggerHelper.WriteInvoceLog("程序报错后，待发送的交易流水全部存入数据库，条数：" + ReadyInvoiceCount);
                LoggerHelper.WriteInvoceLog("发送发票消息失败：" + ex.Message);
                #endregion"
                return false;
            }
        }



        /// <summary>
        /// 2022-01-07 发到发票错误的数据存入数据库
        /// </summary>
        /// <param name="msg"></param>
        /// <returns></returns>
        public void SaveInvoiceErrorMsg(InvoiceReturnMsg msg, string InvoiceTill)
        {
            InvoiceTill invoiceTill = InvoiceTill.ToObject<InvoiceTill>();
            var db = new SqlHelper();
            db.BeginTran();

            foreach (var saledata in msg.result.data)
            {
                SaleDataList SaleData = invoiceTill.SaleDataList.Find(e => e.Site_Number == saledata.site_number && e.POS_Transaction_Number == saledata.pos_transaction_number && e.Time == saledata.time);
                InvoiceErrorMsg invoiceErrorMsg = new InvoiceErrorMsg();
                invoiceErrorMsg.ID = Guid.NewGuid().ToString();
                invoiceErrorMsg.SITEID = SaleData.Site_Number;
                invoiceErrorMsg.SITENAME = SaleData.Site_Number;
                invoiceErrorMsg.POSTDATE = Convert.ToDateTime((SaleData.Date + " " + SaleData.Time));
                invoiceErrorMsg.REQUESTID = saledata.transaction_id;
                invoiceErrorMsg.TILLNUM = Convert.ToInt32(saledata.pos_transaction_number);
                invoiceErrorMsg.CREATEDATE = DateTime.Now;
                invoiceErrorMsg.STATUS = 0;
                invoiceErrorMsg.SENDDATA = ConvertJsonString(SaleData.ToJson());
                invoiceErrorMsg.MESSAGE = saledata.message;
                db.InsertInto<InvoiceErrorMsg>(invoiceErrorMsg);
            }
            db.CommitTran();

            LoggerHelper.WriteInvoceLog("完成发送发票失败的交易流水存入数据库，条数：" + msg.result.data.Count);
        }


        public InvoiceTill GetInvoiceTill(ResParameter Result)
        {
            InvoiceTill InvoiceTill = new InvoiceTill();
            InvoiceTill.Pztoken = _token;
            InvoiceTill.SaleDataList = new List<SaleDataList>();
            foreach (var till in Result.Data.Till)
            {
                SaleDataList saleData = new SaleDataList();
                saleData.Transaction_ID = idworker.nextId().ToString();
                saleData.Transaction_Type = till.STATUSTYPE.ToString();
                saleData.Date = till.POSTED.GetValueOrDefault().ToString("yyyy-MM-dd");
                saleData.Time = till.POSTED.GetValueOrDefault().ToString("hh:mm");
                saleData.POS_ID = Result.Data.TILLITEM.FirstOrDefault(t => t.TILLNUM == till.TILLNUM).POS_ID.ToString();
                saleData.Cashier_Name = till.WTR;
                saleData.Site_Number = till.SITEID;
                saleData.Site_Name = "测试加油站";
                saleData.POS_Transaction_Number = till.TILLNUM.ToString();
                saleData.Product_Count = Result.Data.TILLITEM.Where(t => t.TILLNUM == till.TILLNUM).ToList().Count.ToString();
                saleData.Payment_Count = Result.Data.TILL_PMNT.Where(t => t.TILLNUM == till.TILLNUM).ToList().Count.ToString();
                saleData.Payment_Total_Paid = Math.Round(till.SSUM.GetValueOrDefault(), 2).ToString();
                if (till.CUST_MAGKEY == "")
                    saleData.Customer_Code = "1";
                else
                    saleData.Customer_Code = till.CUST_MAGKEY;
                saleData.Product_Group = new Product_Group();
                saleData.Product_Group.Product = new List<Product>();
                foreach (var tillitem in Result.Data.TILLITEM.Where(t => t.TILLNUM == till.TILLNUM).ToList())
                {
                    Product Product = new Product();
                    Product.Product_Type = tillitem.DEPT.Substring(0, 4);
                    if (tillitem.UNITNAME.Trim() == "公升")
                        tillitem.UNITNAME = "升";
                    Product.UOM = tillitem.UNITNAME;
                    Product.Product_Name = tillitem.ITEMNAME;
                    Product.Product_Code = tillitem.BARCODE;
                    Product.Unit_Price = tillitem.STDPRICE.ToString();
                    Product.Amount = tillitem.SALEQTY.ToString();
                    Product.Total_Price = tillitem.TOTAL.ToString();
                    Product.Discount = tillitem.DISCOUNT.ToString();
                    if (tillitem.ISFUEL == "T")
                    {
                        Product.ISFuel = "1";
                        Product.Total_Price = tillitem.PUMP_TOTAL.ToString();
                        Product.Discount = Math.Round(Convert.ToDouble(tillitem.PUMP_TOTAL - tillitem.TOTAL), 2).ToString();
                    }
                    else
                    {
                        Product.ISFuel = "0";
                        Product.Total_Price = Math.Round(Convert.ToDouble(tillitem.STDPRICE * tillitem.SALEQTY), 2).ToString();
                        Product.Discount = Math.Round(Convert.ToDouble((tillitem.STDPRICE * tillitem.SALEQTY) - tillitem.TOTAL), 2).ToString();
                    }
                    Product.Pump_No = tillitem.PUMP_ID.ToString();
                    saleData.Product_Group.Product.Add(Product);
                }
                saleData.Payment_Group = new Payment_Group();
                saleData.Payment_Group.Payment = new List<Payment>();
                foreach (var tillpmnt in Result.Data.TILL_PMNT.Where(t => t.TILLNUM == till.TILLNUM).ToList())
                {
                    Payment Payment = new Payment();
                    Payment.Payment_Method = tillpmnt.PMNTEXTREF;
                    Payment.Payment_Amount = tillpmnt.SSUM.ToString();
                    saleData.Payment_Group.Payment.Add(Payment);
                }
                InvoiceTill.SaleDataList.Add(saleData);
            }
            return InvoiceTill;
        }

        public InvoiceTill GetInvoiceListTill(List<ResParameter> ResultList)
        {

            InvoiceTill InvoiceTill = new InvoiceTill();
            InvoiceTill.Pztoken = _token;
            InvoiceTill.SaleDataList = new List<SaleDataList>();
            foreach (var Result in ResultList)
            {
                foreach (var till in Result.Data.Till)
                {
                    SaleDataList saleData = new SaleDataList();
                    saleData.Transaction_ID = idworker.nextId().ToString();
                    saleData.Transaction_Type = till.STATUSTYPE.ToString();
                    saleData.Date = till.POSTED.GetValueOrDefault().ToString("yyyy-MM-dd");
                    saleData.Time = till.POSTED.GetValueOrDefault().ToString("hh:mm");
                    saleData.POS_ID = Result.Data.TILLITEM.FirstOrDefault(t => t.TILLNUM == till.TILLNUM).POS_ID.ToString();
                    saleData.Cashier_Name = till.WTR;
                    saleData.Site_Number = till.SITEID;
                    saleData.Site_Name = "测试加油站";
                    saleData.POS_Transaction_Number = till.TILLNUM.ToString();
                    saleData.Product_Count = Result.Data.TILLITEM.Where(t => t.TILLNUM == till.TILLNUM).ToList().Count.ToString();
                    saleData.Payment_Count = Result.Data.TILL_PMNT.Where(t => t.TILLNUM == till.TILLNUM).ToList().Count.ToString();
                    saleData.Payment_Total_Paid = Math.Round(till.SSUM.GetValueOrDefault(), 2).ToString();
                    if (till.CUST_MAGKEY == "")
                        saleData.Customer_Code = "1";
                    else
                        saleData.Customer_Code = till.CUST_MAGKEY;
                    saleData.Product_Group = new Product_Group();
                    saleData.Product_Group.Product = new List<Product>();
                    foreach (var tillitem in Result.Data.TILLITEM.Where(t => t.TILLNUM == till.TILLNUM).ToList())
                    {
                        Product Product = new Product();
                        Product.Product_Type = tillitem.DEPT.Substring(0, 4);
                        if (tillitem.UNITNAME.Trim() == "公升")
                            tillitem.UNITNAME = "升";
                        Product.UOM = tillitem.UNITNAME;
                        Product.Product_Name = tillitem.ITEMNAME;
                        Product.Product_Code = tillitem.BARCODE;
                        Product.Unit_Price = tillitem.STDPRICE.ToString();
                        Product.Amount = tillitem.SALEQTY.ToString();
                        Product.Total_Price = tillitem.TOTAL.ToString();
                        Product.Discount = tillitem.DISCOUNT.ToString();
                        if (tillitem.ISFUEL == "T")
                        {
                            Product.ISFuel = "1";
                            Product.Total_Price = tillitem.PUMP_TOTAL.ToString();
                            Product.Discount = Math.Round(Convert.ToDouble(tillitem.PUMP_TOTAL - tillitem.TOTAL), 2).ToString();
                        }
                        else
                        {
                            Product.ISFuel = "0";
                            Product.Total_Price = Math.Round(Convert.ToDouble(tillitem.STDPRICE * tillitem.SALEQTY), 2).ToString();
                            Product.Discount = Math.Round(Convert.ToDouble((tillitem.STDPRICE * tillitem.SALEQTY) - tillitem.TOTAL), 2).ToString();
                        }
                        Product.Pump_No = tillitem.PUMP_ID.ToString();
                        saleData.Product_Group.Product.Add(Product);
                    }
                    saleData.Payment_Group = new Payment_Group();
                    saleData.Payment_Group.Payment = new List<Payment>();
                    foreach (var tillpmnt in Result.Data.TILL_PMNT.Where(t => t.TILLNUM == till.TILLNUM).ToList())
                    {
                        Payment Payment = new Payment();
                        Payment.Payment_Method = tillpmnt.PMNTEXTREF;
                        Payment.Site_Payment_Method = tillpmnt.PMNTID.ToString();
                        Payment.Payment_Amount = tillpmnt.SSUM.ToString();
                        saleData.Payment_Group.Payment.Add(Payment);
                    }
                    InvoiceTill.SaleDataList.Add(saleData);
                }
                InvoiceTillNumber = InvoiceTillNumber + " ||站点编码：" + Result.Data.Till[0].SITEID + "交易号：" + Result.Data.Till[0].TILLNUM;
            }
            ReadyInvoiceCount = InvoiceTill.SaleDataList.Count.ToString();
            LoggerHelper.WriteInvoceLog("完成发票系统批量交易流水组装！  交易流水条数：" + ReadyInvoiceCount);
            return InvoiceTill;
        }
        #endregion



        private string ConvertJsonString(string str)
        {
            try
            {
                //格式化json字符串
                JsonSerializer serializer = new JsonSerializer();
                TextReader tr = new StringReader(str);
                JsonTextReader jtr = new JsonTextReader(tr);
                object obj = serializer.Deserialize(jtr);
                if (obj != null)
                {
                    StringWriter textWriter = new StringWriter();
                    JsonTextWriter jsonWriter = new JsonTextWriter(textWriter)
                    {
                        Formatting = Formatting.Indented,
                        Indentation = 4,
                        IndentChar = ' '
                    };
                    serializer.Serialize(jsonWriter, obj);
                    return textWriter.ToString();
                }

                return str;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

    }
}
