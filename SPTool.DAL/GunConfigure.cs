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
using System.Text;
using System.Data.SQLite;
using DBUtility;//Please add references
using System.Collections;
using System.Collections.Generic;

namespace SPTool.DAL
{
    /// <summary>
    /// 数据访问类:GunConfigure
    /// </summary>
    public partial class GunConfigure
    {
        public GunConfigure()
        { }
        #region  Method


        /// <summary>
        /// 是否存在该记录
        /// </summary>
        public bool Exists(string GunNo)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select count(1) from GunConfigure");
            strSql.Append(" where GunNo='" + GunNo + "' ");
            return DbHelperSQLite.Exists(strSql.ToString());
        }

        /// <summary>
        /// 增加一条数据
        /// </summary>
        public bool Add(SPTool.Model.GunConfigure model)
        {
            StringBuilder strSql = new StringBuilder();
            StringBuilder strSql1 = new StringBuilder();
            StringBuilder strSql2 = new StringBuilder();
            if (model.GunNo != null)
            {
                strSql1.Append("GunNo,");
                strSql2.Append("'" + model.GunNo + "',");
            }
            if (model.SysId != null)
            {
                strSql1.Append("SysId,");
                strSql2.Append("'" + model.SysId + "',");
            }
            if (model.TankNo != null)
            {
                strSql1.Append("TankNo,");
                strSql2.Append("'" + model.TankNo + "',");
            }
            if (model.PumpId != null)
            {
                strSql1.Append("PumpId,");
                strSql2.Append("'" + model.PumpId + "',");
            }
            if (model.HoseId != null)
            {
                strSql1.Append("HoseId,");
                strSql2.Append("'" + model.HoseId + "',");
            }
            strSql.Append("insert into GunConfigure(");
            strSql.Append(strSql1.ToString().Remove(strSql1.Length - 1));
            strSql.Append(")");
            strSql.Append(" values (");
            strSql.Append(strSql2.ToString().Remove(strSql2.Length - 1));
            strSql.Append(")");
            int rows = DbHelperSQLite.ExecuteSql(strSql.ToString());
            if (rows > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }



        public bool AddList(List<SPTool.Model.GunConfigure> model1)
        {
            ArrayList SQLStringList = new ArrayList();

            foreach (var model in model1)
            {
                StringBuilder strSql = new StringBuilder();
                StringBuilder strSql1 = new StringBuilder();
                StringBuilder strSql2 = new StringBuilder();
                if (model.GunNo != null)
                {
                    strSql1.Append("GunNo,");
                    strSql2.Append("'" + model.GunNo + "',");
                }
                if (model.SysId != null)
                {
                    strSql1.Append("SysId,");
                    strSql2.Append("'" + model.SysId + "',");
                }
                if (model.TankNo != null)
                {
                    strSql1.Append("TankNo,");
                    strSql2.Append("'" + model.TankNo + "',");
                }
                if (model.PumpId != null)
                {
                    strSql1.Append("PumpId,");
                    strSql2.Append("'" + model.PumpId + "',");
                }
                if (model.HoseId != null)
                {
                    strSql1.Append("HoseId,");
                    strSql2.Append("'" + model.HoseId + "',");
                }
                strSql.Append("insert into GunConfigure(");
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
        public bool Update(SPTool.Model.GunConfigure model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update GunConfigure set ");
            if (model.SysId != null)
            {
                strSql.Append("SysId='" + model.SysId + "',");
            }
            else
            {
                strSql.Append("SysId= null ,");
            }
            if (model.TankNo != null)
            {
                strSql.Append("TankNo='" + model.TankNo + "',");
            }
            else
            {
                strSql.Append("TankNo= null ,");
            }
            if (model.PumpId != null)
            {
                strSql.Append("PumpId='" + model.PumpId + "',");
            }
            else
            {
                strSql.Append("PumpId= null ,");
            }
            if (model.HoseId != null)
            {
                strSql.Append("HoseId='" + model.HoseId + "',");
            }
            else
            {
                strSql.Append("HoseId= null ,");
            }
            int n = strSql.ToString().LastIndexOf(",");
            strSql.Remove(n, 1);
            strSql.Append(" where GunNo='" + model.GunNo + "' ");
            int rowsAffected = DbHelperSQLite.ExecuteSql(strSql.ToString());
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
        public bool Delete(string GunNo)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("delete from GunConfigure ");
            strSql.Append(" where GunNo='" + GunNo + "' ");
            int rowsAffected = DbHelperSQLite.ExecuteSql(strSql.ToString());
            if (rowsAffected > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }       /// <summary>
                /// 批量删除数据
                /// </summary>
        public bool DeleteList(string GunNolist)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("delete from GunConfigure ");
            strSql.Append(" where GunNo in (" + GunNolist + ")  ");
            int rows = DbHelperSQLite.ExecuteSql(strSql.ToString());
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
        public SPTool.Model.GunConfigure GetModel(string GunNo)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select  ");
            strSql.Append(" GunNo,SysId,TankNo,PumpId,HoseId ");
            strSql.Append(" from GunConfigure ");
            strSql.Append(" where GunNo='" + GunNo + "' ");
            SPTool.Model.GunConfigure model = new SPTool.Model.GunConfigure();
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


        public SPTool.Model.GunConfigure GetModelByPumpHose(string pumpid, string hoseid)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select  ");
            strSql.Append(" GunNo,SysId,TankNo,PumpId,HoseId ");
            strSql.Append(" from GunConfigure ");
            strSql.Append(" where PumpId='" + pumpid + "' ");
            strSql.Append(" and HoseId='" + hoseid + "' ");
            SPTool.Model.GunConfigure model = new SPTool.Model.GunConfigure();
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
        public SPTool.Model.GunConfigure DataRowToModel(DataRow row)
        {
            SPTool.Model.GunConfigure model = new SPTool.Model.GunConfigure();
            if (row != null)
            {
                if (row["GunNo"] != null)
                {
                    model.GunNo = row["GunNo"].ToString();
                }
                if (row["SysId"] != null)
                {
                    model.SysId = row["SysId"].ToString();
                }
                if (row["TankNo"] != null)
                {
                    model.TankNo = row["TankNo"].ToString();
                }
                if (row["PumpId"] != null)
                {
                    model.PumpId = row["PumpId"].ToString();
                }
                if (row["HoseId"] != null)
                {
                    model.HoseId = row["HoseId"].ToString();
                }
            }
            return model;
        }

        /// <summary>
        /// 获得数据列表
        /// </summary>
        public DataSet GetList(string strWhere)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select GunNo,SysId,TankNo,PumpId,HoseId ");
            strSql.Append(" FROM GunConfigure ");
            if (strWhere.Trim() != "")
            {
                strSql.Append(" where " + strWhere);
            }
            return DbHelperSQLite.Query(strSql.ToString());
        }

        /// <summary>
        /// 获取记录总数
        /// </summary>
        public int GetRecordCount(string strWhere)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select count(1) FROM GunConfigure ");
            if (strWhere.Trim() != "")
            {
                strSql.Append(" where " + strWhere);
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
            StringBuilder strSql = new StringBuilder();
            strSql.Append("SELECT * FROM ( ");
            strSql.Append(" SELECT ROW_NUMBER() OVER (");
            if (!string.IsNullOrEmpty(orderby.Trim()))
            {
                strSql.Append("order by T." + orderby);
            }
            else
            {
                strSql.Append("order by T.GunNo desc");
            }
            strSql.Append(")AS Row, T.*  from GunConfigure T ");
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

