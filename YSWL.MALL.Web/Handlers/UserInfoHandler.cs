using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.SessionState;
using YSWL.Json;

namespace YSWL.MALL.Web.Handlers
{
    public class UserInfoHandler : HandlerBase, IRequiresSessionState
    {
        #region IHttpHandler 成员

        public override bool IsReusable
        {
            get { return false; }
        }

        public override void ProcessRequest(HttpContext context)
        {
            string action = context.Request.Form["Action"];
            context.Response.Clear();
            context.Response.ContentType = "application/json";
            try
            {
                switch (action)
                {
                    case "GetUsersByEmail":
                        context.Response.Write(GetUsersByEmail(context));
                        break;
                    case "GetUsersByPhone":
                        context.Response.Write(GetUsersByPhone(context));
                        break;
                    default:
                        break;
                }
            }
            catch (Exception ex)
            {
                JsonObject json = new JsonObject();
                json.Put(KEY_STATUS, STATUS_ERROR);
                json.Put(KEY_DATA, ex);
                context.Response.Write(json.ToString());
            }
        }

        #endregion

        /// <summary>
        /// 获取
        /// </summary>
        /// <returns></returns>
        private string GetUsersByEmail(HttpContext context)
        {
            JsonObject json = new JsonObject();
            JsonArray jsonArray = new JsonArray();
            JsonObject itemjson;
            string q = context.Request.Form["q"];
            int page_limit = YSWL.Common.Globals.SafeInt(context.Request.Form["page_limit"], 10);
            int page = YSWL.Common.Globals.SafeInt(context.Request.Form["page"], 1);
            int startIndex = page > 1 ? (page - 1) * page_limit + 1 : 1;
            //计算分页结束索引
            int endIndex = page * page_limit;
            YSWL.MALL.BLL.Members.Users userBll = new BLL.Members.Users();

            int total=userBll.GetTotalCount("UU", q);
            List<YSWL.MALL.Model.Members.Users> list = userBll.GetPageListByEmail("UU", q, startIndex, endIndex);
            if (list != null && list.Count > 0)
            {
                foreach (YSWL.MALL.Model.Members.Users item in list)
                {
                    itemjson = new JsonObject();
                    itemjson.Put("id", item.UserID);
                    itemjson.Put("text", item.Email);
                    jsonArray.Add(itemjson);
                }
            }
            json.Put("total", total);
            json.Put("List", jsonArray);
            return json.ToString();
        }

        private string GetUsersByPhone(HttpContext context)
        {
            JsonObject json = new JsonObject();
            JsonArray jsonArray = new JsonArray();
            JsonObject itemjson;
            string q = context.Request.Form["q"];
            int page_limit = YSWL.Common.Globals.SafeInt(context.Request.Form["page_limit"], 10);
            int page = YSWL.Common.Globals.SafeInt(context.Request.Form["page"], 1);
            int startIndex = page > 1 ? (page - 1) * page_limit + 1 : 1;
            //计算分页结束索引
            int endIndex = page * page_limit;
            YSWL.MALL.BLL.Members.Users userBll = new BLL.Members.Users();

            int total = userBll.GetTotalCount("UU", q);
            List<YSWL.MALL.Model.Members.Users> list = userBll.GetPageListByPhone("UU", q, startIndex, endIndex);
            if (list != null && list.Count > 0)
            {
                foreach (YSWL.MALL.Model.Members.Users item in list)
                {
                    itemjson = new JsonObject();
                    itemjson.Put("id", item.UserID);
                    itemjson.Put("text", item.Email);
                    jsonArray.Add(itemjson);
                }
            }
            json.Put("total", total);
            json.Put("List", jsonArray);
            return json.ToString();
        }
    }
}