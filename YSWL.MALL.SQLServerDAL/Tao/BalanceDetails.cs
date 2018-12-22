using System;
using System.Data;
using System.Text;
using System.Data.SqlClient;
using YSWL.IDAL.Tao;
using YSWL.DBUtility;//Please add references
namespace YSWL.SQLServerDAL.Tao
{
	/// <summary>
	/// 数据访问类:BalanceDetails
	/// </summary>
	public partial class BalanceDetails:IBalanceDetails
	{
		public BalanceDetails()
		{}
		#region  BasicMethod

		/// <summary>
		/// 是否存在该记录
		/// </summary>
		public bool Exists(long JournalNumber)
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("select count(1) from Tao_BalanceDetails");
			strSql.Append(" where JournalNumber=@JournalNumber");
			SqlParameter[] parameters = {
					new SqlParameter("@JournalNumber", SqlDbType.BigInt)
			};
			parameters[0].Value = JournalNumber;

			return DbHelperSQL.Exists(strSql.ToString(),parameters);
		}


		/// <summary>
		/// 增加一条数据
		/// </summary>
		public long Add(YSWL.Model.Tao.BalanceDetails model)
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("insert into Tao_BalanceDetails(");
			strSql.Append("UserId,TradeDate,TradeType,Income,Expenses,Balance,Payer,Payee,Remark)");
			strSql.Append(" values (");
			strSql.Append("@UserId,@TradeDate,@TradeType,@Income,@Expenses,@Balance,@Payer,@Payee,@Remark)");
			strSql.Append(";select @@IDENTITY");
			SqlParameter[] parameters = {
					new SqlParameter("@UserId", SqlDbType.Int,4),
					new SqlParameter("@TradeDate", SqlDbType.DateTime),
					new SqlParameter("@TradeType", SqlDbType.Int,4),
					new SqlParameter("@Income", SqlDbType.Money,8),
					new SqlParameter("@Expenses", SqlDbType.Money,8),
					new SqlParameter("@Balance", SqlDbType.Money,8),
					new SqlParameter("@Payer", SqlDbType.Int,4),
					new SqlParameter("@Payee", SqlDbType.Int,4),
					new SqlParameter("@Remark", SqlDbType.NVarChar,2000)};
			parameters[0].Value = model.UserId;
			parameters[1].Value = model.TradeDate;
			parameters[2].Value = model.TradeType;
			parameters[3].Value = model.Income;
			parameters[4].Value = model.Expenses;
			parameters[5].Value = model.Balance;
			parameters[6].Value = model.Payer;
			parameters[7].Value = model.Payee;
			parameters[8].Value = model.Remark;

			object obj = DbHelperSQL.GetSingle(strSql.ToString(),parameters);
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
		public bool Update(YSWL.Model.Tao.BalanceDetails model)
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("update Tao_BalanceDetails set ");
			strSql.Append("UserId=@UserId,");
			strSql.Append("TradeDate=@TradeDate,");
			strSql.Append("TradeType=@TradeType,");
			strSql.Append("Income=@Income,");
			strSql.Append("Expenses=@Expenses,");
			strSql.Append("Balance=@Balance,");
			strSql.Append("Payer=@Payer,");
			strSql.Append("Payee=@Payee,");
			strSql.Append("Remark=@Remark");
			strSql.Append(" where JournalNumber=@JournalNumber");
			SqlParameter[] parameters = {
					new SqlParameter("@UserId", SqlDbType.Int,4),
					new SqlParameter("@TradeDate", SqlDbType.DateTime),
					new SqlParameter("@TradeType", SqlDbType.Int,4),
					new SqlParameter("@Income", SqlDbType.Money,8),
					new SqlParameter("@Expenses", SqlDbType.Money,8),
					new SqlParameter("@Balance", SqlDbType.Money,8),
					new SqlParameter("@Payer", SqlDbType.Int,4),
					new SqlParameter("@Payee", SqlDbType.Int,4),
					new SqlParameter("@Remark", SqlDbType.NVarChar,2000),
					new SqlParameter("@JournalNumber", SqlDbType.BigInt,8)};
			parameters[0].Value = model.UserId;
			parameters[1].Value = model.TradeDate;
			parameters[2].Value = model.TradeType;
			parameters[3].Value = model.Income;
			parameters[4].Value = model.Expenses;
			parameters[5].Value = model.Balance;
			parameters[6].Value = model.Payer;
			parameters[7].Value = model.Payee;
			parameters[8].Value = model.Remark;
			parameters[9].Value = model.JournalNumber;

			int rows=DbHelperSQL.ExecuteSql(strSql.ToString(),parameters);
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
		public bool Delete(long JournalNumber)
		{
			
			StringBuilder strSql=new StringBuilder();
			strSql.Append("delete from Tao_BalanceDetails ");
			strSql.Append(" where JournalNumber=@JournalNumber");
			SqlParameter[] parameters = {
					new SqlParameter("@JournalNumber", SqlDbType.BigInt)
			};
			parameters[0].Value = JournalNumber;

			int rows=DbHelperSQL.ExecuteSql(strSql.ToString(),parameters);
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
		public bool DeleteList(string JournalNumberlist )
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("delete from Tao_BalanceDetails ");
			strSql.Append(" where JournalNumber in ("+JournalNumberlist + ")  ");
			int rows=DbHelperSQL.ExecuteSql(strSql.ToString());
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
		public YSWL.Model.Tao.BalanceDetails GetModel(long JournalNumber)
		{
			
			StringBuilder strSql=new StringBuilder();
			strSql.Append("select  top 1 JournalNumber,UserId,TradeDate,TradeType,Income,Expenses,Balance,Payer,Payee,Remark from Tao_BalanceDetails ");
			strSql.Append(" where JournalNumber=@JournalNumber");
			SqlParameter[] parameters = {
					new SqlParameter("@JournalNumber", SqlDbType.BigInt)
			};
			parameters[0].Value = JournalNumber;

			YSWL.Model.Tao.BalanceDetails model=new YSWL.Model.Tao.BalanceDetails();
			DataSet ds=DbHelperSQL.Query(strSql.ToString(),parameters);
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
		public YSWL.Model.Tao.BalanceDetails DataRowToModel(DataRow row)
		{
			YSWL.Model.Tao.BalanceDetails model=new YSWL.Model.Tao.BalanceDetails();
			if (row != null)
			{
				if(row["JournalNumber"]!=null && row["JournalNumber"].ToString()!="")
				{
					model.JournalNumber=long.Parse(row["JournalNumber"].ToString());
				}
				if(row["UserId"]!=null && row["UserId"].ToString()!="")
				{
					model.UserId=int.Parse(row["UserId"].ToString());
				}
				if(row["TradeDate"]!=null && row["TradeDate"].ToString()!="")
				{
					model.TradeDate=DateTime.Parse(row["TradeDate"].ToString());
				}
				if(row["TradeType"]!=null && row["TradeType"].ToString()!="")
				{
					model.TradeType=int.Parse(row["TradeType"].ToString());
				}
				if(row["Income"]!=null && row["Income"].ToString()!="")
				{
					model.Income=decimal.Parse(row["Income"].ToString());
				}
				if(row["Expenses"]!=null && row["Expenses"].ToString()!="")
				{
					model.Expenses=decimal.Parse(row["Expenses"].ToString());
				}
				if(row["Balance"]!=null && row["Balance"].ToString()!="")
				{
					model.Balance=decimal.Parse(row["Balance"].ToString());
				}
				if(row["Payer"]!=null && row["Payer"].ToString()!="")
				{
					model.Payer=int.Parse(row["Payer"].ToString());
				}
				if(row["Payee"]!=null && row["Payee"].ToString()!="")
				{
					model.Payee=int.Parse(row["Payee"].ToString());
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
			strSql.Append("select JournalNumber,UserId,TradeDate,TradeType,Income,Expenses,Balance,Payer,Payee,Remark ");
			strSql.Append(" FROM Tao_BalanceDetails ");
			if(strWhere.Trim()!="")
			{
				strSql.Append(" where "+strWhere);
			}
			return DbHelperSQL.Query(strSql.ToString());
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
			strSql.Append(" JournalNumber,UserId,TradeDate,TradeType,Income,Expenses,Balance,Payer,Payee,Remark ");
			strSql.Append(" FROM Tao_BalanceDetails ");
			if(strWhere.Trim()!="")
			{
				strSql.Append(" where "+strWhere);
			}
			strSql.Append(" order by " + filedOrder);
			return DbHelperSQL.Query(strSql.ToString());
		}

		/// <summary>
		/// 获取记录总数
		/// </summary>
		public int GetRecordCount(string strWhere)
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("select count(1) FROM Tao_BalanceDetails ");
			if(strWhere.Trim()!="")
			{
				strSql.Append(" where "+strWhere);
			}
			object obj = DbHelperSQL.GetSingle(strSql.ToString());
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
				strSql.Append("order by T.JournalNumber desc");
			}
			strSql.Append(")AS Row, T.*  from Tao_BalanceDetails T ");
			if (!string.IsNullOrEmpty(strWhere.Trim()))
			{
				strSql.Append(" WHERE " + strWhere);
			}
			strSql.Append(" ) TT");
			strSql.AppendFormat(" WHERE TT.Row between {0} and {1}", startIndex, endIndex);
			return DbHelperSQL.Query(strSql.ToString());
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
			parameters[0].Value = "Tao_BalanceDetails";
			parameters[1].Value = "JournalNumber";
			parameters[2].Value = PageSize;
			parameters[3].Value = PageIndex;
			parameters[4].Value = 0;
			parameters[5].Value = 0;
			parameters[6].Value = strWhere;	
			return DbHelperSQL.RunProcedure("UP_GetRecordByPage",parameters,"ds");
		}*/

		#endregion  BasicMethod
		#region  ExtensionMethod

		#endregion  ExtensionMethod
	}
}

