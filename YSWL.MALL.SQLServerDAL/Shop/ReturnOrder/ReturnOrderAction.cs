/**  版本信息模板在安装目录下，可自行修改。
* ReturnOrderAction.cs
*
* 功 能： N/A
* 类 名： ReturnOrderAction
*
* Ver    变更日期             负责人  变更内容
* ───────────────────────────────────
* V0.01  2014/7/2 11:50:35   N/A    初版 
* 负责人   [hhy]
* Copyright (c) 2012 YS56 Corporation. All rights reserved.
*┌──────────────────────────────────┐
*│　此技术信息为本公司机密信息，未经本公司书面同意禁止向第三方披露．　│
*│　版权所有：云商未来（北京）科技有限公司　　　　　　　　　　　　　　│
*└──────────────────────────────────┘
*/
using System;
using System.Data;
using System.Text;
using System.Data.SqlClient;
using YSWL.MALL.IDAL.Shop.ReturnOrder;
using YSWL.DBUtility;//Please add references
namespace YSWL.MALL.SQLServerDAL.Shop.ReturnOrder
{
	/// <summary>
	/// 数据访问类:ReturnOrderAction
	/// </summary>
	public partial class ReturnOrderAction:IReturnOrderAction
	{
		public ReturnOrderAction()
		{}
		#region  BasicMethod

		/// <summary>
		/// 是否存在该记录
		/// </summary>
		public bool Exists(long ActionId)
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("select count(1) from Shop_ReturnOrderAction");
			strSql.Append(" where ActionId=@ActionId");
			SqlParameter[] parameters = {
					new SqlParameter("@ActionId", SqlDbType.BigInt)
			};
			parameters[0].Value = ActionId;

			return DBHelper.DefaultDBHelper.Exists(strSql.ToString(),parameters);
		}


		/// <summary>
		/// 增加一条数据
		/// </summary>
		public long Add(YSWL.MALL.Model.Shop.ReturnOrder.ReturnOrderAction model)
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("insert into Shop_ReturnOrderAction(");
			strSql.Append("ReturnOrderId,ReturnOrderCode,UserId,UserName,ActionCode,ActionDate,Remark)");
			strSql.Append(" values (");
			strSql.Append("@ReturnOrderId,@ReturnOrderCode,@UserId,@UserName,@ActionCode,@ActionDate,@Remark)");
			strSql.Append(";select @@IDENTITY");
			SqlParameter[] parameters = {
					new SqlParameter("@ReturnOrderId", SqlDbType.BigInt,8),
					new SqlParameter("@ReturnOrderCode", SqlDbType.NVarChar,50),
					new SqlParameter("@UserId", SqlDbType.Int,4),
					new SqlParameter("@UserName", SqlDbType.NVarChar,200),
					new SqlParameter("@ActionCode", SqlDbType.NVarChar,100),
					new SqlParameter("@ActionDate", SqlDbType.DateTime),
					new SqlParameter("@Remark", SqlDbType.NVarChar,1000)};
			parameters[0].Value = model.ReturnOrderId;
			parameters[1].Value = model.ReturnOrderCode;
			parameters[2].Value = model.UserId;
			parameters[3].Value = model.UserName;
			parameters[4].Value = model.ActionCode;
			parameters[5].Value = model.ActionDate;
			parameters[6].Value = model.Remark;

			object obj = DBHelper.DefaultDBHelper.GetSingle(strSql.ToString(),parameters);
			if (obj == null)
			{
				return 0;
			}
			else
			{
				return Convert.ToInt64(obj);
			}
		}
		/// <summary>
		/// 更新一条数据
		/// </summary>
		public bool Update(YSWL.MALL.Model.Shop.ReturnOrder.ReturnOrderAction model)
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("update Shop_ReturnOrderAction set ");
			strSql.Append("ReturnOrderId=@ReturnOrderId,");
			strSql.Append("ReturnOrderCode=@ReturnOrderCode,");
			strSql.Append("UserId=@UserId,");
			strSql.Append("UserName=@UserName,");
			strSql.Append("ActionCode=@ActionCode,");
			strSql.Append("ActionDate=@ActionDate,");
			strSql.Append("Remark=@Remark");
			strSql.Append(" where ActionId=@ActionId");
			SqlParameter[] parameters = {
					new SqlParameter("@ReturnOrderId", SqlDbType.BigInt,8),
					new SqlParameter("@ReturnOrderCode", SqlDbType.NVarChar,50),
					new SqlParameter("@UserId", SqlDbType.Int,4),
					new SqlParameter("@UserName", SqlDbType.NVarChar,200),
					new SqlParameter("@ActionCode", SqlDbType.NVarChar,100),
					new SqlParameter("@ActionDate", SqlDbType.DateTime),
					new SqlParameter("@Remark", SqlDbType.NVarChar,1000),
					new SqlParameter("@ActionId", SqlDbType.BigInt,8)};
			parameters[0].Value = model.ReturnOrderId;
			parameters[1].Value = model.ReturnOrderCode;
			parameters[2].Value = model.UserId;
			parameters[3].Value = model.UserName;
			parameters[4].Value = model.ActionCode;
			parameters[5].Value = model.ActionDate;
			parameters[6].Value = model.Remark;
			parameters[7].Value = model.ActionId;

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
		public bool Delete(long ActionId)
		{
			
			StringBuilder strSql=new StringBuilder();
			strSql.Append("delete from Shop_ReturnOrderAction ");
			strSql.Append(" where ActionId=@ActionId");
			SqlParameter[] parameters = {
					new SqlParameter("@ActionId", SqlDbType.BigInt)
			};
			parameters[0].Value = ActionId;

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
		public bool DeleteList(string ActionIdlist )
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("delete from Shop_ReturnOrderAction ");
			strSql.Append(" where ActionId in ("+ActionIdlist + ")  ");
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
		public YSWL.MALL.Model.Shop.ReturnOrder.ReturnOrderAction GetModel(long ActionId)
		{
			
			StringBuilder strSql=new StringBuilder();
			strSql.Append("select  top 1 ActionId,ReturnOrderId,ReturnOrderCode,UserId,UserName,ActionCode,ActionDate,Remark from Shop_ReturnOrderAction ");
			strSql.Append(" where ActionId=@ActionId");
			SqlParameter[] parameters = {
					new SqlParameter("@ActionId", SqlDbType.BigInt)
			};
			parameters[0].Value = ActionId;

			YSWL.MALL.Model.Shop.ReturnOrder.ReturnOrderAction model=new YSWL.MALL.Model.Shop.ReturnOrder.ReturnOrderAction();
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
		public YSWL.MALL.Model.Shop.ReturnOrder.ReturnOrderAction DataRowToModel(DataRow row)
		{
			YSWL.MALL.Model.Shop.ReturnOrder.ReturnOrderAction model=new YSWL.MALL.Model.Shop.ReturnOrder.ReturnOrderAction();
			if (row != null)
			{
				if(row["ActionId"]!=null && row["ActionId"].ToString()!="")
				{
					model.ActionId=long.Parse(row["ActionId"].ToString());
				}
				if(row["ReturnOrderId"]!=null && row["ReturnOrderId"].ToString()!="")
				{
					model.ReturnOrderId=long.Parse(row["ReturnOrderId"].ToString());
				}
				if(row["ReturnOrderCode"]!=null)
				{
					model.ReturnOrderCode=row["ReturnOrderCode"].ToString();
				}
				if(row["UserId"]!=null && row["UserId"].ToString()!="")
				{
					model.UserId=int.Parse(row["UserId"].ToString());
				}
				if(row["UserName"]!=null)
				{
					model.UserName=row["UserName"].ToString();
				}
				if(row["ActionCode"]!=null)
				{
					model.ActionCode=row["ActionCode"].ToString();
				}
				if(row["ActionDate"]!=null && row["ActionDate"].ToString()!="")
				{
					model.ActionDate=DateTime.Parse(row["ActionDate"].ToString());
				}
				if(row["Remark"]!=null)
				{
					model.Remark=row["Remark"].ToString();
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
			strSql.Append("select ActionId,ReturnOrderId,ReturnOrderCode,UserId,UserName,ActionCode,ActionDate,Remark ");
			strSql.Append(" FROM Shop_ReturnOrderAction ");
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
			strSql.Append(" ActionId,ReturnOrderId,ReturnOrderCode,UserId,UserName,ActionCode,ActionDate,Remark ");
			strSql.Append(" FROM Shop_ReturnOrderAction ");
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
			strSql.Append("select count(1) FROM Shop_ReturnOrderAction ");
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
				strSql.Append("order by T.ActionId desc");
			}
			strSql.Append(")AS Row, T.*  from Shop_ReturnOrderAction T ");
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
			parameters[0].Value = "Shop_ReturnOrderAction";
			parameters[1].Value = "ActionId";
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

