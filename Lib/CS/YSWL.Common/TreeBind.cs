/**
* TreeBind.cs
*
* 功 能： 递归绑定数据
* 类 名： TreeBind
*
* Ver    变更日期             负责人  变更内容
* ───────────────────────────────────
* V0.01  2012/5/25 14:42:05  伍伟    初版
*
* Copyright (c) 2012 YSWL Corporation. All rights reserved.
*┌──────────────────────────────────┐
*│　此技术信息为本公司机密信息，未经本公司书面同意禁止向第三方披露．　│
*│　版权所有：云商未来（北京）科技有限公司　　　　　　　　　　　　　　│
*└──────────────────────────────────┘
*/
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Web.UI.WebControls;

namespace YSWL.Common
{
    public class TreeBind
    {
        public static void SetMultiLevelDropDownList(DropDownList dropDownList, string parentColName, DataTable dataTable)
        {
            string nodeid;
            //加载树
            DataRow[] drs = dataTable.Select(parentColName + "= " + 0);
            foreach (DataRow r in drs)
            {
                nodeid = r[0].ToString();
                dropDownList.Items.Add(new ListItem("╋" + Globals.HtmlDecode(r[1].ToString()),nodeid));
                BindNode(dropDownList, parentColName, nodeid, dataTable, "├");
            }
        }

        private static void BindNode(DropDownList dropDownList, string parentColName, string parentid, DataTable dt, string blank)
        {
            DataRow[] drs = dt.Select(parentColName + "= " + parentid);
            string nodeid;
            foreach (DataRow r in drs)
            {
                nodeid = r[0].ToString();
                dropDownList.Items.Add(new ListItem(blank +"『" + Globals.HtmlDecode(r[1].ToString()) + "』",nodeid));
                BindNode(dropDownList, parentColName, nodeid, dt, blank + "─");
            }
        }
    }
}
