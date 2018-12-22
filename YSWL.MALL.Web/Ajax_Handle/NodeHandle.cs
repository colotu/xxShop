/**
* CMSContentHandle.cs
*
* 功 能： [N/A]
* 类 名： CMSContentHandle
*
* Ver    变更日期             负责人  变更内容
* ───────────────────────────────────
* V0.01  2012/5/27 13:12:07  Rock    初版
*
* Copyright (c) 2012 YS56 Corporation. All rights reserved.
*┌──────────────────────────────────┐
*│　此技术信息为本公司机密信息，未经本公司书面同意禁止向第三方披露．　│
*│　版权所有：小鸟科技（北京）科技有限公司　　　　　　　　　　　　　　│
*└──────────────────────────────────┘
*/
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.IO;
using YSWL.Json;
using YSWL.MALL.BLL.CMS;
using YSWL.Common;

namespace YSWL.MALL.Web.AjaxHandle
{
    public class NodeHandle : IHttpHandler
    {
        private PhotoClass photoBLL = new PhotoClass();

        public bool IsReusable
        {
            get { return false; }
        }

        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/plain";

            string Action = context.Request.Params["action"];
            switch (Action)
            {
                case "GetChildNode":
                    GetChildNode(context);
                    break;
                case "GetDepthNode":
                    GetDepthNode(context);
                    break;
                case "GetParentNode":
                    GetParentNode(context);
                    break;
                default:
                    break;
            }
        }

        private void GetChildNode(HttpContext context)
        {
            string parentId = Common.InjectionFilter.SqlFilter(context.Request.Params["ParentId"]);
            JsonObject json = new JsonObject();

            DataSet dsParent = photoBLL.GetList("ParentId=" + (string.IsNullOrWhiteSpace(parentId) ? "0" : parentId));
            if (dsParent.Tables[0].Rows.Count < 1)
            {
                json.Accumulate("STATUS", "NODATA");
                context.Response.Write(json.ToString());
                return;
            }
            json.Accumulate("STATUS", "OK");
            json.Accumulate("DATA", dsParent.Tables[0]);
            context.Response.Write(json.ToString());
        }

        private void GetDepthNode(HttpContext context)
        {
            int nodeId = Globals.SafeInt(context.Request.Params["NodeId"], 0);
            JsonObject json = new JsonObject();
            DataSet dsDepth;
            if (nodeId > 0)
            {
                Model.CMS.PhotoClass photoClass = photoBLL.GetModel(nodeId);
                dsDepth = photoBLL.GetList("Depth=" + photoClass.Depth);
            }
            else
            {
                dsDepth = photoBLL.GetList("Depth=1");
            }
            if (dsDepth.Tables[0].Rows.Count < 1)
            {
                json.Accumulate("STATUS", "NODATA");
                context.Response.Write(json.ToString());
                return;
            }
            json.Accumulate("STATUS", "OK");
            json.Accumulate("DATA", dsDepth.Tables[0]);
            context.Response.Write(json.ToString());
        }

        private void GetParentNode(HttpContext context)
        {
            int ParentId = Globals.SafeInt(context.Request.Params["NodeId"], 0);
            JsonObject json = new JsonObject();
            DataSet ds = photoBLL.GetList("");
            if (ds != null && ds.Tables.Count > 0)
            {
                DataTable dt = ds.Tables[0];
                Model.CMS.PhotoClass photoClass = photoBLL.GetModel(ParentId);
                if (photoClass != null)
                {

                    string[] strList = photoClass.Path.TrimEnd('|').Split('|');
                    string strClassID = string.Empty;
                    if (strList.Length > 0)
                    {
                        List<DataRow[]> list = new List<DataRow[]>();
                        foreach (string str in strList)
                        {
                            DataRow[] dsParent = dt.Select("ParentId=" + str);
                            list.Add(dsParent);
                        }
                        json.Accumulate("STATUS", "OK");
                        json.Accumulate("DATA", list);
                        json.Accumulate("PARENT", strList);
                    }
                    else
                    {
                        json.Accumulate("STATUS", "NODATA");
                        context.Response.Write(json.ToString());
                        return;
                    }
                }
            }

            context.Response.Write(json.ToString());
        }
    }
}