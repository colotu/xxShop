﻿/**
* SupplierRankThemes.cs
*
* 功 能： N/A
* 类 名： SupplierRankThemes
*
* Ver    变更日期             负责人  变更内容
* ───────────────────────────────────
* V0.01  2013/8/26 17:31:49   Ben    初版
*
* Copyright (c) 2012-2013 YS56 Corporation. All rights reserved.
*┌──────────────────────────────────┐
*│　此技术信息为本公司机密信息，未经本公司书面同意禁止向第三方披露．　│
*│　版权所有：云商未来（北京）科技有限公司　　　　　　　　　　　　　　│
*└──────────────────────────────────┘
*/
using System;
using System.Data;
using System.Text;
using System.Data.SqlClient;
using YSWL.MALL.IDAL.Shop.Supplier;
using YSWL.DBUtility;//Please add references
namespace YSWL.MALL.SQLServerDAL.Shop.Supplier
{
	/// <summary>
	/// 数据访问类:SupplierRankThemes
	/// </summary>
	public partial class SupplierRankThemes:ISupplierRankThemes
	{
		public SupplierRankThemes()
		{}
		#region  BasicMethod

		/// <summary>
		/// 得到最大ID
		/// </summary>
		public int GetMaxId()
		{
		return DBHelper.DefaultDBHelper.GetMaxID("RankId", "Shop_SupplierRankThemes"); 
		}

		/// <summary>
		/// 是否存在该记录
		/// </summary>
		public bool Exists(int RankId,int ThemeId)
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("select count(1) from Shop_SupplierRankThemes");
			strSql.Append(" where RankId=@RankId and ThemeId=@ThemeId ");
			SqlParameter[] parameters = {
					new SqlParameter("@RankId", SqlDbType.Int,4),
					new SqlParameter("@ThemeId", SqlDbType.Int,4)			};
			parameters[0].Value = RankId;
			parameters[1].Value = ThemeId;

			return DBHelper.DefaultDBHelper.Exists(strSql.ToString(),parameters);
		}


		/// <summary>
		/// 增加一条数据
		/// </summary>
		public bool Add(YSWL.MALL.Model.Shop.Supplier.SupplierRankThemes model)
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("insert into Shop_SupplierRankThemes(");
			strSql.Append("RankId,ThemeId)");
			strSql.Append(" values (");
			strSql.Append("@RankId,@ThemeId)");
			SqlParameter[] parameters = {
					new SqlParameter("@RankId", SqlDbType.Int,4),
					new SqlParameter("@ThemeId", SqlDbType.Int,4)};
			parameters[0].Value = model.RankId;
			parameters[1].Value = model.ThemeId;

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
		public bool Update(YSWL.MALL.Model.Shop.Supplier.SupplierRankThemes model)
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("update Shop_SupplierRankThemes set ");
			strSql.Append("RankId=@RankId,");
			strSql.Append("ThemeId=@ThemeId");
			strSql.Append(" where RankId=@RankId and ThemeId=@ThemeId ");
			SqlParameter[] parameters = {
					new SqlParameter("@RankId", SqlDbType.Int,4),
					new SqlParameter("@ThemeId", SqlDbType.Int,4)};
			parameters[0].Value = model.RankId;
			parameters[1].Value = model.ThemeId;

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
		public bool Delete(int RankId,int ThemeId)
		{
			
			StringBuilder strSql=new StringBuilder();
			strSql.Append("delete from Shop_SupplierRankThemes ");
			strSql.Append(" where RankId=@RankId and ThemeId=@ThemeId ");
			SqlParameter[] parameters = {
					new SqlParameter("@RankId", SqlDbType.Int,4),
					new SqlParameter("@ThemeId", SqlDbType.Int,4)			};
			parameters[0].Value = RankId;
			parameters[1].Value = ThemeId;

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
		public YSWL.MALL.Model.Shop.Supplier.SupplierRankThemes GetModel(int RankId,int ThemeId)
		{
			
			StringBuilder strSql=new StringBuilder();
			strSql.Append("select  top 1 RankId,ThemeId from Shop_SupplierRankThemes ");
			strSql.Append(" where RankId=@RankId and ThemeId=@ThemeId ");
			SqlParameter[] parameters = {
					new SqlParameter("@RankId", SqlDbType.Int,4),
					new SqlParameter("@ThemeId", SqlDbType.Int,4)			};
			parameters[0].Value = RankId;
			parameters[1].Value = ThemeId;

			YSWL.MALL.Model.Shop.Supplier.SupplierRankThemes model=new YSWL.MALL.Model.Shop.Supplier.SupplierRankThemes();
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
		public YSWL.MALL.Model.Shop.Supplier.SupplierRankThemes DataRowToModel(DataRow row)
		{
			YSWL.MALL.Model.Shop.Supplier.SupplierRankThemes model=new YSWL.MALL.Model.Shop.Supplier.SupplierRankThemes();
			if (row != null)
			{
				if(row["RankId"]!=null && row["RankId"].ToString()!="")
				{
					model.RankId=int.Parse(row["RankId"].ToString());
				}
				if(row["ThemeId"]!=null && row["ThemeId"].ToString()!="")
				{
					model.ThemeId=int.Parse(row["ThemeId"].ToString());
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
			strSql.Append("select RankId,ThemeId ");
			strSql.Append(" FROM Shop_SupplierRankThemes ");
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
			strSql.Append(" RankId,ThemeId ");
			strSql.Append(" FROM Shop_SupplierRankThemes ");
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
			strSql.Append("select count(1) FROM Shop_SupplierRankThemes ");
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
				strSql.Append("order by T.ThemeId desc");
			}
			strSql.Append(")AS Row, T.*  from Shop_SupplierRankThemes T ");
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
					new SqlParameter("@tblName", SqlDbType.NVarChar, 255),
					new SqlParameter("@fldName", SqlDbType.NVarChar, 255),
					new SqlParameter("@PageSize", SqlDbType.Int),
					new SqlParameter("@PageIndex", SqlDbType.Int),
					new SqlParameter("@IsReCount", SqlDbType.Bit),
					new SqlParameter("@OrderType", SqlDbType.Bit),
					new SqlParameter("@strWhere", SqlDbType.NVarChar,1000),
					};
			parameters[0].Value = "Shop_SupplierRankThemes";
			parameters[1].Value = "ThemeId";
			parameters[2].Value = PageSize;
			parameters[3].Value = PageIndex;
			parameters[4].Value = 0;
			parameters[5].Value = 0;
			parameters[6].Value = strWhere;	
			return DBHelper.DefaultDBHelper.RunProcedure("UP_GetRecordByPage",parameters,"ds");
		}*/

		#endregion  BasicMethod
		#region  ExtensionMethod

		#endregion  ExtensionMethod
	}
}

