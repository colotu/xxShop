using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using YSWL.MALL.BLL.Shop.Supplier;
using YSWL.Common;
using YSWL.Json;

namespace YSWL.MALL.Web.Admin.Statistics
{
    public partial class SupplierList : PageBaseAdmin
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                BindSupplier();
            }
        }
        protected void btnReStatistic_Click(object sender, EventArgs e)
        {
            BLL.Shop.Supplier.SupplierInfo spBll=new BLL.Shop.Supplier.SupplierInfo();
            //string spName = this.txtSpName.Text;
            int selectedRegionId = Globals.SafeInt(ajaxRegion.SelectedValue, -1);
           // List<YSWL.MALL.Model.Shop.Supplier.SupplierInfo> supplierInfos = spBll.GetSpByRegion(selectedRegionId,
                    //                                                                             spName);
           // BindData();
        }

        protected JsonObject GetSpInfo(HttpContext context)
        {
            BLL.Shop.Supplier.SupplierInfo spBll = new BLL.Shop.Supplier.SupplierInfo();
            string spName = context.Request.Params["spName"];
            int regionId = Globals.SafeInt(context.Request.Params["regionId"], -1);
            List<YSWL.MALL.Model.Shop.Supplier.SupplierInfo> supplierInfos = spBll.GetSpByRegion(regionId,
                                                                                                 spName);
            JsonObject jsonResult = new JsonObject();
            if (null != supplierInfos)
            {

                List<JsonObject> jList = new List<JsonObject>();
                JsonObject jsonObject;
                foreach (var supplierInfo in supplierInfos)
                {
                    jsonObject = new JsonObject();
                    jsonObject.Put("name", supplierInfo.Name);
                    jsonObject.Put("shopName", supplierInfo.ShopName);
                    jsonObject.Put("phone", supplierInfo.CellPhone);
                    jsonObject.Put("logo", supplierInfo.LOGO);
                    jsonObject.Put("latitude", supplierInfo.Latitude);
                    jsonObject.Put("longitude", supplierInfo.Longitude);

                    jsonObject.Put("pointerTitle", supplierInfo.Name);//以下是标注信息
                    jsonObject.Put("pointImg", supplierInfo.LOGO);
                    jsonObject.Put("pointerContent", supplierInfo.ShopName);
                    jsonObject.Put("markersLongitude", supplierInfo.Longitude);
                    jsonObject.Put("markersDimension", supplierInfo.Latitude);

                    jList.Add(jsonObject);
                }
                jsonResult.Put("count", supplierInfos.Count);
                jsonResult.Put("spInfos", jList);
                return jsonResult;
            }
            jsonResult.Put("count", 0);
            return jsonResult;
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
            this.ddlSupplier.Items.Insert(0, new ListItem("全　部", string.Empty));
            this.ddlSupplier.Items.Insert(0, new ListItem(string.Empty, string.Empty));

        }

        public override void VerifyRenderingInServerForm(Control control)
        {

        }
        #endregion


    }
}