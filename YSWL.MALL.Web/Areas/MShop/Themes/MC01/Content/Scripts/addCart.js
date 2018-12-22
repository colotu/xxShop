$(function() {
    //加入购物车操作
    $(document).on('click','#btnAddToCart',function () {
                if ($(this).hasClass('addShop-n')) return false;
                if (!$(this).attr('itemid')) {
                    $('#SKUOptions,#SKUOptions a').effect('highlight', 500);
                    ShowFailTip('请选择规格！');
                    return false;
                }
                var count = parseInt($("#productCount").val());
                if (isNaN(count) || count <= 0) {
                    count = 1;
                }

                //未开启sku时的判断库存
                if ($('#hdHasSKU').val().toLocaleLowerCase() === "false") {
                    var stock = parseInt($('#hdprodSku').attr('stock'));
                    if (stock < count) {
                        ShowFailTip('库存不足');
                        return false;
                    }
                } else { //开启sku时的判断库存
                    if ($('#stock_num').length > 0) {
                        var stock_num = parseInt($('#stock_num').text());
                        if (isNaN(stock_num) || stock_num <= 0 || stock_num < count) {
                            ShowFailTip('库存不足');
                            return false;
                        }
                    }
                }
              
                if ($('#hidBuyMode').val() && $('#hidBuyMode').val() === "BuyNow") {
                    //立刻购买
                    location.href = $YSWL.BasePath +"Order/SubmitOrder?sku=" +$(this).attr('itemid') +"&Count=" +count +"&r=" +$.getUrlParam("r");
                } else {
                    //ajax加入购物车
                    $.ajax({
                        type: "POST",
                        dataType: "json",
                        async: false,
                        url: $YSWL.BasePath + "ShoppingCart/AddCart?s=" + new Date().format('yyyyMMddhhmmssS'),
                        data: { Sku: $(this).attr('itemid'), Count: count },
                        success: function (resultData) {
                            switch (resultData.STATUS) {
                                case "SUCCESS":
                                    //隐藏弹层
                                    $("#bg,#addCart-Lay").hide();
                                    showAddCartSuccessTip();
                                    //重新获取购物车商品数量
                                    //getCartCount($('#shoppingCount'));
                                    return false;
                                case "FAILED":
                                    switch (resultData.DATA) {
                                        case "NOSTOCK":
                                            ShowFailTip("库存不足！");
                                            return false;
                                        case "NOSKU":
                                        case "NO":
                                        default:
                                            ShowFailTip("请稍候再试~");
                                            return false;
                                    }
                                default:
                                    ShowFailTip("请稍候再试~");
                                    return false;
                            }
                        }
                    });
                }
            });


    //加
    $(document).on('click', '#plus', function () {
        var count = parseInt($("#productCount").val()) + 1;
        $("#productCount").val(count);
    });

    //减
    $(document).on('click', '#subtract', function () {
        var count = parseInt($("#productCount").val());
        if (count > 1) {
            count = count - 1;
        }
        $("#productCount").val(count);
    });


    // 点击 无货图片时 提示无货
    $(".none").on("click", function () {
        $(".no-shop").css({
            left: ($(window).width() - $(".no-shop").width()) / 2 + "px",
            top: ($(window).height() - $(".no-shop").height()) / 2 + $(window).scrollTop() + "px"
        });
        $(".no-shop").fadeIn(200);
        setTimeout(function () {
            $(".no-shop").fadeOut(500);
        }, 2000);
    });


});

//显示加入购物车提示
function showAddCartSuccessTip(){
    //
    $(".addSuccess")
        .css({
            //设置弹出层距离左边的位置
            left: ($(window).width() - $(".addSuccess").width()) / 2 + "px",
            //设置弹出层距离上面的位置
            top: ($(window).height() - $(".addSuccess").height()) / 2 + $(window).scrollTop() + "px"
            //,display: "block"
        });
    $(".addSuccess").fadeIn(200);
    setTimeout(function () {
        $(".addSuccess").fadeOut(500);
    },
        2000);
}


//显示加入购物车弹窗页面
function dialogShow(productId) {
    //保持上次点击的值
    if (parseInt($('#addCart-Lay').attr('currpid')) === productId) {
        $('#addCart-Lay').show();
        $("#bg").css({ height: $(document).height() }).show();
        return true;
    }

    $('#addCart-Lay').load($YSWL.BasePath + 'Partial/AddCart', { productId: productId }, function () {
                $('#addCart-Lay').attr('currpid', productId).show();
                $("#bg").css({ height: $(document).height() }).show();
                $('html').css('overflow-y', 'hidden'); //隐藏页面滚动条

                //重置js sku数据
                InitializationSku();
    });
};

//隐藏窗体
function dialoghide() {
    $('html').css('overflow-y', 'auto'); //恢复页面滚动条
    $('#bg,#addCart-Lay').hide();
}
 