using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using YSWL.MALL.BLL.Ms;
using YSWL.MALL.BLL.Shop.Order;
using YSWL.Common;

namespace YSWL.MALL.Web.Admin.Shop.Order
{
    public partial class OrderShow : PageBaseAdmin
    {
        protected override int Act_PageLoad { get { return 445; } } //Shop_订单管理_详细页
        private int Act_UpdateOrderAmount = 694;  //Shop_订单管理_修改订单应付金额

        private YSWL.MALL.BLL.Shop.Order.Orders orderBll = new Orders();
        private YSWL.MALL.BLL.Shop.Order.OrdersHistory historyBll = new OrdersHistory();
        private YSWL.MALL.BLL.Shop.Order.OrderItems itemBll = new OrderItems();
        private YSWL.MALL.BLL.Shop.Order.OrderAction actionBll = new OrderAction();
        private YSWL.MALL.BLL.Shop.Order.OrderRemark remarkBll = new OrderRemark();
        private YSWL.MALL.BLL.Ms.Regions regionBll = new Regions();
        private YSWL.MALL.BLL.Shop.Products.SKUInfo skuBll = new BLL.Shop.Products.SKUInfo();
        private YSWL.MALL.BLL.Shop.Order.OrderOptions optionBll = new YSWL.MALL.BLL.Shop.Order.OrderOptions();
        public int TabIndex = 0;
        public bool IsConnectionOMS = false;
        protected void Page_Load(object sender, EventArgs e)
        {
            TabIndex = Common.Globals.SafeInt(hfTabIndex.Value,0);
            if (!Page.IsPostBack)
            {
                IsConnectionOMS = YSWL.MALL.BLL.Shop.Service.CommonHelper.ConnectionOMS();
                ShowInfo();
                //获取查询快递单类型
                string apiType = YSWL.MALL.BLL.SysManage.ConfigSystem.GetValueByCache("OpenAPI_Express_ApiType");
                if (apiType == "html")
                {
                   string url= YSWL.MALL.Web.Components.ExpressHelper.GetHtmlExpress(OrderId);
                    this.hfExpressUrl.Value = url;
                }
                else
                {
                    BindExpress();
                }
            }
        }


        #region 订单Id
        /// <summary>
        /// 订单Id
        /// </summary>
        public int OrderId
        {
            get
            {
                return YSWL.Common.Globals.SafeInt(Request.Params["orderId"], 0);
            }
        }
        #endregion

        #region 订单状态
        /// <summary>
        /// 订单状态
        /// </summary>
        public int OrderStatus
        {
            get
            {
                return YSWL.Common.Globals.SafeInt(Request.Params["type"], 0);
            }
        }
        #endregion

        private void ShowInfo()
        {
            if (OrderStatus == -1)
            {
                YSWL.MALL.Model.Shop.Order.OrdersHistory Historymodel = historyBll.GetModelByCache(OrderId);

                var optionModle = optionBll.Get2ListByOrderId(OrderId);
                if (optionModle != null && optionModle.Count > 1)
                {
                    this.lbITitle.Text= optionModle[0].ItemDescription +":"+optionModle[0].CustomerTitle;
                    this.lbIContent.Text = optionModle[1].ItemDescription;
                }

                if (Historymodel != null)
                {
                    this.lblTitle.Text = "你正在查看订单【" + Historymodel.OrderCode + "】的详细信息";
                    //收货人信息
                    this.txtShipName.Text = Historymodel.ShipName;
                    txtShipZipCode.Text = Historymodel.ShipZipCode;
                    txtShipAddress.Text = Historymodel.ShipAddress;
                    txtShipCellPhone.Text = Historymodel.ShipCellPhone;
                    lblShipEmail.Text = Historymodel.ShipEmail;
                    // txtShipRegion.Text = Historymodel.ShipRegion;
                    RegionList.Region_iID = Historymodel.RegionId.HasValue ? Historymodel.RegionId.Value : 0;
                    txtShipTelPhone.Text = Historymodel.ShipTelPhone;
                    //买家信息
                    lblBuyerEmail.Text = Historymodel.BuyerEmail;
                    lblBuyerCellPhone.Text = Historymodel.BuyerCellPhone;
                    lblBuyerName.Text = Historymodel.BuyerName;
                    //订单价格信息
                    lblDiscountAdjusted.Text = Historymodel.DiscountAdjusted.HasValue
                                                   ? Historymodel.DiscountAdjusted.Value.ToString("F")
                                                   : "0";
                    lblFreight.Text = Historymodel.Freight.HasValue
                       ? Historymodel.Freight.Value.ToString("F") : "0";
                    lblFreightAdjusted.Text = Historymodel.FreightAdjusted.HasValue
                        ? Historymodel.FreightAdjusted.Value.ToString("F") : "0";
                    lblFreightActual.Text = Historymodel.FreightActual.HasValue
                        ? Historymodel.FreightActual.Value.ToString("F") : "0";
                    lblOrderTotal.Text = Historymodel.OrderTotal.ToString("F");
                    //lblTotalPrice.Text = Historymodel
                    lblAmount.Text = Historymodel.Amount.ToString("F");
                    lblCouponAmount.Text = (Historymodel.OrderTotal - Historymodel.Amount).ToString("F");

                    //优惠劵信息
                    if(!String.IsNullOrWhiteSpace( Historymodel.CouponCode)){
                        trCoupon.Visible=true;
                        lblCouponCode.Text = Historymodel.CouponCode;
                        lblCouponName.Text = Historymodel.CouponName;
                        labelCouponAmount.Text=Historymodel.CouponAmount.HasValue?Historymodel.CouponAmount.Value.ToString("F"):"0.00";
                    }

                    //订单其它信息
                    lblPaymentTypeName.Text = Historymodel.PaymentTypeName;
                    lblRealShippingModeName.Text = Historymodel.RealShippingModeName;
                    lblShippingModeName.Text = Historymodel.ShippingModeName;
                    hidShippingModeId.Value = Historymodel.ShippingModeId.HasValue ? Historymodel.ShippingModeId.Value.ToString() : "-1";
                    hidRealShippingModeId.Value = Historymodel.RealShippingModeId.HasValue ? Historymodel.RealShippingModeId.Value.ToString() : "-1";
                    lblPoint.Text = Historymodel.OrderPoint.ToString();
                    lblExpressCompanyName.Text = Historymodel.ExpressCompanyName;
                    lblWeight.Text = Historymodel.Weight.ToString();

                    lblShipOrderNumber.Text = string.IsNullOrWhiteSpace(Historymodel.ShipOrderNumber) ? "无" : Historymodel.ShipOrderNumber;

                    txtRemark.Text = Historymodel.Remark;
                    hfOrderMainStatus.Value = "9";
                }
            }
            else
            {
                YSWL.MALL.Model.Shop.Order.OrderInfo model = orderBll.GetModelByCache(OrderId);
                var optionModle = optionBll.Get2ListByOrderId(OrderId);
                if (optionModle != null&&optionModle.Count>1)
                {
                    this.lbITitle.Text = optionModle[0].ItemDescription + ":" + optionModle[0].CustomerTitle;
                    this.lbIContent.Text = optionModle[1].ItemDescription;
                }
                if (model != null)
                {
                    this.lblTitle.Text = "正在查看订单【" + model.OrderCode + "】的详细信息";
                    this.txtShipName.Text = model.ShipName;
                    txtShipZipCode.Text = model.ShipZipCode;
                    txtShipAddress.Text = model.ShipAddress;
                    txtShipCellPhone.Text = model.ShipCellPhone;
                    lblShipEmail.Text = model.ShipEmail;
                    // txtShipRegion.Text = Historymodel.ShipRegion;
                    RegionList.Region_iID = model.RegionId;
                    txtShipTelPhone.Text = model.ShipTelPhone;

                    lblBuyerEmail.Text = model.BuyerEmail;
                    lblBuyerCellPhone.Text = model.BuyerCellPhone;
                    lblBuyerName.Text = model.BuyerName;

                    //订单价格信息
                    lblDiscountAdjusted.Text = model.DiscountAdjusted.HasValue
                                                   ? model.DiscountAdjusted.Value.ToString("F")
                                                   : "0";
                    lblFreight.Text = model.Freight.HasValue
                        ? model.Freight.Value.ToString("F") : "0";
                    lblFreightAdjusted.Text = model.FreightAdjusted.HasValue
                        ? model.FreightAdjusted.Value.ToString("F") : "0";
                    lblFreightActual.Text = model.FreightActual.HasValue
                        ? model.FreightActual.Value.ToString("F") : "0";
                    lblOrderTotal.Text = model.OrderTotal.ToString("F");
                    lblTotalPrice.Text = model.ProductTotal.ToString("F");
                    lblAmount.Text = model.Amount.ToString("F");
                    lblCouponAmount.Text = (model.OrderTotal - model.Amount).ToString("F");


                    //优惠劵信息
                    if (!String.IsNullOrWhiteSpace(model.CouponCode))
                    {
                        trCoupon.Visible = true;
                        lblCouponCode.Text = model.CouponCode;
                        lblCouponName.Text = model.CouponName;
                        labelCouponAmount.Text = model.CouponAmount.HasValue ? model.CouponAmount.Value.ToString("F") : "0.00";
                    }


                    //订单其它信息
                    lblPaymentTypeName.Text = model.PaymentTypeName;
                    lblRealShippingModeName.Text = model.RealShippingModeName;
                    lblShippingModeName.Text = model.ShippingModeName;
                    hidShippingModeId.Value = model.ShippingModeId.HasValue ? model.ShippingModeId.Value.ToString() : "-1";
                    hidRealShippingModeId.Value = model.RealShippingModeId.HasValue ? model.RealShippingModeId.Value.ToString() : "-1";
                    lblPoint.Text = model.OrderPoint.ToString();
                    lblExpressCompanyName.Text = model.ExpressCompanyName;
                    lblWeight.Text = model.Weight.ToString();

                    lblShipOrderNumber.Text = string.IsNullOrWhiteSpace(model.ShipOrderNumber) ? "无" : model.ShipOrderNumber;

                    txtRemark.Text = model.Remark;
                    hfOrderMainStatus.Value = ((int)orderBll.GetOrderType(model.PaymentGateway, model.OrderStatus,
                                                                    model.PaymentStatus, model.ShippingStatus)).ToString();

                    //如果对接了OMS系统   货到付款   支付方式不可以修改支付金额的
                    if (IsConnectionOMS)
                    {
                        if (model.PaymentGateway != "cod")//不是货到付款支付方式
                        { 
                            if (model.OrderStatus > -1 && model.OrderStatus < 2 && model.PaymentStatus < 2 &&
                       (GetPermidByActID(Act_UpdateOrderAmount) == -1 || UserPrincipal.HasPermissionID(GetPermidByActID(Act_UpdateOrderAmount))))
                        {
                            txtAmount.Visible = true;
                            imgModifyAmount.Visible = true;
                            lnkSaveAmount.Visible = true;
                            gridView_Action.Columns[2].Visible = true;
                            btnSaveRemark.Visible = true;
                            btnSave.Visible = true;
                        }
                        }
                    }
                    else
                    {
                        btnSave.Visible = true;
                        btnSaveRemark.Visible = true;
                        if (model.OrderStatus > -1 && model.OrderStatus < 2 && model.PaymentStatus < 2 &&
                       (GetPermidByActID(Act_UpdateOrderAmount) == -1 || UserPrincipal.HasPermissionID(GetPermidByActID(Act_UpdateOrderAmount))))
                        {
                            txtAmount.Visible = true;
                            imgModifyAmount.Visible = true;
                            lnkSaveAmount.Visible = true;
                            gridView_Action.Columns[2].Visible = true;
                        }
                    }
                }
            }

        }

        public void BindData()
        {
            gridView.DataSetSource = itemBll.GetListByCache(" OrderId=" + OrderId);
        }

        public void BindAction()
        {
            gridView_Action.DataSetSource = actionBll.GetList(" OrderId=" + OrderId);
        }

        public void BindRemark()
        {
            gridView_Remark.DataSetSource = remarkBll.GetList(" OrderId=" + OrderId);
        }

        public void BindExpress()
        {
            gridView_Express.DataSource = YSWL.MALL.Web.Components.ExpressHelper.GetExpress(OrderId);

            gridView_Express.DataBind();
        }

        public override void VerifyRenderingInServerForm(Control control)
        {
        }

        protected void gridView_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            hfTabIndex.Value = "1";
            gridView.PageIndex = e.NewPageIndex;
            gridView.OnBind();
        }

        protected void gridView_Action_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            hfTabIndex.Value = "3";
            gridView_Action.PageIndex = e.NewPageIndex;
            gridView_Action.OnBind();
        }
        protected void gridView_Remark_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            hfTabIndex.Value = "4";
            gridView_Remark.PageIndex = e.NewPageIndex;
            gridView_Remark.OnBind();
        }

        protected void gridView_Express_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            hfTabIndex.Value = "5";
            gridView_Express.PageIndex = e.NewPageIndex;
            gridView_Express.OnBind();
        }
        

        protected void btnSave_OnClick(object sender, EventArgs e)
        {
            YSWL.MALL.Model.Shop.Order.OrderInfo orderModel = orderBll.GetModel(OrderId);
            if (orderModel == null)
                return;

            int regionId = RegionList.Region_iID;
            orderModel.RegionId = regionId;
            orderModel.ShipRegion = regionBll.GetRegionNameByRID(regionId);
            orderModel.ShipName = txtShipName.Text;
            orderModel.ShipAddress = txtShipAddress.Text;
            orderModel.ShipTelPhone = txtShipTelPhone.Text;
            orderModel.ShipCellPhone = txtShipCellPhone.Text;
            orderModel.ShipZipCode = txtShipZipCode.Text;


            if (orderBll.Update(orderModel))
            {
                //加操作日志
                YSWL.MALL.Model.Shop.Order.OrderAction actionModel = new Model.Shop.Order.OrderAction();
                int actionCode = (int)YSWL.MALL.Model.Shop.Order.EnumHelper.ActionCode.SysUpdateShip;
                actionModel.ActionCode = actionCode.ToString();
                actionModel.ActionDate = DateTime.Now;
                actionModel.OrderCode = orderModel.OrderCode;
                actionModel.OrderId = orderModel.OrderId;
                actionModel.Remark = "修改收货信息";
                actionModel.UserId = CurrentUser.UserID;
                actionModel.Username = CurrentUser.NickName;
                actionBll.Add(actionModel);
                //清除缓存
                orderBll.RemoveModelInfoCache(orderModel.OrderId);
                MessageBox.ShowSuccessTip(this, "操作成功！");
            }
            else
            {
                MessageBox.ShowFailTip(this, "操作失败！");
            }
        }

        protected void gridView_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            e.Row.Attributes.Add("style", "background:#FFF");
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                if (e.Row.RowIndex % 2 == 0)
                {
                    e.Row.Style.Add(HtmlTextWriterStyle.BackgroundColor, "#F4F4F4");
                }
                else
                {
                    e.Row.Style.Add(HtmlTextWriterStyle.BackgroundColor, "#FFFFFF");
                }
            }
        }



        //修改订单备注
        protected void btnSave_Click(object sender, EventArgs e)
        {
            if (OrderStatus == -1)
            {
                YSWL.MALL.Model.Shop.Order.OrdersHistory Historymodel = historyBll.GetModelByCache(OrderId);
                if (Historymodel != null)
                {
                    Historymodel.Remark = txtRemark.Text;
                    if (historyBll.Update(Historymodel))
                    {
                        MessageBox.ShowSuccessTip(this, "操作成功！");
                    }
                    else
                    {
                        MessageBox.ShowFailTip(this, "操作失败！");
                    }
                }
            }
            else
            {
                YSWL.MALL.Model.Shop.Order.OrderInfo model = orderBll.GetModelByCache(OrderId);
                if (model != null)
                {
                    model.Remark = txtRemark.Text;
                    if (orderBll.Update(model))
                    {
                        MessageBox.ShowSuccessTip(this, "操作成功！");
                    }
                    else
                    {
                        MessageBox.ShowFailTip(this, "操作失败！");
                    }
                }
            }
        }

        /// <summary>
        /// 获得订单追踪状态码信息
        /// </summary>
        /// <param name="actionCode"></param>
        /// <returns></returns>
        protected string GetActionCode(object actionCode)
        {
            if (actionCode == null)
            {
                return "";
            }
            return OrderAction.GetActionCode(actionCode.ToString());
        }

        protected string GetSKUStr(object target)
        {
            string sku = target.ToString();
            List<YSWL.MALL.Model.Shop.Products.SKUItem> itemList = skuBll.GetSKUItemsBySku(sku);
            if (itemList == null || itemList.Count == 0)
            {
                return "";
            }
            string str = "";
            foreach (var item in itemList)
            {
                str += item.AttributeName + "：" + item.AV_ValueStr + "  ";
            }
            return str;
        }
        protected void lnkSaveAmount_OnClick(object sender, EventArgs e)
        {
            if (CurrentUser.UserType != "AA") return;
            if (!UserPrincipal.HasPermissionID(GetPermidByActID(Act_UpdateOrderAmount)) &&
                GetPermidByActID(Act_UpdateOrderAmount) != -1)
            {
                MessageBox.ShowFailTip(this, "您已无权修改应付金额, 操作失败！");
                return;
            }

            YSWL.MALL.Model.Shop.Order.OrderInfo model = orderBll.GetModel(OrderId);
            if (model == null) return;

            if (model.OrderStatus <= -1 || model.OrderStatus >= 2 || model.PaymentStatus >= 2)
            {
                MessageBox.ShowFailTip(this, "订单已被支付/完成, 操作失败！");
                return;
            }

            decimal newAmount = Globals.SafeDecimal(txtAmount.Text, -1);
            if (newAmount < 0) return;

            decimal oldAmount = model.Amount;
            model.Amount = newAmount;

            if (oldAmount == newAmount) return;

            if (orderBll.Update(model))
            {
                //加操作日志
                YSWL.MALL.Model.Shop.Order.OrderAction actionModel = new Model.Shop.Order.OrderAction();
                int actionCode = (int)YSWL.MALL.Model.Shop.Order.EnumHelper.ActionCode.SysUpdateAmount;
                actionModel.ActionCode = actionCode.ToString();
                actionModel.ActionDate = DateTime.Now;
                actionModel.OrderCode = model.OrderCode;
                actionModel.OrderId = model.OrderId;
                actionModel.Remark = string.Format("应付金额由 {0} 变更为 {1}", oldAmount.ToString("F"),
                    newAmount.ToString("F"));
                actionModel.UserId = CurrentUser.UserID;
                actionModel.Username = CurrentUser.NickName;
                actionBll.Add(actionModel);
                //清除缓存
                orderBll.RemoveModelInfoCache(model.OrderId);
                MessageBox.ShowSuccessTipScript(this, "操作成功！", "window.parent.location.reload();");
            }
            else
            {
                MessageBox.ShowFailTip(this, "操作失败！");
            }
        }

    }
}