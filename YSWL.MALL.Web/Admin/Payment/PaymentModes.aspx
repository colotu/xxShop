<%@ Page Title="<%$Resources:AddPaymentMode,ptPaymentModes %>"  Language="C#" MasterPageFile="~/Admin/Basic.Master"
    AutoEventWireup="true" CodeBehind="PaymentModes.aspx.cs" Inherits="YSWL.MALL.Web.Admin.TaoPayment.PaymentModes" %>
  <%@ Register Assembly="YSWL.MALL.Web" Namespace="YSWL.MALL.Web.Controls" TagPrefix="cc1" %>
<%@ Register TagPrefix="YSWL" Namespace="YSWL.Controls" Assembly="YSWL.Controls" %>
<%@ Register TagPrefix="YSWL" Namespace="YSWL.Web.Validator" Assembly="YSWL.Web.Validator" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link rel="stylesheet" href="../css/YSWLv5.css" type="text/css" />
    <script type="text/javascript" src="../js/YSWLv5.js"></script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="newslistabout">
        <YSWL:StatusMessage ID="statusMessage" runat="server" Visible="False" />
        <div class="newslist_title">
            <table width="100%" border="0" cellspacing="0" cellpadding="0" class="borderkuang padd-no">
                <tr>
                    <td bgcolor="#FFFFFF" class="newstitle">
                        <asp:Literal ID="Literal1" runat="server" Text="<%$Resources:AddPaymentMode,ptPaymentModes %>" />
                    </td>
                </tr>
                <tr>
                    <td bgcolor="#FFFFFF" class="newstitlebody">
                        <asp:Literal ID="Literal2" runat="server" Text="<%$Resources:AddPaymentMode,lblPaymentModes %>" />
                    </td>
                </tr>
            </table>
        </div>
        <table width="100%" border="0" cellspacing="0" cellpadding="0" class="borderkuang borderkuang-noc padd-no">
            <tr>
                <td height="35" class="newstitlebody">
                    <asp:Label ID="Label1" runat="server">
                        <asp:Literal ID="Literal4" runat="server" Text="<%$ Resources:Site, lblKeyword%>" />：</asp:Label><asp:TextBox
                            ID="txtKeyword" runat="server"></asp:TextBox><asp:Button ID="btnSearch"
                                runat="server" Text="<%$ Resources:Site, btnSearchText %>" OnClick="btnSearch_Click"
                                class="add-btn"></asp:Button>
                </td>
            </tr>
        </table>
        <div class="newslist mar-bt">
            <div class="newsicon">
                <ul>
                    <li class="add-btn" id="liAdd" runat="server"><a href="AddPaymentMode.aspx">
                        <asp:Literal ID="Literal3" runat="server" Text="<%$Resources:Site,lblAdd %>" /></a>
                         </li>
                </ul>
            </div>
        </div>
            <cc1:GridViewEx ID="grdPaymentMode" runat="server" AllowPaging="True" AllowSorting="True"
            ShowToolBar="False" AutoGenerateColumns="False" OnBind="BindData" OnPageIndexChanging="gridView_PageIndexChanging"
            OnRowDataBound="grdPaymentMode_RowDataBound" UnExportedColumnNames="Modify" Width="100%"
            PageSize="10" ShowExportExcel="false" ShowExportWord="False" CellPadding="3"
            BorderWidth="1px" ShowCheckAll="true" DataKeyNames="ModeId">
                    <Columns>
               <asp:TemplateField HeaderText="<%$ Resources:PaymentModes, IDS_Header_Name %>" ItemStyle-Width="25%">
                    <ItemTemplate>
                        <asp:Label ID="lblModeName" runat="server" Text='<%# Eval("Name") %>'></asp:Label>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:BoundField DataField="merchantCode" HeaderText="<%$ Resources:PaymentModes, IDS_Header_merchantCode %>">
                </asp:BoundField>
                <asp:TemplateField HeaderText="<%$ Resources:PaymentModes, IDS_Header_Gateway %>">
                    <ItemTemplate>
                        <asp:Label ID="lblGatawayType" runat="server" Text='<%# Eval("Gateway") %>'></asp:Label>
                    </ItemTemplate>
                </asp:TemplateField>
<%--                <YSWL:SortImageColumn HeaderText="<%$ Resources:PaymentModes,IDS_Header_DisplaySequence %>" />--%>
                <asp:TemplateField HeaderText="编辑">
                    <ItemStyle Width="100px" CssClass="GridViewTyle" />
                    <ItemTemplate>
                        <a style="color: #1317FC;" class="iframe-modf" href='EditPaymentMode.aspx?modeId=<%# Eval("ModeId") %>'>
                            <asp:Literal ID="lblManagerText" runat="server" Text="<%$ Resources:Resources, IDS_Button_Edit %>"></asp:Literal></a>
                        &nbsp;&nbsp;
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="删除">
                    <ItemStyle Width="100px" CssClass="GridViewTyle" />
                    <ItemTemplate>
                        <YSWL:DeleteImageLinkButton ID="DeleteImageLinkButton1" CssClass="iframe-modf" Style="color: #1317FC;" runat="server"
                            Text="<%$ Resources:Resources, IDS_Button_Delete %>" CommandName="Delete" />
                    </ItemTemplate>
                </asp:TemplateField>
            </Columns>
              <FooterStyle Height="25px" HorizontalAlign="Right" />
            <HeaderStyle Height="35px" />
            <PagerStyle Height="25px" HorizontalAlign="Right" />
            <SortTip AscImg="~/Images/up.JPG" DescImg="~/Images/down.JPG" />
            <RowStyle Height="25px" />
        </cc1:GridViewEx>
        <YSWL:ValidatorContainer runat="server" ID="ValidatorContainer" />
    </div>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceCheckright" runat="server">
</asp:Content>
