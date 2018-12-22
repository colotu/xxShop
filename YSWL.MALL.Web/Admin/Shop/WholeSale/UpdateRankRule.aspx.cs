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
    public partial class UpdateRankRule : PageBaseAdmin
    {
        YSWL.MALL.BLL.Members.UserRank rankBll = new UserRank();
        YSWL.MALL.BLL.Shop.Sales.SalesRule ruleBll = new BLL.Shop.Sales.SalesRule();
        YSWL.MALL.BLL.Shop.Sales.SalesItem itemBll = new SalesItem();
        YSWL.MALL.BLL.Shop.Sales.SalesUserRank userRankBll = new SalesUserRank();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                bool isEnable = BLL.SysManage.ConfigSystem.GetBoolValueByCache("RankScoreEnable");
                this.hfEnable.Value = isEnable.ToString();
                //获取普通会员等级
                DataSet ds = rankBll.GetList(0, " RankType=0", "RankLevel");
                this.ChkUserRank.DataSource = ds;
                ChkUserRank.DataTextField = "Name";
                ChkUserRank.DataValueField = "RankId";
                ChkUserRank.DataBind();
                ShowInfo(RuleId);

            }
        }

        private void ShowInfo(int RuleId)
        {
            //获取选中的会员等级
            List<YSWL.MALL.Model.Shop.Sales.SalesUserRank> userRanks = userRankBll.GetModelList(" RuleId=" + RuleId);
            List<int> rankList = userRanks.Select(c => c.RankId).ToList();
            for (int i = 0; i < ChkUserRank.Items.Count; i++)
            {
                if (rankList.Contains(Common.Globals.SafeInt(ChkUserRank.Items[i].Value, 0)))
                {
                    ChkUserRank.Items[i].Selected = true;
                }
            }

            //规则信息
            YSWL.MALL.Model.Shop.Sales.SalesRule ruleModel = ruleBll.GetModel(RuleId);
            this.txtRuleName.Text = ruleModel.RuleName;
            this.radStatus.SelectedValue = ruleModel.Status.ToString();

            //规则项信息
            List<YSWL.MALL.Model.Shop.Sales.SalesItem> itemList = itemBll.GetModelList(" RuleId=" + RuleId);
            if (itemList != null && itemList.Count > 0)
            {
                //只取一条数据
                YSWL.MALL.Model.Shop.Sales.SalesItem itemModel = itemList.OrderByDescending(c => c.ItemId).First();
                this.radItemType.SelectedValue = itemModel.ItemType.ToString();
                this.txtRateValue.Text = itemModel.RateValue.ToString();
            }
        }

        protected void btnSave_OnClick(object sender, EventArgs e)
        {
            YSWL.MALL.Model.Shop.Sales.SalesRule ruleModel = ruleBll.GetModel(RuleId);
            ruleModel.RuleName = this.txtRuleName.Text;
            ruleModel.RuleName = this.txtRuleName.Text;
            ruleModel.RuleMode = 0;
            ruleModel.RuleUnit = 0;
            ruleModel.Status = Common.Globals.SafeInt(this.radStatus.SelectedValue, 0);
            ruleModel.CreatedDate = DateTime.Now;
            ruleModel.CreatedUserID = CurrentUser.UserID;
            if (String.IsNullOrWhiteSpace(txtRateValue.Text))
            {
                MessageBox.ShowFailTip(this, "请填写优惠规则值");
                return;
            }
            //编辑批发规则

            if (ruleBll.Update(ruleModel))
            {
                //编辑规则的优惠项   （先删除以前的，然后重新新增）
                itemBll.DeleteByRuleId(ruleModel.RuleId);
                int itemtype = Common.Globals.SafeInt(this.radItemType.SelectedValue, 0);
                YSWL.MALL.Model.Shop.Sales.SalesItem itemModel = new Model.Shop.Sales.SalesItem();
                itemModel.ItemType = itemtype;
                itemModel.UnitValue = 1;
                itemModel.RateValue = Common.Globals.SafeInt(txtRateValue.Text, 0);
                itemModel.RuleId = ruleModel.RuleId;
                itemBll.Add(itemModel);
                //编辑规则的对应的用户等级和经销商等级  （先删除以前的，然后重新新增）
                userRankBll.DeleteByRuleId(ruleModel.RuleId);
                for (int i = 0; i < ChkUserRank.Items.Count; i++)
                {
                    if (ChkUserRank.Items[i].Selected)
                    {
                        YSWL.MALL.Model.Shop.Sales.SalesUserRank userRankModel = new Model.Shop.Sales.SalesUserRank();
                        userRankModel.RankId = Common.Globals.SafeInt(ChkUserRank.Items[i].Value, 0);
                        userRankModel.RuleId = ruleModel.RuleId;
                        userRankBll.Add(userRankModel);
                    }
                }
                MessageBox.ShowSuccessTipScript(this, "操作成功！", "window.parent.location.reload();");
            }

        }

        public int RuleId
        {
            get
            {
                int ruleId = 0;
                if (!string.IsNullOrWhiteSpace(Request.Params["id"]))
                {
                    ruleId = Globals.SafeInt(Request.Params["id"], 0);
                }
                return ruleId;
            }
        }
    }
}