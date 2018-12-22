<%@ Page Language="C#" MasterPageFile="~/Admin/Basic.Master" AutoEventWireup="true"
    CodeBehind="CommissionRule.aspx.cs" Inherits="YSWL.MALL.Web.Admin.Statistics.CommissionRule" Title="佣金规则统计" %>
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
            $('[id$="txtFrom"]').prop("readonly", true).datepicker({
                changeMonth: true,
                dateFormat: "yy-mm-dd",
                onClose: function (selectedDate) {
                    $('[id$="txtTo"]').datepicker("option", "minDate", selectedDate);
                }
            });
            
            $('[id$="txtTo"]').prop("readonly", true).datepicker({
                changeMonth: true,
                dateFormat: "yy-mm-dd",
                onClose: function (selectedDate) { 
                    $('[id$="txtFrom"]').datepicker("option", "maxDate", selectedDate);
                    $('[id$="txtTo"]').val($(this).val());
                }
            });

            //佣金统计
            var categories = $("[id$='hfCategory']").val().split(',');
            var totalfee = [];
            var totalProdcut = [];
            var feevalue = $("[id$='hfTotalFee']").val().split(',');  
            var prodcutval = $("[id$='hfTotalProdcut']").val().split(',');
            
            for (var i = 0; i < feevalue.length; i++) {
                var item = parseFloat(feevalue[i]);
                totalfee.push(item);
            }
            for (var i = 0; i < prodcutval.length; i++) {
                var item = parseFloat(prodcutval[i]);
                totalProdcut.push(item);
            }
            $('#feeInfo').highcharts({
                chart: {
                    type: 'bar'
                },
                title: {
                    text: '佣金统计'
                },
            
                xAxis: {
                    categories: categories
                },
                yAxis: {
                    title: {
                        text: '金额'
                    }
                },
                tooltip: {
                    formatter: function () {
                        return '<b>' + this.x + '</b><br/>' + this.series.name + '： ' + this.y + '元';
                    }
                },
                legend: {
                    reversed: true
                },
                plotOptions: {
                    series: {
                        stacking: 'normal'
                    }
                },
                series: [{
                    name: '佣金',
                    data: totalfee
                }]
            });

            $('#prodInfo').highcharts({
                chart: {
                    type: 'bar'
                },
                title: {
                    text: '佣金统计'
                },

                xAxis: {
                    categories: categories
                },
                yAxis: {
                    title: {
                        text: '商品数'
                    }
                },
                tooltip: {
                    formatter: function () {
                        return '<b>' + this.x + '</b><br/>' + this.series.name + '： ' + this.y + '个';
                    }
                },
                legend: {
                    reversed: true
                },
                plotOptions: {
                    series: {
                        stacking: 'normal'
                    }
                },
                series: [{
                    name: '商品数',
                    data: totalProdcut
                }]
            });
         
            $("#feeInfo text:last").hide();
            $("#feeInfo span:last").hide();

            $("#prodInfo text:last").hide();
            $("#prodInfo span:last").hide();


            ////控制线条宽度   最高为25
            //if ($('#proInfo .highcharts-tracker rect').length > 0) {
            //    if (parseInt($('#proInfo .highcharts-tracker rect').eq(0).attr('width')) > 25) {
            //        $('#proInfo .highcharts-tracker rect').css('width', '25px');
            //    }
            //}

            ////控制线条宽度   最高为25
            //if ($('#feeInfo .highcharts-tracker rect').length > 0) {
            //    if (parseInt($('#feeInfo .highcharts-tracker rect').eq(0).attr('width')) > 25) {
            //        $('#feeInfo .highcharts-tracker rect').css('width', '25px');
            //    }
            //}

            //控制线条宽度   最高为25  
            var rec_s = $('#prodInfo .highcharts-tracker rect');
            if (rec_s.length > 0) {
                var rect_width = parseInt(rec_s.eq(0).attr('width'));
                var x_ = (rect_width - 25) / 2;//计算新的X轴位置
                if (rect_width > 25) {
                    for (var i = 0; i < rec_s.length; i++) {
                        rec_s.eq(i).css('width', '25px').attr('x', parseFloat(rec_s.eq(i).attr('x')) + x_);
                    }
                }
            }


            //控制线条宽度   最高为25  
            var f_rec_s = $('#feeInfo .highcharts-tracker rect');
            if (f_rec_s.length > 0) {
                var f_rect_width = parseInt(f_rec_s.eq(0).attr('width'));
                var x_ = (f_rect_width - 25) / 2;//计算新的X轴位置
                if (f_rect_width > 25) {
                    for (var i = 0; i < f_rec_s.length; i++) {
                        f_rec_s.eq(i).css('width', '25px').attr('x', parseFloat(f_rec_s.eq(i).attr('x')) + x_);
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
                        <asp:Literal ID="Literal2" runat="server" Text="佣金规则推广数据统计" />
                    </td>
                </tr>
                <tr>
                    <td bgcolor="#FFFFFF" class="newstitlebody">
                        <asp:Literal ID="Literal3" runat="server" Text="您可以查看推广佣金数据统计信息" />
                    </td>
                </tr>
            </table>
            <asp:HiddenField ID="hfCategory" runat="server" />
            <asp:HiddenField ID="hfTotalFee" runat="server" />
            <asp:HiddenField ID="hfTotalProdcut" runat="server" />
        </div>
           <table width="100%" border="0" cellspacing="0" cellpadding="0" class="borderkuang">
            <tr>
                <td width="430px">
                    时间：
                    <asp:TextBox ID="txtFrom" runat="server" Width="90"></asp:TextBox>
                    --
                    <asp:TextBox ID="txtTo" runat="server" Width="90"></asp:TextBox>
                    &#12288;
                    前 <asp:DropDownList ID="dropTop" runat="server">
                        <asp:ListItem Selected="True" Value="10"></asp:ListItem>
                        <asp:ListItem>20</asp:ListItem>
                    </asp:DropDownList>
                    条
                    </td>
                <td width="100px">
                <asp:Button ID="btnReStatistic" runat="server" Text="统计" class="adminsubmit" style="float:right;" OnClick="btnReStatistic_Click" /></td>
                <td>
                    
                </td>
            </tr>
        </table>
        <br />
        <table id="Table2" border="0" cellpadding="0" cellspacing="1" width="100%" class="borderkuang tabChart"
            runat="server">
            <tr>
                <td>
                    <div id="feeInfo" style="width:100%;min-height:400px;">
                    </div><br />
                    <div id="prodInfo" style="width:100%;min-height:400px;">
                    </div>
                </td>
            </tr>
        </table>
        <br />
        <table border="0" cellpadding="0" cellspacing="1" width="100%" class="borderkuang"
            runat="server" id="Table3">
            <tr>
                <td width="800px">
        <cc1:GridViewEx ID="gridView" runat="server" AllowPaging="false" AllowSorting="True"
            ShowToolBar="True" AutoGenerateColumns="False" OnBind="BindData"
            UnExportedColumnNames="Modify" Width="900" PageSize="20" ShowExportExcel="True"
            ShowExportWord="False" ExcelFileName="FileName1" CellPadding="3" BorderWidth="1px"
            ShowGridLine="true" ShowHeaderStyle="true">
            <Columns>
                <asp:TemplateField HeaderText="规则名称" ItemStyle-HorizontalAlign="Left">
                    <ItemTemplate>
                        <%#Eval("RuleName")%>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="佣金总额" ItemStyle-HorizontalAlign="Center">
                    <ItemTemplate>
                        <%#Eval("TotalFee", "￥{0:N2}")%>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="商品数" ItemStyle-HorizontalAlign="Center">
                    <ItemTemplate>
                        <%#Eval("TotalProduct")%>
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

