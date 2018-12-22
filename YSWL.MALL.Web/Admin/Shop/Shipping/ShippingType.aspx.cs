using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using YSWL.Common;
using System.Text;

namespace YSWL.MALL.Web.Admin.Shop.Shipping
{
    public partial class ShippingType :PageBaseAdmin
    {
        protected override int Act_PageLoad { get { return 517; } } //Shop_配送方式管理_列表页
        protected new int Act_AddData = 518;    //Shop_配送方式管理_新增数据
        protected new int Act_UpdateData = 519;    //Shop_配送方式管理_编辑数据
        protected new int Act_DelData = 520;    //Shop_配送方式管理_删除数据

        YSWL.MALL.BLL.Shop.Shipping.ShippingType bll = new BLL.Shop.Shipping.ShippingType();
        YSWL.MALL.BLL.Shop.Supplier.SupplierInfo suppBll = new BLL.Shop.Supplier.SupplierInfo();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                BindSupplier();
                BindPayment();
                if (!UserPrincipal.HasPermissionID(GetPermidByActID(Act_AddData)) && GetPermidByActID(Act_AddData) != -1)
                {
                    liAdd.Visible = false;
                }
                
            }
        }

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            gridView.OnBind();
        }


        /// <summary>
        /// 数据绑定
        /// </summary>
        public void BindData()
        {
            int supplierId = Globals.SafeInt(ddlSupplier.SelectedValue, -1);
            int payId = Globals.SafeInt(ddlPayment.SelectedValue, -1);
            string name= Common.InjectionFilter.SqlFilter(txtKeyword.Text.Trim());
            gridView.DataSetSource= bll.GetList(0, name, supplierId, payId, "");
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
                if (!UserPrincipal.HasPermissionID(GetPermidByActID(Act_DelData)) && GetPermidByActID(Act_DelData) != -1)
                {
                    LinkButton delbtn = (LinkButton)e.Row.FindControl("linkDel");
                    delbtn.Visible = false;
                }
                if (!UserPrincipal.HasPermissionID(GetPermidByActID(Act_UpdateData)) && GetPermidByActID(Act_UpdateData) != -1)
                {
                    HtmlGenericControl updatebtn = (HtmlGenericControl)e.Row.FindControl("lbtnModify");
                    updatebtn.Visible = false;
                }
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
            int ID = Common.Globals.SafeInt(gridView.DataKeys[e.RowIndex].Value.ToString(), 0);
            bll.Delete(ID);
            gridView.OnBind();
        }



        protected string GetModeName(object target)
        {
            //0:审核通过、1:作为草稿、2:等待审核。
            string str = string.Empty;
            if (!StringPlus.IsNullOrEmpty(target))
            {
                switch (target.ToString())
                {
                    case "1":
                        str = "下拉列表";
                        break;
                    case "2":
                        str = "单选按钮";
                        break;
                    default:
                        str = "下拉列表";
                        break;
                }
            }
            return str;
        }
        /// <summary>
        /// 绑定商家
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
            this.ddlSupplier.Items.Insert(0, new ListItem("平台", "0"));
            this.ddlSupplier.Items.Insert(0, new ListItem("全部", "-1")); 
        }
        /// <summary>
        /// 绑定支付方式
        /// </summary>
        private void BindPayment()
        {
            List<YSWL.Payment.Model.PaymentModeInfo> paymentList = YSWL.Payment.BLL.PaymentModeManage.GetPaymentModes(YSWL.Payment.Model.DriveEnum.ALL);
            if (paymentList!=null )
            {
                this.ddlPayment.DataSource = paymentList;
                this.ddlPayment.DataTextField = "Name";
                this.ddlPayment.DataValueField = "ModeId";
                this.ddlPayment.DataBind();
            }
            this.ddlPayment.Items.Insert(0, new ListItem("全部", "0"));
        }
       
        protected string GetSupplier(object target)
        {
            int supplierId=Globals.SafeInt(target,0);
            if (supplierId <=0)
            {
                return "平台";
            }
            YSWL.MALL.Model.Shop.Supplier.SupplierInfo  info= suppBll.GetModelByCache(supplierId);
            if (info==null) {
                return "";
            }
            return info.Name;
        }
    }
}