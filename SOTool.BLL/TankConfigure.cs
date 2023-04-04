/**  版本信息模板在安装目录下，可自行修改。
* TankConfigure.cs
*
* 功 能： N/A
* 类 名： TankConfigure
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
	/// TankConfigure
	/// </summary>
	public partial class TankConfigure
	{
		private readonly SPTool.DAL.TankConfigure dal=new SPTool.DAL.TankConfigure();
		public TankConfigure()
		{}
		#region  BasicMethod
		/// <summary>
		/// 是否存在该记录
		/// </summary>
		public bool Exists(string TankNo)
		{
			return dal.Exists(TankNo);
		}

		/// <summary>
		/// 增加一条数据
		/// </summary>
		public bool Add(SPTool.Model.TankConfigure model)
		{
			return dal.Add(model);
		}

        public bool AddList(List<SPTool.Model.TankConfigure> model)
        {
            return dal.AddList(model);
        }

        /// <summary>
        /// 更新一条数据
        /// </summary>
        public bool Update(SPTool.Model.TankConfigure model)
		{
			return dal.Update(model);
		}

		/// <summary>
		/// 删除一条数据
		/// </summary>
		public bool Delete(string TankNo)
		{
			
			return dal.Delete(TankNo);
		}
		/// <summary>
		/// 删除一条数据
		/// </summary>
		public bool DeleteList(string TankNolist )
		{
			return dal.DeleteList(TankNolist );
		}

		/// <summary>
		/// 得到一个对象实体
		/// </summary>
		public SPTool.Model.TankConfigure GetModel(string TankNo)
		{
			
			return dal.GetModel(TankNo);
		}



		/// <summary>
		/// 获得数据列表
		/// </summary>
		public DataSet GetList(string strWhere)
		{
			return dal.GetList(strWhere);
		}

        public DataSet GetList1()
        {
            return dal.GetList1();
        }
        /// <summary>
        /// 获得数据列表
        /// </summary>
        public List<SPTool.Model.TankConfigure> GetModelList(string strWhere)
		{
			DataSet ds = dal.GetList(strWhere);
			return DataTableToList(ds.Tables[0]);
		}
		/// <summary>
		/// 获得数据列表
		/// </summary>
		public List<SPTool.Model.TankConfigure> DataTableToList(DataTable dt)
		{
			List<SPTool.Model.TankConfigure> modelList = new List<SPTool.Model.TankConfigure>();
			int rowsCount = dt.Rows.Count;
			if (rowsCount > 0)
			{
				SPTool.Model.TankConfigure model;
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

        public DataSet GetAllList1()
        {
            return GetList1();
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

