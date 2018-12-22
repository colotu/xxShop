<%@ Page Language="C#" MasterPageFile="~/Admin/Basic.Master" AutoEventWireup="true"
    CodeBehind="ProductCategory.aspx.cs" Inherits="YSWL.MALL.Web.Admin.Statistics.ProductCategory" Title="商品分类统计" %>
<%@ Register Assembly="YSWL.MALL.Web" Namespace="YSWL.MALL.Web.Controls" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
<script src="/Scripts/Highcharts/highcharts.js" type="text/javascript"></script>
    <script src="/Scripts/Highcharts/themes/gray.js" type="text/javascript"></script>
    <script type="text/javascript">
        $(document).ready(function () {
            //商品统计
            var categories = $("[id$='hfCategory']").val().split(',');
            var proCount = [];
            var proOffCount = [];
            var datavalue = $("[id$='hfCount']").val().split(',');
            var offDatavalue = $("[id$='hfOffCount']").val().split(',');
            for (var i = 0; i < datavalue.length; i++) {
                var item = parseFloat(datavalue[i]);
                proCount.push(item);
            }
            for (var i = 0; i < offDatavalue.length; i++) {
                var item = parseFloat(offDatavalue[i]);
                proOffCount.push(item);
            }

            $('#proInfo').highcharts({
                chart: {
                    type: 'bar'
                },
                title: {
                    text: '商品统计'
                },
                //                subtitle: {
                //                    text: '商品分类统计'
                //                },
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
                        return '<b>' + this.x + '</b><br/>' + this.series.name + '： ' + this.y + '件';
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
                series: [ {
                    name: '下架',
                    data: proOffCount
                },{
                    name: '在售',
                    data: proCount
                }]
            });
            $("#proInfo text:last").hide();
            $("#proInfo span:last").hide();
        });
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="newslistabout">
        <div class="newslist_title">
            <table width="100%" border="0" cellspacing="0" cellpadding="0" class="borderkuang">
                <tr>
                    <td bgcolor="#FFFFFF" class="newstitle">
                        <asp:Literal ID="Literal2" runat="server" Text="分类商品数据统计" />
                    </td>
                </tr>
                <tr>
                    <td bgcolor="#FFFFFF" class="newstitlebody">
                        <asp:Literal ID="Literal3" runat="server" Text="您可以查看分类商品数据统计信息" />
                    </td>
                </tr>
            </table>
            <asp:HiddenField ID="hfCategory" runat="server" />
            <asp:HiddenField ID="hfCount" runat="server" />
            <asp:HiddenField ID="hfOffCount" runat="server" />
        </div>
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
                <td>
        <cc1:GridViewEx ID="gridView" runat="server" AllowPaging="false" AllowSorting="True"
            ShowToolBar="false" AutoGenerateColumns="False" OnBind="BindData"
            UnExportedColumnNames="Modify" PageSize="1000" ShowExportExcel="False"
            ShowExportWord="False" ExcelFileName="FileName1" CellPadding="3" BorderWidth="1px"
            ShowGridLine="true" ShowHeaderStyle="true">
            <Columns>
                <asp:TemplateField HeaderText="分类名称" ItemStyle-HorizontalAlign="Left">
                    <ItemTemplate>
                        <%#Eval("CategoryName")%>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="在售数量" ItemStyle-HorizontalAlign="Center">
                    <ItemTemplate>
                        <%#(YSWL.Common.Globals.SafeInt(Eval("count1"), 0))%>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="下架数量" ItemStyle-HorizontalAlign="Center">
                    <ItemTemplate>
                        <%#(YSWL.Common.Globals.SafeInt(Eval("count0"), 0))%>
                    </ItemTemplate>
                </asp:TemplateField>
                                <asp:TemplateField HeaderText="总数量" ItemStyle-HorizontalAlign="Center">
                    <ItemTemplate>
                        <%#(YSWL.Common.Globals.SafeInt(Eval("count0"), 0) + YSWL.Common.Globals.SafeInt(Eval("count1"), 0))%>
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
