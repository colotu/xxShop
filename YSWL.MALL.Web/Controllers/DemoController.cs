using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace YSWL.MALL.Web.Controllers
{
    /// <summary>
    /// 根据分类对商品进行查找
    /// </summary>
    public class DemoController : Controller
    {
        //
        // GET: /Demo/

        //演示地址：http://localhost:10003/demo/demo?id=63
        public ActionResult Demo()
        {

            //品牌信息
            BLL.Shop.Products.BrandInfo bll = new BLL.Shop.Products.BrandInfo();
            List<Model.Shop.Products.BrandInfo> list = bll.GetBrandsModelListByCateId(CateId);
            Models.DemoProductSearch model = new Models.DemoProductSearch();
            if (list.Count > 0)
            {
                ViewBag.IsHaveBrands = true;
                ViewBag.BrandList = list;
            }
            else
            {
                ViewBag.IsHaveBrands = false;
            }

            //其他属性信息
            BLL.Shop.Products.AttributeInfo attBll = new BLL.Shop.Products.AttributeInfo();
            List<Model.Shop.Products.AttributeInfo> Attrlist = attBll.GetAttributeListByCateID(CateId);
            if (Attrlist.Count > 0)
            {
                ViewBag.IsHaveAttr = true;
                ViewBag.AttributeList = Attrlist;
            }
            else
            {
                ViewBag.IsHaveAttr = false;
            }

            //其他属性值信息
            BLL.Shop.Products.AttributeValue attValueBll = new BLL.Shop.Products.AttributeValue();
            List<Model.Shop.Products.AttributeValue> attValue = attValueBll.GetModelListByCateID(CateId);
            if (attValue.Count > 0)
            {
                ViewBag.IsHaveAttrValue = true;
                ViewBag.AttributeValueList = attValue;
            }
            else
            {
                ViewBag.IsHaveAttrValue = false;
            }

            //搜索

            Model.Shop.Products.ProductSearch Searchmodel = new Model.Shop.Products.ProductSearch();
            Searchmodel.Parameter1 = BrandsId.Value;
            Searchmodel.Parameter2 = Attr1.Value;
            Searchmodel.Parameter3 = Attr2.Value;
            Searchmodel.Parameter4 = Attr3.Value;
            Searchmodel.Parameter5 = Attr4.Value;
            Searchmodel.Parameter6 = Attr5.Value;
            Searchmodel.Parameter7 = Attr6.Value;


            BLL.Shop.Products.ProductInfo productBll = new BLL.Shop.Products.ProductInfo();
            List<Model.Shop.Products.ProductInfo> productList = productBll.SearchProducts(CateId, Searchmodel);

            if (productList.Count > 0)
            {
                ViewBag.IsHaveProduct = true;
                ViewBag.ProductList = productList;
            }
            else
            {
                ViewBag.IsHaveProduct = false;
            }

            return View();
        }

        #region 参数处理
        /// <summary>
        /// 分类ID
        /// </summary>
        private int CateId
        {
            get
            {
                int cateId = 0;
                if (!string.IsNullOrWhiteSpace(Request.Params["id"]))
                {
                    cateId = YSWL.Common.Globals.SafeInt(Request.Params["id"], 0);
                }
                return cateId;
            }
        }

        /// <summary>
        /// 品牌ID
        /// </summary>
        private int? BrandsId
        {
            get
            {
                int? brandsId = 0;
                if (!string.IsNullOrWhiteSpace(Request.Params["a1"]))
                {
                    brandsId = YSWL.Common.Globals.SafeInt(Request.Params["a1"], 0);
                }
                return brandsId;
            }
        }

        /// <summary>
        /// 属性1
        /// </summary>
        private int? Attr1
        {
            get
            {
                int? attr1 = 0;
                if (!string.IsNullOrWhiteSpace(Request.Params["a2"]))
                {
                    attr1 = YSWL.Common.Globals.SafeInt(Request.Params["a2"], 0);
                }
                return attr1;
            }
        }

        /// <summary>
        ///  属性2
        /// </summary>
        private int? Attr2
        {
            get
            {
                int? attr2 = 0;
                if (!string.IsNullOrWhiteSpace(Request.Params["a3"]))
                {
                    attr2 = YSWL.Common.Globals.SafeInt(Request.Params["a3"], 0);
                }
                return attr2;
            }
        }

        /// <summary>
        ///  属性3
        /// </summary>
        private int? Attr3
        {
            get
            {
                int? attr3 = 0;
                if (!string.IsNullOrWhiteSpace(Request.Params["a4"]))
                {
                    attr3 = YSWL.Common.Globals.SafeInt(Request.Params["a4"], 0);
                }
                return attr3;
            }
        }

        /// <summary>
        ///  属性4
        /// </summary>
        private int? Attr4
        {
            get
            {
                int? attr4 = 0;
                if (!string.IsNullOrWhiteSpace(Request.Params["a5"]))
                {
                    attr4 = YSWL.Common.Globals.SafeInt(Request.Params["a5"], 0);
                }
                return attr4;
            }
        }

        /// <summary>
        ///  属性5
        /// </summary>
        private int? Attr5
        {
            get
            {
                int? attr5 = 0;
                if (!string.IsNullOrWhiteSpace(Request.Params["a6"]))
                {
                    attr5 = YSWL.Common.Globals.SafeInt(Request.Params["a6"], 0);
                }
                return attr5;
            }
        }

        /// <summary>
        ///  属性6
        /// </summary>
        private int? Attr6
        {
            get
            {
                int? attr6 = 0;
                if (!string.IsNullOrWhiteSpace(Request.Params["a7"]))
                {
                    attr6 = YSWL.Common.Globals.SafeInt(Request.Params["a7"], 0);
                }
                return attr6;
            }
        }
        #endregion

        public ActionResult DemoCompare()
        {
            return View();
        }
    }
}
