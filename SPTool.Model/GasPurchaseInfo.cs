using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SPTool.Model
{
    public class GasPurchaseInfo
    {
        public string StationNo { get; set; }
        public string FlowNo { get; set; }

        public string SaleDate { get; set; }
        public int ShiftNo { get; set; }
        public string ReceiveDate { get; set; }

        public string MakeTime { get; set; }

        public string ProviderName { get; set; }


        public string BillNo { get; set; }

        public string OilNo { get; set; }

        public string OilAbbreviate { get; set; }

        public decimal SendVt { get; set; }

        public string GaugerNo { get; set; }
        public string Guager { get; set; }


        public string CarNo { get; set; }


        public string MotorMan { get; set; }


        public string EmplNo { get; set; }


        public string EmplName { get; set; }


        public string Remark { get; set; }
        public List<PurchDetailList> PurchDetailList { get; set; }

    }

    public class PurchDetailList
    {

        public string TankNo { get; set; }
        public string StartTime { get; set; }

        public decimal StartTemp { get; set; }
        public decimal StartOilLen { get; set; }
        public decimal StartWaterLen { get; set; }
        public decimal StartQtyLiter { get; set; }

        public string EndTime { get; set; }


        public decimal EndOilLen { get; set; }
        public decimal NewWaterLen { get; set; }
        public decimal EndTemp { get; set; }
        public decimal EndQtyLiter { get; set; }

        public decimal PeriodSaleQty { get; set; }
        public decimal NetVol { get; set; }
        public string Remark { get; set; }

        
    }
}
