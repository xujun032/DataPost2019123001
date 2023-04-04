using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace Macrosage.RabbitMQ.Server.Product
{
    public interface IMessageProduct
    {
        void Publish(string message, string queueName);
    }
}
