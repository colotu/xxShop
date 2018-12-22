using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using YSWL.MALL.Web.Components;

namespace YSWL.MALL.Web.Admin
{
    public partial class Top3 :PageBaseAdmin
    {
        public string username = "";
        public string strMenu = "";

        protected void Page_Load(object sender, EventArgs e)
        {
            username = string.IsNullOrWhiteSpace(CurrentUser.TrueName) ? CurrentUser.UserName : CurrentUser.TrueName;
            if (!IsPostBack)
            {
                YSWL.MALL.BLL.SysManage.SysTree sm = new YSWL.MALL.BLL.SysManage.SysTree();

                //0:admin后台 1:企业后台  2:代理商后台 3:用户后台
                DataSet ds = sm.GetEnabledTreeByParentId(0, 0, true);
                LoadTree(ds.Tables[0]);
             
            }
            //  hfCurrentID.Value = CurrentUser.UserID.ToString();

            //this.lblTotal.Text = (new YSWL.MALL.BLL.Messages.ReceivedMessages().GetTotal(Convert.ToInt64(CurrentUser.UserID))).ToString();
        }

        public void LoadTree(DataTable dt)
        {
            int n = 1;
            StringBuilder strtemp = new StringBuilder();
            string url = "Left2.aspx";
            foreach (DataRow r in dt.Rows)
            {
                string nodeid = r["NodeID"].ToString();
                string text = r["TreeText"].ToString();

                //if (TreeListofLang[nodeid] != null)
                //{
                //    text = TreeListofLang[nodeid].ToString();
                //}
                string parentid = r["ParentID"].ToString();
                string location = r["Location"].ToString();
                if (r["Url"] != null && r["Url"].ToString().Length > 0)
                {
                    url = r["Url"].ToString();
                }

                int permissionid = -1;
                string imageurl = r["ImageUrl"].ToString();
                if (r["PermissionID"] != null)
                {
                    permissionid = int.Parse(r["PermissionID"].ToString().Trim());
                }

                if ((permissionid == -1) || (UserPrincipal.HasPermissionID(permissionid)))
                {
                    strtemp.Append("<li id=\"Tab" + n.ToString() + "\">");
                    strtemp.Append("<a href=\"left2.aspx?id=" + nodeid + "\" target=\"leftFrame\" onclick=\"javascript:switchTab('TabPage1','Tab" + n.ToString() + "');\">" + text);
                    strtemp.Append("</a></li>");
                }
                n++;
            }
            strtemp.AppendFormat("<script language=\"JavaScript\" type=\"text/javascript\">window.top.document.title='{0}';</script>"
                , (String.IsNullOrWhiteSpace(MvcApplication.SiteName) ? "云订货管理系统" : MvcApplication.SiteName));
            strMenu = strtemp.ToString();
        }
    }
}