/**
* CouponHistory.cs
*
* 功 能： N/A
* 类 名： CouponHistory
*
* Ver    变更日期             负责人  变更内容
* ───────────────────────────────────
* V0.01  2013/8/26 17:20:57   N/A    初版
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
using YSWL.MALL.IDAL.Shop.Coupon;
using YSWL.DBUtility;//Please add references
namespace YSWL.MALL.SQLServerDAL.Shop.Coupon
{
	/// <summary>
	/// 数据访问类:CouponHistory
	/// </summary>
	public partial class CouponHistory:ICouponHistory
	{
		public CouponHistory()
		{}
        #region  BasicMethod

        /// <summary>
        /// 是否存在该记录
        /// </summary>
        public bool Exists(string CouponCode)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select count(1) from Shop_CouponHistory");
            strSql.Append(" where CouponCode=@CouponCode ");
            SqlParameter[] parameters = {
					new SqlParameter("@CouponCode", SqlDbType.NVarChar,200)			};
            parameters[0].Value = CouponCode;

            return DBHelper.DefaultDBHelper.Exists(strSql.ToString(), parameters);
        }


        /// <summary>
        /// 增加一条数据
        /// </summary>
        public bool Add(YSWL.MALL.Model.Shop.Coupon.CouponHistory model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("insert into Shop_CouponHistory(");
            strSql.Append("CouponCode,CategoryId,ProductId,ProductSku,ClassId,SupplierId,RuleId,CouponName,CouponPwd,UserId,UserEmail,Status,CouponPrice,LimitPrice,NeedPoint,IsPwd,IsReuse,StartDate,EndDate,GenerateTime,UsedDate,Type,OrderId,OrderCode)");
            strSql.Append(" values (");
            strSql.Append("@CouponCode,@CategoryId,@ProductId,@ProductSku,@ClassId,@SupplierId,@RuleId,@CouponName,@CouponPwd,@UserId,@UserEmail,@Status,@CouponPrice,@LimitPrice,@NeedPoint,@IsPwd,@IsReuse,@StartDate,@EndDate,@GenerateTime,@UsedDate,@Type,@OrderId,@OrderCode)");
            SqlParameter[] parameters = {
					new SqlParameter("@CouponCode", SqlDbType.NVarChar,200),
					new SqlParameter("@CategoryId", SqlDbType.Int,4),
					new SqlParameter("@ProductId", SqlDbType.BigInt,8),
					new SqlParameter("@ProductSku", SqlDbType.NVarChar,50),
					new SqlParameter("@ClassId", SqlDbType.Int,4),
					new SqlParameter("@SupplierId", SqlDbType.Int,4),
					new SqlParameter("@RuleId", SqlDbType.Int,4),
					new SqlParameter("@CouponName", SqlDbType.NVarChar,200),
					new SqlParameter("@CouponPwd", SqlDbType.NVarChar,200),
					new SqlParameter("@UserId", SqlDbType.Int,4),
					new SqlParameter("@UserEmail", SqlDbType.NVarChar,200),
					new SqlParameter("@Status", SqlDbType.Int,4),
					new SqlParameter("@CouponPrice", SqlDbType.Money,8),
					new SqlParameter("@LimitPrice", SqlDbType.Money,8),
					new SqlParameter("@NeedPoint", SqlDbType.Int,4),
					new SqlParameter("@IsPwd", SqlDbType.Int,4),
					new SqlParameter("@IsReuse", SqlDbType.Int,4),
					new SqlParameter("@StartDate", SqlDbType.DateTime),
					new SqlParameter("@EndDate", SqlDbType.DateTime),
					new SqlParameter("@GenerateTime", SqlDbType.DateTime),
					new SqlParameter("@UsedDate", SqlDbType.DateTime),
					new SqlParameter("@Type", SqlDbType.Int,4),
					new SqlParameter("@OrderId", SqlDbType.BigInt,8),
					new SqlParameter("@OrderCode", SqlDbType.NVarChar,200)};
            parameters[0].Value = model.CouponCode;
            parameters[1].Value = model.CategoryId;
            parameters[2].Value = model.ProductId;
            parameters[3].Value = model.ProductSku;
            parameters[4].Value = model.ClassId;
            parameters[5].Value = model.SupplierId;
            parameters[6].Value = model.RuleId;
            parameters[7].Value = model.CouponName;
            parameters[8].Value = model.CouponPwd;
            parameters[9].Value = model.UserId;
            parameters[10].Value = model.UserEmail;
            parameters[11].Value = model.Status;
            parameters[12].Value = model.CouponPrice;
            parameters[13].Value = model.LimitPrice;
            parameters[14].Value = model.NeedPoint;
            parameters[15].Value = model.IsPwd;
            parameters[16].Value = model.IsReuse;
            parameters[17].Value = model.StartDate;
            parameters[18].Value = model.EndDate;
            parameters[19].Value = model.GenerateTime;
            parameters[20].Value = model.UsedDate;
            parameters[21].Value = model.Type;
            parameters[22].Value = model.OrderId;
            parameters[23].Value = model.OrderCode;

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
        /// <summary>
        /// 更新一条数据
        /// </summary>
        public bool Update(YSWL.MALL.Model.Shop.Coupon.CouponHistory model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update Shop_CouponHistory set ");
            strSql.Append("CategoryId=@CategoryId,");
            strSql.Append("ProductId=@ProductId,");
            strSql.Append("ProductSku=@ProductSku,");
            strSql.Append("ClassId=@ClassId,");
            strSql.Append("SupplierId=@SupplierId,");
            strSql.Append("RuleId=@RuleId,");
            strSql.Append("CouponName=@CouponName,");
            strSql.Append("CouponPwd=@CouponPwd,");
            strSql.Append("UserId=@UserId,");
            strSql.Append("UserEmail=@UserEmail,");
            strSql.Append("Status=@Status,");
            strSql.Append("CouponPrice=@CouponPrice,");
            strSql.Append("LimitPrice=@LimitPrice,");
            strSql.Append("NeedPoint=@NeedPoint,");
            strSql.Append("IsPwd=@IsPwd,");
            strSql.Append("IsReuse=@IsReuse,");
            strSql.Append("StartDate=@StartDate,");
            strSql.Append("EndDate=@EndDate,");
            strSql.Append("GenerateTime=@GenerateTime,");
            strSql.Append("UsedDate=@UsedDate,");
            strSql.Append("Type=@Type,");
            strSql.Append("OrderId=@OrderId,");
            strSql.Append("OrderCode=@OrderCode");
            strSql.Append(" where CouponCode=@CouponCode ");
            SqlParameter[] parameters = {
					new SqlParameter("@CategoryId", SqlDbType.Int,4),
					new SqlParameter("@ProductId", SqlDbType.BigInt,8),
					new SqlParameter("@ProductSku", SqlDbType.NVarChar,50),
					new SqlParameter("@ClassId", SqlDbType.Int,4),
					new SqlParameter("@SupplierId", SqlDbType.Int,4),
					new SqlParameter("@RuleId", SqlDbType.Int,4),
					new SqlParameter("@CouponName", SqlDbType.NVarChar,200),
					new SqlParameter("@CouponPwd", SqlDbType.NVarChar,200),
					new SqlParameter("@UserId", SqlDbType.Int,4),
					new SqlParameter("@UserEmail", SqlDbType.NVarChar,200),
					new SqlParameter("@Status", SqlDbType.Int,4),
					new SqlParameter("@CouponPrice", SqlDbType.Money,8),
					new SqlParameter("@LimitPrice", SqlDbType.Money,8),
					new SqlParameter("@NeedPoint", SqlDbType.Int,4),
					new SqlParameter("@IsPwd", SqlDbType.Int,4),
					new SqlParameter("@IsReuse", SqlDbType.Int,4),
					new SqlParameter("@StartDate", SqlDbType.DateTime),
					new SqlParameter("@EndDate", SqlDbType.DateTime),
					new SqlParameter("@GenerateTime", SqlDbType.DateTime),
					new SqlParameter("@UsedDate", SqlDbType.DateTime),
					new SqlParameter("@Type", SqlDbType.Int,4),
					new SqlParameter("@OrderId", SqlDbType.BigInt,8),
					new SqlParameter("@OrderCode", SqlDbType.NVarChar,200),
					new SqlParameter("@CouponCode", SqlDbType.NVarChar,200)};
            parameters[0].Value = model.CategoryId;
            parameters[1].Value = model.ProductId;
            parameters[2].Value = model.ProductSku;
            parameters[3].Value = model.ClassId;
            parameters[4].Value = model.SupplierId;
            parameters[5].Value = model.RuleId;
            parameters[6].Value = model.CouponName;
            parameters[7].Value = model.CouponPwd;
            parameters[8].Value = model.UserId;
            parameters[9].Value = model.UserEmail;
            parameters[10].Value = model.Status;
            parameters[11].Value = model.CouponPrice;
            parameters[12].Value = model.LimitPrice;
            parameters[13].Value = model.NeedPoint;
            parameters[14].Value = model.IsPwd;
            parameters[15].Value = model.IsReuse;
            parameters[16].Value = model.StartDate;
            parameters[17].Value = model.EndDate;
            parameters[18].Value = model.GenerateTime;
            parameters[19].Value = model.UsedDate;
            parameters[20].Value = model.Type;
            parameters[21].Value = model.OrderId;
            parameters[22].Value = model.OrderCode;
            parameters[23].Value = model.CouponCode;

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

        /// <summary>
        /// 删除一条数据
        /// </summary>
        public bool Delete(string CouponCode)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append("delete from Shop_CouponHistory ");
            strSql.Append(" where CouponCode=@CouponCode ");
            SqlParameter[] parameters = {
					new SqlParameter("@CouponCode", SqlDbType.NVarChar,200)			};
            parameters[0].Value = CouponCode;

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
        /// <summary>
        /// 批量删除数据
        /// </summary>
        public bool DeleteList(string CouponCodelist)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("delete from Shop_CouponHistory ");
            strSql.Append(" where CouponCode in (" + CouponCodelist + ")  ");
            int rows = DBHelper.DefaultDBHelper.ExecuteSql(strSql.ToString());
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
        public YSWL.MALL.Model.Shop.Coupon.CouponHistory GetModel(string CouponCode)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append("select  top 1 CouponCode,CategoryId,ProductId,ProductSku,ClassId,SupplierId,RuleId,CouponName,CouponPwd,UserId,UserEmail,Status,CouponPrice,LimitPrice,NeedPoint,IsPwd,IsReuse,StartDate,EndDate,GenerateTime,UsedDate,Type,OrderId,OrderCode from Shop_CouponHistory ");
            strSql.Append(" where CouponCode=@CouponCode ");
            SqlParameter[] parameters = {
					new SqlParameter("@CouponCode", SqlDbType.NVarChar,200)			};
            parameters[0].Value = CouponCode;

            YSWL.MALL.Model.Shop.Coupon.CouponHistory model = new YSWL.MALL.Model.Shop.Coupon.CouponHistory();
            DataSet ds = DBHelper.DefaultDBHelper.Query(strSql.ToString(), parameters);
            if (ds.Tables[0].Rows.Count > 0)
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
        public YSWL.MALL.Model.Shop.Coupon.CouponHistory DataRowToModel(DataRow row)
        {
            YSWL.MALL.Model.Shop.Coupon.CouponHistory model = new YSWL.MALL.Model.Shop.Coupon.CouponHistory();
            if (row != null)
            {
                if (row["CouponCode"] != null)
                {
                    model.CouponCode = row["CouponCode"].ToString();
                }
                if (row["CategoryId"] != null && row["CategoryId"].ToString() != "")
                {
                    model.CategoryId = int.Parse(row["CategoryId"].ToString());
                }
                if (row["ProductId"] != null && row["ProductId"].ToString() != "")
                {
                    model.ProductId = long.Parse(row["ProductId"].ToString());
                }
                if (row["ProductSku"] != null)
                {
                    model.ProductSku = row["ProductSku"].ToString();
                }
                if (row["ClassId"] != null && row["ClassId"].ToString() != "")
                {
                    model.ClassId = int.Parse(row["ClassId"].ToString());
                }
                if (row["SupplierId"] != null && row["SupplierId"].ToString() != "")
                {
                    model.SupplierId = int.Parse(row["SupplierId"].ToString());
                }
                if (row["RuleId"] != null && row["RuleId"].ToString() != "")
                {
                    model.RuleId = int.Parse(row["RuleId"].ToString());
                }
                if (row["CouponName"] != null)
                {
                    model.CouponName = row["CouponName"].ToString();
                }
                if (row["CouponPwd"] != null)
                {
                    model.CouponPwd = row["CouponPwd"].ToString();
                }
                if (row["UserId"] != null && row["UserId"].ToString() != "")
                {
                    model.UserId = int.Parse(row["UserId"].ToString());
                }
                if (row["UserEmail"] != null)
                {
                    model.UserEmail = row["UserEmail"].ToString();
                }
                if (row["Status"] != null && row["Status"].ToString() != "")
                {
                    model.Status = int.Parse(row["Status"].ToString());
                }
                if (row["CouponPrice"] != null && row["CouponPrice"].ToString() != "")
                {
                    model.CouponPrice = decimal.Parse(row["CouponPrice"].ToString());
                }
                if (row["LimitPrice"] != null && row["LimitPrice"].ToString() != "")
                {
                    model.LimitPrice = decimal.Parse(row["LimitPrice"].ToString());
                }
                if (row["NeedPoint"] != null && row["NeedPoint"].ToString() != "")
                {
                    model.NeedPoint = int.Parse(row["NeedPoint"].ToString());
                }
                if (row["IsPwd"] != null && row["IsPwd"].ToString() != "")
                {
                    model.IsPwd = int.Parse(row["IsPwd"].ToString());
                }
                if (row["IsReuse"] != null && row["IsReuse"].ToString() != "")
                {
                    model.IsReuse = int.Parse(row["IsReuse"].ToString());
                }
                if (row["StartDate"] != null && row["StartDate"].ToString() != "")
                {
                    model.StartDate = DateTime.Parse(row["StartDate"].ToString());
                }
                if (row["EndDate"] != null && row["EndDate"].ToString() != "")
                {
                    model.EndDate = DateTime.Parse(row["EndDate"].ToString());
                }
                if (row["GenerateTime"] != null && row["GenerateTime"].ToString() != "")
                {
                    model.GenerateTime = DateTime.Parse(row["GenerateTime"].ToString());
                }
                if (row["UsedDate"] != null && row["UsedDate"].ToString() != "")
                {
                    model.UsedDate = DateTime.Parse(row["UsedDate"].ToString());
                }
                if (row["Type"] != null && row["Type"].ToString() != "")
                {
                    model.Type = int.Parse(row["Type"].ToString());
                }
                if (row["OrderId"] != null && row["OrderId"].ToString() != "")
                {
                    model.OrderId = long.Parse(row["OrderId"].ToString());
                }
                if (row["OrderCode"] != null)
                {
                    model.OrderCode = row["OrderCode"].ToString();
                }
            }
            return model;
        }

        /// <summary>
        /// 获得数据列表
        /// </summary>
        public DataSet GetList(string strWhere)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select CouponCode,CategoryId,ProductId,ProductSku,ClassId,SupplierId,RuleId,CouponName,CouponPwd,UserId,UserEmail,Status,CouponPrice,LimitPrice,NeedPoint,IsPwd,IsReuse,StartDate,EndDate,GenerateTime,UsedDate,Type,OrderId,OrderCode ");
            strSql.Append(" FROM Shop_CouponHistory ");
            if (strWhere.Trim() != "")
            {
                strSql.Append(" where " + strWhere);
            }
            return DBHelper.DefaultDBHelper.Query(strSql.ToString());
        }

        /// <summary>
        /// 获得前几行数据
        /// </summary>
        public DataSet GetList(int Top, string strWhere, string filedOrder)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select ");
            if (Top > 0)
            {
                strSql.Append(" top " + Top.ToString());
            }
            strSql.Append(" CouponCode,CategoryId,ProductId,ProductSku,ClassId,SupplierId,RuleId,CouponName,CouponPwd,UserId,UserEmail,Status,CouponPrice,LimitPrice,NeedPoint,IsPwd,IsReuse,StartDate,EndDate,GenerateTime,UsedDate,Type,OrderId,OrderCode ");
            strSql.Append(" FROM Shop_CouponHistory ");
            if (strWhere.Trim() != "")
            {
                strSql.Append(" where " + strWhere);
            }
            strSql.Append(" order by " + filedOrder);
            return DBHelper.DefaultDBHelper.Query(strSql.ToString());
        }

        /// <summary>
        /// 获取记录总数
        /// </summary>
        public int GetRecordCount(string strWhere)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select count(1) FROM Shop_CouponHistory ");
            if (strWhere.Trim() != "")
            {
                strSql.Append(" where " + strWhere);
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
            StringBuilder strSql = new StringBuilder();
            strSql.Append("SELECT * FROM ( ");
            strSql.Append(" SELECT ROW_NUMBER() OVER (");
            if (!string.IsNullOrEmpty(orderby.Trim()))
            {
                strSql.Append("order by T." + orderby);
            }
            else
            {
                strSql.Append("order by T.CouponCode desc");
            }
            strSql.Append(")AS Row, T.*  from Shop_CouponHistory T ");
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
            parameters[0].Value = "Shop_CouponHistory";
            parameters[1].Value = "CouponCode";
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

