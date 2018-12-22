using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace YSWL.MALL.IDAL.SysManage
{
    /// <summary>
    /// 接口层IConfigSystem 的摘要说明。
    /// </summary>
    public interface IConfigSystem
    {
        int Add(string Keyname, string Value, string Description);
        int Add(string Keyname, string Value, string Description, YSWL.MALL.Model.SysManage.ApplicationKeyType KeyType);
        void Delete(int ID);
        bool Exists(string Keyname);
        System.Data.DataSet GetList(string strWhere);
        string GetValue(int ID);
        string GetValue(string Keyname);
        void Update(int ID, string Keyname, string Value, string Description);
        bool Update(string Keyname, string Value, string Description);
        bool Update(string Keyname, string Value, YSWL.MALL.Model.SysManage.ApplicationKeyType KeyType);


        void UpdateConnectionString(string connectionString);

        /// <summary>
        /// Update a record
        /// </summary>
        bool Update(string Keyname, string Value);

        string GetDescription(string Keyname);
    }
}
