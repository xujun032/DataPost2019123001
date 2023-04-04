using Quartz;
using Quartz.Impl;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading;

namespace SPTool
{
    public class Task
    {
        /// <summary>
        /// 油罐配置监控任务调度
        /// </summary>
        /// <returns></returns>
        public static void TankConfigureRun(int Seconds)
        {

            var properties = new NameValueCollection();
            // 设置线程池
            properties["quartz.threadPool.type"] = "Quartz.Simpl.SimpleThreadPool, Quartz";
            //设置线程池的最大线程数量
            properties["quartz.threadPool.threadCount"] = "5";
            //设置作业中每个线程的优先级
            properties["quartz.threadPool.threadPriority"] = ThreadPriority.Normal.ToString();

            // 远程输出配置
            properties["quartz.scheduler.exporter.type"] = "Quartz.Simpl.RemotingSchedulerExporter, Quartz";
            properties["quartz.scheduler.exporter.port"] = "555";  //配置端口号
            properties["quartz.scheduler.exporter.bindName"] = "QuartzScheduler";
            properties["quartz.scheduler.exporter.channelType"] = "tcp"; //协议类型
            // 1.创建scheduler的引用
            ISchedulerFactory schedFact = new StdSchedulerFactory(properties);
            IScheduler sched = schedFact.GetScheduler();

            //2.启动 scheduler
            sched.Start();

            // 3.创建 job
            IJobDetail job = JobBuilder.Create<TankConfigureJob>()
                    .WithIdentity("TankConfigureJob", "TankConfigureGroup")
                    .Build();

            // 4.创建 trigger
            ITrigger trigger = TriggerBuilder.Create()
                .WithIdentity("trigger1", "group1")
                .WithSimpleSchedule(x => x.WithIntervalInMinutes(Seconds).RepeatForever())
                  .StartAt(DateTime.UtcNow.AddSeconds(19))
                .Build();
            // 5.使用trigger规划执行任务job
            sched.ScheduleJob(job, trigger);
        }

        public static void GunConfigureRun(int Seconds)
        {

            var properties = new NameValueCollection();
            // 设置线程池
            properties["quartz.threadPool.type"] = "Quartz.Simpl.SimpleThreadPool, Quartz";
            //设置线程池的最大线程数量
            properties["quartz.threadPool.threadCount"] = "5";
            //设置作业中每个线程的优先级
            properties["quartz.threadPool.threadPriority"] = ThreadPriority.Normal.ToString();

            // 远程输出配置
            properties["quartz.scheduler.exporter.type"] = "Quartz.Simpl.RemotingSchedulerExporter, Quartz";
            properties["quartz.scheduler.exporter.port"] = "555";  //配置端口号
            properties["quartz.scheduler.exporter.bindName"] = "QuartzScheduler";
            properties["quartz.scheduler.exporter.channelType"] = "tcp"; //协议类型
            // 1.创建scheduler的引用
            ISchedulerFactory schedFact = new StdSchedulerFactory(properties);
            IScheduler sched = schedFact.GetScheduler();

            //2.启动 scheduler
            sched.Start();

            // 3.创建 job
            IJobDetail job = JobBuilder.Create<GunConfigureJob>()
                    .WithIdentity("GunConfigureJob", "GunConfigureJob")
                    .Build();

            // 4.创建 trigger
            ITrigger trigger = TriggerBuilder.Create()
                .WithIdentity("trigger21", "group21")
                .WithSimpleSchedule(x => x.WithIntervalInMinutes(Seconds).RepeatForever())
                  .StartAt(DateTime.UtcNow.AddSeconds(19))
                .Build();

            // 5.使用trigger规划执行任务job
            sched.ScheduleJob(job, trigger);
        }


        public static void GasSaleDataRun(int Seconds)
        {

            var properties = new NameValueCollection();
            // 设置线程池
            properties["quartz.threadPool.type"] = "Quartz.Simpl.SimpleThreadPool, Quartz";
            //设置线程池的最大线程数量
            properties["quartz.threadPool.threadCount"] = "5";
            //设置作业中每个线程的优先级
            properties["quartz.threadPool.threadPriority"] = ThreadPriority.Normal.ToString();

            // 远程输出配置
            properties["quartz.scheduler.exporter.type"] = "Quartz.Simpl.RemotingSchedulerExporter, Quartz";
            properties["quartz.scheduler.exporter.port"] = "555";  //配置端口号
            properties["quartz.scheduler.exporter.bindName"] = "QuartzScheduler";
            properties["quartz.scheduler.exporter.channelType"] = "tcp"; //协议类型
            // 1.创建scheduler的引用
            ISchedulerFactory schedFact = new StdSchedulerFactory(properties);
            IScheduler sched = schedFact.GetScheduler();

            //2.启动 scheduler
            sched.Start();

            // 3.创建 job
            IJobDetail job = JobBuilder.Create<GasSaleDataJob>()
                    .WithIdentity("GasSaleDataJob", "GasSaleDataJob")
                    .Build();

            // 4.创建 trigger
            ITrigger trigger = TriggerBuilder.Create()
                .WithIdentity("trigger31", "group31")
                .WithSimpleSchedule(x => x.WithIntervalInMinutes(Seconds).RepeatForever())
                .Build();

            // 5.使用trigger规划执行任务job
            sched.ScheduleJob(job, trigger);
        }


        public static void TankDataRun(int Seconds)
        {

            var properties = new NameValueCollection();
            // 设置线程池
            properties["quartz.threadPool.type"] = "Quartz.Simpl.SimpleThreadPool, Quartz";
            //设置线程池的最大线程数量
            properties["quartz.threadPool.threadCount"] = "5";
            //设置作业中每个线程的优先级
            properties["quartz.threadPool.threadPriority"] = ThreadPriority.Normal.ToString();

            // 远程输出配置
            properties["quartz.scheduler.exporter.type"] = "Quartz.Simpl.RemotingSchedulerExporter, Quartz";
            properties["quartz.scheduler.exporter.port"] = "555";  //配置端口号
            properties["quartz.scheduler.exporter.bindName"] = "QuartzScheduler";
            properties["quartz.scheduler.exporter.channelType"] = "tcp"; //协议类型
            // 1.创建scheduler的引用
            ISchedulerFactory schedFact = new StdSchedulerFactory(properties);
            IScheduler sched = schedFact.GetScheduler();

            //2.启动 scheduler
            sched.Start();

            // 3.创建 job
            IJobDetail job = JobBuilder.Create<TankDataJob>()
                    .WithIdentity("TankDataJob", "TankDataJob")
                    .Build();

            // 4.创建 trigger
            ITrigger trigger = TriggerBuilder.Create()
                .WithIdentity("trigger41", "group41")
                .WithSimpleSchedule(x => x.WithIntervalInMinutes(Seconds).RepeatForever())
                .Build();

            // 5.使用trigger规划执行任务job
            sched.ScheduleJob(job, trigger);
        }

        public static void GasPurchaseInfoRun(int Seconds)
        {

            var properties = new NameValueCollection();
            // 设置线程池
            properties["quartz.threadPool.type"] = "Quartz.Simpl.SimpleThreadPool, Quartz";
            //设置线程池的最大线程数量
            properties["quartz.threadPool.threadCount"] = "5";
            //设置作业中每个线程的优先级
            properties["quartz.threadPool.threadPriority"] = ThreadPriority.Normal.ToString();

            // 远程输出配置
            properties["quartz.scheduler.exporter.type"] = "Quartz.Simpl.RemotingSchedulerExporter, Quartz";
            properties["quartz.scheduler.exporter.port"] = "555";  //配置端口号
            properties["quartz.scheduler.exporter.bindName"] = "QuartzScheduler";
            properties["quartz.scheduler.exporter.channelType"] = "tcp"; //协议类型
            // 1.创建scheduler的引用
            ISchedulerFactory schedFact = new StdSchedulerFactory(properties);
            IScheduler sched = schedFact.GetScheduler();

            //2.启动 scheduler
            sched.Start();

            // 3.创建 job
            IJobDetail job = JobBuilder.Create<GasPurchaseInfoJob>()
                    .WithIdentity("GasPurchaseInfoJob", "GasPurchaseInfoJob")
                    .Build();

            // 4.创建 trigger
            ITrigger trigger = TriggerBuilder.Create()
                .WithIdentity("trigger51", "group51")
                .WithSimpleSchedule(x => x.WithIntervalInMinutes(Seconds).RepeatForever())
                .Build();

            // 5.使用trigger规划执行任务job
            sched.ScheduleJob(job, trigger);
        }



        #region 中心模式任务

        public static void ShellGasSaleDataRun(int Minute)
        {

            var properties = new NameValueCollection();
            // 设置线程池
            properties["quartz.threadPool.type"] = "Quartz.Simpl.SimpleThreadPool, Quartz";
            //设置线程池的最大线程数量
            properties["quartz.threadPool.threadCount"] = "5";
            //设置作业中每个线程的优先级
            properties["quartz.threadPool.threadPriority"] = ThreadPriority.Normal.ToString();

            // 远程输出配置
            properties["quartz.scheduler.exporter.type"] = "Quartz.Simpl.RemotingSchedulerExporter, Quartz";
            properties["quartz.scheduler.exporter.port"] = "555";  //配置端口号
            properties["quartz.scheduler.exporter.bindName"] = "QuartzScheduler";
            properties["quartz.scheduler.exporter.channelType"] = "tcp"; //协议类型
            // 1.创建scheduler的引用
            ISchedulerFactory schedFact = new StdSchedulerFactory(properties);
            IScheduler sched = schedFact.GetScheduler();

            //2.启动 scheduler
            sched.Start();

            // 3.创建 job
            IJobDetail job = JobBuilder.Create<ShellGasSaleDataJob>()
                    .WithIdentity("ShellGasSaleDataJob", "ShellGasSaleDataJob")
                    .Build();

            // 4.创建 trigger
            ITrigger trigger = TriggerBuilder.Create()
                .WithIdentity("triggerShellGasSaleDatat", "groupShellGasSaleData")
                .WithSimpleSchedule(x => x.WithIntervalInMinutes(Minute).RepeatForever())
                .Build();

            // 5.使用trigger规划执行任务job
            sched.ScheduleJob(job, trigger);
        }

        public static void ShelDayBatchDataRun(int Minute)
        {

            var properties = new NameValueCollection();
            // 设置线程池
            properties["quartz.threadPool.type"] = "Quartz.Simpl.SimpleThreadPool, Quartz";
            //设置线程池的最大线程数量
            properties["quartz.threadPool.threadCount"] = "5";
            //设置作业中每个线程的优先级
            properties["quartz.threadPool.threadPriority"] = ThreadPriority.Normal.ToString();

            // 远程输出配置
            properties["quartz.scheduler.exporter.type"] = "Quartz.Simpl.RemotingSchedulerExporter, Quartz";
            properties["quartz.scheduler.exporter.port"] = "555";  //配置端口号
            properties["quartz.scheduler.exporter.bindName"] = "QuartzScheduler";
            properties["quartz.scheduler.exporter.channelType"] = "tcp"; //协议类型
            // 1.创建scheduler的引用
            ISchedulerFactory schedFact = new StdSchedulerFactory(properties);
            IScheduler sched = schedFact.GetScheduler();

            //2.启动 scheduler
            sched.Start();

            // 3.创建 job
            IJobDetail job = JobBuilder.Create<ShellDayBatchDataJob>()
                    .WithIdentity("ShellDayBatchDataJob", "ShellDayBatchDataJob")
                    .Build();

            // 4.创建 trigger
            ITrigger trigger = TriggerBuilder.Create()
                .WithIdentity("ShellDayBatchDataJobtrigger", "ShellDayBatchDataJobgroup")
                .WithSimpleSchedule(x => x.WithIntervalInMinutes(Minute).RepeatForever())
                .Build();

            // 5.使用trigger规划执行任务job
            sched.ScheduleJob(job, trigger);
        }

        #endregion 


        public static void DeleteLogRun(int Seconds)
        {

            var properties = new NameValueCollection();
            // 设置线程池
            properties["quartz.threadPool.type"] = "Quartz.Simpl.SimpleThreadPool, Quartz";
            //设置线程池的最大线程数量
            properties["quartz.threadPool.threadCount"] = "5";
            //设置作业中每个线程的优先级
            properties["quartz.threadPool.threadPriority"] = ThreadPriority.Normal.ToString();

            // 远程输出配置
            properties["quartz.scheduler.exporter.type"] = "Quartz.Simpl.RemotingSchedulerExporter, Quartz";
            properties["quartz.scheduler.exporter.port"] = "555";  //配置端口号
            properties["quartz.scheduler.exporter.bindName"] = "QuartzScheduler";
            properties["quartz.scheduler.exporter.channelType"] = "tcp"; //协议类型
            // 1.创建scheduler的引用
            ISchedulerFactory schedFact = new StdSchedulerFactory(properties);
            IScheduler sched = schedFact.GetScheduler();

            //2.启动 scheduler
            sched.Start();

            // 3.创建 job
            IJobDetail job = JobBuilder.Create<DeleteLogJob>()
                    .WithIdentity("DeleteLogJob", "DeleteLogJob")
                    .Build();

            // 4.创建 trigger
            ITrigger trigger = TriggerBuilder.Create()
                .WithIdentity("trigger71", "group71")
                .WithSimpleSchedule(x => x.WithIntervalInHours(Seconds).RepeatForever())
                .Build();

            // 5.使用trigger规划执行任务job
            sched.ScheduleJob(job, trigger);
        }
    }


}
