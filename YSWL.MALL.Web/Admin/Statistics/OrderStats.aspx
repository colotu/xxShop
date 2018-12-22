<%@ Page Title="订单来源统计" Language="C#" MasterPageFile="~/Admin/Basic.Master" AutoEventWireup="true"
    CodeBehind="OrderStats.aspx.cs" Inherits="YSWL.MALL.Web.Admin.Statistics.OrderStats" %>

<%@ Register Assembly="YSWL.MALL.Web" Namespace="YSWL.MALL.Web.Controls" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link href="/Scripts/jqueryui/jquery-ui-1.8.19.custom.css" rel="stylesheet" type="text/css" />
    <script src="/Scripts/jqueryui/jquery-ui-1.8.19.custom.min.js" type="text/javascript"></script>
    <script src="/admin/js/jqueryui/JqueryDataPicker4CN.js" type="text/javascript"></script>
    <script src="/Scripts/Highcharts/highcharts.js" type="text/javascript"></script>
    <%--<script src="/Scripts/Highcharts/themes/gray.js" type="text/javascript"></script>--%>
    <script type="text/javascript">
        $(function () {
            $.datepicker.setDefaults($.datepicker.regional['zh-CN']);
            $("[id$=txtStartDate]").prop("readonly", true).datepicker({
                //defaultDate: "+1w",
                changeMonth: true,
                dateFormat: "yy-mm-dd",
                onClose: function (selectedDate) {
                    $("[id$=txtEndDate]").datepicker("option", "minDate", selectedDate);
                }
            });
            $("[id$=txtEndDate]").prop("readonly", true).datepicker({
                //defaultDate: "+1w",
                changeMonth: true,
                dateFormat: "yy-mm-dd",
                onClose: function (selectedDate) {
                    $("[id$=txtStartDate]").datepicker("option", "maxDate", selectedDate);
                    $("[id$=txtEndDate]").val($(this).val());
                }
            });

            var countData = $("[id$=hfCountData]").val();
            if (countData == "") {
                return;
            }
            var countValue = $.parseJSON(countData);

            $('#tdCountSource').highcharts({
                chart: {
                    plotBackgroundColor: null,
                    plotBorderWidth: null,
                    plotShadow: false
                },
                title: {
                    text: '订单来源统计-订单数'
                },
                tooltip: {
                    pointFormat: '<b>{series.name}:{point.percentage:.1f}%  数值为:{point.y}</b>'
                },
                plotOptions: {
                    pie: {
                        allowPointSelect: true,
                        cursor: 'pointer',
                        dataLabels: {
                            enabled: true,
                            color: '#000000',
                            connectorColor: '#000000',
                            format: '<b>{point.name}</b>: {point.percentage:.1f} %'
                        }
                    }
                },
                series: [{
                    type: 'pie',
                    name: '所占比例',
                    data: countValue
                }]
            });
            $("#tdCountSource text:last").hide();
            $("#tdCountSource span:last").hide();

            var priceData = $("[id$=hfPriceData]").val();
            if (priceData == "") {
                return;
            }
            var priceValue = $.parseJSON(priceData);

            $('#tdPriceSource').highcharts({
                chart: {
                    plotBackgroundColor: null,
                    plotBorderWidth: null,
                    plotShadow: false
                },
                title: {
                    text: '订单来源统计-销售额'
                },
                tooltip: {
                    pointFormat: '<b>{series.name}: {point.percentage:.1f}%  数值为:{point.y}</b>'
                },
                plotOptions: {
                    pie: {
                        allowPointSelect: true,
                        cursor: 'pointer',
                        dataLabels: {
                            enabled: true,
                            color: '#000000',
                            connectorColor: '#000000',
                            format: '<b>{point.name}</b>: {point.percentage:.1f} %'
                        }
                    }
                },
                series: [{
                    type: 'pie',
                    name: '所占比例',
                    data: priceValue
                }]
            });
            $("#tdPriceSource text:last").hide();
            $("#tdPriceSource span:last").hide();
        })
    </script>
    <style type='text/css'>
.text { mso-number-format:\@; }
.GridViewTyle tr td { border: 1px solid #CCCCCC;}
.GridViewTyle tr td table tr td{ border: none;}
.GridViewTyle tr td { border-spacing: 2px; border-color: #CCCCCC;border-collapse: collapse;empty-cells: show; }
.GridViewTyle a{ color:#1317fc;text-decoration: none;}
.GridViewTyle a:hover{ color:#1317fc;}
.GridViewTyle span{  text-align:center;}
</style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:HiddenField ID="hfCountData" runat="server" />
    <asp:HiddenField ID="hfPriceData" runat="server" />
    <div class="newslistabout">
        <div class="newslist_title">
            <table width="100%" border="0" cellspacing="0" cellpadding="0" class="borderkuang">
                <tr>
                    <td bgcolor="#FFFFFF" class="newstitle">
                        <asp:Literal ID="LiteralRT" runat="server" Text="订单来源统计" />
                    </td>
                </tr>
                <tr>
                    <td bgcolor="#FFFFFF" class="newstitlebody">
                        <asp:Literal ID="LiteralIn" runat="server" Text="您可以查看订单来源统计信息" />
                    </td>
                </tr>
            </table>
        </div>
        <table width="100%" border="0" cellspacing="0" cellpadding="0" class="borderkuang">
            <tr>
                <td width="280px">
                    时间：
                    <asp:TextBox ID="txtStartDate" runat="server" Width="90"></asp:TextBox>
                    --
                    <asp:TextBox ID="txtEndDate" runat="server" Width="90"></asp:TextBox>
                    <asp:Button ID="btnReStatistic" runat="server" Text="统计" class="adminsubmit" OnClick="btnReStatistic_Click" />
                </td>
            </tr>
        </table>
        <br />
<%--        <div id="tdCountSource" style="width:100%;height:400px;">
        </div>
        <br />--%>
        <table border="0" cellpadding="0" cellspacing="1" width="100%" class="borderkuang">
            <tr>
                <td id="tdCountSource" style="width:50%;height:400px;">
            
        </td> 
                <td width="800px">
                    <table cellspacing="0" cellpadding="3" border="0px" style="border-color: #CCCCCC;
                        border-width: 1px; border-style: solid; width: 100%; border-collapse: collapse;"
                        class="GridViewTyle">
                        <tr height="35px" style="background-color: #E3EFFF; height: 35px; background: #FFF">
                            <th style="border: 1px solid #dae2e8; border-right: 0px;" scope="col">
                                来源
                            </th>
                            <th style="border: 1px solid #dae2e8; border-left: 0px; border-right: 0px;" scope="col">
                                订单数
                            </th>
                        </tr>
                        <tr height="27px" style="height: 25px; background: none repeat scroll 0% 0% rgb(244, 244, 244);">
                            <td height="27px" align="left" style="padding-left: 5px; height: 27px;">
                                PC商城
                            </td>
                            <td height="27px" align="center" style="padding-left: 5px; height: 27px;">
                                <asp:Label ID="lblCountPC" runat="server" Text="Label"></asp:Label>
                            </td>
                        </tr>
                        <tr height="27px" style="height: 25px; background: none repeat scroll 0% 0% rgb(244, 244, 244);">
                            <td height="27px" align="left" style="padding-left: 5px; height: 27px;">
                                微信商城
                            </td>
                           <td height="27px" align="center" style="padding-left: 5px; height: 27px;">
                                <asp:Label ID="lblCountWe" runat="server" Text="Label"></asp:Label>
                            </td>
                        </tr>
                        <tr height="27px" style="height: 25px; background: none repeat scroll 0% 0% rgb(244, 244, 244);">
                            <td height="27px" align="left" style="padding-left: 5px; height: 27px;">
                                业务员代下单
                            </td>
                            <td height="27px" align="center" style="padding-left: 5px; height: 27px;">
                                <asp:Label ID="lblCountRen" runat="server" Text="Label"></asp:Label>
                            </td>
                        </tr>
                        <tr height="27px" style="height: 25px; background: none repeat scroll 0% 0% rgb(244, 244, 244);">
                            <td height="27px" align="left" style="padding-left: 5px; height: 27px;">
                                客服代下单
                            </td>
                            <td height="27px" align="center" style="padding-left: 5px; height: 27px;">
                                <asp:Label ID="lblCountCust" runat="server" Text=""></asp:Label>
                            </td>
                        </tr>
                        <tr height="27px" style="height: 25px; background: none repeat scroll 0% 0% rgb(244, 244, 244);">
                            <td height="27px" align="left" style="padding-left: 5px; height: 27px;">
                                订货
                            </td>
                            <td height="27px" align="center" style="padding-left: 5px; height: 27px;">
                                <asp:Label ID="lblCountAPP" runat="server" Text="Label"></asp:Label>
                            </td>
                        </tr>
                    </table>
        </td>
        </tr> 
        </table>
       
        <br />
        <table border="0" cellpadding="0" cellspacing="1" width="100%" class="borderkuang">
            <tr>
                  <td id="tdPriceSource" style="width:50%;height:400px;">
                      
                  </td>
                <td width="800px">
                    <table cellspacing="0" cellpadding="3" border="0px" style="border-color: #CCCCCC;
                        border-width: 1px; border-style: solid; width: 100%; border-collapse: collapse;"
                        class="GridViewTyle">
                        <tr height="35px" style="background-color: #E3EFFF; height: 35px; background: #FFF">
                            <th style="border: 1px solid #dae2e8; border-right: 0px;" scope="col">
                                来源
                            </th>
                            <th style="border: 1px solid #dae2e8; border-left: 0px; border-right: 0px;" scope="col">
                                销售额
                            </th>
                        </tr>
                        <tr height="27px" style="height: 25px; background: none repeat scroll 0% 0% rgb(244, 244, 244);">
                            <td height="27px" align="left" style="padding-left: 5px; height: 27px;">
                                PC商城
                            </td>
                            <td height="27px" align="center" style="padding-left: 5px; height: 27px;">
                                <asp:Label ID="lblPricePC" runat="server" Text="Label"></asp:Label>
                            </td>
                        </tr>
                        <tr height="27px" style="height: 25px; background: none repeat scroll 0% 0% rgb(244, 244, 244);">
                            <td height="27px" align="left" style="padding-left: 5px; height: 27px;">
                                微信商城
                            </td>
                           <td height="27px" align="center" style="padding-left: 5px; height: 27px;">
                                <asp:Label ID="lblPriceWe" runat="server" Text="Label"></asp:Label>
                            </td>
                        </tr>
                        <tr height="27px" style="height: 25px; background: none repeat scroll 0% 0% rgb(244, 244, 244);">
                            <td height="27px" align="left" style="padding-left: 5px; height: 27px;">
                               业务员代注册
                            </td>
                            <td height="27px" align="center" style="padding-left: 5px; height: 27px;">
                                <asp:Label ID="lblPriceRen" runat="server" Text="Label"></asp:Label>
                            </td>
                        </tr>
                        
                        <tr height="27px" style="height: 25px; background: none repeat scroll 0% 0% rgb(244, 244, 244);">
                            <td height="27px" align="left" style="padding-left: 5px; height: 27px;">
                               客服代注册
                            </td>
                            <td height="27px" align="center" style="padding-left: 5px; height: 27px;">
                                <asp:Label ID="lblPriceCust" runat="server" Text="Label"></asp:Label>
                            </td>
                        </tr>
                        <tr height="27px" style="height: 25px; background: none repeat scroll 0% 0% rgb(244, 244, 244);">
                            <td height="27px" align="left" style="padding-left: 5px; height: 27px;">
                                订货
                            </td>
                            <td height="27px" align="center" style="padding-left: 5px; height: 27px;">
                                <asp:Label ID="lblPriceAPP" runat="server" Text="Label"></asp:Label>
                            </td>
                        </tr>
                    </table>
        </td>
      
         </tr> </table>
    </div>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceCheckright" runat="server">
</asp:Content>
