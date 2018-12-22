using System;
using System.Data;
using YSWL.MALL.DALFactory;

namespace YSWL.MALL.BLL.Ms.EmailRole
{
	/// <summary>
	/// 接口层EmailTemplet
	/// </summary>
    public class EmailRoleAction
	{
	    private YSWL.MALL.IDAL.Ms.EmailRole.IEmailRoleAction dal = DAMs.CreateEmailRoleAction();
	  public int Add(int roleId, int emailActionId)
	  {
	     return   dal.Add(roleId, emailActionId);
	  }
	  public bool Delete(int roleId)
	  {
	      return dal.Delete(roleId);
	  }

	    #region  MethodEx

	    #endregion  MethodEx
	} 
}
