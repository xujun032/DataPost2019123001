
using Macrosage.RabbitMQ.Server.Customer;
using Macrosage.RabbitMQ.Server.Product;
using Quartz;
using RabbitMQ.Client;
using SPTool.Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace SPTool
{
    [DisallowConcurrentExecution]
    public class ShellGasSaleDataJob : IJob
    {
        SPTool.Model.SiteInfo SiteInfoModel = new SPTool.Model.SiteInfo();
        SPTool.BLL.SiteInfo SiteInfoBLL = new BLL.SiteInfo();
        SPTool.BLL.GunConfigure gunConfigBLL = new BLL.GunConfigure();
        SPTool.Model.GunConfigure gunConfigModel = new GunConfigure();
        SPTool.BLL.FuelItemConfig FuelItemConfigBLL = new BLL.FuelItemConfig();
        SPTool.Model.FuelItemConfig FuelItemConfigModel = new Model.FuelItemConfig();
        SPTool.BLL.TankConfigure tankConfigBLL = new BLL.TankConfigure();
        SPTool.Model.TankConfigure tankConfigModel = new TankConfigure();

        public void Execute(IJobExecutionContext context)
        {
            try
            {
                #region  发布时取消注释
                string DBPath = System.Windows.Forms.Application.StartupPath + "\\" + "DataBase.db";
                string BackUppath = "D:\\SPToolDBBackUp";
                if (!Directory.Exists(BackUppath))
                {
                    Directory.CreateDirectory(BackUppath);
                }
                File.Copy(DBPath, BackUppath + "\\" + "DataBase.db", true);
                #endregion
                Form1.WriteLog("开始执行中心模式销售数据上送定时任务");
                SiteInfoModel = SiteInfoBLL.GetTopModel();
                string str = string.Format("SELECT T.TILLNUM,(SELECT DAY_BATCH_DATE FROM  DAYBATCH D, POSBATCH P WHERE T.POS_BATCH_ID = P.POS_BATCH_ID AND P.DAY_BATCH_ID = D.DAY_BATCH_ID) AS DAY_BATCH_DATE,T.SALEDATE,(SELECT FULLNAME FROM  EMPLOYEE E WHERE T.WTR = E.EMPNO) AS WTR, T.TIMEOPEN,T.TIMECLOSE,T.GUESTS,T.CUST_MAGKEY,T.TIMESLICE,T.STATUSTYPE,T.STATUSCODE,T.POSTED,T.SSUM,T.PRICE_TYPE_ID,T.EXTREF2,(SELECT DAY_BATCH_DATE FROM  DAYBATCH D, POSBATCH P WHERE T.LINK_POS_BATCH_ID = P.POS_BATCH_ID AND P.DAY_BATCH_ID = D.DAY_BATCH_ID) AS LINK_DAY_BATCH_DATE,T.LINK_TILLNUM,T.QTY,T.TICKET,T.PREMIERPOINTS,T.BONUSPOINTS,T.ITEM_SOLD,T.BANKTERMID,T.EXTREF3,T.ENTRY_METHOD,T.CUSTOMER_GROUP_ID,T.FUEL_TRANS_TYPE,T.IS_INVOICED,T.CLOSE_TIMESTAMP,T.FUELTRANSTYPE,T.INVOICE_PRINTED,ISRECONCILED FROM TILL T WHERE   T.STATUSTYPE IN (1,2,3,4,15) AND T.POSTED >'{0}'", SiteInfoModel.ActivedDatetime);
                if (!string.IsNullOrEmpty(SiteInfoModel.SaleLastDatetime))
                {
                    string STR1 = string.Format("AND POSTED >'{0}'", SiteInfoModel.SaleLastDatetime);
                    str = str + STR1;
                }
                str = str + "ORDER BY T.POSTED,T.TILLNUM";
                DataTable dt = DBUtility.DbHelperODBC.Query(str).Tables[0];
                string SaleLastDatetime = "";
                string MySaleLastDatetime = "";
                Form1.WriteLog("中心模式共发现" + dt.Rows.Count.ToString() + "条销售数据");

                string CrurrentDate30 = DateTime.Now.AddDays(-30).ToString("yyyy-MM-dd");

                if (dt.Rows.Count > 0)
                {
                    int Index = (int)Math.Ceiling((double)dt.Rows.Count / (double)10);
                    for (int i = 0; i < Index; i++)
                    {
                        DataTable dt10 = GetPagedTable(dt, i + 1);
                        SaleLastDatetime = dt10.AsEnumerable().Last<DataRow>()["POSTED"].ToString().Trim().ToDate().ToString("yyyy-MM-dd HH:mm:ss");
                        MySaleLastDatetime = dt10.AsEnumerable().Last<DataRow>()["POSTED"].ToString().Trim().ToDate().AddDays(-2).ToString("yyyy-MM-dd");
                        string TillNUM_List = GetTillNum(dt10, "TILLNUM");
                        //查询TILLITEM
                        string tillitemsql = string.Format("SELECT TILLNUM, T.POS_ID,(SELECT DAY_BATCH_DATE FROM DAYBATCH D WHERE D.DAY_BATCH_ID=P.DAY_BATCH_ID) AS DAY_BATCH_DATE,STATUSTYPE,STATUSCODE,I.BARCODE,I.ITEMID,I.ITEMNAME,I.DEPT,POSTED,(T.QTY* T.WEIGHT) AS SALEQTY,(T.TOTAL) AS ACTUAL_AMOUNT,'A' AS PROMO_TYPE,(SELECT DISCOUNTSUM FROM TILLITEMSTATUS WHERE TILLITEMNUM=T.SERNUM) AS DISCOUNT,(SELECT U.UNITNAME FROM UNITSIZE U WHERE I.BASEUNIT = U.UNITID ) as UNITNAME , (SELECT TANK1_ID  FROM FUEL_PUMPS_HOSE FPH WHERE FPH.PUMP_ID= T.PUMP_ID AND FPH.HOSE_ID= T.HOSE_ID) AS TANK_ID ,T.PUMP_ID, T.HOSE_ID,T.PUMPSRV_REF, T.PUMP_OPEN_COUNTER as START_PUMP_READING, T.PUMP_CLOSE_COUNTER as END_PUMP_READING, STDPRICE, PRICE, ISFUEL, T.PUMP_TOTAL, (SELECT SUM(TOTAL_SAVINGS)FROM TILLITEMPROMO TIP, PROMO WHERE TIP.PROMO_ID = PROMO.PROMO_ID AND TILLITEM_SERNUM = T.SERNUM AND PROMO.PROMO_TYPE = 'C') AS PROMO_AMNT,(SELECT MAX(CASE PROMO_TYPE WHEN 'E' THEN EXT_REF_1 ELSE CAST(OUTREF AS varchar(10)) END)FROM TILLITEMPROMO TIP, PROMO WHERE TIP.PROMO_ID = PROMO.PROMO_ID AND TILLITEM_SERNUM = T.SERNUM) AS PROMO_ID FROM TILLITEM T JOIN POSBATCH P ON T.POS_BATCH_ID = P.POS_BATCH_ID JOIN ITEM I ON T.PLU = I.ITEMID  WHERE T.statustype IN(1,2,3,4,15) AND  TILLNUM IN  ({0})  AND P.TIME_OPEN> '{1}' AND POSTED>'{2}' ORDER BY TILLNUM", TillNUM_List, CrurrentDate30, MySaleLastDatetime);                
                        DataTable tilltemdt10 = DBUtility.DbHelperODBC.Query(tillitemsql).Tables[0];

               

                        //查询TILL_PMNT
                        string tillpmntsql = string.Format("SELECT B.TILLNUM,T.POSTED,D.DAY_BATCH_DATE,(SELECT MAX(PMNT_ID) FROM PMNT WHERE PMNTCODE_ID = T.PMCODE AND PMSUBCODE_ID =T.PMSUBCODE) AS PMNTID,(SELECT PMNT_NAME FROM PMNT WHERE PMNTCODE_ID = T.PMCODE AND PMSUBCODE_ID =T.PMSUBCODE) AS PMNT_NAME ,(SELECT MAX(EXTREF) FROM PMNT WHERE PMNTCODE_ID = T.PMCODE AND PMSUBCODE_ID =T.PMSUBCODE) AS PMNTEXTREF,T.SSUM,T.FSUM,(SELECT CURRENCY_SYMBOL FROM CURRENCY WHERE T.CURID = CURRENCY_ID) AS CURRENCY_SYMBOL,T.CURID ,(SELECT CARDNUM FROM TILLPMNT_CRDT_TRL WHERE SERNUM = T.SERNUM) AS CARDNUM,(SELECT REFERENCE FROM TILLPMNT_CRDT_TRL WHERE SERNUM = T.SERNUM) AS REFERENCE ,(SELECT MERCHANT FROM TILLPMNT_CRDT_TRL WHERE SERNUM = T.SERNUM) AS MERCHANT,(SELECT APPROVAL FROM TILLPMNT_CRDT_TRL WHERE SERNUM = T.SERNUM) AS APPROVAL FROM TILLPMNT T ,TILLBILL B, POSBATCH P,DAYBATCH D WHERE T.BILLNUM = B.BILLNUM AND T.POS_BATCH_ID = P.POS_BATCH_ID AND T.POS_BATCH_ID = B.POS_BATCH_ID AND T.POS_BATCH_ID = P.POS_BATCH_ID AND P.DAY_BATCH_ID = D.DAY_BATCH_ID AND B.TILLNUM IN  ({0}) AND P.TIME_OPEN> '{1}' AND T.POSTED>'{2}' ORDER BY B.TILLNUM", TillNUM_List, CrurrentDate30, MySaleLastDatetime);
                        DataTable tillpmntdt10 = DBUtility.DbHelperODBC.Query(tillpmntsql).Tables[0];

               
                        //查询TILL_PROMO
                        string tillpromosql = string.Format("SELECT TP.TILLNUM,(SELECT DAY_BATCH_DATE FROM DAYBATCH D WHERE D.DAY_BATCH_ID=P.DAY_BATCH_ID) AS DAY_BATCH_DATE,TL.POSTED,TP.POS_ID,TP.PROMO_ID,PROMO_QUANTITY,TOTAL_SAVINGS,PR.PROMO_NAME,PR.OPER,PR.PROMO_TYPE,PR.OUTREF FROM  TILLPROMO TP LEFT JOIN   PROMO PR ON PR.PROMO_ID=TP.PROMO_ID JOIN POSBATCH P ON TP.POS_BATCH_ID = P.POS_BATCH_ID JOIN TILL TL ON TL.TILLNUM=TP.TILLNUM WHERE  TP.TILLNUM IN  ({0}) AND P.TIME_OPEN> '{1}' AND TL.POSTED> '{2}' ORDER BY TP.TILLNUM", TillNUM_List, CrurrentDate30, MySaleLastDatetime);
                        DataTable tillpromodt10 = DBUtility.DbHelperODBC.Query(tillpromosql).Tables[0];
                        //查询TILLITEM_PMNT
                        string tillitempmntsql = string.Format("SELECT TPS.TILLNUM, (SELECT DAY_BATCH_DATE FROM DAYBATCH D WHERE D.DAY_BATCH_ID=P.DAY_BATCH_ID) AS DAY_BATCH_DATE,TI.POSTED,I.BARCODE,I.ITEMID,I.ITEMNAME,I.DEPT,(SELECT PMNT_NAME FROM PMNT WHERE TPS.PMNT_ID =PMNT.PMNT_ID) AS PMNT_NAME ,(SELECT MAX(EXTREF) FROM PMNT WHERE TPS.PMNT_ID =PMNT.PMNT_ID) AS PMNTEXTREF,TPS.TOTAL,TPS.QTY FROM TILLITEM_PMNT_SPLIT TPS JOIN POSBATCH P ON TPS.POS_BATCH_ID = P.POS_BATCH_ID JOIN TILLITEM  TI ON TI.SERNUM = TPS.TILLITEM_SERNUM JOIN ITEM     I ON TI.PLU = I.ITEMID WHERE TPS.POS_BATCH_ID = P.POS_BATCH_ID AND TPS.POS_BATCH_ID = P.POS_BATCH_ID  AND TPS.TILLNUM IN  ({0}) AND P.TIME_OPEN> '{1}' AND TI.POSTED>'{2}' ORDER BY TPS.TILLNUM", TillNUM_List, CrurrentDate30, MySaleLastDatetime);
                        DataTable tillitempmntdt10 = DBUtility.DbHelperODBC.Query(tillitempmntsql).Tables[0];

            
                        //查询TILLITEM_PROMO
                        string tillitempromosql = string.Format("SELECT T.TILLNUM,(SELECT DAY_BATCH_DATE FROM DAYBATCH D WHERE D.DAY_BATCH_ID=P.DAY_BATCH_ID) AS DAY_BATCH_DATE,T.POSTED,I.BARCODE,I.ITEMID,I.ITEMNAME,I.DEPT,PR.PROMO_NAME,PR.OPER,PR.PROMO_TYPE,TIP.PROMO_ID,TIP.TOTAL_SAVINGS,TIP.PROMO_QUANTITY ,TIP.PROMO_ITEM_QTY,PR.EXT_REF_1,PR.EXT_REF_2,PR.OUTREF  FROM TILLITEM T, POSBATCH P, TILLITEMPROMO TIP,  PROMO PR JOIN ITEM     I ON T.PLU = I.ITEMID WHERE TIP.PROMO_ID =  PR.PROMO_ID AND TIP.TILLITEM_SERNUM = T.SERNUM AND T.POS_BATCH_ID = P.POS_BATCH_ID AND T.STATUSTYPE IN (1,2,3,4,15) AND T.TILLNUM IN  ({0})  AND P.TIME_OPEN> '{1}'  AND T.POSTED>'{2}' ORDER BY T.TILLNUM", TillNUM_List, CrurrentDate30, MySaleLastDatetime);
                        DataTable tillitempromodt10 = DBUtility.DbHelperODBC.Query(tillitempromosql).Tables[0];
                       // tilltemdt10 = GetAttendant(tilltemdt10);
                        var jsonData = new
                        {
                            Till = dt10,
                            TILLITEM = SetTrim(tilltemdt10),
                            TILL_PMNT = SetTrim(tillpmntdt10),
                            TILL_PROMO = SetTrim(tillpromodt10),
                            TILLITEM_PMNT = SetTrim(tillitempmntdt10),
                            TILLITEM_PROMO = SetTrim(tillitempromodt10)
                        };
                        string message = SendRes("Till", SiteInfoModel.StationNo, jsonData);
                        IMessageProduct product = new MessageProduct();
                        string queueName = "ConsumerWorkerService.ResParameter, ConsumerWorkerService_Site2HOSMessage";
                        product.Publish(message, queueName);
                        SiteInfoModel.SaleLastDatetime = SaleLastDatetime;
                        SiteInfoBLL.Update(SiteInfoModel);
                        Form1.WriteLog("消息已经发送出:" + message);
                    }
                }
            }
            catch (Exception me)
            {
                Form1.WriteLog("错误：" + me.StackTrace);
                Form1.WriteLog("中心模式销售数据上送失败：" + me.Message);
            }
        }

        /// <summary>
        /// 调接口获取加油工数据
        /// </summary>
        /// <param name="dt"></param>
        /// <returns></returns>
        public DataTable GetAttendant(DataTable dt)
        {
            dt.Columns.Add("REQUEST_ID", Type.GetType("System.String"));
            dt.Columns.Add("PUMP_STARTTIME", Type.GetType("System.String"));
            dt.Columns.Add("PUMP_ENDTTIME", Type.GetType("System.String"));
            dt.Columns.Add("ATTENDANT_ID", Type.GetType("System.String"));
            dt.Columns.Add("ATTENDANT_NAME", Type.GetType("System.String"));
            dt.Columns.Add("ACT_NOZZLE_ID", Type.GetType("System.String"));
            dt.Columns.Add("Remark1", Type.GetType("System.String"));
            dt.Columns.Add("Remark2", Type.GetType("System.String"));
            List<SendAttendant> SendAttendantList = new List<SendAttendant>();
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                if (dt.Rows[i]["ISFUEL"].ToString() == "T")
                {
                    SendAttendant sendAttendant = new SendAttendant();
                    sendAttendant.PUMP_ID = dt.Rows[i]["PUMP_ID"].ToString();
                    sendAttendant.HOSE_ID = dt.Rows[i]["HOSE_ID"].ToString();
                    sendAttendant.PUMPSRV_REF = dt.Rows[i]["PUMPSRV_REF"].ToString();
                    sendAttendant.REQUEST_ID = Guid.NewGuid().ToString();
                    dt.Rows[i]["REQUEST_ID"] = sendAttendant.REQUEST_ID;
                    sendAttendant.PUMP_TOTAL = dt.Rows[i]["PUMP_TOTAL"].ToString();
                    sendAttendant.Remark1 = dt.Rows[i]["Remark1"].ToString();
                    sendAttendant.Remark2 = dt.Rows[i]["Remark2"].ToString();
                    SendAttendantList.Add(sendAttendant);
                }
            }
            SendAttendantJson SendAttendantJson = new SendAttendantJson();
            SendAttendantJson.SendAttendant = SendAttendantList;
            string PostJsonStr = SendAttendantJson.ToJson().ToString().Replace("\\", "").Replace("Param\":\"{", "Param\":{").Replace("]}\"}", "]}}");
            string url = "http://10.81.7.12:8096/api/v1/scps/getEmployeeInfo";
            Form1.WriteLog("发送到绩效系统数据：" + PostJsonStr);
            string dd = HttpPost.PostHttps(url, PostJsonStr);
            Form1.WriteLog("绩效系统接口反馈："+dd);
            List<AttendantResult>  Result =dd.ToObject<List<AttendantResult>>();
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                for (int j = 0; j< Result.Count; j++)
                {
                    if (dt.Rows[i]["REQUEST_ID"].ToString() == Result[j].REQUEST_ID)
                    {
                        dt.Rows[i]["PUMP_STARTTIME"] = Result[j].PUMP_STARTTIME;
                        dt.Rows[i]["PUMP_ENDTTIME"] = Result[j].PUMP_ENDTTIME;
                        dt.Rows[i]["ATTENDANT_ID"] = Result[j].ATTENDANT_ID;
                        dt.Rows[i]["ATTENDANT_NAME"] = Result[j].ATTENDANT_NAME;
                        dt.Rows[i]["ACT_NOZZLE_ID"] = Result[j].ACT_NOZZLE_ID;
                        dt.Rows[i]["Remark1"] = Result[j].Remark1;
                        dt.Rows[i]["Remark2"] = Result[j].Remark2;
                    }
                }
            }

            dt.Columns.Remove("REQUEST_ID");
                return dt;
        }



    

        public int RepeatSendTill(string day_batch_date, string tillnum1, string tillnum2)
        {
            //1 成功 2失败 3 数据不对
            int flag = 2;
            try
            {

                Form1.WriteLog("开始执行重传销售数据");
                SiteInfoModel = SiteInfoBLL.GetTopModel();
                string str = string.Format("SELECT T.TILLNUM,(SELECT DAY_BATCH_DATE FROM  DAYBATCH D, POSBATCH P WHERE T.POS_BATCH_ID = P.POS_BATCH_ID AND P.DAY_BATCH_ID = D.DAY_BATCH_ID  AND D.DAY_BATCH_DATE='{0}') AS DAY_BATCH_DATE,T.SALEDATE,(SELECT FULLNAME FROM  EMPLOYEE E WHERE T.WTR = E.EMPNO) AS WTR, T.TIMEOPEN,T.TIMECLOSE,T.GUESTS,T.CUST_MAGKEY,T.TIMESLICE,T.STATUSTYPE,T.STATUSCODE,T.POSTED,T.SSUM,T.PRICE_TYPE_ID,T.EXTREF2,(SELECT DAY_BATCH_DATE FROM  DAYBATCH D, POSBATCH P WHERE T.LINK_POS_BATCH_ID = P.POS_BATCH_ID AND P.DAY_BATCH_ID = D.DAY_BATCH_ID) AS LINK_DAY_BATCH_DATE,T.LINK_TILLNUM,T.QTY,T.TICKET,T.PREMIERPOINTS,T.BONUSPOINTS,T.ITEM_SOLD,T.BANKTERMID,T.EXTREF3,T.ENTRY_METHOD,T.CUSTOMER_GROUP_ID,T.FUEL_TRANS_TYPE,T.IS_INVOICED,T.CLOSE_TIMESTAMP,T.FUELTRANSTYPE,T.INVOICE_PRINTED,ISRECONCILED FROM TILL T WHERE   T.STATUSTYPE IN (1,2,3,4,15) AND TILLNUM>='{1}' And TILLNUM<='{2}'", day_batch_date, tillnum1, tillnum2);

                str = str + "ORDER BY T.POSTED,T.TILLNUM";
                DataTable dt = DBUtility.DbHelperODBC.Query(str).Tables[0];
                Form1.WriteLog("一共"+dt.Rows.Count + "条！");

                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    DateTime  dtime = DateTime.Parse(dt.Rows[i]["SALEDATE"].ToString());
                    Form1.WriteLog(dtime.ToString("yyyy-MM-dd") + "======"+ day_batch_date);
                    if (dtime.ToString("yyyy-MM-dd") != day_batch_date)
                       // if (dt.Rows[i]["SALEDATE"].ToString()!= day_batch_date||dt.Rows[i]["DAY_BATCH_DATE"].ToString() == "" || dt.Rows[i]["DAY_BATCH_DATE"].ToString() == null || dt.Rows[i]["DAY_BATCH_DATE"].ToString() == "null")
                    {
                       
                        dt.Rows[i].Delete();
                    }
                }
                dt.AcceptChanges();
                string SaleLastDatetime = "";
                Form1.WriteLog("中心模式共发现" + dt.Rows.Count.ToString() + "条销售数据");

                string CrurrentDate30 = DateTime.Now.AddDays(-100).ToString("yyyy-MM-dd");
                if (dt.Rows.Count > 0)
                {
                    int Index = (int)Math.Ceiling((double)dt.Rows.Count / (double)10);
                    for (int i = 0; i < Index; i++)
                    {
                        DataTable dt10 = GetPagedTable(dt, i + 1);
                        SaleLastDatetime = dt10.AsEnumerable().Last<DataRow>()["POSTED"].ToString().Trim().ToDate().ToString("yyyy-MM-dd HH:mm:ss");
                        string TillNUM_List = GetTillNum(dt10, "TILLNUM");
                        //查询TILLITEM
                        string tillitemsql = string.Format("SELECT TILLNUM, T.POS_ID,(SELECT DAY_BATCH_DATE FROM DAYBATCH D WHERE D.DAY_BATCH_ID=P.DAY_BATCH_ID) AS DAY_BATCH_DATE,STATUSTYPE,STATUSCODE,I.BARCODE,I.ITEMID,I.ITEMNAME,I.DEPT,POSTED,(T.QTY* T.WEIGHT) AS SALEQTY,(T.TOTAL) AS ACTUAL_AMOUNT,'A' AS PROMO_TYPE,(SELECT DISCOUNTSUM FROM TILLITEMSTATUS WHERE TILLITEMNUM=T.SERNUM) AS DISCOUNT,(SELECT U.UNITNAME FROM UNITSIZE U WHERE I.BASEUNIT = U.UNITID ) as UNITNAME , (SELECT TANK1_ID  FROM FUEL_PUMPS_HOSE FPH WHERE FPH.PUMP_ID= T.PUMP_ID AND FPH.HOSE_ID= T.HOSE_ID) AS TANK_ID ,T.PUMP_ID, T.HOSE_ID,T.PUMPSRV_REF, T.PUMP_OPEN_COUNTER as START_PUMP_READING, T.PUMP_CLOSE_COUNTER as END_PUMP_READING, STDPRICE, PRICE, ISFUEL, T.PUMP_TOTAL, (SELECT SUM(TOTAL_SAVINGS)FROM TILLITEMPROMO TIP, PROMO WHERE TIP.PROMO_ID = PROMO.PROMO_ID AND TILLITEM_SERNUM = T.SERNUM AND PROMO.PROMO_TYPE = 'C') AS PROMO_AMNT,(SELECT MAX(CASE PROMO_TYPE WHEN 'E' THEN EXT_REF_1 ELSE CAST(OUTREF AS varchar(10)) END)FROM TILLITEMPROMO TIP, PROMO WHERE TIP.PROMO_ID = PROMO.PROMO_ID AND TILLITEM_SERNUM = T.SERNUM) AS PROMO_ID FROM TILLITEM T JOIN POSBATCH P ON T.POS_BATCH_ID = P.POS_BATCH_ID JOIN ITEM I ON T.PLU = I.ITEMID  WHERE T.statustype IN(1,2,3,4,15) AND  TILLNUM IN  ({0})  AND P.TIME_OPEN> '{1}'  ORDER BY TILLNUM", TillNUM_List, CrurrentDate30);
               
                        DataTable tilltemdt10 = DBUtility.DbHelperODBC.Query(tillitemsql).Tables[0];
                        //查询TILL_PMNT
                        string tillpmntsql = string.Format("SELECT B.TILLNUM,T.POSTED,D.DAY_BATCH_DATE,(SELECT MAX(PMNT_ID) FROM PMNT WHERE PMNTCODE_ID = T.PMCODE AND PMSUBCODE_ID =T.PMSUBCODE) AS PMNTID,(SELECT PMNT_NAME FROM PMNT WHERE PMNTCODE_ID = T.PMCODE AND PMSUBCODE_ID =T.PMSUBCODE) AS PMNT_NAME ,(SELECT MAX(EXTREF) FROM PMNT WHERE PMNTCODE_ID = T.PMCODE AND PMSUBCODE_ID =T.PMSUBCODE) AS PMNTEXTREF,T.SSUM,T.FSUM,(SELECT CURRENCY_SYMBOL FROM CURRENCY WHERE T.CURID = CURRENCY_ID) AS CURRENCY_SYMBOL,T.CURID ,(SELECT CARDNUM FROM TILLPMNT_CRDT_TRL WHERE SERNUM = T.SERNUM) AS CARDNUM,(SELECT REFERENCE FROM TILLPMNT_CRDT_TRL WHERE SERNUM = T.SERNUM) AS REFERENCE ,(SELECT MERCHANT FROM TILLPMNT_CRDT_TRL WHERE SERNUM = T.SERNUM) AS MERCHANT,(SELECT APPROVAL FROM TILLPMNT_CRDT_TRL WHERE SERNUM = T.SERNUM) AS APPROVAL FROM TILLPMNT T ,TILLBILL B, POSBATCH P,DAYBATCH D WHERE T.BILLNUM = B.BILLNUM AND T.POS_BATCH_ID = P.POS_BATCH_ID AND T.POS_BATCH_ID = B.POS_BATCH_ID AND T.POS_BATCH_ID = P.POS_BATCH_ID AND P.DAY_BATCH_ID = D.DAY_BATCH_ID AND B.TILLNUM IN  ({0}) AND P.TIME_OPEN> '{1}' ORDER BY B.TILLNUM", TillNUM_List, CrurrentDate30);
                        DataTable tillpmntdt10 = DBUtility.DbHelperODBC.Query(tillpmntsql).Tables[0];
                        //查询TILL_PROMO
                        string tillpromosql = string.Format("SELECT TP.TILLNUM,(SELECT DAY_BATCH_DATE FROM DAYBATCH D WHERE D.DAY_BATCH_ID=P.DAY_BATCH_ID) AS DAY_BATCH_DATE,TL.POSTED,TP.POS_ID,TP.PROMO_ID,PROMO_QUANTITY,TOTAL_SAVINGS,PR.PROMO_NAME,PR.OPER,PR.PROMO_TYPE,PR.OUTREF FROM  TILLPROMO TP LEFT JOIN   PROMO PR ON PR.PROMO_ID=TP.PROMO_ID JOIN POSBATCH P ON TP.POS_BATCH_ID = P.POS_BATCH_ID JOIN TILL TL ON TL.TILLNUM=TP.TILLNUM WHERE  TP.TILLNUM IN  ({0}) AND P.TIME_OPEN> '{1}' AND TL.POSTED> '{2}' ORDER BY TP.TILLNUM", TillNUM_List, CrurrentDate30, CrurrentDate30);
                        DataTable tillpromodt10 = DBUtility.DbHelperODBC.Query(tillpromosql).Tables[0];
                        //查询TILLITEM_PMNT
                        string tillitempmntsql = string.Format("SELECT TPS.TILLNUM, (SELECT DAY_BATCH_DATE FROM DAYBATCH D WHERE D.DAY_BATCH_ID=P.DAY_BATCH_ID) AS DAY_BATCH_DATE,TI.POSTED,I.BARCODE,I.ITEMID,I.ITEMNAME,I.DEPT,(SELECT PMNT_NAME FROM PMNT WHERE TPS.PMNT_ID =PMNT.PMNT_ID) AS PMNT_NAME ,(SELECT MAX(EXTREF) FROM PMNT WHERE TPS.PMNT_ID =PMNT.PMNT_ID) AS PMNTEXTREF,TPS.TOTAL,TPS.QTY FROM TILLITEM_PMNT_SPLIT TPS JOIN POSBATCH P ON TPS.POS_BATCH_ID = P.POS_BATCH_ID JOIN TILLITEM  TI ON TI.SERNUM = TPS.TILLITEM_SERNUM JOIN ITEM     I ON TI.PLU = I.ITEMID WHERE TPS.POS_BATCH_ID = P.POS_BATCH_ID AND TPS.POS_BATCH_ID = P.POS_BATCH_ID  AND TPS.TILLNUM IN  ({0}) AND P.TIME_OPEN> '{1}' ORDER BY TPS.TILLNUM", TillNUM_List, CrurrentDate30);
                        DataTable tillitempmntdt10 = DBUtility.DbHelperODBC.Query(tillitempmntsql).Tables[0];
                        //查询TILLITEM_PROMO
                        string tillitempromosql = string.Format("SELECT T.TILLNUM,(SELECT DAY_BATCH_DATE FROM DAYBATCH D WHERE D.DAY_BATCH_ID=P.DAY_BATCH_ID) AS DAY_BATCH_DATE,T.POSTED,I.BARCODE,I.ITEMID,I.ITEMNAME,I.DEPT,PR.PROMO_NAME,PR.OPER,PR.PROMO_TYPE,TIP.PROMO_ID,TIP.TOTAL_SAVINGS,TIP.PROMO_QUANTITY ,TIP.PROMO_ITEM_QTY,PR.EXT_REF_1,PR.EXT_REF_2,PR.OUTREF  FROM TILLITEM T, POSBATCH P, TILLITEMPROMO TIP,  PROMO PR JOIN ITEM     I ON T.PLU = I.ITEMID WHERE TIP.PROMO_ID =  PR.PROMO_ID AND TIP.TILLITEM_SERNUM = T.SERNUM AND T.POS_BATCH_ID = P.POS_BATCH_ID AND T.STATUSTYPE IN (1,2,3,4,15) AND T.TILLNUM IN  ({0})  AND P.TIME_OPEN> '{1}' ORDER BY T.TILLNUM", TillNUM_List, CrurrentDate30);
                        DataTable tillitempromodt10 = DBUtility.DbHelperODBC.Query(tillitempromosql).Tables[0];
                        //tilltemdt10 = GetAttendant(tilltemdt10);


                        var jsonData = new
                        {
                            Till = dt10,
                            TILLITEM = SetTrim(tilltemdt10),
                            TILL_PMNT = SetTrim(tillpmntdt10),
                            TILL_PROMO = SetTrim(tillpromodt10),
                            TILLITEM_PMNT = SetTrim(tillitempmntdt10),
                            TILLITEM_PROMO = SetTrim(tillitempromodt10)
                        };
                        string message = SendRes("RepeatTill", SiteInfoModel.StationNo, jsonData);
                        IMessageProduct product = new MessageProduct();
                        string queueName = "ConsumerWorkerService.ResParameter, ConsumerWorkerService_Site2HOSMessage";
                        product.Publish(message, queueName);
                        //SiteInfoModel.SaleLastDatetime = SaleLastDatetime;
                        //SiteInfoBLL.Update(SiteInfoModel);
                        Form1.WriteLog("重传消息已经发送出:" + message);
                        flag = 1;
                    }
                    return flag;
                }
                else
                {
                    return flag;
                }
            }
            catch (Exception me)
            {
                Form1.WriteLog("错误：" + me.StackTrace);
                Form1.WriteLog("中心模式销售数据上送失败：" + me.Message);
                return flag;
            }
        }

        /// <summary>
        /// /分页10条
        /// </summary>
        /// <param name="dt"></param>
        /// <param name="PageIndex"></param>
        /// <param name="PageSize"></param>
        /// <returns></returns>
        public DataTable GetPagedTable(DataTable dt, int PageIndex)//PageIndex表示第几页，PageSize表示每页的记录数
        {
            int PageSize = 10;
            if (PageIndex == 0)
                return dt;//0页代表每页数据，直接返回

            DataTable newdt = dt.Copy();
            newdt.Clear();//copy dt的框架

            int rowbegin = (PageIndex - 1) * PageSize;
            int rowend = PageIndex * PageSize;

            if (rowbegin >= dt.Rows.Count)
                return newdt;//源数据记录数小于等于要显示的记录，直接返回dt

            if (rowend > dt.Rows.Count)
                rowend = dt.Rows.Count;
            for (int i = rowbegin; i <= rowend - 1; i++)
            {
                DataRow newdr = newdt.NewRow();
                DataRow dr = dt.Rows[i];
                foreach (DataColumn column in dt.Columns)
                {
                    if (column.ColumnName == "WTR")
                        newdr[column.ColumnName] = dr[column.ColumnName].ToString().Trim();
                    else
                        newdr[column.ColumnName] = dr[column.ColumnName];

                }
                newdt.Rows.Add(newdr);
            }
            return newdt;
        }

        /// <summary>
        /// 组装MQ响应消息
        /// </summary>
        /// <param name="type"></param>
        /// <param name="siteid"></param>
        /// <param name="data"></param>
        /// <returns></returns>
        public string SendRes(string type, string siteid, object data)
        {
            ResParameter res = new ResParameter { GUID = Guid.NewGuid().ToString(), Type = type, SendDateTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss:fff"), SiteId = siteid, Data = data };
            return res.ToJson().ToString();
        }


        protected static string createTabColumnDataJson(System.Data.DataTable dt, int rowIndex)
        {
            string jsonStr = "";
            DateTime dtime;
            for (int i = 0; i < dt.Columns.Count; i++)
            {
                string cName = dt.Columns[i].ColumnName;
                string cValue = dt.Rows[rowIndex][cName].ToString();
                if (!String.IsNullOrEmpty(cValue)) //!String.IsNullOrEmpty(cValue) edit by Spark 2011-3-30 f_parcel_term1,f_parcel_term1
                {
                    try
                    {
                        //Regex r = new Regex(@"\d{4}-\d{1,2}-\d{1,2}(\s+\d{1,2}:\d{1,2}:\d{1,2})?");
                        if (dt.Columns[cName].DataType == typeof(DateTime))
                        //if (r.IsMatch(cValue))
                        {
                            dtime = DateTime.Parse(cValue);
                            if (dtime.Hour == 0 && dtime.Minute == 0 && dtime.Second == 0)
                            {
                                cValue = dtime.ToString("yyyy-MM-dd");
                            }
                            else
                            {
                                cValue = dtime.ToString("yyyy-MM-dd HH:mm:ss");
                            }
                        }
                    }
                    catch (Exception ex)
                    { }
                    if (jsonStr == "")
                        jsonStr = cName + ":'" + cValue + "'";
                    else
                        jsonStr += "," + cName + ":'" + cValue + "'";
                }
            }
            return jsonStr;
        }

        /// <summary>
        /// 获取DataTabl所有的TILLMUN的字符串
        /// </summary>
        /// <param name="dt"></param>
        /// <returns></returns>
        protected static string GetTillNum(System.Data.DataTable dt, string cName)
        {
            StringBuilder Str = new StringBuilder();
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                foreach (DataColumn column in dt.Columns)
                {
                    if (column.ColumnName == cName)
                        Str.Append(dt.Rows[i][column.ColumnName].ToString().Trim() + ",");
                }
            }
            string s = Str.ToString();
            return s.Substring(0, s.Length - 1);
        }


        /// <summary>
        /// 去除前后空格
        /// </summary>
        /// <param name="dt"></param>
        /// <returns></returns>
        public DataTable SetTrim(DataTable dt)
        {

            DataTable newdt = dt.Copy();
            newdt.Clear();//copy dt的框架

            for (int i = 0; i < dt.Rows.Count; i++)
            {
                DataRow newdr = newdt.NewRow();
                DataRow dr = dt.Rows[i];
                foreach (DataColumn column in dt.Columns)
                {
                    if (!string.IsNullOrEmpty(dr[column.ColumnName].ToString()))
                        newdr[column.ColumnName] = dr[column.ColumnName].ToString().Trim();
                }
                newdt.Rows.Add(newdr);
            }
            return newdt;
        }
    }


    public class SendAttendantJson
    {
        public List<SendAttendant> SendAttendant { get; set; }
    }

    public class SendAttendant
    {
        public string PUMP_ID { get; set; }
        public string HOSE_ID { get; set; }
        public string PUMPSRV_REF { get; set; }
        public string REQUEST_ID { get; set; }
        public string PUMP_TOTAL { get; set; }
        public string Remark1 { get; set; }
        public string Remark2 { get; set; }
    }

    public class AttendantResultList
    {
        public List<AttendantResult> AttendantResult { get; set; }
    }
    public class AttendantResult
    {
        public string REQUEST_ID        { get; set; }
        public string PUMP_STARTTIME { get; set; }
        public string PUMP_ENDTTIME { get; set; }
        public string ATTENDANT_ID { get; set; }
        public string ATTENDANT_NAME { get; set; }
        public string ACT_NOZZLE_ID { get; set; }
        public string Remark1 { get; set; }
        public string Remark2 { get; set; }
    }
}
