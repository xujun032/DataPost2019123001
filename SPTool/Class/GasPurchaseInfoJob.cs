using Quartz;
using SPTool.Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace SPTool
{
    [DisallowConcurrentExecution]
    public class GasPurchaseInfoJob : IJob
    {
        SPTool.BLL.TankConfigure tankConfigBLL = new BLL.TankConfigure();
        SPTool.Model.TankConfigure tankConfigModel = new TankConfigure();
        SPTool.Model.SiteInfo SiteInfoModel = new SPTool.Model.SiteInfo();
        SPTool.BLL.SiteInfo SiteInfoBLL = new BLL.SiteInfo();
        SPTool.BLL.FuelItemConfig FuelItemConfigBLL = new BLL.FuelItemConfig();
        SPTool.Model.FuelItemConfig FuelItemConfigModel = new Model.FuelItemConfig();
        public void Execute(IJobExecutionContext context)
        {
            try
            {
                Form1.WriteLog("开始执行油站进油上送定时任务");
                //GasPurchaseInfo GasPurchaseInfo = new GasPurchaseInfo();
                //List<PurchDetailList> GasPurchaseInfoLiist = new List<PurchDetailList>();
                SiteInfoModel = SiteInfoBLL.GetTopModel();
                string sql = "SELECT T1.SERNUM,T1.DELIVERY_DATE,T1.SHIFT_CONTROL_ID ,T1.DEPARTURE_TIME ,T3.SUPPLIERNAME,T1.EXTREF,T4.GRADE_ID,T4.INVOICE_VOLUME,T1.TRUCK_NUMBER,T1.DRIVER_NAME,T1.OPERATOR_ID,T1.NOTE FROM FUEL_TANK_DELIVERY_HEADER T1 LEFT JOIN SUPPLIER T3 ON T3.SUPPLIERID = T1.SUPPLIER_ID LEFT JOIN FUEL_TANK_DELIVERY_DOCUMENT T4 ON T4.HEADERID = T1.SERNUM WHERE T1.STATUS = 'A'AND T4.INVOICE_VOLUME > 0 ";
                if (!string.IsNullOrEmpty(SiteInfoModel.PurchaseInfoLastDatetime))
                {
                    string WHERE = string.Format("AND T1.DEPARTURE_TIME>'{0}'", SiteInfoModel.PurchaseInfoLastDatetime);
                    sql = sql + WHERE;
                }
                if (!string.IsNullOrEmpty(SiteInfoModel.ActivedDatetime))
                {
                    string WHERE = string.Format("AND T1.DEPARTURE_TIME>'{0}'", SiteInfoModel.ActivedDatetime);
                    sql = sql + WHERE;
                }
                sql = sql + "ORDER BY T1.DEPARTURE_TIME";
                DataTable dt = DBUtility.DbHelperODBC.Query(sql).Tables[0];
           
                Form1.WriteLog("共发现"+ dt.Rows.Count.ToString()+"条进油数据"  );
                string PurchaseInfoLastDatetime = "";
                if (dt.Rows.Count > 0)
                {
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        GasPurchaseInfo GasPurchaseInfo = new GasPurchaseInfo();
                        List<PurchDetailList> PurchDetailList = new List<Model.PurchDetailList>();
                        GasPurchaseInfo.StationNo = SiteInfoModel.StationNo;
                        GasPurchaseInfo.FlowNo = dt.Rows[i]["SERNUM"].ToString().Trim();
                        GasPurchaseInfo.SaleDate = dt.Rows[i]["DELIVERY_DATE"].ToString().Trim().ToDate().ToString("yyyy-MM-dd");
                        GasPurchaseInfo.ShiftNo = 2;
                        GasPurchaseInfo.ReceiveDate = dt.Rows[i]["DEPARTURE_TIME"].ToString().Trim().ToDate().ToString("yyyy-MM-dd");
                        GasPurchaseInfo.MakeTime = dt.Rows[i]["DEPARTURE_TIME"].ToString().Trim().ToDate().ToString("yyyy-MM-dd HH:mm:ss");
                        if (!string.IsNullOrEmpty(dt.Rows[i]["SUPPLIERNAME"].ToString()))
                            GasPurchaseInfo.ProviderName = dt.Rows[i]["SUPPLIERNAME"].ToString().Trim();
                        GasPurchaseInfo.BillNo = dt.Rows[i]["EXTREF"].ToString().Trim();
                        //通过油品Grade查询油品BARCODE
                        string str1 = string.Format("SELECT BARCODE FROM ITEM WHERE GRADE={0}", dt.Rows[i]["GRADE_ID"].ToString().Trim());
                        string barcode = DBUtility.DbHelperODBC.GetSingle(str1).ToString();
                        FuelItemConfigModel = FuelItemConfigBLL.GetModelByBarcode(barcode);
                        GasPurchaseInfo.OilNo = FuelItemConfigModel.OilCode;
                        GasPurchaseInfo.OilAbbreviate = FuelItemConfigModel.OilName;
                        GasPurchaseInfo.SendVt = Extensions.ToDecimal3(dt.Rows[i]["INVOICE_VOLUME"].ToString().Trim().ToDecimal());
                        if (!string.IsNullOrEmpty(dt.Rows[i]["TRUCK_NUMBER"].ToString()))
                            GasPurchaseInfo.CarNo = dt.Rows[i]["TRUCK_NUMBER"].ToString().Trim();
                        if (!string.IsNullOrEmpty(dt.Rows[i]["DRIVER_NAME"].ToString()))
                            GasPurchaseInfo.MotorMan = dt.Rows[i]["DRIVER_NAME"].ToString().Trim();
                        else
                            GasPurchaseInfo.MotorMan ="";
                        GasPurchaseInfo.EmplNo = dt.Rows[i]["OPERATOR_ID"].ToString().Trim();
                        if (!string.IsNullOrEmpty(dt.Rows[i]["NOTE"].ToString()))
                            GasPurchaseInfo.Remark = dt.Rows[i]["NOTE"].ToString().Trim();
                        else
                            GasPurchaseInfo.Remark = "";
                        PurchaseInfoLastDatetime = GasPurchaseInfo.MakeTime;
                        GasPurchaseInfo.GaugerNo = "";
                        GasPurchaseInfo.Guager = "";
                        GasPurchaseInfo.EmplName = "";
                   

                        //查询子项目PurchDetailList
                        string PurchDetailSQLList = string.Format("SELECT TANK_ID,DELIVERY_DATE,TANK_TEMPERATURE,START_STICK,WATER_STICK,START_VOLUME, DELIVERY_END_TIME, END_STICK, END_WATER_STICK, END_TANK_TEMPERATURE, END_VOLUME, SALES_QTY, DELIVERY_VOLUME FROM FUEL_TANK_DELIVERY WHERE HEADERID = {0}", GasPurchaseInfo.FlowNo);
                        DataTable dt1 = DBUtility.DbHelperODBC.Query(PurchDetailSQLList).Tables[0];
                        if (dt1.Rows.Count > 0)
                        {
                            GasPurchaseInfo.PurchDetailList = GetPurchDetailList(dt1);
                        }
                        PostJson PostJson = new PostJson();
                        PostJson.GasCode = SiteInfoModel.GasCode;
                        PostJson.SysNo = SiteInfoModel.SysNo;
                        PostJson.Param = GasPurchaseInfo.ToJson();
                        string PostJsonStr = PostJson.ToJson().ToString().Replace("\\", "").Replace("Param\":\"{", "Param\":{").Replace("]}\"}", "]}}").Replace(":0.0", ":0.000");
                        string url = SiteInfoModel.PostUrl + "GasPurchaseInfo";
                        Form1.WriteLog("油站进油数据上送发送参数：" + PostJsonStr);
                        PostResult Result = HttpPost.PostHttps(url, PostJsonStr).ToObject<PostResult>();
                        if (Result.Result == 0)
                        {
                            SiteInfoModel.PurchaseInfoLastDatetime = PurchaseInfoLastDatetime;
                            SiteInfoBLL.Update(SiteInfoModel);
                            Form1.WriteLog("油站进油数据上送成功！ ");
                        }
                        else
                        {
                            Form1.WriteLog("油站进油数据数据上送失败！" + Result.Msg);
                        }
                    }
                }
            }
            catch (Exception me)
            {
                Form1.WriteLog("错误：" + me.StackTrace);
                Form1.WriteLog("错误：" + me.Message);
            }

        }

        /// <summary>
        /// 获取流水号
        /// </summary>
        /// <param name="TankLastFlowNo"></param>
        /// <returns></returns>
        public string GetNewFlowNo(string TankLastFlowNo)
        {
            if (string.IsNullOrEmpty(TankLastFlowNo))
            {
                return DateTime.Now.ToString("yyyyMMdd") + "01";
            }
            else
            {
                if (TankLastFlowNo.Substring(0, 8) == DateTime.Now.ToString("yyyyMMdd"))
                {
                    string batch = TankLastFlowNo.Substring(TankLastFlowNo.Length - 2, 2);
                    return DateTime.Now.ToString("yyyyMMdd") + (Convert.ToInt32(batch) + 1).ToString().PadLeft(2, '0');
                }
                else
                {
                    return DateTime.Now.ToString("yyyyMMdd") + "01";
                }

            }

        }

        /// <summary>
        /// 获取收油明细行项目
        /// </summary>
        /// <returns></returns>
        public List<PurchDetailList> GetPurchDetailList(DataTable dt1)
        {
            List<PurchDetailList> detailLists = new List<PurchDetailList>();
            for (int j = 0; j < dt1.Rows.Count; j++)
            {
                PurchDetailList PurchDetail = new Model.PurchDetailList();
                PurchDetail.TankNo = dt1.Rows[j]["TANK_ID"].ToString().Trim();
                PurchDetail.StartTime = dt1.Rows[j]["DELIVERY_DATE"].ToString().Trim().ToDate().ToString("yyyy-MM-dd HH:mm:ss");
                if (!string.IsNullOrEmpty(dt1.Rows[j]["TANK_TEMPERATURE"].ToString()))
                    PurchDetail.StartTemp = Extensions.ToDecimal3(dt1.Rows[j]["TANK_TEMPERATURE"].ToString().Trim().ToDecimal());
                if (!string.IsNullOrEmpty(dt1.Rows[j]["START_STICK"].ToString()))
                    PurchDetail.StartOilLen = Extensions.ToDecimal3(dt1.Rows[j]["START_STICK"].ToString().Trim().ToDecimal());
                else
                    PurchDetail.StartOilLen = 0.00000.ToDecimal(3);
                if (!string.IsNullOrEmpty(dt1.Rows[j]["WATER_STICK"].ToString()))
                    PurchDetail.StartWaterLen = Extensions.ToDecimal3(dt1.Rows[j]["WATER_STICK"].ToString().Trim().ToDecimal());
                if (!string.IsNullOrEmpty(dt1.Rows[j]["START_VOLUME"].ToString()))
                    PurchDetail.StartQtyLiter = Extensions.ToDecimal3(dt1.Rows[j]["START_VOLUME"].ToString().Trim().ToDecimal());
                PurchDetail.EndTime = dt1.Rows[j]["DELIVERY_END_TIME"].ToString().Trim().ToDate().ToString("yyyy-MM-dd HH:mm:ss");
                if (!string.IsNullOrEmpty(dt1.Rows[j]["END_STICK"].ToString()))
                    PurchDetail.EndOilLen = Extensions.ToDecimal3(dt1.Rows[j]["END_STICK"].ToString().Trim().ToDecimal());
                if (!string.IsNullOrEmpty(dt1.Rows[j]["END_WATER_STICK"].ToString()))
                    PurchDetail.NewWaterLen = Extensions.ToDecimal3(dt1.Rows[j]["END_WATER_STICK"].ToString().Trim().ToDecimal());
                if (!string.IsNullOrEmpty(dt1.Rows[j]["END_TANK_TEMPERATURE"].ToString()))
                    PurchDetail.EndTemp = Extensions.ToDecimal3(dt1.Rows[j]["END_TANK_TEMPERATURE"].ToString().Trim().ToDecimal());
                if (!string.IsNullOrEmpty(dt1.Rows[j]["END_VOLUME"].ToString()))
                    PurchDetail.EndQtyLiter = Extensions.ToDecimal3(dt1.Rows[j]["END_VOLUME"].ToString().Trim().ToDecimal());
                if (!string.IsNullOrEmpty(dt1.Rows[j]["SALES_QTY"].ToString()))
                    PurchDetail.PeriodSaleQty = Extensions.ToDecimal3(dt1.Rows[j]["SALES_QTY"].ToString().Trim().ToDecimal());
                if (!string.IsNullOrEmpty(dt1.Rows[j]["DELIVERY_VOLUME"].ToString()))
                    PurchDetail.NetVol = Extensions.ToDecimal3(dt1.Rows[j]["DELIVERY_VOLUME"].ToString().Trim().ToDecimal());
                PurchDetail.Remark = "";
                detailLists.Add(PurchDetail);
            }
            return detailLists;

        }


    }



}
