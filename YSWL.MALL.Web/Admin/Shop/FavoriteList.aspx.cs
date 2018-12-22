using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using YSWL.Common;
using System.Data;
using YSWL.Accounts.Bus;
using System.Drawing;
using YSWL.MALL.Model.Shop;
namespace YSWL.MALL.Web.Admin.Shop
{
    public partial class FavoriteList : PageBaseAdmin
    {
        protected override int Act_PageLoad { get { return 560; } } //Shop_收藏管理页
        private User currentUser;
        YSWL.MALL.BLL.Shop.Favorite favorBll = new BLL.Shop.Favorite();
        YSWL.MALL.BLL.Shop.Products.ProductInfo productBll = new BLL.Shop.Products.ProductInfo();
        protected void Page_Load(object sender, EventArgs e)
        {
           
            if (!Page.IsPostBack)
            {
                if (UserId != 0)
                {
                    currentUser = new User(UserId);
                    if (currentUser == null)
                    {
                        Response.Write("<script language=javascript>window.alert('" + Resources.Site.TooltipUserExist + "\\');history.back();</script>");
                        return;
                    }
                }
                else
                {
                    this.txtTitle.Text = "会员收藏列表";
                }
            
            }
        }

        #region gridView

        public void BindData()
        {
            string keyword = "";// this.txtKeyword.Text;
            int userId = UserId == 0 ? Common.Globals.SafeInt(this.txtUserId.Text, 0) : UserId;
            DataSet ds = favorBll.GetListEX(userId, keyword);
            if (ds != null)
            {
                gridView.DataSetSource = ds;
            }
        }
        public int UserId
        {
            get
            {
                int userid = 0;
                if (Request.Params["userid"] != null && PageValidate.IsNumber(Request.Params["userid"]))
                {
                    userid = int.Parse(Request.Params["userid"]);
                }
                return userid;
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

        /// <summary>
        /// 返回商品名
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public string GetTargetName(long ProductId,int  typeid)
        {
            switch (typeid)
            {
                case (int)FavoriteEnums.Product:
                    string name= productBll.GetProductName(ProductId);
                    return String.IsNullOrWhiteSpace(name) ? "此商品不存在" : name;
                default :
                    return "";
            }  
        }
        /// <summary>
        /// 返回用户名
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public string GetUserName(int userid)
        {
            YSWL.Accounts.Bus.User user=new User(userid);
            if(user!=null)
            {
                return user.UserName;
            }
            return "--";
        }
        #endregion

        protected void btnSearch_Click(object sender, EventArgs e)
        {
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
                    if (gridView.DataKeys[i].Value != null)
                    {
                        //idlist += gridView.Rows[i].Cells[1].Text + ",";
                        idlist += gridView.DataKeys[i].Value.ToString() + ",";
                    }
                }
            }
            if (BxsChkd)
            {
                idlist = idlist.Substring(0, idlist.LastIndexOf(","));
            }
            return idlist;
        }

        protected void btnRemove_Click(object sender, EventArgs e)
        {
            string idlist = GetSelIDlist();
            if (idlist.Trim().Length == 0)
                return;
            if (favorBll.DeleteList(idlist))
            {
                MessageBox.ShowSuccessTip(this, Resources.Site.TooltipDelOK);
               
            }
            else
            {
                MessageBox.ShowFailTip(this, Resources.Site.TooltipDelError);
            }
            gridView.OnBind();
        }

    }
}