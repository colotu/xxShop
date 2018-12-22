

namespace YSWL.MALL.Web.Areas.MShop.Controllers
{
    public partial class ProSalesController : YSWL.MALL.Web.Areas.Shop.Controllers.ProSalesController
    {
        #region 团购

        public override System.Web.Mvc.ActionResult GroupBuy(int regionid = 0, int cid = 0, string mod = "default", int? pageIndex = 1, int pageSize = 16,
            string viewName = "GroupBuy",
            string ajaxViewName = "_GroupBuyList")
        {
            return base.GroupBuy(regionid, cid, mod, pageIndex, pageSize, viewName, ajaxViewName);
        }
        #endregion
    }
}
