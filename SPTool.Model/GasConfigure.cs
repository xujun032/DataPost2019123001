using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SPTool.Model
{

    public class PostJson
    {

        public string SysNo { get; set; }
        public string GasCode { get; set; }

        public string Param { get; set; }

    }

    public class GasConfigure
    {

        public string StationNo { get; set; }

        public List<TankList> TankList { get; set; }
    }
    public class TankList
    {
        public string TankNo { get; set; }
        public string TankName { get; set; }
        public Decimal TankVolume { get; set; }

        public string OilCode { get; set; }
        public string OilName { get; set; }
        public List<GunList> GunList { get; set; }


    }

    public class GunList
    {

        public string SysId { get; set; }
        public string GunNo { get; set; }
     

    }
}
