using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SPTool.Model
{
    public class GasSaleData
    {

        public string StationNo { get; set; }

        public List<SaleDataList> SaleDataList { get; set; }

    }
    public class SaleDataList
    {
        public string FlowNo { get; set; }
        public string GunNo { get; set; }
        public string OptTime { get; set; }

        public string OilNo  { get; set; }
        public string OilAbbreviate { get; set; }

        public decimal Price { get; set; }

        public decimal Qty { get; set; }

        public decimal Amount { get; set; }


        public int TTC { get; set; }
        public decimal? EndTotal { get; set; }
        public string TransType { get; set; }


        public string OperatorName { get; set; }
        public string Consumer { get; set; }
        public string PlateNum { get; set; }

        public string TankNo { get; set; }
    }
}
