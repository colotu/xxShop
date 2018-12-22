using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace YSWL.MALL.Web
{
    public partial class Poll2 : System.Web.UI.Page
    {
        YSWL.MALL.BLL.Poll.Topics blltop = new YSWL.MALL.BLL.Poll.Topics();
        YSWL.MALL.BLL.Poll.Options blloption = new YSWL.MALL.BLL.Poll.Options();
        YSWL.MALL.BLL.Poll.UserPoll bllup = new YSWL.MALL.BLL.Poll.UserPoll();
        YSWL.MALL.BLL.Poll.Reply bllreply = new YSWL.MALL.BLL.Poll.Reply();
        public string nowtime = DateTime.Now.ToString();

        protected void Page_Load(object sender, EventArgs e)
        {
            nowtime = DateTime.Now.ToString();
            if (!IsPostBack)
            {
                if ((Request["fid"] != null) && (Request["fid"].ToString() != ""))
                {
                    ViewState["fid"] = Request["fid"];
                    if ((Request["u"] != null) && (Request["u"].ToString() != ""))
                    {
                        ViewState["uid"] = Request["u"];
                    }
                    else
                    {
                        ViewState["uid"] = 0;
                    }
                    //if (Request.QueryString["s"] != null)
                    //    Response.Write("<script language=javascript>window.open('show.aspx?id=" + ViewState["Tid"].ToString() + "','_blank','')</" + "script>");
                }
            }
        }
        
        public void BindData()
        {
            rptVote.DataSource = blltop.GetListByForm(Convert.ToInt32(ViewState["fid"]));
            rptVote.DataBind();
        }
        public void rptVote_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            string tid = ((Label)e.Item.FindControl("labID")).Text;
            string Type = ((Label)e.Item.FindControl("labType")).Text;
            switch (Type)
            {
                case "0"://单选
                    RadioButtonList rbChoice = (RadioButtonList)e.Item.FindControl("rbChoice");
                    rbChoice.Visible = true;
                    rbChoice.DataSource = blloption.GetListByTopic(Convert.ToInt32(tid)).Tables[0];
                    rbChoice.DataBind();
                    //string DefaultChoice = choice.GetDefaultChoice(Tid);
                    //if (DefaultChoice != "")
                    //    rbChoice.Items.FindByValue(DefaultChoice).Selected = true;
                    break;
                case "1"://多选
                    CheckBoxList cbChoice = (CheckBoxList)e.Item.FindControl("cbChoice");
                    cbChoice.Visible = true;
                    cbChoice.DataSource = blloption.GetListByTopic(Convert.ToInt32(tid)).Tables[0];
                    cbChoice.DataBind();
                    break;
                default://问题
                    TextBox txtContent = (TextBox)e.Item.FindControl("txtContent");
                    txtContent.Visible = true;
                    break;
            }
        }
              
        public bool Select()
        {
            for (int i = 0; i < rptVote.Items.Count; i++)
            {
                string Tid = ((Label)rptVote.Items[i].FindControl("labID")).Text;
                string Type = ((Label)rptVote.Items[i].FindControl("labType")).Text;
                switch (Type)
                {
                    case "0"://单选
                        RadioButtonList rbChoice = (RadioButtonList)rptVote.Items[i].FindControl("rbChoice");
                        if (rbChoice.SelectedIndex < 0)
                            return false;
                        break;
                    case "1"://多选
                        CheckBoxList cbChoice = (CheckBoxList)rptVote.Items[i].FindControl("cbChoice");
                        if (cbChoice.SelectedIndex < 0)
                            return false;
                        break;
                    default://问题
                        TextBox txtContent = (TextBox)rptVote.Items[i].FindControl("txtContent");
                        if (txtContent.Text.Trim() == "")
                            return false;
                        break;
                }
            }
            return true;
        }

        protected void btnOk_Click1(object sender, EventArgs e)
        {
            if (base.Request.Cookies["vote" + ViewState["fid"].ToString()] != null)
            {
                HttpCookie httpCookie = base.Request.Cookies["vote" + ViewState["fid"].ToString()];
                if (httpCookie.Values["voteid"].ToString() != "" || httpCookie.Values["voteid"].ToString() != null)
                {
                    YSWL.Common.MessageBox.Show(this, "您已经投过票，请不要重复投票！");
                    return;
                }
            }
            if (!Select())
            {
                YSWL.Common.MessageBox.Show(this, "对不起，请填写完全部投票结果！");
                return;
            }
            YSWL.MALL.Model.Poll.UserPoll modelup = null;
            YSWL.MALL.Model.Poll.Reply modelre = null;
            for (int i = 0; i < rptVote.Items.Count; i++)
            {
                string tid = ((Label)rptVote.Items[i].FindControl("labID")).Text;
                string Type = ((Label)rptVote.Items[i].FindControl("labType")).Text;
                modelup = new YSWL.MALL.Model.Poll.UserPoll();
                switch (Type)
                {
                    case "0"://单选
                        RadioButtonList rbChoice = (RadioButtonList)rptVote.Items[i].FindControl("rbChoice");
                        modelup.UserID = Convert.ToInt32(ViewState["uid"]);
                        modelup.TopicID = Convert.ToInt32(tid);
                        modelup.OptionID = Convert.ToInt32(rbChoice.SelectedValue);
                        modelup.UserIP = Request.UserHostAddress;
                        bllup.Add(modelup);
                        break;
                    case "1"://多选
                        CheckBoxList cbChoice = (CheckBoxList)rptVote.Items[i].FindControl("cbChoice");
                        for (int j = 0; j < cbChoice.Items.Count; j++)
                        {
                            if (cbChoice.Items[j].Selected)
                            {
                                modelup.UserID = Convert.ToInt32(ViewState["uid"]);
                                modelup.TopicID = Convert.ToInt32(tid);
                                modelup.OptionID = Convert.ToInt32(cbChoice.Items[j].Value);
                                modelup.UserIP = Request.UserHostAddress;
                                bllup.Add(modelup);
                            }
                        }
                        break;
                    default://问题
                        TextBox txtContent = (TextBox)rptVote.Items[i].FindControl("txtContent");
                        modelre = new YSWL.MALL.Model.Poll.Reply();
                        modelre.ReContent = txtContent.Text;
                        modelre.TopicID = Convert.ToInt32(tid);
                        bllreply.Add(modelre);
                        break;
                }
            }
            HttpCookie httpCookie1 = new HttpCookie("vote" + ViewState["fid"].ToString());
            httpCookie1.Values.Add("voteid", ViewState["fid"].ToString());
            httpCookie1.Expires = DateTime.Now.AddHours(240);
            Response.Cookies.Add(httpCookie1);
            Response.Redirect("Poll3.aspx?u=" + ViewState["uid"] + "&fid=" + ViewState["fid"].ToString());
            //LTP.Common.MessageBox.ShowAndRedirect(this, "投票成功，感谢您的参与！", "Poll3.aspx?u=" + ViewState["uid"] + "&fid=" + ViewState["fid"].ToString());
            //Response.Write("<script language=javascript>window.open('show.aspx?id="+ViewState["Tid"].ToString()+"','_blank','')</" + "script>");

        }

        
    }
}
