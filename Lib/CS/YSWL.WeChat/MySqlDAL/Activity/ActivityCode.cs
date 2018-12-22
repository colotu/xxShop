/**  版本信息模板在安装目录下，可自行修改。
* ActivityCode.cs
*
* 功 能： N/A
* 类 名： ActivityCode
*
* Ver    变更日期             负责人  变更内容
* ───────────────────────────────────
* V0.01  2013/12/25 19:04:16   N/A    初版
*
* Copyright (c) 2012 YSWL Corporation. All rights reserved.
*┌──────────────────────────────────┐
*│　此技术信息为本公司机密信息，未经本公司书面同意禁止向第三方披露．　│
*│　版权所有：云商未来（北京）科技有限公司　　　　　　　　　　　　　　│
*└──────────────────────────────────┘
*/
using System;
using System.Data;
using System.Text;
using System.Data.SqlClient;
using YSWL.WeChat.IDAL.Activity;
using YSWL.DBUtility;//Please add references
using MySql.Data.MySqlClient;
namespace YSWL.WeChat.MySqlDAL.Activity
{
	/// <summary>
	/// 数据访问类:ActivityCode
	/// </summary>
	public partial class ActivityCode:IActivityCode
	{
		public ActivityCode()
		{}
        #region  BasicMethod

        /// <summary>
        /// 是否存在该记录
        /// </summary>
        public bool Exists(string CodeName)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select count(1) from WeChat_ActivityCode");
            strSql.Append(" where CodeName=?CodeName ");
            MySqlParameter[] parameters = {
					new MySqlParameter("?CodeName", MySqlDbType.VarChar,50)			};
            parameters[0].Value = CodeName;

            return DbHelperMySQL.Exists(strSql.ToString(), parameters);
        }


        /// <summary>
        /// 增加一条数据
        /// </summary>
        public bool Add(YSWL.WeChat.Model.Activity.ActivityCode model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("insert into WeChat_ActivityCode(");
            strSql.Append("CodeName,ActivityId,AwardId,AwardName,ActivityName,ActivityPwd,UserId,UserName,Phone,Status,IsPwd,StartDate,EndDate,GenerateDate,UsedDate,Remark)");
            strSql.Append(" values (");
            strSql.Append("?CodeName,?ActivityId,?AwardId,?AwardName,?ActivityName,?ActivityPwd,?UserId,?UserName,?Phone,?Status,?IsPwd,?StartDate,?EndDate,?GenerateDate,?UsedDate,?Remark)");
            MySqlParameter[] parameters = {
					new MySqlParameter("?CodeName", MySqlDbType.VarChar,50),
					new MySqlParameter("?ActivityId", MySqlDbType.Int32,4),
					new MySqlParameter("?AwardId", MySqlDbType.Int32,4),
					new MySqlParameter("?AwardName", MySqlDbType.VarChar,200),
					new MySqlParameter("?ActivityName", MySqlDbType.VarChar,200),
					new MySqlParameter("?ActivityPwd", MySqlDbType.VarChar,200),
					new MySqlParameter("?UserId", MySqlDbType.VarChar,200),
					new MySqlParameter("?UserName", MySqlDbType.VarChar,200),
					new MySqlParameter("?Phone", MySqlDbType.VarChar,200),
					new MySqlParameter("?Status", MySqlDbType.Int32,4),
					new MySqlParameter("?IsPwd", MySqlDbType.Bit,1),
					new MySqlParameter("?StartDate", MySqlDbType.DateTime),
					new MySqlParameter("?EndDate", MySqlDbType.DateTime),
					new MySqlParameter("?GenerateDate", MySqlDbType.DateTime),
					new MySqlParameter("?UsedDate", MySqlDbType.DateTime),
					new MySqlParameter("?Remark", MySqlDbType.VarChar,-1)};
            parameters[0].Value = model.CodeName;
            parameters[1].Value = model.ActivityId;
            parameters[2].Value = model.AwardId;
            parameters[3].Value = model.AwardName;
            parameters[4].Value = model.ActivityName;
            parameters[5].Value = model.ActivityPwd;
            parameters[6].Value = model.UserId;
            parameters[7].Value = model.UserName;
            parameters[8].Value = model.Phone;
            parameters[9].Value = model.Status;
            parameters[10].Value = model.IsPwd;
            parameters[11].Value = model.StartDate;
            parameters[12].Value = model.EndDate;
            parameters[13].Value = model.GenerateDate;
            parameters[14].Value = model.UsedDate;
            parameters[15].Value = model.Remark;

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
        /// 更新一条数据
        /// </summary>
        public bool Update(YSWL.WeChat.Model.Activity.ActivityCode model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update WeChat_ActivityCode set ");
            strSql.Append("ActivityId=?ActivityId,");
            strSql.Append("AwardId=?AwardId,");
            strSql.Append("AwardName=?AwardName,");
            strSql.Append("ActivityName=?ActivityName,");
            strSql.Append("ActivityPwd=?ActivityPwd,");
            strSql.Append("UserId=?UserId,");
            strSql.Append("UserName=?UserName,");
            strSql.Append("Phone=?Phone,");
            strSql.Append("Status=?Status,");
            strSql.Append("IsPwd=?IsPwd,");
            strSql.Append("StartDate=?StartDate,");
            strSql.Append("EndDate=?EndDate,");
            strSql.Append("GenerateDate=?GenerateDate,");
            strSql.Append("UsedDate=?UsedDate,");
            strSql.Append("Remark=?Remark");
            strSql.Append(" where CodeName=?CodeName ");
            MySqlParameter[] parameters = {
					new MySqlParameter("?ActivityId", MySqlDbType.Int32,4),
					new MySqlParameter("?AwardId", MySqlDbType.Int32,4),
					new MySqlParameter("?AwardName", MySqlDbType.VarChar,200),
					new MySqlParameter("?ActivityName", MySqlDbType.VarChar,200),
					new MySqlParameter("?ActivityPwd", MySqlDbType.VarChar,200),
					new MySqlParameter("?UserId", MySqlDbType.VarChar,200),
					new MySqlParameter("?UserName", MySqlDbType.VarChar,200),
					new MySqlParameter("?Phone", MySqlDbType.VarChar,200),
					new MySqlParameter("?Status", MySqlDbType.Int32,4),
					new MySqlParameter("?IsPwd", MySqlDbType.Bit,1),
					new MySqlParameter("?StartDate", MySqlDbType.DateTime),
					new MySqlParameter("?EndDate", MySqlDbType.DateTime),
					new MySqlParameter("?GenerateDate", MySqlDbType.DateTime),
					new MySqlParameter("?UsedDate", MySqlDbType.DateTime),
					new MySqlParameter("?Remark", MySqlDbType.VarChar,-1),
					new MySqlParameter("?CodeName", MySqlDbType.VarChar,50)};
            parameters[0].Value = model.ActivityId;
            parameters[1].Value = model.AwardId;
            parameters[2].Value = model.AwardName;
            parameters[3].Value = model.ActivityName;
            parameters[4].Value = model.ActivityPwd;
            parameters[5].Value = model.UserId;
            parameters[6].Value = model.UserName;
            parameters[7].Value = model.Phone;
            parameters[8].Value = model.Status;
            parameters[9].Value = model.IsPwd;
            parameters[10].Value = model.StartDate;
            parameters[11].Value = model.EndDate;
            parameters[12].Value = model.GenerateDate;
            parameters[13].Value = model.UsedDate;
            parameters[14].Value = model.Remark;
            parameters[15].Value = model.CodeName;

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
        public bool Delete(string CodeName)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append("delete from WeChat_ActivityCode ");
            strSql.Append(" where CodeName=?CodeName ");
            MySqlParameter[] parameters = {
					new MySqlParameter("?CodeName", MySqlDbType.VarChar,50)			};
            parameters[0].Value = CodeName;

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
        public bool DeleteList(string CodeNamelist)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("delete from WeChat_ActivityCode ");
            strSql.Append(" where CodeName in (" + CodeNamelist + ")  ");
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
        public YSWL.WeChat.Model.Activity.ActivityCode GetModel(string CodeName)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append("select CodeName,ActivityId,AwardId,AwardName,ActivityName,ActivityPwd,UserId,UserName,Phone,Status,IsPwd,StartDate,EndDate,GenerateDate,UsedDate,Remark from WeChat_ActivityCode ");
            strSql.Append(" where CodeName=?CodeName LIMIT 1 ");
            MySqlParameter[] parameters = {
					new MySqlParameter("?CodeName", MySqlDbType.VarChar,50)			};
            parameters[0].Value = CodeName;

            YSWL.WeChat.Model.Activity.ActivityCode model = new YSWL.WeChat.Model.Activity.ActivityCode();
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
        public YSWL.WeChat.Model.Activity.ActivityCode DataRowToModel(DataRow row)
        {
            YSWL.WeChat.Model.Activity.ActivityCode model = new YSWL.WeChat.Model.Activity.ActivityCode();
            if (row != null)
            {
                if (row["CodeName"] != null)
                {
                    model.CodeName = row["CodeName"].ToString();
                }
                if (row["ActivityId"] != null && row["ActivityId"].ToString() != "")
                {
                    model.ActivityId = int.Parse(row["ActivityId"].ToString());
                }
                if (row["AwardId"] != null && row["AwardId"].ToString() != "")
                {
                    model.AwardId = int.Parse(row["AwardId"].ToString());
                }
                if (row["AwardName"] != null)
                {
                    model.AwardName = row["AwardName"].ToString();
                }
                if (row["ActivityName"] != null)
                {
                    model.ActivityName = row["ActivityName"].ToString();
                }
                if (row["ActivityPwd"] != null)
                {
                    model.ActivityPwd = row["ActivityPwd"].ToString();
                }
                if (row["UserId"] != null)
                {
                    model.UserId = row["UserId"].ToString();
                }
                if (row["UserName"] != null)
                {
                    model.UserName = row["UserName"].ToString();
                }
                if (row["Phone"] != null)
                {
                    model.Phone = row["Phone"].ToString();
                }
                if (row["Status"] != null && row["Status"].ToString() != "")
                {
                    model.Status = int.Parse(row["Status"].ToString());
                }
                if (row["IsPwd"] != null && row["IsPwd"].ToString() != "")
                {
                    if ((row["IsPwd"].ToString() == "1") || (row["IsPwd"].ToString().ToLower() == "true"))
                    {
                        model.IsPwd = true;
                    }
                    else
                    {
                        model.IsPwd = false;
                    }
                }
                if (row["StartDate"] != null && row["StartDate"].ToString() != "")
                {
                    model.StartDate = DateTime.Parse(row["StartDate"].ToString());
                }
                if (row["EndDate"] != null && row["EndDate"].ToString() != "")
                {
                    model.EndDate = DateTime.Parse(row["EndDate"].ToString());
                }
                if (row["GenerateDate"] != null && row["GenerateDate"].ToString() != "")
                {
                    model.GenerateDate = DateTime.Parse(row["GenerateDate"].ToString());
                }
                if (row["UsedDate"] != null && row["UsedDate"].ToString() != "")
                {
                    model.UsedDate = DateTime.Parse(row["UsedDate"].ToString());
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
            strSql.Append("select CodeName,ActivityId,AwardId,AwardName,ActivityName,ActivityPwd,UserId,UserName,Phone,Status,IsPwd,StartDate,EndDate,GenerateDate,UsedDate,Remark ");
            strSql.Append(" FROM WeChat_ActivityCode ");
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
            
            strSql.Append(" CodeName,ActivityId,AwardId,AwardName,ActivityName,ActivityPwd,UserId,UserName,Phone,Status,IsPwd,StartDate,EndDate,GenerateDate,UsedDate,Remark ");
            strSql.Append(" FROM WeChat_ActivityCode ");
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
            strSql.Append("select count(1) FROM WeChat_ActivityCode ");
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
            strSql.Append("SELECT T.* from WeChat_ActivityCode T ");
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
                strSql.Append(" order by T.CodeName desc");
            }
            strSql.AppendFormat(" LIMIT {0},{1}", startIndex - 1, endIndex - startIndex + 1);
            return DbHelperMySQL.Query(strSql.ToString());
        }




        #endregion  BasicMethod

		#region  ExtensionMethod
       public YSWL.WeChat.Model.Activity.ActivityCode GetRandCode(int activityId)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select *  from WeChat_ActivityCode ");
            strSql.Append(" where ActivityId=?ActivityId and Status=0 order  By RAND()  LIMIT 1 ");
            MySqlParameter[] parameters = {
					new MySqlParameter("?ActivityId", MySqlDbType.Int32)			};
            parameters[0].Value = activityId;
            YSWL.WeChat.Model.Activity.ActivityCode model = new YSWL.WeChat.Model.Activity.ActivityCode();
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

       public bool UpdateUser(string CodeName, string userId, string userName, int status, string phone, string remark)
       {
           StringBuilder strSql = new StringBuilder();
           strSql.Append("update WeChat_ActivityCode set ");
           strSql.Append("UserId=?UserId,UserName=?UserName, Status=?Status,");
           strSql.Append("Phone=?Phone,Remark=?Remark");
           strSql.Append(" where CodeName=?CodeName  ");
           MySqlParameter[] parameters = {
                    new MySqlParameter("?UserId", MySqlDbType.VarChar,200),
					new MySqlParameter("?UserName", MySqlDbType.VarChar,200),
					new MySqlParameter("?Phone", MySqlDbType.VarChar,200),
                    new MySqlParameter("?Status", MySqlDbType.Int32,4),
                    new MySqlParameter("?Remark", MySqlDbType.VarChar),
					new MySqlParameter("?CodeName", MySqlDbType.VarChar,200)
					};
           parameters[0].Value = userId;
           parameters[1].Value = userName;
           parameters[2].Value = phone;
           parameters[3].Value = status;
           parameters[4].Value = remark;
           parameters[5].Value = CodeName;

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
       public bool UpdateStatusList(string ids, int status)
       {
           StringBuilder strSql = new StringBuilder();
           strSql.Append("update WeChat_ActivityCode set ");
           strSql.Append("Status=" + status);
           strSql.Append(" where CodeName in (" + ids + ")   ");
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
		#endregion  ExtensionMethod
	}
}

