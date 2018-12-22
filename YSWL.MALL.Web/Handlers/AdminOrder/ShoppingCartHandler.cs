using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.SessionState;
using YSWL.Json;
using YSWL.MALL.BLL.Shop;
using YSWL.MALL.Model.Shop;
using YSWL.MALL.Web.Handlers;
using YSWL.Common;
using YSWL.MALL.BLL.Shop.Products;
using YSWL.MALL.Model.Shop.Products;
using YSWL.MALL.ViewModel.Shop;

namespace YSWL.MALL.Web.Handlers.AdminOrder
{
    public class ShoppingCartHandler : HandlerBase, IRequiresSessionState
    {
        private YSWL.MALL.BLL.Shop.Products.ProductInfo productBll = new BLL.Shop.Products.ProductInfo();
        private YSWL.MALL.BLL.Shop.Products.SKUInfo skuBll = new BLL.Shop.Products.SKUInfo();
        private BLL.Shop.DisDepot.DepotProSKUs depotProSkusBll = new BLL.Shop.DisDepot.DepotProSKUs();
        #region IHttpHandler 成员

        public override bool IsReusable
        {
            get { return false; }
        }

        public int SelectedUserId
        {
            get
            {
                return Globals.SafeInt(Cookies.getCookie("A_Order_SelectUserId", "value"), 0);
            }
        }

        public int DepotId
        {
            get
            {
                return Globals.SafeInt(Cookies.getCookie("A_Order_DepotId", "value"), 0);
            }
        }
        public override void ProcessRequest(HttpContext context)
        {
            string action = context.Request.Form["Action"];
            context.Response.Clear();
            context.Response.ContentType = "application/json";
            try
            {
                if (CurrentUser == null || CurrentUser.UserType != "AA")
                {
                    JsonObject result = new JsonObject();
                    result.Accumulate(KEY_STATUS, STATUS_NOLOGIN);
                    context.Response.Write(result.ToString());
                }
                switch (action)
                {
                    case "AddCart":
                        context.Response.Write(AddCart(context));
                        break;
                    case "AddCartSku":
                        context.Response.Write(AddCartSKU(context));
                        break;
                    case "RemoveItem":
                        context.Response.Write(RemoveItem(context));
                        break;
                    case "UpdateItemCount":
                        context.Response.Write(UpdateItemCount(context));
                        break;
                    case "UpdateItemPrice":
                        context.Response.Write(UpdateItemPrice(context));
                        break;
                    case "ClearShopCart":
                        context.Response.Write(ClearShopCart(context));
                        break;
                    case "GetCartCount":
                        context.Response.Write(GetCartCount(context));
                        break;
                    
                    default:
                        break;
                }
            }
            catch (Exception ex)
            {
                JsonObject json = new JsonObject();
                json.Put(KEY_STATUS, STATUS_ERROR);
                json.Put(KEY_DATA, ex);
                context.Response.Write(json.ToString());
            }
        }

        #endregion

        #region 加入购物车
        /// <summary>
        /// 加入购物车
        /// </summary>   
        /// <param name="context"></param>
        /// <returns></returns>
        private string AddCart(HttpContext context)
        {
            JsonObject result = new JsonObject();
            long productId =  Globals.SafeLong(context.Request.Form["ProductId"], -1);
            int count = Globals.SafeInt(context.Request.Form["Count"], 1);
            if (count < 1) count = 1;   //Safe Reset Count
            decimal orderPrice = Globals.SafeDecimal(context.Request.Form["OrderPrice"], -1);
            string sku = Globals.SafeString(context.Request.Form["Sku"], "");
            if (productId <= 0 && String.IsNullOrWhiteSpace(sku))
            {
                result.Accumulate(HandlerBase.KEY_STATUS, HandlerBase.STATUS_FAILED);
                result.Accumulate(HandlerBase.KEY_DATA, "NO");
                return result.ToString();
            }
            YSWL.MALL.BLL.Shop.Products.ShoppingCartHelper cartHelper = new ShoppingCartHelper(SelectedUserId);
            YSWL.MALL.Model.Shop.Products.ShoppingCartItem cartItem = new ShoppingCartItem();
            YSWL.MALL.ViewModel.Shop.ProductModel model = new ProductModel();
            Model.Shop.Products.SKUInfo skuInfo = skuBll.GetModelBySKU(sku);
            if (skuInfo == null)
            {
                result.Accumulate(HandlerBase.KEY_STATUS, HandlerBase.STATUS_FAILED);
                result.Accumulate(HandlerBase.KEY_DATA, "NOSKU");
                return result.ToString();
            }
            model.ProductSkus = new List<Model.Shop.Products.SKUInfo> { skuInfo };
            model.ProductInfo = productBll.GetModel(skuInfo.ProductId);
            if (model.ProductInfo == null || model.ProductSkus == null)
            {
                result.Accumulate(HandlerBase.KEY_STATUS, HandlerBase.STATUS_FAILED);
                result.Accumulate(HandlerBase.KEY_DATA, "NO");
                return result.ToString();
            }
            #region 判断库存
            //获取现有的购物车数据库存
            var oldCart = cartHelper.GetShoppingCart(true);
            var itemList = oldCart.Items.FindAll(c => c.SKU == skuInfo.SKU);
            int oldStock = 0;//已经加入购物车的库存
            foreach (var item in itemList)
            {
                oldStock += item.Quantity;
            }
            skuInfo.Stock = YSWL.MALL.BLL.Shop.Products.StockHelper.GetSkuStockEx(sku, DepotId, model.ProductInfo.SupplierId);
            if ((skuInfo.Stock - count - oldStock) < 0)
            {
                result.Accumulate(HandlerBase.KEY_STATUS, HandlerBase.STATUS_FAILED);
                result.Accumulate(HandlerBase.KEY_DATA, "NOSTOCK");//库存不足
                return result.ToString();
            }
            #endregion
            cartItem.Name = model.ProductInfo.ProductName;
                cartItem.Quantity = count;
                cartItem.SKU = model.ProductSkus[0].SKU;
                cartItem.ProductId = model.ProductInfo.ProductId;
                cartItem.UserId = SelectedUserId;
                cartItem.BrandId = model.ProductInfo.BrandId;
                //DONE: 根据SkuId 获取SKUItem值和图片数据 BEN 2013-06-30
                List<Model.Shop.Products.SKUItem> listSkuItems = skuBll.GetSKUItemsBySkuId(skuInfo.SkuId);
                //TODO: 未使用SKU缩略图 BEN ADD 2013-06-30
                cartItem.ThumbnailsUrl = model.ProductInfo.ThumbnailUrl1;
                cartItem.CostPrice = model.ProductSkus[0].CostPrice.HasValue ? model.ProductSkus[0].CostPrice.Value : 0;
                cartItem.MarketPrice = model.ProductInfo.MarketPrice.HasValue ? model.ProductInfo.MarketPrice.Value : 0;
                cartItem.SellPrice = model.ProductSkus[0].SalePrice;
                //判断传过来的价格 
                cartItem.AdjustedPrice = orderPrice < 0 ? model.ProductSkus[0].SalePrice : orderPrice ;
                cartItem.Weight = model.ProductSkus[0].Weight.HasValue ? model.ProductSkus[0].Weight.Value : 0;
                cartItem.Points = (int)(model.ProductInfo.Points.HasValue ? model.ProductInfo.Points : 0);
                cartItem.Unit = model.ProductInfo.Unit;
                cartHelper.AddItem(cartItem);
                ShoppingCartInfo cartInfo = cartHelper.GetShoppingCart(true);
                if (cartInfo != null && cartInfo.Items != null && cartInfo.Items.Count > 0)
                {
                    result.Accumulate(HandlerBase.KEY_STATUS, HandlerBase.STATUS_SUCCESS);
                    result.Accumulate(HandlerBase.KEY_DATA, cartInfo.Items[cartInfo.Items.Count - 1].ItemId.ToString());//itemid
                    return result.ToString();
                }
            result.Accumulate(HandlerBase.KEY_STATUS, HandlerBase.STATUS_FAILED);
            result.Accumulate(HandlerBase.KEY_DATA, "NO");
            return result.ToString();
        }


        /// <summary>
        /// 加入购物车
        /// </summary>   
        /// <param name="context"></param>
        /// <returns></returns>
        private string AddCartSKU(HttpContext context)
        {
            JsonObject result = new JsonObject();
            string sku = Globals.SafeString(context.Request.Form["Sku"], "");
            int count = 1;
            if (String.IsNullOrWhiteSpace(sku))
            {
                result.Accumulate(HandlerBase.KEY_STATUS, HandlerBase.STATUS_FAILED);
                result.Accumulate(HandlerBase.KEY_DATA, "NOSKU");
                return result.ToString();
            }

            YSWL.MALL.BLL.Shop.Products.ShoppingCartHelper cartHelper = new ShoppingCartHelper(SelectedUserId);
            YSWL.MALL.Model.Shop.Products.ShoppingCartItem cartItem = new ShoppingCartItem();
            YSWL.MALL.ViewModel.Shop.ProductModel model = new ProductModel();
            Model.Shop.Products.SKUInfo skuInfo = skuBll.GetModelBySKU(sku);
            model.ProductSkus = new List<Model.Shop.Products.SKUInfo> { skuInfo };
            model.ProductInfo = productBll.GetModel(skuInfo.ProductId);
            if (skuInfo == null )
            {
                result.Accumulate(HandlerBase.KEY_STATUS, HandlerBase.STATUS_FAILED);
                result.Accumulate(HandlerBase.KEY_DATA, "NOSKU");
                return result.ToString();
            }
            if (model.ProductInfo == null)
            {
                result.Accumulate(HandlerBase.KEY_STATUS, HandlerBase.STATUS_FAILED);
                result.Accumulate(HandlerBase.KEY_DATA, "NO");
                return result.ToString();
            }
         
            #region 判断库存
            //获取现有的购物车数据库存
            var oldCart = cartHelper.GetShoppingCart(true);
            var itemList = oldCart.Items.FindAll(c => c.SKU == skuInfo.SKU);
            int oldStock = 0;//已经加入购物车的库存
            foreach (var item in itemList)
            {
                oldStock += item.Quantity;
            }
            skuInfo.Stock = YSWL.MALL.BLL.Shop.Products.StockHelper.GetSkuStockEx(sku, DepotId, model.ProductInfo.SupplierId);       
            if ((skuInfo.Stock - count - oldStock) < 0)
            {
                result.Accumulate(HandlerBase.KEY_STATUS, HandlerBase.STATUS_FAILED);
                result.Accumulate(HandlerBase.KEY_DATA, "NOSTOCK");//库存不足
                return result.ToString();
            }
            #endregion

            cartItem.Name = model.ProductInfo.ProductName;
            cartItem.Quantity = count;
            cartItem.SKU = model.ProductSkus[0].SKU;
            cartItem.ProductId = model.ProductInfo.ProductId;
            cartItem.UserId = SelectedUserId;
            cartItem.BrandId = model.ProductInfo.BrandId;
            //DONE: 根据SkuId 获取SKUItem值和图片数据 BEN 2013-06-30
            List<Model.Shop.Products.SKUItem> listSkuItems = skuBll.GetSKUItemsBySkuId(skuInfo.SkuId);
            //TODO: 未使用SKU缩略图 BEN ADD 2013-06-30
            cartItem.ThumbnailsUrl = model.ProductInfo.ThumbnailUrl1;
            cartItem.CostPrice = model.ProductSkus[0].CostPrice.HasValue ? model.ProductSkus[0].CostPrice.Value : 0;
            cartItem.MarketPrice = model.ProductInfo.MarketPrice.HasValue ? model.ProductInfo.MarketPrice.Value : 0;
            cartItem.SellPrice = model.ProductSkus[0].SalePrice;
            //判断传过来的价格 
            cartItem.AdjustedPrice = model.ProductSkus[0].SalePrice;
            cartItem.Weight = model.ProductSkus[0].Weight.HasValue ? model.ProductSkus[0].Weight.Value : 0;
            cartItem.Points = (int)(model.ProductInfo.Points.HasValue ? model.ProductInfo.Points : 0);
            cartItem.Unit = model.ProductInfo.Unit;
            cartHelper.AddItem(cartItem);
            ShoppingCartInfo cartInfo = cartHelper.GetShoppingCart(true);
            if (cartInfo != null && cartInfo.Items != null && cartInfo.Items.Count > 0)
            {
                result.Accumulate(HandlerBase.KEY_STATUS, HandlerBase.STATUS_SUCCESS);
                result.Accumulate(HandlerBase.KEY_DATA, cartInfo.Items[cartInfo.Items.Count - 1].ItemId.ToString());//itemid
                return result.ToString();
            }

            result.Accumulate(HandlerBase.KEY_STATUS, HandlerBase.STATUS_FAILED);
            result.Accumulate(HandlerBase.KEY_DATA, "NO");
            return result.ToString();
        }
        #endregion

        #region 移除购物车项
        /// <summary>
        /// 移除购物车项
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private string RemoveItem(HttpContext context)
        {
            JsonObject result = new JsonObject();
            if (CurrentUser == null)
            {
                result.Accumulate(KEY_STATUS, STATUS_NODATA);
            }
            string itemids = context.Request.Form["ItemIds"];
            if (String.IsNullOrWhiteSpace(itemids))
            {
                result.Accumulate(KEY_STATUS, HandlerBase.STATUS_FAILED);
                return result.ToString();
            }
            else
            {
                var item_arr = itemids.Split(',');
                YSWL.MALL.BLL.Shop.Products.ShoppingCartHelper cartHelper = new ShoppingCartHelper(SelectedUserId);
                foreach (var item in item_arr)
                {
                    int itemId =Globals.SafeInt(item, 0);
                    cartHelper.RemoveItem(itemId);
                }
                result.Accumulate(KEY_STATUS, HandlerBase.STATUS_SUCCESS);
                return result.ToString();
            }
        }
        #endregion

        #region  更新购物车项
        /// <summary>
        /// 更新购物车项数量
        /// </summary>
        /// <param name="Fm"></param>
        /// <returns></returns>
        private string UpdateItemCount(HttpContext context)
        {
            JsonObject result = new JsonObject();
            int itemId = Globals.SafeInt(context.Request.Form["ItemId"], 0);
            int count = Globals.SafeInt(context.Request.Form["Count"], 1);
            if (itemId == 0)
            {
                result.Accumulate(KEY_STATUS, HandlerBase.STATUS_FAILED);
                return result.ToString();
            }
            else
            {
                YSWL.MALL.BLL.Shop.Products.ShoppingCartHelper cartHelper = new ShoppingCartHelper(SelectedUserId);
                //获取现有的购物车数据库存
                var oldCart = cartHelper.GetShoppingCart(true);
                var cartitem = oldCart.Items.Find(c => c.ItemId == itemId);
                var itemList = oldCart.Items.FindAll(c => c.SKU == cartitem.SKU);
                YSWL.MALL.Model.Shop.Products.SKUInfo skuInfo = skuBll.GetModelBySKU(cartitem.SKU);
                if (skuInfo == null)
                {
                    result.Accumulate(HandlerBase.KEY_STATUS, HandlerBase.STATUS_FAILED);
                    result.Accumulate(HandlerBase.KEY_DATA, "NOSKU");
                    return result.ToString();
                }
                int oldStock = 0;
                foreach (var item in itemList)
                {
                    oldStock += item.Quantity;
                }
                skuInfo.Stock = YSWL.MALL.BLL.Shop.Products.StockHelper.GetSkuStockEx(skuInfo.SKU, DepotId,null);   
                if ((skuInfo.Stock - count - oldStock + cartitem.Quantity) < 0)
                {
                    result.Accumulate(HandlerBase.KEY_STATUS, HandlerBase.STATUS_FAILED);
                    result.Accumulate(HandlerBase.KEY_DATA, "NOSTOCK");//库存不足
                    return result.ToString();
                }
                cartHelper.UpdateItemQuantity(itemId, count);
                result.Accumulate(KEY_STATUS, HandlerBase.STATUS_SUCCESS);
                return result.ToString();
            }
        }
        /// <summary>
        /// 更新购物车项价格
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private string UpdateItemPrice(HttpContext context)
        {
            JsonObject result = new JsonObject();
            int itemId = Globals.SafeInt(context.Request.Form["ItemId"], 0);
            decimal Price = Globals.SafeDecimal(context.Request.Form["Price"], 0);
            if (Price <0) {
                Price = 0;
            }
            if (itemId == 0)
            {
                result.Accumulate(KEY_STATUS, HandlerBase.STATUS_FAILED);
                return result.ToString();
            }
            else
            {
                YSWL.MALL.BLL.Shop.Products.ShoppingCartHelper cartHelper = new ShoppingCartHelper(SelectedUserId);
                cartHelper.UpdateItemPrice(itemId, Price);
                result.Accumulate(KEY_STATUS, HandlerBase.STATUS_SUCCESS);
                return result.ToString();
            }
        }
        
        #endregion



        #region  清空购物车
        /// <summary>
        /// 清空购物车
        /// </summary>     
        /// <param name="Fm"></param>
        /// <returns></returns>
        public string ClearShopCart(HttpContext context)
        {
            JsonObject result = new JsonObject();
            YSWL.MALL.BLL.Shop.Products.ShoppingCartHelper cartHelper = new ShoppingCartHelper(SelectedUserId);
            cartHelper.ClearShoppingCart();
            result.Accumulate(KEY_STATUS, HandlerBase.STATUS_SUCCESS);
            return result.ToString();
        }
        #endregion

        /// <summary>
        /// 获取购物车数量
        /// </summary>
        public string GetCartCount(HttpContext context)
        {
            JsonObject result = new JsonObject();
            YSWL.MALL.BLL.Shop.Products.ShoppingCartHelper cartHelper = new ShoppingCartHelper(SelectedUserId);
            int count = cartHelper.GetShoppingCart(true).Quantity;
            result.Accumulate(KEY_STATUS, HandlerBase.STATUS_SUCCESS);
            result.Accumulate(HandlerBase.KEY_DATA, count.ToString(System.Globalization.CultureInfo.InvariantCulture));
            return result.ToString();
        }

 
    }
}