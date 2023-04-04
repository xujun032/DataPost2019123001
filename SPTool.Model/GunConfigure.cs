/**  版本信息模板在安装目录下，可自行修改。
* GunConfigure.cs
*
* 功 能： N/A
* 类 名： GunConfigure
*
* Ver    变更日期             负责人  变更内容
* ───────────────────────────────────
* V0.01  2019-12-28 上午 1:04:27   N/A    初版
*
* Copyright (c) 2012 Maticsoft Corporation. All rights reserved.
*┌──────────────────────────────────┐
*│　此技术信息为本公司机密信息，未经本公司书面同意禁止向第三方披露．　│
*│　版权所有：动软卓越（北京）科技有限公司　　　　　　　　　　　　　　│
*└──────────────────────────────────┘
*/
using System;
namespace SPTool.Model
{
	/// <summary>
	/// GunConfigure:实体类(属性说明自动提取数据库字段的描述信息)
	/// </summary>
	[Serializable]
	public partial class GunConfigure
	{
		public GunConfigure()
		{}
		#region Model
		private string _gunno;
		private string _sysid;
		private string _tankno;
		private string _pumpid;
		private string _hoseid;
		/// <summary>
		/// 
		/// </summary>
		public string GunNo
		{
			set{ _gunno=value;}
			get{return _gunno;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string SysId
		{
			set{ _sysid=value;}
			get{return _sysid;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string TankNo
		{
			set{ _tankno=value;}
			get{return _tankno;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string PumpId
		{
			set{ _pumpid=value;}
			get{return _pumpid;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string HoseId
		{
			set{ _hoseid=value;}
			get{return _hoseid;}
		}
		#endregion Model

	}



    [Serializable]
    public partial class PostGunConfigure
    {
        public PostGunConfigure()
        { }
        #region Model
        private string _stationno;
        private string _gunno;
        private string _sysid;
        private string _tankno;



        public string StationNo
        {
            set { _stationno = value; }
            get { return _stationno; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string GunNo
        {
            set { _gunno = value; }
            get { return _gunno; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string SysId
        {
            set { _sysid = value; }
            get { return _sysid; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string TankNo
        {
            set { _tankno = value; }
            get { return _tankno; }
        }
        /// <summary>
        /// 
        /// </summary>

        #endregion Model

    }
}

