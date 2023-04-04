﻿using Common.RabbitMQ;
using ConsumerWorkerService.MessageManage.Model;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;

namespace ConsumerWorkerService.Class
{
    public static partial class SynchHistoryData
    {

        #region 同步2022年1月份到当前时间点的交易明细
        /// <summary>
        /// 同步2022年1月份到当前时间点的交易明细
        /// </summary>
        /// 

        public static void SynchReport_TILL()
        {
            LoggerHelper.WriteISynchLog("开始查询2022年1月到当前时间的历史交易明细数据！");
            var db = new SqlHelper();
            string sql = string.Format("SELECT * FROM Report_TILL WHERE DAY_BATCH_DATE>'2021-12-31' AND ISSEND IS NULL ");
            //本地测试
            //string sql = string.Format("SELECT * FROM Report_TILL WHERE DAY_BATCH_DATE>'2021-12-31' AND ISSEND IS NULL LIMIT 1000");
            List<Report_TILL> LIstReportTILL = db.SqlQueryInfo<Report_TILL>(sql);
            string logText3 = string.Format("{0} .........  {1}.........条数：{2}", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss:ffff"), "结束查询历史交易明细数据！", LIstReportTILL.Count);
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
                    string txt = string.Format("186交易明细数据发送成功，===站点编码{0}===交易号{1}===业务日期{2}", item.SITEID, item.TILLNUM, item.DAY_BATCH_DATE);
                    LoggerHelper.WriteISynchLog(txt);
                }
            }
        }

        #endregion


        #region 同步2022年1月份到当前时间点的支付明细
        public static void SynchTILLITEM_PMNT()
        {
            LoggerHelper.WriteISynchLog("开始查询2022年1月到当前时间的历史支付明细数据！");
            var db = new SqlHelper();
            string sql = string.Format("SELECT * FROM TILLITEM_PMNT WHERE DAY_BATCH_DATE>'2021-12-31' AND ISSEND IS NULL ");
            //本地测试
           // string sql = string.Format("SELECT * FROM TILLITEM_PMNT WHERE DAY_BATCH_DATE>'2021-12-31' AND ISSEND IS NULL LIMIT 10");
            List<TILLITEM_PMNT> LIstTILLITEMPMNT = db.SqlQueryInfo<TILLITEM_PMNT>(sql);
            string logText3 = string.Format("{0} .........  {1}.........条数：{2}", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss:ffff"), "结束查询历史交易明细数据！", LIstTILLITEMPMNT.Count);
            LoggerHelper.WriteISynchLog(logText3);
            if (LIstTILLITEMPMNT.Count > 50)
            {
                int numb = 50;
                int size = (int)Math.Ceiling((decimal)LIstTILLITEMPMNT.Count / numb);
                for (int i = 1; i <= size; i++)
                {
                    List<TILLITEM_PMNT> EndResultList1 = LIstTILLITEMPMNT.Skip((i - 1) * numb).Take(numb).ToList();
                    List<TILLITEM_PMNT>  EndResultList = HandLIstTILLITEMPMNT(EndResultList1);
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
                List<TILLITEM_PMNT> EndResultList = HandLIstTILLITEMPMNT(EndResultList1); ;
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
        public static List<TILLITEM_PMNT>  HandLIstTILLITEMPMNT(List<TILLITEM_PMNT> LIstTILLITEMPMNT)
        {
            LoggerHelper.WriteISynchLog("补全处理支付明细表中未保存的字段POSID,PMNTID！,条数："+ LIstTILLITEMPMNT.Count);
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
                    string txt1 = string.Format("186支付明细数据补全失败，TILL无数据===站点编码{0}===交易号{1}===业务日期{2}", item.SITEID, item.TILLNUM, item.DAY_BATCH_DATE);
                    LoggerHelper.WriteISynchLog(txt1);
                }
                TILL_PMNT TILLPMNT = db.QueryInfo<TILL_PMNT>(t => t.SITEID == item.SITEID &&  t.PMNT_NAME == item.PMNT_NAME && t.TILLNUM == item.TILLNUM && t.DAY_BATCH_DATE.ToString() == dd);
                if (TILLPMNT != null)
                { item.PMNTID = TILLPMNT.PMNTID; }
                else
                {
                    string txt1 = string.Format("186支付明细数据补全失败，TILL_PMNT无数据===站点编码{0}===交易号{1}===业务日期{2}", item.SITEID, item.TILLNUM, item.DAY_BATCH_DATE);
                    LoggerHelper.WriteISynchLog(txt1);
                }
                int i = db.UpdateInfo<TILLITEM_PMNT>(item);
                if (i > 0)
                {
                    string txt = string.Format("186支付明细数据补全成功，===站点编码{0}===交易号{1}===业务日期{2}", item.SITEID, item.TILLNUM, item.DAY_BATCH_DATE);
                    LoggerHelper.WriteISynchLog(txt);
                }
                newlist.Add(item);
            }
            LoggerHelper.WriteISynchLog("完成==补全处理支付明细表中未保存的字段POSID,PMNTID！,条数：" + LIstTILLITEMPMNT.Count);
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
                    string txt = string.Format("186支付明细数据发送成功，===站点编码{0}===交易号{1}===业务日期{2}", item.SITEID, item.TILLNUM, item.DAY_BATCH_DATE);
                    LoggerHelper.WriteISynchLog(txt);
                }
            }
        }
        #endregion

        /// <summary>
        /// 删除数据同步控制文件
        /// </summary>
        private static void deletelck()
        {
            string dir = AppContext.BaseDirectory + "/" + "log";
            DirectoryInfo d = new DirectoryInfo(dir);
            FileInfo[] files = d.GetFiles();//文件
            DirectoryInfo[] directs = d.GetDirectories();//文件夹
            foreach (FileInfo f in files)
            {
                FileInfo fs = new FileInfo(dir + "/" + f.Name);
                DateTime dates = Convert.ToDateTime(fs.LastWriteTime);
                if (f.Name == "lck.lck")
                {
                    fs.Delete();
                    LoggerHelper.WriteLog("删除数据同步控制文件：" + fs.FullName);
                }
            }
        }


        public static string SendRes(string type, string data)
        {
            ResParameter186 res = new ResParameter186 { GUID = Guid.NewGuid().ToString(), Type = type, SendDateTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss:fff"), Data = data };
            return res.ToJson();
        }


    }
}
