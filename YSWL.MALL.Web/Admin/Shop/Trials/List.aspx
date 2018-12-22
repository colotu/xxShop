<%@ Page Title="试用管理" Language="C#" MasterPageFile="~/Admin/Basic.Master" AutoEventWireup="true" CodeBehind="List.aspx.cs" Inherits="YSWL.MALL.Web.Admin.Shop.Trials.List" %>

<%@ Register Assembly="YSWL.MALL.Web" Namespace="YSWL.MALL.Web.Controls" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <!--Title -->
    <div class="newslistabout">
        <div class="newslist_title">
            <table width="100%" border="0" cellspacing="0" cellpadding="0" class="borderkuang">
                <tr>
                    <td bgcolor="#FFFFFF" class="newstitle">
                        <asp:Literal ID="Literal1" runat="server" Text="试用管理" />
                    </td>
                </tr>
                <tr>
                    <td bgcolor="#FFFFFF" class="newstitlebody">
                        您可以新增、修改、删除
                        <asp:Literal ID="Literal3" runat="server" Text="试用" />
                    </td>
                </tr>
            </table>
        </div>
        <div class="newslist_title">
            <table width="100%" border="0" cellspacing="0" cellpadding="0" class="borderkuang">
                <tr>
                    <td bgcolor="#FFFFFF" class="newstitlebody">
                        更多试用链接:
                        <asp:TextBox runat="server" ID="txtMore" Width="300"></asp:TextBox>
                        <asp:Button runat="server" OnClick="btnSaveMore_OnClick" ID="btnSaveMore" Text="保存修改" class="adminsubmit"></asp:Button>
                    </td>
                </tr>
            </table>
        </div>
        <!--Title end -->
        <!--Add  -->
        <!--Add end -->
        <!--Search -->
        <table width="100%" border="0" cellspacing="0" cellpadding="0" class="borderkuang">
            <tr>
                <td height="35" bgcolor="#FFFFFF" class="newstitlebody">
                    <asp:Literal ID="Literal2" runat="server" Text="<%$ Resources:Site, lblSearch%>" />：
                    <asp:TextBox ID="txtKeyword" runat="server"></asp:TextBox>
                    <asp:Button ID="btnSearch" runat="server" Text="<%$ Resources:Site, btnSearchText %>" OnClick="btnSearch_Click" class="adminsubmit"></asp:Button>
                </td>
            </tr>
        </table>
        <!--Search end-->
        <div class="newslist mar-bt">
            <div class="newsicon">
                <ul>
                    <li class="add-btn"><a href="add.aspx">新增</a></li>
                    <li class="add-btn"><a href="#">删除</a></li>
                </ul>
            </div>
        </div>
        <cc1:GridViewEx ID="gridView" runat="server" AllowPaging="True" AllowSorting="True" ShowToolBar="false" AutoGenerateColumns="False" OnBind="BindData" OnPageIndexChanging="gridView_PageIndexChanging" OnRowDataBound="gridView_RowDataBound" OnRowDeleting="gridView_RowDeleting" UnExportedColumnNames="Modify" Width="100%" PageSize="10" ShowExportExcel="False" ShowExportWord="False" ExcelFileName="FileName1" CellPadding="3" BorderWidth="1px" ShowCheckAll="true" DataKeyNames="TrialId">
            <Columns>
                <asp:BoundField DataField="TrialName" HeaderText="名称" ItemStyle-HorizontalAlign="Left" />
                <asp:BoundField DataField="LinklUrl" HeaderText="试用链接" ItemStyle-HorizontalAlign="Left" />
                <asp:TemplateField HeaderText="状态" ItemStyle-HorizontalAlign="Center">
                    <ItemTemplate>
                        <asp:Label runat="server" Text='<%# GetTrialStatus(Eval("TrialStatus")) %>' />
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:BoundField DataField="StartDate" HeaderText="开始时间" SortExpression="StartDate" DataFormatString="{0:yyyy-MM-dd}" ItemStyle-HorizontalAlign="Center" />
                <asp:BoundField DataField="EndDate" HeaderText="结束时间" SortExpression="EndDate" DataFormatString="{0:yyyy-MM-dd}" ItemStyle-HorizontalAlign="Center" />
                <asp:BoundField DataField="TrialCounts" HeaderText="试用总数" SortExpression="TrialCounts" ItemStyle-HorizontalAlign="Right" />
                <asp:BoundField Visible="False" DataField="DisplaySequence" HeaderText="显示顺序" SortExpression="DisplaySequence" ItemStyle-HorizontalAlign="Center" />
                <asp:BoundField DataField="MarketPrice" HeaderText="市场价" SortExpression="MarketPrice" DataFormatString="{0:F}" ItemStyle-HorizontalAlign="Right" />
                <asp:HyperLinkField HeaderText="<%$ Resources:Site, btnDetailText %>" ControlStyle-Width="50" DataNavigateUrlFields="TrialId" DataNavigateUrlFormatString="Show.aspx?id={0}" Text="<%$ Resources:Site, btnDetailText %>" ItemStyle-HorizontalAlign="Center" />
                <asp:HyperLinkField HeaderText="<%$ Resources:Site, btnEditText %>" ControlStyle-Width="50" DataNavigateUrlFields="TrialId" DataNavigateUrlFormatString="Modify.aspx?id={0}" Text="<%$ Resources:Site, btnEditText %>" ItemStyle-HorizontalAlign="Center" />
                <asp:TemplateField ControlStyle-Width="50" HeaderText="<%$ Resources:Site, btnDeleteText %>" ItemStyle-HorizontalAlign="Center">
                    <ItemTemplate>
                        <asp:LinkButton ID="LinkButton1" runat="server" CausesValidation="False" CommandName="Delete" Text="<%$ Resources:Site, btnDeleteText %>"></asp:LinkButton>
                    </ItemTemplate>
                </asp:TemplateField>
            </Columns>
            <FooterStyle Height="25px" HorizontalAlign="Right" />
             <HeaderStyle Height="35px" />
            <PagerStyle Height="25px" HorizontalAlign="Right" />
            <SortTip AscImg="~/Images/up.JPG" DescImg="~/Images/down.JPG" />
            <RowStyle Height="25px" />
            <SortDirectionStr>DESC</SortDirectionStr>
        </cc1:GridViewEx>
        <table border="0" cellpadding="0" cellspacing="1" style="width: 100%; height: 100%;" class="def-wrapper">
            <tr>
                <td>
                    <asp:Button ID="btnDelete" runat="server" Text="<%$ Resources:Site, btnDeleteListText %>" class="adminsubmit" OnClick="btnDelete_Click" />
                </td>
            </tr>
        </table>
    </div>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceCheckright" runat="server">
</asp:Content>
