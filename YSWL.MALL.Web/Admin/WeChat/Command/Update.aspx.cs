using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using YSWL.MALL.BLL.CMS;
using YSWL.MALL.BLL.Shop.Products;
using YSWL.Common;

namespace YSWL.MALL.Web.Admin.WeChat.Command
{
    public partial class Update : PageBaseAdmin
    {
        protected override int Act_PageLoad { get { return 633; } } //移动营销_微信导航菜单管理_编辑页

        YSWL.WeChat.BLL.Core.Command commandBll = new YSWL.WeChat.BLL.Core.Command();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                //对应操作
                List<YSWL.WeChat.Model.Core.Action> actionList = YSWL.WeChat.BLL.Core.Action.GetAllAction();
                this.dropAction.DataSource = actionList;
                this.dropAction.DataTextField = "Name";
                this.dropAction.DataValueField = "ActionId";
                this.dropAction.DataBind();
                this.dropAction.Items.Insert(0,new ListItem("请选择", "0"));
             
                ShowInfo();
            }
        }

        public int CommandId
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

        private void ShowInfo()
        {
            YSWL.WeChat.Model.Core.Command commandModel = commandBll.GetModel(CommandId);
            this.dropAction.SelectedValue = commandModel.ActionId.ToString();
            tName.Text = commandModel.Name;
            txtDesc.Text = commandModel.Remark;
            if (commandModel.ParseType == 0)
            {
                txtParseType.Text = commandModel.ParseLength.ToString();
            }
            else
            {
                txtParseType.Text = commandModel.ParseChar;
            }

            this.ddTarget.Visible = true;
            switch (commandModel.ActionId)
            {
                //文章栏目
                case 1:
                    YSWL.MALL.BLL.CMS.ContentClass classBll = new ContentClass();
                    List<YSWL.MALL.Model.CMS.ContentClass> classList =
                        classBll.GetModelList(" Depth=1 and State=0");
                    this.ddTarget.DataSource = classList;
                    ddTarget.DataTextField = "ClassName";
                    ddTarget.DataValueField = "ClassID";
                    ddTarget.DataBind();
                    ddTarget.Items.Insert(0, new ListItem("请选择", "0"));
                    ddTarget.SelectedValue = commandModel.TargetId.ToString();
                    break;
                //商品分类
                case 2:
                    YSWL.MALL.BLL.Shop.Products.CategoryInfo cateBll = new CategoryInfo();
                    List<YSWL.MALL.Model.Shop.Products.CategoryInfo> CategoryInfos =
                        cateBll.GetModelList(" Depth=1");
                    this.ddTarget.DataSource = CategoryInfos;
                    ddTarget.DataTextField = "Name";
                    ddTarget.DataValueField = "CategoryId";
                    ddTarget.DataBind();
                    ddTarget.Items.Insert(0, new ListItem("请选择", "0"));
                    ddTarget.SelectedValue = commandModel.TargetId.ToString();
                    break;
                default:
                    this.ddTarget.Visible = false;
                    break;
            }
            ddParseType.SelectedValue = commandModel.ParseType.ToString();
            chkStatus.Checked = commandModel.Status == 1;
            txtSequence.Text = commandModel.Sequence.ToString();
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            YSWL.WeChat.Model.Core.Command commandModel = commandBll.GetModel(CommandId);
              string openId = YSWL.WeChat.BLL.Core.Config.GetValueByCache("WeChat_OpenId", -1, CurrentUser.UserType);
            int actionId = Common.Globals.SafeInt(dropAction.SelectedValue, 0);
            string name = this.tName.Text;
            if (actionId == 0)
            {
                YSWL.Common.MessageBox.ShowFailTip(this, "请选择指定操作");
                return;
            }
            if (String.IsNullOrWhiteSpace(name))
            {
                YSWL.Common.MessageBox.ShowFailTip(this, "请输入指令名称");
                return;
            }
            commandModel.Name = name;
            commandModel.ActionId = Common.Globals.SafeInt(dropAction.SelectedValue, 0);
            commandModel.ParseType = Common.Globals.SafeInt(ddParseType.SelectedValue, 0);
            commandModel.Status = chkStatus.Checked ? 1 : 0;
            if (commandModel.ParseType == 0)
            {
                commandModel.ParseLength = Common.Globals.SafeInt(this.txtParseType.Text, 0);
            }
            else
            {
                commandModel.ParseChar = this.txtParseType.Text.Trim();
            }
            commandModel.Remark = this.txtDesc.Text;
            commandModel.Sequence = Common.Globals.SafeInt(this.txtSequence.Text, 0);
            int targetId = Common.Globals.SafeInt(this.ddTarget.SelectedValue,0);
            commandModel.TargetId = targetId;
            commandModel.OpenId = openId;
            if (commandBll.Update(commandModel))
            {
                MessageBox.ShowSuccessTipScript(this, "操作成功！", "window.parent.location.reload();");
            }
            else
            {
                YSWL.Common.MessageBox.ShowSuccessTip(this, "操作失败！");
            }
        }

        #region dropDown IndexChange  事件

        protected void dropAction_IndexChange(object sender, EventArgs e)
        {
            int actionId = Common.Globals.SafeInt(this.dropAction.SelectedValue, 0);
            this.ddTarget.Visible = true;
            switch (actionId)
            {
                //文章栏目
                case 1:
                    YSWL.MALL.BLL.CMS.ContentClass classBll = new ContentClass();
                    List<YSWL.MALL.Model.CMS.ContentClass> classList =
                        classBll.GetModelList(" Depth=1 and State=0");
                    this.ddTarget.DataSource = classList;
                    ddTarget.DataTextField = "ClassName";
                    ddTarget.DataValueField = "ClassID";
                    ddTarget.DataBind();
                    ddTarget.Items.Insert(0, new ListItem("请选择", "0"));
                    break;
                //商品分类
                case 2:
                    YSWL.MALL.BLL.Shop.Products.CategoryInfo cateBll = new CategoryInfo();
                    List<YSWL.MALL.Model.Shop.Products.CategoryInfo> CategoryInfos =
                        cateBll.GetModelList(" Depth=1");
                    this.ddTarget.DataSource = CategoryInfos;
                    ddTarget.DataTextField = "Name";
                    ddTarget.DataValueField = "CategoryId";
                    ddTarget.DataBind();
                    ddTarget.Items.Insert(0, new ListItem("请选择", "0"));
                    break;
                default:
                    this.ddTarget.Visible = false;
                    break;
            }
            //YSWL.MALL.BLL.Shop.Products.   ddCateImage
        }

        #endregion


    }
}