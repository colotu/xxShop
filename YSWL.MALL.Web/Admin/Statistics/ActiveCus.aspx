<%@ Page Title="订购门店数统计" Language="C#" MasterPageFile="~/Admin/Basic.Master" AutoEventWireup="true"
    CodeBehind="ActiveCus.aspx.cs" Inherits="YSWL.MALL.Web.Admin.Statistics.ActiveCus" %>

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

            //订购门店数统计--活跃数
            var categories = $("[id$='hfCategory']").val().replace(/\d{4}\//g, '').split(',');
            var dayCount = [];

            var datavalue = $("[id$='hfDayCount']").val().split(',');
            for (var i = 0; i < datavalue.length; i++) {
                var item = parseInt(datavalue[i]);
                dayCount.push(item);
            }

            $('#dayCount').highcharts({
                chart: {
                    type: 'line'
                },
                title: {
                    text: '活跃数统计'
                },
                //                subtitle: {
                //                    text: '客户活跃统计'
                //                },
                xAxis: {
                    categories: categories
                },
                yAxis: {
                    title: {
                        text: '活跃数'
                    }
                },
                tooltip: {
                    formatter: function () {
                        return '<b>' + this.x + '：</b><br/>  ' + this.y + '人';
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
                    name: '活跃数统计',
                    data: dayCount
                }]
            });
            $("#dayCount text:last").hide();
            $("#dayCount span:last").hide();
          
        });
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="newslistabout">
        <div class="newslist_title">
            <table width="100%" border="0" cellspacing="0" cellpadding="0" class="borderkuang">
                <tr>
                    <td bgcolor="#FFFFFF" class="newstitle">
                        <asp:Literal ID="Literal1" runat="server" Text="用户活跃数统计" />
                    </td>
                </tr>
                <tr>
                    <td bgcolor="#FFFFFF" class="newstitlebody">
                        <asp:Literal ID="Literal2" runat="server" Text="您可以查看下单的用户活跃数信息" />
                    </td>
                </tr>
            </table>
            <asp:HiddenField ID="hfCategory" runat="server" />
            <asp:HiddenField ID="hfDayCount" runat="server" />
            <asp:HiddenField ID="hfFreData" runat="server" />
        </div>
        <table width="100%" border="0" cellspacing="0" cellpadding="0">
            <tr>
                <td width="450px">
                    时间：
                    <asp:TextBox ID="txtFrom" runat="server" Width="120"></asp:TextBox>
                    --
                    <asp:TextBox ID="txtTo" runat="server" Width="120"></asp:TextBox>
               
                </td>
                       <td width="200px">
                <asp:RadioButtonList ID="rdoMode" runat="server" RepeatDirection="Horizontal" style="height: 28px;line-height: 28px;" >
                                    <asp:ListItem Text="日" Value="0" Selected="True" ></asp:ListItem>
                                    <asp:ListItem Text="月" Value="1"></asp:ListItem>
                    </asp:RadioButtonList>
                    </td>
                          <td width="100px">
               <asp:Button ID="btnReStatistic" runat="server" Text="统计" class="add-btn mar-le" OnClick="btnReStatistic_Click" />
                </td>
                   <td>
                    
                </td>
            </tr>
        </table>
        <br />
        <table id="Table1" border="0" cellpadding="0" cellspacing="1" width="100%" class="tabChart"
            runat="server">
            <tr>
                <td>
                    <div id="dayCount" style="width:100%;height:400px;">
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
                        ShowToolBar="true" AutoGenerateColumns="False" OnBind="BindData" OnPageIndexChanging="gridView_PageIndexChanging"
                        UnExportedColumnNames="Modify" Width="100%" PageSize="100" ShowExportExcel="true"
                        ShowExportWord="false" ExcelFileName="FileName1" CellPadding="3" BorderWidth="1px" SortExpressionStr="D"
                        ShowCheckAll="False">
                        <Columns>
                            <asp:TemplateField HeaderText="时间" ItemStyle-HorizontalAlign="Left">
                                <ItemTemplate>
                                    <%#Eval("D")%></ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="活跃数" ItemStyle-HorizontalAlign="Left">
                                <ItemTemplate>
                                    <%#Eval("BuyerCount")%></ItemTemplate>
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
