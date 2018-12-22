/*
* File:        SubmitOrder.js
* Author:      yaoyuan@ys56.com
* Copyright © 2004-2013 YSWL. All Rights Reserved.
*/
;


function Edit_Consignee(sender, addressId) {
    if (sender) $(sender).hide();
    var $target = $('#step-1');
    Status_Editing($target);
    $target.load($YSWL.BasePath + 'Order/AddressInfo', { addressId: addressId });
}

function Status_Editing(target) {
    $('.step').removeClass('step-current');
    target.addClass('step-current');
    target.find('.step-content').empty().append('<div class="step-loading"><div class="loading-style1" style="margin-bottom: 20px;"><b></b>正在加载中，请稍候...</div></div>');
    $('#mask_maticsoft').remove();
    $(document.body).append(
        ('<div id="mask_maticsoft" style="width: 100%; height: {0}px; position: absolute; top: 0px; left: 0px; z-index: 9998; opacity: 0.7; display: block;"></div>').format(
            $(document).height())
    );
}

function Status_None() {
    $('.step').removeClass('step-current');
    $('#mask_maticsoft').remove();
}

// 验证店铺编号
function checkvalidate() {
    var wdbhVal = $.trim($('#txtwdbh').val());
    $.ajax({
        url: "/UserCenter/ExistsWdbhfzr",
        type: 'post',
        dataType: 'json',
        timeout: 10000,
        async: false,
        data: {
            Action: "post",
            ShenghgName: wdbhVal,
        },
        success: function (JsonData) {
            switch (JsonData.STATUS) {
                case "EXISTS":
                    $("#txtWdbhTip").removeClass("msg msg-err").removeClass("msg msg-info").addClass("msg msg-ok msg-naked").html("<i class=\"msg-ico\"></i>" + JsonData.msg + "");
                    break;
                case "NOTshenghg":
                    $("#txtWdbhTip").removeClass("msg msg-ok msg-naked").removeClass("msg msg-info").addClass("msg msg-err").html("<i class=\"msg-ico\"></i>店铺编号不存在，VIP会员必须填写，普通会员可不填！");
                    break;
                case "PsfsNot":
                    $("#txtWdbhTip").removeClass("msg msg-ok msg-naked").removeClass("msg msg-info").addClass("msg msg-err").html("<i class=\"msg-ico\"></i>VIP会员必须填写店铺编号，普通会员可填也可不填！");
                    break;
                case "NOTEXISTS":
                    $("#txtWdbhTip").html("");
                    break;
                default:
                    ShowServerBusyTip("服务器没有返回数据，可能服务器忙，请稍候再试！");
                    break;
            }
        },
        error: function (XMLHttpRequest, textStatus, errorThrown) {
            ShowServerBusyTip("服务器没有返回数据，可能服务器忙，请稍候再试！");
        }
    });
}

//提交订单
function SubmitOrder(sender, shippingAddressId, paymentModeId, conpon, shipStr) {
    //    $.alert("此功能还在完善中, 敬请期待.");
    //    return;

    var skuArry = [];
    var skuId = $.getUrlParam('sku');
    var count = $.getUrlParam('count');
    var proSalesId = $.getUrlParam('c');
    var groupbuyId = $.getUrlParam('g');
    var acceId = $.getUrlParam('a');
    var refer = $.getUrlParam('r');
    var remark = $('#txtRemark').val();
    var wdbh = $('#txtwdbh').val();

    if (skuId) {
        var sku = {};
        sku.SKU = skuId;
        sku.Count = count ? count : 1;
        skuArry.push(sku);
    } else {
        skuArry = null;
    }

    var checkoutLoading = $('<span id="order-loading" class="checkout-state" style="float:none"><b></b>\u6B63\u5728\u63D0\u4EA4\u8BA2\u5355\uFF0C\u8BF7\u7A0D\u5019\uFF01</span>');
    var originSubmit = $("#order-submit").clone(true);
    //lock
    $(sender).fadeOut('slow', function () {
        $("#order-loading").replaceWith(originSubmit);
        //进入Jquery队列执行
        $(this).replaceWith(checkoutLoading).queue(function (next) {
            $.ajax({
                url: '/Pay/V2/OrderHandler.aspx',
                type: 'post',
                dataType: 'json',
                timeout: 0,
                async: true,
                data: { Action: "SubmitOrder",
                    ShippingAddressId: shippingAddressId,
                    PaymentModeId: paymentModeId,
                    Coupon: conpon,
                    ProSaleId: proSalesId,
                    GroupBuyId: groupbuyId,
                    AcceId: acceId,
                    SkuInfos: skuArry ? JSON.stringify(skuArry) : null,
                    ShipStr: shipStr,
                    Refer: refer,
                    Remark: remark,
                    Wdbh: wdbh,
                },
                success: function (resultData) {
                    var isOK = false;
                    switch (resultData.STATUS) {
                        // 提交订单成功  
                        case "SUCCESS":
                            isOK = true;
                            //延迟两秒后跳转
                            $("#order-loading").html("<b></b>订单提提交成功, 请稍后..").animate({ opacity: 1.0 }, 1000).fadeOut("slow", function () {
                                //DONE: 货到付款/银行汇款 跳转 BEN MODIFY 20131205
                                if (resultData.GATEWAY == 'cod' || resultData.GATEWAY == 'bank') {
                                    window.location.replace('/pay/certification' + resultData.DATA.OrderId + '/' + $YSWL.CurrentArea);
                                } else {
                                    window.location.replace($YSWL.BasePath + 'Order/SubmitSuccess/' + resultData.DATA.OrderId);
                                }
                            });
                            break;
                        case "GROUPBUYREACHEDMAX": //团购已达上限
                        case "NOSTOCK":
                            $.alertEx('很抱歉.<br/><br/><br/>您购买的部分商品已经被其TA人抢先下单了, <br/> 确认后返回购物车.', function () {
                                $.navURL($YSWL.BasePath + 'ShoppingCart/CartInfo');
                            });
                            break;
                        case "NOSHOPPINGCARTINFO":
                            $.alertEx('您的购物车是空的, 请加入商品后提交订单!', function () {
                                $.navURL($YSWL.BasePath + 'ShoppingCart/CartInfo');
                            });
                            break;
                        case "PROSALEEXPIRED":
                            $.alertEx('非常抱歉, 该活动已过期!', function () {
                                window.location.replace($YSWL.BasePath + 'Product/ProSaleDetail/' + $.getUrlParam('c'));
                            });
                            break;
                        case "NOLOGIN":
                            // 用户未登陆
                            $.alertEx('您还没有登陆或者登陆已超时，请您登陆后提交订单．', function () {
                                $.navURL($YSWL.BasePath + 'Account/Login?return=' + encodeURIComponent(window.location.href));
                            });
                            break;
                        case "UNAUTHORIZED":
                            // 权限不足
                            $("#submit_message").html("只有普通用户才可以提交订单喔, 请您登陆普通用户再提交订单.");
                            $("#submit_message").show();
                            break;
                        case "NOTCANUSECOUPON":
                            ShowFailTip('不能使用优惠劵');
                            break;
                        case "OUTGROUPBUYREGION":
                            //收货地区不在团购地区范围内
                            ShowFailTip('收货地区不在团购地区范围内,请修改收货地区');
                            break;
                        case "INVALID"://已失效
                            $.alertEx('很抱歉.<br/><br/><br/>您购买的部分商品已失效, <br/> 确认后返回购物车.', function () {
                                $.navURL($YSWL.BasePath + 'ShoppingCart/CartInfo');
                            });
                            break;
                        default:
                            // 抛出异常消息
                            $.alertError(resultData.STATUS);
                            break;
                    }
                    if (!isOK) {
                        $("#order-loading").fadeOut('slow', function () {
                            $("#order-loading").replaceWith(originSubmit);
                            $(sender).fadeIn('slow');
                        });
                    }
                },

                error: function (xmlHttpRequest, textStatus, errorThrown) {
                    if (textStatus != 'timeout') {
                        $.alertError(xmlHttpRequest.responseText);
                    } else {
                        $("#submit_message").html("噗, 您的网络忒慢了! 访问服务器超时了, 请再试一下!");
                    }
                    $("#order-loading").replaceWith(originSubmit);
                    $("#submit_message").show();
                    $(sender).fadeIn('slow');
                }
            });

            next();
        });
    });
}

