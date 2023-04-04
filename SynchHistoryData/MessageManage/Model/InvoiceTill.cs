using System;
using System.Collections.Generic;
using System.Text;

namespace ConsumerWorkerService.MessageManage.Model
{
    public class Product
    {
        /// <summary>
        /// 
        /// </summary>
        public string Product_Type { get; set; }
        /// <summary>
        /// 公升
        /// </summary>
        public string UOM { get; set; }
        /// <summary>
        /// 92号车用汽油（VI）
        /// </summary>
        public string Product_Name { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string Product_Code { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string Unit_Price { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string Amount { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string Total_Price { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string Discount { get; set; }


        public string  ISFuel { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string Pump_No { get; set; }
    }

    public class Product_Group
    {
        /// <summary>
        /// 
        /// </summary>
        public List<Product> Product { get; set; }
    }

    public class Payment
    {
        /// <summary>
        /// 
        /// </summary>
        public string Payment_Method { get; set; }


        public string Site_Payment_Method { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string Payment_Amount { get; set; }
    }

    public class Payment_Group
    {
        /// <summary>
        /// 
        /// </summary>
        public List<Payment> Payment { get; set; }
    }

    public class SaleDataList
    {
        /// <summary>
        /// 
        /// </summary>
        public string Transaction_ID { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string Transaction_Type { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string Date { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string Time { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string POS_ID { get; set; }
        /// <summary>
        /// 

        /// <summary>
        /// 
        /// </summary>
        public string Cashier_Name { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string Site_Name { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string Site_Number { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string POS_Transaction_Number { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string Product_Count { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string Payment_Count { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string Payment_Total_Paid { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string Customer_Code { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Product_Group Product_Group { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Payment_Group Payment_Group { get; set; }
    }

    public class InvoiceTill
    {
        /// <summary>
        /// 
        /// </summary>
        public string Pztoken { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public List<SaleDataList> SaleDataList { get; set; }
    }


    public class Result
    {
        public List<DataItem> data { get; set; }
    }
    public class InvoiceReturnMsg
    {
        /// <summary>
        /// 
        /// </summary>
        public string success { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string msg { get; set; }
        /// <summary>
        /// 
        /// </summary>
  
        public Result result { get; set; }
    }


    public class DataItem
    {
        /// <summary>
        /// 
        /// </summary>
        public string message { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string transaction_id { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string date { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string pos_transaction_number { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string time { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string site_number { get; set; }
    }

}
