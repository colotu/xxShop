using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using YSWL.MALL.BLL.Shop.Products;
using YSWL.MALL.BLL.Shop.Service;
using YSWL.Common;
using YSWL.MALL.DALFactory;
using YSWL.DBUtility;
using YSWL.MALL.IDAL.Shop.DisDepot;

namespace YSWL.MALL.BLL.Shop.DisDepot
{
    public class DepotProSKUsService
    {
        private static readonly IDepotProSKUs service = DAShopDisDepot.CreateDepotProSKUs();
        private static readonly string ProductKey = "Mall.ALL";
        private static DataCacheCore dataCache = new DataCacheCore(new CacheOption
        {
            CacheType = SAASInfo.GetSystemBoolValue("RedisCacheUse") ? CacheType.Redis : CacheType.IIS,
            ReadWriteHosts = SAASInfo.GetSystemValue("RedisCacheReadWriteHosts"),
            ReadOnlyHosts = SAASInfo.GetSystemValue("RedisCacheReadOnlyHosts"),
            CancelProductKey=true,
            DefaultDb = 2
        });
        /// <summary>
        /// 同步库存
        /// </summary>
        /// <param name="proSKUsList"></param>
        /// <param name="depotId"></param>
        /// <returns></returns>
        public static bool SyncStock(List<YSWL.MALL.Model.Shop.DisDepot.DepotProSKUs> proSKUsList, int depotId)
        {
            List<YSWL.MALL.Model.Shop.DisDepot.DepotProduct> depotProductList=new List<Model.Shop.DisDepot.DepotProduct>();

            YSWL.MALL.BLL.Shop.Products.ProductInfo productBll=new ProductInfo();
            YSWL.MALL.BLL.Shop.Products.SKUInfo skuBll = new BLL.Shop.Products.SKUInfo();

            //填充数据
            YSWL.MALL.Model.Shop.Products.SKUInfo skuInfo = null;
            YSWL.MALL.Model.Shop.Products.ProductInfo productInfo = null;
            YSWL.MALL.Model.Shop.DisDepot.DepotProduct depotProduct = null;

            bool IsMultiDepot = YSWL.MALL.BLL.Shop.Service.CommonHelper.OpenMultiDepot();
            #region 处理未开启多分仓的库存
            if (!IsMultiDepot)
            {
                foreach (var item in proSKUsList)
                {
                    skuInfo = skuBll.GetModelBySKU(item.SKU);
                    if (skuInfo == null)
                    {
                        continue;
                    }
                    else
                    {
                        //获取此SKU 未付款且为线上支付的订单库存
                        int unStock = GetUnSyncStock(item.SKU);
                        item.Stock = item.Stock - unStock;
                        YSWL.MALL.BLL.Shop.Products.ProductManage.ModifyStock(item.SKU, item.Stock);  // 暂未做失败处理
                                                                                                 // StockHelper - StockCache - SKU_" + sku + "_DepotId_" + depotId + "_SuppID_
                        //修改库存
                        string key = string.Format("StockHelper_StockCache_SKU_{0}_DepotId_{1}_SuppID_0-{2}",item.SKU, depotId, ProductKey);
                        dataCache.SetCache(key,item.Stock);
                    }  
                }
                //清空远程缓存
                //CommonHelper.ClearCache();
                return true;
            }

            #endregion

            #region 处理开启多分仓的库存数据
            foreach (var item in proSKUsList)
            {
                skuInfo= skuBll.GetModelBySKU(item.SKU);
                if (skuInfo == null)
                {
                    continue;
                }
                else
                {
                    //获取此SKU 未付款且为线上支付的订单库存
                    int unStock = GetUnSyncStock(item.SKU);
                    item.Stock = item.Stock - unStock;
                    //填充 DepotProSKUs数据
                    item.ProductId = skuInfo.ProductId;
                    item.AlertStock = skuInfo.AlertStock;
                    item.CostPrice = skuInfo.CostPrice.HasValue?skuInfo.CostPrice.Value:0;
                    item.SalePrice = skuInfo.SalePrice;
                    item.Upselling = skuInfo.Upselling;
                    item.Weight = skuInfo.Weight.HasValue?skuInfo.Weight.Value:0;
                    depotProduct = depotProductList.Find(c => c.ProductId == skuInfo.ProductId);
                    if (depotProduct==null)
                    {
                        productInfo = productBll.GetModelByCache(skuInfo.ProductId);
                        if (productInfo != null)
                        {
                            depotProduct=new Model.Shop.DisDepot.DepotProduct();
                            depotProduct.HasSKU = productInfo.HasSKU;
                            depotProduct.ImageUrl = productInfo.ImageUrl;
                            depotProduct.LowestSalePrice = productInfo.LowestSalePrice;
                            depotProduct.MarketPrice = productInfo.MarketPrice;
                            depotProduct.ProductId = productInfo.ProductId;
                            depotProduct.ProductName = productInfo.ProductName;
                            depotProduct.SaleCounts = productInfo.SaleCounts;
                            depotProduct.SaleStatus = productInfo.SaleStatus;
                            depotProduct.Stock =item.Stock;
                            depotProduct.SalesType = productInfo.SalesType;
                        }
                    }
                    else
                    {
                        depotProduct.Stock = depotProduct.Stock+item.Stock;
                    }
                    depotProductList.Add(depotProduct);

                    //修改库存
                    string key = string.Format("StockHelper_StockCache_SKU_{0}_DepotId_{1}_SuppID_0-{2}", item.SKU, depotId, ProductKey);
                    dataCache.SetCache(key, item.Stock);
                }
            }
            //清空远程缓存
            //CommonHelper.ClearCache();
            return service.SyncStock(proSKUsList,depotProductList, depotId);
            #endregion
        }
        /// <summary>
        ///  检测SKU库存
        /// </summary>
        /// <param name="SKU"></param>
        /// <param name="count"></param>
        /// <param name="depotId"></param>
        /// <returns></returns>
        public static bool CheckStock(string SKU, int count, int depotId)
        {
            return service.CheckStock(SKU, count, depotId);
        }
        /// <summary>
        /// 获取未同步至OMS的SKU 库存
        /// </summary>
        /// <param name="sku"></param>
        /// <returns></returns>
        public static int GetUnSyncStock(string sku)
        {
            return service.GetUnSyncStock(sku);
        }
    }
}
