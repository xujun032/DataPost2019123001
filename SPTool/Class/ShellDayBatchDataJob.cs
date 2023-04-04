
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
    public class ShellDayBatchDataJob : IJob
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
                Form1.WriteLog("开始执行中心模式营业日数据上送定时任务");
                SiteInfoModel = SiteInfoBLL.GetTopModel();
                string str = "SELECT DAY_BATCH_DATE, DAY_BATCH_OPEN,DAY_BATCH_READY,DAY_BATCH_CLOSE,STATUS ,EXPORTSTATUS FROM DAYBATCH ORDER BY DAY_BATCH_DATE DESC";
                DataTable dt = DBUtility.DbHelperODBC.Query(str).Tables[0];
                if (dt.Rows.Count > 2)
                {
                    DataTable dt10 = DtSelectTop(7, dt);
                    var jsonData = new
                    {
                        SITEDAYBATCH = dt10,
                    };
                    string message = SendRes("SITEDAYBATCH", SiteInfoModel.StationNo, jsonData);
                    IMessageProduct product = new MessageProduct();
                    string queueName = "ConsumerWorkerService.ResParameter, ConsumerWorkerService_Site2HOSMessage";
                    product.Publish(message, queueName);
                    Form1.WriteLog("消息已经发送出:" + message);
                }
                else
                {
                    Form1.WriteLog("数据不完整，营业日数据将在二天后上传！" );
                }
            }
            catch (Exception me)
            {
                Form1.WriteLog("错误：" + me.StackTrace);
                Form1.WriteLog("中心模式营业日上送失败：" + me.Message);
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
        public string SendRes(string type,string siteid, object data)
        {
            ResParameter res = new ResParameter { GUID=Guid.NewGuid().ToString()   ,Type = type, SendDateTime= DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss:fff"),SiteId = siteid, Data = data };
            return res.ToJson().ToString();
        }


        #region DataTable处理

        /// <summary> 
        /// 获取DataTable前几条数据 
        /// </summary> 
        /// <param name="TopItem">前N条数据</param> 
        /// <param name="oDT">源DataTable</param> 
        /// <returns></returns> 
        public static DataTable DtSelectTop(int TopItem, DataTable oDT)
        {
            if (oDT.Rows.Count < TopItem) return oDT;

            DataTable NewTable = oDT.Clone();
            DataRow[] rows = oDT.Select("1=1");
            for (int i = 0; i < TopItem; i++)
            {
                NewTable.ImportRow((DataRow)rows[i]);
            }
            return NewTable;
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

        #endregion
    }
}
