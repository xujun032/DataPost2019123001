using Macrosage.RabbitMQ.Server.Config;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace Macrosage.RabbitMQ.Server.Product
{
    public class MessageProduct : IMessageProduct
    {
        /// <summary>
        /// 发送消息
        /// </summary>
        /// <param name="message">消息体</param>
        /// <param name="queueName">消息队列</param>
        public void Publish(string message, string queueName)
        {
            queueName = queueName == null ? RabbitMQConfig.PUBLISH_GLOBAL_QUEUE : queueName;

            var factory = RabbitMQFactory.Instance.CreateFactory();
            using (var connection = factory.CreateConnection())
            {
                using (var model = connection.CreateModel())
                {
                    //消息持久化，防止丢失
                    model.QueueDeclare(queueName, RabbitMQConfig.IsDurable,false, false, null);
                   // model.ExchangeDeclare
                    //
                    var properties = model.CreateBasicProperties();
                    properties.SetPersistent(RabbitMQConfig.IsDurable);
                    properties.DeliveryMode = 2;
                    //消息转换为二进制
                    var msgBody = Encoding.UTF8.GetBytes(message);
                    //消息发出到队列
                    model.BasicPublish("", queueName, properties, msgBody);
                }
            }
         
        }
    }
}
