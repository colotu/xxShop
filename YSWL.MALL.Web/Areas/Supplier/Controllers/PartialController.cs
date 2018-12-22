using System.Web.Mvc;

namespace YSWL.MALL.Web.Areas.Supplier.Controllers
{
    public class PartialController : YSWL.MALL.Web.Controllers.ControllerBase
    {
        //
        // GET: /Supplier/Partial/

        public ActionResult Index()
        {
            return View();
        }
        /// <summary>
        /// 小广告
        /// </summary>
        /// <param name="AdvPositionId"></param>
        /// <returns></returns>
        public PartialViewResult AD(int AdvPositionId, string viewName = "_AD")
        {
            BLL.Settings.Advertisement bllAdvertisement = new BLL.Settings.Advertisement();
            Model.Settings.Advertisement model = bllAdvertisement.GetModelByAdvPositionId(AdvPositionId);
            return PartialView(viewName, model);
        }
    }
}
