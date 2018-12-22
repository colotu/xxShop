/**
* DemoProductSearch.cs
*
* 功 能： [N/A]
* 类 名： DemoProductSearch
*
* Ver    变更日期             负责人  变更内容
* ───────────────────────────────────
* V0.01  2012/7/18 11:58:08  Rock    初版
*
* Copyright (c) 2012 YS56 Corporation. All rights reserved.
*┌──────────────────────────────────┐
*│　此技术信息为本公司机密信息，未经本公司书面同意禁止向第三方披露．　│
*│　版权所有：小鸟科技（北京）科技有限公司　　　　　　　　　　　　　　│
*└──────────────────────────────────┘
*/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace YSWL.MALL.Web.Models
{
    public class DemoProductSearch
    {
        public List<Model.Shop.Products.BrandInfo> BrandList
        {
            get;
            set;
        }

        public bool IsHaveBrands { get; set; }
    }
}