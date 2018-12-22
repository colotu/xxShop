/**
* FavoriteProduct.cs
*
* 功 能： [N/A]
* 类 名： FavoriteProduct
*
* Ver    变更日期             负责人  变更内容
* ───────────────────────────────────
* V0.01  2013/6/22 17:10:53  Rock    初版
*
* Copyright (c) 2013 YS56 Corporation. All rights reserved.
*┌──────────────────────────────────┐
*│　此技术信息为本公司机密信息，未经本公司书面同意禁止向第三方披露．　│
*│　版权所有：云商未来（北京）科技有限公司　　　　　　　　　　　　　　│
*└──────────────────────────────────┘
*/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace YSWL.MALL.ViewModel.Shop
{
    public class FavoBuyProdModel:FavoProdModel
    {
        private string productcode;
        public String ProductCode
        {
            get { return productcode; }
            set { productcode = value; }
        }
    }
}
