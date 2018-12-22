using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace YSWL.Common
{
    public class DataTableTools
    {
        /// <summary>
        /// 判断DataTable是否为Null
        /// </summary>
        /// <param name="ds"></param>
        /// <returns></returns>
        public static bool DataTableIsNull(DataTable dt)
        {
            if (null != dt && dt.Columns.Count != 0 && dt.Rows.Count != 0)
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
