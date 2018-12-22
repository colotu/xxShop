<%@ Page Title="佣金统计" Language="C#" MasterPageFile="~/Admin/Basic.Master" AutoEventWireup="true"
   CodeBehind="CommissionStat.aspx.cs" Inherits="YSWL.MALL.Web.Admin.Shop.Commission.CommissionStat" %>

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
            var hfCategoryVal = $("[id$='hfCategory']").val();
            if (hfCategoryVal.length <= 0) {
                return;
            }
            hfCategoryVal = hfCategoryVal.replace(/\d{4}-/g, '');
            var categories = hfCategoryVal.split(',');

            var dayCount = [];

            var datavalue = $("[id$='hfDayCount']").val().split(',');
            for (var i = 0; i < datavalue.length; i++) {
                var item = parseFloat(datavalue[i]);
                dayCount.push(item);
            }
            $('#orderCount').highcharts({
                chart: {
                    type: 'line'
                },
                title: {
                    text: '佣金统计'
                },
                subtitle: {
                    text: '佣金统计'
                },
                xAxis: {
                    categories: categories
                },
                yAxis: {
                    title: {
                        text: '佣金'
                    }
                },
                tooltip: {
                    formatter: function () {
                        return '<b>' + this.x + '：</b><br/> ' + this.y + '元';
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
                    name: '佣金',
                    data: dayCount
                }]
            });

            $("#priceCount text:last").hide();
            $("#priceCount span:last").hide();

            //if ($('#priceCount .highcharts-tracker rect').length > 0) {
            //    if (parseInt($('#priceCount .highcharts-tracker rect').eq(0).attr('width')) > 25) {
            //        $('#priceCount .highcharts-tracker rect').css('width', '25px');
            //    }
            //}

            //控制线条宽度   最高为25  
            var rec_s = $('#priceCount .highcharts-tracker rect');
            if (rec_s.length > 0) {
                var rect_width = parseInt(rec_s.eq(0).attr('width'));
                var x_ = (rect_width - 25) / 2;//计算新的X轴位置
                if (rect_width > 25) {
                    for (var i = 0; i < rec_s.length; i++) {
                        rec_s.eq(i).css('width', '25px').attr('x', parseFloat(rec_s.eq(i).attr('x')) + x_);
                    }
                }
            }

        });
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="newslistabout">
        <div class="newslist_title">
            <table width="100%" border="0" cellspacing="0" cellpadding="0" class="borderkuang">
                <tr>
                    <td bgcolor="#FFFFFF" class="newstitle">
                        <asp:Literal ID="Literal1" runat="server" Text="每日订单统计" />
                    </td>
                </tr>
                <tr>
                    <td bgcolor="#FFFFFF" class="newstitlebody">
                        <asp:Literal ID="Literal2" runat="server" Text="您可以查看每日订单统计信息" />
                    </td>
                </tr>
            </table>
            <asp:HiddenField ID="hfCategory" runat="server" />
            <asp:HiddenField ID="hfDayCount" runat="server" />
        </div>
        <table width="100%" border="0" cellspacing="0" cellpadding="0" class="borderkuang">
            <tr>
                <td width="280px">
                    时间：
                    <asp:TextBox ID="txtFrom" runat="server" Width="90"></asp:TextBox>
                    --
                    <asp:TextBox ID="txtTo" runat="server" Width="90"></asp:TextBox>
                    </td>
                    <td width="200px">
                    <asp:RadioButtonList ID="rdoMode" runat="server" RepeatDirection="Horizontal" style="float: right;height: 28px;line-height: 28px;" >
                                    <asp:ListItem Text="日" Value="0" Selected="True" ></asp:ListItem>
                                    <asp:ListItem Text="月" Value="1"></asp:ListItem>
                    </asp:RadioButtonList>
                </td>
                <td width="100px">
                <asp:Button ID="btnReStatistic" runat="server" Text="统计" class="adminsubmit" style="float:right;" OnClick="btnReStatistic_Click" /></td>
                <td>
                    
                </td>
            </tr>
        </table>
        <br />
        <table id="Table1" border="0" cellpadding="0" cellspacing="1" width="100%" class="borderkuang tabChart"
            runat="server">
            <tr>
                <td>
                    <div id="orderCount" style="width:100%;height:400px;">
                    </div>
                </td>
            </tr>
        </table>
        <br />
        
        <table border="0" cellpadding="0" cellspacing="1" width="100%" class="borderkuang"
            runat="server" id="tabGrid">
            <tr>
                <td width="800px">
                    <cc1:GridViewEx ID="gridView" runat="server" AllowPaging="True" AllowSorting="True"
                        ShowToolBar="true" AutoGenerateColumns="False" OnBind="BindData" OnPageIndexChanging="gridView_PageIndexChanging"
                        OnRowDataBound="gridView_RowDataBound" UnExportedColumnNames="Modify" Width="100%"
                        PageSize="31" ShowExportExcel="true" ShowExportWord="false" ExcelFileName="FileName1"
                        CellPadding="3" BorderWidth="1px" SortExpressionStr="TradeDate" ShowCheckAll="False">
                        <Columns>
                            <asp:TemplateField HeaderText="时间" ItemStyle-HorizontalAlign="Left">
                                <ItemTemplate>
                                    <%#Eval("TradeDate")%>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="佣金 " ItemStyle-HorizontalAlign="Left">  
                                <ItemTemplate>
                                       <%#Eval("TotalFee", "￥{0:N2}")%>
                                    </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="订单数" ItemStyle-HorizontalAlign="Left">
                                <ItemTemplate>
                                    <%#Eval("OrderCount")%>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="用户数" ItemStyle-HorizontalAlign="Left">
                                <ItemTemplate>
                                    <%#Eval("UserCount")%>
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

