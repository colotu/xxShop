using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using YSWL.MALL.BLL.Shop.Order;
using YSWL.Common;

namespace YSWL.MALL.Web.Admin.Shop.Order
{
    public partial class OrderItemInfo : PageBaseAdmin
    {
        protected override int Act_PageLoad { get { return 446; } } //Shop_订单管理_发货页
        private YSWL.MALL.BLL.Shop.Order.Orders orderBll = new Orders();
        private YSWL.MALL.BLL.Shop.Order.OrderItems itemBll = new OrderItems();
        private YSWL.MALL.BLL.Shop.Order.OrderAction actionBll = new YSWL.MALL.BLL.Shop.Order.OrderAction();
        private YSWL.MALL.BLL.Shop.Products.SKUInfo skuBll = new BLL.Shop.Products.SKUInfo();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                ShowInfo();
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

            YSWL.MALL.Model.Shop.Order.OrderInfo model = orderBll.GetModel(OrderId);
            if (model != null)
            {

                this.lblTitle.Text = "正在进行订单【" + model.OrderCode + "】配货操作";
                lblOrderCode.Text = model.OrderCode;
                lblCreatedDate.Text = model.CreatedDate.ToString("yyyy-MM-dd HH:mm:ss");


                lblBuyerEmail.Text = model.BuyerEmail;
                lblBuyerCellPhone.Text = model.BuyerCellPhone;
                lblBuyerName.Text = model.BuyerName;

                lblShipTypeName.Text = model.ShippingModeName;

            }
        }

        public void BindData()
        {
            gridView.DataSetSource = itemBll.GetListByCache(" OrderId=" + OrderId);
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
            YSWL.MALL.Model.Shop.Order.OrderInfo orderModel = orderBll.GetModelInfo(OrderId);
            if (orderModel == null) {
                MessageBox.ShowFailTip(this, "操作失败！");
                return;
            }
            if (orderModel.ShippingStatus != 0 || orderModel.OrderStatus != 0)
            {
                MessageBox.ShowFailTipScript(this, "当前订单的状态已改变,您已不能修改,稍后为您转到列表页...", "$('[id$=btnSearch]', window.parent.document).click(); ");
                return;
            }
            if (BLL.Shop.Order.OrderManage.PackingOrder(orderModel, CurrentUser))
            {
                MessageBox.ShowSuccessTipScript(this, "操作成功！", "window.parent.location.reload();");
                this.btnSave.Enabled = false;
            }else
            {
                MessageBox.ShowFailTip(this, "操作失败！");
            }
        }


    }
}