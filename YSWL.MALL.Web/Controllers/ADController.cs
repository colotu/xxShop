using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace YSWL.MALL.Web.Controllers
{
    public class ADController : ControllerBase
    {
        //
        // GET: /Rss/

        public ActionResult Index(int AdvPositionId)
        {
            YSWL.MALL.BLL.Settings.AdvertisePosition bllAdvertisePosition = new YSWL.MALL.BLL.Settings.AdvertisePosition();
            Model.Settings.AdvertisePosition advertisePosition = bllAdvertisePosition.GetModelByCache(AdvPositionId);
            if (advertisePosition == null || !advertisePosition.ShowType.HasValue)
            {
                return Content("AdvPositionId Not Find!");
            }
            return View();
        }

    }
}
