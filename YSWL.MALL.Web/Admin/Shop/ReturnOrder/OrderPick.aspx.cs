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
    public partial class OrderPick : PageBaseAdmin
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
        public long  OrderId
        {
            get
            {
                return YSWL.Common.Globals.SafeLong(Request.Params["returnId"], 0);
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

            YSWL.MALL.Model.Shop.ReturnOrder.ReturnOrders model = returnorderBll.GetModel(OrderId);
            if (model != null)
            {

                this.lblTitle.Text = "正在进行退货单【" + model.ReturnOrderCode + "】确认取货操作";
                lblOrderCode.Text = model.OrderCode;
                lblCreatedDate.Text = model.CreatedDate.ToString("yyyy-MM-dd HH:mm:ss");

                lblPickName.Text = model.ContactName;
                lblPickAddress.Text = model.PickAddress;
                lblPickRegion.Text = model.PickRegion;
                lblPickCellPhone.Text = model.ContactPhone;
                lblReturnType.Text = returnorderBll.GetReturnTypeStr(model.ReturnType);
                lblAmountAdjusted.Text = model.AmountAdjusted.ToString("F");
            }
        }

        public void BindData()
        {
            gridView.DataSetSource = itemBll.GetListByCache(" ReturnOrderId=" + OrderId);
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
            YSWL.MALL.Model.Shop.ReturnOrder.ReturnOrders model = returnorderBll.GetModel(OrderId);
            if (model == null)
                return;
            if (model.Status != (int)Model.Shop.ReturnOrder.EnumHelper.Status.Handling || model.LogisticStatus <= (int)Model.Shop.ReturnOrder.EnumHelper.LogisticStatus.UnPick || model.LogisticStatus >= (int)Model.Shop.ReturnOrder.EnumHelper.LogisticStatus.Storaged)
            {
                MessageBox.ShowFailTipScript(this, "当前订单的状态已改变,您已不能修改,稍后为您转到列表页...", "$('[id$=btnSearch]', window.parent.document).click(); ");
                return;
            }
            if (returnorderBll.PackedOrder(model, CurrentUser))
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