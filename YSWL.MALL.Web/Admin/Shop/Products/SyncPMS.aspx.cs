using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace YSWL.MALL.Web.Admin.Shop.Products
{
    public partial class SyncPMS : PageBaseAdmin
    {
        protected override int Act_PageLoad { get { return -1; } } 

        protected void Page_Load(object sender, EventArgs e)
        {
        }

        protected void btnSync_Click(object sender, System.EventArgs e)
        {
            try
            {
               bool IsSyncPMS = YSWL.MALL.BLL.SysManage.ConfigSystem.GetBoolValueByCache("Shop_ConnectionPMS");
                if (IsSyncPMS)
                {
                   // YSWL.MALL.BLL.Shop.Service.PMSServiceHelper.SyncAllData();
                    Common.MessageBox.ShowSuccessTip(this, "同步PMS数据成功");
                }
                else
                {
                    Common.MessageBox.ShowFailTipScript(this, "未开启PMS对接，请先设置", "window.parent.location.href='/Admin/SysManage/DepotConfig.aspx';");
                }
            }
            catch (Exception ex)
            {
                LogHelp.AddErrorLog("同步PMS基础数据失败："+ex.Message,ex.StackTrace,this);
                Common.MessageBox.ShowSuccessTip(this, "同步PMS数据失败");
                throw;
            }
        }
    }
}