using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using YSWL.Json;
using YSWL.MALL.Model.Shop.Products;

namespace YSWL.MALL.Web.Areas.Shop.Controllers
{
    public partial class OrderController : ShopControllerBaseUser
    {
        /// <summary>
        /// 获取配送方式
        /// </summary>
        /// <param name="suppId">商家Id(平台小于等于0)</param>
        /// <returns></returns>
        public ActionResult GetShipTypeBySupp(int suppId = 0, string viewName = "_ShipTypeBySupp")
        {
            ViewBag.SuppId = suppId;
            return View(viewName, _shippingTypeManage.GetListBySupplied(suppId));
        }

        /// <summary>
        /// 获取支付方式
        /// </summary>
        /// <returns></returns>
        public ActionResult PayList(string viewName = "_PayList")
        {
            List<YSWL.Payment.Model.PaymentModeInfo> payList = YSWL.Payment.BLL.PaymentModeManage.GetPaymentModes(YSWL.Payment.Model.DriveEnum.Web);
            return View(viewName, payList);
        }

        #region  获取发票信息
        /// <summary>
        /// 获取发票信息
        /// </summary>
        /// <returns></returns>
        //public ActionResult Invoice(string viewName = "Invoice")
        //{
        //    YSWL.MALL.BLL.Shop.Order.OrderLookupItems lookUpItemsBll = new BLL.Shop.Order.OrderLookupItems();
        //    YSWL.MALL.ViewModel.Shop.InvoiceModel model = new ViewModel.Shop.InvoiceModel();
        //    model.Header = lookUpItemsBll.GetModelList(Common.Globals.SafeInt(BLL.SysManage.ConfigSystem.GetValueByCache("Shop_InvoiceInfo_HeaderId"), 9));//发票抬头默认ListId
        //    model.Content = lookUpItemsBll.GetModelList(Common.Globals.SafeInt(BLL.SysManage.ConfigSystem.GetValueByCache("Shop_InvoiceInfo_ContentId"), 10));//发票内容默认ListId
        //    return View(viewName, model);
        //}
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
                return ;
            }
            decimal freight = BLL.Shop.Products.ShoppingCartHelper.CalcFreightGroup(cartInfo, _regionManage.GetModelByCache(GetRegionId), CurrentUser.UserID, dicShip);
             
            result.Accumulate("STATUS", "OK");
            result.Accumulate("DATA", freight.ToString("F"));
            Response.Write(result.ToString());
            return ;
        }
    }
}
