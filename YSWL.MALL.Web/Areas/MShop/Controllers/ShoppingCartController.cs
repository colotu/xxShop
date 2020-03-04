using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using YSWL.MALL.BLL.Shop.Products;
using YSWL.Components.Setting;
using YSWL.MALL.Model.Shop.Products;
using YSWL.MALL.ViewModel.Shop;
using YSWL.MALL.Web.Components.Setting.Shop;
using YSWL.Json;
using YSWL.MALL.Web.Handlers;

namespace YSWL.MALL.Web.Areas.MShop.Controllers
{
    public partial class ShoppingCartController : MShopControllerBase
    {
        private YSWL.MALL.BLL.Shop.Products.ProductInfo productBll = new YSWL.MALL.BLL.Shop.Products.ProductInfo();
        private YSWL.MALL.BLL.Shop.Products.SKUInfo skuBll = new YSWL.MALL.BLL.Shop.Products.SKUInfo();
        public ActionResult CartInfo()
        {
            #region Remove MShop SubmitOrderSession SKU/COUNT/CountDown/GroupBuy
            Session.Remove("SubmitOrder_SKU");
            Session.Remove("SubmitOrder_COUNT");
            Session.Remove("SubmitOrder_CountDown");
            Session.Remove("SubmitOrder_GroupBuy");
            #endregion
                
            #region SEO 优化设置
            IPageSetting pageSetting = PageSetting.GetPageSetting("Home", Model.SysManage.ApplicationKeyType.Shop);
            ViewBag.Title = "购物车信息" + pageSetting.Title;
            ViewBag.Keywords = pageSetting.Keywords;
            ViewBag.Description = pageSetting.Description;
            #endregion
            return View();
        }

        /// <summary>
        /// 获取购物车数量
        /// </summary>
        public ActionResult GetCartCount()
        {
            int userId =  (currentUser == null || currentUser.UserType=="AA") ? -1 : currentUser.UserID;
            ShoppingCartHelper cartHelper = new ShoppingCartHelper(userId);
            return Content(cartHelper.GetShoppingCart().Quantity.ToString(System.Globalization.CultureInfo.InvariantCulture));
        }
        /// <summary>
        /// 购物车列表
        /// </summary>
        /// <returns></returns>
        public ActionResult CartList(string viewName = "_CartList")
        {
            int userId =  (currentUser == null || currentUser.UserType=="AA") ? -1 : currentUser.UserID;
            YSWL.MALL.BLL.Shop.Products.ShoppingCartHelper cartHelper = new ShoppingCartHelper(userId);
            YSWL.MALL.Model.Shop.Products.ShoppingCartInfo cartInfo = cartHelper.GetShoppingCart();

            #region 批销优惠
            try
            {
                BLL.Shop.Sales.SalesRuleProduct salesRule = new BLL.Shop.Sales.SalesRuleProduct();
                cartInfo = salesRule.GetWholeSale(cartInfo);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            #endregion

            //获取库存
            if (cartInfo != null && cartInfo.Items != null)
            {
                foreach (var item in cartInfo.Items)
                {
                    item.Stock = YSWL.MALL.BLL.Shop.Products.StockHelper.GetSkuStockByCache(item.SKU, GetRegionId, item.SupplierId);
                    //获取销售状态
                    item.SaleStatus = skuBll.GetSaleStatus(item.SKU);
                }
            }

            return View(viewName, cartInfo);
        }

        /// <summary>
        /// 添加购物车  
        /// </summary>
        /// <returns></returns>
        public ActionResult AddCart(string sku , int count = 1, string viewName = "AddCart")
        {
            if (string.IsNullOrWhiteSpace(sku)) return RedirectToAction("Index", "Home");
            if (count < 1) count = 1;   //Safe Reset Count

            int userId = (currentUser == null || currentUser.UserType == "AA") ? -1 : currentUser.UserID;

            YSWL.MALL.BLL.Shop.Products.ShoppingCartHelper cartHelper = new ShoppingCartHelper(userId);
            YSWL.MALL.Model.Shop.Products.ShoppingCartItem cartItem = new ShoppingCartItem();
            YSWL.MALL.ViewModel.Shop.ProductModel model = new ProductModel();
            YSWL.MALL.BLL.Shop.Products.BrandInfo brandInfoBLL = new YSWL.MALL.BLL.Shop.Products.BrandInfo();

            Model.Shop.Products.SKUInfo skuInfo=skuBll.GetModelBySKU(sku);

            #region 获取推广信息
            cartItem.ReferId = 0;
            if (!String.IsNullOrWhiteSpace(Request.Params["r"]))
            {
                string refer = YSWL.Common.UrlOper.Base64Decrypt(Request.Params["r"]);
                int referId = Common.Globals.SafeInt(refer, 0);
                cartItem.ReferId = referId;
            }
            #endregion
 
            //NOSKU
            if (skuInfo == null) return Content("NOSKU");

            model.ProductSkus = new List<Model.Shop.Products.SKUInfo> { skuInfo };
            model.ProductInfo = productBll.GetModelByCache(skuInfo.ProductId);
            if (model.ProductInfo != null && model.ProductSkus != null)
            {
                ShoppingCartInfo cartInfo = cartHelper.GetShoppingCart();
                #region  检测限购数
                YSWL.ShoppingCart.Model.CartItemInfo cartItemInfo = cartInfo[model.ProductSkus[0].SKU];
                int cartInfoQuantity = 0;
                if (cartItemInfo != null)
                {
                    cartInfoQuantity = cartItemInfo.Quantity;
                }
                if (model.ProductInfo.RestrictionCount > 0 && (cartInfoQuantity + count) > model.ProductInfo.RestrictionCount)
                {
                    return Content("GreaRestCount");
                }
                #endregion
                
                cartItem.Name = model.ProductInfo.ProductName;
                cartItem.Quantity = count;
                cartItem.SKU = model.ProductSkus[0].SKU;
                cartItem.ProductId = model.ProductInfo.ProductId;
                cartItem.UserId = userId;
                cartItem.BrandId = model.ProductInfo.BrandId;
                cartItem.RestrictionCount = model.ProductInfo.RestrictionCount;

                List<YSWL.MALL.Model.Shop.Products.BrandInfo> allBrands = brandInfoBLL.GetAllBrands();
                YSWL.MALL.Model.Shop.Products.BrandInfo brandInfo =
                    allBrands.Find(c => c.BrandId == model.ProductInfo.BrandId);
                if (brandInfo != null)
                {
                    cartItem.BrandName = brandInfo.BrandName;
                }

                #region 商家
                if (model.ProductInfo.SupplierId > 0)
                {
                    BLL.Shop.Supplier.SupplierInfo supplierManage = new BLL.Shop.Supplier.SupplierInfo();
                    Model.Shop.Supplier.SupplierInfo supplierInfo = supplierManage.GetModelByCache(model.ProductInfo.SupplierId);
                    if (supplierInfo != null)
                    {
                        cartItem.SupplierId = supplierInfo.SupplierId;
                        cartItem.SupplierName = supplierInfo.Name;
                        cartItem.ShopName = supplierInfo.ShopName;
                    }
                }
                #endregion
                //DONE: 根据SkuId 获取SKUItem值和图片数据 BEN 2013-06-30
                List<Model.Shop.Products.SKUItem> listSkuItems = skuBll.GetSKUItemsBySkuId(skuInfo.SkuId);
                if (listSkuItems != null && listSkuItems.Count > 0)
                {
                    cartItem.SkuValues = new string[listSkuItems.Count];
                    int index = 0;
                    listSkuItems.ForEach(xx =>
                    {
                        cartItem.SkuValues[index++] = xx.ValueStr;
                        if (!string.IsNullOrWhiteSpace(xx.ImageUrl))
                        {
                            cartItem.SkuImageUrl = xx.ImageUrl;
                        }
                    });
                }
                //TODO: 未使用SKU缩略图 BEN ADD 2013-06-30
                cartItem.ThumbnailsUrl = model.ProductInfo.ThumbnailUrl1;

                cartItem.CostPrice = model.ProductSkus[0].CostPrice.HasValue ? model.ProductSkus[0].CostPrice.Value : 0;
                cartItem.MarketPrice = model.ProductInfo.MarketPrice.HasValue ? model.ProductInfo.MarketPrice.Value : 0;

                cartItem.SellPrice = cartItem.AdjustedPrice = model.ProductSkus[0].SalePrice;
                cartItem.Weight = model.ProductSkus[0].Weight.HasValue ? model.ProductSkus[0].Weight.Value : 0;
                cartItem.Unit = model.ProductInfo.Unit;
                cartItem.Points = (int)(model.ProductInfo.Points.HasValue ? model.ProductInfo.Points : 0);

                cartItem.Gwjf = (int)(model.ProductInfo.Gwjf.HasValue ? model.ProductInfo.Gwjf : 0);

                cartHelper.AddItem(cartItem);

                cartInfo = cartHelper.GetShoppingCart();
                //TODO: 添加购物车如果要展示, 这里的价格需提示优惠价格 BEN ADD 2013-06-24
                ViewBag.TotalPrice = cartInfo.TotalSellPrice;
                ViewBag.ItemCount = cartInfo.Quantity;
            }
            ViewBag.Title = "添加购物车";
            return RedirectToAction("CartInfo");
            //return View(viewName, model);
        }

        #region Ajax 方法
        /// <summary>
        /// 移除订单项
        /// </summary>
        /// <param name="Fm"></param>
        /// <returns></returns>
        public ActionResult RemoveItem(FormCollection Fm)
        {
            if (String.IsNullOrWhiteSpace(Fm["ItemIds"]))
            {
                return Content("No");
            }
            else
            {
                string itemIds = Fm["ItemIds"];
                var item_arr = itemIds.Split(',');
                int userId =  (currentUser == null || currentUser.UserType=="AA") ? -1 : currentUser.UserID;
                YSWL.MALL.BLL.Shop.Products.ShoppingCartHelper cartHelper = new ShoppingCartHelper(userId);
                foreach (var item in item_arr)
                {
                    int itemId = Common.Globals.SafeInt(item, 0);
                    cartHelper.RemoveItem(itemId);
                }
                return Content("Yes");
            }
        }

        /// <summary>
        /// 更新购物车项数量
        /// </summary>
        /// <param name="Fm"></param>
        /// <returns></returns>
        public ActionResult UpdateItemCount(FormCollection Fm)
        {
            if (String.IsNullOrWhiteSpace(Fm["ItemId"]) || String.IsNullOrWhiteSpace(Fm["Count"]))
            {
                return Content("No");
            }
            else
            {
                int itemId = Common.Globals.SafeInt(Fm["ItemId"], 0);
                int count = Common.Globals.SafeInt(Fm["Count"], 1);
                int userId =  (currentUser == null || currentUser.UserType=="AA") ? -1 : currentUser.UserID;
                YSWL.MALL.BLL.Shop.Products.ShoppingCartHelper cartHelper = new ShoppingCartHelper(userId);
                cartHelper.UpdateItemQuantity(itemId, count);
                return Content("Yes");
            }
        }
        /// <summary>
        /// 清空购物车
        /// </summary>
        /// <param name="Fm"></param>
        /// <returns></returns>
        public ActionResult ClearShopCart()
        {
            int userId =  (currentUser == null || currentUser.UserType=="AA") ? -1 : currentUser.UserID;
            YSWL.MALL.BLL.Shop.Products.ShoppingCartHelper cartHelper = new ShoppingCartHelper(userId);
            cartHelper.ClearShoppingCart();
            return Content("Yes");
        }


        /// <summary>
        ///  商品列表页直接加入购物车
        /// </summary>
        /// <param name="Fm"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult AddCart(FormCollection Fm)
        {
            JsonObject json = new JsonObject();
            long productid = Common.Globals.SafeLong(Fm["Productid"], -1);
            int count = Common.Globals.SafeInt(Fm["Count"], 1);
            if (count < 1) count = 1;   //Safe Reset Count
            string sku = Common.Globals.SafeString(Fm["Sku"], "");
            if (productid <= 0 && String.IsNullOrWhiteSpace(sku))
            {
                json.Accumulate(HandlerBase.KEY_STATUS, HandlerBase.STATUS_FAILED);
                json.Accumulate(HandlerBase.KEY_DATA, "NO");
                return Content(json.ToString());
            }
            
            int userId =  (currentUser == null || currentUser.UserType=="AA") ? -1 : currentUser.UserID;
            YSWL.MALL.BLL.Shop.Products.ShoppingCartHelper cartHelper = new ShoppingCartHelper(userId);
            YSWL.MALL.Model.Shop.Products.ShoppingCartItem cartItem = new ShoppingCartItem();
            YSWL.MALL.ViewModel.Shop.ProductModel model = new ProductModel();
            Model.Shop.Products.SKUInfo skuInfo = null;
            if (!String.IsNullOrWhiteSpace(sku))
            {
                skuInfo = skuBll.GetModelBySKU(sku);
            }
            else
            {
                ProductSKUModel prouctsku = skuBll.GetProductSKUInfoByProductId(productid);
                if (prouctsku == null || prouctsku.ListSKUInfos == null || prouctsku.ListSKUInfos.Count <= 0)
                {
                    json.Accumulate(HandlerBase.KEY_STATUS, HandlerBase.STATUS_FAILED);
                    json.Accumulate(HandlerBase.KEY_DATA, "NOSKU");
                    return Content(json.ToString());
                }
                skuInfo = prouctsku.ListSKUInfos[0];
            }
            //NOSKU
            if (skuInfo == null)
            {
                json.Accumulate(HandlerBase.KEY_STATUS, HandlerBase.STATUS_FAILED);
                json.Accumulate(HandlerBase.KEY_DATA, "NOSKU");
                return Content(json.ToString());
            }

            model.ProductInfo = productBll.GetModelByCache(skuInfo.ProductId);
            model.ProductSkus = new List<Model.Shop.Products.SKUInfo> { skuInfo };

            if (model.ProductInfo == null || model.ProductSkus == null)
            {
                json.Accumulate(HandlerBase.KEY_STATUS, HandlerBase.STATUS_FAILED);
                json.Accumulate(HandlerBase.KEY_DATA, "NO");
                return Content(json.ToString());
            }

            #region 判断库存
            skuInfo.Stock=YSWL.MALL.BLL.Shop.Products.StockHelper.GetSkuStockByCache(skuInfo.SKU, GetRegionId, model.ProductInfo.SupplierId);
            if (skuInfo.Stock <count)
            {
                json.Accumulate(HandlerBase.KEY_STATUS, HandlerBase.STATUS_FAILED);
                json.Accumulate(HandlerBase.KEY_DATA, "NOSTOCK");//库存不足
                return Content(json.ToString());
            }
            #endregion

           
                ShoppingCartInfo cartInfo = cartHelper.GetShoppingCart();
                #region  检测限购数
                YSWL.ShoppingCart.Model.CartItemInfo cartItemInfo = cartInfo[model.ProductSkus[0].SKU];
                int cartInfoQuantity = 0;
                if (cartItemInfo != null)
                {
                    cartInfoQuantity = cartItemInfo.Quantity;
                }
                if (model.ProductInfo.RestrictionCount > 0 && (cartInfoQuantity + count) > model.ProductInfo.RestrictionCount)
                {
                    json.Accumulate(HandlerBase.KEY_STATUS, HandlerBase.STATUS_FAILED);
                    json.Accumulate(HandlerBase.KEY_DATA, "GreaRestCount");
                    return Content(json.ToString());
                }
            #endregion

            #region 获取推广信息
            cartItem.ReferId = 0;
            if (!String.IsNullOrWhiteSpace(Request.Params["r"]))
            {
                string refer = YSWL.Common.UrlOper.Base64Decrypt(Request.Params["r"]);
                int referId = Common.Globals.SafeInt(refer, 0);
                cartItem.ReferId = referId;
            }
            #endregion

            cartItem.Name = model.ProductInfo.ProductName;
                cartItem.Quantity = count;
                cartItem.SKU = model.ProductSkus[0].SKU;
                cartItem.ProductId = model.ProductInfo.ProductId;
                cartItem.UserId = userId;
                cartItem.BrandId = model.ProductInfo.BrandId;
                cartItem.RestrictionCount = model.ProductInfo.RestrictionCount;
                #region 商家
                if (model.ProductInfo.SupplierId > 0)
                {
                    BLL.Shop.Supplier.SupplierInfo supplierManage = new BLL.Shop.Supplier.SupplierInfo();
                    Model.Shop.Supplier.SupplierInfo supplierInfo = supplierManage.GetModelByCache(model.ProductInfo.SupplierId);
                    if (supplierInfo != null)
                    {
                        cartItem.SupplierId = supplierInfo.SupplierId;
                        cartItem.SupplierName = supplierInfo.Name;
                        cartItem.ShopName = supplierInfo.ShopName;
                    }
                }
                #endregion
                //DONE: 根据SkuId 获取SKUItem值和图片数据 BEN 2013-06-30
                List<Model.Shop.Products.SKUItem> listSkuItems = skuBll.GetSKUItemsBySkuId(skuInfo.SkuId);
                if (listSkuItems != null && listSkuItems.Count > 0)
                {
                    cartItem.SkuValues = new string[listSkuItems.Count];
                    int index = 0;
                    listSkuItems.ForEach(xx =>
                    {
                        cartItem.SkuValues[index++] = xx.ValueStr;
                        if (!string.IsNullOrWhiteSpace(xx.ImageUrl))
                        {
                            cartItem.SkuImageUrl = xx.ImageUrl;
                        }
                    });
                }
                //TODO: 未使用SKU缩略图 BEN ADD 2013-06-30
                cartItem.ThumbnailsUrl = model.ProductInfo.ThumbnailUrl1;

                cartItem.CostPrice = model.ProductSkus[0].CostPrice.HasValue ? model.ProductSkus[0].CostPrice.Value : 0;
                cartItem.MarketPrice = model.ProductInfo.MarketPrice.HasValue ? model.ProductInfo.MarketPrice.Value : 0;

                cartItem.SellPrice = cartItem.AdjustedPrice = model.ProductSkus[0].SalePrice;
                cartItem.Weight = model.ProductSkus[0].Weight.HasValue ? model.ProductSkus[0].Weight.Value : 0;
                cartItem.Points = (int)(model.ProductInfo.Points.HasValue ? model.ProductInfo.Points : 0);

            cartItem.Gwjf = (int)(model.ProductInfo.Gwjf.HasValue ? model.ProductInfo.Gwjf : 0);

            cartItem.Unit = model.ProductInfo.Unit;
                cartHelper.AddItem(cartItem);
                cartInfo = cartHelper.GetShoppingCart();
                if (cartInfo != null && cartInfo.Items != null && cartInfo.Items.Count > 0)
                {
                    json.Accumulate(HandlerBase.KEY_STATUS, HandlerBase.STATUS_SUCCESS);
                    json.Accumulate(HandlerBase.KEY_DATA, cartInfo.Items[cartInfo.Items.Count - 1].ItemId.ToString());//itemid
                    return Content(json.ToString());
                }
                else
                {
                    json.Accumulate(HandlerBase.KEY_STATUS, HandlerBase.STATUS_FAILED);
                    json.Accumulate(HandlerBase.KEY_DATA, "NO");
                    return Content(json.ToString());
                }
        }
        /// <summary>
        /// 验证是否超出限购数
        /// </summary>
        /// <param name="Fm"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult CheckRestCount(FormCollection Fm)
        {
            JsonObject json = new JsonObject();
            int userId =  (currentUser == null || currentUser.UserType=="AA") ? -1 : currentUser.UserID;
            YSWL.MALL.BLL.Shop.Products.ShoppingCartHelper cartHelper = new ShoppingCartHelper(userId);
            int count = Common.Globals.SafeInt(Fm["Count"], 1);
            if (count < 1) count = 1;   //Safe Reset Count 
            string sku = Common.Globals.SafeString(Fm["Sku"], "");
            if (String.IsNullOrWhiteSpace(sku))
            {
                json.Accumulate(HandlerBase.KEY_STATUS, HandlerBase.STATUS_FAILED);
                json.Accumulate(HandlerBase.KEY_DATA, "NO");
                return Content(json.ToString());
            }
            YSWL.MALL.Model.Shop.Products.SKUInfo skuInfo = skuBll.GetModelBySKU(sku);
            //NOSKU
            if (skuInfo == null) return Content("NOSKU");

            ShoppingCartInfo cartInfo = cartHelper.GetShoppingCart();
            #region  检测限购数
            YSWL.ShoppingCart.Model.CartItemInfo cartItemInfo = cartInfo[sku];
            int cartInfoQuantity = 0;
            if (cartItemInfo != null)
            {
                cartInfoQuantity = cartItemInfo.Quantity;
            }
            int restCount = productBll.GetRestrictionCount(skuInfo.ProductId);
            if (restCount > 0 && (cartInfoQuantity + count) > restCount)
            {
                return Content("GreaRestCount");
            }
            else
            {
                return Content("True");
            }
            #endregion
        }
        #endregion

        /// <summary>
        /// 获取购物车商品金额
        /// </summary>
        public ActionResult GetCartTotalPrice()
        {
            int userId =  (currentUser == null || currentUser.UserType=="AA") ? -1 : currentUser.UserID;
            ShoppingCartHelper cartHelper = new ShoppingCartHelper(userId);
            JsonObject json = new JsonObject();
            json.Accumulate("Quantity", cartHelper.GetShoppingCart().Quantity.ToString(System.Globalization.CultureInfo.InvariantCulture));
            json.Accumulate("TotalAdjustedPrice", cartHelper.GetShoppingCart().TotalAdjustedPrice.ToString("F"));
            return Content(json.ToString());
        }
        /// <summary>
        /// 购物车列表
        /// </summary>
        /// <returns></returns>
        public ActionResult GetCartList()
        {
            int userId =  (currentUser == null || currentUser.UserType=="AA") ? -1 : currentUser.UserID;
            YSWL.MALL.BLL.Shop.Products.ShoppingCartHelper cartHelper = new ShoppingCartHelper(userId);
            YSWL.MALL.Model.Shop.Products.ShoppingCartInfo cartInfo = cartHelper.GetShoppingCart();

            #region 批销优惠
            try
            {
                BLL.Shop.Sales.SalesRuleProduct salesRule = new BLL.Shop.Sales.SalesRuleProduct();
                cartInfo = salesRule.GetWholeSale(cartInfo);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            #endregion

            JsonObject json = new JsonObject();
            json.Accumulate(HandlerBase.KEY_STATUS, HandlerBase.STATUS_SUCCESS);
            json.Accumulate(HandlerBase.KEY_DATA, cartInfo.Items);
            return Content(json.ToString());
        }
        /// <summary>
        /// 全选或取消全选
        /// </summary>
        [HttpPost]
        public ActionResult SelectedItemAll(string option)
        {
            int userId =  (currentUser == null || currentUser.UserType=="AA") ? -1 : currentUser.UserID;
            YSWL.MALL.BLL.Shop.Products.ShoppingCartHelper cartHelper = new ShoppingCartHelper(userId);
            YSWL.MALL.Model.Shop.Products.ShoppingCartInfo cartInfo = cartHelper.GetShoppingCart();
            if (cartInfo == null || cartInfo.Quantity < 1) return Content("NOITEMS");

            switch (option)
            {
                case "check":
                    foreach (var item in cartInfo.Items)
                    {
                        item.Selected = true;
                    }
                    break;
                case "remove":
                    foreach (var item in cartInfo.Items)
                    {
                        item.Selected = false;
                    }
                    break;
                default:
                    return Content("false");
            }
            cartHelper.SaveShoppingCart(cartInfo);
            return Content("OK");
        }
    }
}
