<%@ Page Language="C#" MasterPageFile="~/Admin/BasicNoFoot.Master" AutoEventWireup="true"
    CodeBehind="Show.aspx.cs" Inherits="YSWL.MALL.Web.Admin.Pay.BalanceDrawRequest.Show"
    Title="显示页" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="newslistabout">
        <div class="newslist_title">
            <table width="100%" border="0" cellspacing="0" cellpadding="0" class="borderkuang">
                <tr>
                    <td bgcolor="#FFFFFF" class="newstitle">
                        <asp:Literal ID="Literal2" runat="server" Text="提现信息" />
                    </td>
                </tr>
                <tr>
                    <td bgcolor="#FFFFFF" class="newstitlebody">
                        <asp:Literal ID="Literal3" runat="server" Text="提现信息" />查看
                    </td>
                </tr>
            </table>
        </div>
        <table style="width: 100%;" cellpadding="2" cellspacing="1" class="border">
            <tr>
                <td class="tdbg">
                    <table cellspacing="3" cellpadding="3" width="100%" border="0">
                        <tr style="display: none">
                            <td height="25" width="30%" align="right">
                                流水号 ：
                            </td>
                            <td height="25" width="*" align="left">
                                <asp:Label ID="lblJournalNumber" runat="server"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td height="25" width="30%" align="right">
                                请求时间 ：
                            </td>
                            <td height="25" width="*" align="left">
                                <asp:Label ID="lblRequestTime" runat="server"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td height="25" width="30%" align="right">
                                金额 ：
                            </td>
                            <td height="25" width="*" align="left">
                                <asp:Label ID="lblAmount" runat="server"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td height="25" width="30%" align="right">
                                申请人 ：
                            </td>
                            <td height="25" width="*" align="left">
                                <asp:Label ID="lblUserID" runat="server"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td height="25" width="30%" align="right">
                                开户姓名 ：
                            </td>
                            <td height="25" width="*" align="left">
                                <asp:Label ID="lblTrueName" runat="server"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td height="25" width="30%" align="right">
                                银行名称 ：
                            </td>
                            <td height="25" width="*" align="left">
                                <asp:Label ID="lblBankName" runat="server"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td height="25" width="30%" align="right">
                                帐号 ：
                            </td>
                            <td height="25" width="*" align="left">
                                <asp:Label ID="lblBankCard" runat="server"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td class="td_class">
                                帐号类型 ：
                            </td>
                            <td height="25">
                                <asp:RadioButtonList ID="radioCardType" runat="server" Enabled="false" RepeatDirection="Horizontal">
                                    <asp:ListItem Value="1">银行帐号</asp:ListItem>
                                    <asp:ListItem Value="2">支付宝帐号</asp:ListItem>
                                </asp:RadioButtonList>
                            </td>
                        </tr>
                        <tr style="display: none">
                            <td class="td_class">
                                状态 ：
                            </td>
                            <td height="25">
                                <asp:DropDownList ID="dropStatus" runat="server" Enabled="false">
                                    <asp:ListItem Value="0">请选择</asp:ListItem>
                                    <asp:ListItem Value="1">未审核</asp:ListItem>
                                    <asp:ListItem Value="2">审核失败</asp:ListItem>
                                    <asp:ListItem Value="3">审核通过</asp:ListItem>
                                </asp:DropDownList>
                            </td>
                        </tr>
                        <tr>
                            <td height="25" width="30%" align="right">
                                备注 ：
                            </td>
                            <td height="25" width="*" align="left">
                                <asp:Label ID="txtRemark" runat="server"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td class="td_class">
                            </td>
                            <td height="25">
                                <asp:Button ID="btnCancle" runat="server" Text="<%$ Resources:Site, btnCancleText %>"
                                    class="adminsubmit_short" OnClick="btnCancle_Click" OnClientClick=" javascript: parent.$.colorbox.close();">
                                </asp:Button>
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
        </table>
    </div>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceCheckright" runat="server">
</asp:Content>
