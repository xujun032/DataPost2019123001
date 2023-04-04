
using System;
using System.Data;
using System.Collections.Generic;

using SPTool.Model;
namespace SPTool.BLL
{
	/// <summary>
	/// SiteInfo
	/// </summary>
	public partial class SiteInfo
	{
		private readonly SPTool.DAL.SiteInfo dal=new SPTool.DAL.SiteInfo();
		public SiteInfo()
		{}
		#region  BasicMethod
		/// <summary>
		/// 是否存在该记录
		/// </summary>
		public bool Exists(string SysNo)
		{
			return dal.Exists(SysNo);
		}

		/// <summary>
		/// 增加一条数据
		/// </summary>
		public bool Add(SPTool.Model.SiteInfo model)
		{
			return dal.Add(model);
		}

		/// <summary>
		/// 更新一条数据
		/// </summary>
		public bool Update(SPTool.Model.SiteInfo model)
		{
			return dal.Update(model);
		}

		/// <summary>
		/// 删除一条数据
		/// </summary>
		public bool Delete(string SysNo)
		{
			
			return dal.Delete(SysNo);
		}
		/// <summary>
		/// 删除一条数据
		/// </summary>
		public bool DeleteList(string SysNolist )
		{
			return dal.DeleteList(SysNolist );
		}

		/// <summary>
		/// 得到一个对象实体
		/// </summary>
		public SPTool.Model.SiteInfo GetTopModel()
		{		
			return dal.GetTopModel();
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
		public List<SPTool.Model.SiteInfo> GetModelList(string strWhere)
		{
			DataSet ds = dal.GetList(strWhere);
			return DataTableToList(ds.Tables[0]);
		}
		/// <summary>
		/// 获得数据列表
		/// </summary>
		public List<SPTool.Model.SiteInfo> DataTableToList(DataTable dt)
		{
			List<SPTool.Model.SiteInfo> modelList = new List<SPTool.Model.SiteInfo>();
			int rowsCount = dt.Rows.Count;
			if (rowsCount > 0)
			{
				SPTool.Model.SiteInfo model;
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

