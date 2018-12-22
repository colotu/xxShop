using System;
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
using YSWL.Common;
namespace YSWL.MALL.Web.Admin.Shop.ActivityInfo
{
    public partial class Modify : PageBaseAdmin
    {
        YSWL.MALL.BLL.Shop.Activity.ActivityInfo bll = new YSWL.MALL.BLL.Shop.Activity.ActivityInfo();
        BLL.Shop.Activity.ActivityRule activityRuleBll = new BLL.Shop.Activity.ActivityRule();
        private YSWL.MALL.BLL.Shop.Products.CategoryInfo categoryInfoBll = new BLL.Shop.Products.CategoryInfo();
        YSWL.MALL.BLL.Shop.Coupon.CouponRule ruleBll = new BLL.Shop.Coupon.CouponRule();
        private YSWL.MALL.BLL.Shop.Products.ProductInfo productInfoBll = new BLL.Shop.Products.ProductInfo();
        BLL.Members.Users usersBll = new BLL.Members.Users();
        		protected void Page_Load(object sender, EventArgs e)
		{
			if (!Page.IsPostBack)
			{
                LoadBind();
                ShowInfo();
			}
		}
        private int ActivityId
                {
                    get
                    {
                        int id = 0;
                        if (!string.IsNullOrWhiteSpace(Request.Params["id"]))
                        {
                            id = Globals.SafeInt(Request.Params["id"], 0);
                        }
                        return id;
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
                    this.ddlCoupon.DataSource = ruleBll.GetList("Status=1 and Type=2 ");
                    ddlCoupon.DataTextField = "Name";
                    ddlCoupon.DataValueField = "RuleId";
                    ddlCoupon.DataBind();
                    ddlCoupon.Items.Insert(0, new ListItem("请选择", "0"));
                }
                #endregion
         

	private void ShowInfo()
	{
        YSWL.MALL.Model.Shop.Activity.ActivityInfo model = bll.GetModel(ActivityId);
        if (model != null)
        {
            this.lblActivityId.Text = model.ActivityId.ToString();
            if (ddlRule.Items.FindByValue(model.RuleId.ToString()) != null)
            {
                this.ddlRule.SelectedValue =  model.RuleId.ToString();
            }

            switch (model.RuleId)
            {
                case 4:
                    trCount.Visible = false;
                    break;
                case 3:
                     trCoupon.Visible = true;
                if (ddlCoupon.Items.FindByValue(model.CpRuleId.ToString()) != null)
                {
                    this.ddlCoupon.SelectedValue = model.CpRuleId.ToString();
                }
                    break;
                case 2:
                case 1:
                default:
                    trgift.Visible = true;
                    break;
            }
           

            this.hidbuyproductId.Value = model.BuyProductId.HasValue ? model.BuyProductId.Value.ToString():"0";
            this.lblBuySKU.InnerText = model.BuySKU;
            this.txtBuyCount.Text = model.BuyCount.ToString();
            this.lblProductName.Text = model.ProductName;
            this.lblSKU.InnerText = model.SKU;
            this.txtSalePrice.Text = model.SalePrice.ToString();
            this.txtLimitPrice.Text = model.LimitPrice.ToString("F");
            this.txtLimitMaxPrice.Text = model.LimitMaxPrice.HasValue ? model.LimitMaxPrice.Value.ToString("F") : "";
            this.txtCount.Text = model.Count.ToString();
            this.txtMaxCount.Text = model.MaxCount.ToString();
            this.checkboxStatus.Checked=model.Status==1?true:false;
            this.txtStartDate.Text = model.StartDate.ToString("yyyy-MM-dd");
            this.txtEndDate.Text = model.EndDate.ToString("yyyy-MM-dd");
            this.lblCreatedUserId.InnerText =usersBll.GetUserName(model.CreatedUserId);
            this.lblCreatedDate.InnerText = model.CreatedDate.ToString("yyyy-MM-dd HH:mm:ss");

            ddlCateList.SelectedValue = model.BuyCategoryId.ToString();
        }
        else
        {
            MessageBox.ShowAndRedirect(this, "该信息不存在或已被删除！", "list.aspx");
            return;
        }

	
	}

		public void btnSave_Click(object sender, EventArgs e)
		{
            int RuleId = Globals.SafeInt(ddlRule.SelectedValue, 0);
            if (RuleId <= 0)
            {
                MessageBox.ShowFailTip(this, "请选择规则");
                return;
            }
            int buyCategoryId = Common.Globals.SafeInt(ddlCateList.SelectedValue, 0);
            long? buyproductId = Globals.SafeLong(hidbuyproductId.Value, null);
            int buyCount =  Globals.SafeInt(txtBuyCount.Text, 0);
            string BuySKU = "";
            if (buyCategoryId <= 0 || (buyproductId.HasValue && buyproductId.Value<=0))
            {
                buyproductId = null;
            }

            if (buyCategoryId>0 ||  buyproductId.HasValue && buyproductId.Value > 0)
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

           // long productId = Globals.SafeLong(ddlProductId.SelectedValue, 0);
            int cpRuleId = Globals.SafeInt(ddlCoupon.SelectedValue, 0);
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
                    break;
                //case 2:
                //case 1:
                //default:
                //    //赠商品
                //    if (productId <= 0)
                //    {
                //        MessageBox.ShowFailTip(this, "请选择赠送商品");
                //        return;
                //    }
                //    cpRuleId = 0;
                //    //if (this.lblSKU.InnerText.Trim().Length == 0)
                //    //{
                //    //    MessageBox.ShowFailTip(this, "该赠送商品没有sku,请重新选择再试");
                //    //    return;
                //    //}
                //    break;
            }

            string SKU = this.lblSKU.InnerText;
            decimal SalePrice = 0;
            int MaxCount = 0;

            decimal LimitPrice =Globals.SafeDecimal(txtLimitPrice.Text, 0);
            decimal? LimitMaxPrice = Globals.SafeDecimal(txtLimitMaxPrice.Text, null);

            if (ddlRule.SelectedValue == "2")
            {
                //当选择的规则为2时最低消费金额必填
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

            if (txtStartDate.Text.Trim().Length == 0)
            {
                MessageBox.ShowFailTip(this, "请选择开始时间！");
                return;
            }
            DateTime? StartDate = Globals.SafeDateTime(txtStartDate.Text, null);
            if (!StartDate.HasValue)
            {
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
            if (EndDate < StartDate)
            {
                MessageBox.ShowFailTip(this, "结束时间必须大于开始时间！");
                return;
            }
            YSWL.MALL.Model.Shop.Activity.ActivityInfo model = bll.GetModelByCache(ActivityId);
            if (model != null)
            {
                model.RuleId = RuleId;
                model.BuyCategoryId = buyCategoryId;
                model.BuyCategoryName = buyCategoryId > 0 ? categoryInfoBll.GetName(buyCategoryId) : "";
                model.BuyProductId = buyproductId;
                model.BuyProductName = buyproductId.HasValue ? productInfoBll.GetNameByCache(buyproductId.Value) : ""; 
                model.BuySKU = String.IsNullOrWhiteSpace(BuySKU) ? null : BuySKU;
                model.BuyCount = buyCount;
                model.SKU = SKU;
                model.SalePrice = SalePrice;
                model.LimitPrice = LimitPrice;
                model.LimitMaxPrice = LimitMaxPrice;
                model.Count = count;
                model.MaxCount = MaxCount;
                model.Status = Status;
                model.StartDate = StartDate.Value;
                model.EndDate = EndDate.Value;

                model.CpRuleId = cpRuleId;
                model.CpRuleName = cpRuleId > 0 ? ruleBll.GetNameByCache(cpRuleId) : "";
                if (bll.Update(model))
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
            else {
                MessageBox.ShowAndRedirect(this, "该信息不存在或已被删除！", "list.aspx");
                return;
            }
     
		}


        public void btnCancle_Click(object sender, EventArgs e)
        {
            Response.Redirect("list.aspx");
        }
    }
}
