/**  版本信息模板在安装目录下，可自行修改。
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
using System.Data;
using System.Text;
using System.Data.SQLite;
using DBUtility;//Please add references
using System.IO;

namespace SPTool.DAL
{
	/// <summary>
	/// 数据访问类:FuelItemConfig
	/// </summary>
	public partial class FuelItemConfig
	{
		public FuelItemConfig()
		{}
		#region  Method


		/// <summary>
		/// 是否存在该记录
		/// </summary>
		public bool Exists(string OilCode)
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("select count(1) from FuelItemConfig");
			strSql.Append(" where OilCode='"+OilCode+"' ");
			return DbHelperSQLite.Exists(strSql.ToString());
		}

		/// <summary>
		/// 增加一条数据
		/// </summary>
		public bool Add(SPTool.Model.FuelItemConfig model)
		{
			StringBuilder strSql=new StringBuilder();
			StringBuilder strSql1=new StringBuilder();
			StringBuilder strSql2=new StringBuilder();
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
			if (model.BarCode != null)
			{
				strSql1.Append("BarCode,");
				strSql2.Append("'"+model.BarCode+"',");
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
			strSql.Append("insert into FuelItemConfig(");
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
		public bool Update(SPTool.Model.FuelItemConfig model)
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("update FuelItemConfig set ");
			if (model.OilName != null)
			{
				strSql.Append("OilName='"+model.OilName+"',");
			}
			else
			{
				strSql.Append("OilName= null ,");
			}
			if (model.BarCode != null)
			{
				strSql.Append("BarCode='"+model.BarCode+"',");
			}
			else
			{
				strSql.Append("BarCode= null ,");
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
			strSql.Append(" where OilCode='"+ model.OilCode+"' ");
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
		public bool Delete(string OilCode)
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("delete from FuelItemConfig ");
			strSql.Append(" where OilCode='"+OilCode+"' " );
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
		public bool DeleteList(string OilCodelist )
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("delete from FuelItemConfig ");
			strSql.Append(" where OilCode in ("+OilCodelist + ")  ");
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
		public SPTool.Model.FuelItemConfig GetModel(string OilCode)
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("select  ");
			strSql.Append(" OilCode,OilName,BarCode,Remark1,Remark2 ");
			strSql.Append(" from FuelItemConfig ");
			strSql.Append(" where OilCode='"+OilCode+"' " );
			SPTool.Model.FuelItemConfig model=new SPTool.Model.FuelItemConfig();
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
        public SPTool.Model.FuelItemConfig GetModelByBarcode(string BarCode)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select  ");
            strSql.Append(" OilCode,OilName,BarCode,Remark1,Remark2 ");
            strSql.Append(" from FuelItemConfig ");
           string t1=string.Format(" where BarCode={0}",BarCode);
            strSql.Append(t1);
            SPTool.Model.FuelItemConfig model = new SPTool.Model.FuelItemConfig();
            DataSet ds = DbHelperSQLite.Query(strSql.ToString());
            if (ds.Tables[0].Rows.Count > 0)
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
        public SPTool.Model.FuelItemConfig DataRowToModel(DataRow row)
		{
			SPTool.Model.FuelItemConfig model=new SPTool.Model.FuelItemConfig();
			if (row != null)
			{
				if(row["OilCode"]!=null)
				{
					model.OilCode=row["OilCode"].ToString();
				}
				if(row["OilName"]!=null)
				{
					model.OilName=row["OilName"].ToString();
				}
				if(row["BarCode"]!=null)
				{
					model.BarCode=row["BarCode"].ToString();
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
			strSql.Append("select OilCode,OilName,BarCode,Remark1,Remark2 ");
			strSql.Append(" FROM FuelItemConfig ");
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
			strSql.Append("select count(1) FROM FuelItemConfig ");
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
				strSql.Append("order by T.OilCode desc");
			}
			strSql.Append(")AS Row, T.*  from FuelItemConfig T ");
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


        public static void WriteLog(string Text)
        {
            string path = System.Windows.Forms.Application.StartupPath + "\\" + "log";
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
            string Path = path + "\\" + DateTime.Now.ToString("yyyyMMdd") + ".log";
            WriteLogFile(Path, Text);
        }
        public static bool WriteLogFile(string FileName, string message)
        {
            string logText = string.Format("{0} .........  {1}", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss:ffff"), message);
            if (FileName == null || FileName == "")
                return false;

            if (message == null)
                message = "null";

            try
            {
                if (!File.Exists(FileName))
                {
                    ///创建日志文件
                    StreamWriter sr = File.CreateText(FileName);
                    sr.Close();
                }
                System.IO.FileInfo fileInfo = new System.IO.FileInfo(FileName);
                if (fileInfo.Length / (1024 * 1024) > 2)//fa.File.Exists(FileName))
                {
                    string newfilename = DateTime.Now.ToString().Replace(":", "_");
                    newfilename = FileName.Replace(".log", "") + "_" + DateTime.Now.ToString("yyyyMMddHHmmssffff") + fileInfo.Extension;
                    fileInfo.MoveTo(newfilename);
                    ///创建日志文件
                    StreamWriter sr = File.CreateText(FileName);
                    //sr.WriteLine(DateTime.Now.ToString() + "   " + message);
                    sr.WriteLine(logText);
                    sr.Close();
                }
                else
                {
                    ///如果日志文件已经存在，则直接写入日志文件
                    StreamWriter sr = File.AppendText(FileName);
                    // sr.WriteLine("[" + DateTime.Now.ToString() + "] " + message);
                    sr.WriteLine(logText);
                    sr.Close();
                }
                return true;
            }
            catch
            {

            }
            return false;
        }
        #endregion  MethodEx
    }
}

