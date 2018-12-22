<%@ Page Language="C#" MasterPageFile="~/Admin/Basic.Master" AutoEventWireup="true" CodeBehind="Show.aspx.cs" Inherits="YSWL.MALL.Web.Admin.Shop.Trials.Show" Title="显示页" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="newslistabout">
        <div class="newslist_title">
            <table width="100%" border="0" cellspacing="0" cellpadding="0" class="borderkuang">
                <tr>
                    <td bgcolor="#FFFFFF" class="newstitle">
                        <asp:Literal ID="Literal2" runat="server" Text="试用信息" />
                    </td>
                </tr>
                <tr>
                    <td bgcolor="#FFFFFF" class="newstitlebody">
                        <asp:Literal ID="Literal3" runat="server" Text="试用信息" />
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
                                <asp:Label ID="lblTrialName" runat="server"></asp:Label>
                                 <asp:Label ID="lblTrialId" runat="server" Visible="False"></asp:Label>
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
                                描述 ：
                            </td>
                            <td height="25" width="*" align="left">
                                <asp:Label ID="lblDescription" runat="server"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td height="25" width="30%" align="right">
                                试用链接 ：
                            </td>
                            <td height="25" width="*" align="left">
                                <asp:Label ID="lblLinklUrl" runat="server"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td height="25" width="30%" align="right">
                                状态 ：
                            </td>
                            <td height="25" width="*" align="left">
                                <asp:Label ID="lblTrialStatus" runat="server"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td height="25" width="30%" align="right">
                                开始时间 ：
                            </td>
                            <td height="25" width="*" align="left">
                                <asp:Label ID="lblStartDate" runat="server"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td height="25" width="30%" align="right">
                                结束时间 ：
                            </td>
                            <td height="25" width="*" align="left">
                                <asp:Label ID="lblEndDate" runat="server"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td height="25" width="30%" align="right">
                                新增日期 ：
                            </td>
                            <td height="25" width="*" align="left">
                                <asp:Label ID="lblCreatedDate" runat="server"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td height="25" width="30%" align="right">
                                试用总数 ：
                            </td>
                            <td height="25" width="*" align="left">
                                <asp:Label ID="lblTrialCounts" runat="server"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td height="25" width="30%" align="right">
                                显示顺序 ：
                            </td>
                            <td height="25" width="*" align="left">
                                <asp:Label ID="lblDisplaySequence" runat="server"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td height="25" width="30%" align="right">
                                市场价 ：
                            </td>
                            <td height="25" width="*" align="left">
                                <asp:Label ID="lblMarketPrice" runat="server"></asp:Label>
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
