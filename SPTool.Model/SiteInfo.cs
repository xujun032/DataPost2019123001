using System;
namespace SPTool.Model
{
    /// <summary>
    /// SiteInfo:实体类(属性说明自动提取数据库字段的描述信息)
    /// </summary>
    [Serializable]
    public partial class SiteInfo
    {
        public SiteInfo()
        { }
        #region Model
        private string _sysno;
        private string _gascode;
        private string _stationno;
        private int? _isactived;
        private int _modeltype;
 
        private string _posturl;
        private string _activeddatetime;
        private string _salelastdatetime;
        private string _tanklastdatetime;
        private string _tankinterval;
        private string _guninterval;
        private string _saledatainterval;
        private string _tankdatainterval;
        private string _purchaseinfointerval;
        private string _tanklastflowno;
        private string _purchaseinfolastdatetime;
        private string _stationname;



  
        public string PurchaseInfoLastDatetime
        {
            set { _purchaseinfolastdatetime = value; }
            get { return _purchaseinfolastdatetime; }
        }
        public string TankLastFlowNo
        {
            set { _tanklastflowno = value; }
            get { return _tanklastflowno; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string SysNo
        {
            set { _sysno = value; }
            get { return _sysno; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string GasCode
        {
            set { _gascode = value; }
            get { return _gascode; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string StationNo
        {
            set { _stationno = value; }
            get { return _stationno; }
        }

        public string StationName
        {
            set { _stationname = value; }
            get { return _stationname; }
        }
        /// <summary>
        /// 
        /// </summary>
        public int? isActived
        {
            set { _isactived = value; }
            get { return _isactived; }
        }
        /// <summary>
        /// 软件运行模式，1为政府追溯模式，2为壳牌内部抽取模式，3 为政府追溯模式+壳牌内部抽取模式
        /// </summary>
        public int ModelType
        {
            set { _modeltype = value; }
            get { return _modeltype; }
        }

        
        /// <summary>
        /// 
        /// </summary>
        public string PostUrl
        {
            set { _posturl = value; }
            get { return _posturl; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string ActivedDatetime
        {
            set { _activeddatetime = value; }
            get { return _activeddatetime; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string SaleLastDatetime
        {
            set { _salelastdatetime = value; }
            get { return _salelastdatetime; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string TankLastDatetime
        {
            set { _tanklastdatetime = value; }
            get { return _tanklastdatetime; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string TankInterval
        {
            set { _tankinterval = value; }
            get { return _tankinterval; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string GunInterval
        {
            set { _guninterval = value; }
            get { return _guninterval; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string SaleDataInterval
        {
            set { _saledatainterval = value; }
            get { return _saledatainterval; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string TankDataInterval
        {
            set { _tankdatainterval = value; }
            get { return _tankdatainterval; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string PurchaseInfoInterval
        {
            set { _purchaseinfointerval = value; }
            get { return _purchaseinfointerval; }
        }
        #endregion Model

    }
}

