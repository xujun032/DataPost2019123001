/*
 *代码由框架生成,任何更改都可能导致被代码生成器覆盖
 *如果数据库字段发生变化，请在代码生器重新生成此Model
 */
using SqlSugar;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace ConsumerWorkerService.MessageManage.Model
{

    /// <summary>
    /// 发票发生失败汇总
    /// </summary>
    public class InvoiceErrorMsg
    {
        /// <summary>
        /// 发票发生失败汇总
        /// </summary>
        public InvoiceErrorMsg()
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

 

        private System.DateTime _POSTDATE;
        /// <summary>
        /// 交易日期
        /// </summary>
        public System.DateTime POSTDATE { get { return this._POSTDATE; } set { this._POSTDATE = value; } }

        private System.String _REQUESTID;
        /// <summary>
        /// 
        /// </summary>
        public System.String REQUESTID { get { return this._REQUESTID; } set { this._REQUESTID = value; } }

        private System.Int32? _TILLNUM;
        /// <summary>
        /// 小票号
        /// </summary>
        public System.Int32? TILLNUM { get { return this._TILLNUM; } set { this._TILLNUM = value; } }

        private System.DateTime? _CREATEDATE;
        /// <summary>
        /// 最后发送时间
        /// </summary>
        public System.DateTime? CREATEDATE { get { return this._CREATEDATE; } set { this._CREATEDATE = value; } }

        private System.Int32? _STATUS;
        /// <summary>
        /// 状态，0--首次发送失败，1--重发失败，2--重发成功
        /// </summary>
        public System.Int32? STATUS { get { return this._STATUS; } set { this._STATUS = value; } }

        private System.String _SENDDATA;
        /// <summary>
        /// 发送报文
        /// </summary>
        public System.String SENDDATA { get { return this._SENDDATA; } set { this._SENDDATA = value; } }

        private System.String _MESSAGE;
        /// <summary>
        /// 失败原因
        /// </summary>
        public System.String MESSAGE { get { return this._MESSAGE; } set { this._MESSAGE = value; } }

        private System.String _REMARK1;
        /// <summary>
        /// 备注1
        /// </summary>
        public System.String REMARK1 { get { return this._REMARK1; } set { this._REMARK1 = value; } }

        private System.String _REMARK2;
        /// <summary>
        /// 备注2
        /// </summary>
        public System.String REMARK2 { get { return this._REMARK2; } set { this._REMARK2 = value; } }
    }
}