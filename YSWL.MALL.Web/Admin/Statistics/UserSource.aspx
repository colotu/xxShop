<%@ Page Title="用户来源统计" Language="C#" MasterPageFile="~/Admin/Basic.Master" AutoEventWireup="true"
    CodeBehind="UserSource.aspx.cs" Inherits="YSWL.MALL.Web.Admin.Statistics.UserSource" %>

<%@ Register Assembly="YSWL.MALL.Web" Namespace="YSWL.MALL.Web.Controls" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link href="/Scripts/jqueryui/jquery-ui-1.8.19.custom.css" rel="stylesheet" type="text/css" />
    <script src="/Scripts/jqueryui/jquery-ui-1.8.19.custom.min.js" type="text/javascript"></script>
    <script src="/admin/js/jqueryui/JqueryDataPicker4CN.js" type="text/javascript"></script>
    <script src="/Scripts/Highcharts/highcharts.js" type="text/javascript"></script>
    <%--<script src="/Scripts/Highcharts/themes/gray.js" type="text/javascript"></script>--%>
    <script type="text/javascript">
        $(function () {
            $.datepicker.setDefaults($.datepicker.regional['zh-CN']);
            $("[id$=txtStartDate]").prop("readonly", true).datepicker({
                //defaultDate: "+1w",
                changeMonth: true,
                dateFormat: "yy-mm-dd",
                onClose: function (selectedDate) {
                    $("[id$=txtEndDate]").datepicker("option", "minDate", selectedDate);
                }
            });
            $("[id$=txtEndDate]").prop("readonly", true).datepicker({
                //defaultDate: "+1w",
                changeMonth: true,
                dateFormat: "yy-mm-dd",
                onClose: function (selectedDate) {
                    $("[id$=txtStartDate]").datepicker("option", "maxDate", selectedDate);
                    $("[id$=txtEndDate]").val($(this).val());
                }
            });

            var data = $("[id$=hfData]").val();
            if (data == "") {
                return;
            }
            var value = $.parseJSON(data);

            $('#tdSourceType').highcharts({
                chart: {
                    plotBackgroundColor: null,
                    plotBorderWidth: null,
                    plotShadow: false
                },
                title: {
                    text: '用户注册来源统计'
                },
                tooltip: {
                    pointFormat: '<b>{series.name}:{point.percentage:.1f}%  数值为:{point.y}</b>'
                },
                plotOptions: {
                    pie: {
                        allowPointSelect: true,
                        cursor: 'pointer',
                        dataLabels: {
                            enabled: true,
                            color: '#000000',
                            connectorColor: '#000000',
                            format: '<b>{point.name}</b>: {point.percentage:.1f} %'
                        }
                    }
                },
                series: [{
                    type: 'pie',
                    name: '所占比例',
                    data: value
                }]
            });
            $("#tdSourceType text:last").hide();
            $("#tdSourceType span:last").hide();
        })
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:HiddenField ID="hfData" runat="server" />
    <div class="newslistabout">
        <div class="newslist_title">
            <table width="100%" border="0" cellspacing="0" cellpadding="0" class="borderkuang">
                <tr>
                    <td bgcolor="#FFFFFF" class="newstitle">
                        <asp:Literal ID="LiteralRT" runat="server" Text="用户来源统计" />
                    </td>
                </tr>
                <tr>
                    <td bgcolor="#FFFFFF" class="newstitlebody">
                        <asp:Literal ID="LiteralIn" runat="server" Text="您可以查看用户注册来源的统计信息" />
                    </td>
                </tr>
            </table>
        </div>
        <table width="100%" border="0" cellspacing="0" cellpadding="0" class="borderkuang">
            <tr>
                <td width="280px">
                    时间：
                    <asp:TextBox ID="txtStartDate" runat="server" Width="90"></asp:TextBox>
                    --
                    <asp:TextBox ID="txtEndDate" runat="server" Width="90"></asp:TextBox>
                     <asp:Button ID="btnReStatistic" runat="server" Text="统计" class="adminsubmit"
                                    OnClick="btnReStatistic_Click" />
                </td>
            </tr>
        </table>
        <br />
        <div id="tdSourceType" style="width:100%;height:400px;">
        </div>
        <br />
        <table border="0" cellpadding="0" cellspacing="1" width="100%" class="borderkuang"
            runat="server" id="tabGrid">
            <tr>
                <td width="800px">
                    <cc1:GridViewEx ID="gridView" runat="server" AllowPaging="True" AllowSorting="True"
                        ShowToolBar="false" AutoGenerateColumns="False" OnBind="BindData" OnPageIndexChanging="gridView_PageIndexChanging"
                        OnRowDataBound="gridView_RowDataBound" UnExportedColumnNames="Modify" Width="100%"
                        PageSize="31" ShowExportExcel="False" ShowExportWord="False" ExcelFileName="FileName1"
                        CellPadding="3" BorderWidth="1px" ShowCheckAll="False">
                        <Columns>
                            <asp:TemplateField HeaderText="来源类型" ItemStyle-HorizontalAlign="Left">
                                <ItemTemplate>
                                    <%#GetSourceType(Eval("SourceType"))%></ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="注册人数" ItemStyle-HorizontalAlign="Center">
                                <ItemTemplate>
                                    <%#Eval("Count")%>
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
