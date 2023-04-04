using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using ConsumerWorkerService;
using ConsumerWorkerService.MessageManage.Model;
using Message.Client;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using SqlSugar;

namespace SynchHistoryData
{
    public class Program
    {

        static IConfiguration configure = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory())
.AddJsonFile("appsettings.json").Build();
        static void Main(string[] args)
        {
          //  CreateTable(false, 50, typeof(bos_payment), typeof(bos_transaction));
            if (configure["TaskConfig:UseMQ"] == "true")
            {
                MessageManService.Subsribe();
            }
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args)
        {
            //是否是windows平台
            bool isWinPlantform = RuntimeInformation.IsOSPlatform(OSPlatform.Windows);
            Console.WriteLine($"Window Plantform:{isWinPlantform}");
            if (!isWinPlantform)
                return Host.CreateDefaultBuilder(args)
                    .UseSystemd()//使用linux服务
                    .ConfigureServices((hostContext, services) =>
                    {
                        services.AddSingleton<Microsoft.Extensions.Hosting.IHostedService, DeleteLogJob>();
                        //if (configure["TaskConfig:Invoiced"] == "true")
                        //{
                        //    services.AddSingleton<Microsoft.Extensions.Hosting.IHostedService, HandInvoiceErrorMsg>();
                        //}
                        //services.AddSingleton<Microsoft.Extensions.Hosting.IHostedService, Worker>();
                        //services.AddSingleton<Microsoft.Extensions.Hosting.IHostedService, SynchroData>();
                    });
            else
            {
                return Host.CreateDefaultBuilder(args)
    .UseWindowsService()//使用windows服务
    .ConfigureServices((hostContext, services) =>
    {
        services.AddSingleton<Microsoft.Extensions.Hosting.IHostedService, DeleteLogJob>();
        //if (configure["TaskConfig:Invoiced"] == "true")
        //{
        //    services.AddSingleton<Microsoft.Extensions.Hosting.IHostedService, HandInvoiceErrorMsg>();
        //}
        //services.AddSingleton<Microsoft.Extensions.Hosting.IHostedService, Worker>();
        //services.AddSingleton<Microsoft.Extensions.Hosting.IHostedService, SynchroData>();
    });
            }
        }


        public static void CreateTable(bool Backup = false, int StringDefaultLength = 50, params Type[] types)
        {
         var    Db = new SqlSugarClient(new ConnectionConfig()
            {
                ConnectionString = "server=172.20.0.186;Database=dws;Uid=root;Pwd=root;",
                DbType = DbType.MySql,//设置数据库类型
                IsAutoCloseConnection = true,//自动释放数据库，如果存在事务，在事务结束之后释放。
                InitKeyType = InitKeyType.Attribute//从实体特性中读取主键自增列信息   
            });


  
            Db.CodeFirst.SetStringDefaultLength(StringDefaultLength);
            Db.DbMaintenance.CreateDatabase();
            if (Backup)
            {
                Db.CodeFirst.BackupTable().InitTables(types);
            }
            else
            {
                Db.CodeFirst.InitTables(types);
            }
        }
    }
}
