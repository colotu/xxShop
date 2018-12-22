/**
* ShoppingCartHelper.cs
*
* 功 能： [N/A]
* 类 名： ShoppingCartHelper
*
* Ver    变更日期             负责人  变更内容
* ───────────────────────────────────
* V0.01  2013/5/17 15:18:50  Ben    初版
*
* Copyright (c) 2013 YS56 Corporation. All rights reserved.
*┌──────────────────────────────────┐
*│　此技术信息为本公司机密信息，未经本公司书面同意禁止向第三方披露．　│
*│　版权所有：云商未来（北京）科技有限公司　　　　　　　　　　　　　　│
*└──────────────────────────────────┘
*/
using YSWL.MALL.Model.Shop.Products;
using YSWL.ShoppingCart.Core;
using System;
using System.Collections.Generic;
using YSWL.Common;

namespace YSWL.MALL.BLL.Shop.Products
{
    public class ShoppingCartHelper
    {
        private readonly ICartProvider<ShoppingCartInfo, ShoppingCartItem> _cartProvider;

        public ShoppingCartHelper(int userId)
        {
            //使用Cookie购物车
            _cartProvider = new CookieProvider<ShoppingCartInfo, ShoppingCartItem>(userId, false);
            //是否更新价格
          // Li _cartProvider.GetShoppingCart()
        }

        #region 获取要购买的商品 组装成购物车对象
       /// <summary>
        /// 根据要购买的商品 组装成购物车对象 (含有批发规则过滤)
       /// </summary>
        /// <param name="userId">userId</param>
        /// <param name="sku">sku</param>
        /// <param name="count">数量</param>
       /// <param name="countDownId">限时抢购Id</param>
       /// <param name="groupBuyId">团购Id</param>
       /// <param name="accessorie">组合套装Id</param>
       /// <returns></returns>
        public static ShoppingCartInfo GetCartInfoByProduct(int userId, string sku, int count = 1, int countDownId = -1, int groupBuyId = -1, int accessorie = -1,int r=0,int regoinId=0)
        {
            BLL.Shop.Products.SKUInfo skuManage = new BLL.Shop.Products.SKUInfo();
            BLL.Shop.Products.ProductInfo productManage = new BLL.Shop.Products.ProductInfo();
            ShoppingCartInfo cartInfo=new ShoppingCartInfo ();
            if (accessorie > 0)
            {
                #region 组合优惠套装
                BLL.Shop.Products.ProductAccessorie prodAcceBll = new BLL.Shop.Products.ProductAccessorie();
                YSWL.MALL.Model.Shop.Products.ProductAccessorie prodAcceModel = prodAcceBll.GetModel(accessorie);
                if (prodAcceModel == null || prodAcceModel.Type != 2) return null;
                List<Model.Shop.Products.SKUInfo> skulist = skuManage.GetSKUListByAcceId(accessorie, 0);
                if (skulist == null || skulist.Count < 2) return null;//每组商品最少有两条数据
                cartInfo = GetCartInfo4SKU(userId,skulist, prodAcceModel,regoinId);
                #endregion
            }
            else if (string.IsNullOrWhiteSpace(sku))
            {
                YSWL.MALL.BLL.Shop.Products.ShoppingCartHelper cartHelper = new BLL.Shop.Products.ShoppingCartHelper(userId);
                cartInfo = cartHelper.GetShoppingCart4Selected();
                if (cartInfo != null  &&　cartInfo.Items != null)
                {
                    foreach (var item in cartInfo.Items)
                    {
                        item.Stock = YSWL.MALL.BLL.Shop.Products.StockHelper.GetSkuStockByCache(item.SKU, regoinId, item.SupplierId);
                        //获取销售状态
                        item.SaleStatus = skuManage.GetSaleStatus(item.SKU);
                    }
                }
            }
            else
            {
                #region 指定SKU提交订单 此功能已投入使用
                //TODO: 未支持多个SKU BEN ADD 2013-06-23
                Model.Shop.Products.SKUInfo skuInfo = skuManage.GetModelBySKU(sku);
                if (skuInfo == null) return null;

                YSWL.MALL.Model.Shop.Products.ProductInfo productInfo = productManage.GetModel(skuInfo.ProductId);
                if (productInfo == null) return null;

                #region 限时抢购
                YSWL.MALL.Model.Shop.Products.ProductInfo proSaleInfo = null;
                if (countDownId > 0)
                {
                    proSaleInfo = productManage.GetProSaleModel(countDownId);
                    if (proSaleInfo == null) return null;
                    //活动已过期
                    if (DateTime.Now > proSaleInfo.ProSalesEndDate)
                        return null;
                }
                #endregion

                #region 团购
                YSWL.MALL.Model.Shop.Products.ProductInfo groupBuyInfo = null;
                if (groupBuyId > 0)
                {
                    groupBuyInfo = productManage.GetGroupBuyModel(groupBuyId);
                    if (groupBuyInfo == null) return null;
                    //活动已过期   团购数量已达上限
                    if (DateTime.Now > groupBuyInfo.GroupBuy.EndDate || groupBuyInfo.GroupBuy.BuyCount >= groupBuyInfo.GroupBuy.MaxCount)
                        return null;
                }
                #endregion
                cartInfo = GetCartInfo4SKU(userId, productInfo, skuInfo, count, proSaleInfo, groupBuyInfo, r, regoinId);
                #endregion
            }
            if (cartInfo != null && countDownId < 1 && groupBuyId < 1 && accessorie < 1)    //限时抢购/团购/组合套装　不参与批销优惠
            {
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
            }
            return cartInfo;
        }
          


        #region GetCartInfo4SKU
        public static ShoppingCartInfo GetCartInfo4SKU(int userId, YSWL.MALL.Model.Shop.Products.ProductInfo productInfo, YSWL.MALL.Model.Shop.Products.SKUInfo skuInfo, int quantity, YSWL.MALL.Model.Shop.Products.ProductInfo proSaleInfo = null, YSWL.MALL.Model.Shop.Products.ProductInfo groupBuyInfo = null,int r=0,int regionId=0)
        {
            ShoppingCartInfo cartInfo = new ShoppingCartInfo();
            //TODO: 未支持多个SKU BEN ADD 2013-06-23
            ShoppingCartItem cartItem = new ShoppingCartItem();
            cartItem.MarketPrice = productInfo.MarketPrice.HasValue ? productInfo.MarketPrice.Value : 0;
            cartItem.Name = productInfo.ProductName;
            cartItem.Quantity = quantity < 1 ? 1 : quantity;
            cartItem.SellPrice = skuInfo.SalePrice;
            cartItem.AdjustedPrice = skuInfo.SalePrice;
            cartItem.SKU = skuInfo.SKU;
            cartItem.ProductId = skuInfo.ProductId;
            cartItem.UserId = userId;
            cartItem.BrandId = productInfo.BrandId;
            cartItem.ReferId = r;
            #region 限时抢购价格处理
            if (proSaleInfo != null)
            {
                //重置价格为 限时抢购价
                cartItem.AdjustedPrice = proSaleInfo.ProSalesPrice;
            }
            #endregion

            #region 团购价格处理
            if (groupBuyInfo != null)
            {
                //重置价格为 限时抢购价
                cartItem.AdjustedPrice = groupBuyInfo.GroupBuy.Price;
            }
            #endregion

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

            BLL.Shop.Products.SKUInfo skuManage = new BLL.Shop.Products.SKUInfo();
            //DONE: 根据SkuId 获取SKUItem值和图片数据 BEN 2013-06-30
            List<Model.Shop.Products.SKUItem> listSkuItems = skuManage.GetSKUItemsBySkuId(skuInfo.SkuId);
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
            cartItem.Weight = skuInfo.Weight.HasValue ? skuInfo.Weight.Value : 0;
            cartItem.Points = (int)(productInfo.Points.HasValue ? productInfo.Points : 0);
            cartItem.Stock = YSWL.MALL.BLL.Shop.Products.StockHelper.GetSkuStockByCache(skuInfo.SKU, regionId, productInfo.SupplierId);
            //获取销售状态
            cartItem.SaleStatus = productInfo.SaleStatus == 1 ? (skuInfo.Upselling ? 1 : 0) : productInfo.SaleStatus;
            cartInfo.Items.Add(cartItem);
            return cartInfo;
        }
        public static ShoppingCartInfo GetCartInfo4SKU(int userId, List<YSWL.MALL.Model.Shop.Products.SKUInfo> skulist, YSWL.MALL.Model.Shop.Products.ProductAccessorie model,int regionId)
        {
            BLL.Shop.Products.ProductInfo proBLL = new BLL.Shop.Products.ProductInfo();
            BLL.Shop.Products.SKUInfo skuManage = new BLL.Shop.Products.SKUInfo();
            decimal totalPrice = 0M;//原　　价
            decimal dealsPrices = 0M;//总优惠金额
            foreach (var item in skulist)
            {
                totalPrice += item.SalePrice;
            }
            dealsPrices = totalPrice - model.DiscountAmount;
            //decimal dealsPrice = dealsPrices / skulist.Count;//单个商品优惠的金额
            ShoppingCartInfo cartInfo = new ShoppingCartInfo();
            ShoppingCartItem cartItem;
            YSWL.MALL.Model.Shop.Products.ProductInfo productInfo;
            foreach (var item in skulist)
            {
                cartItem = new ShoppingCartItem();
                cartItem.MarketPrice = item.MarketPrice.HasValue ? item.MarketPrice.Value : 0;
                cartItem.Name = item.ProductName;
                cartItem.Quantity = 1;
                cartItem.SellPrice = item.SalePrice;
                cartItem.AdjustedPrice = item.SalePrice;// -dealsPrice;
                cartItem.SKU = item.SKU;
                cartItem.ProductId = item.ProductId;
                cartItem.UserId = userId;
                productInfo = proBLL.GetModelByCache(item.ProductId);
                if (null != productInfo)
                {
                    cartItem.BrandId = productInfo.BrandId;
                    //获取销售状态
                    cartItem.SaleStatus = productInfo.SaleStatus == 1 ? (item.Upselling ? 1 : 0) : productInfo.SaleStatus;
                }
                else
                {
                    cartItem.BrandId = -1;
                    cartItem.SaleStatus = 2;
                }
                //DONE: 根据SkuId 获取SKUItem值和图片数据 BEN 2013-06-30
                List<YSWL.MALL.Model.Shop.Products.SKUItem> listSkuItems = item.SkuItems;
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
                cartItem.ThumbnailsUrl = item.ProductThumbnailUrl;
                cartItem.CostPrice = item.CostPrice.HasValue ? item.CostPrice.Value : 0;
                cartItem.Weight = item.Weight.HasValue ? item.Weight.Value : 0;
                cartItem.Stock = YSWL.MALL.BLL.Shop.Products.StockHelper.GetSkuStockByCache(item.SKU, regionId, productInfo.SupplierId);
                #region 商家
                if (item.SupplierId > 0)
                {
                    BLL.Shop.Supplier.SupplierInfo supplierManage = new BLL.Shop.Supplier.SupplierInfo();
                    Model.Shop.Supplier.SupplierInfo supplierInfo = supplierManage.GetModelByCache(item.SupplierId);
                    if (supplierInfo != null)
                    {
                        cartItem.SupplierId = supplierInfo.SupplierId;
                        cartItem.SupplierName = supplierInfo.Name;
                        cartItem.ShopName = supplierInfo.ShopName;
                    }
                }
                #endregion

                cartInfo.Items.Add(cartItem);
            }
            cartInfo.TotalRate = dealsPrices;
            return cartInfo;
        }
        #endregion


        #region  预订商品订单
        public static ShoppingCartInfo GetCartInfoByPre(int userId,YSWL.MALL.Model.Shop.Products.ProductInfo productInfo, YSWL.MALL.Model.Shop.Products.SKUInfo skuInfo, int quantity, YSWL.MALL.Model.Shop.PrePro.PreProduct preProduct )
        {

          
            ShoppingCartInfo cartInfo = new ShoppingCartInfo();
            //TODO: 未支持多个SKU BEN ADD 2013-06-23
            ShoppingCartItem cartItem = new ShoppingCartItem();
            cartItem.MarketPrice = productInfo.MarketPrice.HasValue ? productInfo.MarketPrice.Value : 0;
            cartItem.Name = productInfo.ProductName;
            cartItem.Quantity = quantity < 1 ? 1 : quantity;
            cartItem.SellPrice = skuInfo.SalePrice;
            cartItem.AdjustedPrice = skuInfo.SalePrice;
            cartItem.SKU = skuInfo.SKU;
            cartItem.ProductId = skuInfo.ProductId;
            cartItem.UserId = userId;
            cartItem.BrandId = productInfo.BrandId;

            #region 商家
            if (productInfo.SupplierId > 0)
            {
                BLL.Shop.Supplier.SupplierInfo supplierManage = new BLL.Shop.Supplier.SupplierInfo();
                Model.Shop.Supplier.SupplierInfo supplierInfo = supplierManage.GetModelByCache(productInfo.SupplierId);
                if (supplierInfo != null)
                {
                    cartItem.SupplierId = supplierInfo.SupplierId;
                    cartItem.SupplierName = supplierInfo.Name;
                }
            }
            #endregion

            BLL.Shop.Products.SKUInfo skuManage = new BLL.Shop.Products.SKUInfo();
            //DONE: 根据SkuId 获取SKUItem值和图片数据 BEN 2013-06-30
            List<Model.Shop.Products.SKUItem> listSkuItems = skuManage.GetSKUItemsBySkuId(skuInfo.SkuId);
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
            cartItem.SellPrice = preProduct.PreAmount; 
            cartInfo.Items.Add(cartItem);
            return cartInfo;
        }

        #endregion 

        #endregion

        #region ICartProvider<ShoppingCartInfo,ShoppingCartItem> 成员

        public static void LoadShoppingCart(int userId)
        {
            CookieProvider<ShoppingCartInfo, ShoppingCartItem>.LoadShoppingCart(userId,false);
        }

        public void AddItem(ShoppingCartItem itemInfo)
        {
            _cartProvider.AddItem(itemInfo);
        }

        public void ClearShoppingCart()
        {
            _cartProvider.ClearShoppingCart();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="isAdminOrder">是否是后台代下单</param>
        /// <returns></returns>
        public ShoppingCartInfo GetShoppingCart(bool isAdminOrder=false)
        {
            ShoppingCartInfo cartInfo = _cartProvider.GetShoppingCart();
            if (isAdminOrder) {//后台代下单不需要实时更新价格，因为代下单可以修改价格
                return cartInfo;
            }
            //是否需要实时更新价格
            bool IsUpdate = YSWL.MALL.BLL.SysManage.ConfigSystem.GetBoolValueByCache("Shop_ShoppingCart_IsUpdate");
            if (IsUpdate)
            {
                YSWL.MALL.BLL.Shop.Products.SKUInfo skuBll=new SKUInfo();
                ShoppingCartInfo newcartInfo =new ShoppingCartInfo();
                //更新价格
                foreach (var item in cartInfo.Items)
                {
                    //获取SKU商品
                   YSWL.MALL.Model.Shop.Products.SKUInfo skuInfo= skuBll.GetModelBySKU(item.SKU);
                   if (skuInfo!=null) 
                    {
                        item.SellPrice = skuInfo.SalePrice;
                        item.AdjustedPrice = skuInfo.SalePrice;
                        newcartInfo.Items.Add(item);
                    }
                }
                _cartProvider.SaveShoppingCart(newcartInfo);
            }
            return cartInfo;
        }

        public ShoppingCartInfo GetShoppingCart4Selected()
        {
            return _cartProvider.GetShoppingCart4Selected();
        }

        public void RemoveItem(int itemId)
        {
            _cartProvider.RemoveItem(itemId);
        }

        public void UpdateItemQuantity(int itemId, int quantity)
        {
            _cartProvider.UpdateItemQuantity(itemId, quantity);
        }


        public void SaveShoppingCart(ShoppingCartInfo cartInfo)
        {
            _cartProvider.SaveShoppingCart(cartInfo);
        }
  
        public void UpdateItemPrice(int itemId, decimal price)
        {
            ShoppingCartInfo cartInfo = GetShoppingCart(true);
            if (cartInfo.Items == null || cartInfo.Items.Count < 1) return;

            cartInfo.Items.ForEach(xx => { if (xx.ItemId == itemId) xx.AdjustedPrice = price; });
            _cartProvider.SaveShoppingCart(cartInfo);
        }


        #region  购物车分组

        public static Dictionary<int, List<YSWL.MALL.Model.Shop.Products.ShoppingCartItem>> GetSuppCartItems(
            List<ShoppingCartItem> Items)
        {
            #region 根据商家分组

            Dictionary<int, List<ShoppingCartItem>> dicSuppCartItems = new Dictionary<int, List<ShoppingCartItem>>();
            //购物车 -> 订单项目
            Items.ForEach(item =>
            {
                //填充商家购物车项
                if (item.SupplierId.HasValue && item.SupplierId.Value > 0)
                {
                    if (dicSuppCartItems.ContainsKey(item.SupplierId.Value))
                    {
                        dicSuppCartItems[item.SupplierId.Value].Add(item);
                    }
                    else
                    {
                        dicSuppCartItems.Add(item.SupplierId.Value,
                            new List<ShoppingCartItem> {item});
                    }
                }
                else
                {
                    if (dicSuppCartItems.ContainsKey(0))
                    {
                        dicSuppCartItems[0].Add(item);
                    }
                    else
                    {
                        dicSuppCartItems.Add(0,
                            new List<ShoppingCartItem> {item});
                    }
                }
            });
            return dicSuppCartItems;

            #endregion
        }

        #endregion

        /// <summary>
        /// 按商家分组计算运费
        /// </summary>
        /// <returns>运费</returns>
        public static decimal CalcFreightGroup(ShoppingCartInfo cartInfo)
        {
            YSWL.MALL.BLL.Shop.Shipping.ShippingType bll = new Shipping.ShippingType();
            if (cartInfo == null)
            {
                return 0;
            }

            if (cartInfo.Items == null || cartInfo.Items.Count > 0)
            {
                return 0;
            }

            var dic = GetSuppCartItems(cartInfo.Items);

            int shoplWeight = 0; //商家配送总重

            decimal shopPrice = 0; //商家配送总运费

            int terraceWeight = 0; //平台配送商品总重
            decimal terracePrice = 0; //平台配送商品总运费

            foreach (var di in dic)
            {
                int weight = 0;
                decimal price = 0;

                if (BLL.Shop.Supplier.SupplierConfig.GetShipTypeBycahe(di.Key) == 0) //平台配送
                {
                    foreach (var item in di.Value)
                    {
                        weight = weight + item.Weight*item.Quantity; //获取平台配送商品总重
                    }
                    terraceWeight += weight;

                    continue;
                }


                YSWL.MALL.Model.Shop.Shipping.ShippingType shipModel = bll.GetCacgeModelSupplied(di.Key);
                if (shipModel != null)
                {
                    foreach (var item in di.Value)
                    {
                        weight = weight + item.Weight * item.Quantity; //获取商家商品总重
                    }
                    price = shipModel.Price; //商品首重价格

                    #region 商家商品续重价格

                    if ((shipModel.AddWeight ?? 0) != 0 && (shipModel.AddPrice ?? 0) != 0) //如果有续重和续费  考虑到续重值被设置为0的情况
                    {
                        if (weight - shipModel.Weight > 0)
                        {
                            price = price +
                                    Math.Ceiling((decimal)(weight - shipModel.Weight) / shipModel.AddWeight.Value) *
                                    shipModel.AddPrice.Value; //商品续重价格
                        }
                    }
                }
               

                #endregion

                shoplWeight = shoplWeight + weight;
                shopPrice = shopPrice + price;


            }



            #region 平台商品运费统一计算

            YSWL.MALL.Model.Shop.Shipping.ShippingType terraceShip = bll.GetCacgeModelSupplied(-1);
            if (terraceShip != null)
            {

                terracePrice = terraceShip.Price; //商品首重价格

                #region 平台商品续重价格

                if ((terraceShip.AddWeight ?? 0) != 0 && (terraceShip.AddPrice ?? 0) != 0) //如果有续重和续费  考虑到续重值被设置为0的情况
                {
                    if (terraceWeight - terraceShip.Weight > 0)
                    {
                        terracePrice = terracePrice +
                                       Math.Ceiling((decimal)(terraceWeight - terraceShip.Weight) /
                                                    terraceShip.AddWeight.Value) * terraceShip.AddPrice.Value; //商品续重价格
                    }
                }
            }


            #endregion

            #endregion
            return terracePrice + shopPrice;
        }


        /// <summary>
        /// 按商家分组计算运费 支持区域运费
        /// </summary>
        /// <param name="cartInfo"></param>
        /// <param name="regionInfo"></param>
        /// <returns></returns>
        public static decimal CalcFreightGroup(ShoppingCartInfo cartInfo, Model.Ms.Regions regionInfo)
        {
            BLL.Shop.Shipping.ShippingRegionGroups _shippingRegionManage = new BLL.Shop.Shipping.ShippingRegionGroups();

            #region 获取topRegionId
            int topRegionId = -1;
            if (regionInfo != null)
            {
                if (regionInfo.Depth > 1)
                {
                    topRegionId = Globals.SafeInt(regionInfo.Path.Split(new[] {','})[1], -1);
                }
                else
                {
                    topRegionId = regionInfo.RegionId;
                }
            }

            #endregion

            YSWL.MALL.BLL.Shop.Shipping.ShippingType bll = new Shipping.ShippingType();
            if (cartInfo == null)
            {
                return 0;
            }
            if (cartInfo.Items == null || cartInfo.Items.Count < 1)
            {
                return 0;
            }

            var dic = GetSuppCartItems(cartInfo.Items);

            int shoplWeight = 0; //商家配送总重

            decimal shopPrice = 0; //商家配送总运费

            int terraceWeight = 0; //平台配送商品总重
            decimal terracePrice = 0; //平台配送商品总运费


            foreach (var di in dic)
            {
                int weight = 0;
                decimal price = 0;
                #region 获取平台配送商品总重
                if (BLL.Shop.Supplier.SupplierConfig.GetShipTypeBycahe(di.Key) == 0) //平台配送
                {
                    foreach (var item in di.Value)
                    {
                        weight = weight + item.Weight*item.Quantity; 
                    }
                    terraceWeight += weight;

                    continue;
                }
                #endregion

                #region 计算商家配送商品运费
                YSWL.MALL.Model.Shop.Shipping.ShippingType shipModel = bll.GetCacgeModelSupplied(di.Key);
                if (shipModel != null)
                {
                    #region 获取区域运费model         
                    Model.Shop.Shipping.ShippingRegionGroups shippingRegion =
                            _shippingRegionManage.GetShippingRegion(shipModel.ModeId, topRegionId);
                    #endregion
                    foreach (var item in di.Value)
                    {
                        weight = weight + item.Weight * item.Quantity; //获取商家商品总重
                    }
                    if (shippingRegion != null) //区域价格
                    {
                        price = shippingRegion.Price; //商品首重价格
                        #region 续重价格

                        if ((shipModel.AddWeight ?? 0) != 0 && (shippingRegion.AddPrice ?? 0) != 0) //如果有续重和续费  考虑到续重值被设置为0的情况
                        {
                            if (weight - shipModel.Weight > 0)
                            {
                                price = price +
                                        Math.Ceiling((decimal)(weight - shipModel.Weight) / shipModel.AddWeight.Value) *
                                        shippingRegion.AddPrice.Value; //商品续重价格
                            }
                        }

                        #endregion
                    }
                    else
                    {
                        price = shipModel.Price; //商品首重价格

                        #region 续重价格

                        if ((shipModel.AddWeight ?? 0) != 0 && (shipModel.AddPrice ?? 0) != 0) //如果有续重和续费  考虑到续重值被设置为0的情况
                        {
                            if (weight - shipModel.Weight > 0)
                            {
                                price = price +
                                        Math.Ceiling((decimal)(weight - shipModel.Weight) / shipModel.AddWeight.Value) *
                                        shipModel.AddPrice.Value; //商品续重价格
                            }
                        }

                        #endregion
                    }
                }
                
                #endregion
                shoplWeight = shoplWeight + weight;
                shopPrice = shopPrice + price;
            }



            #region 平台商品运费统一计算

            YSWL.MALL.Model.Shop.Shipping.ShippingType terraceShip = bll.GetCacgeModelSupplied(0);
            if (terraceShip != null)
            {
                Model.Shop.Shipping.ShippingRegionGroups shippingReg =
                       _shippingRegionManage.GetShippingRegion(terraceShip.ModeId, topRegionId);
                if (shippingReg != null)
                {
                    terracePrice = shippingReg.Price; //商品首重价格

                    #region 平台商品续重价格

                    if ((terraceShip.AddWeight ?? 0) != 0 && (shippingReg.AddPrice ?? 0) != 0) //如果有续重和续费  考虑到续重值被设置为0的情况
                    {
                        if (terraceWeight - terraceShip.Weight > 0)
                        {
                            terracePrice = terracePrice +
                                           Math.Ceiling((decimal)(terraceWeight - terraceShip.Weight) /
                                                        terraceShip.AddWeight.Value) * shippingReg.AddPrice.Value; //商品续重价格
                        }
                    }

                    #endregion
                }
                else
                {
                    terracePrice = terraceShip.Price; //商品首重价格

                    #region 平台商品续重价格

                    if ((terraceShip.AddWeight ?? 0) != 0 && (terraceShip.AddPrice ?? 0) != 0) //如果有续重和续费  考虑到续重值被设置为0的情况
                    {
                        if (terraceWeight - terraceShip.Weight > 0)
                        {
                            terracePrice = terracePrice +
                                           Math.Ceiling((decimal)(terraceWeight - terraceShip.Weight) /
                                                        terraceShip.AddWeight.Value) * terraceShip.AddPrice.Value; //商品续重价格
                        }
                    }

                    #endregion
                }
            }
            


            #endregion
            return terracePrice + shopPrice;
        }

        /// <summary>
        /// 按商家分组计算运费 支持区域运费 支持包邮
        /// </summary>
        /// <param name="cartInfo"></param>
        /// <param name="regionInfo"></param>
        /// <param name=""></param>
        /// <returns></returns>
        public static decimal CalcFreightGroup(ShoppingCartInfo cartInfo, Model.Ms.Regions regionInfo, int buyerId)
        {
            BLL.Shop.Shipping.ShippingRegionGroups _shippingRegionManage = new BLL.Shop.Shipping.ShippingRegionGroups();
            BLL.Shop.Activity.ActivityInfo activInfoBll = new BLL.Shop.Activity.ActivityInfo();
            #region 获取topRegionId
            int topRegionId = -1;
            if (regionInfo != null)
            {
                if (regionInfo.Depth > 1)
                {
                    topRegionId = Globals.SafeInt(regionInfo.Path.Split(new[] { ',' })[1], -1);
                }
                else
                {
                    topRegionId = regionInfo.RegionId;
                }
            }

            #endregion

            YSWL.MALL.BLL.Shop.Shipping.ShippingType bll = new Shipping.ShippingType();
            if (cartInfo == null)
            {
                return 0;
            }
            if (cartInfo.Items == null || cartInfo.Items.Count < 1)
            {
                return 0;
            }

            var dic = GetSuppCartItems(cartInfo.Items);

            int shoplWeight = 0; //商家配送总重

            decimal shopPrice = 0; //商家配送总运费

            int terraceWeight = 0; //平台配送商品总重
            decimal terracePrice = 0; //平台配送商品总运费


            foreach (var di in dic)
            {
                int weight = 0;
                decimal price = 0;
                #region 获取平台配送商品总重 (平台运费在后面计算)
                if (BLL.Shop.Supplier.SupplierConfig.GetShipTypeBycahe(di.Key) == 0) //平台配送
                {
                    foreach (var item in di.Value)
                    {
                        weight = weight + item.Weight*item.Quantity;
                    }
                    terraceWeight += weight;

                    continue;
                }
                #endregion

                #region 计算商家配送商品运费
                YSWL.MALL.Model.Shop.Shipping.ShippingType shipModel = bll.GetCacgeModelSupplied(di.Key);
                if (shipModel != null)
                {
                    #region 获取区域运费model         
                    Model.Shop.Shipping.ShippingRegionGroups shippingRegion =
                            _shippingRegionManage.GetShippingRegion(shipModel.ModeId, topRegionId);
                    #endregion
                    foreach (var item in di.Value)
                    {
                        weight = weight + item.Weight * item.Quantity; //获取商家商品总重
                    }
                    if (weight > 0)//重量为0 不计算运费
                    {
                        if (shippingRegion != null) //区域价格
                        {
                            price = shippingRegion.Price; //商品首重价格
                            #region 续重价格

                            if ((shipModel.AddWeight ?? 0) != 0 && (shippingRegion.AddPrice ?? 0) != 0) //如果有续重和续费  考虑到续重值被设置为0的情况
                            {
                                if (weight - shipModel.Weight > 0)
                                {
                                    price = price +
                                            Math.Ceiling((decimal)(weight - shipModel.Weight) / shipModel.AddWeight.Value) *
                                            shippingRegion.AddPrice.Value; //商品续重价格
                                }
                            }

                            #endregion
                        }
                        else
                        {
                            price = shipModel.Price; //商品首重价格

                            #region 续重价格

                            if ((shipModel.AddWeight ?? 0) != 0 && (shipModel.AddPrice ?? 0) != 0) //如果有续重和续费  考虑到续重值被设置为0的情况
                            {
                                if (weight - shipModel.Weight > 0)
                                {
                                    price = price +
                                            Math.Ceiling((decimal)(weight - shipModel.Weight) / shipModel.AddWeight.Value) *
                                            shipModel.AddPrice.Value; //商品续重价格
                                }
                            }

                            #endregion
                        }
                    }
                }

                
                
               

                if (activInfoBll.IsFreeShippByCareItem(di, buyerId))
                {
                    price = 0;
                }
                shoplWeight = shoplWeight + weight;
                shopPrice = shopPrice + price;
                #endregion
            }



            #region 平台商品运费统一计算

            if (terraceWeight > 0)
            {
                YSWL.MALL.Model.Shop.Shipping.ShippingType terraceShip = bll.GetCacgeModelSupplied(0);
                if(terraceShip != null)
                {
                    Model.Shop.Shipping.ShippingRegionGroups shippingReg =
                           _shippingRegionManage.GetShippingRegion(terraceShip.ModeId, topRegionId);
                    if (shippingReg != null)
                    {
                        terracePrice = shippingReg.Price; //商品首重价格

                        #region 平台商品续重价格

                        if ((terraceShip.AddWeight ?? 0) != 0 && (shippingReg.AddPrice ?? 0) != 0) //如果有续重和续费  考虑到续重值被设置为0的情况
                        {
                            if (terraceWeight - terraceShip.Weight > 0)
                            {
                                terracePrice = terracePrice +
                                               Math.Ceiling((decimal)(terraceWeight - terraceShip.Weight) /
                                                            terraceShip.AddWeight.Value) * shippingReg.AddPrice.Value; //商品续重价格
                            }
                        }

                        #endregion
                    }
                    else
                    {
                        terracePrice = terraceShip.Price; //商品首重价格

                        #region 平台商品续重价格

                        if ((terraceShip.AddWeight ?? 0) != 0 && (terraceShip.AddPrice ?? 0) != 0) //如果有续重和续费  考虑到续重值被设置为0的情况
                        {
                            if (terraceWeight - terraceShip.Weight > 0)
                            {
                                terracePrice = terracePrice +
                                               Math.Ceiling((decimal)(terraceWeight - terraceShip.Weight) /
                                                            terraceShip.AddWeight.Value) * terraceShip.AddPrice.Value; //商品续重价格
                            }
                        }

                        #endregion
                    }
                }
                #region 计算平台商品包邮  遍历所有由平台配送的购物车项 如果有一项包邮 则全部包邮
                foreach (var di in dic)
                {
                    if (BLL.Shop.Supplier.SupplierConfig.GetShipTypeBycahe(di.Key) == 0)
                    {
                        if (activInfoBll.IsFreeShippByCareItem(new KeyValuePair<int, List<YSWL.MALL.Model.Shop.Products.ShoppingCartItem>>(di.Key, di.Value), buyerId))
                        {
                            terracePrice = 0;
                            break;
                        }
                    }
                }
            }
            
            #endregion





            #endregion


            return terracePrice + shopPrice;
        }

        #endregion


    }
}
