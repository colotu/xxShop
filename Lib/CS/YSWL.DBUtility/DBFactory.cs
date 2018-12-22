using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Web.UI.WebControls;

namespace YSWL.DBUtility
{
    /// <summary>
    /// 数据库工厂（根据配置创建具体数据库对象）
    /// </summary>
    public class DBFactory:IDisposable
    {
        /// <summary>
        /// 具体链接对象内存字典
        /// </summary>
         protected static Dictionary<string, DBBase> dbObjects;
       
        /// <summary>
        /// 初始化字典
        /// </summary>
       public static void InitDBFactory()
        {
            dbObjects = new Dictionary<string, DBBase>();
        }

        public static void Disposable()
        {
            dbObjects = null;
        }

        public void Dispose()
        {
            dbObjects = null;
        }

        /// <summary>
        /// 根据配置获取具体操作底层数据库对象
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public static DBBase CreateDBObject(string key)
        {
           
            if (dbObjects != null && dbObjects.Any(k => k.Key == key))
            {
                return dbObjects[key];
            }
            string pathStr = ConfigurationManager.AppSettings[key];
            if (String.IsNullOrWhiteSpace(pathStr))
            {
                pathStr = "DbHelperSQLA,YSWL.DBUtility";
            }
            //AssemblyPath   路径：命名空间.类名,程序集名称
            string[] assemblyPaths = pathStr.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
            DBBase dbTemp = System.Reflection.Assembly.Load(assemblyPaths[1]).CreateInstance(assemblyPaths[1] + "." + assemblyPaths[0]) as DBBase;
            if (dbObjects != null)
            {
                dbObjects.Add(key, dbTemp);
            }
           
            return dbTemp;
        }
    }
}
