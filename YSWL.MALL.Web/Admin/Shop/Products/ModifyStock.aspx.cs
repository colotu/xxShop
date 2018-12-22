using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.IO;
using System.Text;
using System.Web.UI;
using System.Web.UI.WebControls;
using YSWL.MALL.BLL.Shop.Products;
using YSWL.Common;
using YSWL.MALL.Model.Settings;
using YSWL.MALL.Model.Shop.Products;
using AttributeValue = YSWL.MALL.Model.Shop.Products.AttributeValue;
using ProductAccessorie = YSWL.MALL.Model.Shop.Products.ProductAccessorie;
using ProductImage = YSWL.MALL.Model.Shop.Products.ProductImage;

namespace YSWL.MALL.Web.Admin.Shop.Products
{
    public partial class ModifyStock : PageBaseAdmin
    {
        protected override int Act_PageLoad { get { return 486; } } //Shop_商品管理_编辑页
        private YSWL.MALL.BLL.Shop.Products.SKUInfo skuBll = new YSWL.MALL.BLL.Shop.Products.SKUInfo();
        BLL.Shop.Products.ProductInfo prodBll = new BLL.Shop.Products.ProductInfo();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                BindProductBaseInfo();
                //获取是否开启相关选项卡
                this.hfIsOpenSku.Value = YSWL.MALL.BLL.SysManage.ConfigSystem.GetValueByCache("Shop_AddProduct_OpenSku");
            }
        }

        public long ProductId
        {
            get
            {
                long pid = 0;
                if (!string.IsNullOrWhiteSpace(Request.Params["pid"]))
                {
                    pid = Globals.SafeLong(Request.Params["pid"], 0);
                }
                return pid;
            }
        }

        private void BindProductBaseInfo()
        {
            BLL.Shop.Products.ProductInfo manage = new BLL.Shop.Products.ProductInfo();
            Model.Shop.Products.ProductInfo info = manage.GetModel(ProductId);
            if (info == null) return;
            hidHasSku.Value= info.HasSKU.ToString();
         
            litProductName.Text = info.ProductName;
            List <Model.Shop.Products.SKUInfo> skuList = skuBll.GetProductSkuInfo(info.ProductId);
            List<Model.Shop.Products.SKUItem> listSkuItems;
            string skuValues;
            if (skuList== null) {
                return;
            }
            Json.JsonArray array = new Json.JsonArray();
            Json.JsonObject json;
            foreach (var item in skuList)
            {
                skuValues = "";
                listSkuItems = skuBll.GetSKUItemsBySkuId(item.SkuId);
                if (listSkuItems != null && listSkuItems.Count > 0)
                {
                    listSkuItems.ForEach(xx =>
                    {
                        skuValues += xx.ValueStr;
                    });
                }
                json = new Json.JsonObject();
                json.Put("sku", item.SKU);
                json.Put("Stock", item.Stock);
                json.Put("skuValues", skuValues);
                array.Add(json);
            }      
            hidSkuJson.Value = array.ToString();
        }

        protected void btnSave_OnClick(object sender, EventArgs e)
        {
            SaveProductInfo();
        }

        private void SaveProductInfo()
        { 

            BLL.Shop.Products.ProductInfo manage = new BLL.Shop.Products.ProductInfo();
            Model.Shop.Products.ProductInfo productInfo = manage.GetModel(ProductId);// new Model.Shop.Products.ProductInfo();
            string SkuInfo = this.Hidden_TempSKUInfo.Value;
            List<Model.Shop.Products.SKUInfo> skuList = GetSKUInfo4Json(LitJson.JsonMapper.ToObject(SkuInfo));


            if (YSWL.MALL.BLL.Shop.Products.ProductManage.ModifyStock(skuList))
            {
                prodBll.ClearCache(ProductId);
                MessageBox.ShowSuccessTipScript(this, "操作成功", " window.parent.location.reload();");
            }
            else
            {
                MessageBox.Show(this, "保存失败! 请重试.");
                return;
            }
        }




        #region Json处理

       

        #region SKU

        private List<Model.Shop.Products.SKUInfo> GetSKUInfo4Json(LitJson.JsonData jsonData)
        {
            List<Model.Shop.Products.SKUInfo> list = new List<Model.Shop.Products.SKUInfo>();
            if (jsonData.IsArray && jsonData.Count > 0)
            {
                //开启SKU时
                foreach (LitJson.JsonData item in jsonData)
                {
                    list.Add(GetSKUInfo4Obj(item));
                }
            }
            else if (jsonData.IsObject)
            {
                //关闭SKU时
                list.Add(GetSKUInfo4Obj(jsonData));
            }
            return list;
        }

        private Model.Shop.Products.SKUInfo GetSKUInfo4Obj(LitJson.JsonData jsonData)
        {
            Model.Shop.Products.SKUInfo skuInfo = null;
            if (!jsonData.IsObject) return null;

            skuInfo = new Model.Shop.Products.SKUInfo();

            //Base Info
            skuInfo.SKU = jsonData["SKU"].ToString();
            skuInfo.Stock = Globals.SafeInt(jsonData["Stock"].ToString(), -1);
 
            return skuInfo;
        }

        #endregion SKU

        #endregion Json处理
 
    }
}