using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using YSWL.MALL.BLL.Shop.Products;
using YSWL.MALL.Model.Shop.Products;
using YSWL.MALL.ViewModel.Shop;
using YSWL.Json;
using YSWL.MALL.Web.Handlers;

namespace YSWL.MALL.Web.Areas.MShop.Controllers
{
    public partial class ShoppingCartController : MShopControllerBase
    {
        /// <summary>
        /// 购物车列表 按商家分组
        /// </summary>
        /// <returns></returns>
        public ActionResult CartListBySupp(string viewName = "_CartList")
        {
            int userId = (currentUser == null || currentUser.UserType == "AA") ? -1 : currentUser.UserID;
            YSWL.MALL.BLL.Shop.Products.ShoppingCartHelper cartHelper = new ShoppingCartHelper(userId);
            //DONE: 获取已选中内容的购物车进行 购物车 部分商品 下单 BEN Modify 20130923
            ShoppingCartModel model = new ShoppingCartModel();
            model.AllCartInfo = cartHelper.GetShoppingCart();
            model.SelectedCartInfo = cartHelper.GetShoppingCart4Selected();

            #region 批销优惠
            try
            {
                BLL.Shop.Sales.SalesRuleProduct salesRule = new BLL.Shop.Sales.SalesRuleProduct();
                model.AllCartInfo = salesRule.GetWholeSale(model.AllCartInfo);
                model.SelectedCartInfo = salesRule.GetWholeSale(model.SelectedCartInfo);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            #endregion

            //获取库存
            if (model.AllCartInfo != null && model.AllCartInfo.Items != null)
            {
                foreach (var item in model.AllCartInfo.Items)
                {
                    //获取库存
                    item.Stock = YSWL.MALL.BLL.Shop.Products.StockHelper.GetSkuStockByCache(item.SKU, GetRegionId, item.SupplierId);
                    //获取销售状态
                    item.SaleStatus = skuBll.GetSaleStatus(item.SKU);
                }
            }

            #region 根据商家分组
            if (model.AllCartInfo != null && model.AllCartInfo.Items != null)
            {
                model.DicSuppCartItems = YSWL.MALL.BLL.Shop.Products.ShoppingCartHelper.GetSuppCartItems(model.AllCartInfo.Items);
            }
            #endregion

            return View(viewName, model);
        }


        /// <summary>
        /// 选择购物项
        /// </summary>
        [HttpPost]
        public ActionResult SelectedItem(int id)
        {
            int itemId = Common.Globals.SafeInt(id, -1);
            if (itemId < 1) return Content("NOITEMID");

            int userId = (currentUser == null || currentUser.UserType == "AA") ? -1 : currentUser.UserID;
            YSWL.MALL.BLL.Shop.Products.ShoppingCartHelper cartHelper = new ShoppingCartHelper(userId);
            YSWL.MALL.Model.Shop.Products.ShoppingCartInfo cartInfo = cartHelper.GetShoppingCart();
            if (cartInfo == null || cartInfo.Quantity < 1) return Content("NOITEMS");

            cartInfo[itemId].Selected = !cartInfo[itemId].Selected;
            cartHelper.SaveShoppingCart(cartInfo);

            return Content("OK");
        }

        /// <summary>
        /// 按商家全选或取消全选
        /// </summary>
        /// <param name="option"></param>
        /// <param name="suppId"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult SelectedItemSuppAll(string option,int suppId)
        {
            int userId = (currentUser == null || currentUser.UserType == "AA") ? -1 : currentUser.UserID;
            YSWL.MALL.BLL.Shop.Products.ShoppingCartHelper cartHelper = new ShoppingCartHelper(userId);
            YSWL.MALL.Model.Shop.Products.ShoppingCartInfo cartInfo = cartHelper.GetShoppingCart();
            if (cartInfo == null || cartInfo.Quantity < 1) return Content("NOITEMS");
            List<YSWL.MALL.Model.Shop.Products.ShoppingCartItem> suppCartInfo = cartInfo.Items.Where(o => o.SupplierId == suppId).ToList();
            if (suppId <= 0)
            {
                suppCartInfo = cartInfo.Items.Where(o => (o.SupplierId<=0 || o.SupplierId==null)).ToList();
            }else {
                suppCartInfo = cartInfo.Items.Where(o => o.SupplierId == suppId).ToList();
            }
            if (suppCartInfo == null || suppCartInfo.Count<= 0) {
                return Content("fasle");
            }
            switch (option)
            {
                case "check":
                    foreach (var item in suppCartInfo)
                    {
                        item.Selected = true;
                    }
                    break;
                case "remove":
                    foreach (var item in suppCartInfo)         
                    {
                        item.Selected = false;
                    }
                    break;
                default:
                    return Content("fasle");
            }
            cartHelper.SaveShoppingCart(cartInfo);
            return Content("OK");
        }


        #region 再次订购
        /// <summary>
        ///  根据订单号将商品加入购物车
        /// </summary>
        /// <param name="Fm"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult OrderAddCart(FormCollection Fm)
        {
            BLL.Shop.Order.Orders _orderManage = new BLL.Shop.Order.Orders();
            JsonObject json = new JsonObject();

            #region 验证数据
            long OrderId = Common.Globals.SafeLong(Fm["orderId"], -1);
            if (OrderId <= 0)
            {
                json.Accumulate(HandlerBase.KEY_STATUS, HandlerBase.STATUS_FAILED);
                json.Accumulate(HandlerBase.KEY_DATA, "OrderNO");
                return Content(json.ToString());
            }

            int userId = (currentUser == null || currentUser.UserType == "AA") ? -1 : currentUser.UserID;
            YSWL.MALL.Model.Shop.Order.OrderInfo orderModel = _orderManage.GetModelInfo(OrderId);
            //Safe
            if (orderModel == null || orderModel.BuyerID != userId || orderModel.OrderItems == null)
            {
                json.Accumulate(HandlerBase.KEY_STATUS, HandlerBase.STATUS_FAILED);
                json.Accumulate(HandlerBase.KEY_DATA, "OrderNO");
                return Content(json.ToString());
            }
            #endregion

            //是否清空之前的购物车
            bool IsClearShoppingCart = Common.Globals.SafeBool(BLL.SysManage.ConfigSystem.GetValueByCache("OrderProdIsClearShoppingCart"), true);//订购商品是否清空购物车
            YSWL.MALL.BLL.Shop.Products.ShoppingCartHelper cartHelper = new ShoppingCartHelper(userId);
            List<YSWL.MALL.Model.Shop.Products.ShoppingCartItem> cartItemList = new List<ShoppingCartItem>();
            YSWL.MALL.Model.Shop.Products.ShoppingCartItem cartItem;
            //YSWL.MALL.ViewModel.Shop.ProductModel model;
            YSWL.MALL.Model.Shop.Products.ProductInfo productInfo;

            Model.Shop.Products.SKUInfo skuInfo;
            ShoppingCartInfo cartInfo = cartHelper.GetShoppingCart();
            foreach (YSWL.MALL.Model.Shop.Order.OrderItems item in orderModel.OrderItems)
            {
                cartItem = new ShoppingCartItem();

                #region 获取商品及sku信息
                productInfo = new Model.Shop.Products.ProductInfo();
                skuInfo = skuBll.GetModelBySKU(item.SKU);
                //NOSKU
                if (skuInfo == null || !skuInfo.Upselling)
                {
                    continue;
                }
                productInfo = productBll.GetModelByCache(skuInfo.ProductId);
                if (productInfo == null || productInfo.SaleStatus != 1 || productInfo.SalesType != 1)//判断商品状态
                {
                    continue;
                }
                #endregion

                int cartInfoQuantity = 0; //在购物车中的商品数量
                if (!IsClearShoppingCart)
                {
                    YSWL.ShoppingCart.Model.CartItemInfo cartItemInfo = cartInfo[skuInfo.SKU];
                    if (cartItemInfo != null)
                    {
                        cartInfoQuantity = cartItemInfo.Quantity;
                    }
                }

                #region 判断库存
                skuInfo.Stock = YSWL.MALL.BLL.Shop.Products.StockHelper.GetSkuStockByCache(skuInfo.SKU, GetRegionId, productInfo.SupplierId);
                if (skuInfo.Stock == 0)
                {
                    continue;//没有库存
                }
                if (skuInfo.Stock < (item.ShipmentQuantity + cartInfoQuantity))//加上购物车中已存在的数量
                {
                    cartItem.Quantity = skuInfo.Stock - cartInfoQuantity;  //库存不足  按现有库存添加
                }
                else
                {
                    cartItem.Quantity = item.ShipmentQuantity; // skuInfo.Stock- cartInfoQuantity;
                }
                #endregion

                #region  检测限购数
                if (productInfo.RestrictionCount > 0 && (cartInfoQuantity + cartItem.Quantity) > productInfo.RestrictionCount)
                {
                    cartItem.Quantity = productInfo.RestrictionCount - cartInfoQuantity;  //大于限购数  按最大限购数添加
                }
                #endregion
                if (cartItem.Quantity <= 0)
                {
                    continue;
                }


                cartItem.Name = productInfo.ProductName;
                cartItem.SKU = skuInfo.SKU;
                cartItem.ProductId = productInfo.ProductId;
                cartItem.UserId = userId;
                cartItem.BrandId = productInfo.BrandId;
                cartItem.RestrictionCount = productInfo.RestrictionCount;
                #region 商家
                if (productInfo.SupplierId > 0)
                {
                    BLL.Shop.Supplier.SupplierInfo supplierManage = new BLL.Shop.Supplier.SupplierInfo();
                    Model.Shop.Supplier.SupplierInfo supplierInfo = supplierManage.GetModelByCache(productInfo.SupplierId);
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
                cartItem.ThumbnailsUrl = productInfo.ThumbnailUrl1;

                cartItem.CostPrice = skuInfo.CostPrice.HasValue ? skuInfo.CostPrice.Value : 0;
                cartItem.MarketPrice = productInfo.MarketPrice.HasValue ? productInfo.MarketPrice.Value : 0;

                cartItem.SellPrice = cartItem.AdjustedPrice = skuInfo.SalePrice;
                cartItem.Weight = skuInfo.Weight.HasValue ? skuInfo.Weight.Value : 0;
                cartItem.Points = (int)(productInfo.Points.HasValue ? productInfo.Points : 0);
                cartItem.Unit = productInfo.Unit;
                cartItemList.Add(cartItem);
            }

            if (cartItemList == null || cartItemList.Count <= 0)
            {
                json.Accumulate(HandlerBase.KEY_STATUS, HandlerBase.STATUS_FAILED);
                json.Accumulate(HandlerBase.KEY_DATA, "NOTCANBUY");//该订单下的商品全部无法购买  可能的情况
                return Content(json.ToString());
            }


            if (IsClearShoppingCart)//清空之前的购物车
            {
                cartHelper.ClearShoppingCart();
            }

            #region 加入购物车
            foreach (YSWL.MALL.Model.Shop.Products.ShoppingCartItem item in cartItemList)
            {
                cartHelper.AddItem(item);
            }
            cartInfo = cartHelper.GetShoppingCart();
            #endregion

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
        #endregion
    }
}