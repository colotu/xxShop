using System.Web.Mvc;
using YSWL.Common;

namespace YSWL.MALL.Web.Areas.CMS.Controllers
{
    public class AboutController : CMSControllerBase
    {
        private BLL.SysManage.WebSiteSet WebSiteSet = new BLL.SysManage.WebSiteSet(Model.SysManage.ApplicationKeyType.CMS);

        //
        // GET: /CMS/About/

        public ActionResult Index()
        { 
            ViewBag.Title = "关于";
            if (null != WebSiteSet)
            {
                //ViewBag.Title = "-" + Globals.HtmlDecode(WebSiteSet.WebName);
                ViewBag.Keywords = Globals.HtmlDecode(WebSiteSet.KeyWords);
                ViewBag.Description = Globals.HtmlDecode(WebSiteSet.Description);
            }
            return View("Index");
        }
    }
}