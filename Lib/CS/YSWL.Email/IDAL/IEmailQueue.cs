/**
* IEmailQueue.cs
*
* 功 能： [N/A]
* 类 名： IEmailQueue
*
* Ver    变更日期             负责人  变更内容
* ───────────────────────────────────
* V0.01  2013/3/4 19:45:13  Ben    初版
*
* Copyright (c) 2013 YSWL Corporation. All rights reserved.
*┌──────────────────────────────────┐
*│　此技术信息为本公司机密信息，未经本公司书面同意禁止向第三方披露．　│
*│　版权所有：云商未来（北京）科技有限公司　　　　　　　　　　　　　　│
*└──────────────────────────────────┘
*/
using System.Data;

namespace YSWL.Email.IDAL
{
    public interface IEmailQueue
    {
        /// <summary>
        /// 增加一条数据
        /// </summary>
        bool Add(YSWL.Email.Model.EmailQueue model);

        /// <summary>
        /// 更新一条数据
        /// </summary>
        bool Update(YSWL.Email.Model.EmailQueue model);

        /// <summary>
        /// 删除一条数据
        /// </summary>
        bool Delete();

        /// <summary>
        /// 得到一个对象实体
        /// </summary>
        YSWL.Email.Model.EmailQueue GetModel();

        /// <summary>
        /// 获得数据列表
        /// </summary>
        DataSet GetList(string strWhere);

        /// <summary>
        /// 获得前几行数据
        /// </summary>
        DataSet GetList(int Top, string strWhere, string filedOrder);

        /// <summary>
        /// 获取记录总数
        /// </summary>
        int GetRecordCount(string strWhere);

        /// <summary>
        /// 分页获取数据列表
        /// </summary>
        DataSet GetListByPage(string strWhere, string orderby, int startIndex, int endIndex);

        /// <summary>
        /// 将邮件加入邮件发送队列
        /// </summary>
        bool PushEmailQueur(string uType, string uName, string EmailSubject, string EmailBody, string EmailFrom);

        bool Exists(string emailSubject);
    }

}
