using Common.RabbitMQ;
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
        /// <summary>
        /// 同步2022年1月份到当前时间点的交易明细
        /// </summary>
        /// 
       
        public static void SynchReport_TILL()
        {
            LoggerHelper.WriteISynchLog("开始查询2022年1月到当前时间的历史交易明细数据！");
            var db = new SqlHelper();
            string sql = string.Format("SELECT * FROM Report_TILL WHERE DAY_BATCH_DATE>'2021-12-31' AND ISSEND IS NULL LIMIT 10");
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
                    RabbitMQManage.PushMessage(pushMsg,"186ServerQueen", EndResultList.ToJson().ToString());                 
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
                string dd = EndResultList.ToJson().ToString();
                DataTable dt = ToTable(EndResultList.ToJson());
          

                var jsonData = new
                {
                    ReportTILL186 = dt,
                };
                RabbitMQManage.PushMessage(pushMsg, "186ServerQueen", SendRes("ReportTILL186", jsonData));
            }
        }




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


        public static string SendRes(string type, object data)
        {
            ResParameter186 res = new ResParameter186 { GUID = Guid.NewGuid().ToString(), Type = type, SendDateTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss:fff"), Data = data };
            return res.ToJson().ToString();
        }

        public static DataTable ToTable(this string Json)
        {
            return Json == null ? null : JsonConvert.DeserializeObject<DataTable>(Json);
        }
    }
}
