using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Threading;
using System.Threading.Tasks;
using System.IO;
using ConsumerWorkerService.MessageManage.Model;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using System.Linq;
using Common.RabbitMQ;

namespace ConsumerWorkerService
{
    public class SynchroData : BackgroundService
    {
        public ILogger _logger;
        public SynchroData(ILogger<HandInvoiceErrorMsg> logger)
        {
            this._logger = logger;
        }
        static IConfiguration configure = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory())
.AddJsonFile("appsettings.json").Build();
        private static readonly string _token = configure["InvoiceSettings:token"];
        private static readonly string _url = configure["InvoiceSettings:URL"];
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            try
            {
                _logger.LogInformation(DateTime.Now.ToString() + "186数据同步服务启动！");
                while (!stoppingToken.IsCancellationRequested)
                {
                    //HandPMNTDATA();
                   SynchTILLITEM_PMNT();
                    SynchReport_TILL();
                    await Task.Delay(1000 * 60 * 10, stoppingToken);
                }
                LoggerHelper.WriteInvoceLog("186数据同步服务启动：启动");
            }
            catch (Exception ex)
            {
                if (!stoppingToken.IsCancellationRequested)
                {
                    _logger.LogInformation(DateTime.Now.ToString() + "186数据同步服务启动：异常" + ex.Message + ex.StackTrace);
                    LoggerHelper.WriteInvoceLog("186数据同步服务启动：异常" + ex.Message + ex.StackTrace);
                }
                else
                {
                    _logger.LogInformation(DateTime.Now.ToString() + "历史数据同步：停止");
                    LoggerHelper.WriteInvoceLog("186数据同步服务启动：停止");
                }
            }
        }


        private static void SynchReport_TILL()
        {
            LoggerHelper.WriteISynchLog("186定时任务===开始查询前3天内未同步的交易明细数据！");
            var db = new SqlHelper();
            string sql = string.Format("SELECT * FROM Report_TILL WHERE  ISSEND IS NULL AND DAY_BATCH_DATE> date_sub(curdate(),INTERVAL 3 day)  ");
            //本地测试
            // string sql = string.Format("SELECT * FROM Report_TILL WHERE DAY_BATCH_DATE>'2021-12-31' AND ISSEND IS NULL AND DAY_BATCH_DATE> date_sub(curdate(),INTERVAL 3 day) LIMIT 1000 ");

            List<Report_TILL> LIstReportTILL = db.SqlQueryInfo<Report_TILL>(sql);
            string logText3 = string.Format("{0} .........  {1}.........条数：{2}", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss:ffff"), "186定时任务===结束查询前3天内未同步的交易明细数据！", LIstReportTILL.Count);
            LoggerHelper.WriteISynchLog(logText3);
            if (LIstReportTILL.Count > 50)
            {
                int numb = 50;
                int size = (int)Math.Ceiling((decimal)LIstReportTILL.Count / numb);
                for (int i = 1; i <= size; i++)
                {
                    List<Report_TILL> EndResultList = LIstReportTILL.Skip((i - 1) * numb).Take(numb).ToList();
                    PushMsg pushMsg = new PushMsg();
                    pushMsg.sendMsg = EndResultList.ToJson();
                    pushMsg.sendEnum = SendEnum.订阅模式;
                    pushMsg.exchangeName = "186exchange";
                    pushMsg.routeName = "186route";
                    var jsonData = EndResultList.ToJson();
                    if (RabbitMQManage.PushMessage(pushMsg, "186ServerQueen", SendRes("ReportTILL186", EndResultList.ToJson())))
                        UPDATEISSEND(EndResultList);
                }
            }
            else
            {
                List<Report_TILL> EndResultList = LIstReportTILL;
                PushMsg pushMsg = new PushMsg();
                pushMsg.sendMsg = EndResultList.ToJson();
                pushMsg.sendEnum = SendEnum.订阅模式;
                pushMsg.exchangeName = "186exchange";
                pushMsg.routeName = "186route";
                var jsonData = EndResultList.ToJson();
                if (RabbitMQManage.PushMessage(pushMsg, "186ServerQueen", SendRes("ReportTILL186", EndResultList.ToJson())))
                    UPDATEISSEND(EndResultList);
            }
        }
        private static void UPDATEISSEND(List<Report_TILL> EndResultList)
        {
            var db = new SqlHelper();
            foreach (var item in EndResultList)
            {
                item.ISSEND = 1;
                int i = db.UpdateInfo<Report_TILL>(item);
                if (i > 0)
                {
                    string txt = string.Format("186定时任务===交易明细数据发送成功，===站点编码{0}===交易号{1}===业务日期{2}", item.SITEID, item.TILLNUM, item.DAY_BATCH_DATE);
                    LoggerHelper.WriteISynchLog(txt);
                }
            }
        }


        public static string SendRes(string type, string data)
        {
            ResParameter186 res = new ResParameter186 { GUID = Guid.NewGuid().ToString(), Type = type, SendDateTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss:fff"), Data = data };
            return res.ToJson();
        }



        public static void SynchTILLITEM_PMNT()
        {
            LoggerHelper.WriteISynchLog("186定时任务===开始查询前3天内未同步的支付明细数据！！");
            var db = new SqlHelper();
            string sql = string.Format("SELECT * FROM TILLITEM_PMNT WHERE ISSEND IS NULL AND DAY_BATCH_DATE> date_sub(curdate(),INTERVAL 3 day) ");
            //本地测试
            //string sql = string.Format("SELECT * FROM TILLITEM_PMNT WHERE DAY_BATCH_DATE>'2021-12-31' AND ISSEND IS NULL LIMIT 10");
            List<TILLITEM_PMNT> LIstTILLITEMPMNT = db.SqlQueryInfo<TILLITEM_PMNT>(sql);
            string logText3 = string.Format("{0} .........  {1}.........条数：{2}", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss:ffff"), "186定时任务===结束查询3天内未同步的支付明细数据！", LIstTILLITEMPMNT.Count);
            LoggerHelper.WriteISynchLog(logText3);
          

            if (LIstTILLITEMPMNT.Count > 50)
            {
                int numb = 50;
                int size = (int)Math.Ceiling((decimal)LIstTILLITEMPMNT.Count / numb);
                for (int i = 1; i <= size; i++)
                {
                    List<TILLITEM_PMNT> EndResultList1 = LIstTILLITEMPMNT.Skip((i - 1) * numb).Take(numb).ToList();
                    List<TILLITEM_PMNT> EndResultList = HandLIstTILLITEMPMNT(EndResultList1);
                    PushMsg pushMsg = new PushMsg();
                    pushMsg.sendMsg = EndResultList.ToJson();
                    pushMsg.sendEnum = SendEnum.订阅模式;
                    pushMsg.exchangeName = "186exchange";
                    pushMsg.routeName = "186route";
                    var jsonData = EndResultList.ToJson();
                    if (RabbitMQManage.PushMessage(pushMsg, "186ServerQueen", SendRes("TILLITEM_PMNT186", EndResultList.ToJson())))
                        UPDATEISSEND(EndResultList);
                }
            }
            else
            {
                List<TILLITEM_PMNT> EndResultList1 = LIstTILLITEMPMNT;
                List<TILLITEM_PMNT> EndResultList = HandLIstTILLITEMPMNT(EndResultList1);
                PushMsg pushMsg = new PushMsg();
                pushMsg.sendMsg = EndResultList.ToJson();
                pushMsg.sendEnum = SendEnum.订阅模式;
                pushMsg.exchangeName = "186exchange";
                pushMsg.routeName = "186route";
                var jsonData = EndResultList.ToJson();
                if (RabbitMQManage.PushMessage(pushMsg, "186ServerQueen", SendRes("TILLITEM_PMNT186", EndResultList.ToJson())))
                    UPDATEISSEND(EndResultList);
            }
        }
        /// <summary>
        /// 补全处理支付明细表中未保存的字段POSID,PMNTID
        /// </summary>
        /// <param name=""></param>
        public static List<TILLITEM_PMNT> HandLIstTILLITEMPMNT(List<TILLITEM_PMNT> LIstTILLITEMPMNT)
        {
            LoggerHelper.WriteISynchLog("186定时任务===开始补全处理支付明细表中未保存的字段POSID,PMNTID！,条数：" + LIstTILLITEMPMNT.Count);
            var db = new SqlHelper();
            List<TILLITEM_PMNT> newlist = new List<TILLITEM_PMNT>();
            foreach (var item in LIstTILLITEMPMNT)
            {
                string dd = item.DAY_BATCH_DATE.GetValueOrDefault().ToString("yyyy-MM-dd");
                TILLITEM till1 = db.QueryInfo<TILLITEM>(t => t.SITEID == item.SITEID && t.TILLNUM == item.TILLNUM && t.DAY_BATCH_DATE.ToString() == dd);
                if (till1 != null)
                { item.POS_ID = till1.POS_ID; }
                else
                {
                    string txt1 = string.Format("186定时任务===186支付明细数据补全失败，TILL无数据===站点编码{0}===交易号{1}===业务日期{2}", item.SITEID, item.TILLNUM, item.DAY_BATCH_DATE);
                    LoggerHelper.WriteISynchLog(txt1);
                }
                TILL_PMNT TILLPMNT = db.QueryInfo<TILL_PMNT>(t => t.SITEID == item.SITEID && t.PMNT_NAME == item.PMNT_NAME && t.TILLNUM == item.TILLNUM && t.DAY_BATCH_DATE.ToString() == dd);
                if (TILLPMNT != null)
                { item.PMNTID = TILLPMNT.PMNTID; }
                else
                {
                    string txt1 = string.Format("186定时任务===186支付明细数据补全失败，TILL_PMNT无数据===站点编码{0}===交易号{1}===业务日期{2}", item.SITEID, item.TILLNUM, item.DAY_BATCH_DATE);
                    LoggerHelper.WriteISynchLog(txt1);
                }
                int i = db.UpdateInfo<TILLITEM_PMNT>(item);
                if (i > 0)
                {
                    string txt = string.Format("186定时任务===186支付明细数据补全成功，===站点编码{0}===交易号{1}===业务日期{2}", item.SITEID, item.TILLNUM, item.DAY_BATCH_DATE);
                    LoggerHelper.WriteISynchLog(txt);
                }
                newlist.Add(item);
            }
            LoggerHelper.WriteISynchLog("186定时任务===完成==补全处理支付明细表中未保存的字段POSID,PMNTID！,条数：" + LIstTILLITEMPMNT.Count);
            return newlist;
        }



        private static void UPDATEISSEND(List<TILLITEM_PMNT> EndResultList)
        {
            var db = new SqlHelper();
            foreach (var item in EndResultList)
            {
                item.ISSEND = 1;
                int i = db.UpdateInfo<TILLITEM_PMNT>(item);
                if (i > 0)
                {
                    string txt = string.Format("186定时任务===186支付明细数据发送成功，===站点编码{0}===交易号{1}===业务日期{2}", item.SITEID, item.TILLNUM, item.DAY_BATCH_DATE);
                    LoggerHelper.WriteISynchLog(txt);
                }
            }
        }
    }
}
