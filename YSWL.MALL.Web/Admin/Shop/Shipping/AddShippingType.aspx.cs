using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using YSWL.Common;
using YSWL.MALL.Model.Shop.Shipping;
using YSWL.Payment;

namespace YSWL.MALL.Web.Admin.Shop.Shipping
{
    public partial class AddShippingType : PageBaseAdmin
    {
        protected override int Act_PageLoad { get { return 521; } } //Shop_配送方式管理_新增页
        private YSWL.MALL.BLL.Shop.Shipping.ShippingType typeBll = new BLL.Shop.Shipping.ShippingType();
        YSWL.MALL.BLL.Shop.Shipping.ShippingPayment payBll=new BLL.Shop.Shipping.ShippingPayment();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                ckPayType.DataSource = YSWL.Payment.BLL.PaymentModeManage.GetPaymentModes(YSWL.Payment.Model.DriveEnum.ALL);
                this.ckPayType.DataTextField = "Name";
                this.ckPayType.DataValueField = "ModeId";
                this.ckPayType.DataBind();
                for (int i = 0; i < this.ckPayType.Items.Count; i++)
                {
                        ckPayType.Items[i].Selected = true;
                }
                ddlType.DataSource = YSWL.MALL.Web.Components.ExpressHelper.GetAllComType();
                BLL.Ms.Regions regionManage = new BLL.Ms.Regions();
                List<Model.Ms.Regions> regionses = regionManage.GetProvinceList();
                this.ddlType.DataTextField = "ComName";
                this.ddlType.DataValueField = "ComEn";
                this.ddlType.DataBind();
                this.ddlType.Items.Insert(0, new ListItem("请选择", ""));
                if (regionses != null && regionses.Count > 0)
                {
                    foreach (Model.Ms.Regions item in regionses)
                    {
                        drpRegion.Items.Add(
                            new ListItem
                            {
                                Text = item.RegionName,
                                Value = item.RegionId.ToString()
                            });
                    }
                }
                BindSupplier();
            }
        }

        public void btnSave_Click(object sender, System.EventArgs e)
        {
            YSWL.MALL.Model.Shop.Shipping.ShippingType typeModel=new Model.Shop.Shipping.ShippingType();
            typeModel.AddPrice = Common.Globals.SafeDecimal(this.tAddPrice.Text, 0);
            typeModel.Price = Common.Globals.SafeDecimal(this.tPrice.Text, 0);
            typeModel.Weight = Common.Globals.SafeInt(this.tWeight.Text, 0);
            typeModel.AddWeight = Common.Globals.SafeInt(this.tAddWeight.Text, 0);
            typeModel.Description = this.tDesc.Text;
            typeModel.DisplaySequence = -1;
            typeModel.ExpressCompanyName = ddlType.SelectedItem.Text;
            typeModel.ExpressCompanyEn = ddlType.SelectedValue;
            typeModel.Name = this.tName.Text;
            typeModel.SupplierId = Common.Globals.SafeInt(ddlSupplier.SelectedValue, -1);
            typeModel.ModeId = typeBll.Add(typeModel);
            if (typeModel.ModeId > 0)
                {
                    //保存地区价格
                    SaveShippingRegionGroups(typeModel);
                    for (int i = 0; i < this.ckPayType.Items.Count; i++)
                    {
                        if (ckPayType.Items[i].Selected)
                        {
                            YSWL.MALL.Model.Shop.Shipping.ShippingPayment payModel=new ShippingPayment();
                            payModel.PaymentModeId = Common.Globals.SafeInt(ckPayType.Items[i].Value, 0);
                            payModel.ShippingModeId = typeModel.ModeId; 
                            payBll.Add(payModel);
                        }
                    }
                    Response.Redirect("ShippingType.aspx");
                }
                else
                {
                    MessageBox.ShowFailTip(this, "新增失败！请重试。");
                }
        }
        
        private void SaveShippingRegionGroups(Model.Shop.Shipping.ShippingType shippingType)
        {
            string data = hfRegionData.Value;
            if (string.IsNullOrWhiteSpace(data)) return;

            List<ShippingRegionGroups> list = GetRegionGroups(shippingType, data);
            if (list == null || list.Count < 1) return;

            BLL.Shop.Shipping.ShippingRegionGroups regionGroupManage = new BLL.Shop.Shipping.ShippingRegionGroups();
            regionGroupManage.SaveShippingRegionGroups(shippingType, list);
        }

        private List<ShippingRegionGroups> GetRegionGroups(
            Model.Shop.Shipping.ShippingType shippingType,
            string data)
        {
            List<ShippingRegionGroups> list = new List<ShippingRegionGroups>();
            Json.JsonArray jsonArray = null;
            try
            {
                jsonArray = Json.Conversion.JsonConvert.Import<Json.JsonArray>(data);
                if (jsonArray == null || jsonArray.Length < 1) return null;

                ShippingRegionGroups model;
                foreach (Json.JsonObject jsonObject in jsonArray)
                {
                    model = new ShippingRegionGroups();
                    model.ModeId = shippingType.ModeId;
                    model.RegionIds = ((Json.JsonArray)jsonObject["ids"]).Cast<string>().ToArray();
                    model.Price = Globals.SafeDecimal(jsonObject["price"], decimal.Zero);
                    model.AddPrice = Globals.SafeDecimal(jsonObject["addprice"], null);
                    list.Add(model);
                }
            }
            catch { }
            return list;
        }

        public void btnCancle_Click(object sender, EventArgs e)
        {
            Response.Redirect("ShippingType.aspx");
        }

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
            this.ddlSupplier.Items.Insert(0, new ListItem("请选择商家", "-1"));
            //this.ddlSupplier.Items.Insert(0, new ListItem("全　部", string.Empty));
            //this.ddlSupplier.Items.Insert(0, new ListItem(string.Empty, string.Empty));
            if (SupplierId != 0)
            {
                ddlSupplier.SelectedValue = SupplierId.ToString();
            }
            else
            {
                ddlSupplier.SelectedIndex = 0;
            }
        }
    }
}
