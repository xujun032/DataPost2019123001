﻿/**  版本信息模板在安装目录下，可自行修改。
* FuelItemConfig.cs
*
* 功 能： N/A
* 类 名： FuelItemConfig
*
* Ver    变更日期             负责人  变更内容
* ───────────────────────────────────
* V0.01  2019-12-27 上午 11:30:08   N/A    初版
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
	/// FuelItemConfig:实体类(属性说明自动提取数据库字段的描述信息)
	/// </summary>
	[Serializable]
	public partial class FuelItemConfig
	{
		public FuelItemConfig()
		{}
		#region Model
		private string _oilcode;
		private string _oilname;
		private string _barcode;
		private string _remark1;
		private string _remark2;
		/// <summary>
		/// 
		/// </summary>
		public string OilCode
		{
			set{ _oilcode=value;}
			get{return _oilcode;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string OilName
		{
			set{ _oilname=value;}
			get{return _oilname;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string BarCode
		{
			set{ _barcode=value;}
			get{return _barcode;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string Remark1
		{
			set{ _remark1=value;}
			get{return _remark1;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string Remark2
		{
			set{ _remark2=value;}
			get{return _remark2;}
		}
		#endregion Model

	}
}

