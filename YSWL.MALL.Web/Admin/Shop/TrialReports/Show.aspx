<%@ Page Language="C#" MasterPageFile="~/Admin/Basic.Master" AutoEventWireup="true" CodeBehind="Show.aspx.cs" Inherits="YSWL.MALL.Web.Admin.Shop.TrialReports.Show" Title="显示页" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="newslistabout">
        <div class="newslist_title">
            <table width="100%" border="0" cellspacing="0" cellpadding="0" class="borderkuang">
                <tr>
                    <td bgcolor="#FFFFFF" class="newstitle">
                        <asp:Literal ID="Literal2" runat="server" Text="试用报告信息" />
                    </td>
                </tr>
                <tr>
                    <td bgcolor="#FFFFFF" class="newstitlebody">
                        <asp:Literal ID="Literal3" runat="server" Text="试用报告信息" />
                    </td>
                </tr>
            </table>
        </div>
        <table style="width: 100%;" cellpadding="2" cellspacing="1" class="border">
            <tr>
                <td class="tdbg">
                    <table cellspacing="0" cellpadding="0" width="100%" border="0">
                        <tr>
                            <td height="25" width="30%" align="right">
                                名称 ：
                            </td>
                            <td height="25" width="*" align="left">
                                <asp:Label ID="lblTitle" runat="server"></asp:Label>
                                <asp:Label ID="lblReportId" runat="server" Visible="False"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td height="25" width="30%" align="right">
                                链接 ：
                            </td>
                            <td height="25" width="*" align="left">
                                <asp:Label ID="lblLinkUrl" runat="server"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td height="25" width="30%" align="right">
                                介绍 ：
                            </td>
                            <td height="25" width="*" align="left">
                                <asp:Label ID="lblShortDescription" runat="server"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td height="25" width="30%" align="right">
                                发表者 ：
                            </td>
                            <td height="25" width="*" align="left">
                                <asp:Label ID="lblCreatedUserName" runat="server"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td height="25" width="30%" align="right">
                                图片 ：
                            </td>
                            <td height="25" width="*" align="left">
                                <asp:Image runat="server" ID="imgUrl" />
                            </td>
                        </tr><tr>
                            <td class="td_class">
                            </td>
                            <td height="25">
                                <asp:Button ID="btnCancle" runat="server" Text="返回"
                                    class="adminsubmit_short" OnClick="btnCancle_Click"></asp:Button>
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
