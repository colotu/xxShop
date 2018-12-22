<%@ Page Title="" Language="C#" MasterPageFile="~/Admin/Basic.Master" AutoEventWireup="true" CodeBehind="UserPositionList.aspx.cs" Inherits="YSWL.MALL.Web.Admin.Statistics.UserPositionList" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
<!--百度地图-->
 <script src="http://api.map.baidu.com/api?v=1.3" type="text/javascript"></script>
<script src="/Scripts/jquery/maticsoft.map.baidu-1.6.js" type="text/javascript"></script>
<!--百度地图-->
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
                         <asp:HiddenField runat="server" ID="hiddenSpId"/>
                         <!-- 地图信息start -->
                     <div class="newsadd_title MapDiv" style="width: 82%;float: right;">
                         <ul>
                             <li class="loading" style="margin: 0; width: 100%;">
                                 <p style="text-align: center">
                                     <img src="/Images/data-loading.gif" alt="数据装载中" /></p>
                             </li>
                         </ul>
                         <ul>
                             <li>
                                 <div id="MapContent" style="width: 750px; height: 500px; margin: 0px;">
                                 </div>
                             </li>
                         </ul>
                         <ul>
                             <li class="loading" style="margin: 0; width: 100%;">
                                 <p style="text-align: center">
                                     <img src="/Images/data-loading.gif" alt="数据装载中" /></p>
                             </li>
                         </ul>
                     </div>
                     <!-- 地图信息end -->
                     <script type="text/javascript">
                         $(function () {
                             var supplierId = $.getUrlParam("supplierId");
                             $.ajax({
                                 url: "/MapHandle.aspx",
                                 type: 'post',
                                 dataType: 'json',
                                 async: false,
                                 timeout: 10000,
                                 data: { Action: "GetSpInfo", spId: supplierId },
                                 success: function (resultData) {
                                     $('#MapContent').empty();
                                     var option = InitMapOption();
                                     option.SearchCity = '北京市';
                                     if (resultData.count > 0) {//有商家数据
                                         $("#resultCount").text(resultData.count);
                                         for (var i = 0; i < resultData.count; i++) {
                                             $("#dataDetailul").append("<li class='datali'> <div class='datadiv'><p>" + resultData.spInfos[i].name + "</p><p>" + resultData.spInfos[i].shopName + "</p><p>电话：" + resultData.spInfos[i].phone + "</p></div></li>");
                                         }
                                         // option.MarkerIcon = "/Admin/Images/antlogo.png";
                                         option.Markers = LoadMakers(resultData.spInfos);
                                     } else {
                                         $("#resultCount").text(0);
                                     }
                                     $("#result").show();
                                     baidumap.baidumapload(option);
                                     $('.MapDiv').show();
                                 },
                                 error: function (xmlHttpRequest, textStatus, errorThrown) {
                                     alert(xmlHttpRequest.responseText);
                                 }
                             });
                         });
                     </script>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceCheckright" runat="server">
</asp:Content>
