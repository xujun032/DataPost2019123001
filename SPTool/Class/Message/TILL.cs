

namespace SPTool
{
    /// <summary>
    /// 
    /// </summary>
    public class TILL
    {
        /// <summary>
        /// 
        /// </summary>
        public TILL()
        {
        }

        private System.String _ID;
        /// <summary>
        /// 主键
        /// </summary>
        public System.String ID { get { return this._ID; } set { this._ID = value; } }

        private System.String _SITEID;
        /// <summary>
        /// 门店编码
        /// </summary>
        public System.String SITEID { get { return this._SITEID; } set { this._SITEID = value; } }

        private System.Int32? _TILLNUM;
        /// <summary>
        /// 小票号
        /// </summary>
        public System.Int32? TILLNUM { get { return this._TILLNUM; } set { this._TILLNUM = value; } }

        private System.DateTime? _DAY_BATCH_DATE;
        /// <summary>
        /// 业务日期
        /// </summary>
        public System.DateTime? DAY_BATCH_DATE { get { return this._DAY_BATCH_DATE; } set { this._DAY_BATCH_DATE = value; } }

        private System.DateTime? _SALEDATE;
        /// <summary>
        /// 订单销售的时间
        /// </summary>
        public System.DateTime? SALEDATE { get { return this._SALEDATE; } set { this._SALEDATE = value; } }

        private System.String _WTR;
        /// <summary>
        /// 收银员名称
        /// </summary>
        public System.String WTR { get { return this._WTR; } set { this._WTR = value; } }

        private System.DateTime? _TIMEOPEN;
        /// <summary>
        /// 开始时间
        /// </summary>
        public System.DateTime? TIMEOPEN { get { return this._TIMEOPEN; } set { this._TIMEOPEN = value; } }

        private System.DateTime? _TIMECLOSE;
        /// <summary>
        /// 结束时间
        /// </summary>
        public System.DateTime? TIMECLOSE { get { return this._TIMECLOSE; } set { this._TIMECLOSE = value; } }

        private System.Int32? _GUESTS;
        /// <summary>
        /// 是否会员0 非会员 1 是会员
        /// </summary>
        public System.Int32? GUESTS { get { return this._GUESTS; } set { this._GUESTS = value; } }

        private System.String _CUST_MAGKEY;
        /// <summary>
        /// 会员编号
        /// </summary>
        public System.String CUST_MAGKEY { get { return this._CUST_MAGKEY; } set { this._CUST_MAGKEY = value; } }

        private System.Int32? _TIMESLICE;
        /// <summary>
        /// 时段编号 15分钟加1
        /// </summary>
        public System.Int32? TIMESLICE { get { return this._TIMESLICE; } set { this._TIMESLICE = value; } }

        private System.Int32? _STATUSTYPE;
        /// <summary>
        /// 
        /// </summary>
        public System.Int32? STATUSTYPE { get { return this._STATUSTYPE; } set { this._STATUSTYPE = value; } }

        private System.Int32? _STATUSCODE;
        /// <summary>
        /// 
        /// </summary>
        public System.Int32? STATUSCODE { get { return this._STATUSCODE; } set { this._STATUSCODE = value; } }

        private System.DateTime? _POSTED;
        /// <summary>
        /// 入库时间
        /// </summary>
        public System.DateTime? POSTED { get { return this._POSTED; } set { this._POSTED = value; } }

        private System.Double? _SSUM;
        /// <summary>
        /// 整单实收金额
        /// </summary>
        public System.Double? SSUM { get { return this._SSUM; } set { this._SSUM = value; } }

        private System.Double? _RSUM;
        /// <summary>
        /// 整单应收收金额(优惠前)
        /// </summary>
        public System.Double? RSUM { get { return this._RSUM; } set { this._RSUM = value; } }

        private System.Int32? _PRICE_TYPE_ID;
        /// <summary>
        /// 价格类型ID
        /// </summary>
        public System.Int32? PRICE_TYPE_ID { get { return this._PRICE_TYPE_ID; } set { this._PRICE_TYPE_ID = value; } }

        private System.String _EXTREF2;
        /// <summary>
        /// 授权号
        /// </summary>
        public System.String EXTREF2 { get { return this._EXTREF2; } set { this._EXTREF2 = value; } }

        private System.Int32? _LINK_DAY_BATCH_DATE;
        /// <summary>
        /// 退货时关联的业务日期
        /// </summary>
        public System.Int32? LINK_DAY_BATCH_DATE { get { return this._LINK_DAY_BATCH_DATE; } set { this._LINK_DAY_BATCH_DATE = value; } }

        private System.Int32? _LINK_TILLNUM;
        /// <summary>
        /// 退货时原始小票号
        /// </summary>
        public System.Int32? LINK_TILLNUM { get { return this._LINK_TILLNUM; } set { this._LINK_TILLNUM = value; } }

        private System.Double? _QTY;
        /// <summary>
        /// 数量
        /// </summary>
        public System.Double? QTY { get { return this._QTY; } set { this._QTY = value; } }

        private System.Int32? _TICKET;
        /// <summary>
        /// 是否已打印出小票1已打印 0 未出票
        /// </summary>
        public System.Int32? TICKET { get { return this._TICKET; } set { this._TICKET = value; } }

        private System.Int32? _PREMIERPOINTS;
        /// <summary>
        /// 原有积分
        /// </summary>
        public System.Int32? PREMIERPOINTS { get { return this._PREMIERPOINTS; } set { this._PREMIERPOINTS = value; } }

        private System.Int32? _BONUSPOINTS;
        /// <summary>
        /// 获得积分
        /// </summary>
        public System.Int32? BONUSPOINTS { get { return this._BONUSPOINTS; } set { this._BONUSPOINTS = value; } }

        private System.Int32? _ITEM_SOLD;
        /// <summary>
        /// 订单中商品的种类个数
        /// </summary>
        public System.Int32? ITEM_SOLD { get { return this._ITEM_SOLD; } set { this._ITEM_SOLD = value; } }

        private System.String _BANKTERMID;
        /// <summary>
        /// 银行终端号
        /// </summary>
        public System.String BANKTERMID { get { return this._BANKTERMID; } set { this._BANKTERMID = value; } }

        private System.String _EXTREF3;
        /// <summary>
        /// 
        /// </summary>
        public System.String EXTREF3 { get { return this._EXTREF3; } set { this._EXTREF3 = value; } }

        private System.String _ENTRY_METHOD;
        /// <summary>
        /// 
        /// </summary>
        public System.String ENTRY_METHOD { get { return this._ENTRY_METHOD; } set { this._ENTRY_METHOD = value; } }

        private System.Int32? _CUSTOMER_GROUP_ID;
        /// <summary>
        /// 客人群组ID 预留
        /// </summary>
        public System.Int32? CUSTOMER_GROUP_ID { get { return this._CUSTOMER_GROUP_ID; } set { this._CUSTOMER_GROUP_ID = value; } }

        private System.Int32? _FUEL_TRANS_TYPE;
        /// <summary>
        /// 
        /// </summary>
        public System.Int32? FUEL_TRANS_TYPE { get { return this._FUEL_TRANS_TYPE; } set { this._FUEL_TRANS_TYPE = value; } }

        private System.String _IS_INVOICED;
        /// <summary>
        /// 
        /// </summary>
        public System.String IS_INVOICED { get { return this._IS_INVOICED; } set { this._IS_INVOICED = value; } }

        private System.DateTime? _CLOSE_TIMESTAMP;
        /// <summary>
        /// 
        /// </summary>
        public System.DateTime? CLOSE_TIMESTAMP { get { return this._CLOSE_TIMESTAMP; } set { this._CLOSE_TIMESTAMP = value; } }

        private System.Int32? _FUELTRANSTYPE;
        /// <summary>
        /// 
        /// </summary>
        public System.Int32? FUELTRANSTYPE { get { return this._FUELTRANSTYPE; } set { this._FUELTRANSTYPE = value; } }

        private System.String _INVOICE_PRINTED;
        /// <summary>
        /// 
        /// </summary>
        public System.String INVOICE_PRINTED { get { return this._INVOICE_PRINTED; } set { this._INVOICE_PRINTED = value; } }

        private System.String _ISRECONCILED;
        /// <summary>
        /// 
        /// </summary>
        public System.String ISRECONCILED { get { return this._ISRECONCILED; } set { this._ISRECONCILED = value; } }

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
    }
}
