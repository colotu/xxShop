/**
* Trials.cs
*
* 功 能： N/A
* 类 名： Trials
*
* Ver    变更日期             负责人  变更内容
* ───────────────────────────────────
* V0.01  2013/5/22 17:39:52   N/A    初版
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
using YSWL.MALL.IDAL.Shop.Trial;
using YSWL.DBUtility;//Please add references
namespace YSWL.MALL.SQLServerDAL.Shop.Trial
{
	/// <summary>
	/// 数据访问类:Trials
	/// </summary>
	public partial class Trials:ITrials
	{
		public Trials()
		{}
		#region  BasicMethod

		/// <summary>
		/// 得到最大ID
		/// </summary>
		public int GetMaxId()
		{
		return DBHelper.DefaultDBHelper.GetMaxID("TrialId", "Shop_Trials"); 
		}

		/// <summary>
		/// 是否存在该记录
		/// </summary>
		public bool Exists(int TrialId)
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("select count(1) from Shop_Trials");
			strSql.Append(" where TrialId=@TrialId");
			SqlParameter[] parameters = {
					new SqlParameter("@TrialId", SqlDbType.Int,4)
			};
			parameters[0].Value = TrialId;

			return DBHelper.DefaultDBHelper.Exists(strSql.ToString(),parameters);
		}


		/// <summary>
		/// 增加一条数据
		/// </summary>
		public int Add(YSWL.MALL.Model.Shop.Trial.Trials model)
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("insert into Shop_Trials(");
			strSql.Append("CategoryId,TrialName,EnterpriseId,RegionId,ShortDescription,Unit,Description,Meta_Title,Meta_Description,Meta_Keywords,LinklUrl,TrialStatus,StartDate,EndDate,CreatedDate,CreatedUserID,VistiCounts,TrialCounts,DisplaySequence,MarketPrice,LowestSalePrice,MainCategoryPath,ExtendCategoryPath,Points,ImageUrl,ThumbnailUrl,MaxQuantity,MinQuantity,Tags,SeoUrl,SeoImageAlt,SeoImageTitle)");
			strSql.Append(" values (");
			strSql.Append("@CategoryId,@TrialName,@EnterpriseId,@RegionId,@ShortDescription,@Unit,@Description,@Meta_Title,@Meta_Description,@Meta_Keywords,@LinklUrl,@TrialStatus,@StartDate,@EndDate,@CreatedDate,@CreatedUserID,@VistiCounts,@TrialCounts,@DisplaySequence,@MarketPrice,@LowestSalePrice,@MainCategoryPath,@ExtendCategoryPath,@Points,@ImageUrl,@ThumbnailUrl,@MaxQuantity,@MinQuantity,@Tags,@SeoUrl,@SeoImageAlt,@SeoImageTitle)");
			strSql.Append(";select @@IDENTITY");
			SqlParameter[] parameters = {
					new SqlParameter("@CategoryId", SqlDbType.Int,4),
					new SqlParameter("@TrialName", SqlDbType.NVarChar,200),
					new SqlParameter("@EnterpriseId", SqlDbType.Int,4),
					new SqlParameter("@RegionId", SqlDbType.Int,4),
					new SqlParameter("@ShortDescription", SqlDbType.NVarChar,2000),
					new SqlParameter("@Unit", SqlDbType.NVarChar,50),
					new SqlParameter("@Description", SqlDbType.NText),
					new SqlParameter("@Meta_Title", SqlDbType.NVarChar,100),
					new SqlParameter("@Meta_Description", SqlDbType.NVarChar,1000),
					new SqlParameter("@Meta_Keywords", SqlDbType.NVarChar,1000),
					new SqlParameter("@LinklUrl", SqlDbType.NVarChar,500),
					new SqlParameter("@TrialStatus", SqlDbType.SmallInt,2),
					new SqlParameter("@StartDate", SqlDbType.DateTime),
					new SqlParameter("@EndDate", SqlDbType.DateTime),
					new SqlParameter("@CreatedDate", SqlDbType.DateTime),
					new SqlParameter("@CreatedUserID", SqlDbType.Int,4),
					new SqlParameter("@VistiCounts", SqlDbType.Int,4),
					new SqlParameter("@TrialCounts", SqlDbType.Int,4),
					new SqlParameter("@DisplaySequence", SqlDbType.Int,4),
					new SqlParameter("@MarketPrice", SqlDbType.Money,8),
					new SqlParameter("@LowestSalePrice", SqlDbType.Money,8),
					new SqlParameter("@MainCategoryPath", SqlDbType.NVarChar,256),
					new SqlParameter("@ExtendCategoryPath", SqlDbType.NVarChar,256),
					new SqlParameter("@Points", SqlDbType.Decimal,9),
					new SqlParameter("@ImageUrl", SqlDbType.NVarChar,255),
					new SqlParameter("@ThumbnailUrl", SqlDbType.NVarChar,255),
					new SqlParameter("@MaxQuantity", SqlDbType.Int,4),
					new SqlParameter("@MinQuantity", SqlDbType.Int,4),
					new SqlParameter("@Tags", SqlDbType.NVarChar,50),
					new SqlParameter("@SeoUrl", SqlDbType.NVarChar,300),
					new SqlParameter("@SeoImageAlt", SqlDbType.NVarChar,300),
					new SqlParameter("@SeoImageTitle", SqlDbType.NVarChar,300)};
			parameters[0].Value = model.CategoryId;
			parameters[1].Value = model.TrialName;
			parameters[2].Value = model.EnterpriseId;
			parameters[3].Value = model.RegionId;
			parameters[4].Value = model.ShortDescription;
			parameters[5].Value = model.Unit;
			parameters[6].Value = model.Description;
			parameters[7].Value = model.Meta_Title;
			parameters[8].Value = model.Meta_Description;
			parameters[9].Value = model.Meta_Keywords;
			parameters[10].Value = model.LinklUrl;
			parameters[11].Value = model.TrialStatus;
			parameters[12].Value = model.StartDate;
			parameters[13].Value = model.EndDate;
			parameters[14].Value = model.CreatedDate;
			parameters[15].Value = model.CreatedUserID;
			parameters[16].Value = model.VistiCounts;
			parameters[17].Value = model.TrialCounts;
			parameters[18].Value = model.DisplaySequence;
			parameters[19].Value = model.MarketPrice;
			parameters[20].Value = model.LowestSalePrice;
			parameters[21].Value = model.MainCategoryPath;
			parameters[22].Value = model.ExtendCategoryPath;
			parameters[23].Value = model.Points;
			parameters[24].Value = model.ImageUrl;
			parameters[25].Value = model.ThumbnailUrl;
			parameters[26].Value = model.MaxQuantity;
			parameters[27].Value = model.MinQuantity;
			parameters[28].Value = model.Tags;
			parameters[29].Value = model.SeoUrl;
			parameters[30].Value = model.SeoImageAlt;
			parameters[31].Value = model.SeoImageTitle;

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
		public bool Update(YSWL.MALL.Model.Shop.Trial.Trials model)
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("update Shop_Trials set ");
			strSql.Append("CategoryId=@CategoryId,");
			strSql.Append("TrialName=@TrialName,");
			strSql.Append("EnterpriseId=@EnterpriseId,");
			strSql.Append("RegionId=@RegionId,");
			strSql.Append("ShortDescription=@ShortDescription,");
			strSql.Append("Unit=@Unit,");
			strSql.Append("Description=@Description,");
			strSql.Append("Meta_Title=@Meta_Title,");
			strSql.Append("Meta_Description=@Meta_Description,");
			strSql.Append("Meta_Keywords=@Meta_Keywords,");
			strSql.Append("LinklUrl=@LinklUrl,");
			strSql.Append("TrialStatus=@TrialStatus,");
			strSql.Append("StartDate=@StartDate,");
			strSql.Append("EndDate=@EndDate,");
			strSql.Append("CreatedDate=@CreatedDate,");
			strSql.Append("CreatedUserID=@CreatedUserID,");
			strSql.Append("VistiCounts=@VistiCounts,");
			strSql.Append("TrialCounts=@TrialCounts,");
			strSql.Append("DisplaySequence=@DisplaySequence,");
			strSql.Append("MarketPrice=@MarketPrice,");
			strSql.Append("LowestSalePrice=@LowestSalePrice,");
			strSql.Append("MainCategoryPath=@MainCategoryPath,");
			strSql.Append("ExtendCategoryPath=@ExtendCategoryPath,");
			strSql.Append("Points=@Points,");
			strSql.Append("ImageUrl=@ImageUrl,");
			strSql.Append("ThumbnailUrl=@ThumbnailUrl,");
			strSql.Append("MaxQuantity=@MaxQuantity,");
			strSql.Append("MinQuantity=@MinQuantity,");
			strSql.Append("Tags=@Tags,");
			strSql.Append("SeoUrl=@SeoUrl,");
			strSql.Append("SeoImageAlt=@SeoImageAlt,");
			strSql.Append("SeoImageTitle=@SeoImageTitle");
			strSql.Append(" where TrialId=@TrialId");
			SqlParameter[] parameters = {
					new SqlParameter("@CategoryId", SqlDbType.Int,4),
					new SqlParameter("@TrialName", SqlDbType.NVarChar,200),
					new SqlParameter("@EnterpriseId", SqlDbType.Int,4),
					new SqlParameter("@RegionId", SqlDbType.Int,4),
					new SqlParameter("@ShortDescription", SqlDbType.NVarChar,2000),
					new SqlParameter("@Unit", SqlDbType.NVarChar,50),
					new SqlParameter("@Description", SqlDbType.NText),
					new SqlParameter("@Meta_Title", SqlDbType.NVarChar,100),
					new SqlParameter("@Meta_Description", SqlDbType.NVarChar,1000),
					new SqlParameter("@Meta_Keywords", SqlDbType.NVarChar,1000),
					new SqlParameter("@LinklUrl", SqlDbType.NVarChar,500),
					new SqlParameter("@TrialStatus", SqlDbType.SmallInt,2),
					new SqlParameter("@StartDate", SqlDbType.DateTime),
					new SqlParameter("@EndDate", SqlDbType.DateTime),
					new SqlParameter("@CreatedDate", SqlDbType.DateTime),
					new SqlParameter("@CreatedUserID", SqlDbType.Int,4),
					new SqlParameter("@VistiCounts", SqlDbType.Int,4),
					new SqlParameter("@TrialCounts", SqlDbType.Int,4),
					new SqlParameter("@DisplaySequence", SqlDbType.Int,4),
					new SqlParameter("@MarketPrice", SqlDbType.Money,8),
					new SqlParameter("@LowestSalePrice", SqlDbType.Money,8),
					new SqlParameter("@MainCategoryPath", SqlDbType.NVarChar,256),
					new SqlParameter("@ExtendCategoryPath", SqlDbType.NVarChar,256),
					new SqlParameter("@Points", SqlDbType.Decimal,9),
					new SqlParameter("@ImageUrl", SqlDbType.NVarChar,255),
					new SqlParameter("@ThumbnailUrl", SqlDbType.NVarChar,255),
					new SqlParameter("@MaxQuantity", SqlDbType.Int,4),
					new SqlParameter("@MinQuantity", SqlDbType.Int,4),
					new SqlParameter("@Tags", SqlDbType.NVarChar,50),
					new SqlParameter("@SeoUrl", SqlDbType.NVarChar,300),
					new SqlParameter("@SeoImageAlt", SqlDbType.NVarChar,300),
					new SqlParameter("@SeoImageTitle", SqlDbType.NVarChar,300),
					new SqlParameter("@TrialId", SqlDbType.Int,4)};
			parameters[0].Value = model.CategoryId;
			parameters[1].Value = model.TrialName;
			parameters[2].Value = model.EnterpriseId;
			parameters[3].Value = model.RegionId;
			parameters[4].Value = model.ShortDescription;
			parameters[5].Value = model.Unit;
			parameters[6].Value = model.Description;
			parameters[7].Value = model.Meta_Title;
			parameters[8].Value = model.Meta_Description;
			parameters[9].Value = model.Meta_Keywords;
			parameters[10].Value = model.LinklUrl;
			parameters[11].Value = model.TrialStatus;
			parameters[12].Value = model.StartDate;
			parameters[13].Value = model.EndDate;
			parameters[14].Value = model.CreatedDate;
			parameters[15].Value = model.CreatedUserID;
			parameters[16].Value = model.VistiCounts;
			parameters[17].Value = model.TrialCounts;
			parameters[18].Value = model.DisplaySequence;
			parameters[19].Value = model.MarketPrice;
			parameters[20].Value = model.LowestSalePrice;
			parameters[21].Value = model.MainCategoryPath;
			parameters[22].Value = model.ExtendCategoryPath;
			parameters[23].Value = model.Points;
			parameters[24].Value = model.ImageUrl;
			parameters[25].Value = model.ThumbnailUrl;
			parameters[26].Value = model.MaxQuantity;
			parameters[27].Value = model.MinQuantity;
			parameters[28].Value = model.Tags;
			parameters[29].Value = model.SeoUrl;
			parameters[30].Value = model.SeoImageAlt;
			parameters[31].Value = model.SeoImageTitle;
			parameters[32].Value = model.TrialId;

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
		public bool Delete(int TrialId)
		{
			
			StringBuilder strSql=new StringBuilder();
			strSql.Append("delete from Shop_Trials ");
			strSql.Append(" where TrialId=@TrialId");
			SqlParameter[] parameters = {
					new SqlParameter("@TrialId", SqlDbType.Int,4)
			};
			parameters[0].Value = TrialId;

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
		public bool DeleteList(string TrialIdlist )
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("delete from Shop_Trials ");
			strSql.Append(" where TrialId in ("+TrialIdlist + ")  ");
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
		public YSWL.MALL.Model.Shop.Trial.Trials GetModel(int TrialId)
		{
			
			StringBuilder strSql=new StringBuilder();
			strSql.Append("select  top 1 TrialId,CategoryId,TrialName,EnterpriseId,RegionId,ShortDescription,Unit,Description,Meta_Title,Meta_Description,Meta_Keywords,LinklUrl,TrialStatus,StartDate,EndDate,CreatedDate,CreatedUserID,VistiCounts,TrialCounts,DisplaySequence,MarketPrice,LowestSalePrice,MainCategoryPath,ExtendCategoryPath,Points,ImageUrl,ThumbnailUrl,MaxQuantity,MinQuantity,Tags,SeoUrl,SeoImageAlt,SeoImageTitle from Shop_Trials ");
			strSql.Append(" where TrialId=@TrialId");
			SqlParameter[] parameters = {
					new SqlParameter("@TrialId", SqlDbType.Int,4)
			};
			parameters[0].Value = TrialId;

			YSWL.MALL.Model.Shop.Trial.Trials model=new YSWL.MALL.Model.Shop.Trial.Trials();
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
		public YSWL.MALL.Model.Shop.Trial.Trials DataRowToModel(DataRow row)
		{
			YSWL.MALL.Model.Shop.Trial.Trials model=new YSWL.MALL.Model.Shop.Trial.Trials();
			if (row != null)
			{
				if(row["TrialId"]!=null && row["TrialId"].ToString()!="")
				{
					model.TrialId=int.Parse(row["TrialId"].ToString());
				}
				if(row["CategoryId"]!=null && row["CategoryId"].ToString()!="")
				{
					model.CategoryId=int.Parse(row["CategoryId"].ToString());
				}
				if(row["TrialName"]!=null)
				{
					model.TrialName=row["TrialName"].ToString();
				}
				if(row["EnterpriseId"]!=null && row["EnterpriseId"].ToString()!="")
				{
					model.EnterpriseId=int.Parse(row["EnterpriseId"].ToString());
				}
				if(row["RegionId"]!=null && row["RegionId"].ToString()!="")
				{
					model.RegionId=int.Parse(row["RegionId"].ToString());
				}
				if(row["ShortDescription"]!=null)
				{
					model.ShortDescription=row["ShortDescription"].ToString();
				}
				if(row["Unit"]!=null)
				{
					model.Unit=row["Unit"].ToString();
				}
				if(row["Description"]!=null)
				{
					model.Description=row["Description"].ToString();
				}
				if(row["Meta_Title"]!=null)
				{
					model.Meta_Title=row["Meta_Title"].ToString();
				}
				if(row["Meta_Description"]!=null)
				{
					model.Meta_Description=row["Meta_Description"].ToString();
				}
				if(row["Meta_Keywords"]!=null)
				{
					model.Meta_Keywords=row["Meta_Keywords"].ToString();
				}
				if(row["LinklUrl"]!=null)
				{
					model.LinklUrl=row["LinklUrl"].ToString();
				}
				if(row["TrialStatus"]!=null && row["TrialStatus"].ToString()!="")
				{
					model.TrialStatus=int.Parse(row["TrialStatus"].ToString());
				}
				if(row["StartDate"]!=null && row["StartDate"].ToString()!="")
				{
					model.StartDate=DateTime.Parse(row["StartDate"].ToString());
				}
				if(row["EndDate"]!=null && row["EndDate"].ToString()!="")
				{
					model.EndDate=DateTime.Parse(row["EndDate"].ToString());
				}
				if(row["CreatedDate"]!=null && row["CreatedDate"].ToString()!="")
				{
					model.CreatedDate=DateTime.Parse(row["CreatedDate"].ToString());
				}
				if(row["CreatedUserID"]!=null && row["CreatedUserID"].ToString()!="")
				{
					model.CreatedUserID=int.Parse(row["CreatedUserID"].ToString());
				}
				if(row["VistiCounts"]!=null && row["VistiCounts"].ToString()!="")
				{
					model.VistiCounts=int.Parse(row["VistiCounts"].ToString());
				}
				if(row["TrialCounts"]!=null && row["TrialCounts"].ToString()!="")
				{
					model.TrialCounts=int.Parse(row["TrialCounts"].ToString());
				}
				if(row["DisplaySequence"]!=null && row["DisplaySequence"].ToString()!="")
				{
					model.DisplaySequence=int.Parse(row["DisplaySequence"].ToString());
				}
				if(row["MarketPrice"]!=null && row["MarketPrice"].ToString()!="")
				{
					model.MarketPrice=decimal.Parse(row["MarketPrice"].ToString());
				}
				if(row["LowestSalePrice"]!=null && row["LowestSalePrice"].ToString()!="")
				{
					model.LowestSalePrice=decimal.Parse(row["LowestSalePrice"].ToString());
				}
				if(row["MainCategoryPath"]!=null)
				{
					model.MainCategoryPath=row["MainCategoryPath"].ToString();
				}
				if(row["ExtendCategoryPath"]!=null)
				{
					model.ExtendCategoryPath=row["ExtendCategoryPath"].ToString();
				}
				if(row["Points"]!=null && row["Points"].ToString()!="")
				{
					model.Points=decimal.Parse(row["Points"].ToString());
				}
				if(row["ImageUrl"]!=null)
				{
					model.ImageUrl=row["ImageUrl"].ToString();
				}
				if(row["ThumbnailUrl"]!=null)
				{
					model.ThumbnailUrl=row["ThumbnailUrl"].ToString();
				}
				if(row["MaxQuantity"]!=null && row["MaxQuantity"].ToString()!="")
				{
					model.MaxQuantity=int.Parse(row["MaxQuantity"].ToString());
				}
				if(row["MinQuantity"]!=null && row["MinQuantity"].ToString()!="")
				{
					model.MinQuantity=int.Parse(row["MinQuantity"].ToString());
				}
				if(row["Tags"]!=null)
				{
					model.Tags=row["Tags"].ToString();
				}
				if(row["SeoUrl"]!=null)
				{
					model.SeoUrl=row["SeoUrl"].ToString();
				}
				if(row["SeoImageAlt"]!=null)
				{
					model.SeoImageAlt=row["SeoImageAlt"].ToString();
				}
				if(row["SeoImageTitle"]!=null)
				{
					model.SeoImageTitle=row["SeoImageTitle"].ToString();
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
			strSql.Append("select TrialId,CategoryId,TrialName,EnterpriseId,RegionId,ShortDescription,Unit,Description,Meta_Title,Meta_Description,Meta_Keywords,LinklUrl,TrialStatus,StartDate,EndDate,CreatedDate,CreatedUserID,VistiCounts,TrialCounts,DisplaySequence,MarketPrice,LowestSalePrice,MainCategoryPath,ExtendCategoryPath,Points,ImageUrl,ThumbnailUrl,MaxQuantity,MinQuantity,Tags,SeoUrl,SeoImageAlt,SeoImageTitle ");
			strSql.Append(" FROM Shop_Trials ");
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
			strSql.Append(" TrialId,CategoryId,TrialName,EnterpriseId,RegionId,ShortDescription,Unit,Description,Meta_Title,Meta_Description,Meta_Keywords,LinklUrl,TrialStatus,StartDate,EndDate,CreatedDate,CreatedUserID,VistiCounts,TrialCounts,DisplaySequence,MarketPrice,LowestSalePrice,MainCategoryPath,ExtendCategoryPath,Points,ImageUrl,ThumbnailUrl,MaxQuantity,MinQuantity,Tags,SeoUrl,SeoImageAlt,SeoImageTitle ");
			strSql.Append(" FROM Shop_Trials ");
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
			strSql.Append("select count(1) FROM Shop_Trials ");
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
				strSql.Append("order by T.TrialId desc");
			}
			strSql.Append(")AS Row, T.*  from Shop_Trials T ");
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
			parameters[0].Value = "Shop_Trials";
			parameters[1].Value = "TrialId";
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

