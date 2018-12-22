using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using YSWL.MALL.BLL.Members;
using YSWL.MALL.BLL.Shop.Sales;
using YSWL.Common;

namespace YSWL.MALL.Web.Admin.Shop.WholeSale
{
    public partial class AddRankRule : PageBaseAdmin
    {
        private YSWL.MALL.BLL.Members.UserRank rankBll = new UserRank();
        private YSWL.MALL.BLL.Shop.Sales.SalesRule ruleBll = new BLL.Shop.Sales.SalesRule();
        private YSWL.MALL.BLL.Shop.Sales.SalesItem itemBll = new SalesItem();
        private YSWL.MALL.BLL.Shop.Sales.SalesUserRank userRankBll = new SalesUserRank();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                bool isEnable = BLL.SysManage.ConfigSystem.GetBoolValueByCache("RankScoreEnable");
                this.hfEnable.Value = isEnable.ToString();
                //获取普通会员等级
                DataSet ds = rankBll.GetList(-1, " RankType=0", "RankLevel");
                this.ChkUserRank.DataSource = ds;
                ChkUserRank.DataTextField = "Name";
                ChkUserRank.DataValueField = "RankId";
                ChkUserRank.DataBind();
                //经销商等级  没有值先隐藏
                //this.ChkDealerRank.DataSource = rankBll.GetList(" RankType=1");
                //ChkDealerRank.DataTextField = "Name";
                //ChkDealerRank.DataValueField = "RankId";
                //ChkDealerRank.DataBind();

            }
        }

        protected void btnSave_OnClick(object sender, EventArgs e)
        {
            YSWL.MALL.Model.Shop.Sales.SalesRule ruleModel = new Model.Shop.Sales.SalesRule();
            ruleModel.RuleName = this.txtRuleName.Text;
            ruleModel.RuleMode = 0;
            ruleModel.RuleUnit = 0;
            ruleModel.Status = Common.Globals.SafeInt(this.radStatus.SelectedValue, 0);
            ruleModel.CreatedDate = DateTime.Now;
            ruleModel.CreatedUserID = CurrentUser.UserID;
            ruleModel.Type = 1;
            if (String.IsNullOrWhiteSpace(txtRateValue.Text))
            {
                MessageBox.ShowFailTip(this, "请填写优惠规则值");
                return;
            }
            //新增批发规则
            int ruleId = ruleBll.Add(ruleModel);
            if (ruleId > 0)
            {
                //新增规则的优惠项
                int itemtype = Common.Globals.SafeInt(this.radItemType.SelectedValue, 0);
                YSWL.MALL.Model.Shop.Sales.SalesItem itemModel = new Model.Shop.Sales.SalesItem();
                itemModel.ItemType = itemtype;
                itemModel.UnitValue = 1;
                itemModel.RateValue = Common.Globals.SafeInt(txtRateValue.Text, 0);
                itemModel.RuleId = ruleId;
                itemBll.Add(itemModel);
                //新增规则的对应的用户等级和经销商等级
                for (int i = 0; i < ChkUserRank.Items.Count; i++)
                {
                    if (ChkUserRank.Items[i].Selected)
                    {
                        YSWL.MALL.Model.Shop.Sales.SalesUserRank userRankModel = new Model.Shop.Sales.SalesUserRank();
                        userRankModel.RankId = Common.Globals.SafeInt(ChkUserRank.Items[i].Value, 0);
                        userRankModel.RuleId = ruleId;
                        userRankBll.Add(userRankModel);
                    }
                }
                MessageBox.ShowSuccessTipScript(this, "操作成功！", "window.parent.location.reload();");
            }

        }
    }
}