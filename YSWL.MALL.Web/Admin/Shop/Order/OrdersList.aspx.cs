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
using System.Web.UI.HtmlControls;

namespace YSWL.MALL.Web.Admin.Shop.Order
{
    public partial class OrdersList : PageBaseAdmin
    {
        protected override int Act_PageLoad { get { return 443; } } //Shop_订单管理_列表页
        protected new int Act_UpdateData = 444;    //Shop_订单管理_编辑数据
        public YSWL.MALL.BLL.Shop.Order.Orders orderBll = new Orders();
        public YSWL.MALL.BLL.Shop.Order.OrdersHistory historyBll = new OrdersHistory();
        public YSWL.MALL.BLL.Shop.Order.OrderAction actionBll = new BLL.Shop.Order.OrderAction();
        public int Type = 0;
        private bool IsConnectionOMS = false;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                gridView.Columns[12].Visible = YSWL.MALL.BLL.Shop.Service.CommonHelper.OpenMultiDepot();
                hidIsMultiDepot.Value = YSWL.MALL.BLL.Shop.Service.CommonHelper.OpenMultiDepot().ToString();
                IsConnectionOMS = YSWL.MALL.BLL.Shop.Service.CommonHelper.ConnectionOMS();
                hidIsConnectionOMS.Value = IsConnectionOMS.ToString();
          
                Type = YSWL.Common.Globals.SafeInt(Request.Params["type"], 0);                

                //获取是否开启相关选项卡
                this.hfPaying.Value = YSWL.MALL.BLL.SysManage.ConfigSystem.GetValueByCache("Shop_OrderList_Paying");
                this.hfPreHandle.Value = YSWL.MALL.BLL.SysManage.ConfigSystem.GetValueByCache("Shop_OrderList_PreHandle");
                this.hfCancel.Value = YSWL.MALL.BLL.SysManage.ConfigSystem.GetValueByCache("Shop_OrderList_Cancel");

                this.hfLocking.Value = YSWL.MALL.BLL.SysManage.ConfigSystem.GetValueByCache("Shop_OrderList_Locking");
                this.hfPreConfirm.Value = YSWL.MALL.BLL.SysManage.ConfigSystem.GetValueByCache("Shop_OrderList_PreConfirm");
                this.hfHandling.Value = YSWL.MALL.BLL.SysManage.ConfigSystem.GetValueByCache("Shop_OrderList_Handling");

                this.hfShipping.Value = YSWL.MALL.BLL.SysManage.ConfigSystem.GetValueByCache("Shop_OrderList_Shipping");
                this.hfShiped.Value = YSWL.MALL.BLL.SysManage.ConfigSystem.GetValueByCache("Shop_OrderList_Shiped");
                this.hfSuccess.Value = YSWL.MALL.BLL.SysManage.ConfigSystem.GetValueByCache("Shop_OrderList_Complete");
                BindSupplier();
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
            if (dropPaymentStatus.SelectedValue != "-1")
            {
                if (strWhere.Length > 1)
                {
                    strWhere.Append(" and ");
                }
                strWhere.AppendFormat(" PaymentStatus = {0}", dropPaymentStatus.SelectedValue);
            }
            if (dropShippingStatus.SelectedValue != "-1")
            {
                if (strWhere.Length > 1)
                {
                    strWhere.Append(" and ");
                }
                strWhere.AppendFormat(" ShippingStatus = {0}", dropShippingStatus.SelectedValue);
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

            int supplierId = Common.Globals.SafeInt(this.ddlSupplier.SelectedValue, 0);
            if (supplierId == 0) supplierId = SupplierId;
            if (supplierId > 0)
            {
                if (strWhere.Length > 1) strWhere.Append(" and ");
                strWhere.AppendFormat(" SupplierId = {0}", supplierId);
            }
            if (supplierId == -1)
            {
                if (strWhere.Length > 1) strWhere.Append(" and ");
                strWhere.AppendFormat(" SupplierId IS NULL");
            }

            string str =string.Empty;
            if (chkshowMainOrder.Checked)
            {
                str = " or  OrderType = 1 ";
            }
            if (strWhere.Length > 1) strWhere.Append(" and ");
            //主订单 无子订单
            strWhere.AppendFormat(" ((OrderType = 1 AND HasChildren = 0) " +
                //子订单 已支付 或 货到付款/银行转账
            "OR (OrderType = 2 AND (PaymentStatus > 1 OR (PaymentGateway='cod' OR PaymentGateway='bank')) ) " +
                //主订单 有子订单 未支付的主订单 非 货到付款/银行转账 子订单
            "OR (OrderType = 1 AND HasChildren = 1 AND PaymentStatus < 2 AND PaymentGateway<>'cod' AND PaymentGateway<>'bank')  {0}  )", str);


          
            if (OrderStatus == -1)
            {
                gridView.DataSetSource = historyBll.GetList(-1, strWhere.ToString(), "OrderId desc");
            }
            else
            {
                gridView.DataSetSource = orderBll.GetList(-1, strWhere.ToString(), "OrderId desc");
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
                    //LinkButton linkReturn = (LinkButton)e.Row.FindControl("linkReturn");
                    //linkReturn.Visible = false;
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

                #region 检查订单状态，防止一个订单操作两次
                //是否对接oms
                if (IsConnectionOMS)
                {
                    //对接了oms 
                    //在线支付未支付时可取消
                    if (orderInfo.OrderStatus != (int)YSWL.MALL.Model.Shop.Order.EnumHelper.OrderStatus.UnHandle || orderInfo.PaymentStatus != (int)Payment.Model.PaymentStatus.NotYet || orderInfo.PaymentGateway == "cod")
                    {
                        MessageBox.ShowFailTipScript(this, "当前订单的状态已改变,您已不能修改,稍后为您刷新页面...", "$('[id$=btnSearch]').click(); ");
                        return;
                    }
                }
                else
                {
                    //未对接oms    未发货时 可取消
                    if (orderInfo.OrderStatus < (int)YSWL.MALL.Model.Shop.Order.EnumHelper.OrderStatus.UnHandle || orderInfo.OrderStatus > (int)YSWL.MALL.Model.Shop.Order.EnumHelper.OrderStatus.Handling || orderInfo.ShippingStatus >= (int)YSWL.MALL.Model.Shop.Order.EnumHelper.ShippingStatus.Shipped)
                    {
                        MessageBox.ShowFailTipScript(this, "当前订单的状态已改变,您已不能修改,稍后为您刷新页面...", "$('[id$=btnSearch]').click(); ");
                        return;
                    }
                }

                #endregion
                if (YSWL.MALL.BLL.Shop.Order.OrderManage.CancelOrder(orderInfo, CurrentUser))
                {
                    MessageBox.ShowSuccessTip(this, "操作成功！");
                }
                else
                {
                    MessageBox.ShowSuccessTip(this, "操作失败，请稍候再试！");
                }
            }
            if (e.CommandName == "Success")
            {
                object[] arg = e.CommandArgument.ToString().Split(',');  //注意是单引号
                long orderId = Common.Globals.SafeLong(arg[0].ToString(), 0);
                string orderCode = arg[1].ToString();
                OrderInfo orderInfo = orderBll.GetModelInfo(orderId);
                if (!(orderInfo.OrderStatus == (int)YSWL.MALL.Model.Shop.Order.EnumHelper.OrderStatus.Handling && orderInfo.ShippingStatus == (int)YSWL.MALL.Model.Shop.Order.EnumHelper.ShippingStatus.Shipped))
                {
                    MessageBox.ShowFailTipScript(this, "当前订单的状态已改变,您已不能修改,稍后为您刷新页面...", "$('[id$=btnSearch]').click(); ");
                    return;
                } 
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
                //在线支付未支付时 可支付
                if (orderInfo.PaymentStatus != (int)Payment.Model.PaymentStatus.NotYet || orderInfo.OrderStatus != (int)YSWL.MALL.Model.Shop.Order.EnumHelper.OrderStatus.UnHandle || orderInfo.PaymentGateway == "cod")
                {
                    MessageBox.ShowFailTipScript(this, "当前订单的状态已改变,您已不能修改,稍后为您刷新页面...", "$('[id$=btnSearch]').click(); ");
                    return;
                }
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

        #region 供应商
        /// <summary>
        /// 供应商
        /// </summary>
        private void BindSupplier()
        {
            YSWL.MALL.BLL.Shop.Supplier.SupplierInfo infoBll = new BLL.Shop.Supplier.SupplierInfo();
            DataSet ds = infoBll.GetList("  Status = 1 ");
            if (!DataSetTools.DataSetIsNull(ds))
            {
                this.ddlSupplier.DataSource = ds;
                this.ddlSupplier.DataTextField = "Name";
                this.ddlSupplier.DataValueField = "SupplierId";
                this.ddlSupplier.DataBind();
            }
            this.ddlSupplier.Items.Insert(0, new ListItem("平　台", "-1"));
            this.ddlSupplier.Items.Insert(0, new ListItem("全　部", string.Empty));
            this.ddlSupplier.Items.Insert(0, new ListItem(string.Empty, string.Empty));
            if (SupplierId != 0)
            {
                ddlSupplier.SelectedValue = SupplierId.ToString();
            }
            else
            {
                ddlSupplier.SelectedIndex = 0;
            }
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
                str = orderBll.GetOrderTypeStr(paymentGateway_obj.ToString(),
                                        Common.Globals.SafeInt(orderStatus_obj.ToString(), 0),
                                        Common.Globals.SafeInt(paymentStatus_obj.ToString(), 0),
                                        Common.Globals.SafeInt(shippingStatus_obj.ToString(), 0));
            }
            return str;
        }

        #endregion

        #region 导出订单
        //-姓名
        //-地址
        //-联系电话
        //-邮编
        //-商品金额
        //-付款金额
        //-运费金额
        //-付款时间
        //-购买商品内容
        //-订单备注

        protected void btnExport_Click(object sender, EventArgs e)
        {
            BindData();
            DataSet dataSet = gridView.DataSetSource as DataSet;
            if (Common.DataSetTools.DataSetIsNull(dataSet))
            {
                MessageBox.ShowServerBusyTip(this, "抱歉, 当前没有可以导出的数据!");
                return;
            }
            DataTable dataTable = new DataTable();
            dataTable.Columns.Add(new DataColumn("订单编号", typeof(string)));
            dataTable.Columns.Add(new DataColumn("姓名", typeof(string)));
            dataTable.Columns.Add(new DataColumn("地址", typeof(string)));
            dataTable.Columns.Add(new DataColumn("联系电话", typeof(string)));
            dataTable.Columns.Add(new DataColumn("邮编", typeof(string)));
            dataTable.Columns.Add(new DataColumn("商品金额", typeof(string)));
            dataTable.Columns.Add(new DataColumn("付款金额", typeof(string)));
            dataTable.Columns.Add(new DataColumn("运费金额", typeof(string)));
            dataTable.Columns.Add(new DataColumn("下单时间", typeof(DateTime)));
            dataTable.Columns.Add(new DataColumn("购买商品内容", typeof(string)));
            dataTable.Columns.Add(new DataColumn("订单备注", typeof(string)));

            DataRow tmpRow;
            YSWL.MALL.BLL.Ms.Regions regionBll = new YSWL.MALL.BLL.Ms.Regions();
            string remark;
            foreach (DataRow row in dataSet.Tables[0].Rows)
            {
                tmpRow = dataTable.NewRow();
                tmpRow["订单编号"] = row.Field<string>("OrderCode");
                tmpRow["姓名"] = row.Field<string>("ShipName");
                tmpRow["地址"] = regionBll.GetRegionNameByRID(row.Field<int>("RegionId")) + " " +
                    row.Field<string>("ShipAddress");
                tmpRow["联系电话"] = row.Field<string>("ShipCellPhone");
                tmpRow["邮编"] = row.Field<string>("ShipZipCode");
                tmpRow["商品金额"] = (row.Field<decimal>("Amount") - row.Field<decimal>("FreightAdjusted")).ToString("F");
                tmpRow["付款金额"] = row.Field<decimal>("Amount").ToString("F");
                tmpRow["运费金额"] = row.Field<decimal>("FreightAdjusted").ToString("F");

                //TODO: 没有付款时间, 只有创建时间和最后操作时间
                tmpRow["下单时间"] = row.Field<DateTime>("CreatedDate");

                tmpRow["购买商品内容"] = GetProductContent(row.Field<long>("OrderId"));

                remark = row.Field<string>("Remark");
                tmpRow["订单备注"] = remark == " | " ? "" : remark;
                dataTable.Rows.Add(tmpRow);
            }
            DataSetToExcel(dataTable);
        }

        BLL.Shop.Order.OrderItems orderItemManage = new BLL.Shop.Order.OrderItems();
        private string GetProductContent(long orderId)
        {
            StringBuilder sb = new StringBuilder();
            List<OrderItems> list = orderItemManage.GetModelListByCache(" OrderId=" + orderId);
            if (list == null || list.Count < 1) return string.Empty;
            list.ForEach(info => sb.AppendFormat("{0} x{1} |", info.Name, info.Quantity));
            return sb.ToString().TrimEnd('|');
        }

        private void DataSetToExcel(DataTable data)
        {
            Response.Clear();
            using (System.IO.MemoryStream ms = new System.IO.MemoryStream())
            {
                string nowDate = DateTime.Now.ToString("yyyy-MM-dd_HHmmss");
                NPOI.HSSF.UserModel.HSSFWorkbook workbook = new NPOI.HSSF.UserModel.HSSFWorkbook();
                NPOI.HSSF.UserModel.HSSFSheet sheet = (NPOI.HSSF.UserModel.HSSFSheet)workbook.CreateSheet(
                    string.Format("导出订单_{0}",
                    nowDate));
                NPOI.HSSF.UserModel.HSSFRow headerRow = (NPOI.HSSF.UserModel.HSSFRow)sheet.CreateRow(0);
                foreach (DataColumn column in data.Columns)
                {
                    headerRow.CreateCell(column.Ordinal).SetCellValue(column.ColumnName);
                }
                int rowIndex = 1;
                foreach (DataRow row in data.Rows)
                {
                    NPOI.HSSF.UserModel.HSSFRow dataRow = (NPOI.HSSF.UserModel.HSSFRow)sheet.CreateRow(rowIndex);
                    foreach (DataColumn column in data.Columns)
                    {
                        dataRow.CreateCell(column.Ordinal).SetCellValue(row[column].ToString());
                    }
                    dataRow = null;
                    rowIndex++;
                }
                //自动调整列宽
                for (int i = 0; i < data.Columns.Count; i++)
                {
                    sheet.AutoSizeColumn(i);
                }
                workbook.Write(ms);
                headerRow = null;
                sheet = null;
                workbook = null;
                Response.AddHeader("Content-Disposition",
                    string.Format("attachment; filename=ExportOrder_{0}.xls",
                    nowDate));
                Response.BinaryWrite(ms.ToArray());
                ms.Close();
                ms.Dispose();
            }
            Response.End();
        }
        #endregion

        #region 根据订单类型过滤数据
        protected void btnBindData_Click(object sender, EventArgs e)
        {
            int orderType = 0;
            LinkButton linkButton = sender as LinkButton ?? new LinkButton();
            if (!string.IsNullOrWhiteSpace(linkButton.ID))
            {
                string type = linkButton.ID.Substring(1);//得到需要获取的订单类型
                orderType = Convert.ToInt32(type) == 10 ? -1 : Convert.ToInt32(type);//-1无法用来做ID
                this.orderType.Value = type;
            } 
            Type = orderType;
            gridView.OnBind();
        } 
        #endregion

        protected string GetOrderTypeSub(object typeSub_obj, object supplierId_obj, object supplierNameobj)
        {
            int typeSub = Common.Globals.SafeInt(typeSub_obj, 0);
            int supplierId = Common.Globals.SafeInt(supplierId_obj, 0);
            string supplierName = supplierNameobj.ToString();
            if (supplierId > 0)
            {
                return supplierName;
            }
            //1：A类，2：AB类，3：B类
            switch (typeSub)
            {
                case 1:
                    return "平台";
                case 2:
                    return "混合";
                case 3:
                    return "商家混合";
                default:
                    break;
            }
            return null;
        }
        protected string GetReferType(object referType_obj)
        {
            //来源   1：表示PC  2：表示微信  3:表示业务员代下单  4:客服代下单  5:APP
            string str = string.Empty;
            if (!StringPlus.IsNullOrEmpty(referType_obj))
            {
                switch (referType_obj.ToString())
                {
                    case "1":
                        str = "PC";
                        break;
                    case "2":
                        str = "微信C端";
                        break;
                    case "3":
                        str = "业务员代下单";
                        break;
                    case "4":
                        str = "客服代下单";
                        break;
                    case "5":
                        str = "APP";
                        break;
                    case "6":
                        str = "Pos下单";
                        break;
                    case "7":
                        str = "微信B端";
                        break;
                    default:
                        str = "未知";
                        break;
                }
            }
            return str;
        }
    }
}