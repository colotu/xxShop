using System;
using System.Data;
using System.Text;
using System.Data.SqlClient;
using YSWL.MALL.IDAL.Shop.Sample;
using YSWL.DBUtility;//Please add references
namespace YSWL.MALL.SQLServerDAL.Shop.Sample
{
	/// <summary>
	/// 数据访问类:Sample
	/// </summary>
	public partial class Sample:ISample
	{
		public Sample()
		{}
		#region  Method

		/// <summary>
		/// 得到最大ID
		/// </summary>
		public int GetMaxId()
		{
		return DBHelper.DefaultDBHelper.GetMaxID("SampleId", "Shop_Sample"); 
		}

		/// <summary>
		/// 是否存在该记录
		/// </summary>
		public bool Exists(int SampleId)
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("select count(1) from Shop_Sample");
			strSql.Append(" where SampleId=@SampleId");
			SqlParameter[] parameters = {
					new SqlParameter("@SampleId", SqlDbType.Int,4)
			};
			parameters[0].Value = SampleId;

			return DBHelper.DefaultDBHelper.Exists(strSql.ToString(),parameters);
		}


		/// <summary>
		/// 增加一条数据
		/// </summary>
		public int Add(YSWL.MALL.Model.Shop.Sample.Sample model)
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("insert into Shop_Sample(");
			strSql.Append("Tiltle,ElecCoverImageUrl,NormalElecCoverImageUrl,ThumblElecCoverImageUrl,PdfCoverImageUrl,NormalPdfCoverImageUrl,ThumbPdfCoverImageUrl,Sequence,Status,CreatedDate,Remark,Meta_Title,Meta_Description,Meta_KeyWords,SeoUrl,SeoImageAlt,SeoImageTitle)");
			strSql.Append(" values (");
			strSql.Append("@Tiltle,@ElecCoverImageUrl,@NormalElecCoverImageUrl,@ThumblElecCoverImageUrl,@PdfCoverImageUrl,@NormalPdfCoverImageUrl,@ThumbPdfCoverImageUrl,@Sequence,@Status,@CreatedDate,@Remark,@Meta_Title,@Meta_Description,@Meta_KeyWords,@SeoUrl,@SeoImageAlt,@SeoImageTitle)");
			strSql.Append(";select @@IDENTITY");
			SqlParameter[] parameters = {
					new SqlParameter("@Tiltle", SqlDbType.NVarChar,200),
					new SqlParameter("@ElecCoverImageUrl", SqlDbType.NVarChar,200),
					new SqlParameter("@NormalElecCoverImageUrl", SqlDbType.NVarChar,200),
					new SqlParameter("@ThumblElecCoverImageUrl", SqlDbType.NVarChar,200),
					new SqlParameter("@PdfCoverImageUrl", SqlDbType.NVarChar,200),
					new SqlParameter("@NormalPdfCoverImageUrl", SqlDbType.NVarChar,200),
					new SqlParameter("@ThumbPdfCoverImageUrl", SqlDbType.NVarChar,200),
					new SqlParameter("@Sequence", SqlDbType.Int,4),
					new SqlParameter("@Status", SqlDbType.SmallInt,2),
					new SqlParameter("@CreatedDate", SqlDbType.DateTime),
					new SqlParameter("@Remark", SqlDbType.NVarChar,200),
					new SqlParameter("@Meta_Title", SqlDbType.NVarChar,1000),
					new SqlParameter("@Meta_Description", SqlDbType.NVarChar,1000),
					new SqlParameter("@Meta_KeyWords", SqlDbType.NVarChar,1000),
					new SqlParameter("@SeoUrl", SqlDbType.NVarChar,300),
					new SqlParameter("@SeoImageAlt", SqlDbType.NVarChar,300),
					new SqlParameter("@SeoImageTitle", SqlDbType.NVarChar,300)};
			parameters[0].Value = model.Tiltle;
			parameters[1].Value = model.ElecCoverImageUrl;
			parameters[2].Value = model.NormalElecCoverImageUrl;
			parameters[3].Value = model.ThumblElecCoverImageUrl;
			parameters[4].Value = model.PdfCoverImageUrl;
			parameters[5].Value = model.NormalPdfCoverImageUrl;
			parameters[6].Value = model.ThumbPdfCoverImageUrl;
			parameters[7].Value = model.Sequence;
			parameters[8].Value = model.Status;
			parameters[9].Value = model.CreatedDate;
			parameters[10].Value = model.Remark;
			parameters[11].Value = model.Meta_Title;
			parameters[12].Value = model.Meta_Description;
			parameters[13].Value = model.Meta_KeyWords;
			parameters[14].Value = model.SeoUrl;
			parameters[15].Value = model.SeoImageAlt;
			parameters[16].Value = model.SeoImageTitle;

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
		public bool Update(YSWL.MALL.Model.Shop.Sample.Sample model)
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("update Shop_Sample set ");
			strSql.Append("Tiltle=@Tiltle,");
			strSql.Append("ElecCoverImageUrl=@ElecCoverImageUrl,");
			strSql.Append("NormalElecCoverImageUrl=@NormalElecCoverImageUrl,");
			strSql.Append("ThumblElecCoverImageUrl=@ThumblElecCoverImageUrl,");
			strSql.Append("PdfCoverImageUrl=@PdfCoverImageUrl,");
			strSql.Append("NormalPdfCoverImageUrl=@NormalPdfCoverImageUrl,");
			strSql.Append("ThumbPdfCoverImageUrl=@ThumbPdfCoverImageUrl,");
			strSql.Append("Sequence=@Sequence,");
			strSql.Append("Status=@Status,");
			strSql.Append("CreatedDate=@CreatedDate,");
			strSql.Append("Remark=@Remark,");
			strSql.Append("Meta_Title=@Meta_Title,");
			strSql.Append("Meta_Description=@Meta_Description,");
			strSql.Append("Meta_KeyWords=@Meta_KeyWords,");
			strSql.Append("SeoUrl=@SeoUrl,");
			strSql.Append("SeoImageAlt=@SeoImageAlt,");
			strSql.Append("SeoImageTitle=@SeoImageTitle");
			strSql.Append(" where SampleId=@SampleId");
			SqlParameter[] parameters = {
					new SqlParameter("@Tiltle", SqlDbType.NVarChar,200),
					new SqlParameter("@ElecCoverImageUrl", SqlDbType.NVarChar,200),
					new SqlParameter("@NormalElecCoverImageUrl", SqlDbType.NVarChar,200),
					new SqlParameter("@ThumblElecCoverImageUrl", SqlDbType.NVarChar,200),
					new SqlParameter("@PdfCoverImageUrl", SqlDbType.NVarChar,200),
					new SqlParameter("@NormalPdfCoverImageUrl", SqlDbType.NVarChar,200),
					new SqlParameter("@ThumbPdfCoverImageUrl", SqlDbType.NVarChar,200),
					new SqlParameter("@Sequence", SqlDbType.Int,4),
					new SqlParameter("@Status", SqlDbType.SmallInt,2),
					new SqlParameter("@CreatedDate", SqlDbType.DateTime),
					new SqlParameter("@Remark", SqlDbType.NVarChar,200),
					new SqlParameter("@Meta_Title", SqlDbType.NVarChar,1000),
					new SqlParameter("@Meta_Description", SqlDbType.NVarChar,1000),
					new SqlParameter("@Meta_KeyWords", SqlDbType.NVarChar,1000),
					new SqlParameter("@SeoUrl", SqlDbType.NVarChar,300),
					new SqlParameter("@SeoImageAlt", SqlDbType.NVarChar,300),
					new SqlParameter("@SeoImageTitle", SqlDbType.NVarChar,300),
					new SqlParameter("@SampleId", SqlDbType.Int,4)};
			parameters[0].Value = model.Tiltle;
			parameters[1].Value = model.ElecCoverImageUrl;
			parameters[2].Value = model.NormalElecCoverImageUrl;
			parameters[3].Value = model.ThumblElecCoverImageUrl;
			parameters[4].Value = model.PdfCoverImageUrl;
			parameters[5].Value = model.NormalPdfCoverImageUrl;
			parameters[6].Value = model.ThumbPdfCoverImageUrl;
			parameters[7].Value = model.Sequence;
			parameters[8].Value = model.Status;
			parameters[9].Value = model.CreatedDate;
			parameters[10].Value = model.Remark;
			parameters[11].Value = model.Meta_Title;
			parameters[12].Value = model.Meta_Description;
			parameters[13].Value = model.Meta_KeyWords;
			parameters[14].Value = model.SeoUrl;
			parameters[15].Value = model.SeoImageAlt;
			parameters[16].Value = model.SeoImageTitle;
			parameters[17].Value = model.SampleId;

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
		public bool Delete(int SampleId)
		{
			
			StringBuilder strSql=new StringBuilder();
			strSql.Append("delete from Shop_Sample ");
			strSql.Append(" where SampleId=@SampleId");
			SqlParameter[] parameters = {
					new SqlParameter("@SampleId", SqlDbType.Int,4)
			};
			parameters[0].Value = SampleId;

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
		public bool DeleteList(string SampleIdlist )
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("delete from Shop_Sample ");
			strSql.Append(" where SampleId in ("+SampleIdlist + ")  ");
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
		public YSWL.MALL.Model.Shop.Sample.Sample GetModel(int SampleId)
		{
			
			StringBuilder strSql=new StringBuilder();
			strSql.Append("select  top 1 SampleId,Tiltle,ElecCoverImageUrl,NormalElecCoverImageUrl,ThumblElecCoverImageUrl,PdfCoverImageUrl,NormalPdfCoverImageUrl,ThumbPdfCoverImageUrl,Sequence,Status,CreatedDate,Remark,Meta_Title,Meta_Description,Meta_KeyWords,SeoUrl,SeoImageAlt,SeoImageTitle from Shop_Sample ");
			strSql.Append(" where SampleId=@SampleId");
			SqlParameter[] parameters = {
					new SqlParameter("@SampleId", SqlDbType.Int,4)
			};
			parameters[0].Value = SampleId;

			YSWL.MALL.Model.Shop.Sample.Sample model=new YSWL.MALL.Model.Shop.Sample.Sample();
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
		public YSWL.MALL.Model.Shop.Sample.Sample DataRowToModel(DataRow row)
		{
			YSWL.MALL.Model.Shop.Sample.Sample model=new YSWL.MALL.Model.Shop.Sample.Sample();
			if (row != null)
			{
				if(row["SampleId"]!=null && row["SampleId"].ToString()!="")
				{
					model.SampleId=int.Parse(row["SampleId"].ToString());
				}
				if(row["Tiltle"]!=null && row["Tiltle"].ToString()!="")
				{
					model.Tiltle=row["Tiltle"].ToString();
				}
				if(row["ElecCoverImageUrl"]!=null && row["ElecCoverImageUrl"].ToString()!="")
				{
					model.ElecCoverImageUrl=row["ElecCoverImageUrl"].ToString();
				}
				if(row["NormalElecCoverImageUrl"]!=null && row["NormalElecCoverImageUrl"].ToString()!="")
				{
					model.NormalElecCoverImageUrl=row["NormalElecCoverImageUrl"].ToString();
				}
				if(row["ThumblElecCoverImageUrl"]!=null && row["ThumblElecCoverImageUrl"].ToString()!="")
				{
					model.ThumblElecCoverImageUrl=row["ThumblElecCoverImageUrl"].ToString();
				}
				if(row["PdfCoverImageUrl"]!=null && row["PdfCoverImageUrl"].ToString()!="")
				{
					model.PdfCoverImageUrl=row["PdfCoverImageUrl"].ToString();
				}
				if(row["NormalPdfCoverImageUrl"]!=null && row["NormalPdfCoverImageUrl"].ToString()!="")
				{
					model.NormalPdfCoverImageUrl=row["NormalPdfCoverImageUrl"].ToString();
				}
				if(row["ThumbPdfCoverImageUrl"]!=null && row["ThumbPdfCoverImageUrl"].ToString()!="")
				{
					model.ThumbPdfCoverImageUrl=row["ThumbPdfCoverImageUrl"].ToString();
				}
				if(row["Sequence"]!=null && row["Sequence"].ToString()!="")
				{
					model.Sequence=int.Parse(row["Sequence"].ToString());
				}
				if(row["Status"]!=null && row["Status"].ToString()!="")
				{
					model.Status=int.Parse(row["Status"].ToString());
				}
				if(row["CreatedDate"]!=null && row["CreatedDate"].ToString()!="")
				{
					model.CreatedDate=DateTime.Parse(row["CreatedDate"].ToString());
				}
				if(row["Remark"]!=null && row["Remark"].ToString()!="")
				{
					model.Remark=row["Remark"].ToString();
				}
				if(row["Meta_Title"]!=null && row["Meta_Title"].ToString()!="")
				{
					model.Meta_Title=row["Meta_Title"].ToString();
				}
				if(row["Meta_Description"]!=null && row["Meta_Description"].ToString()!="")
				{
					model.Meta_Description=row["Meta_Description"].ToString();
				}
				if(row["Meta_KeyWords"]!=null && row["Meta_KeyWords"].ToString()!="")
				{
					model.Meta_KeyWords=row["Meta_KeyWords"].ToString();
				}
				if(row["SeoUrl"]!=null && row["SeoUrl"].ToString()!="")
				{
					model.SeoUrl=row["SeoUrl"].ToString();
				}
				if(row["SeoImageAlt"]!=null && row["SeoImageAlt"].ToString()!="")
				{
					model.SeoImageAlt=row["SeoImageAlt"].ToString();
				}
				if(row["SeoImageTitle"]!=null && row["SeoImageTitle"].ToString()!="")
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
			strSql.Append("select SampleId,Tiltle,ElecCoverImageUrl,NormalElecCoverImageUrl,ThumblElecCoverImageUrl,PdfCoverImageUrl,NormalPdfCoverImageUrl,ThumbPdfCoverImageUrl,Sequence,Status,CreatedDate,Remark,Meta_Title,Meta_Description,Meta_KeyWords,SeoUrl,SeoImageAlt,SeoImageTitle ");
			strSql.Append(" FROM Shop_Sample ");
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
			strSql.Append(" SampleId,Tiltle,ElecCoverImageUrl,NormalElecCoverImageUrl,ThumblElecCoverImageUrl,PdfCoverImageUrl,NormalPdfCoverImageUrl,ThumbPdfCoverImageUrl,Sequence,Status,CreatedDate,Remark,Meta_Title,Meta_Description,Meta_KeyWords,SeoUrl,SeoImageAlt,SeoImageTitle ");
			strSql.Append(" FROM Shop_Sample ");
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
			strSql.Append("select count(1) FROM Shop_Sample ");
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
				strSql.Append("order by T.SampleId desc");
			}
			strSql.Append(")AS Row, T.*  from Shop_Sample T ");
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
			parameters[0].Value = "Shop_Sample";
			parameters[1].Value = "SampleId";
			parameters[2].Value = PageSize;
			parameters[3].Value = PageIndex;
			parameters[4].Value = 0;
			parameters[5].Value = 0;
			parameters[6].Value = strWhere;	
			return DBHelper.DefaultDBHelper.RunProcedure("UP_GetRecordByPage",parameters,"ds");
		}*/

		#endregion  Method
	}
}

