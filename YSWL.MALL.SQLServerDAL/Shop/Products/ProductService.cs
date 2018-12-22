/**
* ProductService.cs
*
* 功 能： Shop模块-产品相关 多表事务操作类
* 类 名： ProductService
*
* Ver    变更日期             负责人  变更内容
* ───────────────────────────────────
* V0.01  2012/6/22 10:46:33  Ben    初版
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
using System.Data.SqlClient;
using System.Globalization;
using System.Linq;
using System.Text;
using YSWL.Common;
using YSWL.DBUtility;
using YSWL.MALL.IDAL.Shop.Products;
using YSWL.MALL.Model.Shop.Products;

namespace YSWL.MALL.SQLServerDAL.Shop.Products
{
    /// <summary>
    /// Shop模块-产品相关 多表事务操作类
    /// </summary>
    public class ProductService : IProductService
    {
        #region IProductService 成员

        #region 新增产品
        public bool AddProduct(Model.Shop.Products.ProductInfo productInfo, out long ProductId)
        {
            ProductId = 0;
            using (SqlConnection connection = DBHelper.DefaultDBHelper.GetDBObject().GetConnection)
            {
                connection.Open();
                using (SqlTransaction transaction = connection.BeginTransaction())
                {
                    object result;
                    try
                    {
                        //添加商品
                        result = DBHelper.DefaultDBHelper.GetSingle4Trans(GenerateProductInfo(productInfo), transaction);
                        //获取新增的商品主键
                        productInfo.ProductId = Globals.SafeLong(result.ToString(), -1);

                        ProductId = productInfo.ProductId;

                        //添加产品分类
                        DBHelper.DefaultDBHelper.ExecuteSqlTran4Indentity(SaveProductCategories(productInfo), transaction);

                        //添加属性
                        DBHelper.DefaultDBHelper.ExecuteSqlTran4Indentity(GenerateAttributeInfo(productInfo, transaction), transaction);

                        //添加SKU
                        DBHelper.DefaultDBHelper.ExecuteSqlTran4Indentity(GenerateSKUs(productInfo, transaction), transaction);

                        //添加相关商品
                        DBHelper.DefaultDBHelper.ExecuteSqlTran4Indentity(GenerateRelatedProduct(productInfo), transaction);

                        //添加图片
                        DBHelper.DefaultDBHelper.ExecuteSqlTran4Indentity(GenerateImages(productInfo), transaction);

                        //推荐商品
                        if (productInfo.isRec)
                        {
                            DBHelper.DefaultDBHelper.GetSingle4Trans(GenerateProductStationModes(productInfo, 0), transaction);
                        }
                        //最新商品
                        if (productInfo.isNow)
                        {
                            DBHelper.DefaultDBHelper.GetSingle4Trans(GenerateProductStationModes(productInfo, 3), transaction);
                        }
                        //最热商品
                        if (productInfo.isHot)
                        {
                            DBHelper.DefaultDBHelper.GetSingle4Trans(GenerateProductStationModes(productInfo, 1), transaction);
                        }
                        //特价商品
                        if (productInfo.isLowPrice)
                        {
                            DBHelper.DefaultDBHelper.GetSingle4Trans(GenerateProductStationModes(productInfo, 2), transaction);
                        }

                        //添加包装
                        DBHelper.DefaultDBHelper.ExecuteSqlTran4Indentity(GeneratePackage(productInfo), transaction);

                        //商家上架的商品 状态发生改变时 更新商家中上架商品总数
                        if (productInfo.SupplierId > 0 &&   productInfo.SaleStatus==1)
                        {
                            //减少 在售商品数
                            DBHelper.DefaultDBHelper.GetSingle4Trans(AddSuppProductCount(productInfo.SupplierId), transaction);
                        }

                        transaction.Commit();
                    }
                    catch (SqlException)
                    {
                        transaction.Rollback();
                        return false;
                    }
                }
            }
            return true;
        }


        public bool AddPMSProduct(Model.Shop.Products.ProductInfo productInfo)
        {

            using (SqlConnection connection = DBHelper.DefaultDBHelper.GetDBObject().GetConnection)
            {
                connection.Open();
                using (SqlTransaction transaction = connection.BeginTransaction())
                {
                    object result;
                    try
                    {
                        //TODO：删除原有信息
                        DeletePMSProductInfo(productInfo);
                        //添加商品
                        result = DBHelper.DefaultDBHelper.GetSingle4Trans(GeneratePMSProductInfo(productInfo), transaction);
                        //获取新增的商品主键
                       

                     //  long  ProductId = productInfo.ProductId;

                        //添加产品分类
                        DBHelper.DefaultDBHelper.ExecuteSqlTran4Indentity(SaveProductPMSCategories(productInfo), transaction);

                        //添加属性
                        DBHelper.DefaultDBHelper.ExecuteSqlTran4Indentity(GeneratePMSAttributeInfo(productInfo, transaction), transaction);

                        //添加SKU
                        DBHelper.DefaultDBHelper.ExecuteSqlTran4Indentity(GeneratePMSSKUs(productInfo, transaction), transaction);

                        //添加图片
                        DBHelper.DefaultDBHelper.ExecuteSqlTran4Indentity(GeneratePMSImages(productInfo), transaction);
                    
                        transaction.Commit();
                    }
                    catch (SqlException ex)
                    {
                        Log.LogHelper.AddTextLog("同步新增商品信息失败", ex.Message + "-----" + ex.StackTrace);
                        transaction.Rollback();
                        return false;
                    }
                }
            }
            return true;
        }

        public bool AddProductOne(Model.Shop.Products.ProductInfo productInfo)
        {
            using (SqlConnection connection = DBHelper.DefaultDBHelper.GetDBObject().GetConnection)
            {
                connection.Open();
                using (SqlTransaction transaction = connection.BeginTransaction())
                {
                    try
                    {
                        //TODO：删除原有信息
                        DeleteOldProductInfo(productInfo);
                        //添加商品
                        DBHelper.DefaultDBHelper.GetSingle4Trans(GeneratePMSProductInfo(productInfo), transaction);
                        //获取新增的商品主键

                        //添加产品分类
                        DBHelper.DefaultDBHelper.ExecuteSqlTran4Indentity(SaveProductPMSCategories(productInfo), transaction);

                        //添加属性
                        DBHelper.DefaultDBHelper.ExecuteSqlTran4Indentity(GeneratePMSAttributeInfo(productInfo, transaction), transaction);

                        //添加SKU
                        DBHelper.DefaultDBHelper.ExecuteSqlTran4Indentity(GeneratePMSSKUs(productInfo, transaction), transaction);

                        //添加图片
                        DBHelper.DefaultDBHelper.ExecuteSqlTran4Indentity(GeneratePMSImages(productInfo), transaction);
                        if (productInfo.BrandInfo != null)
                        {
                            //添加商品品牌
                            DBHelper.DefaultDBHelper.GetSingle4Trans(AddBrand(productInfo), transaction);
                        }
                        if (productInfo.CategoryInfo.Any())
                        {
                            //添加商品分类信息
                            DBHelper.DefaultDBHelper.ExecuteSqlTran4Indentity(AddCategoryInfo(productInfo), transaction);
                        }

                        transaction.Commit();
                    }
                    catch (SqlException ex)
                    {
                        Log.LogHelper.AddTextLog("同步单个商品信息失败", ex.Message + "-----" + ex.StackTrace);
                        transaction.Rollback();
                        return false;
                    }
                }
            }
            return true;
        }

        #endregion

        #region 修改产品
        public bool ModifyProduct(Model.Shop.Products.ProductInfo productInfo,int oldSaleStatus)
        {
            using (SqlConnection connection = DBHelper.DefaultDBHelper.GetDBObject().GetConnection)
            {
                connection.Open();
                using (SqlTransaction transaction = connection.BeginTransaction())
                {
                    try
                    {
                        //TODO：删除原有信息
                        //DeleteOldProductInfo(productInfo);
                        DeletePMSProductInfo(productInfo);

                        //TODO：更新商品基本信息
                        DBHelper.DefaultDBHelper.GetSingle4Trans(UpdateProductInfo(productInfo), transaction);

                        //TODO  更新团购表商品名称
                        DBHelper.DefaultDBHelper.GetSingle4Trans(UpdateProductName(productInfo), transaction);

                        //添加产品分类
                        DBHelper.DefaultDBHelper.ExecuteSqlTran4Indentity(SaveProductCategories(productInfo), transaction);

                        //添加属性
                        DBHelper.DefaultDBHelper.ExecuteSqlTran4Indentity(GenerateAttributeInfo(productInfo, transaction), transaction);

                        //添加SKU
                        DBHelper.DefaultDBHelper.ExecuteSqlTran4Indentity(GenerateSKUs(productInfo, transaction), transaction);

                        //添加相关商品
                        DBHelper.DefaultDBHelper.ExecuteSqlTran4Indentity(GenerateRelatedProduct(productInfo), transaction);

                        //添加图片
                        DBHelper.DefaultDBHelper.ExecuteSqlTran4Indentity(GenerateImages(productInfo), transaction);

                        //商家上架的商品 状态发生改变时 更新商家中上架商品总数
                        if (productInfo.SupplierId > 0 && oldSaleStatus == 1 && productInfo.SaleStatus != oldSaleStatus)
                        {
                            //减少 在售商品数
                            DBHelper.DefaultDBHelper.GetSingle4Trans(CutSuppProductCount(productInfo.SupplierId), transaction);
                        }
                        transaction.Commit();
                    }
                    catch (SqlException)
                    {
                        transaction.Rollback();
                        return false;
                    }
                }
            }
            return true;
        }
        /// <summary>
        /// 同步PMS数据使用
        /// </summary>
        /// <param name="productInfo"></param>
        /// <returns></returns>
        public bool ModifyPMSProduct(Model.Shop.Products.ProductInfo productInfo)
        {
            using (SqlConnection connection = DBHelper.DefaultDBHelper.GetDBObject().GetConnection)
            {
                connection.Open();
                using (SqlTransaction transaction = connection.BeginTransaction())
                {
                    try
                    {
                        //TODO：删除原有信息
                        DeletePMSProductInfo(productInfo);

                        //TODO：更新商品基本信息
                        DBHelper.DefaultDBHelper.GetSingle4Trans(UpdatePMSProductInfo(productInfo), transaction);

                        //TODO  更新团购表商品名称
                        DBHelper.DefaultDBHelper.GetSingle4Trans(UpdateProductName(productInfo), transaction);

                        //添加产品分类
                        DBHelper.DefaultDBHelper.ExecuteSqlTran4Indentity(SaveProductCategories(productInfo), transaction);

                        //添加属性
                        DBHelper.DefaultDBHelper.ExecuteSqlTran4Indentity(GeneratePMSAttributeInfo(productInfo, transaction), transaction);

                        //添加SKU
                        DBHelper.DefaultDBHelper.ExecuteSqlTran4Indentity(GeneratePMSSKUs(productInfo, transaction), transaction);

                        //添加图片
                        DBHelper.DefaultDBHelper.ExecuteSqlTran4Indentity(GeneratePMSImages(productInfo), transaction);

                        transaction.Commit();
                    }
                    catch (SqlException ex)
                    {
                        Log.LogHelper.AddTextLog("同步修改商品信息失败", ex.Message + "-----" + ex.StackTrace);
                        transaction.Rollback();
                        return false;
                    }
                }
            }
            return true;
        }
        #endregion

        #endregion IProductService 成员

        #region 新增供应商产品
        public bool AddSuppProduct(Model.Shop.Products.ProductInfo productInfo, out long ProductId)
        {
            ProductId = 0;
            using (SqlConnection connection = DBHelper.DefaultDBHelper.GetDBObject().GetConnection)
            {
                connection.Open();
                using (SqlTransaction transaction = connection.BeginTransaction())
                {
                    object result;
                    try
                    {
                        //添加商品
                        result = DBHelper.DefaultDBHelper.GetSingle4Trans(GenerateProductInfo(productInfo), transaction);
                        //获取新增的商品主键
                        productInfo.ProductId = Globals.SafeLong(result.ToString(), -1);

                        ProductId = productInfo.ProductId;

                        //添加产品分类
                        DBHelper.DefaultDBHelper.ExecuteSqlTran4Indentity(SaveProductCategories(productInfo), transaction);

                        //添加店铺产品分类
                        DBHelper.DefaultDBHelper.ExecuteSqlTran4Indentity(SaveSuppProductCategories(productInfo), transaction);

                        //添加属性
                        DBHelper.DefaultDBHelper.ExecuteSqlTran4Indentity(GenerateAttributeInfo(productInfo, transaction), transaction);

                        //添加SKU
                        DBHelper.DefaultDBHelper.ExecuteSqlTran4Indentity(GenerateSKUs(productInfo, transaction), transaction);

                        //添加相关商品
                        DBHelper.DefaultDBHelper.ExecuteSqlTran4Indentity(GenerateRelatedProduct(productInfo), transaction);

                        //添加图片
                        DBHelper.DefaultDBHelper.ExecuteSqlTran4Indentity(GenerateImages(productInfo), transaction);

                        //推荐商品
                        if (productInfo.isRec)
                        {
                            DBHelper.DefaultDBHelper.GetSingle4Trans(GenerateProductStationModes(productInfo, 0), transaction);
                        }
                        //最新商品
                        if (productInfo.isNow)
                        {
                            DBHelper.DefaultDBHelper.GetSingle4Trans(GenerateProductStationModes(productInfo, 3), transaction);
                        }
                        //最热商品
                        if (productInfo.isHot)
                        {
                            DBHelper.DefaultDBHelper.GetSingle4Trans(GenerateProductStationModes(productInfo, 1), transaction);
                        }
                        //特价商品
                        if (productInfo.isLowPrice)
                        {
                            DBHelper.DefaultDBHelper.GetSingle4Trans(GenerateProductStationModes(productInfo, 2), transaction);
                        }

                        //添加包装
                        DBHelper.DefaultDBHelper.ExecuteSqlTran4Indentity(GeneratePackage(productInfo), transaction);
                        transaction.Commit();
                    }
                    catch (SqlException)
                    {
                        transaction.Rollback();
                        return false;
                    }
                }
            }
            return true;
        }
        #endregion

        #region 修改供应商产品
        public bool ModifySuppProduct(Model.Shop.Products.ProductInfo productInfo, int oldSaleStatus)
        {
            using (SqlConnection connection = DBHelper.DefaultDBHelper.GetDBObject().GetConnection)
            {
                connection.Open();
                using (SqlTransaction transaction = connection.BeginTransaction())
                {
                    try
                    {
                        //TODO：删除原有信息
                        // DeleteOldProductInfo(productInfo);
                        DeletePMSProductInfo(productInfo);

                         DeleteOldSuppProductCate(productInfo);//删除原有分类

                        //TODO：更新商品基本信息
                        DBHelper.DefaultDBHelper.GetSingle4Trans(UpdateProductInfo(productInfo), transaction);

                        //添加产品分类
                        DBHelper.DefaultDBHelper.ExecuteSqlTran4Indentity(SaveProductCategories(productInfo), transaction);
                        //添加店铺产品分类

                        DBHelper.DefaultDBHelper.ExecuteSqlTran4Indentity(SaveSuppProductCategories(productInfo), transaction);

                        //添加属性
                        DBHelper.DefaultDBHelper.ExecuteSqlTran4Indentity(GenerateAttributeInfo(productInfo, transaction), transaction);

                        //添加SKU
                        DBHelper.DefaultDBHelper.ExecuteSqlTran4Indentity(GenerateSKUs(productInfo, transaction), transaction);

                        //添加相关商品
                        DBHelper.DefaultDBHelper.ExecuteSqlTran4Indentity(GenerateRelatedProduct(productInfo), transaction);

                        //添加图片
                        DBHelper.DefaultDBHelper.ExecuteSqlTran4Indentity(GenerateImages(productInfo), transaction);

                        //商家上架的商品 状态发生改变时 更新商家中上架商品总数
                        if (productInfo.SupplierId > 0 && oldSaleStatus==1 && productInfo.SaleStatus!= oldSaleStatus)
                        {
                            //减少 在售商品数
                            DBHelper.DefaultDBHelper.GetSingle4Trans(CutSuppProductCount(productInfo.SupplierId), transaction);
                        }
                        transaction.Commit();
                    }
                    catch (SqlException)
                    {
                        transaction.Rollback();
                        return false;
                    }
                }
            }
            return true;
        }
        private CommandInfo CutSuppProductCount(int supplierId)
        {
            StringBuilder strSql = new StringBuilder();
            strSql = new StringBuilder();
            strSql.AppendFormat(" update Shop_Suppliers set ProductCount=ProductCount-1 where SupplierId = {0}  ", supplierId);
            return new CommandInfo(strSql.ToString(), null, EffentNextType.ExcuteEffectRows);
        }
        private CommandInfo AddSuppProductCount(int supplierId)
        {
            StringBuilder strSql = new StringBuilder();
            strSql = new StringBuilder();
            strSql.AppendFormat(" update Shop_Suppliers set ProductCount=ProductCount+1 where SupplierId = {0}  ", supplierId);
            return new CommandInfo(strSql.ToString(), null, EffentNextType.ExcuteEffectRows);
        }
        #endregion


        #region 产品重新编辑保存前，删除产品相关联信息
        private void DeleteOldProductInfo(Model.Shop.Products.ProductInfo productInfo)
        {
            SqlParameter[] parameter = {
                                       new SqlParameter("@ProductId",SqlDbType.BigInt,8)
                                       };
            parameter[0].Value = productInfo.ProductId;

            DBHelper.DefaultDBHelper.RunProcedure("sp_Shop_DeleteBeforeUpdate", parameter);
        }

        private void DeletePMSProductInfo(Model.Shop.Products.ProductInfo productInfo)
        {
            SqlParameter[] parameter = {
                                       new SqlParameter("@ProductId",SqlDbType.BigInt,8)
                                       };
            parameter[0].Value = productInfo.ProductId;
            List<CommandInfo> list = new List<CommandInfo>();
            list.Add(new CommandInfo("DELETE FROM PMS_ProductAttributes WHERE ProductId=@ProductId", parameter));
            list.Add(new CommandInfo("DELETE FROM PMS_SKURelation WHERE ProductId=@ProductId ", parameter));
            list.Add(new CommandInfo("DELETE FROM PMS_SKUItems WHERE ProductId=@ProductId", parameter));
            list.Add(new CommandInfo("  DELETE FROM PMS_SKUs WHERE ProductId=@ProductId ", parameter));
            list.Add(new CommandInfo("DELETE FROM PMS_ProductCategories WHERE ProductId=@ProductId", parameter));

            list.Add(new CommandInfo("DELETE FROM Shop_RelatedProducts WHERE ProductId=@ProductId", parameter));
            list.Add(new CommandInfo("DELETE FROM Shop_RelatedProducts WHERE RelatedId=@ProductId", parameter));
            list.Add(new CommandInfo("DELETE FROM Shop_AccessoriesValues WHERE  AccessoriesId in  (SELECT AccessoriesId  FROM Shop_ProductAccessories WHERE  ProductId = @ProductId )", parameter));
            list.Add(new CommandInfo(" DELETE FROM Shop_ProductAccessories WHERE  ProductId = @ProductId", parameter));
            list.Add(new CommandInfo("DELETE FROM PMS_ProductImages WHERE ProductId = @ProductId", parameter));


            DBHelper.DefaultDBHelper.ExecuteSqlTran4Indentity(list);

        }
        private void DeleteOldSuppProductCate(Model.Shop.Products.ProductInfo productInfo)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("delete from Shop_SuppProductCategories ");
            strSql.Append(" where ProductId=@ProductId ");
            SqlParameter[] parameters = {
                    new SqlParameter("@ProductId", SqlDbType.BigInt,8)          };
            parameters[0].Value = productInfo.ProductId;
            DBHelper.DefaultDBHelper.ExecuteSql(strSql.ToString(), parameters);
        }
        #endregion

        #region 产品信息

        private CommandInfo UpdateProductInfo(Model.Shop.Products.ProductInfo productInfo)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("UPDATE PMS_Products SET ");
            strSql.Append("CategoryId=@CategoryId,");
            strSql.Append("TypeId=@TypeId,");
            strSql.Append("BrandId=@BrandId,");
            strSql.Append("ProductName=@ProductName,");
            strSql.Append("ProductCode=@ProductCode,");
            strSql.Append("SupplierId=@SupplierId,");
            strSql.Append("RegionId=@RegionId,");
            strSql.Append("ShortDescription=@ShortDescription,");
            strSql.Append("Unit=@Unit,");
            strSql.Append("Description=@Description,");
            strSql.Append("Meta_Title=@Title,");
            strSql.Append("Meta_Description=@Meta_Description,");
            strSql.Append("Meta_Keywords=@Meta_Keywords,");
            strSql.Append("SaleStatus=@SaleStatus,");
            strSql.Append("VistiCounts=@VistiCounts,");
            strSql.Append("SaleCounts=@SaleCounts,");
            strSql.Append("DisplaySequence=@DisplaySequence,");
            strSql.Append("LineId=@LineId,");
            strSql.Append("MarketPrice=@MarketPrice,");
            strSql.Append("LowestSalePrice=@LowestSalePrice,");
            strSql.Append("PenetrationStatus=@PenetrationStatus,");
            strSql.Append("MainCategoryPath=@MainCategoryPath,");
            strSql.Append("ExtendCategoryPath=@ExtendCategoryPath,");
            strSql.Append("HasSKU=@HasSKU,");
            strSql.Append("Points=@Points,");
            strSql.Append("ImageUrl=@ImageUrl,");
            strSql.Append("ThumbnailUrl1=@ThumbnailUrl1,");
            strSql.Append("ThumbnailUrl2=@ThumbnailUrl2,");
            strSql.Append("ThumbnailUrl3=@ThumbnailUrl3,");
            strSql.Append("ThumbnailUrl4=@ThumbnailUrl4,");
            strSql.Append("ThumbnailUrl5=@ThumbnailUrl5,");
            strSql.Append("ThumbnailUrl6=@ThumbnailUrl6,");
            strSql.Append("ThumbnailUrl7=@ThumbnailUrl7,");
            strSql.Append("ThumbnailUrl8=@ThumbnailUrl8,");
            strSql.Append("MaxQuantity=@MaxQuantity,");
            strSql.Append("MinQuantity=@MinQuantity,");
            strSql.Append("Tags=@Tags,");
            strSql.Append("SeoUrl=@SeoUrl,");
            strSql.Append("SeoImageAlt=@SeoImageAlt,");
            strSql.Append("SeoImageTitle=@SeoImageTitle,");
            strSql.Append("SalesType=@SalesType,");
            strSql.Append("RestrictionCount=@RestrictionCount,");
            strSql.Append("DeliveryTip=@DeliveryTip,");
            strSql.Append("Remark=@Remark,");
            strSql.Append("Gwjf=@Gwjf");
            strSql.Append(" WHERE ProductId=@ProductId");
            SqlParameter[] parameters = {
                    new SqlParameter("@CategoryId", SqlDbType.Int,4),
                    new SqlParameter("@TypeId", SqlDbType.Int,4),
                    new SqlParameter("@BrandId", SqlDbType.Int,4),
                    new SqlParameter("@ProductName", SqlDbType.NVarChar,200),
                    new SqlParameter("@ProductCode", SqlDbType.NVarChar,50),
                    new SqlParameter("@SupplierId", SqlDbType.Int,4),
                    new SqlParameter("@RegionId", SqlDbType.Int,4),
                    new SqlParameter("@ShortDescription", SqlDbType.NVarChar,2000),
                    new SqlParameter("@Unit", SqlDbType.NVarChar,50),
                    new SqlParameter("@Description", SqlDbType.NText),
                    new SqlParameter("@Title", SqlDbType.NVarChar,100),
                    new SqlParameter("@Meta_Description", SqlDbType.NVarChar,1000),
                    new SqlParameter("@Meta_Keywords", SqlDbType.NVarChar,1000),
                    new SqlParameter("@SaleStatus", SqlDbType.Int,4),
                    new SqlParameter("@VistiCounts", SqlDbType.Int,4),
                    new SqlParameter("@SaleCounts", SqlDbType.Int,4),
                    new SqlParameter("@DisplaySequence", SqlDbType.Int,4),
                    new SqlParameter("@LineId", SqlDbType.Int,4),
                    new SqlParameter("@MarketPrice", SqlDbType.Money,8),
                    new SqlParameter("@LowestSalePrice", SqlDbType.Money,8),
                    new SqlParameter("@PenetrationStatus", SqlDbType.SmallInt,2),
                    new SqlParameter("@MainCategoryPath", SqlDbType.NVarChar,256),
                    new SqlParameter("@ExtendCategoryPath", SqlDbType.NVarChar,256),
                    new SqlParameter("@HasSKU", SqlDbType.Bit,1),
                    new SqlParameter("@Points", SqlDbType.Decimal,9),
                    new SqlParameter("@ImageUrl", SqlDbType.NVarChar,255),
                    new SqlParameter("@ThumbnailUrl1", SqlDbType.NVarChar,255),
                    new SqlParameter("@ThumbnailUrl2", SqlDbType.NVarChar,255),
                    new SqlParameter("@ThumbnailUrl3", SqlDbType.NVarChar,255),
                    new SqlParameter("@ThumbnailUrl4", SqlDbType.NVarChar,255),
                    new SqlParameter("@ThumbnailUrl5", SqlDbType.NVarChar,255),
                    new SqlParameter("@ThumbnailUrl6", SqlDbType.NVarChar,255),
                    new SqlParameter("@ThumbnailUrl7", SqlDbType.NVarChar,255),
                    new SqlParameter("@ThumbnailUrl8", SqlDbType.NVarChar,255),
                    new SqlParameter("@MaxQuantity", SqlDbType.Int,4),
                    new SqlParameter("@MinQuantity", SqlDbType.Int,4),
                    new SqlParameter("@Tags", SqlDbType.NVarChar,50),
                    new SqlParameter("@SeoUrl", SqlDbType.NVarChar,300),
                    new SqlParameter("@SeoImageAlt", SqlDbType.NVarChar,300),
                    new SqlParameter("@SeoImageTitle", SqlDbType.NVarChar,300),
                    new SqlParameter("@SalesType", SqlDbType.SmallInt,2),
                    new SqlParameter("@RestrictionCount", SqlDbType.Int,4),
                    new SqlParameter("@DeliveryTip", SqlDbType.NVarChar,100),
                    new SqlParameter("@Remark", SqlDbType.NVarChar,500),
                    new SqlParameter("@Gwjf", SqlDbType.Decimal,9),
                    new SqlParameter("@ProductId", SqlDbType.BigInt,8)};
            parameters[0].Value = productInfo.CategoryId;
            parameters[1].Value = productInfo.TypeId;
            parameters[2].Value = productInfo.BrandId;
            parameters[3].Value = productInfo.ProductName;
            parameters[4].Value = productInfo.ProductCode;
            parameters[5].Value = productInfo.SupplierId;
            parameters[6].Value = productInfo.RegionId;
            parameters[7].Value = productInfo.ShortDescription;
            parameters[8].Value = productInfo.Unit;
            parameters[9].Value = productInfo.Description;
            parameters[10].Value = productInfo.Meta_Title;
            parameters[11].Value = productInfo.Meta_Description;
            parameters[12].Value = productInfo.Meta_Keywords;
            parameters[13].Value = productInfo.SaleStatus;
            parameters[14].Value = productInfo.VistiCounts;
            parameters[15].Value = productInfo.SaleCounts;
            parameters[16].Value = productInfo.DisplaySequence;
            parameters[17].Value = productInfo.LineId;
            parameters[18].Value = productInfo.MarketPrice;
            parameters[19].Value = productInfo.LowestSalePrice;
            parameters[20].Value = productInfo.PenetrationStatus;
            parameters[21].Value = productInfo.MainCategoryPath;
            parameters[22].Value = productInfo.ExtendCategoryPath;
            parameters[23].Value = productInfo.HasSKU;
            parameters[24].Value = productInfo.Points;
            parameters[25].Value = productInfo.ImageUrl;
            parameters[26].Value = productInfo.ThumbnailUrl1;
            parameters[27].Value = productInfo.ThumbnailUrl2;
            parameters[28].Value = productInfo.ThumbnailUrl3;
            parameters[29].Value = productInfo.ThumbnailUrl4;
            parameters[30].Value = productInfo.ThumbnailUrl5;
            parameters[31].Value = productInfo.ThumbnailUrl6;
            parameters[32].Value = productInfo.ThumbnailUrl7;
            parameters[33].Value = productInfo.ThumbnailUrl8;
            parameters[34].Value = productInfo.MaxQuantity;
            parameters[35].Value = productInfo.MinQuantity;
            parameters[36].Value = productInfo.Tags;
            parameters[37].Value = productInfo.SeoUrl;
            parameters[38].Value = productInfo.SeoImageAlt;
            parameters[39].Value = productInfo.SeoImageTitle;
            parameters[40].Value = productInfo.SalesType;
            parameters[41].Value = productInfo.RestrictionCount;
            parameters[42].Value = productInfo.DeliveryTip;
            parameters[43].Value = productInfo.Remark;
            parameters[44].Value = productInfo.Gwjf;
            parameters[45].Value = productInfo.ProductId;

            return new CommandInfo(strSql.ToString(),
                                    parameters, EffentNextType.ExcuteEffectRows);
        }

        private CommandInfo UpdatePMSProductInfo(Model.Shop.Products.ProductInfo productInfo)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("UPDATE PMS_Products SET ");
            strSql.Append("CategoryId=@CategoryId,");
            strSql.Append("TypeId=@TypeId,");
            strSql.Append("BrandId=@BrandId,");
            strSql.Append("ProductName=@ProductName,");
            strSql.Append("ProductCode=@ProductCode,");
            strSql.Append("SupplierId=@SupplierId,");
            strSql.Append("RegionId=@RegionId,");
            strSql.Append("ShortDescription=@ShortDescription,");
            strSql.Append("Unit=@Unit,");
            strSql.Append("Description=@Description,");
            strSql.Append("Meta_Title=@Title,");
            strSql.Append("Meta_Description=@Meta_Description,");
            strSql.Append("Meta_Keywords=@Meta_Keywords,");
            strSql.Append("DisplaySequence=@DisplaySequence,");
            strSql.Append("LineId=@LineId,");
            strSql.Append("MarketPrice=@MarketPrice,");
            strSql.Append("LowestSalePrice=@LowestSalePrice,");
            strSql.Append("PenetrationStatus=@PenetrationStatus,");
            strSql.Append("MainCategoryPath=@MainCategoryPath,");
            strSql.Append("ExtendCategoryPath=@ExtendCategoryPath,");
            strSql.Append("HasSKU=@HasSKU,");
            strSql.Append("Points=@Points,");
            strSql.Append("ImageUrl=@ImageUrl,");
            strSql.Append("ThumbnailUrl1=@ThumbnailUrl1,");
            strSql.Append("ThumbnailUrl2=@ThumbnailUrl2,");
            strSql.Append("ThumbnailUrl3=@ThumbnailUrl3,");
            strSql.Append("ThumbnailUrl4=@ThumbnailUrl4,");
            strSql.Append("ThumbnailUrl5=@ThumbnailUrl5,");
            strSql.Append("ThumbnailUrl6=@ThumbnailUrl6,");
            strSql.Append("ThumbnailUrl7=@ThumbnailUrl7,");
            strSql.Append("ThumbnailUrl8=@ThumbnailUrl8,");
            strSql.Append("MaxQuantity=@MaxQuantity,");
            strSql.Append("MinQuantity=@MinQuantity,");
            strSql.Append("Tags=@Tags,");
            strSql.Append("SeoUrl=@SeoUrl,");
            strSql.Append("SeoImageAlt=@SeoImageAlt,");
            strSql.Append("SeoImageTitle=@SeoImageTitle,");
            strSql.Append("SalesType=@SalesType,");
            strSql.Append("RestrictionCount=@RestrictionCount,");
            strSql.Append("DeliveryTip=@DeliveryTip,");
            strSql.Append("Remark=@Remark,");
            strSql.Append("Gwjf=@Gwjf");
            strSql.Append(" WHERE ProductId=@ProductId");
            SqlParameter[] parameters = {
                    new SqlParameter("@CategoryId", SqlDbType.Int,4),
                    new SqlParameter("@TypeId", SqlDbType.Int,4),
                    new SqlParameter("@BrandId", SqlDbType.Int,4),
                    new SqlParameter("@ProductName", SqlDbType.NVarChar,200),
                    new SqlParameter("@ProductCode", SqlDbType.NVarChar,50),
                    new SqlParameter("@SupplierId", SqlDbType.Int,4),
                    new SqlParameter("@RegionId", SqlDbType.Int,4),
                    new SqlParameter("@ShortDescription", SqlDbType.NVarChar,2000),
                    new SqlParameter("@Unit", SqlDbType.NVarChar,50),
                    new SqlParameter("@Description", SqlDbType.NText),
                    new SqlParameter("@Title", SqlDbType.NVarChar,100),
                    new SqlParameter("@Meta_Description", SqlDbType.NVarChar,1000),
                    new SqlParameter("@Meta_Keywords", SqlDbType.NVarChar,1000),
                    new SqlParameter("@DisplaySequence", SqlDbType.Int,4),
                    new SqlParameter("@LineId", SqlDbType.Int,4),
                    new SqlParameter("@MarketPrice", SqlDbType.Money,8),
                    new SqlParameter("@LowestSalePrice", SqlDbType.Money,8),
                    new SqlParameter("@PenetrationStatus", SqlDbType.SmallInt,2),
                    new SqlParameter("@MainCategoryPath", SqlDbType.NVarChar,256),
                    new SqlParameter("@ExtendCategoryPath", SqlDbType.NVarChar,256),
                    new SqlParameter("@HasSKU", SqlDbType.Bit,1),
                    new SqlParameter("@Points", SqlDbType.Decimal,9),
                    new SqlParameter("@ImageUrl", SqlDbType.NVarChar,255),
                    new SqlParameter("@ThumbnailUrl1", SqlDbType.NVarChar,255),
                    new SqlParameter("@ThumbnailUrl2", SqlDbType.NVarChar,255),
                    new SqlParameter("@ThumbnailUrl3", SqlDbType.NVarChar,255),
                    new SqlParameter("@ThumbnailUrl4", SqlDbType.NVarChar,255),
                    new SqlParameter("@ThumbnailUrl5", SqlDbType.NVarChar,255),
                    new SqlParameter("@ThumbnailUrl6", SqlDbType.NVarChar,255),
                    new SqlParameter("@ThumbnailUrl7", SqlDbType.NVarChar,255),
                    new SqlParameter("@ThumbnailUrl8", SqlDbType.NVarChar,255),
                    new SqlParameter("@MaxQuantity", SqlDbType.Int,4),
                    new SqlParameter("@MinQuantity", SqlDbType.Int,4),
                    new SqlParameter("@Tags", SqlDbType.NVarChar,50),
                    new SqlParameter("@SeoUrl", SqlDbType.NVarChar,300),
                    new SqlParameter("@SeoImageAlt", SqlDbType.NVarChar,300),
                    new SqlParameter("@SeoImageTitle", SqlDbType.NVarChar,300),
                    new SqlParameter("@SalesType", SqlDbType.SmallInt,2),
                    new SqlParameter("@RestrictionCount", SqlDbType.Int,4),
                    new SqlParameter("@DeliveryTip", SqlDbType.NVarChar,100),
                    new SqlParameter("@Remark", SqlDbType.NVarChar,500),
                    new SqlParameter("@Gwjf", SqlDbType.Decimal,9),
                    new SqlParameter("@ProductId", SqlDbType.BigInt,8)};
            parameters[0].Value = productInfo.CategoryId;
            parameters[1].Value = productInfo.TypeId;
            parameters[2].Value = productInfo.BrandId;
            parameters[3].Value = productInfo.ProductName;
            parameters[4].Value = productInfo.ProductCode;
            parameters[5].Value = productInfo.SupplierId;
            parameters[6].Value = productInfo.RegionId;
            parameters[7].Value = productInfo.ShortDescription;
            parameters[8].Value = productInfo.Unit;
            parameters[9].Value = productInfo.Description;
            parameters[10].Value = productInfo.Meta_Title;
            parameters[11].Value = productInfo.Meta_Description;
            parameters[12].Value = productInfo.Meta_Keywords;
            parameters[13].Value = productInfo.DisplaySequence;
            parameters[14].Value = productInfo.LineId;
            parameters[15].Value = productInfo.MarketPrice;
            parameters[16].Value = productInfo.LowestSalePrice;
            parameters[17].Value = productInfo.PenetrationStatus;
            parameters[18].Value = productInfo.MainCategoryPath;
            parameters[19].Value = productInfo.ExtendCategoryPath;
            parameters[20].Value = productInfo.HasSKU;
            parameters[21].Value = productInfo.Points;
            parameters[22].Value = productInfo.ImageUrl;
            parameters[23].Value = productInfo.ThumbnailUrl1;
            parameters[24].Value = productInfo.ThumbnailUrl2;
            parameters[25].Value = productInfo.ThumbnailUrl3;
            parameters[26].Value = productInfo.ThumbnailUrl4;
            parameters[27].Value = productInfo.ThumbnailUrl5;
            parameters[28].Value = productInfo.ThumbnailUrl6;
            parameters[29].Value = productInfo.ThumbnailUrl7;
            parameters[30].Value = productInfo.ThumbnailUrl8;
            parameters[31].Value = productInfo.MaxQuantity;
            parameters[32].Value = productInfo.MinQuantity;
            parameters[33].Value = productInfo.Tags;
            parameters[34].Value = productInfo.SeoUrl;
            parameters[35].Value = productInfo.SeoImageAlt;
            parameters[36].Value = productInfo.SeoImageTitle;
            parameters[37].Value = productInfo.SalesType;
            parameters[38].Value = productInfo.RestrictionCount;
            parameters[39].Value = productInfo.DeliveryTip;
            parameters[40].Value = productInfo.Remark;
            parameters[41].Value = productInfo.Gwjf;
            parameters[42].Value = productInfo.ProductId;

            return new CommandInfo(strSql.ToString(),
                                    parameters, EffentNextType.ExcuteEffectRows);
        }
        /// <summary>
        /// 修改团购表商品名称
        /// </summary>
        /// <param name="productInfo"></param>
        /// <returns></returns>
        private CommandInfo UpdateProductName(Model.Shop.Products.ProductInfo productInfo)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append(" UPDATE Shop_GroupBuy  SET ");
            strSql.Append("ProductName=@ProductName ");
            strSql.Append(" WHERE ProductId=@ProductId");
            SqlParameter[] parameters = {
                    new SqlParameter("@ProductName", SqlDbType.NVarChar,200),
                    new SqlParameter("@ProductId", SqlDbType.BigInt,8)};
            parameters[0].Value = productInfo.ProductName;
            parameters[1].Value = productInfo.ProductId;
            return new CommandInfo(strSql.ToString(), parameters);
        }
        private CommandInfo GenerateProductInfo(Model.Shop.Products.ProductInfo productInfo)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("INSERT INTO PMS_Products(");
            strSql.Append("CategoryId,TypeId,BrandId,ProductName,ProductCode,SupplierId,RegionId,ShortDescription,Unit,");
            strSql.Append("Description,Meta_Title,Meta_Description,Meta_Keywords,SaleStatus,AddedDate,VistiCounts,SaleCounts,");
            strSql.Append("DisplaySequence,LineId,MarketPrice,LowestSalePrice,PenetrationStatus,MainCategoryPath,");
            strSql.Append("ExtendCategoryPath,HasSKU,Points,ImageUrl,ThumbnailUrl1,ThumbnailUrl2,ThumbnailUrl3,ThumbnailUrl4,");
            strSql.Append("ThumbnailUrl5,ThumbnailUrl6,ThumbnailUrl7,ThumbnailUrl8,MaxQuantity,MinQuantity,Tags,SeoUrl,SeoImageAlt,SeoImageTitle,SalesType,RestrictionCount,DeliveryTip,Remark,Gwjf)");
            strSql.Append(" VALUES (");
            strSql.Append("@CategoryId,@TypeId,@BrandId,@ProductName,@ProductCode,@SupplierId,@RegionId,");
            strSql.Append("@ShortDescription,@Unit,@Description,@Title,@Meta_Description,@Meta_Keywords,");
            strSql.Append("@SaleStatus,@AddedDate,@VistiCounts,@SaleCounts,@DisplaySequence,@LineId,@MarketPrice,");
            strSql.Append("@LowestSalePrice,@PenetrationStatus,@MainCategoryPath,@ExtendCategoryPath,@HasSKU,");
            strSql.Append("@Points,@ImageUrl,@ThumbnailUrl1,@ThumbnailUrl2,@ThumbnailUrl3,@ThumbnailUrl4,");
            strSql.Append("@ThumbnailUrl5,@ThumbnailUrl6,@ThumbnailUrl7,@ThumbnailUrl8,@MaxQuantity,@MinQuantity,@Tags,@SeoUrl,@SeoImageAlt,@SeoImageTitle,@SalesType,@RestrictionCount,@DeliveryTip,@Remark,@Gwjf)");
            strSql.Append(";SELECT @@IDENTITY");
            SqlParameter[] parameters =
                            {
                                new SqlParameter("@CategoryId", SqlDbType.Int, 4),
                                new SqlParameter("@TypeId", SqlDbType.Int, 4),
                                new SqlParameter("@BrandId", SqlDbType.Int, 4),
                                new SqlParameter("@ProductName", SqlDbType.NVarChar, 200),
                                new SqlParameter("@ProductCode", SqlDbType.NVarChar, 50),
                                new SqlParameter("@SupplierId", SqlDbType.Int, 4),
                                new SqlParameter("@RegionId", SqlDbType.Int, 4),
                                new SqlParameter("@ShortDescription", SqlDbType.NVarChar, 2000),
                                new SqlParameter("@Unit", SqlDbType.NVarChar, 50),
                                new SqlParameter("@Description", SqlDbType.NText),
                                new SqlParameter("@Title", SqlDbType.NVarChar, 100),
                                new SqlParameter("@Meta_Description", SqlDbType.NVarChar, 1000),
                                new SqlParameter("@Meta_Keywords", SqlDbType.NVarChar, 1000),
                                new SqlParameter("@SaleStatus", SqlDbType.Int, 4),
                                new SqlParameter("@AddedDate", SqlDbType.DateTime),
                                new SqlParameter("@VistiCounts", SqlDbType.Int, 4),
                                new SqlParameter("@SaleCounts", SqlDbType.Int, 4),
                                new SqlParameter("@DisplaySequence", SqlDbType.Int, 4),
                                new SqlParameter("@LineId", SqlDbType.Int, 4),
                                new SqlParameter("@MarketPrice", SqlDbType.Money, 8),
                                new SqlParameter("@LowestSalePrice", SqlDbType.Money, 8),
                                new SqlParameter("@PenetrationStatus", SqlDbType.SmallInt, 2),
                                new SqlParameter("@MainCategoryPath", SqlDbType.NVarChar, 256),
                                new SqlParameter("@ExtendCategoryPath", SqlDbType.NVarChar, 256),
                                new SqlParameter("@HasSKU", SqlDbType.Bit, 1),
                                new SqlParameter("@Points", SqlDbType.Decimal, 9),
                                new SqlParameter("@ImageUrl", SqlDbType.NVarChar, 255),
                                new SqlParameter("@ThumbnailUrl1", SqlDbType.NVarChar, 255),
                                new SqlParameter("@ThumbnailUrl2", SqlDbType.NVarChar, 255),
                                new SqlParameter("@ThumbnailUrl3", SqlDbType.NVarChar, 255),
                                new SqlParameter("@ThumbnailUrl4", SqlDbType.NVarChar, 255),
                                new SqlParameter("@ThumbnailUrl5", SqlDbType.NVarChar, 255),
                                new SqlParameter("@ThumbnailUrl6", SqlDbType.NVarChar, 255),
                                new SqlParameter("@ThumbnailUrl7", SqlDbType.NVarChar, 255),
                                new SqlParameter("@ThumbnailUrl8", SqlDbType.NVarChar, 255),
                                new SqlParameter("@MaxQuantity", SqlDbType.Int, 4),
                                new SqlParameter("@MinQuantity", SqlDbType.Int, 4),
                                new SqlParameter("@Tags", SqlDbType.NVarChar,50),
                                new SqlParameter("@SeoUrl", SqlDbType.NVarChar,300),
                                new SqlParameter("@SeoImageAlt", SqlDbType.NVarChar,300),
                                new SqlParameter("@SeoImageTitle", SqlDbType.NVarChar,300),
                                new SqlParameter("@SalesType", SqlDbType.SmallInt,2),
                                new SqlParameter("@RestrictionCount", SqlDbType.Int,4),
                                new SqlParameter("@DeliveryTip", SqlDbType.NVarChar,100),
                                new SqlParameter("@Remark", SqlDbType.NVarChar,500),
                                new SqlParameter("@Gwjf", SqlDbType.Decimal,9)
                            };
            parameters[0].Value = productInfo.CategoryId;
            parameters[1].Value = productInfo.TypeId;
            parameters[2].Value = productInfo.BrandId;
            parameters[3].Value = productInfo.ProductName;
            parameters[4].Value = productInfo.ProductCode;
            parameters[5].Value = productInfo.SupplierId;
            parameters[6].Value = productInfo.RegionId;
            parameters[7].Value = productInfo.ShortDescription;
            parameters[8].Value = productInfo.Unit;
            parameters[9].Value = productInfo.Description;
            parameters[10].Value = productInfo.Meta_Title;
            parameters[11].Value = productInfo.Meta_Description;
            parameters[12].Value = productInfo.Meta_Keywords;
            parameters[13].Value = productInfo.SaleStatus;
            parameters[14].Value = productInfo.AddedDate;
            parameters[15].Value = productInfo.VistiCounts;
            parameters[16].Value = productInfo.SaleCounts;
            parameters[17].Value = productInfo.DisplaySequence;
            parameters[18].Value = productInfo.LineId;
            parameters[19].Value = productInfo.MarketPrice;
            parameters[20].Value = productInfo.LowestSalePrice;
            parameters[21].Value = productInfo.PenetrationStatus;
            parameters[22].Value = productInfo.MainCategoryPath;
            parameters[23].Value = productInfo.ExtendCategoryPath;
            parameters[24].Value = productInfo.HasSKU;
            parameters[25].Value = productInfo.Points;
            parameters[26].Value = productInfo.ImageUrl;
            parameters[27].Value = productInfo.ThumbnailUrl1;
            parameters[28].Value = productInfo.ThumbnailUrl2;
            parameters[29].Value = productInfo.ThumbnailUrl3;
            parameters[30].Value = productInfo.ThumbnailUrl4;
            parameters[31].Value = productInfo.ThumbnailUrl5;
            parameters[32].Value = productInfo.ThumbnailUrl6;
            parameters[33].Value = productInfo.ThumbnailUrl7;
            parameters[34].Value = productInfo.ThumbnailUrl8;
            parameters[35].Value = productInfo.MaxQuantity;
            parameters[36].Value = productInfo.MinQuantity;
            parameters[37].Value = productInfo.Tags;
            parameters[38].Value = productInfo.SeoUrl;
            parameters[39].Value = productInfo.SeoImageAlt;
            parameters[40].Value = productInfo.SeoImageTitle;
            parameters[41].Value = productInfo.SalesType;
            parameters[42].Value = productInfo.RestrictionCount;
            parameters[43].Value = productInfo.DeliveryTip;
            parameters[44].Value = productInfo.Remark;
            parameters[45].Value = productInfo.Gwjf;
            return new CommandInfo(strSql.ToString(), parameters, EffentNextType.ExcuteEffectRows);
        }
        /// <summary>
        /// 生产PMS的商品数据
        /// </summary>
        /// <param name="productInfo"></param>
        /// <returns></returns>
        private CommandInfo GeneratePMSProductInfo(Model.Shop.Products.ProductInfo productInfo)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("SET IDENTITY_INSERT [PMS_Products] ON ");
            strSql.Append(" delete from PMS_Products where ProductId=@ProductId ");
            strSql.Append(" INSERT INTO PMS_Products(");
            strSql.Append("ProductId,CategoryId,TypeId,BrandId,ProductName,ProductCode,SupplierId,RegionId,ShortDescription,Unit,");
            strSql.Append("Description,Meta_Title,Meta_Description,Meta_Keywords,SaleStatus,AddedDate,VistiCounts,SaleCounts,");
            strSql.Append("DisplaySequence,LineId,MarketPrice,LowestSalePrice,PenetrationStatus,MainCategoryPath,");
            strSql.Append("ExtendCategoryPath,HasSKU,Points,ImageUrl,ThumbnailUrl1,ThumbnailUrl2,ThumbnailUrl3,ThumbnailUrl4,");
            strSql.Append("ThumbnailUrl5,ThumbnailUrl6,ThumbnailUrl7,ThumbnailUrl8,MaxQuantity,MinQuantity,Tags,SeoUrl,SeoImageAlt,SeoImageTitle,SalesType,RestrictionCount,DeliveryTip,Remark,Gwjf)");
            strSql.Append(" VALUES (");
            strSql.Append("@ProductId,@CategoryId,@TypeId,@BrandId,@ProductName,@ProductCode,@SupplierId,@RegionId,");
            strSql.Append("@ShortDescription,@Unit,@Description,@Title,@Meta_Description,@Meta_Keywords,");
            strSql.Append("@SaleStatus,@AddedDate,@VistiCounts,@SaleCounts,@DisplaySequence,@LineId,@MarketPrice,");
            strSql.Append("@LowestSalePrice,@PenetrationStatus,@MainCategoryPath,@ExtendCategoryPath,@HasSKU,");
            strSql.Append("@Points,@ImageUrl,@ThumbnailUrl1,@ThumbnailUrl2,@ThumbnailUrl3,@ThumbnailUrl4,");
            strSql.Append("@ThumbnailUrl5,@ThumbnailUrl6,@ThumbnailUrl7,@ThumbnailUrl8,@MaxQuantity,@MinQuantity,@Tags,@SeoUrl,@SeoImageAlt,@SeoImageTitle,@SalesType,@RestrictionCount,@DeliveryTip,@Remark,@Gwjf)");
            strSql.Append("; SET IDENTITY_INSERT [PMS_Products] OFF");

            SqlParameter[] parameters =
                            {
                                new SqlParameter("@CategoryId", SqlDbType.Int, 4),
                                new SqlParameter("@TypeId", SqlDbType.Int, 4),
                                new SqlParameter("@BrandId", SqlDbType.Int, 4),
                                new SqlParameter("@ProductName", SqlDbType.NVarChar, 200),
                                new SqlParameter("@ProductCode", SqlDbType.NVarChar, 50),
                                new SqlParameter("@SupplierId", SqlDbType.Int, 4),
                                new SqlParameter("@RegionId", SqlDbType.Int, 4),
                                new SqlParameter("@ShortDescription", SqlDbType.NVarChar, 2000),
                                new SqlParameter("@Unit", SqlDbType.NVarChar, 50),
                                new SqlParameter("@Description", SqlDbType.NText),
                                new SqlParameter("@Title", SqlDbType.NVarChar, 100),
                                new SqlParameter("@Meta_Description", SqlDbType.NVarChar, 1000),
                                new SqlParameter("@Meta_Keywords", SqlDbType.NVarChar, 1000),
                                new SqlParameter("@SaleStatus", SqlDbType.Int, 4),
                                new SqlParameter("@AddedDate", SqlDbType.DateTime),
                                new SqlParameter("@VistiCounts", SqlDbType.Int, 4),
                                new SqlParameter("@SaleCounts", SqlDbType.Int, 4),
                                new SqlParameter("@DisplaySequence", SqlDbType.Int, 4),
                                new SqlParameter("@LineId", SqlDbType.Int, 4),
                                new SqlParameter("@MarketPrice", SqlDbType.Money, 8),
                                new SqlParameter("@LowestSalePrice", SqlDbType.Money, 8),
                                new SqlParameter("@PenetrationStatus", SqlDbType.SmallInt, 2),
                                new SqlParameter("@MainCategoryPath", SqlDbType.NVarChar, 256),
                                new SqlParameter("@ExtendCategoryPath", SqlDbType.NVarChar, 256),
                                new SqlParameter("@HasSKU", SqlDbType.Bit, 1),
                                new SqlParameter("@Points", SqlDbType.Decimal, 9),
                                new SqlParameter("@ImageUrl", SqlDbType.NVarChar, 255),
                                new SqlParameter("@ThumbnailUrl1", SqlDbType.NVarChar, 255),
                                new SqlParameter("@ThumbnailUrl2", SqlDbType.NVarChar, 255),
                                new SqlParameter("@ThumbnailUrl3", SqlDbType.NVarChar, 255),
                                new SqlParameter("@ThumbnailUrl4", SqlDbType.NVarChar, 255),
                                new SqlParameter("@ThumbnailUrl5", SqlDbType.NVarChar, 255),
                                new SqlParameter("@ThumbnailUrl6", SqlDbType.NVarChar, 255),
                                new SqlParameter("@ThumbnailUrl7", SqlDbType.NVarChar, 255),
                                new SqlParameter("@ThumbnailUrl8", SqlDbType.NVarChar, 255),
                                new SqlParameter("@MaxQuantity", SqlDbType.Int, 4),
                                new SqlParameter("@MinQuantity", SqlDbType.Int, 4),
                                new SqlParameter("@Tags", SqlDbType.NVarChar,50),
                                new SqlParameter("@SeoUrl", SqlDbType.NVarChar,300),
                                new SqlParameter("@SeoImageAlt", SqlDbType.NVarChar,300),
                                new SqlParameter("@SeoImageTitle", SqlDbType.NVarChar,300),
                              new SqlParameter("@SalesType", SqlDbType.SmallInt,2),
                              new SqlParameter("@RestrictionCount", SqlDbType.Int,4),
                             new SqlParameter("@DeliveryTip", SqlDbType.NVarChar,100),
                            new SqlParameter("@Remark", SqlDbType.NVarChar,500),
                            new SqlParameter("@Gwjf", SqlDbType.Decimal,9),
                      new SqlParameter("@ProductId", SqlDbType.BigInt, 8)
                            };
            parameters[0].Value = productInfo.CategoryId;
            parameters[1].Value = productInfo.TypeId;
            parameters[2].Value = productInfo.BrandId;
            parameters[3].Value = productInfo.ProductName;
            parameters[4].Value = productInfo.ProductCode;
            parameters[5].Value = productInfo.SupplierId;
            parameters[6].Value = productInfo.RegionId;
            parameters[7].Value = productInfo.ShortDescription;
            parameters[8].Value = productInfo.Unit;
            parameters[9].Value = productInfo.Description;
            parameters[10].Value = productInfo.Meta_Title;
            parameters[11].Value = productInfo.Meta_Description;
            parameters[12].Value = productInfo.Meta_Keywords;
            parameters[13].Value = productInfo.SaleStatus;
            parameters[14].Value = productInfo.AddedDate;
            parameters[15].Value = productInfo.VistiCounts;
            parameters[16].Value = productInfo.SaleCounts;
            parameters[17].Value = productInfo.DisplaySequence;
            parameters[18].Value = productInfo.LineId;
            parameters[19].Value = productInfo.MarketPrice;
            parameters[20].Value = productInfo.LowestSalePrice;
            parameters[21].Value = productInfo.PenetrationStatus;
            parameters[22].Value = productInfo.MainCategoryPath;
            parameters[23].Value = productInfo.ExtendCategoryPath;
            parameters[24].Value = productInfo.HasSKU;
            parameters[25].Value = productInfo.Points;
            parameters[26].Value = productInfo.ImageUrl;
            parameters[27].Value = productInfo.ThumbnailUrl1;
            parameters[28].Value = productInfo.ThumbnailUrl2;
            parameters[29].Value = productInfo.ThumbnailUrl3;
            parameters[30].Value = productInfo.ThumbnailUrl4;
            parameters[31].Value = productInfo.ThumbnailUrl5;
            parameters[32].Value = productInfo.ThumbnailUrl6;
            parameters[33].Value = productInfo.ThumbnailUrl7;
            parameters[34].Value = productInfo.ThumbnailUrl8;
            parameters[35].Value = productInfo.MaxQuantity;
            parameters[36].Value = productInfo.MinQuantity;
            parameters[37].Value = productInfo.Tags;
            parameters[38].Value = productInfo.SeoUrl;
            parameters[39].Value = productInfo.SeoImageAlt;
            parameters[40].Value = productInfo.SeoImageTitle;
            parameters[41].Value = productInfo.SalesType;
            parameters[42].Value = productInfo.RestrictionCount;
            parameters[43].Value = productInfo.DeliveryTip;
            parameters[44].Value = productInfo.Remark;
            parameters[45].Value = productInfo.Gwjf;
            parameters[46].Value = productInfo.ProductId;
            return new CommandInfo(strSql.ToString(), parameters, EffentNextType.ExcuteEffectRows);
        }

        #endregion 产品信息

        #region 属性

        private List<CommandInfo> GenerateAttributeInfo(Model.Shop.Products.ProductInfo productInfo, SqlTransaction transaction)
        {
            List<CommandInfo> list = new List<CommandInfo>();
            if (productInfo.AttributeInfos == null)
            {
                return list;
            }
            foreach (Model.Shop.Products.AttributeInfo attributeInfo in productInfo.AttributeInfos)
            {
                switch (Globals.SafeEnum<ProductAttributeModel>(
                    attributeInfo.UsageMode.ToString(CultureInfo.InvariantCulture),
                    ProductAttributeModel.None))
                {
                    case ProductAttributeModel.One:
                        list.Add(GenerateAttribute4One(attributeInfo.AttributeValues[0], productInfo.ProductId));
                        break;

                    case ProductAttributeModel.Input:
                        list.Add(GenerateAttribute4Input(attributeInfo.AttributeValues[0], productInfo.ProductId, transaction));
                        break;

                    case ProductAttributeModel.Any:
                        foreach (Model.Shop.Products.AttributeValue attributeValue in attributeInfo.AttributeValues)
                        {
                            list.Add(GenerateAttribute4One(attributeValue, productInfo.ProductId));
                        }
                        break;
                    default:
                        break;
                }
            }
            return list;
        }

        private List<CommandInfo> GeneratePMSAttributeInfo(Model.Shop.Products.ProductInfo productInfo, SqlTransaction transaction)
        {
            List<CommandInfo> list = new List<CommandInfo>();
            if (productInfo.AttributeInfos == null)
            {
                return list;
            }
            foreach (Model.Shop.Products.AttributeInfo attributeInfo in productInfo.AttributeInfos)
            {
                switch (Globals.SafeEnum<ProductAttributeModel>(
                    attributeInfo.UsageMode.ToString(CultureInfo.InvariantCulture),
                    ProductAttributeModel.None))
                {
                    case ProductAttributeModel.One:
                        list.Add(GenerateAttribute4One(attributeInfo.AttributeValues[0], productInfo.ProductId));
                        break;

                    case ProductAttributeModel.Input:
                        list.Add(GeneratePMSAttribute4Input(attributeInfo.AttributeValues[0], productInfo.ProductId, transaction));
                        break;

                    case ProductAttributeModel.Any:
                        foreach (Model.Shop.Products.AttributeValue attributeValue in attributeInfo.AttributeValues)
                        {
                            list.Add(GenerateAttribute4One(attributeValue, productInfo.ProductId));
                        }
                        break;
                    default:
                        break;
                }
            }
            return list;
        }

        private CommandInfo GeneratePMSAttribute4Input(Model.Shop.Products.AttributeValue attributeValue, long productId, SqlTransaction transaction)
        {
            // Insert Input Value
            StringBuilder strSql = new StringBuilder();
            strSql.Append("SET IDENTITY_INSERT [PMS_AttributeValues] ON ");
            strSql.Append(" delete from PMS_AttributeValues where ValueId=@ValueId ");
            strSql.Append(" INSERT INTO PMS_AttributeValues(");
            strSql.Append("ValueId,AttributeId,DisplaySequence,ValueStr,ImageUrl)");
            strSql.Append(" VALUES (");
            strSql.Append("@ValueId,@AttributeId,@DisplaySequence,@ValueStr,@ImageUrl)");
            strSql.Append("; SET IDENTITY_INSERT [PMS_Products] OFF");
            SqlParameter[] parameters = {
                                            new SqlParameter("@AttributeId", SqlDbType.BigInt, 8),
                                            new SqlParameter("@DisplaySequence", SqlDbType.Int, 4),
                                            new SqlParameter("@ValueStr", SqlDbType.NVarChar, 200),
                                            new SqlParameter("@ImageUrl", SqlDbType.NVarChar, 255),
                                            new SqlParameter("@ValueId", SqlDbType.NVarChar, 255)
                                        };
            parameters[0].Value = attributeValue.AttributeId;
            parameters[1].Value = -1;
            parameters[2].Value = attributeValue.ValueStr;
            parameters[3].Value = attributeValue.ImageUrl;
            parameters[4].Value = attributeValue.ValueId;

            DBHelper.DefaultDBHelper.GetSingle4Trans(new CommandInfo(strSql.ToString(),
                                     parameters, EffentNextType.ExcuteEffectRows), transaction);
            return GenerateAttribute4One(attributeValue, productId);

        }

        private CommandInfo GenerateAttribute4Input(Model.Shop.Products.AttributeValue attributeValue, long productId, SqlTransaction transaction)
        {
            // Insert Input Value
            StringBuilder strSql = new StringBuilder();
            strSql.Append("INSERT INTO PMS_AttributeValues(");
            strSql.Append("AttributeId,DisplaySequence,ValueStr,ImageUrl)");
            strSql.Append(" VALUES (");
            strSql.Append("@AttributeId,@DisplaySequence,@ValueStr,@ImageUrl)");
            strSql.Append(";SELECT @@IDENTITY");
            SqlParameter[] parameters = {
                                            new SqlParameter("@AttributeId", SqlDbType.BigInt, 8),
                                            new SqlParameter("@DisplaySequence", SqlDbType.Int, 4),
                                            new SqlParameter("@ValueStr", SqlDbType.NVarChar, 200),
                                            new SqlParameter("@ImageUrl", SqlDbType.NVarChar, 255)
                                        };
            parameters[0].Value = attributeValue.AttributeId;
            parameters[1].Value = -1;
            parameters[2].Value = attributeValue.ValueStr;
            parameters[3].Value = attributeValue.ImageUrl;

            object obj = DBHelper.DefaultDBHelper.GetSingle4Trans(new CommandInfo(strSql.ToString(),
                                    parameters, EffentNextType.ExcuteEffectRows), transaction);
            attributeValue.ValueId = Globals.SafeInt(obj.ToString(), -1);

            return GenerateAttribute4One(attributeValue, productId);
        }

        private CommandInfo GenerateAttribute4One(Model.Shop.Products.AttributeValue attributeValue, long productId)
        {
            // Insert ValueId
            StringBuilder strSql;
            strSql = new StringBuilder();
            strSql.Append("delete from PMS_ProductAttributes where ProductId=@ProductId and AttributeId=@AttributeId and ValueId=@ValueId;");
            strSql.Append("INSERT INTO PMS_ProductAttributes(");
            strSql.Append("ProductId,AttributeId,ValueId)");
            strSql.Append(" VALUES (");
            strSql.Append("@ProductId,@AttributeId,@ValueId)");
            SqlParameter[] parameters = {
                    new SqlParameter("@ProductId", SqlDbType.BigInt, 8),
                    new SqlParameter("@AttributeId", SqlDbType.BigInt, 8),
                    new SqlParameter("@ValueId", SqlDbType.Int, 4)};
            parameters[0].Value = productId;
            parameters[1].Value = attributeValue.AttributeId;
            parameters[2].Value = attributeValue.ValueId;
            return new CommandInfo(strSql.ToString(),
                                    parameters, EffentNextType.ExcuteEffectRows);
        }

        #endregion 属性

        #region SKU

        private List<CommandInfo> GenerateSKUs(Model.Shop.Products.ProductInfo productInfo, SqlTransaction transaction)
        {
            Dictionary<long, long> specValues = new Dictionary<long, long>();   //Key:ValueId , Value:specId
            List<CommandInfo> list = new List<CommandInfo>();
            if (productInfo.SkuInfos == null)
            {
                return list;
            }
            foreach (Model.Shop.Products.SKUInfo skuInfo in productInfo.SkuInfos)
            {
                StringBuilder strSql = new StringBuilder();
                strSql.Append("INSERT INTO PMS_SKUs(");
                strSql.Append("ProductId,SKU,Weight,Stock,AlertStock,CostPrice,SalePrice,Upselling,Gwjf)");
                strSql.Append(" VALUES (");
                strSql.Append("@ProductId,@SKU,@Weight,@Stock,@AlertStock,@CostPrice,@SalePrice,@Upselling,@Gwjf)");
                strSql.Append(";SELECT @RESULT = @@IDENTITY");
                SqlParameter[] parameters = {
                                                new SqlParameter("@ProductId", SqlDbType.BigInt, 8),
                                                new SqlParameter("@SKU", SqlDbType.NVarChar, 50),
                                                new SqlParameter("@Weight", SqlDbType.Int, 4),
                                                new SqlParameter("@Stock", SqlDbType.Int, 4),
                                                new SqlParameter("@AlertStock", SqlDbType.Int, 4),
                                                new SqlParameter("@CostPrice", SqlDbType.Money, 8),
                                                new SqlParameter("@SalePrice", SqlDbType.Money, 8),
                                                new SqlParameter("@Upselling", SqlDbType.Decimal,9),
                                                new SqlParameter("@Gwjf", SqlDbType.Bit, 1),
                                                DBHelper.DefaultDBHelper.CreateOutParam("@RESULT", SqlDbType.BigInt, 8)//输出主键
                                            };
                parameters[0].Value = productInfo.ProductId;
                parameters[1].Value = skuInfo.SKU;
                parameters[2].Value = skuInfo.Weight;
                parameters[3].Value = skuInfo.Stock;
                parameters[4].Value = skuInfo.AlertStock;
                parameters[5].Value = skuInfo.CostPrice;
                parameters[6].Value = skuInfo.SalePrice;
                parameters[7].Value = skuInfo.Upselling;
                parameters[8].Value = skuInfo.Gwjf;
                list.Add(new CommandInfo(strSql.ToString(),
                                         parameters, EffentNextType.ExcuteEffectRows));
                if (skuInfo.SkuItems != null)
                {
                    foreach (Model.Shop.Products.SKUItem skuItem in skuInfo.SkuItems)
                    {
                        if (!specValues.ContainsKey(skuItem.ValueId))
                        {
                            object result = DBHelper.DefaultDBHelper.GetSingle4Trans(GenerateSKUItems(skuItem, productInfo), transaction);
                            long specId = Globals.SafeLong(result.ToString(), -1);

                            specValues.Add(skuItem.ValueId, specId);
                        }

                        strSql = new StringBuilder();
                        strSql.Append("INSERT INTO PMS_SKURelation(");
                        strSql.Append("SkuId,SpecId,ProductId)");
                        strSql.Append(" VALUES (");
                        strSql.Append("@SkuId,@SpecId,@ProductId)");
                        parameters = new[]{
                            DBHelper.DefaultDBHelper.CreateInputOutParam("@SkuId", SqlDbType.BigInt, 8, null), //输入主键
                            new SqlParameter("@SpecId", SqlDbType.BigInt,8),
                            new SqlParameter("@ProductId", SqlDbType.BigInt,8)
                        };
                        parameters[1].Value = specValues[skuItem.ValueId];
                        parameters[2].Value = productInfo.ProductId;

                        list.Add(new CommandInfo(strSql.ToString(),
                                             parameters, EffentNextType.ExcuteEffectRows));
                    }
                }

            }
            return list;
        }

        //TODO: 说明代码
        private CommandInfo CheckSkuItems(Model.Shop.Products.ProductInfo oldProductInfo, Model.Shop.Products.ProductInfo newProductInfo)
        {
            DataTable oldSKUItem = new DataTable(); //DB
            List<Model.Shop.Products.SKUItem> newSKUItem = new List<Model.Shop.Products.SKUItem>(); //页面

            foreach (DataRow row in oldSKUItem.Rows)
            {
                //NULL
                string imgURL = row["ImageUrl"].ToString();
                if (!newSKUItem.Exists(xx => xx.ImageUrl == imgURL))
                {
                    //DEL File 物理删除
                }
            }

            return null;
        }

        private CommandInfo GenerateSKUItems(Model.Shop.Products.SKUItem skuItem, Model.Shop.Products.ProductInfo productInfo)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("INSERT INTO PMS_SKUItems(");
            strSql.Append("AttributeId,ValueId,ImageUrl,ValueStr,ProductId)");
            strSql.Append(" VALUES (");
            strSql.Append("@AttributeId,@ValueId,@ImageUrl,@ValueStr,@ProductId)");
            strSql.Append(";SELECT @@IDENTITY");
            SqlParameter[] parameters = new[]{
                            new SqlParameter("@AttributeId", SqlDbType.BigInt,8),
                            new SqlParameter("@ValueId", SqlDbType.BigInt,8),
                            new SqlParameter("@ImageUrl", SqlDbType.NVarChar),
                            new SqlParameter("@ValueStr", SqlDbType.NVarChar),
                            new SqlParameter("@ProductId", SqlDbType.BigInt,8)
                        };
            parameters[0].Value = skuItem.AttributeId;
            parameters[1].Value = skuItem.ValueId;
            parameters[2].Value = skuItem.ImageUrl;
            parameters[3].Value = skuItem.ValueStr;
            parameters[4].Value = productInfo.ProductId;
            return new CommandInfo(strSql.ToString(),
                                    parameters, EffentNextType.ExcuteEffectRows);
        }

        #endregion SKU

        #region Package

        private List<CommandInfo> GeneratePackage(Model.Shop.Products.ProductInfo productInfo)
        {
            List<CommandInfo> list = new List<CommandInfo>();
            if (productInfo.PackageId != null)
            {
                foreach (int PackageId in productInfo.PackageId)
                {
                    StringBuilder strSql = new StringBuilder();
                    strSql.Append("insert into Shop_ProductPackage(");
                    strSql.Append("ProductId,PackageId)");
                    strSql.Append(" values (");
                    strSql.Append("@ProductId,@PackageId)");
                    SqlParameter[] parameters =
                        {
                            new SqlParameter("@ProductId", SqlDbType.BigInt, 8),
                            new SqlParameter("@PackageId", SqlDbType.Int, 4)
                        };
                    parameters[0].Value = productInfo.ProductId;
                    parameters[1].Value = PackageId;
                    list.Add(new CommandInfo(strSql.ToString(), parameters, EffentNextType.ExcuteEffectRows));
                }
            }
            return list;
        }
        #endregion

        #region 图片

        private List<CommandInfo> GenerateImages(Model.Shop.Products.ProductInfo productInfo)
        {
            List<CommandInfo> list = new List<CommandInfo>();
            if (productInfo.ProductImages == null)
            {
                return list;
            }
            foreach (Model.Shop.Products.ProductImage productImage in productInfo.ProductImages)
            {
                StringBuilder strSql = new StringBuilder();
                strSql.Append("INSERT INTO PMS_ProductImages(");
                strSql.Append("ProductId,ImageUrl,ThumbnailUrl1,ThumbnailUrl2,ThumbnailUrl3,ThumbnailUrl4,ThumbnailUrl5,ThumbnailUrl6,ThumbnailUrl7,ThumbnailUrl8)");
                strSql.Append(" VALUES (");
                strSql.Append("@ProductId,@ImageUrl,@ThumbnailUrl1,@ThumbnailUrl2,@ThumbnailUrl3,@ThumbnailUrl4,@ThumbnailUrl5,@ThumbnailUrl6,@ThumbnailUrl7,@ThumbnailUrl8)");
                strSql.Append(";SELECT @@IDENTITY");
                SqlParameter[] parameters = {
                    new SqlParameter("@ProductId", SqlDbType.BigInt,8),
                    new SqlParameter("@ImageUrl", SqlDbType.NVarChar,255),
                    new SqlParameter("@ThumbnailUrl1", SqlDbType.NVarChar,255),
                    new SqlParameter("@ThumbnailUrl2", SqlDbType.NVarChar,255),
                    new SqlParameter("@ThumbnailUrl3", SqlDbType.NVarChar,255),
                    new SqlParameter("@ThumbnailUrl4", SqlDbType.NVarChar,255),
                    new SqlParameter("@ThumbnailUrl5", SqlDbType.NVarChar,255),
                    new SqlParameter("@ThumbnailUrl6", SqlDbType.NVarChar,255),
                    new SqlParameter("@ThumbnailUrl7", SqlDbType.NVarChar,255),
                    new SqlParameter("@ThumbnailUrl8", SqlDbType.NVarChar,255)};
                parameters[0].Value = productInfo.ProductId;    //产品主键
                parameters[1].Value = productImage.ImageUrl;
                parameters[2].Value = productImage.ThumbnailUrl1;
                parameters[3].Value = productImage.ThumbnailUrl2;
                parameters[4].Value = productImage.ThumbnailUrl3;
                parameters[5].Value = productImage.ThumbnailUrl4;
                parameters[6].Value = productImage.ThumbnailUrl5;
                parameters[7].Value = productImage.ThumbnailUrl6;
                parameters[8].Value = productImage.ThumbnailUrl7;
                parameters[9].Value = productImage.ThumbnailUrl8;

                list.Add(new CommandInfo(strSql.ToString(),
                                         parameters, EffentNextType.ExcuteEffectRows));
            }
            return list;
        }

        #endregion 图片


        //#region 添加配件  
        //private List<CommandInfo> GenerateAccessories(Model.Shop.Products.ProductInfo productInfo)
        //{
        //    List<CommandInfo> list = new List<CommandInfo>();
        //    foreach (Model.Shop.Products.ProductAccessorie productAccess in productInfo.ProductAccessories)
        //    {
        //        StringBuilder strSql = new StringBuilder();
        //        strSql.Append("INSERT INTO Shop_AccessoriesValues (");
        //        strSql.Append(" ProductAccessoriesId ,ProductAccessoriesSKU)");
        //        strSql.Append(" VALUES  ((SELECT ProductId FROM PMS_SKUs WHERE SkuId=@ProductAccessoriesSKU),@ProductAccessoriesSKU)");
        //        strSql.Append(";SELECT @RESULT = @@IDENTITY");
        //        SqlParameter[] parameters ={
        //                                  new SqlParameter("@ProductAccessoriesSKU",SqlDbType.NVarChar),
        //                                        DBHelper.DefaultDBHelper.CreateOutParam("@RESULT", SqlDbType.BigInt, 8)//输出主键
        //                                  };
        //        parameters[0].Value = productAccess.SkuId;

        //        list.Add(new CommandInfo(strSql.ToString(),
        //                                 parameters, EffentNextType.ExcuteEffectRows));

        //        strSql = new StringBuilder();
        //        strSql.Append("INSERT INTO Shop_ProductAccessories(");
        //        strSql.Append("ProductId ,AccessoriesValueId ,Name ,MaxQuantity ,MinQuantity ,DiscountType ,DiscountAmount)");
        //        strSql.Append(" VALUES (");
        //        strSql.Append("@ProductId ,@AccessoriesValueId ,@AccessoriesName ,@MaxQuantity ,@MinQuantity ,@DiscountType ,@DiscountAmount)");
        //        SqlParameter[] param ={
        //                             new SqlParameter("@ProductId",SqlDbType.BigInt,8),
        //                            DBHelper.DefaultDBHelper.CreateInputOutParam("@AccessoriesValueId", SqlDbType.BigInt, 8, null), //输入主键
        //                             new SqlParameter("@Name",SqlDbType.NVarChar),
        //                             new SqlParameter("@MaxQuantity",SqlDbType.Int),
        //                             new SqlParameter("@MinQuantity",SqlDbType.Int),
        //                             new SqlParameter("@DiscountType",SqlDbType.Int),
        //                             new SqlParameter("@DiscountAmount",SqlDbType.Int)
        //                             };
        //        param[0].Value = productInfo.ProductId;
        //        param[2].Value = productAccess.Name;
        //        param[3].Value = productAccess.MaxQuantity;
        //        param[4].Value = productAccess.MinQuantity;
        //        param[5].Value = productAccess.DiscountType;
        //        param[6].Value = productAccess.DiscountAmount;
        //        list.Add(new CommandInfo(strSql.ToString(), param, EffentNextType.ExcuteEffectRows));
        //    }
        //    return list;
        //}

        //   #endregion 添加配件

        #region 相关商品

        private List<CommandInfo> GenerateRelatedProduct(Model.Shop.Products.ProductInfo productInfo)
        {
            List<CommandInfo> list = new List<CommandInfo>();
            if (productInfo.RelatedProductId == null || productInfo.RelatedProductId.Length == 0) return list;
            foreach (string item in productInfo.RelatedProductId)
            {
                string[] relatedPid = item.Split(new char[] { '_' }, StringSplitOptions.RemoveEmptyEntries);
                if (Globals.SafeInt(relatedPid[1], 0) == 0)
                {
                    StringBuilder strSql = new StringBuilder();
                    strSql.Append("INSERT INTO Shop_RelatedProducts(");
                    strSql.Append(" RelatedId, ProductId )");
                    strSql.Append("VALUES  (");
                    strSql.Append("@RelatedId,@ProductId)");
                    SqlParameter[] parameters = {
                                                        new SqlParameter("@ProductId", SqlDbType.BigInt, 8),
                                                        new SqlParameter("@RelatedId", SqlDbType.BigInt, 8)
                                                    };
                    parameters[0].Value = productInfo.ProductId;
                    parameters[1].Value = Globals.SafeLong(relatedPid[0], -1);

                    list.Add(new CommandInfo(strSql.ToString(),
                                             parameters, EffentNextType.ExcuteEffectRows));
                }
                else
                {
                    StringBuilder strSql = new StringBuilder();
                    strSql.Append("INSERT INTO Shop_RelatedProducts(");
                    strSql.Append(" RelatedId, ProductId )");
                    strSql.Append("VALUES  (");
                    strSql.Append("@RelatedId,@ProductId)");
                    SqlParameter[] parameters = {
                                                        new SqlParameter("@ProductId", SqlDbType.BigInt, 8),
                                                        new SqlParameter("@RelatedId", SqlDbType.BigInt, 8)
                                                    };
                    parameters[0].Value = productInfo.ProductId;
                    parameters[1].Value = Globals.SafeLong(relatedPid[0], -1);

                    list.Add(new CommandInfo(strSql.ToString(), parameters, EffentNextType.ExcuteEffectRows));

                    StringBuilder strSqlRe = new StringBuilder();
                    strSqlRe.Append("INSERT INTO Shop_RelatedProducts(");
                    strSqlRe.Append(" RelatedId, ProductId )");
                    strSqlRe.Append("VALUES  (");
                    strSqlRe.Append("@RelatedId,@ProductId)");
                    SqlParameter[] para = {
                                                        new SqlParameter("@ProductId", SqlDbType.BigInt, 8),
                                                        new SqlParameter("@RelatedId", SqlDbType.BigInt, 8)
                                                    };
                    para[0].Value = Globals.SafeLong(relatedPid[0], -1);
                    para[1].Value = productInfo.ProductId;

                    list.Add(new CommandInfo(strSqlRe.ToString(), para, EffentNextType.ExcuteEffectRows));
                }
            }
            return list;
        }

        #endregion 相关商品

        #region 添加产品分类

        private List<CommandInfo> SaveProductCategories(Model.Shop.Products.ProductInfo productInfo)
        {
            List<CommandInfo> list = new List<CommandInfo>();
            if (productInfo.Product_Categories == null)
            {
                return list;
            }
            foreach (string productCategory in productInfo.Product_Categories)
            {
                if (!string.IsNullOrWhiteSpace(productCategory))
                {
                    string[] categoryArray = productCategory.Split(new char[] { '_' }, StringSplitOptions.RemoveEmptyEntries);
                    int categoryId = Globals.SafeInt(categoryArray[0], 0);
                    list.Add(GeneratePaoductCategoriesOne(categoryId, productInfo.ProductId, categoryArray[1]));
                }
            }
            return list;
        }

        private CommandInfo GeneratePaoductCategoriesOne(int categoriesId, long productId, string path)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("INSERT INTO PMS_ProductCategories ( CategoryId, ProductId,CategoryPath ) ");
            strSql.Append(" VALUES (");
            strSql.Append("@CategoryId,@ProductId,@CategoryPath)");
            SqlParameter[] parameters = {
                    new SqlParameter("@ProductId", SqlDbType.BigInt, 8),
                    new SqlParameter("@CategoryId", SqlDbType.Int, 4),
                    new SqlParameter("@CategoryPath", SqlDbType.NVarChar)
                                        };
            parameters[0].Value = productId;
            parameters[1].Value = categoriesId;
            parameters[2].Value = path;
            return new CommandInfo(strSql.ToString(),
                                    parameters, EffentNextType.ExcuteEffectRows);
        }

        #endregion 添加产品分类

        #region 添加店铺产品分类

        private List<CommandInfo> SaveSuppProductCategories(Model.Shop.Products.ProductInfo productInfo)
        {
            List<CommandInfo> list = new List<CommandInfo>();
            StringBuilder strSql = new StringBuilder();
            strSql.Append("INSERT INTO Shop_SuppProductCategories ( CategoryId, ProductId,CategoryPath ) ");
            strSql.Append(" VALUES (");
            strSql.Append("@CategoryId,@ProductId,@CategoryPath)");
            SqlParameter[] parameters = {
                    new SqlParameter("@ProductId", SqlDbType.BigInt, 8),
                    new SqlParameter("@CategoryId", SqlDbType.Int, 4),
                    new SqlParameter("@CategoryPath", SqlDbType.NVarChar)
                                        };
            parameters[0].Value = productInfo.ProductId;
            parameters[1].Value = productInfo.SuppCategoryId;
            parameters[2].Value = productInfo.SuppCategoryPath;
            list.Add(new CommandInfo(strSql.ToString(), parameters, EffentNextType.ExcuteEffectRows));
            return list;
        }
        private List<CommandInfo> UpdateSuppProductCategories(Model.Shop.Products.ProductInfo productInfo)
        {
            List<CommandInfo> list = new List<CommandInfo>();
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update Shop_SuppProductCategories set ");
            strSql.Append("CategoryPath=@CategoryPath ,");
            strSql.Append(" CategoryId=@CategoryId");
            strSql.Append(" where and ProductId=@ProductId ");
            SqlParameter[] parameters = {
                    new SqlParameter("@CategoryPath", SqlDbType.NVarChar,4000),
                    new SqlParameter("@CategoryId", SqlDbType.Int,4),
                    new SqlParameter("@ProductId", SqlDbType.BigInt,8)};
            parameters[0].Value = productInfo.SuppCategoryPath;
            parameters[1].Value = productInfo.SuppCategoryId;
            parameters[2].Value = productInfo.SuppCategoryPath;
            list.Add(new CommandInfo(strSql.ToString(), parameters, EffentNextType.ExcuteEffectRows));
            return list;
        }

        #endregion 添加产品分类

        #region 产品对比
        public DataSet GetCompareProudctInfo(string ids)
        {
            SqlParameter[] parameters = {
                    new SqlParameter("@ProductIDs", SqlDbType.NVarChar)
                    };
            parameters[0].Value = ids;
            return DBHelper.DefaultDBHelper.RunProcedure("sp_Shop_CompareProduct", parameters, "ds");
        }

        public DataSet GetCompareProudctBasicInfo(string ids)
        {
            SqlParameter[] parameters = {
                    new SqlParameter("@ProductIDs", SqlDbType.NVarChar)
                    };
            parameters[0].Value = ids;
            return DBHelper.DefaultDBHelper.RunProcedure("sp_Shop_CompareProductBasicInfo", parameters, "ds");
        }
        #endregion

        #region 产品推荐
        private CommandInfo GenerateProductStationModes(Model.Shop.Products.ProductInfo productInfo, int type)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("DELETE Shop_ProductStationModes WHERE ProductId = @ProductId AND [Type] = @Type; ");
            strSql.Append("INSERT INTO Shop_ProductStationModes(");
            strSql.Append("ProductId,DisplaySequence,Type)");
            strSql.Append(" VALUES (");
            strSql.Append("@ProductId,@DisplaySequence,@Type)");
            strSql.Append(";SELECT @@IDENTITY");
            SqlParameter[] parameters =
                {
                    new SqlParameter("@ProductId", SqlDbType.Int, 4),
                    new SqlParameter("@DisplaySequence", SqlDbType.Int, 4),
                    new SqlParameter("@Type", SqlDbType.Int, 4)
                };
            parameters[0].Value = productInfo.ProductId;
            parameters[1].Value = productInfo.ProductId;
            parameters[2].Value = type;
            return new CommandInfo(strSql.ToString(),
                                    parameters, EffentNextType.ExcuteEffectRows);
        }
        private CommandInfo DelProductStationModes(Model.Shop.Products.ProductInfo productInfo, int type)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("DELETE Shop_ProductStationModes WHERE ProductId = @ProductId AND [Type] = @Type; ");
            strSql.Append(";SELECT @@IDENTITY");
            SqlParameter[] parameters =
                {
                    new SqlParameter("@ProductId", SqlDbType.Int, 4),
                    new SqlParameter("@Type", SqlDbType.Int, 4)
                };
            parameters[0].Value = productInfo.ProductId;
            parameters[1].Value = type;
            return new CommandInfo(strSql.ToString(),
                                    parameters, EffentNextType.ExcuteEffectRows);
        }
        #endregion

        #region 修改SKU库存

        public bool ModifyStock(string sku,int stock)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("UPDATE PMS_SKUs SET ");
            strSql.Append("Stock=@Stock");
            strSql.Append(" WHERE SKU=@SKU");
            SqlParameter[] parameters = {
                    new SqlParameter("@SKU", SqlDbType.NVarChar,50),
                    new SqlParameter("@Stock", SqlDbType.Int,4)
            };
            parameters[0].Value = sku;
            parameters[1].Value = stock;

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

        #endregion

        #region　修改导入商品
        public bool ImportModifyProduct(Model.Shop.Products.ProductInfo productInfo)
        {
            using (SqlConnection connection = DBHelper.DefaultDBHelper.GetDBObject().GetConnection)
            {
                connection.Open();
                using (SqlTransaction transaction = connection.BeginTransaction())
                {
                    try
                    {
                        //删除商品分类
                        DBHelper.DefaultDBHelper.GetSingle4Trans(DelCategories(productInfo.ProductId), transaction);

                        //TODO：更新商品基本信息
                        DBHelper.DefaultDBHelper.GetSingle4Trans(UpdateImportProduct(productInfo), transaction);

                        //TODO  更新团购表商品名称
                        DBHelper.DefaultDBHelper.GetSingle4Trans(UpdateProductName(productInfo), transaction);

                        //添加产品分类
                        DBHelper.DefaultDBHelper.ExecuteSqlTran4Indentity(SaveProductCategories(productInfo), transaction);

                        //更新SKU信息
                        CommandInfo cmd = UpdateSKU(productInfo);
                        if (cmd != null) {
                            DBHelper.DefaultDBHelper.GetSingle4Trans(cmd, transaction);
                        }

                        transaction.Commit();
                    }
                    catch (SqlException)
                    {
                        transaction.Rollback();
                        return false;
                    }
                }
            }
            return true;
        }
        private CommandInfo DelCategories(long productId)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append(" DELETE FROM PMS_ProductCategories  ");
            strSql.Append(" WHERE ProductId=@ProductId");
            SqlParameter[] parameters = {
                    new SqlParameter("@ProductId", SqlDbType.BigInt,8)};
            parameters[0].Value = productId;
            return new CommandInfo(strSql.ToString(),parameters);
        }
        private CommandInfo UpdateSKU(Model.Shop.Products.ProductInfo productInfo)
        {
            if (productInfo.SkuInfos == null || productInfo.SkuInfos.Count == 0) {
                return null;
            }
            YSWL.MALL.Model.Shop.Products.SKUInfo model = productInfo.SkuInfos[0];
            StringBuilder strSql = new StringBuilder();
            strSql.Append("UPDATE PMS_SKUs SET ");
            strSql.Append("Weight=@Weight,");
            strSql.Append("Stock=@Stock,");
            strSql.Append("CostPrice=@CostPrice,");
            strSql.Append("SalePrice=@SalePrice ");
            strSql.Append(" WHERE Sku=@Sku");
            SqlParameter[] parameters = {
                    new SqlParameter("@Weight", SqlDbType.Int,4),
                    new SqlParameter("@Stock", SqlDbType.Int,4),
                    new SqlParameter("@CostPrice", SqlDbType.Money,8),
                    new SqlParameter("@SalePrice", SqlDbType.Money,8),
                     new SqlParameter("@SKU", SqlDbType.NVarChar,50)};
            parameters[0].Value = model.Weight;
            parameters[1].Value = model.Stock;
            parameters[2].Value = model.CostPrice;
            parameters[3].Value = model.SalePrice;
            parameters[4].Value = model.SKU;
            return new CommandInfo(strSql.ToString(), parameters);
        }
        private CommandInfo UpdateImportProduct(Model.Shop.Products.ProductInfo productInfo)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("UPDATE PMS_Products SET ");
            strSql.Append("TypeId=@TypeId,");
            strSql.Append("BrandId=@BrandId,");
            strSql.Append("ProductName=@ProductName,");
            strSql.Append("ProductCode=@ProductCode,");
            strSql.Append("Description=@Description,");
            strSql.Append("SaleStatus=@SaleStatus,");
            strSql.Append("MarketPrice=@MarketPrice,");
            strSql.Append("LowestSalePrice=@LowestSalePrice,");
            strSql.Append("Points=@Points,");
            strSql.Append("ImageUrl=@ImageUrl,");
            strSql.Append("ThumbnailUrl1=@ThumbnailUrl1");
            strSql.Append(" WHERE ProductId=@ProductId");
            SqlParameter[] parameters = {
                    new SqlParameter("@TypeId", SqlDbType.Int,4),
                    new SqlParameter("@BrandId", SqlDbType.Int,4),
                    new SqlParameter("@ProductName", SqlDbType.NVarChar,200),
                    new SqlParameter("@ProductCode", SqlDbType.NVarChar,50),
                    new SqlParameter("@Description", SqlDbType.NText),
                    new SqlParameter("@SaleStatus", SqlDbType.Int,4),
                    new SqlParameter("@MarketPrice", SqlDbType.Money,8),
                    new SqlParameter("@LowestSalePrice", SqlDbType.Money,8),
                    new SqlParameter("@Points", SqlDbType.Decimal,9),
                    new SqlParameter("@ImageUrl", SqlDbType.NVarChar,255),
                    new SqlParameter("@ThumbnailUrl1", SqlDbType.NVarChar,255),
                    new SqlParameter("@ProductId", SqlDbType.BigInt,8)};
            parameters[0].Value = productInfo.TypeId;
            parameters[1].Value = productInfo.BrandId;
            parameters[2].Value = productInfo.ProductName;
            parameters[3].Value = productInfo.ProductCode;
            parameters[4].Value = productInfo.Description;
            parameters[5].Value = productInfo.SaleStatus;
            parameters[6].Value = productInfo.MarketPrice;
            parameters[7].Value = productInfo.LowestSalePrice;
            parameters[8].Value = productInfo.Points;
            parameters[9].Value = productInfo.ImageUrl;
            parameters[10].Value = productInfo.ThumbnailUrl1;
            parameters[11].Value = productInfo.ProductId;
            return new CommandInfo(strSql.ToString(),
                                    parameters, EffentNextType.ExcuteEffectRows);
        }
        #endregion

        #region 商品信息同步
        private List<CommandInfo> GeneratePMSSKUs(Model.Shop.Products.ProductInfo productInfo, SqlTransaction transaction)
        {
            Dictionary<long, long> specValues = new Dictionary<long, long>();   //Key:ValueId , Value:specId
            List<CommandInfo> list = new List<CommandInfo>();
            if (productInfo.SkuInfos == null)
            {
                return list;
            }
            foreach (Model.Shop.Products.SKUInfo skuInfo in productInfo.SkuInfos)
            {
                StringBuilder strSql = new StringBuilder();
                strSql.Append("SET IDENTITY_INSERT PMS_SKUs ON ");
                strSql.Append(" delete from PMS_SKUs where SkuId=@SkuId ");
                strSql.Append(" INSERT INTO PMS_SKUs(");
                strSql.Append("SkuId,ProductId,SKU,Weight,Stock,AlertStock,CostPrice,SalePrice,Upselling)");
                strSql.Append(" VALUES (");
                strSql.Append("@SkuId,@ProductId,@SKU,@Weight,@Stock,@AlertStock,@CostPrice,@SalePrice,@Upselling)");
                strSql.Append(";SET IDENTITY_INSERT PMS_SKUs OFF");
                SqlParameter[] parameters = {
                                                new SqlParameter("@ProductId", SqlDbType.BigInt, 8),
                                                new SqlParameter("@SKU", SqlDbType.NVarChar, 50),
                                                new SqlParameter("@Weight", SqlDbType.Int, 4),
                                                new SqlParameter("@Stock", SqlDbType.Int, 4),
                                                new SqlParameter("@AlertStock", SqlDbType.Int, 4),
                                                new SqlParameter("@CostPrice", SqlDbType.Money, 8),
                                                new SqlParameter("@SalePrice", SqlDbType.Money, 8),
                                                new SqlParameter("@Upselling", SqlDbType.Bit, 1),
                                                new SqlParameter("@SkuId", SqlDbType.BigInt, 8)
                                            };
                parameters[0].Value = productInfo.ProductId;
                parameters[1].Value = skuInfo.SKU;
                parameters[2].Value = skuInfo.Weight;
                parameters[3].Value = skuInfo.Stock;
                parameters[4].Value = skuInfo.AlertStock;
                parameters[5].Value = skuInfo.CostPrice;
                parameters[6].Value = skuInfo.SalePrice;
                parameters[7].Value = skuInfo.Upselling;
                parameters[8].Value = skuInfo.SkuId;

                list.Add(new CommandInfo(strSql.ToString(),
                                         parameters, EffentNextType.ExcuteEffectRows));
                if (skuInfo.SkuItems != null)
                {
                    foreach (Model.Shop.Products.SKUItem skuItem in skuInfo.SkuItems)
                    {
                        if (!specValues.ContainsKey(skuItem.ValueId))
                        {
                            object result = DBHelper.DefaultDBHelper.GetSingle4Trans(GenerateSKUItems(skuItem, productInfo), transaction);
                            long specId = Globals.SafeLong(result.ToString(), -1);

                            specValues.Add(skuItem.ValueId, specId);
                        }

                        strSql = new StringBuilder();
                        strSql.Append("delete from PMS_SKURelation where SkuId=@SkuId and SpecId=@SpecId and ProductId=@ProductId;");
                        strSql.Append("INSERT INTO PMS_SKURelation(");
                        strSql.Append("SkuId,SpecId,ProductId)");
                        strSql.Append(" VALUES (");
                        strSql.Append("@SkuId,@SpecId,@ProductId)");
                        parameters = new[]{
                          new SqlParameter("@SkuId", SqlDbType.BigInt,8), //输入主键
                            new SqlParameter("@SpecId", SqlDbType.BigInt,8),
                            new SqlParameter("@ProductId", SqlDbType.BigInt,8)
                        };
                        parameters[0].Value = skuInfo.SkuId;
                        parameters[1].Value = specValues[skuItem.ValueId];
                        parameters[2].Value = productInfo.ProductId;

                        list.Add(new CommandInfo(strSql.ToString(),
                                             parameters, EffentNextType.ExcuteEffectRows));
                    }
                }

            }
            return list;
        }

        private List<CommandInfo> SaveProductPMSCategories(Model.Shop.Products.ProductInfo productInfo)
        {
            List<CommandInfo> list = new List<CommandInfo>();
            if (productInfo.Product_Categories == null)
            {
                return list;
            }
            foreach (string productCategory in productInfo.Product_Categories)
            {
                if (!string.IsNullOrWhiteSpace(productCategory))
                {
                    string[] categoryArray = productCategory.Split(new char[] { '_' }, StringSplitOptions.RemoveEmptyEntries);
                    int categoryId = Globals.SafeInt(categoryArray[0], 0);
                    list.Add(GeneratePaoductPMSCategoriesOne(categoryId, productInfo.ProductId, categoryArray[1]));
                }
            }
            return list;
        }

        private CommandInfo GeneratePaoductPMSCategoriesOne(int categoriesId, long productId, string path)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("delete from PMS_ProductCategories where ProductId=@ProductId and CategoryId=@CategoryId;");
            strSql.Append("INSERT INTO PMS_ProductCategories ( CategoryId, ProductId,CategoryPath ) ");
            strSql.Append(" VALUES (");
            strSql.Append("@CategoryId,@ProductId,@CategoryPath)");
            SqlParameter[] parameters = {
                    new SqlParameter("@ProductId", SqlDbType.BigInt, 8),
                    new SqlParameter("@CategoryId", SqlDbType.Int, 4),
                    new SqlParameter("@CategoryPath", SqlDbType.NVarChar)
                                        };
            parameters[0].Value = productId;
            parameters[1].Value = categoriesId;
            parameters[2].Value = path;
            return new CommandInfo(strSql.ToString(),
                                    parameters, EffentNextType.ExcuteEffectRows);
        }

        private List<CommandInfo> GeneratePMSImages(Model.Shop.Products.ProductInfo productInfo)
        {
            List<CommandInfo> list = new List<CommandInfo>();
            if (productInfo.ProductImages == null)
            {
                return list;
            }
            foreach (Model.Shop.Products.ProductImage productImage in productInfo.ProductImages)
            {
                StringBuilder strSql = new StringBuilder();
                strSql.Append("SET IDENTITY_INSERT PMS_ProductImages ON ");
                strSql.Append(" delete from PMS_ProductImages where ProductId=@ProductId and ProductImageId=@ProductImageId ");
                strSql.Append(" INSERT INTO PMS_ProductImages(");
                strSql.Append("ProductImageId,ProductId,ImageUrl,ThumbnailUrl1,ThumbnailUrl2,ThumbnailUrl3,ThumbnailUrl4,ThumbnailUrl5,ThumbnailUrl6,ThumbnailUrl7,ThumbnailUrl8)");
                strSql.Append(" VALUES (");
                strSql.Append("@ProductImageId,@ProductId,@ImageUrl,@ThumbnailUrl1,@ThumbnailUrl2,@ThumbnailUrl3,@ThumbnailUrl4,@ThumbnailUrl5,@ThumbnailUrl6,@ThumbnailUrl7,@ThumbnailUrl8)");
                strSql.Append(";SET IDENTITY_INSERT PMS_ProductImages OFF");
                SqlParameter[] parameters = {
                    new SqlParameter("@ProductId", SqlDbType.BigInt,8),
                    new SqlParameter("@ImageUrl", SqlDbType.NVarChar,255),
                    new SqlParameter("@ThumbnailUrl1", SqlDbType.NVarChar,255),
                    new SqlParameter("@ThumbnailUrl2", SqlDbType.NVarChar,255),
                    new SqlParameter("@ThumbnailUrl3", SqlDbType.NVarChar,255),
                    new SqlParameter("@ThumbnailUrl4", SqlDbType.NVarChar,255),
                    new SqlParameter("@ThumbnailUrl5", SqlDbType.NVarChar,255),
                    new SqlParameter("@ThumbnailUrl6", SqlDbType.NVarChar,255),
                    new SqlParameter("@ThumbnailUrl7", SqlDbType.NVarChar,255),
                    new SqlParameter("@ThumbnailUrl8", SqlDbType.NVarChar,255),
                    new SqlParameter("@ProductImageId", SqlDbType.BigInt,8)};
                parameters[0].Value = productInfo.ProductId;    //产品主键
                parameters[1].Value = productImage.ImageUrl;
                parameters[2].Value = productImage.ThumbnailUrl1;
                parameters[3].Value = productImage.ThumbnailUrl2;
                parameters[4].Value = productImage.ThumbnailUrl3;
                parameters[5].Value = productImage.ThumbnailUrl4;
                parameters[6].Value = productImage.ThumbnailUrl5;
                parameters[7].Value = productImage.ThumbnailUrl6;
                parameters[8].Value = productImage.ThumbnailUrl7;
                parameters[9].Value = productImage.ThumbnailUrl8;
                parameters[10].Value = productImage.ProductImageId;

                list.Add(new CommandInfo(strSql.ToString(),
                                         parameters, EffentNextType.ExcuteEffectRows));
            }
            return list;
        }
        #endregion

        /// <summary>
        /// 增加一条品牌数据
        /// </summary>
        private CommandInfo AddBrand(Model.Shop.Products.ProductInfo productInfo)
        {
            Model.Shop.Products.BrandInfo model = productInfo.BrandInfo;
            StringBuilder strSql = new StringBuilder();
            strSql.Append("SET IDENTITY_INSERT PMS_Brands ON ");
            strSql.Append(" delete from PMS_Brands where BrandId=@BrandId ");
            strSql.Append(" insert into PMS_Brands(");
            strSql.Append("BrandId,BrandName,BrandSpell,Meta_Description,Meta_Keywords,Logo,CompanyUrl,Description,DisplaySequence,Theme)");
            strSql.Append(" values (");
            strSql.Append("@BrandId,@BrandName,@BrandSpell,@Meta_Description,@Meta_Keywords,@Logo,@CompanyUrl,@Description,@DisplaySequence,@Theme)");
            strSql.Append(";SET IDENTITY_INSERT PMS_Brands OFF");
            SqlParameter[] parameters = {
                    new SqlParameter("@BrandId", SqlDbType.Int,4),
                    new SqlParameter("@BrandName", SqlDbType.NVarChar,50),
                    new SqlParameter("@BrandSpell", SqlDbType.NVarChar,200),
                    new SqlParameter("@Meta_Description", SqlDbType.NVarChar,1000),
                    new SqlParameter("@Meta_Keywords", SqlDbType.NVarChar,1000),
                    new SqlParameter("@Logo", SqlDbType.NVarChar,255),
                    new SqlParameter("@CompanyUrl", SqlDbType.NVarChar,255),
                    new SqlParameter("@Description", SqlDbType.NText),
                    new SqlParameter("@DisplaySequence", SqlDbType.Int,4),
                    new SqlParameter("@Theme", SqlDbType.NVarChar,100)};
            parameters[0].Value = model.BrandId;
            parameters[1].Value = model.BrandName;
            parameters[2].Value = model.BrandSpell;
            parameters[3].Value = model.Meta_Description;
            parameters[4].Value = model.Meta_Keywords;
            parameters[5].Value = model.Logo;
            parameters[6].Value = model.CompanyUrl;
            parameters[7].Value = model.Description;
            parameters[8].Value = model.DisplaySequence;
            parameters[9].Value = model.Theme;

            return new CommandInfo(strSql.ToString(),
                                    parameters, EffentNextType.ExcuteEffectRows);
        }

        /// <summary>
        /// 添加商品分类信息
        /// </summary>
        /// <param name="productInfo"></param>
        /// <returns></returns>
        private List<CommandInfo> AddCategoryInfo(Model.Shop.Products.ProductInfo productInfo)
        {
            List<CommandInfo> list = new List<CommandInfo>();
            if (productInfo.CategoryInfo == null)
            {
                return list;
            }
            foreach (Model.Shop.Products.CategoryInfo model in productInfo.CategoryInfo)
            {
                StringBuilder strSql = new StringBuilder();
                strSql.Append("SET IDENTITY_INSERT PMS_Categories ON ");
                strSql.Append(" delete from PMS_Categories where CategoryId=@CategoryId ");
                strSql.Append(" insert into PMS_Categories(");
                strSql.Append("CategoryId,DisplaySequence,Name,Meta_Title,Meta_Description,Meta_Keywords,Description,ParentCategoryId,Depth,Path,RewriteName,SKUPrefix,AssociatedProductType,ImageUrl,Notes1,Notes2,Notes3,Notes4,Notes5,Theme,HasChildren,SeoUrl,SeoImageAlt,SeoImageTitle,Status)");
                strSql.Append(" values (");
                strSql.Append("@CategoryId,@DisplaySequence,@Name,@Meta_Title,@Meta_Description,@Meta_Keywords,@Description,@ParentCategoryId,@Depth,@Path,@RewriteName,@SKUPrefix,@AssociatedProductType,@ImageUrl,@Notes1,@Notes2,@Notes3,@Notes4,@Notes5,@Theme,@HasChildren,@SeoUrl,@SeoImageAlt,@SeoImageTitle,@Status)");
                strSql.Append(";SET IDENTITY_INSERT PMS_Categories OFF");
                SqlParameter[] parameters = {
                    new SqlParameter("@CategoryId", SqlDbType.Int,4),
                    new SqlParameter("@DisplaySequence", SqlDbType.Int,4),
                    new SqlParameter("@Name", SqlDbType.NVarChar,100),
                    new SqlParameter("@Meta_Title", SqlDbType.NVarChar,1000),
                    new SqlParameter("@Meta_Description", SqlDbType.NVarChar,1000),
                    new SqlParameter("@Meta_Keywords", SqlDbType.NVarChar,1000),
                    new SqlParameter("@Description", SqlDbType.NVarChar,1000),
                    new SqlParameter("@ParentCategoryId", SqlDbType.Int,4),
                    new SqlParameter("@Depth", SqlDbType.Int,4),
                    new SqlParameter("@Path", SqlDbType.VarChar,4000),
                    new SqlParameter("@RewriteName", SqlDbType.NVarChar,50),
                    new SqlParameter("@SKUPrefix", SqlDbType.NVarChar,10),
                    new SqlParameter("@AssociatedProductType", SqlDbType.Int,4),
                    new SqlParameter("@ImageUrl", SqlDbType.NVarChar,300),
                    new SqlParameter("@Notes1", SqlDbType.NText),
                    new SqlParameter("@Notes2", SqlDbType.NText),
                    new SqlParameter("@Notes3", SqlDbType.NText),
                    new SqlParameter("@Notes4", SqlDbType.NText),
                    new SqlParameter("@Notes5", SqlDbType.NText),
                    new SqlParameter("@Theme", SqlDbType.VarChar,100),
                    new SqlParameter("@HasChildren", SqlDbType.Bit,1),
                    new SqlParameter("@SeoUrl", SqlDbType.NVarChar,300),
                    new SqlParameter("@SeoImageAlt", SqlDbType.NVarChar,300),
                    new SqlParameter("@SeoImageTitle", SqlDbType.NVarChar,300),
                    new SqlParameter("@Status", SqlDbType.Bit,1)};
                parameters[0].Value = model.CategoryId;
                parameters[1].Value = model.DisplaySequence;
                parameters[2].Value = model.Name;
                parameters[3].Value = model.Meta_Title;
                parameters[4].Value = model.Meta_Description;
                parameters[5].Value = model.Meta_Keywords;
                parameters[6].Value = model.Description;
                parameters[7].Value = model.ParentCategoryId;
                parameters[8].Value = model.Depth;
                parameters[9].Value = model.Path;
                parameters[10].Value = model.RewriteName;
                parameters[11].Value = model.SKUPrefix;
                parameters[12].Value = model.AssociatedProductType;
                parameters[13].Value = model.ImageUrl;
                parameters[14].Value = model.Notes1;
                parameters[15].Value = model.Notes2;
                parameters[16].Value = model.Notes3;
                parameters[17].Value = model.Notes4;
                parameters[18].Value = model.Notes5;
                parameters[19].Value = model.Theme;
                parameters[20].Value = model.HasChildren;
                parameters[21].Value = model.SeoUrl;
                parameters[22].Value = model.SeoImageAlt;
                parameters[23].Value = model.SeoImageTitle;
                parameters[24].Value = model.Status;

                list.Add(new CommandInfo(strSql.ToString(),
                                         parameters, EffentNextType.ExcuteEffectRows));
            }
            return list;
        }

        public bool SubUpdateList(string depotIds, Model.Shop.Products.ProductInfo productInfo)
        {
            try
            {
                string[] ids = depotIds.Split(',');
                foreach (var depotId in ids)
                {
                    string tabName = "Shop_DepotProduct_" + depotId;
                    string tabName2 = "Shop_DepotProSKUs_" + depotId;
                    if (!TabExists(tabName2)|| !TabExists(tabName))
                    {
                        continue;
                    }
                    StringBuilder strSql = new StringBuilder();
                    strSql.Append($"update {tabName} set ");
                    strSql.Append("TypeId=@TypeId,");
                    strSql.Append("BrandId=@BrandId,");
                    strSql.Append("ProductName=@ProductName,");
                    strSql.Append("SaleStatus=@SaleStatus,");
                    strSql.Append("SaleCounts=@SaleCounts,");
                    strSql.Append("MarketPrice=@MarketPrice,");
                    strSql.Append("LowestSalePrice=@LowestSalePrice,");
                    strSql.Append("HasSKU=@HasSKU,");
                    strSql.Append("DisplaySequence=@DisplaySequence,");
                    strSql.Append("ImageUrl=@ImageUrl,");
                    strSql.Append("SalesType=@SalesType,");
                    strSql.Append("ThumbnailUrl1=@ThumbnailUrl1");
                    strSql.Append(" where ProductId=@ProductId ");
                    SqlParameter[] parameters = {
                    new SqlParameter("@TypeId", SqlDbType.Int,4),
                    new SqlParameter("@BrandId", SqlDbType.Int,4),
                    new SqlParameter("@ProductName", SqlDbType.NVarChar,200),
                    new SqlParameter("@SaleStatus", SqlDbType.Int,4),
                    new SqlParameter("@SaleCounts", SqlDbType.Int,4),
                    new SqlParameter("@MarketPrice", SqlDbType.Money,8),
                    new SqlParameter("@LowestSalePrice", SqlDbType.Money,8),
                    new SqlParameter("@HasSKU", SqlDbType.Bit,1),
                    new SqlParameter("@DisplaySequence", SqlDbType.Int,4),
                    new SqlParameter("@ImageUrl", SqlDbType.NVarChar,255),
                    new SqlParameter("@SalesType", SqlDbType.SmallInt,2),
                    new SqlParameter("@ThumbnailUrl1", SqlDbType.NVarChar,255),
                    new SqlParameter("@ProductId", SqlDbType.BigInt,8)};

                    parameters[0].Value = productInfo.TypeId;
                    parameters[1].Value = productInfo.BrandId;
                    parameters[2].Value = productInfo.ProductName;
                    parameters[3].Value = productInfo.SaleStatus;
                    parameters[4].Value = productInfo.SaleCounts;
                    parameters[5].Value = productInfo.MarketPrice;
                    parameters[6].Value = productInfo.LowestSalePrice;
                    parameters[7].Value = productInfo.HasSKU;
                    parameters[8].Value = productInfo.DisplaySequence;
                    parameters[9].Value = productInfo.ImageUrl;
                    parameters[10].Value = productInfo.SalesType;
                    parameters[11].Value = productInfo.ThumbnailUrl1;
                    parameters[12].Value = productInfo.ProductId;

                    DBHelper.DefaultDBHelper.ExecuteSql(strSql.ToString(), parameters);

                    foreach (var skuInfo in productInfo.SkuInfos)
                    {
                        StringBuilder strSql2 = new StringBuilder();
                 
                        strSql2.Append($"update {tabName2} set ");
                        strSql2.Append("ProductId=@ProductId,");
                        strSql2.Append("Weight=@Weight,");
                        strSql2.Append("AlertStock=@AlertStock,");
                        strSql2.Append("CostPrice=@CostPrice,");
                        strSql2.Append("SalePrice=@SalePrice,");
                        strSql2.Append("Upselling=@Upselling");
                        strSql2.Append(" where SKU=@SKU ");
                        SqlParameter[] parameters2 =
                        {
                            new SqlParameter("@ProductId", SqlDbType.BigInt, 8),
                            new SqlParameter("@Weight", SqlDbType.Int, 4),
                            new SqlParameter("@AlertStock", SqlDbType.Int, 4),
                            new SqlParameter("@CostPrice", SqlDbType.Money, 8),
                            new SqlParameter("@SalePrice", SqlDbType.Money, 8),
                            new SqlParameter("@Upselling", SqlDbType.Bit, 1),
                            new SqlParameter("@SKU", SqlDbType.NVarChar, 50)
                        };
                        parameters2[0].Value = productInfo.ProductId;
                        parameters2[1].Value = skuInfo.Weight;
                        parameters2[2].Value = skuInfo.AlertStock;
                        parameters2[3].Value = skuInfo.CostPrice;
                        parameters2[4].Value = skuInfo.SalePrice;
                        parameters2[5].Value = skuInfo.Upselling;
                        parameters2[6].Value = skuInfo.SKU;
                        DBHelper.DefaultDBHelper.ExecuteSql(strSql2.ToString(), parameters2);
                    }
                }
            }
            catch (Exception ex)
            {
                Log.LogHelper.AddTextLog("修改分仓库存同步信息", ex.Message + "----" + ex.StackTrace);
            }
            return true;
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
    }
}