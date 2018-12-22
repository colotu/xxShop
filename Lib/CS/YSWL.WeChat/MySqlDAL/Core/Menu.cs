/**
* Menu.cs
*
* 功 能： N/A
* 类 名： Menu
*
* Ver    变更日期             负责人  变更内容
* ───────────────────────────────────
* V0.01  2013/9/17 12:25:28   N/A    初版
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
using MySql.Data.MySqlClient;
namespace YSWL.WeChat.MySqlDAL.Core
{
	/// <summary>
	/// 数据访问类:Menu
	/// </summary>
	public partial class Menu:IMenu
	{
		public Menu()
		{}
        #region  BasicMethod

        /// <summary>
        /// 得到最大ID
        /// </summary>
        public int GetMaxId()
        {
            return DbHelperMySQL.GetMaxID("MenuId", "WeChat_Menu");
        }

        /// <summary>
        /// 是否存在该记录
        /// </summary>
        public bool Exists(int MenuId)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select count(1) from WeChat_Menu");
            strSql.Append(" where MenuId=?MenuId");
            MySqlParameter[] parameters = {
					new MySqlParameter("?MenuId", MySqlDbType.Int32,4)
			};
            parameters[0].Value = MenuId;

            return DbHelperMySQL.Exists(strSql.ToString(), parameters);
        }


        /// <summary>
        /// 增加一条数据
        /// </summary>
        public int Add(YSWL.WeChat.Model.Core.Menu model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("insert into WeChat_Menu(");
            strSql.Append("OpenId,ParentId,Name,Type,Sequence,MenuKey,MenuUrl,Status,CreateDate,Remark,HasChildren)");
            strSql.Append(" values (");
            strSql.Append("?OpenId,?ParentId,?Name,?Type,?Sequence,?MenuKey,?MenuUrl,?Status,?CreateDate,?Remark,?HasChildren)");
            strSql.Append(";select last_insert_id()");
            MySqlParameter[] parameters = {
					new MySqlParameter("?OpenId", MySqlDbType.VarChar,200),
					new MySqlParameter("?ParentId", MySqlDbType.Int32,4),
					new MySqlParameter("?Name", MySqlDbType.VarChar,200),
					new MySqlParameter("?Type", MySqlDbType.VarChar,50),
					new MySqlParameter("?Sequence", MySqlDbType.Int32,4),
					new MySqlParameter("?MenuKey", MySqlDbType.VarChar,50),
					new MySqlParameter("?MenuUrl", MySqlDbType.VarChar,300),
					new MySqlParameter("?Status", MySqlDbType.Int32,4),
					new MySqlParameter("?CreateDate", MySqlDbType.DateTime),
					new MySqlParameter("?Remark", MySqlDbType.VarChar,-1),
					new MySqlParameter("?HasChildren", MySqlDbType.Bit,1)};
            parameters[0].Value = model.OpenId;
            parameters[1].Value = model.ParentId;
            parameters[2].Value = model.Name;
            parameters[3].Value = model.Type;
            parameters[4].Value = model.Sequence;
            parameters[5].Value = model.MenuKey;
            parameters[6].Value = model.MenuUrl;
            parameters[7].Value = model.Status;
            parameters[8].Value = model.CreateDate;
            parameters[9].Value = model.Remark;
            parameters[10].Value = model.HasChildren;

            object obj = DbHelperMySQL.GetSingle(strSql.ToString(), parameters);
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
        public bool Update(YSWL.WeChat.Model.Core.Menu model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update WeChat_Menu set ");
            strSql.Append("OpenId=?OpenId,");
            strSql.Append("ParentId=?ParentId,");
            strSql.Append("Name=?Name,");
            strSql.Append("Type=?Type,");
            strSql.Append("Sequence=?Sequence,");
            strSql.Append("MenuKey=?MenuKey,");
            strSql.Append("MenuUrl=?MenuUrl,");
            strSql.Append("Status=?Status,");
            strSql.Append("CreateDate=?CreateDate,");
            strSql.Append("Remark=?Remark,");
            strSql.Append("HasChildren=?HasChildren");
            strSql.Append(" where MenuId=?MenuId");
            MySqlParameter[] parameters = {
					new MySqlParameter("?OpenId", MySqlDbType.VarChar,200),
					new MySqlParameter("?ParentId", MySqlDbType.Int32,4),
					new MySqlParameter("?Name", MySqlDbType.VarChar,200),
					new MySqlParameter("?Type", MySqlDbType.VarChar,50),
					new MySqlParameter("?Sequence", MySqlDbType.Int32,4),
					new MySqlParameter("?MenuKey", MySqlDbType.VarChar,50),
					new MySqlParameter("?MenuUrl", MySqlDbType.VarChar,300),
					new MySqlParameter("?Status", MySqlDbType.Int32,4),
					new MySqlParameter("?CreateDate", MySqlDbType.DateTime),
					new MySqlParameter("?Remark", MySqlDbType.VarChar,-1),
					new MySqlParameter("?HasChildren", MySqlDbType.Bit,1),
					new MySqlParameter("?MenuId", MySqlDbType.Int32,4)};
            parameters[0].Value = model.OpenId;
            parameters[1].Value = model.ParentId;
            parameters[2].Value = model.Name;
            parameters[3].Value = model.Type;
            parameters[4].Value = model.Sequence;
            parameters[5].Value = model.MenuKey;
            parameters[6].Value = model.MenuUrl;
            parameters[7].Value = model.Status;
            parameters[8].Value = model.CreateDate;
            parameters[9].Value = model.Remark;
            parameters[10].Value = model.HasChildren;
            parameters[11].Value = model.MenuId;

            int rows = DbHelperMySQL.ExecuteSql(strSql.ToString(), parameters);
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
        public bool Delete(int MenuId)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append("delete from WeChat_Menu ");
            strSql.Append(" where MenuId=?MenuId");
            MySqlParameter[] parameters = {
					new MySqlParameter("?MenuId", MySqlDbType.Int32,4)
			};
            parameters[0].Value = MenuId;

            int rows = DbHelperMySQL.ExecuteSql(strSql.ToString(), parameters);
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
        public bool DeleteList(string MenuIdlist)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("delete from WeChat_Menu ");
            strSql.Append(" where MenuId in (" + MenuIdlist + ")  ");
            int rows = DbHelperMySQL.ExecuteSql(strSql.ToString());
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
        public YSWL.WeChat.Model.Core.Menu GetModel(int MenuId)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append("select MenuId,OpenId,ParentId,Name,Type,Sequence,MenuKey,MenuUrl,Status,CreateDate,Remark,HasChildren from WeChat_Menu ");
            strSql.Append(" where MenuId=?MenuId LIMIT 1 ");
            MySqlParameter[] parameters = {
					new MySqlParameter("?MenuId", MySqlDbType.Int32,4)
			};
            parameters[0].Value = MenuId;

            YSWL.WeChat.Model.Core.Menu model = new YSWL.WeChat.Model.Core.Menu();
            DataSet ds = DbHelperMySQL.Query(strSql.ToString(), parameters);
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
        public YSWL.WeChat.Model.Core.Menu DataRowToModel(DataRow row)
        {
            YSWL.WeChat.Model.Core.Menu model = new YSWL.WeChat.Model.Core.Menu();
            if (row != null)
            {
                if (row["MenuId"] != null && row["MenuId"].ToString() != "")
                {
                    model.MenuId = int.Parse(row["MenuId"].ToString());
                }
                if (row["OpenId"] != null)
                {
                    model.OpenId = row["OpenId"].ToString();
                }
                if (row["ParentId"] != null && row["ParentId"].ToString() != "")
                {
                    model.ParentId = int.Parse(row["ParentId"].ToString());
                }
                if (row["Name"] != null)
                {
                    model.Name = row["Name"].ToString();
                }
                if (row["Type"] != null)
                {
                    model.Type = row["Type"].ToString();
                }
                if (row["Sequence"] != null && row["Sequence"].ToString() != "")
                {
                    model.Sequence = int.Parse(row["Sequence"].ToString());
                }
                if (row["MenuKey"] != null)
                {
                    model.MenuKey = row["MenuKey"].ToString();
                }
                if (row["MenuUrl"] != null)
                {
                    model.MenuUrl = row["MenuUrl"].ToString();
                }
                if (row["Status"] != null && row["Status"].ToString() != "")
                {
                    model.Status = int.Parse(row["Status"].ToString());
                }
                if (row["CreateDate"] != null && row["CreateDate"].ToString() != "")
                {
                    model.CreateDate = DateTime.Parse(row["CreateDate"].ToString());
                }
                if (row["Remark"] != null)
                {
                    model.Remark = row["Remark"].ToString();
                }
                if (row["HasChildren"] != null && row["HasChildren"].ToString() != "")
                {
                    if ((row["HasChildren"].ToString() == "1") || (row["HasChildren"].ToString().ToLower() == "true"))
                    {
                        model.HasChildren = true;
                    }
                    else
                    {
                        model.HasChildren = false;
                    }
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
            strSql.Append("select MenuId,OpenId,ParentId,Name,Type,Sequence,MenuKey,MenuUrl,Status,CreateDate,Remark,HasChildren ");
            strSql.Append(" FROM WeChat_Menu ");
            if (strWhere.Trim() != "")
            {
                strSql.Append(" where " + strWhere);
            }
            return DbHelperMySQL.Query(strSql.ToString());
        }

        /// <summary>
        /// 获得前几行数据
        /// </summary>
        public DataSet GetList(int Top, string strWhere, string filedOrder)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select ");
            
            strSql.Append(" MenuId,OpenId,ParentId,Name,Type,Sequence,MenuKey,MenuUrl,Status,CreateDate,Remark,HasChildren ");
            strSql.Append(" FROM WeChat_Menu ");
            if (strWhere.Trim() != "")
            {
                strSql.Append(" where " + strWhere);
            }
            strSql.Append(" order by " + filedOrder);
            if (Top > 0)
            {
                strSql.Append(" LIMIT " + Top.ToString());
            }
            return DbHelperMySQL.Query(strSql.ToString());
        }

        /// <summary>
        /// 获取记录总数
        /// </summary>
        public int GetRecordCount(string strWhere)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select count(1) FROM WeChat_Menu ");
            if (strWhere.Trim() != "")
            {
                strSql.Append(" where " + strWhere);
            }
            object obj = DbHelperMySQL.GetSingle(strSql.ToString());
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
            strSql.Append("SELECT T.* from WeChat_Menu T ");
            if (!string.IsNullOrWhiteSpace(strWhere.Trim()))
            {
                strSql.Append(" WHERE " + strWhere);
            }
            if (!string.IsNullOrWhiteSpace(orderby.Trim()))
            {
                strSql.Append(" order by T." + orderby);
            }
            else
            {
                strSql.Append(" order by T.MenuId desc");
            }
            strSql.AppendFormat(" LIMIT {0},{1}", startIndex - 1, endIndex - startIndex + 1);
            return DbHelperMySQL.Query(strSql.ToString());
        }



        #endregion  BasicMethod
		#region  ExtensionMethod

	    public bool UpdateSeq(int seq, int menuId)
	    {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update WeChat_Menu set ");
            strSql.Append("Sequence=?Sequence");
            strSql.Append(" where MenuId=?MenuId");
            MySqlParameter[] parameters = {
					new MySqlParameter("?Sequence", MySqlDbType.Int32,4),
					new MySqlParameter("?MenuId", MySqlDbType.Int32,4)};
            parameters[0].Value = seq;
            parameters[1].Value = menuId;

            int rows = DbHelperMySQL.ExecuteSql(strSql.ToString(), parameters);
            if (rows > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
	    }

        public int GetSequence(string openId)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append(" SELECT MAX(Sequence) FROM WeChat_Menu where OpenId=?OpenId");
            MySqlParameter[] parameters = {
						new MySqlParameter("?OpenId", MySqlDbType.VarChar,200)
                                        };
            parameters[0].Value = openId;
            object obj = DbHelperMySQL.GetSingle(strSql.ToString(), parameters);
            if (obj == null)
            {
                return 0;
            }
            else
            {
                return Convert.ToInt32(obj);
            }
        }

        public bool AddEx(YSWL.WeChat.Model.Core.Menu model)
	    {

            // 事件添加
            List<CommandInfo> sqllist = new List<CommandInfo>();

            StringBuilder strSql = new StringBuilder();
            strSql.Append("insert into WeChat_Menu(");
            strSql.Append("OpenId,ParentId,Name,Type,Sequence,MenuKey,MenuUrl,Status,CreateDate,Remark,HasChildren)");
            strSql.Append(" values (");
            strSql.Append("?OpenId,?ParentId,?Name,?Type,?Sequence,?MenuKey,?MenuUrl,?Status,?CreateDate,?Remark,?HasChildren)");
            MySqlParameter[] parameters = {
					new MySqlParameter("?OpenId", MySqlDbType.VarChar,200),
					new MySqlParameter("?ParentId", MySqlDbType.Int32,4),
					new MySqlParameter("?Name", MySqlDbType.VarChar,200),
					new MySqlParameter("?Type", MySqlDbType.VarChar,50),
					new MySqlParameter("?Sequence", MySqlDbType.Int32,4),
					new MySqlParameter("?MenuKey", MySqlDbType.VarChar,50),
					new MySqlParameter("?MenuUrl", MySqlDbType.VarChar,300),
					new MySqlParameter("?Status", MySqlDbType.Int32,4),
					new MySqlParameter("?CreateDate", MySqlDbType.DateTime),
					new MySqlParameter("?Remark", MySqlDbType.VarChar,-1),
					new MySqlParameter("?HasChildren", MySqlDbType.Bit,1)};
            parameters[0].Value = model.OpenId;
            parameters[1].Value = model.ParentId;
            parameters[2].Value = model.Name;
            parameters[3].Value = model.Type;
            parameters[4].Value = model.Sequence;
            parameters[5].Value = model.MenuKey;
            parameters[6].Value = model.MenuUrl;
            parameters[7].Value = model.Status;
            parameters[8].Value = model.CreateDate;
            parameters[9].Value = model.Remark;
            parameters[10].Value = model.HasChildren;

            CommandInfo cmd = new CommandInfo(strSql.ToString(), parameters);
            sqllist.Add(cmd);

            //更新父级
            if (model.ParentId > 0)
            {
                StringBuilder strSql1 = new StringBuilder();
                strSql1.Append("update WeChat_Menu set ");
                strSql1.Append("HasChildren=?HasChildren");
                strSql1.Append(" where MenuId=?MenuId");
                MySqlParameter[] parameters1 = {
					new MySqlParameter("?HasChildren", MySqlDbType.Bit,1),
					new MySqlParameter("?MenuId", MySqlDbType.Int32,4)};
                parameters1[0].Value = true;
                parameters1[1].Value = model.ParentId;
                CommandInfo cmd1 = new CommandInfo(strSql1.ToString(), parameters1);
                sqllist.Add(cmd1);
            }
            return DbHelperMySQL.ExecuteSqlTran(sqllist) > 0 ? true : false;
      
	    }

	    public bool DeleteEx(int menuId)
	    {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("delete from WeChat_Menu ");
            strSql.Append(" where MenuId=?MenuId or ParentId=?MenuId");
            MySqlParameter[] parameters = {
					new MySqlParameter("?MenuId", MySqlDbType.Int32,4)
			};
            parameters[0].Value = menuId;

            int rows = DbHelperMySQL.ExecuteSql(strSql.ToString(), parameters);
            if (rows > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
	    }

      public  YSWL.WeChat.Model.Core.Menu GetMenu(string key)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select * from WeChat_Menu ");
            strSql.Append(" where MenuKey=?MenuKey and Type='click' LIMIT 1 ");
            MySqlParameter[] parameters = {
					new MySqlParameter("?MenuKey", MySqlDbType.VarChar,50)
			};
            parameters[0].Value = key;

            YSWL.WeChat.Model.Core.Menu model = new YSWL.WeChat.Model.Core.Menu();
            DataSet ds = DbHelperMySQL.Query(strSql.ToString(), parameters);
            if (ds.Tables[0].Rows.Count > 0)
            {
                return DataRowToModel(ds.Tables[0].Rows[0]);
            }
            else
            {
                return null;
            }
        }
	    #endregion  ExtensionMethod
	}
}

