using Macrosage.RabbitMQ.Server.Config;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace Macrosage.RabbitMQ.Server.Customer
{
    public class MessageCustomer : IMessageCustomer
    {
        public MessageCustomer() {
            _queueName = RabbitMQConfig.PUBLISH_GLOBAL_QUEUE;
        }
        public MessageCustomer(string queueName) {
            _queueName = queueName;
        }

        public Func<string, bool> ReceiveMessageCallback { get; set; }

        private IModel ListenChannel { get; set; }

        private string _queueName { get; set; }

        #region  开始监听消息
        /// <summary>
        /// 开始监听消息
        /// </summary>
        /// <param name="queueName"></param>
        public void StartListening(string queueName)
        {
                Consume(queueName);
       
            
        }
        #endregion

        #region 消费消息
        /// <summary>
        /// 消费信息
        /// </summary>
        /// <param name="queueName"></param>
        public void Consume(string queueName)
        {
            var factory = RabbitMQFactory.Instance.CreateFactory();

            var connection = factory.CreateConnection();

            connection.ConnectionShutdown += Connection_ConnectionShutdown;

            ListenChannel = connection.CreateModel();


            bool autoDeleteMessage = false;
            var queue = ListenChannel.QueueDeclare( queueName, RabbitMQConfig.IsDurable, false, false, null);

            //公平分发,不要同一时间给一个工作者发送多于一个消息
            ListenChannel.BasicQos(0, 1, false);
            //创建事件驱动的消费者类型，不要用下边的死循环来消费消息
            var consumer = new EventingBasicConsumer(ListenChannel);
            consumer.Received += Consumer_Received;

            ListenChannel.BasicConsume(queueName, autoDeleteMessage, consumer);

            #region 废弃代码
            //while (true)
            //{
            //    var ea = (BasicDeliverEventArgs)consumer.Queue.Dequeue();

            //    var body = ea.Body;
            //    var message = Encoding.UTF8.GetString(body);

            //    bool result = fun(message, queue.ConsumerCount, queue.MessageCount);
            //    if (result)
            //    {
            //        BasicAck(model, ea);
            //    }

            //}
            #endregion

        }

        private void Connection_ConnectionShutdown(IConnection connection, ShutdownEventArgs reason)
        {
            //日志记录
        }

        /// <summary>
        /// 接收到消息触发事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        private void Consumer_Received(IBasicConsumer sender, BasicDeliverEventArgs args)
        {
            try
            {
                var body = args.Body;
                var message = Encoding.UTF8.GetString(body);

                bool result = ReceiveMessageCallback(message);//将消息业务处理交给外部业务
                if (result)
                {
                    if (ListenChannel != null && !ListenChannel.IsClosed)
                    {
                        ListenChannel.BasicAck(args.DeliveryTag, false);
                    }
                }
                else
                {
                    if (ListenChannel != null)
                    {
                        ListenChannel.BasicReject(args.DeliveryTag, true);//重新放入对列头
                    }
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #endregion

        #region 获取某个队列的未处理消息个数
        public uint MessageCount
        {
            get
            {

                var queue = ListenChannel.QueueDeclare(_queueName, RabbitMQConfig.IsDurable,false, false, null);
                return queue.MessageCount;
            }
        }
        #endregion

    }
}
