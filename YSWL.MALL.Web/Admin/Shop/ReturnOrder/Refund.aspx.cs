using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using YSWL.MALL.BLL.Shop.ReturnOrder;
using YSWL.Common;

namespace YSWL.MALL.Web.Admin.Shop.ReturnOrder
{
    public partial class Refund : PageBaseAdmin
    {
        private YSWL.MALL.BLL.Shop.ReturnOrder.ReturnOrders returnorderBll = new ReturnOrders();
        private YSWL.MALL.BLL.Shop.ReturnOrder.ReturnOrderItems itemBll = new ReturnOrderItems();
        private YSWL.MALL.BLL.Shop.ReturnOrder.ReturnOrderAction actionBll = new ReturnOrderAction();
        private YSWL.MALL.BLL.Shop.Products.SKUInfo skuBll = new BLL.Shop.Products.SKUInfo();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                ShowInfo();

            }
        }


        #region Id
        /// <summary>
        /// Id
        /// </summary>
        public long ReturnOrderId
        {
            get
            {
                return YSWL.Common.Globals.SafeLong(Request.Params["returnId"], 0);
            }
        }
        #endregion
 
        private void ShowInfo()
        {
            YSWL.MALL.Model.Shop.ReturnOrder.ReturnOrders model = returnorderBll.GetModel(ReturnOrderId);
            if (model != null)
            {

                this.lblTitle.Text = "正在进行退货单【" + model.ReturnOrderCode + "】确认退款操作";
                lblOrderCode.Text = model.OrderCode;
                lblCreatedDate.Text = model.CreatedDate.ToString("yyyy-MM-dd HH:mm:ss");

                lblPickName.Text = model.ContactName;
                lblPickAddress.Text = model.PickAddress;
                lblPickRegion.Text = model.PickRegion;
                lblPickCellPhone.Text = model.ContactPhone;
                lblReturnType.Text = returnorderBll.GetReturnTypeStr(model.ReturnType);
                lblAmountAdjusted.Text = model.AmountAdjusted.ToString("F");
                lblAmount.Text = model.Amount.ToString("F");
                this.lblActualSalesTotal.Text= model.ActualSalesTotal.ToString("F");
                txtAmountActual.Text = model.AmountAdjusted.ToString("F");
                hfSupplierId.Value = model.SupplierId.ToString();
                lblReturnGoodsType.Text = returnorderBll.GetReturnGoodsTypeStr(model.ReturnGoodsType);
             
                //优惠劵信息
                if (!String.IsNullOrWhiteSpace(model.CouponCode))
                {
                    //if (model.ReturnGoodsType == (int)Model.Shop.ReturnOrder.EnumHelper.ReturngoodsType.All)
                    //{
                    //    trReturnCoupon.Visible = true;
                    //}

                    trCoupon.Visible = true;
                    lblCouponCode.Text = model.CouponCode;
                    lblCouponName.Text = model.CouponName;
                    labelCouponAmount.Text = model.CouponAmount.HasValue ? model.CouponAmount.Value.ToString("F") : "0.00";
                }
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
            gridView.PageIndex = e.NewPageIndex;
            gridView.OnBind();
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



        protected void btnSave_OnClick(object sender, EventArgs e)
        {
            this.btnSave.Enabled = false;
            Model.Shop.ReturnOrder.ReturnOrders model = returnorderBll.GetModel(ReturnOrderId);
            if (model == null) {
                return;
            }
            decimal amountActual =Globals.SafeDecimal(txtAmountActual.Text,-1);
            if (amountActual < 0) {
                this.btnSave.Enabled = true;
                MessageBox.ShowFailTip(this, "实退金额不能小于0！");
                return;
            }
            if (model.Status != (int)Model.Shop.ReturnOrder.EnumHelper.Status.Handling || model.RefundStatus != (int)Model.Shop.ReturnOrder.EnumHelper.RefundStatus.Apply)
            {
                MessageBox.ShowFailTip(this, "当前退货单的状态可能已发生改变,您已不能修改.");
                this.btnSave.Enabled = true;
                return;
            }
            model.AmountActual = amountActual;

            //商家的用户Id
            int suppUserId =0;
            //扣除商家金额
            decimal deductionSuppAmount = 0;
            if (model.SupplierId > 0)
            {
                //获取应扣除商家的金额
                deductionSuppAmount = returnorderBll.GetDeductionSuppAmount(model);
                BLL.Members.UsersExp userBll = new BLL.Members.UsersExp();
                BLL.Members.Users uBll = new BLL.Members.Users();
                suppUserId = uBll.GetUserIdByDepartmentID(model.SupplierId.ToString());
                if ((userBll.GetUserBalance(suppUserId) - deductionSuppAmount) < 0)
                {
                    MessageBox.ShowFailTip(this, "商家账户余额不足，请联系商家充值后再进行退款.");
                    this.btnSave.Enabled = true;
                    return;
                }
            }
            if (returnorderBll.Refunds(model, CurrentUser, chkReturnCoupon.Checked, suppUserId, deductionSuppAmount))
            {
                MessageBox.ShowSuccessTipScript(this, "操作成功！", "window.parent.location.reload();");
                this.btnSave.Enabled = false;
            }
            else
            {
                MessageBox.ShowFailTip(this, "操作失败！");
            }
        }



    }
}