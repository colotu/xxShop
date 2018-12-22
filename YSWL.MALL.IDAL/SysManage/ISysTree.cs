﻿using System;
using System.Collections.Generic;
using System.Data;
using YSWL.MALL.Model.SysManage;
namespace YSWL.MALL.IDAL.SysManage
{

    /// <summary>
    ///  接口层-系统管理。
    /// </summary>
    public interface ISysTree
    {

        void AddLog(string time, string loginfo, string Particular);
        int AddTreeNode(YSWL.MALL.Model.SysManage.SysNode model);
        void DeleteLog(int ID);
        void DeleteLog(string strWhere);
        void DelOverdueLog(int days);
        void DelTreeNode(int NodeID);
        void DelTreeNodes(string nodeidlist);
        void MoveNodes(string nodeidlist, int ParentID);
        System.Data.DataRow GetLog(string ID);
        System.Data.DataSet GetLogs(string strWhere);
        YSWL.MALL.Model.SysManage.SysNode GetNode(int NodeID);
        int GetPermissionCatalogID(int permissionID);
        System.Data.DataSet GetTreeList(string strWhere);
        void UpdateNode(YSWL.MALL.Model.SysManage.SysNode model);

        /// <summary>
        /// 修改启用状态
        /// </summary>
        /// <param name="nodeid"></param>
        void UpdateEnabled(int nodeid);


        YSWL.MALL.Model.SysManage.SysNode DataRowToModel(DataRow row);
    }
}
