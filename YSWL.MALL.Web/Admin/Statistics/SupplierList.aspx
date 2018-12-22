<%@ Page Title="" Language="C#" MasterPageFile="~/Admin/Basic.Master" AutoEventWireup="true" EnableEventValidation="false"
 CodeBehind="SupplierList.aspx.cs" Inherits="YSWL.MALL.Web.Admin.Statistics.SupplierList" %>
<%@ Register TagPrefix="YSWL" Src="~/Controls/AjaxRegion.ascx" TagName="AjaxRegion" %>
<%@ Register TagPrefix="YSWL" Namespace="YSWL.Web.Validator" Assembly="YSWL.Web.Validator" %>
<%@ Register TagPrefix="cc1" Namespace="YSWL.MALL.Web.Controls" Assembly="YSWL.MALL.Web" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
        <script src="/Scripts/jquery/regionjs.js" type="text/javascript"></script>
        <!--百度地图-->
    <script type="text/javascript" src="http://api.map.baidu.com/api?v=2.0&ak=v25CVSdGjepm3LAZiEdbMFo4"></script>
<script src="/Scripts/jquery/maticsoft.map.baidu-1.6.js" type="text/javascript"></script>
<script src="/Scripts/baidu/MarkerClusterer.js" type="text/javascript"></script>
<script src="/Scripts/baidu/TextIconOverlay.js" type="text/javascript"></script>
<!--百度地图-->
    <link href="/Admin/js/select2-3.4.1/select2.css" rel="stylesheet" type="text/css" />
    <script src="/Admin/js/select2-3.4.1/select2.min.js" type="text/javascript" charset="utf-8"></script>
<style type="text/css">
    .datadiv p {
        margin: 5px;
    }
    .datali {
        margin: 10px 0;
    }
</style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
        <div class="newslistabout">
            <div class="newslist_title">
            <table width="100%" border="0" cellspacing="0" cellpadding="0" class="borderkuang">
                <tr>
                    <td bgcolor="#FFFFFF" class="newstitle">
                        <asp:Literal ID="Literal1" runat="server" Text="店铺统计" />
                    </td>
                </tr>
                <tr>
                    <td bgcolor="#FFFFFF" class="newstitlebody">
                        <asp:Literal ID="Literal2" runat="server" Text="您可以查看店铺分布信息" />
                    </td>
                </tr>
            </table>
        </div>
                <table width="100%" border="0" cellspacing="0" cellpadding="0" class="borderkuang">
            <tr>
                            <td height="25"> 地区：
                                  <YSWL:AjaxRegion runat="server" ID="ajaxRegion" />
                                                  &nbsp;&nbsp;<asp:Literal ID="LiteralSupplier" runat="server" Text="加盟商" />：
                    <asp:DropDownList ID="ddlSupplier" runat="server" Width="200px">
                    </asp:DropDownList>
                                  <input type="button" class="adminsubmit" value="查询" id="btnSearch"/>
<%--                    <asp:Button ID="btnSearch" runat="server" Text="查询" class="adminsubmit" />--%>
                </td>
            </tr>
        </table>
        <br />
        <div class="borderkuang" style="width: 100%;overflow: hidden;">
             <iframe class="iframe" width="400px" height="649px" frameborder="0">
             </iframe>
                             
                     <!-- 地图信息start -->
                     <div class="newsadd_title MapDiv" style="width: 74%;float: right;">
                         <ul>
                             <li>
                                 <div id="MapContent" style=" height: 590px; margin: 0px;">
                                 </div>
                             </li>
                         </ul>
                     </div>
                     <!-- 地图信息end -->
        </div>
        </div>
        <script type="text/javascript">
            $(function () {
                $("[id$='ddlSupplier']").select2({ placeholder: "请选择" });
                var width = $(".MapDiv ul").width();
                var height = $(".MapDiv ul").height();
                $("#MapContent").width(width).height(height);
                //网页点击查询时执行
                var regionId, supplierId;
                $("[id$='btnSearch']").click(function () {
                    if (parseInt($("select:eq(2)").val()) > 0) {
                        regionId = parseInt($("select:eq(2)").val());
                    } else if ($("select:eq(1)").val() > 0) {
                        regionId = parseInt($("select:eq(1)").val());
                    } else if ($("select:eq(0)").val() > 0) {
                        regionId = parseInt($("select:eq(0)").val());
                    } else {
                        regionId = 0;
                    }
                    supplierId = $("select[id$='ddlSupplier']").val();
                    $(".iframe").attr('src', 'SupplierLeftData.aspx?regionId=' + regionId + '&sid=' + supplierId);
                    $("#dataDetailul").empty();
                    $('#MapContent').html('');
                    $.ajax({
                        url: "/MapHandle.aspx",
                        type: 'post',
                        dataType: 'json',
                        async: true,
                        timeout: 10000,
                        data: { Action: "GetUserPosition", regionId: regionId, supplierId: supplierId },
                        success: function (resultData) {
                            $('#MapContent').empty();
                            var option = InitMapOption();
                            option.SearchCity = '北京市';

                            if (resultData.count > 0 && resultData.count < 150) {//数据小于150条直接加载
                                baidumap.baidumapload(option); //先加载地图
                                option.MarkerIcon = "/Admin/Images/mapMark.png";
                                option.Markers = LoadMakers(resultData.data);
                                baidumap.baidumapload(option);
                            } else if (resultData.count < 1) { //没有数据
                                baidumap.baidumapload(option);
                            } else {//数据大于500条
                                var map = new BMap.Map("MapContent");          // 创建地图实例
                                var point = new BMap.Point(116.418261, 39.921984);
                                map.centerAndZoom(point, 15);             // 初始化地图，设置中心点坐标和地图级别
                                map.enableScrollWheelZoom(); // 允许滚轮缩放
                                map.enableKeyboard = true;
                                map.addControl(new BMap.NavigationControl());
                                map.addControl(new BMap.ScaleControl()); // 启用比例尺。            
                                map.addControl(new BMap.MapTypeControl()); // 是否启用卫星地图等等。
                                var MAX = resultData.count;
                                var markers = [];
                                var pt = null;
                                var i = 0;
                                for (; i < MAX; i++) {
                                    pt = new BMap.Point(resultData.data[i].Longitude, resultData.data[i].Dimension);
                                    markers.push(new BMap.Marker(pt));
                                }
                                ////最简单的用法，生成一个marker数组，然后调用markerClusterer类即可。
                                var markerClusterer = new BMapLib.MarkerClusterer(map, { markers: markers });
                            }
                            $('.MapDiv').show();
                        },
                        error: function (xmlHttpRequest, textStatus, errorThrown) {
                            alert(xmlHttpRequest.responseText);
                        }
                    });


                });

                function SetMarkerPoint(lng, lat) {
                    $("[id$=MarkersLongitude]").val(lng);
                    $("[id$=MarkersDimension]").val(lat);
                }

                function InitMapOption() {
                    return {
                        Container: "MapContent",        //地图的容器
                        Longitude: undefined,           //经度
                        Dimension: undefined,           //纬度
                        Level: undefined,               //缩放级别
                        SearchCity: undefined,          //查询城市
                        SearchArea: undefined,          //查询地区
                        EnableKeyboard: true,           // 是否开启键盘 上下键
                        NavigationControl: true,        //是否有鱼骨工具（进行上下左右放大或小的图标）
                        ScaleControl: true,             //是否显示比例尺
                        MapTypeControl: true,           //是否有卫星地图等的图标
                        Markers: undefined,             //标注点集合
                        MenuItem: {
                            MenuItemsetPoint: {
                                EnableDragging: true,   //启用标注点拖拽
                                SetAnimation: true,     //标注点动画
                                MenuEvent: "dragend",   //设置坐标事件名称 目前是标注点拖拽 用于记录最后坐标
                                CallBack: function (lng, lat) { //坐标事件回调
                                    SetMarkerPoint(lng, lat);
                                },
                                MenuClickCallBack: function (lng, lat) {    //通过右键菜单新增标注点事件
                                    SetMarkerPoint(lng, lat);
                                    alert("新增标注成功, 请您填写地图下方的标注点信息.");
                                }
                            }
                        }
                    };
                }
            });
</script>
 <YSWL:ValidatorContainer runat="server" ID="ValidatorContainer" />
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceCheckright" runat="server">
</asp:Content>
