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
using YSWL.MALL.BLL.Shop.Order;
using YSWL.MALL.BLL.Shop.Products;
using YSWL.MALL.BLL.Shop.Supplier;
using YSWL.Common;
using YSWL.MALL.Model.Shop.Order;

namespace YSWL.MALL.Web.Admin.Shop.Coupon
{
    public partial class CouponList : PageBaseAdmin
    {
        protected override int Act_PageLoad { get { return 418; } } //Shop_优惠券管理_列表页
        protected new int Act_AddData = 417;    //Shop_优惠券规则管理_新增数据
        protected new int Act_DelData = 419;    //Shop_优惠券管理_删除数据
        private YSWL.MALL.BLL.Shop.Coupon.CouponInfo couponBll = new CouponInfo();
        private YSWL.MALL.BLL.Shop.Coupon.CouponClass classBll = new CouponClass();
        private  YSWL.MALL.BLL.Shop.Coupon.CouponRule ruleBll=new CouponRule();
        private  YSWL.MALL.BLL.Shop.Order.Orders orderBll=new Orders();
        private YSWL.MALL.BLL.Shop.Products.ProductInfo prodBll = new ProductInfo();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                if (!UserPrincipal.HasPermissionID(GetPermidByActID(Act_AddData)) && GetPermidByActID(Act_AddData) != -1)
                {
                    liAdd.Visible = false;
                }
                if (!UserPrincipal.HasPermissionID(GetPermidByActID(Act_DelData)) && GetPermidByActID(Act_DelData) != -1)
                {
                    btnDelete.Visible = false;
                }
        

                //获取优惠券分类
                ddlClass.DataSource = classBll.GetList("Status=1");
                ddlClass.DataTextField = "Name";
                ddlClass.DataValueField = "ClassId";
                ddlClass.DataBind();
                ddlClass.Items.Insert(0,new ListItem("请选择","0"));

                //获取规则名称
                ddlRule.DataSource = ruleBll.GetList("Status=1");
                ddlRule.DataTextField = "Name";
                ddlRule.DataValueField = "RuleId";
                ddlRule.DataBind();
                ddlRule.Items.Insert(0, new ListItem("请选择", "0"));
            }
        }

        #region gridView

        public void BindData()
        {
            StringBuilder whereStr = new StringBuilder();
            int classId = Common.Globals.SafeInt(ddlClass.SelectedValue, 0);
            int ruleId = Common.Globals.SafeInt(ddlRule.SelectedValue, 0);
            int status = Common.Globals.SafeInt(ddlStatus.SelectedValue, -2);
            string startDate = txtStartDate.Text;
            string endDate = txtEndDate.Text;
            //状态
            if (status>-2)
            {
                whereStr.AppendFormat(" Status ={0}", status);
            }
            //分类
            if (classId > 0)
            {
                if (!String.IsNullOrWhiteSpace(whereStr.ToString()))
                {
                    whereStr.Append(" and ");
                }
                whereStr.AppendFormat(" ClassId ={0}", classId);
            }
            //规则
            if (ruleId > 0)
            {
                if (!String.IsNullOrWhiteSpace(whereStr.ToString()))
                {
                    whereStr.Append(" and ");
                }
                whereStr.AppendFormat(" RuleId ={0}", ruleId);
            }
            //开始时间
            if (!String.IsNullOrWhiteSpace(endDate))
            {
                if (!String.IsNullOrWhiteSpace(whereStr.ToString()))
                {
                    whereStr.Append(" and ");
                }
                whereStr.AppendFormat(" EndDate <='{0}'", endDate);
            }
            //结束时间
            if (!String.IsNullOrWhiteSpace(startDate))
            {
                if (!String.IsNullOrWhiteSpace(whereStr.ToString()))
                {
                    whereStr.Append(" and ");
                }
                whereStr.AppendFormat(" StartDate >='{0}'", startDate);
            }
            string keyword = this.txtKeyword.Text;
            if (!String.IsNullOrWhiteSpace(keyword))
            {
                if (!String.IsNullOrWhiteSpace(whereStr.ToString()))
                {
                    whereStr.Append(" and ");
                }
                whereStr.AppendFormat(" CouponCode Like '%{0}%'", Common.InjectionFilter.SqlFilter(keyword));
            }
            string orderCode = InjectionFilter.SqlFilter(this.txtOrderCode.Text.Trim());
            if (!String.IsNullOrWhiteSpace(orderCode))
            {
                if (!String.IsNullOrWhiteSpace(whereStr.ToString()))
                {
                    whereStr.Append(" and ");
                }
                whereStr.AppendFormat(" OrderCode Like '%{0}%'", Common.InjectionFilter.SqlFilter(orderCode));
            }
            DataSet ds = couponBll.GetList(0, whereStr.ToString(), " GenerateTime desc ");
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

        }

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            gridView.Columns[2].Visible = chkRule.Checked;
            gridView.Columns[8].Visible = chkCategory.Checked;
            gridView.Columns[9].Visible = chkProductId.Checked;
            gridView.Columns[4].Visible = chkSupplier.Checked;
            gridView.Columns[10].Visible = chkUser.Checked;
            gridView.Columns[11].Visible = chkOrder.Checked;
            gridView.Columns[12].Visible = chkzengOrder.Checked;
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
        /// 商品名称
        /// </summary>
        /// <param name="target"></param>
        /// <returns></returns>
        public string GetProdName(object target)
        {
            string str = string.Empty;
            if (!StringPlus.IsNullOrEmpty(target))
            {
               long pId = Common.Globals.SafeLong(target, 0); 
               return prodBll.GetProductName(pId);
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

        public string GetOrderCode(object target)
        {
            string str = string.Empty;
            if (!StringPlus.IsNullOrEmpty(target))
            {
                string coupon = target.ToString();
                YSWL.MALL.Model.Shop.Order.OrderInfo orderInfo= orderBll.GetOrderByCoupon(coupon);
                str = orderInfo == null ? "" : orderInfo.OrderCode;
            }
            return str;
        }

        /// <summary>
        /// 获取状态
        /// </summary>
        /// <param name="target"></param>
        /// <returns></returns>
        public string GetStatusName(object target)
        {
            return couponBll.GetStatusName(Globals.SafeInt(target, 0));
        }
        /// <summary>
        /// 删除操作
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnDelete_Click(object sender, EventArgs e)
        {
            string idlist = GetSelIDlist();
            if (idlist.Trim().Length == 0)
                return;
            if (couponBll.DeleteList(idlist))
            {
                MessageBox.ShowSuccessTip(this, "操作成功！");
            }
            else
            {
                YSWL.Common.MessageBox.ShowFailTip(this, Resources.Site.TooltipDelError);
            }
            gridView.OnBind();

        }


        protected void ddlAction_Changed(object sender, EventArgs e)
        {
            string idlist = GetSelIDlist();
            if (idlist.Trim().Length == 0)
                return;
            int status = Common.Globals.SafeInt(ddlAction.SelectedValue, -1);
            if(status==-1)
                return;
            if (couponBll.UpdateStatusList(idlist, status))
            {
                MessageBox.ShowSuccessTip(this, "操作成功！");
            }
            else
            {
                YSWL.Common.MessageBox.ShowFailTip(this, "操作失败");
            }
            gridView.OnBind();

        }

        protected void btnMove_Click(object sender, EventArgs e)
        {
            bool isNotData;
            if (couponBll.MoveHistory(out isNotData))
            {
                if (isNotData)
                {
                    MessageBox.ShowFailTip(this,"没有需要转移的数据");
                }
                else
                {
                    MessageBox.ShowSuccessTip(this, "操作成功！");
                }
            }
            else
            {
                YSWL.Common.MessageBox.ShowFailTip(this, Resources.Site.TooltipDelError);
            }
            gridView.OnBind();

        }
        

        private string GetSelIDlist()
        {
            string idlist = "";
            bool BxsChkd = false;
            for (int i = 0; i < gridView.Rows.Count; i++)
            {
                CheckBox ChkBxItem = (CheckBox)gridView.Rows[i].FindControl(gridView.CheckBoxID);
                if (ChkBxItem != null && ChkBxItem.Checked)
                {
                    BxsChkd = true;
                    //#warning 代码生成警告：请检查确认Cells的列索引是否正确
                    if (gridView.DataKeys[i].Value != null)
                    {
                        //idlist += gridView.Rows[i].Cells[1].Text + ",";
                        idlist +="'"+ gridView.DataKeys[i].Value.ToString() + "',";
                    }
                }
            }
            if (BxsChkd)
            {
                idlist = idlist.Substring(0, idlist.LastIndexOf(","));
            }
            return idlist;
        }
        #endregion


        protected void txtUser_Change(object sender, System.EventArgs e)
        {
            string value=this.txtUser.Text;
            int userId = Common.Globals.SafeInt(value, 0);
            YSWL.MALL.BLL.Members.Users userBll = new BLL.Members.Users();
            YSWL.MALL.BLL.Members.UserCard cardBll = new BLL.Members.UserCard();
            //是否存在
            if (!userBll.Exists(userId))
            {
                 //不存在就在会员卡中取
               YSWL.MALL.Model.Members.UserCard cardModel=  cardBll.GetModel(value);
               userId = cardModel == null ? 0 : cardModel.UserId;
            }
            List<YSWL.MALL.Model.Shop.Coupon.CouponInfo> infoList = couponBll.GetModelList(" Status=1 and UserId=" + userId);

            this.ddlInfo.DataSource = infoList;

            this.ddlInfo.DataTextField = "CouponCode";
            this.ddlInfo.DataValueField = "CouponCode";
            this.ddlInfo.DataBind();
            this.ddlInfo.Items.Insert(0, new ListItem("--请选择--", ""));
        }

        protected void btnUse_Click(object sender, EventArgs e)
        {
            string code = this.ddlInfo.SelectedValue;
            if (String.IsNullOrWhiteSpace(code))
            {
                MessageBox.ShowFailTip(this, "请选择用户优惠券！");
                return;
            }
            if (couponBll.UseCoupon(code))
            {
                MessageBox.ShowSuccessTip(this, "操作成功！");
            }
            else
            {
                YSWL.Common.MessageBox.ShowFailTip(this, Resources.Site.TooltipDelError);
            }
            gridView.OnBind();

        }
        
    }
}