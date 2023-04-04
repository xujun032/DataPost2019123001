using SqlSugar;

namespace ConsumerWorkerService.MessageManage.Model
{
    /// <summary>
    /// 
    /// </summary>
    public class TILLITEM_PROMO
    {
        /// <summary>
        /// 
        /// </summary>
        public TILLITEM_PROMO()
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

        private System.DateTime? _POSTED;
        /// <summary>
        /// 
        /// </summary>
        public System.DateTime? POSTED { get { return this._POSTED; } set { this._POSTED = value; } }

        private System.DateTime? _DAY_BATCH_DATE;
        /// <summary>
        /// 业务日期
        /// </summary>
        public System.DateTime? DAY_BATCH_DATE { get { return this._DAY_BATCH_DATE; } set { this._DAY_BATCH_DATE = value; } }

        private System.String _BARCODE;
        /// <summary>
        /// 
        /// </summary>
        public System.String BARCODE { get { return this._BARCODE; } set { this._BARCODE = value; } }

        private System.Int32? _ITEMID;
        /// <summary>
        /// 
        /// </summary>
        public System.Int32? ITEMID { get { return this._ITEMID; } set { this._ITEMID = value; } }

        private System.String _ITEMNAME;
        /// <summary>
        /// 
        /// </summary>
        public System.String ITEMNAME { get { return this._ITEMNAME; } set { this._ITEMNAME = value; } }

        private System.Int32? _DEPT;
        /// <summary>
        /// 
        /// </summary>
        public System.Int32? DEPT { get { return this._DEPT; } set { this._DEPT = value; } }

        private System.String _PROMO_NAME;
        /// <summary>
        /// 
        /// </summary>
        public System.String PROMO_NAME { get { return this._PROMO_NAME; } set { this._PROMO_NAME = value; } }

        private System.String _OPER;
        /// <summary>
        /// 
        /// </summary>
        public System.String OPER { get { return this._OPER; } set { this._OPER = value; } }

        private System.String _PROMO_TYPE;
        /// <summary>
        /// 
        /// </summary>
        public System.String PROMO_TYPE { get { return this._PROMO_TYPE; } set { this._PROMO_TYPE = value; } }

        private System.Int32? _PROMO_ID;
        /// <summary>
        /// 
        /// </summary>
        public System.Int32? PROMO_ID { get { return this._PROMO_ID; } set { this._PROMO_ID = value; } }

        private System.Int32? _OUTREF;
        /// <summary>
        /// 外部促销编号
        /// </summary>
        public System.Int32? OUTREF { get { return this._OUTREF; } set { this._OUTREF = value; } }

        private System.Double? _TOTAL_SAVINGS;
        /// <summary>
        /// 
        /// </summary>
        public System.Double? TOTAL_SAVINGS { get { return this._TOTAL_SAVINGS; } set { this._TOTAL_SAVINGS = value; } }

        private System.Int32? _PROMO_QUANTITY;
        /// <summary>
        /// 
        /// </summary>
        public System.Int32? PROMO_QUANTITY { get { return this._PROMO_QUANTITY; } set { this._PROMO_QUANTITY = value; } }

        private System.Double? _PROMO_ITEM_QTY;
        /// <summary>
        /// 
        /// </summary>
        public System.Double? PROMO_ITEM_QTY { get { return this._PROMO_ITEM_QTY; } set { this._PROMO_ITEM_QTY = value; } }

        private System.String _EXT_REF_1;
        /// <summary>
        /// 
        /// </summary>
        public System.String EXT_REF_1 { get { return this._EXT_REF_1; } set { this._EXT_REF_1 = value; } }

        private System.String _EXT_REF_2;
        /// <summary>
        /// 
        /// </summary>
        public System.String EXT_REF_2 { get { return this._EXT_REF_2; } set { this._EXT_REF_2 = value; } }

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
