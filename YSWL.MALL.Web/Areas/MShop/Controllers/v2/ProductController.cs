using System;
using System.Collections.Generic;
using System.Web.Mvc;

namespace YSWL.MALL.Web.Areas.MShop.Controllers
{
    public partial class ProductController :YSWL.MALL.Web.Areas.Shop.Controllers.ProductController
    {
        #region 商品SKU规格选择 返回的库存为分仓商品库存
        /// <summary>
        /// 商品SKU规格选择 返回的库存为分仓商品库存
        /// </summary>
        /// <param name="productId"></param>
        /// <param name="SuppId"></param>
        /// <param name="viewName"></param>
        /// <returns></returns>
        public override ActionResult OptionSKUByDepot(long productId, int SuppId, string viewName = "_OptionSKU")
        {
            if (productId < 1) return new EmptyResult();
            ViewModel.Shop.ProductSKUModel productSKUModel =YSWL.MALL.BLL.Shop.Products.StockHelper.GetProductSKUInfo(productId, GetRegionId, SuppId);
            //NO SKU ERROR
            if (productSKUModel == null) return new EmptyResult();
            //NO SKU ERROR
            if (productSKUModel.ListSKUInfos == null || productSKUModel.ListSKUInfos.Count < 1 ||
                productSKUModel.ListSKUItems == null)
                return new EmptyResult();

            ViewBag.HasSKU = true;

            //木有开启SKU的情况
            if (productSKUModel.ListSKUItems.Count == 0)
            {
                ViewBag.HasSKU = false;
                productSKUModel.ListSKUInfos[0].Stock= YSWL.MALL.BLL.Shop.Products.StockHelper.GetSkuStockByCache(productSKUModel.ListSKUInfos[0].SKU, GetRegionId, SuppId);
                return View(viewName, productSKUModel);
            }
            ViewBag.SKUJson = SKUInfoToJson(productSKUModel.ListSKUInfos, SuppId).ToString();
            return View(viewName, productSKUModel);
        }
        #endregion
    }
}