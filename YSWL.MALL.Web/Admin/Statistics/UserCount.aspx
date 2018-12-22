<%@ Page Title="" Language="C#" MasterPageFile="~/Admin/Basic.Master" AutoEventWireup="true" CodeBehind="UserCount.aspx.cs" Inherits="YSWL.MALL.Web.Admin.Statistics.UserCount" %>
<%@ Register Assembly="YSWL.MALL.Web" Namespace="YSWL.MALL.Web.Controls" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
<link href="/Scripts/jqueryui/jquery-ui-1.8.19.custom.css" rel="stylesheet" type="text/css" />
    <script src="/Scripts/jqueryui/jquery-ui-1.8.19.custom.min.js" type="text/javascript"></script>
    <script src="/admin/js/jqueryui/JqueryDataPicker4CN.js" type="text/javascript"></script>
    <script type="text/javascript">
        $(function () {
            $.datepicker.setDefaults($.datepicker.regional['zh-CN']);
            $("[id$=txtStartDate]").prop("readonly", true).datepicker({
               
                changeMonth: true,
                dateFormat: "yy-mm-dd",
                onClose: function (selectedDate) {
                    $("[id$=txtEndDate]").datepicker("option", "minDate", selectedDate);
                }
            });
            $("[id$=txtEndDate]").prop("readonly", true).datepicker({
               
                changeMonth: true,
                dateFormat: "yy-mm-dd",
                onClose: function (selectedDate) {
                    $("[id$=txtStartDate]").datepicker("option", "maxDate", selectedDate);
                    $("[id$=txtEndDate]").val($(this).val());
                }
            });
        })
    </script>
    <script src="/FusionCharts/FusionCharts.js" type="text/javascript"></script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
<div class="newslistabout">
        <div class="newslist_title">
            <table width="100%" border="0" cellspacing="0" cellpadding="0" class="borderkuang">
                <tr>
                    <td bgcolor="#FFFFFF" class="newstitle">
                        <asp:Literal ID="LiteralRT" runat="server" Text="用户统计" />
                    </td>
                </tr>
                <tr>
                    <td bgcolor="#FFFFFF" class="newstitlebody">
                        <asp:Literal ID="LiteralIn" runat="server" Text="您可以查看用户的统计信息" />
                    </td>
                </tr>
            </table>
        </div>
        <table style="width: 100%;" cellpadding="2" cellspacing="1" class="border">
            <tr>
                <td class="tdbg">
                    <table cellspacing="0" cellpadding="0" width="100%" border="0">
                        <tr>
                            <td style="width: 610px;float: left;height: 38px;line-height: 38px;margin-left: 10px;">
                                开始时间：<asp:TextBox ID="txtStartDate" runat="server"></asp:TextBox>
                                &nbsp;&nbsp;结束时间：<asp:TextBox ID="txtEndDate" runat="server"></asp:TextBox>
                                <asp:RadioButtonList ID="rdoMode" runat="server" RepeatDirection="Horizontal" style="float: right;height: 38px;line-height: 38px;" >
                                    <asp:ListItem Text="天" Value="0" ></asp:ListItem>
                                    <asp:ListItem Text="月" Value="1" Selected="True"></asp:ListItem>
                                    <asp:ListItem Text="年" Value="2"></asp:ListItem>
                                </asp:RadioButtonList>
                            </td>
                            <td style="float: left;height: 38px;line-height: 38px;">
                                &emsp;&emsp;<asp:Button ID="btnReStatistic" runat="server" Text="统计" 
                                    class="adminsubmit" onclick="btnReStatistic_Click"/>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="2">
                                <asp:Literal ID="litChart" runat="server"></asp:Literal>
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
        </table>


             <br />
       <table border="0" cellpadding="0" cellspacing="1" width="100%" class="borderkuang" runat="server" id="tabGrid">
            <tr>
                <td width="800px">
                    <cc1:GridViewEx ID="gridView" runat="server" AllowPaging="True" AllowSorting="True"
                        ShowToolBar="false" AutoGenerateColumns="False" OnBind="BindData" OnPageIndexChanging="gridView_PageIndexChanging"
                        OnRowDataBound="gridView_RowDataBound" UnExportedColumnNames="Modify" Width="100%"
                        PageSize="31" ShowExportExcel="False" ShowExportWord="False" ExcelFileName="FileName1"
                        CellPadding="3" BorderWidth="1px" ShowCheckAll="False">
                        <Columns>
                            <asp:TemplateField HeaderText="时间" ItemStyle-HorizontalAlign="Left">
                                <ItemTemplate>
                                    <%#GetDate(Eval("GeneratedDate"))%></ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="注册人数" ItemStyle-HorizontalAlign="Right">
                                <ItemTemplate>
                                    <%#String.IsNullOrWhiteSpace(Eval("Users").ToString())? "0" : Eval("Users")%>
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
                </td>
                <td>
                </td>
            </tr>
        </table>
    </div>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceCheckright" runat="server">
</asp:Content>
