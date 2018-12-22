using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using YSWL.MALL.BLL.Ms;
using YSWL.MALL.BLL.Shop.Products;
using YSWL.MALL.BLL.Shop.PromoteSales;
using YSWL.Components.Setting;
using YSWL.MALL.ViewModel.Shop;
using YSWL.MALL.Web.Components.Setting.Shop;
using Webdiyer.WebControls.Mvc;

namespace YSWL.MALL.Web.Areas.Shop.Controllers
{
    public class ProSalesController : ShopControllerBase
    {
        //
        // GET: /Shop/ProSales/
        private BLL.Shop.Products.ProductInfo productManage = new BLL.Shop.Products.ProductInfo();
        private BLL.Shop.Products.CategoryInfo cateInfo = new CategoryInfo();
        private BLL.Shop.PromoteSales.GroupBuy groupBuy = new GroupBuy();
        private BLL.Ms.Regions regionsBll = new Regions();
        private int _basePageSize = 32;
        private int _waterfallSize = 0;
        private int _waterfallDataCount = 1;

        #region  限时抢购
        public ActionResult CountDown(int? pageIndex = 1,
                                  string viewName = "CountDown", string ajaxViewName = "_ProductList")
        {
            ProductListModel model = new ProductListModel();
            //model.CategoryList = categoryManage.MainCategoryList(null);
            YSWL.MALL.Model.Shop.Products.CategoryInfo categoryInfo = null;

            int pageSize = _basePageSize + _waterfallSize;
            ViewBag.BasePageSize = _basePageSize;
            //重置页面索引
            pageIndex = pageIndex.HasValue && pageIndex.Value > 1 ? pageIndex.Value : 1;
            //计算分页起始索引
            int startIndex = pageIndex.Value > 1 ? (pageIndex.Value - 1) * pageSize + 1 : 1;
            //计算分页结束索引
            int endIndex = pageIndex.Value > 1 ? startIndex + _basePageSize - 1 : _basePageSize;
            int toalCount = productManage.GetProSalesCount();
            //瀑布流Index
            ViewBag.CurrentPageAjaxStartIndex = endIndex;
            ViewBag.CurrentPageAjaxEndIndex = pageIndex * pageSize;

            ViewBag.PageSize = pageSize;

            List<Model.Shop.Products.ProductInfo> list = productManage.GetProSalesList(startIndex, endIndex);

            #region SEO 优化设置
            IPageSetting pageSetting = PageSetting.GetCategorySetting(categoryInfo);
            //pageSetting.Replace(
            //    new[] { PageSetting.RKEY_CNAME, model.CurrentCateName }, 
            //    new[] { PageSetting.RKEY_CID, model.CurrentCid.ToString() }); //分类名称
            ViewBag.Title = pageSetting.Title;
            ViewBag.Keywords = pageSetting.Keywords;
            ViewBag.Description = pageSetting.Description;
            #endregion

            //获取总条数
            if (toalCount < 1) {
                //检测Ajax请求, 进行无刷新分页
                if (Request.IsAjaxRequest())
                    return PartialView(ajaxViewName, model);
                return View(viewName, model); //NO DATA
            }

            //分页获取数据
            model.ProductPagedList = new PagedList<Model.Shop.Products.ProductInfo>(list, pageIndex ?? 1, pageSize, toalCount);

            //检测Ajax请求, 进行无刷新分页
            if (Request.IsAjaxRequest())
                return PartialView(ajaxViewName, model);

            return View(viewName, model);
        }

        public ActionResult ListWaterfall(int startIndex, string viewName = "_ListWaterfall")
        {
            ViewBag.BasePageSize = _basePageSize;

            //重置分页起始索引
            startIndex = startIndex > 1 ? startIndex + 1 : 1;
            //计算分页结束索引
            int endIndex = startIndex > 1 ? startIndex + _waterfallDataCount - 1 : _waterfallDataCount;
            int toalCount = productManage.GetProSalesCount();

            //获取总条数 并加载数据
            List<Model.Shop.Products.ProductInfo> list;

            list = productManage.GetProSalesList(startIndex, endIndex);

            if (toalCount < 1) return new EmptyResult();   //NO DATA

            return View(viewName, list);
        }
        #endregion

        #region 团购
        public virtual ActionResult GroupBuy(int regionid, int cid, string mod = "default", int? pageIndex = 1, int pageSize = 16,
                               string viewName = "GroupBuyIndex", string ajaxViewName = "_BuyProductListGroup")
        {
            if (regionid <= 0)
            {
                regionid = BLL.SysManage.ConfigSystem.GetIntValueByCache("Shop_GroupBuy_DefaultRegion");
            }
            regionid = regionid <= 0 ? 643 : regionid;//防止从cache中未取到参数报错
            //  regionid = 643;
            YSWL.MALL.Model.Shop.Products.CategoryInfo categoryInfo = null;
            List<YSWL.MALL.Model.Shop.PromoteSales.GroupBuy> groupList;//new List<Model.Shop.PromoteSales.GroupBuy>();
            //重置页面索引
            pageIndex = pageIndex.HasValue && pageIndex.Value > 1 ? pageIndex.Value : 1;
            //计算分页起始索引
            int startIndex = pageIndex.Value > 1 ? (pageIndex.Value - 1) * pageSize + 1 : 1;
            //计算分页结束索引
            int endIndex = pageIndex.Value > 1 ? startIndex + pageSize - 1 : pageSize;
            int totalCount = groupBuy.GetCount(cid, regionid);
            ViewBag.regionId = regionid;
            groupList = groupBuy.GetListByPage(null, cid, regionid, mod, startIndex, endIndex);
            PagedList<Model.Shop.PromoteSales.GroupBuy> pagedList = new PagedList<Model.Shop.PromoteSales.GroupBuy>(groupList, pageIndex.Value, pageSize, totalCount);
            bool isParentRegion = regionsBll.IsParentRegion(regionid);

            #region RouteDataParam
            string dataParam = "{";
            foreach (KeyValuePair<string, object> item in Request.RequestContext.RouteData.Values)
            {
                dataParam += item.Key + ":'" + item.Value + "',";
            }
            int parentId;
            if (isParentRegion)//如果是下边有子区域
            {
                parentId = regionid;
                dataParam += "parentId" + ":'" + parentId + "',";
            }
            else//没有子区域
            {
                parentId = regionsBll.GetCurrentParentId(regionid);
                dataParam += "parentId" + ":'" + parentId + "',";
            }
            dataParam = dataParam.TrimEnd(',') + "}";
            RouteData.Values.Add("parentId", parentId);
            ViewBag.DataParam = dataParam;
            #endregion

            //获取总条数
            //  if (totalCount < 1) return View(viewName, pagedList); //NO DATA
            //检测Ajax请求, 进行无刷新分页
            if (Request.IsAjaxRequest())
            {
                return PartialView(ajaxViewName, pagedList);
            }
            #region SEO 优化设置
            IPageSetting pageSetting = PageSetting.GetCategorySetting(categoryInfo);
            ViewBag.Title = pageSetting.Title;
            ViewBag.Keywords = pageSetting.Keywords;
            ViewBag.Description = pageSetting.Description;
            #endregion
            return View(viewName, pagedList);
        }

        public ActionResult GetArea(int regionId, int cid, string mod = "default", string viewName = "_AreaList")
        {
            if (regionId < 1)
            {
                regionId = BLL.SysManage.ConfigSystem.GetIntValueByCache("Shop_GroupBuy_DefaultRegion");
            }
            regionId = regionId < 1 ? 643 : regionId;//防止从cache中未取到参数报错
            List<Model.Ms.Regions> regionList = regionsBll.GetListDistrictByParentId(regionId);
            int parentId;
            bool isParentRegion = regionsBll.IsParentRegion(regionId);
            if (isParentRegion)
            {
                parentId = regionId;
            }
            else
            {
                parentId = regionsBll.GetCurrentParentId(regionId);
            }
            if (regionList.Count < 1)
            {
                regionList = regionsBll.GetSamePathArea(regionId);
            }
            #region RouteDataParam
            string dataParam = "{";
            foreach (KeyValuePair<string, object> item in Request.RequestContext.RouteData.Values)
            {
                dataParam += item.Key + ":'" + item.Value + "',";
            }
            dataParam = dataParam.TrimEnd(',') + "}";
            RouteData.Values.Add("parentId", parentId);
            RouteData.Values.Add("cid", cid);
            RouteData.Values.Add("mod", mod);
            ViewBag.DataParam = dataParam;
            #endregion
            if (Request.IsAjaxRequest())
            {
                return PartialView(viewName, regionList);
            }
            return View(viewName, regionList);
        }

        public ActionResult GetListCate(string viewName = "_CategoryList")
        {
            List<Model.Shop.PromoteSales.GroupBuy> groupList = groupBuy.GetCategory("");
            return View(viewName, groupList);
        }

        public ActionResult BuyListWaterfall(int startIndex, string viewName = "_BuyListWaterfall")
        {
            ViewBag.BasePageSize = _basePageSize;

            //重置分页起始索引
            startIndex = startIndex > 1 ? startIndex + 1 : 1;
            //计算分页结束索引
            int endIndex = startIndex > 1 ? startIndex + _waterfallDataCount - 1 : _waterfallDataCount;
            int toalCount = productManage.GetGroupBuyCount();

            //获取总条数 并加载数据
            List<Model.Shop.Products.ProductInfo> list = productManage.GetGroupBuyList(startIndex, endIndex);

            if (toalCount < 1) return new EmptyResult();   //NO DATA

            return View(viewName, list);
        }
        #endregion



        #region 团购新  地区在别处设置
        public virtual ActionResult Group(int rid = 0, int cid = 0, string mod = "default", int? pageIndex = 1, int pageSize = 30,
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
            ViewBag.DataParam = String.Format("{0}regionid:'{1}',cid:'{2}',mod:'{3}'{4}", "{", rid, cid, mod, "}");
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
