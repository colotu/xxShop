using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace YSWL.MALL.Web.Admin.Statistics
{
    public partial class UserPositionList : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            int supplierId = YSWL.Common.Globals.SafeInt(Request.Params["supplierId"], 0);
            this.hiddenSpId.Value = supplierId.ToString();
        }
    }
}