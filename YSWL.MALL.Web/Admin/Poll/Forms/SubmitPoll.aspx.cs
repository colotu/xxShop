/**
* Submit.cs
*
* 功 能： [N/A]
* 类 名： Submit
*
* Ver    变更日期             负责人  变更内容
* ───────────────────────────────────
* V0.01  2014/4/10 10:46:50  Rock    初版
*
* Copyright (c) 2014 YS56 Corporation. All rights reserved.
*┌──────────────────────────────────┐
*│　此技术信息为本公司机密信息，未经本公司书面同意禁止向第三方披露．　│
*│　版权所有：小鸟科技（北京）科技有限公司　　　　　　　　　　　　　　│
*└──────────────────────────────────┘
*/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using YSWL.Common;
using YSWL.Json;
using YSWL.Json.Conversion;

namespace YSWL.MALL.Web.Admin.Forms
{
    public partial class SubmitPoll : PageBaseAdmin
    {
        BLL.Poll.PollUsers pollUserBll = new BLL.Poll.PollUsers();
        BLL.Members.Users bll = new BLL.Members.Users();
        #region Load
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(this.Request.Form["Callback"]) && (this.Request.Form["Callback"] == "true"))
            {
                this.Controls.Clear();
                this.DoCallback();
            }
            if (!IsPostBack)
            {
                BLL.Poll.Forms formsBll = new BLL.Poll.Forms();
                Model.Poll.Forms formsModel = formsBll.GetModelByCache(Fid);
                if (null != formsModel)
                {
                    lblFormID.Text = formsModel.FormID.ToString();
                    lblFormName.Text = formsModel.Name;
                }
                if (UId > 0)
                {
                    Model.Members.Users usermodel = bll.GetModelByCache(UId);
                    if (usermodel == null || usermodel.UserID < 0)
                    {
                        ltlNotSelectUser.Text = "该用户不存在，请重新选择";
                    }
                    else
                    {
                        ltlCurrentUser.Text = "当前所选用户：" + usermodel.UserName;
                        if (pollUserBll.ExistsSysUser(UId))
                        {
                            ltlNotSelectUser.Text = "该用户已提交过问卷，请重新选择用户";
                        }
                    }
                }
                else
                {
                    ltlNotSelectUser.Text = "您还没有选择用户";
                }
            }
        }
        #endregion

        #region 获取列表
        public string GetStrList()
        {
            BLL.Poll.Topics topicManage = new BLL.Poll.Topics();
            BLL.Poll.Options optionBll = new BLL.Poll.Options();
            List<Model.Poll.Topics> list = topicManage.GetModelList(-1, Fid);
            StringBuilder str = new StringBuilder();
            List<Model.Poll.Options> optionList;
            if (list == null)
            {
                return "";
            }
            int i = 0;
            foreach (var item in list)
            {
                i++;
                str.AppendFormat(" <tr> <td class=\"tdbg\">{0}. {1} </td> </tr>",i, item.Title);
                optionList = optionBll.GetModelList(string.Format(" TopicID={0}", item.ID));
                if (optionList != null)
                {
                    str.Append(" <tr> <td style=\"padding-left: 20px;\" >");
                    switch (item.Type)
                    {
                        case 0: //单选
                            foreach (var optionItem in optionList)
                            {
                                str.AppendFormat("<input type=\"radio\" id=\"{0}_surveytopic0\" name=\"radionls{1}\" value=\"{0}\" > <label for=\"{0}_surveytopic0\">{2}</label>", optionItem.ID, optionItem.TopicID, optionItem.Name);
                            }
                            str.AppendFormat("<input value=\"0\" type=\"hidden\" name=\"hidtopics0\" topicstype=\"0\"  qnumber=\"{0}\"  topicsid=\"{1}\" class=\"topicsoptions\"/> ", i, item.ID);//<!--用来保存每一个问题的所有答案的name-->
                            break;
                        case 1://多选
                            foreach (var optionItem in optionList)
                            {
                                str.AppendFormat("<input type=\"checkbox\" id=\"{0}_surveytopic1\" name=\"checkbox{1}\" value=\"{0}\"> <label for=\"{0}_surveytopic1\">{2}</label> ", optionItem.ID, optionItem.TopicID, optionItem.Name);
                            }
                            str.AppendFormat("<input value=\"\" type=\"hidden\" name=\"hidtopics1\" topicstype=\"1\"  qnumber=\"{0}\"   topicsid=\"{1}\" class=\"topicsoptions\"/> ", i, item.ID)
                            ;//<!--用来保存每一个问题的所有答案的name-->
                            break;
                        case 2://填写反馈 
                            str.AppendFormat("<input type=\"text\" name=\"hidtopics2\" topicstype=\"2\" value=\"\"  class=\"topicsoptions\" qnumber=\"{0}\"  topicsid=\"{1}\"> ", i, item.ID);
                            break;
                        default:
                            break;
                    }
                    str.Append("  </td> </tr>");
                }
            }
            return str.ToString();
        }
        #endregion

        #region 获取参数
        public int Fid
        {
            get
            {
                if (Request.QueryString["fid"] != null)
                {
                    return Globals.SafeInt(Request.QueryString["fid"], 0);
                }
                return 0;
            }
        }
        private int UId
        {
            get
            {
                if (Request.Params["uid"] != null)
                {
                    return Globals.SafeInt(Request.Params["uid"], 0);
                }
                return 0;
            }
        }
        #endregion

        #region AjaxCallback

        private void DoCallback()
        {
            string action = this.Request.Form["Action"];
            this.Response.Clear();
            this.Response.ContentType = "application/json";
            string writeText = string.Empty;

            switch (action)
            {
                case "SubmitPolls":
                    writeText = SubmitPolls();
                    break;
               
            }
            this.Response.Write(writeText);
            this.Response.End();
        }
      
        private string SubmitPolls()
        {
            Model.Poll.PollUsers polluserModel = new Model.Poll.PollUsers();
            Model.Members.Users usermodel = bll.GetModelByCache(UId);
            if (usermodel == null || usermodel.UserID < 0)
            {
                return "NotSelectUser";//没有选择用户
            }
            if (pollUserBll.ExistsSysUser(UId))
            {
                return "Repeat";//该用户已提交过问卷，请重新选择用户
            }
            string data = Request.Form["TopicIDjson"];
            if (String.IsNullOrWhiteSpace(data))
            {
                return "false";
            }
            polluserModel.Email = usermodel.Email;
            polluserModel.Phone = usermodel.Phone;
            switch (usermodel.Sex)
            {
                case "0":
                    polluserModel.Sex = "女";
                    break;
                case "1":
                     polluserModel.Sex = "男";
                    break;
                default:
                    break;
            }
            polluserModel.SysUserId = usermodel.UserID;
            polluserModel.TrueName = usermodel.TrueName;
            polluserModel.UserName = usermodel.UserName;
            polluserModel.UserType = usermodel.UserType;
            int userid = pollUserBll.Add(polluserModel);//创建投票用户
            if (userid < 0)
            {
                return "false";
            }
            Model.Poll.UserPoll modelup = new Model.Poll.UserPoll();
            BLL.Poll.UserPoll bllup = new BLL.Poll.UserPoll();
            modelup.UserIP = Request.UserHostAddress;
            modelup.UserID = userid;
            JsonArray jsonArray = JsonConvert.Import<JsonArray>(data);
            foreach (JsonObject jsonObject in jsonArray)
            {
                int topicid = Common.Globals.SafeInt(jsonObject["topicid"].ToString(), 0);
                string topicvlaue = jsonObject["topicvlaue"].ToString();
                int type = Common.Globals.SafeInt(jsonObject["type"].ToString(), -1);
                modelup.TopicID = topicid;  //TopicID; //问题的ID
                switch (type)
                {
                    case 0://单选
                        modelup.OptionID = Common.Globals.SafeInt(topicvlaue, -1); // Option; //答案
                        bllup.Add(modelup);
                        break;
                    case 1://多选
                        modelup.OptionIDList = topicvlaue; // Option; //答案
                        if (!String.IsNullOrWhiteSpace(topicvlaue))
                        {
                          bllup.Add2(modelup);
                        }
                        break;
                    case 2://反馈  待补充  暂时只支持单选、多选
                        break;
                    default:
                        break;
                }
            }
            return "true"; //成功   
        }

        #endregion AjaxCallback
    }
}