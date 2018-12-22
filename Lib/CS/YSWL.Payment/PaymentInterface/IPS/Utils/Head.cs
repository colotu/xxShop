using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// Head 的摘要说明
/// </summary>
public class Head
{
	public Head()
	{
	}

    private string m_Version;
    /// <summary>
    /// 版本号
    /// </summary>
    public string Version
    {
        get { return m_Version; }
        set { m_Version = value; }
    }

    private string m_MerCode;
    /// <summary>
    /// 商户号
    /// </summary>
    public string MerCode
    {
        get { return m_MerCode; }
        set { m_MerCode = value; }
    }
    
    private string m_MerName;
    /// <summary>
    /// 商户名
    /// </summary>
    public string MerName
    {
        get { return m_MerName; }
        set { m_MerName = value; }
    }

    private string m_Account;
    /// <summary>
    /// 账户号
    /// </summary>
    public string Account
    {
        get { return m_Account; }
        set { m_Account = value; }
    }

    private string m_MsgId;
    /// <summary>
    /// 消息编号
    /// </summary>
    public string MsgId
    {
        get { return m_MsgId; }
        set { m_MsgId = value; }
    }

    private string m_ReqDate;
    /// <summary>
    /// 商户请求时间
    /// </summary>
    public string ReqDate
    {
        get { return m_ReqDate; }
        set { m_ReqDate = value; }
    }

    private string m_Signature;
    /// <summary>
    /// 数字签名
    /// </summary>
    public string Signature
    {
        get { return m_Signature; }
        set { m_Signature = value; }
    }
}