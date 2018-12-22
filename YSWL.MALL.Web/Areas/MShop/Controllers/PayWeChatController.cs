using System.Web.Mvc;
using YSWL.Components.Setting;
using YSWL.MALL.Web.Components.Setting.Shop;

namespace YSWL.MALL.Web.Areas.MShop.Controllers
{
    public class PayWeChatController : MShopControllerBase
    {
        BLL.Shop.Order.Orders orderManage = new BLL.Shop.Order.Orders();
        BLL.Shop.Order.OrderItems orderItemManage = new BLL.Shop.Order.OrderItems();

        #region Pay
        public ActionResult Pay(string viewName = "Pay")
        {
            string orderIdStr = Session[Handlers.Shop.Pay.PaymentReturnHandler.KEY_ORDERID] as string;
            string statusStr = Session[Handlers.Shop.Pay.PaymentReturnHandler.KEY_STATUS] as string;

            if (string.IsNullOrWhiteSpace(orderIdStr) ||
                string.IsNullOrWhiteSpace(statusStr))
                return Redirect("/");

            long orderId = Common.Globals.SafeLong(orderIdStr, -1);

            if (statusStr.ToLower() != "success")
                return Content("ERROR_NOSUCCESS");

            if (orderId < 1)
                return Content("ERROR_NOTSAFEORDERID");

            Session.Remove(Handlers.Shop.Pay.PaymentReturnHandler.KEY_ORDERID);
            Session.Remove(Handlers.Shop.Pay.PaymentReturnHandler.KEY_STATUS);

            Model.Shop.Order.OrderInfo orderInfo = orderManage.GetModel(orderId);
            ViewBag.OrderId = orderInfo.OrderId;
            //订单编号
            ViewBag.OrderCode = orderInfo.OrderCode;
            //收货人
            ViewBag.ShipName =orderInfo.ShipName;
            //项目数量
            ViewBag.ItemsCount =orderItemManage.GetOrderItemCountByOrderId(orderId);
            //应付金额
            ViewBag.OrderAmount =  orderInfo.Amount;

            #region SEO 优化设置
            IPageSetting pageSetting = PageSetting.GetPageSetting("Home", Model.SysManage.ApplicationKeyType.Shop);
            ViewBag.Title = "微信支付";
            ViewBag.Keywords = pageSetting.Keywords;
            ViewBag.Description = pageSetting.Description;
            #endregion
            return View(viewName);
        }
        #endregion
 
    }
}

