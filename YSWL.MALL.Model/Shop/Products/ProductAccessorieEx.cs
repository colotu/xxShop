/*----------------------------------------------------------------
// Copyright (C) 2012 云商未来 版权所有。 
//
// 文件名：ProductAccessorieEx.cs
// 文件功能描述：
// 
// 创建标识：
// 修改标识：
// 修改描述：
//
// 修改标识：
// 修改描述：
//----------------------------------------------------------------*/

using System;
using System.Collections.Generic;
using System.Text;

namespace YSWL.MALL.Model.Shop.Products
{
   public partial class ProductAccessorie
    {
        private string _skuId;

        public string SkuId
        {
            get { return _skuId; }
            set { _skuId = value; }
        }
    }
}
