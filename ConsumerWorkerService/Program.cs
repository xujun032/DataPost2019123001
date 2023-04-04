using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using Common.RabbitMQ;
using ConsumerWorkerService.Class;
using ConsumerWorkerService.MessageManage.Consume;
using ConsumerWorkerService.MessageManage.Model;
using Message.Client;
using Message.Client.MessageManage.Consume;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;


namespace ConsumerWorkerService
{
    public class Program
    {

        static IConfiguration configure = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory())
      .AddJsonFile("appsettings.json").Build();


        public static void Main(string[] args)
        {
            if (configure["TaskConfig:UseMQ"] == "true")
            {
                MessageManService.Subsribe();
                SubscribeToErrorQueueSpike errorQueueSpike = new SubscribeToErrorQueueSpike();
                errorQueueSpike.Subscribe_Error_Messages();
            }
          // Parallel.Invoke(Foo, Bar);
            CreateHostBuilder(args).Build().Run();

        }


        public static IHostBuilder CreateHostBuilder(string[] args)
        {
            //�Ƿ���windowsƽ̨
            bool isWinPlantform = RuntimeInformation.IsOSPlatform(OSPlatform.Windows);
            Console.WriteLine($"Window Plantform:{isWinPlantform}");
            if (!isWinPlantform)
                return Host.CreateDefaultBuilder(args)
                    .UseSystemd()//ʹ��linux����
                    .ConfigureServices((hostContext, services) =>
                    {
                        services.AddSingleton<Microsoft.Extensions.Hosting.IHostedService, DeleteLogJob>();
                        if (configure["TaskConfig:Invoiced"] == "true")
                        {
                            services.AddSingleton<Microsoft.Extensions.Hosting.IHostedService, HandInvoiceErrorMsg>();
                        }
                        //services.AddSingleton<Microsoft.Extensions.Hosting.IHostedService, Worker>();
                        //LoggerHelper.WriteISynchLog("186��ʱ����===���ע�ᣡ");
                        //services.AddSingleton<Microsoft.Extensions.Hosting.IHostedService, SynchroData>();
                
                    });
            else
            {
                return Host.CreateDefaultBuilder(args)
    .UseWindowsService()//ʹ��windows����
    .ConfigureServices((hostContext, services) =>
    {
        services.AddSingleton<Microsoft.Extensions.Hosting.IHostedService, DeleteLogJob>();
        if (configure["TaskConfig:Invoiced"] == "true")
        {
           // services.AddSingleton<Microsoft.Extensions.Hosting.IHostedService, HandInvoiceErrorMsg>();
        }
        // services.AddSingleton<Microsoft.Extensions.Hosting.IHostedService, Worker>();
        LoggerHelper.WriteISynchLog("186��ʱ����===���ע�ᣡ");
        services.AddSingleton<Microsoft.Extensions.Hosting.IHostedService, SynchroData>();
    });
            }
        }


        /// <summary>
        /// ��ʷ����ͬ��
        /// </summary>
        static void Foo()
        {
            string path = AppContext.BaseDirectory + "/" + "log";
            DirectoryInfo d = new DirectoryInfo(path);
            FileInfo[] files = d.GetFiles();//�ļ�
            DirectoryInfo[] directs = d.GetDirectories();//�ļ���
            foreach (FileInfo f in files)
            {
                if (f.Name == "lck.lck")
                {
                   SynchHistoryData.SynchTILLITEM_PMNT();
                }
            }
        }
        static void Bar()
        {
            string path = AppContext.BaseDirectory + "/" + "log";
            DirectoryInfo d = new DirectoryInfo(path);
            FileInfo[] files = d.GetFiles();//�ļ�
            DirectoryInfo[] directs = d.GetDirectories();//�ļ���
            foreach (FileInfo f in files)
            {
                if (f.Name == "lck.lck")
                {
                    SynchHistoryData.SynchReport_TILL();
                }
            }
        }
    }
}
