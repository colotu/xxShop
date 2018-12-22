<%@ Page Title="用户注册统计" Language="C#" MasterPageFile="~/Admin/Basic.Master" AutoEventWireup="true"
    CodeBehind="UserRegCount.aspx.cs" Inherits="YSWL.MALL.Web.Admin.Statistics.UserRegCount" %>

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
            hfCategoryVal = hfCategoryVal.replace(/\d{4}\//g, '');
            var categories = hfCategoryVal.split(',');

            var dayCount = [];
            var totalCount = [];

            var datavalue = $("[id$='hfDayCount']").val().split(',');
            for (var i = 0; i < datavalue.length; i++) {
                var item = parseInt(datavalue[i]);
                dayCount.push(item);
            }

            var totalData = $("[id$='hfTotalCount']").val().split(',');
            for (var i = 0; i < totalData.length; i++) {
                var item = parseInt(totalData[i]);
                totalCount.push(item);
            }

            $('#dayCount').highcharts({
                chart: {
                    type: 'line'
                },
                title: {
                    text: '用户注册统计'
                },
                subtitle: {
                    text: '新增注册人数'
                },
                xAxis: {
                    categories: categories
                },
                yAxis: {
                    title: {
                        text: '人数'
                    }
                },
                tooltip: {
                    formatter: function () {
                        return this.x+'：' + this.y + '人';
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
                    name: '新增注册人数',
                    data: dayCount
                }]
            });
            $("#dayCount text:last").hide();
            $("#dayCount span:last").hide();

            $('#totalCount').highcharts({
                chart: {
                    type: 'line'
                },
                title: {
                    text: '用户注册统计'
                },
                subtitle: {
                    text: '累积注册人数'
                },
                xAxis: {
                    categories: categories
                },
                yAxis: {
                    title: {
                        text: '人数'
                    }
                },
                tooltip: {
                    formatter: function () {
                        return  this.x+'：' + this.y + '人';  
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
                    name: '累积注册人数',
                    data: totalCount
                }]
            });

            $("#totalCount text:last").hide();
            $("#totalCount span:last").hide();
        });
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="newslistabout">
        <div class="newslist_title">
            <table width="100%" border="0" cellspacing="0" cellpadding="0" class="borderkuang">
                <tr>
                    <td bgcolor="#FFFFFF" class="newstitle">
                        <asp:Literal ID="Literal1" runat="server" Text="用户注册统计" />
                    </td>
                </tr>
                <tr>
                    <td bgcolor="#FFFFFF" class="newstitlebody">
                        <asp:Literal ID="Literal2" runat="server" Text="您可以查看用户注册统计信息" />
                    </td>
                </tr>
            </table>
            <asp:HiddenField ID="hfCategory" runat="server" />
            <asp:HiddenField ID="hfDayCount" runat="server" />
            <asp:HiddenField ID="hfTotalCount" runat="server" />
        </div>
        <table width="100%" border="0" cellspacing="0" cellpadding="0">
            <tr>
                <td width="450px">
                    注册时间：
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
        <table width="100%" border="0" cellspacing="0" cellpadding="0"
            height="50px">
            <tr>
                <td style="width: 150px;">
                    今日新增注册人数：
                    <asp:Literal ID="lblDayCount" runat="server" Text="0"></asp:Literal>
                </td>
                <td>
                    累计注册人数：
                    <asp:Literal ID="lblTotalCount" runat="server" Text="0"></asp:Literal>
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
        <table id="Table2" border="0" cellpadding="0" cellspacing="1" width="100%"
            runat="server">
            <tr>
                <td>
                    <div id="totalCount" style="width:100%;height:400px;">
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
                        PageSize="100" ShowExportExcel="true" ShowExportWord="false" ExcelFileName="FileName1"
                        CellPadding="3" BorderWidth="1px" SortExpressionStr="D" ShowCheckAll="False">
                        <Columns>
                            <asp:TemplateField HeaderText="时间" ItemStyle-HorizontalAlign="Left">
                                <ItemTemplate>
                                    <%#Eval("D")%>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="新增注册人数" ItemStyle-HorizontalAlign="Left">
                                <ItemTemplate>
                                    <%#Eval("UserCount")%>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="累积注册人数 " ItemStyle-HorizontalAlign="Left">
                                <ItemTemplate>
                                    <%#Eval("TotalUserCount")%></ItemTemplate>
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
