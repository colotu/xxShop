using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Text;
using YSWL.Common;
using Webdiyer.WebControls.Mvc;

namespace YSWL.MALL.Web.Areas.Supplier.Controllers
{
    public class ReturnOrderController : SupplierControllerBase
    {
        //
        // GET: /Supplier/ReturnOrder/
        public YSWL.MALL.BLL.Shop.ReturnOrder.ReturnOrders returnorderBll = new BLL.Shop.ReturnOrder.ReturnOrders();
        public YSWL.MALL.BLL.Shop.ReturnOrder.ReturnOrderAction actionBll = new BLL.Shop.ReturnOrder.ReturnOrderAction();
        private YSWL.MALL.BLL.Shop.ReturnOrder.ReturnOrderItems itemBll = new BLL.Shop.ReturnOrder.ReturnOrderItems();
        private YSWL.MALL.BLL.Ms.Regions regionBll = new BLL.Ms.Regions();
        private YSWL.MALL.BLL.Shop.Products.SKUInfo skuBll = new BLL.Shop.Products.SKUInfo();

        #region 退货单列表
        public ActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// 退货单列表
        /// </summary>
        /// <param name="roc">退货单号</param>
        /// <param name="oc">订单号</param>
        /// <param name="cn">联系人</param>
        /// <param name="un">用户名</param>
        /// <param name="sd">开始日期</param>
        /// <param name="ed">结束日期</param>
        /// <param name="p"></param>
        /// <param name="viewName"></param>
        /// <returns></returns>
        public PartialViewResult List(string roc, string oc, string cn, string un, DateTime? sd, DateTime? ed, int p = 1, string viewName = "_List")
        {
            int _pageSize = 10;

            //计算分页起始索引
            int startIndex = p > 1 ? (p - 1) * _pageSize + 1 : 1;

            //计算分页结束索引
            int endIndex = p * _pageSize;
            int toalCount;//获取总条数 

            List<YSWL.MALL.Model.Shop.ReturnOrder.ReturnOrders> list = returnorderBll.GetListByPage(SupplierId, roc, oc, cn, un, sd, ed, startIndex, endIndex, out toalCount);

            if (list == null)
                return PartialView(viewName);
            PagedList<YSWL.MALL.Model.Shop.ReturnOrder.ReturnOrders> lists = new PagedList<YSWL.MALL.Model.Shop.ReturnOrder.ReturnOrders>(list, p, _pageSize, toalCount);
            return PartialView(viewName, lists);
        }

        #endregion

        #region 退货单详情
        public ActionResult Show(long rid = 0, string viewName = "Show")
        {
            YSWL.MALL.Model.Shop.ReturnOrder.ReturnOrders model = returnorderBll.GetModelByCache(rid);
            if (model != null && model.SupplierId == SupplierId)
            {
                model.ReturnTypeStr = returnorderBll.GetReturnTypeStr(model.ReturnType);
                model.ReturnGoodsTypeStr = returnorderBll.GetReturnGoodsTypeStr(model.ReturnGoodsType);


                YSWL.MALL.ViewModel.Shop.ReturnOrderDetailModel detailmodel = new YSWL.MALL.ViewModel.Shop.ReturnOrderDetailModel();
                detailmodel.Info = model;
                detailmodel.ListItems = itemBll.GetModelList(rid);
                detailmodel.ListAction = actionBll.GetModelList(rid);
 
                return View(viewName, detailmodel);
            }
            return Content("该信息不存在或者您没有权限访问！");
        }
        #endregion


        #region  审核退货单
        public ActionResult Audit(long rid = 0, string viewName = "Audit")
        {
            YSWL.MALL.Model.Shop.ReturnOrder.ReturnOrders model = returnorderBll.GetModelByCache(rid);
            if (model != null && model.SupplierId == SupplierId)
            {
                model.ReturnTypeStr = returnorderBll.GetReturnTypeStr(model.ReturnType);
                model.ReturnGoodsTypeStr = returnorderBll.GetReturnGoodsTypeStr(model.ReturnGoodsType);
                YSWL.MALL.ViewModel.Shop.ReturnOrderDetailModel detailmodel = new YSWL.MALL.ViewModel.Shop.ReturnOrderDetailModel();
                detailmodel.Info = model;
                detailmodel.ListItems = itemBll.GetModelList(rid);
                return View(viewName, detailmodel);
            }
            return Content("该信息不存在或者您没有权限访问！");
        }
        #endregion


        #region  退货单取货
        public ActionResult Pick(long rid = 0, string viewName = "Pick")
        {
            YSWL.MALL.Model.Shop.ReturnOrder.ReturnOrders model = returnorderBll.GetModelByCache(rid);
            if (model != null && model.SupplierId == SupplierId)
            {
                model.ReturnTypeStr = returnorderBll.GetReturnTypeStr(model.ReturnType);
                model.ReturnGoodsTypeStr = returnorderBll.GetReturnGoodsTypeStr(model.ReturnGoodsType);
                YSWL.MALL.ViewModel.Shop.ReturnOrderDetailModel detailmodel = new YSWL.MALL.ViewModel.Shop.ReturnOrderDetailModel();
                detailmodel.Info = model;
                detailmodel.ListItems = itemBll.GetModelList(rid);
                return View(viewName, detailmodel);
            }
            return Content("该信息不存在或者您没有权限访问！");
        }
        #endregion



        #region  退款
        public ActionResult Refund(long rid = 0, string viewName = "Refund")
        {
            YSWL.MALL.Model.Shop.ReturnOrder.ReturnOrders model = returnorderBll.GetModelByCache(rid);
            if (model != null && model.SupplierId == SupplierId)
            {
                model.ReturnTypeStr = returnorderBll.GetReturnTypeStr(model.ReturnType);
                model.ReturnGoodsTypeStr = returnorderBll.GetReturnGoodsTypeStr(model.ReturnGoodsType);
                YSWL.MALL.ViewModel.Shop.ReturnOrderDetailModel detailmodel = new YSWL.MALL.ViewModel.Shop.ReturnOrderDetailModel();
                detailmodel.Info = model;
                detailmodel.ListItems = itemBll.GetModelList(rid);
                return View(viewName, detailmodel);
            }
            return Content("该信息不存在或者您没有权限访问！");
        }
        #endregion


        #region Ajax方法

          #region 修改备注
        /// <summary>
        /// 修改订单备注
        /// </summary>
        /// <param name="fm"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult UpdateRemark(FormCollection fm)
        {
            long roid = Globals.SafeLong(fm["roid"], 0);
            if (roid <= 0)
            {
                return Content("NO");
            }
            string remark = fm["remark"];
            if (returnorderBll.UpdateRemark(roid, remark, SupplierId))
            {
                return Content("OK");
            }
            return Content("NO");
        }
        #endregion

          #region 修改取货信息
        /// <summary>
        /// 修改收货人信息
        /// </summary>
        /// <param name="fm"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult UpdatePick(FormCollection fm)
        {
            long roid = Globals.SafeLong(fm["roid"], 0);//退货单Id
            int regionId = Globals.SafeInt(fm["rid"], 0);//地区Id
            string ContactName = InjectionFilter.SqlFilter(fm["cn"]);//联系人
            string ContactPhone = InjectionFilter.SqlFilter(fm["cp"]);//联系人电话
            string PickAddress = InjectionFilter.SqlFilter(fm["pa"]);//取货地址
            if (roid <= 0 || regionId <= 0 || String.IsNullOrWhiteSpace(ContactName) || String.IsNullOrWhiteSpace(PickAddress) || String.IsNullOrWhiteSpace(ContactPhone))
            {
                return Content("NO");
            }

            YSWL.MALL.Model.Shop.ReturnOrder.ReturnOrders model = returnorderBll.GetModel(roid);
            if (model == null || model.SupplierId != SupplierId)
            {
                return Content("NO");
            }
            BLL.Ms.Regions regionBll = new BLL.Ms.Regions();
            model.PickRegionId = regionId;
            model.PickRegion = regionBll.GetRegionNameByRID(regionId);
            model.ContactName = ContactName;
            model.ContactPhone = ContactPhone;
            model.PickAddress = PickAddress;
            model.UpdatedDate = DateTime.Now;
            model.UpdatedUserId = CurrentUser.UserID;
            if (returnorderBll.Update(model))
            {
                //加操作日志
                YSWL.MALL.Model.Shop.ReturnOrder.ReturnOrderAction actionModel = new Model.Shop.ReturnOrder.ReturnOrderAction();
                int actionCode = (int)YSWL.MALL.Model.Shop.ReturnOrder.EnumHelper.ActionCode.SellerUpdatePick;
                actionModel.ActionCode = actionCode.ToString();
                actionModel.ActionDate = DateTime.Now;
                actionModel.ReturnOrderCode = model.ReturnOrderCode;
                actionModel.ReturnOrderId = model.ReturnOrderId;
                actionModel.Remark = "修改取货信息";
                actionModel.UserId = CurrentUser.UserID;
                actionModel.UserName = CurrentUser.NickName;
                actionBll.Add(actionModel);
                //清除缓存
                returnorderBll.RemoveModelCache(model.ReturnOrderId);
                return Content("OK");
            }
            else
            {
                return Content("NO");
            }

         

        }
        #endregion

          #region 修改应退金额
          [HttpPost]
        public ActionResult SaveAmountAdjusted(FormCollection fm)
        {
            long roid = Globals.SafeLong(fm["roid"], 0);//退货单Id
            decimal newAmountAdjusted = Globals.SafeDecimal(fm["aa"], -1);//应退金额
            if (newAmountAdjusted < 0) return Content("NO");

            YSWL.MALL.Model.Shop.ReturnOrder.ReturnOrders model = returnorderBll.GetModel(roid);
            if (model == null || model.SupplierId != SupplierId) return Content("NO");

            if (model.Status != (int)Model.Shop.ReturnOrder.EnumHelper.Status.UnHandle)
            {
                return Content("AUDITED");//退货单已被审核, 操作失败！
            } 

            decimal oldAmountAdjusted = model.AmountAdjusted;
            model.AmountAdjusted = newAmountAdjusted;

            if (oldAmountAdjusted == newAmountAdjusted) return Content("Repeat");//重复

            if (returnorderBll.UpdateAmountAdjusted(roid, model.ReturnOrderCode, oldAmountAdjusted, newAmountAdjusted, CurrentUser))
            {
                
                //清除缓存
                returnorderBll.RemoveModelCache(model.ReturnOrderId);
                return Content("OK");
            }
            else
            {
                return Content("NO");
            }
        }
        #endregion

          #region 拒绝
        [HttpPost]
          public ActionResult Refusal(FormCollection fm)
          {
              long roid = Globals.SafeLong(fm["roid"], 0);//退货单Id
              if (roid <= 0) {
                  return Content("NO");
              }
              YSWL.MALL.Model.Shop.ReturnOrder.ReturnOrders model = returnorderBll.GetModel(roid);
              if (model == null || model.SupplierId != SupplierId)
              {
                  return Content("NO");
              }
              if (model.Status != (int)Model.Shop.ReturnOrder.EnumHelper.Status.UnHandle)
              {
                  return Content("AUDITED");//退货单已被审核, 操作失败！
              } 
              string refuseReason = Common.InjectionFilter.SqlFilter(fm["refuseReason"]);//拒绝理由
              string remark = Common.InjectionFilter.SqlFilter(fm["remark"]);//备注
              if (String.IsNullOrWhiteSpace(refuseReason))
              {
                  return Content("REFUSEREASONISNULL");//请填写拒绝原因
              }
              model.RefuseReason = refuseReason;
              if (returnorderBll.AuditFail(roid, refuseReason, CurrentUser.UserID, remark))
              {
                  //加操作日志
                  YSWL.MALL.Model.Shop.ReturnOrder.ReturnOrderAction actionModel = new Model.Shop.ReturnOrder.ReturnOrderAction();
                  int actionCode = (int)YSWL.MALL.Model.Shop.ReturnOrder.EnumHelper.ActionCode.SellerAudit;
                  actionModel.ActionCode = actionCode.ToString();
                  actionModel.ActionDate = DateTime.Now;
                  actionModel.ReturnOrderCode = model.ReturnOrderCode;
                  actionModel.ReturnOrderId = model.ReturnOrderId;
                  actionModel.Remark = "审核未通过";
                  actionModel.UserId = CurrentUser.UserID;
                  actionModel.UserName = CurrentUser.NickName;
                  actionBll.Add(actionModel);
                  //清除缓存
                  returnorderBll.RemoveModelCache(model.ReturnOrderId);
                  return Content("OK");//操作成功
              }
              else
              {
                  return Content("NO"); 
              }
          }
          #endregion

          #region 审核通过
        [HttpPost]
          public ActionResult Pass(FormCollection fm)
          {
              long roid = Common.Globals.SafeLong(fm["roid"], 0);//退货单Id
              bool IsReturngoods = Common.Globals.SafeBool(fm["rg"], false);  //是否需要取货
              string remark = Common.InjectionFilter.SqlFilter(fm["remark"]);//备注
           
              YSWL.MALL.Model.Shop.ReturnOrder.ReturnOrders model = returnorderBll.GetModel(roid);
              if (model == null || model.SupplierId != SupplierId)
                  return Content("NO");
              if (model.AmountAdjusted < 0)
              {
                  return Content("AMOUNTADJUSTED");//应退金额不能小于0！ 
              }
              if (model.Status != (int)Model.Shop.ReturnOrder.EnumHelper.Status.UnHandle)
              {
                  return Content("AUDITED");//退货单已被审核, 操作失败！
              }
              if (IsReturngoods)//是否需要取货
              {
                  model.LogisticStatus = (int)YSWL.MALL.Model.Shop.ReturnOrder.EnumHelper.LogisticStatus.Packing;
              }
              else
              {
                  model.RefundStatus = (int)YSWL.MALL.Model.Shop.ReturnOrder.EnumHelper.RefundStatus.Apply;
              }
              model.Remark = remark;
              model.Status = (int)YSWL.MALL.Model.Shop.ReturnOrder.EnumHelper.Status.Handling;
              model.UpdatedDate = DateTime.Now;
              model.UpdatedUserId = CurrentUser.UserID;
              if (returnorderBll.AuditPass(model, IsReturngoods))
              {
                  //加操作日志
                  YSWL.MALL.Model.Shop.ReturnOrder.ReturnOrderAction actionModel = new Model.Shop.ReturnOrder.ReturnOrderAction();
                  int actionCode = (int)YSWL.MALL.Model.Shop.ReturnOrder.EnumHelper.ActionCode.SellerAudit;
                  actionModel.ActionCode = actionCode.ToString();
                  actionModel.ActionDate = DateTime.Now;
                  actionModel.ReturnOrderCode = model.ReturnOrderCode;
                  actionModel.ReturnOrderId = model.ReturnOrderId;
                  actionModel.Remark = "审核通过";
                  actionModel.UserId = CurrentUser.UserID;
                  actionModel.UserName = CurrentUser.NickName;
                  actionBll.Add(actionModel);
                  //清除缓存
                  returnorderBll.RemoveModelCache(model.ReturnOrderId);
                  return Content("OK");//操作成功
              }
              else
              {
                  return Content("NO");//操作失败
              }
          }
          #endregion

         #region 确认取货
        [HttpPost]
        public ActionResult Pick(FormCollection fm)
          {
              long roid = Common.Globals.SafeLong(fm["roid"], 0);//退货单Id
              YSWL.MALL.Model.Shop.ReturnOrder.ReturnOrders model = returnorderBll.GetModel(roid);
              if (model == null || model.SupplierId != SupplierId)
                  return Content("NO");
              if (model.Status != (int)Model.Shop.ReturnOrder.EnumHelper.Status.Handling || model.LogisticStatus <= (int)Model.Shop.ReturnOrder.EnumHelper.LogisticStatus.UnPick || model.LogisticStatus >= (int)Model.Shop.ReturnOrder.EnumHelper.LogisticStatus.Storaged)
              {
                  return Content("HASCHANGED");// 当前退货单的状态已改变,您已不能修改,稍后为您转到列表页...
              }

              if (returnorderBll.PackedOrder(model, CurrentUser))
              {
                  return Content("OK");//操作成功
              }
              else
              {
                  return Content("NO");//操作失败
              }
          }
          #endregion

        #region 退款
        [HttpPost]
        public ActionResult Refund(FormCollection fm)
        {
            long roid = Common.Globals.SafeLong(fm["roid"], 0);//退货单Id
            decimal amountActual = Globals.SafeDecimal(fm["amountActual"], -1);//实退金额
            bool isReturnCoupon = Globals.SafeBool(fm["isReturnCoupon"], false);//是否退还优惠券
            Model.Shop.ReturnOrder.ReturnOrders model = returnorderBll.GetModel(roid);
            if (model == null)
            {
                 return Content("NO");
            }    
            if (amountActual < 0)
            {
                return Content("AMOUNTACTUAL");//实退金额不能小于0！
            }
            if (model.Status != (int)Model.Shop.ReturnOrder.EnumHelper.Status.Handling || model.RefundStatus != (int)Model.Shop.ReturnOrder.EnumHelper.RefundStatus.Apply)
            {
                return Content("HASCHANGED");//当前状态不符合完成退款条件
            }
            model.AmountActual = amountActual;

            //扣除商家金额  //获取应扣除商家的金额
            decimal deductionSuppAmount = returnorderBll.GetDeductionSuppAmount(model);
            BLL.Members.UsersExp userBll = new BLL.Members.UsersExp();
            if ((userBll.GetUserBalance(CurrentUser.UserID) - deductionSuppAmount) < 0)
            {
                return Content("BALANCENOTENOUGH");//余额不足
            }
            if (returnorderBll.Refunds(model, CurrentUser, isReturnCoupon, CurrentUser.UserID, deductionSuppAmount,SupplierId))
            {
                return Content("OK");   
            }
            else
            {
                return Content("NO");
            }
        }
        #endregion

        #endregion


        #region 辅助方法
        /// <summary>
        ///  获取状态
        /// </summary>
        /// <param name="objectStatus"></param>
        /// <param name="objectLogistic"></param>
        /// <param name="objecRefund"></param>
        /// <returns></returns>
        public static string GetMainStatus(int status, int logistic, int refund)
        {
            return YSWL.MALL.BLL.Shop.ReturnOrder.ReturnOrders.GetMainStatusStr(status, logistic, refund);
        }
        /// <summary>
        /// 获取退货单追踪状态码信息
        /// </summary>
        /// <param name="actionCode"></param>
        /// <returns></returns>
        public static string GetActionCode(string actionCode)
        {
            return YSWL.MALL.BLL.Shop.ReturnOrder.ReturnOrderAction.GetActionCode(actionCode);
        }
        #endregion

        

    }
}
