using System.Collections.Generic;
using System.Web.Mvc;
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
    }
}
