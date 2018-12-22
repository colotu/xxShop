/**
* ShoppingCartModel.cs
*
* 功 能： ShoppingCartModel
* 类 名： ShoppingCartModel
*
* Ver    变更日期             负责人  变更内容
* ───────────────────────────────────
* V0.01  2013/9/23 22:03:50  Ben    初版
*
* Copyright (c) 2013 YS56 Corporation. All rights reserved.
*┌──────────────────────────────────┐
*│　此技术信息为本公司机密信息，未经本公司书面同意禁止向第三方披露．　│
*│　版权所有：云商未来（北京）科技有限公司　　　　　　　　　　　　　　│
*└──────────────────────────────────┘
*/

namespace YSWL.MALL.ViewModel.Shop
{
    public class ShoppingCartModel
    {
        public YSWL.MALL.Model.Shop.Products.ShoppingCartInfo AllCartInfo { get; set; }
        public YSWL.MALL.Model.Shop.Products.ShoppingCartInfo SelectedCartInfo { get; set; }
        public System.Collections.Generic.Dictionary<int, System.Collections.Generic.List<YSWL.MALL.Model.Shop.Products.ShoppingCartItem>> DicSuppCartItems { get; set; }
    }
}
