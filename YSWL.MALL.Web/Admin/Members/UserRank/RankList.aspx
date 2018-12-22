<%@ Page Title="用户等级" Language="C#" MasterPageFile="~/Admin/Basic.Master" AutoEventWireup="true"
    CodeBehind="RankList.aspx.cs" Inherits="YSWL.MALL.Web.Admin.Members.UserRank.RankList" %>

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
                        会员等级等级计算规则
                    </td>
                </tr>
                <tr>
                    <td bgcolor="#FFFFFF" class="newstitlebody">
                        会员等级是根据用户成长值确定的。等级越高，享有的特权越多。
                    </td>
                </tr>
            </table>
        </div>
        <!--Title end -->
        <!--Add  -->
        <!--Add end -->
        <!--Search -->
        <table width="100%" border="0" cellspacing="0" cellpadding="0" class="borderkuang-noc">
            <tr>
                <td height="35" bgcolor="#FFFFFF" class="newstitlebody">
                    <asp:Literal ID="Literal2" runat="server" Text="<%$ Resources:Site, lblSearch%>" />：
                    <asp:TextBox ID="txtKeyword" runat="server"></asp:TextBox>
                    <asp:Button ID="btnSearch" runat="server" Text="<%$ Resources:Site, btnSearchText %>"
                        OnClick="btnSearch_Click" class="adminsubmit-short add-btn mar-le"></asp:Button>
                </td>
            </tr>
        </table>
        <!--Search end-->
        <div class="newslist mar-bt">
            <div class="newsicon">
                <ul>
                    <li id="liAdd" class="add-btn" runat="server">
                        <a href="AddRank.aspx">新增</a> <%--<b>|</b>--%> </li>
                </ul>
            </div>
        </div>
        <cc1:GridViewEx ID="gridView" runat="server" AllowPaging="True" AllowSorting="True"
            ShowToolBar="True" AutoGenerateColumns="False" OnBind="BindData" OnPageIndexChanging="gridView_PageIndexChanging"
            OnRowDataBound="gridView_RowDataBound" OnRowDeleting="gridView_RowDeleting" UnExportedColumnNames="Modify"
            Width="100%" PageSize="20" ShowExportExcel="False" ShowExportWord="False" ExcelFileName="FileName1"
            CellPadding="3" BorderWidth="1px" ShowCheckAll="false" DataKeyNames="RankId">
            <Columns>
                <asp:BoundField DataField="RankLevel" HeaderText="等级" ItemStyle-HorizontalAlign="Center"
                    ItemStyle-Width="50" />
                <asp:BoundField DataField="Name" HeaderText="等级名称" ItemStyle-HorizontalAlign="Left"
                    ItemStyle-Width="120" />
                <asp:TemplateField HeaderText="等级图标" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="80">
                    <ItemTemplate>
                        <span class='W_level_ico<%#Eval("RankLevel") %>'><span node-type="level"></span>
                        </span>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="成长值下限" ItemStyle-HorizontalAlign="Right" ItemStyle-Width="100">
                    <ItemTemplate>
                        <%#Eval("ScoreMin")%>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="成长值上限" ItemStyle-HorizontalAlign="Right" ItemStyle-Width="100">
                    <ItemTemplate>
                        <%#Eval("ScoreMax")%>
                    </ItemTemplate>
                </asp:TemplateField>
                 <asp:TemplateField HeaderText="等级类型" ItemStyle-HorizontalAlign="left" ItemStyle-Width="80">
                    <ItemTemplate>
                        <%#GetRankType(Eval("RankType"))%>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:BoundField DataField="Description" HeaderText="描述" ItemStyle-HorizontalAlign="Left" />
                <asp:HyperLinkField HeaderText="<%$ Resources:Site, btnEditText %>" ItemStyle-Width="50"
                    DataNavigateUrlFields="RankId" DataNavigateUrlFormatString="UpdateRank.aspx?id={0}"
                    Text="<%$ Resources:Site, btnEditText %>" ItemStyle-HorizontalAlign="Center" />
                <asp:TemplateField ControlStyle-Width="50" HeaderText="<%$ Resources:Site, btnDeleteText %>"
                    ItemStyle-HorizontalAlign="Center" ItemStyle-Width="50">
                    <ItemTemplate>
                        <asp:LinkButton ID="LinkButton1" runat="server" CausesValidation="False" CommandName="Delete"
                            OnClientClick='return confirm($(this).attr("ConfirmText"))' ConfirmText="<%$Resources:Site,TooltipDelConfirm %>"
                            Text="<%$ Resources:Site, btnDeleteText %>"></asp:LinkButton>
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
        <table border="0" cellpadding="0" cellspacing="1" style="width: 100%; height: 100%; display: none;" class="def-wrapper">
            <tr>
                <td>
                    <asp:Button ID="btnDelete" runat="server" Text="<%$ Resources:Site, btnDeleteListText %>"
                        OnClientClick='return confirm($(this).attr("ConfirmText"))' ConfirmText="<%$Resources:Site,TooltipDelConfirm %>"
                        class="adminsubmit" OnClick="btnDelete_Click" />
                </td>
            </tr>
        </table>
    </div>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceCheckright" runat="server">
</asp:Content>
