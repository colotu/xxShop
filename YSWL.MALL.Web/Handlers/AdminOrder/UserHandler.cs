using System;
using System.Collections.Generic;
using System.Data;
using System.Web;
using System.Web.SessionState;
using YSWL.Json;

namespace YSWL.MALL.Web.Handlers.AdminOrder
{
    public class UserHandler : HandlerBase, IRequiresSessionState
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
                if (CurrentUser == null  || CurrentUser.UserType!="AA")
                {
                    return;
                }
                switch (action)
                {
                    case "GetUserList":
                        context.Response.Write(GetUserList(context));
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
        private string GetUserList(HttpContext context)
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
            int total = 0;
            BLL.Members.Users bllUsers = new BLL.Members.Users();
            DataSet ds = bllUsers.GetListByPage(q, startIndex, endIndex, out total);
            if (!Common.DataSetTools.DataSetIsNull(ds)) {
                DataTable dt = ds.Tables[0];
                foreach (DataRow dr in dt.Rows)
                {
                    itemjson = new JsonObject();
                    itemjson.Put("id", dr["UserID"]);
                    itemjson.Put("text", dr["UserName"]);
                    jsonArray.Add(itemjson);
                }         
            }
            json.Put("total", total);
            json.Put("List", jsonArray);
            return json.ToString();
        }
    }
}