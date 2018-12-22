<%@ Page Title="" Language="C#" MasterPageFile="~/Admin/Basic.Master" AutoEventWireup="true" EnableEventValidation="false"
 CodeBehind="EMSList.aspx.cs" Inherits="YSWL.MALL.Web.Admin.Statistics.EMSList" %>
<%@ Register TagPrefix="YSWL" Src="~/Controls/AjaxRegion.ascx" TagName="AjaxRegion" %>
<%@ Register TagPrefix="YSWL" Namespace="YSWL.Web.Validator" Assembly="YSWL.Web.Validator" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
        <script src="/Scripts/jquery/regionjs.js" type="text/javascript"></script>
        <!--百度地图-->
 <script src="http://api.map.baidu.com/api?v=1.3" type="text/javascript"></script>
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
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
        <div class="newslistabout">
            <div class="newslist_title">
            <table width="100%" border="0" cellspacing="0" cellpadding="0" class="borderkuang">
                <tr>
                    <td bgcolor="#FFFFFF" class="newstitle">
                        <asp:Literal ID="Literal1" runat="server" Text="配送站统计" />
                    </td>
                </tr>
                <tr>
                    <td bgcolor="#FFFFFF" class="newstitlebody">
                        <asp:Literal ID="Literal2" runat="server" Text="您可以查看配送站分布信息" />
                    </td>
                </tr>
            </table>
        </div>
                <table width="100%" border="0" cellspacing="0" cellpadding="0" class="borderkuang">
            <tr>
                            <td height="25"> 地区：
                                  <YSWL:AjaxRegion runat="server" ID="ajaxRegion" />
                                  配送站名称：<asp:TextBox runat="server" ID="txtSpName"></asp:TextBox>
                                  <input type="button" class="adminsubmit" value="查询" id="btnSearch"/>
                    <%--<asp:Button ID="btnSearch" runat="server" Text="查询" class="adminsubmit" />--%>
                </td>
            </tr>
        </table>
        <br />
        <div class="borderkuang" style="width: 100%;overflow: hidden;">
            <input type="hidden" class="mouseEnter" id="mouseEnter" latitude="0" longitude="0"/>
            <input type="hidden" class="mouseLeave" id="mouseLeave"  latitude="0" longitude="0"/>
                                          <iframe id="iframe" class="iframe" width="370px" height="649px" frameborder="0">
             </iframe>
                     <!-- 地图信息start -->
                     <div class="newsadd_title MapDiv" style="width: 74%;float: right;">
                         <ul>
                             <li>
                                 <div id="MapContent" style="height: 590px; margin: 0px;">
                                 </div>
                             </li>
                         </ul>
                        
                     </div>
                     <!-- 地图信息end -->
        </div>
        </div>
        <script type="text/javascript">
            $(function () {
                var width = $(".MapDiv ul").width();
                var height = $(".MapDiv ul").height();
                $("#MapContent").width(width).height(height);
                //网页点击查询时执行
                var regionId, spName;
                $("[id$='btnSearch']").click(function () {
                    if (parseInt($("select:eq(2)").val()) > 0) {
                        regionId = parseInt($("select:eq(2)").val());
                    } else if ($("select:eq(1)").val() > 0) {
                        regionId = parseInt($("select:eq(1)").val());
                    } else if ($("select:eq(0)").val() > 0) {
                        regionId = parseInt($("select:eq(0)").val());
                    } else {
                        regionId = 643;
                    }
                    spName = $("[id$='txtSpName']").val();
                    $(".iframe").attr('src', 'EMSLeftData.aspx?regionId=' + regionId + '&keyword=' + spName);
                    $("#dataDetailul").empty();
                    $('#MapContent').html('');
                    $.ajax({
                        url: "/MapHandle.aspx",
                        type: 'post',
                        dataType: 'json',
                        async: false,
                        timeout: 10000,
                        data: { Action: "GetSpInfo", spName: $("[id$='txtSpName']").val(), regionId: regionId },
                        success: function (resultData) {
                            $('#MapContent').empty();
                            var option = InitMapOption();
                            option.SearchCity = '北京市';
                            if (resultData.count > 0) {//有商家数据
                                $("#resultCount").text(resultData.count);
                                //                                for (var i = 0; i < resultData.count; i++) {
                                //                                   // $("#dataDetailul").append("<li class='datali'> <div class='datadiv'><p>" + resultData.spInfos[i].name + "</p><p>" + resultData.spInfos[i].shopName + "</p><p>电话：" + resultData.spInfos[i].phone + "</p></div></li>");
                                //                                }
                                option.MarkerIcon = "/Admin/Images/mapMark.png";
                                option.Markers = LoadMakers(resultData.spInfos);
                            }
                            baidumap.baidumapload(option);
                            $('.MapDiv').show();
                        },
                        error: function (xmlHttpRequest, textStatus, errorThrown) {
                            alert(xmlHttpRequest.responseText);
                        }
                    });

                    $("#iframe").contents().find(".hdValue").die('click').live('click', function () {
                        alert('Enter');
                    });

                    $("#iframe").contents().find(".hdValue").die('mouseenter').live('mouseenter', function () {
                        alert('Enter');
                    });
                    $("#iframe").contents().find(".hdValue").die('mouseleave').live('mouseleave', function () {
                        alert('Leave');
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

                $("#mouseEnter").change(function () {
                    alert($(this).text());
                });
            });
</script>
 <YSWL:ValidatorContainer runat="server" ID="ValidatorContainer" />
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceCheckright" runat="server">
</asp:Content>
