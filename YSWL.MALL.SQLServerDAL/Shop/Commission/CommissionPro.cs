/**  版本信息模板在安装目录下，可自行修改。
* CommissionPro.cs
*
* 功 能： N/A
* 类 名： CommissionPro
*
* Ver    变更日期             负责人  变更内容
* ───────────────────────────────────
* V0.01  2015/2/13 13:59:34   N/A    初版
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
using YSWL.MALL.IDAL.Shop.Commission;
using YSWL.DBUtility;//Please add references
namespace YSWL.MALL.SQLServerDAL.Shop.Commission
{
	/// <summary>
	/// 数据访问类:CommissionPro
	/// </summary>
	public partial class CommissionPro:ICommissionPro
	{
		public CommissionPro()
		{}
		#region  BasicMethod

		/// <summary>
		/// 得到最大ID
		/// </summary>
		public int GetMaxId()
		{
		return DBHelper.DefaultDBHelper.GetMaxID("RuleId", "Shop_CommissionPro"); 
		}

		/// <summary>
		/// 是否存在该记录
		/// </summary>
		public bool Exists(int RuleId,long ProductId)
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("select count(1) from Shop_CommissionPro");
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
		public bool Add(YSWL.MALL.Model.Shop.Commission.CommissionPro model)
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("insert into Shop_CommissionPro(");
			strSql.Append("RuleId,ProductId)");
			strSql.Append(" values (");
			strSql.Append("@RuleId,@ProductId)");
			SqlParameter[] parameters = {
					new SqlParameter("@RuleId", SqlDbType.Int,4),
					new SqlParameter("@ProductId", SqlDbType.BigInt,8)};
			parameters[0].Value = model.RuleId;
			parameters[1].Value = model.ProductId;

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
		public bool Update(YSWL.MALL.Model.Shop.Commission.CommissionPro model)
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("update Shop_CommissionPro set ");
			strSql.Append("RuleId=@RuleId,");
			strSql.Append("ProductId=@ProductId");
			strSql.Append(" where RuleId=@RuleId and ProductId=@ProductId ");
			SqlParameter[] parameters = {
					new SqlParameter("@RuleId", SqlDbType.Int,4),
					new SqlParameter("@ProductId", SqlDbType.BigInt,8)};
			parameters[0].Value = model.RuleId;
			parameters[1].Value = model.ProductId;

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
			strSql.Append("delete from Shop_CommissionPro ");
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
		public YSWL.MALL.Model.Shop.Commission.CommissionPro GetModel(int RuleId,long ProductId)
		{
			
			StringBuilder strSql=new StringBuilder();
			strSql.Append("select  top 1 RuleId,ProductId from Shop_CommissionPro ");
			strSql.Append(" where RuleId=@RuleId and ProductId=@ProductId ");
			SqlParameter[] parameters = {
					new SqlParameter("@RuleId", SqlDbType.Int,4),
					new SqlParameter("@ProductId", SqlDbType.BigInt,8)			};
			parameters[0].Value = RuleId;
			parameters[1].Value = ProductId;

			YSWL.MALL.Model.Shop.Commission.CommissionPro model=new YSWL.MALL.Model.Shop.Commission.CommissionPro();
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
		public YSWL.MALL.Model.Shop.Commission.CommissionPro DataRowToModel(DataRow row)
		{
			YSWL.MALL.Model.Shop.Commission.CommissionPro model=new YSWL.MALL.Model.Shop.Commission.CommissionPro();
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
			}
			return model;
		}

		/// <summary>
		/// 获得数据列表
		/// </summary>
		public DataSet GetList(string strWhere)
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("select RuleId,ProductId ");
			strSql.Append(" FROM Shop_CommissionPro ");
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
			strSql.Append(" RuleId,ProductId ");
			strSql.Append(" FROM Shop_CommissionPro ");
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
			strSql.Append("select count(1) FROM Shop_CommissionPro ");
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
			strSql.Append(")AS Row, T.*  from Shop_CommissionPro T ");
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
			parameters[0].Value = "Shop_CommissionPro";
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
            strSql.Append("delete from Shop_CommissionPro ");
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


        #region 一键导入
        public int AddList(int ruleId, string name, int categoryId, int status = 1)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append(" INSERT  INTO Shop_CommissionPro ( RuleId ,  ProductId   )  ");
            strSql.AppendFormat("    SELECT  {0} , ProductId    FROM    PMS_Products", ruleId);
            strSql.AppendFormat(" where  NOT EXISTS(SELECT *  FROM  Shop_CommissionPro WHERE   ProductId=PMS_Products.ProductId  and RuleId={0} )",ruleId);

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
        #endregion

	    public DataSet GetRuleProducts(int ruleId, int categoryId, string pName)
	    {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("SELECT PSM.ProductId ");
            strSql.Append("FROM Shop_CommissionPro PSM ");
            strSql.Append("INNER JOIN (SELECT ProductId FROM  PMS_Products P ");
            strSql.Append("WHERE P.SaleStatus=1 ");
	        if (categoryId > 0)
	        {
                strSql.AppendFormat("AND P.ProductId IN (SELECT DISTINCT ProductId FROM  PMS_ProductCategories PC WHERE (CategoryPath LIKE (SELECT Path FROM PMS_Categories WHERE CategoryId={0})+'|%'  or CategoryId={0}))", categoryId);
	        }
            if (!String.IsNullOrWhiteSpace(pName))
	        {
                strSql.AppendFormat(" AND P.ProductName LIKE '%{0}%'", Common.InjectionFilter.SqlFilter(pName));
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

        /// <summary>
        /// 获取规则商品
        /// </summary>
        /// <param name="productId"></param>
        /// <returns></returns>
        public YSWL.MALL.Model.Shop.Commission.CommissionPro GetRuleProduct(long productId)
	    {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select  top 1  *  from Shop_CommissionPro ");
            strSql.Append(" where   ProductId=@ProductId AND EXISTS(SELECT *  FROM Shop_CommissionRule WHERE Status=1 AND Shop_CommissionPro.RuleId=RuleId) ");
            SqlParameter[] parameters = {
					new SqlParameter("@ProductId", SqlDbType.BigInt,8)			};
            parameters[0].Value = productId;

            YSWL.MALL.Model.Shop.Commission.CommissionPro model = new YSWL.MALL.Model.Shop.Commission.CommissionPro();
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
        /// 获取佣金商品
        /// </summary>
        /// <param name="startIndex"></param>
        /// <param name="endIndex"></param>
        /// <param name="ruleId"></param>
        /// <returns></returns>
        public DataSet GetComProByPage(int cid,string name,int startIndex, int endIndex, int  ruleId)
	    {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("SELECT * FROM ( ");
            strSql.Append(" SELECT ROW_NUMBER() OVER (");
            strSql.Append("order by P.ProductId desc");
            strSql.Append(")AS Row, P.* ,R.*  from PMS_Products P ,  Shop_CommissionRule R ");
            //有全部商品的佣金规则
            if (ruleId > 0)
            {
                strSql.AppendFormat(" WHERE   R.Status = 1  AND R.RuleId = {0}  AND P.SaleStatus = 1", ruleId);
            }
            else
            {
                strSql.Append(" WHERE   R.Status = 1 AND R.IsAll = 0 AND EXISTS ( SELECT * FROM  Shop_CommissionPro  WHERE  ProductId = p.ProductId AND RuleId = R.RuleId )");
            }
            if (cid > 0)
            {
                strSql.Append(" AND EXISTS (  SELECT DISTINCT  *  FROM   PMS_ProductCategories  ");
                strSql.AppendFormat(
                    "  WHERE  ( CategoryPath LIKE  ( SELECT Path FROM PMS_Categories WHERE     CategoryId = {0} ) +  '|%'  OR CategoryId = {0}  )  AND ProductId = P.ProductId ) ",
                    cid);
            }
            if (!String.IsNullOrWhiteSpace(name))
            {
                strSql.AppendFormat(" AND ProductName LIKE '%{0}%' ", Common.InjectionFilter.SqlFilter(name));
            }
            strSql.Append(" ) TT");
            strSql.AppendFormat(" WHERE TT.Row between {0} and {1}", startIndex, endIndex);
            return DBHelper.DefaultDBHelper.Query(strSql.ToString());
	    }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="ruleId"></param>
        /// <returns></returns>
        public int GetComProCount(int cid, string name, int ruleId)
	    {
            StringBuilder strSql = new StringBuilder();
            //有全部商品的佣金规则
            if (ruleId > 0)
            {
                strSql.Append("select count(1) FROM PMS_Products P ");
                strSql.Append(" WHERE   P.SaleStatus = 1");
            }
            else
            {
                strSql.Append("select count(1) FROM PMS_Products P,  Shop_CommissionRule R ");
                strSql.Append(" WHERE   R.Status = 1 AND R.IsAll = 0 AND EXISTS ( SELECT * FROM  Shop_CommissionPro  WHERE  ProductId = P.ProductId AND RuleId = R.RuleId )");
            }
            if (cid > 0)
            {
                strSql.Append(" AND EXISTS (  SELECT DISTINCT  *  FROM   PMS_ProductCategories  ");
                strSql.AppendFormat(
                    "  WHERE  ( CategoryPath LIKE  ( SELECT Path FROM PMS_Categories WHERE     CategoryId = {0} ) +  '|%'  OR CategoryId = {0}  )  AND ProductId = P.ProductId ) ",
                    cid);
            }
            if (!String.IsNullOrWhiteSpace(name))
            {
                strSql.AppendFormat(" AND ProductName LIKE '%{0}%' ", Common.InjectionFilter.SqlFilter(name));
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

	    #endregion  ExtensionMethod
	}
}

