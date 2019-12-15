/**
* PaymentOption.cs
*
* 功 能： 支付模块配置
* 类 名： PaymentOption
*
* Ver    变更日期             负责人  变更内容
* ───────────────────────────────────
* V0.01  2013/5/24 1:17:23  Ben    初版
*
* Copyright (c) 2013 YS56 Corporation. All rights reserved.
*┌──────────────────────────────────┐
*│　此技术信息为本公司机密信息，未经本公司书面同意禁止向第三方披露．　│
*│　版权所有：小鸟科技（北京）科技有限公司　　　　　　　　　　　　　　│
*└──────────────────────────────────┘
*/
using YSWL.MALL.BLL.SysManage;
using YSWL.MALL.Model.Shop.Order;
using YSWL.MALL.BLL.Shop.Order;
using System;
using System.Collections.Generic;

namespace YSWL.MALL.Web.Handlers.Shop.Pay
{    
    /// <summary>
    /// 支付模块配置
    /// </summary>
    public class PaymentOption : Payment.Model.IPaymentOption<OrderInfo>
    {
        BLL.Shop.Supplier.SupplierInfo suppinfobll = new BLL.Shop.Supplier.SupplierInfo();

        BLL.SysManage.ConfigSystem consys = new ConfigSystem();
        BLL.Members.Users userbll = new BLL.Members.Users();
        
        BLL.Members.PointsDetail pointbll = new BLL.Members.PointsDetail();
        #region 成员
        private readonly BLL.Shop.Order.Orders _orderManage = new BLL.Shop.Order.Orders();
        private const string _returnUrl = "/pay/payment/{0}/return_url.aspx";
        private const string _notifyUrl = "/pay/payment/{0}/notify_url.aspx"; 
        #endregion

        #region IPaymentOption 成员
        /// <summary>
        /// 异步通知地址
        /// </summary>
        public string NotifyUrl
        {
            get { return _notifyUrl; }
        }
        /// <summary>
        /// 支付返回地址
        /// </summary>
        public string ReturnUrl
        {
            get { return _returnUrl; }
        }

        #region 获取订单信息
        /// <summary>
        /// 获取订单信息
        /// </summary>
        /// <param name="orderIdStr">订单ID</param>
        public OrderInfo GetOrderInfo(string orderIdStr)
        {
            long orderId = Common.Globals.SafeLong(orderIdStr, -1);
            if (orderId < 1) return null;

            //返回订单信息
            return _orderManage.GetModel(orderId);
        }
        #endregion

        /// <summary>
        /// 已验证支付网站签名 继续完成支付
        /// </summary>
        /// <param name="orderInfo">订单信息</param>
        public bool PayForOrder(OrderInfo orderInfo)
        {
            bool retubool = true;
            //更新订单为已支付 返回结果
            retubool = OrderManage.PayForOrder(orderInfo);

            #region  订单状态修改完之后，扣商城积分
            //if (retubool&& orderInfo.PaymentGateway!="gouwujifencount")
            //{
            //    #region 扣除会员商城积分

            //    decimal orderGwjf = orderInfo.Gwjf;//订单中抵扣的商城积分。
            //    int userGwJF = spcom.GetPointByUsername(orderInfo.BuyerName);//获取用户的商城积分

            //    // 商城积分抵扣金额开始
            //    //获取积分抵扣商品
            //    if (orderGwjf > 0 && userGwJF > 0)//商品订单积分大于用户商城积分,并用户积分大于0，进行抵扣
            //    {
            //        if (orderGwjf > decimal.Parse(userGwJF.ToString()))
            //        {
            //            orderGwjf = userGwJF;
            //        }
                    
            //        int PointSop = int.Parse(orderGwjf.ToString().Substring(0, orderGwjf.ToString().IndexOf('.')));


            //        pointbll.PointsHuzhuan(-2, orderInfo.BuyerID, "抵扣商品订单号：" + orderInfo.OrderCode + "，抵扣商城积分：" + orderGwjf + "", PointSop, "", 1);//把

            //    }
            //    else
            //    {
            //    }
            //    //商城积分抵扣金额开始
                
            //    #endregion
            //}
            //订单状态修改完之后，扣商城积分
            #endregion


            #region 订单状态支付完成后给 提成 购物返佣金


            #region  给店铺增加消费积分记录
            
            string strsuppEmpid = userbll.GetEmployeeIDByUserid(orderInfo.BuyerID.ToString()).ToString();//获取所有店铺的会员编号
            string suppid = suppinfobll.GetSuppidBywhere(" UserId='" + strsuppEmpid + "'");//店铺会员编号获得店铺ID
            if (suppid.Trim().Length > 0)
            {
                int dpxfjf = (int)Math.Round(orderInfo.OrderTotal, 0);
                suppinfobll.UpdateSupXfPoint(dpxfjf, int.Parse(suppid));//增加商家的消费积分金额 
            }
            
            #endregion

            #region 购物返佣金（）

            bool isOpen = YSWL.MALL.BLL.SysManage.ConfigSystem.GetBoolValueByCache("Shop_Commssion_IsOpen");
            if (isOpen)
            {
                //YSWL.MALL.BLL.Shop.Commission.CommissionPro comProBll = new CommissionPro();

                YSWL.MALL.BLL.Shop.Commission.CommissionPro comProBll = new BLL.Shop.Commission.CommissionPro();
                comProBll.AddCommissionT(orderInfo);
            }

            #endregion


            #endregion




            return retubool;
        }
        #endregion
    }
}