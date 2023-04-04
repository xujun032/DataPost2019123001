using Common.RabbitMQ;
using EasyNetQ;
using Message.Client.MessageManage.Consume;
using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace ConsumerWorkerService.MessageManage.Consume
{
    public  class SubscribeToErrorQueueSpike : IDisposable
    {
        private IBus bus;

        public SubscribeToErrorQueueSpike()
        {
            bus = RabbitMQManage.CreateEventBus();
        }

        public void Dispose()
        {
            bus.Dispose();
        }


     


        public  void Subscribe_Error_Messages()
        {
            var errorQueueName = new Conventions(new DefaultTypeNameSerializer()).ErrorQueueNamingConvention(new MessageReceivedInfo());

            var queue = bus.Advanced.QueueDeclare(errorQueueName);
            var autoResetEvent = new AutoResetEvent(false);

            bus.Advanced.Consume<EasyNetQ.SystemMessages.Error>(queue, (message, info) =>
            {
                var error = message.Body;
                Console.Out.WriteLine("error.DateTime = {0}", error.DateTime);
                Console.Out.WriteLine("error.Exception = {0}", error.Exception);
                Console.Out.WriteLine("error.Message = {0}", error.Message);
                Console.Out.WriteLine("error.RoutingKey = {0}", error.RoutingKey);
                FanoutMessageConsume fanoutMessageConsume = new FanoutMessageConsume();
                LoggerHelper.WriteLog("开始处理发生异常的消息队列"+ error.Message);
                LoggerHelper.WrireErrorJson(error.Message);
                LoggerHelper.WriteLog("异常的消息队列持久化成功" + error.Message);
                autoResetEvent.Set();
                return Task.Factory.StartNew(() => { });
            });
            autoResetEvent.WaitOne(1000);
        }
    }
}
