﻿/**
* User.cs
*
* 功 能： N/A
* 类 名： User
*
* Ver    变更日期             负责人  变更内容
* ───────────────────────────────────
* V0.01  2013/7/22 17:43:22   N/A    初版
*
* Copyright (c) 2012-2013 YSWL Corporation. All rights reserved.
*┌──────────────────────────────────┐
*│　此技术信息为本公司机密信息，未经本公司书面同意禁止向第三方披露．　│
*│　版权所有：云商未来（北京）科技有限公司　　　　　　　　　　　　　　│
*└──────────────────────────────────┘
*/
using System;
using System.Data;
using System.Text;
using System.Data.SqlClient;
using YSWL.WeChat.IDAL.Core;
using YSWL.DBUtility;
using MySql.Data.MySqlClient;
namespace YSWL.WeChat.MySqlDAL.Core
{
	/// <summary>
	/// 数据访问类:User
	/// </summary>
	public partial class User:IUser
	{
		public User()
		{}
        #region  BasicMethod

        /// <summary>
        /// 得到最大ID
        /// </summary>
        public int GetMaxId()
        {
            return DbHelperMySQL.GetMaxID("ID", "WeChat_User");
        }

        /// <summary>
        /// 是否存在该记录
        /// </summary>
        public bool Exists(int ID)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select count(1) from WeChat_User");
            strSql.Append(" where ID=?ID");
            MySqlParameter[] parameters = {
					new MySqlParameter("?ID", MySqlDbType.Int32,4)
			};
            parameters[0].Value = ID;

            return DbHelperMySQL.Exists(strSql.ToString(), parameters);
        }


        /// <summary>
        /// 增加一条数据
        /// </summary>
        public int Add(YSWL.WeChat.Model.Core.User model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("insert into WeChat_User(");
            strSql.Append("UserId,GroupId,OpenId,UserName,NickName,Sex,City,Province,Country,Language,Headimgurl,CreateTime,Status,CancelTime,Remark,LastMsgTime)");
            strSql.Append(" values (");
            strSql.Append("?UserId,?GroupId,?OpenId,?UserName,?NickName,?Sex,?City,?Province,?Country,?Language,?Headimgurl,?CreateTime,?Status,?CancelTime,?Remark,?LastMsgTime)");
            strSql.Append(";select last_insert_id()");
            MySqlParameter[] parameters = {
					new MySqlParameter("?UserId", MySqlDbType.Int32,4),
					new MySqlParameter("?GroupId", MySqlDbType.Int32,4),
					new MySqlParameter("?OpenId", MySqlDbType.VarChar,200),
					new MySqlParameter("?UserName", MySqlDbType.VarChar,200),
					new MySqlParameter("?NickName", MySqlDbType.VarChar,200),
					new MySqlParameter("?Sex", MySqlDbType.Int32,4),
					new MySqlParameter("?City", MySqlDbType.VarChar,200),
					new MySqlParameter("?Province", MySqlDbType.VarChar,200),
					new MySqlParameter("?Country", MySqlDbType.VarChar,200),
					new MySqlParameter("?Language", MySqlDbType.VarChar,200),
					new MySqlParameter("?Headimgurl", MySqlDbType.VarChar,500),
					new MySqlParameter("?CreateTime", MySqlDbType.DateTime),
					new MySqlParameter("?Status", MySqlDbType.Int32,4),
					new MySqlParameter("?CancelTime", MySqlDbType.DateTime),
					new MySqlParameter("?Remark", MySqlDbType.VarChar,300),
					new MySqlParameter("?LastMsgTime", MySqlDbType.DateTime)};
            parameters[0].Value = model.UserId;
            parameters[1].Value = model.GroupId;
            parameters[2].Value = model.OpenId;
            parameters[3].Value = model.UserName;
            parameters[4].Value = model.NickName;
            parameters[5].Value = model.Sex;
            parameters[6].Value = model.City;
            parameters[7].Value = model.Province;
            parameters[8].Value = model.Country;
            parameters[9].Value = model.Language;
            parameters[10].Value = model.Headimgurl;
            parameters[11].Value = model.CreateTime;
            parameters[12].Value = model.Status;
            parameters[13].Value = model.CancelTime;
            parameters[14].Value = model.Remark;
            parameters[15].Value = model.LastMsgTime;

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
        public bool Update(YSWL.WeChat.Model.Core.User model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update WeChat_User set ");
            strSql.Append("UserId=?UserId,");
            strSql.Append("GroupId=?GroupId,");
            strSql.Append("OpenId=?OpenId,");
            strSql.Append("UserName=?UserName,");
            strSql.Append("NickName=?NickName,");
            strSql.Append("Sex=?Sex,");
            strSql.Append("City=?City,");
            strSql.Append("Province=?Province,");
            strSql.Append("Country=?Country,");
            strSql.Append("Language=?Language,");
            strSql.Append("Headimgurl=?Headimgurl,");
            strSql.Append("CreateTime=?CreateTime,");
            strSql.Append("Status=?Status,");
            strSql.Append("CancelTime=?CancelTime,");
            strSql.Append("Remark=?Remark,");
            strSql.Append("LastMsgTime=?LastMsgTime");
            strSql.Append(" where ID=?ID");
            MySqlParameter[] parameters = {
					new MySqlParameter("?UserId", MySqlDbType.Int32,4),
					new MySqlParameter("?GroupId", MySqlDbType.Int32,4),
					new MySqlParameter("?OpenId", MySqlDbType.VarChar,200),
					new MySqlParameter("?UserName", MySqlDbType.VarChar,200),
					new MySqlParameter("?NickName", MySqlDbType.VarChar,200),
					new MySqlParameter("?Sex", MySqlDbType.Int32,4),
					new MySqlParameter("?City", MySqlDbType.VarChar,200),
					new MySqlParameter("?Province", MySqlDbType.VarChar,200),
					new MySqlParameter("?Country", MySqlDbType.VarChar,200),
					new MySqlParameter("?Language", MySqlDbType.VarChar,200),
					new MySqlParameter("?Headimgurl", MySqlDbType.VarChar,500),
					new MySqlParameter("?CreateTime", MySqlDbType.DateTime),
					new MySqlParameter("?Status", MySqlDbType.Int32,4),
					new MySqlParameter("?CancelTime", MySqlDbType.DateTime),
					new MySqlParameter("?Remark", MySqlDbType.VarChar,300),
					new MySqlParameter("?LastMsgTime", MySqlDbType.DateTime),
					new MySqlParameter("?ID", MySqlDbType.Int32,4)};
            parameters[0].Value = model.UserId;
            parameters[1].Value = model.GroupId;
            parameters[2].Value = model.OpenId;
            parameters[3].Value = model.UserName;
            parameters[4].Value = model.NickName;
            parameters[5].Value = model.Sex;
            parameters[6].Value = model.City;
            parameters[7].Value = model.Province;
            parameters[8].Value = model.Country;
            parameters[9].Value = model.Language;
            parameters[10].Value = model.Headimgurl;
            parameters[11].Value = model.CreateTime;
            parameters[12].Value = model.Status;
            parameters[13].Value = model.CancelTime;
            parameters[14].Value = model.Remark;
            parameters[15].Value = model.LastMsgTime;
            parameters[16].Value = model.ID;

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
        public bool Delete(int ID)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append("delete from WeChat_User ");
            strSql.Append(" where ID=?ID");
            MySqlParameter[] parameters = {
					new MySqlParameter("?ID", MySqlDbType.Int32,4)
			};
            parameters[0].Value = ID;

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
        public bool DeleteList(string IDlist)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("delete from WeChat_User ");
            strSql.Append(" where ID in (" + IDlist + ")  ");
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
        public YSWL.WeChat.Model.Core.User GetModel(int ID)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append("select ID,UserId,GroupId,OpenId,UserName,NickName,Sex,City,Province,Country,Language,Headimgurl,CreateTime,Status,CancelTime,Remark,LastMsgTime from WeChat_User ");
            strSql.Append(" where ID=?ID LIMIT 1 ");
            MySqlParameter[] parameters = {
					new MySqlParameter("?ID", MySqlDbType.Int32,4)
			};
            parameters[0].Value = ID;

            YSWL.WeChat.Model.Core.User model = new YSWL.WeChat.Model.Core.User();
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
        public YSWL.WeChat.Model.Core.User DataRowToModel(DataRow row)
        {
            YSWL.WeChat.Model.Core.User model = new YSWL.WeChat.Model.Core.User();
            if (row != null)
            {
                if (row["ID"] != null && row["ID"].ToString() != "")
                {
                    model.ID = int.Parse(row["ID"].ToString());
                }
                if (row["UserId"] != null && row["UserId"].ToString() != "")
                {
                    model.UserId = int.Parse(row["UserId"].ToString());
                }
                if (row["GroupId"] != null && row["GroupId"].ToString() != "")
                {
                    model.GroupId = int.Parse(row["GroupId"].ToString());
                }
                if (row["OpenId"] != null)
                {
                    model.OpenId = row["OpenId"].ToString();
                }
                if (row["UserName"] != null)
                {
                    model.UserName = row["UserName"].ToString();
                }
                if (row["NickName"] != null)
                {
                    model.NickName = row["NickName"].ToString();
                }
                if (row["Sex"] != null && row["Sex"].ToString() != "")
                {
                    model.Sex = int.Parse(row["Sex"].ToString());
                }
                if (row["City"] != null)
                {
                    model.City = row["City"].ToString();
                }
                if (row["Province"] != null)
                {
                    model.Province = row["Province"].ToString();
                }
                if (row["Country"] != null)
                {
                    model.Country = row["Country"].ToString();
                }
                if (row["Language"] != null)
                {
                    model.Language = row["Language"].ToString();
                }
                if (row["Headimgurl"] != null)
                {
                    model.Headimgurl = row["Headimgurl"].ToString();
                }
                if (row["CreateTime"] != null && row["CreateTime"].ToString() != "")
                {
                    model.CreateTime = DateTime.Parse(row["CreateTime"].ToString());
                }
                if (row["Status"] != null && row["Status"].ToString() != "")
                {
                    model.Status = int.Parse(row["Status"].ToString());
                }
                if (row["CancelTime"] != null && row["CancelTime"].ToString() != "")
                {
                    model.CancelTime = DateTime.Parse(row["CancelTime"].ToString());
                }
                if (row["Remark"] != null)
                {
                    model.Remark = row["Remark"].ToString();
                }
                if (row["LastMsgTime"] != null && row["LastMsgTime"].ToString() != "")
                {
                    model.LastMsgTime = DateTime.Parse(row["LastMsgTime"].ToString());
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
            strSql.Append("select ID,UserId,GroupId,OpenId,UserName,NickName,Sex,City,Province,Country,Language,Headimgurl,CreateTime,Status,CancelTime,Remark,LastMsgTime ");
            strSql.Append(" FROM WeChat_User ");
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
            
            strSql.Append(" ID,UserId,GroupId,OpenId,UserName,NickName,Sex,City,Province,Country,Language,Headimgurl,CreateTime,Status,CancelTime,Remark,LastMsgTime ");
            strSql.Append(" FROM WeChat_User ");
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
            strSql.Append("select count(1) FROM WeChat_User ");
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
            strSql.Append("SELECT T.* from WeChat_User T ");
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
                strSql.Append(" order by T.ID desc");
            }
            strSql.AppendFormat(" LIMIT {0},{1}", startIndex - 1, endIndex - startIndex + 1);
            return DbHelperMySQL.Query(strSql.ToString());
        }

     


        #endregion  BasicMethod
		#region  ExtensionMethod

	    public bool DeleteUser(string OpenId, string userName)
	    {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("delete from WeChat_User ");
            strSql.Append(" where OpenId=?OpenId and UserName=?UserName");
            MySqlParameter[] parameters = {
					new MySqlParameter("?OpenId", MySqlDbType.VarChar,200),
                    new MySqlParameter("?UserName", MySqlDbType.VarChar,200)
			};
            parameters[0].Value = OpenId;
            parameters[1].Value = userName;

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

        public bool Exists(string OpenId, string userName)
	    {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select count(1) from WeChat_User");
            strSql.Append(" where OpenId=?OpenId and UserName=?UserName");
            MySqlParameter[] parameters = {
					new MySqlParameter("?OpenId", MySqlDbType.VarChar,200),
                    new MySqlParameter("?UserName", MySqlDbType.VarChar,200)
			};
            parameters[0].Value = OpenId;
            parameters[1].Value = userName;

            return DbHelperMySQL.Exists(strSql.ToString(), parameters);
	    }


        public bool UpdateEx(YSWL.WeChat.Model.Core.User model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update WeChat_User set ");
            strSql.Append("CreateTime=?CreateTime,");
            strSql.Append("Status=?Status");
            strSql.Append(" where OpenId=?OpenId and UserName=?UserName");
            MySqlParameter[] parameters = {
					new MySqlParameter("?OpenId", MySqlDbType.VarChar,200),
					new MySqlParameter("?UserName", MySqlDbType.VarChar,200),
					new MySqlParameter("?CreateTime", MySqlDbType.DateTime),
					new MySqlParameter("?Status", MySqlDbType.Int32,4)
                                        };
            parameters[0].Value = model.OpenId;
            parameters[1].Value = model.UserName;
            parameters[2].Value = model.CreateTime;
            parameters[3].Value = model.Status;

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

        public bool CancelUser(string OpenId, string userName)
	    {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update WeChat_User set ");
            strSql.Append("Status=0,");
            strSql.Append("CancelTime=?CancelTime");
            strSql.Append(" where OpenId=?OpenId and UserName=?UserName");
            MySqlParameter[] parameters = {
					new MySqlParameter("?OpenId", MySqlDbType.VarChar,200),
					new MySqlParameter("?UserName", MySqlDbType.VarChar,200),
					new MySqlParameter("?CancelTime", MySqlDbType.DateTime)
                                        };
            parameters[0].Value = OpenId;
            parameters[1].Value = userName;
            parameters[2].Value = DateTime.Now;

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

	    public bool UpdateGroup(int groupId, string ids)
	    {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update WeChat_User set ");
            strSql.Append("GroupId=?GroupId");
            strSql.Append(" where ID in (" + ids + ")  ");
            MySqlParameter[] parameters = {
					new MySqlParameter("?GroupId", MySqlDbType.Int32,4)
        };
            parameters[0].Value = groupId;
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

        public string GetNickName(string userName, string OpenId)
	    {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select NickName from WeChat_User ");
            strSql.Append(" where OpenId=?OpenId and UserName=?UserName LIMIT 1 ");
            MySqlParameter[] parameters = {
                                            new MySqlParameter("?OpenId", MySqlDbType.VarChar,200),
					new MySqlParameter("?UserName", MySqlDbType.VarChar,200)
			};
            parameters[0].Value = OpenId;
            parameters[1].Value = userName;

            YSWL.WeChat.Model.Core.User model = new YSWL.WeChat.Model.Core.User();
            object obj = DbHelperMySQL.GetSingle(strSql.ToString(), parameters);
            if (obj == null)
            {
                return "";
            }
            else
            {
                return obj.ToString();
            }
	    }

        public bool UpdateNick(string userName,string OpenId, string nickName)
	    {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update WeChat_User set  ");
            strSql.Append(" NickName=?NickName");
            strSql.Append(" where OpenId=?OpenId and UserName=?UserName");
            MySqlParameter[] parameters = {
                                           
					new MySqlParameter("?NickName", MySqlDbType.VarChar,200),
                     new MySqlParameter("?OpenId", MySqlDbType.VarChar,200),
					new MySqlParameter("?UserName", MySqlDbType.VarChar,200)
                                        };
            parameters[0].Value = nickName;
            parameters[1].Value = OpenId;
            parameters[2].Value = userName;

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

        public bool UpdateNick(int  Id, string nickName)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update WeChat_User set  ");
            strSql.Append(" NickName=?NickName");
            strSql.Append(" where ID=?ID");
            MySqlParameter[] parameters = {
					new MySqlParameter("?NickName", MySqlDbType.VarChar,200),
					new MySqlParameter("?ID", MySqlDbType.Int32,4)
                                        };
            parameters[0].Value = nickName;
            parameters[1].Value = Id;

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

	    public YSWL.WeChat.Model.Core.User GetUser(string OpenId, string userName)
	    {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select * from WeChat_User ");
            strSql.Append(" where OpenId=?OpenId and UserName=?UserName LIMIT 1 ");
            MySqlParameter[] parameters = {
					new MySqlParameter("?OpenId", MySqlDbType.VarChar,200),
                    new MySqlParameter("?UserName", MySqlDbType.VarChar,200)
			};
            parameters[0].Value = OpenId;
            parameters[1].Value = userName;

            YSWL.WeChat.Model.Core.User model = new YSWL.WeChat.Model.Core.User();
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
        public YSWL.WeChat.Model.Core.User GetUser(string OpenId, int userId)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select * from WeChat_User ");
            strSql.Append(" where OpenId=?OpenId and UserId=?UserId LIMIT 1 ");
            MySqlParameter[] parameters = {
					new MySqlParameter("?OpenId", MySqlDbType.VarChar,200),
                    new MySqlParameter("?UserId", MySqlDbType.Int32,4)
			};
            parameters[0].Value = OpenId;
            parameters[1].Value = userId;

            YSWL.WeChat.Model.Core.User model = new YSWL.WeChat.Model.Core.User();
            DataSet ds = DbHelperMySQL.Query(strSql.ToString(), parameters);
            if (ds.Tables[0].Rows.Count > 0)
            {
                return DataRowToModel(ds.Tables[0].Rows[0]);
            }
            return null;
        }


        public bool UpdateMsgTime(string openId, string userName, DateTime date)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update WeChat_User set  ");
            strSql.Append(" LastMsgTime=?LastMsgTime");
            strSql.Append(" where OpenId=?OpenId and UserName=?UserName");
            MySqlParameter[] parameters = {
                    new MySqlParameter("?LastMsgTime", MySqlDbType.DateTime),
					new MySqlParameter("?OpenId", MySqlDbType.VarChar,200),
					new MySqlParameter("?UserName", MySqlDbType.VarChar,200)
                                        };
            parameters[0].Value = date;
            parameters[1].Value = openId;
            parameters[2].Value = userName;

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


        public bool IsCanSend(string user,int hours=48)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select count(1) from WeChat_User");
            strSql.Append(" where UserName=?UserName and LastMsgTime>=DATE_ADD(now(),INTERVAL -?hours HOUR)");
            MySqlParameter[] parameters = {
					new MySqlParameter("?UserName", MySqlDbType.VarChar,200),
                    	new MySqlParameter("?hours", MySqlDbType.Int32)
			};
            parameters[0].Value = user;
            parameters[1].Value = hours;

            return DbHelperMySQL.Exists(strSql.ToString(), parameters);
        }

        public string GetNickName(string userName)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select NickName from WeChat_User ");
            strSql.Append(" where UserName=?UserName LIMIT 1 ");
            MySqlParameter[] parameters = {
					new MySqlParameter("?UserName", MySqlDbType.VarChar,200)
			};
            parameters[0].Value = userName;

            YSWL.WeChat.Model.Core.User model = new YSWL.WeChat.Model.Core.User();
            object obj = DbHelperMySQL.GetSingle(strSql.ToString(), parameters);
            if (obj == null)
            {
                return "";
            }
            else
            {
                return obj.ToString();
            }
        }


        public bool UpdateUser(YSWL.WeChat.Model.Core.User model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update WeChat_User set ");
            strSql.Append("UserId=?UserId,");
            strSql.Append("Sex=?Sex,");
            strSql.Append("City=?City,");
            strSql.Append("Province=?Province,");
            strSql.Append("Country=?Country,");
            strSql.Append("Language=?Language,");
            strSql.Append("NickName=?NickName,");
            strSql.Append("Headimgurl=?Headimgurl");
            strSql.Append(" where UserName=?UserName");
            MySqlParameter[] parameters = {
                                            	new MySqlParameter("?UserId", MySqlDbType.Int32,4),
					new MySqlParameter("?Sex", MySqlDbType.Int32,4),
					new MySqlParameter("?City", MySqlDbType.VarChar,200),
					new MySqlParameter("?Province", MySqlDbType.VarChar,200),
					new MySqlParameter("?Country", MySqlDbType.VarChar,200),
					new MySqlParameter("?Language", MySqlDbType.VarChar,200),
					new MySqlParameter("?Headimgurl", MySqlDbType.VarChar,500),
                    	new MySqlParameter("?NickName", MySqlDbType.VarChar,200),
                                        	new MySqlParameter("?UserName", MySqlDbType.VarChar,200)
                                        };
            parameters[0].Value = model.UserId;
            parameters[1].Value = model.Sex;
            parameters[2].Value = model.City;
            parameters[3].Value = model.Province;
            parameters[4].Value = model.Country;
            parameters[5].Value = model.Language;
            parameters[6].Value = model.Headimgurl;
            parameters[7].Value = model.NickName;
            parameters[8].Value = model.UserName;
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

        public DataSet GetDayCount(string strWhere)
        {
            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.Append("select CONVERT(varchar(12) , CreateTime, 111 )  AS Date  , COUNT(1)  AS DayCount   ");
            stringBuilder.Append("FROM WeChat_User  where Status=1 ");
            if (strWhere.Trim() != "")
            {
                stringBuilder.Append("  and " + strWhere);
            }
            stringBuilder.Append(" GROUP BY CONVERT(varchar(12) , CreateTime, 111 )  ORDER BY Date ");
            return DbHelperMySQL.Query(stringBuilder.ToString());
        }
        public DataSet GetCancelCount(string strWhere)
        {
            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.Append("select CONVERT(varchar(12) , CancelTime, 111 )  AS Date  , COUNT(1)  AS DayCount   ");
            stringBuilder.Append("FROM WeChat_User  where Status=0 ");
            if (strWhere.Trim() != "")
            {
                stringBuilder.Append("  and " + strWhere);
            }
            stringBuilder.Append(" GROUP BY CONVERT(varchar(12) , CancelTime, 111 )  ORDER BY Date ");
            return DbHelperMySQL.Query(stringBuilder.ToString());
        }
        public DataSet GetUserList(string openId, int groupId, int hours = 48)
        {
            StringBuilder strWhere = new StringBuilder();
            strWhere.AppendFormat(" Status=1 and OpenId='{0}'", Common.InjectionFilter.SqlFilter(openId));
            if (groupId > 0)
            {
                strWhere.AppendFormat("  and  groupId={0}", groupId);
            }
            strWhere.AppendFormat("  and  LastMsgTime>=DATE_ADD(now(),INTERVAL -{0} HOUR) ", hours);
            return GetList(strWhere.ToString());
        }
        public DataSet GetList(int Top, int hour, string filedOrder)
        {
            StringBuilder strWhere = new StringBuilder();
            strWhere.AppendFormat("    LastMsgTime>=DATE_ADD(now(),INTERVAL -{0} HOUR))", hour);
            return GetList(Top, strWhere.ToString(), filedOrder);
        }
	    #endregion  ExtensionMethod
	}
}

