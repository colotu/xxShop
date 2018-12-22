/**
* PayAndShipModel.cs
*
* 功 能： [N/A]
* 类 名： PayAndShipModel
*
* Ver    变更日期             负责人  变更内容
* ───────────────────────────────────
* V0.01  2013/6/20 15:14:10  Ben    初版
*
* Copyright (c) 2013 YS56 Corporation. All rights reserved.
*┌──────────────────────────────────┐
*│　此技术信息为本公司机密信息，未经本公司书面同意禁止向第三方披露．　│
*│　版权所有：云商未来（北京）科技有限公司　　　　　　　　　　　　　　│
*└──────────────────────────────────┘
*/

using System.Collections.Generic;
namespace YSWL.MALL.ViewModel.Shop
{
    public class PayAndShip
    {
        public List<YSWL.Payment.Model.PaymentModeInfo> ListPaymentMode { get; set; }
        public List<YSWL.MALL.Model.Shop.Shipping.ShippingType> ListShippingType { get; set; }

        public YSWL.Payment.Model.PaymentModeInfo CurrentPaymentMode { get; set; }
        public Model.Shop.Shipping.ShippingType CurrentShippingType { get; set; }
    }
}
