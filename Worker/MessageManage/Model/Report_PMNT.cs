using SqlSugar;

namespace ConsumerWorkerService.MessageManage.Model
{
    public class Report_PMNT
    {
        /// <summary>
        /// 
        /// </summary>
        public Report_PMNT()
        {
        }



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



        private System.Int32? _FUELTIMES;
        /// <summary>
        /// 车次
        /// </summary>
        public System.Int32? FUELTIMES { get { return this._FUELTIMES; } set { this._FUELTIMES = value; } }


    }
}
