/**  版本信息模板在安装目录下，可自行修改。
* UserDistribution.cs
*
* 功 能： N/A
* 类 名： UserDistribution
*
* Ver    变更日期             负责人  变更内容
* ───────────────────────────────────
* V0.01  2017/1/14 12:07:50   N/A    初版
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
using System.Data.SqlClient;
using YSWL.DBUtility;
using YSWL.MALL.IDAL.Members;
namespace YSWL.MALL.SQLServerDAL.Members
{
	/// <summary>
	/// 数据访问类:UserDistribution
	/// </summary>
	public partial class UserDistribution:IUserDistribution
	{
		public UserDistribution()
		{}
		#region  BasicMethod

		/// <summary>
		/// 得到最大ID
		/// </summary>
		public int GetMaxId()
		{
		return DBHelper.DefaultDBHelper.GetMaxID("UserId", "Accounts_UserDistribution"); 
		}

		/// <summary>
		/// 是否存在该记录
		/// </summary>
		public bool Exists(int UserId,int DistributionId)
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("select count(1) from Accounts_UserDistribution");
			strSql.Append(" where UserId=@UserId and DistributionId=@DistributionId ");
			SqlParameter[] parameters = {
					new SqlParameter("@UserId", SqlDbType.Int,4),
					new SqlParameter("@DistributionId", SqlDbType.Int,4)			};
			parameters[0].Value = UserId;
			parameters[1].Value = DistributionId;

			return DBHelper.DefaultDBHelper.Exists(strSql.ToString(),parameters);
		}


		/// <summary>
		/// 增加一条数据
		/// </summary>
		public bool Add(YSWL.MALL.Model.Members.UserDistribution model)
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("insert into Accounts_UserDistribution(");
			strSql.Append("UserId,DistributionId)");
			strSql.Append(" values (");
			strSql.Append("@UserId,@DistributionId)");
			SqlParameter[] parameters = {
					new SqlParameter("@UserId", SqlDbType.Int,4),
					new SqlParameter("@DistributionId", SqlDbType.Int,4)};
			parameters[0].Value = model.UserId;
			parameters[1].Value = model.DistributionId;

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
		/// 更新一条数据
		/// </summary>
		public bool Update(YSWL.MALL.Model.Members.UserDistribution model)
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("update Accounts_UserDistribution set ");
			strSql.Append("UserId=@UserId,");
			strSql.Append("DistributionId=@DistributionId");
			strSql.Append(" where UserId=@UserId and DistributionId=@DistributionId ");
			SqlParameter[] parameters = {
					new SqlParameter("@UserId", SqlDbType.Int,4),
					new SqlParameter("@DistributionId", SqlDbType.Int,4)};
			parameters[0].Value = model.UserId;
			parameters[1].Value = model.DistributionId;

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
		public bool Delete(int UserId,int DistributionId)
		{
			
			StringBuilder strSql=new StringBuilder();
			strSql.Append("delete from Accounts_UserDistribution ");
			strSql.Append(" where UserId=@UserId and DistributionId=@DistributionId ");
			SqlParameter[] parameters = {
					new SqlParameter("@UserId", SqlDbType.Int,4),
					new SqlParameter("@DistributionId", SqlDbType.Int,4)			};
			parameters[0].Value = UserId;
			parameters[1].Value = DistributionId;

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
		/// 得到一个对象实体
		/// </summary>
		public YSWL.MALL.Model.Members.UserDistribution GetModel(int UserId,int DistributionId)
		{
			
			StringBuilder strSql=new StringBuilder();
			strSql.Append("select  top 1 UserId,DistributionId from Accounts_UserDistribution ");
			strSql.Append(" where UserId=@UserId and DistributionId=@DistributionId ");
			SqlParameter[] parameters = {
					new SqlParameter("@UserId", SqlDbType.Int,4),
					new SqlParameter("@DistributionId", SqlDbType.Int,4)			};
			parameters[0].Value = UserId;
			parameters[1].Value = DistributionId;

			YSWL.MALL.Model.Members.UserDistribution model=new YSWL.MALL.Model.Members.UserDistribution();
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
		public YSWL.MALL.Model.Members.UserDistribution DataRowToModel(DataRow row)
		{
			YSWL.MALL.Model.Members.UserDistribution model=new YSWL.MALL.Model.Members.UserDistribution();
			if (row != null)
			{
				if(row["UserId"]!=null && row["UserId"].ToString()!="")
				{
					model.UserId=int.Parse(row["UserId"].ToString());
				}
				if(row["DistributionId"]!=null && row["DistributionId"].ToString()!="")
				{
					model.DistributionId=int.Parse(row["DistributionId"].ToString());
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
			strSql.Append("select UserId,DistributionId ");
			strSql.Append(" FROM Accounts_UserDistribution ");
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
			strSql.Append(" UserId,DistributionId ");
			strSql.Append(" FROM Accounts_UserDistribution ");
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
			strSql.Append("select count(1) FROM Accounts_UserDistribution ");
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
				strSql.Append("order by T.DistributionId desc");
			}
			strSql.Append(")AS Row, T.*  from Accounts_UserDistribution T ");
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
			parameters[0].Value = "Accounts_UserDistribution";
			parameters[1].Value = "DistributionId";
			parameters[2].Value = PageSize;
			parameters[3].Value = PageIndex;
			parameters[4].Value = 0;
			parameters[5].Value = 0;
			parameters[6].Value = strWhere;	
			return DBHelper.DefaultDBHelper.RunProcedure("UP_GetRecordByPage",parameters,"ds");
		}*/

        #endregion  BasicMethod
        #region  ExtensionMethod

	    public int GetDistributionId(int userId)
	    {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select top 1   DistributionId FROM Accounts_UserDistribution ");
            strSql.AppendFormat("  where UserId={0} ", userId);
            object obj = DBHelper.DefaultDBHelper.GetSingle(strSql.ToString());
            return Common.Globals.SafeInt(obj,0);
        }

	    #endregion  ExtensionMethod
    }
}

