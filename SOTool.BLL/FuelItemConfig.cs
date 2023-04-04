/**  版本信息模板在安装目录下，可自行修改。
* FuelItemConfig.cs
*
* 功 能： N/A
* 类 名： FuelItemConfig
*
* Ver    变更日期             负责人  变更内容
* ───────────────────────────────────
* V0.01  2019-12-27 上午 11:30:09   N/A    初版
*
* Copyright (c) 2012 Maticsoft Corporation. All rights reserved.
*┌──────────────────────────────────┐
*│　此技术信息为本公司机密信息，未经本公司书面同意禁止向第三方披露．　│
*│　版权所有：动软卓越（北京）科技有限公司　　　　　　　　　　　　　　│
*└──────────────────────────────────┘
*/
using System;
using System.Data;
using System.Collections.Generic;

using SPTool.Model;
namespace SPTool.BLL
{
	/// <summary>
	/// FuelItemConfig
	/// </summary>
	public partial class FuelItemConfig
	{
		private readonly SPTool.DAL.FuelItemConfig dal=new SPTool.DAL.FuelItemConfig();
		public FuelItemConfig()
		{}
		#region  BasicMethod
		/// <summary>
		/// 是否存在该记录
		/// </summary>
		public bool Exists(string OilCode)
		{
			return dal.Exists(OilCode);
		}

		/// <summary>
		/// 增加一条数据
		/// </summary>
		public bool Add(SPTool.Model.FuelItemConfig model)
		{
			return dal.Add(model);
		}

		/// <summary>
		/// 更新一条数据
		/// </summary>
		public bool Update(SPTool.Model.FuelItemConfig model)
		{
			return dal.Update(model);
		}

		/// <summary>
		/// 删除一条数据
		/// </summary>
		public bool Delete(string OilCode)
		{
			
			return dal.Delete(OilCode);
		}
		/// <summary>
		/// 删除一条数据
		/// </summary>
		public bool DeleteList(string OilCodelist )
		{
			return dal.DeleteList(OilCodelist );
		}

		/// <summary>
		/// 得到一个对象实体
		/// </summary>
		public SPTool.Model.FuelItemConfig GetModel(string OilCode)
		{
			
			return dal.GetModel(OilCode);
		}


        public SPTool.Model.FuelItemConfig GetModelByBarcode(string Barcode)
        {

            return dal.GetModelByBarcode(Barcode);
        }
        /// <summary>
        /// 获得数据列表
        /// </summary>
        public DataSet GetList(string strWhere)
		{
			return dal.GetList(strWhere);
		}
		/// <summary>
		/// 获得数据列表
		/// </summary>
		public List<SPTool.Model.FuelItemConfig> GetModelList(string strWhere)
		{
			DataSet ds = dal.GetList(strWhere);
			return DataTableToList(ds.Tables[0]);
		}
		/// <summary>
		/// 获得数据列表
		/// </summary>
		public List<SPTool.Model.FuelItemConfig> DataTableToList(DataTable dt)
		{
			List<SPTool.Model.FuelItemConfig> modelList = new List<SPTool.Model.FuelItemConfig>();
			int rowsCount = dt.Rows.Count;
			if (rowsCount > 0)
			{
				SPTool.Model.FuelItemConfig model;
				for (int n = 0; n < rowsCount; n++)
				{
					model = dal.DataRowToModel(dt.Rows[n]);
					if (model != null)
					{
						modelList.Add(model);
					}
				}
			}
			return modelList;
		}

		/// <summary>
		/// 获得数据列表
		/// </summary>
		public DataSet GetAllList()
		{
			return GetList("");
		}

		/// <summary>
		/// 分页获取数据列表
		/// </summary>
		public int GetRecordCount(string strWhere)
		{
			return dal.GetRecordCount(strWhere);
		}
		/// <summary>
		/// 分页获取数据列表
		/// </summary>
		public DataSet GetListByPage(string strWhere, string orderby, int startIndex, int endIndex)
		{
			return dal.GetListByPage( strWhere,  orderby,  startIndex,  endIndex);
		}
		/// <summary>
		/// 分页获取数据列表
		/// </summary>
		//public DataSet GetList(int PageSize,int PageIndex,string strWhere)
		//{
			//return dal.GetList(PageSize,PageIndex,strWhere);
		//}

		#endregion  BasicMethod
		#region  ExtensionMethod

		#endregion  ExtensionMethod
	}
}

