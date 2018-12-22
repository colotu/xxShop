using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using NPOI.SS.Formula.Functions;

namespace YSWL.MALL.Web.Admin
{
    public partial class SyncUserInfo : PageBaseAdmin
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        public void btnSave_Click(object sender, System.EventArgs e)
        {
            YSWL.MALL.BLL.Members.Users userBll = new BLL.Members.Users();
         List<YSWL.MALL.Model.Members.Users> userList=   userBll.GetModelList("UserType='UU'");
            foreach (var item in userList)
            {
                YSWL.Accounts.Bus.User userinfo=new YSWL.Accounts.Bus.User(item.UserName);
                userinfo.Phone = "";
                userinfo.Update(true);
            }
         
          
        }
    }
}