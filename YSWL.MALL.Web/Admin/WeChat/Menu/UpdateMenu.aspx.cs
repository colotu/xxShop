using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using YSWL.Common;
using YSWL.Json;
using YSWL.Web;

namespace YSWL.MALL.Web.Admin.WeChat.Menu
{
    public partial class UpdateMenu : PageBaseAdmin
    {
        YSWL.WeChat.BLL.Core.Menu menuBll = new YSWL.WeChat.BLL.Core.Menu();
        YSWL.MALL.BLL.Shop.Products.CategoryInfo cateBll = new BLL.Shop.Products.CategoryInfo();
        private YSWL.MALL.BLL.CMS.Content contentBll = new BLL.CMS.Content();
        protected void Page_Load(object sender, EventArgs e)
        {

            if (!string.IsNullOrWhiteSpace(this.Request.Form["Callback"]) && (this.Request.Form["Callback"] == "true"))
            {
                this.Controls.Clear();
                this.DoCallback();
            }
            if (!IsPostBack)
            {
                List<YSWL.WeChat.Model.Core.Action> actionList = YSWL.WeChat.BLL.Core.Action.GetAllAction();
                this.ddMenuAction.DataSource = actionList;
                this.ddMenuAction.DataTextField = "Name";
                this.ddMenuAction.DataValueField = "ActionId";
                this.ddMenuAction.DataBind();
                this.ddMenuAction.Items.Insert(0, new ListItem("自定义网址", "0"));

                this.ddMenuAction.Items.Insert(1, new ListItem("微信商城", "-1"));
                this.ddMenuAction.Items.Insert(2, new ListItem("微官网", "-2"));
                this.ddMenuAction.Items.Insert(3, new ListItem("我的账户", "-3"));
                this.ddMenuAction.Items.Insert(4, new ListItem("商品分类", "-4"));
                this.ddMenuAction.Items.Insert(5, new ListItem("我的订单", "-5"));
                this.ddMenuAction.Items.Insert(6, new ListItem("我的会员卡", "-6"));
                this.ddMenuAction.Items.Insert(7, new ListItem("会员签到", "-7"));
                this.ddMenuAction.Items.Insert(8, new ListItem("我要报名", "-8"));
                this.ddMenuAction.Items.Insert(9, new ListItem("单篇文章", "-9"));
                this.ddMenuAction.Items.Insert(10, new ListItem("优惠券兑换", "-10"));

                this.ddMenuAction.Items.Insert(11, new ListItem("天气查询", "-11"));
                this.ddMenuAction.Items.Insert(12, new ListItem("周公解梦", "-12"));
                this.ddMenuAction.Items.Insert(13, new ListItem("快递查询", "-13"));
                //商品一级分类
                this.ddCategory.DataSource = cateBll.GetCategorysByDepth(1);
                this.ddCategory.DataTextField = "Name";
                this.ddCategory.DataValueField = "CategoryId";
                this.ddCategory.DataBind();
                this.ddCategory.Items.Insert(0, new ListItem("请选择", "0"));

                //栏目分类
                List<YSWL.MALL.Model.CMS.ContentClass> AllClass = YSWL.MALL.BLL.CMS.ContentClass.GetAllClass();
                this.ddCMSClass.DataSource = AllClass.Where(c => c.Depth == 1);
                this.ddCMSClass.DataTextField = "ClassName";
                this.ddCMSClass.DataValueField = "ClassID";
                this.ddCMSClass.DataBind();
                this.ddCMSClass.Items.Insert(0, new ListItem("请选择", "0"));


                    if (this.ddMenuAction.Items.FindByValue("15") == null)
                    {
                        this.ddMenuAction.Items.Insert(5, new ListItem("推广二维码", "15"));
                    }
                ShowInfo();

            }
        }

        #region 编号

        /// <summary>
        /// 编号
        /// </summary>
        protected int MenuId
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

        #endregion

        private void ShowInfo()
        {
            YSWL.WeChat.Model.Core.Menu menuModel = menuBll.GetModel(MenuId);
            if (menuModel != null)
            {
                txtDesc.Text = menuModel.Remark;
                txtSequence.Text = menuModel.Sequence.ToString();
                YSWL.WeChat.Model.Core.Menu parent = menuBll.GetModel(menuModel.ParentId);
                lblParentName.Text = parent == null ? "主菜单" : parent.Name;
                chkStatus.Checked = menuModel.Status == 1;
                int actionId= YSWL.WeChat.BLL.Core.Action.GetActionId(menuModel.MenuKey);
                if (actionId == 0)
                {
                    actionId = GetSelectValue(menuModel.MenuUrl);
                }
                ddMenuAction.SelectedValue = actionId.ToString();
                txtRemark.Text = menuModel.MenuUrl;
                tName.Text = menuModel.Name;
                if (menuModel.Type == "click")
                {
                    txtRemark.Text = menuModel.Remark;
                    if (actionId == 1)
                    {
                        ddCMSClass.SelectedValue = Common.Globals.SafeInt(menuModel.Remark, 0).ToString();
                    }
                }
            }
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            YSWL.WeChat.Model.Core.Menu menuModel = menuBll.GetModel(MenuId);
            int actionId = Common.Globals.SafeInt(ddMenuAction.SelectedValue, 0);
            int categoryId = Common.Globals.SafeInt(ddCategory.SelectedValue, 0);
            string remark = this.txtRemark.Text;
            string name = this.tName.Text;
         
            if (String.IsNullOrWhiteSpace(name))
            {
                YSWL.Common.MessageBox.ShowFailTip(this, "请输入菜单名称");
                return;
            }
        
            menuModel.Name = name;
            menuModel.MenuKey = "";
            menuModel.MenuUrl = "";
            menuModel.Type = "view";
            if (actionId <= 0)
            {
                switch (actionId)
                {
                    case -17:
                        menuModel.MenuUrl = YSWL.Components.MvcApplication.GetCurrentRoutePath(AreaRoute.MShop);
                        break;
                    case -16:
                        menuModel.MenuUrl = YSWL.Components.MvcApplication.GetCurrentRoutePath(AreaRoute.MBShop) + "u/Orders";
                        break;
                    case -15:
                        menuModel.MenuUrl = YSWL.Components.MvcApplication.GetCurrentRoutePath(AreaRoute.MBShop) + "u";
                        break;
                    case -14:
                        menuModel.MenuUrl = YSWL.Components.MvcApplication.GetCurrentRoutePath(AreaRoute.MBShop);
                        break;
                    //快递查询
                    case -13:
                        string expressUrl = YSWL.MALL.BLL.SysManage.ConfigSystem.GetValueByCache("WeChat_Core_ExpressUrl");
                        menuModel.MenuUrl = String.IsNullOrWhiteSpace(expressUrl) ? "http://m.kuaidi100.com/index_all.html" : expressUrl;
                        break;
                    //周公解梦
                    case -12:
                        string jiemengUrl = YSWL.MALL.BLL.SysManage.ConfigSystem.GetValueByCache("WeChat_Core_JieMeng");
                        menuModel.MenuUrl = String.IsNullOrWhiteSpace(jiemengUrl) ? "http://jiemengmobi.duapp.com/" : jiemengUrl;
                        break;
                    //天气查询
                    case -11:
                        string weather = YSWL.MALL.BLL.SysManage.ConfigSystem.GetValueByCache("WeChat_Core_WeatherUrl");
                        menuModel.MenuUrl = String.IsNullOrWhiteSpace(weather) ? "http://mobile.weather.com.cn/" : weather;
                        break;
                    case -10:
                        menuModel.MenuUrl = YSWL.Components.MvcApplication.GetCurrentRoutePath(AreaRoute.COM) + "Admin/CouponEx";
                        break;
                    case -9:
                        int articleId = Globals.SafeInt(this.hfArticleId.Value, 0);
                        menuModel.MenuUrl = YSWL.Components.MvcApplication.GetCurrentRoutePath(AreaRoute.COM) + "Article/Detail/" + articleId;
                        break;
                    case -8:
                        menuModel.MenuUrl = YSWL.Components.MvcApplication.GetCurrentRoutePath(AreaRoute.COM) + "WeChat/Apply";
                        break;
                    case -7:
                        menuModel.MenuUrl = YSWL.Components.MvcApplication.GetCurrentRoutePath(AreaRoute.COM) + "UserCenter/signpoint";
                        break;
                    case -6:
                        menuModel.MenuUrl = YSWL.Components.MvcApplication.GetCurrentRoutePath(AreaRoute.COM) + "WeChat/usercard";
                        break;
                    case -5:
                        menuModel.MenuUrl = YSWL.Components.MvcApplication.GetCurrentRoutePath(AreaRoute.MShop) + "u/Orders";
                        break;
                    case -4:
                        menuModel.MenuUrl = YSWL.Components.MvcApplication.GetCurrentRoutePath(AreaRoute.MShop) + "p/" + categoryId;
                        break;
                    case -3:
                        menuModel.MenuUrl = YSWL.Components.MvcApplication.GetCurrentRoutePath(AreaRoute.MShop) + "u";
                        break;
                    case -2:
                        menuModel.MenuUrl = YSWL.Components.MvcApplication.GetCurrentRoutePath(AreaRoute.MPage);
                        break;
                    case -1:
                        menuModel.MenuUrl = YSWL.Components.MvcApplication.GetCurrentRoutePath(AreaRoute.MShop);
                        break;
                    case 0:
                        menuModel.MenuUrl = String.IsNullOrWhiteSpace(remark) ? YSWL.Components.MvcApplication.GetCurrentRoutePath(AreaRoute.MShop) : remark;
                        break;
                    default:
                        menuModel.MenuUrl = YSWL.Components.MvcApplication.GetCurrentRoutePath(AreaRoute.MShop);
                        break;
                }
            }
            // 走Action里面的逻辑
            else
            {
                menuModel.Type = "click";
                menuModel.MenuKey = "Action_" + actionId;
                menuModel.Remark = remark;
                if (actionId == 1)
                {
                    menuModel.Remark = ddCMSClass.SelectedValue;
                    menuModel.MenuKey = "Action_" + actionId + "_" + ddCMSClass.SelectedValue;
                }
            }
            menuModel.Status = chkStatus.Checked ? 1 : 0;
            menuModel.Remark = this.txtDesc.Text;
            menuModel.Sequence = Common.Globals.SafeInt(this.txtSequence.Text, 0);
            if (menuBll.Update(menuModel))
            {
                MessageBox.ShowSuccessTipScript(this, "操作成功", " window.parent.location.reload();");
            }
            else
            {
                YSWL.Common.MessageBox.ShowFailTip(this, "操作失败！");
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
                case "GetArticles":
                    writeText = GetArticles();
                    break;
                default:
                    break;

            }
            this.Response.Write(writeText);
            this.Response.End();
        }

        private string GetArticles()
        {
            JsonObject json = new JsonObject();
            int classId = Common.Globals.SafeInt(this.Request.Form["ClassId"], 0);
            List<YSWL.MALL.Model.CMS.Content> ContentList = contentBll.GetModelList(classId);
            JsonArray newsArry = new JsonArray();
            JsonObject itemObj = null;
            foreach (var item in ContentList)
            {
                itemObj = new JsonObject();
                itemObj.Accumulate("title", item.Title);
                itemObj.Accumulate("articleId", item.ContentID);
                newsArry.Add(itemObj);
            }
            json.Accumulate("Data", newsArry.ToString());
            return json.ToString();
        }

        #endregion

        public int GetSelectValue(string menuUrl)
        {
            string expressUrl = YSWL.MALL.BLL.SysManage.ConfigSystem.GetValueByCache("WeChat_Core_ExpressUrl");
            expressUrl = String.IsNullOrWhiteSpace(expressUrl) ? "http://m.kuaidi100.com/index_all.html" : expressUrl;

            if (menuUrl == expressUrl)
            {
                return -13;
            }
            string jiemengUrl = YSWL.MALL.BLL.SysManage.ConfigSystem.GetValueByCache("WeChat_Core_JieMeng");
            jiemengUrl = String.IsNullOrWhiteSpace(jiemengUrl) ? "http://jiemengmobi.duapp.com/" : jiemengUrl;
            if (menuUrl == jiemengUrl)
            {
                return -12;
            }
            string weather = YSWL.MALL.BLL.SysManage.ConfigSystem.GetValueByCache("WeChat_Core_WeatherUrl");
            weather = String.IsNullOrWhiteSpace(weather) ? "http://mobile.weather.com.cn/" : weather;
            if (menuUrl == weather)
            {
                return -11;
            }


            if(menuUrl.Contains("Admin/CouponEx"))
            {
                return -10;
            }
            if (menuUrl.Contains("WeChat/Apply"))
            {
                return -8;
            }
            if (menuUrl.Contains("UserCenter/signpoint"))
            {
                return -7;
            }
            if (menuUrl.Contains("WeChat/usercard"))
            {
                return -6;
            }
            if (menuUrl== YSWL.Components.MvcApplication.GetCurrentRoutePath(AreaRoute.MShop) + "u/Orders")
            {
                return -5;
            }
            if (menuUrl == YSWL.Components.MvcApplication.GetCurrentRoutePath(AreaRoute.MShop) + "u")
            {
                return -3;
            }
            if(menuUrl == YSWL.Components.MvcApplication.GetCurrentRoutePath(AreaRoute.MPage))
            {
                return -2;
            }
            //if(menuUrl == YSWL.Components.MvcApplication.GetCurrentRoutePath(AreaRoute.MShop))
            //{
            //    return -1;
            //}

            if (menuUrl == YSWL.Components.MvcApplication.GetCurrentRoutePath(AreaRoute.MBShop) + "u")
            {
                return -15;
            }
            if (menuUrl == YSWL.Components.MvcApplication.GetCurrentRoutePath(AreaRoute.MBShop) + "u/Orders")
            {
                return -16;
            }
            if (menuUrl == YSWL.Components.MvcApplication.GetCurrentRoutePath(AreaRoute.MBShop) + "u/Orders")
            {
                return -17;
            }
            if (menuUrl == YSWL.Components.MvcApplication.GetCurrentRoutePath(AreaRoute.MBShop))
            {
                return -14;
            }

            return 0;
        }


    }
}