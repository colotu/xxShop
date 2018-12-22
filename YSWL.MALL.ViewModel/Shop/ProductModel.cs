using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace YSWL.MALL.ViewModel.Shop
{
    public class ProductModel
    {
        /// <summary>
        /// 产品信息
        /// </summary>
        public Model.Shop.Products.ProductInfo ProductInfo { get; set; }
        /// <summary>
        /// 产品图片信息
        /// </summary>
        public List<Model.Shop.Products.ProductImage> ProductImages { get; set; }
        /// <summary>
        /// 产品属性信息
        /// </summary>
        public List<Model.Shop.Products.ProductAttribute> ProductAttributes { get; set; }


        public List<Model.Shop.Products.SKUInfo> ProductSkus { get; set; }

        /// <summary>
        /// Product CAtegories
        /// </summary>
        public List<Model.Shop.Products.CategoryInfo> CategoryInfos { get; set; }

        public List<Model.Shop.Package.Package> Package { get; set; }

        public List<YSWL.MALL.Model.Shop.Tags.Tags> TagList { set; get; }
        /// <summary>
        /// 产品预订信息
        /// </summary>
        public Model.Shop.PrePro.PreProduct PreProduct { get; set; }

    }


    public class SalesModel
    {
        /// <summary>
        /// 对应批发规则
        /// </summary>
        public YSWL.MALL.Model.Shop.Sales.SalesRule SalesRule { get; set; }
        /// <summary>
        /// 对应批发规则项
        /// </summary>
        public List<YSWL.MALL.Model.Shop.Sales.SalesItem> SalesItems { get; set; }
    }

    
}
