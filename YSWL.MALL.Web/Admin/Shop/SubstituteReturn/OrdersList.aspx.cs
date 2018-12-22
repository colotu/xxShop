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
using Orders = YSWL.MALL.BLL.Shop.Order.Orders;
using OrdersHistory = YSWL.MALL.BLL.Shop.Order.OrdersHistory;
using YSWL.Json;

namespace YSWL.MALL.Web.Admin.Shop.SubstituteReturn
{
    public partial class OrdersList : PageBaseAdmin
    {
 
        public YSWL.MALL.BLL.Shop.Order.Orders orderBll = new Orders();
        public YSWL.MALL.BLL.Shop.Order.OrdersHistory historyBll = new OrdersHistory();
        public YSWL.MALL.BLL.Shop.Order.OrderAction actionBll = new BLL.Shop.Order.OrderAction();
        public int Type = 0;
        private BLL.Shop.ReturnOrder.ReturnOrders retuOrderBll = new BLL.Shop.ReturnOrder.ReturnOrders();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(this.Request.Form["Callback"]) && (this.Request.Form["Callback"] == "true"))
            {
                this.Controls.Clear();
                this.DoCallback();
            }
            if (!Page.IsPostBack)
            {
                Type = YSWL.Common.Globals.SafeInt(Request.Params["type"], 0); 
            
 
            }
        }

        #region 订单状态
        /// <summary>
        /// 订单状态
        /// </summary>
        public int OrderStatus
        {
            get
            {
                return YSWL.Common.Globals.SafeInt(Request.Params["type"], 0); 
               // return Type;
            }
        }
        #endregion

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            gridView.OnBind();
        }

        #region gridView


        public void BindData()
        {
            if (!IsPostBack) {
                return;    
            }

            StringBuilder strWhere = new StringBuilder();

            if (OrderStatus > 0)
            {
                strWhere.Append(orderBll.GetWhereByStatus(OrderStatus));
            }
            if (!string.IsNullOrWhiteSpace(this.txtOrderCode.Text.Trim()))
            {
                if (strWhere.Length > 1)
                {
                    strWhere.Append(" and ");
                }
                strWhere.AppendFormat(" OrderCode like '%{0}%'", InjectionFilter.QuoteFilter(txtOrderCode.Text.Trim()));
            }
            if (!string.IsNullOrWhiteSpace(this.txtShipName.Text.Trim()))
            {
                if (strWhere.Length > 1)
                {
                    strWhere.Append(" and ");
                }
                strWhere.AppendFormat(" ShipName like '%{0}%'", InjectionFilter.QuoteFilter(txtShipName.Text.Trim()));
            }
            if (!string.IsNullOrWhiteSpace(this.txtBuyerName.Text.Trim()))
            {
                if (strWhere.Length > 1)
                {
                    strWhere.Append(" and ");
                }
                strWhere.AppendFormat(" BuyerName like '%{0}%'", InjectionFilter.QuoteFilter(txtBuyerName.Text.Trim()));
            }
          
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

            if (strWhere.Length > 1)
            {
                strWhere.Append(" and  ");
            }
            //主订单 无子订单
            strWhere.AppendFormat(" ((OrderType = 1 AND HasChildren = 0) " +
                //子订单 已支付 或 货到付款/银行转账
            "OR (OrderType = 2 AND (PaymentStatus > 1 OR (PaymentGateway='cod' OR PaymentGateway='bank')) ) " +
                //主订单 有子订单 未支付的主订单 非 货到付款/银行转账 子订单
            "OR (OrderType = 1 AND HasChildren = 1 AND PaymentStatus < 2 AND PaymentGateway<>'cod' AND PaymentGateway<>'bank'))");

            if (OrderStatus == -1)
            {
                gridView.DataSetSource = historyBll.GetList(-1, strWhere.ToString(), "CreatedDate desc");
            }
            else
            {
                gridView.DataSetSource = orderBll.GetList(-1, strWhere.ToString(), "CreatedDate desc");
            }
           
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
            if (orderBll.Delete((int)gridView.DataKeys[e.RowIndex].Value))
            {
                gridView.OnBind();
                MessageBox.ShowSuccessTip(this, Resources.Site.TooltipDelOK);
            }
        }


        protected void gridView_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            //取消订单
            if (e.CommandName == "CancelOrder")
            {
                object[] arg = e.CommandArgument.ToString().Split(',');  //注意是单引号
                long orderId = Common.Globals.SafeLong(arg[0].ToString(), 0);
                YSWL.MALL.Model.Shop.Order.OrderInfo orderInfo = orderBll.GetModelInfo(orderId);
                if (YSWL.MALL.BLL.Shop.Order.OrderManage.CancelOrder(orderInfo, CurrentUser))
                {
                    MessageBox.ShowSuccessTip(this, "操作成功！");
                }else{
                    MessageBox.ShowSuccessTip(this, "操作失败，请稍候再试！");
                }
            }
            if (e.CommandName == "Success")
            {
                object[] arg = e.CommandArgument.ToString().Split(',');  //注意是单引号
                long orderId = Common.Globals.SafeLong(arg[0].ToString(), 0);
                string orderCode = arg[1].ToString();
                OrderInfo orderInfo = orderBll.GetModelInfo(orderId);
                if (BLL.Shop.Order.OrderManage.CompleteOrder(orderInfo, CurrentUser))
                {
                    MessageBox.ShowSuccessTip(this, "操作成功！");
                }
                else
                {
                    MessageBox.ShowSuccessTip(this, "操作失败，请稍候再试！");
                }

            }
            if (e.CommandName == "Pay")
            {
                object[] arg = e.CommandArgument.ToString().Split(',');  //注意是单引号
                long orderId = Common.Globals.SafeLong(arg[0].ToString(), 0);
                string orderCode = arg[1].ToString();

                OrderInfo orderInfo = orderBll.GetModel(orderId);
                if (orderInfo != null && BLL.Shop.Order.OrderManage.PayForOrder(orderInfo, CurrentUser))
                {
                    MessageBox.ShowSuccessTip(this, "操作成功！");
                }
                else
                {
                    MessageBox.ShowSuccessTip(this, "操作失败，请稍候再试！");
                }
            }
            gridView.OnBind();

        }
 
        #endregion
 
        #region 获取状态
        /// <summary>
        /// 获取订单状态
        /// </summary>
        /// <param name="target"></param>
        /// <returns></returns>
        protected string GetOrderStatus(object target)
        {
            // -4 系统锁定   | -3 后台锁定 | -2 用户锁定 | -1 死单（取消） | 0 未处理 | 1 进行中 |2 完成  
            string str = string.Empty;
            if (!StringPlus.IsNullOrEmpty(target))
            {
                switch (target.ToString())
                {
                    case "-4":
                        str = "系统锁定";
                        break;
                    case "-3":
                        str = "后台锁定";
                        break;
                    case "-2":
                        str = "用户锁定";
                        break;
                    case "-1":
                        str = "死单（取消）";
                        break;
                    case "0":
                        str = "未处理";
                        break;
                    case "1":
                        str = "进行中";
                        break;
                    case "2":
                        str = "完成";
                        break;
                    default:
                        str = "未知状态";
                        break;
                }
            }
            return str;
        }

        /// <summary>
        /// 获取发货状态
        /// </summary>
        /// <param name="target"></param>
        /// <returns></returns>
        protected string GetShippingStatus(object target)
        {
            //  配送状态 0 未发货 | 1 打包中 | 2 已发货 | 3 已确认收货 | 4 拒收退货中 | 5 拒收已退货
            string str = string.Empty;
            if (!StringPlus.IsNullOrEmpty(target))
            {
                switch (target.ToString())
                {
                    case "0":
                        str = "未发货";
                        break;
                    case "1":
                        str = "打包中";
                        break;
                    case "2":
                        str = "已发货";
                        break;
                    case "3":
                        str = "已确认收货";
                        break;
                    case "4":
                        str = "拒收退货中";
                        break;
                    case "5":
                        str = "拒收已退货";
                        break;
                    default:
                        str = "未知状态";
                        break;
                }
            }
            return str;
        }


        protected string GetOrderType(object paymentGateway_obj, object orderStatus_obj, object paymentStatus_obj, object shippingStatus_obj)
        {
            // 1 等待买家付款   | 2 等待发货 | 3 已发货 | 4 退款中 | 5 成功订单 | 6 已退款 |7 已退货  |8 已关闭  
            string str = string.Empty;
            if (!StringPlus.IsNullOrEmpty(paymentGateway_obj) && !StringPlus.IsNullOrEmpty(orderStatus_obj) && !StringPlus.IsNullOrEmpty(paymentStatus_obj) && !StringPlus.IsNullOrEmpty(shippingStatus_obj))
            {
                EnumHelper.OrderMainStatus orderType = orderBll.GetOrderType(paymentGateway_obj.ToString(),
                                        Common.Globals.SafeInt(orderStatus_obj.ToString(), 0),
                                        Common.Globals.SafeInt(paymentStatus_obj.ToString(), 0),
                                        Common.Globals.SafeInt(shippingStatus_obj.ToString(), 0));
                switch (orderType)
                {
                    //  订单组合状态 1 等待付款   | 2 等待处理 | 3 取消订单 | 4 订单锁定 | 5 等待付款确认 | 6 正在处理 |7 配货中  |8 已发货 |9  已完成
                    case EnumHelper.OrderMainStatus.Paying:
                        str = "等待付款";
                        break;
                    case EnumHelper.OrderMainStatus.PreHandle:
                        str = "等待处理";
                        break;
                    case EnumHelper.OrderMainStatus.Cancel:
                        str = "取消订单";
                        break;
                    case EnumHelper.OrderMainStatus.Locking:
                        str = "订单锁定";
                        break;
                    case EnumHelper.OrderMainStatus.PreConfirm:
                        str = "等待付款确认";
                        break;
                    case EnumHelper.OrderMainStatus.Handling:
                        str = "正在处理";
                        break;
                    case EnumHelper.OrderMainStatus.Shipping:
                        str = "配货中";
                        break;
                    case EnumHelper.OrderMainStatus.Shiped:
                        str = "已发货";
                        break;
                    case EnumHelper.OrderMainStatus.Complete:
                        str = "已完成";
                        break;
                    default:
                        str = "未知状态";
                        break;
                }
            }
            return str;
        }

        #endregion
 
        private void DoCallback()
        {
            string action = this.Request.Form["Action"];
            this.Response.Clear();
            this.Response.ContentType = "application/json";
            string writeText = string.Empty;

            switch (action)
            {
                case "AjaxRetuOrder":
                    writeText = GetRetuOrder();
                    break;
                default:
                    break;

            }
            this.Response.Write(writeText);
            this.Response.End();
        }
        /// <summary>
        /// 判断是否符合退单的条件
        /// </summary>
        /// <returns></returns>
        private string GetRetuOrder()
        {
            JsonObject json = new JsonObject();
            long orderId = Globals.SafeLong(this.Request.Form["oi"], 0);
            int orderStates = Globals.SafeInt(this.Request.Form["os"], 0);
            if (retuOrderBll.IsMeetCondition(orderId, orderStates))
            {
                json.Put("STATUS", "True");
            }
            else
            {
                json.Put("STATUS", "False");
            }  
            return json.ToString();
        }

    }
}