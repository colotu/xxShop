/**
* KeyRule.cs
*
* 功 能： N/A
* 类 名： KeyRule
*
* Ver    变更日期             负责人  变更内容
* ───────────────────────────────────
* V0.01  2013/7/29 15:35:25   N/A    初版
*
* Copyright (c) 2012-2013 YSWL Corporation. All rights reserved.
*┌──────────────────────────────────┐
*│　此技术信息为本公司机密信息，未经本公司书面同意禁止向第三方披露．　│
*│　版权所有：云商未来（北京）科技有限公司　　　　　　　　　　　　　　│
*└──────────────────────────────────┘
*/
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Data.SqlClient;
using YSWL.WeChat.IDAL.Core;
using YSWL.DBUtility;//Please add references
namespace YSWL.WeChat.SQLServerDAL.Core
{
	/// <summary>
	/// 数据访问类:KeyRule
	/// </summary>
	public partial class KeyRule:IKeyRule
	{
		public KeyRule()
		{}
        #region  BasicMethod

        /// <summary>
        /// 得到最大ID
        /// </summary>
        public int GetMaxId()
        {
            return DBHelper.DefaultDBHelper.GetMaxID("RuleId", "WeChat_KeyRule");
        }

        /// <summary>
        /// 是否存在该记录
        /// </summary>
        public bool Exists(int RuleId)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select count(1) from WeChat_KeyRule");
            strSql.Append(" where RuleId=@RuleId");
            SqlParameter[] parameters = {
					new SqlParameter("@RuleId", SqlDbType.Int,4)
			};
            parameters[0].Value = RuleId;

            return DBHelper.DefaultDBHelper.Exists(strSql.ToString(), parameters);
        }


        /// <summary>
        /// 增加一条数据
        /// </summary>
        public int Add(YSWL.WeChat.Model.Core.KeyRule model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("insert into WeChat_KeyRule(");
            strSql.Append("OpenId,Name,Remark)");
            strSql.Append(" values (");
            strSql.Append("@OpenId,@Name,@Remark)");
            strSql.Append(";select @@IDENTITY");
            SqlParameter[] parameters = {
					new SqlParameter("@OpenId", SqlDbType.NVarChar,200),
					new SqlParameter("@Name", SqlDbType.NVarChar,200),
					new SqlParameter("@Remark", SqlDbType.NVarChar,-1)};
            parameters[0].Value = model.OpenId;
            parameters[1].Value = model.Name;
            parameters[2].Value = model.Remark;

            object obj = DBHelper.DefaultDBHelper.GetSingle(strSql.ToString(), parameters);
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
        /// 更新一条数据
        /// </summary>
        public bool Update(YSWL.WeChat.Model.Core.KeyRule model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update WeChat_KeyRule set ");
            strSql.Append("OpenId=@OpenId,");
            strSql.Append("Name=@Name,");
            strSql.Append("Remark=@Remark");
            strSql.Append(" where RuleId=@RuleId");
            SqlParameter[] parameters = {
					new SqlParameter("@OpenId", SqlDbType.NVarChar,200),
					new SqlParameter("@Name", SqlDbType.NVarChar,200),
					new SqlParameter("@Remark", SqlDbType.NVarChar,-1),
					new SqlParameter("@RuleId", SqlDbType.Int,4)};
            parameters[0].Value = model.OpenId;
            parameters[1].Value = model.Name;
            parameters[2].Value = model.Remark;
            parameters[3].Value = model.RuleId;

            int rows = DBHelper.DefaultDBHelper.ExecuteSql(strSql.ToString(), parameters);
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
        /// 删除一条数据
        /// </summary>
        public bool Delete(int RuleId)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append("delete from WeChat_KeyRule ");
            strSql.Append(" where RuleId=@RuleId");
            SqlParameter[] parameters = {
					new SqlParameter("@RuleId", SqlDbType.Int,4)
			};
            parameters[0].Value = RuleId;

            int rows = DBHelper.DefaultDBHelper.ExecuteSql(strSql.ToString(), parameters);
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
        /// 批量删除数据
        /// </summary>
        public bool DeleteList(string RuleIdlist)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("delete from WeChat_KeyRule ");
            strSql.Append(" where RuleId in (" + RuleIdlist + ")  ");
            int rows = DBHelper.DefaultDBHelper.ExecuteSql(strSql.ToString());
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
        public YSWL.WeChat.Model.Core.KeyRule GetModel(int RuleId)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append("select  top 1 RuleId,OpenId,Name,Remark from WeChat_KeyRule ");
            strSql.Append(" where RuleId=@RuleId");
            SqlParameter[] parameters = {
					new SqlParameter("@RuleId", SqlDbType.Int,4)
			};
            parameters[0].Value = RuleId;

            YSWL.WeChat.Model.Core.KeyRule model = new YSWL.WeChat.Model.Core.KeyRule();
            DataSet ds = DBHelper.DefaultDBHelper.Query(strSql.ToString(), parameters);
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
        public YSWL.WeChat.Model.Core.KeyRule DataRowToModel(DataRow row)
        {
            YSWL.WeChat.Model.Core.KeyRule model = new YSWL.WeChat.Model.Core.KeyRule();
            if (row != null)
            {
                if (row["RuleId"] != null && row["RuleId"].ToString() != "")
                {
                    model.RuleId = int.Parse(row["RuleId"].ToString());
                }
                if (row["OpenId"] != null)
                {
                    model.OpenId = row["OpenId"].ToString();
                }
                if (row["Name"] != null)
                {
                    model.Name = row["Name"].ToString();
                }
                if (row["Remark"] != null)
                {
                    model.Remark = row["Remark"].ToString();
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
            strSql.Append("select RuleId,OpenId,Name,Remark ");
            strSql.Append(" FROM WeChat_KeyRule ");
            if (strWhere.Trim() != "")
            {
                strSql.Append(" where " + strWhere);
            }
            return DBHelper.DefaultDBHelper.Query(strSql.ToString());
        }

        /// <summary>
        /// 获得前几行数据
        /// </summary>
        public DataSet GetList(int Top, string strWhere, string filedOrder)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select ");
            if (Top > 0)
            {
                strSql.Append(" top " + Top.ToString());
            }
            strSql.Append(" RuleId,OpenId,Name,Remark ");
            strSql.Append(" FROM WeChat_KeyRule ");
            if (strWhere.Trim() != "")
            {
                strSql.Append(" where " + strWhere);
            }
            strSql.Append(" order by " + filedOrder);
            return DBHelper.DefaultDBHelper.Query(strSql.ToString());
        }

        /// <summary>
        /// 获取记录总数
        /// </summary>
        public int GetRecordCount(string strWhere)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select count(1) FROM WeChat_KeyRule ");
            if (strWhere.Trim() != "")
            {
                strSql.Append(" where " + strWhere);
            }
            object obj = DBHelper.DefaultDBHelper.GetSingle(strSql.ToString());
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
                strSql.Append("order by T.RuleId desc");
            }
            strSql.Append(")AS Row, T.*  from WeChat_KeyRule T ");
            if (!string.IsNullOrEmpty(strWhere.Trim()))
            {
                strSql.Append(" WHERE " + strWhere);
            }
            strSql.Append(" ) TT");
            strSql.AppendFormat(" WHERE TT.Row between {0} and {1}", startIndex, endIndex);
            return DBHelper.DefaultDBHelper.Query(strSql.ToString());
        }

 

        #endregion  BasicMethod
		#region  ExtensionMethod
        /// <summary>
        /// 批量删除数据(级联删除)
        /// </summary>
        public bool DeleteListEx(string RuleIdlist)
        {

            List<CommandInfo> sqllist = new List<CommandInfo>();
            StringBuilder strSql = new StringBuilder();
            strSql.Append("delete from WeChat_KeyRule ");
            strSql.Append(" where RuleId in (" + RuleIdlist + ")  ");
            CommandInfo cmd = new CommandInfo(strSql.ToString(), null);
            sqllist.Add(cmd);

            //删除关键字信息
            StringBuilder strSql1 = new StringBuilder();
            strSql1.Append("delete from WeChat_KeyValue ");
            strSql1.Append(" where RuleId in (" + RuleIdlist + ")  ");
            CommandInfo cmd1 = new CommandInfo(strSql1.ToString(), null);
            sqllist.Add(cmd1);
            //删除回复消息
            StringBuilder strSql2 = new StringBuilder();
            strSql2.Append("delete from WeChat_PostMsg ");
            strSql2.Append(" where RuleId in (" + RuleIdlist + ")  ");
            CommandInfo cmd2 = new CommandInfo(strSql2.ToString(), null);
            sqllist.Add(cmd2);

            return DBHelper.DefaultDBHelper.ExecuteSqlTran(sqllist) > 0 ? true : false;
        }
		#endregion  ExtensionMethod
	}
}

