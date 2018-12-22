using System.Web.Mvc;
using YSWL.MALL.Web.Controllers;

namespace YSWL.MALL.Web.Areas.CMS.Controllers
{
    public class SocialController : SocialControllerBase
    {
        public override ActionResult RedirectToUserBind( )
        {
            return RedirectToAction("Index", "Home", new { area = "CMS" });
        }
        public override ActionResult RedirectToHome( )
        {
            return RedirectToAction("Index", "Home", new { area = "CMS" });
        }
    }
}
