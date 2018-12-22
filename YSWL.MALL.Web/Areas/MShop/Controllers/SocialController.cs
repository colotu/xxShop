using System.Web.Mvc;
using YSWL.MALL.Web.Controllers;

namespace YSWL.MALL.Web.Areas.MShop.Controllers
{
    public class SocialController : SocialControllerBase
    {
        public override ActionResult RedirectToUserBind()
        {
            return RedirectToAction("Index", "Home", new { area = "MShop" });
        }
        public override ActionResult RedirectToHome()
        {
            return RedirectToAction("Index", "Home", new { area = "MShop" });
        }
    }
}
