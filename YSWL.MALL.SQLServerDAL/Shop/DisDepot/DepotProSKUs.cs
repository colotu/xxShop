/**  版本信息模板在安装目录下，可自行修改。
* DepotProSKUs.cs
*
* 功 能： N/A
* 类 名： DepotProSKUs
*
* Ver    变更日期             负责人  变更内容
* ───────────────────────────────────
* V0.01  2015/7/27 17:36:55   N/A    初版
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
using YSWL.MALL.IDAL.Shop.DisDepot;
using YSWL.DBUtility;//Please add references
namespace YSWL.MALL.SQLServerDAL.Shop.DisDepot
{
	/// <summary>
	/// 数据访问类:DepotProSKUs
	/// </summary>
	public partial class DepotProSKUs:IDepotProSKUs
	{
		public DepotProSKUs()
		{}
		#region  BasicMethod

		/// <summary>
		/// 是否存在该记录
		/// </summary>
		public bool Exists(string SKU)
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("select count(1) from Shop_DepotProSKUs");
			strSql.Append(" where SKU=@SKU ");
			SqlParameter[] parameters = {
					new SqlParameter("@SKU", SqlDbType.NVarChar,50)			};
			parameters[0].Value = SKU;

			return DBHelper.DefaultDBHelper.Exists(strSql.ToString(),parameters);
		}


		/// <summary>
		/// 增加一条数据
		/// </summary>
		public bool Add(YSWL.MALL.Model.Shop.DisDepot.DepotProSKUs model)
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("insert into Shop_DepotProSKUs(");
			strSql.Append("SKU,ProductId,Weight,Stock,AlertStock,CostPrice,SalePrice,Upselling)");
			strSql.Append(" values (");
			strSql.Append("@SKU,@ProductId,@Weight,@Stock,@AlertStock,@CostPrice,@SalePrice,@Upselling)");
			SqlParameter[] parameters = {
					new SqlParameter("@SKU", SqlDbType.NVarChar,50),
					new SqlParameter("@ProductId", SqlDbType.BigInt,8),
					new SqlParameter("@Weight", SqlDbType.Int,4),
					new SqlParameter("@Stock", SqlDbType.Int,4),
					new SqlParameter("@AlertStock", SqlDbType.Int,4),
					new SqlParameter("@CostPrice", SqlDbType.Money,8),
					new SqlParameter("@SalePrice", SqlDbType.Money,8),
					new SqlParameter("@Upselling", SqlDbType.Bit,1)};
			parameters[0].Value = model.SKU;
			parameters[1].Value = model.ProductId;
			parameters[2].Value = model.Weight;
			parameters[3].Value = model.Stock;
			parameters[4].Value = model.AlertStock;
			parameters[5].Value = model.CostPrice;
			parameters[6].Value = model.SalePrice;
			parameters[7].Value = model.Upselling;

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
		public bool Update(YSWL.MALL.Model.Shop.DisDepot.DepotProSKUs model)
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("update Shop_DepotProSKUs set ");
			strSql.Append("ProductId=@ProductId,");
			strSql.Append("Weight=@Weight,");
			strSql.Append("Stock=@Stock,");
			strSql.Append("AlertStock=@AlertStock,");
			strSql.Append("CostPrice=@CostPrice,");
			strSql.Append("SalePrice=@SalePrice,");
			strSql.Append("Upselling=@Upselling");
			strSql.Append(" where SKU=@SKU ");
			SqlParameter[] parameters = {
					new SqlParameter("@ProductId", SqlDbType.BigInt,8),
					new SqlParameter("@Weight", SqlDbType.Int,4),
					new SqlParameter("@Stock", SqlDbType.Int,4),
					new SqlParameter("@AlertStock", SqlDbType.Int,4),
					new SqlParameter("@CostPrice", SqlDbType.Money,8),
					new SqlParameter("@SalePrice", SqlDbType.Money,8),
					new SqlParameter("@Upselling", SqlDbType.Bit,1),
					new SqlParameter("@SKU", SqlDbType.NVarChar,50)};
			parameters[0].Value = model.ProductId;
			parameters[1].Value = model.Weight;
			parameters[2].Value = model.Stock;
			parameters[3].Value = model.AlertStock;
			parameters[4].Value = model.CostPrice;
			parameters[5].Value = model.SalePrice;
			parameters[6].Value = model.Upselling;
			parameters[7].Value = model.SKU;

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
		public bool Delete(string SKU)
		{
			
			StringBuilder strSql=new StringBuilder();
			strSql.Append("delete from Shop_DepotProSKUs ");
			strSql.Append(" where SKU=@SKU ");
			SqlParameter[] parameters = {
					new SqlParameter("@SKU", SqlDbType.NVarChar,50)			};
			parameters[0].Value = SKU;

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
		public bool DeleteList(string SKUlist )
		{
			StringBuilder strSql=new StringBuilder();
			strSql.Append("delete from Shop_DepotProSKUs ");
			strSql.Append(" where SKU in ("+SKUlist + ")  ");
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
		public YSWL.MALL.Model.Shop.DisDepot.DepotProSKUs GetModel(string SKU)
		{
			
			StringBuilder strSql=new StringBuilder();
			strSql.Append("select  top 1 SKU,ProductId,Weight,Stock,AlertStock,CostPrice,SalePrice,Upselling from Shop_DepotProSKUs ");
			strSql.Append(" where SKU=@SKU ");
			SqlParameter[] parameters = {
					new SqlParameter("@SKU", SqlDbType.NVarChar,50)			};
			parameters[0].Value = SKU;

			YSWL.MALL.Model.Shop.DisDepot.DepotProSKUs model=new YSWL.MALL.Model.Shop.DisDepot.DepotProSKUs();
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
		public YSWL.MALL.Model.Shop.DisDepot.DepotProSKUs DataRowToModel(DataRow row)
		{
			YSWL.MALL.Model.Shop.DisDepot.DepotProSKUs model=new YSWL.MALL.Model.Shop.DisDepot.DepotProSKUs();
			if (row != null)
			{
				if(row["SKU"]!=null)
				{
					model.SKU=row["SKU"].ToString();
				}
				if(row["ProductId"]!=null && row["ProductId"].ToString()!="")
				{
					model.ProductId=long.Parse(row["ProductId"].ToString());
				}
				if(row["Weight"]!=null && row["Weight"].ToString()!="")
				{
					model.Weight=int.Parse(row["Weight"].ToString());
				}
				if(row["Stock"]!=null && row["Stock"].ToString()!="")
				{
					model.Stock=int.Parse(row["Stock"].ToString());
				}
				if(row["AlertStock"]!=null && row["AlertStock"].ToString()!="")
				{
					model.AlertStock=int.Parse(row["AlertStock"].ToString());
				}
				if(row["CostPrice"]!=null && row["CostPrice"].ToString()!="")
				{
					model.CostPrice=decimal.Parse(row["CostPrice"].ToString());
				}
				if(row["SalePrice"]!=null && row["SalePrice"].ToString()!="")
				{
					model.SalePrice=decimal.Parse(row["SalePrice"].ToString());
				}
				if(row["Upselling"]!=null && row["Upselling"].ToString()!="")
				{
					if((row["Upselling"].ToString()=="1")||(row["Upselling"].ToString().ToLower()=="true"))
					{
						model.Upselling=true;
					}
					else
					{
						model.Upselling=false;
					}
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
			strSql.Append("select SKU,ProductId,Weight,Stock,AlertStock,CostPrice,SalePrice,Upselling ");
			strSql.Append(" FROM Shop_DepotProSKUs ");
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
			strSql.Append(" SKU,ProductId,Weight,Stock,AlertStock,CostPrice,SalePrice,Upselling ");
			strSql.Append(" FROM Shop_DepotProSKUs ");
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
			strSql.Append("select count(1) FROM Shop_DepotProSKUs ");
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
				strSql.Append("order by T.SKU desc");
			}
			strSql.Append(")AS Row, T.*  from Shop_DepotProSKUs T ");
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
			parameters[0].Value = "Shop_DepotProSKUs";
			parameters[1].Value = "SKU";
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
        /// 同步库存
        /// </summary>
        /// <param name="stockList"></param>
        /// <param name="depotId"></param>
        /// <returns></returns>
        public bool SyncStock(List<YSWL.MALL.Model.Shop.DisDepot.DepotProSKUs> proSKUsList, List<YSWL.MALL.Model.Shop.DisDepot.DepotProduct> depotProductList,int depotId)
        {
            string tableName = "Shop_DepotProSKUs_" + depotId;
            string proTable = "Shop_DepotProduct_" + depotId;
            if (!TabExists(tableName))//库存表是否存在
            {
                CreateTab(tableName);
            }

            if (!TabExists(proTable))//日志表是否存在
            {
                CreateProTab(proTable);
            }

            using (SqlConnection connection =  DBHelper.DefaultDBHelper.GetDBObject().GetConnection)
            {
                connection.Open();
                using (SqlTransaction transaction = connection.BeginTransaction())
                {
                    try
                    {
                        //添加库存数据
                        DBHelper.DefaultDBHelper.ExecuteSqlTran4Indentity(GenerateStockItems(proSKUsList, depotId), transaction);
                        //添加仓库商品
                        DBHelper.DefaultDBHelper.ExecuteSqlTran4Indentity(GenerateProItems(depotProductList, depotId), transaction);
                        transaction.Commit();
                        return true;
                    }
                    catch (SqlException)
                    {
                        transaction.Rollback();
                        throw;
                    }
                }
            }

        }


        public bool Exists(string SKU, string tableName)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.AppendFormat("select count(1) from {0}", tableName);
            strSql.Append(" where SKU=@SKU ");
            SqlParameter[] parameters = {
					new SqlParameter("@SKU", SqlDbType.NVarChar,50)			};
            parameters[0].Value = SKU;

            return DBHelper.DefaultDBHelper.Exists(strSql.ToString(), parameters);
        }

        /// <summary>
        /// 生成SKU 库存数据
        /// </summary>
        /// <param name="proSKUsList"></param>
        /// <param name="depotId"></param>
        /// <returns></returns>
        private List<CommandInfo> GenerateStockItems(List<YSWL.MALL.Model.Shop.DisDepot.DepotProSKUs> proSKUsList, int depotId)
        {
            List<CommandInfo> list = new List<CommandInfo>();
            string tableName = "Shop_DepotProSKUs_" + depotId;
            foreach (var model in proSKUsList)
            {
                StringBuilder strSql = new StringBuilder();
                strSql.AppendFormat("delete from {0} ", tableName);
                strSql.Append(" where SKU=@SKU ;");
                //先删除，然后再添加数据
                strSql.AppendFormat("insert into {0}(", tableName);
                strSql.Append("SKU,ProductId,Weight,Stock,AlertStock,CostPrice,SalePrice,Upselling)");
                strSql.Append(" values (");
                strSql.Append("@SKU,@ProductId,@Weight,@Stock,@AlertStock,@CostPrice,@SalePrice,@Upselling)");
                SqlParameter[] parameters = {
					new SqlParameter("@SKU", SqlDbType.NVarChar,50),
					new SqlParameter("@ProductId", SqlDbType.BigInt,8),
					new SqlParameter("@Weight", SqlDbType.Int,4),
					new SqlParameter("@Stock", SqlDbType.Int,4),
					new SqlParameter("@AlertStock", SqlDbType.Int,4),
					new SqlParameter("@CostPrice", SqlDbType.Money,8),
					new SqlParameter("@SalePrice", SqlDbType.Money,8),
					new SqlParameter("@Upselling", SqlDbType.Bit,1)};
                parameters[0].Value = model.SKU;
                parameters[1].Value = model.ProductId;
                parameters[2].Value = model.Weight;
                parameters[3].Value = model.Stock;
                parameters[4].Value = model.AlertStock;
                parameters[5].Value = model.CostPrice;
                parameters[6].Value = model.SalePrice;
                parameters[7].Value = model.Upselling;

                list.Add(new CommandInfo(strSql.ToString(),
                                         parameters, EffentNextType.ExcuteEffectRows));
            }
            return list;
        }

        /// <summary>
        /// 生成商品数据
        /// </summary>
        /// <param name="depotProductList"></param>
        /// <param name="depotId"></param>
        /// <returns></returns>
        private List<CommandInfo> GenerateProItems(List<YSWL.MALL.Model.Shop.DisDepot.DepotProduct> depotProductList, int depotId)
        {
            List<CommandInfo> list = new List<CommandInfo>();
            string tableName = "Shop_DepotProduct_" + depotId;
            foreach (var model in depotProductList)
            {
                StringBuilder strSql = new StringBuilder();
                strSql.AppendFormat("delete from {0} ", tableName);
                strSql.Append(" where ProductId=@ProductId ;");
                //先删除，然后再添加数据
                strSql.AppendFormat("insert into {0}(", tableName);
                strSql.Append("ProductId,TypeId,BrandId,ProductName,SaleStatus,AddedDate,SaleCounts,MarketPrice,Stock,LowestSalePrice,HasSKU,DisplaySequence,ImageUrl,SalesType,ThumbnailUrl1)");
                strSql.Append(" values (");
                strSql.Append("@ProductId,@TypeId,@BrandId,@ProductName,@SaleStatus,@AddedDate,@SaleCounts,@MarketPrice,@Stock,@LowestSalePrice,@HasSKU,@DisplaySequence,@ImageUrl,@SalesType,@ThumbnailUrl1)");
                SqlParameter[] parameters = {
					new SqlParameter("@ProductId", SqlDbType.BigInt,8),
					new SqlParameter("@TypeId", SqlDbType.Int,4),
					new SqlParameter("@BrandId", SqlDbType.Int,4),
					new SqlParameter("@ProductName", SqlDbType.NVarChar,200),
					new SqlParameter("@SaleStatus", SqlDbType.Int,4),
					new SqlParameter("@AddedDate", SqlDbType.DateTime),
					new SqlParameter("@SaleCounts", SqlDbType.Int,4),
					new SqlParameter("@MarketPrice", SqlDbType.Money,8),
					new SqlParameter("@Stock", SqlDbType.Int,4),
					new SqlParameter("@LowestSalePrice", SqlDbType.Money,8),
					new SqlParameter("@HasSKU", SqlDbType.Bit,1),
					new SqlParameter("@DisplaySequence", SqlDbType.Int,4),
					new SqlParameter("@ImageUrl", SqlDbType.NVarChar,255),
                	new SqlParameter("@SalesType", SqlDbType.SmallInt,2),
					new SqlParameter("@ThumbnailUrl1", SqlDbType.NVarChar,255)};
                parameters[0].Value = model.ProductId;
                parameters[1].Value = model.TypeId;
                parameters[2].Value = model.BrandId;
                parameters[3].Value = model.ProductName;
                parameters[4].Value = model.SaleStatus;
                parameters[5].Value = model.AddedDate;
                parameters[6].Value = model.SaleCounts;
                parameters[7].Value = model.MarketPrice;
                parameters[8].Value = model.Stock;
                parameters[9].Value = model.LowestSalePrice;
                parameters[10].Value = model.HasSKU;
                parameters[11].Value = model.DisplaySequence;
                parameters[12].Value = model.ImageUrl;
                parameters[13].Value = model.SalesType;
                parameters[14].Value = model.ThumbnailUrl1;

                list.Add(new CommandInfo(strSql.ToString(),
                                         parameters, EffentNextType.ExcuteEffectRows));
            }
            return list;
        }

        /// <summary>
        /// 表是否存在
        /// </summary>
        /// <param name="tableName"></param>
        /// <returns></returns>
        public bool TabExists(string tableName)
        {
            string strsql = "select count(*) from sysobjects where id = object_id(N'[" + tableName + "]') and OBJECTPROPERTY(id, N'IsUserTable') = 1";
            object obj = DBHelper.DefaultDBHelper.GetSingle(strsql);
            int cmdresult;
            if ((Object.Equals(obj, null)) || (Object.Equals(obj, System.DBNull.Value)))
            {
                cmdresult = 0;
            }
            else
            {
                cmdresult = int.Parse(obj.ToString());
            }
            if (cmdresult == 0)
            {
                return false;
            }
            else
            {
                return true;
            }
        }
        /// <summary>
        /// 创建表
        /// </summary>
        /// <param name="tableName"></param>
        public void CreateTab(string tableName)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.AppendFormat(
         @"
CREATE TABLE [{0}](
	[SKU] [nvarchar](50) NOT NULL,
	[ProductId] [bigint] NOT NULL,
	[Weight] [int] NOT NULL,
	[Stock] [int] NOT NULL,
	[AlertStock] [int] NOT NULL,
	[CostPrice] [money] NOT NULL,
	[SalePrice] [money] NOT NULL,
	[Upselling] [bit] NOT NULL,
    
 CONSTRAINT [PK_{0}] PRIMARY KEY CLUSTERED 
(
	[SKU] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY] ", tableName);
            DBHelper.DefaultDBHelper.ExecuteSql(strSql.ToString());
        }


        public void CreateProTab(string tableName)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.AppendFormat(
         @"
CREATE TABLE [{0}](
	[ProductId] [bigint] NOT NULL,
	[TypeId] [int] NULL,
	[BrandId] [int] NOT NULL,
	[ProductName] [nvarchar](200) NOT NULL,
	[SaleStatus] [int] NOT NULL,
	[AddedDate] [datetime] NOT NULL,
	[SaleCounts] [int] NOT NULL,
	[MarketPrice] [money] NULL,
	[Stock] [int] NOT NULL,
	[LowestSalePrice] [money] NOT NULL,
	[HasSKU] [bit] NOT NULL,
	[DisplaySequence] [int] NOT NULL,
	[ImageUrl] [nvarchar](255) NULL,
    [SalesType] [smallint]  NOT NULL,
	[ThumbnailUrl1] [nvarchar](255) NULL,
 CONSTRAINT [PK_{0}] PRIMARY KEY CLUSTERED 
(
	[ProductId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]", tableName);
            DBHelper.DefaultDBHelper.ExecuteSql(strSql.ToString());
        }

        /// <summary>
        /// 检测SKU库存
        /// </summary>
        /// <param name="SKU"></param>
        /// <param name="count"></param>
        /// <param name="depotId"></param>
        /// <returns></returns>
        public bool CheckStock(string SKU, int count, int depotId)
        {
            string tableName = "Shop_DepotProSKUs_" + depotId;
            if (!TabExists(tableName))
            {
                return false;
            }
            else
            {
                StringBuilder strSql = new StringBuilder();
                strSql.AppendFormat("select Stock  FROM {0} ", tableName);
                strSql.Append(" where SKU=@SKU ");
                SqlParameter[] parameters = {
                    new SqlParameter("@SKU", SqlDbType.NVarChar, 50)
                    };
                parameters[0].Value = SKU;
                object obj = DBHelper.DefaultDBHelper.GetSingle(strSql.ToString(), parameters);
                if (obj == null)
                {
                    return false;
                }
                else
                {
                    return Common.Globals.SafeInt(obj.ToString(), 0) > count;
                }
            }


        }
        /// <summary>
        /// 获取分仓仓库SKU
        /// </summary>
        /// <param name="productId"></param>
        /// <param name="depotId"></param>
        /// <returns></returns>
	    public DataSet GetProductSkuInfo(long productId, int depotId)
	    {
            string tableName = "Shop_DepotProSKUs_" + depotId;
	        if (!TabExists(tableName))
	        {
	            return null;
	        }
	        StringBuilder strSql = new StringBuilder();
            strSql.AppendFormat("SELECT * FROM  {0} ", tableName);
            strSql.Append("WHERE ProductId=@ProducId ");
            SqlParameter[] parameters = { 
                                        new SqlParameter("@ProducId",SqlDbType.BigInt,8)
                                        };
            parameters[0].Value = productId;
            return DBHelper.DefaultDBHelper.Query(strSql.ToString(), parameters);
	    }
        /// <summary>
        /// 分页获取数据列表
        /// </summary>
        /// <param name="Top"></param>
        /// <param name="depotId">仓库id</param>
        /// <param name="strWhere"></param>
        /// <param name="orderby"></param>
        /// <returns></returns>
        public DataSet GetList(int Top, int depotId, string strWhere, string orderby)
        {
            string tableName = "Shop_DepotProSKUs_" + depotId;
            string tableName2 = "Shop_DepotProduct_" + depotId;
            if (!TabExists(tableName))
            {
                return null;
            }
            if (!TabExists(tableName2))
            {
                return null;
            }
            StringBuilder strSql = new StringBuilder();
            strSql.Append(" SELECT ");
            if (Top > 0)
            {
                strSql.Append(" top " + Top.ToString());
            }
            strSql.AppendFormat("  S.*,ProductName,SaleStatus,AddedDate,SalesType  FROM   {0}  S", tableName);
            strSql.AppendFormat(" Inner Join  {0}  P ", tableName2);
            strSql.Append(" on S.ProductId=P.ProductId ");
            if (!string.IsNullOrEmpty(strWhere.Trim()))
            {
                strSql.Append(" WHERE " + strWhere);
            }
            if (!string.IsNullOrEmpty(orderby.Trim()))
            {
                strSql.Append(" order by S." + orderby);
            }
            else
            {
                strSql.Append(" order by S.ProductId desc");
            }
            return DBHelper.DefaultDBHelper.Query(strSql.ToString());
        }
     /// <summary>
        /// 分页获取数据列表
     /// </summary>
     /// <param name="depotId">仓库id</param>
     /// <param name="strWhere"></param>
     /// <param name="orderby"></param>
     /// <param name="startIndex"></param>
     /// <param name="endIndex"></param>
     /// <returns></returns>
        public DataSet GetListByPage(int depotId, string strWhere, string orderby, int startIndex, int endIndex)
        {
            string tableName = "Shop_DepotProSKUs_" + depotId;
            string tableName2 = "Shop_DepotProduct_" + depotId;
            if (!TabExists(tableName))
            {
                return null;
            }
            if (!TabExists(tableName2))
            {
                return null;
            }
            StringBuilder strSql = new StringBuilder();
            strSql.Append("SELECT * FROM ( ");
            strSql.Append(" SELECT ROW_NUMBER() OVER (");
            if (!string.IsNullOrEmpty(orderby.Trim()))
            {
                strSql.Append("order by S." + orderby);
            }
            else
            {
                strSql.Append("order by S.ProductId desc");
            }
            strSql.Append(")AS Row,  ");
            strSql.AppendFormat(" S.*,ProductName,ThumbnailUrl1,SaleStatus,AddedDate  FROM   {0}  S", tableName);
            strSql.AppendFormat(" Inner Join  {0}  P ", tableName2);
            strSql.Append(" on S.ProductId=P.ProductId ");

            if (!string.IsNullOrEmpty(strWhere.Trim()))
            {
                strSql.Append(" WHERE " + strWhere);
            }
            strSql.Append(" ) TT");
            strSql.AppendFormat(" WHERE TT.Row between {0} and {1}", startIndex, endIndex);
            return DBHelper.DefaultDBHelper.Query(strSql.ToString());
        }
       /// <summary>
        /// 获取记录总数
       /// </summary>
       /// <param name="depotId">仓库id</param>
       /// <param name="strWhere"></param>
       /// <returns></returns>
        public int GetRecordCount(int depotId,string strWhere)
        {
            string tableName = "Shop_DepotProSKUs_" + depotId;
            string tableName2 = "Shop_DepotProduct_" + depotId;
            if (!TabExists(tableName))
            {
                return 0;
            }
            if (!TabExists(tableName2))
            {
                return 0;
            }
            StringBuilder strSql = new StringBuilder();
            strSql.AppendFormat("SELECT count(1)  FROM   {0}  S", tableName);
            strSql.AppendFormat(" Inner Join  {0}  P ", tableName2);
            strSql.Append(" on S.ProductId=P.ProductId ");
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
        /// 获取库存
        /// </summary>
        /// <param name="SKU"></param>
        /// <param name="depotId"></param>
        /// <param name="IsOpenAS"></param>
        /// <returns></returns>
        public int GetStockBySKU(string SKU, int depotId, bool IsOpenAS,int ownerId)
        {
            string tableName = "Shop_DepotProSKUs_" + depotId;
                  string proTable = "Shop_DepotProduct_" + depotId;
            if (!TabExists(tableName)||!TabExists(proTable))
            {
                return 0;
            }
            StringBuilder strSql = new StringBuilder();
            if (IsOpenAS)
            {
                strSql.AppendFormat("SELECT  (Stock-AlertStock) as AlertStock FROM {0} ", tableName);
            }
            else
            {
                strSql.AppendFormat("SELECT  Stock  FROM {0} ", tableName);
            }
            strSql.AppendFormat("WHERE SKU=@SKU and ownerId={0} ", ownerId);
            strSql.AppendFormat("and Upselling=1  and  EXISTS ( SELECT *  FROM   {0}  WHERE   SaleStatus =1  AND ProductId = {1}.ProductId ) ", proTable, tableName); //加上商品状态过滤
            SqlParameter[] parameters = {
					new SqlParameter("@SKU", SqlDbType.NVarChar,50)
			};
            parameters[0].Value = SKU;
            object obj = DBHelper.DefaultDBHelper.GetSingle(strSql.ToString(), parameters);
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
        /// 获取未同步OMS的SKU库存
        /// </summary>
        /// <param name="sku"></param>
        /// <returns></returns>
	    public int GetUnSyncStock(string sku)
	    {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("SELECT  SUM(Quantity) FROM    OMS_OrderItems WHERE   sku = @SKU");
            strSql.Append(" AND EXISTS ( SELECT * FROM   OMS_Orders  WHERE  PaymentGateway <> 'cod' ");
            strSql.Append("    AND OrderId = OMS_OrderItems.OrderId AND PaymentStatus<2 ) ");
            SqlParameter[] parameters = { 
                                        new SqlParameter("@SKU",SqlDbType.NVarChar,50)
                                        };
            parameters[0].Value = sku;
            object obj = DBHelper.DefaultDBHelper.GetSingle(strSql.ToString(), parameters);
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
        /// 获取未添加sku 数据 总条数
        /// </summary>
        /// <param name="depotId"></param>
        /// <param name="categoryId"></param>
        /// <param name="pName"></param>
        /// <returns></returns>
        public int GetNoAddSKURecordCount(int depotId, int categoryId, string pName)
        {
            string skuTable = "Shop_DepotProSKUs_" + depotId;
            string proTable = "Shop_DepotProduct_" + depotId;
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select  count(1)   from  ( ");

            strSql.Append("  select  p.ProductName,p.ThumbnailUrl1,s.*  from PMS_SKUs s  Inner Join PMS_Products p  on S.ProductId=P.ProductId and s.Upselling=1  and p.SaleStatus = 1 ");
            strSql.AppendFormat(" AND SupplierId<=0 ");
            if (!String.IsNullOrWhiteSpace(pName))
            {
                strSql.AppendFormat(" AND ProductName LIKE '%{0}%' ", Common.InjectionFilter.SqlFilter(pName));
            }
            if (categoryId > 0)
            {
                strSql.Append(" AND EXISTS (  SELECT DISTINCT  *  FROM   PMS_ProductCategories  ");
                strSql.AppendFormat(
                    "  WHERE  ( CategoryPath LIKE  ( SELECT Path FROM PMS_Categories WHERE     CategoryId = {0} ) +  '|%' OR CategoryId = {0}  )  AND ProductId = p.ProductId ) ",
                    categoryId);
            }

            strSql.Append(" ) as  T  ");

            strSql.Append(" WHERE  ");
            strSql.AppendFormat("  NOT EXISTS ( select ds.* from {0} ds  Inner Join {1} dp  on dS.ProductId=dP.ProductId  ", skuTable, proTable);
            strSql.AppendFormat("  WHERE   ds.sku = T.sku  ) ");


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
        /// 获取未添加sku 数据列表
        /// </summary>
        /// <param name="depotId"></param>
        /// <param name="categoryId"></param>
        /// <param name="pName"></param>
        /// <param name="startIndex"></param>
        /// <param name="endIndex"></param>
        /// <returns></returns>
        public DataSet GetNoAddSKUList(int depotId, int categoryId, string pName, int startIndex, int endIndex)
        {
            string skuTable = "Shop_DepotProSKUs_" + depotId;
            string proTable = "Shop_DepotProduct_" + depotId;
            StringBuilder strSql = new StringBuilder();
            strSql.Append("SELECT * FROM ( ");
            strSql.Append(" SELECT ROW_NUMBER() OVER (");
            strSql.Append("order by T.ProductId desc");
            strSql.Append(")AS Row, T.*  from  ( ");

            strSql.Append("  select  p.ProductName,p.ThumbnailUrl1,s.*  from PMS_SKUs s  Inner Join PMS_Products p  on S.ProductId=P.ProductId and s.Upselling=1  and p.SaleStatus = 1 ");
            strSql.AppendFormat(" AND SupplierId<=0 ");
            if (!String.IsNullOrWhiteSpace(pName))
            {
                strSql.AppendFormat(" AND ProductName LIKE '%{0}%' ", Common.InjectionFilter.SqlFilter(pName));
            }
            if (categoryId > 0)
            {
                strSql.Append(" AND EXISTS (  SELECT DISTINCT  *  FROM   PMS_ProductCategories  ");
                strSql.AppendFormat(
                    "  WHERE  ( CategoryPath LIKE  ( SELECT Path FROM PMS_Categories WHERE     CategoryId = {0} ) +  '|%' OR CategoryId = {0}  )  AND ProductId = p.ProductId ) ",
                    categoryId);
            }

            strSql.Append(" ) as  T  ");

            strSql.Append(" WHERE  ");
            strSql.AppendFormat("  NOT EXISTS ( select ds.* from {0} ds  Inner Join {1} dp  on dS.ProductId=dP.ProductId  ", skuTable, proTable);
            strSql.AppendFormat("  WHERE   ds.sku = T.sku  ) ");
            strSql.Append(" ) TT");
            strSql.AppendFormat(" WHERE TT.Row between {0} and {1}", startIndex, endIndex);
            return DBHelper.DefaultDBHelper.Query(strSql.ToString());
        }

        public bool AddProduct(int depotId, YSWL.MALL.Model.Shop.Products.SKUInfo skuInfo, YSWL.MALL.Model.Shop.Products.ProductInfo prodInfo)
        {
            List<CommandInfo> sqllist = new List<CommandInfo>();
            CommandInfo cmd;
            string skuTable = "Shop_DepotProSKUs_" + depotId;
            string proTable = "Shop_DepotProduct_" + depotId;

            //商品数据不存在就添加
            if (!ProdExists(depotId, prodInfo.ProductId))
            {
                StringBuilder strSqlp = new StringBuilder();
                strSqlp.AppendFormat("insert into {0}(", proTable);
                strSqlp.Append("ProductId,TypeId,BrandId,ProductName,SaleStatus,AddedDate,SaleCounts,Stock,DisplaySequence,MarketPrice,LowestSalePrice,HasSKU,ImageUrl,ThumbnailUrl1,SalesType)");
                strSqlp.Append(" values (");
                strSqlp.Append("@ProductId,@TypeId,@BrandId,@ProductName,@SaleStatus,@AddedDate,@SaleCounts,@Stock,@DisplaySequence,@MarketPrice,@LowestSalePrice,@HasSKU,@ImageUrl,@ThumbnailUrl1,@SalesType)");
                SqlParameter[] parametersp = {
                    new SqlParameter("@ProductId", SqlDbType.BigInt,8),
					new SqlParameter("@TypeId", SqlDbType.Int,4),
					new SqlParameter("@BrandId", SqlDbType.Int,4),
					new SqlParameter("@ProductName", SqlDbType.NVarChar,200),
					new SqlParameter("@SaleStatus", SqlDbType.Int,4),
					new SqlParameter("@AddedDate", SqlDbType.DateTime),
					new SqlParameter("@SaleCounts", SqlDbType.Int,4),
					new SqlParameter("@Stock", SqlDbType.Int,4),
					new SqlParameter("@DisplaySequence", SqlDbType.Int,4),
					new SqlParameter("@MarketPrice", SqlDbType.Money,8),
					new SqlParameter("@LowestSalePrice", SqlDbType.Money,8),
					new SqlParameter("@HasSKU", SqlDbType.Bit,1),
					new SqlParameter("@ImageUrl", SqlDbType.NVarChar,255),
					new SqlParameter("@ThumbnailUrl1", SqlDbType.NVarChar,255),
					new SqlParameter("@SalesType", SqlDbType.SmallInt,2)};
                parametersp[0].Value = prodInfo.ProductId;
                parametersp[1].Value = prodInfo.TypeId;
                parametersp[2].Value = prodInfo.BrandId;
                parametersp[3].Value = prodInfo.ProductName;
                parametersp[4].Value = prodInfo.SaleStatus;
                parametersp[5].Value = prodInfo.AddedDate;
                parametersp[6].Value = prodInfo.SaleCounts;
                parametersp[7].Value = prodInfo.Stock;
                parametersp[8].Value = prodInfo.DisplaySequence;
                parametersp[9].Value = prodInfo.MarketPrice;
                parametersp[10].Value = prodInfo.LowestSalePrice;
                parametersp[11].Value = prodInfo.HasSKU;
                parametersp[12].Value = prodInfo.ImageUrl;
                parametersp[13].Value = prodInfo.ThumbnailUrl1;
                parametersp[14].Value = prodInfo.SalesType;
                cmd = new CommandInfo(strSqlp.ToString(), parametersp, EffentNextType.ExcuteEffectRows);
                sqllist.Add(cmd);
            }

            //添加sku 数据
            StringBuilder strSql = new StringBuilder();
            strSql.AppendFormat("insert into {0}(", skuTable);
            strSql.Append("SKU,ProductId,Weight,Stock,AlertStock,CostPrice,SalePrice,Upselling)");
            strSql.Append(" values (");
            strSql.Append("@SKU,@ProductId,@Weight,@Stock,@AlertStock,@CostPrice,@SalePrice,@Upselling)");
            SqlParameter[] parameters = {
					new SqlParameter("@SKU", SqlDbType.NVarChar,50),
					new SqlParameter("@ProductId", SqlDbType.BigInt,8),
					new SqlParameter("@Weight", SqlDbType.Int,4),
					new SqlParameter("@Stock", SqlDbType.Int,4),
					new SqlParameter("@AlertStock", SqlDbType.Int,4),
					new SqlParameter("@CostPrice", SqlDbType.Money,8),
					new SqlParameter("@SalePrice", SqlDbType.Money,8),
					new SqlParameter("@Upselling", SqlDbType.Bit,1)};
            parameters[0].Value = skuInfo.SKU;
            parameters[1].Value = skuInfo.ProductId;
            parameters[2].Value = skuInfo.Weight;
            parameters[3].Value = skuInfo.Stock;
            parameters[4].Value = skuInfo.AlertStock;
            parameters[5].Value = skuInfo.CostPrice;
            parameters[6].Value = skuInfo.SalePrice;
            parameters[7].Value = skuInfo.Upselling;
            cmd = new CommandInfo(strSql.ToString(), parameters,EffentNextType.ExcuteEffectRows);
            sqllist.Add(cmd);

            int rows = DBHelper.DefaultDBHelper.ExecuteSqlTran(sqllist);
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
        /// 检测sku是否存在该记录
        /// </summary>
        public bool SkuExists(int depotId, string sku)
        {
            string skuTable = "Shop_DepotProSKUs_" + depotId;
            StringBuilder strSql = new StringBuilder();
            strSql.AppendFormat("select count(1) from {0}", skuTable);
            strSql.Append(" where SKU=@SKU ");
            SqlParameter[] parameters = {
					new SqlParameter("@SKU", SqlDbType.NVarChar,50)			};
            parameters[0].Value = sku;

            return DBHelper.DefaultDBHelper.Exists(strSql.ToString(), parameters);
        }
        /// <summary>
        /// 检测商品是否存在
        /// </summary>
        private bool ProdExists(int depotId, long productId)
        {
            string proTable = "Shop_DepotProduct_" + depotId;
            StringBuilder strSql = new StringBuilder();
            strSql.AppendFormat("select count(1) from {0}", proTable);
            strSql.Append(" where ProductId=@ProductId ");
            SqlParameter[] parameters = {
					new SqlParameter("@ProductId", SqlDbType.BigInt,8)		};
            parameters[0].Value = productId;

            return DBHelper.DefaultDBHelper.Exists(strSql.ToString(), parameters);
        }
        public bool DeleteProduct(int depotId, long productId,string sku)
        {
            List<CommandInfo> sqllist = new List<CommandInfo>();
            CommandInfo cmd;
            string skuTable = "Shop_DepotProSKUs_" + depotId;
            string proTable = "Shop_DepotProduct_" + depotId;
            bool IsDelProd = true;
            if (GetRecordCount(depotId, productId) > 1) {//sku记录数大于1  不删除商品
                IsDelProd = false;
            }

            StringBuilder strSql = new StringBuilder();
            strSql.AppendFormat("delete from {0} ", skuTable);
            strSql.Append(" where SKU=@SKU ");
            SqlParameter[] parameters = {
					new SqlParameter("@SKU", SqlDbType.NVarChar,50)			};
            parameters[0].Value = sku;
            cmd = new CommandInfo(strSql.ToString(), parameters, EffentNextType.ExcuteEffectRows);
            sqllist.Add(cmd);

            if (IsDelProd) {
                StringBuilder strSqlp = new StringBuilder();
                strSqlp.AppendFormat("delete from {0} ", proTable);
                strSqlp.AppendFormat(" where ProductId={0} ", productId);
                SqlParameter[] parametersp = {};
                cmd = new CommandInfo(strSqlp.ToString(), parametersp, EffentNextType.ExcuteEffectRows);
                sqllist.Add(cmd);
            }

            int rows = DBHelper.DefaultDBHelper.ExecuteSqlTran(sqllist);
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
        /// 获取单个商品的sku记录总数
        /// </summary>
        private int GetRecordCount(int depotId, long productId)
        {
            string skuTable = "Shop_DepotProSKUs_" + depotId;
            StringBuilder strSql = new StringBuilder();
            strSql.AppendFormat("select count(1) FROM {0} ", skuTable);
            strSql.AppendFormat(" where ProductId={0}",productId);
            
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
        /// 得到一个对象实体
        /// </summary>
        public YSWL.MALL.Model.Shop.DisDepot.DepotProSKUs GetModel(int depotId, string sku)
        {
            string skuTable = "Shop_DepotProSKUs_" + depotId;
            StringBuilder strSql = new StringBuilder();
            strSql.AppendFormat("select * from {0}", skuTable);
            strSql.Append(" where SKU=@SKU ");
            SqlParameter[] parameters = {
					new SqlParameter("@SKU", SqlDbType.NVarChar,50)			};
            parameters[0].Value = sku;


            YSWL.MALL.Model.Shop.DisDepot.DepotProSKUs model = new Model.Shop.DisDepot.DepotProSKUs();
            DataSet ds = DBHelper.DefaultDBHelper.Query(strSql.ToString(), parameters);
            if (ds!=null &&  ds.Tables[0].Rows.Count > 0)
            {
                return DataRowToModel(ds.Tables[0].Rows[0]);
            }
            else
            {
                return null;
            }
        }
        public bool UpdateStockNum(int depotId, string sku, int stock)
        {
            string skuTable = "Shop_DepotProSKUs_" + depotId;
            StringBuilder strSql = new StringBuilder();
            strSql.AppendFormat("update {0} set ",skuTable);
            strSql.Append("Stock=@Stock ");
            strSql.Append(" where SKU=@SKU ");
            SqlParameter[] parameters = {
					new SqlParameter("@Stock", SqlDbType.Int,4),
					new SqlParameter("@SKU", SqlDbType.NVarChar,50)};
            parameters[0].Value = stock;
            parameters[1].Value = sku;

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
        /// 更新sku上下架状态
        /// </summary>
        /// <param name="depotId"></param>
        /// <param name="sku"></param>
        /// <param name="IsUp"></param>
        /// <returns></returns>
        public bool UpdateSkuStatus(int depotId, string sku, bool IsUp,int status=1)
	    {
            List<CommandInfo> sqllist = new List<CommandInfo>();
            CommandInfo cmd;
            string skuTable = "Shop_DepotProSKUs_" + depotId;
            string proTable = "Shop_DepotProduct_" + depotId;
            if (IsUp)  //分仓SKU 上架，分仓商品需要上架，总商品状态也要上架
            {
                StringBuilder strSqlq = new StringBuilder();
                strSqlq.AppendFormat("UPDATE  {0}  SET   SaleStatus = 1  ", proTable);
                strSqlq.AppendFormat("WHERE   EXISTS ( SELECT *  FROM   {0}  WHERE  SKU = @SKU AND ProductId = {1}.ProductId ) ", skuTable, proTable);
                SqlParameter[] parametersq = {
					new SqlParameter("@SKU", SqlDbType.NVarChar,50)			};
                parametersq[0].Value = sku;
                cmd = new CommandInfo(strSqlq.ToString(), parametersq, EffentNextType.ExcuteEffectRows);
                sqllist.Add(cmd);

                StringBuilder strSqlp = new StringBuilder();
                strSqlp.Append("UPDATE  PMS_Products  SET   SaleStatus = 1  ");
                strSqlp.AppendFormat("WHERE   EXISTS ( SELECT *  FROM   {0}  WHERE  SKU = @SKU AND ProductId = PMS_Products.ProductId ) ", skuTable);
                SqlParameter[] parametersp = {
					new SqlParameter("@SKU", SqlDbType.NVarChar,50)			};
                parametersp[0].Value = sku;
                cmd = new CommandInfo(strSqlp.ToString(), parametersp, EffentNextType.ExcuteEffectRows);
                sqllist.Add(cmd);
            } 

            StringBuilder strSql = new StringBuilder();
            strSql.AppendFormat("update {0} set ", skuTable);
            strSql.Append("Upselling=@Upselling ");
            strSql.Append(" where SKU=@SKU ");
            SqlParameter[] parameters = {
					new SqlParameter("@Upselling", SqlDbType.Bit),
					new SqlParameter("@SKU", SqlDbType.NVarChar,50)};
            parameters[0].Value = IsUp;
            parameters[1].Value = sku;

            cmd = new CommandInfo(strSql.ToString(), parameters, EffentNextType.ExcuteEffectRows);
                sqllist.Add(cmd);

            int rows = DBHelper.DefaultDBHelper.ExecuteSqlTran(sqllist);
            if (rows > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
       
	    }

        public bool SyncProdcut(List<YSWL.MALL.Model.Shop.DisDepot.Depot> depotList, YSWL.MALL.Model.Shop.Products.ProductInfo prodInfo, List<Model.Shop.Products.SKUInfo> skuList)
        {
            try{
                string skuTable;
                string proTable;
                foreach (YSWL.MALL.Model.Shop.DisDepot.Depot depotItem in depotList)
                {
                    skuTable = "Shop_DepotProSKUs_" + depotItem.DepotId;
                    proTable = "Shop_DepotProduct_" + depotItem.DepotId;
                    StringBuilder strSqlp = new StringBuilder(); 
                    strSqlp.AppendFormat("update {0} set ", proTable);
                    strSqlp.Append("TypeId=@TypeId,");
                    strSqlp.Append("BrandId=@BrandId,");
                    strSqlp.Append("ProductName=@ProductName,");
                    strSqlp.Append("SaleStatus=@SaleStatus,");
                    strSqlp.Append("SaleCounts=@SaleCounts,");
                    strSqlp.Append("DisplaySequence=@DisplaySequence,");
                    strSqlp.Append("MarketPrice=@MarketPrice,");
                    strSqlp.Append("LowestSalePrice=@LowestSalePrice,");
                    strSqlp.Append("HasSKU=@HasSKU,");
                    strSqlp.Append("ImageUrl=@ImageUrl,");
                    strSqlp.Append("ThumbnailUrl1=@ThumbnailUrl1,");
                    strSqlp.Append("SalesType=@SalesType");
                    strSqlp.Append(" where ProductId=@ProductId ");
                    SqlParameter[] parametersp = {
					new SqlParameter("@TypeId", SqlDbType.Int,4),
					new SqlParameter("@BrandId", SqlDbType.Int,4),
					new SqlParameter("@ProductName", SqlDbType.NVarChar,200),
					new SqlParameter("@SaleStatus", SqlDbType.Int,4),
					new SqlParameter("@SaleCounts", SqlDbType.Int,4),
					new SqlParameter("@Stock", SqlDbType.Int,4),
					new SqlParameter("@DisplaySequence", SqlDbType.Int,4),
					new SqlParameter("@MarketPrice", SqlDbType.Money,8),
					new SqlParameter("@LowestSalePrice", SqlDbType.Money,8),
					new SqlParameter("@HasSKU", SqlDbType.Bit,1),
					new SqlParameter("@ImageUrl", SqlDbType.NVarChar,255),
					new SqlParameter("@ThumbnailUrl1", SqlDbType.NVarChar,255),
					new SqlParameter("@SalesType", SqlDbType.SmallInt,2),
                    new SqlParameter("@ProductId", SqlDbType.BigInt,8)};
                    parametersp[0].Value = prodInfo.TypeId;
                    parametersp[1].Value = prodInfo.BrandId;
                    parametersp[2].Value = prodInfo.ProductName;
                    parametersp[3].Value = prodInfo.SaleStatus;
                    parametersp[4].Value = prodInfo.SaleCounts;
                    parametersp[5].Value = prodInfo.Stock;
                    parametersp[6].Value = prodInfo.DisplaySequence;
                    parametersp[7].Value = prodInfo.MarketPrice;
                    parametersp[8].Value = prodInfo.LowestSalePrice;
                    parametersp[9].Value = prodInfo.HasSKU;
                    parametersp[10].Value = prodInfo.ImageUrl;
                    parametersp[11].Value = prodInfo.ThumbnailUrl1;
                    parametersp[12].Value = prodInfo.SalesType;
                    parametersp[13].Value = prodInfo.ProductId;
                    DBHelper.DefaultDBHelper.ExecuteSql(strSqlp.ToString(), parametersp);

                    foreach (Model.Shop.Products.SKUInfo skuInfo in skuList)
                    {
                        StringBuilder strSql = new StringBuilder();
                        strSql.AppendFormat("update {0} set ", skuTable);
                        strSql.Append("ProductId=@ProductId,");
                        strSql.Append("Weight=@Weight,");
                        strSql.Append("CostPrice=@CostPrice,");
                        strSql.Append("SalePrice=@SalePrice,");
                        strSql.Append("Upselling=@Upselling");
                        strSql.Append(" where SKU=@SKU ");
                        SqlParameter[] parameters = {
					new SqlParameter("@ProductId", SqlDbType.BigInt,8),
					new SqlParameter("@Weight", SqlDbType.Int,4),
					new SqlParameter("@CostPrice", SqlDbType.Money,8),
					new SqlParameter("@SalePrice", SqlDbType.Money,8),
					new SqlParameter("@Upselling", SqlDbType.Bit,1),
					new SqlParameter("@SKU", SqlDbType.NVarChar,50)};
                        parameters[0].Value = skuInfo.ProductId;
                        parameters[1].Value = skuInfo.Weight;
                        parameters[2].Value = skuInfo.CostPrice;
                        parameters[3].Value = skuInfo.SalePrice;
                        parameters[4].Value = prodInfo.SaleStatus==0?false:skuInfo.Upselling;//如果是主商品下架，所有的分仓商品全部下架
                        parameters[5].Value = skuInfo.SKU;
                        DBHelper.DefaultDBHelper.ExecuteSql(strSql.ToString(), parameters);
                    }
                }
                return true;
            }
            catch(Exception ex)
            {
                throw ex;
            }    
        }

        /// <summary>
        /// 获取分仓商品sku列表 （分页）
        /// </summary>
        /// <param name="depotId">仓库Id</param>
        /// <param name="keyw">商品名称或编码</param>
        /// <param name="startIndex"></param>
        /// <param name="endIndex"></param>
        /// <param name="orderby">排序方式</param>
        /// <returns></returns>
        public DataSet GetSKUListByPage(int depotId, string keyw, int startIndex, int endIndex, string orderby)
        {
            string tableName = "Shop_DepotProSKUs_" + depotId;
            StringBuilder strSql = new StringBuilder();
            strSql.Append("SELECT * FROM ( ");
            strSql.Append(" SELECT ROW_NUMBER() OVER (");
            if (!string.IsNullOrEmpty(orderby.Trim()))
            {
                strSql.Append("order by T." + orderby);
            }
            else
            {
                strSql.Append("order by T.ProductId desc");
            }
            strSql.Append(")AS Row, T.*  from  ");
            strSql.Append(" ( ");
            strSql.Append("  select p.TypeId,p.ProductId,p.ProductName,p.SaleStatus ,d_sku.SKU, d_sku.Stock,sku.SkuId,sku.SalePrice,sku.Upselling,sku.CostPrice,sku.Weight,sku.AlertStock  ");
            strSql.AppendFormat("   from  {0} d_sku  ", tableName);
            strSql.Append("  inner Join PMS_SKUs sku on d_sku.SKU = sku.SKU  and sku.Upselling = 1  ");
            strSql.Append("  inner Join PMS_Products p on p.ProductId = sku.ProductId  and p.SaleStatus = 1 and p.SalesType = 1  ");//在售状态的正常商品
            if (!String.IsNullOrWhiteSpace(keyw))
            {
                strSql.AppendFormat(" and  (  p.ProductName like '%{0}%'  or  sku.SKU like '%{0}%' )", Common.InjectionFilter.SqlFilter(keyw));
            }
            strSql.Append(" )   T ");
            //if (!string.IsNullOrEmpty(strWhere.Trim()))
            //{
            //    strSql.Append(" WHERE " + strWhere);
            //}
            strSql.Append(" ) TT");
            strSql.AppendFormat(" WHERE TT.Row between {0} and {1}", startIndex, endIndex);
            return DBHelper.DefaultDBHelper.Query(strSql.ToString());
        }
        /// <summary>
        ///  获取记录总数
        /// </summary>
        /// <param name="depotId">仓库id</param>
        /// <param name="keyw">商品名称或编码</param>
        /// <returns></returns>
        public int GetSKURecordCount(int depotId, string keyw)
        {
            string tableName = "Shop_DepotProSKUs_" + depotId;
            StringBuilder strSql = new StringBuilder();
            strSql.Append("  select count(1)");
            strSql.AppendFormat("   from  {0} d_sku  ", tableName);
            strSql.Append("  inner Join PMS_SKUs sku on d_sku.SKU = sku.SKU  and sku.Upselling = 1  ");
            strSql.Append("  inner Join PMS_Products p on p.ProductId = sku.ProductId  and p.SaleStatus = 1 and p.SalesType = 1  ");//在售状态的正常商品
            if (!String.IsNullOrWhiteSpace(keyw))
            {
                strSql.AppendFormat(" and  (  p.ProductName like '%{0}%'  or  sku.SKU like '%{0}%' )", Common.InjectionFilter.SqlFilter(keyw));
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

