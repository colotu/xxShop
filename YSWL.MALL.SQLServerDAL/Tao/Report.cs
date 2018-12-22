using System;
using System.Data;
using System.Text;
using System.Data.SqlClient;
using YSWL.IDAL.Tao;
using YSWL.DBUtility;//Please add references
namespace YSWL.SQLServerDAL.Tao
{
	/// <summary>
	/// 数据访问类:Report
	/// </summary>
	public partial class Report:IReport
	{
		public Report()
		{}
		#region  BasicMethod

		/// <summary>
		/// 得到最大ID
		/// </summary>
		public int GetMaxId()
		{
		return DbHelperSQL.GetMaxID("ReportId", "Tao_Report"); 
		}

		/// <summary>
		/// 是否存在该记录
		/// </summary>
		public bool Exists(int ReportId)
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("select count(1) from Tao_Report");
			strSql.Append(" where ReportId=@ReportId");
			SqlParameter[] parameters = {
					new SqlParameter("@ReportId", SqlDbType.Int,4)
			};
			parameters[0].Value = ReportId;

			return DbHelperSQL.Exists(strSql.ToString(),parameters);
		}


		/// <summary>
		/// 增加一条数据
		/// </summary>
		public int Add(YSWL.Model.Tao.Report model)
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("insert into Tao_Report(");
			strSql.Append("UserId,TradeNo,RealPayFee,CommissionRate,Commission,Rebate,PayTime,PayPrice,ProductId,ProductName,ProductNum,ShopName,SellerName,CategoryId,CategoryName,Status)");
			strSql.Append(" values (");
			strSql.Append("@UserId,@TradeNo,@RealPayFee,@CommissionRate,@Commission,@Rebate,@PayTime,@PayPrice,@ProductId,@ProductName,@ProductNum,@ShopName,@SellerName,@CategoryId,@CategoryName,@Status)");
			strSql.Append(";select @@IDENTITY");
			SqlParameter[] parameters = {
					new SqlParameter("@UserId", SqlDbType.Int,4),
					new SqlParameter("@TradeNo", SqlDbType.Int,4),
					new SqlParameter("@RealPayFee", SqlDbType.Decimal,9),
					new SqlParameter("@CommissionRate", SqlDbType.NVarChar,200),
					new SqlParameter("@Commission", SqlDbType.Decimal,9),
					new SqlParameter("@Rebate", SqlDbType.Decimal,9),
					new SqlParameter("@PayTime", SqlDbType.DateTime),
					new SqlParameter("@PayPrice", SqlDbType.Decimal,9),
					new SqlParameter("@ProductId", SqlDbType.Int,4),
					new SqlParameter("@ProductName", SqlDbType.NVarChar,300),
					new SqlParameter("@ProductNum", SqlDbType.Int,4),
					new SqlParameter("@ShopName", SqlDbType.NVarChar,300),
					new SqlParameter("@SellerName", SqlDbType.NVarChar,300),
					new SqlParameter("@CategoryId", SqlDbType.Int,4),
					new SqlParameter("@CategoryName", SqlDbType.NVarChar,300),
					new SqlParameter("@Status", SqlDbType.Int,4)};
			parameters[0].Value = model.UserId;
			parameters[1].Value = model.TradeNo;
			parameters[2].Value = model.RealPayFee;
			parameters[3].Value = model.CommissionRate;
			parameters[4].Value = model.Commission;
			parameters[5].Value = model.Rebate;
			parameters[6].Value = model.PayTime;
			parameters[7].Value = model.PayPrice;
			parameters[8].Value = model.ProductId;
			parameters[9].Value = model.ProductName;
			parameters[10].Value = model.ProductNum;
			parameters[11].Value = model.ShopName;
			parameters[12].Value = model.SellerName;
			parameters[13].Value = model.CategoryId;
			parameters[14].Value = model.CategoryName;
			parameters[15].Value = model.Status;

			object obj = DbHelperSQL.GetSingle(strSql.ToString(),parameters);
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
		public bool Update(YSWL.Model.Tao.Report model)
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("update Tao_Report set ");
			strSql.Append("UserId=@UserId,");
			strSql.Append("TradeNo=@TradeNo,");
			strSql.Append("RealPayFee=@RealPayFee,");
			strSql.Append("CommissionRate=@CommissionRate,");
			strSql.Append("Commission=@Commission,");
			strSql.Append("Rebate=@Rebate,");
			strSql.Append("PayTime=@PayTime,");
			strSql.Append("PayPrice=@PayPrice,");
			strSql.Append("ProductId=@ProductId,");
			strSql.Append("ProductName=@ProductName,");
			strSql.Append("ProductNum=@ProductNum,");
			strSql.Append("ShopName=@ShopName,");
			strSql.Append("SellerName=@SellerName,");
			strSql.Append("CategoryId=@CategoryId,");
			strSql.Append("CategoryName=@CategoryName,");
			strSql.Append("Status=@Status");
			strSql.Append(" where ReportId=@ReportId");
			SqlParameter[] parameters = {
					new SqlParameter("@UserId", SqlDbType.Int,4),
					new SqlParameter("@TradeNo", SqlDbType.Int,4),
					new SqlParameter("@RealPayFee", SqlDbType.Decimal,9),
					new SqlParameter("@CommissionRate", SqlDbType.NVarChar,200),
					new SqlParameter("@Commission", SqlDbType.Decimal,9),
					new SqlParameter("@Rebate", SqlDbType.Decimal,9),
					new SqlParameter("@PayTime", SqlDbType.DateTime),
					new SqlParameter("@PayPrice", SqlDbType.Decimal,9),
					new SqlParameter("@ProductId", SqlDbType.Int,4),
					new SqlParameter("@ProductName", SqlDbType.NVarChar,300),
					new SqlParameter("@ProductNum", SqlDbType.Int,4),
					new SqlParameter("@ShopName", SqlDbType.NVarChar,300),
					new SqlParameter("@SellerName", SqlDbType.NVarChar,300),
					new SqlParameter("@CategoryId", SqlDbType.Int,4),
					new SqlParameter("@CategoryName", SqlDbType.NVarChar,300),
					new SqlParameter("@Status", SqlDbType.Int,4),
					new SqlParameter("@ReportId", SqlDbType.Int,4)};
			parameters[0].Value = model.UserId;
			parameters[1].Value = model.TradeNo;
			parameters[2].Value = model.RealPayFee;
			parameters[3].Value = model.CommissionRate;
			parameters[4].Value = model.Commission;
			parameters[5].Value = model.Rebate;
			parameters[6].Value = model.PayTime;
			parameters[7].Value = model.PayPrice;
			parameters[8].Value = model.ProductId;
			parameters[9].Value = model.ProductName;
			parameters[10].Value = model.ProductNum;
			parameters[11].Value = model.ShopName;
			parameters[12].Value = model.SellerName;
			parameters[13].Value = model.CategoryId;
			parameters[14].Value = model.CategoryName;
			parameters[15].Value = model.Status;
			parameters[16].Value = model.ReportId;

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
		public bool Delete(int ReportId)
		{
			
			StringBuilder strSql=new StringBuilder();
			strSql.Append("delete from Tao_Report ");
			strSql.Append(" where ReportId=@ReportId");
			SqlParameter[] parameters = {
					new SqlParameter("@ReportId", SqlDbType.Int,4)
			};
			parameters[0].Value = ReportId;

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
		public bool DeleteList(string ReportIdlist )
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("delete from Tao_Report ");
			strSql.Append(" where ReportId in ("+ReportIdlist + ")  ");
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
		public YSWL.Model.Tao.Report GetModel(int ReportId)
		{
			
			StringBuilder strSql=new StringBuilder();
			strSql.Append("select  top 1 ReportId,UserId,TradeNo,RealPayFee,CommissionRate,Commission,Rebate,PayTime,PayPrice,ProductId,ProductName,ProductNum,ShopName,SellerName,CategoryId,CategoryName,Status from Tao_Report ");
			strSql.Append(" where ReportId=@ReportId");
			SqlParameter[] parameters = {
					new SqlParameter("@ReportId", SqlDbType.Int,4)
			};
			parameters[0].Value = ReportId;

			YSWL.Model.Tao.Report model=new YSWL.Model.Tao.Report();
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
		public YSWL.Model.Tao.Report DataRowToModel(DataRow row)
		{
			YSWL.Model.Tao.Report model=new YSWL.Model.Tao.Report();
			if (row != null)
			{
				if(row["ReportId"]!=null && row["ReportId"].ToString()!="")
				{
					model.ReportId=int.Parse(row["ReportId"].ToString());
				}
				if(row["UserId"]!=null && row["UserId"].ToString()!="")
				{
					model.UserId=int.Parse(row["UserId"].ToString());
				}
				if(row["TradeNo"]!=null && row["TradeNo"].ToString()!="")
				{
					model.TradeNo=int.Parse(row["TradeNo"].ToString());
				}
				if(row["RealPayFee"]!=null && row["RealPayFee"].ToString()!="")
				{
					model.RealPayFee=decimal.Parse(row["RealPayFee"].ToString());
				}
				if(row["CommissionRate"]!=null)
				{
					model.CommissionRate=row["CommissionRate"].ToString();
				}
				if(row["Commission"]!=null && row["Commission"].ToString()!="")
				{
					model.Commission=decimal.Parse(row["Commission"].ToString());
				}
				if(row["Rebate"]!=null && row["Rebate"].ToString()!="")
				{
					model.Rebate=decimal.Parse(row["Rebate"].ToString());
				}
				if(row["PayTime"]!=null && row["PayTime"].ToString()!="")
				{
					model.PayTime=DateTime.Parse(row["PayTime"].ToString());
				}
				if(row["PayPrice"]!=null && row["PayPrice"].ToString()!="")
				{
					model.PayPrice=decimal.Parse(row["PayPrice"].ToString());
				}
				if(row["ProductId"]!=null && row["ProductId"].ToString()!="")
				{
					model.ProductId=int.Parse(row["ProductId"].ToString());
				}
				if(row["ProductName"]!=null)
				{
					model.ProductName=row["ProductName"].ToString();
				}
				if(row["ProductNum"]!=null && row["ProductNum"].ToString()!="")
				{
					model.ProductNum=int.Parse(row["ProductNum"].ToString());
				}
				if(row["ShopName"]!=null)
				{
					model.ShopName=row["ShopName"].ToString();
				}
				if(row["SellerName"]!=null)
				{
					model.SellerName=row["SellerName"].ToString();
				}
				if(row["CategoryId"]!=null && row["CategoryId"].ToString()!="")
				{
					model.CategoryId=int.Parse(row["CategoryId"].ToString());
				}
				if(row["CategoryName"]!=null)
				{
					model.CategoryName=row["CategoryName"].ToString();
				}
				if(row["Status"]!=null && row["Status"].ToString()!="")
				{
					model.Status=int.Parse(row["Status"].ToString());
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
			strSql.Append("select ReportId,UserId,TradeNo,RealPayFee,CommissionRate,Commission,Rebate,PayTime,PayPrice,ProductId,ProductName,ProductNum,ShopName,SellerName,CategoryId,CategoryName,Status ");
			strSql.Append(" FROM Tao_Report ");
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
			strSql.Append(" ReportId,UserId,TradeNo,RealPayFee,CommissionRate,Commission,Rebate,PayTime,PayPrice,ProductId,ProductName,ProductNum,ShopName,SellerName,CategoryId,CategoryName,Status ");
			strSql.Append(" FROM Tao_Report ");
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
			strSql.Append("select count(1) FROM Tao_Report ");
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
				strSql.Append("order by T.ReportId desc");
			}
			strSql.Append(")AS Row, T.*  from Tao_Report T ");
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
			parameters[0].Value = "Tao_Report";
			parameters[1].Value = "ReportId";
			parameters[2].Value = PageSize;
			parameters[3].Value = PageIndex;
			parameters[4].Value = 0;
			parameters[5].Value = 0;
			parameters[6].Value = strWhere;	
			return DbHelperSQL.RunProcedure("UP_GetRecordByPage",parameters,"ds");
		}*/

		#endregion  BasicMethod
		#region  ExtensionMethod
        public DataSet GetUserRankMonth()
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("SELECT top 9   ROW_NUMBER() OVER (order by SumRebate desc) as Num,temp.* from");
            strSql.Append(" (select  UserName,SumRebate=(select Sum(Rebate) from Tao_Report where UserId=u.UserID and Status=1)  ");
            strSql.Append("FROM Accounts_Users u where UserType<>'AA') temp  ");
            return DbHelperSQL.Query(strSql.ToString());
        }
		#endregion  ExtensionMethod
	}
}

