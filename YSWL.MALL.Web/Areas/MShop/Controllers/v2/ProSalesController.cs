using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Webdiyer.WebControls.Mvc;
using YSWL.Components.Setting;
using YSWL.MALL.Web.Components.Setting.Shop;

namespace YSWL.MALL.Web.Areas.MShop.Controllers
{
    public partial class ProSalesController : YSWL.MALL.Web.Areas.Shop.Controllers.ProSalesController
    {
        private BLL.Shop.Products.ProductInfo productManage = new BLL.Shop.Products.ProductInfo();
        private BLL.Shop.PromoteSales.GroupBuy groupBuy = new BLL.Shop.PromoteSales.GroupBuy();
        // GET: MShop/ProSales
        #region 团购
        public override ActionResult Group(int rid=0, int cid=0, string mod = "default", int? pageIndex = 1, int pageSize = 30,
                               string vName = "Group", string ajaxVName = "_Group")
        {
            //优先取传过来的值 //其次取分仓地区的值 //如果没有开启分仓 取前台用户自己设置的值 //再去团购默认地区的值
            if (rid <= 0)
            {
                if (IsMultiDepot)
                {
                    //开启了分仓
                    rid = GetRegionId;
                }
                else
                {
                    rid = Common.Globals.SafeInt(Common.Cookies.getKeyCookie("groupbuy_regionId"), 0);
                }
                if (rid <= 0)
                {
                    rid = Common.Globals.SafeInt(BLL.SysManage.ConfigSystem.GetValueByCache("Shop_GroupBuy_DefaultRegion"), 651);//默认北京
                }
            }
            rid = rid <= 0 ? 643 : rid;//防止从cache中未取到参数报错
            YSWL.MALL.Model.Shop.Products.CategoryInfo categoryInfo = null;
            List<YSWL.MALL.Model.Shop.PromoteSales.GroupBuy> groupList;//new List<Model.Shop.PromoteSales.GroupBuy>();
            //重置页面索引
            pageIndex = pageIndex.HasValue && pageIndex.Value > 1 ? pageIndex.Value : 1;
            //计算分页起始索引
            int startIndex = pageIndex.Value > 1 ? (pageIndex.Value - 1) * pageSize + 1 : 1;
            //计算分页结束索引
            int endIndex = pageIndex.Value > 1 ? startIndex + pageSize - 1 : pageSize;
            int totalCount = groupBuy.GetCount(cid, rid);

            #region DataParam
            ViewBag.PageSize = pageSize;
            ViewBag.DataParam = String.Format("{0}regionid:'{1}',cid:'{2}',mod:'{3}'{4}", "{", rid, cid, mod,"}");
            #endregion
            groupList = groupBuy.GetListByPage(null, cid, rid, mod, startIndex, endIndex);
            if (groupList != null && groupList.Count > 0)
            {
                foreach (Model.Shop.PromoteSales.GroupBuy item in groupList)
                {
                    item.LowestSalePrice = productManage.GetLowestSalePrice(item.ProductId);
                }
            }

            PagedList<Model.Shop.PromoteSales.GroupBuy> pagedList = new PagedList<Model.Shop.PromoteSales.GroupBuy>(groupList, pageIndex.Value, pageSize, totalCount);
            #region SEO 优化设置
            IPageSetting pageSetting = PageSetting.GetCategorySetting(categoryInfo);
            ViewBag.Title = pageSetting.Title;
            ViewBag.Keywords = pageSetting.Keywords;
            ViewBag.Description = pageSetting.Description;
            #endregion

            //检测Ajax请求, 进行无刷新分页
            if (Request.IsAjaxRequest())
            {
                return PartialView(ajaxVName, pagedList);
            }
            return View(vName, pagedList);
        }
        #endregion

    }
}