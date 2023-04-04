using SqlSugar;

namespace ConsumerWorkerService.MessageManage.Model
{
    /// <summary>
    /// 
    /// </summary>
    public class SITEDAYBATCH
    {
        /// <summary>
        /// 
        /// </summary>
        public SITEDAYBATCH()
        {
        }

        private System.String _ID;
        [SugarColumn(IsPrimaryKey =true)]
        /// <summary>
        /// 
        /// </summary>
        public System.String ID { get { return this._ID; } set { this._ID = value; } }

        private System.String _SITEID;
        /// <summary>
        /// 
        /// </summary>
        public System.String SITEID { get { return this._SITEID; } set { this._SITEID = value; } }

        private System.String _SITENAME;
        /// <summary>
        /// 
        /// </summary>
        public System.String SITENAME { get { return this._SITENAME; } set { this._SITENAME = value; } }

        private System.Int32? _PRENTID;
        /// <summary>
        /// 
        /// </summary>
        public System.Int32? PRENTID { get { return this._PRENTID; } set { this._PRENTID = value; } }

        private System.DateTime _DAY_BATCH_DATE;
        /// <summary>
        /// 
        /// </summary>
        public System.DateTime DAY_BATCH_DATE { get { return this._DAY_BATCH_DATE; } set { this._DAY_BATCH_DATE = value; } }

        private System.DateTime? _DAY_BATCH_OPEN;
        /// <summary>
        /// 营业日开始时间
        /// </summary>
        public System.DateTime? DAY_BATCH_OPEN { get { return this._DAY_BATCH_OPEN; } set { this._DAY_BATCH_OPEN = value; } }

        private System.DateTime? _DAY_BATCH_READY;
        /// <summary>
        /// 营业日结束时间
        /// </summary>
        public System.DateTime? DAY_BATCH_READY { get { return this._DAY_BATCH_READY; } set { this._DAY_BATCH_READY = value; } }

        private System.DateTime? _DAY_BATCH_CLOSE;
        /// <summary>
        /// 
        /// </summary>
        public System.DateTime? DAY_BATCH_CLOSE { get { return this._DAY_BATCH_CLOSE; } set { this._DAY_BATCH_CLOSE = value; } }

        private System.String _STATUS;
        /// <summary>
        /// 营业日状态
        /// </summary>
        public System.String STATUS { get { return this._STATUS; } set { this._STATUS = value; } }

        private System.String _EXPORTSTATUS;
        /// <summary>
        /// 数据上传状态
        /// </summary>
        public System.String EXPORTSTATUS { get { return this._EXPORTSTATUS; } set { this._EXPORTSTATUS = value; } }

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
