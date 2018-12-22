/**  版本信息模板在安装目录下，可自行修改。
* ActivityAward.cs
*
* 功 能： N/A
* 类 名： ActivityAward
*
* Ver    变更日期             负责人  变更内容
* ───────────────────────────────────
* V0.01  2013/12/25 19:04:15   N/A    初版
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
	/// 数据访问类:ActivityAward
	/// </summary>
	public partial class ActivityAward:IActivityAward
	{
		public ActivityAward()
		{}
        #region  BasicMethod

        /// <summary>
        /// 得到最大ID
        /// </summary>
        public int GetMaxId()
        {
            return DbHelperMySQL.GetMaxID("AwardId", "WeChat_ActivityAward");
        }

        /// <summary>
        /// 是否存在该记录
        /// </summary>
        public bool Exists(int AwardId)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select count(1) from WeChat_ActivityAward");
            strSql.Append(" where AwardId=?AwardId");
            MySqlParameter[] parameters = {
					new MySqlParameter("?AwardId", MySqlDbType.Int32,4)
			};
            parameters[0].Value = AwardId;

            return DbHelperMySQL.Exists(strSql.ToString(), parameters);
        }


        /// <summary>
        /// 增加一条数据
        /// </summary>
        public int Add(YSWL.WeChat.Model.Activity.ActivityAward model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("insert into WeChat_ActivityAward(");
            strSql.Append("ActivityId,AwardName,GiftName,ImageUrl,ThumbImage,Count,AwardDesc,Remark,TargetId)");
            strSql.Append(" values (");
            strSql.Append("?ActivityId,?AwardName,?GiftName,?ImageUrl,?ThumbImage,?Count,?AwardDesc,?Remark,?TargetId)");
            strSql.Append(";select last_insert_id()");
            MySqlParameter[] parameters = {
					new MySqlParameter("?ActivityId", MySqlDbType.Int32,4),
					new MySqlParameter("?AwardName", MySqlDbType.VarChar,200),
					new MySqlParameter("?GiftName", MySqlDbType.VarChar,200),
					new MySqlParameter("?ImageUrl", MySqlDbType.VarChar,200),
					new MySqlParameter("?ThumbImage", MySqlDbType.VarChar,200),
					new MySqlParameter("?Count", MySqlDbType.Int32,4),
					new MySqlParameter("?AwardDesc", MySqlDbType.VarChar,-1),
					new MySqlParameter("?Remark", MySqlDbType.VarChar,-1),
					new MySqlParameter("?TargetId", MySqlDbType.Int32,4)};
            parameters[0].Value = model.ActivityId;
            parameters[1].Value = model.AwardName;
            parameters[2].Value = model.GiftName;
            parameters[3].Value = model.ImageUrl;
            parameters[4].Value = model.ThumbImage;
            parameters[5].Value = model.Count;
            parameters[6].Value = model.AwardDesc;
            parameters[7].Value = model.Remark;
            parameters[8].Value = model.TargetId;

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
        public bool Update(YSWL.WeChat.Model.Activity.ActivityAward model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update WeChat_ActivityAward set ");
            strSql.Append("ActivityId=?ActivityId,");
            strSql.Append("AwardName=?AwardName,");
            strSql.Append("GiftName=?GiftName,");
            strSql.Append("ImageUrl=?ImageUrl,");
            strSql.Append("ThumbImage=?ThumbImage,");
            strSql.Append("Count=?Count,");
            strSql.Append("AwardDesc=?AwardDesc,");
            strSql.Append("Remark=?Remark,");
            strSql.Append("TargetId=?TargetId");
            strSql.Append(" where AwardId=?AwardId");
            MySqlParameter[] parameters = {
					new MySqlParameter("?ActivityId", MySqlDbType.Int32,4),
					new MySqlParameter("?AwardName", MySqlDbType.VarChar,200),
					new MySqlParameter("?GiftName", MySqlDbType.VarChar,200),
					new MySqlParameter("?ImageUrl", MySqlDbType.VarChar,200),
					new MySqlParameter("?ThumbImage", MySqlDbType.VarChar,200),
					new MySqlParameter("?Count", MySqlDbType.Int32,4),
					new MySqlParameter("?AwardDesc", MySqlDbType.VarChar,-1),
					new MySqlParameter("?Remark", MySqlDbType.VarChar,-1),
					new MySqlParameter("?TargetId", MySqlDbType.Int32,4),
					new MySqlParameter("?AwardId", MySqlDbType.Int32,4)};
            parameters[0].Value = model.ActivityId;
            parameters[1].Value = model.AwardName;
            parameters[2].Value = model.GiftName;
            parameters[3].Value = model.ImageUrl;
            parameters[4].Value = model.ThumbImage;
            parameters[5].Value = model.Count;
            parameters[6].Value = model.AwardDesc;
            parameters[7].Value = model.Remark;
            parameters[8].Value = model.TargetId;
            parameters[9].Value = model.AwardId;

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
        public bool Delete(int AwardId)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append("delete from WeChat_ActivityAward ");
            strSql.Append(" where AwardId=?AwardId");
            MySqlParameter[] parameters = {
					new MySqlParameter("?AwardId", MySqlDbType.Int32,4)
			};
            parameters[0].Value = AwardId;

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
        public bool DeleteList(string AwardIdlist)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("delete from WeChat_ActivityAward ");
            strSql.Append(" where AwardId in (" + AwardIdlist + ")  ");
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
        public YSWL.WeChat.Model.Activity.ActivityAward GetModel(int AwardId)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append("select AwardId,ActivityId,AwardName,GiftName,ImageUrl,ThumbImage,Count,AwardDesc,Remark,TargetId from WeChat_ActivityAward ");
            strSql.Append(" where AwardId=?AwardId LIMIT 1 ");
            MySqlParameter[] parameters = {
					new MySqlParameter("?AwardId", MySqlDbType.Int32,4)
			};
            parameters[0].Value = AwardId;

            YSWL.WeChat.Model.Activity.ActivityAward model = new YSWL.WeChat.Model.Activity.ActivityAward();
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
        public YSWL.WeChat.Model.Activity.ActivityAward DataRowToModel(DataRow row)
        {
            YSWL.WeChat.Model.Activity.ActivityAward model = new YSWL.WeChat.Model.Activity.ActivityAward();
            if (row != null)
            {
                if (row["AwardId"] != null && row["AwardId"].ToString() != "")
                {
                    model.AwardId = int.Parse(row["AwardId"].ToString());
                }
                if (row["ActivityId"] != null && row["ActivityId"].ToString() != "")
                {
                    model.ActivityId = int.Parse(row["ActivityId"].ToString());
                }
                if (row["AwardName"] != null)
                {
                    model.AwardName = row["AwardName"].ToString();
                }
                if (row["GiftName"] != null)
                {
                    model.GiftName = row["GiftName"].ToString();
                }
                if (row["ImageUrl"] != null)
                {
                    model.ImageUrl = row["ImageUrl"].ToString();
                }
                if (row["ThumbImage"] != null)
                {
                    model.ThumbImage = row["ThumbImage"].ToString();
                }
                if (row["Count"] != null && row["Count"].ToString() != "")
                {
                    model.Count = int.Parse(row["Count"].ToString());
                }
                if (row["AwardDesc"] != null)
                {
                    model.AwardDesc = row["AwardDesc"].ToString();
                }
                if (row["Remark"] != null)
                {
                    model.Remark = row["Remark"].ToString();
                }
                if (row["TargetId"] != null && row["TargetId"].ToString() != "")
                {
                    model.TargetId = int.Parse(row["TargetId"].ToString());
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
            strSql.Append("select AwardId,ActivityId,AwardName,GiftName,ImageUrl,ThumbImage,Count,AwardDesc,Remark,TargetId ");
            strSql.Append(" FROM WeChat_ActivityAward ");
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
            
            strSql.Append(" AwardId,ActivityId,AwardName,GiftName,ImageUrl,ThumbImage,Count,AwardDesc,Remark,TargetId ");
            strSql.Append(" FROM WeChat_ActivityAward ");
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
            strSql.Append("select count(1) FROM WeChat_ActivityAward ");
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
            strSql.Append("SELECT T.* from WeChat_ActivityAward T ");
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
                strSql.Append(" order by T.AwardId desc");
            }
            strSql.AppendFormat(" LIMIT {0},{1}", startIndex - 1, endIndex - startIndex + 1);
            return DbHelperMySQL.Query(strSql.ToString());
        }




        #endregion  BasicMethod
		#region  ExtensionMethod
        public YSWL.WeChat.Model.Activity.ActivityAward GetAwardInfo(int targetId, int ActivityId)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select *  from WeChat_ActivityAward ");
            strSql.Append(" where TargetId=?TargetId and ActivityId=?ActivityId LIMIT 1 ");
            MySqlParameter[] parameters = new MySqlParameter[]
			{
				new MySqlParameter("?TargetId", MySqlDbType.Int32, 4),
				new MySqlParameter("?ActivityId", MySqlDbType.Int32, 4)
			};
            parameters[0].Value = targetId;
            parameters[1].Value = ActivityId;
            YSWL.WeChat.Model.Activity.ActivityAward model = new YSWL.WeChat.Model.Activity.ActivityAward();
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

