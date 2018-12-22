using System;
using System.Collections.Generic;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Text;
using YSWL.MALL.BLL.Shop.Coupon;
using YSWL.Common;
using YSWL.Json;
namespace YSWL.MALL.Web.Admin.Shop.ActivityInfo
{
    public partial class Add : PageBaseAdmin
    {
        YSWL.MALL.BLL.Shop.Activity.ActivityInfo bll = new YSWL.MALL.BLL.Shop.Activity.ActivityInfo();
        BLL.Shop.Activity.ActivityRule activityRuleBll = new BLL.Shop.Activity.ActivityRule();
        private YSWL.MALL.BLL.Shop.Products.CategoryInfo categoryInfoBll = new BLL.Shop.Products.CategoryInfo();
        private YSWL.MALL.BLL.Shop.Products.ProductInfo productInfoBll = new BLL.Shop.Products.ProductInfo();
        YSWL.MALL.BLL.Shop.Coupon.CouponRule ruleBll=new CouponRule();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                LoadBind();
                //ddlBuyProductId.Items.Insert(0, new ListItem("请选择", ""));
            }
            if (!string.IsNullOrWhiteSpace(this.Request.Form["Callback"]) && (this.Request.Form["Callback"] == "true"))
            {
                this.Controls.Clear();
                this.DoCallback();
            }
        }

        #region 加载数据
        public void LoadBind()
        {
            //规则
            this.ddlRule.DataSource = activityRuleBll.GetList(" Status=1");
            ddlRule.DataTextField = "RuleName";
            ddlRule.DataValueField = "RuleId";
            ddlRule.DataBind();
            ddlRule.Items.Insert(0, new ListItem("请选择", "0"));

            //加载赠送优惠券类型
            this.ddlCoupon.DataSource= ruleBll.GetList("Status=1 and Type=2 ");
            ddlCoupon.DataTextField = "Name";
            ddlCoupon.DataValueField = "RuleId";
            ddlCoupon.DataBind();
            ddlCoupon.Items.Insert(0, new ListItem("请选择", "0"));

            //赠品数据
            StringBuilder strWhere = new StringBuilder();
            strWhere.Append(" SalesType=3 ");
            if (!BLL.SysManage.ConfigSystem.GetBoolValueByCache("StoreIsInActivity"))
            {
                strWhere.AppendFormat(" and   SupplierId <=0");
            }
            ddlProductId.DataSource = productInfoBll.GetModelList(strWhere.ToString());
            ddlProductId.DataTextField = "ProductName";
            ddlProductId.DataValueField = "ProductId";
            ddlProductId.DataBind();
            this.ddlProductId.Items.Insert(0, new ListItem(string.Empty, string.Empty));
        }
        #endregion

        

        #region  保存
        protected void btnSave_Click(object sender, EventArgs e)
		{			
            int RuleId = Globals.SafeInt(ddlRule.SelectedValue, 0);
            if (RuleId <= 0)
            {
                MessageBox.ShowFailTip(this, "请选择规则");
                return;
            }
            int buyCategoryId =Common.Globals.SafeInt(ddlCateList.SelectedValue, 0);
            long? buyproductId = Globals.SafeLong(hidbuyproductId.Value, null);
            int buyCount = Globals.SafeInt(txtBuyCount.Text, 0);
            string BuySKU = "";
            if (buyCategoryId <= 0 || (buyproductId.HasValue && buyproductId.Value <= 0))
            {
                buyproductId = null;
            }

            if (buyCategoryId > 0 ||  buyproductId.HasValue && buyproductId.Value > 0)
            {
                BuySKU = lblBuySKU.InnerText.Trim();
                //if (BuySKU.Length == 0)
                //{
                //    MessageBox.ShowFailTip(this, "购买指定的商品没有sku,请重新选择再试");
                //    return;
                //}
                if (txtBuyCount.Text.Trim().Length == 0)
                {
                    MessageBox.ShowFailTip(this, "指定数量不能为空");
                    return;
                }  
                if (buyCount <= 0)
                {
                    MessageBox.ShowFailTip(this, "指定数量不能小于0");
                    return;
                }
            }
            long productId=Globals.SafeLong( ddlProductId.SelectedValue,0);
            int cpRuleId=Globals.SafeInt(ddlCoupon.SelectedValue,0);
            switch (RuleId)
            {
                case 4://包邮
                    break;
                case 3://赠优惠劵
                    if (cpRuleId <= 0)
                    {
                        MessageBox.ShowFailTip(this, "请选择赠送优惠劵");
                        return;
                    }
                    productId = 0;
                    break;
                case 2:
                case 1:
                default:
                    //赠商品
                    if (productId <= 0)
                    {
                        MessageBox.ShowFailTip(this, "请选择赠送商品");
                        return;
                    }
                    cpRuleId = 0;
                    //if (this.lblSKU.InnerText.Trim().Length == 0)
                    //{
                    //    MessageBox.ShowFailTip(this, "该赠送商品没有sku,请重新选择再试");
                    //    return;
                    //}
                    break;
            }
          
            string SKU = this.lblSKU.InnerText;
            decimal SalePrice = 0;
            int MaxCount = 0;

            decimal LimitPrice = Globals.SafeDecimal(txtLimitPrice.Text, 0);
            decimal? LimitMaxPrice = Globals.SafeDecimal(txtLimitMaxPrice.Text, null);
            if (ddlRule.SelectedValue == "2")
            {//当选择的规则为2时最低消费金额必填
                if (txtLimitPrice.Text.Trim().Length == 0)
                {
                    MessageBox.ShowFailTip(this, "最低消费金额不能为空");
                    return;
                }
                if (LimitPrice <= 0)
                {
                    MessageBox.ShowFailTip(this, "最低消费金额必须大于0");
                    return;
                }
            }
            if (LimitMaxPrice.HasValue && LimitMaxPrice.Value >= 0)
            {
                if (LimitMaxPrice.Value < LimitPrice)
                {
                    MessageBox.ShowFailTip(this, "最大消费金额必须大于最小消费金额");
                    return;
                }
            }
            else
            {
                LimitMaxPrice = null;
            }
            if (this.txtCount.Text.Trim().Length == 0)
            {
                MessageBox.ShowFailTip(this, "数量不能为空！");
                return;
            }
            int count = Globals.SafeInt(this.txtCount.Text, 0);
            if (RuleId == 4)//包邮没用
            {
                count = 0;
            }
            else
            {
                if (count <= 0)
                {
                    MessageBox.ShowFailTip(this, "数量格式不正确！");
                    return;
                }
            }
       
            int Status = checkboxStatus.Checked ? 1 : 0;

            if (txtStartDate.Text.Trim().Length == 0) {
                MessageBox.ShowFailTip(this, "请选择开始时间！");
                return;
            }       
            DateTime? StartDate = Globals.SafeDateTime(txtStartDate.Text, null);
            if(!StartDate.HasValue){
              MessageBox.ShowFailTip(this, "开始时间格式错误！");
                return;
            }
            if (txtEndDate.Text.Trim().Length == 0)
            {
                MessageBox.ShowFailTip(this, "请选择结束时间！");
                return;
            }
            DateTime? EndDate = Globals.SafeDateTime(txtEndDate.Text, null);
            if (!EndDate.HasValue)
            {
                MessageBox.ShowFailTip(this, "结束时间格式错误！");
                return;
            }
            if (EndDate<StartDate)
            {
                MessageBox.ShowFailTip(this, "结束时间必须大于开始时间！");
                return;
            }
            YSWL.MALL.Model.Shop.Activity.ActivityInfo model = new YSWL.MALL.Model.Shop.Activity.ActivityInfo();
			model.RuleId=RuleId;
            model.BuyCategoryId = buyCategoryId;
            model.BuyCategoryName = buyCategoryId > 0 ? categoryInfoBll.GetName(buyCategoryId) : "";
            model.BuyProductId = buyproductId;
            model.BuyProductName = buyproductId.HasValue ? productInfoBll.GetNameByCache(buyproductId.Value) : ""; 
            model.BuySKU = String.IsNullOrWhiteSpace(BuySKU) ? null : BuySKU;
            model.BuyCount = buyCount;
            model.ProductId = productId;
            model.ProductName = productId > 0 ? productInfoBll.GetNameByCache(productId) : ""; 
            model.CpRuleId = cpRuleId;
            model.CpRuleName = cpRuleId > 0 ? ruleBll.GetNameByCache(cpRuleId) : "";
			model.SKU=SKU;
			model.SalePrice=SalePrice;
			model.LimitPrice=LimitPrice;
            model.LimitMaxPrice = LimitMaxPrice;
            model.Count = count;
			model.MaxCount=MaxCount;
			model.Status=Status;
			model.StartDate=StartDate.Value;
			model.EndDate=EndDate.Value;
			model.CreatedUserId=CurrentUser.UserID;
			model.CreatedDate=DateTime.Now;
            if (bll.Add(model) > 0)
            {
                //清空对应的缓存数据
                YSWL.Common.DataCache.DeleteCache("GetAvailable_ActivityInfo");
                YSWL.Common.MessageBox.ShowSuccessTip(this, "保存成功！", "list.aspx");
            }
            else
            {
                YSWL.Common.MessageBox.ShowFailTip(this, "保存失败！");
            }
		}
        #endregion



        public void btnCancle_Click(object sender, EventArgs e)
        {
            Response.Redirect("list.aspx");
        }

        ////指定购买商品
        //protected void ddlBuyProductId_SelectedIndexChanged(object sender, EventArgs e)
        //{
        //   // Globals.SafeLong(ddlBuyProductId
        //}
        ////指定赠送商品
        //protected void ddlProductId_SelectedIndexChanged(object sender, EventArgs e)
        //{

        //}


        #region Ajax 方法
        private void DoCallback()
        {
            string action = this.Request.Form["Action"];
            this.Response.Clear();
            this.Response.ContentType = "application/json";
            string writeText = string.Empty;
            switch (action)
            {
                case "GetProductList":
                    writeText = GetProductList();
                    break;
                default:
                    break;

            }
            this.Response.Write(writeText);
            this.Response.End();
        }


        /// <summary>
        /// 获取分页
        /// </summary>
        /// <returns></returns>
        private string GetProductList()
        {
            JsonObject json = new JsonObject();
            JsonArray jsonArray = new JsonArray();
            JsonObject itemjson;
            int cId = Common.Globals.SafeInt(this.Request.Form["CId"], 0);
            itemjson = new JsonObject();
            itemjson.Put("Id", "");
            itemjson.Put("Name", "");
            jsonArray.Add(itemjson);
            if (cId <= 0)
            {
                json.Put("STATUS", "Success");
                json.Put("List", jsonArray);
                return json.ToString();
            }
            List<YSWL.MALL.Model.Shop.Products.ProductInfo> prodList = productInfoBll.GetProductsByCid(cId, BLL.SysManage.ConfigSystem.GetBoolValueByCache("StoreIsInActivity"));
           
            if (prodList != null)
            {
                foreach (YSWL.MALL.Model.Shop.Products.ProductInfo item in prodList)
                {
                    itemjson = new JsonObject();
                    itemjson.Put("Id", item.ProductId);
                    itemjson.Put("Name", item.ProductName);
                    jsonArray.Add(itemjson);
                }
            }
            json.Put("STATUS", "Success");
            json.Put("List", jsonArray);
            return json.ToString();
        }
        #endregion

        protected void ddlRule_SelectedIndexChanged(object sender, EventArgs e)
        {
            int ruleId = Common.Globals.SafeInt(ddlRule.SelectedValue, 0);
            switch (ruleId)
            {
                case 4:
                    trCount.Visible = false;
                    trCoupon.Visible = false;
                    trgift.Visible = false;
                    break;
                case 3:
                    trCount.Visible = true;
                    trCoupon.Visible = true;
                    trgift.Visible = false;
                    break;
                case 2:
                case 1:
                default:
                    trCount.Visible = true;
                    trgift.Visible = true;
                    trCoupon.Visible = false;
                    break;
            }
        }
    }
}
