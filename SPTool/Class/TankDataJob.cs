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
    public class TankDataJob : IJob
    {
        SPTool.BLL.TankConfigure tankConfigBLL = new BLL.TankConfigure();
        SPTool.Model.TankConfigure tankConfigModel = new TankConfigure();
        SPTool.Model.SiteInfo SiteInfoModel = new SPTool.Model.SiteInfo();
        SPTool.BLL.SiteInfo SiteInfoBLL = new BLL.SiteInfo();
        public void Execute(IJobExecutionContext context)
        {
            try
            {
                Form1.WriteLog("开始执行油罐液位仪上送定时任务");
                TankData TankData = new TankData();
                List<TankDataList> TankDataList = new List<Model.TankDataList>();
                SiteInfoModel = SiteInfoBLL.GetTopModel();
                string sql = "SELECT A.* FROM FUEL_TANK_READING A WHERE 1 > (SELECT  COUNT(*)  FROM FUEL_TANK_READING  WHERE SHIFT_CONTROL_ID > A.SHIFT_CONTROL_ID)";
                if (!string.IsNullOrEmpty(SiteInfoModel.TankLastDatetime))
                {
                    string WHERE = string.Format("AND A.END_READING>'{0}'", SiteInfoModel.TankLastDatetime);
                    sql = sql + WHERE;
                }
                DataTable dt = DBUtility.DbHelperODBC.Query(sql).Tables[0];
                string TankLastDatetime = "";
                Form1.WriteLog("共发现" + dt.Rows.Count.ToString() + "条液位仪数据");
                if (dt.Rows.Count > 0)
                {
                    TankData.StationNo = SiteInfoModel.StationNo;
                    TankData.FlowNo = GetNewFlowNo(SiteInfoModel.TankLastFlowNo);
                    for (int j = 0; j < dt.Rows.Count; j++)
                    {
                        if (dt.Rows[j]["CLOSE_QTY"].ToString()!="0" && dt.Rows[j]["OPEN_QTY"].ToString()!="0")
                        {
                            TankDataList TankData1 = new TankDataList();
                            TankData1.TankNo = dt.Rows[j]["TANK_ID"].ToString().Trim();
                            if (!string.IsNullOrEmpty(dt.Rows[j]["END_READING"].ToString()))
                                TankData1.ReadTime = dt.Rows[j]["END_READING"].ToString().Trim().ToDate().ToString("yyyy-MM-dd HH:mm:ss");
                            else
                                TankData1.ReadTime = GetStartBatchTime(dt.Rows[j]["SHIFT_CONTROL_ID"].ToString().Trim());
                            TankLastDatetime = TankData1.ReadTime;
                            tankConfigModel = tankConfigBLL.GetModel(TankData1.TankNo);
                            TankData1.OilNo = tankConfigModel.OilCode;
                            TankData1.OilAbbreviate = tankConfigModel.OilName;
                            TankData1.Stock = Extensions.ToDecimal3(dt.Rows[j]["CLOSE_QTY"].ToString().Trim().ToDecimal());
                            if (!string.IsNullOrEmpty(dt.Rows[j]["NET_STICK"].ToString()))
                                TankData1.OilLevel = Extensions.ToDecimal3(dt.Rows[j]["NET_STICK"].ToString().Trim().ToDecimal());
                            else
                                TankData1.OilLevel = 0.00000.ToDecimal(3);

                            if (!string.IsNullOrEmpty(dt.Rows[j]["WATER_STICK"].ToString()))
                                TankData1.WaterLevel = Extensions.ToDecimal3(dt.Rows[j]["WATER_STICK"].ToString().Trim().ToDecimal());
                            else
                                TankData1.WaterLevel = 0.00000.ToDecimal(3);


                            if (!string.IsNullOrEmpty(dt.Rows[j]["TANK_TEMP"].ToString()))
                                TankData1.Temp = Extensions.ToDecimal3(dt.Rows[j]["TANK_TEMP"].ToString().Trim().ToDecimal());
                            else
                                TankData1.Temp = 0.00000.ToDecimal(3);


                            decimal TankEmpty = (tankConfigModel.TankVolume - TankData1.Stock).ToDecimal();
                            TankData1.TankEmpty = Extensions.ToDecimal3(TankEmpty);
                            TankData1.State = 0;
                            TankDataList.Add(TankData1);
                        }
                    }
                    TankData.TankDataList = TankDataList;
                    PostJson PostJson = new PostJson();
                    PostJson.GasCode = SiteInfoModel.GasCode;
                    PostJson.SysNo = SiteInfoModel.SysNo;
                    PostJson.Param = TankData.ToJson();
                    string PostJsonStr = PostJson.ToJson().ToString().Replace("\\", "").Replace("Param\":\"{", "Param\":{").Replace("]}\"}", "]}}");
                    string url = SiteInfoModel.PostUrl + "GasTankData";
                    Form1.WriteLog("油罐液位仪数据上送发送参数：" + PostJsonStr);
                    PostResult Result = HttpPost.PostHttps(url, PostJsonStr).ToObject<PostResult>();
                    if (Result.Result == 0)
                    {
                        SiteInfoModel.TankLastDatetime = TankLastDatetime;
                        SiteInfoModel.TankLastFlowNo = TankData.FlowNo;
                        SiteInfoBLL.Update(SiteInfoModel);
                        Form1.WriteLog("油罐液位仪数据上送成功！ " + dt.Rows.Count + "条油罐库存记录");
                    }
                    else
                    {
                        //数据不成功需要入库缓存
                        Form1.WriteLog("油罐液位仪数据上送失败！" + Result.Msg);
                    }
                }
            }
            catch (Exception me)
            {
                Form1.WriteLog("错误：" + me.StackTrace);
                Form1.WriteLog("液位仪数据上送错误：" + me.Message);
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

        public string GetStartBatchTime(string SHIFT_CONTROL_ID)
        {
            string str = string.Format("SELECT * FROM FUEL_SHIFT_CONTROL WHERE SHIFT_CONTROL_ID={0}", SHIFT_CONTROL_ID);
          DataTable dt=  DBUtility.DbHelperODBC.Query(str).Tables[0];
            return dt.Rows[0]["SHIFT_START"].ToString().Trim();
        }


    }



}
