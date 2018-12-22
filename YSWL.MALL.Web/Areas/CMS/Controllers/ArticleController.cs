using System;
using System.Collections.Generic;
using System.Web;
using System.Web.Mvc;
using YSWL.Json;
using YSWL.Common;
using YSWL.Components.Filters;
using YSWL.MALL.Web.Components.Setting.CMS;

namespace YSWL.MALL.Web.Areas.CMS.Controllers
{
    public class ArticleController : CMSControllerBase
    {
        private const string SESSIONKEY_COMMENTDATE = "CMS_CommentDate";
        private readonly TimeSpan _commentTimeSpan = new TimeSpan(0, 0, 0, 30);

        private BLL.CMS.Content bll = new BLL.CMS.Content();
        private BLL.SysManage.WebSiteSet WebSiteSet = new BLL.SysManage.WebSiteSet(Model.SysManage.ApplicationKeyType.CMS);
        private int Act_EditContent = 15; //编辑文章
        public ActionResult Details(int? id)
        {
            if (id.HasValue)
            {
                int contentid = id.Value;
                Model.CMS.Content model = bll.GetModelExByCache(contentid);
                if (null != model)
                {
                    ViewBag.Title = Globals.HtmlDecode(model.Title);
                    if (null != WebSiteSet)
                    {
                        ViewBag.Title += "-" + Globals.HtmlDecode(WebSiteSet.WebName);
                    }
                    ViewBag.Keywords = Globals.HtmlDecode(model.Keywords);
                    ViewBag.Description = Globals.HtmlDecode(model.Summary);

                    ViewBag.Domain = WebSiteSet.BaseHost;
                    ViewBag.WebName = WebSiteSet.WebName;

                    #region 是否静态化
                    string IsStatic = YSWL.MALL.BLL.SysManage.ConfigSystem.GetValueByCache("ArticleIsStatic");
                    string area = BLL.SysManage.ConfigSystem.GetValueByCache("MainArea");
                    int PrevId = bll.GetPrevID(contentid);
                    int NextId = bll.GetNextID(contentid);
                    if (IsStatic != "true")
                    {
                        if (PrevId > 0)
                        {
                            if (area == "CMS")
                            {
                                ViewBag.PrevUrl = "/Article/Details/" + PrevId;
                            }
                            else
                            {
                                ViewBag.PrevUrl = "/CMS/Article/Details/" + PrevId;
                            }
                        }
                        else
                        {
                            ViewBag.PrevUrl = "";
                        }
                        if (NextId > 0)
                        {
                            if (area == "CMS")
                            {
                                ViewBag.NextUrl = "/Article/Details/" + NextId;
                            }
                            else
                            {
                                ViewBag.NextUrl = "/CMS/Article/Details/" + NextId;
                            }
                        }
                        else
                        {
                            ViewBag.NextUrl = "";
                        }
                    }
                    else
                    {
                        if (PrevId > 0)
                        {
                            ViewBag.PrevUrl = PageSetting.GetCMSUrl(PrevId);
                        }
                        else
                        {
                            ViewBag.PrevUrl = "";
                        }
                        if (NextId > 0)
                        {
                            ViewBag.NextUrl = PageSetting.GetCMSUrl(PrevId);
                        }
                        else
                        {
                            ViewBag.NextUrl = "";
                        }
                    }
                    #endregion

                    if (UserPrincipal != null
                        && currentUser != null
                        && UserPrincipal.HasPermissionID(GetPermidByActID(Act_EditContent)))
                    {
                        ViewBag.EditContent = "";
                    }
                    else
                    {
                        ViewBag.EditContent = "display:none;";
                    }
                }
                return View(model);
            }
            return View("Details");
        }

        /// <summary>
        /// 赞
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPost]
        public void Support(int id)
        {
            JsonObject json = new JsonObject();
            if (Request.Cookies["UsersSupports" + id] != null && Request.Cookies["UsersSupports" + id].Value == id.ToString())
            {
                json.Accumulate("STATUS", "NOTALLOW");
            }
            else
            {
                if (bll.UpdateTotalSupport(id))
                {
                    Model.CMS.Content model = bll.GetModel(id);
                    Model.CMS.Content modelCache = bll.GetModelExByCache(id);
                    if (model != null)
                    {
                        json.Accumulate("STATUS", "SUCC");
                        json.Accumulate("TotalSupport", model.TotalSupport);
                        modelCache.TotalSupport = model.TotalSupport;   //更新缓存

                        //写入Cookie,防止重复操作“赞”。
                        HttpCookie cookie = new HttpCookie("UsersSupports" + id);
                        cookie.Value = id.ToString();
                        cookie.Expires = DateTime.MaxValue;
                        Response.AppendCookie(cookie);
                    }
                    else
                    {
                        json.Accumulate("STATUS", "FAIL");
                    }
                }
                else
                {
                    json.Accumulate("STATUS", "FAIL");
                }
            }
            Response.Write(json.ToString());
        }

        public ActionResult Comment(YSWL.MALL.Model.CMS.Comment model)
        {
            if (Session[SESSIONKEY_COMMENTDATE] != null && !string.IsNullOrEmpty(model.Description))
            {
                DateTime? commentDate = Globals.SafeDateTime(Session[SESSIONKEY_COMMENTDATE].ToString(), null);
                if (commentDate.HasValue && DateTime.Now - commentDate.Value < _commentTimeSpan)
                {
                    return Content("NOCOMMENT");
                }
            }
            model.CreatedDate = DateTime.Now;
            YSWL.MALL.BLL.CMS.Comment bll = new BLL.CMS.Comment();
            List<YSWL.MALL.Model.CMS.Comment> list = new List<YSWL.MALL.Model.CMS.Comment>();
            if (string.IsNullOrEmpty(model.Description))
            {
                list = bll.GetModelList("ContentId=" + model.ContentId + "");
                if (list != null && list.Count > 0)
                {
                    foreach (var item in list)
                    {
                        item.CreatedNickName = String.IsNullOrWhiteSpace(item.CreatedNickName) ? "游客" : item.CreatedNickName;
                    }
                }
                return PartialView(list);
            }
            model.TypeID = 3;
            if (currentUser != null)
            {
                model.CreatedUserID = currentUser.UserID;
                model.CreatedNickName = currentUser.NickName;
            }

            if ((model.ContentId = bll.Add(model)) > 0)
            {
                model.CreatedNickName = String.IsNullOrWhiteSpace(model.CreatedNickName) ? "游客" : model.CreatedNickName;
                list.Add(model);
                Session[SESSIONKEY_COMMENTDATE] = DateTime.Now;
                return PartialView(list);
            }
            return Content("False");

        }
        /// <summary>
        ///浏览数
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPost]
        public void GetPvCount(int id)
        {
            JsonObject json = new JsonObject();
            int count = bll.UpdatePV(id);
            json.Accumulate("STATUS", "SUCC");
            json.Accumulate("DATA", count);
            Response.Write(json.ToString());
        }

    }
}