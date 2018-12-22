$(function () {
    resizeImg('.jqzoom', 350, 350);

    //放大镜
    $(".jqzoom").jqueryzoom({ xzoom: 300, yzoom: 300, offset: 10, position: "right", preload: 1, lens: 1 });

    //评论图片
    $.scaleFixedLoad('.detail_proimage', 100, 100);

    //左右轮换
    qh();

    //优惠套装切换
    $('#special-pack #ul_detail_list li').click(function () {
        $(this).siblings().removeClass('on');
        $(this).addClass('on');
        $('#special-pack .div_access_item').hide();
        $('#special-pack #' + $(this).attr('item')).show();
    });

    //组合配件切换
    $('#parts-with #parts_suit li').click(function () {
        $(this).siblings().removeClass('on');
        $(this).addClass('on');
        $('#parts-with .div_access_item').hide();
        $('#parts-with #' + $(this).attr('item')).show();
    });

    //配件 加入购物车
    $(".acce-cart-btn").click(function () {
        if (!$(this).attr('itemid')) {
            return false;
        }
        location.href = $YSWL.BasePath + "ShoppingCart/AddCart?sku=" + $(this).attr('itemid');
    });
   
    //检测商品是否已加入收藏
    IsAddedFav('#btnProductFav', $('#hdProductId').val(), 1);

    //收藏操作
    $("#btnProductFav").click(function () {
        if (CheckUserState()) {
            var productId = $(this).attr("productId");
            $.ajax({
                type: "POST",
                dataType: "text",
                url: $YSWL.BasePath + "UserCenter/AjaxAddFav",
                async: false,
                data: { ProductId: productId },
                success: function (data) {
                    if (data == "Rep") {
                        $('#btnProductFav span').removeClass('icon-hollow-favor').addClass('icon-collect');
                        $('#btnProductFav em').text('已收藏');
                        ShowSuccessTip('您已经收藏了该商品，请不要重复收藏');
                    } else if (data == "True") {
                        $('#btnProductFav span').removeClass('icon-hollow-favor').addClass('icon-collect');
                        $('#btnProductFav em').text('已收藏');
                    } else {
                        ShowFailTip('服务器繁忙，请稍候再试！');
                    }
                }
            });
        }
    });


    $("#id-goods-info").click(function () {//商品详情
        $("#product-detail").show();
        $("#LoadData").empty();
        $(this).addClass("current").siblings().removeClass("current");
    });

    $("#id-buyer-comment").click(function () {//顾客评论
        $("#LoadData").html("<div style='margin:0 auto;text-align: center;'><img src='/Areas/Shop/Themes/PC01/Content/images/loads.gif' ></div>");
        $("#LoadData").load($YSWL.BasePath + "Product/ProductComments/" + $('#hdProductId').val(), function () {
            $("[data-pagerid='Webdiyer.MvcPager']").initMvcPagers();//调用ajax分页方法
            $("#product-detail").hide();
            $("#id-buyer-comment").addClass("current").siblings().removeClass("current");
            encryption($('.td_buyname'));
        });

    });
    
    $('#wholeSalePanel .wholeSaleTip-list').click(function () {
        $("#wholeSalePanel .wholeSaleTip-list .act-tag").toggleClass('hidden');
        $("#wholeSalePanel .wholeSaleTip-list .showTiptxt").toggleClass('hidden');
        $('#wholeSale_Expand').toggleClass('icon-ugray');
        $("#wholeSalePanel .listItem").toggleClass('hidden');
    });

    $('#activityListPanel .activityTip-list').click(function () {
        $("#activityListPanel .activityTip-list .act-tag").toggleClass('hidden');
        $("#activityListPanel .activityTip-list .showTiptxt").toggleClass('hidden');
        $('#activity_Expand').toggleClass('icon-ugray');
        $("#activityListPanel .listItem").toggleClass('hidden');
    });

})

//判断是否含有禁用词
function ContainsDisWords(desc) {
    var isContain = false;
    $.ajax({
        url: "/Partial/ContainsDisWords",
        type: 'post', dataType: 'text', timeout: 10000,
        async: false,
        data: { Desc: desc },
        success: function (resultData) {
            if (resultData == "True") {
                isContain = true;
            }
        },
        error: function (XMLHttpRequest, textStatus, errorThrown) {
            ShowFailTip("操作失败：" + errorThrown);
        }
    });
    return isContain;
}

var CheckUserState4UserType = function () {
    var islogin;
    $.ajax({
        url: "/User/CheckUserState4UserType",
        type: 'post',
        dataType: 'text',
        timeout: 10000,
        async: false,
        success: function (resultData) {
            if (resultData == "Yes") {
                islogin = true;
                return true;
            } else if (resultData == "Yes4AA") {
                $.jBox.tip('管理员不能操作, 请您更换普通帐号再试!');
                $(".jbox-button").hide();
                islogin = false;
                return false;
            } else {

                islogin = false;
                return false;
            }
        },
        error: function (XMLHttpRequest, textStatus, errorThrown) {

        }
    });

    return islogin;
};
var CheckUserLogin = function () {
    var islogin;
    $.ajax({
        url: "/User/CheckUserState",
        type: 'post',
        dataType: 'text',
        timeout: 10000,
        async: false,
        success: function (resultData) {
            if (resultData != "Yes") {
                islogin = false;
                return false;
            } else {
                islogin = true;
                return true;

            }
        },
        error: function (XMLHttpRequest, textStatus, errorThrown) {

        }
    });
    return islogin;
};

$(function () {
    //商品评论
    $(".btnAddComment").die("click").live("click", function () {
        if (CheckUserState()) {
            var dialogOpts = {
                title: "商品评论",
                width: 400,
                modal: true,
                buttons: {
                    "确定": function () {
                        submitAjaxAddComment();
                    }
                    //                        "取消": function () {
                    //                            //  $(this).dialog("close"); //关闭层
                    //                            $("#divAjaxComments").dialog("close");
                    //                        }

                }
            };
            $("#divAjaxComments").dialog(dialogOpts);
        }
    });

});

function submitAjaxAddConsult() {
    var productId = $("#hdProductId").val();
    var content = $("#txtConsult").val();
    if (content == "") {
        ShowFailTip('请填写咨询内容！');
        return;
    }
    if (ContainsDisWords(content)) {
        ShowFailTip('您输入的内容含有禁用词，请重新输入！');
        return;
    }
    $.ajax({
        type: "POST",
        dataType: "text",
        url: $YSWL.BasePath +"UserCenter/AjaxAddConsult",
        data: { ProductId: productId, Content: content },
        success: function (data) {
            if (data == "True") {
                ShowSuccessTip('咨询成功！请等待管理员回复');
                $("#divAjaxConsults").dialog("close");
                $(".ui-dialog").empty();
            } else {
                ShowFailTip('服务器繁忙，请稍候再试！');
            }
        }
    });
}

function submitAjaxAddComment() {
    var productId = $("#hdProductId").val();
    var productName = $("#hdProductName").val();
    var content = $("#txtComment").val();
    if (content == "") {
        ShowFailTip('请填写评论内容！');
        return;
    }
    if (ContainsDisWords(content)) {
        ShowFailTip('您输入的内容含有禁用词，请重新输入！');
        return;
    }
    $.ajax({
        type: "POST",
        dataType: "text",
        url: $YSWL.BasePath +"UserCenter/AjaxAddComment",
        data: { ProductId: productId, Content: content, ProductName: productName },
        success: function (data) {
            if (data == "True") {
                ShowSuccessTip('评论成功!');
                $("#divAjaxComments").dialog("close");
                $(".ui-dialog").empty();
            } else {
                ShowFailTip('服务器繁忙，请稍候再试！');
            }
        }
    });
}



//加密用户名
function encryption($userNameList) {
    $userNameList.each(function () {
        var self = $(this);
        var self_length = self.text().trim().length;
        var self_text = self.text().trim();
        if (self_length >= 7) {
            self.text(self_text.substring(0, 3) + "****" + self_text.substring(7, self_length));
        } else if (self_length > 3) {
            self.text(self_text.substring(0, 2) + "****");
        } else {
            self.text('****');
        }
        self.show();
    });
}

//左右轮换
function qh() {
    var now = 0;
    var len = $('#lbtp li').length;
    $('#next').click(function () {
        if (now == len - 1) {
            now = 0;
        } else {
            now++;
        }
        tshow(now);
    });

    $('#prev').click(function () {
        if (now == 0) {
            now = len - 1;
        } else {
            now--;
        }
        tshow(now);
    });
    $('#lbtp li').click(function () {
        now = $('#lbtp li').index(this);
        tshow(now);
    });
    function tshow(i) {
        $('#lbtp li').removeClass('img-hover').eq(i).addClass('img-hover');
        $("#samllImg").attr({ jqimg: $('#lbtp li img').eq(i).attr('jqimg'), src:  $('#lbtp li img').eq(i).attr('src').replace('T88X88_', 'T350X350_') });
    }
}
 
//是否添加过收藏
function IsAddedFav(_self, targetId, type) {
    if (!CheckLogin()) {
        //未登录 返回
        return;
    }
    $.ajax({
        type: "POST",
        dataType: "text",
        url: $YSWL.BasePath + "UserCenter/IsAddedFav",
        async: false,
        data: { id: targetId, type: type },
        success: function (data) {
            if (data === "True") {
                $(_self).find('span').removeClass('icon-hollow-favor').addClass('icon-collect');
                $(_self).find('em').text('已收藏');
            }
        }
    });
}



//获取分仓商品库存
function getDepotProdSkus() {

    //商家商品不走分仓
    if (parseInt($('#hdsuppId').val()) > 0) {
        return;
    }

    var suppid = $('#hdsuppId').val();
    var hasSKU = $('#hdHasSKU').val().toLocaleLowerCase();
    if (hasSKU == "true") {//开启了sku
        var pid = parseInt($("#hdProductId").val());
        $.ajax({
            url: $YSWL.BasePath + "Product/GetSKUInfos",
            type: 'post',
            dataType: 'text',
            timeout: 10000,
            async: true,
            data: { productId: pid, suppId: suppid },
            success: function (resultData) {
                $('#SKUDATA').val(resultData);
                InitializationSku();
            },
            error: function (XMLHttpRequest, textStatus, errorThrown) {

            }
        });
    } else {
        //没有开启sku
        var prodSku = $('#hdprodSku').val();
        $.ajax({
            url: $YSWL.BasePath + "Product/GetSKUStock",
            type: 'post',
            dataType: 'text',
            timeout: 10000,
            async: true,
            data: { sku: prodSku, suppId: suppid },
            success: function (resultData) {
                $('#hdprodSku').attr('stock', resultData);
                if (parseInt(resultData) <= 0) {//无库存
                    $('#div_stock').show().find('#stock_num').text('0');
                    noStock();
                } else { //有库存
                    $('#div_stock').show().find('#stock_num').text(resultData);
                    inStock();
                }
            },
            error: function (XMLHttpRequest, textStatus, errorThrown) {

            }
        });
    }
}


//无库存
function noStock() {
    $('#btnAddToCart').removeClass('addCart').addClass('addCart-gray');
    $('#divBuyInfo').hide();
    $('#closeArrivingNotifyMess').text("非常抱歉, 此商品已售罄!");
    $('#closeArrivingNotifyMess').show();
}

//有库存
function inStock() {
    $('#btnAddToCart').removeClass('addCart-gray').addClass('addCart');
    $('#divBuyInfo').show();
    $('#closeArrivingNotifyMess').hide();
}

//设置单品页配送地区
function setProdDetailDeliveryAreas(regoinId) {
    setDeliveryAreas(regoinId);
    getDepotProdSkus();
}
                                  