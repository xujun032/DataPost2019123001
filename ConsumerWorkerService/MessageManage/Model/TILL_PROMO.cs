using SqlSugar;

namespace ConsumerWorkerService.MessageManage.Model
{
    /// <summary>
    /// 
    /// </summary>
    public class TILL_PROMO
    {
        /// <summary>
        /// 
        /// </summary>
        public TILL_PROMO()
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

        private System.Int32? _POS_ID;
        /// <summary>
        /// POS编号
        /// </summary>
        public System.Int32? POS_ID { get { return this._POS_ID; } set { this._POS_ID = value; } }

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

        private System.Int32? _PROMO_ID;
        /// <summary>
        /// 内部促销编号
        /// </summary>
        public System.Int32? PROMO_ID { get { return this._PROMO_ID; } set { this._PROMO_ID = value; } }

        private System.Int32? _OUTREF;
        /// <summary>
        /// 外部促销编号
        /// </summary>
        public System.Int32? OUTREF { get { return this._OUTREF; } set { this._OUTREF = value; } }

        private System.Int32? _PROMO_QUANTITY;
        /// <summary>
        /// 促销数量
        /// </summary>
        public System.Int32? PROMO_QUANTITY { get { return this._PROMO_QUANTITY; } set { this._PROMO_QUANTITY = value; } }

        private System.Double? _TOTAL_SAVINGS;
        /// <summary>
        /// 促销金额
        /// </summary>
        public System.Double? TOTAL_SAVINGS { get { return this._TOTAL_SAVINGS; } set { this._TOTAL_SAVINGS = value; } }

        private System.String _PROMO_NAME;
        /// <summary>
        /// 促销规则名称
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
