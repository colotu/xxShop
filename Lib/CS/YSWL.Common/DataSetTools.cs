using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace YSWL.Common
{
    /// <summary>
    /// DataSet工具处理类
    /// </summary>
    public class DataSetTools
    {
        /// <summary>
        /// 从DataSet获取所有行
        /// </summary>
        /// <returns></returns>
        public static DataRowCollection GetDataSetRows(DataSet ds)
        {            
            if (ds.Tables.Count > 0)
            {
                return ds.Tables[0].Rows;
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// 判断DataSet是否为Null
        /// </summary>
        /// <param name="ds"></param>
        /// <returns></returns>
        public static bool DataSetIsNull(DataSet ds)
        {
            if (null != ds && ds.Tables.Count != 0 && ds.Tables[0].Rows.Count != 0 && ds.Tables[0].Columns.Count != 0)
            {
                return false;
            }
            else
            {
                return true;
            }
        }
    }
}
