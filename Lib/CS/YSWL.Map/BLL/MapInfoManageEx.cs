using System;
using System.Data;
using System.Collections.Generic;

namespace YSWL.Map.BLL
{
    /// <summary>
    /// BaiduMap
    /// </summary>
    public partial class MapInfoManage
    {
        /// <summary>
        /// 是否存在该企业记录
        /// </summary>
        public bool ExistsByDepartmentId(int departmentId)
        {
            return dal.ExistsByDepartmentId(departmentId);
        }

        /// <summary>
        /// 得到一个对象实体
        /// </summary>
        public YSWL.Map.Model.MapInfo GetModelByDepartmentId(int departmentId)
        {
            return dal.GetModelByDepartmentId(departmentId);
        }
    }
}

