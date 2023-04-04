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
    public class TankConfigureJob : IJob
    {
        SPTool.BLL.TankConfigure tankConfigBLL = new BLL.TankConfigure();
        SPTool.Model.TankConfigure tankConfigModel = new TankConfigure();
        SPTool.Model.FuelItemConfig fuelItemModel = new FuelItemConfig();
        SPTool.Model.SiteInfo SiteInfoModel = new SPTool.Model.SiteInfo();
        SPTool.BLL.SiteInfo SiteInfoBLL = new BLL.SiteInfo();
        public void Execute(IJobExecutionContext context)
        {
            Form1.WriteLog("开始执行油罐配置变更定时任务");
            List<TankConfigure> oldlist = tankConfigBLL.GetAllList().Tables[0].ToJson().ToList<TankConfigure>();
            List<TankConfigure> newlist = GetNewTankConfig();
            List<TankConfigure> deltelist = new List<TankConfigure>();
            foreach (var newitem in newlist)
            {
                foreach (var olditem in oldlist)
                {
                    if (newitem.TankNo == olditem.TankNo && newitem.OilCode == olditem.OilCode)
                    {
                        deltelist.Add(newitem);
                    }
                }
            }
            foreach (var item in deltelist)
            {
                newlist.Remove(item);
            }
            Form1.WriteLog("共发现" + newlist.Count + "条油罐变更数据");
            if (newlist.Count > 0)
            {
                SiteInfoModel = SiteInfoBLL.GetTopModel();
                foreach (var item in newlist)
                {
                    PostTankConfigure PostTankConfigure = new PostTankConfigure();
                    PostTankConfigure.StationNo = SiteInfoModel.StationNo;
                    PostTankConfigure.TankNo = item.TankNo;
                    PostTankConfigure.TankName = item.TankName;
                    decimal TankVolume = Extensions.ToDecimal3(0);
                    if (item.TankVolume != null)
                    {
                        TankVolume = Extensions.ToDecimal3((decimal)item.TankVolume);
                    }
                    PostTankConfigure.TankVolume = TankVolume;
                    PostTankConfigure.OilCode = item.OilCode;
                    PostTankConfigure.OilName = item.OilName;
                    PostJson PostJson = new PostJson();
                    PostJson.GasCode = SiteInfoModel.GasCode;
                    PostJson.SysNo = SiteInfoModel.SysNo;
                    PostJson.Param = PostTankConfigure.ToJson().ToString();
                    string PostJsonStr = PostJson.ToJson().ToString().Replace("\\", "").Replace("Param\":\"{", "Param\":{").Replace("\"}\"}", "\"}}");
                    string url = SiteInfoModel.PostUrl + "TankConfigure";
                    Form1.WriteLog("油罐配置变更发送参数：" + PostJsonStr);
                    PostResult Result = HttpPost.PostHttps(url, PostJsonStr).ToObject<PostResult>();
                    if (Result.Result == 0)
                    {
                        UpdateTankConfig();
                        Form1.WriteLog("油罐配置变更成功！");
                    }
                    else
                    {
                        //数据不成功需要入库缓存
                        Form1.WriteLog("油罐配置变更失败！");
                    }
                }
            }
        }

        /// <summary>
        /// 获取当前的油罐配置数据
        /// </summary>
        /// <returns></returns>
        public List<TankConfigure> GetNewTankConfig()
        {
            List<TankConfigure> newlist = new List<TankConfigure>();
            //1.获取油罐数据
            SPTool.BLL.FuelItemConfig fuelItemBLL = new BLL.FuelItemConfig();
            string Tank_Sql = "select t.tank_id,t.tank_name,t.grade_plu,t1.barcode,t.volume_qty from fuel_tanks t left join item t1 on t.grade_plu=t1.grade";
            DataTable dt = DBUtility.DbHelperODBC.Query(Tank_Sql).Tables[0];
            if (dt.Rows.Count > 0)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    if (!string.IsNullOrEmpty(dt.Rows[i]["BARCODE"].ToString()))
                    {
                        SPTool.Model.TankConfigure tankConfigModel1 = new TankConfigure();
                        fuelItemModel = fuelItemBLL.GetModelByBarcode(dt.Rows[i]["BARCODE"].ToString());
                        if (fuelItemModel != null)
                        {
                            tankConfigModel1.TankNo = dt.Rows[i]["TANK_ID"].ToString().Trim();
                            tankConfigModel1.TankName = dt.Rows[i]["TANK_NAME"].ToString().Trim();
                            tankConfigModel1.TankVolume = dt.Rows[i]["VOLUME_QTY"].ToString().ToDecimal(3);
                            tankConfigModel1.OilCode = fuelItemModel.OilCode.Trim();
                            tankConfigModel1.OilName = fuelItemModel.OilName.Trim();
                            tankConfigModel1.BarCode = dt.Rows[i]["BARCODE"].ToString().Trim();
                            tankConfigModel1.Remark1 = null;
                            tankConfigModel1.Remark2 = null;
                            newlist.Add(tankConfigModel1);
                        }
                    }           
                }
            }
            dt.Clear();
            dt.Dispose();
            return newlist;
        }


        /// <summary>
        /// 配置发生变化时做更新
        /// </summary>
        public void UpdateTankConfig()
        {
            //1.获取油罐数据
            SPTool.BLL.FuelItemConfig fuelItemBLL = new BLL.FuelItemConfig();
            string Tank_Sql = "select t.tank_id,t.tank_name,t.grade_plu,t1.barcode,t.volume_qty from fuel_tanks t left join item t1 on t.grade_plu=t1.grade";
            DataTable dt = DBUtility.DbHelperODBC.Query(Tank_Sql).Tables[0];
            if (dt.Rows.Count > 0)
            {
                List<SPTool.Model.TankConfigure> tanklist = new List<TankConfigure>();
                DBUtility.DbHelperSQLite.ExecuteSql("DELETE FROM TankConfigure");
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    if (!string.IsNullOrEmpty(dt.Rows[i]["BARCODE"].ToString()))
                    {
                        SPTool.Model.TankConfigure tankConfigModel1 = new TankConfigure();
                        fuelItemModel = fuelItemBLL.GetModelByBarcode(dt.Rows[i]["BARCODE"].ToString());
                        if (fuelItemModel != null)
                        {
                            tankConfigModel1.TankNo = dt.Rows[i]["TANK_ID"].ToString().Trim();
                            tankConfigModel1.TankName = dt.Rows[i]["TANK_NAME"].ToString().Trim();
                            tankConfigModel1.TankVolume = dt.Rows[i]["VOLUME_QTY"].ToString().ToDecimal(3);
                            tankConfigModel1.OilCode = fuelItemModel.OilCode.Trim();
                            tankConfigModel1.OilName = fuelItemModel.OilName.Trim();
                            tankConfigModel1.BarCode = dt.Rows[i]["BARCODE"].ToString().Trim();
                            tankConfigModel1.Remark1 = null;
                            tankConfigModel1.Remark2 = null;
                            tanklist.Add(tankConfigModel1);
                        }
                    }

                }
                tankConfigBLL.AddList(tanklist);
            }
            dt.Clear();
            dt.Dispose();
        }
    }
}
