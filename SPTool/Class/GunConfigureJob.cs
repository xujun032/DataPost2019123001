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
    public  class GunConfigureJob : IJob
    {
     
        SPTool.BLL.GunConfigure gunConfigBLL = new BLL.GunConfigure();
        SPTool.Model.GunConfigure gunConfigModel = new GunConfigure();
        SPTool.Model.FuelItemConfig fuelItemModel = new FuelItemConfig();
        SPTool.Model.SiteInfo SiteInfoModel = new SPTool.Model.SiteInfo();
        SPTool.BLL.SiteInfo SiteInfoBLL = new BLL.SiteInfo();
        public void Execute(IJobExecutionContext context)
        {      
            Form1.WriteLog("开始执行油枪配置变更定时任务");
            List<GunConfigure> oldlist = gunConfigBLL.GetAllList().Tables[0].ToJson().ToList<GunConfigure>();
            List<GunConfigure> newlist = GetNewGunConfig();
            List<GunConfigure> deltelist = new List<GunConfigure>();
            foreach (var newitem in newlist)
            {
                foreach (var olditem in oldlist)
                {
                    if (newitem.TankNo == olditem.TankNo && newitem.GunNo == olditem.GunNo)
                    {
                        deltelist.Add(newitem);
                    }
                }
            }
            foreach (var item in deltelist)
            {
                newlist.Remove(item);
            }
            Form1.WriteLog("共发现" + newlist.Count + "条油枪变更数据");
            if (newlist.Count > 0)
            {
                SiteInfoModel = SiteInfoBLL.GetTopModel();
                foreach (var item in newlist)
                {
                    PostGunConfigure PostGunConfigure = new PostGunConfigure();
                    PostGunConfigure.StationNo = SiteInfoModel.StationNo;
                    PostGunConfigure.TankNo = item.TankNo;
                    PostGunConfigure.GunNo = item.GunNo;
                    PostGunConfigure.SysId = item.SysId;
                    PostJson PostJson = new PostJson();
                    PostJson.GasCode = SiteInfoModel.GasCode;
                    PostJson.SysNo = SiteInfoModel.SysNo;
                    PostJson.Param = PostGunConfigure.ToJson().ToString();
                    string PostJsonStr = PostJson.ToJson().ToString().Replace("\\", "").Replace("Param\":\"{", "Param\":{").Replace("\"}\"}", "\"}}");
                    string url = SiteInfoModel.PostUrl + "GunConfigure";
                    Form1.WriteLog("油枪配置变更发送参数：" + PostJsonStr);
                    PostResult Result = HttpPost.PostHttps(url, PostJsonStr).ToObject<PostResult>();
                    if (Result.Result == 0)
                    {
                        UpdateGunConfig();
                        Form1.WriteLog("油枪配置变更成功！");
                    }
                    else
                    {
                        Form1.WriteLog("油枪配置变更失败！");
                    }


                }
            }


        }

        /// <summary>
        /// 获取当前的油枪配置数据
        /// </summary>
        /// <returns></returns>
        public List<GunConfigure> GetNewGunConfig()
        {
            List<GunConfigure> newlist = new List<GunConfigure>();
            //1.获取枪数据
            SPTool.BLL.FuelItemConfig fuelItemBLL = new BLL.FuelItemConfig();
            //2. 获取面向客户的枪号
            string pump_sql = "SELECT PUMP_ID,HOSE_ID,TANK1_ID FROM FUEL_PUMPS_HOSE";
            DataTable dt2 = DBUtility.DbHelperODBC.Query(pump_sql).Tables[0];
            if (dt2.Rows.Count > 0)
            {
                for (int i = 0; i < dt2.Rows.Count; i++)
                {
                    SPTool.Model.GunConfigure gunConfigModel1 = new GunConfigure();
                    gunConfigModel1.GunNo = (i + 1).ToString();
                    gunConfigModel1.SysId = dt2.Rows[i]["PUMP_ID"].ToString().Trim();
                    gunConfigModel1.PumpId = dt2.Rows[i]["PUMP_ID"].ToString().Trim();
                    gunConfigModel1.TankNo = dt2.Rows[i]["TANK1_ID"].ToString().Trim();
                    gunConfigModel1.HoseId = dt2.Rows[i]["HOSE_ID"].ToString().Trim();
                    newlist.Add(gunConfigModel1);
                }
            }
            dt2.Clear();
            dt2.Dispose();
            return newlist;
        }
        /// <summary>
        /// 更新油枪配置
        /// </summary>
        public void UpdateGunConfig()
        {
            SPTool.BLL.GunConfigure GunConfigureBLL = new BLL.GunConfigure();
            //1.获取枪数据
            SPTool.BLL.FuelItemConfig fuelItemBLL = new BLL.FuelItemConfig();
            //2. 获取面向客户的枪号
            string pump_sql = "SELECT PUMP_ID,HOSE_ID,TANK1_ID FROM FUEL_PUMPS_HOSE";
            DataTable dt2 = DBUtility.DbHelperODBC.Query(pump_sql).Tables[0];
            if (dt2.Rows.Count > 0)
            {
                List<SPTool.Model.GunConfigure> gunlist = new List<GunConfigure>();
                DBUtility.DbHelperSQLite.ExecuteSql("DELETE FROM GunConfigure");
                for (int i = 0; i < dt2.Rows.Count; i++)
                {
                    SPTool.Model.GunConfigure gunConfigModel1 = new GunConfigure();
                    gunConfigModel1.GunNo = (i + 1).ToString();
                    gunConfigModel1.SysId = dt2.Rows[i]["PUMP_ID"].ToString().Trim();
                    gunConfigModel1.PumpId = dt2.Rows[i]["PUMP_ID"].ToString().Trim();
                    gunConfigModel1.TankNo = dt2.Rows[i]["TANK1_ID"].ToString().Trim();
                    gunConfigModel1.HoseId = dt2.Rows[i]["HOSE_ID"].ToString().Trim();
                    gunlist.Add(gunConfigModel1);
                }
                gunConfigBLL.AddList(gunlist);
            }
            dt2.Clear();
            dt2.Dispose();
        }
    }
}
