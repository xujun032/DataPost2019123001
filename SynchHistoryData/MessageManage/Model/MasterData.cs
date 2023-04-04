using System;
using System.Collections.Generic;
using System.Text;
namespace ConsumerWorkerService.MessageManage.Model
{
    public class MasterProduct
    {
        /// <summary>
        /// 
        /// </summary>
        public string Product_Code { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string TaxRate { get; set; }
        /// <summary>
        /// 暧宝宝可贴1片
        /// </summary>
        public string Product_Name { get; set; }
        /// <summary>
        /// 室外高清汽油卡
        /// </summary>
        public string UOM { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string TaxCode { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string Product_Type { get; set; }
    }

    public class MasterProductRoot
    {
        public string pztoken { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public List<Product> Product { get; set; }
    }


    public class MasterPayment
    {
        /// <summary>
        /// 
        /// </summary>
        public string Payment_code { get; set; }
        /// <summary>
        /// 室外B2C卡
        /// </summary>
        public string Payment_Method { get; set; }
    }

    public class MasterPaymentRoot
    {
        public string pztoken { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public List<Payment> Payment { get; set; }
    }


    public class Product_Dept
    {
        /// <summary>
        /// 
        /// </summary>
        public string Dept_code { get; set; }
        /// <summary>
        /// 汽油
        /// </summary>
        public string Dept_Name { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string Prent_Dept_code { get; set; }
        /// <summary>
        /// 室外高清汽油
        /// </summary>
        public string Prent_Dept_Name { get; set; }
    }

    public class MasterProduct_DeptRoot
    {

        public string pztoken { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public List<Product_Dept> Product_Dept { get; set; }
    }


    public class SiteInfo
    {
        /// <summary>
        /// 
        /// </summary>
        public string SiteID { get; set; }
        /// <summary>
        /// 汽油
        /// </summary>
        public string SiteName { get; set; }
        /// <summary>
        /// 
        /// </summary>

    }

    public class MasterSIteInfoRoot
    {

        public string pztoken { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public List<SiteInfo> SiteInfo { get; set; }
    }


}
