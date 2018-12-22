/**
* SalesRuleProduct.cs
*
* 功 能： N/A
* 类 名： SalesRuleProduct
*
* Ver    变更日期             负责人  变更内容
* ───────────────────────────────────
* V0.01  2013/6/8 18:54:58   N/A    初版
*
* Copyright (c) 2012-2013 YS56 Corporation. All rights reserved.
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
using System.Linq;
using YSWL.MALL.IDAL.Shop.Sales;
using YSWL.DBUtility;//Please add references
namespace YSWL.MALL.SQLServerDAL.Shop.Sales
{
	/// <summary>
	/// 数据访问类:SalesRuleProduct
	/// </summary>
	public partial class SalesRuleProduct:ISalesRuleProduct
	{
		public SalesRuleProduct()
		{}
		#region  BasicMethod

		/// <summary>
		/// 得到最大ID
		/// </summary>
		public int GetMaxId()
		{
		return DBHelper.DefaultDBHelper.GetMaxID("RuleId", "Shop_SalesRuleProduct"); 
		}

		/// <summary>
		/// 是否存在该记录
		/// </summary>
		public bool Exists(int RuleId,long ProductId)
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("select count(1) from Shop_SalesRuleProduct");
			strSql.Append(" where RuleId=@RuleId and ProductId=@ProductId ");
			SqlParameter[] parameters = {
					new SqlParameter("@RuleId", SqlDbType.Int,4),
					new SqlParameter("@ProductId", SqlDbType.BigInt,8)			};
			parameters[0].Value = RuleId;
			parameters[1].Value = ProductId;

			return DBHelper.DefaultDBHelper.Exists(strSql.ToString(),parameters);
		}


		/// <summary>
		/// 增加一条数据
		/// </summary>
		public bool Add(YSWL.MALL.Model.Shop.Sales.SalesRuleProduct model)
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("insert into Shop_SalesRuleProduct(");
			strSql.Append("RuleId,ProductId,ProductName)");
			strSql.Append(" values (");
			strSql.Append("@RuleId,@ProductId,@ProductName)");
			SqlParameter[] parameters = {
					new SqlParameter("@RuleId", SqlDbType.Int,4),
					new SqlParameter("@ProductId", SqlDbType.BigInt,8),
					new SqlParameter("@ProductName", SqlDbType.NVarChar,200)};
			parameters[0].Value = model.RuleId;
			parameters[1].Value = model.ProductId;
			parameters[2].Value = model.ProductName;

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
		public bool Update(YSWL.MALL.Model.Shop.Sales.SalesRuleProduct model)
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("update Shop_SalesRuleProduct set ");
			strSql.Append("ProductName=@ProductName");
			strSql.Append(" where RuleId=@RuleId and ProductId=@ProductId ");
			SqlParameter[] parameters = {
					new SqlParameter("@ProductName", SqlDbType.NVarChar,200),
					new SqlParameter("@RuleId", SqlDbType.Int,4),
					new SqlParameter("@ProductId", SqlDbType.BigInt,8)};
			parameters[0].Value = model.ProductName;
			parameters[1].Value = model.RuleId;
			parameters[2].Value = model.ProductId;

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
		public bool Delete(int RuleId,long ProductId)
		{
			
			StringBuilder strSql=new StringBuilder();
			strSql.Append("delete from Shop_SalesRuleProduct ");
			strSql.Append(" where RuleId=@RuleId and ProductId=@ProductId ");
			SqlParameter[] parameters = {
					new SqlParameter("@RuleId", SqlDbType.Int,4),
					new SqlParameter("@ProductId", SqlDbType.BigInt,8)			};
			parameters[0].Value = RuleId;
			parameters[1].Value = ProductId;

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
		public YSWL.MALL.Model.Shop.Sales.SalesRuleProduct GetModel(int RuleId,long ProductId)
		{
			
			StringBuilder strSql=new StringBuilder();
			strSql.Append("select  top 1 RuleId,ProductId,ProductName from Shop_SalesRuleProduct ");
			strSql.Append(" where RuleId=@RuleId and ProductId=@ProductId ");
			SqlParameter[] parameters = {
					new SqlParameter("@RuleId", SqlDbType.Int,4),
					new SqlParameter("@ProductId", SqlDbType.BigInt,8)			};
			parameters[0].Value = RuleId;
			parameters[1].Value = ProductId;

			YSWL.MALL.Model.Shop.Sales.SalesRuleProduct model=new YSWL.MALL.Model.Shop.Sales.SalesRuleProduct();
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
		public YSWL.MALL.Model.Shop.Sales.SalesRuleProduct DataRowToModel(DataRow row)
		{
			YSWL.MALL.Model.Shop.Sales.SalesRuleProduct model=new YSWL.MALL.Model.Shop.Sales.SalesRuleProduct();
			if (row != null)
			{
				if(row["RuleId"]!=null && row["RuleId"].ToString()!="")
				{
					model.RuleId=int.Parse(row["RuleId"].ToString());
				}
				if(row["ProductId"]!=null && row["ProductId"].ToString()!="")
				{
					model.ProductId=long.Parse(row["ProductId"].ToString());
				}
				if(row["ProductName"]!=null)
				{
					model.ProductName=row["ProductName"].ToString();
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
			strSql.Append("select RuleId,ProductId,ProductName ");
			strSql.Append(" FROM Shop_SalesRuleProduct ");
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
			strSql.Append(" RuleId,ProductId,ProductName ");
			strSql.Append(" FROM Shop_SalesRuleProduct ");
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
			strSql.Append("select count(1) FROM Shop_SalesRuleProduct ");
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
				strSql.Append("order by T.ProductId desc");
			}
			strSql.Append(")AS Row, T.*  from Shop_SalesRuleProduct T ");
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
			parameters[0].Value = "Shop_SalesRuleProduct";
			parameters[1].Value = "ProductId";
			parameters[2].Value = PageSize;
			parameters[3].Value = PageIndex;
			parameters[4].Value = 0;
			parameters[5].Value = 0;
			parameters[6].Value = strWhere;	
			return DBHelper.DefaultDBHelper.RunProcedure("UP_GetRecordByPage",parameters,"ds");
		}*/

		#endregion  BasicMethod
		#region  ExtensionMethod

	    public bool DeleteByRule(int RuleId)
	    {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("delete from Shop_SalesRuleProduct ");
            strSql.Append(" where RuleId=@RuleId ");
            SqlParameter[] parameters = {
					new SqlParameter("@RuleId", SqlDbType.Int,4)		};
            parameters[0].Value = RuleId;

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


        public DataSet GetRuleProducts(int ruleId, string strWhere)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("SELECT PSM.ProductId ");
            strSql.Append("FROM Shop_SalesRuleProduct PSM ");
            strSql.Append("INNER JOIN (SELECT ProductId FROM  PMS_Products P ");
            strSql.Append("WHERE P.SaleStatus=1 ");
            if (!string.IsNullOrWhiteSpace(strWhere))
            {
                strSql.Append(strWhere);
            }
            strSql.Append(")A ON PSM.ProductId = A.ProductId ");
            strSql.Append(" WHERE RuleId=@RuleId ");
            SqlParameter[] parameters =
                {
                    new SqlParameter("@RuleId", SqlDbType.Int, 4)
                };
            parameters[0].Value = ruleId;
            return DBHelper.DefaultDBHelper.Query(strSql.ToString(), parameters);
        }


        public DataSet GetRuleProList(long productId)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append(" SELECT *  FROM Shop_SalesRuleProduct  ");
            strSql.AppendFormat(" WHERE ProductId={0} AND EXISTS( SELECT *  FROM Shop_SalesRule WHERE RuleId=Shop_SalesRuleProduct.RuleId AND Status=1) ", productId);
            SqlParameter[] parameters = {
					new SqlParameter("@ProductId", SqlDbType.BigInt,8)			};
            parameters[0].Value = productId;
            return DBHelper.DefaultDBHelper.Query(strSql.ToString(), parameters);
        }


        /// <summary>
        /// 批量添加活动数据
        /// </summary>
        /// <param name="name"></param>
        /// <param name="categoryId"></param>
        /// <param name="status"></param>
        /// <returns></returns>
        public int AddList(int ruleId, string name, int categoryId, int status = 1)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append(" INSERT  INTO Shop_SalesRuleProduct ( RuleId ,  ProductId , ProductName  )  ");
            strSql.AppendFormat("    SELECT  {0} , ProductId , ProductName   FROM    PMS_Products", ruleId);
            strSql.AppendFormat(" where  NOT EXISTS(SELECT *  FROM  Shop_SalesRuleProduct WHERE   ProductId=PMS_Products.ProductId " );
            strSql.AppendFormat(
             "  AND EXISTS ( SELECT * FROM   Shop_SalesUserRank A WHERE  EXISTS ( SELECT  * FROM  Shop_SalesUserRank B  WHERE A.RankId = b.RankId  AND B.RuleId = {0} ) AND A.RuleId = Shop_SalesRuleProduct.RuleId )  )",
             ruleId);
            //排除商家商品
            strSql.AppendFormat(" and  SupplierId<=0");
            if (status > -1)
            {
                strSql.AppendFormat(" and  SaleStatus={0}",status);
            }
            if (!string.IsNullOrWhiteSpace(name))
            {
                strSql.AppendFormat(" AND ProductName LIKE '%{0}%'", Common.InjectionFilter.SqlFilter(name));
            }
            if (categoryId > 0)
            {
                strSql.AppendFormat(" AND ProductId IN( SELECT DISTINCT ProductId FROM PMS_ProductCategories WHERE (CategoryPath LIKE (SELECT Path FROM PMS_Categories WHERE CategoryId={0})+'|%' or CategoryId={0}) ) ", categoryId);
            }
            int rows = DBHelper.DefaultDBHelper.ExecuteSql(strSql.ToString());
            return rows;
        }


        public bool ExistsProduct(int ruleId, long productId)
	    {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select count(1) from Shop_SalesRuleProduct");
            strSql.Append("  WHERE    EXISTS ( SELECT * FROM   Shop_SalesUserRank A  WHERE  ");
            strSql.AppendFormat("  EXISTS ( SELECT * FROM   Shop_SalesUserRank B WHERE  A.RankId = b.RankId AND B.RuleId = {0} )  ",ruleId);
            strSql.AppendFormat("   AND A.RuleId = Shop_SalesRuleProduct.RuleId ) AND ProductId={0}  ", productId);
            return DBHelper.DefaultDBHelper.Exists(strSql.ToString());
	    }

        #region saas app

        /// <summary>
        /// 批量添加商品规则
        /// </summary>
        /// <param name="productItem"></param>
        /// <returns></returns>
        public bool AddSaleRuleBatch(List<Model.Shop.Sales.SalesRuleProduct> productItem)
        {
            if (productItem.Any())
            {
                List<CommandInfo> sqllist = new List<CommandInfo>();
                CommandInfo cmd;
                foreach (Model.Shop.Sales.SalesRuleProduct model in productItem)
                {
                    StringBuilder strSql = new StringBuilder();
                    strSql.Append("insert into Shop_SalesRuleProduct(");
                    strSql.Append("RuleId,ProductId,ProductName)");
                    strSql.Append(" values (");
                    strSql.Append("@RuleId,@ProductId,@ProductName)");
                    SqlParameter[] parameters = {
                    new SqlParameter("@RuleId", SqlDbType.Int,4),
                    new SqlParameter("@ProductId", SqlDbType.BigInt,8),
                    new SqlParameter("@ProductName", SqlDbType.NVarChar,200)};
                    parameters[0].Value = model.RuleId;
                    parameters[1].Value = model.ProductId;
                    parameters[2].Value = model.ProductName;
                    cmd = new CommandInfo(strSql.ToString(), parameters, EffentNextType.ExcuteEffectRows);
                    sqllist.Add(cmd);
                }
                int rows = DBHelper.DefaultDBHelper.ExecuteSqlTran(sqllist);
                return rows > 0;
            }
            return true;
        }

        /// <summary>
        /// 批量添加商品规则
        /// </summary>
        /// <param name="productItem"></param>
        /// <returns></returns>
        public bool DeleteSaleRuleBatch(List<Model.Shop.Sales.SalesRuleProduct> productItem)
        {
            if (productItem.Any())
            {
                List<CommandInfo> sqllist = new List<CommandInfo>();
                CommandInfo cmd;
                foreach (Model.Shop.Sales.SalesRuleProduct model in productItem)
                {
                    StringBuilder strSql = new StringBuilder();
                    strSql.Append("delete from Shop_SalesRuleProduct");
                    strSql.Append(" where RuleId=@RuleId and ProductId=@ProductId");
                    SqlParameter[] parameters = {
                    new SqlParameter("@RuleId", SqlDbType.Int,4),
                    new SqlParameter("@ProductId", SqlDbType.BigInt,8)};
                    parameters[0].Value = model.RuleId;
                    parameters[1].Value = model.ProductId;
                    cmd = new CommandInfo(strSql.ToString(), parameters, EffentNextType.ExcuteEffectRows);
                    sqllist.Add(cmd);
                }
                int rows = DBHelper.DefaultDBHelper.ExecuteSqlTran(sqllist);
                return rows > 0;
            }
            return true;
        }

        #endregion



        #endregion  ExtensionMethod
    }
}

