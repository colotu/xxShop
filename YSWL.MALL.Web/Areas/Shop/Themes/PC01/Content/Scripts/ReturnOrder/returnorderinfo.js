/**
* $itemname$.js
*
* 功 能： [N/A]
*
* Ver    变更日期             负责人  变更内容
* ───────────────────────────────────
* V0.01  $time$  $username$    初版
*
* Copyright (c) $year$ YS56 Corporation. All rights reserved.
*┌──────────────────────────────────┐
*│　此技术信息为本公司机密信息，未经本公司书面同意禁止向第三方披露．　│
*│　版权所有：小鸟科技（北京）科技有限公司　　　　　　　　　　　　　　│
*└──────────────────────────────────┘
*/

$(function () {
    var mainstates = $.trim($('#hidMainstates').val());
    var status = parseInt($('#hidStatus').val());
    var logisticstates = parseInt($('#hidLogisticstates').val());
    if (mainstates != '已取消' && mainstates != '审核未通过') {
        $('#process').show();
    } else if (mainstates == '审核未通过') {
        $('#refuseReason').show();
    }

    if (status > 0 && logisticstates == 0) { //不需要取货
        $('#process').find('div').eq(3).hide();
        $('#process').find('div').eq(4).hide();
    }

    switch (mainstates) {
        case "等待审核":
            $('.ftx14').text('等待审核');
            break;
        case "已取消":
            $('.ftx14').text('已取消');
            break;
        case "审核未通过":
            $('.ftx14').text('审核未通过');
            break;
        case "正在处理":
        case "取货中":
        case "返程中": //取货中
            $('#process').find('div').eq(3).removeClass('wait').addClass('ready');
            $('#process').find('div').eq(4).removeClass('wait').addClass('ready');
            $('.ftx14').text('取货中');
            break;
        case "等待退款":
        case "退款中": //待退款
            $('#process').find('div').eq(3).removeClass('wait').addClass('ready');
            $('#process').find('div').eq(4).removeClass('wait').addClass('ready');
            $('#process').find('div').eq(5).removeClass('wait').addClass('ready');
            $('#process').find('div').eq(6).removeClass('wait').addClass('ready');
            $('.ftx14').text('等待退款');
            break;
        case "已完成": //完成
            $('#process').find('div').eq(3).removeClass('wait').addClass('ready');
            $('#process').find('div').eq(4).removeClass('wait').addClass('ready');
            $('#process').find('div').eq(5).removeClass('wait').addClass('ready');
            $('#process').find('div').eq(6).removeClass('wait').addClass('ready');
            $('#process').find('div').eq(7).removeClass('wait').addClass('ready');
            $('#process').find('div').eq(8).removeClass('wait').addClass('ready');
            $('.ftx14').text('已完成');
            break;
        case "未知状态":
            break;
        default:
            break;

    }

    //case "等待付款确认":   
    // case "订单锁定":
    // case "取消订单": 
});
 