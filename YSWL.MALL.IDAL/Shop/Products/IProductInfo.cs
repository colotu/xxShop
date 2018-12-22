/*----------------------------------------------------------------
// Copyright (C) 2012 云商未来 版权所有。
//
// 文件名：IProducts.cs
// 文件功能描述：
// 
// 创建标识： [Ben]  2012/06/11 20:36:27
// 修改标识：
// 修改描述：
//
// 修改标识：
// 修改描述：
//----------------------------------------------------------------*/
using System;
using System.Data;
using System.Collections.Generic;
using YSWL.MALL.Model.Shop.Products;

namespace YSWL.MALL.IDAL.Shop.Products
{
    /// <summary>
    /// 接口层ProductInfo
    /// </summary>
    public interface IProductInfo
    {
        #region  成员方法
        /// <summary>
        /// 是否存在该记录
        /// </summary>
        bool Exists(long ProductId);
        /// <summary>
        /// 增加一条数据
        /// </summary>
        long Add(YSWL.MALL.Model.Shop.Products.ProductInfo model);
        /// <summary>
        /// 更新一条数据
        /// </summary>
        bool Update(YSWL.MALL.Model.Shop.Products.ProductInfo model);
        /// <summary>
        /// 删除一条数据
        /// </summary>
        bool Delete(long ProductId);
        bool DeleteList(string ProductIdlist);
        /// <summary>
        /// 得到一个对象实体
        /// </summary>
        YSWL.MALL.Model.Shop.Products.ProductInfo GetModel(long ProductId);
        YSWL.MALL.Model.Shop.Products.ProductInfo DataRowToModel(DataRow row);
        /// <summary>
        /// 获得数据列表
        /// </summary>
        DataSet GetList(string strWhere);
        /// <summary>
        /// 获得前几行数据
        /// </summary>
        DataSet GetList(int Top, string strWhere, string filedOrder);
        int GetRecordCount(string strWhere);
        DataSet GetListByPage(string strWhere, string orderby, int startIndex, int endIndex);
        /// <summary>
        /// 根据分页获得数据列表
        /// </summary>
        //DataSet GetList(int PageSize,int PageIndex,string strWhere);
        #endregion  成员方法

        #region 新增的成员方法
        /// <summary>
        /// 批量处理状态
        /// </summary>
        /// <param name="IDlist"></param>
        /// <param name="strSetValue"></param>
        /// <returns></returns>
        bool UpdateList(string IDlist, string strSetValue);
        bool SubUpdateList(string depotIds,string IDlist,string strWhere);
        bool UpdateProductName(long productId, string strSetValue);

        DataSet GetListByCategoryIdSaleStatus(string strWhere, int SupplierId);

        DataSet GetListByExport(int SaleStatus, string ProductName, int CategoryId, string SKU, int BrandId);

        DataSet SearchProducts(int cateId, Model.Shop.Products.ProductSearch model );
        DataSet GetProductListByCategoryId(int? categoryId, string strWhere, string orderBy, int startIndex, int endIndex, int SupplierId, out int dataCount);


        DataSet GetProductListByCategoryIdEx(int? categoryId, string strWhere, string orderBy, int startIndex, int endIndex, int SupplierId, out int dataCount);

        DataSet GetProductListInfo(string strWhere, string orderBy, int startIndex, int endIndex, out int dataCount, long productId);
        /// <summary>
        /// 商品推荐列表信息
        /// </summary>
        DataSet GetProductCommendListInfo(string strWhere, string orderBy, int startIndex, int endIndex, out int dataCount, long productId,int modeType);

        DataSet GetProductListInfo(string strProductIds, int SupplierId);

        string GetProductName(long productId);

        bool ExistsBrands(int BrandId);
        /// <summary>
        /// 得到商品表的结构
        /// </summary>
        DataSet GetTableSchema();
        DataSet GetTableSchemaEx();
        /// <summary>
        /// 根据需要的字段获得相应的数据
        /// </summary>
        DataSet GetList(string strWhere, string DataField, int SupplierId);
        
        #endregion

        DataSet GetProductInfo(string strWhere);

        DataSet DeleteProducts(string Ids, out int Result);


        DataSet GetRecycleList(string strWhere, int SupplierId);
        /// <summary>
        /// 还原所有商品
        /// </summary>
        /// <returns></returns>
        bool RevertAll(int SupplierId);
        /// <summary>
        /// 更新商品状态
        /// </summary>
        /// <param name="productId"></param>
        /// <param name="SaleStatus"></param>
        /// <returns></returns>
        bool UpdateStatus(long productId, int SaleStatus, int SupplierId);

        bool ChangeProductsCategory(string productIds, int categoryId);

        long StockNum(long productId);

        bool UpdateMarketPrice(long productId, decimal price, int SupplierId);

        bool UpdateLowestSalePrice(long productId, decimal price, int SupplierId);

        DataSet GetProductRecList(ProductRecType type, int categoryId, int top, int SupplierId);
        int GetProductRecCount(ProductRecType type, int categoryId, int SupplierId);
        DataSet GetProductRanList(int top, int SupplierId);

        DataSet RelatedProductSource(long productId, int top);

        DataSet GetProductsListEx(int Cid, int BrandId, string attrValues, string priceRange,
                                  string mod, int startIndex, int endIndex, int SupplierId);

        int GetProductsCountEx(int Cid, int BrandId, string attrValues, string priceRange, int SupplierId);


        int MaxSequence();

        /// <summary>
        /// 根据类别地址 得到该类别下最大顺序值
        /// </summary>
        /// <param name="CategoryPath"></param>
        /// <returns></returns>
        int MaxSequence(string CategoryPath);

        int GetSearchCountEx(int Cid, int BrandId, List<String> keyWords, string priceRange, int SupplierId);

        DataSet GetSearchListEx(int Cid, int BrandId, List<String> keyWords, string priceRange,
                                 string mod, int startIndex, int endIndex, int SupplierId);

        int GetProductNoRecCount(int categoryId, string pName, int modeType,int supplierId);
        DataSet GetProductNoRecList(int categoryId,int supplierId, string pName, int modeType, int startIdex, int endIndex);

        bool Exists(string productCode);

        DataSet GetProductsByCid(int cid);
      
        int GetProSalesCount();

        int GetGroupBuyCount();

        DataSet GetProSalesList(int startIndex, int endIndex);

        DataSet GetProSaleModel(int id);

        DataSet GetGroupBuyList(int startIndex, int endIndex);

        DataSet GetGroupBuyModel(int id);

        int GetProductStatus(long productId);

        /// <summary>
        /// 获取供应商商品数量
        /// </summary>
        /// <param name="Cid">分类</param>
        /// <param name="supplierId">供应商ID</param>
        /// <param name="keyword">关键词</param>
        /// <param name="priceRange">价格区间</param>
        /// <returns></returns>
        int GetSuppProductsCount(int Cid, int supplierId, string keyword, string priceRange);

        /// <summary>
        /// 根据条件获取供应商商品
        /// </summary>
        /// <param name="Cid">类别ID</param>
        /// <param name="supplierId">供应商id</param>
        /// <param name="keyword">关键词</param>
        /// <param name="priceRange">价格区间</param>
        /// <param name="orderby">排序</param>
        /// <param name="startIndex"></param>
        /// <param name="endIndex"></param>
        /// <returns></returns>
        DataSet GetSuppProductsList(int Cid, int supplierId, string keyword, string priceRange, string orderby, int startIndex, int endIndex);

        DataSet GetSuppProductsList(int top,int Cid, int supplierId, string orderby, string keyword, string priceRange);

        bool UpdateThumbnail(YSWL.MALL.Model.Shop.Products.ProductInfo model);

        DataSet GetListToReGen(string strWhere);

        /// <summary>
        /// 获取记录总数
        /// </summary>
        int GetProdRecordCount(string strWhere, int SupplierId);

        /// <summary>
        /// 商品数据分页列表
        /// </summary>
        /// <param name="strWhere"></param>
        /// <param name="orderby"></param>
        /// <param name="startIndex"></param>
        /// <param name="endIndex"></param>
        /// <returns></returns>
        DataSet GetProdListByPage(string strWhere, string orderby, int startIndex, int endIndex, int SupplierId);

        /// <summary>
        /// 获取商家推荐的商品列表
        /// </summary>
        /// <param name="supplierId"></param>
        /// <param name="strType"></param>
        /// <param name="orderby"></param>
        /// <returns></returns>
        DataSet GetSuppRecList(int supplierId, int strType, string orderby);

        DataSet GetProList(string strWhere);
        int GetCount(int cid, int regionId);
        DataSet GetProSalesList(int cid, int regionId, int startIndex, int endIndex, string orderby = "");
        DataSet GetTableHead();
        DataSet GetProductRanListByRec(ProductRecType type, int categoryId, int top,int SupplierId);

        /// <summary>
        /// 根据商家id获得是否存在该记录
        /// </summary>
        bool Exists(int supplierId);

        DataSet GetSearchListEx(int Cid, int BrandId, string keyWord, string priceRange,
                         string orderby, int startIndex, int endIndex, int SupplierId, int type, string strWhere);


        DataSet GetProductsListEx(int Cid, int BrandId, string attrValues, string priceRange,
                              string mod, int startIndex, int endIndex, int SupplierId,int type);

        DataSet GetProducts(int supplierId);
        /// <summary>
        /// 按品类统计商品数量
        /// </summary>
        /// <returns></returns>
        DataSet GetCategoriesCount();
        /// <summary>
        /// 获取限购数量
        /// </summary>
        int GetRestrictionCount(long productId);
        List<YSWL.MALL.Model.Shop.Products.ProductInfo> DataTableToList(DataTable dt);
        DataSet GetGiftsByCid(int Cid);
        DataSet GetProSalesList(int top);

        /// <summary>
        /// 更新一条数据
        /// </summary>
        long UpdatePV(long pId);

        DataSet GetProductsByCid(int Cid, bool storeIsInActivity);

       DataSet GetNoRuleProductList(string pName, string categoryId, int status, int ruleId, int startIndex, int endIndex);

        int GetNoRuleProductCount(string pName, string categoryId, int ruleId, int status = 1);

        DataSet GetPreProductList();

        DataSet  GetSalesRuleProductsList(int startIndex, int endIndex, string orderBy, int rankId);
        /// <summary>
        /// 获取参与批发规则的商品总数
        /// </summary>
        int GetSalesRuleProdCount(int rankId);
        /// <summary>
        /// 获取用户最近订购的商品
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="startDate"></param>
        /// <param name="endDate"></param>
        /// <param name="startIndex"></param>
        /// <param name="endIndex"></param>
        /// <returns></returns>
        DataSet GetOrderedProdList(int userId, DateTime startDate, DateTime endDate, int startIndex, int endIndex);
        /// <summary>
        /// 获取用户最近订购的商品总数
        /// </summary>
        int GetOrderedProdCount(int userId, DateTime startDate, DateTime endDate);

        DataSet GetListExport(string Ids, string DataField, int SupplierId);

        /// <summary>
        /// 根据分页获取推荐产品信息
        /// </summary>
        /// <param name="type">推荐类型</param>
        /// <param name="categoryId">分类id</param>
        /// <param name="orderby">排序方式</param>
        /// <param name="startIndex">开始索引</param>
        /// <param name="endIndex">结束索引</param>
        /// <returns></returns>
        DataSet GetProductRecListByPage(ProductRecType type, int categoryId, string orderby, int startIndex, int endIndex);
        /// <summary>
        /// 获取推荐产品信息记录总数
        /// </summary>
        /// <param name="type"></param>
        /// <param name="categoryId"></param>
        /// <returns></returns>
        int GetProductRecCount(ProductRecType type, int categoryId);
        /// <summary>
        /// 根据sku获取商品model
        /// </summary>
        /// <param name="sku"></param>
        /// <returns></returns>
        Model.Shop.Products.ProductInfo GetModelBySku(string sku);


		        decimal GetLowestSalePrice(long productId);

        /// <summary>
        /// 获取商家推荐的商品数据分页列表
        /// </summary>
        /// <param name="type"></param>
        /// <param name="supplierId"></param>
        /// <param name="startIndex"></param>
        /// <param name="endIndex"></param>
        /// <param name="orderby"></param>
        /// <returns></returns>
        DataSet GetSuppRecListByPage(int type, int supplierId, int startIndex, int endIndex, string orderby);

        #region saas app

        int GetSearchCountExApp(int Cid, int BrandId, List<String> keyWords, string priceRange, int SupplierId, int? SaleStatus = null);

        DataSet GetSearchListExApp(int Cid, int BrandId, List<String> keyWords, string priceRange,
                                 string mod, int startIndex, int endIndex, int SupplierId, int? SaleStatus = null);
        DataSet GetNoRuleProductListApp(string pName, string categoryId, string brandId, int status, int ruleId, int startIndex, int endIndex);
        #endregion
        /// <summary>
        /// 获取商品记录总数(-1全部/0下架/1上架)
        /// </summary>
        int GetProductCount(int status);
    }
}
