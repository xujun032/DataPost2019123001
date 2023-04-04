using C5;
using Macrosage.RabbitMQ.Server.Customer;
using Macrosage.RabbitMQ.Server.Product;
using Microsoft.Win32;
using Quartz;
using RabbitMQ.Client;
using SPTool.Model;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.Odbc;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Windows.Forms;

namespace SPTool
{
    public partial class Form1 : Form
    {


        SPTool.Model.SiteInfo SiteInfoModel = new SPTool.Model.SiteInfo();
        SPTool.BLL.SiteInfo SiteInfoBLL = new SPTool.BLL.SiteInfo();
        SPTool.BLL.FuelItemConfig fuelItemBLL = new BLL.FuelItemConfig();
        SPTool.Model.FuelItemConfig fuelItemModel = new FuelItemConfig();
        SPTool.BLL.TankConfigure tankConfigBLL = new BLL.TankConfigure();
        SPTool.Model.TankConfigure tankConfigModel = new TankConfigure();
        SPTool.BLL.GunConfigure gunConfigBLL = new BLL.GunConfigure();
        SPTool.Model.GunConfigure gunConfigModel = new GunConfigure();
        string[] arg;
        public Form1(string[] args)
        {
            Microsoft.Win32.SystemEvents.SessionEnding += SessionEndingEvent;
            InitializeComponent();
            button1.Visible = false;
            label1.Visible = false;
            textBox1.Visible = false;
            label4.Visible = false;
            textBox3.Visible = false;
            arg = args;
        }
        public Form1()
        {
            Microsoft.Win32.SystemEvents.SessionEnding += SessionEndingEvent;
            InitializeComponent();
            button1.Visible = false;
            label1.Visible = false;
            textBox1.Visible = false;
        }

        private void Form1_Load(object sender, EventArgs e)
        {
  
            //IMessageCustomer Customer = new MessageCustomer();
            //Customer.StartListening("186ServerQueen");
            //Customer.ReceiveMessageCallback = message =>
            //{
            //    //客户端处理消息（打印）
            //  ResParameter186 res = message.ToJson().ToString().ToObject<ResParameter186>();
            //   // string dd = message.Replace("\"", "");
            //    return true;
            //};
            this.Text = this.Text + "V1.60";
            byte ff = (byte)103;
            try
            {

                AutoStart(true);
                string temp = DBUtility.DbHelperODBC.connectionString;
                if (temp.Contains("C:") || temp.Contains("c:"))
                {
                    WriteText("数据库路径：c:\\Office\\db\\Office.gdb");
                }
                else
                {
                    WriteText("数据库路径：d:\\Office\\db\\Office.gdb");
                }
                WriteText("===============工具已启动==============");
            }
            catch (Exception me)
            {
                WriteText("错误：获取数据库路径失败!       " + me.Message);
            }
            try
            {
                SiteInfoModel = SiteInfoBLL.GetTopModel();
                if (SiteInfoModel.isActived == 0)
                {
                    this.Text = this.Text + "   未激活";
                    FirstGasConfigure(SiteInfoModel.ModelType);
                }
                else
                {
                    if (arg != null)
                    {
                        foreach (var item in arg)
                        {
                            if (item == "-a" && SiteInfoModel.ModelType == 1)
                            {
                                GetGasConfigure();
                                SynchronizeConfig();
                            }
                            if (item == "-d")
                            {
                                this.label2.Visible = true;
                                this.dateTimePicker1.Visible = true;
                                this.label3.Visible = true;
                                this.textBox2.Visible = true;
                                this.button2.Visible = true;
                                label4.Visible = true;
                                textBox3.Visible = true;
                                groupBox1.Visible = true;
                                radioButton1.Visible = true;
                                radioButton2.Visible = true;
                                radioButton3.Visible = true;
                                button3.Visible = true;
                                button4.Visible = true;
                                if (SiteInfoModel.ModelType == 1)
                                    radioButton1.Checked = true;
                                if (SiteInfoModel.ModelType == 2)
                                    radioButton2.Checked = true;
                                if (SiteInfoModel.ModelType == 3)
                                    radioButton3.Checked = true;


                            }
                        }
                    }

                    this.Text = this.Text + "   站点编码:" + SiteInfoModel.StationNo + "   已激活";
                    button1.Enabled = false;
                    label1.Visible = false;
                    textBox1.Visible = false;
                    StartTask(SiteInfoModel.ModelType);
                }
            }
            catch (Exception me)
            {

                WriteText(me.Message);
            }
        }
        /// <summary>
        /// 根据不同模式执行不同的任务
        /// </summary>
        /// <param name="Type"></param>
        public void StartTask(int Type)
        {
            if (Type == 1)
            {
                //1为政府追溯模式
                WriteText("===============政府追溯模式==============");
                Task.TankDataRun(Convert.ToInt32(SiteInfoModel.TankDataInterval));
                Task.GasSaleDataRun(Convert.ToInt32(SiteInfoModel.SaleDataInterval));
                Task.TankConfigureRun(Convert.ToInt32(SiteInfoModel.TankInterval));
                Task.GunConfigureRun(Convert.ToInt32(SiteInfoModel.GunInterval));
                Task.GasPurchaseInfoRun(Convert.ToInt32(SiteInfoModel.PurchaseInfoInterval));
            }
            if (Type == 2)
            {
                // 2为壳牌内部抽取模式
                WriteText("===============中心模式==============");
                Task.ShellGasSaleDataRun(Convert.ToInt32(SiteInfoModel.SaleDataInterval));
                Task.ShelDayBatchDataRun(30);
            }
            if (Type == 3)
            {
                //政府追溯模式+壳牌内部抽取模式
                this.Text = this.Text + "------" + " 混合模式";
                Task.TankDataRun(Convert.ToInt32(SiteInfoModel.TankDataInterval));
                Task.GasSaleDataRun(Convert.ToInt32(SiteInfoModel.SaleDataInterval));
                Task.TankConfigureRun(Convert.ToInt32(SiteInfoModel.TankInterval));
                Task.GunConfigureRun(Convert.ToInt32(SiteInfoModel.GunInterval));
                Task.GasPurchaseInfoRun(Convert.ToInt32(SiteInfoModel.PurchaseInfoInterval));
                Task.ShellGasSaleDataRun(Convert.ToInt32(SiteInfoModel.SaleDataInterval));
                Task.ShelDayBatchDataRun(30);
            }
            Task.DeleteLogRun(24);
        }

        /// <summary>
        /// 根据不同模式下站点初始化 1.淄博模式，2，中心模式，3 淄博和中心混合模式
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void FirstGasConfigure(int Type)
        {
            string siteid = GetSiteIDByRegistry();
            WriteText("获取到站点:" + siteid + "   成功！");
            try
            {
                GetGasConfigure();
                string path = "C:\\eai\\Runtime\\Config\\client\\momclient.properties";
                if (File.Exists(path))
                {
                    if (Type == 1)
                    {
                        string StationNo = siteid;
                        SiteInfoModel.StationNo = StationNo;
                        //获取StationName
                        string sql = "SELECT * FROM STORE";

                        DataTable dt2 = DBUtility.DbHelperODBC.Query(sql).Tables[0];

                        SiteInfoModel.StationName = dt2.Rows[0]["STORE_NAME"].ToString();
                        SiteInfoModel.PostUrl = textBox1.Text.ToString().Trim();
                        PostJson PostJson = GetGasConfigureJson();
                        string PostJsonStr = PostJson.ToJson().ToString().Replace("\\", "").Replace("Param\":\"{", "Param\":{").Replace("]}\"}", "]}}");
                        string url = SiteInfoModel.PostUrl + "GasConfigure";
                        PostResult Result = HttpPost.PostHttps(url, PostJsonStr).ToObject<PostResult>();
                        if (Result.Result == 0)
                        {
                            WriteText("网关返回成功");
                            SiteInfoModel.isActived = 1;
                            SiteInfoModel.ActivedDatetime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                            if (SiteInfoBLL.Update(SiteInfoModel))
                            {
                                WriteText("站点:" + SiteInfoModel.StationNo + "   设备配置初始化成功！");
                                WriteText("注册成功，请关闭我再次重新打开应用");
                                MessageBox.Show("注册成功，请关闭我再次重新打开应用");
                                button1.Enabled = false;
                                label1.Visible = false;
                                textBox1.Visible = false;
                            }
                        }
                        else
                        {
                            WriteText(Result.Msg);
                        }
                    }

                    if (Type == 2)
                    {
                        string StationNo = siteid;
                        SiteInfoModel.StationNo = StationNo;
                        //获取StationName
                        string sql = "SELECT * FROM STORE";
                        DataTable dt2 = DBUtility.DbHelperODBC.Query(sql).Tables[0];

                        SiteInfoModel.StationName = dt2.Rows[0]["STORE_NAME"].ToString().Trim();
                        WriteText(SiteInfoModel.StationName);
                        SiteInfoModel.isActived = 1;
                        SiteInfoModel.ActivedDatetime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                        var jsonData = new
                        {
                            SiteID = SiteInfoModel.StationNo,
                            SiteName = SiteInfoModel.StationName
                        };

                        if (SiteInfoBLL.Update(SiteInfoModel))
                        {
                            string message = SendRes("SiteInfo", SiteInfoModel.StationNo, jsonData);
                            IMessageProduct product = new MessageProduct();
                            string queueName = "ConsumerWorkerService.ResParameter, ConsumerWorkerService_Site2HOSMessage";
                            product.Publish(message, queueName);
                            WriteText("站点:" + SiteInfoModel.StationNo + "   设备配置初始化成功！");
                            WriteText("注册成功，请关闭我再次重新打开应用");
                            MessageBox.Show("注册成功，请关闭我再次重新打开应用");
                            button1.Enabled = false;
                            label1.Visible = false;
                            textBox1.Visible = false;
                        }
                    }
                    if (Type == 3)
                    {
                        string StationNo = siteid;
                        SiteInfoModel.StationNo = StationNo;
                        string sql = "SELECT * FROM STORE";

                        DataTable dt2 = DBUtility.DbHelperODBC.Query(sql).Tables[0];

                        SiteInfoModel.StationName = dt2.Rows[0]["STORE_NAME"].ToString();
                        SiteInfoModel.PostUrl = textBox1.Text.ToString().Trim();
                        PostJson PostJson = GetGasConfigureJson();
                        string PostJsonStr = PostJson.ToJson().ToString().Replace("\\", "").Replace("Param\":\"{", "Param\":{").Replace("]}\"}", "]}}");
                        string url = SiteInfoModel.PostUrl + "GasConfigure";
                        PostResult Result = HttpPost.PostHttps(url, PostJsonStr).ToObject<PostResult>();
                        if (Result.Result == 0)
                        {
                            WriteText("网关返回成功");
                            SiteInfoModel.isActived = 1;
                            SiteInfoModel.ActivedDatetime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                            if (SiteInfoBLL.Update(SiteInfoModel))
                            {
                                var jsonData = new
                                {
                                    SiteID = SiteInfoModel.StationNo,
                                    SiteName = SiteInfoModel.StationName
                                };
                                string message = SendRes("SiteInfo", SiteInfoModel.StationNo, jsonData);
                                IMessageProduct product = new MessageProduct();
                                string queueName = "ConsumerWorkerService.ResParameter, ConsumerWorkerService_Site2HOSMessage";
                                product.Publish(message, queueName);
                                WriteText("站点:" + SiteInfoModel.StationNo + "   设备配置初始化成功！");
                                WriteText("注册成功，请关闭我再次重新打开应用");
                                MessageBox.Show("注册成功，请关闭我再次重新打开应用");
                                button1.Enabled = false;
                                label1.Visible = false;
                                textBox1.Visible = false;
                            }
                        }
                        else
                        {
                            WriteText(Result.Msg);
                        }
                    }

                }
            }
            catch (Exception e)
            {
                WriteText(e.StackTrace);
            }
        }

        /// <summary>
        /// 获取发送网关的初始化Json对象
        /// </summary>
        /// <returns></returns>
        public PostJson GetGasConfigureJson()
        {
            //组装设备初始化调用数据
            GasConfigure GasConfigureModel = new GasConfigure();
            GasConfigureModel.StationNo = SiteInfoModel.StationNo;
            List<SPTool.Model.TankConfigure> tanklist = tankConfigBLL.GetAllList().Tables[0].ToJson().ToList<SPTool.Model.TankConfigure>();
            List<SPTool.Model.GunConfigure> gunlist = gunConfigBLL.GetAllList().Tables[0].ToJson().ToList<SPTool.Model.GunConfigure>();
            List<TankList> TankList = new List<Model.TankList>();
            foreach (var item in tanklist)
            {
                TankList TankInfo = new TankList();
                TankInfo.TankNo = item.TankNo;
                TankInfo.TankName = item.TankName;
                decimal TankVolume = Extensions.ToDecimal3(0);
                if (item.TankVolume != null)
                {
                    TankVolume = Extensions.ToDecimal3((decimal)item.TankVolume);
                }
                TankInfo.TankVolume = TankVolume;
                TankInfo.OilCode = item.OilCode;
                TankInfo.OilName = item.OilName;
                List<GunList> GunList = new List<GunList>();
                foreach (var item1 in gunlist)
                {
                    if (item1.TankNo == item.TankNo)
                    {
                        GunList GunInfo = new Model.GunList();
                        GunInfo.GunNo = item1.GunNo;
                        GunInfo.SysId = item1.SysId;
                        GunList.Add(GunInfo);
                    }
                }
                TankInfo.GunList = GunList;
                TankList.Add(TankInfo);
            }
            GasConfigureModel.TankList = TankList;
            PostJson PostJson = new PostJson();
            PostJson.GasCode = SiteInfoModel.GasCode;
            PostJson.SysNo = SiteInfoModel.SysNo;
            PostJson.Param = GasConfigureModel.ToJson().ToString();
            return PostJson;
        }

        #region  业务数据处理

        /// <summary>
        ///初始化站点油罐和油枪数据
        /// </summary
        public void GetGasConfigure()
        {
            SetTankConfigure();
            SetGunConfigure();
        }


        /// <summary>
        /// 获取油罐-油品数据到数据库
        /// </summary>
        public void SetTankConfigure()
        {
            try
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
                                tankConfigModel1.TankVolume = dt.Rows[i]["VOLUME_QTY"].ToDecimal(3);
                                tankConfigModel1.OilCode = fuelItemModel.OilCode.Trim();
                                tankConfigModel1.OilName = fuelItemModel.OilName.Trim();
                                tankConfigModel1.BarCode = dt.Rows[i]["BARCODE"].ToString().Trim();
                                //油罐数据入库
                                tanklist.Add(tankConfigModel1);
                            }
                        }
                    }
                    tankConfigBLL.AddList(tanklist);
                    WriteText("获取油罐配置数据成功！");
                }
                dt.Clear();
                dt.Dispose();
            }
            catch (Exception me)
            {
                WriteText(me.Message);
            }
        }

        /// <summary>
        /// 获取油枪，油罐数据
        /// </summary>
        public void SetGunConfigure()
        {
            try
            {
                //2. 获取面向客户的枪号
                string pump_sql = "SELECT PUMP_ID,HOSE_ID,TANK1_ID FROM FUEL_PUMPS_HOSE ORDER BY PUMP_ID,HOSE_ID";
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
                    WriteText("获取油枪配置数据成功！");
                }
                dt2.Clear();
                dt2.Dispose();
            }
            catch (Exception me)
            {
                WriteText(me.Message);
            }


        }


        /// <summary>
        /// 窗体启动时与网关同步一下配置
        /// </summary>
        public void SynchronizeConfig()
        {
            List<TankConfigure> newlist = tankConfigBLL.GetAllList().Tables[0].ToJson().ToList<TankConfigure>();
            if (newlist.Count > 0)
            {
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
                        Form1.WriteLog("油罐配置变更成功！");
                    }
                    else
                    {
                        //数据不成功需要入库缓存
                        Form1.WriteLog("油罐配置变更失败！");
                    }
                }
            }
            List<GunConfigure> newlist1 = gunConfigBLL.GetAllList().Tables[0].ToJson().ToList<GunConfigure>();
            if (newlist1.Count > 0)
            {
                foreach (var item in newlist1)
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
        #endregion

        #region 基础方法
        public void WriteText(string Text)
        {
            string logText = string.Format("{0}   {1}" + Environment.NewLine, DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss:ffff"), Text);
            richTextBox1.AppendText(logText);
            string path = System.Windows.Forms.Application.StartupPath + "\\" + "log";
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
            string Path = path + "\\" + DateTime.Now.ToString("yyyyMMdd") + ".log";
            WriteLogFile(Path, Text);
        }

        public static void WriteLog(string Text)
        {
            string path = System.Windows.Forms.Application.StartupPath + "\\" + "log";
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
            string Path = path + "\\" + DateTime.Now.ToString("yyyyMMdd") + ".log";
            WriteLogFile(Path, Text);
        }
        public static bool WriteLogFile(string FileName, string message)
        {
            string logText = string.Format("{0} .........  {1}", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss:ffff"), message);
            if (FileName == null || FileName == "")
                return false;

            if (message == null)
                message = "null";

            try
            {
                if (!File.Exists(FileName))
                {
                    ///创建日志文件
                    StreamWriter sr = File.CreateText(FileName);
                    sr.Close();
                }
                System.IO.FileInfo fileInfo = new System.IO.FileInfo(FileName);
                if (fileInfo.Length / (1024 * 1024) > 2)//fa.File.Exists(FileName))
                {
                    string newfilename = DateTime.Now.ToString().Replace(":", "_");
                    newfilename = FileName.Replace(".log", "") + "_" + DateTime.Now.ToString("yyyyMMddHHmmssffff") + fileInfo.Extension;
                    fileInfo.MoveTo(newfilename);
                    ///创建日志文件
                    StreamWriter sr = File.CreateText(FileName);
                    //sr.WriteLine(DateTime.Now.ToString() + "   " + message);
                    sr.WriteLine(logText);
                    sr.Close();
                }
                else
                {
                    ///如果日志文件已经存在，则直接写入日志文件
                    StreamWriter sr = File.AppendText(FileName);
                    // sr.WriteLine("[" + DateTime.Now.ToString() + "] " + message);
                    sr.WriteLine(logText);
                    sr.Close();
                }
                return true;
            }
            catch
            {

            }
            return false;
        }


        /// <summary>
        /// 2023-03-21从注册表获取HOS站点编码
        /// </summary>
        /// <returns></returns>
        public string GetSiteIDByRegistry()
        {

            string registData;
            RegistryKey hkml = Registry.LocalMachine;
            RegistryKey software = hkml.OpenSubKey("SOFTWARE", true);
            RegistryKey aimdir = software.OpenSubKey("Positive", true);
            RegistryKey subaimdir1 = aimdir.OpenSubKey("HOS", true);
            registData = subaimdir1.GetValue("HOSSiteId").ToString();
            return registData;

        }





        /// <summary>  
        /// 修改程序在注册表中的键值  
        /// </summary>  
        /// <param name="isAuto">true:开机启动,false:不开机自启</param> 
        public static void AutoStart(bool isAuto)
        {
            try
            {
                if (isAuto == true)
                {
                    RegistryKey R_local = Registry.LocalMachine;//RegistryKey R_local = Registry.CurrentUser;
                    RegistryKey R_run = R_local.CreateSubKey(@"SOFTWARE\Microsoft\Windows\CurrentVersion\Run");
                    R_run.SetValue("应用名称", Application.ExecutablePath);
                    R_run.Close();
                    R_local.Close();
                }
                else
                {
                    RegistryKey R_local = Registry.LocalMachine;//RegistryKey R_local = Registry.CurrentUser;
                    RegistryKey R_run = R_local.CreateSubKey(@"SOFTWARE\Microsoft\Windows\CurrentVersion\Run");
                    R_run.DeleteValue("应用名称", false);
                    R_run.Close();
                    R_local.Close();
                }

                //GlobalVariant.Instance.UserConfig.AutoStart = isAuto;
            }
            catch (Exception)
            {
                MessageBox.Show("您需要管理员权限修改", "提示");
            }


        }

        protected override void OnClosed(EventArgs e)
        {

            base.OnClosed(e);
        }
        /// <summary>
        /// 关机拦截事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SessionEndingEvent(object sender, SessionEndingEventArgs e)
        {
            KillProcess("SPTool.exe");
        }



        private void Form1_SizeChanged(object sender, EventArgs e)
        {
            if (this.WindowState == FormWindowState.Minimized)
            {
                this.Hide(); //或者是this.Visible = false;
                this.notifyIcon1.Visible = true;
            }
        }

        private void exitMenuItem_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("你确定要关闭吗？", "确认", MessageBoxButtons.OKCancel, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) == DialogResult.OK)
            {
                PassWordInput pw = new PassWordInput();

                DialogResult dr = pw.ShowDialog();
                if (pw.DialogResult == DialogResult.OK)
                {
                    WriteText("===============工具被关闭了===============");
                    notifyIcon1.Visible = false;
                    notifyIcon1.Dispose();
                    System.Environment.Exit(0);
                }

            }
        }
        private void hideMenuItem_Click(object sender, EventArgs e)
        {
            this.Hide();
        }
        private void showMenuItem_Click(object sender, EventArgs e)
        {
            this.Show();
            this.WindowState = FormWindowState.Normal;
            this.Activate();
        }
        private void notifyIcon1_DoubleClick(object sender, EventArgs e)
        {
            if (this.WindowState == FormWindowState.Normal)
            {
                this.WindowState = FormWindowState.Minimized;
                this.Hide();
            }
            else if (this.WindowState == FormWindowState.Minimized)
            {
                this.Show();
                this.WindowState = FormWindowState.Normal;
                this.Activate();
            }
        }
        private void Form1_FormClosing_1(object sender, FormClosingEventArgs e)
        {
            this.Hide();
            e.Cancel = true;
        }

        /// <summary>
        /// 关闭进程
        /// </summary>
        /// <param name="processName">进程名</param>
        private void KillProcess(string processName)
        {
            Process[] myproc = Process.GetProcesses();
            foreach (Process item in myproc)
            {
                if (item.ProcessName == processName)
                {
                    item.Kill();
                }
            }
        }


        private string ReadFirstLine(string fileName)
        {
            string[] lines = File.ReadAllLines(fileName, Encoding.Default);
            lines[0] = Regex.Replace(lines[0], "\\s+", " ");
            return lines[0];
        }

        public void Restart()
        {
            Application.Restart();
            //Application.Exit();
            //System.Environment.Exit(0);
            //System.Diagnostics.Process.Start(System.Reflection.Assembly.GetExecutingAssembly().Location);

            //this.Text = this.Text + "111";
            //WriteText("我重启了");
        }



        #endregion

        private void button2_Click(object sender, EventArgs e)
        {
            ShellGasSaleDataJob dataJob = new ShellGasSaleDataJob();
            string daybathcdate = this.dateTimePicker1.Value.ToString("yyyy-MM-dd");
            string tillnum1 = this.textBox2.Text.Trim();
            string tillnum2 = this.textBox3.Text.Trim();
            int count = int.Parse(tillnum2) - int.Parse(tillnum1);
            if (count > 201)
            {
                MessageBox.Show("重传请最大不要超过200条！");
            }
            else
            {
                int i = dataJob.RepeatSendTill(daybathcdate, tillnum1, tillnum2);
                if (i == 1)
                {
                    MessageBox.Show("重传成功！");
                }
                if (i == 2)
                {
                    MessageBox.Show("重传失败！");
                }
                if (i == 3)
                {
                    MessageBox.Show("错误，请确保业务日期和交易号都正确！");
                }

            }
        }

        //设置模式
        private void button3_Click(object sender, EventArgs e)
        {
            string sql = "";
            //设置政府模式=1
            if (radioButton1.Checked)
            {
                SiteInfoModel.ModelType = 1;
            }
            if (radioButton2.Checked)
            {
                SiteInfoModel.ModelType = 2;
      
            }
            if (radioButton3.Checked)
            {
                SiteInfoModel.ModelType = 3;

            }
            if (SiteInfoBLL.Update(SiteInfoModel))
            {
                MessageBox.Show("设置成功，请退出软件手工重新开启！");
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {

            SiteInfoModel.isActived = 0;
           if (SiteInfoBLL.Update(SiteInfoModel))
            {
                MessageBox.Show("设置成功，请退出软件手工重新开启！");
            }
        }


    }
}