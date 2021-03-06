﻿/**
* SalesItem.cs
*
* 功 能： N/A
* 类 名： SalesItem
*
* Ver    变更日期             负责人  变更内容
* ───────────────────────────────────
* V0.01  2013/6/8 18:54:18   N/A    初版
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
using YSWL.MALL.IDAL.Shop.Sales;
using YSWL.DBUtility;//Please add references
namespace YSWL.MALL.SQLServerDAL.Shop.Sales
{
	/// <summary>
	/// 数据访问类:SalesItem
	/// </summary>
	public partial class SalesItem:ISalesItem
	{
		public SalesItem()
		{}
		#region  BasicMethod

		/// <summary>
		/// 得到最大ID
		/// </summary>
		public int GetMaxId()
		{
		return DBHelper.DefaultDBHelper.GetMaxID("ItemId", "Shop_SalesItem"); 
		}

		/// <summary>
		/// 是否存在该记录
		/// </summary>
		public bool Exists(int ItemId)
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("select count(1) from Shop_SalesItem");
			strSql.Append(" where ItemId=@ItemId");
			SqlParameter[] parameters = {
					new SqlParameter("@ItemId", SqlDbType.Int,4)
			};
			parameters[0].Value = ItemId;

			return DBHelper.DefaultDBHelper.Exists(strSql.ToString(),parameters);
		}


		/// <summary>
		/// 增加一条数据
		/// </summary>
		public int Add(YSWL.MALL.Model.Shop.Sales.SalesItem model)
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("insert into Shop_SalesItem(");
			strSql.Append("RuleId,ItemType,UnitValue,RateValue)");
			strSql.Append(" values (");
			strSql.Append("@RuleId,@ItemType,@UnitValue,@RateValue)");
			strSql.Append(";select @@IDENTITY");
			SqlParameter[] parameters = {
					new SqlParameter("@RuleId", SqlDbType.Int,4),
					new SqlParameter("@ItemType", SqlDbType.Int,4),
					new SqlParameter("@UnitValue", SqlDbType.Int,4),
					new SqlParameter("@RateValue", SqlDbType.Int,4)};
			parameters[0].Value = model.RuleId;
			parameters[1].Value = model.ItemType;
			parameters[2].Value = model.UnitValue;
			parameters[3].Value = model.RateValue;

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
		public bool Update(YSWL.MALL.Model.Shop.Sales.SalesItem model)
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("update Shop_SalesItem set ");
			strSql.Append("RuleId=@RuleId,");
			strSql.Append("ItemType=@ItemType,");
			strSql.Append("UnitValue=@UnitValue,");
			strSql.Append("RateValue=@RateValue");
			strSql.Append(" where ItemId=@ItemId");
			SqlParameter[] parameters = {
					new SqlParameter("@RuleId", SqlDbType.Int,4),
					new SqlParameter("@ItemType", SqlDbType.Int,4),
					new SqlParameter("@UnitValue", SqlDbType.Int,4),
					new SqlParameter("@RateValue", SqlDbType.Int,4),
					new SqlParameter("@ItemId", SqlDbType.Int,4)};
			parameters[0].Value = model.RuleId;
			parameters[1].Value = model.ItemType;
			parameters[2].Value = model.UnitValue;
			parameters[3].Value = model.RateValue;
			parameters[4].Value = model.ItemId;

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
		public bool Delete(int ItemId)
		{
			
			StringBuilder strSql=new StringBuilder();
			strSql.Append("delete from Shop_SalesItem ");
			strSql.Append(" where ItemId=@ItemId");
			SqlParameter[] parameters = {
					new SqlParameter("@ItemId", SqlDbType.Int,4)
			};
			parameters[0].Value = ItemId;

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
		public bool DeleteList(string ItemIdlist )
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("delete from Shop_SalesItem ");
			strSql.Append(" where ItemId in ("+ItemIdlist + ")  ");
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
		public YSWL.MALL.Model.Shop.Sales.SalesItem GetModel(int ItemId)
		{
			
			StringBuilder strSql=new StringBuilder();
			strSql.Append("select  top 1 ItemId,RuleId,ItemType,UnitValue,RateValue from Shop_SalesItem ");
			strSql.Append(" where ItemId=@ItemId");
			SqlParameter[] parameters = {
					new SqlParameter("@ItemId", SqlDbType.Int,4)
			};
			parameters[0].Value = ItemId;

			YSWL.MALL.Model.Shop.Sales.SalesItem model=new YSWL.MALL.Model.Shop.Sales.SalesItem();
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
		public YSWL.MALL.Model.Shop.Sales.SalesItem DataRowToModel(DataRow row)
		{
			YSWL.MALL.Model.Shop.Sales.SalesItem model=new YSWL.MALL.Model.Shop.Sales.SalesItem();
			if (row != null)
			{
				if(row["ItemId"]!=null && row["ItemId"].ToString()!="")
				{
					model.ItemId=int.Parse(row["ItemId"].ToString());
				}
				if(row["RuleId"]!=null && row["RuleId"].ToString()!="")
				{
					model.RuleId=int.Parse(row["RuleId"].ToString());
				}
				if(row["ItemType"]!=null && row["ItemType"].ToString()!="")
				{
					model.ItemType=int.Parse(row["ItemType"].ToString());
				}
				if(row["UnitValue"]!=null && row["UnitValue"].ToString()!="")
				{
					model.UnitValue=int.Parse(row["UnitValue"].ToString());
				}
				if(row["RateValue"]!=null && row["RateValue"].ToString()!="")
				{
					model.RateValue=int.Parse(row["RateValue"].ToString());
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
			strSql.Append("select ItemId,RuleId,ItemType,UnitValue,RateValue ");
			strSql.Append(" FROM Shop_SalesItem ");
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
			strSql.Append(" ItemId,RuleId,ItemType,UnitValue,RateValue ");
			strSql.Append(" FROM Shop_SalesItem ");
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
			strSql.Append("select count(1) FROM Shop_SalesItem ");
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
				strSql.Append("order by T.ItemId desc");
			}
			strSql.Append(")AS Row, T.*  from Shop_SalesItem T ");
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
			parameters[0].Value = "Shop_SalesItem";
			parameters[1].Value = "ItemId";
			parameters[2].Value = PageSize;
			parameters[3].Value = PageIndex;
			parameters[4].Value = 0;
			parameters[5].Value = 0;
			parameters[6].Value = strWhere;	
			return DBHelper.DefaultDBHelper.RunProcedure("UP_GetRecordByPage",parameters,"ds");
		}*/

		#endregion  BasicMethod
		#region  ExtensionMethod

	    public bool DeleteByRuleId(int ruleId)
	    {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("delete from Shop_SalesItem ");
            strSql.Append(" where RuleId=@RuleId ");
            SqlParameter[] parameters = {
					new SqlParameter("@RuleId", SqlDbType.Int,4)			};
            parameters[0].Value = ruleId;

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

        public Model.Shop.Sales.SalesItem GetLowesSales(int ruleId)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select  top 1 ItemId,RuleId,ItemType,UnitValue,RateValue from Shop_SalesItem ");
            strSql.Append(" where RuleId=@RuleId order by RateValue asc");
            SqlParameter[] parameters = {
					new SqlParameter("@RuleId", SqlDbType.Int,4)
			};
            parameters[0].Value = ruleId;

            YSWL.MALL.Model.Shop.Sales.SalesItem model = new YSWL.MALL.Model.Shop.Sales.SalesItem();
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
	    #endregion  ExtensionMethod


   
    }
}

