/**
* SubmitOrderHandler.cs
*
* 功 能： 生成订单
* 类 名： SubmitOrderHandler
*
* Ver    变更日期             负责人  变更内容
* ───────────────────────────────────
* V0.01  2013/5/20 17:06:54  Ben    初版
*
* Copyright (c) 2013 YS56 Corporation. All rights reserved.
*┌──────────────────────────────────┐
*│　此技术信息为本公司机密信息，未经本公司书面同意禁止向第三方披露．　│
*│　版权所有：小鸟科技（北京）科技有限公司　　　　　　　　　　　　　　│
*└──────────────────────────────────┘
*/

using System;
using System.Web;
using System.Web.SessionState;
using YSWL.MALL.BLL.Ms;
using YSWL.Common;
using YSWL.Json;
using System.Collections.Generic;
using System.Linq;
using SKUInfo = YSWL.MALL.Model.Shop.Products.SKUInfo;
using SKUItem = YSWL.MALL.Model.Shop.Products.SKUItem;
using YSWL.Json.Conversion;
using YSWL.MALL.Model.Shop;
using YSWL.MALL.Model.Shop.Order;
using YSWL.MALL.Model.Shop.Products;

namespace YSWL.MALL.Web.Handlers.AdminOrder
{
    public class OrderHandler : HandlerBase, IRequiresSessionState
    {
        private readonly BLL.Ms.Regions _regionManage = new BLL.Ms.Regions();
        private readonly BLL.Shop.Shipping.ShippingType _shippingTypeManage = new BLL.Shop.Shipping.ShippingType();
        private readonly BLL.Shop.Shipping.ShippingAddress _shippingAddressManage =  new BLL.Shop.Shipping.ShippingAddress();
        private readonly BLL.Shop.Shipping.ShippingRegionGroups _shippingRegionManage =  new BLL.Shop.Shipping.ShippingRegionGroups();
        private BLL.Shop.Products.ProductInfo _productInfoManage = new BLL.Shop.Products.ProductInfo();
        private BLL.Shop.Products.SKUInfo _skuInfoManage = new BLL.Shop.Products.SKUInfo();
        private  YSWL.MALL.BLL.Members.Users userBll=new BLL.Members.Users();
        private YSWL.MALL.BLL.Shop.Products.BrandInfo brandInfoBll = new BLL.Shop.Products.BrandInfo();
        #region IHttpHandler 成员

        public override bool IsReusable
        {
            get { return false; }
        }

        public override void ProcessRequest(HttpContext context)
        {
            string action = context.Request.Form["Action"];

            context.Response.Clear();
            context.Response.ContentType = "application/json";
             if (CurrentUser == null || CurrentUser.UserType != "AA")
            {
                JsonObject result = new JsonObject();
                result.Accumulate(KEY_STATUS, STATUS_NOLOGIN);
                context.Response.Write(result.ToString());
            }
            try
            {
                switch (action)
                {
                    case "SubmitOrder":
                        context.Response.Write(SubmitOrder(context));
                        break;
                    default:
                        break;
                }
            }
            catch (Exception ex)
            {
                LogHelp.AddErrorLog(ex.Message,ex.StackTrace);
                JsonObject json = new JsonObject();
                json.Put(KEY_STATUS, STATUS_ERROR);
                json.Put(KEY_DATA, ex);
                context.Response.Write(json.ToString());
            }
        }

        #endregion

        #region 提交订单

        private string SubmitOrder(HttpContext context)
        {
            JsonObject result = new JsonObject();
            #region 获取基础数据
            string orderRemark = context.Request.Form["Remark"];
            if (!string.IsNullOrWhiteSpace(orderRemark))
            {
                orderRemark = InjectionFilter.Filter(orderRemark);
            }

            int payType = Globals.SafeInt(context.Request.Form["payType"], 1); //1  货到付款    2 已支付
            if (payType != 2 && payType != 1) {
                result.Accumulate(KEY_STATUS, "PayTypeISNULL");//支付类型不正确
                return result.ToString();
            }
            #endregion

            #region 1.获取购买人
            //DONE: 1.获取购买人
            YSWL.Accounts.Bus.User userBuyer = GetBuyerUserInfo();
            if (userBuyer == null)
            {
                result.Accumulate(KEY_STATUS, STATUS_NOLOGIN);
                return result.ToString();
            }
            #endregion

            #region 2.获取收货人

            //DONE: 3.获取收货人
            Model.Shop.Shipping.ShippingAddress shippingAddress = GetShippingAddress();
            if (shippingAddress == null)
            {
                result.Accumulate(KEY_STATUS, "NOSHIPPINGADDRESS");
                return result.ToString();
            }
            Model.Ms.Regions regionInfo = _regionManage.GetModelByCache(shippingAddress.RegionId);
            if (regionInfo == null)
            {
                result.Accumulate(KEY_STATUS, "NOREGIONINFO");
                return result.ToString();
            }
            #endregion

            #region 4.获取购物车

            //DONE: 2.获取购物车
            YSWL.MALL.BLL.Shop.Products.ShoppingCartHelper shoppingCartHelper;
            Model.Shop.Products.ShoppingCartInfo shoppingCartInfo;
            try
            {
                shoppingCartInfo = GetShoppingCart( userBuyer, out shoppingCartHelper);
            }
            catch (ArgumentNullException)
            {
                result.Accumulate(KEY_STATUS, "PROSALEEXPIRED");
                return result.ToString();
            }
            if (shoppingCartInfo == null ||
                shoppingCartInfo.Items == null ||
                shoppingCartInfo.Items.Count < 1)
            {
                result.Accumulate(KEY_STATUS, "NOSHOPPINGCARTINFO");
                return result.ToString();
            }
            //DONE: 2.1 Check 商品库存
            List<ShoppingCartItem> noStockList = new List<ShoppingCartItem>();
            List<ShoppingCartItem> greaRestCountList = new List<ShoppingCartItem>();
            int stock = 0;
            int restCount = 0;
            foreach (ShoppingCartItem item in shoppingCartInfo.Items)
            {
                stock=YSWL.MALL.BLL.Shop.Products.StockHelper.GetSkuStockEx(item.SKU, DepotId, null);
                //检查购买数量是否大于库存
                if (item.Quantity < 1 || item.Quantity > stock)
                {
                    noStockList.Add(item);
                }
                restCount = _productInfoManage.GetRestrictionCount(item.ProductId);
                if (restCount > 0 && item.Quantity > restCount)
                {
                    greaRestCountList.Add(item);
                }
            }
            if (noStockList.Count > 0)
            {
                result.Accumulate(KEY_STATUS, "NOSTOCK");
                result.Accumulate(KEY_DATA, noStockList);
                return result.ToString();
            }
            if (greaRestCountList.Count > 0)
            {
                result.Accumulate(KEY_STATUS, "GreaRestCount");//超出限购数量
                result.Accumulate(KEY_DATA, greaRestCountList);
                ////自动修改Cookie/DB购物车中超出限购数量的项目
                //if (shoppingCartHelper != null)
                //{
                //    greaRestCountList.ForEach(info =>
                //    {
                //        //TODO: 修改购物车中的数量
                //        shoppingCartHelper.UpdateItemQuantity(info.ItemId, _productInfoManage.GetRestrictionCount(info.ProductId));
                //    });
                //}
                return result.ToString();
            }
            #endregion

            #region 5.获取支付信息
            Payment.Model.PaymentModeInfo payModel = GetPayType(payType);   
            //DONE: 5.获取支付信息
            if (payModel == null)
            {
                result.Accumulate(KEY_STATUS, "NOPAYTYPE");
                return result.ToString();
            }

            #endregion


            #region 6.获取配送(物流)

            //DONE: 6.获取配送(物流)
            Model.Shop.Shipping.ShippingType  shippingType = GetShippingType();
            if (shippingType == null)
            {
                result.Accumulate(KEY_STATUS, "NOSHIPPINGTYPE");
                return result.ToString();
            }


            #endregion

            #region 7.生成订单
            //DONE: 7.生成订单
            OrderInfo mainOrder = new OrderInfo();
            #region 填充订单数据
            
            #region 基础数据

            #region 下单类型
            mainOrder.ReferType = (int)YSWL.MALL.Model.Shop.Order.EnumHelper.ReferType.Cust;
            #endregion

            bool codePre = BLL.SysManage.ConfigSystem.GetBoolValueByCache("Shop_CreateOrder_PreCode");
            mainOrder.CreateUserId = CurrentUser.UserID;
            mainOrder.CreatedDate = DateTime.Now;
            if (codePre)//订单需要加前缀
            {
                mainOrder.OrderCode = mainOrder.ReferOrderPrefix + mainOrder.CreatedDate.ToString("yyyyMMddHHmmssfff");
            }
            else
            {
                mainOrder.OrderCode = mainOrder.CreatedDate.ToString("yyyyMMddHHmmssfff");
            }
            mainOrder.Remark = orderRemark;

            #region 优惠券数据
            mainOrder.CouponAmount = 0;
            #endregion

            #region 重量/运费/积分

            #region 区域差异运费计算
            int topRegionId;
            if (regionInfo.Depth > 1)
            {
                topRegionId = Globals.SafeInt(regionInfo.Path.Split(new[] { ',' })[1], -1);
            }
            else
            {
                topRegionId = regionInfo.RegionId;
            }

            Model.Shop.Shipping.ShippingRegionGroups shippingRegion =
                _shippingRegionManage.GetShippingRegion(shippingType.ModeId, topRegionId);
            #endregion

            mainOrder.Weight = shoppingCartInfo.TotalWeight;

            //程序计算出的运费
            decimal Freight = shoppingCartInfo.CalcFreight(shippingType, shippingRegion);
            mainOrder.Freight =mainOrder.FreightAdjusted= mainOrder.FreightActual = Freight;
            //外面设置的运费 
            decimal newFreight= Globals.SafeDecimal(context.Request.Form["freight"],-1); //提交订单时可以修改运费
            mainOrder.FreightAdjusted = newFreight >= 0 ? newFreight : Freight;
            mainOrder.IsFreeShipping =false;                 
            mainOrder.OrderPoint = shoppingCartInfo.TotalPoints;
            #endregion

 
            #region 订单价格

            //订单商品总价(无任何优惠)
            mainOrder.ProductTotal = shoppingCartInfo.TotalSellPrice;

            //订单总成本价 = 商品总成本价
            mainOrder.OrderCostPrice = shoppingCartInfo.TotalCostPrice;

            //订单总金额(无任何优惠) 商品总价 + 运费
            mainOrder.OrderTotal = shoppingCartInfo.TotalSellPrice + mainOrder.Freight.Value;

            //订单最终支付金额 = 项目调整后总售价 + 调整后运费-优惠券价格
            decimal amount = shoppingCartInfo.TotalAdjustedPrice + mainOrder.FreightAdjusted.Value - mainOrder.CouponAmount.Value;
            mainOrder.Amount = amount > 0 ? amount : 0;
            //折扣金额
            mainOrder.DiscountAdjusted = shoppingCartInfo.TotalSellPrice - shoppingCartInfo.TotalAdjustedPrice;
            decimal totalRate = shoppingCartInfo.TotalRate; //总价优惠值
            if (mainOrder.Amount < 0)
            {
                LogHelp.AddInvadeLog(
                    string.Format("非法订单金额|{0}|_YSWL.Web.Handlers.Shop.OrderHandler.SubmitOrder",
                        mainOrder.Amount.ToString("F2")), HttpContext.Current.Request);
                result.Accumulate(KEY_STATUS, "ILLEGALORDERAMOUNT");
                return result.ToString();
            }

            #endregion

            mainOrder.OrderType = 1;
            #endregion

            #region 购买人信息

            mainOrder.BuyerID = userBuyer.UserID;
            mainOrder.BuyerName = userBuyer.UserName; //String.IsNullOrWhiteSpace(userBuyer.TrueName) ? userBuyer.UserName : userBuyer.TrueName;
            mainOrder.ReferID = userBuyer.EmployeeID.ToString();
            mainOrder.BuyerEmail = String.IsNullOrWhiteSpace(userBuyer.Email)?"": userBuyer.Email;
            mainOrder.BuyerCellPhone = userBuyer.Phone;

            #endregion
 
            #region 购物车 -> 订单项目
            OrderItems tmpOrderItem;
            
            //购物车 -> 订单项目
            shoppingCartInfo.Items.ForEach(item =>
            {
                tmpOrderItem = new OrderItems
                 {
                     //TODO: 警告: 商品信息根据Cookie获取, 暂未与DB及时同步
                     Name = item.Name,
                     SKU = item.SKU,
                     Quantity = item.Quantity,
                     ShipmentQuantity = item.Quantity,
                     ThumbnailsUrl = item.ThumbnailsUrl,
                     Points = item.Points,
                     Weight = item.Weight>0?item.Weight:0,
                     ProductId = item.ProductId,
                     Description = item.Description,
                     CostPrice = item.CostPrice,
                     SellPrice = item.SellPrice,
                     AdjustedPrice = item.AdjustedPrice,
                     Deduct = item.SellPrice - item.AdjustedPrice,
                     OrderCode = mainOrder.OrderCode,
                     //品牌信息
                     BrandId = item.BrandId,
                     BrandName = item.BrandName,
                     //商家信息
                     SupplierId = item.SupplierId,
                     SupplierName = item.SupplierName,
                     ProductType = 1,
                     //推广信息
                     ReferId = item.ReferId
                 };
                //将SKU信息记录到订单项目的Attribute中 简单记录 逗号分割, 复杂的可以为Json结构
                if (item.SkuValues != null && item.SkuValues.Length > 0)
                {
                    tmpOrderItem.Attribute = string.Join(",", item.SkuValues);
                }

                //填充订单项
                mainOrder.OrderItems.Add(tmpOrderItem);
                //
 
            });
            #endregion

            #region 收货人信息
            
            mainOrder.RegionId = shippingAddress.RegionId;
            mainOrder.ShipRegion = shippingAddress.RegionFullName; //_regionManage.GetFullNameById4Cache(regionInfo.RegionId);
            mainOrder.ShipName = shippingAddress.ShipName;
            mainOrder.ShipEmail = shippingAddress.EmailAddress;
            mainOrder.ShipCellPhone = shippingAddress.CelPhone;
            mainOrder.ShipTelPhone = shippingAddress.TelPhone;
            mainOrder.ShipAddress = shippingAddress.Address;
            mainOrder.ShipZipCode = shippingAddress.Zipcode;

            #endregion
            mainOrder.OrderTypeSub = 1;
           #region 配送信息(物流)

            mainOrder.ShippingModeId = shippingType.ModeId;
            mainOrder.ShippingModeName = shippingType.Name;
            mainOrder.RealShippingModeId = shippingType.ModeId;
            mainOrder.RealShippingModeName = shippingType.Name;
            mainOrder.ShippingStatus = (int)EnumHelper.ShippingStatus.UnShipped;
            mainOrder.ExpressCompanyName = shippingType.ExpressCompanyName;
            mainOrder.ExpressCompanyAbb = shippingType.ExpressCompanyEn;

            #endregion

            #region 支付信息及订单状态
            mainOrder.PaymentTypeId = payModel.ModeId;
            mainOrder.PaymentTypeName = payModel.Name;
            mainOrder.PaymentGateway = payModel.Gateway;
            mainOrder.PaymentStatus = (int)EnumHelper.PaymentStatus.Unpaid;
            if (mainOrder.PaymentGateway !="cod")
            {
                mainOrder.PaymentStatus = (int)EnumHelper.PaymentStatus.Paid;
            }
            mainOrder.OrderStatus = (int)EnumHelper.OrderStatus.UnHandle;
            #endregion
            #endregion

            #endregion

            #region 执行事务-创建订单
            try
            {
                //mainOrder.OrderId =   BLL.OMS.OrderManage.CreateOrder(mainOrder,CurrentUser);
                mainOrder.OrderId = BLL.Shop.Order.OrderManage.CreateOrder(mainOrder,CurrentUser);
            }
            catch (Exception ex)
            {
                LogHelp.AddErrorLog("订单创建失败: " + ex.Message, ex.StackTrace, context.Request);
            }
            #endregion

            #endregion

            #region 8.提示生成成功, 进行支付
            //移除下单类型Session

            //DONE: 7.提示生成成功, 进行支付
            //result.Accumulate(KEY_DATA, new
            //    {
            //        mainOrder.OrderId,
            //        mainOrder.OrderCode,
            //        mainOrder.Amount,
            //        mainOrder.PaymentTypeId,
            //        mainOrder.PaymentTypeName
            //    });

            if (mainOrder.OrderId<=0)
            {
                result.Accumulate(KEY_STATUS, STATUS_FAILED);
                return result.ToString();
            }

           //清空Cookie/DB购物车
            if (shoppingCartHelper != null  && shoppingCartHelper != null)
            {
                //自动移除Cookie/DB购物车中已下单的项目
                    shoppingCartInfo.Items.ForEach(info =>
                        {
                            shoppingCartHelper.RemoveItem(info.ItemId);
                        });

                //shoppingCartHelper.ClearShoppingCart(); //移除全部项
            }
            result.Accumulate(KEY_STATUS, STATUS_SUCCESS);
            Cookies.setCookie("A_Order_SelectUserId", "0", -1);
            Cookies.setCookie("A_Order_DepotId", "0", -1);
            Cookies.setCookie("A_Order_ShipTypeId", "0", -1);
            #endregion

            return result.ToString();
        }

        #region 获取购物车数据

        private ShoppingCartInfo GetShoppingCart(YSWL.Accounts.Bus.User userBuyer,
           out BLL.Shop.Products.ShoppingCartHelper shoppingCartHelper)
        {
            ShoppingCartInfo shoppingCartInfo = null;
 
            BLL.Shop.Products.SKUInfo skuManage = new BLL.Shop.Products.SKUInfo();
             //判断 是否使用 Cookie 加载购物车
             
                shoppingCartHelper = new BLL.Shop.Products.ShoppingCartHelper(userBuyer.UserID);
                //DONE: 获取已选中内容的购物车进行 购物车 部分商品 下单 BEN Modify 20130923
                shoppingCartInfo = shoppingCartHelper.GetShoppingCart4Selected();
                //获取库存
                //if (shoppingCartInfo != null && shoppingCartInfo.Items!= null)
                //{
                //    foreach (var item in shoppingCartInfo.Items)
                //    {
                //        //获取销售状态
                //        item.SaleStatus = skuManage.GetSaleStatus(item.SKU);
                //    }
                //}
            return shoppingCartInfo;
        }

        #endregion

        #region 获取购买人数据
        private YSWL.Accounts.Bus.User GetBuyerUserInfo()
        {
            return new YSWL.Accounts.Bus.User(UserId);
        }
        private int UserId {
              get { return Globals.SafeInt(Cookies.getCookie("A_Order_SelectUserId", "value"), 0); }
        }
        #endregion

        #region 获取收货人数据

        private Model.Shop.Shipping.ShippingAddress GetShippingAddress()
        {
            return _shippingAddressManage.GetModelByUserId(UserId);
        }

        #endregion

        #region 获取配送信息

        private Model.Shop.Shipping.ShippingType GetShippingType()
        {
            int shippingTypeId = Globals.SafeInt(Cookies.getCookie("A_Order_ShipTypeId", "value"), 0);
            if (shippingTypeId < 1) return null;
            return _shippingTypeManage.GetModel(shippingTypeId);
        }
        #endregion

        #region 获取支付信息

        private Payment.Model.PaymentModeInfo GetPayType(int payType)
        {
            Payment.Model.PaymentModeInfo payModel = null;
            if (payType == 1)
            {
                //在支付方式中找货到付款
                List<Payment.Model.PaymentModeInfo> paylist = Payment.BLL.PaymentModeManage.GetPaymentModes();
                if (paylist != null)
                {
                    paylist = paylist.Where(o => o.Gateway == "cod").OrderBy(o => o.DisplaySequence).ToList();
                }
                if (paylist != null && paylist.Count > 0)
                {
                    payModel = paylist[0];
                }
            }
            else
            {
                payModel = new Payment.Model.PaymentModeInfo
                {
                    ModeId = -2,
                    Name = "未知",
                    Gateway = "NULL"
                };
            }
            return payModel;
        }
        #endregion
 
        #region 获取仓库id
        protected int DepotId
        {
            get
            {
                return Globals.SafeInt(Cookies.getCookie("A_Order_DepotId", "value"), 0);
            }
        }
        #endregion


    }
}