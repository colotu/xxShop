using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using YSWL.Common;
using YSWL.MALL.Model.Shop.Order;


namespace YSWL.MALL.Web.Admin.Shop.ReturnOrder
{
    public partial class List : PageBaseAdmin
    {
        public YSWL.MALL.BLL.Shop.ReturnOrder.ReturnOrders returnorderBll = new BLL.Shop.ReturnOrder.ReturnOrders();
        public YSWL.MALL.BLL.Shop.ReturnOrder.ReturnOrderAction actionBll = new BLL.Shop.ReturnOrder.ReturnOrderAction();
        public int Type = 0;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                Type = YSWL.Common.Globals.SafeInt(Request.Params["type"], 0);
             
            }
        }
 
        protected void btnSearch_Click(object sender, EventArgs e)
        {
            gridView.OnBind();
        }

        #region gridView


        public void BindData()
        {
            StringBuilder strWhere = new StringBuilder();
  
            if (!string.IsNullOrWhiteSpace(this.txtReturnOrderCode.Text.Trim()))
            {
                if (strWhere.Length > 1)
                {
                    strWhere.Append(" and ");
                }
                strWhere.AppendFormat(" ReturnOrderCode like '%{0}%'", InjectionFilter.QuoteFilter(txtReturnOrderCode.Text.Trim()));
            }
            if (!string.IsNullOrWhiteSpace(this.txtOrderCode.Text.Trim()))
            {
                if (strWhere.Length > 1)
                {
                    strWhere.Append(" and ");
                }
                strWhere.AppendFormat(" OrderCode like '%{0}%'", InjectionFilter.QuoteFilter(txtOrderCode.Text.Trim()));
            }
            
            if (!string.IsNullOrWhiteSpace(this.txtContactName.Text.Trim()))
            {
                if (strWhere.Length > 1)
                {
                    strWhere.Append(" and ");
                }
                strWhere.AppendFormat(" ContactName like '%{0}%'", InjectionFilter.QuoteFilter(txtContactName.Text.Trim()));
            }
            if (!string.IsNullOrWhiteSpace(this.txtReturnUserName.Text.Trim()))
            {
                if (strWhere.Length > 1)
                {
                    strWhere.Append(" and ");
                }
                strWhere.AppendFormat(" ReturnUserName like '%{0}%'", InjectionFilter.QuoteFilter(txtReturnUserName.Text.Trim()));
            }
            //if (dropPaymentStatus.SelectedValue != "-1")
            //{
            //    if (strWhere.Length > 1)
            //    {
            //        strWhere.Append(" and ");
            //    }
            //    strWhere.AppendFormat(" PaymentStatus = {0}", dropPaymentStatus.SelectedValue);
            //}
            //if (dropShippingStatus.SelectedValue != "-1")
            //{
            //    if (strWhere.Length > 1)
            //    {
            //        strWhere.Append(" and ");
            //    }
            //    strWhere.AppendFormat(" ShippingStatus = {0}", dropShippingStatus.SelectedValue);
            //}
            if (!String.IsNullOrWhiteSpace(this.txtCreatedDateStart.Text) && Common.PageValidate.IsDateTime(this.txtCreatedDateStart.Text))
            {
                if (strWhere.Length > 1)
                {
                    strWhere.Append(" and  ");
                }
                strWhere.AppendFormat(" CreatedDate >='" + Common.InjectionFilter.SqlFilter(this.txtCreatedDateStart.Text) + "' ");
            }
            //时间段
            if (!String.IsNullOrWhiteSpace(this.txtCreatedDateEnd.Text) && Common.PageValidate.IsDateTime(this.txtCreatedDateEnd.Text))
            {
                string endTime = Common.Globals.SafeDateTime(this.txtCreatedDateEnd.Text, DateTime.Now).AddDays(1).ToString("yyyy-MM-dd");
                if (strWhere.Length > 1)
                {
                    strWhere.Append(" and  ");
                }
                strWhere.AppendFormat(" CreatedDate <='" + Common.InjectionFilter.SqlFilter(endTime) + "' ");
            }
 
            gridView.DataSetSource = returnorderBll.GetList(-1, strWhere.ToString(), "  ReturnOrderId desc");
 
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

                if (!UserPrincipal.HasPermissionID(GetPermidByActID(Act_UpdateData)) && GetPermidByActID(Act_UpdateData) != -1)
                {
                    LinkButton linkComplete = (LinkButton)e.Row.FindControl("linkComplete");
                    linkComplete.Visible = false;
                    LinkButton linkCancel = (LinkButton)e.Row.FindControl("linkCancel");
                    linkCancel.Visible = false;
                    LinkButton linkReturn = (LinkButton)e.Row.FindControl("linkReturn");
                    linkReturn.Visible = false;
                }
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

        protected void gridView_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            //if (orderBll.Delete((int)gridView.DataKeys[e.RowIndex].Value))
            //{
            //    gridView.OnBind();
            //    MessageBox.ShowSuccessTip(this, Resources.Site.TooltipDelOK);
            //}
        }


        protected void gridView_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            //取消订单
            //if (e.CommandName == "CancelOrder")
            //{
            //    object[] arg = e.CommandArgument.ToString().Split(',');  //注意是单引号
            //    long orderId = Common.Globals.SafeLong(arg[0].ToString(), 0);
            //    YSWL.MALL.Model.Shop.Order.OrderInfo orderInfo = orderBll.GetModelInfo(orderId);
            //    if (YSWL.MALL.BLL.Shop.Order.OrderManage.CancelOrder(orderInfo, CurrentUser))
            //    {
            //        MessageBox.ShowSuccessTip(this, "操作成功！");
            //    }
            //    else
            //    {
            //        MessageBox.ShowSuccessTip(this, "操作失败，请稍候再试！");
            //    }
            //}
            //if (e.CommandName == "Success")
            //{
            //    object[] arg = e.CommandArgument.ToString().Split(',');  //注意是单引号
            //    long orderId = Common.Globals.SafeLong(arg[0].ToString(), 0);
            //    string orderCode = arg[1].ToString();
            //    OrderInfo orderInfo = orderBll.GetModelInfo(orderId);
            //    if (BLL.Shop.Order.OrderManage.CompleteOrder(orderInfo, CurrentUser))
            //    {
            //        MessageBox.ShowSuccessTip(this, "操作成功！");
            //    }
            //    else
            //    {
            //        MessageBox.ShowSuccessTip(this, "操作失败，请稍候再试！");
            //    }

            //}
            //if (e.CommandName == "Pay")
            //{
            //    object[] arg = e.CommandArgument.ToString().Split(',');  //注意是单引号
            //    long orderId = Common.Globals.SafeLong(arg[0].ToString(), 0);
            //    string orderCode = arg[1].ToString();

            //    OrderInfo orderInfo = orderBll.GetModel(orderId);
            //    if (orderInfo != null && BLL.Shop.Order.OrderManage.PayForOrder(orderInfo, CurrentUser))
            //    {
            //        MessageBox.ShowSuccessTip(this, "操作成功！");
            //    }
            //    else
            //    {
            //        MessageBox.ShowSuccessTip(this, "操作失败，请稍候再试！");
            //    }
            //}
            //gridView.OnBind();

        }

        private string GetSelIDlist()
        {
            string idlist = "";
            bool BxsChkd = false;
            for (int i = 0; i < gridView.Rows.Count; i++)
            {
                CheckBox ChkBxItem = (CheckBox)gridView.Rows[i].FindControl(gridView.CheckBoxID);
                if (ChkBxItem != null && ChkBxItem.Checked)
                {
                    BxsChkd = true;
                    if (gridView.DataKeys[i].Value != null)
                    {
                        idlist += gridView.DataKeys[i].Value.ToString() + ",";
                    }
                }
            }
            if (BxsChkd)
            {
                idlist = idlist.Substring(0, idlist.LastIndexOf(","));
            }
            return idlist;
        }

        #endregion


        protected int SupplierId
        {
            get
            {
                int supplierId = 0;
                if (!string.IsNullOrWhiteSpace(Request.Params["sid"]))
                {
                    supplierId = Common.Globals.SafeInt(Request.Params["sid"], 0);
                }
                return supplierId;
            }
        }
 
        #region 获取状态
      /// <summary>
        ///  获取状态
      /// </summary>
      /// <param name="objectStatus"></param>
      /// <param name="objectLogistic"></param>
      /// <param name="objecRefund"></param>
      /// <returns></returns>
        protected string GetMainStatus(object objectStatus,object objectLogistic,object objecRefund)
        {
            return YSWL.MALL.BLL.Shop.ReturnOrder.ReturnOrders.GetMainStatusStr(Globals.SafeInt(objectStatus, 0), Globals.SafeInt(objectLogistic, 0), Globals.SafeInt(objecRefund, 0));
        }

       


        protected string GetOrderType(object paymentGateway_obj, object orderStatus_obj, object paymentStatus_obj, object shippingStatus_obj)
        {
            // 1 等待买家付款   | 2 等待发货 | 3 已发货 | 4 退款中 | 5 成功订单 | 6 已退款 |7 已退货  |8 已关闭  
            string str = string.Empty;
            if (!StringPlus.IsNullOrEmpty(paymentGateway_obj) && !StringPlus.IsNullOrEmpty(orderStatus_obj) && !StringPlus.IsNullOrEmpty(paymentStatus_obj) && !StringPlus.IsNullOrEmpty(shippingStatus_obj))
            {
                //EnumHelper.OrderMainStatus orderType = orderBll.GetOrderType(paymentGateway_obj.ToString(),
                //                        Common.Globals.SafeInt(orderStatus_obj.ToString(), 0),
                //                        Common.Globals.SafeInt(paymentStatus_obj.ToString(), 0),
                //                        Common.Globals.SafeInt(shippingStatus_obj.ToString(), 0));
                //switch (orderType)
                //{
                //    //  订单组合状态 1 等待付款   | 2 等待处理 | 3 取消订单 | 4 订单锁定 | 5 等待付款确认 | 6 正在处理 |7 配货中  |8 已发货 |9  已完成
                //    case EnumHelper.OrderMainStatus.Paying:
                //        str = "等待付款";
                //        break;
                //    case EnumHelper.OrderMainStatus.PreHandle:
                //        str = "等待处理";
                //        break;
                //    case EnumHelper.OrderMainStatus.Cancel:
                //        str = "取消订单";
                //        break;
                //    case EnumHelper.OrderMainStatus.Locking:
                //        str = "订单锁定";
                //        break;
                //    case EnumHelper.OrderMainStatus.PreConfirm:
                //        str = "等待付款确认";
                //        break;
                //    case EnumHelper.OrderMainStatus.Handling:
                //        str = "正在处理";
                //        break;
                //    case EnumHelper.OrderMainStatus.Shipping:
                //        str = "配货中";
                //        break;
                //    case EnumHelper.OrderMainStatus.Shiped:
                //        str = "已发货";
                //        break;
                //    case EnumHelper.OrderMainStatus.Complete:
                //        str = "已完成";
                //        break;
                //    default:
                //        str = "未知状态";
                //        break;
                //}
            }
            return str;
        }

        #endregion
 
        //public string GetSupplierName(object  o,object name)
        //{
        //    if (o== null) {
        //        return "平台";
        //    }
        //    int supplierId = Globals.SafeInt(o, 0);
        //    if (supplierId > 0) { 

        //    }
        //    return "平台";
        //}
    }
}