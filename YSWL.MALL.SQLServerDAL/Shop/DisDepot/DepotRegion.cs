/**  版本信息模板在安装目录下，可自行修改。
* DepotRegion.cs
*
* 功 能： N/A
* 类 名： DepotRegion
*
* Ver    变更日期             负责人  变更内容
* ───────────────────────────────────
* V0.01  2015/7/27 17:36:56   N/A    初版
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
using YSWL.MALL.IDAL.Shop.DisDepot;
using YSWL.DBUtility;//Please add references
namespace YSWL.MALL.SQLServerDAL.Shop.DisDepot
{
	/// <summary>
	/// 数据访问类:DepotRegion
	/// </summary>
	public partial class DepotRegion:IDepotRegion
	{
		public DepotRegion()
		{}
		#region  BasicMethod

		/// <summary>
		/// 得到最大ID
		/// </summary>
		public int GetMaxId()
		{
		return DBHelper.DefaultDBHelper.GetMaxID("DepotId", "OMS_DepotRegion"); 
		}

		/// <summary>
		/// 是否存在该记录
		/// </summary>
		public bool Exists(int DepotId,int RegionId)
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("select count(1) from OMS_DepotRegion");
			strSql.Append(" where DepotId=@DepotId and RegionId=@RegionId ");
			SqlParameter[] parameters = {
					new SqlParameter("@DepotId", SqlDbType.Int,4),
					new SqlParameter("@RegionId", SqlDbType.Int,4)			};
			parameters[0].Value = DepotId;
			parameters[1].Value = RegionId;

			return DBHelper.DefaultDBHelper.Exists(strSql.ToString(),parameters);
		}


		/// <summary>
		/// 增加一条数据
		/// </summary>
		public bool Add(YSWL.MALL.Model.Shop.DisDepot.DepotRegion model)
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("insert into OMS_DepotRegion(");
			strSql.Append("DepotId,RegionId,RegionName,Status,Path,Depth)");
			strSql.Append(" values (");
			strSql.Append("@DepotId,@RegionId,@RegionName,@Status,@Path,@Depth)");
			SqlParameter[] parameters = {
					new SqlParameter("@DepotId", SqlDbType.Int,4),
					new SqlParameter("@RegionId", SqlDbType.Int,4),
					new SqlParameter("@RegionName", SqlDbType.NVarChar,50),
					new SqlParameter("@Status", SqlDbType.Bit,1),
					new SqlParameter("@Path", SqlDbType.NVarChar,4000),
					new SqlParameter("@Depth", SqlDbType.Int,4)};
			parameters[0].Value = model.DepotId;
			parameters[1].Value = model.RegionId;
			parameters[2].Value = model.RegionName;
			parameters[3].Value = model.Status;
			parameters[4].Value = model.Path;
			parameters[5].Value = model.Depth;

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
		public bool Update(YSWL.MALL.Model.Shop.DisDepot.DepotRegion model)
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("update OMS_DepotRegion set ");
			strSql.Append("RegionName=@RegionName,");
			strSql.Append("Status=@Status,");
			strSql.Append("Path=@Path,");
			strSql.Append("Depth=@Depth");
			strSql.Append(" where DepotId=@DepotId and RegionId=@RegionId ");
			SqlParameter[] parameters = {
					new SqlParameter("@RegionName", SqlDbType.NVarChar,50),
					new SqlParameter("@Status", SqlDbType.Bit,1),
					new SqlParameter("@Path", SqlDbType.NVarChar,4000),
					new SqlParameter("@Depth", SqlDbType.Int,4),
					new SqlParameter("@DepotId", SqlDbType.Int,4),
					new SqlParameter("@RegionId", SqlDbType.Int,4)};
			parameters[0].Value = model.RegionName;
			parameters[1].Value = model.Status;
			parameters[2].Value = model.Path;
			parameters[3].Value = model.Depth;
			parameters[4].Value = model.DepotId;
			parameters[5].Value = model.RegionId;

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
		public bool Delete(int DepotId,int RegionId)
		{
			
			StringBuilder strSql=new StringBuilder();
			strSql.Append("delete from OMS_DepotRegion ");
			strSql.Append(" where DepotId=@DepotId and RegionId=@RegionId ");
			SqlParameter[] parameters = {
					new SqlParameter("@DepotId", SqlDbType.Int,4),
					new SqlParameter("@RegionId", SqlDbType.Int,4)			};
			parameters[0].Value = DepotId;
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
		public YSWL.MALL.Model.Shop.DisDepot.DepotRegion GetModel(int DepotId,int RegionId)
		{
			
			StringBuilder strSql=new StringBuilder();
			strSql.Append("select  top 1 DepotId,RegionId,RegionName,Status,Path,Depth from OMS_DepotRegion ");
			strSql.Append(" where DepotId=@DepotId and RegionId=@RegionId ");
			SqlParameter[] parameters = {
					new SqlParameter("@DepotId", SqlDbType.Int,4),
					new SqlParameter("@RegionId", SqlDbType.Int,4)			};
			parameters[0].Value = DepotId;
			parameters[1].Value = RegionId;

			YSWL.MALL.Model.Shop.DisDepot.DepotRegion model=new YSWL.MALL.Model.Shop.DisDepot.DepotRegion();
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
		public YSWL.MALL.Model.Shop.DisDepot.DepotRegion DataRowToModel(DataRow row)
		{
			YSWL.MALL.Model.Shop.DisDepot.DepotRegion model=new YSWL.MALL.Model.Shop.DisDepot.DepotRegion();
			if (row != null)
			{
				if(row["DepotId"]!=null && row["DepotId"].ToString()!="")
				{
					model.DepotId=int.Parse(row["DepotId"].ToString());
				}
				if(row["RegionId"]!=null && row["RegionId"].ToString()!="")
				{
					model.RegionId=int.Parse(row["RegionId"].ToString());
				}
				if(row["RegionName"]!=null)
				{
					model.RegionName=row["RegionName"].ToString();
				}
				if(row["Status"]!=null && row["Status"].ToString()!="")
				{
					if((row["Status"].ToString()=="1")||(row["Status"].ToString().ToLower()=="true"))
					{
						model.Status=true;
					}
					else
					{
						model.Status=false;
					}
				}
				if(row["Path"]!=null)
				{
					model.Path=row["Path"].ToString();
				}
				if(row["Depth"]!=null && row["Depth"].ToString()!="")
				{
					model.Depth=int.Parse(row["Depth"].ToString());
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
			strSql.Append("select DepotId,RegionId,RegionName,Status,Path,Depth ");
			strSql.Append(" FROM OMS_DepotRegion ");
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
			strSql.Append(" DepotId,RegionId,RegionName,Status,Path,Depth ");
			strSql.Append(" FROM OMS_DepotRegion ");
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
			strSql.Append("select count(1) FROM OMS_DepotRegion ");
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
			strSql.Append(")AS Row, T.*  from OMS_DepotRegion T ");
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
			parameters[0].Value = "OMS_DepotRegion";
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
        /// <summary>
        /// 同步仓库库存
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public bool SyncDepotRegion(YSWL.MALL.Model.Shop.DisDepot.DepotRegion model)
	    {
            StringBuilder strSql = new StringBuilder();
            //先删除 然后在添加数据
            strSql.Append("delete from OMS_DepotRegion ");
            strSql.Append(" where DepotId=@DepotId and RegionId=@RegionId ; ");

            strSql.Append("insert into OMS_DepotRegion(");
            strSql.Append("DepotId,RegionId,RegionName,Status,Path,Depth)");
            strSql.Append(" values (");
            strSql.Append("@DepotId,@RegionId,@RegionName,@Status,@Path,@Depth)");
            SqlParameter[] parameters = {
					new SqlParameter("@DepotId", SqlDbType.Int,4),
					new SqlParameter("@RegionId", SqlDbType.Int,4),
					new SqlParameter("@RegionName", SqlDbType.NVarChar,50),
					new SqlParameter("@Status", SqlDbType.Bit,1),
					new SqlParameter("@Path", SqlDbType.NVarChar,4000),
					new SqlParameter("@Depth", SqlDbType.Int,4)};
            parameters[0].Value = model.DepotId;
            parameters[1].Value = model.RegionId;
            parameters[2].Value = model.RegionName;
            parameters[3].Value = model.Status;
            parameters[4].Value = model.Path;
            parameters[5].Value = model.Depth;

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
     

	    #endregion  ExtensionMethod
	}
}

