using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.ServiceModel.Configuration;
using System.Text;
using System.Web;
using System.Web.Configuration;
using System.Web.UI;
using System.Web.UI.WebControls;
using YSWL.MALL.BLL.Shop.DisDepot;

namespace YSWL.MALL.Web.Admin.SysManage
{
    public partial class DepotConfig : PageBaseAdmin
    {
        YSWL.MALL.BLL.Shop.DisDepot.Depot depotBll = new Depot();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                //获取所有的仓库
                ddlDepot.DataSource = depotBll.GetList("Status=1");
                ddlDepot.DataTextField = "Name";
                ddlDepot.DataValueField = "DepotId";
                ddlDepot.DataBind();
                this.ddlDepot.Items.Insert(0, new ListItem("请选择", "0"));

                BoundData();
            }
        }

        private void BoundData()
        {
            this.chk_ConOMS.Checked = YSWL.Common.Globals.SafeBool(BLL.SysManage.ConfigSystem.GetValue("Shop_ConnectionOMS"), false);
            this.txtOMSUrl.Text = GetOMSUrl();
            this.chk_ConPMS.Checked = YSWL.Common.Globals.SafeBool(BLL.SysManage.ConfigSystem.GetValue("Shop_ConnectionPMS"), false);
            this.txtPMSUrl.Text = GetPMSUrl();

            this.chk_OpenMultiDepot.Checked = YSWL.Common.Globals.SafeBool(BLL.SysManage.ConfigSystem.GetValue("Shop_OpenMultiDepot"), false);
            this.chk_OpenDepotPro.Checked = YSWL.Common.Globals.SafeBool(BLL.SysManage.ConfigSystem.GetValue("Shop_OpenDepotProFilter"), false);//是否开启分仓商品过滤展示
            int depotId = Common.Globals.SafeInt(BLL.SysManage.ConfigSystem.GetValue("OMS_DefaultDepot"), 0);
            this.ddlDepot.SelectedValue = depotId.ToString();
            this.chk_OpenDepot.Checked = depotId > 0;
            this.txtToken.Text = BLL.SysManage.ConfigSystem.GetValueByCache("Shop_SAAS_Enterprise");
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                //是否对接OMS
                bool IsConOMS = chk_ConOMS.Checked;

                BLL.SysManage.ConfigSystem.Modify("Shop_ConnectionOMS", IsConOMS.ToString(), "是否对接OMS系统 ");

                BLL.SysManage.ConfigSystem.Modify("Shop_OpenMultiDepot", this.chk_OpenMultiDepot.Checked.ToString(), "是否开启多分仓");
                BLL.SysManage.ConfigSystem.Modify("Shop_OpenDepotProFilter", this.chk_OpenDepotPro.Checked.ToString(), "是否开启多分仓商品过滤");

                BLL.SysManage.ConfigSystem.Modify("Shop_SAAS_Enterprise", txtToken.Text, "订单管理系统SAAS版Token");

                bool IsOpen = this.chk_OpenDepot.Checked;
                int depotId = 0;
                if (IsOpen)
                {
                    depotId = Common.Globals.SafeInt(ddlDepot.SelectedValue, 0);
                    YSWL.MALL.BLL.Shop.DisDepot.DepotRegionService.SetDefaultDepot(depotId);//设置默认分仓
                }
                YSWL.MALL.BLL.Shop.DisDepot.DepotRegionService.SetDefaultDepot(depotId);//设置默认分仓     //todo:解决默认仓无法关闭的问题       
                string omsUrl = this.txtOMSUrl.Text.Trim();
                string pmsUrl = this.txtPMSUrl.Text.Trim();
                //清空缓存
                #region 清空所有缓存
                YSWL.Common.DataCache.ClearAll();
                #endregion

                YSWL.Common.MessageBox.ShowSuccessTip(this, Resources.Site.TooltipUpdateOK, "DepotConfig.aspx");
            }
            catch (Exception ex)
            {
                YSWL.Log.LogHelper.AddTextLog(ex.Message,ex.StackTrace);
                YSWL.Common.MessageBox.ShowFailTip(this, Resources.Site.TooltipTryAgainLater, "DepotConfig.aspx");
            }
        }


        #region 辅助方法
        public bool UpdateWCFConfig(string omsUrl,string pmsUrl)
        {
            try
            {
                System.Configuration.Configuration configuration =
                    WebConfigurationManager.OpenWebConfiguration(base.Request.ApplicationPath);
                #region  更新WCF接口节点
                ConfigurationSectionGroup sec = configuration.SectionGroups["system.serviceModel"];
                ServiceModelSectionGroup serviceModelSectionGroup = sec as ServiceModelSectionGroup;
                if (serviceModelSectionGroup != null)
                {
                    ClientSection clientSection = serviceModelSectionGroup.Client;
                    foreach (ChannelEndpointElement item in clientSection.Endpoints)
                    {
                        switch (item.Contract)
                        {
                            case "OMS.IService":
                                if (!String.IsNullOrWhiteSpace(omsUrl))
                                {
                                    omsUrl = omsUrl + "/OMS/OMSService.svc";
                                    item.Address = new Uri(omsUrl);
                                }
                                break;
                            case "PMS.IService":
                                if (!String.IsNullOrWhiteSpace(pmsUrl))
                                {
                                    pmsUrl = pmsUrl + "/PMS/PMSService.svc";
                                    item.Address = new Uri(pmsUrl);
                                }
                                break;
                            default:
                                break;
                        }
                    }
                }
                #endregion
                configuration.Save();
                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public string GetOMSUrl()
        {
            try
            {
                System.Configuration.Configuration configuration =
                    WebConfigurationManager.OpenWebConfiguration(base.Request.ApplicationPath);
                ConfigurationSectionGroup sec = configuration.SectionGroups["system.serviceModel"];
                ServiceModelSectionGroup serviceModelSectionGroup = sec as ServiceModelSectionGroup;
                if (serviceModelSectionGroup != null)
                {
                    ClientSection clientSection = serviceModelSectionGroup.Client;
                    foreach (ChannelEndpointElement item in clientSection.Endpoints)
                    {
                        switch (item.Contract)
                        {
                            case "OMS.IService":

                                return String.IsNullOrWhiteSpace(item.Address.ToString())
                                    ? ""
                                    : item.Address.ToString().Replace("/OMS/OMSService.svc", "");
                            default:
                               
                                break;
                        }
                    }
                }
                return "";
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public string GetPMSUrl()
        {
            try
            {
                System.Configuration.Configuration configuration =
                    WebConfigurationManager.OpenWebConfiguration(base.Request.ApplicationPath);
                ConfigurationSectionGroup sec = configuration.SectionGroups["system.serviceModel"];
                ServiceModelSectionGroup serviceModelSectionGroup = sec as ServiceModelSectionGroup;
                if (serviceModelSectionGroup != null)
                {
                    ClientSection clientSection = serviceModelSectionGroup.Client;
                    foreach (ChannelEndpointElement item in clientSection.Endpoints)
                    {
                        switch (item.Contract)
                        {
                            case "PMS.IService":

                                return String.IsNullOrWhiteSpace(item.Address.ToString())
                                    ? ""
                                    : item.Address.ToString().Replace("/PMS/PMSService.svc", "");
                            default:

                                break;
                        }
                    }
                }
                return "";
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #endregion
    }
}