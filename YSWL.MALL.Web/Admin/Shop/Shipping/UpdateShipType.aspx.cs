using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using YSWL.Common;
using YSWL.MALL.Model.Shop.Shipping;

namespace YSWL.MALL.Web.Admin.Shop.Shipping
{
    public partial class UpdateShipType : PageBaseAdmin
    {
        protected override int Act_PageLoad { get { return 522; } } //Shop_配送方式管理_编辑页
        private YSWL.MALL.BLL.Shop.Shipping.ShippingType typeBll = new BLL.Shop.Shipping.ShippingType();
        private YSWL.MALL.BLL.Shop.Shipping.ShippingPayment payBll = new BLL.Shop.Shipping.ShippingPayment();
        private BLL.Shop.Shipping.ShippingRegionGroups regionGroupManage = new BLL.Shop.Shipping.ShippingRegionGroups();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                ckPayType.DataSource = YSWL.Payment.BLL.PaymentModeManage.GetPaymentModes(YSWL.Payment.Model.DriveEnum.ALL);
                this.ckPayType.DataTextField = "Name";
                this.ckPayType.DataValueField = "ModeId";
                this.ckPayType.DataBind();
                BindData();


            }
        }


        #region 编号
        /// <summary>
        /// 可选项Id
        /// </summary>
        public int ModeId
        {
            get
            {
                int id = 0;
                string strid = Request.Params["id"];
                if (!string.IsNullOrWhiteSpace(strid))
                {
                    id = Globals.SafeInt(strid, 0);
                }
                return id;
            }
        }
        #endregion

        public void BindData()
        {
            
            YSWL.MALL.Model.Shop.Shipping.ShippingType typeModel = typeBll.GetModel(ModeId);
            if (typeModel != null)
            {

                BindSupplier(typeModel.SupplierId);
                this.tAddPrice.Text = typeModel.AddPrice.ToString();
                this.tAddWeight.Text =typeModel.AddWeight.ToString();
                this.tDesc.Text = typeModel.Description;
                this.tPrice.Text = typeModel.Price.ToString();
                this.tWeight.Text = typeModel.Weight.ToString();
                this.tAddWeight.Text = typeModel.AddWeight.ToString();
                this.tName.Text = typeModel.Name;

                ddlType.DataSource = YSWL.MALL.Web.Components.ExpressHelper.GetAllComType();
              
                this.ddlType.DataTextField = "ComName";
                this.ddlType.DataValueField = "ComEn";
                this.ddlType.DataBind();
                this.ddlType.Items.Insert(0, new ListItem("请选择", ""));
                if (ddlType.Items.FindByValue(typeModel.ExpressCompanyEn) != null)
                {
                    this.ddlType.SelectedValue = typeModel.ExpressCompanyEn;
                }
            }
            List<YSWL.MALL.Model.Shop.Shipping.ShippingPayment> payments =
                payBll.GetModelList(" ShippingModeId=" + ModeId);
            if (payments != null && payments.Count > 0)
            {
                
                for (int i = 0; i < this.ckPayType.Items.Count; i++)
                {
                    if (
                        payments.Select(c => c.PaymentModeId)
                                .Contains(Common.Globals.SafeInt(ckPayType.Items[i].Value, 0)))
                    {
                        ckPayType.Items[i].Selected = true;
                    }
                }
            }
            BLL.Ms.Regions regionManage = new BLL.Ms.Regions();
            List<Model.Ms.Regions> regionses = regionManage.GetProvinceList();
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
            //获取地区价格
            BLL.Shop.Shipping.ShippingRegionGroups shippingRegionManage = new BLL.Shop.Shipping.ShippingRegionGroups();
            List<ShippingRegionGroups> list = shippingRegionManage.GetShippingRegionGroups(ModeId);
            if (list == null) return;
            Json.JsonArray jsonArray = new Json.JsonArray();
            Json.JsonObject jsonObject = new Json.JsonObject();
            foreach (ShippingRegionGroups item in list)
            {
                jsonObject = new Json.JsonObject();
                jsonObject.Accumulate("ids", item.RegionIds);
                jsonObject.Accumulate("price", item.Price.ToString("F2"));
                if (item.AddPrice.HasValue)
                {
                    jsonObject.Accumulate("addprice", item.AddPrice.Value.ToString("F2"));
                }
                jsonArray.Add(jsonObject);
            }
            hfRegionData.Value = jsonArray.ToString();
        }

        public void btnSave_Click(object sender, System.EventArgs e)
        {
            Model.Shop.Shipping.ShippingType typeModel = typeBll.GetModel(ModeId);
            typeModel.AddPrice = Common.Globals.SafeDecimal(this.tAddPrice.Text, 0);
            typeModel.Price = Common.Globals.SafeDecimal(this.tPrice.Text, 0);
            typeModel.Weight = Common.Globals.SafeInt(this.tWeight.Text, 0);
            typeModel.AddWeight = Common.Globals.SafeInt(this.tAddWeight.Text, 0);
            typeModel.Description = this.tDesc.Text;
            typeModel.ExpressCompanyName = ddlType.SelectedItem.Text;
            typeModel.ExpressCompanyEn = ddlType.SelectedValue;
            typeModel.Name = this.tName.Text;
            typeModel.SupplierId = Common.Globals.SafeInt(this.ddlSupplier.SelectedValue,-1);
            if (typeBll.Update(typeModel))
            {
                //保存地区价格
                SaveShippingRegionGroups(typeModel);
                //首先删除选择的支付方式
                payBll.Delete(ModeId);
                for (int i = 0; i < this.ckPayType.Items.Count; i++)
                {
                    if (ckPayType.Items[i].Selected)
                    {
                        YSWL.MALL.Model.Shop.Shipping.ShippingPayment payModel = new ShippingPayment();
                        payModel.PaymentModeId = Common.Globals.SafeInt(ckPayType.Items[i].Value, 0);
                        payModel.ShippingModeId = ModeId;
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
            if (string.IsNullOrWhiteSpace(data))
            {
                regionGroupManage.ClearShippingRegionGroups(shippingType.ModeId);
                return;
            } 

            List<ShippingRegionGroups> list = GetRegionGroups(shippingType, data);
            if (list == null || list.Count < 1) return;
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





        /// <summary>
        /// 供应商
        /// </summary>
        private void BindSupplier(int supplierId)
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
            if (supplierId != 0)
            {
                ddlSupplier.SelectedValue = supplierId.ToString();
            }
            else
            {
                ddlSupplier.SelectedIndex = 0;
            }
        }
    }
}
