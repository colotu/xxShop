using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using YSWL.Common;

namespace YSWL.MALL.Web.Admin.Shop.ReturnOrder
{

    public partial class Audit : PageBaseAdmin
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
                imgModifyAmountAdjusted.Visible = true;
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
                    this.lblTitle.Text = "正在审核退货单【" + model.ReturnOrderCode + "】的详细信息";
                  
                    lblOrderCode.Text = model.OrderCode;
                    lblCreatedDate.Text = model.CreatedDate.ToString("yyyy-MM-dd HH:mm:ss");
                    lblPickAddress.Text= model.PickAddress;
                    lblPickCellPhone.Text = model.ContactPhone;
                    lblPickName.Text = model.ContactName;
                    lblPickRegion.Text = model.PickRegion;
                   

                    //价格信息
                    this.lblActualSalesTotal.Text = model.ActualSalesTotal.ToString("F");
                    this.lblAmount.Text = model.Amount.ToString("F");
                    this.txtAmountAdjusted.Text = model.AmountAdjusted.ToString("F");
                    this.lblAmountAdjusted.Text = model.AmountAdjusted.ToString("F");
                    lblReturnType.Text = returnorderBll.GetReturnTypeStr(model.ReturnType);
                    lblReturnGoodsType.Text = returnorderBll.GetReturnGoodsTypeStr(model.ReturnGoodsType);
                    
                    txtRemark.Text = model.Remark;
                    lblDescription.Text = model.Description;
                    //优惠劵信息
                    if (!String.IsNullOrWhiteSpace(model.CouponCode))
                    {
                        trCoupon.Visible = true;
                        lblCouponCode.Text = model.CouponCode;
                        lblCouponName.Text = model.CouponName;
                        labelCouponAmount.Text = model.CouponAmount.HasValue ? model.CouponAmount.Value.ToString("F") : "0.00";
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
 
        public override void VerifyRenderingInServerForm(Control control)
        {
        }

        protected void gridView_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            hfTabIndex.Value = "1";
            gridView.PageIndex = e.NewPageIndex;
            gridView.OnBind();
        }

        #region 拒绝
        protected void btnRefusal_OnClick(object sender, EventArgs e)
        {
            YSWL.MALL.Model.Shop.ReturnOrder.ReturnOrders model = returnorderBll.GetModel(ReturnOrderId);
            if (model == null)
                return;
            if (model.Status != (int)Model.Shop.ReturnOrder.EnumHelper.Status.UnHandle)
            {
                MessageBox.ShowFailTip(this, "退货单已被审核, 操作失败！");
                return;
            }
             string refuseReason=Common.InjectionFilter.SqlFilter( txtRefuseReason.Text);
             string remark=Common.InjectionFilter.SqlFilter(txtRemark.Text);
            if(String.IsNullOrWhiteSpace(refuseReason)){
                MessageBox.ShowFailTip(this, "请填写拒绝原因");
                return;
            }
             model.RefuseReason = refuseReason;
             if (returnorderBll.AuditFail(ReturnOrderId,refuseReason, CurrentUser.UserID, remark))
            {
                //加操作日志
                YSWL.MALL.Model.Shop.ReturnOrder.ReturnOrderAction actionModel = new Model.Shop.ReturnOrder.ReturnOrderAction();
                int actionCode = (int)YSWL.MALL.Model.Shop.ReturnOrder.EnumHelper.ActionCode.SystemAudit;
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
                MessageBox.ShowSuccessTipScript(this, "操作成功！", "window.parent.location.reload();");
                this.btnRefusal.Enabled = false;
            }
            else
            {
                MessageBox.ShowFailTip(this, "操作失败！");
            }
        }
        #endregion

        #region 审核通过
        protected void btnPass_OnClick(object sender, EventArgs e)
        {
            decimal amountAdjusted= Common.Globals.SafeDecimal(  lblAmountAdjusted.Text,-1);
            if (amountAdjusted < 0) {
                MessageBox.ShowFailTip(this, "应退金额不能小于0！");
                return;
            }
            YSWL.MALL.Model.Shop.ReturnOrder.ReturnOrders model = returnorderBll.GetModel(ReturnOrderId);
            if (model == null)
                return;
            if (model.Status != (int)Model.Shop.ReturnOrder.EnumHelper.Status.UnHandle)
            {
                MessageBox.ShowFailTip(this, "退货单已被审核, 操作失败！");
                return;
            }
            bool IsReturngoods = chkReturngoods.Checked;
            if (IsReturngoods)//是否需要取货
            {
                model.LogisticStatus = (int)YSWL.MALL.Model.Shop.ReturnOrder.EnumHelper.LogisticStatus.Packing;
            }
            else 
            {
                model.RefundStatus = (int)YSWL.MALL.Model.Shop.ReturnOrder.EnumHelper.RefundStatus.Apply;
            }
            model.Remark = txtRemark.Text;
            model.Status = (int)YSWL.MALL.Model.Shop.ReturnOrder.EnumHelper.Status.Handling;
            model.UpdatedDate = DateTime.Now;
            model.UpdatedUserId = CurrentUser.UserID;
            if (returnorderBll.AuditPass(model, IsReturngoods))
            {
                //加操作日志
                YSWL.MALL.Model.Shop.ReturnOrder.ReturnOrderAction actionModel = new Model.Shop.ReturnOrder.ReturnOrderAction();
                int actionCode = (int)YSWL.MALL.Model.Shop.ReturnOrder.EnumHelper.ActionCode.SystemAudit;
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
                MessageBox.ShowSuccessTipScript(this, "操作成功！", "window.parent.location.reload();");
                this.btnPass.Enabled = false;
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

        #region 修改应退金额
        protected void lnkSaveAmountAdjusted_OnClick(object sender, EventArgs e)
        {
            if (CurrentUser.UserType != "AA") return;

            YSWL.MALL.Model.Shop.ReturnOrder.ReturnOrders model = returnorderBll.GetModel(ReturnOrderId);
            if (model == null) return;

            if (model.Status!=(int)Model.Shop.ReturnOrder.EnumHelper.Status.UnHandle )
            {
                MessageBox.ShowFailTip(this, "退货单已被审核, 操作失败！");
                return;
            }

            decimal newAmountAdjusted = Globals.SafeDecimal(txtAmountAdjusted.Text, -1);
            if (newAmountAdjusted <0) return;

            decimal oldAmountAdjusted = model.AmountAdjusted;
            model.AmountAdjusted = newAmountAdjusted;

            if (oldAmountAdjusted == newAmountAdjusted) return;

            if (returnorderBll.UpdateAmountAdjusted(ReturnOrderId,model.ReturnOrderCode,oldAmountAdjusted,newAmountAdjusted,CurrentUser))
            {
                lblAmountAdjusted.Text = newAmountAdjusted.ToString("F");
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

    }
}