<%@ Page Title="商品销量排行" Language="C#" MasterPageFile="~/Admin/Basic.Master" AutoEventWireup="true"
    CodeBehind="ProductSales.aspx.cs" Inherits="YSWL.MALL.Web.Admin.Statistics.ProductSales" %>

<%@ Register Assembly="YSWL.MALL.Web" Namespace="YSWL.MALL.Web.Controls" TagPrefix="cc1" %>
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
            var datavalue = $("[id$='hfAmount']").val().split(',');
            for (var i = 0; i < datavalue.length; i++) {
                var item = parseInt(datavalue[i]);
                dayCount.push(item);
            }

            $('#productCharts').highcharts({
                chart: {
                    type: 'bar'
                },
                title: {
                    text: '商品销量'
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
                        return '<b>' + this.x + '</b><br/>' + this.series.name + '： ' + this.y + '件';
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
            $("#productCharts text:last").hide();
            $("#productCharts span:last").hide();

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
                        <asp:Literal ID="litTitle" runat="server" Text="商品销量排行" />
                    </td>
                </tr>
                <tr>
                    <td bgcolor="#FFFFFF" class="newstitlebody">
                        <asp:Literal ID="litDec" runat="server" Text="您可以查看商品销量排行走势图信息" />
                    </td>
                </tr>
            </table>
            <asp:HiddenField ID="hfCategory" runat="server" />
            <asp:HiddenField ID="hfAmount" runat="server" />
        </div>
        <table width="100%" border="0" cellspacing="0" cellpadding="0" class="borderkuang-noc">
            <tr>
                <td width="450px">
                    时间：
                    <asp:TextBox ID="txtFrom" runat="server" Width="120"></asp:TextBox>
                    --
                    <asp:TextBox ID="txtTo" runat="server" Width="120"></asp:TextBox>
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
                    <div id="productCharts" style="width:100%;min-height:600px;">
                    </div>
                </td>
            </tr>
        </table>
        <br />
        <table border="0" cellpadding="0" cellspacing="1" width="100%" class="borderkuang-noc"
            runat="server" id="Table3">
            <tr>
                <td>
<cc1:GridViewEx ID="gridView" runat="server" AllowPaging="True" AllowSorting="True"
            OnPageIndexChanging="gridView_PageIndexChanging" ShowToolBar="True" AutoGenerateColumns="False"
            OnBind="BindData" OnRowDataBound="gridView_RowDataBound" UnExportedColumnNames="Modify"
            PageSize="100" ShowExportExcel="False" ShowExportWord="False" ExcelFileName="FileName1"
            CellPadding="3" BorderWidth="1px" ShowGridLine="true" ShowHeaderStyle="true">
            <Columns>
                <asp:TemplateField HeaderText="商品名称" ItemStyle-HorizontalAlign="Left">
                    <ItemTemplate>
                       <div class="tx-l"><%#Eval("ProductName")%></div>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="销售量(份)" ItemStyle-HorizontalAlign="Center">
                    <ItemTemplate>
                        <%#Eval("ToalQuantity")%>
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
