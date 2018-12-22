var selectItem;
$(function () {
    $('#cart_edit').show();
    resetSelectItem();

    //编辑
    $("#cart_edit").on('click', function () {
        $('.editState').toggle();
        $('.normalStatus').toggle();
        $('#footerbox').toggleClass('payment_total_bar');
        if ($('.editState').is(':visible')) {
            $(this).text('完成');
        } else {
            $(this).text('编辑');
        }
    });

    //减
    $(document).on('click', '.redu', function () {
        var itemId = $(this).parent().attr("ItemId");
        var count = parseInt($(this).next().val());
        if (count == 1) {
            deleteItem(itemId);
        } else {
            $(this).next().val(count - 1);
            $.ajax({
                type: "POST",
                dataType: "text",
                async: false,
                url: $YSWL.BasePath + "ShoppingCart/UpdateItemCount?s=" + new Date().format('yyyyMMddhhmmssS'),
                data: { ItemId: itemId, Count: count - 1 },
                success: function (data) {
                    if (data != "No") {
                        $("#LoadCartList").load($YSWL.BasePath + "ShoppingCart/CartListBySupp?s=" + new Date().format('yyyyMMddhhmmssS'), function () {
                            resetSelectItem();
                        });
                    } else {
                        ShowFailTip("服务器繁忙，请稍候再试！");
                    }
                }
            });
        }
    });

    //文本框
    $(document).on('blur', '.txtQuantity', function () {
        var count = parseInt($(this).val());
        var $this = $(this);
        var itemId = $(this).attr('ItemId');
        if (count < 1) {
            ShowConfirm("您确定要删除该商品吗？", function () {
                $.ajax({
                    type: "POST",
                    dataType: "text",
                    url: $YSWL.BasePath + "ShoppingCart/RemoveItem",
                    data: { ItemIds: itemId },
                    success: function (data) {
                        if (data != "No") {
                            $("#LoadCartList").load($YSWL.BasePath + "ShoppingCart/CartListBySupp?s=" + new Date().format('yyyyMMddhhmmssS'), function () {
                                resetSelectItem();
                            });
                        }
                    }
                });
            }, function () { updateQuantity(1, $this); })
        } else {
            updateQuantity(count, $this);
        }
    });

    function updateQuantity(count, $this) {
        var itemId = $this.parent().attr("ItemId");
        $.ajax({
            type: "POST",
            dataType: "text",
            url: $YSWL.BasePath + "ShoppingCart/UpdateItemCount?s=" + new Date().format('yyyyMMddhhmmssS'),
            data: { ItemId: itemId, Count: count },
            success: function (data) {
                if (data != "No") {
                    $("#LoadCartList").load($YSWL.BasePath + "ShoppingCart/CartListBySupp?s=" + new Date().format('yyyyMMddhhmmssS'), function () {
                        resetSelectItem();
                    });
                } else {
                    ShowFailTip("服务器繁忙，请稍候再试！");
                }
            }
        });
    }

    //加
    $(document).on('click', '.add', function () {
        var count = parseInt($(this).prev().val());
        $(this).prev().val(count + 1);
        var itemId = $(this).parent().attr("ItemId");
        var stock = $(this).parent().attr("stock");
        if (stock <= 0) {
            ShowFailTip("无货");
            return;
        }
        if (stock < (count + 1)) {
            ShowFailTip("库存不足！");
            return;
        }
        $.ajax({
            type: "POST",
            dataType: "text",
            async: false,
            url: $YSWL.BasePath + "ShoppingCart/UpdateItemCount?s=" + new Date().format('yyyyMMddhhmmssS'),
            data: { ItemId: itemId, Count: count + 1 },
            success: function (data) {
                if (data != "No") {
                    $("#LoadCartList").load($YSWL.BasePath + "ShoppingCart/CartListBySupp?s=" + new Date().format('yyyyMMddhhmmssS'), function () {
                        resetSelectItem();
                    });
                } else {
                    ShowFailTip("服务器繁忙，请稍候再试！");
                }
            }
        });
    });

    //全选
    $("#btnCheckAll").click(function () {
        var option;
        if (!$(this).hasClass('checked')) {
            $(".btnCheck").addClass("checked");
            option = "check";
        } else {
            $(".btnCheck").removeClass('checked');
            option = "remove";
        }
        $.ajax({
            type: "POST",
            dataType: "text",
            url: $YSWL.BasePath+"ShoppingCart/SelectedItemAll",
            data: { option: option },
            success: function (data) {
                if (data == "OK") {
                    $("#LoadCartList").load($YSWL.BasePath + "ShoppingCart/CartListBySupp?s=" + new Date().format('yyyyMMddhhmmssS'), function () {
                        resetSelectItem();
                    });
                } else {
                    ShowFailTip("服务器繁忙，请稍候再试！");
                }
            }
        });
    });

    //单选
    $(document).on('click', '.btnCheck', function () {
        var itemId = $(this).attr("ItemId");
        $.ajax({
            type: "POST",
            dataType: "text",
            url: $YSWL.BasePath + "ShoppingCart/SelectedItem",
            data: { id: itemId },
            success: function (data) {
                if (data == "OK") {
                    $("#LoadCartList").load($YSWL.BasePath + "ShoppingCart/CartListBySupp?s=" + new Date().format('yyyyMMddhhmmssS'), function () {
                        resetSelectItem();
                    });
                } else {
                    ShowFailTip("服务器繁忙，请稍候再试！");
                }
            }
        });
    });

    //删除选中项
    $(document).on('click', '#btnRemoveSelect', function () {
        if (!selectItem || selectItem.length <= 0) {
            ShowFailTip("请选择要删除的商品！");
            return;
        }
        var itemIds = selectItem.join(",");
        ShowConfirm("您确定要删除吗?", function () {
            $.ajax({
                type: "POST",
                dataType: "text",
                url: $YSWL.BasePath + "ShoppingCart/RemoveItem",
                data: { ItemIds: itemIds },
                success: function (data) {
                    if (data != "No") {
                        $("#LoadCartList").load($YSWL.BasePath + "ShoppingCart/CartListBySupp?s=" + new Date().format('yyyyMMddhhmmssS'), function () {
                            resetSelectItem();
                        });
                    } else {
                        ShowFailTip("服务器繁忙，请稍候再试！");
                    }
                }
            });
        });
    });
 
    //点击某组选中或取消选中
    $(document).on('click', '[id^="groupKey_"]', function () {
        var id = $(this).attr('suppId');
        var option;
        if (!$(this).hasClass('checked')) {
            $(this).addClass("checked");
            option = "check";
        } else {
            $(this).removeClass('checked');
            option = "remove";
        }
        $.ajax({
            type: "POST",
            dataType: "text",
            url: $YSWL.BasePath + "ShoppingCart/SelectedItemSuppAll",
            data: { option: option, suppId: id },
            success: function (data) {
                if (data == "OK") {
                    $("#LoadCartList").load($YSWL.BasePath + "ShoppingCart/CartListBySupp?s=" + new Date().format('yyyyMMddhhmmssS'), function () {
                        resetSelectItem();
                    });
                } else {
                    ShowFailTip("服务器繁忙，请稍候再试！");
                }
            }
        });
    });

    $('#toSettlement').click(function () {
        if (parseInt($('#SelectedQuantity').attr('Quantity')) < 1) {
            return false;
        }
        location.href = $YSWL.BasePath + 'Order/SubmitOrder';
    });
    


});

function resetSelectItem() {
    selectItem = new Array();
    //填充数组
    $(".btnCheck.checked").each(function () {
        var itemId = $(this).attr("ItemId");
        //判断选中数组中是否已存在该项
        var index = $.inArray(itemId, selectItem);
        if (index > -1) {
            selectItem.splice(index, 1);
        } else {
            selectItem.push(itemId);
        }
    });
}

function deleteItem(itemId) {
    ShowConfirm("您确定要删除吗?", function () {
        $.ajax({
            type: "POST",
            dataType: "text",
            url: $YSWL.BasePath + "ShoppingCart/RemoveItem",
            data: { ItemIds: itemId },
            success: function (data) {
                if (data != "No") {
                    $("#LoadCartList").load($YSWL.BasePath + "ShoppingCart/CartListBySupp?s=" + new Date().format('yyyyMMddhhmmssS'), function () {
                        resetSelectItem();
                    });
                }
            }
        });
    });
}






