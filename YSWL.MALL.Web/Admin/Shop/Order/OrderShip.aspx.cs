using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using YSWL.MALL.BLL.Ms;
using YSWL.MALL.BLL.Shop.Order;
using YSWL.MALL.BLL.Shop.Products;
using YSWL.MALL.BLL.Shop.Shipping;
using YSWL.Common;
using YSWL.MALL.Model.Shop.Order;
using OrderItems = YSWL.MALL.BLL.Shop.Order.OrderItems;

namespace YSWL.MALL.Web.Admin.Shop.Order
{
    public partial class OrderShip : PageBaseAdmin
    {
        protected override int Act_PageLoad { get { return 446; } } //Shop_订单管理_发货页
        private YSWL.MALL.BLL.Shop.Order.Orders orderBll = new Orders();
        private YSWL.MALL.BLL.Shop.Order.OrderItems itemBll = new OrderItems();
        private YSWL.MALL.BLL.Shop.Shipping.ShippingType typeBll = new ShippingType();
        private YSWL.MALL.BLL.Shop.Products.SKUInfo skuBll = new SKUInfo();
        private  YSWL.MALL.BLL.Ms.Regions regionBll=new Regions();
        private  YSWL.MALL.BLL.Shop.Order.OrderAction actionBll=new YSWL.MALL.BLL.Shop.Order.OrderAction();
        private BLL.Shop.Shippers shippersBll = new BLL.Shop.Shippers();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                ShowInfo();
                LoadShippers();
                LoadExpressTemp();
            }
        }

        #region 快递单打印
        #region 发货人信息
        // dropShippers
        //加载发货人
        private void LoadShippers()
        {
            List<Model.Shop.Shippers> list = shippersBll.GetModelList("");
            dropShippers.DataSource = list;
            dropShippers.DataTextField = "ShipperTag";
            dropShippers.DataValueField = "ShipperId";
            dropShippers.DataBind();
            dropShippers.Items.Insert(0, new ListItem("请选择发货标签", "0"));
            foreach (Model.Shop.Shippers item in list)
            {
                if (item.IsDefault)
                {
                    dropShippers.SelectedValue = item.ShipperId.ToString();
                    regionListShip.Region_iID = item.RegionId;
                    labelTelPhone.Text = item.TelPhone;
                    labelZipcode.Text = item.Zipcode;
                    lalelShipperName.Text = item.ShipperName;
                    break;
                }
            }
          
        }
        protected void dropShippers_Changed(object sender, EventArgs e)
        {
            Model.Shop.Shippers shipperModel = shippersBll.GetModelByCache(Globals.SafeInt(dropShippers.SelectedValue,0));
            if (shipperModel != null)
            {
                dropShippers.SelectedValue = shipperModel.ShipperId.ToString();
                regionListShip.Region_iID = shipperModel.RegionId;
                labelTelPhone.Text = shipperModel.TelPhone;
                labelZipcode.Text = shipperModel.Zipcode;
                lalelShipperName.Text = shipperModel.ShipperName;
            }
            else
            {
                dropShippers.SelectedValue ="0";
                regionListShip.Area_iID = 0;
                labelTelPhone.Text = "";
                labelZipcode.Text = "";
                lalelShipperName.Text = "";
            }

        }
        #endregion
        BLL.Shop.Sales.ExpressTemplate expresstempBll = new BLL.Shop.Sales.ExpressTemplate();
        #region 选择快递单模版
        private void LoadExpressTemp()
        {
            dropExpressTemp.DataSource = expresstempBll.GetList(" IsUse=1 ");
            dropExpressTemp.DataTextField = "ExpressName";
            dropExpressTemp.DataValueField = "XmlFile";
            dropExpressTemp.DataBind();
            dropExpressTemp.Items.Insert(0, new ListItem("请选择", ""));
        }
        
        #endregion

        #endregion

       

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

                this.lblTitle.Text = "正在进行订单【" + model.OrderCode + "】发货操作";
                lblOrderCode.Text = model.OrderCode;
                lblCreatedDate.Text = model.CreatedDate.ToString("yyyy-MM-dd HH:mm:ss");
                this.txtShipName.Text = model.ShipName;
                lblShipZipCode.Text = model.ShipZipCode;
                txtShipAddress.Text = model.ShipAddress;
                txtShipCellPhone.Text = model.ShipCellPhone;
                lblShipEmail.Text = model.ShipEmail;
                RegionList.Region_iID = model.RegionId;
                //lblShipRegion.Text = model.ShipRegion;
                txtShipTelPhone.Text = model.ShipTelPhone;

                lblBuyerEmail.Text = model.BuyerEmail;
                lblBuyerCellPhone.Text = model.BuyerCellPhone;
                lblBuyerName.Text = model.BuyerName;

                txtShipOrderNumber.Text = model.ShipOrderNumber;


                hfWeight.Value = model.Weight.ToString();
                txtFreightActual.Text = model.FreightActual.HasValue ? model.FreightActual.Value.ToString("F") : "0.00";
                lblFreightAdjusted.Text = model.FreightAdjusted.HasValue ? model.FreightAdjusted.Value.ToString("F") : "0.00";

                //加载物流方式
                this.ddlShipType.DataSource = typeBll.GetList("");
                ddlShipType.DataTextField = "Name";
                ddlShipType.DataValueField = "ModeId";
                ddlShipType.DataBind();
                ddlShipType.Items.Insert(0, new ListItem("请选择配送方式", "0"));
                ddlShipType.SelectedValue = model.ShippingModeId.HasValue ? model.ShippingModeId.Value.ToString() : "0";

                //快递单打印
                labelCellPhone.Text = model.ShipCellPhone;
                labelShipAddress.Text = model.ShipAddress;
                labelShipEmail.Text = model.ShipEmail;
                labelShipName.Text = model.ShipName;
                labelShipTelPhone.Text = model.ShipTelPhone;
                labelShipZipCode.Text = model.ShipZipCode;
                regionLists.Region_iID = model.RegionId;
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

        //获取库存
        //protected int GetStock(object obj_sku, object obj_Id)
        //{
        //    if (obj_sku != null)
        //    {
        //        if (!string.IsNullOrWhiteSpace(obj_sku.ToString()))
        //        {
        //            return skuBll.GetStockBySKU(obj_sku.ToString());
        //        }

        //    }
        //    if (obj_Id != null)
        //    {
        //        if (!string.IsNullOrWhiteSpace(obj_Id.ToString()))
        //        {
        //            long productId = Common.Globals.SafeLong(obj_Id.ToString(), 0);
        //            return skuBll.GetStockById(productId);
        //        }
        //    }
        //    return 0;

        //}

        protected void btnSave_OnClick(object sender, EventArgs e)
        {
            YSWL.MALL.Model.Shop.Order.OrderInfo orderModel = orderBll.GetModel(OrderId);
            int modeId = Common.Globals.SafeInt(this.ddlShipType.SelectedValue, 0);
            YSWL.MALL.Model.Shop.Shipping.ShippingType typeModel = typeBll.GetModelByCache(modeId);
            orderModel.ExpressCompanyName = typeModel.ExpressCompanyName;
            orderModel.ExpressCompanyAbb = typeModel.ExpressCompanyEn;
            orderModel.ShippingModeId = typeModel.ModeId;
            orderModel.ShippingModeName = typeModel.Name;
            orderModel.RealShippingModeName = typeModel.Name;
            orderModel.ShipOrderNumber = this.txtShipOrderNumber.Text;
            orderModel.FreightActual = Common.Globals.SafeDecimal(txtFreightActual.Text, 0);

            int regionId = RegionList.Region_iID;
            orderModel.RegionId = regionId;
            orderModel.ShipRegion = regionBll.GetRegionNameByRID(regionId);

            orderModel.ShipName = txtShipName.Text;
            orderModel.ShipAddress = txtShipAddress.Text;
            orderModel.ShipTelPhone = txtShipTelPhone.Text;
            orderModel.ShipCellPhone = txtShipCellPhone.Text;
            if (orderModel.OrderStatus !=  (int)YSWL.MALL.Model.Shop.Order.EnumHelper.OrderStatus.Handling  ||    orderModel.ShippingStatus != (int)YSWL.MALL.Model.Shop.Order.EnumHelper.ShippingStatus.Packing )
            {
                MessageBox.ShowFailTipScript(this, "当前订单的状态已改变,您已不能修改,稍后为您转到列表页...", "$('[id$=btnSearch]', window.parent.document).click(); ");
                return;
            }

            //已发货
            orderModel.ShippingStatus = (int) YSWL.MALL.Model.Shop.Order.EnumHelper.ShippingStatus.Shipped;
            orderModel.OrderStatus = (int) YSWL.MALL.Model.Shop.Order.EnumHelper.OrderStatus.Handling;

            if (orderBll.UpdateShipped(orderModel))
            {
                //新增订单日志
                YSWL.MALL.Model.Shop.Order.OrderAction actionModel = new YSWL.MALL.Model.Shop.Order.OrderAction();
                int actionCode = (int) YSWL.MALL.Model.Shop.Order.EnumHelper.ActionCode.Shipped;
                actionModel.ActionCode = actionCode.ToString();
                actionModel.ActionDate = DateTime.Now;
                actionModel.OrderCode = orderModel.OrderCode;
                actionModel.OrderId = orderModel.OrderId;
                actionModel.Remark = "发货操作";
                actionModel.UserId = CurrentUser.UserID;
                actionModel.Username =String.IsNullOrWhiteSpace(CurrentUser.NickName)? CurrentUser.UserName: CurrentUser.NickName;
                actionBll.Add(actionModel);

                //清除缓存
                orderBll.RemoveModelInfoCache(orderModel.OrderId);

                MessageBox.ShowSuccessTipScript(this, "操作成功！", "window.parent.location.reload();");
                //MessageBox.ResponseScript(this,);
                //Page.ClientScript.RegisterClientScriptBlock();
            }
            else
            {
                MessageBox.ShowFailTip(this, "操作失败！");
            }
        }

        protected void ShipType_Changed(object sender, EventArgs e)
        {
            int modeId = Common.Globals.SafeInt(this.ddlShipType.SelectedValue, 0);
            YSWL.MALL.Model.Shop.Shipping.ShippingType typeModel = typeBll.GetModelByCache(modeId);
            if (typeModel != null)
            {
                txtFreightActual.Text = typeBll.GetFreight(typeModel, Common.Globals.SafeInt(hfWeight.Value, 0)).ToString("F");
                lblFreightAdjusted.Text = txtFreightActual.Text;
            }
            else
            {
                txtFreightActual.Text = "0.00";
                lblFreightAdjusted.Text = "0.00";
            }

        }



    }
}