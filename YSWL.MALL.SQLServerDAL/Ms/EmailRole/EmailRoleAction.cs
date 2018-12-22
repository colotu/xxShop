using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using YSWL.DBUtility;
using YSWL.MALL.IDAL.Ms.EmailRole;

namespace YSWL.MALL.SQLServerDAL.Ms.EmailRole
{
    class EmailRoleAction : IEmailRoleAction
    {
        public int Add(int roleId, int emailActionId)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("insert into Ms_EmailRoleAction(RoleId,ActionId)  values(");
            strSql.Append(" @RoleId,@ActionId)");
            SqlParameter[] parameters = {
					new SqlParameter("@RoleId", SqlDbType.Int,4),
                    new SqlParameter("@ActionId", SqlDbType.Int,4)
			};
            parameters[0].Value = roleId;
            parameters[1].Value = emailActionId;
            int rows = DBHelper.DefaultDBHelper.ExecuteSql(strSql.ToString(), parameters);
            return rows;
        }

        public bool Delete(int roleId)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("delete from Ms_EmailRoleAction ");
            strSql.Append(" where RoleId=@RoleId ");
            SqlParameter[] parameters = {
					new SqlParameter("@RoleId", SqlDbType.Int,4)
			};
            parameters[0].Value = roleId;
            int rows = DBHelper.DefaultDBHelper.ExecuteSql(strSql.ToString(), parameters);
            if (rows > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
