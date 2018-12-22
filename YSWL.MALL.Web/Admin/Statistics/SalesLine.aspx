<%@ Page Language="C#" MasterPageFile="~/Admin/Basic.Master" AutoEventWireup="true" CodeBehind="SalesLine.aspx.cs" Inherits="YSWL.MALL.Web.Admin.Statistics.SalesLine" Title="销量业绩统计" %>
<%@ Register TagPrefix="cc1" Namespace="YSWL.MALL.Web.Controls" Assembly="YSWL.MALL.Web" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link href="/Scripts/jqueryui/jquery-ui-1.8.19.custom.css" rel="stylesheet" type="text/css" />
    <script src="/Scripts/jqueryui/jquery-ui-1.8.19.custom.min.js" type="text/javascript"></script>
    <script src="/admin/js/jqueryui/JqueryDataPicker4CN.js" type="text/javascript"></script>
    <script src="/Scripts/Highcharts/highcharts.js" type="text/javascript"></script>
    <script src="/Scripts/Highcharts/themes/gray.js" type="text/javascript"></script>
    <script type="text/javascript">
        $(function () {
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

            var hfCategoryVal = $("[id$='hfCategory']").val();
            if (hfCategoryVal.length <= 0) {
                return;
            }
            hfCategoryVal = hfCategoryVal.replace(/\d{4}-/g, '') ;
            var categories = hfCategoryVal.split(',');

            var dayCount = [];
            var priceCount = [];

            var datavalue = $("[id$='hfDayCount']").val().split(',');
            for (var i = 0; i < datavalue.length; i++) {
                var item = parseInt(datavalue[i]);
                dayCount.push(item);
            }

            var priceData = $("[id$='hfPrice']").val().split(',');
            for (var i = 0; i < priceData.length; i++) {
                var item = parseFloat(priceData[i]);
                priceCount.push(item);
            }

            $('#orderCount').highcharts({
                chart: {
                    type: 'line'
                },
                title: {
                    text: '销量统计'
                },
                subtitle: {
                    text: '销量'
                },
                xAxis: {
                    categories: categories
                },
                yAxis: {
                    title: {
                        text: '销量'
                    }
                },
                tooltip: {
                    formatter: function () {
                        return '<b>' + this.x + '</b><br/>  ' + this.y + '件';
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
                    name: '销量',
                    data: dayCount
                }]
            });
            $("#orderCount text:last").hide();
            $("#orderCount span:last").hide();

            $('#priceCount').highcharts({
                chart: {
                    type: 'line'
                },
                title: {
                    text: '销售额统计'
                },
                subtitle: {
                    text: '销售额'
                },
                xAxis: {
                    categories: categories
                },
                yAxis: {
                    title: {
                        text: '销售额'
                    }
                },
                tooltip: {
                    formatter: function () {
                        return '<b>' + this.x + '</b><br/>  ' + this.y + '元';
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
                    name: '销售额',
                    data: priceCount
                }]
            });

            $("#priceCount text:last").hide();
            $("#priceCount span:last").hide();
        })
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
<div class="newslistabout">
        <div class="newslist_title">
            <table width="100%" border="0" cellspacing="0" cellpadding="0" class="borderkuang">
                <tr>
                    <td bgcolor="#FFFFFF" class="newstitle">
                        <asp:Literal ID="Literal1" runat="server" Text="销量销售额统计" />
                    </td>
                </tr>
                <tr>
                    <td bgcolor="#FFFFFF" class="newstitlebody">
                        <asp:Literal ID="Literal2" runat="server" Text="您可以查看已付款的商品销量和销售额统计" />
                    </td>
                </tr>
            </table>
            <asp:HiddenField ID="hfCategory" runat="server" />
            <asp:HiddenField ID="hfDayCount" runat="server" />
            <asp:HiddenField ID="hfPrice" runat="server" />
        </div>
        <table width="100%" border="0" cellspacing="0" cellpadding="0" class="borderkuang-noc">
            <tr>
                <td width="280px">
                    时间：
                    <asp:TextBox ID="txtFrom" runat="server" Width="90"></asp:TextBox>
                    --
                    <asp:TextBox ID="txtTo" runat="server" Width="90"></asp:TextBox>
                    </td>
                    <td width="200px">
                    <asp:RadioButtonList ID="rdoMode" runat="server" RepeatDirection="Horizontal" style="float: right;height: 28px;line-height: 28px;" >
                                    <asp:ListItem Text="天" Value="0" Selected="True" ></asp:ListItem>
                                    <asp:ListItem Text="月" Value="1"></asp:ListItem>
                                    <asp:ListItem Text="年" Value="2"></asp:ListItem>
                    </asp:RadioButtonList>
                </td>
                <td width="100px">
                <asp:Button ID="btnReStatistic" runat="server" Text="统计" class="add-btn mar-le" style="float:right;" OnClick="btnReStatistic_Click" /></td>
                <td></td>
            </tr>
        </table>
        <br />
        <table id="Table1" border="0" cellpadding="0" cellspacing="1" width="100%" class="tabChart"
            runat="server">
            <tr>
                <td>
                    <div id="orderCount" style="width:100%;height:400px;">
                    </div>
                </td>
            </tr>
        </table>
        <br />
        <table id="Table2" border="0" cellpadding="0" cellspacing="1" width="100%"
            runat="server">
            <tr>
                <td>
                    <div id="priceCount" style="width:100%;height:400px;">
                    </div>
                </td>
            </tr>
        </table>
         <br />
        <table border="0" cellpadding="0" cellspacing="1" width="100%" 
            runat="server" id="tabGrid">
            <tr>
                <td>
                    <cc1:GridViewEx ID="gridView" runat="server" AllowPaging="True" AllowSorting="True"
                        ShowToolBar="true" AutoGenerateColumns="False" OnBind="BindData" OnPageIndexChanging="gridView_PageIndexChanging"
                        OnRowDataBound="gridView_RowDataBound" UnExportedColumnNames="Modify" Width="100%"
                        PageSize="31" ShowExportExcel="true" ShowExportWord="false" ExcelFileName="FileName1"
                        CellPadding="3" BorderWidth="1px" SortExpressionStr="DateFormat" ShowCheckAll="False">
                        <Columns>
                            <asp:TemplateField HeaderText="时间" ItemStyle-HorizontalAlign="Left">
                                <ItemTemplate>
                                    <%#Eval("DateFormat")%>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="销量" ItemStyle-HorizontalAlign="Left">
                                <ItemTemplate>
                                    <%#Eval("ToalQuantity")%>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="业绩 " ItemStyle-HorizontalAlign="Right">
                                <ItemTemplate>
                                   <div class="tx-r"><%#Eval("ToalPrice") == null ? "0.00" : Convert.ToDecimal(Eval("ToalPrice").ToString()).ToString("F")%></div> 
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
