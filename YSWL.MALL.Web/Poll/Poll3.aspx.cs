using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace YSWL.MALL.Web
{
    public partial class Poll3 : System.Web.UI.Page
    {
        public string nowtime = DateTime.Now.ToString();
        public string userid = "0";
        protected void Page_Load(object sender, EventArgs e)
        {
            nowtime = DateTime.Now.ToString();
            if ((Request["fid"] != null) && (Request["fid"].ToString() != ""))
            {
                ViewState["fid"] = Request["fid"];
                if ((Request["u"] != null) && (Request["u"].ToString() != ""))
                {                   
                    userid = Request["u"];
                }
            }
        }
    }
}
