<%@ Page Title="" Language="C#" MasterPageFile="~/Admin/Basic.Master" AutoEventWireup="true" EnableEventValidation="false"
 CodeBehind="DistributionLoad.aspx.cs" Inherits="YSWL.MALL.Web.Admin.Statistics.DistributionLoad" %>
<%@ Register TagPrefix="YSWL" Src="~/Controls/AjaxRegion.ascx" TagName="AjaxRegion" %>
<%@ Register TagPrefix="YSWL" Namespace="YSWL.Web.Validator" Assembly="YSWL.Web.Validator" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
        <link href="/Scripts/jqueryui/jquery-ui-1.8.19.custom.css" rel="stylesheet" type="text/css" />
        <script src="/Scripts/jquery/regionjs.js" type="text/javascript"></script>
            <script src="/Scripts/jqueryui/jquery-ui-1.8.19.custom.min.js" type="text/javascript"></script>
            <script src="/admin/js/jqueryui/JqueryDataPicker4CN.js" type="text/javascript"></script>
        <!--百度地图-->
<%-- <script src="http://api.map.baidu.com/api?v=1.3" type="text/javascript"></script>--%>


    <script type="text/javascript" src="http://api.map.baidu.com/api?v=2.0&ak=v25CVSdGjepm3LAZiEdbMFo4"></script>
    <script type="text/javascript" src="http://api.map.baidu.com/library/Heatmap/2.0/src/Heatmap_min.js"></script>
    <script src="/Scripts/jquery/maticsoft.map.baidu-1.6.js" type="text/javascript"></script>
<!--百度地图-->
<style type="text/css">
    .datadiv p {
        margin: 5px;
    }
    .datali {
        margin: 10px 0;
    }
</style>

<style type="text/css">
    ul,li{list-style: none;margin:0;padding: 0}

    html{height:100%}
    body{height:100%;margin:0px;padding:0px}
    #container{height:100%;width:78%;float:left;border-right:2px solid #bcbcbc;}
    #r-result{height:100%;width:20%;float:left;}
    .btn-container{margin:20px;}
    fieldset{border: 1px solid;border-radius: 3px;}
    fieldset label{font-size: 14px; line-height: 30px;}
    .btn{
    	color: #333;background-color: #fff;display: inline-block;padding: 6px 12px;font-size: 14px;
		font-weight: normal;line-height: 1.428571429;border: 1px solid #ccc;border-radius: 4px;
		margin-top: 5px;margin-bottom: 5px;}	
	.btn:hover{color: #333;background-color: #ebebeb;border-color: #adadad;}
	.text-primary{
		font-weight: bold;
	}
	textarea{border: 1px solid #ccc;border-radius: 4px;}
	textarea:focus{border-color: #66afe9;outline: 0;box-shadow: inset 0 1px 1px rgba(0,0,0,0.075),0 0 8px rgba(102,175,233,0.6);}
	.color-list li{font-size: 14px; line-height: 30px;}
    </style>
<script type="text/javascript">
    $(function() {
        $.datepicker.setDefaults($.datepicker.regional['zh-CN']);
        $("[id$='txtCreatedDateStart']").prop("readonly", true).datepicker({
            changeMonth: true,
            dateFormat: "yy-mm-dd",
            onClose: function (selectedDate) {
                $("[id$='txtCreatedDateEnd']").datepicker("option", "minDate", selectedDate);
            }
        });
        $("[id$='txtCreatedDateEnd']").prop("readonly", true).datepicker({

            changeMonth: true,
            dateFormat: "yy-mm-dd",
            onClose: function (selectedDate) {
                $("[id$='txtCreatedDateStart']").datepicker("option", "maxDate", selectedDate);
                $("[id$='txtCreatedDateEnd']").val($(this).val());
            }
        });
    });
</script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
        <div class="newslistabout">
            <div class="newslist_title">
            <table width="100%" border="0" cellspacing="0" cellpadding="0" class="borderkuang">
                <tr>
                    <td bgcolor="#FFFFFF" class="newstitle">
                        <asp:Literal ID="Literal1" runat="server" Text="配送负荷统计" />
                    </td>
                </tr>
                <tr>
                    <td bgcolor="#FFFFFF" class="newstitlebody">
                        <asp:Literal ID="Literal2" runat="server" Text="您可以查看配送负荷信息" />
                    </td>
                </tr>
            </table>
        </div>
                <table width="100%" border="0" cellspacing="0" cellpadding="0" class="borderkuang">
            <tr>
                            <td height="25"> 地区：
                                  <YSWL:AjaxRegion runat="server" ID="ajaxRegion" />
<%--                                  配送站名称：<asp:TextBox runat="server" ID="txtSpName"></asp:TextBox>--%>
                    &nbsp;&nbsp;<asp:Literal ID="LiteralCreatedDate" runat="server" Text="日期时间" />：
                    <asp:TextBox ID="txtCreatedDateStart" runat="server" Width="80px">
                        
                    </asp:TextBox>-<asp:TextBox ID="txtCreatedDateEnd" Width="80px" runat="server"></asp:TextBox>
                                  <input type="button" class="adminsubmit" value="查询" id="btnSearch"/>
                    <%--<asp:Button ID="btnSearch" runat="server" Text="查询" class="adminsubmit" />--%>
                </td>
            </tr>
        </table>
        <br />
        <div class="borderkuang" style="width: 100%;overflow: hidden;">
                     <!-- 地图信息start -->
                     <div class="newsadd_title MapDiv"  style="width: 75%;">

                         <ul>
                             <li>
                                 <div id="MapContent" style="height: 590px; margin: 0px;">
                                 </div>
                             </li>
                             
                             <fieldset>
    		 <legend>热力图设置区域</legend>
    	<label>设置热力图半径0-100</label>
    	<input type="range" max="100" style="width:150px" min="1" value="20" onchange="setRadius(this.value)">
    	<span id="radius-result" class="text-primary">20</span>
    	<br>
    	<label>设置热力图透明度0-100</label>
    	<input type="range" max="100" style="width:150px" min="1" value="60" onchange="setOpacity(this.value)">
    	<span id="opacity-result" class="text-primary">60</span>
		<br>
		<label>设置热力图渐变区间</label>

		<ul class="color-list">
			<li>起始颜色: <input data-key="0.1" type="color" value="#66FF00" onchange="setGradient()"></li>
			<li>中间颜色: <input data-key="0.5" type="color" value="#FFAA00" onchange="setGradient()"></li>
			<li>结束颜色: <input data-key="1.0" type="color" value="#FF0000" onchange="setGradient()"></li>
		</ul>

        <span style="font-size:14px;">显示热力图:</span><input type="checkbox" onclick="toggle();" checked="checked"><br>
        </fieldset>

                         </ul>

                     </div>
                     <!-- 地图信息end -->
                     
                     <div class="btn-container" style="float: right;width: 20%;">
    	
    </div>
        </div>
        </div>
        <script type="text/javascript">
            $(function () {
                var width = $(".MapDiv ul").width();
                var height = $(".MapDiv ul").height();
                $("#MapContent").width(width).height(height);
                var points; //= [];
                var map;
                $("#btnSearch").click(function () {
                    $.ajax({
                        url: "/MapHandle.aspx",
                        type: 'post',
                        dataType: 'json',
                        async: true,
                        timeout: 10000,
                        data: { Action: "GetDistributionLoad", startDate: $("[id$='startDate']").val(), endDate: $("[id$='endDate']").val() },
                        success: function (resultData) {
                            map = new BMap.Map("MapContent");          // 创建地图实例
                            var point = new BMap.Point(116.418261, 39.921984);
                            map.centerAndZoom(point, 15);             // 初始化地图，设置中心点坐标和地图级别
                            map.enableScrollWheelZoom(); // 允许滚轮缩放
                            map.enableKeyboard = true;
                            map.addControl(new BMap.NavigationControl());
                            map.addControl(new BMap.ScaleControl()); // 启用比例尺。            
                            map.addControl(new BMap.MapTypeControl()); // 是否启用卫星地图等等。
                            if (resultData.status == "Ok") {//
                                points = resultData.data;
//                                points = [
//    { "lng": 116.418261, "lat": 39.921984, "count": 50 },
//    { "lng": 116.423332, "lat": 39.916532, "count": 51 },
//    { "lng": 116.419787, "lat": 39.930658, "count": 15 },
//    { "lng": 116.418455, "lat": 39.920921, "count": 40 },
//    { "lng": 116.418843, "lat": 39.915516, "count": 100 },
//    { "lng": 116.42546, "lat": 39.918503, "count": 6 },
//    { "lng": 116.423289, "lat": 39.919989, "count": 18 },
//    { "lng": 116.418162, "lat": 39.915051, "count": 80 },
//    { "lng": 116.422039, "lat": 39.91782, "count": 11 },
//    { "lng": 116.41387, "lat": 39.917253, "count": 7 },
//    { "lng": 116.41773, "lat": 39.919426, "count": 42 },
//    { "lng": 116.421107, "lat": 39.916445, "count": 4 },
//    { "lng": 116.417521, "lat": 39.917943, "count": 27 },
//    { "lng": 116.419812, "lat": 39.920836, "count": 23 },
//    { "lng": 116.420682, "lat": 39.91463, "count": 60 },
//    { "lng": 116.415424, "lat": 39.924675, "count": 8 },
//    { "lng": 116.419242, "lat": 39.914509, "count": 15 },
//    { "lng": 116.422766, "lat": 39.921408, "count": 25 },
//    { "lng": 116.421674, "lat": 39.924396, "count": 21 },
//    { "lng": 116.427268, "lat": 39.92267, "count": 1 },
//    { "lng": 116.417721, "lat": 39.920034, "count": 51 },
//    { "lng": 116.412456, "lat": 39.92667, "count": 7 },
//    { "lng": 116.420432, "lat": 39.919114, "count": 11 },
//    { "lng": 116.425013, "lat": 39.921611, "count": 35 },
//    { "lng": 116.418733, "lat": 39.931037, "count": 22 },
//    { "lng": 116.419336, "lat": 39.931134, "count": 4 },
//    { "lng": 116.413557, "lat": 39.923254, "count": 5 },
//    { "lng": 116.418367, "lat": 39.92943, "count": 3 },
//    { "lng": 116.424312, "lat": 39.919621, "count": 100 },
//    { "lng": 116.423874, "lat": 39.919447, "count": 87 },
//    { "lng": 116.424225, "lat": 39.923091, "count": 32 },
//    { "lng": 116.417801, "lat": 39.921854, "count": 44 },
//    { "lng": 116.417129, "lat": 39.928227, "count": 21 },
//    { "lng": 116.426426, "lat": 39.922286, "count": 80 },
//    { "lng": 116.421597, "lat": 39.91948, "count": 32 },
//    { "lng": 116.423895, "lat": 39.920787, "count": 26 },
//    { "lng": 116.423563, "lat": 39.921197, "count": 17 },
//    { "lng": 116.417982, "lat": 39.922547, "count": 17 },
//    { "lng": 116.426126, "lat": 39.921938, "count": 25 },
//    { "lng": 116.42326, "lat": 39.915782, "count": 100 },
//    { "lng": 116.419239, "lat": 39.916759, "count": 39 },
//    { "lng": 116.417185, "lat": 39.929123, "count": 11 },
//    { "lng": 116.417237, "lat": 39.927518, "count": 9 },
//    { "lng": 116.417784, "lat": 39.915754, "count": 47 },
//    { "lng": 116.420193, "lat": 39.917061, "count": 52 },
//    { "lng": 116.422735, "lat": 39.915619, "count": 100 },
//    { "lng": 116.418495, "lat": 39.915958, "count": 46 },
//    { "lng": 116.416292, "lat": 39.931166, "count": 9 },
//    { "lng": 116.419916, "lat": 39.924055, "count": 8 },
//    { "lng": 116.42189, "lat": 39.921308, "count": 11 },
//    { "lng": 116.413765, "lat": 39.929376, "count": 3 },
//    { "lng": 116.418232, "lat": 39.920348, "count": 50 },
//    { "lng": 116.417554, "lat": 39.930511, "count": 15 },
//    { "lng": 116.418568, "lat": 39.918161, "count": 23 },
//    { "lng": 116.413461, "lat": 39.926306, "count": 3 },
//    { "lng": 116.42232, "lat": 39.92161, "count": 13 },
//    { "lng": 116.4174, "lat": 39.928616, "count": 6 },
//    { "lng": 116.424679, "lat": 39.915499, "count": 21 },
//    { "lng": 116.42171, "lat": 39.915738, "count": 29 },
//    { "lng": 116.417836, "lat": 39.916998, "count": 99 },
//    { "lng": 116.420755, "lat": 39.928001, "count": 10 },
//    { "lng": 116.414077, "lat": 39.930655, "count": 14 },
//    { "lng": 116.426092, "lat": 39.922995, "count": 16 },
//    { "lng": 116.41535, "lat": 39.931054, "count": 15 },
//    { "lng": 116.413022, "lat": 39.921895, "count": 13 },
//    { "lng": 116.415551, "lat": 39.913373, "count": 17 },
//    { "lng": 116.421191, "lat": 39.926572, "count": 1 },
//    { "lng": 116.419612, "lat": 39.917119, "count": 9 },
//    { "lng": 116.418237, "lat": 39.921337, "count": 54 },
//    { "lng": 116.423776, "lat": 39.921919, "count": 26 },
//    { "lng": 116.417694, "lat": 39.92536, "count": 17 },
//    { "lng": 116.415377, "lat": 39.914137, "count": 19 },
//    { "lng": 116.417434, "lat": 39.914394, "count": 43 },
//    { "lng": 116.42588, "lat": 39.922622, "count": 27 },
//    { "lng": 116.418345, "lat": 39.919467, "count": 8 },
//    { "lng": 116.426883, "lat": 39.917171, "count": 3 },
//    { "lng": 116.423877, "lat": 39.916659, "count": 34 },
//    { "lng": 116.415712, "lat": 39.915613, "count": 14 },
//    { "lng": 116.419869, "lat": 39.931416, "count": 12 },
//    { "lng": 116.416956, "lat": 39.925377, "count": 11 },
//    { "lng": 116.42066, "lat": 39.925017, "count": 38 },
//    { "lng": 116.416244, "lat": 39.920215, "count": 91 },
//    { "lng": 116.41929, "lat": 39.915908, "count": 54 },
//    { "lng": 116.422116, "lat": 39.919658, "count": 21 },
//    { "lng": 116.4183, "lat": 39.925015, "count": 15 },
//    { "lng": 116.421969, "lat": 39.913527, "count": 3 },
//    { "lng": 116.422936, "lat": 39.921854, "count": 24 },
//    { "lng": 116.41905, "lat": 39.929217, "count": 12 },
//    { "lng": 116.424579, "lat": 39.914987, "count": 57 },
//    { "lng": 116.42076, "lat": 39.915251, "count": 70 },
//    { "lng": 116.425867, "lat": 39.918989, "count": 8}];
                                heatmapOverlay = new BMapLib.HeatmapOverlay({ "radius": 20 });
                                map.addOverlay(heatmapOverlay);
                                heatmapOverlay.setDataSet({ data: points, max: 100 });
                            } 

                        },
                        error: function (xmlHttpRequest, textStatus, errorThrown) {
                            alert(xmlHttpRequest.responseText);
                        }
                    });

                });



            });








            function setRadius(radius) {
                document.getElementById("radius-result").innerHTML = radius;
                heatmapOverlay.setOptions({ "radius": radius });
            }

            function setOpacity(opacity) {
                document.getElementById("opacity-result").innerHTML = opacity;
                heatmapOverlay.setOptions({ "opacity": opacity });
            }

            function toggle() {
                heatmapOverlay.toggle();
            }

            function setGradient() {

                var gradient = {};
                var colors = document.querySelectorAll("input[type='color']");
                colors = [].slice.call(colors, 0);
                colors.forEach(function (ele) {
                    gradient[ele.getAttribute("data-key")] = ele.value;
                });
                heatmapOverlay.setOptions({ "gradient": gradient });
            }


            function isSupportCanvas() {
                var elem = document.createElement('canvas');
                return !!(elem.getContext && elem.getContext('2d'));
            }

        </script>
        
        
 <YSWL:ValidatorContainer runat="server" ID="ValidatorContainer" />
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceCheckright" runat="server">
</asp:Content>
