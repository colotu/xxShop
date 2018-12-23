﻿/**
* SupplierRank.cs
*
* 功 能： N/A
* 类 名： SupplierRank
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
	/// 数据访问类:SupplierRank
	/// </summary>
	public partial class SupplierRank:ISupplierRank
	{
		public SupplierRank()
		{}
		#region  BasicMethod

		/// <summary>
		/// 得到最大ID
		/// </summary>
		public int GetMaxId()
		{
		return DBHelper.DefaultDBHelper.GetMaxID("RankId", "Shop_SupplierRank"); 
		}

		/// <summary>
		/// 是否存在该记录
		/// </summary>
		public bool Exists(int RankId)
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("select count(1) from Shop_SupplierRank");
			strSql.Append(" where RankId=@RankId");
			SqlParameter[] parameters = {
					new SqlParameter("@RankId", SqlDbType.Int,4)
			};
			parameters[0].Value = RankId;

			return DBHelper.DefaultDBHelper.Exists(strSql.ToString(),parameters);
		}


		/// <summary>
		/// 增加一条数据
		/// </summary>
		public int Add(YSWL.MALL.Model.Shop.Supplier.SupplierRank model)
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("insert into Shop_SupplierRank(");
			strSql.Append("Name,ProductCount,ImageCount,Price,IsDefault,IsApproval,Description,RankMin,RankMax)");
			strSql.Append(" values (");
			strSql.Append("@Name,@ProductCount,@ImageCount,@Price,@IsDefault,@IsApproval,@Description,@RankMin,@RankMax)");
			strSql.Append(";select @@IDENTITY");
			SqlParameter[] parameters = {
					new SqlParameter("@Name", SqlDbType.NVarChar,50),
					new SqlParameter("@ProductCount", SqlDbType.Int,4),
					new SqlParameter("@ImageCount", SqlDbType.Int,4),
					new SqlParameter("@Price", SqlDbType.Money,8),
					new SqlParameter("@IsDefault", SqlDbType.Bit,1),
					new SqlParameter("@IsApproval", SqlDbType.Bit,1),
					new SqlParameter("@Description", SqlDbType.Text),
					new SqlParameter("@RankMin", SqlDbType.Money,8),
					new SqlParameter("@RankMax", SqlDbType.Money,8)};
			parameters[0].Value = model.Name;
			parameters[1].Value = model.ProductCount;
			parameters[2].Value = model.ImageCount;
			parameters[3].Value = model.Price;
			parameters[4].Value = model.IsDefault;
			parameters[5].Value = model.IsApproval;
			parameters[6].Value = model.Description;
			parameters[7].Value = model.RankMin;
			parameters[8].Value = model.RankMax;

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
		public bool Update(YSWL.MALL.Model.Shop.Supplier.SupplierRank model)
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("update Shop_SupplierRank set ");
			strSql.Append("Name=@Name,");
			strSql.Append("ProductCount=@ProductCount,");
			strSql.Append("ImageCount=@ImageCount,");
			strSql.Append("Price=@Price,");
			strSql.Append("IsDefault=@IsDefault,");
			strSql.Append("IsApproval=@IsApproval,");
			strSql.Append("Description=@Description,");
			strSql.Append("RankMin=@RankMin,");
			strSql.Append("RankMax=@RankMax");
			strSql.Append(" where RankId=@RankId");
			SqlParameter[] parameters = {
					new SqlParameter("@Name", SqlDbType.NVarChar,50),
					new SqlParameter("@ProductCount", SqlDbType.Int,4),
					new SqlParameter("@ImageCount", SqlDbType.Int,4),
					new SqlParameter("@Price", SqlDbType.Money,8),
					new SqlParameter("@IsDefault", SqlDbType.Bit,1),
					new SqlParameter("@IsApproval", SqlDbType.Bit,1),
					new SqlParameter("@Description", SqlDbType.Text),
					new SqlParameter("@RankMin", SqlDbType.Money,8),
					new SqlParameter("@RankMax", SqlDbType.Money,8),
					new SqlParameter("@RankId", SqlDbType.Int,4)};
			parameters[0].Value = model.Name;
			parameters[1].Value = model.ProductCount;
			parameters[2].Value = model.ImageCount;
			parameters[3].Value = model.Price;
			parameters[4].Value = model.IsDefault;
			parameters[5].Value = model.IsApproval;
			parameters[6].Value = model.Description;
			parameters[7].Value = model.RankMin;
			parameters[8].Value = model.RankMax;
			parameters[9].Value = model.RankId;

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
		public bool Delete(int RankId)
		{
			
			StringBuilder strSql=new StringBuilder();
			strSql.Append("delete from Shop_SupplierRank ");
			strSql.Append(" where RankId=@RankId");
			SqlParameter[] parameters = {
					new SqlParameter("@RankId", SqlDbType.Int,4)
			};
			parameters[0].Value = RankId;

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
		public bool DeleteList(string RankIdlist )
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("delete from Shop_SupplierRank ");
			strSql.Append(" where RankId in ("+RankIdlist + ")  ");
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
		public YSWL.MALL.Model.Shop.Supplier.SupplierRank GetModel(int RankId)
		{
			
			StringBuilder strSql=new StringBuilder();
			strSql.Append("select  top 1 RankId,Name,ProductCount,ImageCount,Price,IsDefault,IsApproval,Description,RankMin,RankMax from Shop_SupplierRank ");
			strSql.Append(" where RankId=@RankId");
			SqlParameter[] parameters = {
					new SqlParameter("@RankId", SqlDbType.Int,4)
			};
			parameters[0].Value = RankId;

			YSWL.MALL.Model.Shop.Supplier.SupplierRank model=new YSWL.MALL.Model.Shop.Supplier.SupplierRank();
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
		public YSWL.MALL.Model.Shop.Supplier.SupplierRank DataRowToModel(DataRow row)
		{
			YSWL.MALL.Model.Shop.Supplier.SupplierRank model=new YSWL.MALL.Model.Shop.Supplier.SupplierRank();
			if (row != null)
			{
				if(row["RankId"]!=null && row["RankId"].ToString()!="")
				{
					model.RankId=int.Parse(row["RankId"].ToString());
				}
				if(row["Name"]!=null)
				{
					model.Name=row["Name"].ToString();
				}
				if(row["ProductCount"]!=null && row["ProductCount"].ToString()!="")
				{
					model.ProductCount=int.Parse(row["ProductCount"].ToString());
				}
				if(row["ImageCount"]!=null && row["ImageCount"].ToString()!="")
				{
					model.ImageCount=int.Parse(row["ImageCount"].ToString());
				}
				if(row["Price"]!=null && row["Price"].ToString()!="")
				{
					model.Price=decimal.Parse(row["Price"].ToString());
				}
				if(row["IsDefault"]!=null && row["IsDefault"].ToString()!="")
				{
					if((row["IsDefault"].ToString()=="1")||(row["IsDefault"].ToString().ToLower()=="true"))
					{
						model.IsDefault=true;
					}
					else
					{
						model.IsDefault=false;
					}
				}
				if(row["IsApproval"]!=null && row["IsApproval"].ToString()!="")
				{
					if((row["IsApproval"].ToString()=="1")||(row["IsApproval"].ToString().ToLower()=="true"))
					{
						model.IsApproval=true;
					}
					else
					{
						model.IsApproval=false;
					}
				}
				if(row["Description"]!=null)
				{
					model.Description=row["Description"].ToString();
				}
				if(row["RankMin"]!=null && row["RankMin"].ToString()!="")
				{
					model.RankMin=decimal.Parse(row["RankMin"].ToString());
				}
				if(row["RankMax"]!=null && row["RankMax"].ToString()!="")
				{
					model.RankMax=decimal.Parse(row["RankMax"].ToString());
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
			strSql.Append("select RankId,Name,ProductCount,ImageCount,Price,IsDefault,IsApproval,Description,RankMin,RankMax ");
			strSql.Append(" FROM Shop_SupplierRank ");
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
			strSql.Append(" RankId,Name,ProductCount,ImageCount,Price,IsDefault,IsApproval,Description,RankMin,RankMax ");
			strSql.Append(" FROM Shop_SupplierRank ");
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
			strSql.Append("select count(1) FROM Shop_SupplierRank ");
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
				strSql.Append("order by T.RankId desc");
			}
			strSql.Append(")AS Row, T.*  from Shop_SupplierRank T ");
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
			parameters[0].Value = "Shop_SupplierRank";
			parameters[1].Value = "RankId";
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
