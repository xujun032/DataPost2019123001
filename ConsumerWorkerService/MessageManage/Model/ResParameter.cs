using System;
using System.Collections.Generic;
using System.Text;

namespace ConsumerWorkerService.MessageManage.Model
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
        public Data Data { get; set; }

    }


    public class Data
    {
        /// <summary>
        /// 
        /// </summary>
        public List<TILL> Till { get; set; }
        public List<TILLITEM> TILLITEM { get; set; }
        public List<TILL_PMNT> TILL_PMNT { get; set; }
        public List<TILL_PROMO> TILL_PROMO { get; set; }
        public List<TILLITEM_PMNT> TILLITEM_PMNT { get; set; }
        public List<TILLITEM_PROMO> TILLITEM_PROMO { get; set; }

        public List<SITEDAYBATCH> SITEDAYBATCH { get; set; }
        public string SiteID { get; set; }
        public string SiteName { get; set; }




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
