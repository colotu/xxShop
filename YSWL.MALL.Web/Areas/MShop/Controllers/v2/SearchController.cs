using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace YSWL.MALL.Web.Areas.MShop.Controllers
{
    public partial class SearchController : MShopControllerBase
    {
        BLL.Shop.Supplier.SupplierInfo suppBll = new BLL.Shop.Supplier.SupplierInfo();
        // GET: MShop/Search
        /// <summary>
        /// 搜索店铺
        /// </summary>
        /// <param name="keyword"></param>
        /// <param name="viewName"></param>
        /// <returns></returns>
        public ActionResult StoreList(string keyword = "", int? pageIndex = 1, int pageSize = 30,string vName = "StoreList",string ajaxVName= "_StoreListMore")
        {
            pageSize = pageSize > 0 ? pageSize : 10;
            //重置页面索引
            pageIndex = pageIndex.HasValue && pageIndex.Value > 1 ? pageIndex.Value : 1;
            //计算分页起始索引
            int startIndex = pageIndex.Value > 1 ? (pageIndex.Value - 1) * pageSize + 1 : 1;
            //计算分页结束索引
            int endIndex = pageIndex.Value > 1 ? startIndex + pageSize - 1 : pageSize;

            #region DataParam
            ViewBag.PageSize = pageSize;
            ViewBag.DataParam = String.Format("{0}keyword:'{1}'{2}", "{", keyword, "}");
            #endregion

            List<YSWL.MALL.Model.Shop.Supplier.SupplierInfo> list = suppBll.GetListByPageEx(keyword, startIndex, endIndex);
            if (Request.IsAjaxRequest()) {
                return View(ajaxVName, list);
            }
            return View(vName, list);
        }
    }
}