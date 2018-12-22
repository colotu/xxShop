$(function () {
    //切换列表样式
    $('#switch').on('click', function () {
        $("#bgloading,#bg").css({ height: $(window).height(), display: 'block' });
        $('#mainProductList').hide();
        if ($(this).hasClass('icon_switch_view')) {
            $(this).removeClass('icon_switch_view').addClass('icon_switch2_view');
        } else {
            $(this).removeClass('icon_switch2_view').addClass('icon_switch_view');
        }
        $('#mainProductList').load(getProductUrl(), function () {
            $("#bgloading,#bg").hide();
            $('#mainProductList').show();
        });
    });

    //排序方式切换
    $('.modList').on('click', function () {
        if ($('#filter span').hasClass('icon_up')) {
            //关闭筛选
            closeFilter();
        }
        if ($(this).attr('item') == "price") {//是价格
            if ($(this).attr('mod') != "price") {
                $(this).attr('mod', 'price').find('span').addClass('icon_sort_up').removeClass('icon_sort_down');//价格升序
            } else {
                $(this).attr('mod', 'pricedesc').find('span').addClass('icon_sort_down').removeClass('icon_sort_up');//价格降序
            }
        } else {
            //点击自己保持不变
            if ($(this).hasClass('sort_a')) {
                return;
            }
            //重置价格样式为灰色
            $('#li_price .icon_sort').removeClass('icon_sort_down').removeClass('icon_sort_up');
        }
        $('.modList').removeClass('sort_a');
        $(this).addClass('sort_a');
        $("#bgloading,#bg").css({ height: $(window).height(), display: 'block' });
        $('#mainProductList').load(getProductUrl(), function () {
            $("#bgloading,#bg").hide();
            $('#mainProductList').show();
        });
    });

    //点击筛选按钮 显示筛选页面
    $('#filter').on('click', function (e) {
            var e = window.event || event;
            if (e.stopPropagation) {
                e.stopPropagation();
            } else {
                e.cancelBubble = true;
            }           
            //判断是否已加载  如果已加载过，则直接显示
            if ($('#filter_content').attr('isload') != "false") {
                showFilter();
                pullUp.Load = false;
                return;
            }
            //第一次点击  请求服务器加载数据
            //显示 loading刷新
            $("#bgloading,#bg").css({ height: $(window).height(), display: 'block' });
            $('#filter_content').load($YSWL.BasePath + "product/Filter/" + $('#hidCid').val(), function () {
                showFilter();
                $("#bgloading,#bg").hide();
                $('#filter_content').attr('isload', true);//.show();
               pullUp.Load = false;
            });
    });

    //关闭筛选
    $(document).on('click', '#filter-back,#f_black_overlay', function () {
        closeFilter();
    });
   
});

var getProductUrl = function () {
    var url = $YSWL.BasePath + "p/{0}/{1}/{2}/{3}{4}?{5}";
    var brandId = 0;//品牌
    if ($("#hfBrand").length > 0) {
        if ($("#hfBrand").val() != '') {
            brandId = $("#hfBrand").val();
        }
    }
    var attrValue = "0";//属性
    if ($("#hfAttrValue").length > 0) {
        if ($("#hfAttrValue").val() != '') {
            attrValue = $("#hfAttrValue").val();
        }
    }
    var param = "";
    if ($('#switch').hasClass('icon_switch_view')) {
        param="ajaxViewName=_ProdList2";//列表2
    } else {
        param="ajaxViewName=_ProdList";//列表1
    }
    var priceStr = '';
    if ($('#txtPrice1').length > 0 && $('#txtPrice2').length > 0) {
        var price1 = parseInt($("#txtPrice1").val());
        var price2 = parseInt($("#txtPrice2").val());
        if (isNaN(price1)) {
            price1 = 0;
        }
        if (isNaN(price2)) {
            price2 = 0;
        }
        if (price1 > price2 && price1 > 0 && price2 > 0) {
            var sw = price1;
            price1 = price2;
            price2 = sw;
            $("#txtPrice1").val(price1);
            $("#txtPrice2").val(price2);
        }
        priceStr ='/'+price1 + '-' + price2;
    }
    return url.format($('#hidCid').val(), brandId, attrValue, $('.modList.sort_a').attr("mod"),priceStr, param);
}

//关闭筛选
function closeFilter() {
    $("#pop1,#pop2").animate({ right: "-90%" }, function () {
        $("#f_black_overlay,#pop1").hide();
    });
    $("body,html").css({ "overflow": "auto" , "height": "auto" });
    pullUp.Load = true;
}
 
function showFilter() {
    $("#f_black_overlay,#pop1").show();
    $("#pop1").animate({ right: "0" });
    $("body,html").css({ "overflow": "hidden", "height": $(window).height() });
    //页面内容超过一屏时  底部按钮hold住
    holdBottomButton($('.cover-wrap'), $('.btn_wrapper'), 'hold2-fixed');
    $("#pop1 .header_title").show();
}