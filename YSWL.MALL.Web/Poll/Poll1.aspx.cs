using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace YSWL.MALL.Web
{
    public partial class Poll1 : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if ((Request["fid"] != null) && (Request["fid"].ToString() != ""))
            {
                ViewState["fid"] = Request["fid"];
            }
        }                     
        protected void btnNext_Click(object sender, EventArgs e)
        {
            YSWL.MALL.Model.Poll.PollUsers model = new YSWL.MALL.Model.Poll.PollUsers();
            model.Age = int.Parse(txtAge.Text);
            model.Phone = txtPhone.Text;
            if (radbtnMan.Checked)
            {
                model.Sex = "男";
            }
            else
            {
                model.Sex = "女";
            }
            model.TrueName = txtName.Text;
            YSWL.MALL.BLL.Poll.PollUsers bll = new YSWL.MALL.BLL.Poll.PollUsers();
            int id = bll.Add(model);            
            Response.Redirect("Poll2.aspx?u=" + id + "&fid=" + ViewState["fid"]);
            
        }
    }
}
