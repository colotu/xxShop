using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using YSWL.Accounts.Bus;
using YSWL.MALL.BLL.Shop.Coupon;
using YSWL.MALL.BLL.Shop.Products;
using YSWL.MALL.BLL.Shop.Supplier;
using YSWL.Common;
using System.Web.UI.HtmlControls;

namespace YSWL.MALL.Web.Admin.Shop.Coupon
{
    public partial class RuleList : PageBaseAdmin
    {
        protected override int Act_PageLoad { get { return 415; } } //Shop_优惠券规则管理_列表页
        protected new int Act_AddData =417;    //Shop_优惠券规则管理_新增数据

        private YSWL.MALL.BLL.Shop.Coupon.CouponRule ruleBll = new CouponRule();
         private YSWL.MALL.BLL.Shop.Coupon.CouponClass classBll = new CouponClass();
        private  YSWL.MALL.BLL.Shop.Coupon.CouponInfo infoBll=new CouponInfo();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
               
           
            }
        }

        #region gridView

        public void BindData()
        {

            StringBuilder whereStr = new StringBuilder();
            string keyword = this.txtKeyword.Text;
            if (!String.IsNullOrWhiteSpace(keyword))
            {
                whereStr.AppendFormat(" Name Like '%{0}%'", Common.InjectionFilter.SqlFilter(keyword));
            }
            DataSet ds = ruleBll.GetList(whereStr.ToString());
            if (ds != null)
            {
                gridView.DataSetSource = ds;
            }
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
                #region 类型为积分兑换的不显示增发按钮 ，其他类型显示增发按钮
                HtmlGenericControl addSendbtn = (HtmlGenericControl)e.Row.FindControl("AddSend");
                object obj1 = DataBinder.Eval(e.Row.DataItem, "Type");
                if ((obj1 != null) && ((obj1.ToString() != "")))
                {
                    if (obj1.ToString() != "1")
                    {
                        addSendbtn.Visible = true;
                    }
                }
                #endregion

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
        protected void gridView_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            int ruleId = (int)gridView.DataKeys[e.RowIndex].Value;
            ruleBll.Delete(ruleId);
            gridView.OnBind();
        }
        protected void gridView_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "DeleteCoupon")
            {
                if (e.CommandArgument != null)
                {
                    int ruleId = Common.Globals.SafeInt(e.CommandArgument.ToString(),0);
                    if (infoBll.DeleteEx(ruleId))
                    {
                        MessageBox.ShowSuccessTip(this, "操作成功！");
                        gridView.OnBind();
                    }
                }
            }
        }
        

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            gridView.OnBind();
        }
        /// <summary>
        /// 商品分类名称
        /// </summary>
        /// <param name="target"></param>
        /// <returns></returns>
        public string GetCategoryName(object target)
        {
            string str = string.Empty;
            if (!StringPlus.IsNullOrEmpty(target))
            {
                int categoryId = Common.Globals.SafeInt(target, 0);
                YSWL.MALL.BLL.Shop.Products.CategoryInfo cateBll = new CategoryInfo();
                YSWL.MALL.Model.Shop.Products.CategoryInfo categoryModel = cateBll.GetModel(categoryId);
                str = categoryModel == null ? "" : categoryModel.Name;
            }
            return str;
        }
        /// <summary>
        /// 优惠券分类名称
        /// </summary>
        /// <param name="target"></param>
        /// <returns></returns>
        public string GetClassName(object target)
        {
            string str = string.Empty;
            if (!StringPlus.IsNullOrEmpty(target))
            {
                int classId = Common.Globals.SafeInt(target, 0);
                YSWL.MALL.Model.Shop.Coupon.CouponClass classModel = classBll.GetModel(classId);
                str = classModel == null ? "" : classModel.Name;
            }
            return str;
        }
        /// <summary>
        /// 获取商家名称
        /// </summary>
        /// <param name="target"></param>
        /// <returns></returns>
        public string GetSupplierName(object target)
        {
            string str = string.Empty;
            if (!StringPlus.IsNullOrEmpty(target))
            {
                int supplierId = Common.Globals.SafeInt(target, 0);
                YSWL.MALL.BLL.Shop.Supplier.SupplierInfo supplierBll = new SupplierInfo();
                YSWL.MALL.Model.Shop.Supplier.SupplierInfo supplierModel = supplierBll.GetModel(supplierId);
                str = supplierModel == null ? "" : supplierModel.Name;
            }
            return str;
        }

        /// <summary>
        /// 获取用户名称
        /// </summary>
        /// <param name="target"></param>
        /// <returns></returns>
        public string GetUserName(object target)
        {
            string str = string.Empty;
            if (!StringPlus.IsNullOrEmpty(target))
            {
                int userId = Common.Globals.SafeInt(target, 0);
                YSWL.Accounts.Bus.User userModel = new User(userId);
                str = userModel == null ? "" : userModel.NickName;
            }
            return str;
        }

        /// <summary>
        /// 获取状态
        /// </summary>
        /// <param name="target"></param>
        /// <returns></returns>
        public string GeStatusName(object target)
        {
            string str = string.Empty;
            if (!StringPlus.IsNullOrEmpty(target))
            {
                int status = Common.Globals.SafeInt(target, 0);
                switch (status)
                {
                    case 0:
                        str = "不启用";
                        break;
                    case 1:
                        str = "启用";
                        break;
                    default:
                        break;
                }
            }
            return str;
        }


        #endregion
    }
}