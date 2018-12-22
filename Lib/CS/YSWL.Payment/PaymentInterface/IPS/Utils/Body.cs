using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// Body 的摘要说明
/// </summary>
public class Body
{
	public Body()
	{
        
	}
    private string m_MerBillno;
    /// <summary>
    /// 商户订单号
    /// </summary>
    public string MerBillno
    {
        get { return m_MerBillno; }
        set { m_MerBillno = value; }
    }

    private string m_Date;

     

    /// <summary>
    /// 订单日期
    /// </summary>
    public string Date
    {
        get { return m_Date; }
        set { m_Date = value; }
    }

    private string m_CurrencyType;
    /// <summary>
    /// 币种 
    /// </summary>
    public string CurrencyType
    {
        get { return m_CurrencyType; }
        set { m_CurrencyType = value; }
    }

    private string m_GatewayType;
    /// <summary>
    /// 支付方式
    /// </summary>
    public string GatewayType
    {
        get { return m_GatewayType; }
        set { m_GatewayType = value; }
    }

    private string m_Amount;
    /// <summary>
    /// 订单金额
    /// </summary>
    public string Amount
    {
        get { return m_Amount; }
        set { m_Amount = value; }
    }
    private string m_Lang;
    /// <summary>
    /// 语言 
    /// </summary>
    public string Lang
    {
        get { return m_Lang; }
        set { m_Lang = value; }
    }

    private string m_Merchanturl;
    /// <summary>
    /// 支付结果成功返回的商户URL 
    /// </summary>
    public string Merchanturl
    {
        get { return m_Merchanturl; }
        set { m_Merchanturl = value; }
    }

    private string m_FailUrl;
    /// <summary>
    /// 支付结果失败返回的商户URL 
    /// </summary>
    public string FailUrl
    {
        get { return m_FailUrl; }
        set { m_FailUrl = value; }
    }

    private string m_Attach;
    /// <summary>
    /// 商户数据包
    /// </summary>
    public string Attach
    {
        get { return m_Attach; }
        set { m_Attach = value; }
    }

    private string m_OrderEncodeType;
    /// <summary>
    /// 订单支付接口加密方式
    /// </summary>
    public string OrderEncodeType
    {
        get { return m_OrderEncodeType; }
        set { m_OrderEncodeType = value; }
    }

    private string m_RetEncodeType;
    /// <summary>
    /// 交易返回接口加密方式
    /// </summary>
    public string RetEncodeType
    {
        get { return m_RetEncodeType; }
        set { m_RetEncodeType = value; }
    }

    private string m_RetType;
    /// <summary>
    /// 返回方式 
    /// </summary>
    public string RetType
    {
        get { return m_RetType; }
        set { m_RetType = value; }
    }

    private string m_ServerUrl;
    /// <summary>
    /// 异步S2S返回
    /// </summary>
    public string ServerUrl
    {
        get { return m_ServerUrl; }
        set { m_ServerUrl = value; }
    }

    private string m_BillEXP;
    /// <summary>
    /// 订单有效期
    /// </summary>
    public string BillEXP
    {
        get { return m_BillEXP; }
        set { m_BillEXP = value; }
    }

    private string m_GoodsName;
    /// <summary>
    /// 商品名称
    /// </summary>
    public string GoodsName
    {
        get { return m_GoodsName; }
        set { m_GoodsName = value; }
    }

    private string m_IsCredit;
    /// <summary>
    /// 直连选项
    /// </summary>
    public string IsCredit
    {
        get { return m_IsCredit; }
        set { m_IsCredit = value; }
    }

    private string m_BankCode;
    /// <summary>
    /// 银行号
    /// </summary>
    public string BankCode
    {
        get { return m_BankCode; }
        set { m_BankCode = value; }
    }

    private string m_ProductType;
    /// <summary>
    /// 产品类型
    /// </summary>
    public string ProductType
    {
        get { return m_ProductType; }
        set { m_ProductType = value; }
    }
}