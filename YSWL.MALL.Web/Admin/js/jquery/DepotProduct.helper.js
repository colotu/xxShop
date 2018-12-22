//(function () {
var baseAddTable = '<table id="dlstAddedProducts" cellspacing="0" border="0" style="width:96%;border-collapse:collapse;"><tbody></tbody></table>';
var baseDelTable = '<table id="dlstSearchProducts" cellspacing="0" border="0" style="width:96%;border-collapse:collapse;"><tbody></tbody></table>';

var highlightTime = 1500;
//动态效果函数
(function ($) {
    $.extend({
        add2cart: function (sender, target, text) {

            if (sender.length < 1) return;
            if (target.length < 1) return;

            var shadow = $('#' + sender.attr("id") + '_shadow');
            if (text == undefined || text == "") {
                text = "&nbsp;";
            }
            if (!shadow.attr('id')) {
                $('body').prepend('<div id="' + sender.attr("id") + '_shadow" style="display: none; background-color: #FFDA4D; border: solid 1px darkgray; position: static; top: 0px; z-index: 100000;">' + text + '</div>');
                var shadow = $('#' + sender.attr("id") + '_shadow');
            }
            if (!shadow) {
                alert('Cannot create the shadow div');
            }
            shadow.width(sender.css('width')).height(sender.css('height')).css('top', sender.offset().top).css('left', sender.offset().left).css('opacity', 0.5).show();
            shadow.css('position', 'absolute');

            //追加处理 目标高亮 取消JqueryUI
//            sender.hide('highlight', highlightTime);
//            target.show('highlight', highlightTime);

            shadow.animate({ width: target.innerWidth(), height: target.innerHeight(), top: target.offset().top, left: target.offset().left }, { duration: 400 })
                .animate({ opacity: 0 }, {
                    duration: 300,
                    complete: function () {
                        //                        target.queue(function() {
                        //                            $(this).dequeue();
                        //                        });

                        //追加处理 删除原对象/重置已选择SKU
                        sender.remove();
                        shadow.remove();
                    }
                });
        }
    });
})(jQuery);


var addedproductslist;
var searchproductslist;
$(document).ready(function () {
    addedproductslist = $(".addedproductslist");
    searchproductslist = $(".searchproductslist");

    $('.submit_add').bind('click', function () {
        InitAddProduct(this, addedproductslist);
        //新增 
        var currentTableTR = $(this).parents('tr:last');
        var currentTable = currentTableTR.find('table');
        var stock = currentTableTR.find('#i_stock').val();
        var sku = currentTableTR.find('#i_stock').attr('sku');
        insertProduct(sku, $('[id$="hiddepotId"]').val(), stock);
    });
    $('.submit_del').bind('click', function () {
        InitDelProduct(this, searchproductslist);
        RemoveProduct($(this).attr('sku'), $('[id$="hiddepotId"]').val());
    });

});

//新增
function insertProduct(sku, depotId, stock) {
    $.ajax({
        url: "/ShopManage.aspx",
        type: 'POST', dataType: 'json', timeout: 10000,
        data: { Action: "AddDepotProduct", Callback: "true", sku: sku, Depot: depotId, stock: stock },
        async: false,
        success: function (resultData) {
            if (resultData.STATUS == "OK") {
                return true;
            } else {
                alert("新增失败");
                //失败
            }
        }
    });
}
//删除
function RemoveProduct(sku, depotId) {
    $.ajax({
        url: "/ShopManage.aspx",
        type: 'POST', dataType: 'json', timeout: 10000,
        data: { Action: "DeleteDepotProduct", Callback: "true", sku: sku, depotId: depotId },
        async: false,
        success: function (resultData) {
            if (resultData.STATUS == "OK") {
                return true;
            } else {
                alert("删除失败");
                //失败
            }
        }
    });
}

function InitDelProduct(send, targetContext) {
    $(send).unbind('click'); //撤销事件, 防止恶意点击
    var currentTableTR = $(send).parents('tr:last');
    var currentTable = currentTableTR.find('table');
    var targetTableTR = currentTableTR.clone();

    var thisEvent = targetContext;
    targetTableTR.find('#span_stock').text('').hide();
    targetTableTR.find('#i_stock').val('9999').show();
    targetTableTR.find('.submit_del').removeClass('submit_del').addClass('submit_add').text('新增').bind('click', function() {
        InitAddProduct(this, addedproductslist);
    });

    if (targetContext.find('table').length == 0) {
        var tmp = $(baseAddTable);
        tmp.find('tbody').append(targetTableTR);
        targetContext.prepend(tmp);
    } else {
        $("[id$=dlstSearchProducts]").find('tbody:first').prepend(targetTableTR);
    }
    targetTableTR.queue(function() {
        //重置左侧滚动条到顶部
        searchproductslist.scrollTo(0, highlightTime / 2, { queue: false });
        //购物车效果
        $.add2cart(currentTableTR, targetTableTR, currentTableTR.html());
        $(this).dequeue();
    });
}

function InitAddProduct(send, targetContext) {
    $(send).unbind('click'); //撤销事件, 防止恶意点击
    var currentTableTR = $(send).parents('tr:last');
    var currentTable = currentTableTR.find('table');
    var targetTableTR = currentTableTR.clone();

    var thisEvent = targetContext;

    targetTableTR.find('#span_stock').text(targetTableTR.find('#i_stock').hide().val()).show();
    targetTableTR.find('.submit_add').removeClass('submit_add').addClass('submit_del').text('删除').bind('click', function() {
        InitDelProduct(this, searchproductslist);
    });

    if (targetContext.find('table').length == 0) {
        var tmp = $(baseAddTable);
        tmp.find('tbody').append(targetTableTR);
        targetContext.prepend(tmp);
    } else {
        $("[id$=dlstAddedProducts]").find('tbody:first').prepend(targetTableTR);
    }
    targetTableTR.queue(function() {
        //重置右侧滚动条到顶部
        addedproductslist.scrollTo(0, highlightTime / 2, { queue: false });
        //购物车效果
        $.add2cart(currentTableTR, targetTableTR, currentTableTR.html());
        $(this).dequeue();
    });
}



//} ());

