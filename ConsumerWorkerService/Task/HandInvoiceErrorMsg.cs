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

namespace ConsumerWorkerService
{
    public class HandInvoiceErrorMsg : BackgroundService
    {
        public ILogger _logger;
        public HandInvoiceErrorMsg(ILogger<HandInvoiceErrorMsg> logger)
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
                _logger.LogInformation(DateTime.Now.ToString() + "与异常发票数据重新发送：启动");
                while (!stoppingToken.IsCancellationRequested)
                {
                    RepeatHandInvoiceErrorMsg();
                    await Task.Delay(1000 * 60 * 30, stoppingToken);
                }
                LoggerHelper.WriteInvoceLog("异常发票数据重新发送：启动");
            }
            catch (Exception ex)
            {
                if (!stoppingToken.IsCancellationRequested)
                {
                    _logger.LogInformation(DateTime.Now.ToString() + "与异常发票数据重新发送：异常" + ex.Message + ex.StackTrace);
                    LoggerHelper.WriteInvoceLog("与异常发票数据重新发送：异常" + ex.Message + ex.StackTrace);
                }
                else
                {
                    _logger.LogInformation(DateTime.Now.ToString() + "与异常发票数据重新发送：停止");
                    LoggerHelper.WriteInvoceLog("与异常发票数据重新发送：停止");
                }
            }
        }


        /// <summary>
        /// 2022-01-19启用定时任务将InvoiceErrorMsg表错误信息为空的数据重新发送给发票系统
        /// </summary>
        public void RepeatHandInvoiceErrorMsg()
        {
            var db = new SqlHelper();
            var slavedb = new SlaveSqlHelper();
            LoggerHelper.WriteInvoceLog("======================================");
            LoggerHelper.WriteInvoceLog("开始执行===异常发票数据重新发送===定时任务！！");
            try
            {
                string sql1 = "SELECT * FROM InvoiceErrorMsg WHERE  MESSAGE='' AND STATUS!='2'";
                List<InvoiceErrorMsg> InvoiceErrorMsg = slavedb.SqlQueryInfo<InvoiceErrorMsg>(sql1);
                if (InvoiceErrorMsg.Count > 50)
                {
                    int numb = 50;
                    int size = (int)Math.Ceiling((decimal)InvoiceErrorMsg.Count / numb);
                    for (int i = 1; i <= size; i++)
                    {
                        List<InvoiceErrorMsg> EndResultList = InvoiceErrorMsg.Skip((i - 1) * numb).Take(numb).ToList();
                        SendInvoiceErrorMsg(EndResultList);
                        LoggerHelper.WriteInvoceLog("执行完毕===异常发票数据重新发送===定时任务");
                    }
                }
                else
                {
                    List<InvoiceErrorMsg> EndResultList = InvoiceErrorMsg;
                    SendInvoiceErrorMsg(EndResultList);
                    LoggerHelper.WriteInvoceLog("执行完毕===异常发票数据重新发送===定时任务");
                }
            }
            catch (Exception ex)
            {
                LoggerHelper.WriteInvoceLog("异常发票数据重新发送消息失败：" + ex.Message);
            }
        }


        public void SendInvoiceErrorMsg(List<InvoiceErrorMsg> InvoiceErrorMsg)
        {
            var db = new SqlHelper();
            var slavedb = new SlaveSqlHelper();
            List<InvoiceErrorMsg> EndResultList = InvoiceErrorMsg;
            InvoiceTill InvoiceTill = new InvoiceTill();
            InvoiceTill.Pztoken = "26B372ADCBBF8AB70066E1B577AA7B30";
            InvoiceTill.SaleDataList = new List<SaleDataList>();
            foreach (var item in EndResultList)
            {
                InvoiceTill.SaleDataList.Add(item.SENDDATA.ToObject<SaleDataList>());
            }
            var message = HttpManager.HttpPostAsync(_url, InvoiceTill.ToJson(), "application / json", 5000, null);
            InvoiceReturnMsg msg = message.Result.ToObject<InvoiceReturnMsg>();
            if (msg.msg == "000000")
            {
                foreach (var item in EndResultList)
                {
                    item.STATUS = 2;
                    item.CREATEDATE = DateTime.Now;
                    db.UpdateInfo<InvoiceErrorMsg>(item);
                }
                string datamsg = string.Format("异常发票数据重新发送全部成功,发送{0}条，成功{1}条", InvoiceErrorMsg.Count, InvoiceErrorMsg.Count);
                LoggerHelper.WriteInvoceLog(datamsg);
            }
            else
            {
                int k = 0;
                for (int i = 0; i < EndResultList.Count; i++)
                {
                    EndResultList[i].STATUS = 2;
                    EndResultList[i].CREATEDATE = DateTime.Now;
                    foreach (var item in msg.result.data)
                    {
                        if (EndResultList[i].SITEID == item.site_number && EndResultList[i].TILLNUM.ToString() == item.pos_transaction_number)
                        {
                            k = k + 1;
                            EndResultList[i].STATUS = 1;
                        }
                    }
                    db.UpdateInfo<InvoiceErrorMsg>(EndResultList[i]);
                }
                string datamsg = string.Format("发票系统同步交易流水失败,发送{0}条，失败{1}条", InvoiceErrorMsg.Count, msg.result.data.Count);
                LoggerHelper.WriteInvoceLog(datamsg);
            }
        }


        #region 主数据同步
        public void SendMasterProduct()
        {
            var slavedb = new SlaveSqlHelper();
            MasterProductRoot MasterProduct = new MasterProductRoot();
            MasterProduct.pztoken = _token;
            MasterProduct.Product = slavedb.SqlQueryInfo<Product>("SELECT ItemCode AS Product_Code ,REMARK1 AS TaxRate ,ITEMNAME AS Product_Name , UNITNAME AS UOM,REMARK2 AS TaxCode ,LEFT(DEPT,5) AS Product_Type FROM Item)");
            var message = HttpManager.HttpPostAsync(_url + "goods", MasterProduct.ToJson(), "application / json", 30, null);
            InvoiceReturnMsg msg = message.Result.ToObject<InvoiceReturnMsg>();
            if (msg.msg == "000000")
            {
                LoggerHelper.WriteLog("商品主数据同步成功：");
            }
            else
            {
                LoggerHelper.WriteLog("商品主数据同步失败：     " );
            }
        }

        public void SendMasterPayment()
        {
            var slavedb = new SlaveSqlHelper();
            MasterPaymentRoot MasterPayment = new MasterPaymentRoot();
            MasterPayment.pztoken = _token;
            MasterPayment.Payment = slavedb.SqlQueryInfo<Payment>("SELECT PMNTCode as Payment_code,PMNTName as Payment_Method FROM PMNT");
            var message = HttpManager.HttpPostAsync(_url + "Payment", MasterPayment.ToJson(), "application / json", 30, null);
            InvoiceReturnMsg msg = message.Result.ToObject<InvoiceReturnMsg>();
            if (msg.msg == "000000")
            {
                LoggerHelper.WriteLog("与发票系统支付方式主数据同步成功：");
            }
            else
            {
                LoggerHelper.WriteLog("与发票系统支付方式主数据同步失败：     " );
            }
        }
        public void SendMasterDept()
        {
            var slavedb = new SlaveSqlHelper();
            MasterProduct_DeptRoot MasterDept = new MasterProduct_DeptRoot();
            MasterDept.pztoken = _token;
            MasterDept.Product_Dept = slavedb.SqlQueryInfo<Product_Dept>("SELECT DEPT as Dept_code,DEPTNAME as Dept_Name ,PRENTDEPT as Prent_Dept_code,PRENTDEPTNAME as Prent_Dept_Name FROM ItemDEPT");
            var message = HttpManager.HttpPostAsync(_url + "grade", MasterDept.ToJson(), "application / json", 30, null);
            InvoiceReturnMsg msg = message.Result.ToObject<InvoiceReturnMsg>();
            if (msg.msg == "000000")
            {
                LoggerHelper.WriteLog("与发票系统商品品类主数据同步成功：");
            }
            else
            {
                LoggerHelper. WriteLog("与发票系统商品品类主数据同步失败：     " );
            }
        }

        #endregion 
    }
}
