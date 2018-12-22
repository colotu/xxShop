using System;
using System.Data;
using System.Text;
using System.Data.SqlClient;
using YSWL.MALL.IDAL.Shop.Sample;
using YSWL.DBUtility;//Please add references
namespace YSWL.MALL.SQLServerDAL.Shop.Sample
{
	/// <summary>
	/// 数据访问类:SampleDetail
	/// </summary>
	public partial class SampleDetail:ISampleDetail
	{
		public SampleDetail()
		{}
		#region  Method

		/// <summary>
		/// 得到最大ID
		/// </summary>
		public int GetMaxId()
		{
		return DBHelper.DefaultDBHelper.GetMaxID("ID", "Shop_SampleDetail"); 
		}

		/// <summary>
		/// 是否存在该记录
		/// </summary>
		public bool Exists(int ID)
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("select count(1) from Shop_SampleDetail");
			strSql.Append(" where ID=@ID");
			SqlParameter[] parameters = {
					new SqlParameter("@ID", SqlDbType.Int,4)
			};
			parameters[0].Value = ID;

			return DBHelper.DefaultDBHelper.Exists(strSql.ToString(),parameters);
		}


		/// <summary>
		/// 增加一条数据
		/// </summary>
		public int Add(YSWL.MALL.Model.Shop.Sample.SampleDetail model)
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("insert into Shop_SampleDetail(");
			strSql.Append("SampleId,Title,Type,ImageUrl,NormalImageUrl,ThumbImageUrl,PdfUrl,CreatedDate,Status,Remark)");
			strSql.Append(" values (");
			strSql.Append("@SampleId,@Title,@Type,@ImageUrl,@NormalImageUrl,@ThumbImageUrl,@PdfUrl,@CreatedDate,@Status,@Remark)");
			strSql.Append(";select @@IDENTITY");
			SqlParameter[] parameters = {
					new SqlParameter("@SampleId", SqlDbType.Int,4),
					new SqlParameter("@Title", SqlDbType.NVarChar,100),
					new SqlParameter("@Type", SqlDbType.SmallInt,2),
					new SqlParameter("@ImageUrl", SqlDbType.NVarChar,300),
					new SqlParameter("@NormalImageUrl", SqlDbType.NVarChar,300),
					new SqlParameter("@ThumbImageUrl", SqlDbType.NVarChar,300),
					new SqlParameter("@PdfUrl", SqlDbType.NVarChar,300),
					new SqlParameter("@CreatedDate", SqlDbType.DateTime),
					new SqlParameter("@Status", SqlDbType.SmallInt,2),
					new SqlParameter("@Remark", SqlDbType.NVarChar,300)};
			parameters[0].Value = model.SampleId;
			parameters[1].Value = model.Title;
			parameters[2].Value = model.Type;
			parameters[3].Value = model.ImageUrl;
			parameters[4].Value = model.NormalImageUrl;
			parameters[5].Value = model.ThumbImageUrl;
			parameters[6].Value = model.PdfUrl;
			parameters[7].Value = model.CreatedDate;
			parameters[8].Value = model.Status;
			parameters[9].Value = model.Remark;

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
		public bool Update(YSWL.MALL.Model.Shop.Sample.SampleDetail model)
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("update Shop_SampleDetail set ");
			strSql.Append("SampleId=@SampleId,");
			strSql.Append("Title=@Title,");
			strSql.Append("Type=@Type,");
			strSql.Append("ImageUrl=@ImageUrl,");
			strSql.Append("NormalImageUrl=@NormalImageUrl,");
			strSql.Append("ThumbImageUrl=@ThumbImageUrl,");
			strSql.Append("PdfUrl=@PdfUrl,");
			strSql.Append("CreatedDate=@CreatedDate,");
			strSql.Append("Status=@Status,");
			strSql.Append("Remark=@Remark");
			strSql.Append(" where ID=@ID");
			SqlParameter[] parameters = {
					new SqlParameter("@SampleId", SqlDbType.Int,4),
					new SqlParameter("@Title", SqlDbType.NVarChar,100),
					new SqlParameter("@Type", SqlDbType.SmallInt,2),
					new SqlParameter("@ImageUrl", SqlDbType.NVarChar,300),
					new SqlParameter("@NormalImageUrl", SqlDbType.NVarChar,300),
					new SqlParameter("@ThumbImageUrl", SqlDbType.NVarChar,300),
					new SqlParameter("@PdfUrl", SqlDbType.NVarChar,300),
					new SqlParameter("@CreatedDate", SqlDbType.DateTime),
					new SqlParameter("@Status", SqlDbType.SmallInt,2),
					new SqlParameter("@Remark", SqlDbType.NVarChar,300),
					new SqlParameter("@ID", SqlDbType.Int,4)};
			parameters[0].Value = model.SampleId;
			parameters[1].Value = model.Title;
			parameters[2].Value = model.Type;
			parameters[3].Value = model.ImageUrl;
			parameters[4].Value = model.NormalImageUrl;
			parameters[5].Value = model.ThumbImageUrl;
			parameters[6].Value = model.PdfUrl;
			parameters[7].Value = model.CreatedDate;
			parameters[8].Value = model.Status;
			parameters[9].Value = model.Remark;
			parameters[10].Value = model.ID;

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
		public bool Delete(int ID)
		{
			
			StringBuilder strSql=new StringBuilder();
			strSql.Append("delete from Shop_SampleDetail ");
			strSql.Append(" where ID=@ID");
			SqlParameter[] parameters = {
					new SqlParameter("@ID", SqlDbType.Int,4)
			};
			parameters[0].Value = ID;

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
		public bool DeleteList(string IDlist )
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("delete from Shop_SampleDetail ");
			strSql.Append(" where ID in ("+IDlist + ")  ");
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
		public YSWL.MALL.Model.Shop.Sample.SampleDetail GetModel(int ID)
		{
			
			StringBuilder strSql=new StringBuilder();
			strSql.Append("select  top 1 ID,SampleId,Title,Type,ImageUrl,NormalImageUrl,ThumbImageUrl,PdfUrl,CreatedDate,Status,Remark from Shop_SampleDetail ");
			strSql.Append(" where ID=@ID");
			SqlParameter[] parameters = {
					new SqlParameter("@ID", SqlDbType.Int,4)
			};
			parameters[0].Value = ID;

			YSWL.MALL.Model.Shop.Sample.SampleDetail model=new YSWL.MALL.Model.Shop.Sample.SampleDetail();
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
		public YSWL.MALL.Model.Shop.Sample.SampleDetail DataRowToModel(DataRow row)
		{
			YSWL.MALL.Model.Shop.Sample.SampleDetail model=new YSWL.MALL.Model.Shop.Sample.SampleDetail();
			if (row != null)
			{
				if(row["ID"]!=null && row["ID"].ToString()!="")
				{
					model.ID=int.Parse(row["ID"].ToString());
				}
				if(row["SampleId"]!=null && row["SampleId"].ToString()!="")
				{
					model.SampleId=int.Parse(row["SampleId"].ToString());
				}
				if(row["Title"]!=null && row["Title"].ToString()!="")
				{
					model.Title=row["Title"].ToString();
				}
				if(row["Type"]!=null && row["Type"].ToString()!="")
				{
					model.Type=int.Parse(row["Type"].ToString());
				}
				if(row["ImageUrl"]!=null && row["ImageUrl"].ToString()!="")
				{
					model.ImageUrl=row["ImageUrl"].ToString();
				}
				if(row["NormalImageUrl"]!=null && row["NormalImageUrl"].ToString()!="")
				{
					model.NormalImageUrl=row["NormalImageUrl"].ToString();
				}
				if(row["ThumbImageUrl"]!=null && row["ThumbImageUrl"].ToString()!="")
				{
					model.ThumbImageUrl=row["ThumbImageUrl"].ToString();
				}
				if(row["PdfUrl"]!=null && row["PdfUrl"].ToString()!="")
				{
					model.PdfUrl=row["PdfUrl"].ToString();
				}
				if(row["CreatedDate"]!=null && row["CreatedDate"].ToString()!="")
				{
					model.CreatedDate=DateTime.Parse(row["CreatedDate"].ToString());
				}
				if(row["Status"]!=null && row["Status"].ToString()!="")
				{
					model.Status=int.Parse(row["Status"].ToString());
				}
				if(row["Remark"]!=null && row["Remark"].ToString()!="")
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
			strSql.Append("select ID,SampleId,Title,Type,ImageUrl,NormalImageUrl,ThumbImageUrl,PdfUrl,CreatedDate,Status,Remark ");
			strSql.Append(" FROM Shop_SampleDetail ");
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
			strSql.Append(" ID,SampleId,Title,Type,ImageUrl,NormalImageUrl,ThumbImageUrl,PdfUrl,CreatedDate,Status,Remark ");
			strSql.Append(" FROM Shop_SampleDetail ");
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
			strSql.Append("select count(1) FROM Shop_SampleDetail ");
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
				strSql.Append("order by T.ID desc");
			}
			strSql.Append(")AS Row, T.*  from Shop_SampleDetail T ");
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
			parameters[0].Value = "Shop_SampleDetail";
			parameters[1].Value = "ID";
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

