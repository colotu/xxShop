/**  版本信息模板在安装目录下，可自行修改。
* SuppDistProduct.cs
*
* 功 能： N/A
* 类 名： SuppDistProduct
*
* Ver    变更日期             负责人  变更内容
* ───────────────────────────────────
* V0.01  2014/1/27 17:36:25   N/A    初版
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
using YSWL.MALL.IDAL.Shop.Distribution;
using YSWL.DBUtility;//Please add references
namespace YSWL.MALL.SQLServerDAL.Shop.Distribution
{
    /// <summary>
    /// 数据访问类:SuppDistProduct
    /// </summary>
    public partial class SuppDistProduct : ISuppDistProduct
    {
        public SuppDistProduct()
        { }
        #region  BasicMethod

        /// <summary>
        /// 是否存在该记录
        /// </summary>
        public bool Exists(int supplierId, long ProductId)
        {
            string TableName = "Shop_SuppDistProduct_" + supplierId;
            if (TabExists(supplierId))
            {
                StringBuilder strSql = new StringBuilder();
                strSql.Append("select count(1) from " + TableName);
                strSql.Append(" where ProductId=@ProductId ");
                SqlParameter[] parameters = {
					new SqlParameter("@ProductId", SqlDbType.BigInt,8)			};
                parameters[0].Value = ProductId;

                return DBHelper.DefaultDBHelper.Exists(strSql.ToString(), parameters);
            }
            return false;
        }


        /// <summary>
        /// 增加一条数据
        /// </summary>
        public bool Add(int supplierId, YSWL.MALL.Model.Shop.Distribution.SuppDistProduct model)
        {
            string TableName = "Shop_SuppDistProduct_" + supplierId;
            CreateTab(supplierId);
            StringBuilder strSql = new StringBuilder();
            strSql.Append("insert into " + TableName + " (");
            strSql.Append("ProductId,TypeId,BrandId,ProductName,SaleStatus,AddedDate,SaleCounts,MarketPrice,Stock,LowestSalePrice,HasSKU,DisplaySequence,ImageUrl,ThumbnailUrl1)");
            strSql.Append(" values (");
            strSql.Append("@ProductId,@TypeId,@BrandId,@ProductName,@SaleStatus,@AddedDate,@SaleCounts,@MarketPrice,@Stock,@LowestSalePrice,@HasSKU,@DisplaySequence,@ImageUrl,@ThumbnailUrl1)");
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
            parameters[13].Value = model.ThumbnailUrl1;

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
        public bool Update(int supplierId, YSWL.MALL.Model.Shop.Distribution.SuppDistProduct model)
        {
            string TableName = "Shop_SuppDistProduct_" + supplierId;
            if (TabExists(supplierId))
            {
                StringBuilder strSql = new StringBuilder();
                strSql.Append("update " + TableName + " set ");
                strSql.Append("TypeId=@TypeId,");
                strSql.Append("BrandId=@BrandId,");
                strSql.Append("ProductName=@ProductName,");
                strSql.Append("SaleStatus=@SaleStatus,");
                strSql.Append("AddedDate=@AddedDate,");
                strSql.Append("SaleCounts=@SaleCounts,");
                strSql.Append("MarketPrice=@MarketPrice,");
                strSql.Append("Stock=@Stock,");
                strSql.Append("LowestSalePrice=@LowestSalePrice,");
                strSql.Append("HasSKU=@HasSKU,");
                strSql.Append("DisplaySequence=@DisplaySequence,");
                strSql.Append("ImageUrl=@ImageUrl,");
                strSql.Append("ThumbnailUrl1=@ThumbnailUrl1");
                strSql.Append(" where ProductId=@ProductId ");
                SqlParameter[] parameters = {
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
					new SqlParameter("@ThumbnailUrl1", SqlDbType.NVarChar,255),
					new SqlParameter("@ProductId", SqlDbType.BigInt,8)};
                parameters[0].Value = model.TypeId;
                parameters[1].Value = model.BrandId;
                parameters[2].Value = model.ProductName;
                parameters[3].Value = model.SaleStatus;
                parameters[4].Value = model.AddedDate;
                parameters[5].Value = model.SaleCounts;
                parameters[6].Value = model.MarketPrice;
                parameters[7].Value = model.Stock;
                parameters[8].Value = model.LowestSalePrice;
                parameters[9].Value = model.HasSKU;
                parameters[10].Value = model.DisplaySequence;
                parameters[11].Value = model.ImageUrl;
                parameters[12].Value = model.ThumbnailUrl1;
                parameters[13].Value = model.ProductId;

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
            return false;
        }

        /// <summary>
        /// 删除一条数据
        /// </summary>
        public bool Delete(int supplierId, long ProductId)
        {
            string TableName = "Shop_SuppDistProduct_" + supplierId;
            if (TabExists(supplierId))
            {
                StringBuilder strSql = new StringBuilder();
                strSql.Append("delete from  " + TableName);
                strSql.Append(" where ProductId=@ProductId ");
                SqlParameter[] parameters = {
					new SqlParameter("@ProductId", SqlDbType.BigInt,8)			};
                parameters[0].Value = ProductId;

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
            return false;
        }
        /// <summary>
        /// 批量删除数据
        /// </summary>
        public bool DeleteList(int supplierId, string ProductIdlist)
        {
            string TableName = "Shop_SuppDistProduct_" + supplierId;
            if (TabExists(supplierId))
            {
                StringBuilder strSql = new StringBuilder();
                strSql.Append("delete from  " + TableName);
                strSql.Append(" where ProductId in (" + ProductIdlist + ")  ");
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
            return false;
        }


        /// <summary>
        /// 得到一个对象实体
        /// </summary>
        public YSWL.MALL.Model.Shop.Distribution.SuppDistProduct GetModel(int supplierId, long ProductId)
        {
            string TableName = "Shop_SuppDistProduct_" + supplierId;
            if (TabExists(supplierId))
            {
                StringBuilder strSql = new StringBuilder();
                strSql.Append("select  top 1 ProductId,TypeId,BrandId,ProductName,SaleStatus,AddedDate,SaleCounts,MarketPrice,Stock,LowestSalePrice,HasSKU,DisplaySequence,ImageUrl,ThumbnailUrl1 from  " + TableName);
                strSql.Append(" where ProductId=@ProductId ");
                SqlParameter[] parameters = {
					new SqlParameter("@ProductId", SqlDbType.BigInt,8)			};
                parameters[0].Value = ProductId;

                YSWL.MALL.Model.Shop.Distribution.SuppDistProduct model = new YSWL.MALL.Model.Shop.Distribution.SuppDistProduct();
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
            return null;
        }


        /// <summary>
        /// 得到一个对象实体
        /// </summary>
        public YSWL.MALL.Model.Shop.Distribution.SuppDistProduct DataRowToModel(DataRow row)
        {
            YSWL.MALL.Model.Shop.Distribution.SuppDistProduct model = new YSWL.MALL.Model.Shop.Distribution.SuppDistProduct();
            if (row != null)
            {
                if (row["ProductId"] != null && row["ProductId"].ToString() != "")
                {
                    model.ProductId = long.Parse(row["ProductId"].ToString());
                }
                if (row["TypeId"] != null && row["TypeId"].ToString() != "")
                {
                    model.TypeId = int.Parse(row["TypeId"].ToString());
                }
                if (row["BrandId"] != null && row["BrandId"].ToString() != "")
                {
                    model.BrandId = int.Parse(row["BrandId"].ToString());
                }
                if (row["ProductName"] != null)
                {
                    model.ProductName = row["ProductName"].ToString();
                }
                if (row["SaleStatus"] != null && row["SaleStatus"].ToString() != "")
                {
                    model.SaleStatus = int.Parse(row["SaleStatus"].ToString());
                }
                if (row["AddedDate"] != null && row["AddedDate"].ToString() != "")
                {
                    model.AddedDate = DateTime.Parse(row["AddedDate"].ToString());
                }
                if (row["SaleCounts"] != null && row["SaleCounts"].ToString() != "")
                {
                    model.SaleCounts = int.Parse(row["SaleCounts"].ToString());
                }
                if (row["MarketPrice"] != null && row["MarketPrice"].ToString() != "")
                {
                    model.MarketPrice = decimal.Parse(row["MarketPrice"].ToString());
                }
                if (row["Stock"] != null && row["Stock"].ToString() != "")
                {
                    model.Stock = int.Parse(row["Stock"].ToString());
                }
                if (row["LowestSalePrice"] != null && row["LowestSalePrice"].ToString() != "")
                {
                    model.LowestSalePrice = decimal.Parse(row["LowestSalePrice"].ToString());
                }
                if (row["HasSKU"] != null && row["HasSKU"].ToString() != "")
                {
                    if ((row["HasSKU"].ToString() == "1") || (row["HasSKU"].ToString().ToLower() == "true"))
                    {
                        model.HasSKU = true;
                    }
                    else
                    {
                        model.HasSKU = false;
                    }
                }
                if (row["DisplaySequence"] != null && row["DisplaySequence"].ToString() != "")
                {
                    model.DisplaySequence = int.Parse(row["DisplaySequence"].ToString());
                }
                if (row["ImageUrl"] != null)
                {
                    model.ImageUrl = row["ImageUrl"].ToString();
                }
                if (row["ThumbnailUrl1"] != null)
                {
                    model.ThumbnailUrl1 = row["ThumbnailUrl1"].ToString();
                }
            }
            return model;
        }

        /// <summary>
        /// 获得数据列表
        /// </summary>
        public DataSet GetList(int supplierId, string strWhere)
        {
            string TableName = "Shop_SuppDistProduct_" + supplierId;
            if (TabExists(supplierId))
            {
                StringBuilder strSql = new StringBuilder();
                strSql.Append("select ProductId,TypeId,BrandId,ProductName,SaleStatus,AddedDate,SaleCounts,MarketPrice,Stock,LowestSalePrice,HasSKU,DisplaySequence,ImageUrl,ThumbnailUrl1 ");
                strSql.Append(" FROM  " + TableName);
                if (strWhere.Trim() != "")
                {
                    strSql.Append(" where " + strWhere);
                }
                return DBHelper.DefaultDBHelper.Query(strSql.ToString());
            }
            return null;
        }

        /// <summary>
        /// 获得前几行数据
        /// </summary>
        public DataSet GetList(int supplierId, int Top, string strWhere, string filedOrder)
        {
            string TableName = "Shop_SuppDistProduct_" + supplierId;
            if (TabExists(supplierId))
            {
                StringBuilder strSql = new StringBuilder();
                strSql.Append("select ");
                if (Top > 0)
                {
                    strSql.Append(" top " + Top.ToString());
                }
                strSql.Append(" ProductId,TypeId,BrandId,ProductName,SaleStatus,AddedDate,SaleCounts,MarketPrice,Stock,LowestSalePrice,HasSKU,DisplaySequence,ImageUrl,ThumbnailUrl1 ");
                strSql.Append(" FROM  " + TableName);
                if (strWhere.Trim() != "")
                {
                    strSql.Append(" where " + strWhere);
                }
                strSql.Append(" order by " + filedOrder);
                return DBHelper.DefaultDBHelper.Query(strSql.ToString());
            }
            return null;
        }

        /// <summary>
        /// 获取记录总数
        /// </summary>
        public int GetRecordCount(int supplierId, string strWhere)
        {
            string TableName = "Shop_SuppDistProduct_" + supplierId;
            if (TabExists(supplierId))
            {
                StringBuilder strSql = new StringBuilder();
                strSql.Append("select count(1) FROM  " + TableName);
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
            return 0;
        }
        /// <summary>
        /// 分页获取数据列表
        /// </summary>
        public DataSet GetListByPage(int supplierId, string strWhere, string orderby, int startIndex, int endIndex)
        {
            string TableName = "Shop_SuppDistProduct_" + supplierId;
            if (TabExists(supplierId))
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
                    strSql.Append("order by T.ProductId desc");
                }
                strSql.Append(")AS Row, T.*  from " + TableName + " T ");
                if (!string.IsNullOrEmpty(strWhere.Trim()))
                {
                    strSql.Append(" WHERE " + strWhere);
                }
                strSql.Append(" ) TT");
                strSql.AppendFormat(" WHERE TT.Row between {0} and {1}", startIndex, endIndex);
                return DBHelper.DefaultDBHelper.Query(strSql.ToString());
            }
            return null;
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
            parameters[0].Value = "Shop_SuppDistProduct";
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
        public bool TabExists(int supplierId)
        {
            string TableName = "Shop_SuppDistProduct_" + supplierId;
            string strsql = "select count(*) from sysobjects where id = object_id(N'[" + TableName + "]') and OBJECTPROPERTY(id, N'IsUserTable') = 1";
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

        public void CreateTab(int supplierId)
        {
            if (!TabExists(supplierId))
            {
                string TableName = "Shop_SuppDistProduct_" + supplierId;
                StringBuilder strSql = new StringBuilder();
                strSql.Append("CREATE TABLE [" + TableName + "] (");
                strSql.Append("[ProductId] [bigint] NOT NULL,");
                strSql.Append("[TypeId] [int] NULL,");
                strSql.Append("[BrandId] [int] NOT NULL,");
                strSql.Append("[ProductName] [nvarchar](200) NOT NULL,");
                strSql.Append("[SaleStatus] [int] NOT NULL,");
                strSql.Append("[AddedDate] [datetime] NOT NULL,");
                strSql.Append("[SaleCounts] [int] NOT NULL,");
                strSql.Append("[MarketPrice] [money] NULL,"); 
                strSql.Append("[Stock] [int] NOT NULL,");
                strSql.Append("[LowestSalePrice] [money] NOT NULL,");
                strSql.Append("[HasSKU] [bit] NOT NULL,");
                strSql.Append("[DisplaySequence] [int] NOT NULL,");
                strSql.Append("[ImageUrl] [nvarchar](255) NULL,");
                strSql.Append("[ThumbnailUrl1] [nvarchar](255) NULL,");
                strSql.Append(" CONSTRAINT [PK_" + TableName + "] PRIMARY KEY CLUSTERED  ( [ProductId] ASC ");
                strSql.Append(")WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ");
                strSql.Append(" ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON)  ON [PRIMARY] ) ON [PRIMARY]");
                DBHelper.DefaultDBHelper.ExecuteSql(strSql.ToString());
            }
        }
        #endregion  ExtensionMethod
    }
}

