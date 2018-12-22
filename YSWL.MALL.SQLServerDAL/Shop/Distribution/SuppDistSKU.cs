/**  版本信息模板在安装目录下，可自行修改。
* SuppDistSKU.cs
*
* 功 能： N/A
* 类 名： SuppDistSKU
*
* Ver    变更日期             负责人  变更内容
* ───────────────────────────────────
* V0.01  2014/1/26 18:31:56   N/A    初版
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
using YSWL.DBUtility;
using System.Collections.Generic;//Please add references
namespace YSWL.MALL.SQLServerDAL.Shop.Distribution
{
    /// <summary>
    /// 数据访问类:SuppDistSKU
    /// </summary>
    public partial class SuppDistSKU : ISuppDistSKU
    {
        public SuppDistSKU()
        { }
        #region  BasicMethod

        /// <summary>
        /// 是否存在该记录
        /// </summary>
        public bool Exists(int supplierId,string SKU)
        {
            if (TabExists(supplierId))
            {
                string TableName = "Shop_SuppDistSKU_" + supplierId;
                StringBuilder strSql = new StringBuilder();
                strSql.Append("select count(1) from " + TableName);
                strSql.Append(" where SKU=@SKU ");
                SqlParameter[] parameters = {
					new SqlParameter("@SKU", SqlDbType.NVarChar,50)			};
                parameters[0].Value = SKU;

                return DBHelper.DefaultDBHelper.Exists(strSql.ToString(), parameters);
            }
            return false;
        }


        /// <summary>
        /// 增加一条数据
        /// </summary>
        public bool Add(int supplierId,YSWL.MALL.Model.Shop.Distribution.SuppDistSKU model)
        {
            string TableName = "Shop_SuppDistSKU_" + supplierId;
            CreateTab(supplierId);
            StringBuilder strSql = new StringBuilder();
            strSql.Append("insert into " + TableName + "(");
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
        public bool Update(int supplierId,YSWL.MALL.Model.Shop.Distribution.SuppDistSKU model)
        {
            if (TabExists(supplierId))
            {
                string TableName = "Shop_SuppDistSKU_" + supplierId;
                StringBuilder strSql = new StringBuilder();
                strSql.Append("update " + TableName + " set ");
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
        public bool Delete(int supplierId, string SKU)
        {
            if (TabExists(supplierId))
            {
                string TableName = "Shop_SuppDistSKU_" + supplierId;
                StringBuilder strSql = new StringBuilder();
                strSql.Append("delete from " + TableName);
                strSql.Append(" where SKU=@SKU ");
                SqlParameter[] parameters = {
					new SqlParameter("@SKU", SqlDbType.NVarChar,50)			};
                parameters[0].Value = SKU;

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
        public bool DeleteList(int supplierId, string SKUlist)
        {
            if (TabExists(supplierId))
            {
                string TableName = "Shop_SuppDistSKU_" + supplierId;
                StringBuilder strSql = new StringBuilder();
                strSql.Append("delete from  " + TableName);
                strSql.Append(" where SKU in (" + SKUlist + ")  ");
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
        public YSWL.MALL.Model.Shop.Distribution.SuppDistSKU GetModel(int supplierId, string SKU)
        {
            string TableName = "Shop_SuppDistSKU_" + supplierId;
            if (TabExists(supplierId))
            {
                StringBuilder strSql = new StringBuilder();
                strSql.Append("select  top 1 SKU,ProductId,Weight,Stock,AlertStock,CostPrice,SalePrice,Upselling from  " + TableName);
                strSql.Append(" where SKU=@SKU ");
                SqlParameter[] parameters = {
					new SqlParameter("@SKU", SqlDbType.NVarChar,50)			};
                parameters[0].Value = SKU;

                YSWL.MALL.Model.Shop.Distribution.SuppDistSKU model = new YSWL.MALL.Model.Shop.Distribution.SuppDistSKU();
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
        public YSWL.MALL.Model.Shop.Distribution.SuppDistSKU DataRowToModel(DataRow row)
        {
            YSWL.MALL.Model.Shop.Distribution.SuppDistSKU model = new YSWL.MALL.Model.Shop.Distribution.SuppDistSKU();
            if (row != null)
            {
                if (row["SKU"] != null)
                {
                    model.SKU = row["SKU"].ToString();
                }
                if (row["ProductId"] != null && row["ProductId"].ToString() != "")
                {
                    model.ProductId = long.Parse(row["ProductId"].ToString());
                }
                if (row["Weight"] != null && row["Weight"].ToString() != "")
                {
                    model.Weight = int.Parse(row["Weight"].ToString());
                }
                if (row["Stock"] != null && row["Stock"].ToString() != "")
                {
                    model.Stock = int.Parse(row["Stock"].ToString());
                }
                if (row["AlertStock"] != null && row["AlertStock"].ToString() != "")
                {
                    model.AlertStock = int.Parse(row["AlertStock"].ToString());
                }
                if (row["CostPrice"] != null && row["CostPrice"].ToString() != "")
                {
                    model.CostPrice = decimal.Parse(row["CostPrice"].ToString());
                }
                if (row["SalePrice"] != null && row["SalePrice"].ToString() != "")
                {
                    model.SalePrice = decimal.Parse(row["SalePrice"].ToString());
                }
                if (row["Upselling"] != null && row["Upselling"].ToString() != "")
                {
                    if ((row["Upselling"].ToString() == "1") || (row["Upselling"].ToString().ToLower() == "true"))
                    {
                        model.Upselling = true;
                    }
                    else
                    {
                        model.Upselling = false;
                    }
                }
            }
            return model;
        }

        /// <summary>
        /// 获得数据列表
        /// </summary>
        public DataSet GetList(int supplierId, string strWhere)
        {
            string TableName = "Shop_SuppDistSKU_" + supplierId;
            if (TabExists(supplierId))
            {
                StringBuilder strSql = new StringBuilder();
                strSql.Append("select SKU,ProductId,Weight,Stock,AlertStock,CostPrice,SalePrice,Upselling ");
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
              string TableName = "Shop_SuppDistSKU_" + supplierId;
              if (TabExists(supplierId))
              {
                  StringBuilder strSql = new StringBuilder();
                  strSql.Append("select ");
                  if (Top > 0)
                  {
                      strSql.Append(" top " + Top.ToString());
                  }
                  strSql.Append(" SKU,ProductId,Weight,Stock,AlertStock,CostPrice,SalePrice,Upselling ");
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
               string TableName = "Shop_SuppDistSKU_" + supplierId;
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
            string TableName = "Shop_SuppDistSKU_" + supplierId;
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
                    strSql.Append("order by T.SKU desc");
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
            parameters[0].Value = "Shop_SuppDistSKU";
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
        public bool TabExists(int  supplierId)
        {
            string TableName = "Shop_SuppDistSKU_" + supplierId;
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

        public void CreateTab(int  supplierId)
        {
            if (!TabExists(supplierId))
            {
                string TableName = "Shop_SuppDistSKU_" + supplierId;
                StringBuilder strSql = new StringBuilder();
                strSql.Append("CREATE TABLE [" + TableName + "] (");
                strSql.Append(" [SKU] [nvarchar](50) NOT NULL,");
                strSql.Append(" [ProductId] [bigint] NOT NULL,");
                strSql.Append(" [Weight] [int] NOT NULL,");
                strSql.Append(" [Stock] [int] NOT NULL,");
                strSql.Append(" [AlertStock] [int] NOT NULL,");
                strSql.Append(" [CostPrice] [money] NOT NULL,");
                strSql.Append(" [SalePrice] [money] NOT NULL,");
                strSql.Append(" [Upselling] [bit] NOT NULL,");
                strSql.Append(" CONSTRAINT [PK_" + TableName + "] PRIMARY KEY CLUSTERED  ( [SKU] ASC ");
                strSql.Append(")WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ");
                strSql.Append(" ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON)  ON [PRIMARY] ) ON [PRIMARY]");
                DBHelper.DefaultDBHelper.ExecuteSql(strSql.ToString());
            }
        }

        /// <summary>
        /// 删除一条数据
        /// </summary>
        public bool DeleteEx(int supplierId, YSWL.MALL.Model.Shop.Distribution.SuppDistSKU SKUInfo)
        {
            if (TabExists(supplierId))
            {
                string TableName = "Shop_SuppDistSKU_" + supplierId;
                string ProductTable = "Shop_SuppDistProduct_" + supplierId;
                List<CommandInfo> sqllist = new List<CommandInfo>();
                CommandInfo cmd = new CommandInfo();
                StringBuilder strSql = new StringBuilder();
                strSql.Append("delete from " + TableName);
                strSql.Append(" where SKU=@SKU ");
                SqlParameter[] parameters = {
					new SqlParameter("@SKU", SqlDbType.NVarChar,50)			
                                            };
                parameters[0].Value = SKUInfo.SKU;
                cmd = new CommandInfo(strSql.ToString(), parameters);
                sqllist.Add(cmd);
                //更新 分销商库存
                StringBuilder strSql2 = new StringBuilder();
                strSql2.Append("update " + ProductTable + " set ");
                strSql2.Append("Stock=Stock-@Stock");
                strSql2.Append(" where ProductId=@ProductId ");
                SqlParameter[] parameters2 = {
					new SqlParameter("@Stock", SqlDbType.Int,4),
					new SqlParameter("@ProductId", SqlDbType.BigInt,8)};
                parameters2[0].Value = SKUInfo.Stock;
                parameters2[1].Value = SKUInfo.ProductId;
                cmd = new CommandInfo(strSql2.ToString(), parameters2);
                sqllist.Add(cmd);

               //更新整个商品SKU库存
                StringBuilder strSql3 = new StringBuilder();
                strSql3.Append("UPDATE PMS_SKUs SET ");
                strSql3.Append("Stock=Stock+@Stock");
                strSql3.Append(" WHERE Sku=@SKU");
                SqlParameter[] parameters3 = {
                    new SqlParameter("@Stock", SqlDbType.Int,4),
                    new SqlParameter("@SKU", SqlDbType.NVarChar,50)
                                             };
                parameters3[0].Value = SKUInfo.Stock;
                parameters3[1].Value = SKUInfo.SKU;
                cmd = new CommandInfo(strSql3.ToString(), parameters3);
                sqllist.Add(cmd);

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
            return false;
        }
        #endregion  ExtensionMethod
    }
}

