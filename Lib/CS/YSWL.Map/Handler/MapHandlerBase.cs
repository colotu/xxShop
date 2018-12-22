/**
* MapHandlerBase.cs
*
* 功 能： 地图Ajax处理基类
* 类 名： MapHandlerBase
*
* Ver    变更日期             负责人  变更内容
* ───────────────────────────────────
* V0.01  2012/5/30 21:06:25   Ben    初版
*
* Copyright (c) 2012 YSWL Corporation. All rights reserved.
*┌──────────────────────────────────┐
*│　此技术信息为本公司机密信息，未经本公司书面同意禁止向第三方披露．　│
*│　版权所有：云商未来（北京）科技有限公司　　　　　　　　　　　　　　│
*└──────────────────────────────────┘
*/
using System;
using System.Web;
using System.Web.SessionState;
using YSWL.Json;
using YSWL.Map.BLL;
using System.Data;

namespace YSWL.Map.Handler
{
    public abstract class MapHandlerBase : IHttpHandler, IRequiresSessionState
    {
        protected YSWL.Map.BLL.MapInfoManage mapInfoManage = new MapInfoManage();

        #region IHttpHandler 成员

        public bool IsReusable
        {
            get { return false; }
        }

        public void ProcessRequest(HttpContext context)
        {
            JsonObject json = new JsonObject();
            context.Response.ContentType = "application/json";

            //Check Login
            if (!CheckUser(context))
            {
                context.Response.Write("{\"STATUS\":\"NOLONGIN\"}");
                json.Accumulate("STATUS", "NOLONGIN");
                return;
            }

            try
            {
                string action = context.Request["Action"];
                switch (action)
                {
                    case "CkeckLogin":
                        json.Accumulate("STATUS", "OK");
                        context.Response.Write(json.ToString());
                        return;
                    case "GetDepartmentMapById":
                        GetDepartmentMapById(context);
                        return;
                    default:
                        ProcessAction(action, context);
                        return;
                }
            }
            catch (Exception exception)
            {
                json.Accumulate("ERROR", exception.Message.Replace("\"", "'"));
                context.Response.Write(json.ToString());
            }
        }
        #endregion

        #region 基础功能代码
        protected virtual void GetDepartmentMapById(HttpContext context)
        {
            JsonObject json = new JsonObject();
            int departmentId = YSWL.Common.Globals.SafeInt(context.Request.Params["DepartmentId"], 0);
            if (departmentId < 1)
            {
                json.Accumulate("ERROR", "NOENTERPRISEID");
                context.Response.Write(json.ToString());
                return;
            }
#if flase   //使用DataSet后 Json数据为首字母大写
            DataSet dataSetMaps = mapInfoManage.GetList("DepartmentId=" + departmentId);
            if (dataSetMaps.Tables[0] == null || dataSetMaps.Tables[0].Rows.Count < 1)
            {
                json.Accumulate("STATUS", "NODATA");
                context.Response.Write(json.ToString());
                return;
            }
            json.Accumulate("STATUS", "OK");
            json.Accumulate("DATA", dataSetMaps.Tables[0]);
#else
            //使用实体对象后 Json数据首字母小写
            YSWL.Map.Model.MapInfo mapInfo = mapInfoManage.GetModelByDepartmentId(departmentId);
            if (mapInfo == null)
            {
                json.Accumulate("STATUS", "NODATA");
                context.Response.Write(json.ToString());
                return;
            }
#endif
            json.Accumulate("STATUS", "OK");
            json.Accumulate("DATA", mapInfo);
            context.Response.Write(json.ToString());
            return;
        }

        #endregion

        #region 子类实现
        /// <summary>
        /// Check是否已登录
        /// </summary>
        protected abstract bool CheckUser(HttpContext context);
        /// <summary>
        /// 通过子类执行
        /// </summary>
        protected abstract void ProcessAction(string actionName, HttpContext context);
        #endregion
    }
}
