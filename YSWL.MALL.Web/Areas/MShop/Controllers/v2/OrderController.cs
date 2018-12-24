using System;
using System.Collections.Generic;
using System.Web.Mvc;
using YSWL.MALL.BLL.SysManage;
using YSWL.Common;
using YSWL.MALL.Model.Shop.Products;
using YSWL.TaoBao.Domain;
using System.Linq;
using YSWL.Json;
using System.Web;

namespace YSWL.MALL.Web.Areas.MShop.Controllers
{
    public partial class OrderController : MShopControllerBaseUser
    {
        // GET: MShop/Order
        public ActionResult Product(string viewName= "Product")
        {
            ViewModel.Shop.ShoppingCartModel model = new ViewModel.Shop.ShoppingCartModel();
            model.AllCartInfo = GetCartInfoByProduct();
            model.DicSuppCartItems = BLL.Shop.Products.ShoppingCartHelper.GetSuppCartItems(model.AllCartInfo.Items);
            return View(viewName,model);
        }

        /// <summary>
        /// 获取配送方式
        /// </summary>
        /// <param name="suppId">商家Id(平台小于等于0)</param>
        /// <returns></returns>
        public ActionResult GetShipTypeBySupp(int suppId=0,string viewName= "_ShipTypeBySupp")
        {
            ViewBag.SuppId = suppId;

            string shipStr = Common.Cookies.getKeyCookie("shipStr");
            shipStr = shipStr.Replace("value=", "");
            shipStr =HttpUtility.UrlDecode(shipStr);
            Dictionary<int, int> dicShip = new Dictionary<int, int>();
            if (!String.IsNullOrWhiteSpace(shipStr))
            {
                var shipArr = shipStr.Split('|');
                foreach (var item in shipArr)
                {
                    if (item.Contains('-'))
                    {
                        var itemArr = item.Split('-');
                        if (dicShip.ContainsKey(YSWL.Common.Globals.SafeInt(itemArr[0], 0)))
                        {
                            dicShip[YSWL.Common.Globals.SafeInt(itemArr[0], 0)] = YSWL.Common.Globals.SafeInt(itemArr[1], 0);
                        }
                        else
                        {
                            dicShip.Add(YSWL.Common.Globals.SafeInt(itemArr[0], 0), YSWL.Common.Globals.SafeInt(itemArr[1], 0));
                        }
                    }
                }
            }
            ViewBag.SelectShip = dicShip[suppId];

            return View(viewName, _shippingTypeManage.GetListBySupplied(suppId));
        }

        /// <summary>
        /// 获取支付方式
        /// </summary>
        /// <returns></returns>
        public ActionResult PayList(string viewName = "_PayList")
        {
            List<YSWL.Payment.Model.PaymentModeInfo> payList= YSWL.Payment.BLL.PaymentModeManage.GetPaymentModes(YSWL.Payment.Model.DriveEnum.Wap);
            return View(viewName, payList);
        }


        #region  获取发票信息
        /// <summary>
        /// 获取发票信息
        /// </summary>
        /// <returns></returns>
        public ActionResult Invoice(string viewName = "Invoice")
        {
            YSWL.MALL.BLL.Shop.Order.OrderLookupItems lookUpItemsBll = new BLL.Shop.Order.OrderLookupItems();
            YSWL.MALL.ViewModel.Shop.InvoiceModel model = new ViewModel.Shop.InvoiceModel();
            model.Header = lookUpItemsBll.GetModelList(Common.Globals.SafeInt(BLL.SysManage.ConfigSystem.GetValueByCache("Shop_InvoiceInfo_HeaderId"), 9));//发票抬头默认ListId
            model.Content = lookUpItemsBll.GetModelList(Common.Globals.SafeInt(BLL.SysManage.ConfigSystem.GetValueByCache("Shop_InvoiceInfo_ContentId"), 10));//发票内容默认ListId
            return View(viewName, model);
        }
        #endregion


        #region 运费

        public ActionResult ShowPayAndShipV2(string viewName = "_ShowPayAndShip")
        {
            YSWL.MALL.ViewModel.Shop.PayAndShip payAndShip = new ViewModel.Shop.PayAndShip();
            int payId = Common.Globals.SafeInt(Common.Cookies.getKeyCookie("m_so_payId"), 0);
          //  int shipId = Common.Globals.SafeInt(Common.Cookies.getKeyCookie("m_so_shipId"), 0);
            int addrId = Common.Globals.SafeInt(Common.Cookies.getCookie("m_so_addrId", "value"), 0);

            #region  配送方式
            string shipStr= Common.Cookies.getKeyCookie("shipStr"); 
            Dictionary<int, int> dicShip = new Dictionary<int, int>();
            if (!String.IsNullOrWhiteSpace(shipStr))
            {
                var shipArr = shipStr.Split('|');
                foreach (var item in shipArr)
                {
                    if (item.Contains('-'))
                    {
                        var itemArr = item.Split('-');
                        if (dicShip.ContainsKey(YSWL.Common.Globals.SafeInt(itemArr[0], 0)))
                        {
                            dicShip[YSWL.Common.Globals.SafeInt(itemArr[0], 0)] = YSWL.Common.Globals.SafeInt(itemArr[1], 0);
                        }
                        else
                        {
                            dicShip.Add(YSWL.Common.Globals.SafeInt(itemArr[0], 0), YSWL.Common.Globals.SafeInt(itemArr[1], 0));
                        }
                    }
                }
            }
            #endregion  

            #region 支付
            if (payId > 0)
            {
                //TODO: 未使用缓存 BEN ADD 20130620
                payAndShip.CurrentPaymentMode = YSWL.Payment.BLL.PaymentModeManage.GetPaymentModeById(payId);
            }
            else
            {
                payAndShip.ListPaymentMode = YSWL.Payment.BLL.PaymentModeManage.GetPaymentModes(YSWL.Payment.Model.DriveEnum.Wap);
                if (payAndShip.ListPaymentMode != null && payAndShip.ListPaymentMode.Count > 0)
                {
                    //默认当前第一个支付方式
                    payAndShip.CurrentPaymentMode = payAndShip.ListPaymentMode[0];
                }
                else
                {
                    payAndShip.CurrentPaymentMode = new Payment.Model.PaymentModeInfo
                    {
                        ModeId = -1,
                        Name = "未选择支付方式"
                    };
                }
            }
            #endregion
 

            #region 运费
            YSWL.MALL.Model.Shop.Products.ShoppingCartInfo cartInfo = GetCartInfoByProduct();


            if (cartInfo == null)
            {
                //终止计算运费, 运费0 在提交订单环节提示用户
                ViewBag.Freight = 0;
                return View(viewName, payAndShip);
            }
            #region 区域差异运费计算
            Model.Shop.Shipping.ShippingAddress shippingAddress = addrId < 1
                ? null
                : _shippingAddressManage.GetModel(addrId);
         
            //默认收货地址
            if (shippingAddress == null && currentUser != null)
            {
                List<YSWL.MALL.Model.Shop.Shipping.ShippingAddress> listAddress =_addressManage.GetDefaultModelList(currentUser.UserID);
                if (listAddress != null && listAddress.Count > 0)
                    shippingAddress = listAddress[0];
            }

            

            //有收货地址 且 已选择配送 计算差异运费
            if (shippingAddress != null  && shippingAddress.RegionId > 0)
            {
                Model.Ms.Regions regionInfo = _regionManage.GetModelByCache(shippingAddress.RegionId);
                //ViewBag.Freight = YSWL.MALL.BLL.Shop.Products.ShoppingCartHelper.CalcFreightGroup(cartInfo, regionInfo);
                ViewBag.Freight = YSWL.MALL.BLL.Shop.Products.ShoppingCartHelper.CalcFreightGroup(cartInfo, regionInfo, currentUser.UserID, dicShip);
            }
            else
            {
                ViewBag.Freight = decimal.Zero;
            }

            #endregion
            #endregion

            return View(viewName, payAndShip);
        }
        #endregion

        #region 获取促销活动赠品
        public ActionResult ActivListV2(decimal coupPrice = 0, string viewName = "_ActivList")
        {
            string sku = Session["SubmitOrder_SKU"] as string;
            int count = Common.Globals.SafeInt(Session["SubmitOrder_COUNT"], 1);
            int c = Common.Globals.SafeInt(Session["SubmitOrder_CountDown"], 0);
            int g = Common.Globals.SafeInt(Session["SubmitOrder_GroupBuy"], 0);

            //tuzh
            #region  配送方式
            string shipStr = Common.Cookies.getKeyCookie("shipStr");
            Dictionary<int, int> dicShip = new Dictionary<int, int>();
            if (!String.IsNullOrWhiteSpace(shipStr))
            {
                var shipArr = shipStr.Split('|');
                foreach (var item in shipArr)
                {
                    if (item.Contains('-'))
                    {
                        var itemArr = item.Split('-');
                        if (dicShip.ContainsKey(YSWL.Common.Globals.SafeInt(itemArr[0], 0)))
                        {
                            dicShip[YSWL.Common.Globals.SafeInt(itemArr[0], 0)] = YSWL.Common.Globals.SafeInt(itemArr[1], 0);
                        }
                        else
                        {
                            dicShip.Add(YSWL.Common.Globals.SafeInt(itemArr[0], 0), YSWL.Common.Globals.SafeInt(itemArr[1], 0));
                        }
                    }
                }
            }
            #endregion  


            ViewBag.IsMerge = ConfigSystem.GetBoolValueByCache("Shop_Activity_IsMerge");
            if (c > 0 || g > 0)//限时抢购/团购/组合套装 不参与促销活动
            {
                return View(viewName, null);
            }

            ShoppingCartInfo cartInfo = BLL.Shop.Products.ShoppingCartHelper.GetCartInfoByProduct((currentUser != null ? currentUser.UserID : -1), sku, count, c, g, 0, GetRegionId);
            ViewModel.Shop.ActicityGiveList model = activInfoBll.GetActivityGiveList(cartInfo,
                                                                                     CurrentUser.UserID, coupPrice,
                                                                                     GetRegionId);
            #region 按商家分组计算运费

            Model.Ms.Regions regionInfo = _regionManage.GetModelByCache(GetRegionId);
            ViewBag.AdjustedFreight = YSWL.MALL.BLL.Shop.Products.ShoppingCartHelper.CalcFreightGroup(cartInfo, regionInfo,CurrentUser.UserID, dicShip);
            #endregion

            return View(viewName, model); 
        }
        #endregion

        #region  查看配送及物流信息
        /// <summary>
        /// 查看配送及物流信息
        /// </summary>
        /// <returns></returns>
        public ActionResult ShipInfo(long orderId, string viewName = "ShipInfo")
        {
            YSWL.MALL.Model.Shop.Order.OrderInfo orderModel = _orderManage.GetModel(orderId);
            //Safe
            if (orderModel == null ||
                orderModel.BuyerID != currentUser.UserID
                )
                return Redirect(ViewBag.BasePath + "UserCenter/Orders");

            return View(viewName, orderModel);
        }
        #endregion

        [HttpPost]
        public void GetFreight()
        {

            string shipStr = Common.Cookies.getKeyCookie("shipStr");
            Dictionary<int, int> dicShip = new Dictionary<int, int>();
            if (!String.IsNullOrWhiteSpace(shipStr))
            {
                var shipArr = shipStr.Split('|');
                foreach (var item in shipArr)
                {
                    if (item.Contains('-'))
                    {
                        var itemArr = item.Split('-');
                        if (dicShip.ContainsKey(YSWL.Common.Globals.SafeInt(itemArr[0], 0)))
                        {
                            dicShip[YSWL.Common.Globals.SafeInt(itemArr[0], 0)] = YSWL.Common.Globals.SafeInt(itemArr[1], 0);
                        }
                        else
                        {
                            dicShip.Add(YSWL.Common.Globals.SafeInt(itemArr[0], 0), YSWL.Common.Globals.SafeInt(itemArr[1], 0));
                        }
                    }
                }
            }

            JsonObject result = new JsonObject();
            JsonArray array = new JsonArray();
            JsonObject json;

            int userId = currentUser == null ? -1 : currentUser.UserID;
            BLL.Shop.Products.ShoppingCartHelper cartHelper = new BLL.Shop.Products.ShoppingCartHelper(userId);
            ShoppingCartInfo cartInfo = cartHelper.GetShoppingCart();



            if (cartInfo.Quantity < 1)
            {
                result.Accumulate("STATUS", "NO");
                result.Accumulate("DATA", "NODATA");
                Response.Write(result.ToString());
                return;
            }
            decimal freight = BLL.Shop.Products.ShoppingCartHelper.CalcFreightGroup(cartInfo, _regionManage.GetModelByCache(GetRegionId), CurrentUser.UserID, dicShip);

            result.Accumulate("STATUS", "OK");
            result.Accumulate("DATA", freight.ToString("F"));
            Response.Write(result.ToString());
            return;
        }

    }
}