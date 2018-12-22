<%@ Page Title="订购门店数统计" Language="C#" MasterPageFile="~/Admin/Basic.Master" AutoEventWireup="true" 
CodeBehind="FreCount.aspx.cs" Inherits="YSWL.MALL.Web.Admin.Statistics.FreCount" %>

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

            //订购门店数统计--频次
            var freCount = [];
            var categories = $("[id$='hfCategory']").val().replace(/\d{4}\//g, '').split(',');
            var datavalue = $("[id$='hfFreData']").val().split(',');
            for (var i = 0; i < datavalue.length; i++) {
                var item = parseFloat(datavalue[i]);
                freCount.push(item);
            }

            $('#freChart').highcharts({
                chart: {
                    type: 'line'
                },
                title: {
                    text: '频次统计'
                },
                //                subtitle: {
                //                    text: '客户活跃统计'
                //                },
                xAxis: {
                    categories: categories
                },
                yAxis: {
                    title: {
                        text: '频次'
                    }
                },
                tooltip: {
                    formatter: function () {
                        return '<b>' + this.series.name + '</b><br/>  ' + this.y + '次';
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
                    name: '频次统计',
                    data: freCount
                }]
            });
            $("#freChart text:last").hide();
            $("#freChart span:last").hide();

        });
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="newslistabout">
        <div class="newslist_title">
            <table width="100%" border="0" cellspacing="0" cellpadding="0" class="borderkuang">
                <tr>
                    <td bgcolor="#FFFFFF" class="newstitle">
                        <asp:Literal ID="Literal1" runat="server" Text="用户购买频次统计" />
                    </td>
                </tr>
                <tr>
                    <td bgcolor="#FFFFFF" class="newstitlebody">
                        <asp:Literal ID="Literal2" runat="server" Text="您可以查看下单的用户购买频次统计信息" />
                    </td>
                </tr>
            </table>
            <asp:HiddenField ID="hfCategory" runat="server" />
            <asp:HiddenField ID="hfFreData" runat="server" />
        </div>
        <table width="100%" border="0" cellspacing="0" cellpadding="0" class="borderkuang">
            <tr>
                <td width="360px">
                    时间：
                    <asp:TextBox ID="txtFrom" runat="server" Width="90"></asp:TextBox>
                    --
                    <asp:TextBox ID="txtTo" runat="server" Width="90"></asp:TextBox>
               
                </td>
                      
                          <td width="100px">
               <asp:Button ID="btnReStatistic" runat="server" Text="统计" class="adminsubmit" OnClick="btnReStatistic_Click" />
                </td>
                   <td>
                    
                </td>
            </tr>
        </table>
        
        <br />
        <table id="Table2" border="0" cellpadding="0" cellspacing="1" width="100%" class="borderkuang tabChart"
            runat="server">
            <tr>
                <td>
                    <div id="freChart" style="width:100%;height:400px;">
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
                        ShowToolBar="true" AutoGenerateColumns="False" OnBind="BindData" OnPageIndexChanging="gridView_PageIndexChanging"
                        UnExportedColumnNames="Modify" Width="100%" PageSize="100" ShowExportExcel="true"
                        ShowExportWord="false" ExcelFileName="FileName1" CellPadding="3" BorderWidth="1px" SortExpressionStr="DateFormat"
                        ShowCheckAll="False">
                        <Columns>
                            <asp:TemplateField HeaderText="时间" ItemStyle-HorizontalAlign="Left">
                                <ItemTemplate>
                                    <%#Eval("DateFormat")%></ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="频次" ItemStyle-HorizontalAlign="Left">
                                <ItemTemplate>
                                    <%#Eval("FreCount")%></ItemTemplate>
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