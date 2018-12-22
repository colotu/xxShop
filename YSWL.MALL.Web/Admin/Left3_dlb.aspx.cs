using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using System.Data;
using YSWL.Accounts.Bus;
using System.Text;
using System.Collections;
using YSWL.Common;
namespace YSWL.MALL.Web.Admin
{
    public partial class Left3_dlb : PageBaseAdmin
    {
        public string strMenuTree = "";
        public string NodeName = "";
        //Hashtable TreeListofLang;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                YSWL.MALL.BLL.SysManage.SysTree sm = new YSWL.MALL.BLL.SysManage.SysTree();
         
                Page.Title = NodeName;
                //0:admin后台 1:企业后台  2:代理商后台 3:用户后台
                List<YSWL.MALL.Model.SysManage.SysNode> nodeList = sm.GetTreeListByTypeCache(0, true);
               
                #region  根据应用判断，是否移除分仓库存菜单
                var node = nodeList.Find(c => c.NodeID == 901);
                if (!YSWL.MALL.BLL.Shop.Service.CommonHelper.ConnectionOMS())
                {
                    nodeList.Remove(node);
                }
                #endregion
                LoadMenu(nodeList);
            }
        }

        public void LoadMenu(List<YSWL.MALL.Model.SysManage.SysNode> nodeList)
        {
            //bool hasLevel3 = false;
            StringBuilder strtemp = new StringBuilder();
            List<YSWL.MALL.Model.SysManage.SysNode> firstList = nodeList.Where(c => c.ParentID == 0).OrderBy(c => c.OrderID).ToList();
            foreach (var item in firstList)
            {
                if ((item.PermissionID == -1) || (UserPrincipal.HasPermissionID(item.PermissionID)))
                {
                    List<YSWL.MALL.Model.SysManage.SysNode> secNode = nodeList.Where(c => c.ParentID == item.NodeID).OrderBy(c => c.OrderID).ToList();
                    string sectemp = LoadMenu2(secNode,nodeList);
                    strtemp.AppendFormat("<li><a  src=\"{0}\" href=\"javascript:;\" ><i class=\"fa\"><img class=\"menu1\" src=\"img/navbar_r_dlb.png\" alt=\"right\" ></i><span class=\"title\">{1}</span></a>{2}</li>", item.Url, item.TreeText, sectemp);
                }

            }
            strMenuTree = strtemp.ToString();
        }

        public string LoadMenu2(List<YSWL.MALL.Model.SysManage.SysNode> nodeList, List<YSWL.MALL.Model.SysManage.SysNode> allNodes)
        {
            StringBuilder strtemp = new StringBuilder();
            StringBuilder mailtemp = new StringBuilder();
            string childTemp="";
            foreach (var item in nodeList)
            {
                List<YSWL.MALL.Model.SysManage.SysNode> childNode = allNodes.Where(c => c.ParentID == item.NodeID).OrderBy(c => c.OrderID).ToList();
                if ( childNode.Count > 0) //有子分类
                {
                   childTemp= LoadMenu2(childNode, allNodes);
                }
                if ((item.PermissionID == -1) || (UserPrincipal.HasPermissionID(item.PermissionID)))
                {
                    if (String.IsNullOrWhiteSpace(childTemp))
                    {
                        strtemp.AppendFormat("<li><a  src=\"{0}\" href=\"javascript:;\" target=\"mainFrame\">{1}</a></li>", item.Url, item.TreeText);
                    }
                    else
                    {
                        strtemp.AppendFormat("<li><a  src=\"{0}\" href=\"javascript:;\" target=\"mainFrame\"><i class=\"fa\"><img class=\"menu1\" src=\"img/navbar_r_dlb.png\" alt=\"right\" ></i>{1}</a>{2}</li>", item.Url, item.TreeText, childTemp);
                    }
                    
                }
            }
            mailtemp.AppendFormat(" <ul class=\"sub-menu dropdown_ul\">{0}</ul>", strtemp.ToString());
          
            return mailtemp.ToString();
        }

       
    }
}