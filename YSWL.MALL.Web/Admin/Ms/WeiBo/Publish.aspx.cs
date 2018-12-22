﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using YSWL.MALL.BLL.Members;
using YSWL.MALL.Model.Ms;

namespace YSWL.MALL.Web.Admin.Ms.WeiBo
{
    public partial class Publish : PageBaseAdmin
    {

        protected override int Act_PageLoad { get { return 346; } } //微博管理_微博群发_新增页
        private  YSWL.MALL.BLL.Members.UserBind bindBll=new UserBind();
        private  YSWL.MALL.BLL.Ms.WeiBoMsg msgBll=new BLL.Ms.WeiBoMsg();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                //bindBll.GetListEx(CurrentUser.UserID);
            }
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
           
            //新增微博信息
            string desc = this.txtDesc.Text.Trim();
            if (String.IsNullOrWhiteSpace(desc))
            {
                YSWL.Common.MessageBox.ShowFailTip(this, "微博消息不能为空！");
                return;
            }
            YSWL.MALL.Model.Ms.WeiBoMsg msg=new WeiBoMsg();
            msg.CreateDate = DateTime.Now;
            msg.WeiboMsg = desc;
            msg.ImageUrl = this.txtimgUrl.ImageUrl;
            msgBll.Add(msg);
                #region  同步微博
            //现在暂时支持同步微博到用户绑定的全部微博

            //获取用户绑定的所有微博帐号
            List<YSWL.MALL.Model.Members.UserBind> userBinds = bindBll.GetModelList(" userid=" + CurrentUser.UserID);
            if (userBinds == null || userBinds.Count == 0)
            {
                YSWL.Common.MessageBox.ShowFailTip(this,"该帐号没有绑定任何微博，请先绑定微博！");
                return;
            }
            string mediaIDs = String.Join(",", userBinds.Select(c => c.MediaID));
   
            string url = "http://" + Common.Globals.DomainFullName;
                bindBll.SendWeiBo(-1, mediaIDs, desc, url, "");
                #endregion
            YSWL.Common.MessageBox.ShowSuccessTip(this,"发布成功！");
        }
    }
}