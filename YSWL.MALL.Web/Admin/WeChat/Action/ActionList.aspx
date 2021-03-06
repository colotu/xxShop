﻿<%@ Page Title="微信系统指令管理" Language="C#" MasterPageFile="~/Admin/Basic.Master" AutoEventWireup="true"
    CodeBehind="ActionList.aspx.cs" Inherits="YSWL.MALL.Web.Admin.WeChat.Action.ActionList" %>

<%@ Register Assembly="YSWL.MALL.Web" Namespace="YSWL.MALL.Web.Controls" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link href="/Admin/js/colorbox/colorbox.css" rel="stylesheet" type="text/css" />
    <script src="/Admin/js/colorbox/jquery.colorbox-min.js" type="text/javascript"></script>
    <script type="text/javascript">
        $(document).ready(function () {
            $(".iframe").colorbox({ iframe: true, width: "480", height: "380", overlayClose: false });
        });
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="newslistabout">
        <div class="newslist_title">
            <table width="100%" border="0" cellspacing="0" cellpadding="0" class="borderkuang">
                <tr>
                    <td bgcolor="#FFFFFF" class="newstitle">
                        <asp:Literal ID="Literal1" runat="server" Text="微信系统功能指令管理" />
                    </td>
                </tr>
                <tr>
                    <td bgcolor="#FFFFFF" class="newstitlebody">
                        <asp:Literal ID="Literal2" runat="server" Text="微信系统指令是程序级功能行为，只有开发人员可以新增和维护该数据。" />
                    </td>
                </tr>
            </table>
        </div>
        <table style="width: 100%;" cellpadding="2" cellspacing="1" class="border">
            <tr>
                <td class="tdbg">
                    <table cellspacing="0" cellpadding="3" width="100%" border="0">
                        <tr>
                            <td class="td_class">
                                <asp:Literal ID="Literal3" runat="server" Text="系统指令名称" />：
                            </td>
                            <td height="25">
                                <asp:TextBox ID="tName" TabIndex="1" runat="server" Width="250px" MaxLength="20"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td class="td_class">
                                <asp:Literal ID="Literal4" runat="server" Text="操作描述" />：
                            </td>
                            <td height="25">
                                <asp:TextBox ID="tDesc" runat="server" Width="250px" MaxLength="50" TextMode="MultiLine"
                                    Rows="3"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td class="td_class">
                            </td>
                            <td height="25">
                                <asp:Button ID="btnSave" runat="server" Text="<%$ Resources:Site, btnSaveText %>"
                                    OnClick="btnSave_Click" class="adminsubmit_short"></asp:Button>&nbsp;&nbsp;
                            </td>
                        </tr>
                        <tr>
                            <td colspan="2">
                                <asp:Label ID="lblMsg" runat="server" ForeColor="Red"></asp:Label>
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
        </table>
        <div class="newslist" style="display:none;">
            <div class="newsicon">
                <ul>
                    <li id="Li1" runat="server"></li>
                </ul>
            </div>
        </div>
        <cc1:GridViewEx ID="gridView" runat="server" AllowPaging="True" AllowSorting="True"
            ShowToolBar="True" AutoGenerateColumns="False" OnBind="BindData" OnPageIndexChanging="gridView_PageIndexChanging"
            OnRowDataBound="gridView_RowDataBound" UnExportedColumnNames="Modify" Width="100%"
            PageSize="10" ShowExportExcel="false" ShowExportWord="False" CellPadding="3"
            BorderWidth="1px" ShowCheckAll="true" DataKeyNames="ActionId">
            <Columns>
                <asp:TemplateField HeaderText="系统指令ID" ItemStyle-HorizontalAlign="center" ItemStyle-Width="80">
                    <ItemTemplate>
                        <%# Eval("ActionId")%>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="系统指令名称" ItemStyle-HorizontalAlign="Left" ItemStyle-Width="200">
                    <ItemTemplate>
                        <%# Eval("Name")%>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="备注" ItemStyle-HorizontalAlign="Left">
                    <ItemTemplate>
                        <%#Eval("Remark")%>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="编辑" ItemStyle-HorizontalAlign="center">
                    <ItemTemplate>
                        <a href='UpdateAction.aspx?id=<%# Eval("ActionId")%>' class="iframe">编辑</a>
                    </ItemTemplate>
                </asp:TemplateField>
            </Columns>
                <FooterStyle Height="25px" HorizontalAlign="Right" />
            <HeaderStyle Height="35px" />
            <PagerStyle Height="25px" HorizontalAlign="Right" />
            <SortTip AscImg="~/Images/up.JPG" DescImg="~/Images/down.JPG" />
            <RowStyle Height="25px" />
        </cc1:GridViewEx>
        <div class="def-wrapper">
            <asp:Button ID="btnDelete" runat="server" Text="批量删除"  OnClick="btnDelete_Click" CssClass="adminsubmit"/>
        </div>
          
    </div>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceCheckright" runat="server">
</asp:Content>
