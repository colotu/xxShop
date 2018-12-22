<%@ Page Title="成长值限制管理" Language="C#" MasterPageFile="~/Admin/Basic.Master" AutoEventWireup="true"
  CodeBehind="RankLimit.aspx.cs" Inherits="YSWL.MALL.Web.Admin.Members.UserRank.RankLimit" %>

<%@ Register Assembly="YSWL.MALL.Web" Namespace="YSWL.MALL.Web.Controls" TagPrefix="Grv" %>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <script type="text/javascript">
        function GetDeleteM() {
            $("[id$='btnDelete']").click();
        }
    </script>
    <div class="newslistabout">
        <div class="newslist_title">
            <table width="100%" border="0" cellspacing="0" cellpadding="0" class="borderkuang">
                <tr>
                    <td bgcolor="#FFFFFF" class="newstitle">
                        <asp:Literal ID="Literal1" runat="server" Text="成长值限制管理" />
                    </td>
                </tr>
                <tr>
                    <td bgcolor="#FFFFFF" class="newstitlebody">
                        <asp:Literal ID="Literal4" runat="server" Text="设置每种成长值规则使用的限制条件。" />
                    </td>
                </tr>
            </table>
        </div>
          <table width="100%" border="0" cellspacing="0" cellpadding="0" class="borderkuang-noc">
            <tr>
                <td height="35" class="newstitlebody">
                    <asp:Label ID="Label1" runat="server">
                        <asp:Literal ID="Literal3" runat="server" Text="<%$ Resources:Site, lblKeyword%>" />:</asp:Label>&nbsp;&nbsp;<asp:TextBox
                            ID="txtKeyword" runat="server"></asp:TextBox><asp:Button ID="btnSearch"
                                runat="server" Text="<%$ Resources:Site, btnSearchText %>" OnClick="btnSearch_Click"
                                class="adminsubmit-short add-btn mar-le"></asp:Button>
                </td>
            </tr>
        </table>
        <br />
        <div class="newslist mar-bt">
            <div class="newsicon">
                <ul>
                    <li id="liadd" class="add-btn" runat="server"><a href="AddLimit.aspx">
                        <asp:Literal ID="Literal5" runat="server" Text="<%$ Resources:Site, lblAdd%>" /></a>
                    </li>
                </ul>
            </div>
        </div>
        <Grv:GridViewEx ID="gridView" runat="server" AllowPaging="True" AllowSorting="True"
            ShowToolBar="True" AutoGenerateColumns="False" OnBind="BindData" OnPageIndexChanging="gridView_PageIndexChanging"
            OnRowDataBound="gridView_RowDataBound" OnRowDeleting="gridView_RowDeleting" UnExportedColumnNames="Modify"
            Width="100%" PageSize="10" DataKeyNames="LimitID" ShowExportExcel="False"
            ShowExportWord="False" ExcelFileName="FileName1" CellPadding="3" BorderWidth="1px"
            ShowCheckAll="false">
            <Columns>
                <asp:BoundField DataField="LimitID" HeaderText="ID" SortExpression="LimitID"
                    ControlStyle-Width="40" ItemStyle-HorizontalAlign="center" />
                <asp:BoundField DataField="Name" HeaderText="限制名称" SortExpression="Name" ItemStyle-HorizontalAlign="center" />
                <asp:BoundField DataField="Cycle" HeaderText="周期频率" SortExpression="Cycle" ItemStyle-HorizontalAlign="center" />
<%--                <asp:BoundField DataField="CycleUnit" HeaderText="单位" SortExpression="CycleUnit"
                    ItemStyle-HorizontalAlign="center" />--%>

                    <asp:TemplateField HeaderText="单位" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="80px">
                    <ItemTemplate>
                        <%#GetUnitName(Eval("CycleUnit"))%>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:BoundField DataField="MaxTimes" HeaderText="次数限制" SortExpression="MaxTimes"
                    ItemStyle-HorizontalAlign="center" />
                <asp:HyperLinkField HeaderText="<%$ Resources:Site, btnEditText %>" ControlStyle-Width="50"
                    DataNavigateUrlFields="LimitID" DataNavigateUrlFormatString="UpdateLimit.aspx?limitId={0}"
                    Text="<%$ Resources:Site, btnEditText %>" ItemStyle-HorizontalAlign="Center" />
                <asp:TemplateField ControlStyle-Width="50" HeaderText="<%$ Resources:Site, btnDeleteText %>"
                    ItemStyle-HorizontalAlign="Center">
                    <ItemTemplate>
                        <asp:LinkButton ID="LinkButton1" runat="server" CausesValidation="False" CommandName="Delete"
                          OnClientClick='return confirm($(this).attr("ConfirmText"))' ConfirmText="<%$Resources:Site,TooltipDelConfirm %>"   Text="<%$ Resources:Site, btnDeleteText %>"></asp:LinkButton></ItemTemplate>
                </asp:TemplateField>
            </Columns>
            <FooterStyle Height="25px" HorizontalAlign="Right" />
            <HeaderStyle Height="35px" />
            <PagerStyle Height="25px" HorizontalAlign="Right" />
            <SortTip AscImg="~/Images/up.JPG" DescImg="~/Images/down.JPG" />
            <RowStyle Height="25px" />
            <SortDirectionStr>DESC</SortDirectionStr>
        </Grv:GridViewEx>
    </div>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceCheckright" runat="server">
</asp:Content>
