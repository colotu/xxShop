using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using YSWL.MALL.BLL.Ms;

namespace YSWL.MALL.Web.Admin.Shop.Depot
{
    public partial class AddDepotRegion : PageBaseAdmin
    {
        private YSWL.MALL.BLL.Shop.DisDepot.Depot depotBll = new BLL.Shop.DisDepot.Depot();
        private YSWL.MALL.BLL.Shop.DisDepot.DepotRegion depotRegionBll = new BLL.Shop.DisDepot.DepotRegion();
        private YSWL.MALL.BLL.Ms.Regions regionBll=new Regions();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                bool IsConnectionOMS = YSWL.MALL.BLL.Shop.Service.CommonHelper.ConnectionOMS();//是否开启分仓
                bool IsOpenMultiDepot = YSWL.MALL.BLL.Shop.Service.CommonHelper.OpenMultiDepot();//是否对接oms
                if (!IsConnectionOMS && IsOpenMultiDepot)
                {
                    btnSave.Visible = true;
                }
                else {
                    Common.MessageBox.Show(this, "此系统对接了OMS系统，请到OMS系统进行分仓地区的设置！");
                    return;
                }


                this.ddlDepot.DataSource = depotBll.GetList("Status=1");
                ddlDepot.DataTextField = "Name";
                ddlDepot.DataValueField = "DepotId";
                ddlDepot.DataBind();
            }
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            int depotId = Common.Globals.SafeInt(ddlDepot.SelectedValue, 0);
            if (depotId == 0)
            {
                Common.MessageBox.ShowFailTip(this, "请选择仓库");
                return;
            }
            int regionId = RegionID.Region_iID;
            if (regionId == 0)
            {
                Common.MessageBox.ShowFailTip(this, "请选择仓库对应地区");
                return;
            }
            if (YSWL.MALL.BLL.Shop.DisDepot.DepotRegion.ExistsDepotByRegion(regionId) > 0)
            {
                Common.MessageBox.ShowFailTip(this, "该地区下已存在对应仓库，请不要重复设置");
                return;
            }
            YSWL.MALL.Model.Shop.DisDepot.DepotRegion model = new Model.Shop.DisDepot.DepotRegion();
            model.DepotId = depotId;
            model.RegionId = regionId;
            YSWL.MALL.Model.Ms.Regions regionModel = regionBll.GetModelByCache(regionId);
            if (regionModel == null)
            {
                Common.MessageBox.ShowFailTip(this, "该地区不存在");
                return;
            }
            model.RegionName = regionBll.GetAddress(regionId);
            model.Depth = regionModel.Depth;
            model.Path = regionModel.Path;
            model.Status = chkStatus.Checked;
            if (depotRegionBll.Add(model))
            {
                //清空缓存
                Cache.Remove("GetAllDepotRegion-DepotRegion");
                Common.MessageBox.ShowSuccessTipScript(this, "操作成功", "window.parent.location.reload();");
                return;
            }
            Common.MessageBox.ShowFailTip(this, "操作失败，请稍候再试！");
        }
    }
}