using System;
using System.Data;
using System.Text;
using System.Data.SqlClient;
using YSWL.MALL.IDAL.Ms;
using YSWL.DBUtility;//Please add references
namespace YSWL.MALL.SQLServerDAL.Ms
{
	/// <summary>
	/// 数据访问类:RegionRec
	/// </summary>
	public partial class RegionRec:IRegionRec
	{
		public RegionRec()
		{}
		#region  BasicMethod

		/// <summary>
		/// 得到最大ID
		/// </summary>
		public int GetMaxId()
		{
		return DBHelper.DefaultDBHelper.GetMaxID("ID", "Ms_RegionRec"); 
		}

		/// <summary>
		/// 是否存在该记录
		/// </summary>
		public bool Exists(int ID)
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("select count(1) from Ms_RegionRec");
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
		public int Add(YSWL.MALL.Model.Ms.RegionRec model)
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("insert into Ms_RegionRec(");
			strSql.Append("RegionId,RegionName,DisplaySequence,Type)");
			strSql.Append(" values (");
			strSql.Append("@RegionId,@RegionName,@DisplaySequence,@Type)");
			strSql.Append(";select @@IDENTITY");
			SqlParameter[] parameters = {
					new SqlParameter("@RegionId", SqlDbType.Int,4),
					new SqlParameter("@RegionName", SqlDbType.NVarChar,100),
					new SqlParameter("@DisplaySequence", SqlDbType.Int,4),
					new SqlParameter("@Type", SqlDbType.SmallInt,2)};
			parameters[0].Value = model.RegionId;
			parameters[1].Value = model.RegionName;
			parameters[2].Value = model.DisplaySequence;
			parameters[3].Value = model.Type;

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
		public bool Update(YSWL.MALL.Model.Ms.RegionRec model)
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("update Ms_RegionRec set ");
			strSql.Append("RegionId=@RegionId,");
			strSql.Append("RegionName=@RegionName,");
			strSql.Append("DisplaySequence=@DisplaySequence,");
			strSql.Append("Type=@Type");
			strSql.Append(" where ID=@ID");
			SqlParameter[] parameters = {
					new SqlParameter("@RegionId", SqlDbType.Int,4),
					new SqlParameter("@RegionName", SqlDbType.NVarChar,100),
					new SqlParameter("@DisplaySequence", SqlDbType.Int,4),
					new SqlParameter("@Type", SqlDbType.SmallInt,2),
					new SqlParameter("@ID", SqlDbType.Int,4)};
			parameters[0].Value = model.RegionId;
			parameters[1].Value = model.RegionName;
			parameters[2].Value = model.DisplaySequence;
			parameters[3].Value = model.Type;
			parameters[4].Value = model.ID;

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
			strSql.Append("delete from Ms_RegionRec ");
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
			strSql.Append("delete from Ms_RegionRec ");
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
		public YSWL.MALL.Model.Ms.RegionRec GetModel(int ID)
		{
			
			StringBuilder strSql=new StringBuilder();
			strSql.Append("select  top 1 ID,RegionId,RegionName,DisplaySequence,Type from Ms_RegionRec ");
			strSql.Append(" where ID=@ID");
			SqlParameter[] parameters = {
					new SqlParameter("@ID", SqlDbType.Int,4)
			};
			parameters[0].Value = ID;

			YSWL.MALL.Model.Ms.RegionRec model=new YSWL.MALL.Model.Ms.RegionRec();
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
		public YSWL.MALL.Model.Ms.RegionRec DataRowToModel(DataRow row)
		{
			YSWL.MALL.Model.Ms.RegionRec model=new YSWL.MALL.Model.Ms.RegionRec();
			if (row != null)
			{
				if(row["ID"]!=null && row["ID"].ToString()!="")
				{
					model.ID=int.Parse(row["ID"].ToString());
				}
				if(row["RegionId"]!=null && row["RegionId"].ToString()!="")
				{
					model.RegionId=int.Parse(row["RegionId"].ToString());
				}
				if(row["RegionName"]!=null)
				{
					model.RegionName=row["RegionName"].ToString();
				}
				if(row["DisplaySequence"]!=null && row["DisplaySequence"].ToString()!="")
				{
					model.DisplaySequence=int.Parse(row["DisplaySequence"].ToString());
				}
				if(row["Type"]!=null && row["Type"].ToString()!="")
				{
					model.Type=int.Parse(row["Type"].ToString());
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
			strSql.Append("select ID,RegionId,RegionName,DisplaySequence,Type ");
			strSql.Append(" FROM Ms_RegionRec ");
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
			strSql.Append(" ID,RegionId,RegionName,DisplaySequence,Type ");
			strSql.Append(" FROM Ms_RegionRec ");
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
			strSql.Append("select count(1) FROM Ms_RegionRec ");
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
			strSql.Append(")AS Row, T.*  from Ms_RegionRec T ");
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
			parameters[0].Value = "Ms_RegionRec";
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

	    public bool Delete(int regionId, int type)
	    {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("delete from Ms_RegionRec ");
            strSql.Append(" where RegionId=@RegionId and Type=@Type");
            SqlParameter[] parameters = {
					new SqlParameter("@RegionId", SqlDbType.Int,4),
                    new SqlParameter("@Type", SqlDbType.Int,4)
			};
            parameters[0].Value = regionId;
            parameters[1].Value = type;

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

