using SqlSugar;

namespace ConsumerWorkerService.MessageManage.Model
{
    /// <summary>
    /// 
    /// </summary>
    public class TILL_PMNT
    {
        /// <summary>
        /// 
        /// </summary>
        public TILL_PMNT()
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
        /// 自然日
        /// </summary>
        public System.DateTime? POSTED { get { return this._POSTED; } set { this._POSTED = value; } }

        private System.DateTime? _DAY_BATCH_DATE;
        /// <summary>
        /// 业务日期
        /// </summary>
        public System.DateTime? DAY_BATCH_DATE { get { return this._DAY_BATCH_DATE; } set { this._DAY_BATCH_DATE = value; } }

        private System.Int32? _PMNTID;
        /// <summary>
        /// 站内支付编码
        /// </summary>
        public System.Int32? PMNTID { get { return this._PMNTID; } set { this._PMNTID = value; } }

        private System.String _PMNT_NAME;
        /// <summary>
        /// 支付名称
        /// </summary>
        public System.String PMNT_NAME { get { return this._PMNT_NAME; } set { this._PMNT_NAME = value; } }

        private System.String _PMNTEXTREF;
        /// <summary>
        /// 外部支付编码
        /// </summary>
        public System.String PMNTEXTREF { get { return this._PMNTEXTREF; } set { this._PMNTEXTREF = value; } }

        private System.Double? _SSUM;
        /// <summary>
        /// 实际支付金额
        /// </summary>
        public System.Double? SSUM { get { return this._SSUM; } set { this._SSUM = value; } }

        private System.Double? _FSUM;
        /// <summary>
        /// 应支付金额
        /// </summary>
        public System.Double? FSUM { get { return this._FSUM; } set { this._FSUM = value; } }

        private System.String _CURRENCY_SYMBOL;
        /// <summary>
        /// 
        /// </summary>
        public System.String CURRENCY_SYMBOL { get { return this._CURRENCY_SYMBOL; } set { this._CURRENCY_SYMBOL = value; } }

        private System.Int32? _CURID;
        /// <summary>
        /// 
        /// </summary>
        public System.Int32? CURID { get { return this._CURID; } set { this._CURID = value; } }

        private System.String _CARDNUM;
        /// <summary>
        /// 卡号
        /// </summary>
        public System.String CARDNUM { get { return this._CARDNUM; } set { this._CARDNUM = value; } }

        private System.Int32? _REFERENCE;
        /// <summary>
        /// 
        /// </summary>
        public System.Int32? REFERENCE { get { return this._REFERENCE; } set { this._REFERENCE = value; } }

        private System.Int32? _MERCHANT;
        /// <summary>
        /// 
        /// </summary>
        public System.Int32? MERCHANT { get { return this._MERCHANT; } set { this._MERCHANT = value; } }

        private System.String _APPROVAL;
        /// <summary>
        /// 
        /// </summary>
        public System.String APPROVAL { get { return this._APPROVAL; } set { this._APPROVAL = value; } }

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
