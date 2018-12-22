using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using YSWL.Common;
using YSWL.Json;
using YSWL.Json.Conversion;
using System.Text;
using YSWL.MALL.BLL.Shop;
using YSWL.MALL.Model.Shop;
using YSWL.MALL.BLL.Shop.Products;

namespace YSWL.MALL.Web.Admin.Shop.Order.SubmitOrder
{
    public partial class CartList : PageBaseAdmin
    {
        private BLL.Shop.Shipping.ShippingType shippTypeBll = new BLL.Shop.Shipping.ShippingType();
        private BLL.Shop.Shipping.ShippingAddress shipAddressBll = new BLL.Shop.Shipping.ShippingAddress();
        protected void Page_Load(object sender, EventArgs e)
        { 
        }

        #region gridView

        public void BindData()
        {
            ShoppingCartHelper cartHelper = new ShoppingCartHelper(UserId);
            this.gridViewCart.DataSource = cartHelper.GetShoppingCart(true).Items;
            this.gridViewCart.DataBind();

            hidCartProdTotalSellPrice.Value = cartHelper.GetShoppingCart(true).TotalSellPrice.ToString("F");
            hidCartProdTotalAdjustedPrice.Value = cartHelper.GetShoppingCart(true).TotalAdjustedPrice.ToString("F");
            decimal freight = decimal.Zero;
            if (ShipTypeId > 0)
            {
                Model.Shop.Shipping.ShippingType shippingType = shippTypeBll.GetModelByCache(ShipTypeId);
                #region 计算运费
                YSWL.MALL.Model.Shop.Products.ShoppingCartInfo cartInfo = cartHelper.GetShoppingCart(true);
                BLL.Shop.Shipping.ShippingRegionGroups _shippingRegionManage = new BLL.Shop.Shipping.ShippingRegionGroups();
                BLL.Ms.Regions _regionManage = new BLL.Ms.Regions();
                YSWL.MALL.Model.Shop.Shipping.ShippingAddress shippingAddress = shipAddressBll.GetModelByUserId(Globals.SafeInt(Cookies.getCookie("A_Order_SelectUserId", "value"), 0));


                #region 区域差异运费计算
                //有收货地址 且 已选择配送 计算差异运费
                if (shippingAddress!=null && cartInfo!=null &&  shippingType != null && shippingType.ModeId > 0 && shippingAddress.RegionId > 0)
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
                        freight = cartInfo.CalcFreight(shippingType, shippingRegion);
                    }
                }
                #endregion

                #endregion
            }
            hidFreight.Value = freight.ToString("F");
        }
        private int UserId {
            get { return   Globals.SafeInt(Cookies.getCookie("A_Order_SelectUserId", "value"), 0); }
        }
        private int ShipTypeId
        {
            get { return Globals.SafeInt(Cookies.getCookie("A_Order_ShipTypeId", "value"), 0); }
        }
        protected void btnSearch_Click(object sender, EventArgs e)
        {

            this.gridViewCart.OnBind();
        }



        protected void gridView_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gridViewCart.PageIndex = e.NewPageIndex;
            gridViewCart.OnBind();
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
        #endregion

        #region 库存
        /// <summary>
        /// 获取库存
        /// </summary>
        /// <param name="o">sku</param>
        /// <returns></returns>
        protected int GetStock(object o_sku)
        {
            if (o_sku == null) return 0;
            return  YSWL.MALL.BLL.Shop.Products.StockHelper.GetSkuStockEx(o_sku.ToString(), DepotId,null);
        }
        #endregion

        #region 获取仓库id
        protected int DepotId
        {
            get
            {
                return Globals.SafeInt(Cookies.getCookie("A_Order_DepotId", "value"), 0);
            }
        }
        #endregion

    }
}