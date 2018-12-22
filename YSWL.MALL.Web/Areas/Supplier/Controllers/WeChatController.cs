using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using YSWL.Json;
using YSWL.WeChat.BLL;
using YSWL.Common;
using Webdiyer.WebControls.Mvc;
using System.Text.RegularExpressions; 
using System.Data;
using YSWL.MALL.BLL.Shop.Products;
using YSWL.MALL.BLL.CMS;
using System.IO;
using System.Text;
using YSWL.WeChat.BLL.Core;

namespace YSWL.MALL.Web.Areas.Supplier.Controllers
{

    public class WeChatController : SupplierControllerBase
    {
        //
        // GET: /Shop/WeChat/
        YSWL.WeChat.BLL.Core.Group groupBll = new YSWL.WeChat.BLL.Core.Group();
        YSWL.WeChat.BLL.Core.User userBll = new YSWL.WeChat.BLL.Core.User();
        YSWL.WeChat.BLL.Core.RequestMsg msgBll = new YSWL.WeChat.BLL.Core.RequestMsg();

        #region 微信设置
        public ViewResult Setting()
        {
            ViewBag.WeChatUrl = "http://" + Common.Globals.DomainFullName + "/wcapi.aspx";
            ViewBag.WCOriId = YSWL.WeChat.BLL.Core.Config.GetValueByCache("WeChat_OpenId", SupplierId, currentUser.UserType);
            ViewBag.WCAppId = YSWL.WeChat.BLL.Core.Config.GetValueByCache("WeChat_AppId", SupplierId, currentUser.UserType);
            ViewBag.WCAppSercet = YSWL.WeChat.BLL.Core.Config.GetValueByCache("WeChat_AppSercet", SupplierId, currentUser.UserType);
            ViewBag.WCToken = BLL.SysManage.ConfigSystem.GetValueByCache("System_WeChat_Token");

            ViewBag.ChkNoMsg = YSWL.WeChat.BLL.Core.Config.GetValueByCache("WeChat_ChkNoMsg", SupplierId, currentUser.UserType);
            ViewBag.ChkSubscribe = YSWL.WeChat.BLL.Core.Config.GetValueByCache("WeChat_ChkSubscribe", SupplierId, currentUser.UserType);
            ViewBag.WCEmail = YSWL.WeChat.BLL.Core.Config.GetValueByCache("WeChat_Email", SupplierId, currentUser.UserType);
            return View();
        }

        [HttpPost]
        public ViewResult Setting(FormCollection Fm)
        {
            YSWL.WeChat.BLL.Core.Config.Modify("WeChat_OpenId", Fm["WCOriId"], SupplierId, currentUser.UserType, "微信原始ID", false);
            YSWL.WeChat.BLL.Core.Config.Modify("WeChat_AppId", Fm["WCAppId"], SupplierId, currentUser.UserType, "微信AppId", false);
            YSWL.WeChat.BLL.Core.Config.Modify("WeChat_AppSercet", Fm["WCAppSercet"], SupplierId, currentUser.UserType, "微信AppSercet", false);
            YSWL.WeChat.BLL.Core.Config.Modify("WeChat_ChkNoMsg", Fm["ChkNoMsg"], SupplierId, currentUser.UserType, "系统默认消息邮件");
            YSWL.WeChat.BLL.Core.Config.Modify("WeChat_ChkSubscribe", Fm["ChkSubscribe"], SupplierId, currentUser.UserType, "用户关注消息邮件");

            YSWL.WeChat.BLL.Core.Config.Modify("WeChat_Email", Fm["WCEmail"], SupplierId, currentUser.UserType, "用户接收邮件邮箱");

            ViewBag.WeChatUrl = "http://" + Common.Globals.DomainFullName + "/wcapi.aspx";
            ViewBag.WCOriId = Fm["WCOriId"];
            ViewBag.WCAppId = Fm["WCAppId"];
            ViewBag.WCAppSercet = Fm["WCAppSercet"];
            ViewBag.WCToken = BLL.SysManage.ConfigSystem.GetValueByCache("System_WeChat_Token");

            ViewBag.WCEmail = Fm["WCEmail"];

            ViewBag.ChkNoMsg = Fm["ChkNoMsg"];
            ViewBag.ChkSubscribe = Fm["ChkSubscribe"];
            //清空缓存
            YSWL.WeChat.BLL.Core.Config.ClearCache();
            return View();
        }
        #endregion

        #region 微信

        #region 用户小组
        public ViewResult GroupList(int pageIndex = 1, string viewName = "GroupList")
        {
            int _pageSize = 10;
            //计算分页起始索引
            int startIndex = pageIndex > 1 ? (pageIndex - 1) * _pageSize + 1 : 1;
            //计算分页结束索引
            int endIndex = pageIndex * _pageSize;
            int toalCount = groupBll.GetCount(OpenId, "");
            if (toalCount < 1)
            {
                return View(viewName);//NO DATA
            }
            List<YSWL.WeChat.Model.Core.Group> groupList = groupBll.GetGroupList(OpenId, "", startIndex, endIndex);
            PagedList<YSWL.WeChat.Model.Core.Group> lists = new PagedList<YSWL.WeChat.Model.Core.Group>(groupList, pageIndex, _pageSize, toalCount);
            return View(viewName, lists);
        }

        [HttpPost]
        public ActionResult GroupList(string GroupName, string Remark, string viewName = "GroupList")
        {
            YSWL.WeChat.Model.Core.Group groupModel = new WeChat.Model.Core.Group();
            groupModel.GroupName = GroupName;
            groupModel.Remark = Remark;
            groupModel.OpenId = OpenId;
            JsonObject json = new JsonObject();
            if (groupBll.Add(groupModel) > 0)
            {
                json.Put("Result", "OK");
                return Json(json);
            }
            else
            {
                json.Put("Result", "NO");
                return Json(json);
            }
        }
        #endregion

        #region 用户小组修改
        public ActionResult UpdateGroup(int id)
        {
            YSWL.WeChat.Model.Core.Group groupModel = groupBll.GetModel(id);
            return View("UpdateGroup", groupModel);
        }

        [HttpPost]
        public ActionResult UpdateGroup(int hfGroupId, string GroupName = "", string Remark = "", string viewName = "GroupList")
        {
            YSWL.WeChat.Model.Core.Group groupModel = groupBll.GetModel(hfGroupId);
            groupModel.GroupName = GroupName;
            groupModel.Remark = Remark;
            JsonObject json = new JsonObject();
            if (groupBll.Update(groupModel))
            {
                json.Put("Result", "OK");
                return Json(json);
            }
            else
            {
                json.Put("Result", "NO");
                return Json(json);
            }
        }
        #endregion

        #region 微信用户消息
        public ViewResult RequestMsg()
        {
            return View();
        }

        public PartialViewResult LoadMsgContent(int pageIndex = 1, string viewName = "_MsgList", string txtFrom = "", bool isEvent = false, string txtTo = "", string txtKeyWord = "")
        {
            int pageSize = 10;
            int startIndex = pageIndex > 1 ? (pageIndex - 1) * pageSize + 1 : 1;
            int endIndex = pageIndex * pageSize;

            int totalCount = msgBll.GetCount(OpenId, txtFrom, txtTo, isEvent, txtKeyWord);
            List<YSWL.WeChat.Model.Core.RequestMsg> list = msgBll.GetMsgList(OpenId, txtFrom, txtTo, isEvent, txtKeyWord, startIndex, endIndex);
            if (null == list)
            {
                return PartialView(viewName);
            }
            PagedList<YSWL.WeChat.Model.Core.RequestMsg> pagedList = new PagedList<WeChat.Model.Core.RequestMsg>(list, pageIndex, pageSize, totalCount);
            if (Request.IsAjaxRequest())
            {
                return PartialView(viewName, pagedList);
            }
            return PartialView(viewName, pagedList);
        }


        #region 微信用户消息批量删除
        public ActionResult MsgDelete(string ids)
        {
            ids = ids.Substring(0, ids.LastIndexOf(","));
            JsonObject json = new JsonObject();
            if (msgBll.DeleteList(ids))
            {
                json.Put("Result", "OK");
                return Json(json);
            }
            else
            {
                json.Put("Result", "NO");
                return Json(json);
            }
        }
        #endregion

        #region 指令操作
        public ViewResult ActionList(int pageIndex = 1, string viewName = "ActionList")
        {
            //YSWL.WeChat.BLL.Core.Action actionBll = new WeChat.BLL.Core.Action();
            //int _pageSize = 10;
            ////计算分页起始索引
            //int startIndex = pageIndex > 1 ? (pageIndex - 1) * _pageSize + 1 : 1;
            ////计算分页结束索引
            //int endIndex = pageIndex * _pageSize;
            //int totalCount = actionBll.getCount("", "");
            //if (toalCount < 1)
            //{
            //    return View(viewName);//NO DATA
            //}
            //List<YSWL.WeChat.Model.Core.Action> groupList = actionBll.GetActionList("", "", startIndex, endIndex);
            //PagedList<YSWL.WeChat.Model.Core.Group> lists = new PagedList<YSWL.WeChat.Model.Core.Group>(groupList, pageIndex, _pageSize, toalCount);
            //return View(viewName, lists);
            return View(viewName);
        }

        [HttpPost]
        public ActionResult ActionList(string tName, string Remark, string viewName = "ActionList")
        {
            YSWL.WeChat.BLL.Core.Action actionBll = new YSWL.WeChat.BLL.Core.Action();
            YSWL.WeChat.Model.Core.Action actionModel = new WeChat.Model.Core.Action();
            actionModel.Name = tName;
            actionModel.Remark = Remark;
            JsonObject json = new JsonObject();
            if (actionBll.Add(actionModel) > 0)
            {
                json.Put("Result", "OK");
                return Json(json);
            }
            else
            {
                json.Put("Result", "NO");
                return Json(json);
            }
        }
        #endregion

        #region 批量删除指令
        public ActionResult ActionDelete(string ids)
        {
            YSWL.WeChat.BLL.Core.Action bll = new YSWL.WeChat.BLL.Core.Action();
            ids = ids.Substring(0, ids.LastIndexOf(","));
            JsonObject json = new JsonObject();
            if (bll.DeleteList(ids))
            {
                json.Put("Result", "OK");
                return Json(json);
            }
            else
            {
                json.Put("Result", "NO");
                return Json(json);
            }
        }
        #endregion

        #region 操作指令修改
        public ActionResult UpdateAction(int id)
        {
            YSWL.WeChat.BLL.Core.Action actionBll = new YSWL.WeChat.BLL.Core.Action();
            YSWL.WeChat.Model.Core.Action actionModel = actionBll.GetModel(id);
            return View("UpdateAction", actionModel);
        }

        [HttpPost]
        public ActionResult UpdateAction(YSWL.WeChat.Model.Core.Action model, int ActionId, string viewName = "ActionList")
        {
            YSWL.WeChat.BLL.Core.Action actionBll = new YSWL.WeChat.BLL.Core.Action();
            YSWL.WeChat.Model.Core.Action actionModel = actionBll.GetModel(ActionId);
            actionModel.Name = model.Name;
            actionModel.Remark = model.Remark;
            JsonObject json = new JsonObject();
            if (actionBll.Update(actionModel))
            {
                json.Put("Result", "OK");
                return Json(json);
            }
            else
            {
                json.Put("Result", "NO");
                return Json(json);
            }
        }
        #endregion

        #endregion

        #endregion

        #region 系统消息
        public ViewResult MsgList()
        {
            return View();
        }
        public PartialViewResult loadMsg(string viewName = "_LoadMsg")
        {
            YSWL.WeChat.BLL.Core.SysMsg bll = new YSWL.WeChat.BLL.Core.SysMsg();
            string IndexPath = YSWL.Components.MvcApplication.GetCurrentRoutePath(YSWL.Web.AreaRoute.MPageSP);
            string openId = YSWL.WeChat.BLL.Core.Config.GetValueByCache("WeChat_OpenId", -1, CurrentUser.UserType);
            ViewBag.OpenId = openId;
            List<YSWL.WeChat.Model.Core.SysMsg> sysMsgs = new List<WeChat.Model.Core.SysMsg>();// bll.GetSysMsgList(openId);
            if (sysMsgs != null && sysMsgs.Count > 0)
            {
                YSWL.WeChat.Model.Core.SysMsg Subscription = sysMsgs.FirstOrDefault(c => c.SysType == 1);
                if (Subscription != null)
                {
                    if (Subscription.MsgType == "text")
                    {
                        ViewBag.Subscription = Subscription.Description;
                    }
                    if (Subscription.MsgType == "news")
                    {
                        YSWL.WeChat.Model.Core.MsgItem item = Subscription.MsgItems != null && Subscription.MsgItems.Count > 0 ? Subscription.MsgItems[Subscription.MsgItems.Count-1] : new YSWL.WeChat.Model.Core.MsgItem();
                        ViewBag.title = item.Title;
                        if (String.IsNullOrEmpty(item.PicUrl))
                        {
                            ViewBag.path = "/";
                        }
                        else
                        {
                            ViewBag.path = item.PicUrl;
                        }
                        if (String.IsNullOrEmpty(item.PicUrl))
                        {
                            ViewBag.url = String.Format("/", "T_");
                        }
                        else
                        {
                            ViewBag.url = String.Format(item.PicUrl, "T_");
                        }
                        ViewBag.Subscription = Subscription.Description;
                    }
                }
                YSWL.WeChat.Model.Core.SysMsg ReplyMsg = sysMsgs.FirstOrDefault(c => c.SysType == 2);

                if (ReplyMsg != null)
                {
                    if (ReplyMsg.MsgType == "text")
                    {
                        ViewBag.ReplyMsg = ReplyMsg.Description;
                    }
                    if (ReplyMsg.MsgType == "news")
                    {
                        YSWL.WeChat.Model.Core.MsgItem item = ReplyMsg.MsgItems != null && ReplyMsg.MsgItems.Count > 0 ? ReplyMsg.MsgItems[ReplyMsg.MsgItems.Count-1] : new YSWL.WeChat.Model.Core.MsgItem();
                        ViewBag.Rtitle = item.Title;
                        //this.txtUrl_R.Text = String.IsNullOrWhiteSpace(item.Url) ? IndexPath : item.Url;
                        if (String.IsNullOrEmpty(item.PicUrl))
                        {
                            ViewBag.pathR = "/";
                        }
                        else
                        {
                            ViewBag.pathR = item.PicUrl;
                        }
                        if (String.IsNullOrEmpty(item.PicUrl))
                        {
                            ViewBag.urlR = String.Format("/", "T_");
                        }
                        else
                        {
                            ViewBag.urlR = String.Format(item.PicUrl, "T_");
                        }
                       
                        ViewBag.ReplyMsg = ReplyMsg.Description;
                    }
                }
            }
            return PartialView(viewName);
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult MsgList(FormCollection fm, string viewName = "MsgList")
        {
            string savePath = "/Upload/WeChat/" + DateTime.Now.ToString("yyyyMMdd") + "/";
            bool IsSuccess = true;
            string openId = YSWL.WeChat.BLL.Core.Config.GetValueByCache("WeChat_OpenId", -1, CurrentUser.UserType);

            YSWL.WeChat.BLL.Core.SysMsg bll = new YSWL.WeChat.BLL.Core.SysMsg();
            //List<YSWL.WeChat.Model.Core.SysMsg> sysMsgs = new List<WeChat.Model.Core.SysMsg>();//bll.GetSysMsgList(OpenId);
            //YSWL.WeChat.Model.Core.SysMsg Subscription = new WeChat.Model.Core.SysMsg();
            //YSWL.WeChat.Model.Core.SysMsg ReplyMsg = new WeChat.Model.Core.SysMsg();
            //if (sysMsgs != null && sysMsgs.Count > 0)
            //{
            //    Subscription = sysMsgs.FirstOrDefault(c => c.SysType == 1) == null ? Subscription : sysMsgs.FirstOrDefault(c => c.SysType == 1);
            //    ReplyMsg = sysMsgs.FirstOrDefault(c => c.SysType == 2) == null ? ReplyMsg : sysMsgs.FirstOrDefault(c => c.SysType == 2);
            //}
            //Subscription.OpenId = OpenId;
            //ReplyMsg.OpenId = OpenId;
            //Subscription.Description = RemoveSpecifyHtml(fm["txtSubscription"]).Replace("&nbsp;", "");
            //ReplyMsg.Description = RemoveSpecifyHtml(fm["txtReplyMsg"]).Replace("&nbsp;", "");

            ////处理 文本消息
            //if (fm["type_S"] == "text")
            //{
            //    if (Subscription != null && Subscription.SysMsgId > 0)
            //    {
            //        IsSuccess = bll.Update(Subscription) ? IsSuccess : false;
            //    }
            //    else
            //    {
            //        Subscription.SysType = 1;
            //        Subscription.MsgType = "text";
            //        Subscription.CreateTime = DateTime.Now;
            //        IsSuccess = bll.Add(Subscription) > 0 ? IsSuccess : false;
            //    }
            //}
            //if (fm["type_S"] == "news")
            //{
            //    string title = fm["txtTitle_S"];
            //    bll.DeleteEx(Subscription.SysMsgId, 2);
            //    Subscription.SysType = 1;
            //    Subscription.OpenId = openId;
            //    Subscription.CreateTime = DateTime.Now;
            //    Subscription.MsgType = "news";
            //    //暂时考虑单图文的情况
            //    YSWL.WeChat.Model.Core.MsgItem item = new YSWL.WeChat.Model.Core.MsgItem();
            //    item.Description = Common.InjectionFilter.HtmlFilter(fm["txtSubscription"]);
            //    item.Title = title;
            //    if (fm["sele_S"] == "0")
            //    {
            //        item.Url = YSWL.Components.MvcApplication.GetCurrentRoutePath(YSWL.Web.AreaRoute.Shop);
            //    }
            //    else
            //    {
            //        item.Url = fm["url_S"];
            //    }

            //    //移动图片 
            //    string tempImg = fm["path_s"];
            //    string imgname = tempImg.Substring(tempImg.LastIndexOf("/") + 1);
            //    string saveImg = tempImg;
            //    if (String.IsNullOrWhiteSpace(tempImg) && tempImg.Contains("/Upload/Temp"))
            //    {
            //        if (!Directory.Exists(System.Web.HttpContext.Current.Server.MapPath(savePath)))
            //        {
            //            Directory.CreateDirectory(System.Web.HttpContext.Current.Server.MapPath(savePath));
            //        }
            //        if (System.IO.File.Exists(System.Web.HttpContext.Current.Server.MapPath(String.Format(tempImg, "N_"))))
            //        {
            //            string originalUrl = String.Format(savePath + imgname, "N_");
            //            System.IO.File.Move(System.Web.HttpContext.Current.Server.MapPath(String.Format(tempImg, "N_")), System.Web.HttpContext.Current.Server.MapPath(originalUrl));
            //        }
            //        if (System.IO.File.Exists(System.Web.HttpContext.Current.Server.MapPath(String.Format(tempImg, "T_"))))
            //        {
            //            string originalUrl = String.Format(savePath + imgname, "T_");
            //            System.IO.File.Move(System.Web.HttpContext.Current.Server.MapPath(String.Format(tempImg, "T_")), System.Web.HttpContext.Current.Server.MapPath(originalUrl));
            //        }
            //        saveImg = savePath + imgname;
            //    }
            //    item.PicUrl = saveImg;
            //    Subscription.MsgItems.Add(item);
            //    if (!bll.AddEx(Subscription))
            //    {
            //        IsSuccess = false;
            //    }
            //}
            //if (fm["type_R"] == "text")
            //{
            //    if (ReplyMsg != null && ReplyMsg.SysMsgId > 0)
            //    {
            //        IsSuccess = bll.Update(ReplyMsg) ? IsSuccess : false;
            //    }
            //    else
            //    {
            //        ReplyMsg.SysType = 2;
            //        ReplyMsg.MsgType = "text";
            //        ReplyMsg.CreateTime = DateTime.Now;
            //        ReplyMsg.OpenId = openId;
            //        IsSuccess = bll.Add(ReplyMsg) > 0 ? IsSuccess : false;
            //    }
            //}
            //if (fm["type_R"] == "news")
            //{
            //    string title = fm["txtTitle_R"];
            //    bll.DeleteEx(ReplyMsg.SysMsgId, 2);
            //    ReplyMsg.OpenId = openId;
            //    ReplyMsg.CreateTime = DateTime.Now;
            //    ReplyMsg.MsgType = "news";
            //    ReplyMsg.SysType = 2;
            //    //暂时考虑单图文的情况
            //    YSWL.WeChat.Model.Core.MsgItem item = new YSWL.WeChat.Model.Core.MsgItem();
            //    item.Description = Common.InjectionFilter.HtmlFilter(fm["txtReplyMsg"]);
            //    item.Title = title;
            //    if (fm["sele_R"] == "0")
            //    {
            //        item.Url = YSWL.Components.MvcApplication.GetCurrentRoutePath(YSWL.Web.AreaRoute.Shop);
            //    }
            //    else
            //    {
            //        item.Url = fm["txtUrl_R"];
            //    }
            //    //移动图片 
            //    string tempImg = fm["path_R"];
            //    string imgname = tempImg.Substring(tempImg.LastIndexOf("/") + 1);
            //    string saveImg = tempImg;
            //    if (String.IsNullOrWhiteSpace(tempImg) && tempImg.Contains("/Upload/Temp"))
            //    {
            //        if (!Directory.Exists(System.Web.HttpContext.Current.Server.MapPath(savePath)))
            //        {
            //            Directory.CreateDirectory(System.Web.HttpContext.Current.Server.MapPath(savePath));
            //        }
            //        if (System.IO.File.Exists(System.Web.HttpContext.Current.Server.MapPath(String.Format(tempImg, "N_"))))
            //        {
            //            string originalUrl = String.Format(savePath + imgname, "N_");
            //            System.IO.File.Move(System.Web.HttpContext.Current.Server.MapPath(String.Format(tempImg, "N_")), System.Web.HttpContext.Current.Server.MapPath(originalUrl));
            //        }
            //        if (System.IO.File.Exists(System.Web.HttpContext.Current.Server.MapPath(String.Format(tempImg, "T_"))))
            //        {
            //            string originalUrl = String.Format(savePath + imgname, "T_");
            //            System.IO.File.Move(System.Web.HttpContext.Current.Server.MapPath(String.Format(tempImg, "T_")), System.Web.HttpContext.Current.Server.MapPath(originalUrl));
            //        }
            //        saveImg = savePath + imgname;
            //    }
            //    item.PicUrl = saveImg;
            //    ReplyMsg.MsgItems.Add(item);
            //    if (!bll.AddEx(ReplyMsg))
            //    {
            //        IsSuccess = false;
            //    }
            //}

            if (IsSuccess)
            {
                return View(viewName);
            }
            else
            {
                return Content("操作失败！");
            }

        }
        #endregion

        #region 过滤多余的标签，只保留a标签
        private static string RemoveSpecifyHtml(string ctx)
        {
            string[] holdTags = { "a" };//保留的 tag
            string regStr = string.Format(@"<(?!((/?\s?{0})))[^>]+>", string.Join(@"\b)|(/?\s?", holdTags));
            Regex reg = new Regex(regStr, RegexOptions.Compiled | RegexOptions.Multiline | RegexOptions.IgnoreCase);
            return reg.Replace(ctx, "");
        }
        #endregion

        #region 关键字内容
        public ActionResult RuleList(int pageIndex = 1, string viewName = "RuleList")
        {
            YSWL.WeChat.BLL.Core.KeyRule ruleBll = new YSWL.WeChat.BLL.Core.KeyRule();
            int _pageSize = 10;
            //计算分页起始索引
            int startIndex = pageIndex > 1 ? (pageIndex - 1) * _pageSize + 1 : 1;
            //计算分页结束索引
            int endIndex = pageIndex * _pageSize;
            int toalCount = ruleBll.GetCount(OpenId, "");
            List<YSWL.WeChat.Model.Core.KeyRule> list = ruleBll.GetRuleList(OpenId, "", startIndex, endIndex);
            List<YSWL.MALL.ViewModel.Supplier.RuleKeyValue> rulelist = new List<ViewModel.Supplier.RuleKeyValue>();
            YSWL.MALL.ViewModel.Supplier.RuleKeyValue ruleModel = null;
            foreach (var item in list)
            {
                ruleModel = new ViewModel.Supplier.RuleKeyValue();
                ruleModel.RuleId = item.RuleId;
                ruleModel.OpenId = item.OpenId;
                ruleModel.Name = item.Name;
                ruleModel.Remark = item.Remark;
                ruleModel.RuleValue = GetValues(item.RuleId);
                rulelist.Add(ruleModel);
            }
            if (toalCount < 1)
            {
                return View(viewName);//NO DATA
            }
            PagedList<YSWL.MALL.ViewModel.Supplier.RuleKeyValue> lists = new PagedList<YSWL.MALL.ViewModel.Supplier.RuleKeyValue>(rulelist, pageIndex, _pageSize, toalCount);
            if (Request.IsAjaxRequest())
            {
                return PartialView(viewName, lists);
            }
            return View(viewName, lists);
        }


        #region 获取关键字集合

        protected string GetValues(int ruleId)
        {
            YSWL.WeChat.BLL.Core.KeyValue valueBll = new KeyValue();
            string values = "";

            List<YSWL.WeChat.Model.Core.KeyValue> ruleList = valueBll.GetModelList(" ruleId=" + ruleId);
            if (ruleList != null && ruleList.Count > 0)
            {
                values = String.Join(",", ruleList.Select(c => c.Value));
            }

            return values;
        }

        #endregion


        #region 批量删除
        public ActionResult RuleDelete(string ids)
        {
            YSWL.WeChat.BLL.Core.KeyRule bll = new YSWL.WeChat.BLL.Core.KeyRule();
            ids = ids.Substring(0, ids.LastIndexOf(","));
            JsonObject json = new JsonObject();
            if (bll.DeleteList(ids))
            {
                json.Put("Result", "OK");
                return Json(json);
            }
            else
            {
                json.Put("Result", "NO");
                return Json(json);
            }

        }
        #endregion

        #region 添加规则
        public ActionResult AddRule()
        {
            return View();
        }

        [HttpPost]
        public ActionResult AddRule(string tName, string Remark)
        {
            YSWL.WeChat.BLL.Core.KeyRule ruleBll = new KeyRule();
            YSWL.WeChat.Model.Core.KeyRule ruleModel = new YSWL.WeChat.Model.Core.KeyRule();
            ruleModel.Name = tName;
            ruleModel.Remark = Remark;
            ruleModel.OpenId = OpenId;
            JsonObject json = new JsonObject();
            if (ruleBll.Add(ruleModel) > 0)
            {
                json.Put("Result", "OK");
                return Json(json);
            }
            else
            {
                json.Put("Result", "NO");
                return Json(json);
            }
        }
        #endregion

        #region 更改规则
        public ActionResult UpdateRule(int id)
        {
            YSWL.WeChat.BLL.Core.KeyRule ruleBll = new KeyRule();
            YSWL.WeChat.Model.Core.KeyRule ruleModel = ruleBll.GetModel(id);
            return View("UpdateRule", ruleModel);
        }


        [HttpPost]
        public ActionResult UpdateRule(string tName, string Remark, int RuleId = 0)
        {
            YSWL.WeChat.BLL.Core.KeyRule ruleBll = new KeyRule();
            YSWL.WeChat.Model.Core.KeyRule ruleModel = ruleBll.GetModel(RuleId);
            JsonObject json = new JsonObject();
            if (ruleModel != null)
            {
                ruleModel.Name = tName;
                ruleModel.Remark = Remark;
                if (ruleBll.Update(ruleModel))
                {
                    json.Put("Result", "OK");
                    return Json(json);
                }
                else
                {
                    return Content("");
                }
            }
            else
            {
                json.Put("Result", "NO");
                return Json(json);
            }
        }

        #region 关键字与回复内容
        public ActionResult PostMsgList(int id, string viewName = "PostMsgList")
        {
            ViewBag.RuleID = id;
            YSWL.WeChat.BLL.Core.KeyValue valueBll = new KeyValue();
            List<YSWL.WeChat.Model.Core.KeyValue> valueList = valueBll.GetModelList(" RuleId=" + id);
            YSWL.WeChat.BLL.Core.PostMsg msgBll = new YSWL.WeChat.BLL.Core.PostMsg();
            List<YSWL.WeChat.Model.Core.PostMsg> msgList = msgBll.GetModelList(" RuleId=" + id);
            if (valueList != null && valueList.Count > 0)
            {
                foreach (var keyValue in valueList)
                {
                    //模糊匹配
                    if (keyValue.MatchType == 0)
                    {
                        ViewBag.NoMatchValue = String.Format("<span class='SKUValue'><span class='span1'  href='javascript:void(0)'  class='updatetype' valueId='{1}' title='点击设为全匹配'><a>{0}</a></span><span class='span2'><a href='javascript:void(0)'  class='del' valueId='{1}'>删除</a></span> </span>", keyValue.Value, keyValue.ValueId);

                    }
                    else
                    {
                        ViewBag.AllMatchValue = String.Format("<span class='SKUValue'><span class='span1'  href='javascript:void(0)'  class='updatetype' valueId='{1}' title='点击设为模糊匹配'><a >{0}</a></span><span class='span2'><a href='javascript:void(0)'  class='del' valueId='{1}'>删除</a></span> </span>", keyValue.Value, keyValue.ValueId);
                    }
                }
            }
            msgList = msgList.OrderByDescending(c => c.CreateTime).ToList();
            if (msgList != null && msgList.Count > 0)
            {
                foreach (var msg in msgList)
                {
                    ViewBag.PostMsg = String.Format(
                        "<li><div class='userPic'>▶</div><div class='content'><div class='msgInfo'> {0}</div><div class='times'> <a class='delMsg' href='javascript:;' msgId='{1}'>删除</a></div></div> </li>",
                        msg.Description, msg.PostMsgId);
                }
            }
            return View(viewName);
        }

        public ActionResult UpdateType()
        {
            YSWL.WeChat.BLL.Core.KeyValue valueBll = new KeyValue();
            JsonObject json = new JsonObject();
            int valueId = Common.Globals.SafeInt(this.Request.Form["ValueId"], 0);
            int type = Common.Globals.SafeInt(Request.Params["MatchType"], 0);
            if (valueId == 0)
            {
                json.Put("STATUS", "FAILED");
            }
            else
            {
                if (valueBll.UpdateType(valueId, type))
                {
                    json.Put("STATUS", "SUCCESS");
                }
                else
                {
                    json.Put("STATUS", "FAILED");
                }
            }
            return Json(json);
        }
        /// <summary>
        /// 添加关键字
        /// </summary>
        /// <returns></returns>
        public ActionResult AddValue()
        {
            YSWL.WeChat.BLL.Core.KeyValue valueBll = new KeyValue();
            JsonObject json = new JsonObject();
            int ruleId = Common.Globals.SafeInt(this.Request.Form["RuleId"], 0);
            string value = Request.Form["Value"];
            json.Put("STATUS", "FAILED");
            if (ruleId == 0)
            {
                json.Put("STATUS", "");
                return Json(json);
            }
            YSWL.WeChat.Model.Core.KeyValue valueModel = new YSWL.WeChat.Model.Core.KeyValue();
            if (valueBll.Exists(value, ""))
            {
                json.Put("STATUS", "Exist");
                return Json(json);
            }
            valueModel.Value = value;
            valueModel.MatchType = 1;
            valueModel.RuleId = ruleId;
            int valueId = valueBll.Add(valueModel);
            if (valueId > 0)
            {
                json.Put("STATUS", "Success");
                json.Put("DATA", valueId);
            }
            return Json(json);
        }

        /// <summary>
        /// 添加回复
        /// </summary>
        /// <returns></returns>
        public ActionResult AddMsg()
        {
            YSWL.WeChat.BLL.Core.PostMsg msgBll = new YSWL.WeChat.BLL.Core.PostMsg();
            JsonObject json = new JsonObject();
            int ruleId = Common.Globals.SafeInt(this.Request.Form["RuleId"], 0);
            string msg = Request.Form["Msg"];
            json.Put("STATUS", "FAILED");
            if (ruleId == 0)
            {
                json.Put("STATUS", "");
            }
            YSWL.WeChat.Model.Core.PostMsg valueModel = new YSWL.WeChat.Model.Core.PostMsg();
            valueModel.Description = msg;
            valueModel.MsgType = "text";
            valueModel.RuleId = ruleId;
            valueModel.CreateTime = DateTime.Now;
            long msgId = msgBll.Add(valueModel);
            if (msgId > 0)
            {
                json.Put("STATUS", "Success");
                json.Put("DATA", msgId);
            }
            return Json(json);
        }

        /// <summary>
        /// 删除关键字
        /// </summary>
        /// <returns></returns>
        public ActionResult DeleteValue()
        {
            YSWL.WeChat.BLL.Core.KeyValue valueBll = new KeyValue();
            JsonObject json = new JsonObject();
            int valueId = Common.Globals.SafeInt(this.Request.Form["ValueId"], 0);
            json.Put("STATUS", "FAILED");
            if (valueBll.Delete(valueId))
            {
                json.Put("STATUS", "Success");
            }
            return Json(json);
        }
        /// <summary>
        /// 删除回复
        /// </summary>
        /// <returns></returns>
        public ActionResult DeleteMsg()
        {
            YSWL.WeChat.BLL.Core.PostMsg msgBll = new YSWL.WeChat.BLL.Core.PostMsg();
            JsonObject json = new JsonObject();
            int msgId = Common.Globals.SafeInt(this.Request.Form["MsgId"], 0);
            json.Put("STATUS", "FAILED");
            if (msgBll.Delete(msgId))
            {
                json.Put("STATUS", "Success");
            }
            return Json(json);
        }

        #endregion

        #endregion
        #endregion

        #region 微信用户管理
        public ViewResult UserList()
        {
            return View();
        }
        //移动
        public PartialViewResult LoadMove(string viewName = "_WCLoadMove")
        {
            List<YSWL.WeChat.Model.Core.Group> list = groupBll.GetGroupList(OpenId);
            return PartialView(viewName, list);
        }
        //微信用户搜索
        public PartialViewResult LoadContent(int pageIndex = 1, string viewName = "_WCContentList", string txtFrom = "", string txtTo = "", string txtKeyWord = "", int ddStatus = -1)
        {
            int pageSize = 10;
            int startIndex = pageIndex > 1 ? (pageIndex - 1) * pageSize + 1 : 1;
            int endIndex = pageIndex * pageSize;
            int totalCount = userBll.GetCount(OpenId, ddStatus, txtFrom, txtTo, txtKeyWord);
            List<YSWL.WeChat.Model.Core.User> list = userBll.GetListByPageEx(OpenId, ddStatus, txtFrom, txtTo, txtKeyWord, startIndex, endIndex);
            List<YSWL.MALL.ViewModel.Supplier.WeChatUser> userlist = new List<ViewModel.Supplier.WeChatUser>();
            YSWL.MALL.ViewModel.Supplier.WeChatUser userModel = null;
            foreach (var item in list)
            {
                userModel = new ViewModel.Supplier.WeChatUser();
                userModel.CancelTime = item.CancelTime;
                userModel.City = item.City;
                userModel.Country = item.Country;
                userModel.CreateTime = item.CreateTime;
                userModel.GroupId = item.GroupId;
                userModel.Headimgurl = item.Headimgurl;
                userModel.ID = item.ID;
                userModel.Language = item.Language;
                userModel.NickName = item.NickName;
                userModel.OpenId = item.OpenId;
                userModel.Province = item.Province;
                userModel.Remark = item.Remark;
                userModel.Sex = item.Sex;
                userModel.Status = item.Status;
                userModel.UserId = item.UserId;
                userModel.GroupName = GetGroupName(item.GroupId);
                userModel.UserName = item.UserName;
                userModel.UserStatus = GetUserStatus(item.Status);
                userlist.Add(userModel);
            }
            if (null == userlist)
            {
                return PartialView(viewName);
            }
            PagedList<YSWL.MALL.ViewModel.Supplier.WeChatUser> pagedList = new PagedList<YSWL.MALL.ViewModel.Supplier.WeChatUser>(userlist, pageIndex, pageSize, totalCount);
            if (Request.IsAjaxRequest())
            {
                return PartialView(viewName, pagedList);
            }
            return PartialView(viewName, pagedList);
        }
        #endregion

        #region 移动分组
        public ActionResult DDGroup(string ids, int groupId)
        {
            JsonObject json = new JsonObject();
            if (groupId == 0)
            {
                json.Put("Result", "Group");
                return Json(json);
            }
            ids = ids.Substring(0, ids.LastIndexOf(","));
            if (String.IsNullOrWhiteSpace(ids))
            {
                json.Put("Result", "User");
                return Json(json);
            }
            if (userBll.UpdateGroup(groupId, ids))
            {
                json.Put("Result", "OK");
                return Json(json);
            }
            else
            {
                json.Put("Result", "NO");
                return Json(json);
            }
        }
        #endregion

        #region 批量删除
        public ActionResult AllDelete(string ids)
        {
            ids = ids.Substring(0, ids.LastIndexOf(","));
            JsonObject json = new JsonObject();
            if (userBll.DeleteList(ids))
            {
                json.Put("Result", "OK");
                return Json(json);
            }
            else
            {
                json.Put("Result", "NO");
                return Json(json);
            }

        }
        #endregion

        #region 绑定用户

        [HttpPost]
        public ActionResult BindUser(int id = 0)
        {
            YSWL.WeChat.Model.Core.User userModel = userBll.GetModel(id);
            YSWL.Accounts.Bus.User user = new YSWL.Accounts.Bus.User(id);
            userModel.UserId = id;
            userModel.NickName = user.NickName;
            JsonObject json = new JsonObject();
            if (userBll.Update(userModel))
            {
                json.Put("Result", "OK");
                return Json(json);
            }
            else
            {
                json.Put("Result", "NO");
                return Json(json);
            }
        }
        #endregion

        #region 辅助方法

        #region 获取分组名称
        private string GetGroupName(int groupId)
        {
            string str = "未分组";
            YSWL.WeChat.Model.Core.Group groupModel = groupBll.GetModelByCache(groupId);
            str = groupModel == null ? str : groupModel.GroupName;
            return str;
        }
        #endregion

        #region 获取用户名
        private string GetUserName(int userId)
        {
            string str = "";
            YSWL.MALL.BLL.Members.Users userBll = new BLL.Members.Users();
            YSWL.MALL.Model.Members.Users userModel = userBll.GetModelByCache(userId);
            str = userModel == null ? str : userModel.UserName;
            return str;
        }

        protected string GetUserStatus(int status)
        {
            //0:取消关注、1:关注、
            string str = string.Empty;
            switch (status)
            {
                case 0:
                    str = "取消关注";
                    break;
                case 1:
                    str = "关注";
                    break;
                default:
                    break;
            }

            return str;
        }
        #endregion

        #endregion

        #region 绑定用户
        public ActionResult BindUser(string viewName = "BindUser", int id = 0)
        {
            ViewBag.ID = id;
            YSWL.MALL.BLL.Members.Users userBll = new YSWL.MALL.BLL.Members.Users();
            List<YSWL.MALL.Model.Members.Users> list = userBll.GetModelList("Activity='True'");
            return View(viewName, list);
        }

        #endregion

        #region  邮件模版
        public ViewResult EmailTemplate()
        {
            ViewBag.SubMsgTitle = YSWL.WeChat.BLL.Core.Config.GetValueByCache("WeChat_SubMsgEmailTitle", SupplierId, currentUser.UserType);
            ViewBag.SubMsgDesc = YSWL.WeChat.BLL.Core.Config.GetValueByCache("WeChat_SubMsgEmailDesc", SupplierId, currentUser.UserType);
            ViewBag.NoMsgTitle = YSWL.WeChat.BLL.Core.Config.GetValueByCache("WeChat_NoMsgEmailTitle", SupplierId, currentUser.UserType);
            ViewBag.NoMsgDesc = YSWL.WeChat.BLL.Core.Config.GetValueByCache("WeChat_NoMsgEmailDesc", SupplierId, currentUser.UserType);
            return View();
        }

        [HttpPost]
        [ValidateInput(false)]
        public ViewResult EmailTemplate(FormCollection Fm)
        {
            YSWL.WeChat.BLL.Core.Config.Modify("WeChat_SubMsgEmailTitle", Fm["SubMsgTitle"], SupplierId, currentUser.UserType, "关注邮件模版标题");
            YSWL.WeChat.BLL.Core.Config.Modify("WeChat_SubMsgEmailDesc", Fm["SubMsgDesc"], SupplierId, currentUser.UserType, "关注邮件模版内容");
            YSWL.WeChat.BLL.Core.Config.Modify("WeChat_NoMsgEmailTitle", Fm["NoMsgTitle"], SupplierId, currentUser.UserType, "默认消息邮件标题");
            YSWL.WeChat.BLL.Core.Config.Modify("WeChat_NoMsgEmailDesc", Fm["NoMsgDesc"], SupplierId, currentUser.UserType, "默认消息邮件内容");
            //清空缓存
            YSWL.WeChat.BLL.Core.Config.ClearCache();
            ViewBag.SubMsgTitle = YSWL.WeChat.BLL.Core.Config.GetValueByCache("WeChat_SubMsgEmailTitle", SupplierId, currentUser.UserType);
            ViewBag.SubMsgDesc = YSWL.WeChat.BLL.Core.Config.GetValueByCache("WeChat_SubMsgEmailDesc", SupplierId, currentUser.UserType);
            ViewBag.NoMsgTitle = YSWL.WeChat.BLL.Core.Config.GetValueByCache("WeChat_NoMsgEmailTitle", SupplierId, currentUser.UserType);
            ViewBag.NoMsgDesc = YSWL.WeChat.BLL.Core.Config.GetValueByCache("WeChat_NoMsgEmailDesc", SupplierId, currentUser.UserType);
            return View();

        }
        #endregion

        #region 指令管理
        public ActionResult CommandList(int pageIndex = 1, int status = -1, string keyWord = "", string viewName = "CommandList")
        {
            int pageSize = 10;
            int startIndex = pageIndex > 1 ? (pageIndex - 1) * pageSize + 1 : 1;
            int endIndex = pageIndex * pageSize;
            YSWL.WeChat.BLL.Core.Command commandBll = new YSWL.WeChat.BLL.Core.Command();
            List<WeChat.Model.Core.Command> list = commandBll.GetCommandList(OpenId, status, keyWord, startIndex, endIndex);
            List<YSWL.MALL.ViewModel.Supplier.CommandAction> commandAction = new List<ViewModel.Supplier.CommandAction>();
            YSWL.MALL.ViewModel.Supplier.CommandAction commandModel = null;
            foreach (var item in list)
            {
                commandModel = new ViewModel.Supplier.CommandAction();
                commandModel.CommandId = item.CommandId;
                commandModel.ActionId = item.ActionId;
                commandModel.Name = item.Name;
                commandModel.Remark = item.Remark;
                commandModel.OpenId = item.OpenId;
                commandModel.TargetId = item.TargetId;
                commandModel.Sequence = item.Sequence;
                commandModel.Status = item.Status;
                commandModel.ParseType = item.ParseType;
                commandModel.ParseLength = item.ParseLength;
                commandModel.ParseChar = item.ParseChar;
                commandModel.ActionName = GetAction(item.ActionId);
                commandModel.ActionType = GetParseType(item.ParseType);
                commandModel.ActionStatus = GetStatus(item.Status);
                commandModel.ActionTarget = GetTarget(item.TargetId, item.ActionId);
                commandAction.Add(commandModel);
            }
            return View(viewName, commandAction);
        }
        //批量删除指令
        public ActionResult CommandDelete(string ids)
        {
            YSWL.WeChat.BLL.Core.Command commandBll = new YSWL.WeChat.BLL.Core.Command();
            //YSWL.WeChat.BLL.Core.User bll = new YSWL.WeChat.BLL.Core.User();
            ids = ids.Substring(0, ids.LastIndexOf(","));
            JsonObject json = new JsonObject();
            try
            {
                if (commandBll.DeleteList(ids))
                {
                    json.Put("Result", "OK");
                    return Json(json);
                }
            }
            catch (Exception ex)
            {
                json.Put("Result", ex.Message);
                return Json(json);
            }
            return Json(json);
            //else
            //{
            //    json.Put("Result", "NO");
            //    return Json(json);
            //}
        }

        public ActionResult AddCommand(string viewName = "AddCommand")
        {
            List<YSWL.WeChat.Model.Core.Action> actionList = YSWL.WeChat.BLL.Core.Action.GetAllAction();

            return View(viewName, actionList);
        }

        //指令增加
        public ActionResult Save(int dropAction = 0, string Name = "", string txtSequence = "", int ddParseType = 0, int status = 0, int ddTarget = 0, string txtParseType = "", string Remark = "")
        {
            JsonObject json = new JsonObject();
            YSWL.WeChat.BLL.Core.Command commandBll = new YSWL.WeChat.BLL.Core.Command();
            YSWL.WeChat.Model.Core.Command commandModel = new YSWL.WeChat.Model.Core.Command();
            if (dropAction == -1)
            {
                json.Put("Result", "Action");
                return Json(json);
            }
            if (String.IsNullOrWhiteSpace(Name))
            {
                json.Put("Result", "Name");
                return Json(json);
            }
            commandModel.Name = Name;
            commandModel.ActionId = dropAction;
            commandModel.ParseType = ddParseType;
            commandModel.Status = status;
            commandModel.TargetId = ddTarget;
            commandModel.OpenId = OpenId;
            if (commandModel.ParseType == 0)
            {
                commandModel.ParseLength = Common.Globals.SafeInt(txtParseType, 0);
            }
            else
            {
                commandModel.ParseChar = txtParseType.Trim();
            }
            commandModel.Remark = Remark;
            commandModel.Sequence = Common.Globals.SafeInt(txtSequence, 0);
            if (commandBll.Add(commandModel) > 0)
            {
                json.Put("Result", "OK");
                return Json(json);
            }
            else
            {
                json.Put("Result", "NO");
                return Json(json);
            }

        }

        public ActionResult ActionOne(string viewName = "AddCommand")
        {
            YSWL.MALL.BLL.CMS.ContentClass classBll = new ContentClass();
            List<YSWL.MALL.Model.CMS.ContentClass> classList =
                classBll.GetModelList(" Depth=1 and State=0");
            JsonObject json = new JsonObject();
            json.Put("Status", "OK");
            if (classList != null)
            {
                json.Put("ClassID", "ClassID");
                json.Put("ClassName", "ClassName");
                return Json(json);
            }
            else
            {
                return View(viewName);
            }

        }

        public ActionResult ActionTwo(string viewName = "AddCommand")
        {
            YSWL.MALL.BLL.Shop.Products.CategoryInfo cateBll = new CategoryInfo();
            List<YSWL.MALL.Model.Shop.Products.CategoryInfo> CategoryInfos =
                cateBll.GetModelList(" Depth=1");

            JsonObject json = new JsonObject();
            json.Put("Status", "OK");
            JsonArray arr = new JsonArray();
            JsonObject tmp;
            if (CategoryInfos != null)
            {
                JsonArray data = new JsonArray();
                foreach (YSWL.MALL.Model.Shop.Products.CategoryInfo item in CategoryInfos)
                {
                    tmp = new JsonObject();
                    tmp.Accumulate("Name", item.Name);
                    tmp.Accumulate("Value", item.CategoryId);
                    arr.Put(tmp);
                }
                json.Put("DATA", arr);
                return Json(json);
            }
            else
            {
                return View(viewName);
            }
        }

        #region 编辑
        public ActionResult Update(string viewName = "Update", int id = 0)
        {
            List<YSWL.WeChat.Model.Core.Action> actionList = YSWL.WeChat.BLL.Core.Action.GetAllAction();

            YSWL.WeChat.BLL.Core.Command commandBll = new YSWL.WeChat.BLL.Core.Command();
            YSWL.WeChat.Model.Core.Command commandModel = commandBll.GetModel(id);


            return View(viewName, commandModel);
        }

        [HttpPost]
        public ActionResult Update(int dropAction = 0, string Name = "", string txtSequence = "", int ddParseType = 0, int status = 0, int ddTarget = 0, string txtParseType = "", string Remark = "", int CommandId = 0, string viewName = "Update")
        {
            JsonObject json = new JsonObject();
            YSWL.WeChat.BLL.Core.Command commandBll = new YSWL.WeChat.BLL.Core.Command();
            YSWL.WeChat.Model.Core.Command commandModel = commandBll.GetModel(CommandId);
            if (dropAction == 0)
            {
                json.Put("Result", "Action");
                return Json(json);
            }
            if (String.IsNullOrWhiteSpace(Name))
            {
                json.Put("Result", "Name");
                return Json(json);
            }
            commandModel.Name = Name;
            commandModel.ActionId = dropAction;
            commandModel.ParseType = ddParseType;
            commandModel.Status = status;
            commandModel.TargetId = ddTarget;
            if (commandModel.ParseType == 0)
            {
                commandModel.ParseLength = Common.Globals.SafeInt(txtParseType, 0);
            }
            else
            {
                commandModel.ParseChar = txtParseType.Trim();
            }
            commandModel.Remark = Remark;
            commandModel.Sequence = Common.Globals.SafeInt(txtSequence, 0);
            if (commandBll.Update(commandModel))
            {
                json.Put("Result", "OK");
            }
            else
            {
                json.Put("Result", "NO");
            }
            return Json(json);
        }

        #endregion

        #region 获取对应系统指令名称

        protected string GetAction(object target)
        {
            string value = "";
            if (!StringPlus.IsNullOrEmpty(target))
            {
                int actionId = Common.Globals.SafeInt(target.ToString(), 0);
                List<YSWL.WeChat.Model.Core.Action> actionList = YSWL.WeChat.BLL.Core.Action.GetAllAction();
                YSWL.WeChat.Model.Core.Action actionModel = actionList.FirstOrDefault(c => c.ActionId == actionId);
                if (actionModel != null)
                {
                    value = actionModel.Name;
                }
            }
            return value;
        }

        #endregion

        protected string GetTarget(object target_obj, object actionId_obj)
        {
            string value = "";
            if (!StringPlus.IsNullOrEmpty(target_obj) && !StringPlus.IsNullOrEmpty(actionId_obj))
            {
                int targetId = Common.Globals.SafeInt(target_obj.ToString(), 0);
                int actionId = Common.Globals.SafeInt(actionId_obj.ToString(), 0);
                switch (actionId)
                {
                    //文章栏目
                    case 1:
                        YSWL.MALL.BLL.CMS.ContentClass classBll = new ContentClass();
                        YSWL.MALL.Model.CMS.ContentClass classModel =
                              classBll.GetModel(targetId);
                        value = classModel == null ? "" : "文章栏目：【" + classModel.ClassName + "】";
                        break;
                    //商品分类
                    case 2:
                        YSWL.MALL.BLL.Shop.Products.CategoryInfo cateBll = new CategoryInfo();
                        YSWL.MALL.Model.Shop.Products.CategoryInfo CategoryInfo =
                            cateBll.GetModel(targetId);
                        value = CategoryInfo == null ? "" : "商品分类：【" + CategoryInfo.Name + "】";
                        break;
                    default:
                        value = "";
                        break;
                }
            }
            return value;
        }

        protected string GetParseType(object target)
        {
            string value = "";
            if (!StringPlus.IsNullOrEmpty(target))
            {
                int typeId = Common.Globals.SafeInt(target.ToString(), 0);
                switch (typeId)
                {
                    case 0:
                        value = "长度";
                        break;
                    case 1:
                        value = "特殊字符";
                        break;
                    default:
                        value = "长度";
                        break;
                }
            }
            return value;
        }

        protected string GetStatus(object target)
        {
            string value = "";
            if (!StringPlus.IsNullOrEmpty(target))
            {
                int status = Common.Globals.SafeInt(target.ToString(), 0);
                switch (status)
                {
                    case 0:
                        value = "不可用";
                        break;
                    case 1:
                        value = "可用";
                        break;
                    default:
                        value = "不可用";
                        break;
                }
            }
            return value;
        }
        #endregion

        #region  获取微信分组
        public ActionResult GetGroups(bool IsCover)
        {
            string token = YSWL.MALL.Web.Components.PostMsgHelper.GetToken(AppId, AppSercet);
            if (String.IsNullOrWhiteSpace(token))
            {
                return Content("No");
            }
            bool IsSuccess = YSWL.MALL.Web.Components.PostMsgHelper.GetGroups(token, OpenId, IsCover);
            return IsSuccess ? Content("True") : Content("False");
        }


        #endregion

        #region 获取用户详细资料
        public ActionResult GetUserInfos(string ids)
        {
            string token = YSWL.MALL.Web.Components.PostMsgHelper.GetToken(AppId, AppSercet);
            if (String.IsNullOrWhiteSpace(token))
            {
                return Content("No");
            }
            ids = ids.Substring(0, ids.LastIndexOf(","));
            List<YSWL.WeChat.Model.Core.User> UserList = userBll.GetUserList(ids, OpenId);
            YSWL.WeChat.Model.Core.User userModel = null;
            foreach (var item in UserList)
            {
                userModel = userBll.GetWcInfo(token, item.UserName);
                if (userModel == null)
                    continue;
                item.NickName = userModel.NickName;
                item.Province = userModel.Province;
                item.City = userModel.City;
                item.Country = userModel.Country;
                item.Headimgurl = userModel.Headimgurl;
                item.Language = userModel.Language;
                item.Sex = userModel.Sex;
                userBll.Update(item);
            }
            return Content("True");
        }

        #endregion

        #region 单个用户发消息

        #region 发消息页面
        public ActionResult SendMsg(string userName)
        {
            YSWL.WeChat.BLL.Core.User userBll = new YSWL.WeChat.BLL.Core.User();
            List<YSWL.WeChat.Model.Core.User> UserList;
            string openId = YSWL.WeChat.BLL.Core.Config.GetValueByCache("WeChat_OpenId", -1, CurrentUser.UserType);
            UserList = userBll.GetUserList(openId, 0);
            ViewBag.currentUser = userName;
            return View(UserList);
        }
        #endregion

        #region 消息提交
        public ActionResult MsgSubmit()
        {
            string username = Request.Form["username"];
            string message = Request.Form["msg"];
            //先授权 
            string openId = YSWL.WeChat.BLL.Core.Config.GetValueByCache("WeChat_OpenId", -1, CurrentUser.UserType);
            string AppId = YSWL.WeChat.BLL.Core.Config.GetValueByCache("WeChat_AppId", -1, CurrentUser.UserType);
            string AppSecret = YSWL.WeChat.BLL.Core.Config.GetValueByCache("WeChat_AppSercet", -1, CurrentUser.UserType);
            string token = YSWL.MALL.Web.Components.PostMsgHelper.GetToken(AppId, AppSecret);
            if (String.IsNullOrWhiteSpace(token))
            {//MessageBox.ShowFailTip(this, "获取微信授权失败！请检查您的微信API设置和对应的权限");
                return Content("error");
            }
            List<string> users = new List<string>();
            users.Add(username);
            YSWL.WeChat.Model.Core.CustomerMsg msg = new YSWL.WeChat.Model.Core.CustomerMsg();
            msg.OpenId = openId;
            msg.CreateTime = DateTime.Now;
            msg.Description = message;
            msg.MsgType = "text";
            YSWL.WeChat.BLL.Core.CustomerMsg.SendCustomMsg(msg, token, users);
            return Content("ok");
        }
        #endregion
        #endregion

        #region 消息群发
        public ActionResult GetAllGroup(string viewName = "_AllGroup")
        {
            string openId = YSWL.WeChat.BLL.Core.Config.GetValueByCache("WeChat_OpenId", -1, CurrentUser.UserType);
            List<YSWL.WeChat.Model.Core.Group> groupList = groupBll.GetGroupList(openId);
            return View(viewName, groupList);
        }
        public ActionResult SendGroupMsg()
        {
            //group: groupid, msgs: msg
            string group = Request.Form["group"];
            string msgs = Request.Form["msgs"];
            //先授权 
            string AppId = YSWL.WeChat.BLL.Core.Config.GetValueByCache("WeChat_AppId", -1, CurrentUser.UserType);
            string AppSecret = YSWL.WeChat.BLL.Core.Config.GetValueByCache("WeChat_AppSercet", -1, CurrentUser.UserType);
            string token = YSWL.MALL.Web.Components.PostMsgHelper.GetToken(AppId, AppSecret);
            if (String.IsNullOrWhiteSpace(token))
            {
                //MessageBox.ShowFailTip(this, "获取微信授权失败！请检查您的微信API设置和对应的权限");
                return Content("error");
            }
            //获取用户列表 
            int groupId = Common.Globals.SafeInt(group, 0);
            string openId = YSWL.WeChat.BLL.Core.Config.GetValueByCache("WeChat_OpenId", -1, CurrentUser.UserType);
            List<YSWL.WeChat.Model.Core.User> UserList = userBll.GetUserList(openId, groupId);
            if (UserList.Count == 0)
            {
                return Content("noperson");
            }
            var users = UserList.Select(c => c.UserName).ToList();
            YSWL.WeChat.Model.Core.CustomerMsg msg = new YSWL.WeChat.Model.Core.CustomerMsg();
            msg.OpenId = openId;
            msg.CreateTime = DateTime.Now;
            msg.Description = msgs;
            msg.MsgType = "text";
            YSWL.WeChat.BLL.Core.CustomerMsg.SendCustomMsg(msg, token, users);
            return Content("ok");
        }
        #endregion


        #region 微信菜单
        public ActionResult MenuList(string viewName = "MenuList")
        {
            YSWL.WeChat.BLL.Core.Menu menuBll = new YSWL.WeChat.BLL.Core.Menu();
            List<YSWL.WeChat.Model.Core.Menu> menuList = menuBll.GetMenuList(OpenId);
            List<YSWL.MALL.ViewModel.Supplier.WeChatMenu> mulist = new List<ViewModel.Supplier.WeChatMenu>();
            YSWL.MALL.ViewModel.Supplier.WeChatMenu menuModel = null;
            foreach (var item in menuList)
            {
                menuModel = new ViewModel.Supplier.WeChatMenu();
                menuModel.MenuId = item.MenuId;
                menuModel.OpenId = item.OpenId;
                menuModel.ParentId = item.ParentId;
                menuModel.Name = item.Name;
                menuModel.Type = item.Type;
                menuModel.Sequence = item.Sequence;
                menuModel.MenuKey = item.MenuKey;
                menuModel.MenuUrl = item.MenuUrl;
                menuModel.Status = item.Status;
                menuModel.CreateDate = item.CreateDate;
                menuModel.Remark = item.Remark;
                menuModel.HasChildren = item.HasChildren;
                menuModel.MenuStatus = GetMenuStatus(item.Status);
                menuModel.MenuType = GetTypeName(item.Type);
                mulist.Add(menuModel);
            }

            //对商品数据进行排序
            List<YSWL.MALL.ViewModel.Supplier.WeChatMenu> orderList = new List<YSWL.MALL.ViewModel.Supplier.WeChatMenu>();


            var RootList = mulist.Where(c => c.ParentId == 0).OrderBy(c => c.Sequence).ToList();
            foreach (var item in RootList)
            {
                orderList = MenuOrder(item, mulist, orderList);
            }
            return View(viewName, orderList);
        }

        private List<YSWL.MALL.ViewModel.Supplier.WeChatMenu> MenuOrder(YSWL.MALL.ViewModel.Supplier.WeChatMenu model, List<YSWL.MALL.ViewModel.Supplier.WeChatMenu> menuList, List<YSWL.MALL.ViewModel.Supplier.WeChatMenu> orderList)
        {
            orderList.Add(model);
            if (model.HasChildren)
            {
                var list = menuList.Where(c => c.ParentId == model.MenuId).OrderBy(c => c.Sequence);
                foreach (var item in list)
                {
                    MenuOrder(item, menuList, orderList);
                }
            }
            else
            {
                return orderList;
            }
            return orderList;
        }
        #endregion

        public static string GetMenuStatus(object target)
        {
            string str = string.Empty;
            if (!StringPlus.IsNullOrEmpty(target))
            {
                int status = Common.Globals.SafeInt(target, 0);
                switch (status)
                {
                    case 0:
                        str = "不启用";
                        break;
                    case 1:
                        str = "启用";
                        break;

                    default:
                        break;
                }
            }
            return str;
        }

        public string GetTypeName(object target)
        {
            string str = string.Empty;
            if (!StringPlus.IsNullOrEmpty(target))
            {
                string type = target.ToString();
                switch (type)
                {
                    case "view":
                        str = "页面跳转";
                        break;
                    case "click":
                        str = "发送消息";
                        break;

                    default:
                        break;
                }
            }
            return str;
        }

        #region 客服消息管理
        public ActionResult CusMsgList()
        {
            return View();
        }

        public ActionResult LoadCusContent(int pageIndex = 1, string viewName = "_WCCusMsgList", string txtFrom = "", string txtTo = "", int ddStatus = -1)
        {
            YSWL.WeChat.BLL.Core.CustomerMsg msgBll = new YSWL.WeChat.BLL.Core.CustomerMsg();
            int pageSize = 10;
            int startIndex = pageIndex > 1 ? (pageIndex - 1) * pageSize + 1 : 1;
            int endIndex = pageIndex * pageSize;
            int totalCount = msgBll.GetCount(OpenId, txtFrom, txtTo, "");
            List<YSWL.WeChat.Model.Core.CustomerMsg> list = msgBll.GetMsgList("", txtFrom, txtTo, "");

            List<YSWL.MALL.ViewModel.Supplier.WeChatCusMsg> custList = new List<YSWL.MALL.ViewModel.Supplier.WeChatCusMsg>();
            YSWL.MALL.ViewModel.Supplier.WeChatCusMsg cusModel = null;
            foreach (var item in list)
            {
                cusModel = new ViewModel.Supplier.WeChatCusMsg();
                cusModel.MsgId = item.MsgId;
                cusModel.OpenId = item.OpenId;
                cusModel.MsgType = item.MsgType;
                cusModel.MsgTypes = GetMsgType(item.MsgType);
                cusModel.CreateTime = item.CreateTime;
                cusModel.Title = item.Title;
                cusModel.Description = item.Description;
                cusModel.MusicUrl = item.MusicUrl;
                cusModel.HQMusicUrl = item.HQMusicUrl;
                cusModel.ArticleCount = item.ArticleCount;
                custList.Add(cusModel);
            }
            if (null == custList)
            {
                return PartialView(viewName);
            }
            PagedList<YSWL.MALL.ViewModel.Supplier.WeChatCusMsg> pagedList = new PagedList<YSWL.MALL.ViewModel.Supplier.WeChatCusMsg>(custList, pageIndex, pageSize, totalCount);
            if (Request.IsAjaxRequest())
            {
                return PartialView(viewName, pagedList);
            }
            return PartialView(viewName, pagedList);
        }

        //批量删除
        public ActionResult AllMsgDelete(string ids)
        {
            YSWL.WeChat.BLL.Core.CustomerMsg msgBll = new YSWL.WeChat.BLL.Core.CustomerMsg();
            ids = ids.Substring(0, ids.LastIndexOf(","));
            JsonObject json = new JsonObject();
            if (msgBll.DeleteList(ids))
            {
                json.Put("Result", "OK");
                return Json(json);
            }
            else
            {
                json.Put("Result", "NO");
                return Json(json);
            }
        }

        /// <summary>
        ///消息类型
        /// </summary>
        /// <param name="target"></param>
        /// <returns></returns>
        protected string GetMsgType(object target)
        {
            //0:取消关注、1:关注、
            string str = "";
            if (!StringPlus.IsNullOrEmpty(target))
            {
                //  地理位置:location,文本消息:text,消息类型:image，链接信息：link，事件信息：event
                string msgType = target.ToString();
                switch (msgType)
                {
                    case "text":
                        str = "文本消息";
                        break;
                    case "location":
                        str = "地理位置";
                        break;
                    case "image":
                        str = "图片消息";
                        break;
                    case "link":
                        str = "链接信息";
                        break;
                    case "event":
                        str = "事件信息";
                        break;
                    default:
                        str = "文本消息";
                        break;
                }
            }
            return str;
        }

        public ActionResult UserCusList(string viewName="UserCusList",int id=0)
        {
            StringBuilder strWhere = new StringBuilder();
            strWhere.AppendFormat(" MsgId={0}", id);
            YSWL.WeChat.BLL.Core.CustUserMsg msgBll = new YSWL.WeChat.BLL.Core.CustUserMsg();
            YSWL.WeChat.Model.Core.CustUserMsg userModel = msgBll.GetModel(id,OpenId);
            return View(viewName, userModel);
        }
        #endregion

        #region 微信菜单修改
        public ActionResult UpdateMenu(string viewName = "UpdateMenu",int id=0)
        {
            YSWL.WeChat.BLL.Core.Menu menuBll = new YSWL.WeChat.BLL.Core.Menu();
            YSWL.WeChat.Model.Core.Menu menuModel = menuBll.GetModel(id);
            if (menuModel != null)
            {
                return View(viewName, menuModel);
            }
            return View(viewName);
        }
        // data: { MenuName: MenuName, txtSequenc: txtSequence, menu: menu, chkStatus: chkStatus, Remark: Remark },
        [HttpPost]
        public ActionResult UpdateMenu(string viewName = "UpdateMenu", int id = 0,string menuName="",string sequence="",string menu="",string remark="")
        {
            YSWL.WeChat.BLL.Core.Menu menuBll = new YSWL.WeChat.BLL.Core.Menu();
            YSWL.WeChat.Model.Core.Menu menuModel = menuBll.GetModel(id);
           // string stype = GetMenu(id, cid, url);
            return View();
        }

        public YSWL.WeChat.Model.Core.Menu GetMenu(int id,int cid=0,string url="")
        {
            YSWL.WeChat.Model.Core.Menu menu = new WeChat.Model.Core.Menu();
            switch (id)
            {
                case 1:
                    menu.Type = "view";
                    menu.MenuUrl = YSWL.Components.MvcApplication.GetCurrentRoutePath(YSWL.Web.AreaRoute.MobileSP);
                    break;
                case 2:
                    menu.Type = "view";
                    menu.MenuUrl = YSWL.Components.MvcApplication.GetCurrentRoutePath(YSWL.Web.AreaRoute.MobileSP);
                    break;
                case 3:
                    menu.Type = "view";
                    menu.MenuUrl = YSWL.Components.MvcApplication.GetCurrentRoutePath(YSWL.Web.AreaRoute.MobileSP)+"u";
                    break;
                case 4:
                    menu.Type = "click";
                    menu.MenuUrl = "";
                    break;
                case 5:
                    menu.Type = "view";
                    menu.MenuUrl = YSWL.Components.MvcApplication.GetCurrentRoutePath(YSWL.Web.AreaRoute.MobileSP) + "/p/c/" + cid;
                    break;
                case 6:
                    menu.Type = "view";
                    menu.MenuUrl = "";
                    break;
                case 7:
                    menu.Type = "view";
                    menu.MenuUrl = url;
                    break;
                case 8:
                    menu.Type = "click";
                    menu.MenuUrl = "";
                    break;
            }
            return menu;

        }
        #endregion

    }
}
