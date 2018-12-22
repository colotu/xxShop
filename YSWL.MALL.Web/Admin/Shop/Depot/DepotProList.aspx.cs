using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace YSWL.MALL.Web.Admin.Shop.Depot
{
    public partial class DepotProList : PageBaseAdmin
    {
        protected void Page_Load(object sender, EventArgs e)
        {
           BLL.Shop.DisDepot.Depot depotBll = new BLL.Shop.DisDepot.Depot();
           YSWL.MALL.Model.Shop.DisDepot.Depot model = depotBll.GetModel(DepotId);
           if (model == null) {
               return;
           }
           lblTitle.Text ="您正在设置【"+model.Name+"】仓库的商品数据";
            
        }
        protected int DepotId
        {
            get
            {
                if (!string.IsNullOrWhiteSpace(Request.Params["did"]))
                {
                    return YSWL.Common.Globals.SafeInt(Request.Params["did"], 0);
                }
                return 0;
            }
        }
    }
}