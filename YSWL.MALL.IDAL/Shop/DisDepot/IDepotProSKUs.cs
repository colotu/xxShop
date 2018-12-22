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
namespace YSWL.MALL.IDAL.Shop.DisDepot
{
    /// <summary>
    /// 接口层DepotProSKUs
    /// </summary>
    public interface IDepotProSKUs
    {
        #region  成员方法
        /// <summary>
        /// 是否存在该记录
        /// </summary>
        bool Exists(string SKU);
        /// <summary>
        /// 增加一条数据
        /// </summary>
        bool Add(YSWL.MALL.Model.Shop.DisDepot.DepotProSKUs model);
        /// <summary>
        /// 更新一条数据
        /// </summary>
        bool Update(YSWL.MALL.Model.Shop.DisDepot.DepotProSKUs model);
        /// <summary>
        /// 删除一条数据
        /// </summary>
        bool Delete(string SKU);
        bool DeleteList(string SKUlist);
        /// <summary>
        /// 得到一个对象实体
        /// </summary>
        YSWL.MALL.Model.Shop.DisDepot.DepotProSKUs GetModel(string SKU);
        YSWL.MALL.Model.Shop.DisDepot.DepotProSKUs DataRowToModel(DataRow row);
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

        #region  MethodEx

        bool SyncStock(List<YSWL.MALL.Model.Shop.DisDepot.DepotProSKUs> proSKUsList, List<YSWL.MALL.Model.Shop.DisDepot.DepotProduct> depotProductList, int depotId);

        bool CheckStock(string SKU, int count, int depotId);

        DataSet GetProductSkuInfo(long productId, int depotId);
          /// <summary>
        /// 分页获取数据列表
        /// </summary>
        /// <param name="Top"></param>
        /// <param name="depotId">仓库id</param>
        /// <param name="strWhere"></param>
        /// <param name="orderby"></param>
        /// <returns></returns>
        DataSet GetList(int Top, int depotId, string strWhere, string orderby);
       
        DataSet GetListByPage(int depotId, string strWhere, string orderby, int startIndex, int endIndex);

        int GetRecordCount(int depotId, string strWhere);

        int GetStockBySKU(string SKU, int depotId, bool IsOpenAS,int ownerId);


        int GetUnSyncStock(string sku);

         /// <summary>
        /// 获取未添加sku 数据 总条数
        /// </summary>
        /// <param name="depotId"></param>
        /// <param name="categoryId"></param>
        /// <param name="pName"></param>
        /// <returns></returns>
        int GetNoAddSKURecordCount(int depotId, int categoryId, string pName);
         /// <summary>
        /// 获取未添加sku 数据列表
        /// </summary>
        /// <param name="depotId"></param>
        /// <param name="categoryId"></param>
        /// <param name="pName"></param>
        /// <param name="startIndex"></param>
        /// <param name="endIndex"></param>
        /// <returns></returns>
        DataSet GetNoAddSKUList(int depotId, int categoryId, string pName, int startIndex, int endIndex);
        bool AddProduct(int depotId, YSWL.MALL.Model.Shop.Products.SKUInfo skuInfo, YSWL.MALL.Model.Shop.Products.ProductInfo prodInfo);
        /// <summary>
        /// 检测sku是否存在该记录
        /// </summary>
        bool SkuExists(int depotId, string sku);
        bool DeleteProduct(int depotId, long productId, string sku);
           /// <summary>
        /// 得到一个对象实体
        /// </summary>
        YSWL.MALL.Model.Shop.DisDepot.DepotProSKUs GetModel(int depotId, string sku);
        bool UpdateStockNum(int depotId, string sku, int stock);

        bool UpdateSkuStatus(int depotId, string sku, bool IsUp,int status=1);
        bool SyncProdcut(List<YSWL.MALL.Model.Shop.DisDepot.Depot> depotList ,YSWL.MALL.Model.Shop.Products.ProductInfo  prodModel, List<Model.Shop.Products.SKUInfo> skuList);
        /// <summary>
        /// 获取分仓商品sku列表 （分页）
        /// </summary>
        /// <param name="depotId">仓库Id</param>
        /// <param name="keyw">商品名称或编码</param>
        /// <param name="startIndex"></param>
        /// <param name="endIndex"></param>
        /// <param name="orderby">排序方式</param>
        /// <returns></returns>
        DataSet GetSKUListByPage(int depotId, string keyw, int startIndex, int endIndex, string orderby);
        /// <summary>
        ///  获取记录总数
        /// </summary>
        /// <param name="depotId">仓库id</param>
        /// <param name="keyw">商品名称或编码</param>
        /// <returns></returns>
        int GetSKURecordCount(int depotId, string keyw);
            #endregion  MethodEx

    } 
}
