using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Macrosage.RabbitMQ.Server.Config
{
    public sealed class RabbitMQConfig
    {
        /// <summary>
        /// 全局队列
        /// </summary>
        public const string PUBLISH_GLOBAL_QUEUE = "PUBLISH_GLOBAL_QUEUE";

        /*
          ExceptionHandler.ThrowArgumentNull("_hostName", RabbitMQConfig.HostName);
                ExceptionHandler.ThrowArgumentNull("_userName", RabbitMQConfig.HostName);
                ExceptionHandler.ThrowArgumentNull("_passWord", _passWord);
            */
        /// <summary>
        /// 主机服务地址
        /// </summary>
        public static string HostName
        {
            get
            {
                var _hostName = "172.18.97.71";
                ExceptionHandler.ThrowArgumentNull("_hostName", _hostName);
                return _hostName;
            }
        }
        /// <summary>
        /// 用户名
        /// </summary>
        public static string UserName
        {
            get
            {
                var _userName = "sa";
                ExceptionHandler.ThrowArgumentNull("_userName", _userName);
                return _userName;
            }
        }
        /// <summary>
        /// 密码
        /// </summary>
        public static string PassWord
        {
            get
            {
                var _password = "8681276Xu";
                ExceptionHandler.ThrowArgumentNull("_password", _password);
                return _password;
            }
        }

        /// <summary>
        /// 是否持久化
        /// </summary>
        public static bool IsDurable
        {
            get
            {
                return true;
            }
        }
        /// <summary>
        /// 线程个数
        /// </summary>
        public static int ThreadCount
        {
            get
            {
                return 3;
            }
        }
    }
}
