using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SPTool
{
   public class ResParameter
    {

        /// <summary>
        /// 发送数据批次
        /// </summary>
        public string GUID { get; set; }
        /// <summary>
        ///发送数据类型
        /// </summary>
        public string Type { get; set; }

        /// <summary>
        ///发送数据时间
        /// </summary>
        public string SendDateTime { get; set; }

        /// <summary>
        /// 发送站点编码
        /// </summary>
        public string SiteId { get; set; }



        /// <summary>
        /// 响应数据
        /// </summary>
        public object Data { get; set; }

    }


    public class ResParameter186
    {

        /// <summary>
        /// 发送数据批次
        /// </summary>
        public string GUID { get; set; }
        /// <summary>
        ///发送数据类型
        /// </summary>
        public string Type { get; set; }

        /// <summary>
        ///发送数据时间
        /// </summary>
        public string SendDateTime { get; set; }





        /// <summary>
        /// 响应数据
        /// </summary>
        public object Data { get; set; }

    }
}
