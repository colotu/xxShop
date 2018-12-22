/**
* Add.cs
*
* 功 能： 新增网站菜单
* 类 名： Add.cs
*
* Ver    变更日期             负责人  变更内容
* ───────────────────────────────────
* V0.01  2012/5/23 16:06:13  孙鹏    初版
*
* Copyright (c) 2012 YS56 Corporation. All rights reserved.
*┌──────────────────────────────────┐
*│　此技术信息为本公司机密信息，未经本公司书面同意禁止向第三方披露．　│
*│　版权所有：小鸟科技（北京）科技有限公司　　　　　　　　　　　　　　│
*└──────────────────────────────────┘
*/
using System;
using YSWL.MALL.BLL.Shop.Products;
using YSWL.Common;
using System.Web.UI.WebControls;
namespace YSWL.MALL.Web.Settings.MainMenus
{
    public partial class Add : PageBaseAdmin
    {
        protected override int Act_PageLoad { get { return 387; } } //设置_导航菜单管理_新增页
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                if (!UserPrincipal.HasPermissionID(GetPermidByActID(Act_AddData)))
                {
                    MessageBox.ShowAndBack(this, "您没有权限");
                    return;
                }
                //获取主区域的
               string mainArea = BLL.SysManage.ConfigSystem.GetValue("MainArea");
               this.ddlTheme.DataSource = YSWL.MALL.Web.Components.FileHelper.GetThemeList(mainArea);
                this.ddlTheme.DataTextField = "Name";
                this.ddlTheme.DataValueField = "Name";
                ddlTheme.DataBind();
                ddlTheme.Items.Insert(0,new ListItem("全部",""));
            }
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(this.txtMenuName.Text))
            {
                YSWL.Common.MessageBox.ShowFailTip(this, Resources.CMS.WMErrorMenuName);
                return;
            }
            //if (string.IsNullOrWhiteSpace(this.txtNavURL.Text))
            //{
            //    YSWL.Common.MessageBox.ShowFailTip(this, Resources.CMS.WMErrorPageUrl);
            //    return;
            //}
            if (string.IsNullOrWhiteSpace(this.txtSqueeze.Text) || !PageValidate.IsNumber(this.txtSqueeze.Text))
            {
                YSWL.Common.MessageBox.ShowFailTip(this, Resources.CMS.WMErrorSqueeze);
                return;
            }
            
            string MenuName = this.txtMenuName.Text;
            string NavURL = this.txtNavURL.Text;
            int Target = int.Parse(this.ddlTarget.SelectedValue);
            bool IsUsed = this.chkIsUsed.Checked;

            YSWL.MALL.Model.Settings.MainMenus model = new YSWL.MALL.Model.Settings.MainMenus();
            model.NavArea = ddlType.AreaInt;
            model.URLType = Common.Globals.SafeInt(ddNavType.SelectedValue, 0);
            switch (model.URLType)
            {
                case 0:
                    model.NavURL = this.txtNavURL.Text.Trim();
                    break;
                case 1:
                case 2:
                case 3:
                case 4:
                    model.NavURL =this.ddValue.SelectedValue;
                    break;
                default:
                    model.NavURL = this.txtNavURL.Text.Trim();
                    break;
            }
            if (string.IsNullOrWhiteSpace(model.NavURL))
            {
                YSWL.Common.MessageBox.ShowFailTip(this, Resources.CMS.WMErrorPageUrl);
                return;
            }
            model.MenuName = MenuName;
            model.MenuType = 1;
            model.Target = Target;
            model.IsUsed = IsUsed;
            model.MenuTitle = this.txtTile.Text;
            model.Sequence = int.Parse(this.txtSqueeze.Text);
            model.NavTheme = ddlTheme.SelectedValue;

            YSWL.MALL.BLL.Settings.MainMenus bll = new YSWL.MALL.BLL.Settings.MainMenus();
            if (bll.Add(model) > 0)
            {
                this.btnSave.Enabled = false;
                this.btnCancle.Enabled = false;
                YSWL.Common.MessageBox.ShowSuccessTip(this, Resources.Site.TooltipSaveOK, "list.aspx");
            }
            else
            {
                this.btnSave.Enabled = false;
                this.btnCancle.Enabled = false;
                YSWL.Common.MessageBox.ShowFailTip(this, Resources.Site.TooltipTryAgainLater, "list.aspx");
            }
        }

        public void btnCancle_Click(object sender, EventArgs e)
        {
            Response.Redirect("list.aspx");
        }

        protected void ddNavType_Change(object sender, EventArgs e)
        {
            int NavType = Common.Globals.SafeInt(this.ddNavType.SelectedValue, 0);
            switch (NavType)
            {
                case 0:
                    this.txtNavURL.Visible = true;
                    this.ddValue.Visible = false;
                    break;
                case 1:
                    YSWL.MALL.BLL.CMS.ContentClass classBll = new BLL.CMS.ContentClass();
                    ddValue.DataSource = classBll.GetList(" ParentId=0");
                    this.ddValue.DataTextField = "ClassName";
                    this.ddValue.DataValueField = "ClassID";
                    this.ddValue.DataBind();
                    ddValue.Items.Insert(0, new ListItem("请选择", ""));
                    this.ddValue.Visible = true;
                    break;
                case 4:
                    YSWL.MALL.BLL.Shop.Products.CategoryInfo shopCate =new CategoryInfo();
                    ddValue.DataSource = shopCate.GetList(" ParentCategoryId=0  and Status=1 ");
                    this.ddValue.DataTextField = "Name";
                    this.ddValue.DataValueField = "CategoryId";
                    this.ddValue.DataBind();
                    ddValue.Items.Insert(0, new ListItem("请选择", ""));
                    this.ddValue.Visible = true;
                    break;
                default:
                    this.ddValue.Visible = false;
                    this.txtNavURL.Visible = true;
                    break;

            }
        }

        protected void ddlType_Change(object sender, EventArgs e)
        {
            this.ddlTheme.DataSource = YSWL.MALL.Web.Components.FileHelper.GetThemeList(ddlType.AreaName);
            this.ddlTheme.DataTextField = "Name";
            this.ddlTheme.DataValueField = "Name";
            ddlTheme.DataBind();
            ddlTheme.Items.Insert(0, new ListItem("全部", ""));
            this.ddValue.Visible = false;
            this.ddNavType.SelectedValue = "0";
            this.txtNavURL.Visible = true;
          
        }


       

    }
}
