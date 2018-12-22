
<%@ Page Title="等级成长值明细管理" Language="C#" MasterPageFile="~/Admin/Basic.Master" AutoEventWireup="true"
CodeBehind="DetailList.aspx.cs" Inherits="YSWL.MALL.Web.Admin.Members.UserRank.DetailList" %>

<%@ Register Assembly="YSWL.MALL.Web" Namespace="YSWL.MALL.Web.Controls" TagPrefix="Grv" %>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
 <link href="/Scripts/jqueryui/jquery-ui-1.8.19.custom.css" rel="stylesheet" type="text/css" />
    <script src="/Scripts/jqueryui/jquery-ui-1.8.19.custom.min.js" type="text/javascript"></script>
    <script src="/admin/js/jqueryui/JqueryDataPicker4CN.js" type="text/javascript"></script>
    <script type="text/javascript">
        $(document).ready(function () {
            $.datepicker.setDefaults($.datepicker.regional['zh-CN']);
            $("#ctl00_ContentPlaceHolder1_txtFrom").prop("readonly", true).datepicker({

                changeMonth: true,
                dateFormat: "yy-mm-dd",
                onClose: function (selectedDate) {
                    $("#ctl00_ContentPlaceHolder1_txtTo").datepicker("option", "minDate", selectedDate);
                }
            });
            $("#ctl00_ContentPlaceHolder1_txtTo").prop("readonly", true).datepicker({

                changeMonth: true,
                dateFormat: "yy-mm-dd",
                onClose: function (selectedDate) {
                    $("#ctl00_ContentPlaceHolder1_txtFrom").datepicker("option", "maxDate", selectedDate);
                    $("#ctl00_ContentPlaceHolder1_txtTo").val($(this).val());
                }
            });
        });
    </script>
    <div class="newslistabout">
        <div class="newslist_title">
            <table width="100%" border="0" cellspacing="0" cellpadding="0" class="borderkuang">
                <tr>
                    <td bgcolor="#FFFFFF" class="newstitle">
                        <asp:Literal ID="Literal1" runat="server" Text="等级成长值明细管理" />
                    </td>
                </tr>
                <tr>
                    <td bgcolor="#FFFFFF" class="newstitlebody">
                        <asp:Literal ID="Literal4" runat="server" Text="您可以查看网站会员的等级成长值明细。" />
                    </td>
                </tr>
            </table>
        </div>
        <table width="100%" border="0" cellspacing="0" cellpadding="0" class="borderkuang-noc">
            <tr>
                <td width="450px">
                    成长值时间：
                    <asp:TextBox ID="txtFrom" runat="server" Width="120"></asp:TextBox>
                    --
                    <asp:TextBox ID="txtTo" runat="server" Width="120"></asp:TextBox>
                    <asp:Button ID="Button1" runat="server" Text="<%$ Resources:Site, btnSearchText %>" OnClick="btnSearch_Click"  class="adminsubmit-short mar-le"></asp:Button>
                </td>
            </tr>
        </table>
           <br />
        <Grv:GridViewEx ID="gridView" runat="server" AllowPaging="True" AllowSorting="True"
            ShowToolBar="True" AutoGenerateColumns="False" OnBind="BindData" OnPageIndexChanging="gridView_PageIndexChanging"
            OnRowDataBound="gridView_RowDataBound" UnExportedColumnNames="Modify"
            Width="100%" PageSize="10" DataKeyNames="DetailID" ShowExportExcel="False" ShowExportWord="False"
            ExcelFileName="FileName1" CellPadding="3" BorderWidth="1px" ShowCheckAll="false">
            <Columns>
                 <asp:TemplateField HeaderText="成长值时间" ItemStyle-HorizontalAlign="center" ItemStyle-Width="150" >
                    <ItemTemplate>
                            <%#Eval("CreatedDate")%>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="规则名称" ItemStyle-HorizontalAlign="center" ItemStyle-Width="150">
                    <ItemTemplate>
                            <%# GetRuleName( Eval("RuleId"))%>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="用户名" ItemStyle-HorizontalAlign="left" ItemStyle-Width="150">
                    <ItemTemplate>
                            <%# GetUserName(Eval("UserID"))%>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:BoundField DataField="Score" HeaderText="影响分数" SortExpression="Score" ItemStyle-HorizontalAlign="center"  ItemStyle-Width="80"/>
                <asp:BoundField DataField="Description" HeaderText="成长值描述" SortExpression="Description"
                    ItemStyle-HorizontalAlign="left" />
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

