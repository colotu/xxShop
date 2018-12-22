using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using YSWL.MALL.BLL.Shop.Coupon;
using YSWL.MALL.BLL.Shop.Supplier;
using YSWL.Json;

namespace YSWL.MALL.Web.Admin.Shop.Coupon
{
    public partial class AddRule : PageBaseAdmin
    {

        protected override int Act_PageLoad { get { return 416; } } //Shop_优惠券规则管理_新增页
        private  YSWL.MALL.BLL.Shop.Coupon.CouponClass classBll=new CouponClass();
        private  YSWL.MALL.BLL.Shop.Coupon.CouponRule ruleBll=new CouponRule();
        private  YSWL.MALL.BLL.Shop.Supplier.SupplierInfo supplierBll=new SupplierInfo();
        private YSWL.MALL.BLL.Shop.Products.ProductInfo productInfoBll = new BLL.Shop.Products.ProductInfo();
       
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                //获取是否隐藏分类和商家
                this.hfCategory.Value = BLL.SysManage.ConfigSystem.GetValueByCache("Shop_CouponRule_IsCategory");
                hfSupplier.Value = BLL.SysManage.ConfigSystem.GetValueByCache("Shop_CouponRule_IsSupplier");

                //获取优惠券分类
                this.ddlClass.DataSource = classBll.GetList(" Status=1");
                ddlClass.DataTextField = "Name";
                ddlClass.DataValueField = "ClassId";
                ddlClass.DataBind();
                ddlClass.Items.Insert(0,new ListItem("请选择","0"));
                //获取商家
                this.ddlSupplier.DataSource = supplierBll.GetList("Status=1");
                ddlSupplier.DataTextField = "Name";
                ddlSupplier.DataValueField = "SupplierId";
                ddlSupplier.DataBind();
                ddlSupplier.Items.Insert(0, new ListItem("请选择", "0"));


                if (!string.IsNullOrWhiteSpace(this.Request.Form["Callback"]) && (this.Request.Form["Callback"] == "true"))
                {
                    this.Controls.Clear();
                    this.DoCallback();
                }

                

            }
        }
        protected void btnSave_Click(object sender, EventArgs e)
        {
          YSWL.MALL.Model.Shop.Coupon.CouponRule ruleModel=new Model.Shop.Coupon.CouponRule();
            string name = txtName.Text;
            int type = Common.Globals.SafeInt(rblType.SelectedValue, 0);
            if (String.IsNullOrWhiteSpace(name))
            {
                Common.MessageBox.ShowFailTip(this,"请填写优惠券名称");
                return;
            }
            decimal limitPrice = Common.Globals.SafeDecimal(txtLimitPrice.Text, -1);
            if (limitPrice == -1)
            {
                Common.MessageBox.ShowFailTip(this, "请填写最低消费金额");
                return;
            }
            decimal price = Common.Globals.SafeDecimal(txtPrice.Text, -1);
            if (price==-1)
            {
                Common.MessageBox.ShowFailTip(this, "请填写消费券面值");
                return;
            }
            if ((String.IsNullOrWhiteSpace(txtStartDate.Text) || String.IsNullOrWhiteSpace(txtEndDate.Text)) && !chkNoDate.Checked && type!=2)
            {
                Common.MessageBox.ShowFailTip(this, "请选择优惠券使用时间");
                return;
            }
            int count = Common.Globals.SafeInt(txtSendCount.Text,0);
         
            if (count == 0 && type==0)
            {
                Common.MessageBox.ShowFailTip(this, "请填写生成数量");
                return;
            }
            int point = Common.Globals.SafeInt(txtPoint.Text, 0);
            if (point == 0 && type==1)
            {
                Common.MessageBox.ShowFailTip(this, "请填写兑换所需积分");
                return;
            }

            #region 商品Id
            ruleModel.CategoryId = Common.Globals.SafeInt(ddlCateList.SelectedValue, 0);
            ruleModel.ProductId = Common.Globals.SafeLong(hidproductId.Value, 0);
            #endregion

            ruleModel.Name = name;
            ruleModel.NeedPoint = type==1 ? point : 0;
            ruleModel.IsPwd = chkIsPwd.Checked ? 1 : 0;
            ruleModel.IsReuse = chkIsReuse.Checked ? 1 : 0;
            ruleModel.LimitPrice = limitPrice;
            ruleModel.PreName =Common.InjectionFilter.SqlFilter(txtPreName.Text);
            ruleModel.SendCount = type==0 ?  count:0;
            ruleModel.StartDate =chkNoDate.Checked?DateTime.Now: Common.Globals.SafeDateTime(txtStartDate.Text, DateTime.Now);
            ruleModel.Status = chkStatus.Checked ? 1 : 0;
            ruleModel.SupplierId = Common.Globals.SafeInt(ddlSupplier.SelectedValue, 0);
            ruleModel.CategoryId = Common.Globals.SafeInt(ddlCateList.SelectedValue, 0);
            ruleModel.ClassId = Common.Globals.SafeInt(ddlClass.SelectedValue, 0);
            ruleModel.CouponPrice = price;
            ruleModel.CreateDate = DateTime.Now;
            ruleModel.CreateUserId = CurrentUser.UserID;
            ruleModel.Type = type;
            ruleModel.EndDate = chkNoDate.Checked ? DateTime.MaxValue : Common.Globals.SafeDateTime(txtEndDate.Text, DateTime.MaxValue);
            ruleModel.EndDate =ruleModel.EndDate==DateTime.MaxValue? DateTime.MaxValue: ruleModel.EndDate.AddDays(1).AddSeconds(-1);
            ruleModel.CpLength = Common.Globals.SafeInt(ddlLength.SelectedValue, 0);
            ruleModel.PwdLength = Common.Globals.SafeInt(ddlPwd.SelectedValue, 0);
            ruleModel.DeferDay = Common.Globals.SafeInt(txtDeferDay.Text, 0);
            ruleModel.AvaType = chkAvaType.Checked ? 1 : 0;
            if (ruleBll.AddEx(ruleModel))
            {
                Common.MessageBox.ShowSuccessTip(this, "生成优惠券成功", "CouponList.aspx");
            }
            else
            {
                Common.MessageBox.ShowFailTip(this, "生成优惠券失败");
            }
        }
 
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
        /// 获取装箱单分页
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
            if (cId <= 0) {
                json.Put("STATUS", "Success");
                json.Put("List", jsonArray);
                return json.ToString();
            }
            List<YSWL.MALL.Model.Shop.Products.ProductInfo> prodList = productInfoBll.GetProductsByCid(cId);
           if (prodList != null)
           {
               foreach (YSWL.MALL.Model.Shop.Products.ProductInfo item in prodList) 
               {
                   itemjson = new JsonObject();
                   itemjson.Put("Id",item.ProductId);
                   itemjson.Put("Name",item.ProductName);
                   jsonArray.Add(itemjson);
               }
           }
           json.Put("STATUS", "Success");
           json.Put("List", jsonArray);      
           return json.ToString();
        }
        #endregion

    }
}