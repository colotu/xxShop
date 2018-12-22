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

    //显示筛选内容
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
            $('#filter_content').load($YSWL.BasePath + "s/f/" + $('#hidCid').val() + "/" + $('#se_keyword').text(), function () {
                showCurrCateName();
                showFilter();
                $("#bgloading,#bg").hide();
                 $('#filter_content').attr('isload', true);
                pullUp.Load = false;
            });
    });

    //删除关键词
    $("#delky").on('click', function (event) {
        $('#txtKeyWord').val('');
        $("#search_page").addClass('dsb');
        $('#main_page').addClass('dsn').removeClass('dsb');
        event.stopPropagation();
    });
    

    //筛选

    //关闭筛选
    $(document).on('click', '#filter-back,#f_black_overlay', function () {
        closeFilter();
    });

    //点击一级筛选页面的  全部分类  二级筛选页面出来
    $(document).on('click', '#all_into', function () {
        $("#pop1 .header_title").hide();
        $("#pop2").show().animate({ right: "0" });
    });

    //点击一级分类  
    $(document).on('click', '#pop2 .firstCategory', function (e) {
        var e = window.event || event;
        if (e.stopPropagation) {
            e.stopPropagation();
        }else{
            e.cancelBubble = true;
        }
        //对应二级分类展开或收起
        $('#secondCate_' + $(this).attr('cid')).slideToggle();
    });

    //分类页面返回按钮
    $(document).on('click', '#pop2 .icon_goback', function () {
        $("#pop1 .header_title").show();
        $("#pop2").hide();
    });

    //点击二级分类
    $(document).on('click', '.cateValues', function () {
        $('.cateValues.active').not(this).removeClass('active'); //清除已选中的 
        $(this).toggleClass('active');//选中效果
        if (!$(this).hasClass('active')){
            return false;
        }
        $('#cateSelect-Tip').text($('.cateValues.active span').text());
        $("#pop1 .header_title").show();
        $("#pop2").hide();
    });

    //重置筛选条件
    $(document).on('click', '.btn_reset', function(){
        $("#hfBrand").val(0);
        $("#hidCid").val(0);
        $('.cateValues.active').removeClass('active');
        $('#cateSelect-Tip').text('全部');
        $('#txtPrice1').val('');
        $('#txtPrice2').val('');
        $("#catelist span.tag_a").removeClass("tag_a");
        $("#catelist [cid='0'] span").addClass("tag_a");
        $("#brandlist .brandValues span.tag_a").removeClass("tag_a");
        $("#brandlist a[BrandId='0'] span").addClass("tag_a");
        $('#filter_brand_tip').text("全部");
        $('#filter_cate_tip').text("全部");
    });

    //选中品牌值
    $(document).on('click', '.brandValues', function () {
        $("#brandlist").find("span.tag_a").removeClass("tag_a");
        $(this).find("span").addClass("tag_a");
        $('#filter_brand_tip').text($(this).find("span").text());
        $("#hfBrand").val($(this).attr("BrandId"));
    });

    //应用本次筛选条件
    $(document).on('click', '#btn-goset', function () {
        closeFilter();
        if ($('.cateValues.active').length > 0){
            //记录选中的分类
            $('#hidCid').val($('.cateValues.active').attr('cid'));
        }else{
            //无选中的分类
            $('#hidCid').val('0');
        }
        $('#mainProductList').load(getProductUrl(), function () {
            $('#mainProductList').show();
            $('#loading').hide();
        });
    });
  
});

var getProductUrl = function () {
    var url = $YSWL.BasePath + "s/{0}/{1}/{2}/{3}/{4}?{5}";
    var brandId = 0;//品牌
    if ($("#hfBrand").length > 0) {
       if ($("#hfBrand").val() != '') {
            brandId = $("#hfBrand").val();
        }
    }
    var kw = $('#se_keyword').text();
    var param = "";
    if ($('#switch').hasClass('icon_switch_view')) {
        param = "ajaxViewName=_ProdList2";//列表2
    } else {
        param = "ajaxViewName=_ProdList";//列表1
    }
    var priceStr = '0-0';
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
    return url.format($('#hidCid').val(), brandId, $('.modList.sort_a').attr("mod"), priceStr, kw, param);
}

//关闭筛选
function closeFilter() {
    $("#pop1,#pop2").animate({ right: "-90%" }, function () {
        $("#f_black_overlay,#pop1").hide();
    });
    $("body,html").css({ "overflow": "auto", "height": "auto" });
    //$("body").removeClass('overflow-h');
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

function showCurrCateName() {
    var cateId =parseInt($('#hidCid').val());
    if (cateId > 0) {
        $('#cateSelect-Tip').text($('#hidCateName').val());
    } 
}