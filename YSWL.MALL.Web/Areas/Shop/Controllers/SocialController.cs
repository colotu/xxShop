using System;
using System.Web.Mvc;
using YSWL.MALL.Web.Controllers;
using YSWL.Web;

namespace YSWL.MALL.Web.Areas.Shop.Controllers
{
    public class SocialController : SocialControllerBase
    {
        public override ActionResult RedirectToHome()
        {
            //加载购物车数据
            if (MvcApplication.MainAreaRoute == AreaRoute.Shop)
            {
                BLL.Shop.Products.ShoppingCartHelper.LoadShoppingCart(currentUser.UserID);
            }
            return RedirectToAction("Index", "UserCenter", new { area = "Shop" });
        }
        public override ActionResult RedirectToUserBind()
        {
            return RedirectToAction("UserBind", "UserCenter", new { area = "Shop" });
        }
    }
}
