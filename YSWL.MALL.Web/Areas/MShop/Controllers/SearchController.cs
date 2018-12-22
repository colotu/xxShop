using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using YSWL.Common;
using YSWL.Components.Setting;
using YSWL.MALL.Model.Shop.Products;
using YSWL.MALL.Model.SysManage;
using YSWL.MALL.ViewModel.Shop;
using YSWL.MALL.Web.Components.Setting.Shop;
using Webdiyer.WebControls.Mvc;

namespace YSWL.MALL.Web.Areas.MShop.Controllers
{
    public partial class SearchController : MShopControllerBase
    {
        //
        // GET: /Shop/Search/
                #region 全局变量

        private BLL.Shop.Products.ProductInfo productManage = new BLL.Shop.Products.ProductInfo();
        private  BLL.Shop.Products.BrandInfo brandBll=new YSWL.MALL.BLL.Shop.Products.BrandInfo();
        private BLL.CMS.Content contBll = new BLL.CMS.Content();
        #endregion


        public ActionResult Index(int cid = 0, int brandid = 0, string keyword = "", string mod = "hot", string price = "0-0",
                                        int? pageIndex = 1, int pageSize = 16,
                                        string viewName = "Index", string ajaxViewName = "_ProductList")
        {
            ProductListModel model = new ProductListModel();
            keyword= YSWL.Common.InjectionFilter.SqlFilter(keyword);
            if (String.IsNullOrWhiteSpace(keyword))
            {
                return RedirectToAction("Index", "Home", new { Area = "" });
            }
            //model.CategoryList = categoryManage.MainCategoryList(null);
            if (cid > 0)
            {
                List<YSWL.MALL.Model.Shop.Products.CategoryInfo> cateList = YSWL.MALL.BLL.Shop.Products.CategoryInfo.GetAvailableCateList();
                YSWL.MALL.Model.Shop.Products.CategoryInfo categoryInfo =
                    cateList.FirstOrDefault(c => c.CategoryId == cid);
                if (categoryInfo != null)
                {
                    var path_arr = categoryInfo.Path.Split('|');
                    List<YSWL.MALL.Model.Shop.Products.CategoryInfo> categorysPath =
                        cateList.Where(c => path_arr.Contains(c.CategoryId.ToString())).OrderBy(c => c.Depth)
                                .ToList();
                    model.CategoryPathList = categorysPath;
                    model.CurrentCateName = categoryInfo.Name;
                }
            }
            model.CurrentCid = cid;
            model.CurrentMod = mod;
            //  model.CurrentCateName = cname == "all" ? "全部" : cname;
            #region RouteDataParam
            string dataParam = "{";
            foreach (KeyValuePair<string, object> item in Request.RequestContext.RouteData.Values)
            {
                dataParam += item.Key + ":'" + item.Value + "',";
            }
            dataParam = dataParam.TrimEnd(',') + "}";
            ViewBag.DataParam = dataParam;
            #endregion
            pageSize= pageSize>0?pageSize:10;
            //重置页面索引
            pageIndex = pageIndex.HasValue && pageIndex.Value > 1 ? pageIndex.Value : 1;
            //计算分页起始索引
            int startIndex = pageIndex.Value > 1 ? (pageIndex.Value - 1) * pageSize + 1 : 1;
            //计算分页结束索引
            int endIndex = pageIndex.Value > 1 ? startIndex + pageSize - 1 : pageSize;
            int toalCount = productManage.GetSearchCountEx(cid, brandid, keyword, price);
            ViewBag.TotalCount = toalCount;
            //瀑布流Index
            ViewBag.CurrentPageAjaxStartIndex = endIndex;
            ViewBag.CurrentPageAjaxEndIndex = pageIndex * pageSize;
            ViewBag.PageSize = pageSize;
            List<Model.Shop.Products.ProductInfo> list;
            list = productManage.GetSearchListEx(cid, brandid, keyword, price, mod, startIndex, endIndex);

            //#region SEO 优化设置


            //IPageSetting pageSetting = PageSetting.GetPageSetting("Category", ApplicationKeyType.Shop);
            //pageSetting.Replace(
            //    new[] { PageSetting.RKEY_CNAME, model.CurrentCateName }); //分类名称
            //ViewBag.Title = pageSetting.Title;
            //ViewBag.Keywords = pageSetting.Keywords;
            //ViewBag.Description = pageSetting.Description;
            //#endregion

            //获取总条数
            if (toalCount < 1)
            {
                if (Request.IsAjaxRequest())
                    return PartialView(ajaxViewName, model);
                return View(viewName, model); //NO DATA
            }
            if (list ==null || list.Count<=0)
            {
                if (Request.IsAjaxRequest())
                    return PartialView(ajaxViewName, model);
                return View(viewName, model); //NO DATA
            }
            
            //分页获取数据
            model.ProductPagedList = list.ToPagedList(
                pageIndex ?? 1,
                pageSize);

            //检测Ajax请求, 进行无刷新分页
            if (Request.IsAjaxRequest())
                return PartialView(ajaxViewName, model);

            return View(viewName, model);
        }

        #region 商品分类
        public PartialViewResult ProductCategory(int Cid, string viewName = "_ProductCategory", int Top = -1)
        {
            List<YSWL.MALL.Model.Shop.Products.CategoryInfo> cateList = YSWL.MALL.BLL.Shop.Products.CategoryInfo.GetAvailableCateList();
            YSWL.MALL.Model.Shop.Products.CategoryInfo categoryInfo =
                cateList.FirstOrDefault(c => c.CategoryId == Cid);
            ViewBag.CateName = categoryInfo != null ? categoryInfo.Name : "全部分类";
            ViewBag.Cid = Cid;
            List<YSWL.MALL.Model.Shop.Products.CategoryInfo> categoryInfos = new List<CategoryInfo>();
            if (categoryInfo != null && !categoryInfo.HasChildren)
            {
                YSWL.MALL.Model.Shop.Products.CategoryInfo ParentcategoryInfo =
              cateList.FirstOrDefault(c => c.CategoryId == categoryInfo.ParentCategoryId);
                if (ParentcategoryInfo != null)
                {
                    ViewBag.CateName = ParentcategoryInfo.Name;
                    ViewBag.Cid = ParentcategoryInfo.CategoryId;
                }
                categoryInfos = cateList.Where(c => c.ParentCategoryId == categoryInfo.ParentCategoryId).ToList();
            }
            else
            {
                if (Cid <= 0)
                {
                    categoryInfos = cateList.Where(c => c.Depth == 1).ToList();
                }
                else {
                    categoryInfos = cateList.Where(c => c.ParentCategoryId == Cid).ToList();
                }
            }
       
            return PartialView(viewName, categoryInfos);
        }
        #endregion

        #region 品牌
        public PartialViewResult BrandList(int Cid = 0, int productType = 0, int top = 10, string viewName = "_BrandList")
        {
            List<YSWL.MALL.Model.Shop.Products.BrandInfo> brandInfos =
                new List<YSWL.MALL.Model.Shop.Products.BrandInfo>();
            if (Cid > 0)
            {
                brandInfos = brandBll.GetBrandsByCateId(Cid, true, top);
            }
            else
            {
                brandInfos = brandBll.GetModelListByProductTypeId(productType, top);
            }
            return PartialView(viewName, brandInfos);
        }
        #endregion 

        #region 条件筛选
        public ActionResult Filter(int cid = 0, string keyword="")
        {
            ViewBag.Cid = cid;
            return View();
        }
        #endregion



        /// <summary>
        /// 搜索店铺
        /// </summary>
        /// <param name="keyword"></param>
        /// <param name="viewName"></param>
        /// <returns></returns>
        public PartialViewResult Store(string keyword = "", string viewName = "_Store")
        {
            BLL.Shop.Supplier.SupplierInfo suppBll = new BLL.Shop.Supplier.SupplierInfo();
            Model.Shop.Supplier.SupplierInfo suppModel = suppBll.GetModelByShopName(Common.InjectionFilter.SqlFilter(keyword));
            return PartialView(viewName, suppModel);
        }

    }
}
