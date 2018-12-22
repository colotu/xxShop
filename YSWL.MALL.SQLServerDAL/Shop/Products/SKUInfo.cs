/*----------------------------------------------------------------
// Copyright (C) 2012 云商未来 版权所有。
//
// 文件名：SKUs.cs
// 文件功能描述：
// 
// 创建标识： [Ben]  2012/06/11 20:36:34
// 修改标识：
// 修改描述：
//
// 修改标识：
// 修改描述：
//----------------------------------------------------------------*/
using System;
using System.Data;
using System.Text;
using System.Data.SqlClient;
using YSWL.MALL.IDAL.Shop.Products;
using YSWL.DBUtility;
using System.Collections.Generic;
namespace YSWL.MALL.SQLServerDAL.Shop.Products
{
    /// <summary>
    /// 数据访问类:SKUInfo
    /// </summary>
    public partial class SKUInfo : ISKUInfo
    {
        public SKUInfo()
        { }
        #region  Method

        /// <summary>
        /// 是否存在该记录
        /// </summary>
        public bool Exists(long SkuId)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("SELECT COUNT(1) FROM PMS_SKUs");
            strSql.Append(" WHERE SkuId=@SkuId");
            SqlParameter[] parameters = {
                    new SqlParameter("@SkuId", SqlDbType.BigInt)
            };
            parameters[0].Value = SkuId;
            return DBHelper.DefaultDBHelper.Exists(strSql.ToString(), parameters);
        }


        /// <summary>
        /// 增加一条数据
        /// </summary>
        public long Add(YSWL.MALL.Model.Shop.Products.SKUInfo model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("INSERT INTO PMS_SKUs(");
            strSql.Append("ProductId,SKU,Weight,Stock,AlertStock,CostPrice,SalePrice,Upselling,Gwjf)");
            strSql.Append(" VALUES (");
            strSql.Append("@ProductId,@SKU,@Weight,@Stock,@AlertStock,@CostPrice,@SalePrice,@Upselling,@Gwjf)");
            strSql.Append(";SELECT @@IDENTITY");
            SqlParameter[] parameters = {
                    new SqlParameter("@ProductId", SqlDbType.BigInt,8),
                    new SqlParameter("@SKU", SqlDbType.NVarChar,50),
                    new SqlParameter("@Weight", SqlDbType.Int,4),
                    new SqlParameter("@Stock", SqlDbType.Int,4),
                    new SqlParameter("@AlertStock", SqlDbType.Int,4),
                    new SqlParameter("@CostPrice", SqlDbType.Money,8),
                    new SqlParameter("@SalePrice", SqlDbType.Money,8),
                    new SqlParameter("@Upselling", SqlDbType.Bit,1),
                     new SqlParameter("@Gwjf", SqlDbType.Decimal,9)};
            parameters[0].Value = model.ProductId;
            parameters[1].Value = model.SKU;
            parameters[2].Value = model.Weight;
            parameters[3].Value = model.Stock;
            parameters[4].Value = model.AlertStock;
            parameters[5].Value = model.CostPrice;
            parameters[6].Value = model.SalePrice;
            parameters[7].Value = model.Upselling;
            parameters[8].Value = model.Gwjf;

            object obj = DBHelper.DefaultDBHelper.GetSingle(strSql.ToString(), parameters);
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
        public bool Update(YSWL.MALL.Model.Shop.Products.SKUInfo model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("UPDATE PMS_SKUs SET ");
            strSql.Append("ProductId=@ProductId,");
            strSql.Append("SKU=@SKU,");
            strSql.Append("Weight=@Weight,");
            strSql.Append("Stock=@Stock,");
            strSql.Append("AlertStock=@AlertStock,");
            strSql.Append("CostPrice=@CostPrice,");
            strSql.Append("SalePrice=@SalePrice,");
            strSql.Append("Upselling=@Upselling,");
            strSql.Append("Gwjf=@Gwjf");
            strSql.Append(" WHERE SkuId=@SkuId");
            SqlParameter[] parameters = {
                    new SqlParameter("@ProductId", SqlDbType.BigInt,8),
                    new SqlParameter("@SKU", SqlDbType.NVarChar,50),
                    new SqlParameter("@Weight", SqlDbType.Int,4),
                    new SqlParameter("@Stock", SqlDbType.Int,4),
                    new SqlParameter("@AlertStock", SqlDbType.Int,4),
                    new SqlParameter("@CostPrice", SqlDbType.Money,8),
                    new SqlParameter("@SalePrice", SqlDbType.Money,8),
                    new SqlParameter("@Upselling", SqlDbType.Bit,1),
                    new SqlParameter("@Gwjf", SqlDbType.Decimal,9),
                    new SqlParameter("@SkuId", SqlDbType.BigInt,8)};
            parameters[0].Value = model.ProductId;
            parameters[1].Value = model.SKU;
            parameters[2].Value = model.Weight;
            parameters[3].Value = model.Stock;
            parameters[4].Value = model.AlertStock;
            parameters[5].Value = model.CostPrice;
            parameters[6].Value = model.SalePrice;
            parameters[7].Value = model.Upselling;
            parameters[8].Value = model.Gwjf;
            parameters[9].Value = model.SkuId;

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
        public bool Delete(long SkuId)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append("DELETE FROM PMS_SKUs ");
            strSql.Append(" WHERE SkuId=@SkuId");
            SqlParameter[] parameters = {
                    new SqlParameter("@SkuId", SqlDbType.BigInt)
            };
            parameters[0].Value = SkuId;

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
        public bool DeleteList(string SkuIdlist)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("DELETE FROM PMS_SKUs ");
            strSql.Append(" WHERE SkuId in (" + SkuIdlist + ")  ");
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
        public YSWL.MALL.Model.Shop.Products.SKUInfo GetModel(long SkuId)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append("SELECT  TOP 1 SkuId,ProductId,SKU,Weight,Stock,AlertStock,CostPrice,SalePrice,Upselling,Gwjf FROM PMS_SKUs ");
            strSql.Append(" WHERE SkuId=@SkuId");
            SqlParameter[] parameters = {
                    new SqlParameter("@SkuId", SqlDbType.BigInt)
            };
            parameters[0].Value = SkuId;

            YSWL.MALL.Model.Shop.Products.SKUInfo model = new YSWL.MALL.Model.Shop.Products.SKUInfo();
            DataSet ds = DBHelper.DefaultDBHelper.Query(strSql.ToString(), parameters);
            if (ds.Tables[0].Rows.Count > 0)
            {
                if (ds.Tables[0].Rows[0]["SkuId"] != null && ds.Tables[0].Rows[0]["SkuId"].ToString() != "")
                {
                    model.SkuId = long.Parse(ds.Tables[0].Rows[0]["SkuId"].ToString());
                }
                if (ds.Tables[0].Rows[0]["ProductId"] != null && ds.Tables[0].Rows[0]["ProductId"].ToString() != "")
                {
                    model.ProductId = long.Parse(ds.Tables[0].Rows[0]["ProductId"].ToString());
                }
                if (ds.Tables[0].Rows[0]["SKU"] != null && ds.Tables[0].Rows[0]["SKU"].ToString() != "")
                {
                    model.SKU = ds.Tables[0].Rows[0]["SKU"].ToString();
                }
                if (ds.Tables[0].Rows[0]["Weight"] != null && ds.Tables[0].Rows[0]["Weight"].ToString() != "")
                {
                    model.Weight = int.Parse(ds.Tables[0].Rows[0]["Weight"].ToString());
                }
                if (ds.Tables[0].Rows[0]["Stock"] != null && ds.Tables[0].Rows[0]["Stock"].ToString() != "")
                {
                    model.Stock = int.Parse(ds.Tables[0].Rows[0]["Stock"].ToString());
                }
                if (ds.Tables[0].Rows[0]["AlertStock"] != null && ds.Tables[0].Rows[0]["AlertStock"].ToString() != "")
                {
                    model.AlertStock = int.Parse(ds.Tables[0].Rows[0]["AlertStock"].ToString());
                }
                if (ds.Tables[0].Rows[0]["CostPrice"] != null && ds.Tables[0].Rows[0]["CostPrice"].ToString() != "")
                {
                    model.CostPrice = decimal.Parse(ds.Tables[0].Rows[0]["CostPrice"].ToString());
                }
                if (ds.Tables[0].Rows[0]["SalePrice"] != null && ds.Tables[0].Rows[0]["SalePrice"].ToString() != "")
                {
                    model.SalePrice = decimal.Parse(ds.Tables[0].Rows[0]["SalePrice"].ToString());
                }
                if (ds.Tables[0].Rows[0]["Upselling"] != null && ds.Tables[0].Rows[0]["Upselling"].ToString() != "")
                {
                    if ((ds.Tables[0].Rows[0]["Upselling"].ToString() == "1") || (ds.Tables[0].Rows[0]["Upselling"].ToString().ToLower() == "true"))
                    {
                        model.Upselling = true;
                    }
                    else
                    {
                        model.Upselling = false;
                    }
                }

                if (ds.Tables[0].Rows[0]["Gwjf"] != null && ds.Tables[0].Rows[0]["Gwjf"].ToString() != "")
                {
                    model.Gwjf = decimal.Parse(ds.Tables[0].Rows[0]["Gwjf"].ToString());
                }

                return model;
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// 获得数据列表
        /// </summary>
        public DataSet GetList(string strWhere)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("SELECT SkuId,ProductId,SKU,Weight,Stock,AlertStock,CostPrice,SalePrice,Upselling,Gwjf ");
            strSql.Append(" FROM PMS_SKUs ");
            if (!string.IsNullOrWhiteSpace(strWhere.Trim()))
            {
                strSql.Append(" WHERE " + strWhere);
            }
            return DBHelper.DefaultDBHelper.Query(strSql.ToString());
        }

        /// <summary>
        /// 获得前几行数据
        /// </summary>
        public DataSet GetList(int Top, string strWhere, string filedOrder)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("SELECT ");
            if (Top > 0)
            {
                strSql.Append(" TOP " + Top.ToString());
            }
            strSql.Append(" SkuId,ProductId,SKU,Weight,Stock,AlertStock,CostPrice,SalePrice,Upselling,Gwjf ");
            strSql.Append(" FROM PMS_SKUs ");
            if (!string.IsNullOrWhiteSpace(strWhere.Trim()))
            {
                strSql.Append(" WHERE " + strWhere);
            }
            strSql.Append(" ORDER BY " + filedOrder);
            return DBHelper.DefaultDBHelper.Query(strSql.ToString());
        }

        /// <summary>
        /// 获取记录总数
        /// </summary>
        public int GetRecordCount(string strWhere)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("SELECT COUNT(1) FROM PMS_SKUs ");
            if (!string.IsNullOrWhiteSpace(strWhere.Trim()))
            {
                strSql.Append(" WHERE " + strWhere);
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
            if (!string.IsNullOrWhiteSpace(orderby.Trim()))
            {
                strSql.Append("ORDER BY T." + orderby);
            }
            else
            {
                strSql.Append("ORDER BY T.SkuId desc");
            }
            strSql.Append(")AS Row, T.*  FROM PMS_SKUs T ");
            if (!string.IsNullOrWhiteSpace(strWhere.Trim()))
            {
                strSql.Append(" WHERE " + strWhere);
            }
            strSql.Append(" ) TT");
            strSql.AppendFormat(" WHERE TT.Row BETWEEN {0} AND {1}", startIndex, endIndex);
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
            parameters[0].Value = "PMS_SKUs";
            parameters[1].Value = "SkuId";
            parameters[2].Value = PageSize;
            parameters[3].Value = PageIndex;
            parameters[4].Value = 0;
            parameters[5].Value = 0;
            parameters[6].Value = strWhere;	
            return DBHelper.DefaultDBHelper.RunProcedure("UP_GetRecordByPage",parameters,"ds");
        }*/

        #endregion  Method

        #region NewMethod
        /// <summary>
        /// 分页获取SKU胶塑列表
        /// </summary>
        public DataSet GetSKUListByPage(string strWhere, string orderby, int startIndex, int endIndex, out int dataCount,long productId)
        {
            //            StringBuilder strSql = new StringBuilder();
            //            strSql.Append("SELECT * FROM ( ");
            //            strSql.Append(" SELECT ROW_NUMBER() OVER (");
            //            if (!string.IsNullOrWhiteSpace(orderby))
            //            {
            //                strSql.Append("ORDER BY T." + orderby);
            //            }
            //            else
            //            {
            //                strSql.Append("ORDER BY T.SkuId desc");
            //            }
            //            strSql.Append(")AS Row, T.*  FROM " +
            //                          @"(SELECT SP.* ,
            //                                    SI.AttributeId ,
            //                                    SI.ValueId ,
            //                                    AV.ValueStr
            //                            FROM    ( SELECT    S.* ,
            //                                                P.ProductName
            //                                        FROM      PMS_SKUs S
            //                                                LEFT JOIN PMS_Products P ON S.ProductId = P.ProductId");
            //            if (!string.IsNullOrWhiteSpace(strWhere.Trim()))
            //            {
            //                strSql.Append(" WHERE " + strWhere);
            //            }
            //            strSql.Append(@" ) SP ,
            //                                    PMS_SKUItems SI ,
            //                                    PMS_AttributeValues AV
            //                            WHERE   SP.SkuId = SI.SkuId AND AV.AttributeId = SI.AttributeId
            //                            AND AV.ValueId = SI.ValueId) T ");
            //            strSql.Append(" ) TT");
            //            strSql.AppendFormat(" WHERE TT.Row BETWEEN {0} AND {1}", startIndex, endIndex);
            //            return DBHelper.DefaultDBHelper.Query(strSql.ToString());
            //if (!string.IsNullOrWhiteSpace(strWhere))
            //{
            //    strWhere = strWhere.Insert(0, " AND ");
            //}
            SqlParameter[] parameters = {
                                            new SqlParameter("@SqlWhere", SqlDbType.NVarChar, 4000),
                                            new SqlParameter("@OrderBy", SqlDbType.NVarChar, 1000),
                                            new SqlParameter("@StartIndex", SqlDbType.Int, 4),
                                            new SqlParameter("@EndIndex", SqlDbType.Int, 4),
                                            new SqlParameter("@ProductId", SqlDbType.BigInt, 8),
                                            DBHelper.DefaultDBHelper.CreateReturnParam("ReturnValue", SqlDbType.Int, 4)
                                        };
            parameters[0].Value = strWhere;
            parameters[1].Value = orderby;
            parameters[2].Value = startIndex;
            parameters[3].Value = endIndex;
            parameters[4].Value = productId;
            return DBHelper.DefaultDBHelper.RunProcedure("sp_PMS_ProductSkuInfo_Get", parameters, "ProductSkuInfo", out dataCount);
        }


        public DataSet PrductsSkuInfo(long prductId)
        {
            StringBuilder strSql = new StringBuilder();

            strSql.Append("SELECT * FROM  PMS_SKUs ");
            strSql.Append("WHERE ProductId=@ProducId ");
            SqlParameter[] parameters = { 
                                        new SqlParameter("@ProducId",SqlDbType.BigInt,8)
                                        };
            parameters[0].Value = prductId;
            return DBHelper.DefaultDBHelper.Query(strSql.ToString(), parameters);
        }


        /// <summary>
        /// 是否存在该记录
        /// </summary>
        /// <remarks>prductId 编辑时使用, 排除自己</remarks>
        public bool Exists(string skuCode, long prductId)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("SELECT COUNT(1) FROM PMS_SKUs");
            strSql.Append(" WHERE SKU=@SkuCode");
            if (prductId > 0)
            {
                strSql.Append(" AND ProductId<>" + prductId);
            }
            SqlParameter[] parameters = {
                    new SqlParameter("@SkuCode", SqlDbType.NVarChar)
            };
            parameters[0].Value = skuCode;
            return DBHelper.DefaultDBHelper.Exists(strSql.ToString(), parameters);
        }

        public int GetStockById(long productId)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("SELECT SUM(Stock)Stock FROM PMS_SKUs ");
            strSql.Append("WHERE ProductId=@ProductId ");

            SqlParameter[] parameters = {
					new SqlParameter("@ProductId", SqlDbType.BigInt)
			};
            parameters[0].Value = productId;
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

        public int GetStockBySKU(string SKU,bool IsOpenAS)
        {

            StringBuilder strSql = new StringBuilder();
            if (IsOpenAS)
            {
                strSql.Append("SELECT  (Stock-AlertStock) as AlertStock FROM PMS_SKUs ");
            }
            else
            {
                strSql.Append("SELECT  Stock  FROM PMS_SKUs ");
            }
         
            strSql.Append("WHERE SKU=@SKU ");

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
        /// 得到一个对象实体
        /// </summary>
        public YSWL.MALL.Model.Shop.Products.SKUInfo GetModelBySKU(string sku)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("SELECT  TOP 1 SkuId,ProductId,SKU,Weight,Stock,AlertStock,CostPrice,SalePrice,Upselling FROM PMS_SKUs ");
            strSql.Append(" WHERE SKU=@SKU");
            SqlParameter[] parameters = {
                    new SqlParameter("@SKU", SqlDbType.NVarChar,50)
            };
            parameters[0].Value = sku;

            YSWL.MALL.Model.Shop.Products.SKUInfo model = new YSWL.MALL.Model.Shop.Products.SKUInfo();
            DataSet ds = DBHelper.DefaultDBHelper.Query(strSql.ToString(), parameters);
            if (ds.Tables[0].Rows.Count > 0)
            {
                if (ds.Tables[0].Rows[0]["SkuId"] != null && ds.Tables[0].Rows[0]["SkuId"].ToString() != "")
                {
                    model.SkuId = long.Parse(ds.Tables[0].Rows[0]["SkuId"].ToString());
                }
                if (ds.Tables[0].Rows[0]["ProductId"] != null && ds.Tables[0].Rows[0]["ProductId"].ToString() != "")
                {
                    model.ProductId = long.Parse(ds.Tables[0].Rows[0]["ProductId"].ToString());
                }
                if (ds.Tables[0].Rows[0]["SKU"] != null && ds.Tables[0].Rows[0]["SKU"].ToString() != "")
                {
                    model.SKU = ds.Tables[0].Rows[0]["SKU"].ToString();
                }
                if (ds.Tables[0].Rows[0]["Weight"] != null && ds.Tables[0].Rows[0]["Weight"].ToString() != "")
                {
                    model.Weight = int.Parse(ds.Tables[0].Rows[0]["Weight"].ToString());
                }
                if (ds.Tables[0].Rows[0]["Stock"] != null && ds.Tables[0].Rows[0]["Stock"].ToString() != "")
                {
                    model.Stock = int.Parse(ds.Tables[0].Rows[0]["Stock"].ToString());
                }
                if (ds.Tables[0].Rows[0]["AlertStock"] != null && ds.Tables[0].Rows[0]["AlertStock"].ToString() != "")
                {
                    model.AlertStock = int.Parse(ds.Tables[0].Rows[0]["AlertStock"].ToString());
                }
                if (ds.Tables[0].Rows[0]["CostPrice"] != null && ds.Tables[0].Rows[0]["CostPrice"].ToString() != "")
                {
                    model.CostPrice = decimal.Parse(ds.Tables[0].Rows[0]["CostPrice"].ToString());
                }
                if (ds.Tables[0].Rows[0]["SalePrice"] != null && ds.Tables[0].Rows[0]["SalePrice"].ToString() != "")
                {
                    model.SalePrice = decimal.Parse(ds.Tables[0].Rows[0]["SalePrice"].ToString());
                }
                if (ds.Tables[0].Rows[0]["Upselling"] != null && ds.Tables[0].Rows[0]["Upselling"].ToString() != "")
                {
                    if ((ds.Tables[0].Rows[0]["Upselling"].ToString() == "1") || (ds.Tables[0].Rows[0]["Upselling"].ToString().ToLower() == "true"))
                    {
                        model.Upselling = true;
                    }
                    else
                    {
                        model.Upselling = false;
                    }
                }
                return model;
            }
            else
            {
                return null;
            }
        }


       
        /// <summary>
        /// 是否存在该记录
        /// </summary>
        /// <remarks>添加组合商品时，判断这个sku是否是自己的</remarks>
        public bool ExistsEx(string SKU, long prductId)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("SELECT COUNT(1) FROM PMS_SKUs");
            strSql.Append(" WHERE SKU=@SKU");
            strSql.Append(" AND ProductId=" + prductId);
            SqlParameter[] parameters = {
                    new SqlParameter("@SKU", SqlDbType.NVarChar)
            };
            parameters[0].Value = SKU;
            return DBHelper.DefaultDBHelper.Exists(strSql.ToString(), parameters);
        }
        /// <summary>
        /// 获得数据列表
        /// </summary>
        public DataSet GetListInnerJoinProd(string strWhere)
        {
            //SELECT s.*,p.ProductName AS  PMS_Products,p.ThumbnailUrl1 AS ThumbnailUrl1   FROM PMS_SKUs s INNER JOIN     PMS_Products p ON  s.ProductId=p.ProductId

            StringBuilder strSql = new StringBuilder();
            strSql.Append(" SELECT s.*,p.ProductName AS  ProductName ,p.ThumbnailUrl1 AS ThumbnailUrl1   ");
            strSql.Append(" FROM PMS_SKUs s");
            strSql.Append(" INNER JOIN  PMS_Products p ON  s.ProductId=p.ProductId ");   
            if (!string.IsNullOrWhiteSpace(strWhere.Trim()))
            {
                strSql.Append(" WHERE " + strWhere);
            }
            return DBHelper.DefaultDBHelper.Query(strSql.ToString());
        }
        /// <summary>
        /// 获取SKU数据列表
        /// </summary>
        public DataSet GetSKUList(string strWhere,int AccessoriesId ,  string orderby , long productId)
        { 
            SqlParameter[] parameters = {
                                            new SqlParameter("@SqlWhere", SqlDbType.NVarChar, 4000),
                                            new SqlParameter("@OrderBy", SqlDbType.NVarChar, 1000), 
                                            new SqlParameter("@AccessoriesId", SqlDbType.Int), 
                                            new SqlParameter("@ProductId", SqlDbType.BigInt, 8)
                                        };
            parameters[0].Value = strWhere;
            parameters[1].Value = orderby;
            parameters[2].Value = AccessoriesId;
            parameters[3].Value = productId;
            return DBHelper.DefaultDBHelper.RunProcedure("sp_PMS_ProductSkuInfo_NotPage_GetProdAcce", parameters, "ProductSkuInfoGetProdAcce");
        }

        /// <summary>
        /// 根据商品分类 获取SKU
        /// </summary>
        /// <param name="Cid"></param>
        /// <returns></returns>
        public DataSet GetSKUListByCid(int Cid)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append(" SELECT * ");
            strSql.Append(" FROM PMS_SKUs K ");
            strSql.Append(" WHERE   EXISTS ( SELECT ProductId FROM   PMS_Products P WHERE  P.SaleStatus = 1  ");
            //查询分类
            if (Cid > 0)
            {
                strSql.AppendFormat(
                    " AND EXISTS ( SELECT *  FROM   PMS_ProductCategories WHERE  ProductId =P.ProductId  ");
                strSql.AppendFormat(
              "   AND ( CategoryPath LIKE ( SELECT Path FROM PMS_Categories WHERE CategoryId = {0}  ) + '|%' ",
              Cid);
                strSql.AppendFormat(" OR PMS_ProductCategories.CategoryId = {0}))", Cid);
            }
            strSql.Append("   AND P.ProductId = K.ProductId ) ");
            return DBHelper.DefaultDBHelper.Query(strSql.ToString());
        }



        /// <summary>
        /// 根据商品分类 获取SKU
        /// </summary>
        /// <param name="Cid"></param>
        /// <returns></returns>
        public DataSet GetSKUListEx(int Cid, int supplierId, string productName, string productNum, bool showAlert=false)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append(" SELECT * ");
            strSql.Append(" FROM PMS_SKUs K ");
            strSql.Append(" WHERE   EXISTS ( SELECT ProductId FROM   PMS_Products P WHERE  P.SaleStatus = 1  ");
            //查询分类
            if (Cid > 0)
            {
                strSql.AppendFormat(
                    " AND EXISTS ( SELECT *  FROM   PMS_ProductCategories WHERE  ProductId =P.ProductId  ");
                strSql.AppendFormat(
              "   AND ( CategoryPath LIKE ( SELECT Path FROM PMS_Categories WHERE CategoryId = {0}  ) + '|%' ",
              Cid);
                strSql.AppendFormat(" OR PMS_ProductCategories.CategoryId = {0}))", Cid);
            }
            if (supplierId !=0)
            {
                strSql.AppendFormat(
                    " AND  P.SupplierId={0} ",supplierId);
            }
            if (!string.IsNullOrWhiteSpace(productName))
            {
                strSql.AppendFormat(
                   " AND   P.ProductName LIKE '%{0}%' ", Common.InjectionFilter.SqlFilter(productName));
            }
            if (!string.IsNullOrWhiteSpace(productNum))
            {
                strSql.AppendFormat(
                   " AND  P.ProductCode LIKE '%{0}%' ", Common.InjectionFilter.SqlFilter(productNum));
            }
            if (showAlert)
            {
                strSql.Append("  AND  K.Stock<=K.AlertStock  ");
            }
            strSql.Append("   AND P.ProductId = K.ProductId ) ");
            return DBHelper.DefaultDBHelper.Query(strSql.ToString());
        }






        /// <summary>
        /// 获取分销商库存
        /// </summary>
        /// <param name="SkuInfo"></param>
        /// <param name="Stock"></param>
        /// <param name="supplierId"></param>
        /// <returns></returns>
        public bool UpdateSuppStock(YSWL.MALL.Model.Shop.Products.SKUInfo SkuInfo, int Stock,int supplierId)
        {
            #region 事务处理数据
            YSWL.MALL.SQLServerDAL.Shop.Distribution.SuppDistProduct distProductBll = new Distribution.SuppDistProduct();
            YSWL.MALL.SQLServerDAL.Shop.Distribution.SuppDistSKU distSkuBll = new Distribution.SuppDistSKU();
            // 首先 获取商品
            List<CommandInfo> sqllist = new List<CommandInfo>();
            CommandInfo cmd = new CommandInfo();
            string ProductTable = "Shop_SuppDistProduct_" + supplierId;
            if (!distProductBll.Exists(supplierId, SkuInfo.ProductId))
            {
                StringBuilder strSql = new StringBuilder();
                distProductBll.CreateTab(supplierId);
                strSql.Append("insert into " + ProductTable + " (");
                strSql.Append("ProductId,TypeId,BrandId,ProductName,SaleStatus,AddedDate,SaleCounts,MarketPrice,Stock,LowestSalePrice,HasSKU,DisplaySequence,ImageUrl,ThumbnailUrl1)");
                strSql.Append(" Select ProductId,TypeId,BrandId,ProductName,SaleStatus,AddedDate,SaleCounts,MarketPrice,0,LowestSalePrice,HasSKU,DisplaySequence,ImageUrl,ThumbnailUrl1 ");
                strSql.Append(" from PMS_Products where productId=@ProductId ");
                SqlParameter[] parameters = {
					new SqlParameter("@ProductId", SqlDbType.BigInt,8)
                                            };
                parameters[0].Value = SkuInfo.ProductId;
                cmd = new CommandInfo(strSql.ToString(), parameters);
                sqllist.Add(cmd);
            }
            
            //更新分销商库存
            if (distProductBll.TabExists(supplierId))
            {
                StringBuilder strSql2 = new StringBuilder();
                strSql2.Append("update " + ProductTable + " set ");
                strSql2.Append("Stock=Stock+@Stock");
                strSql2.Append(" where ProductId=@ProductId ");
                SqlParameter[] parameters2 = {
					new SqlParameter("@Stock", SqlDbType.Int,4),
					new SqlParameter("@ProductId", SqlDbType.BigInt,8)};
                parameters2[0].Value = Stock;
                parameters2[1].Value = SkuInfo.ProductId;
                cmd = new CommandInfo(strSql2.ToString(), parameters2);
                sqllist.Add(cmd);
            }
           //更新商品SKU的库存
            StringBuilder strSql3 = new StringBuilder();
            strSql3.Append("UPDATE PMS_SKUs SET ");
            strSql3.Append("Stock=Stock-@Stock");
            strSql3.Append(" WHERE SkuId=@SkuId");
            SqlParameter[] parameters3 = {
                    new SqlParameter("@Stock", SqlDbType.Int,4),
                    new SqlParameter("@SkuId", SqlDbType.BigInt,8)};
            parameters3[0].Value = Stock;
            parameters3[1].Value = SkuInfo.SkuId;
            cmd = new CommandInfo(strSql3.ToString(), parameters3);
            sqllist.Add(cmd);

            //添加分销商SKU
            string SkuTable = "Shop_SuppDistSKU_" + supplierId;
            if (!distSkuBll.Exists(supplierId, SkuInfo.SKU))
            {
                StringBuilder strSql4 = new StringBuilder();
                distSkuBll.CreateTab(supplierId);
                strSql4.Append("insert into " + SkuTable + "(");
                strSql4.Append("SKU,ProductId,Weight,Stock,AlertStock,CostPrice,SalePrice,Upselling)");
                strSql4.Append(" Select SKU,ProductId,Weight,0,AlertStock,CostPrice,SalePrice,Upselling ");
                strSql4.Append(" from PMS_SKUs where SKU=@SKU  ");
                SqlParameter[] parameters4 = {
					new SqlParameter("@SKU", SqlDbType.NVarChar,50)
                                            };
                parameters4[0].Value = SkuInfo.SKU;
                cmd = new CommandInfo(strSql4.ToString(), parameters4);
                sqllist.Add(cmd);
            }
            //更新分销商库存
            if (distSkuBll.TabExists(supplierId))
            {
                StringBuilder strSql5 = new StringBuilder();
                strSql5.Append("update " + SkuTable + " set ");
                strSql5.Append("Stock=Stock+@Stock");
                strSql5.Append(" where  SKU=@SKU ");
                SqlParameter[] parameters5 = {
					new SqlParameter("@Stock", SqlDbType.Int,4),
					new SqlParameter("@SKU", SqlDbType.NVarChar,50)};
                parameters5[0].Value = Stock;
                parameters5[1].Value = SkuInfo.SKU;
                cmd = new CommandInfo(strSql5.ToString(), parameters5);
                sqllist.Add(cmd);
            }
            
            int rowsAffected = DBHelper.DefaultDBHelper.ExecuteSqlTran(sqllist);
            if (rowsAffected > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
            #endregion 
        }
        /// <summary>
        /// 根据商品id获取该商品的sku数
        /// </summary>
        /// <param name="productId"></param>
        /// <returns></returns>
        public int skuCount(long productId)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append(" SELECT COUNT(*)  FROM  PMS_SKUs ");
            strSql.Append(" where ProductId =@ProductId ");
            SqlParameter[] parameters = {
                    new SqlParameter("@ProductId", SqlDbType.BigInt)
            };
            parameters[0].Value = productId;
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
        /// 获取商品sku列表 （分页） 排除商家商品
        /// </summary>
        /// <param name="keyw">商品名称或编码</param>
        /// <param name="startIndex"></param>
        /// <param name="endIndex"></param>
        /// <param name="orderby">排序方式</param>
        /// <returns></returns>
        public DataSet GetSKUListByPage(string keyw, int startIndex, int endIndex, string orderby)
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
            strSql.Append(")AS Row, T.*  from  ");
            strSql.Append(" ( ");
            strSql.Append("  select p.TypeId,p.ProductId,p.ProductName,p.SaleStatus ,sku.SKU, sku.Stock,sku.SkuId,sku.SalePrice,sku.Upselling,sku.CostPrice,sku.Weight,sku.AlertStock  ");
            strSql.Append("  from  PMS_SKUs sku  ");
            strSql.Append("  inner Join PMS_Products p on p.ProductId = sku.ProductId  and p.SaleStatus = 1 and p.SalesType = 1  and sku.Upselling = 1  and p.supplierId<=0  ");//在售状态的正常商品
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
        ///  获取记录总数 排除商家商品
        /// </summary>
        /// <param name="keyw">商品名称或编码</param>
        /// <returns></returns>
        public int GetSKURecordCount( string keyw)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("  select count(1)");
            strSql.Append("   from  PMS_SKUs sku   ");
            strSql.Append("  inner Join PMS_Products p on p.ProductId = sku.ProductId  and p.SaleStatus = 1 and p.SalesType = 1   and sku.Upselling = 1  and p.supplierId<=0  ");//在售状态的正常商品
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

        #endregion
    }
}

