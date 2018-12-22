using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;
using YSWL.MALL.BLL.Shop;
using YSWL.MALL.BLL.Shop.Products;
using YSWL.Common;
using YSWL.Json;
using YSWL.MALL.Model.Shop;
using YSWL.Payment.Model;

namespace YSWL.MALL.Web.Admin.Shop.Order.SubmitOrder
{
    public partial class ProductSKUList : PageBaseAdmin
    {
        private BLL.Members.UsersExp userBll = new BLL.Members.UsersExp();
        BLL.Shop.Shipping.ShippingAddress shipAddressBll = new BLL.Shop.Shipping.ShippingAddress();
        BLL.Shop.Shipping.ShippingType shippTypeBll = new BLL.Shop.Shipping.ShippingType();
        BLL.Shop.DisDepot.Depot depotBll = new BLL.Shop.DisDepot.Depot();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(this.Request.Form["Callback"]) && (this.Request.Form["Callback"] == "true"))
            {
                this.Controls.Clear();
                this.DoCallback();
            }
            if (!IsPostBack)
            {
                Model.Members.UsersExpModel userModel=userBll.GetUsersModel(UserId); 
                if(userModel==null || userModel.UserID<=0){
                      MessageBox.ShowAndRedirect(this, "请先选择用户", "SelectUser.aspx");
                    return;
                }
                ltlSelectUser.Text = userModel.UserName;
                Model.Shop.Shipping.ShippingAddress addressModel = shipAddressBll.GetModelByUserId(UserId);
                if (addressModel != null)
                {
                    ltAddress.Text = addressModel.RegionFullName + "&#12288;" + addressModel.Address;
                    ltshipName.Text = addressModel.ShipName;
                    ltphone.Text = addressModel.CelPhone;
                }
                if (IsMultiDepot) {//开启了分仓
                    YSWL.MALL.Model.Shop.DisDepot.Depot depotModel = depotBll.GetModel(DepotId);
                    if (depotModel != null)
                    {
                        ltdepot.Text = depotModel.Name;
                    }
                    selectedUserDepotId.Value = DepotId.ToString();
                }
                LoadShippingType();


                #region 新增支付方式  货到付款
                List<PaymentModeInfo> paylist = Payment.BLL.PaymentModeManage.GetPaymentModes();
                if (paylist != null)
                {
                    paylist = paylist.Where(o => o.Gateway == "cod").OrderBy(o => o.DisplaySequence).ToList();
                }
                if (paylist != null && paylist.Count > 0)
                {
                    dropPayType.Items.Insert(1, new ListItem(paylist[0].Name, "1"));
                }
                #endregion
            }
        }

        #region 已选用户

        /// <summary>
        /// 已选用户
        /// </summary>
        protected int UserId
        {
            get
            {
                return Globals.SafeInt(Cookies.getCookie("A_Order_SelectUserId", "value"),0);
            }
        }
        

        #endregion


        #region 获取仓库id
        protected int DepotId
        {
            get
            {
                return Globals.SafeInt(Cookies.getCookie("A_Order_DepotId", "value"),0);
            }
        } 
        #endregion



        #region 加载配送方式
        private void LoadShippingType()
        {
            DataSet ds = shippTypeBll.GetList(0," ", " DisplaySequence ");
            if (!DataSetTools.DataSetIsNull(ds))
            {
                this.dropShippingType.DataSource = ds;
                this.dropShippingType.DataTextField = "Name";
                this.dropShippingType.DataValueField = "ModeId";
                this.dropShippingType.DataBind();
            }
            this.dropShippingType.Items.Insert(0, new ListItem("请选择", "-1"));
        }
        #endregion


        #region Ajax方法
        private void DoCallback()
        {
            string action = this.Request.Form["Action"];
            this.Response.Clear();
            this.Response.ContentType = "application/json";
            string writeText = string.Empty;

            switch (action)
            {
                case "SetShippingType": //设置配送方式并计算运费
                    writeText = SetShippingType();
                    break;
                default:
                    break;
            }
            this.Response.Write(writeText);
            this.Response.End();
        }
 
        private string SetShippingType()
        {
            JsonObject json = new JsonObject();
            int shipTypeId = Globals.SafeInt(this.Request.Form["shipTypeId"], 0);
            //写Cookie
            Cookies.setCookie("A_Order_ShipTypeId", shipTypeId.ToString(), 1440);
            json.Put("STATUS", "SUCCESS");
            if (shipTypeId <= 0)
            {
                json.Put("DATA", 0);
                return json.ToString();
            }
            Model.Shop.Shipping.ShippingType shippingType = shippTypeBll.GetModelByCache(shipTypeId);
            if (shippingType == null)
            {
                json.Put("DATA", 0);
                return json.ToString();
            }               
            ShoppingCartHelper cartHelper = new ShoppingCartHelper(Globals.SafeInt(Cookies.getCookie("A_Order_SelectUserId", "value"), 0));
            //计算运费

            #region 计算运费
            decimal freight = decimal.Zero;
            YSWL.MALL.Model.Shop.Products.ShoppingCartInfo cartInfo = cartHelper.GetShoppingCart(true);
            BLL.Shop.Shipping.ShippingRegionGroups _shippingRegionManage = new BLL.Shop.Shipping.ShippingRegionGroups();
            BLL.Ms.Regions _regionManage = new BLL.Ms.Regions();
            YSWL.MALL.Model.Shop.Shipping.ShippingAddress shippingAddress = shipAddressBll.GetModelByUserId(Globals.SafeInt(Cookies.getCookie("A_Order_SelectUserId", "value"), 0));
            if (shippingAddress == null || cartInfo == null)
            {
                json.Put("DATA", freight);
                return json.ToString();
            }
            #region 区域差异运费计算
            //有收货地址 且 已选择配送 计算差异运费
            if (shippingType != null && shippingType.ModeId > 0 && shippingAddress.RegionId > 0)
            {
                Model.Ms.Regions regionInfo = _regionManage.GetModelByCache(shippingAddress.RegionId);
                if (regionInfo != null)
                {
                    int topRegionId;
                    if (regionInfo.Depth > 1)
                    {
                        topRegionId = Common.Globals.SafeInt(regionInfo.Path.Split(new[] { ',' })[1], -1);
                    }
                    else
                    {
                        topRegionId = regionInfo.RegionId;
                    }
                    Model.Shop.Shipping.ShippingRegionGroups shippingRegion =
                        _shippingRegionManage.GetShippingRegion(shippingType.ModeId, topRegionId);
                    freight= cartInfo.CalcFreight(shippingType, shippingRegion);
                }
            }
            #endregion

            #endregion

            json.Put("DATA", freight);
            return json.ToString();
        }

        #endregion
    }
}