using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SPTool.Model
{
  public class TankData
    {
        public string StationNo { get; set; }
        public string FlowNo { get; set; }
        public List<TankDataList> TankDataList { get; set; }
    }

    public class TankDataList {
        public string TankNo { get; set; }
        public string ReadTime { get; set; }
        public string OilNo { get; set; }

        public string OilAbbreviate { get; set; }
        public decimal Stock { get; set; }
        public decimal  OilLevel { get; set; }
        public decimal WaterLevel { get; set; }
        public decimal Temp { get; set; }

        public decimal TankEmpty { get; set; }

        public int State { get; set; }
        


    }
}
