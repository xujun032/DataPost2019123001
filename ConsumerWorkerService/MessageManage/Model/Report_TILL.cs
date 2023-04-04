using SqlSugar;

namespace ConsumerWorkerService.MessageManage.Model
{
    /// <summary>
    /// 
    /// </summary>
    public class Report_TILL
    {
        /// <summary>
        /// 
        /// </summary>
        public Report_TILL()
        {
        }

        private System.String _ID;
        [SugarColumn(IsPrimaryKey = true)]
        /// <summary>
        /// 主键
        /// </summary>
        public System.String ID { get { return this._ID; } set { this._ID = value; } }

        private System.String _SITEID;
        /// <summary>
        /// 门店编码
        /// </summary>
        public System.String SITEID { get { return this._SITEID; } set { this._SITEID = value; } }

        private System.String _SITENAME;
        /// <summary>
        /// 加油站名称
        /// </summary>
        public System.String SITENAME { get { return this._SITENAME; } set { this._SITENAME = value; } }

        private System.Int32? _POS_ID;
        /// <summary>
        /// 订单销售的POS设备编号
        /// </summary>
        public System.Int32? POS_ID { get { return this._POS_ID; } set { this._POS_ID = value; } }

        private System.String _WTR;
        /// <summary>
        /// 收银员名称
        /// </summary>
        public System.String WTR { get { return this._WTR; } set { this._WTR = value; } }

        private System.Int32? _TILLNUM;
        /// <summary>
        /// 小票号
        /// </summary>
        public System.Int32? TILLNUM { get { return this._TILLNUM; } set { this._TILLNUM = value; } }

        private System.DateTime? _POSTED;
        /// <summary>
        /// 订单交易日期
        /// </summary>
        public System.DateTime? POSTED { get { return this._POSTED; } set { this._POSTED = value; } }

        private System.DateTime? _DAY_BATCH_DATE;
        /// <summary>
        /// 业务日期
        /// </summary>
        public System.DateTime? DAY_BATCH_DATE { get { return this._DAY_BATCH_DATE; } set { this._DAY_BATCH_DATE = value; } }

        private System.String _BARCODE;
        /// <summary>
        /// 商品编码 非条码
        /// </summary>
        public System.String BARCODE { get { return this._BARCODE; } set { this._BARCODE = value; } }

        private System.String _DEPT;
        /// <summary>
        /// 商品品类
        /// </summary>
        public System.String DEPT { get { return this._DEPT; } set { this._DEPT = value; } }

        private System.String _ITEMNAME;
        /// <summary>
        /// 商品名称
        /// </summary>
        public System.String ITEMNAME { get { return this._ITEMNAME; } set { this._ITEMNAME = value; } }

        private System.Double? _STDPRICE;
        /// <summary>
        /// 单价
        /// </summary>
        public System.Double? STDPRICE { get { return this._STDPRICE; } set { this._STDPRICE = value; } }

        private System.Double? _SALEQTY;
        /// <summary>
        /// 数量
        /// </summary>
        public System.Double? SALEQTY { get { return this._SALEQTY; } set { this._SALEQTY = value; } }

        private System.Double? _RETAIL_AMOUNT;
        /// <summary>
        /// 应收金额
        /// </summary>
        public System.Double? RETAIL_AMOUNT { get { return this._RETAIL_AMOUNT; } set { this._RETAIL_AMOUNT = value; } }

        private System.Double? _DISCOUNT_AMOUNT;
        /// <summary>
        /// 优惠金额
        /// </summary>
        public System.Double? DISCOUNT_AMOUNT { get { return this._DISCOUNT_AMOUNT; } set { this._DISCOUNT_AMOUNT = value; } }

        private System.Double? _TOTAL;
        /// <summary>
        /// 实付金额
        /// </summary>
        public System.Double? TOTAL { get { return this._TOTAL; } set { this._TOTAL = value; } }

        private System.Int32? _STATUSTYPE;
        /// <summary>
        /// 交易类型
        /// </summary>
        public System.Int32? STATUSTYPE { get { return this._STATUSTYPE; } set { this._STATUSTYPE = value; } }

        private System.String _CUST_MAGKEY;
        /// <summary>
        /// 会员编号
        /// </summary>
        public System.String CUST_MAGKEY { get { return this._CUST_MAGKEY; } set { this._CUST_MAGKEY = value; } }

        private System.String? _ISVIP;
        /// <summary>
        /// 是否会员
        /// </summary>
        public System.String ISVIP { get { return this._ISVIP; } set { this._ISVIP = value; } }

        private System.DateTime? _CREATEDATE;
        /// <summary>
        /// 数据上传时间
        /// </summary>
        public System.DateTime? CREATEDATE { get { return this._CREATEDATE; } set { this._CREATEDATE = value; } }

        private System.String _REMARK1;
        /// <summary>
        /// 
        /// </summary>
        public System.String REMARK1 { get { return this._REMARK1; } set { this._REMARK1 = value; } }

        private System.String _REMARK2;
        /// <summary>
        /// 
        /// </summary>
        public System.String REMARK2 { get { return this._REMARK2; } set { this._REMARK2 = value; } }


        private System.Int32? _ISSEND;
        /// <summary>
        /// 
        /// </summary>
        public System.Int32? ISSEND { get { return this._ISSEND; } set { this._ISSEND = value; } }
    }
}
