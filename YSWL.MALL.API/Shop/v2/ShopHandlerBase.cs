/**
* ShopHandlerBase.cs
*
* 功 能： Shop Json.RPC API 基类
* 类 名： ShopHandlerBase
*
* Ver    变更日期             负责人  变更内容
* ───────────────────────────────────
* V0.01  2013/12/17 17:04:23  Ben    初版
*
* Copyright (c) 2012 YS56 Corporation. All rights reserved.
*┌──────────────────────────────────┐
*│　此技术信息为本公司机密信息，未经本公司书面同意禁止向第三方披露．　│
*│　版权所有：云商未来（北京）科技有限公司　　　　　　　　　　　　　　│
*└──────────────────────────────────┘
*/

using System;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Web.SessionState;
using YSWL.Components;
using YSWL.Json.RPC.Web;

namespace YSWL.MALL.API.Shop.v2
{
    public partial class ShopHandler : YSWL.MALL.API.Shop.v1.ShopHandler
    {
        public ShopHandler() : base(false)
        {
            
        }

    }
}