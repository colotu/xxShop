<%@ Page Language="C#" MasterPageFile="~/Admin/Basic.Master" AutoEventWireup="true"
    CodeBehind="DetailCount.aspx.cs" Inherits="YSWL.MALL.Web.Admin.WeChat.DetailCount" Title="微信推广场景统计" %>
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

            //推广场景统计
            var categories = $("[id$='hfCategory']").val().split(',');
            var proCount = [];
            var countValue = $("[id$='hfCount']").val().split(',');
            for (var i = 0; i < countValue.length; i++) {
                var item = parseFloat(countValue[i]);
                proCount.push(item);
            }

            $('#proInfo').highcharts({
                chart: {
                    type: 'bar'
                },
                title: {
                    text: '推广场景统计'
                },
            
                xAxis: {
                    categories: categories
                },
                yAxis: {
                    title: {
                        text: '数量'
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
                    name: '数量',
                    data: proCount
                }]
            });
            $("#proInfo text:last").hide();
            $("#proInfo span:last").hide();

            //控制线条宽度   最高为25  
            var rec_s= $('#proInfo .highcharts-tracker rect');
            if (rec_s.length > 0) {
                var rect_width = parseInt(rec_s.eq(0).attr('width'));
                var x_ = (rect_width - 25) / 2;//计算新的X轴位置
                if (rect_width > 25) {
                    for(var i=0;i< rec_s.length;i++)
                    {
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
                        <asp:Literal ID="Literal2" runat="server" Text="微信渠道推广统计管理" />
                    </td>
                </tr>
                <tr>
                    <td bgcolor="#FFFFFF" class="newstitlebody">
                        <asp:Literal ID="Literal3" runat="server" Text="您可以查看微信渠道推广统计信息" />
                    </td>
                </tr>
            </table>
            <asp:HiddenField ID="hfCategory" runat="server" />
            <asp:HiddenField ID="hfCount" runat="server" />
        </div>
           <table width="100%" border="0" cellspacing="0" cellpadding="0" class="borderkuang borderkuang-noc">
            <tr>
                <td width="550px">
                    时间：
                    <asp:TextBox ID="txtFrom" runat="server" Width="120"></asp:TextBox>
                    --
                    <asp:TextBox ID="txtTo" runat="server" Width="120"></asp:TextBox>
                    &#12288;
                    前 <asp:DropDownList ID="dropTop" runat="server">
                        <asp:ListItem Selected="True" Value="10"></asp:ListItem>
                        <asp:ListItem>20</asp:ListItem>
                    </asp:DropDownList>
                    条
                    </td>
                <td width="100px">
                <asp:Button ID="btnReStatistic" runat="server" Text="统计" class="adminsubmit add-btn" style="float:right;" OnClick="btnReStatistic_Click" /></td>
                <td>
                    
                </td>
            </tr>
        </table>
        <br />
        <table id="Table2" border="0" cellpadding="0" cellspacing="1" width="100%" class="tabChart"
            runat="server">
            <tr>
                <td>
                    <div id="proInfo" style="width:100%;min-height:400px;">
                    </div>
                </td>
            </tr>
        </table>
        <br />
        <table border="0" cellpadding="0" cellspacing="1" width="100%" 
            runat="server" id="Table3">
            <tr>
                <td width="300px">
        <cc1:GridViewEx ID="gridView" runat="server" AllowPaging="false" AllowSorting="True"
            ShowToolBar="True" AutoGenerateColumns="False" OnBind="BindData"
            UnExportedColumnNames="Modify" Width="900" PageSize="20" ShowExportExcel="True"
            ShowExportWord="False" ExcelFileName="FileName1" CellPadding="3" BorderWidth="1px"
            ShowGridLine="true" ShowHeaderStyle="true">
            <Columns>
                <asp:TemplateField HeaderText="场景" ItemStyle-HorizontalAlign="Left">
                    <ItemTemplate>
                      <%#Eval("Name")%>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="数量" ItemStyle-HorizontalAlign="Center">
                    <ItemTemplate>
                        <%#Eval("Count")%>
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

