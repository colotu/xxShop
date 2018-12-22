﻿/**
* IProductService.cs
*
* 功 能： Shop模块-产品相关 多表事务操作接口
* 类 名： IProductService
*
* Ver    变更日期             负责人  变更内容
* ───────────────────────────────────
* V0.01  2012/6/22 10:47:31  Ben    初版
*
* Copyright (c) 2012 YS56 Corporation. All rights reserved.
*┌──────────────────────────────────┐
*│　此技术信息为本公司机密信息，未经本公司书面同意禁止向第三方披露．　│
*│　版权所有：云商未来（北京）科技有限公司　　　　　　　　　　　　　　│
*└──────────────────────────────────┘
*/

using System.Collections.Generic;
using System.Data;
using System.Linq;
namespace YSWL.MALL.IDAL.Shop.Products
{
    /// <summary>
    /// Shop模块-产品相关 含多表事务操作接口
    /// </summary>
    public interface IProductService
    {
        bool AddProduct(Model.Shop.Products.ProductInfo productInfo, out long ProductId);
        bool AddProductOne(Model.Shop.Products.ProductInfo productInfo);

        bool SubUpdateList(string depotIds, Model.Shop.Products.ProductInfo productInfo);
        bool ModifyProduct(Model.Shop.Products.ProductInfo productInfo, int oldSaleStatus);
        bool AddSuppProduct(Model.Shop.Products.ProductInfo productInfo, out long ProductId);
        bool ModifySuppProduct(Model.Shop.Products.ProductInfo productInfo,int oldSaleStatus);
        System.Data.DataSet GetCompareProudctInfo(string ids);
        System.Data.DataSet GetCompareProudctBasicInfo(string ids);

        bool ModifyPMSProduct(Model.Shop.Products.ProductInfo productInfo);

        bool AddPMSProduct(Model.Shop.Products.ProductInfo productInfo);
        bool ModifyStock(string sku, int stock);

        bool ImportModifyProduct(Model.Shop.Products.ProductInfo productInfo);
    }
}
