using Quartz;
using SPTool.Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;

namespace SPTool
{
    [DisallowConcurrentExecution]
    public class GasSaleDataJob : IJob
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
                #region 
                string DBPath = System.Windows.Forms.Application.StartupPath + "\\" + "DataBase.db";
                string BackUppath = "D:\\SPToolDBBackUp";
                if (!Directory.Exists(BackUppath))
                {
                    Directory.CreateDirectory(BackUppath);
                }
                File.Copy(DBPath, BackUppath + "\\" + "DataBase.db", true);
                #endregion
                Form1.WriteLog("开始执行实时销售数据上送定时任务");
                SiteInfoModel = SiteInfoBLL.GetTopModel();
                string str = string.Format("SELECT DISTINCT PUMPSRV_REF, TILLNUM, TOTAL, PUMP_ID, HOSE_ID, POSTED, BARCODE, PRICE, WEIGHT, PUMP_TOTAL, PUMP_CLOSE_COUNTER FROM TILLITEM WHERE PUMP_ID > 0 AND TOTAL > 0 AND POSTED >'{0}'", SiteInfoModel.ActivedDatetime);
                if (!string.IsNullOrEmpty(SiteInfoModel.SaleLastDatetime))
                {
                    string STR1 = string.Format("AND POSTED >'{0}'", SiteInfoModel.SaleLastDatetime);
                    str = str + STR1;
                }
                str = str + "ORDER BY POSTED";
                DataTable dt = DBUtility.DbHelperODBC.Query(str).Tables[0];
                string SaleLastDatetime = "";
                Form1.WriteLog("共发现" + dt.Rows.Count.ToString() + "条销售数据");
                if (dt.Rows.Count > 0)
                {
                    int Index = (int)Math.Ceiling((double)dt.Rows.Count / (double)10);
                    for (int i = 0; i < Index; i++)
                    {
                        DataTable dt10 = GetPagedTable(dt, i + 1);
                        GasSaleData GasSaleData = new GasSaleData();
                        GasSaleData.StationNo = SiteInfoModel.StationNo;
                        List<SaleDataList> SaleDataList = new List<Model.SaleDataList>();
                        for (int j = 0; j < dt10.Rows.Count; j++)
                        {
                            SaleDataList SaleData = new SaleDataList();
                            SaleData.FlowNo = dt10.Rows[j]["TILLNUM"].ToString().Trim();
                            string pumpid = dt10.Rows[j]["PUMP_ID"].ToString().Trim();
                            string hoseid = dt10.Rows[j]["HOSE_ID"].ToString().Trim();
                            gunConfigModel = gunConfigBLL.GetModelByPumpHose(pumpid, hoseid);
                            SaleData.GunNo = gunConfigModel.GunNo;
                            SaleData.OptTime = dt10.Rows[j]["POSTED"].ToString().Trim().ToDate().ToString("yyyy-MM-dd HH:mm:ss");
                            tankConfigModel = tankConfigBLL.GetModel(gunConfigModel.TankNo);
                            SaleData.OilNo = tankConfigModel.OilCode;
                            SaleData.OilAbbreviate = tankConfigModel.OilName;
                            SaleData.Price = Extensions.ToDecimal3(dt10.Rows[j]["PRICE"].ToString().Trim().ToDecimal());
                            SaleData.Qty = Extensions.ToDecimal3(dt10.Rows[j]["WEIGHT"].ToString().Trim().ToDecimal());
                            if(!string.IsNullOrEmpty(dt10.Rows[j]["PUMP_TOTAL"].ToString()))
                            SaleData.Amount = Extensions.ToDecimal3(dt10.Rows[j]["PUMP_TOTAL"].ToString().Trim().ToDecimal());
                            else
                                SaleData.Amount = 0.00000.ToDecimal(3);
                            SaleData.TTC = -1;
                            if(!string.IsNullOrEmpty(dt10.Rows[j]["PUMP_CLOSE_COUNTER"].ToString()))
                            SaleData.EndTotal = Extensions.ToDecimal3(dt10.Rows[j]["PUMP_CLOSE_COUNTER"].ToString().Trim().ToDecimal());
                            else
                                SaleData.EndTotal = 0.00000.ToDecimal(3);
                            SaleData.TransType = "0";
                            SaleData.TankNo = gunConfigModel.TankNo;
                            SaleLastDatetime = SaleData.OptTime;
                            SaleDataList.Add(SaleData);
                        }
                        GasSaleData.SaleDataList = SaleDataList;
                        PostJson PostJson = new PostJson();
                        PostJson.GasCode = SiteInfoModel.GasCode;
                        PostJson.SysNo = SiteInfoModel.SysNo;
                        PostJson.Param = GasSaleData.ToJson();
                        string PostJsonStr = PostJson.ToJson().ToString().Replace("\\", "").Replace("Param\":\"{", "Param\":{").Replace("]}\"}", "]}}");
                        string url = SiteInfoModel.PostUrl + "GasSaleData";
                        Form1.WriteLog("实时销售数据上送发送参数：" + PostJsonStr);
                        PostResult Result = HttpPost.PostHttps(url, PostJsonStr).ToObject<PostResult>();
                        if (Result.Result == 0)
                        {
                            SiteInfoModel.SaleLastDatetime = SaleLastDatetime;
                            SiteInfoBLL.Update(SiteInfoModel);
                            Form1.WriteLog("实时销售数据上送成功！ " + dt10.Rows.Count + "条销售记录");
                        }
                        else
                        {
                            //数据不成功需要入库缓存
                            Form1.WriteLog("实时销售数据上送失败！" + Result.Msg);
                        }
                    }
                }
            }
            catch (Exception me)
            {
                Form1.WriteLog("错误：" + me.StackTrace);
                Form1.WriteLog("实时销售数据上送失败：" + me.Message);
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
                    newdr[column.ColumnName] = dr[column.ColumnName];
                }
                newdt.Rows.Add(newdr);
            }
            return newdt;
        }
    }
}
