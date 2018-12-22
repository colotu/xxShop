using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using YSWL.Common;

namespace YSWL.MALL.Web.Admin.Shop.ReturnOrder
{

    public partial class Show : PageBaseAdmin
    {
        public YSWL.MALL.BLL.Shop.ReturnOrder.ReturnOrders returnorderBll = new BLL.Shop.ReturnOrder.ReturnOrders();
        public YSWL.MALL.BLL.Shop.ReturnOrder.ReturnOrderAction actionBll = new BLL.Shop.ReturnOrder.ReturnOrderAction();
        private YSWL.MALL.BLL.Shop.ReturnOrder.ReturnOrderItems itemBll = new BLL.Shop.ReturnOrder.ReturnOrderItems();
        private YSWL.MALL.BLL.Ms.Regions regionBll = new BLL.Ms.Regions();
        private YSWL.MALL.BLL.Shop.Products.SKUInfo skuBll = new BLL.Shop.Products.SKUInfo();
        public int TabIndex = 0;
        protected void Page_Load(object sender, EventArgs e)
        {
            TabIndex = Common.Globals.SafeInt(hfTabIndex.Value, 0);
            if (!Page.IsPostBack)
            {
                ShowInfo();
            }
        }


        #region Id
        /// <summary>
        /// Id
        /// </summary>
        public long  ReturnOrderId
        {
            get
            {
                return YSWL.Common.Globals.SafeLong(Request.Params["returnorderId"], 0);
            }
        }
        #endregion

        public string ImageUrls
        {
            set;
            get;
        }

        private void ShowInfo()
        {
            YSWL.MALL.Model.Shop.ReturnOrder.ReturnOrders model = returnorderBll.GetModelByCache(ReturnOrderId);
                if (model != null)
                {
                    //取货信息
                    this.lblTitle.Text = "正在查看【" + model.ReturnOrderCode + "】的详细信息";
                    this.txtContactName.Text = model.ContactName;
                    this.txtContactPhone.Text = model.ContactPhone;
                    RegionList.Region_iID = model.PickRegionId;
                    this.txtPickAddress.Text = model.PickAddress;
 
                    ////价格信息
                    this.lblActualSalesTotal.Text= model.ActualSalesTotal.ToString("F");
                    this.lblAmount.Text = model.Amount.ToString("F");
                    this.lblAmountAdjusted.Text = model.AmountAdjusted.ToString("F");
                    this.lblAmountActual.Text = model.AmountActual.ToString("F");
                    txtRemark.Text = model.Remark;
                    lblDescription.Text = model.Description;

                    lblOrderCode.Text = model.OrderCode;
                    lblCreatedDate.Text = model.CreatedDate.ToString("yyyy-MM-dd HH:mm:ss");
                    lblReturnType.Text = returnorderBll.GetReturnTypeStr(model.ReturnType);
                    lblReturnGoodsType.Text = returnorderBll.GetReturnGoodsTypeStr(model.ReturnGoodsType);

                    if (!String.IsNullOrWhiteSpace(model.RefuseReason)) {
                        refuseReason.Visible = true;
                        lblRefuseReason.Text = model.RefuseReason;
                    }
                    //优惠劵信息
                    if (!String.IsNullOrWhiteSpace(model.CouponCode))
                    {
                        trCoupon.Visible = true;
                        lblCouponCode.Text = model.CouponCode;
                        lblCouponName.Text = model.CouponName;
                        labelCouponAmount.Text = model.CouponAmount.HasValue ? model.CouponAmount.Value.ToString("F") : "0.00";
                        BLL.Shop.Coupon.CouponInfo coupBll = new BLL.Shop.Coupon.CouponInfo();
                        YSWL.MALL.Model.Shop.Coupon.CouponInfo coupmodel=coupBll.GetModelByCache(model.CouponCode);
                        if (coupmodel != null) {
                            string strTitle = string.Empty;
                            string strContent = string.Empty;
                            CouponDesignation.Visible = true;
                            if (!String.IsNullOrWhiteSpace(coupmodel.ProductSku))
                            {
                                //指定sku
                                strTitle = "指定sku";
                                strContent = coupmodel.ProductSku;
                            }
                            else if (coupmodel.ProductId > 0)
                            {
                                //指定商品
                                BLL.Shop.Products.ProductInfo prodBll = new BLL.Shop.Products.ProductInfo();
                                strTitle = "指定商品";
                                strContent = prodBll.GetNameByCache(coupmodel.ProductId);
                            }
                            else if (coupmodel.CategoryId > 0)
                            {
                                //指定分类
                                strTitle = "指定分类";
                                BLL.Shop.Products.CategoryInfo cateBll = new BLL.Shop.Products.CategoryInfo();
                                strContent = cateBll.GetName(coupmodel.CategoryId);
                            }
                            else {
                                CouponDesignation.Visible = false;
                            }
                            ltlCouponMes.Text = strTitle;
                            lblCouponMes.Text = strContent;
                        }

                    }

                    #region 图片
                    ImageUrls = string.Empty;
                    if (!String.IsNullOrWhiteSpace(model.ImageUrl))
                    {
                        string[] imageurl = model.ImageUrl.Split(new char[] { '|' }, StringSplitOptions.RemoveEmptyEntries);
                        foreach (string item in imageurl)
                        {
                            ImageUrls += "<a target=_blank href=" + item + "><img src='" + item + "'  runat='server' width='50' height='50' /></a>";
                        }
                    }
                    #endregion

                }
 
        }

        public void BindData()
        {
            gridView.DataSetSource = itemBll.GetListByCache(" ReturnOrderId=" + ReturnOrderId);
        }

        public void BindAction()
        {
            gridView_Action.DataSetSource = actionBll.GetList(" ReturnOrderId=" + ReturnOrderId);
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

        #region 修改取货信息
        protected void btnPickSave_OnClick(object sender, EventArgs e)
        {
            YSWL.MALL.Model.Shop.ReturnOrder.ReturnOrders model = returnorderBll.GetModel(ReturnOrderId);
            if (model == null)
                return;

            int regionId = RegionList.Region_iID;
            model.PickRegionId= regionId;
            model.PickRegion = regionBll.GetRegionNameByRID(regionId);
            model.ContactName = txtContactName.Text;
            model.ContactPhone = txtContactPhone.Text;
            model.PickAddress = txtPickAddress.Text;
            model.UpdatedDate = DateTime.Now;
            model.UpdatedUserId = CurrentUser.UserID;
            if (returnorderBll.Update(model))
            {
                //加操作日志
                YSWL.MALL.Model.Shop.ReturnOrder.ReturnOrderAction actionModel = new Model.Shop.ReturnOrder.ReturnOrderAction();
                int actionCode = (int)YSWL.MALL.Model.Shop.ReturnOrder.EnumHelper.ActionCode.SystemUpdatePick;
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
                MessageBox.ShowSuccessTip(this, "操作成功！");
            }
            else
            {
                MessageBox.ShowFailTip(this, "操作失败！");
            }
        }
        #endregion
 
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

        //修改备注
        protected void btnSave_Click(object sender, EventArgs e)
        {
                YSWL.MALL.Model.Shop.ReturnOrder.ReturnOrders model = returnorderBll.GetModelByCache(ReturnOrderId);
                if (model != null)
                {
                    model.Remark = txtRemark.Text;
                    if (returnorderBll.Update(model))
                    {
                        //清除缓存
                        returnorderBll.RemoveModelCache(model.ReturnOrderId);
                        MessageBox.ShowSuccessTipScript(this, "操作成功！", "window.parent.location.reload();");
                    }
                    else
                    {
                        MessageBox.ShowFailTip(this, "操作失败！");
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
            return YSWL.MALL.BLL.Shop.ReturnOrder.ReturnOrderAction.GetActionCode(actionCode.ToString());
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
         
    }
}