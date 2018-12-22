/**  版本信息模板在安装目录下，可自行修改。
* RankDetail.cs
*
* 功 能： N/A
* 类 名： RankDetail
*
* Ver    变更日期             负责人  变更内容
* ───────────────────────────────────
* V0.01  2014/12/17 16:54:41   N/A    初版
*
* Copyright (c) 2012 YS56 Corporation. All rights reserved.
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
using YSWL.MALL.IDAL.Members;
using YSWL.DBUtility;//Please add references
namespace YSWL.MALL.SQLServerDAL.Members
{
	/// <summary>
	/// 数据访问类:RankDetail
	/// </summary>
	public partial class RankDetail:IRankDetail
	{
		public RankDetail()
		{}
		#region  BasicMethod

		/// <summary>
		/// 得到最大ID
		/// </summary>
		public int GetMaxId()
		{
		return DBHelper.DefaultDBHelper.GetMaxID("DetailID", "Accounts_RankDetail"); 
		}

		/// <summary>
		/// 是否存在该记录
		/// </summary>
		public bool Exists(int DetailID)
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("select count(1) from Accounts_RankDetail");
			strSql.Append(" where DetailID=@DetailID");
			SqlParameter[] parameters = {
					new SqlParameter("@DetailID", SqlDbType.Int,4)
			};
			parameters[0].Value = DetailID;

			return DBHelper.DefaultDBHelper.Exists(strSql.ToString(),parameters);
		}


		/// <summary>
		/// 增加一条数据
		/// </summary>
		public int Add(YSWL.MALL.Model.Members.RankDetail model)
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("insert into Accounts_RankDetail(");
			strSql.Append("RuleId,UserID,Score,ExtData,Description,CreatedDate,Type)");
			strSql.Append(" values (");
			strSql.Append("@RuleId,@UserID,@Score,@ExtData,@Description,@CreatedDate,@Type)");
			strSql.Append(";select @@IDENTITY");
			SqlParameter[] parameters = {
					new SqlParameter("@RuleId", SqlDbType.Int,4),
					new SqlParameter("@UserID", SqlDbType.Int,4),
					new SqlParameter("@Score", SqlDbType.Int,4),
					new SqlParameter("@ExtData", SqlDbType.NVarChar,-1),
					new SqlParameter("@Description", SqlDbType.NVarChar,-1),
					new SqlParameter("@CreatedDate", SqlDbType.DateTime),
					new SqlParameter("@Type", SqlDbType.Int,4)};
			parameters[0].Value = model.RuleId;
			parameters[1].Value = model.UserID;
			parameters[2].Value = model.Score;
			parameters[3].Value = model.ExtData;
			parameters[4].Value = model.Description;
			parameters[5].Value = model.CreatedDate;
			parameters[6].Value = model.Type;

			object obj = DBHelper.DefaultDBHelper.GetSingle(strSql.ToString(),parameters);
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
		public bool Update(YSWL.MALL.Model.Members.RankDetail model)
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("update Accounts_RankDetail set ");
			strSql.Append("RuleId=@RuleId,");
			strSql.Append("UserID=@UserID,");
			strSql.Append("Score=@Score,");
			strSql.Append("ExtData=@ExtData,");
			strSql.Append("Description=@Description,");
			strSql.Append("CreatedDate=@CreatedDate,");
			strSql.Append("Type=@Type");
			strSql.Append(" where DetailID=@DetailID");
			SqlParameter[] parameters = {
					new SqlParameter("@RuleId", SqlDbType.Int,4),
					new SqlParameter("@UserID", SqlDbType.Int,4),
					new SqlParameter("@Score", SqlDbType.Int,4),
					new SqlParameter("@ExtData", SqlDbType.NVarChar,-1),
					new SqlParameter("@Description", SqlDbType.NVarChar,-1),
					new SqlParameter("@CreatedDate", SqlDbType.DateTime),
					new SqlParameter("@Type", SqlDbType.Int,4),
					new SqlParameter("@DetailID", SqlDbType.Int,4)};
			parameters[0].Value = model.RuleId;
			parameters[1].Value = model.UserID;
			parameters[2].Value = model.Score;
			parameters[3].Value = model.ExtData;
			parameters[4].Value = model.Description;
			parameters[5].Value = model.CreatedDate;
			parameters[6].Value = model.Type;
			parameters[7].Value = model.DetailID;

			int rows=DBHelper.DefaultDBHelper.ExecuteSql(strSql.ToString(),parameters);
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
		public bool Delete(int DetailID)
		{
			
			StringBuilder strSql=new StringBuilder();
			strSql.Append("delete from Accounts_RankDetail ");
			strSql.Append(" where DetailID=@DetailID");
			SqlParameter[] parameters = {
					new SqlParameter("@DetailID", SqlDbType.Int,4)
			};
			parameters[0].Value = DetailID;

			int rows=DBHelper.DefaultDBHelper.ExecuteSql(strSql.ToString(),parameters);
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
		public bool DeleteList(string DetailIDlist )
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("delete from Accounts_RankDetail ");
			strSql.Append(" where DetailID in ("+DetailIDlist + ")  ");
			int rows=DBHelper.DefaultDBHelper.ExecuteSql(strSql.ToString());
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
		public YSWL.MALL.Model.Members.RankDetail GetModel(int DetailID)
		{
			
			StringBuilder strSql=new StringBuilder();
			strSql.Append("select  top 1 DetailID,RuleId,UserID,Score,ExtData,Description,CreatedDate,Type from Accounts_RankDetail ");
			strSql.Append(" where DetailID=@DetailID");
			SqlParameter[] parameters = {
					new SqlParameter("@DetailID", SqlDbType.Int,4)
			};
			parameters[0].Value = DetailID;

			YSWL.MALL.Model.Members.RankDetail model=new YSWL.MALL.Model.Members.RankDetail();
			DataSet ds=DBHelper.DefaultDBHelper.Query(strSql.ToString(),parameters);
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
		public YSWL.MALL.Model.Members.RankDetail DataRowToModel(DataRow row)
		{
			YSWL.MALL.Model.Members.RankDetail model=new YSWL.MALL.Model.Members.RankDetail();
			if (row != null)
			{
				if(row["DetailID"]!=null && row["DetailID"].ToString()!="")
				{
					model.DetailID=int.Parse(row["DetailID"].ToString());
				}
				if(row["RuleId"]!=null && row["RuleId"].ToString()!="")
				{
					model.RuleId=int.Parse(row["RuleId"].ToString());
				}
				if(row["UserID"]!=null && row["UserID"].ToString()!="")
				{
					model.UserID=int.Parse(row["UserID"].ToString());
				}
				if(row["Score"]!=null && row["Score"].ToString()!="")
				{
					model.Score=int.Parse(row["Score"].ToString());
				}
				if(row["ExtData"]!=null)
				{
					model.ExtData=row["ExtData"].ToString();
				}
				if(row["Description"]!=null)
				{
					model.Description=row["Description"].ToString();
				}
				if(row["CreatedDate"]!=null && row["CreatedDate"].ToString()!="")
				{
					model.CreatedDate=DateTime.Parse(row["CreatedDate"].ToString());
				}
				if(row["Type"]!=null && row["Type"].ToString()!="")
				{
					model.Type=int.Parse(row["Type"].ToString());
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
			strSql.Append("select DetailID,RuleId,UserID,Score,ExtData,Description,CreatedDate,Type ");
			strSql.Append(" FROM Accounts_RankDetail ");
			if(strWhere.Trim()!="")
			{
				strSql.Append(" where "+strWhere);
			}
			return DBHelper.DefaultDBHelper.Query(strSql.ToString());
		}

		/// <summary>
		/// 获得前几行数据
		/// </summary>
		public DataSet GetList(int Top,string strWhere,string filedOrder)
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("select ");
			if(Top>0)
			{
				strSql.Append(" top "+Top.ToString());
			}
			strSql.Append(" DetailID,RuleId,UserID,Score,ExtData,Description,CreatedDate,Type ");
			strSql.Append(" FROM Accounts_RankDetail ");
			if(strWhere.Trim()!="")
			{
				strSql.Append(" where "+strWhere);
			}
			strSql.Append(" order by " + filedOrder);
			return DBHelper.DefaultDBHelper.Query(strSql.ToString());
		}

		/// <summary>
		/// 获取记录总数
		/// </summary>
		public int GetRecordCount(string strWhere)
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("select count(1) FROM Accounts_RankDetail ");
			if(strWhere.Trim()!="")
			{
				strSql.Append(" where "+strWhere);
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
			StringBuilder strSql=new StringBuilder();
			strSql.Append("SELECT * FROM ( ");
			strSql.Append(" SELECT ROW_NUMBER() OVER (");
			if (!string.IsNullOrEmpty(orderby.Trim()))
			{
				strSql.Append("order by T." + orderby );
			}
			else
			{
				strSql.Append("order by T.DetailID desc");
			}
			strSql.Append(")AS Row, T.*  from Accounts_RankDetail T ");
			if (!string.IsNullOrEmpty(strWhere.Trim()))
			{
				strSql.Append(" WHERE " + strWhere);
			}
			strSql.Append(" ) TT");
			strSql.AppendFormat(" WHERE TT.Row between {0} and {1}", startIndex, endIndex);
			return DBHelper.DefaultDBHelper.Query(strSql.ToString());
		}

		/*
		/// <summary>
		/// 分页获取数据列表
		/// </summary>
		public DataSet GetList(int PageSize,int PageIndex,string strWhere)
		{
			SqlParameter[] parameters = {
					new SqlParameter("@tblName", SqlDbType.VarChar, 255),
					new SqlParameter("@fldName", SqlDbType.VarChar, 255),
					new SqlParameter("@PageSize", SqlDbType.Int),
					new SqlParameter("@PageIndex", SqlDbType.Int),
					new SqlParameter("@IsReCount", SqlDbType.Bit),
					new SqlParameter("@OrderType", SqlDbType.Bit),
					new SqlParameter("@strWhere", SqlDbType.VarChar,1000),
					};
			parameters[0].Value = "Accounts_RankDetail";
			parameters[1].Value = "DetailID";
			parameters[2].Value = PageSize;
			parameters[3].Value = PageIndex;
			parameters[4].Value = 0;
			parameters[5].Value = 0;
			parameters[6].Value = strWhere;	
			return DBHelper.DefaultDBHelper.RunProcedure("UP_GetRecordByPage",parameters,"ds");
		}*/

		#endregion  BasicMethod
        #region 扩展方法
        /// <summary>
        /// 添加积分明细（事务处理，需要更新个人的总成长值）
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public bool AddDetail(YSWL.MALL.Model.Members.RankDetail model,bool IsAuto)
        {
            List<CommandInfo> sqllist = new List<CommandInfo>();
            //添加成长值明细
            StringBuilder strSql = new StringBuilder();
            strSql.Append("insert into Accounts_RankDetail(");
            strSql.Append("RuleID,UserID,Score,ExtData,Description,CreatedDate,Type)");
            strSql.Append(" values (");
            strSql.Append("@RuleID,@UserID,@Score,@ExtData,@Description,@CreatedDate,@Type)");
            SqlParameter[] parameters = {
                    new SqlParameter("@RuleID", SqlDbType.Int,4),
                    new SqlParameter("@UserID", SqlDbType.Int,4),
                    new SqlParameter("@Score", SqlDbType.Int,4),
                    new SqlParameter("@ExtData", SqlDbType.NVarChar),
                    new SqlParameter("@Description", SqlDbType.NVarChar),
                    new SqlParameter("@CreatedDate", SqlDbType.DateTime),
            new SqlParameter("@Type", SqlDbType.Int,4)};
            parameters[0].Value = model.RuleId;
            parameters[1].Value = model.UserID;
            parameters[2].Value = model.Score;
            parameters[3].Value = model.ExtData;
            //if (model.Type == 0)
            //{
            //    parameters[4].Value = model.Score;
            //}
            //if (model.Type == 1)
            //{
            //    parameters[4].Value = -model.Score;
            //}
            parameters[4].Value = model.Description;
            parameters[5].Value = model.CreatedDate;
            parameters[6].Value = model.Type;
            CommandInfo cmd = new CommandInfo(strSql.ToString(), parameters);
            sqllist.Add(cmd);

            //更新个人成长值数
            StringBuilder strSql2 = new StringBuilder();
            strSql2.Append("update Accounts_UsersExp set ");
            if (model.Type == 0)//获取成长值
            {
                strSql2.Append("RankScore=RankScore+@RankScore");
            }
            if (model.Type == 1)//扣除成长值
            {
                strSql2.Append("RankScore=RankScore-@RankScore");
            }
            strSql2.Append(" where UserID=@UserID ");
            SqlParameter[] parameters2 = {
					new SqlParameter("@RankScore", SqlDbType.Int,4),
                    new SqlParameter("@UserID", SqlDbType.Int,4)
                                        };
            parameters2[0].Value = model.Score;
            parameters2[1].Value = model.UserID;
            cmd = new CommandInfo(strSql2.ToString(), parameters2);
            sqllist.Add(cmd);

            //更新等级
            if (IsAuto)
            {
                StringBuilder strSql3 = new StringBuilder();
                strSql3.Append("update Accounts_UsersExp  ");

                strSql3.Append(" SET Grade=(SELECT  TOP 1  RankId FROM  Accounts_UserRank WHERE Accounts_UsersExp.RankScore BETWEEN ScoreMin AND ScoreMax )  WHERE   UserID=@UserID");
                SqlParameter[] parameters3 = {
                    new SqlParameter("@UserID", SqlDbType.Int,4)
                                        };
                parameters3[0].Value = model.UserID;
                cmd = new CommandInfo(strSql3.ToString(), parameters3);
                sqllist.Add(cmd);
            }

            int rowsAffected = DBHelper.DefaultDBHelper.ExecuteSqlTran(sqllist);
            if (rowsAffected > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        public bool UpdatePoints(int userId, int points, int type)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update Accounts_UsersExp set ");
            if (type == 0)//获取成长值
            {
                strSql.Append("RankScore=RankScore+@RankScore");
            }
            if (type == 1)//消费成长值
            {
                strSql.Append("RankScore=RankScore-@RankScore");
            }
            strSql.Append(" where UserID=@UserID ");
            SqlParameter[] parameters2 = {
					new SqlParameter("@RankScore", SqlDbType.Int,4),
                    new SqlParameter("@UserID", SqlDbType.Int,4)
                                        };
            parameters2[0].Value = points;
            parameters2[1].Value = userId;
            int rows = DBHelper.DefaultDBHelper.ExecuteSql(strSql.ToString(), parameters2);
            if (rows > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public int GetSignCount(int userId)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select count(1) FROM Accounts_RankDetail ");
            strSql.Append(" WHERE UserID=@UserID and  RuleId=(SELECT RuleId  FROM Accounts_RankRule WHERE ActionId=10) ");
            SqlParameter[] parameters = {
					new SqlParameter("@UserID", SqlDbType.Int,4)
			};
            parameters[0].Value = userId;
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

        public DataSet GetSignListByPage(int userId, string orderby, int startIndex, int endIndex)
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
                strSql.Append("order by T.DetailID desc");
            }
            strSql.Append(")AS Row, T.*  from Accounts_RankDetail T ");
            strSql.AppendFormat(" WHERE UserID={0} and  RuleId=(SELECT RuleId  FROM Accounts_RankRule WHERE ActionId=10) ", userId);
            strSql.Append(" ) TT");
            strSql.AppendFormat(" WHERE TT.Row between {0} and {1}", startIndex, endIndex);
            return DBHelper.DefaultDBHelper.Query(strSql.ToString());
        }

        public int GetCount(int userid, string unit, int cycle, int RuleId)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select count(1) FROM Accounts_RankDetail ");
            strSql.AppendFormat(" where  userid={0}  and RuleId={1} and datediff({2}, CreatedDate, GETDATE())<{3} ", userid, RuleId, unit, cycle);
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

        #endregion
	}
}

