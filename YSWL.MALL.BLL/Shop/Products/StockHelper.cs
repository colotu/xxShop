using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using YSWL.Common;
using YSWL.MALL.DALFactory;
using YSWL.DBUtility;
using YSWL.MALL.IDAL.Shop.Products;
#pragma warning disable 612

namespace YSWL.MALL.BLL.Shop.Products
{
    public  class StockHelper
    {
        private static readonly YSWL.MALL.BLL.Shop.DisDepot.DepotProSKUs disSkUsBll = new YSWL.MALL.BLL.Shop.DisDepot.DepotProSKUs();
        private static readonly ISKUInfo skuDal = DAShopProducts.CreateSKUInfo();
        private static readonly SKUInfo skuInfoBll = new SKUInfo();
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
        /// 获取SKU 库存
        /// </summary>
        /// <param name="sku"></param>
        /// <param name="depotId"></param>
        /// <param name="supplierId"></param>
        /// <returns></returns>
        public static int GetSkuStock(string sku,int  depotId,int? supplierId)
        {
           //是否开启了警戒库存
            bool IsOpenAS = YSWL.MALL.BLL.SysManage.ConfigSystem.GetBoolValueByCache("Shop_OpenAlertStock");
            //是否开启多分仓库存
            bool IsMultiDepot = YSWL.MALL.BLL.Shop.Service.CommonHelper.OpenMultiDepot();
            if (IsMultiDepot && (!supplierId.HasValue||supplierId <= 0))  //开启了多仓对接，就需要对接仓库库存,且不是商家商品
            {
                return disSkUsBll.GetStockBySKU(sku, depotId, IsOpenAS,0);
            }
            else
            {
                return skuDal.GetStockBySKU(sku, IsOpenAS);
            }
        }
        /// <summary>
        /// 采用缓存库存
        /// </summary>
        /// <param name="sku"></param>
        /// <param name="regionId"></param>
        /// <param name="supplierId"></param>
        /// <returns></returns>
        public static int GetSkuStockByCache(string sku, int regionId, int? supplierId)
        {
            bool IsMultiDepot = YSWL.MALL.BLL.Shop.Service.CommonHelper.OpenMultiDepot();
            int depotId = 0;
            #region  判断是否开启多仓 且是平台自营商品
            if (IsMultiDepot && (!supplierId.HasValue||supplierId<=0))
            {
                depotId=YSWL.MALL.BLL.Shop.DisDepot.DepotRegion.GetDepotByRegion(regionId);
                if (depotId <= 0)
                {
                    return 0;
                }
            }
            #endregion
            string CacheKey = "StockHelper_StockCache_SKU_" + sku + "_DepotId_" + depotId + "_SuppID_" + (supplierId.HasValue && supplierId.Value>0 ? supplierId.Value : 0)+"-"+ ProductKey;
            object objModel = dataCache.GetCache(CacheKey);
        
            if (objModel == null)
            {
                try
                {
                    objModel = GetSkuStock(sku, depotId, supplierId);
                    if (objModel != null)
                    {
                        int ModelCache = Globals.SafeInt(BLL.SysManage.ConfigSystem.GetValueByCache("ModelCache"), 30);
                        dataCache.SetCache(CacheKey, objModel, DateTime.Now.AddMinutes(ModelCache),
                            TimeSpan.Zero);
                    }
                }
                catch (Exception ex)
                {
                    Log.LogHelper.AddWarnLog(ex.Message, ex.StackTrace);
                }
            }
            return Common.Globals.SafeInt(objModel,0);
        }

        /// <summary>
        /// 采用缓存库存
        /// </summary>
        /// <param name="sku"></param>
        /// <param name="regionId"></param>
        /// <param name="supplierId"></param>
        /// <returns></returns>
        public static int GetSkuStockByCacheDepotId(string sku, int depotId,int? supplierId)
        {
            bool IsMultiDepot = YSWL.MALL.BLL.Shop.Service.CommonHelper.OpenMultiDepot();
            string CacheKey = "StockHelper_StockCache_SKU_" + sku + "_DepotId_" + depotId + "_SuppID_" + (supplierId.HasValue && supplierId.Value > 0 ? supplierId.Value : 0)+"-"+ ProductKey;
            object objModel = dataCache.GetCache(CacheKey);
            if (objModel == null)
            {
                try
                {
                    objModel = GetSkuStock(sku, depotId, supplierId);
                    if (objModel != null)
                    {
                        int ModelCache = Globals.SafeInt(BLL.SysManage.ConfigSystem.GetValueByCache("ModelCache"), 30);
                        dataCache.SetCache(CacheKey, objModel, DateTime.Now.AddMinutes(ModelCache),
                            TimeSpan.Zero);
                    }
                }
                catch (Exception ex)
                {
                    Log.LogHelper.AddWarnLog(ex.Message, ex.StackTrace);
                }
            }
            return Common.Globals.SafeInt(objModel, 0);
        }
        /// <summary>
        /// 更新SKU缓存
        /// </summary>
        /// <param name="sku"></param>
        /// <param name="count">SKU变化的数量 减少就为正数,如下单，增加就为负数，如用户取消订单</param>
        /// <param name="regionId"></param>
        /// <param name="supplierId"></param>
        public static void UpdateStockCache(string sku,int count, int regionId, int? supplierId)
        {
            bool IsMultiDepot = YSWL.MALL.BLL.Shop.Service.CommonHelper.OpenMultiDepot();
            int depotId = 0;

            #region  判断是否开启多仓
            if (IsMultiDepot)
            {
                depotId = YSWL.MALL.BLL.Shop.DisDepot.DepotRegion.GetDepotByRegion(regionId);
                if (depotId <= 0)
                {
                    return;
                }
            }
            #endregion 
            //int depotId = YSWL.MALL.BLL.Shop.DisDepot.DepotRegion.GetDepotByRegion(regionId);
            //if (depotId <= 0) return;
            string CacheKey = "StockHelper_StockCache_SKU_" + sku + "_DepotId_" + depotId + "_SuppID_" + (supplierId.HasValue && supplierId.Value > 0 ? supplierId.Value : 0)+"-"+ ProductKey;
          
            object objModel = dataCache.GetCache(CacheKey);
            if (objModel == null)//恰好这个时候缓存失效
            {
                try
                {
                    objModel = GetSkuStock(sku, depotId, supplierId);
                    //TODO: 需要做GETSKU分支处理
                    if (objModel != null)
                    {
                        int ModelCache = Globals.SafeInt(BLL.SysManage.ConfigSystem.GetValueByCache("ModelCache"), 30);
                        dataCache.SetCache(CacheKey, objModel, DateTime.Now.AddMinutes(ModelCache), TimeSpan.Zero);
                    }
                }
                catch { }
            }
            else
            {
                int newObj = Common.Globals.SafeInt(objModel,0) - count;
                dataCache.SetCache(CacheKey, newObj);
            }
           
        }

        /// <summary>
        /// 更新SKU缓存
        /// </summary>
        /// <param name="sku"></param>
        /// <param name="count">SKU变化的数量 减少就为正数,如下单，增加就为负数，如用户取消订单</param>
        /// <param name="regionId"></param>
        /// <param name="depotId"></param>
        /// <param name="supplierId"></param>
        public static void UpdatePosStockCache(string sku, int count, int depotId, int? supplierId)
        {
            bool IsMultiDepot = YSWL.MALL.BLL.Shop.Service.CommonHelper.OpenMultiDepot();

            #region  判断是否开启多仓
            if (IsMultiDepot)
            {
                if (depotId <= 0)
                {
                    return;
                }
            }
            #endregion 
            //int depotId = YSWL.MALL.BLL.Shop.DisDepot.DepotRegion.GetDepotByRegion(regionId);
            //if (depotId <= 0) return;
            string CacheKey = "StockHelper_StockCache_SKU_" + sku + "_DepotId_" + depotId + "_SuppID_" + (supplierId.HasValue && supplierId.Value > 0 ? supplierId.Value : 0)+"-"+ ProductKey;

            object objModel = dataCache.GetCache(CacheKey);
            if (objModel == null)//恰好这个时候缓存失效
            {
                try
                {
                    objModel = GetSkuStock(sku, depotId, supplierId);
                    //TODO: 需要做GETSKU分支处理
                    if (objModel != null)
                    {
                        int ModelCache = Globals.SafeInt(BLL.SysManage.ConfigSystem.GetValueByCache("ModelCache"), 30);
                        dataCache.SetCache(CacheKey, objModel, DateTime.Now.AddMinutes(ModelCache), TimeSpan.Zero);
                    }
                }
                catch { }
            }
            else
            {
                int newObj = Common.Globals.SafeInt(objModel, 0) - count;
                dataCache.SetCache(CacheKey, newObj);
            }

        }

        /// <summary>
        /// 更新下单后的SKU缓存
        /// </summary>
        /// <param name="?"></param>
        public static void UpdateOrderStock(YSWL.MALL.Model.Shop.Order.OrderInfo orderInfo)
        {
            foreach (var item in orderInfo.OrderItems)
            {
                UpdateStockCache(item.SKU,item.Quantity, orderInfo.RegionId,
                    orderInfo.SupplierId);
            }
        }

        /// <summary>
        /// 更新Pos下单后的SKU缓存
        /// </summary>
        /// <param name="?"></param>
        public static void UpdatePosOrderStock(YSWL.MALL.Model.Shop.Order.OrderInfo orderInfo,int depotid)
        {
            foreach (var item in orderInfo.OrderItems)
            {
                UpdatePosStockCache(item.SKU, item.Quantity, depotid,
                    orderInfo.SupplierId);
            }
        }

        /// <summary>
        /// 取消订单更新SKU库存
        /// </summary>
        /// <param name="orderInfo"></param>
        public static void CancelOrderStock(YSWL.MALL.Model.Shop.Order.OrderInfo orderInfo)
        {
            foreach (var item in orderInfo.OrderItems)
            {
                UpdateStockCache(item.SKU, -item.Quantity, orderInfo.RegionId,
                    orderInfo.SupplierId);
            }
        }
        /// <summary>
        /// 获取商品的分仓仓库库存
        /// </summary>
        /// <param name="productId"></param>
        /// <param name="regionId"></param>
        /// <param name="supplierId"></param>
        /// <returns></returns>
        public static ViewModel.Shop.ProductSKUModel GetProductSKUInfo(long productId,int regionId,int supplierId)
        {
            //获取SKU信息
            ViewModel.Shop.ProductSKUModel productSKUModel = skuInfoBll.GetProductSKUInfoByCache(productId);
            if (productSKUModel == null)
            {
                return null;
            }
            if (productSKUModel.ListSKUInfos == null || productSKUModel.ListSKUInfos.Count < 1 ||
                productSKUModel.ListSKUItems == null)
            {
                return null;
            }
            //获取分仓仓库的库存
            foreach (var item in productSKUModel.ListSKUInfos)
            {
                item.Stock = GetSkuStockByCache(item.SKU, regionId, supplierId);
            }
            return productSKUModel;
        }
        /// <summary>
        /// 获取SKU 库存
        /// </summary>
        /// <param name="sku"></param>
        /// <param name="depotId"></param>
        /// <param name="supplierId"></param>
        /// <returns></returns>
        public static int GetSkuStockEx(string sku, int depotId, int? supplierId)
        {
            //是否开启了警戒库存
            bool IsOpenAS = YSWL.MALL.BLL.SysManage.ConfigSystem.GetBoolValueByCache("Shop_OpenAlertStock");
            //是否开启多分仓库存
            bool IsMultiDepot = YSWL.MALL.BLL.Shop.Service.CommonHelper.OpenMultiDepot();
            if (IsMultiDepot && (!supplierId.HasValue || supplierId <= 0))  //开启了多仓对接，就需要对接仓库库存,且不是商家商品
            {
                if (depotId == 0)
                {
                    return 0;
                }
                return disSkUsBll.GetStockBySKU(sku, depotId, IsOpenAS);
            }
            else
            {
                return skuDal.GetStockBySKU(sku, IsOpenAS);
            }
        }

    }
}
