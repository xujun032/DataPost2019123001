
using System;
using System.Data;
using System.Text;
using System.Data.SQLite;
using DBUtility;//Please add references
using System.IO;

namespace SPTool.DAL
{
	/// <summary>
	/// 数据访问类:SiteInfo
	/// </summary>
	public partial class SiteInfo
	{
		public SiteInfo()
		{}
		#region  Method


		/// <summary>
		/// 是否存在该记录
		/// </summary>
		public bool Exists(string SysNo)
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("select count(1) from SiteInfo");
			strSql.Append(" where SysNo='"+SysNo+"' ");
			return DbHelperSQLite.Exists(strSql.ToString());
		}

		/// <summary>
		/// 增加一条数据
		/// </summary>
		public bool Add(SPTool.Model.SiteInfo model)
		{
			StringBuilder strSql=new StringBuilder();
			StringBuilder strSql1=new StringBuilder();
			StringBuilder strSql2=new StringBuilder();
			if (model.SysNo != null)
			{
				strSql1.Append("SysNo,");
				strSql2.Append("'"+model.SysNo+"',");
			}
			if (model.GasCode != null)
			{
				strSql1.Append("GasCode,");
				strSql2.Append("'"+model.GasCode+"',");
			}
			if (model.StationNo != null)
			{
				strSql1.Append("StationNo,");
				strSql2.Append("'"+model.StationNo+"',");
			}
            if (model.StationName != null)
            {
                strSql1.Append("StationName,");
                strSql2.Append("'" + model.StationName + "',");
            }
            if (model.isActived != null)
			{
				strSql1.Append("isActived,");
				strSql2.Append(""+model.isActived+",");
			}
            strSql1.Append("ModelType,");
            strSql2.Append("" + model.ModelType + ",");
            
            if (model.PostUrl != null)
            {
                strSql1.Append("PostUrl,");
                strSql2.Append("'" + model.PostUrl + "',");
            }
            if (model.ActivedDatetime != null)
            {
                strSql1.Append("ActivedDatetime,");
                strSql2.Append("'" + model.ActivedDatetime + "',");
            }
            if (model.SaleLastDatetime != null)
            {
                strSql1.Append("SaleLastDatetime,");
                strSql2.Append("'" + model.SaleLastDatetime + "',");
            }
            if (model.TankLastDatetime != null)
            {
                strSql1.Append("TankLastDatetime,");
                strSql2.Append("'" + model.TankLastDatetime + "',");
            }
            if (model.TankLastFlowNo != null)
            {
                strSql1.Append("TankLastFlowNo,");
                strSql2.Append("'" + model.TankLastFlowNo + "',");
            }
            
            if (model.TankInterval != null)
            {
                strSql1.Append("TankInterval,");
                strSql2.Append("'" + model.TankInterval + "',");
            }
            if (model.GunInterval != null)
            {
                strSql1.Append("GunInterval,");
                strSql2.Append("'" + model.GunInterval + "',");
            }
            if (model.SaleDataInterval != null)
            {
                strSql1.Append("SaleDataInterval,");
                strSql2.Append("'" + model.SaleDataInterval + "',");
            }
            if (model.TankDataInterval != null)
            {
                strSql1.Append("TankDataInterval,");
                strSql2.Append("'" + model.TankDataInterval + "',");
            }
            if (model.PurchaseInfoInterval != null)
            {
                strSql1.Append("PurchaseInfoInterval,");
                strSql2.Append("'" + model.PurchaseInfoInterval + "',");
            }
            if (model.PurchaseInfoLastDatetime != null)
            {
                strSql1.Append("PurchaseInfoLastDatetime,");
                strSql2.Append("'" + model.PurchaseInfoLastDatetime + "',");
            }
            
            strSql.Append("insert into SiteInfo(");
			strSql.Append(strSql1.ToString().Remove(strSql1.Length - 1));
			strSql.Append(")");
			strSql.Append(" values (");
			strSql.Append(strSql2.ToString().Remove(strSql2.Length - 1));
			strSql.Append(")");
			int rows=DbHelperSQLite.ExecuteSql(strSql.ToString());
			if (rows > 0)
			{
				return true;
			}
			else
			{
				return false;
			}
		}

		/// <summary>
		/// 更新一条数据
		/// </summary>
		public bool Update(SPTool.Model.SiteInfo model)
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("update SiteInfo set ");
			if (model.GasCode != null)
			{
				strSql.Append("GasCode='"+model.GasCode+"',");
			}
			else
			{
				strSql.Append("GasCode= null ,");
			}
			if (model.StationNo != null)
			{
				strSql.Append("StationNo='"+model.StationNo+"',");
			}
			else
			{
				strSql.Append("StationNo= null ,");
			}
            if (model.StationName != null)
            {
                strSql.Append("StationName='" + model.StationName + "',");
            }
            else
            {
                strSql.Append("StationNo= null ,");
            }
            if (model.isActived != null)
			{
				strSql.Append("isActived="+model.isActived+",");
			}
			else
			{
				strSql.Append("isActived= null ,");
			}

                strSql.Append("ModelType=" + model.ModelType + ",");
  


            
            if (model.PostUrl != null)
            {
                strSql.Append("PostUrl='" + model.PostUrl + "',");
            }
            else
            {
                strSql.Append("StationNo= null ,");
            }
            if (model.ActivedDatetime != null)
            {
              
                strSql.Append("ActivedDatetime='" + model.ActivedDatetime + "',");
            }
            else
            {
                strSql.Append("ActivedDatetime= null ,");
            }
            if (model.SaleLastDatetime != null)
            {
                strSql.Append("SaleLastDatetime='" + model.SaleLastDatetime + "',");
            }
            else
            {
                strSql.Append("SaleLastDatetime= null ,");
            }
            if (model.TankLastDatetime != null)
            {
                strSql.Append("TankLastDatetime='" + model.TankLastDatetime + "',");
            }
            else
            {
                strSql.Append("TankLastDatetime= null ,");
            }

            if (model.TankLastFlowNo != null)
            {
                strSql.Append("TankLastFlowNo='" + model.TankLastFlowNo + "',");
            }
            else
            {
                strSql.Append("TankLastDatetime= null ,");
            }
            if (model.TankInterval != null)
            {
                strSql.Append("TankInterval='" + model.TankInterval + "',");
            }
            else
            {
                strSql.Append("TankInterval= null ,");
            }
            if (model.GunInterval != null)
            {
                strSql.Append("GunInterval='" + model.GunInterval + "',");
            }
            else
            {
                strSql.Append("GunInterval= null ,");
            }
            if (model.SaleDataInterval != null)
            {
                strSql.Append("SaleDataInterval='" + model.SaleDataInterval + "',");
            }
            else
            {
                strSql.Append("SaleDataInterval= null ,");
            }
            if (model.TankDataInterval != null)
            {
                strSql.Append("TankDataInterval='" + model.TankDataInterval + "',");
            }
            else
            {
                strSql.Append("TankDataInterval= null ,");
            }
            if (model.PurchaseInfoInterval != null)
            {
                strSql.Append("PurchaseInfoInterval='" + model.PurchaseInfoInterval + "',");
            }
            else
            {
                strSql.Append("PurchaseInfoInterval= null ,");
            }
            if (model.PurchaseInfoLastDatetime != null)
            {
                strSql.Append("PurchaseInfoLastDatetime='" + model.PurchaseInfoLastDatetime + "',");
            }
            else
            {
                strSql.Append("PurchaseInfoInterval= null ,");
            }
            
            int n = strSql.ToString().LastIndexOf(",");
			strSql.Remove(n, 1);
			strSql.Append(" where SysNo='"+ model.SysNo+"' ");
			int rowsAffected=DbHelperSQLite.ExecuteSql(strSql.ToString());
			if (rowsAffected > 0)
			{
				return true;
			}
			else
			{
				return false;
			}
		}

		/// <summary>
		/// 删除一条数据
		/// </summary>
		public bool Delete(string SysNo)
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("delete from SiteInfo ");
			strSql.Append(" where SysNo='"+SysNo+"' " );
			int rowsAffected=DbHelperSQLite.ExecuteSql(strSql.ToString());
			if (rowsAffected > 0)
			{
				return true;
			}
			else
			{
				return false;
			}
		}		/// <summary>
		/// 批量删除数据
		/// </summary>
		public bool DeleteList(string SysNolist )
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("delete from SiteInfo ");
			strSql.Append(" where SysNo in ("+SysNolist + ")  ");
			int rows=DbHelperSQLite.ExecuteSql(strSql.ToString());
			if (rows > 0)
			{
				return true;
			}
			else
			{
				return false;
			}
		}


		/// <summary>
		/// 得到一个对象实体
		/// </summary>
		public SPTool.Model.SiteInfo GetTopModel()
		{
            StringBuilder strSql=new StringBuilder();
			strSql.Append("select *  from SiteInfo limit 0,1  ");
			SPTool.Model.SiteInfo model=new SPTool.Model.SiteInfo();
			DataSet ds=DbHelperSQLite.Query(strSql.ToString());

			if(ds.Tables[0].Rows.Count>0)
			{
				return DataRowToModel(ds.Tables[0].Rows[0]);
			}
			else
			{
				return null;
			}
		}

		/// <summary>
		/// 得到一个对象实体
		/// </summary>
		public SPTool.Model.SiteInfo DataRowToModel(DataRow row)
		{
			SPTool.Model.SiteInfo model=new SPTool.Model.SiteInfo();
			if (row != null)
			{
				if(row["SysNo"]!=null)
				{
					model.SysNo=row["SysNo"].ToString();
				}
				if(row["GasCode"]!=null)
				{
					model.GasCode=row["GasCode"].ToString();
				}
                if (row["StationNo"] != null)
                {
                    model.StationNo = row["StationNo"].ToString();
                }
                if (row["StationName"] != null)
                {
                    model.StationName = row["StationName"].ToString();
                }
   
				if(row["isActived"]!=null && row["isActived"].ToString()!="")
				{
					model.isActived=int.Parse(row["isActived"].ToString());
				}
                if (row["ModelType"] != null && row["ModelType"].ToString() != "")
                {
                    model.ModelType = int.Parse(row["ModelType"].ToString());
                }
                if (row["PostUrl"] != null)
                {
                    model.PostUrl = row["PostUrl"].ToString();
                }
                if (row["ActivedDatetime"] != null && row["ActivedDatetime"].ToString() != "")
                {
                    model.ActivedDatetime = row["ActivedDatetime"].ToString();
                }
                if (row["SaleLastDatetime"] != null && row["SaleLastDatetime"].ToString() != "")
                {
                    model.SaleLastDatetime = row["SaleLastDatetime"].ToString();
                }
                if (row["TankLastDatetime"] != null && row["TankLastDatetime"].ToString() != "")
                {
                    model.TankLastDatetime = row["TankLastDatetime"].ToString();
                }

                if (row["TankLastFlowNo"] != null && row["TankLastFlowNo"].ToString() != "")
                {
                    model.TankLastFlowNo = row["TankLastFlowNo"].ToString();
                }
                if (row["TankInterval"] != null)
                {
                    model.TankInterval = row["TankInterval"].ToString();
                }
                if (row["GunInterval"] != null)
                {
                    model.GunInterval = row["GunInterval"].ToString();
                }
                if (row["SaleDataInterval"] != null)
                {
                    model.SaleDataInterval = row["SaleDataInterval"].ToString();
                }
                if (row["TankDataInterval"] != null)
                {
                    model.TankDataInterval = row["TankDataInterval"].ToString();
                }
                if (row["PurchaseInfoInterval"] != null)
                {
                    model.PurchaseInfoInterval = row["PurchaseInfoInterval"].ToString();
                }
                if (row["PurchaseInfoLastDatetime"] != null)
                {
                    model.PurchaseInfoLastDatetime = row["PurchaseInfoLastDatetime"].ToString();
                }
                
            }
			return model;
		}

		/// <summary>
		/// 获得数据列表
		/// </summary>
		public DataSet GetList(string strWhere)
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("select SysNo,GasCode,StationNo,StationName,isActived,PostUrl,ActivedDatetime,SaleLastDatetime,TankLastDatetime,PurchaseInfoLastDatetime,TankLastFlowNo,TankInterval,GunInterval,SaleDataInterval,TankDataInterval,PurchaseInfoInterval ");
			strSql.Append(" FROM SiteInfo ");
			if(strWhere.Trim()!="")
			{
				strSql.Append(" where "+strWhere);
			}
			return DbHelperSQLite.Query(strSql.ToString());
		}

		/// <summary>
		/// 获取记录总数
		/// </summary>
		public int GetRecordCount(string strWhere)
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("select count(1) FROM SiteInfo ");
			if(strWhere.Trim()!="")
			{
				strSql.Append(" where "+strWhere);
			}
			object obj = DbHelperSQLite.GetSingle(strSql.ToString());
			if (obj == null)
			{
				return 0;
			}
			else
			{
				return Convert.ToInt32(obj);
			}
		}
		/// <summary>
		/// 分页获取数据列表
		/// </summary>
		public DataSet GetListByPage(string strWhere, string orderby, int startIndex, int endIndex)
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("SELECT * FROM ( ");
			strSql.Append(" SELECT ROW_NUMBER() OVER (");
			if (!string.IsNullOrEmpty(orderby.Trim()))
			{
				strSql.Append("order by T." + orderby );
			}
			else
			{
				strSql.Append("order by T.SysNo desc");
			}
			strSql.Append(")AS Row, T.*  from SiteInfo T ");
			if (!string.IsNullOrEmpty(strWhere.Trim()))
			{
				strSql.Append(" WHERE " + strWhere);
			}
			strSql.Append(" ) TT");
			strSql.AppendFormat(" WHERE TT.Row between {0} and {1}", startIndex, endIndex);
			return DbHelperSQLite.Query(strSql.ToString());
		}

        /*
		*/

        #endregion  Method
        #region  MethodEx

        #endregion  MethodEx
    }
}

