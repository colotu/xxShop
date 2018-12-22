/**  版本信息模板在安装目录下，可自行修改。
* SuppDistRegion.cs
*
* 功 能： N/A
* 类 名： SuppDistRegion
*
* Ver    变更日期             负责人  变更内容
* ───────────────────────────────────
* V0.01  2014/1/26 18:31:53   N/A    初版
*
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
using YSWL.MALL.IDAL.Shop.Distribution;
using YSWL.DBUtility;//Please add references
namespace YSWL.MALL.SQLServerDAL.Shop.Distribution
{
	/// <summary>
	/// 数据访问类:SuppDistRegion
	/// </summary>
	public partial class SuppDistRegion:ISuppDistRegion
	{
		public SuppDistRegion()
		{}
		#region  BasicMethod

		/// <summary>
		/// 得到最大ID
		/// </summary>
		public int GetMaxId()
		{
		return DBHelper.DefaultDBHelper.GetMaxID("SupplierId", "Shop_SuppDistRegion"); 
		}

		/// <summary>
		/// 是否存在该记录
		/// </summary>
		public bool Exists(int SupplierId,int RegionId)
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("select count(1) from Shop_SuppDistRegion");
			strSql.Append(" where SupplierId=@SupplierId and RegionId=@RegionId ");
			SqlParameter[] parameters = {
					new SqlParameter("@SupplierId", SqlDbType.Int,4),
					new SqlParameter("@RegionId", SqlDbType.Int,4)			};
			parameters[0].Value = SupplierId;
			parameters[1].Value = RegionId;

			return DBHelper.DefaultDBHelper.Exists(strSql.ToString(),parameters);
		}


		/// <summary>
		/// 增加一条数据
		/// </summary>
		public bool Add(YSWL.MALL.Model.Shop.Distribution.SuppDistRegion model)
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("insert into Shop_SuppDistRegion(");
			strSql.Append("SupplierId,RegionId)");
			strSql.Append(" values (");
			strSql.Append("@SupplierId,@RegionId)");
			SqlParameter[] parameters = {
					new SqlParameter("@SupplierId", SqlDbType.Int,4),
					new SqlParameter("@RegionId", SqlDbType.Int,4)};
			parameters[0].Value = model.SupplierId;
			parameters[1].Value = model.RegionId;

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
		public bool Update(YSWL.MALL.Model.Shop.Distribution.SuppDistRegion model)
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("update Shop_SuppDistRegion set ");
			strSql.Append("SupplierId=@SupplierId,");
			strSql.Append("RegionId=@RegionId");
			strSql.Append(" where SupplierId=@SupplierId and RegionId=@RegionId ");
			SqlParameter[] parameters = {
					new SqlParameter("@SupplierId", SqlDbType.Int,4),
					new SqlParameter("@RegionId", SqlDbType.Int,4)};
			parameters[0].Value = model.SupplierId;
			parameters[1].Value = model.RegionId;

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
		public bool Delete(int SupplierId,int RegionId)
		{
			
			StringBuilder strSql=new StringBuilder();
			strSql.Append("delete from Shop_SuppDistRegion ");
			strSql.Append(" where SupplierId=@SupplierId and RegionId=@RegionId ");
			SqlParameter[] parameters = {
					new SqlParameter("@SupplierId", SqlDbType.Int,4),
					new SqlParameter("@RegionId", SqlDbType.Int,4)			};
			parameters[0].Value = SupplierId;
			parameters[1].Value = RegionId;

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
		public YSWL.MALL.Model.Shop.Distribution.SuppDistRegion GetModel(int SupplierId,int RegionId)
		{
			
			StringBuilder strSql=new StringBuilder();
			strSql.Append("select  top 1 SupplierId,RegionId from Shop_SuppDistRegion ");
			strSql.Append(" where SupplierId=@SupplierId and RegionId=@RegionId ");
			SqlParameter[] parameters = {
					new SqlParameter("@SupplierId", SqlDbType.Int,4),
					new SqlParameter("@RegionId", SqlDbType.Int,4)			};
			parameters[0].Value = SupplierId;
			parameters[1].Value = RegionId;

			YSWL.MALL.Model.Shop.Distribution.SuppDistRegion model=new YSWL.MALL.Model.Shop.Distribution.SuppDistRegion();
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
		public YSWL.MALL.Model.Shop.Distribution.SuppDistRegion DataRowToModel(DataRow row)
		{
			YSWL.MALL.Model.Shop.Distribution.SuppDistRegion model=new YSWL.MALL.Model.Shop.Distribution.SuppDistRegion();
			if (row != null)
			{
				if(row["SupplierId"]!=null && row["SupplierId"].ToString()!="")
				{
					model.SupplierId=int.Parse(row["SupplierId"].ToString());
				}
				if(row["RegionId"]!=null && row["RegionId"].ToString()!="")
				{
					model.RegionId=int.Parse(row["RegionId"].ToString());
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
			strSql.Append("select SupplierId,RegionId ");
			strSql.Append(" FROM Shop_SuppDistRegion ");
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
			strSql.Append(" SupplierId,RegionId ");
			strSql.Append(" FROM Shop_SuppDistRegion ");
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
			strSql.Append("select count(1) FROM Shop_SuppDistRegion ");
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
				strSql.Append("order by T.RegionId desc");
			}
			strSql.Append(")AS Row, T.*  from Shop_SuppDistRegion T ");
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
			parameters[0].Value = "Shop_SuppDistRegion";
			parameters[1].Value = "RegionId";
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

