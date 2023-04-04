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
using System.Data;
using System.Collections.Generic;
using SPTool.Model;
namespace SPTool.BLL
{
	/// <summary>
	/// GunConfigure
	/// </summary>
	public partial class GunConfigure
	{
		private readonly SPTool.DAL.GunConfigure dal=new SPTool.DAL.GunConfigure();
		public GunConfigure()
		{}
		#region  BasicMethod
		/// <summary>
		/// 是否存在该记录
		/// </summary>
		public bool Exists(string GunNo)
		{
			return dal.Exists(GunNo);
		}

		/// <summary>
		/// 增加一条数据
		/// </summary>
		public bool Add(SPTool.Model.GunConfigure model)
		{
			return dal.Add(model);
		}

        public bool AddList(List<SPTool.Model.GunConfigure> model)
        {
            return dal.AddList(model);
        }

        /// <summary>
        /// 更新一条数据
        /// </summary>
        public bool Update(SPTool.Model.GunConfigure model)
		{
			return dal.Update(model);
		}

		/// <summary>
		/// 删除一条数据
		/// </summary>
		public bool Delete(string GunNo)
		{
			
			return dal.Delete(GunNo);
		}
		/// <summary>
		/// 删除一条数据
		/// </summary>
		public bool DeleteList(string GunNolist )
		{
			return dal.DeleteList(GunNolist );
		}

		/// <summary>
		/// 得到一个对象实体
		/// </summary>
		public SPTool.Model.GunConfigure GetModel(string GunNo)
		{
			
			return dal.GetModel(GunNo);
		}

        public SPTool.Model.GunConfigure GetModelByPumpHose(string pumpid, string hoseid)
        {
            return dal.GetModelByPumpHose(pumpid,hoseid);
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
		public List<SPTool.Model.GunConfigure> GetModelList(string strWhere)
		{
			DataSet ds = dal.GetList(strWhere);
			return DataTableToList(ds.Tables[0]);
		}
		/// <summary>
		/// 获得数据列表
		/// </summary>
		public List<SPTool.Model.GunConfigure> DataTableToList(DataTable dt)
		{
			List<SPTool.Model.GunConfigure> modelList = new List<SPTool.Model.GunConfigure>();
			int rowsCount = dt.Rows.Count;
			if (rowsCount > 0)
			{
				SPTool.Model.GunConfigure model;
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

