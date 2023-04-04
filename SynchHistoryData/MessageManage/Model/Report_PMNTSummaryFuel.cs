using SqlSugar;


namespace ConsumerWorkerService.MessageManage.Model
{
    /// <summary>
    /// 
    /// </summary>
    public class Report_PMNTSummaryFuel
    {
        /// <summary>
        /// 
        /// </summary>
        public Report_PMNTSummaryFuel()
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

        private System.DateTime _DAY_BATCH_DATE;
        /// <summary>
        /// 业务日期
        /// </summary>
        public System.DateTime DAY_BATCH_DATE { get { return this._DAY_BATCH_DATE; } set { this._DAY_BATCH_DATE = value; } }

        private System.String _BARCODE;
        /// <summary>
        /// 商品编码 非条码
        /// </summary>
        public System.String BARCODE { get { return this._BARCODE; } set { this._BARCODE = value; } }

        private System.String _ITEMNAME;
        /// <summary>
        /// 商品名称
        /// </summary>
        public System.String ITEMNAME { get { return this._ITEMNAME; } set { this._ITEMNAME = value; } }

        private System.Double? _QTY;
        /// <summary>
        /// 全部油品销量合计
        /// </summary>
        public System.Double? QTY { get { return this._QTY; } set { this._QTY = value; } }

        private System.String _STATUS;
        /// <summary>
        /// 营业日状态
        /// </summary>
        public System.String STATUS { get { return this._STATUS; } set { this._STATUS = value; } }

        private System.Int32? _FUELTIMES;
        /// <summary>
        /// 车次
        /// </summary>
        public System.Int32? FUELTIMES { get { return this._FUELTIMES; } set { this._FUELTIMES = value; } }

        private System.Double? _VIPQTY;
        /// <summary>
        /// 会员销量
        /// </summary>
        public System.Double? VIPQTY { get { return this._VIPQTY; } set { this._VIPQTY = value; } }

        private System.Double? _B2CQTY;
        /// <summary>
        /// B2C卡销量
        /// </summary>
        public System.Double? B2CQTY { get { return this._B2CQTY; } set { this._B2CQTY = value; } }

        private System.Double? _VIPQTY1;
        /// <summary>
        /// 会员销量减去B2C卡销量,普通会员销量
        /// </summary>
        public System.Double? VIPQTY1 { get { return this._VIPQTY1; } set { this._VIPQTY1 = value; } }

        private System.Double? _B2BQTY;
        /// <summary>
        /// B2B卡销量
        /// </summary>
        public System.Double? B2BQTY { get { return this._B2BQTY; } set { this._B2BQTY = value; } }


        /// <summary>
        /// 一键加油销量
        /// </summary>
        private System.Double? _OneKeyQTY;
        /// <summary>
        /// 一键加油销量
        /// </summary>
        public System.Double? OneKeyQTY { get { return this._OneKeyQTY; } set { this._OneKeyQTY = value; } }

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

        private System.String _REMARK3;
        /// <summary>
        /// 
        /// </summary>
        public System.String REMARK3 { get { return this._REMARK3; } set { this._REMARK3 = value; } }

        private System.String _REMARK4;
        /// <summary>
        /// 
        /// </summary>
        public System.String REMARK4 { get { return this._REMARK4; } set { this._REMARK4 = value; } }
    }
}
