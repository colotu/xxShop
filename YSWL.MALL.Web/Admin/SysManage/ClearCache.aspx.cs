using System;
using System.Collections;
using System.Text;
using YSWL.Common;
using YSWL.DBUtility;

namespace YSWL.MALL.Web.Admin.SysManage
{
    public partial class ClearCache : PageBaseAdmin
    {
        protected override int Act_PageLoad { get { return 62; } } //系统管理_是否显示清空缓存

        protected void Page_Load(object sender, EventArgs e)
        {
        }

        protected void btnClear_Click(object sender, System.EventArgs e)
        {
            IDictionaryEnumerator de = Cache.GetEnumerator();
            ArrayList list = new ArrayList();
            StringBuilder str = new StringBuilder();
            while (de.MoveNext())
            {
                list.Add(de.Key.ToString());
            }
            foreach (string key in list)
            {
                Cache.Remove(key);
                str.Append("<li>" + key + "......OK! <br>");
            }

            #region 强制清除Redis db1 
            if (SAASInfo.GetSystemBoolValue("RedisCacheUse"))
            {
                CacheOption cacheOption = new CacheOption();
                cacheOption.CacheType = CacheType.Redis;
                cacheOption.ReadWriteHosts = SAASInfo.GetSystemValue("RedisCacheReadWriteHosts");
                cacheOption.ReadOnlyHosts = SAASInfo.GetSystemValue("RedisCacheReadOnlyHosts");
                cacheOption.DefaultDb = 1;
                Common.DataCacheCore db = new DataCacheCore(cacheOption);
                db.ClearAll();
                cacheOption.DefaultDb = 2;
                db = new DataCacheCore(cacheOption);
                db.ClearAll();
            }
            #endregion
            Common.DataCache.ClearAll();
            Label1.Text = string.Format("<br>{0}<br>{1}", str.ToString(), Resources.SysManage.lblClearSucceed);
        }
    }
}