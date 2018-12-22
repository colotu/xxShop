<%@ Page Title="" Language="C#" MasterPageFile="~/Admin/Basic.Master" AutoEventWireup="true"
    CodeBehind="ShopSales.aspx.cs" Inherits="YSWL.MALL.Web.Admin.Statistics.ShopSales" %>

<%@ Register Assembly="YSWL.MALL.Web" Namespace="YSWL.MALL.Web.Controls" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link href="/Scripts/jqueryui/jquery-ui-1.8.19.custom.css" rel="stylesheet" type="text/css" />
    <script src="/Scripts/jqueryui/jquery-ui-1.8.19.custom.min.js" type="text/javascript"></script>
    <script src="/admin/js/jqueryui/JqueryDataPicker4CN.js" type="text/javascript"></script>
    <script src="/Scripts/Highcharts/highcharts.js" type="text/javascript"></script>
    <script src="/Scripts/Highcharts/themes/gray.js" type="text/javascript"></script>
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
            //店铺排行统计
            var categories = $("[id$='hfCategory']").val().split(',');
            var shopAmount = [];
            var datavalue = $("[id$='hfAmount']").val().split(',');
            for (var i = 0; i < datavalue.length; i++) {
                var item = parseFloat(datavalue[i]);
                shopAmount.push(item);
            }

            $('#shopSaleInfo').highcharts({
                chart: {
                    type: 'bar'
                },
                title: {
                    text: '加盟商配送额排行统计'
                },
                //                subtitle: {
                //                    text: '店铺排行统计'
                //                },
                xAxis: {
                    categories: categories
                },
                yAxis: {
                    title: {
                        text: '配送额'
                    }
                },
                tooltip: {
                    formatter: function () {
                        return '<b>' + this.x + '</b><br/>' + this.series.name + '： ' + this.y + '元';
                    }
                },
                plotOptions: {
                    line: {
                        dataLabels: {
                            enabled: true
                        },
                        enableMouseTracking: true
                    }
                },
                series: [{
                    name: '配送额',
                    data: shopAmount
                }]
            });
            $("#shopSaleInfo text:last").hide();
            $("#shopSaleInfo span:last").hide();
        });
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="newslistabout">
        <div class="newslist_title">
            <table width="100%" border="0" cellspacing="0" cellpadding="0" class="borderkuang">
                <tr>
                    <td bgcolor="#FFFFFF" class="newstitle">
                        <asp:Literal ID="LiteralRT" runat="server" Text="加盟商配送额排行统计" />
                    </td>
                </tr>
                <tr>
                    <td bgcolor="#FFFFFF" class="newstitlebody">
                        <asp:Literal ID="LiteralIn" runat="server" Text="您可以查看加盟商配送额排行的统计信息" />
                    </td>
                </tr>
            </table>
            <asp:HiddenField ID="hfCategory" runat="server" />
            <asp:HiddenField ID="hfAmount" runat="server" />
        </div>
        <table width="100%" border="0" cellspacing="0" cellpadding="0" class="borderkuang">
            <tr>
                <td width="280px">
                    时间：
                    <asp:TextBox ID="txtFrom" runat="server" Width="90"></asp:TextBox>
                    --
                    <asp:TextBox ID="txtTo" runat="server" Width="90"></asp:TextBox>
                    </td>
                    <%--<td width="200px">
                    <asp:RadioButtonList ID="rdoMode" runat="server" RepeatDirection="Horizontal" style="float: right;height: 28px;line-height: 28px;" >
                                    <asp:ListItem Text="天" Value="0" Selected="True" ></asp:ListItem>
                                    <asp:ListItem Text="月" Value="1"></asp:ListItem>
                                    <asp:ListItem Text="年" Value="2"></asp:ListItem>
                    </asp:RadioButtonList>
                </td>--%>
                <td width="100px">
                <asp:Button ID="btnReStatistic" runat="server" Text="统计" class="adminsubmit" style="float:right;" OnClick="btnReStatistic_Click" /></td>
                <td></td>
            </tr>
        </table>
        <br />
        <table id="Table2" border="0" cellpadding="0" cellspacing="1" width="100%" class="borderkuang tabChart"
            runat="server">
            <tr>
                <td>
                    <div id="shopSaleInfo" style="width:100%;min-height:600px;">
                    </div>
                </td>
            </tr>
        </table>
        <br />
                <table border="0" cellpadding="0" cellspacing="1" width="100%" class="borderkuang"
            runat="server" id="Table3">
            <tr>
                <td width="800px">
        <cc1:GridViewEx ID="gridView" runat="server" AllowPaging="True" AllowSorting="True"
            OnPageIndexChanging="gridView_PageIndexChanging" ShowToolBar="True" AutoGenerateColumns="False"
            OnBind="BindData" OnRowDataBound="gridView_RowDataBound" UnExportedColumnNames="Modify"
            Width="900" PageSize="10" ShowExportExcel="False" ShowExportWord="False" ExcelFileName="FileName1"
            CellPadding="3" BorderWidth="1px" ShowGridLine="true" ShowHeaderStyle="true">
            <Columns>
                <asp:TemplateField HeaderText="加盟商" ItemStyle-HorizontalAlign="Left">
                    <ItemTemplate>
                        <%#Eval("Name")%>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="配送额" ItemStyle-HorizontalAlign="Center">
                    <ItemTemplate>
                        <%#Eval("Amount","{0:F2}") %>
                    </ItemTemplate>
                </asp:TemplateField>
            </Columns>
            <FooterStyle Height="25px" HorizontalAlign="Right" />
            <HeaderStyle Height="35px" />
            <PagerStyle Height="25px" HorizontalAlign="Right" />
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
