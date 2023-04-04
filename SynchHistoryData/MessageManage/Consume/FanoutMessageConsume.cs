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


        public void Consume(string message)
        {
            LoggerHelper.WriteLog("获得消息：" + message);
            Console.WriteLine("Get the message:  " + message);
            ResParameter186 res = message.ToJson().ToString().ToObject<ResParameter186>();
            switch (res.Type)
            {
                case "ReportTILL186":
                    SaveReportTILL(res);
                    break;
                case "TILLITEM_PMNT186":
                    SaveTILLITEMPMNT(res);
                    break;
                case "SITEDAYBATCH":
                    break;
                case "RepeatTill":
                    break;
            }
        }
        public void SaveReportTILL(ResParameter186 res)
        {
            // string dd = res.Data.Replace("\\","");
            List<Report_TILL> ListReport_TILL = res.Data.ToString().ToList<Report_TILL>();
            var db = new SqlHelper();
            db.BeginTran();
            foreach (var item in ListReport_TILL)
            {
                bos_transaction bos_Transaction = new bos_transaction();
                bos_Transaction.ID = Guid.NewGuid().ToString();
                bos_Transaction.SITEID = item.SITEID;
                bos_Transaction.SITENAME = item.SITENAME;
                bos_Transaction.POS_ID = item.POS_ID;
                bos_Transaction.WTR = item.WTR;
                bos_Transaction.TILLNUM = item.TILLNUM;
                bos_Transaction.POSTED = item.POSTED;
                bos_Transaction.DAY_BATCH_DATE = item.DAY_BATCH_DATE;
                bos_Transaction.BARCODE = item.BARCODE;
                bos_Transaction.DEPT = item.DEPT;
                bos_Transaction.ITEMNAME = item.ITEMNAME;
                bos_Transaction.STDPRICE = item.STDPRICE;
                bos_Transaction.SALEQTY = item.SALEQTY;
                bos_Transaction.RETAIL_AMOUNT = item.RETAIL_AMOUNT;
                bos_Transaction.DISCOUNT_AMOUNT = item.DISCOUNT_AMOUNT;
                bos_Transaction.TOTAL = item.TOTAL;
                bos_Transaction.STATUSTYPE = item.STATUSTYPE;
                bos_Transaction.REMARK1 = item.REMARK1;
                bos_Transaction.REMARK2 = item.REMARK2;
                bos_Transaction.CUST_MAGKEY = item.CUST_MAGKEY;
                bos_Transaction.ISVIP = item.ISVIP;
                bos_Transaction.CUST_MAGKEY = item.CUST_MAGKEY;
                bos_Transaction.ISVIP = item.ISVIP;
                bos_Transaction.RECEIVEDATE = DateTime.Now;
                bos_Transaction.CREATEDATE = item.CREATEDATE;
                db.InsertInto<bos_transaction>(bos_Transaction);
                LoggerHelper.WriteLog("交易明细数据接收成功：" + bos_Transaction.SITEID + "===" + bos_Transaction.TILLNUM + "===" + bos_Transaction.DAY_BATCH_DATE);
            }
            db.CommitTran();
        }

        public void SaveTILLITEMPMNT(ResParameter186 res)
        {
            List<TILLITEM_PMNT> ListTILLITEM_PMNT = res.Data.ToString().ToList<TILLITEM_PMNT>();
            var db = new SqlHelper();
            db.BeginTran();
            foreach (var item in ListTILLITEM_PMNT)
            {
                bos_payment bos_Payment = new bos_payment();
                bos_Payment.ID= Guid.NewGuid().ToString();
                bos_Payment.TILLNUM = item.TILLNUM;
                bos_Payment.SITEID = item.SITEID;
                bos_Payment.POSTED = item.POSTED;
                bos_Payment.DAY_BATCH_DATE = item.DAY_BATCH_DATE;
                bos_Payment.POS_ID = item.POS_ID;
                bos_Payment.BARCODE = item.BARCODE;
                bos_Payment.ITEMID = item.ITEMID;
                bos_Payment.DEPT = item.DEPT;
                bos_Payment.ITEMNAME = item.ITEMNAME;
                bos_Payment.PMNTEXTREF = item.PMNTEXTREF;
                bos_Payment.PMNTID = item.PMNTID;
                bos_Payment.PMNT_NAME = item.PMNT_NAME;
                bos_Payment.TOTAL = item.TOTAL;
                bos_Payment.QTY = item.QTY;
                bos_Payment.RECEIVEDATE = DateTime.Now;
                bos_Payment.CREATEDATE = item.CREATEDATE;
                bos_Payment.REMARK1 = item.REMARK1;
                bos_Payment.REMARK2 = item.REMARK2;
                db.InsertInto<bos_payment>(bos_Payment);
                LoggerHelper.WriteLog("支付明细数据接收成功：" + bos_Payment.SITEID + "===" + bos_Payment.TILLNUM + "===" + bos_Payment.DAY_BATCH_DATE);
            }
            db.CommitTran();
        }
    }
}
