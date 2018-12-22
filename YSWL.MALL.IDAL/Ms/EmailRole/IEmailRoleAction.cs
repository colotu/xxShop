using System;
using System.Data;
namespace YSWL.MALL.IDAL.Ms.EmailRole
{
	/// <summary>
	/// 接口层EmailTemplet
	/// </summary>
    public interface IEmailRoleAction
	{

	    int Add(int roleId, int emailActionId);
	    bool Delete(int roleId);

	    #region  MethodEx

	    #endregion  MethodEx
	} 
}
