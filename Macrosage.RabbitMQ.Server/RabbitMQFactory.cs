using Macrosage.RabbitMQ.Server.Config;
using Macrosage.RabbitMQ.Server.Customer;
using RabbitMQ.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace Macrosage.RabbitMQ.Server
{
    public class RabbitMQFactory
    {
        #region 单例
        private RabbitMQFactory() {
            Customers = new Dictionary<string, IMessageCustomer>();
        }
        private static RabbitMQFactory _instance;

        private static object _lock = new object();
        public static RabbitMQFactory Instance
        {
            get
            {
                if (_instance == null)
                {
                    lock (_lock)
                    {
                        if (_instance == null)
                        {
                            _instance = new RabbitMQFactory();
                        }
                    }
                }
                return _instance;
            }
        }
        #endregion

        #region 私有变量
        private ConnectionFactory _factory { get; set; }

        private Dictionary<string, IMessageCustomer> Customers { get; set; }
        #endregion

        #region 创建工厂
        public ConnectionFactory CreateFactory()
        {
            if (_factory == null) {

                const ushort heartbeat = 60;

                _factory = new ConnectionFactory();
                _factory.HostName = RabbitMQConfig.HostName;
                _factory.UserName = RabbitMQConfig.UserName;
                _factory.Password = RabbitMQConfig.PassWord;
                _factory.Port =5672;
                _factory.VirtualHost = "/";
                _factory.Protocol = Protocols.DefaultProtocol;
                _factory.RequestedHeartbeat = heartbeat;
                _factory.AutomaticRecoveryEnabled = true;//自动重连
            }
            return _factory;
        }
        #endregion

        #region 创建消费者

        public IMessageCustomer CreateCustomer(string queueName)
        {
            IMessageCustomer customer;
            if (!Customers.ContainsKey(queueName))
            {
                customer = new MessageCustomer(queueName);
                Customers.Add(queueName, customer);
            }
            else
            {
                customer = Customers[queueName];
            }
            return customer;
        }

        #endregion
    }
}
