/**
* IOrderSync.cs
*
* 功 能： Shop模块-订单同步 跨库操作类
* 类 名： IOrderSync
*
* Ver    变更日期             负责人  变更内容
* ───────────────────────────────────
* V0.01  2014/04/10 16:25:45  Ben    初版
*
* Copyright (c) 2014 YS56 Corporation. All rights reserved.
*┌──────────────────────────────────┐
*│　此技术信息为本公司机密信息，未经本公司书面同意禁止向第三方披露．　│
*│　版权所有：云商未来（北京）科技有限公司　　　　　　　　　　　　　　│
*└──────────────────────────────────┘
*/

using YSWL.MALL.Model.Shop.Order;
using System;
namespace YSWL.MALL.IDAL.Shop.Order
{
    /// <summary>
    /// Shop模块-订单同步 跨库操作类
    /// </summary>
    public interface IOrderSync
    {
        long CreateOrder(Model.Shop.Order.OrderInfo orderInfo, bool borrowEnable);
    }
}
