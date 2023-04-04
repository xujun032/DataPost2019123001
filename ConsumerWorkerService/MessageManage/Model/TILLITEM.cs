using SqlSugar;

namespace ConsumerWorkerService.MessageManage.Model
{
    /// <summary>
    /// 
    /// </summary>
    public class TILLITEM
    {
        /// <summary>
        /// 
        /// </summary>
        public TILLITEM()
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

        private System.Int32? _POS_ID;
        /// <summary>
        /// 订单销售的POS设备编号
        /// </summary>
        public System.Int32? POS_ID { get { return this._POS_ID; } set { this._POS_ID = value; } }

        private System.DateTime? _DAY_BATCH_DATE;
        /// <summary>
        /// 业务日期
        /// </summary>
        public System.DateTime? DAY_BATCH_DATE { get { return this._DAY_BATCH_DATE; } set { this._DAY_BATCH_DATE = value; } }

        private System.Int32? _STATUSTYPE;
        /// <summary>
        /// 订单类型
        /// </summary>
        public System.Int32? STATUSTYPE { get { return this._STATUSTYPE; } set { this._STATUSTYPE = value; } }

        private System.Int32? _STATUSCODE;
        /// <summary>
        /// 订单类型编号
        /// </summary>
        public System.Int32? STATUSCODE { get { return this._STATUSCODE; } set { this._STATUSCODE = value; } }

        private System.String _BARCODE;
        /// <summary>
        /// 商品编码 非条码
        /// </summary>
        public System.String BARCODE { get { return this._BARCODE; } set { this._BARCODE = value; } }

        private System.Int32? _ITEMID;
        /// <summary>
        /// 站级内部商品编码
        /// </summary>
        public System.Int32? ITEMID { get { return this._ITEMID; } set { this._ITEMID = value; } }

        private System.String _ITEMNAME;
        /// <summary>
        /// 商品名称
        /// </summary>
        public System.String ITEMNAME { get { return this._ITEMNAME; } set { this._ITEMNAME = value; } }

        private System.String _DEPT;
        /// <summary>
        /// 商品品类编号
        /// </summary>
        public System.String DEPT { get { return this._DEPT; } set { this._DEPT = value; } }

        private System.DateTime? _POSTED;
        /// <summary>
        /// 入库时间
        /// </summary>
        public System.DateTime? POSTED { get { return this._POSTED; } set { this._POSTED = value; } }

        private System.Double? _SALEQTY;
        /// <summary>
        /// 销售数量
        /// </summary>
        public System.Double? SALEQTY { get { return this._SALEQTY; } set { this._SALEQTY = value; } }

        private System.Double? _TOTAL;
        /// <summary>
        /// 销售金额
        /// </summary>
        public System.Double? TOTAL { get { return this._TOTAL; } set { this._TOTAL = value; } }

        private System.String _PROMO_TYPE;
        /// <summary>
        /// 促销类型编号
        /// </summary>
        public System.String PROMO_TYPE { get { return this._PROMO_TYPE; } set { this._PROMO_TYPE = value; } }

        private System.Double? _DISCOUNT;
        /// <summary>
        /// 整单应收收金额(优惠前)
        /// </summary>
        public System.Double? DISCOUNT { get { return this._DISCOUNT; } set { this._DISCOUNT = value; } }

        private System.String _UNITNAME;
        /// <summary>
        /// 商品单位
        /// </summary>
        public System.String UNITNAME { get { return this._UNITNAME; } set { this._UNITNAME = value; } }

        private System.Int32? _TANK_ID;
        /// <summary>
        /// 油罐号
        /// </summary>
        public System.Int32? TANK_ID { get { return this._TANK_ID; } set { this._TANK_ID = value; } }

        private System.Int32? _PUMP_ID;
        /// <summary>
        /// 油泵号
        /// </summary>
        public System.Int32? PUMP_ID { get { return this._PUMP_ID; } set { this._PUMP_ID = value; } }

        private System.Int32? _HOSE_ID;
        /// <summary>
        /// 油枪号
        /// </summary>
        public System.Int32? HOSE_ID { get { return this._HOSE_ID; } set { this._HOSE_ID = value; } }

        private System.Int32? _PUMPSRV_REF;
        /// <summary>
        /// 油机交易索引号
        /// </summary>
        public System.Int32? PUMPSRV_REF { get { return this._PUMPSRV_REF; } set { this._PUMPSRV_REF = value; } }

        private System.Double? _PUMP_OPEN_COUNTER;
        /// <summary>
        /// 提枪泵码
        /// </summary>
        public System.Double? PUMP_OPEN_COUNTER { get { return this._PUMP_OPEN_COUNTER; } set { this._PUMP_OPEN_COUNTER = value; } }

        private System.Double? _PUMP_CLOSE_COUNTER;
        /// <summary>
        /// 挂枪泵码
        /// </summary>
        public System.Double? PUMP_CLOSE_COUNTER { get { return this._PUMP_CLOSE_COUNTER; } set { this._PUMP_CLOSE_COUNTER = value; } }

        private System.Double? _STDPRICE;
        /// <summary>
        /// 标准价格
        /// </summary>
        public System.Double? STDPRICE { get { return this._STDPRICE; } set { this._STDPRICE = value; } }

        private System.Double? _PRICE;
        /// <summary>
        /// 价格
        /// </summary>
        public System.Double? PRICE { get { return this._PRICE; } set { this._PRICE = value; } }

        private System.String _ISFUEL;
        /// <summary>
        /// 是否油品
        /// </summary>
        public System.String ISFUEL { get { return this._ISFUEL; } set { this._ISFUEL = value; } }

        private System.Double? _PUMP_TOTAL;
        /// <summary>
        /// 油机显示金额
        /// </summary>
        public System.Double? PUMP_TOTAL { get { return this._PUMP_TOTAL; } set { this._PUMP_TOTAL = value; } }

        private System.Double? _PROMO_AMNT;
        /// <summary>
        /// 
        /// </summary>
        public System.Double? PROMO_AMNT { get { return this._PROMO_AMNT; } set { this._PROMO_AMNT = value; } }

        private System.Int32? _PROMO_ID;
        /// <summary>
        /// 内部促销编号
        /// </summary>
        public System.Int32? PROMO_ID { get { return this._PROMO_ID; } set { this._PROMO_ID = value; } }

        private System.DateTime? _CREATEDATE;
        /// <summary>
        /// 数据插入时间
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


    }
}
