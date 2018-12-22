using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using YSWL.MALL.BLL.SysManage;

namespace YSWL.MALL.BLL.Shop.Service
{
    public class OMSServiceHelper
    {

        public static  OMSServiceHelper DefaultHelper
        {
            get { return new OMSServiceHelper(); }
        }

        public OMSServiceHelper()
        {
            long enterpriseId = Common.Globals.SafeLong(YSWL.Common.CallContextHelper.GetAutoTag(), 0);
            Common.CallContextHelper.SetAutoTag(enterpriseId);
        }

        //private  bool IsConnectionOMS= YSWL.MALL.BLL.Shop.Service.CommonHelper.ConnectionOMS();//是否对接OMS 系统
        /// <summary>
        /// 同步订单
        /// </summary>
        /// <returns></returns>
        //public  bool SyncOrder(YSWL.MALL.Model.Shop.Order.OrderInfo model)
        //{
        //    if (!IsConnectionOMS)
        //    {
        //        return true;//未开启OMS对接 直接返回。
        //    }
        //    if (model == null) return false;
   
        //       OMS.OrderInfo orderInfo = new OMS.OrderInfo
        //       {
        //           _sourceorderid = model.OrderId,
        //           _sourcetype = 0,
        //           _activityfreeamount = model.ActivityFreeAmount,
        //           _activitystatus = model.ActivityStatus,
        //           _amount = model.Amount,
        //           _activityname = model.ActivityName,
        //           _buyercellphone = model.BuyerCellPhone,
        //           _buyeremail = model.BuyerEmail,
        //           _buyerid = model.BuyerID,
        //           _buyername = model.BuyerName,
        //           _commentstatus = model.CommentStatus,
        //           _couponamount = model.CouponAmount,
        //           _couponcode = model.CouponCode,
        //           _couponname = model.CouponName,
        //           _couponvaluetype = model.CouponValueType,
        //           _couponvalue = model.CouponValue,
        //           _createddate = model.CreatedDate,
        //           _createuserid = model.CreateUserId,
        //           _discountadjusted = model.DiscountAdjusted,
        //           _depotname = model.DepotName,
        //           _discountamount = model.DiscountAmount,
        //           _discountname = model.DiscountName,
        //           _discountvalue = model.DiscountValue,
        //           _discountvaluetype = model.DiscountValueType,
        //           _expresscompanyabb = model.ExpressCompanyAbb,
        //           _expresscompanyname = model.ExpressCompanyName,
        //           _freight = model.Freight,
        //           _freightactual = model.FreightActual,
        //           _freightadjusted = model.FreightAdjusted,
        //           _gatewayorderid = model.GatewayOrderId,
        //           _groupbuyid = model.GroupBuyId,
        //           _groupbuyprice = model.GroupBuyPrice,
        //           _groupbuystatus = model.GroupBuyStatus,
        //           _haschildren = model.HasChildren,
        //           _isfreeshipping = model.IsFreeShipping,
        //           _isreviews = model.IsReviews,
        //           _ordercode = model.OrderCode,
        //           _ordercostprice = model.OrderCostPrice,
        //           _orderip = model.OrderIP,
        //           _orderoptionprice = model.OrderOptionPrice,
        //           _orderothercost = model.OrderOtherCost,
        //           _orderpoint = model.OrderPoint,
        //           _orderprofit = model.OrderProfit,
        //           _orderstatus = model.OrderStatus,
        //           _ordertotal = model.OrderTotal,
        //           _ordertype = model.OrderType,
        //           _ordertypesub = model.OrderTypeSub,
        //           _originalid = model.OriginalId,
        //           _parentorderid = model.ParentOrderId,
        //           _paycurrencycode = model.PayCurrencyCode,
        //           _paycurrencyname = model.PayCurrencyName,
        //           _paymentfee = model.PaymentFee,
        //           _paymentfeeadjusted = model.PaymentFeeAdjusted,
        //           _paymentgateway = model.PaymentGateway,
        //           _paymentstatus = model.PaymentStatus,
        //           _paymenttypeid = model.PaymentTypeId,
        //           _paymenttypename = model.PaymentTypeName,
        //           _producttotal = model.ProductTotal,
        //           _realshippingmodeid = model.RealShippingModeId,
        //           _realshippingmodename = model.RealShippingModeName,
        //           _referid = model.ReferID,
        //           _refertype = model.ReferType,
        //           _referurl = model.ReferURL,
        //           _refundstatus = model.RefundStatus,
        //           _regionid = model.RegionId,
        //           _remark = model.Remark,
        //           _sellercellphone = model.SellerCellPhone,
        //           _selleremail = model.SellerEmail,
        //           _sellerid = model.SellerID,
        //           _sellername = model.SellerName,
        //           _shipcellphone = model.ShipCellPhone,
        //           _shipaddress = model.ShipAddress,
        //           _shipemail = model.ShipEmail,
        //           _shipname = model.ShipName,
        //           _shipordernumber = model.ShipOrderNumber,
        //           _shipperaddress = model.ShipperAddress,
        //           _shippercellphone = model.ShipperCellPhone,
        //           _shipperid = model.ShipperId,
        //           _shippername = model.ShipperName,
        //           _shippingmodeid = model.ShippingModeId,
        //           _shippingmodename = model.ShippingModeName,
        //           _shippingstatus = model.ShippingStatus,
        //           _shipregion = model.ShipRegion,
        //           _shiptelphone = model.ShipTelPhone,
        //           _shipzipcode = model.ShipZipCode,
        //           _supplierid = model.SupplierId,
        //           _suppliername = model.SupplierName,
        //           _updateddate = model.UpdatedDate,
        //           _weight = model.Weight
        //       };
        //       orderInfo._orderItems = model.OrderItems.Select(a => new OMS.OrderItems()
        //       {
        //           _itemid = a.ItemId,
        //           _adjustedprice = a.AdjustedPrice,
        //           _sourcetype = 0,
        //           _sourceorderid = model.OrderId,
        //           _attribute = a.Attribute,
        //           _brandid = a.BrandId,
        //           _brandname = a.BrandName,
        //           _costprice = a.CostPrice,
        //           _deduct = a.Deduct,
        //           _description = a.Description,
        //           _name = a.Name,
        //           _ordercode = a.OrderCode,
        //           _points = a.Points,
        //           _productcode = a.ProductCode,
        //           _productid = a.ProductId,
        //           _referid = a.ReferId,
        //           _refertype = a.ReferType,
        //           _productlineid = a.ProductLineId,
        //           _producttype = a.ProductType,
        //           _quantity = a.Quantity,
        //           _remark = a.Remark,
        //           _sellprice = a.SellPrice,
        //           _shipmentquantity = a.ShipmentQuantity,
        //           _sku = a.SKU,
        //           _supplierid = a.SupplierId,
        //           _suppliername = a.SupplierName,
        //           _weight = a.Weight,
        //           _thumbnailsurl = a.ThumbnailsUrl
        //       }).ToArray();
        //       try
        //       {
        //           using (OMS.ServiceClient client = new ServiceClient())
        //           {
        //               long enterpriseId = Common.Globals.SafeLong(YSWL.Common.CallContextHelper.GetAutoTag(), 0);
        //               Common.CallContextHelper.SetAutoTag(enterpriseId);
        //               return client.SyncOrder(orderInfo);
        //           }
        //       }
        //       catch (Exception ex)
        //       {
        //           LogHelp.AddErrorLog("同步OMS订单失败: " + ex.Message, ex.StackTrace, HttpContext.Current.Request);
        //           throw;
        //       }
      
        //    return true;
        //}
        /// <summary>
        /// 完成订单
        /// </summary>
        /// <param name="orderId"></param>
        /// <param name="userId"></param>
        /// <param name="userName"></param>
        /// <returns></returns>
        //public  bool CompleteOrder(long orderId, int userId, string userName)
        //{
        //    if (!IsConnectionOMS)
        //    {
        //        return true;//未开启OMS对接 直接返回。
        //    }
         
        //    try
        //    {
        //        using (OMS.ServiceClient client = new ServiceClient())
        //        {
        //                return client.CompleteOrder(orderId, userId, userName);
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        LogHelp.AddErrorLog("同步完成订单失败: " + ex.Message, ex.StackTrace, HttpContext.Current.Request);
        //        throw;
        //    }
      
        //    return true;
        //}

        /// <summary>
        /// 同步OMS默认仓库设置
        /// </summary>
        /// <param name="depotId"></param>
        //public  bool SyncDefaultDepot(int depotId)
        //{
        //    if (!IsConnectionOMS)
        //    {
        //        return true;//未开启OMS对接 直接返回。
        //    }
        //    Func<bool> func = () =>
        //    {
        //        try
        //        {
        //            using (OMS.ServiceClient client = new ServiceClient())
        //            {
        //                client.SyncDefaultDepot(depotId);
        //                return true;
        //            }
        //        }
        //        catch (Exception ex)
        //        {
        //            LogHelp.AddErrorLog("同步完成订单失败: " + ex.Message, ex.StackTrace, HttpContext.Current.Request);
        //            throw;
        //        }
        //    };
        //    YSWL.Common.ServiceExecHelper.AddQueue(func);
        //    return true;
        //}

        //#region 商家订单操作
        ///// <summary>
        ///// 商家备货
        ///// </summary>
        ///// <param name="orderId"></param>
        ///// <param name="userId"></param>
        ///// <param name="userName"></param>
        ///// <returns></returns>
        //public  bool SpPickOrder(long orderId, int userId, string userName)
        //{
        //    if (!IsConnectionOMS)
        //    {
        //        return true;//未开启OMS对接 直接返回。
        //    }
        //    Func<bool> func = () =>
        //    {
        //        try
        //        {
        //            using (OMS.ServiceClient client = new ServiceClient())
        //            {
        //                client.SpPickOrder(orderId, userId, userName);
        //                return true;
        //            }
        //        }
        //        catch (Exception ex)
        //        {
        //            LogHelp.AddErrorLog("同步商家备货订单失败: " + ex.Message, ex.StackTrace, HttpContext.Current.Request);
        //            throw;
        //        }
        //    };
        //    YSWL.Common.ServiceExecHelper.AddQueue(func);
        //    return true;
        //}
        ///// <summary>
        ///// 商家发货
        ///// </summary>
        ///// <param name="orderId"></param>
        ///// <param name="userId"></param>
        ///// <param name="userName"></param>
        ///// <param name="freightAdjusted"></param>
        ///// <param name="freightActual"></param>
        ///// <param name="shipNumber"></param>
        ///// <param name="expressName"></param>
        ///// <param name="expressAbb"></param>
        ///// <returns></returns>
        //public  bool SpShipOrder(long orderId, int userId, string userName, decimal freightAdjusted,
        //    decimal freightActual, string shipNumber, string expressName, string expressAbb)
        //{
        //    if (!IsConnectionOMS)
        //    {
        //        return true;//未开启OMS对接 直接返回。
        //    }
        //        try
        //        {
        //            using (OMS.ServiceClient client = new ServiceClient())
        //            {
        //                client.SpShipOrder(orderId, userId, userName,freightAdjusted,freightActual,shipNumber,expressName,expressAbb);
        //                return true;
        //            }
        //        }
        //        catch (Exception ex)
        //        {
        //            LogHelp.AddErrorLog("同步商家订单发货操作失败: " + ex.Message, ex.StackTrace, HttpContext.Current.Request);
        //            throw;
        //        }
          
        //    return true;
        //}
        ///// <summary>
        ///// 更新订单备注
        ///// </summary>
        ///// <param name="orderId"></param>
        ///// <param name="Remark"></param>
        ///// <returns></returns>
        //public  bool UpdateRemark(long orderId, string Remark)
        //{
        //    if (!IsConnectionOMS)
        //    {
        //        return true;//未开启OMS对接 直接返回。
        //    }
     
        //        try
        //        {
        //            using (OMS.ServiceClient client = new ServiceClient())
        //            {
        //                client.UpdateRemark(orderId, Remark);
        //                return true;
        //            }
        //        }
        //        catch (Exception ex)
        //        {
        //            LogHelp.AddErrorLog("同步修改商家订单备注信息失败: " + ex.Message, ex.StackTrace, HttpContext.Current.Request);
        //            throw;
        //        }
         
        //    return true;
        //}
        ///// <summary>
        ///// 更新收货人信息
        ///// </summary>
        ///// <param name="orderId"></param>
        ///// <param name="userId"></param>
        ///// <param name="userName"></param>
        ///// <param name="regionId"></param>
        ///// <param name="shipName"></param>
        ///// <param name="shipAddress"></param>
        ///// <param name="shipTelPhone"></param>
        ///// <param name="shipCellPhone"></param>
        ///// <param name="shipZipCode"></param>
        ///// <returns></returns>
        //public  bool UpdateShipInfo(long orderId, int userId, string userName, int regionId, string shipName,
        //    string shipAddress,
        //    string shipTelPhone, string shipCellPhone, string shipZipCode)
        //{
        //    if (!IsConnectionOMS)
        //    {
        //        return true;//未开启OMS对接 直接返回。
        //    }
        //    Func<bool> func = () =>
        //    {
        //        try
        //        {
        //            using (OMS.ServiceClient client = new ServiceClient())
        //            {
        //                client.UpdateShipInfo(orderId, userId, userName,regionId,shipName,shipAddress,shipTelPhone,shipCellPhone,shipZipCode);
        //                return true;
        //            }
        //        }
        //        catch (Exception ex)
        //        {
        //            LogHelp.AddErrorLog("同步修改商家订单收货地址失败: " + ex.Message, ex.StackTrace, HttpContext.Current.Request);
        //            throw;
        //        }
        //    };
        //    YSWL.Common.ServiceExecHelper.AddQueue(func);
        //    return true;
        //}

        //#endregion 
    }
}
