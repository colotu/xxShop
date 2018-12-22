/*
* File:        SubmitOrder.js
* Author:      yaoyuan@ys56.com
* Copyright © 2004-2013 YSWL. All Rights Reserved.
*/
;


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
function SubmitOrder(sender, shippingAddressId, paymentModeId, conpon, remark, shipStr) {
    //    $.alert("此功能还在完善中, 敬请期待.");
    //    return;

    var skuArry = [];
    var skuId = $('#SkuInfo').val();
    var count = $('#SkuCount').val();
    var proSaleId = $('#ProSale').val();
    var groupBuyId = $('#GroupBuy').val();
    var refer = $.getUrlParam('r');
    var wdbh = $('#txtwdbh').val();

    if (skuId) {
        var sku = {};
        sku.SKU = skuId;
        sku.Count = count ? count : 1;
        skuArry.push(sku);
    } else {
        skuArry = null;
    }

    var checkoutLoading = $('<span id="order-loading" class="checkout-state"><b></b>\u6B63\u5728\u63D0\u4EA4\u8BA2\u5355\uFF0C\u8BF7\u7A0D\u5019\uFF01</span>');
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
                    ProSaleId: proSaleId,
                    GroupBuyId: groupBuyId,
                    Refer: refer,
                    ShipStr: shipStr,
                    SkuInfos: skuArry ? JSON.stringify(skuArry) : null,
                    Remark: remark,
                    Wdbh: wdbh,
                },
                success: function (resultData) {
                    var isOK = false;
                    switch (resultData.STATUS) {
                        // 提交订单成功  
                        case "SUCCESS":
                            isOK = true;
                            //清空一下cooke 
                            $.cookie('m_so_code', "", { expires: -1, path: '/' });
                            $.cookie('m_so_payId', "", { expires: -1, path: '/' });
                            $.cookie('m_so_shipId', "", { expires: -1, path: '/' }); 
                            $.cookie('m_so_remark', "", { expires: -1, path: '/' }); 
                            //延迟两秒后跳转
                            $("#order-loading").html("<b></b>订单提交成功, 请稍后..").animate({ opacity: 1.0 }, 1000).fadeOut("slow", function () {
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
                            ShowConfirm("很抱歉.您购买的部分商品已经被其TA人抢先下单了, 确认后返回购物车.", function () {
                                $.navURL($YSWL.BasePath + 'ShoppingCart/CartInfo');
                            });
                            break;
                        case "NOSHOPPINGCARTINFO":
                            ShowConfirm("您的购物车是空的, 请加入商品后提交订单!", function () {
                                $.navURL($YSWL.BasePath + 'ShoppingCart/CartInfo');
                            });
                            break;
                        case "NOLOGIN":
                            // 用户未登陆
                            ShowConfirm("您还没有登陆或者登陆已超时，请您登陆后提交订单．", function () {
                                $.navURL($YSWL.BasePath + 'Account/Login?return=' + encodeURIComponent(window.location.href));
                            });
                            break;
                        case "UNAUTHORIZED":
                            // 权限不足
                            ShowFailTip('只有普通用户才可以提交订单喔, 请您登陆普通用户再提交订单.');
                            break; 
                        case "NOTCANUSECOUPON":
                            ShowFailTip('不能使用优惠劵');
                            break;
                        case "OUTGROUPBUYREGION":
                            //收货地区不在团购地区范围内
                            ShowFailTip('收货地区不在团购地区范围内,请修改收货地区');
                            break;
                        case "INVALID": //已失效
                            ShowConfirm("很抱歉. 您购买的部分商品已失效, 确认后返回购物车.", function () {
                                $.navURL($YSWL.BasePath + 'ShoppingCart/CartInfo');
                            });
                            break;
                        default:
                            // 抛出异常消息
                            ShowFailTip(resultData.STATUS);
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
                        ShowConfirm(xmlHttpRequest.responseText);
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

