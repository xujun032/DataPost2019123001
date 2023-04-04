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
using System.Text;
using System.Data.SQLite;
using DBUtility;//Please add references
using System.Collections.Generic;
using System.Collections;

namespace SPTool.DAL
{
	/// <summary>
	/// 数据访问类:TankConfigure
	/// </summary>
	public partial class TankConfigure
	{
		public TankConfigure()
		{}
		#region  Method


		/// <summary>
		/// 是否存在该记录
		/// </summary>
		public bool Exists(string TankNo)
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("select count(1) from TankConfigure");
			strSql.Append(" where TankNo='"+TankNo+"' ");
			return DbHelperSQLite.Exists(strSql.ToString());
		}

		/// <summary>
		/// 增加一条数据
		/// </summary>
		public bool Add(SPTool.Model.TankConfigure model)
		{
			StringBuilder strSql=new StringBuilder();
			StringBuilder strSql1=new StringBuilder();
			StringBuilder strSql2=new StringBuilder();
			if (model.TankNo != null)
			{
				strSql1.Append("TankNo,");
				strSql2.Append("'"+model.TankNo+"',");
			}

     
            if (model.TankName != null)
			{
				strSql1.Append("TankName,");
				strSql2.Append("'"+model.TankName+"',");
			}
			if (model.TankVolume != null)
			{
				strSql1.Append("TankVolume,");
				strSql2.Append(""+model.TankVolume+",");
			}
			if (model.OilCode != null)
			{
				strSql1.Append("OilCode,");
				strSql2.Append("'"+model.OilCode+"',");
			}
			if (model.OilName != null)
			{
				strSql1.Append("OilName,");
				strSql2.Append("'"+model.OilName+"',");
			}
			if (model.Remark1 != null)
			{
				strSql1.Append("Remark1,");
				strSql2.Append("'"+model.Remark1+"',");
			}
			if (model.Remark2 != null)
			{
				strSql1.Append("Remark2,");
				strSql2.Append("'"+model.Remark2+"',");
			}
            if (model.BarCode != null)
            {
                strSql1.Append("BarCode,");
                strSql2.Append("'" + model.BarCode + "',");
            }
            strSql.Append("insert into TankConfigure(");
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

        public bool AddList(List<SPTool.Model.TankConfigure> model1)
        {
            ArrayList SQLStringList = new ArrayList();
            foreach (var model in model1)
            {
                StringBuilder strSql = new StringBuilder();
                StringBuilder strSql1 = new StringBuilder();
                StringBuilder strSql2 = new StringBuilder();
                if (model.TankNo != null)
                {
                    strSql1.Append("TankNo,");
                    strSql2.Append("'" + model.TankNo + "',");
                }


                if (model.TankName != null)
                {
                    strSql1.Append("TankName,");
                    strSql2.Append("'" + model.TankName + "',");
                }
                if (model.TankVolume != null)
                {
                    strSql1.Append("TankVolume,");
                    strSql2.Append("" + model.TankVolume + ",");
                }
                if (model.OilCode != null)
                {
                    strSql1.Append("OilCode,");
                    strSql2.Append("'" + model.OilCode + "',");
                }
                if (model.OilName != null)
                {
                    strSql1.Append("OilName,");
                    strSql2.Append("'" + model.OilName + "',");
                }
                if (model.Remark1 != null)
                {
                    strSql1.Append("Remark1,");
                    strSql2.Append("'" + model.Remark1 + "',");
                }
                if (model.Remark2 != null)
                {
                    strSql1.Append("Remark2,");
                    strSql2.Append("'" + model.Remark2 + "',");
                }
                if (model.BarCode != null)
                {
                    strSql1.Append("BarCode,");
                    strSql2.Append("'" + model.BarCode + "',");
                }
                strSql.Append("insert into TankConfigure(");
                strSql.Append(strSql1.ToString().Remove(strSql1.Length - 1));
                strSql.Append(")");
                strSql.Append(" values (");
                strSql.Append(strSql2.ToString().Remove(strSql2.Length - 1));
                strSql.Append(")");
                SQLStringList.Add(strSql);
            }

          
           DbHelperSQLite.ExecuteSqlTran(SQLStringList);
            return true;
        }

        /// <summary>
        /// 更新一条数据
        /// </summary>
        public bool Update(SPTool.Model.TankConfigure model)
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("update TankConfigure set ");
			if (model.TankName != null)
			{
				strSql.Append("TankName='"+model.TankName+"',");
			}
			else
			{
				strSql.Append("TankName= null ,");
			}

            if (model.BarCode != null)
            {
                strSql.Append("BarCode='" + model.BarCode + "',");
            }
            else
            {
                strSql.Append("BarCode= null ,");
            }
            if (model.TankVolume != null)
			{
				strSql.Append("TankVolume="+model.TankVolume+",");
			}
			else
			{
				strSql.Append("TankVolume= null ,");
			}
			if (model.OilCode != null)
			{
				strSql.Append("OilCode='"+model.OilCode+"',");
			}
			else
			{
				strSql.Append("OilCode= null ,");
			}
			if (model.OilName != null)
			{
				strSql.Append("OilName='"+model.OilName+"',");
			}
			else
			{
				strSql.Append("OilName= null ,");
			}
			if (model.Remark1 != null)
			{
				strSql.Append("Remark1='"+model.Remark1+"',");
			}
			else
			{
				strSql.Append("Remark1= null ,");
			}
			if (model.Remark2 != null)
			{
				strSql.Append("Remark2='"+model.Remark2+"',");
			}
			else
			{
				strSql.Append("Remark2= null ,");
			}
			int n = strSql.ToString().LastIndexOf(",");
			strSql.Remove(n, 1);
			strSql.Append(" where TankNo='"+ model.TankNo+"' ");
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
		public bool Delete(string TankNo)
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("delete from TankConfigure ");
			strSql.Append(" where TankNo='"+TankNo+"' " );
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
		public bool DeleteList(string TankNolist )
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("delete from TankConfigure ");
			strSql.Append(" where TankNo in ("+TankNolist + ")  ");
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
		public SPTool.Model.TankConfigure GetModel(string TankNo)
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("select  ");
			strSql.Append(" TankNo,TankName,TankVolume,OilCode,BarCode,OilName,Remark1,Remark2 ");
			strSql.Append(" from TankConfigure ");
			strSql.Append(" where TankNo='"+TankNo+"' " );
			SPTool.Model.TankConfigure model=new SPTool.Model.TankConfigure();
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
		public SPTool.Model.TankConfigure DataRowToModel(DataRow row)
		{
			SPTool.Model.TankConfigure model=new SPTool.Model.TankConfigure();
			if (row != null)
			{
				if(row["TankNo"]!=null)
				{
					model.TankNo=row["TankNo"].ToString();
				}
                if (row["BarCode"] != null)
                {
                    model.BarCode = row["BarCode"].ToString();
                }
                if (row["TankName"]!=null)
				{
					model.TankName=row["TankName"].ToString();
				}
				if(row["TankVolume"]!=null && row["TankVolume"].ToString()!="")
				{
					model.TankVolume=decimal.Parse(row["TankVolume"].ToString());
				}
				if(row["OilCode"]!=null)
				{
					model.OilCode=row["OilCode"].ToString();
				}
				if(row["OilName"]!=null)
				{
					model.OilName=row["OilName"].ToString();
				}
				if(row["Remark1"]!=null)
				{
					model.Remark1=row["Remark1"].ToString();
				}
				if(row["Remark2"]!=null)
				{
					model.Remark2=row["Remark2"].ToString();
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
			strSql.Append("select TankNo,TankName,TankVolume,BarCode,OilCode,OilName,Remark1,Remark2 ");
			strSql.Append(" FROM TankConfigure ");
			if(strWhere.Trim()!="")
			{
				strSql.Append(" where "+strWhere);
			}
			return DbHelperSQLite.Query(strSql.ToString());
		}


        public DataSet GetList1()
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select TankNo,TankName,BarCode,OilCode,OilName,Remark1,Remark2 ");
            strSql.Append(" FROM TankConfigure ");
            return DbHelperSQLite.Query(strSql.ToString());
        }

        /// <summary>
        /// 获取记录总数
        /// </summary>
        public int GetRecordCount(string strWhere)
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("select count(1) FROM TankConfigure ");
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
				strSql.Append("order by T.TankNo desc");
			}
			strSql.Append(")AS Row, T.*  from TankConfigure T ");
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

