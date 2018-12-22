using System.Web.Mvc;

namespace YSWL.MALL.Web.Areas.Shop.Controllers
{
    public class ShopErrorAttribute : FilterAttribute, IExceptionFilter
    {
        #region IExceptionFilter 成员

        public void OnException(ExceptionContext filterContext)
        {
            filterContext.Result = new RedirectResult(MvcApplication.GetCurrentRoutePath(filterContext.Controller) + "Error");
        }

        #endregion
    }
}
