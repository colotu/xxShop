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

namespace YSWL.MALL.Web.Admin.Shop.Coupon
{
    public partial class CouponUsed : PageBaseAdmin
    {
        protected override int Act_PageLoad { get { return 418; } } //Shop_优惠券管理_列表页
        protected new int Act_AddData = 417;    //Shop_优惠券规则管理_新增数据
        protected new int Act_DelData = 419;    //Shop_优惠券管理_删除数据
        private YSWL.MALL.BLL.Shop.Coupon.CouponInfo couponBll = new CouponInfo();
        private YSWL.MALL.BLL.Shop.Coupon.CouponClass classBll = new CouponClass();
        private YSWL.MALL.BLL.Shop.Order.Orders orderBll = new Orders();
        private YSWL.MALL.BLL.Shop.Products.ProductInfo prodBll = new ProductInfo();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
         
           
            }
        }

        #region gridView

        public void BindData()
        {
            //清空值
            lblUserName.Text ="";
            lblNickName.Text ="";
            lblTrueName.Text = "";
            lblPhone.Text ="";

            string value = this.txtUser.Text;
            int userId = Common.Globals.SafeInt(value, 0);
            YSWL.MALL.BLL.Members.Users userBll = new BLL.Members.Users();
            YSWL.MALL.BLL.Members.UserCard cardBll = new BLL.Members.UserCard();
            //是否存在
            if (!userBll.Exists(userId))
            {
                //不存在就在会员卡中取
                YSWL.MALL.Model.Members.UserCard cardModel = cardBll.GetModel(value);
                userId = cardModel == null ? 0 : cardModel.UserId;
            }
            if (userId == 0 && !String.IsNullOrWhiteSpace(value))
            {
                YSWL.Common.MessageBox.ShowFailTip(this, "该用户不存在");
                return;
            }
          

            //获取用户信息
            YSWL.Accounts.Bus.User userModel = new User(userId);
            lblUserName.Text = userModel.UserName;
            lblNickName.Text = userModel.NickName;
            lblTrueName.Text = userModel.TrueName;
            lblPhone.Text = userModel.Phone;

            StringBuilder whereStr = new StringBuilder();
            whereStr.AppendFormat("Status=1 and UserId={0}", userId);
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
                YSWL.MALL.Model.Shop.Order.OrderInfo orderInfo = orderBll.GetOrderByCoupon(coupon);
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
            string str = string.Empty;
            if (!StringPlus.IsNullOrEmpty(target))
            {
                int status = Common.Globals.SafeInt(target, 0);
                switch (status)
                {
                    case 0:
                        str = "未分配";
                        break;
                    case 1:
                        str = "已分配";
                        break;
                    case 2:
                        str = "已使用";
                        break;
                    default:
                        break;
                }
            }
            return str;
        }



        protected void btnUsed_Click(object sender, EventArgs e)
        {
            string idlist = GetSelIDlist();
            if (idlist.Trim().Length == 0)
                return;
            //int status = Common.Globals.SafeInt(ddlAction.SelectedValue, -1);
            //if (status == -1)
            //    return;
            if (couponBll.UpdateStatusList(idlist, 2))
            {
                MessageBox.ShowSuccessTip(this, "批量使用成功！");
            }
            else
            {
                YSWL.Common.MessageBox.ShowFailTip(this, "操作失败");
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
                        idlist += "'" + gridView.DataKeys[i].Value.ToString() + "',";
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
            gridView.OnBind();
        }

        protected void btnUse_Click(object sender, EventArgs e)
        {
            string code = "";// this.ddlInfo.SelectedValue;
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