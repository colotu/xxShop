﻿/**
* Theme.cs
*
* 功 能： N/A
* 类 名： Theme
*
* Ver    变更日期             负责人  变更内容
* ───────────────────────────────────
* V0.01  2012/12/27 15:56:15   N/A    初版
*
* Copyright (c) 2012 YS56 Corporation. All rights reserved.
*┌──────────────────────────────────┐
*│　此技术信息为本公司机密信息，未经本公司书面同意禁止向第三方披露．　│
*│　版权所有：云商未来（北京）科技有限公司　　　　　　　　　　　　　　│
*└──────────────────────────────────┘
*/
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Data.SqlClient;
using YSWL.MALL.IDAL.Ms;
using YSWL.DBUtility;//Please add references
namespace YSWL.MALL.SQLServerDAL.Ms
{
	/// <summary>
	/// 数据访问类:Theme
	/// </summary>
	public partial class Theme:ITheme
	{
		public Theme()
		{}
		#region  BasicMethod

		/// <summary>
		/// 得到最大ID
		/// </summary>
		public int GetMaxId()
		{
		return DBHelper.DefaultDBHelper.GetMaxID("ID", "Ms_Theme"); 
		}

		/// <summary>
		/// 是否存在该记录
		/// </summary>
		public bool Exists(int ID)
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("select count(1) from Ms_Theme");
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
		public int Add(YSWL.MALL.Model.Ms.Theme model)
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("insert into Ms_Theme(");
			strSql.Append("Name,Description,PreviewPhotoSrc,ZipPackageSrc,ThemeSize,Author,IsCurrent,Language,CreatedDate,Remark)");
			strSql.Append(" values (");
			strSql.Append("@Name,@Description,@PreviewPhotoSrc,@ZipPackageSrc,@ThemeSize,@Author,@IsCurrent,@Language,@CreatedDate,@Remark)");
			strSql.Append(";select @@IDENTITY");
			SqlParameter[] parameters = {
					new SqlParameter("@Name", SqlDbType.NVarChar,100),
					new SqlParameter("@Description", SqlDbType.NVarChar,200),
					new SqlParameter("@PreviewPhotoSrc", SqlDbType.NVarChar,100),
					new SqlParameter("@ZipPackageSrc", SqlDbType.NVarChar,50),
					new SqlParameter("@ThemeSize", SqlDbType.Int,4),
					new SqlParameter("@Author", SqlDbType.NVarChar,100),
					new SqlParameter("@IsCurrent", SqlDbType.Bit,1),
					new SqlParameter("@Language", SqlDbType.NVarChar,50),
					new SqlParameter("@CreatedDate", SqlDbType.DateTime),
					new SqlParameter("@Remark", SqlDbType.NVarChar,100)};
			parameters[0].Value = model.Name;
			parameters[1].Value = model.Description;
			parameters[2].Value = model.PreviewPhotoSrc;
			parameters[3].Value = model.ZipPackageSrc;
			parameters[4].Value = model.ThemeSize;
			parameters[5].Value = model.Author;
			parameters[6].Value = model.IsCurrent;
			parameters[7].Value = model.Language;
			parameters[8].Value = model.CreatedDate;
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
		public bool Update(YSWL.MALL.Model.Ms.Theme model)
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("update Ms_Theme set ");
			strSql.Append("Name=@Name,");
			strSql.Append("Description=@Description,");
			strSql.Append("PreviewPhotoSrc=@PreviewPhotoSrc,");
			strSql.Append("ZipPackageSrc=@ZipPackageSrc,");
			strSql.Append("ThemeSize=@ThemeSize,");
			strSql.Append("Author=@Author,");
			strSql.Append("IsCurrent=@IsCurrent,");
			strSql.Append("Language=@Language,");
			strSql.Append("CreatedDate=@CreatedDate,");
			strSql.Append("Remark=@Remark");
			strSql.Append(" where ID=@ID");
			SqlParameter[] parameters = {
					new SqlParameter("@Name", SqlDbType.NVarChar,100),
					new SqlParameter("@Description", SqlDbType.NVarChar,200),
					new SqlParameter("@PreviewPhotoSrc", SqlDbType.NVarChar,100),
					new SqlParameter("@ZipPackageSrc", SqlDbType.NVarChar,50),
					new SqlParameter("@ThemeSize", SqlDbType.Int,4),
					new SqlParameter("@Author", SqlDbType.NVarChar,100),
					new SqlParameter("@IsCurrent", SqlDbType.Bit,1),
					new SqlParameter("@Language", SqlDbType.NVarChar,50),
					new SqlParameter("@CreatedDate", SqlDbType.DateTime),
					new SqlParameter("@Remark", SqlDbType.NVarChar,100),
					new SqlParameter("@ID", SqlDbType.Int,4)};
			parameters[0].Value = model.Name;
			parameters[1].Value = model.Description;
			parameters[2].Value = model.PreviewPhotoSrc;
			parameters[3].Value = model.ZipPackageSrc;
			parameters[4].Value = model.ThemeSize;
			parameters[5].Value = model.Author;
			parameters[6].Value = model.IsCurrent;
			parameters[7].Value = model.Language;
			parameters[8].Value = model.CreatedDate;
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
			strSql.Append("delete from Ms_Theme ");
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
			strSql.Append("delete from Ms_Theme ");
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
		public YSWL.MALL.Model.Ms.Theme GetModel(int ID)
		{
			
			StringBuilder strSql=new StringBuilder();
			strSql.Append("select  top 1 ID,Name,Description,PreviewPhotoSrc,ZipPackageSrc,ThemeSize,Author,IsCurrent,Language,CreatedDate,Remark from Ms_Theme ");
			strSql.Append(" where ID=@ID");
			SqlParameter[] parameters = {
					new SqlParameter("@ID", SqlDbType.Int,4)
			};
			parameters[0].Value = ID;

			YSWL.MALL.Model.Ms.Theme model=new YSWL.MALL.Model.Ms.Theme();
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
		public YSWL.MALL.Model.Ms.Theme DataRowToModel(DataRow row)
		{
			YSWL.MALL.Model.Ms.Theme model=new YSWL.MALL.Model.Ms.Theme();
			if (row != null)
			{
				if(row["ID"]!=null && row["ID"].ToString()!="")
				{
					model.ID=int.Parse(row["ID"].ToString());
				}
				if(row["Name"]!=null)
				{
					model.Name=row["Name"].ToString();
				}
				if(row["Description"]!=null)
				{
					model.Description=row["Description"].ToString();
				}
				if(row["PreviewPhotoSrc"]!=null)
				{
					model.PreviewPhotoSrc=row["PreviewPhotoSrc"].ToString();
				}
				if(row["ZipPackageSrc"]!=null)
				{
					model.ZipPackageSrc=row["ZipPackageSrc"].ToString();
				}
				if(row["ThemeSize"]!=null && row["ThemeSize"].ToString()!="")
				{
					model.ThemeSize=int.Parse(row["ThemeSize"].ToString());
				}
				if(row["Author"]!=null)
				{
					model.Author=row["Author"].ToString();
				}
				if(row["IsCurrent"]!=null && row["IsCurrent"].ToString()!="")
				{
					if((row["IsCurrent"].ToString()=="1")||(row["IsCurrent"].ToString().ToLower()=="true"))
					{
						model.IsCurrent=true;
					}
					else
					{
						model.IsCurrent=false;
					}
				}
				if(row["Language"]!=null)
				{
					model.Language=row["Language"].ToString();
				}
				if(row["CreatedDate"]!=null && row["CreatedDate"].ToString()!="")
				{
					model.CreatedDate=DateTime.Parse(row["CreatedDate"].ToString());
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
			strSql.Append("select ID,Name,Description,PreviewPhotoSrc,ZipPackageSrc,ThemeSize,Author,IsCurrent,Language,CreatedDate,Remark ");
			strSql.Append(" FROM Ms_Theme ");
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
			strSql.Append(" ID,Name,Description,PreviewPhotoSrc,ZipPackageSrc,ThemeSize,Author,IsCurrent,Language,CreatedDate,Remark ");
			strSql.Append(" FROM Ms_Theme ");
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
			strSql.Append("select count(1) FROM Ms_Theme ");
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
			strSql.Append(")AS Row, T.*  from Ms_Theme T ");
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
			parameters[0].Value = "Ms_Theme";
			parameters[1].Value = "ID";
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
        /// 删除一条数据
        /// </summary>
        public bool UpdateEx(int Id)
        {
            List<string> sqllist = new List<string>();

            StringBuilder strSql2 = new StringBuilder();
            strSql2.Append("update Ms_Theme set IsCurrent=0 ");
            sqllist.Add(strSql2.ToString());


            StringBuilder strSql = new StringBuilder();
            strSql.Append("update Ms_Theme set IsCurrent=1 where  ID="+Id+" ");
            sqllist.Add(strSql.ToString());


            int rowsAffected = DBHelper.DefaultDBHelper.ExecuteSqlTran(sqllist);
            if (rowsAffected > 0)
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

