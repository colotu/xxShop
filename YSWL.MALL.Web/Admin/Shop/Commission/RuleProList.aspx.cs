using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using YSWL.Common;

namespace YSWL.MALL.Web.Admin.Shop.Commission
{
    public partial class RuleProList : PageBaseAdmin
    {
        public int RuleId = 0;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                if (!string.IsNullOrWhiteSpace(Request.QueryString["id"]))
                {
                    RuleId = Globals.SafeInt(Request.QueryString["id"], 0);
                }
            }
        }
    }
}