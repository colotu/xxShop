
$(function () {
    var orderstates = $('#hidorderstates').val().toString();
    switch (orderstates) {
        case "等待付款":
            //去付款 
            $('.ftx14').text('等待付款');
            var orderid = $('#hidorderid').val();
            if (orderid) {
                $('#pay-button').prepend('<a  href="/pay/certification' + orderid + '/' + $YSWL.CurrentArea + '"  > <img  src="/Areas/Shop/Themes/M3/Content/images/btn_pay.gif" width="46" height="25" style="display: inline;"> </a>');
            }
            break;
        case "正在处理":
        case "等待处理":
        case "配货中": //商品出库
            $('#process').find('div').eq(3).removeClass('wait').addClass('ready');
            $('#process').find('div').eq(4).removeClass('wait').addClass('ready');
            $('#pay-button').empty();
            $('.ftx14').text('商品出库');
            break;
        case "已发货": //等待收货
            $('#process').find('div').eq(3).removeClass('wait').addClass('ready');
            $('#process').find('div').eq(4).removeClass('wait').addClass('ready');
            $('#process').find('div').eq(5).removeClass('wait').addClass('ready');
            $('#process').find('div').eq(6).removeClass('wait').addClass('ready');
            $('.ftx14').text('已发货');
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
 