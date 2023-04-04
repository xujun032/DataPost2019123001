using SqlSugar;

namespace ConsumerWorkerService.MessageManage.Model
{
    /// <summary>
    /// 
    /// </summary>
    public class Report_SummaryDry
    {
        /// <summary>
        /// 
        /// </summary>
        public Report_SummaryDry()
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

        private System.String _STATUS;
        /// <summary>
        /// 营业日状态
        /// </summary>
        public System.String STATUS { get { return this._STATUS; } set { this._STATUS = value; } }

        private System.Int32? _DRYTIMES;
        /// <summary>
        /// 非油来客数
        /// </summary>
        public System.Int32? DRYTIMES { get { return this._DRYTIMES; } set { this._DRYTIMES = value; } }

        private System.String _DEPT1;
        /// <summary>
        /// 商品品类
        /// </summary>
        public System.String DEPT1 { get { return this._DEPT1; } set { this._DEPT1 = value; } }

        private System.Double? _QTY;
        /// <summary>
        /// 销售金额
        /// </summary>
        public System.Double? QTY { get { return this._QTY; } set { this._QTY = value; } }

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
