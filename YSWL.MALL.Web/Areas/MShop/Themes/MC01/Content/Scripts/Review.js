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


//控制字数
function textCounter(thistext, maxlimit) {
    var content = $(thistext);
    var txtlength = parseInt(content.val().length);
    if (txtlength > maxlimit) {
        content.val(content.val().slice(0, 500));
    } else {
        content.prev('div').find('[name="shengyu"]').text(parseInt(maxlimit) - txtlength);
    }
}

$(function () {
    var uploadArr = $("[name='UploadPhoto']");
    for (var i = 0; i < uploadArr.length; i++) {
        qqupload(i);
    }

    //改变上传图片按钮的样式
    $('.qq-upload-button').css({  'width': 78, 'height': 78,'opacity':'0' });
});


//删除图片
function nameDel(sender) {
    var ulbtnparent = $(sender).parents('.reviewimg-upload');
    var targetVal = $(sender).attr('item');
    $(sender).parents('.upload_img_wrap').remove();
    var pathArray = ulbtnparent.find('[name="UploadPhotoPath"]').val().split('|');
    var index = pathArray.getIndexByValue(targetVal);
    pathArray.remove(index);
    ulbtnparent.find('[name="UploadPhotoPath"]').val(pathArray.join('|'));

    var nameVal = $(sender).attr('itemname');
    var nameArray = ulbtnparent.find('[name="UploadPhotoNames"]').val().split('|');
    var indexname = nameArray.getIndexByValue(nameVal);
    nameArray.remove(indexname);
    ulbtnparent.find('[name="UploadPhotoNames"]').val(nameArray.join('|'));
}

//上传图片按钮
var qqupload = function (k) {
    var ulbtnparent = $("[name='UploadPhoto']").eq(k).parent().parents('.upload');
    new qq.FineUploader({
        element: $("[name='UploadPhoto']")[k],
        request: {
            endpoint: '/UploadMultipleFileHandler.aspx'
        },
        text: {
            uploadButton: ''
        },
        multiple: true,
        validation: {
            allowedExtensions: ['jpeg', 'jpg', 'gif', 'png'],
            itemLimit: 10,
            sizeLimit: 5242880, // 50 kB = 50 * 1024 bytes
        },
        callbacks: {
            onUpload: function (id, fileName) {
                showLoad("正在上传");
            },
            onComplete: function (id, fileName, responseJSON) {

                $(".qq-upload-list").hide();
                if (responseJSON.success) {
                    ulbtnparent.append(('<div class="upload_img_wrap"><div class="upload_img"><img src="{0}"></div><div class="toolbar"><a href="javascript:;"  onclick="nameDel(this);"  item="{0}" itemname="{1}" ><img src="/Areas/MShop/Themes/MC01/Content/images/close.png" alt="删除" /></a></div></div>').format(
                       responseJSON.path.format(responseJSON.names), responseJSON.names));
                    ShowSuccessTip('上传成功！');
                    hideLoad();
                    ulbtnparent.find('[name="UploadPhotoPath"]').val(ulbtnparent.find('[name="UploadPhotoPath"]').val() + '|' + responseJSON.path.format(responseJSON.names));
                    ulbtnparent.find('[name="UploadPhotoNames"]').val(ulbtnparent.find('[name="UploadPhotoNames"]').val() + '|' + responseJSON.names);
                    imghover();
                    

                } else {
                    ShowFailTip("服务器没有返回数据，可能服务器忙，请稍候再试：");
                    setTimeout('hideLoad()', 1500);
                }
            }
        }
    });
};

//鼠标移入移出图片
var imghover = function () {
    $('.reviewimg-upload').find('img').parent('div').unbind('hover').hover(function () {
        $(this).find('span').css('display', 'inline-block');
    }, function () {
        $(this).find('span').css('display', 'none');
    });
};

//提交评论
var submit = function () {

    var json = []; //声明json
    for (var i = 0; i < $('.review_a').length; i++) {
        var contentval = $('[name="content"]').eq(i).val();

        if (contentval == "") {
            ShowFailTip("请填写评论内容！");
            return false;
        }
        if (contentval.length > 500) {
            ShowFailTip("评论内容过长！");
            return false;
        }
        if (ContainsDisWords(contentval)) {
            ShowFailTip('您评论的内容含有禁用词，请重新输入！');
            return false;
        }
        var imagesurlPath = $('[name="UploadPhotoPath"]').eq(i).val();
        var imagesurlName = $('[name="UploadPhotoNames"]').eq(i).val();
        var attribute = $('[name="attribute"]').eq(i).val();
        var sku = $('[name="sku"]').eq(i).val();
        var pid = $('[name="pid"]').eq(i).val();
        var itemid = $('[name="orderId"]').eq(i).val();
        json.push({ "pid": pid, "orderId": itemid, "attribute": attribute, "sku": sku, "contentval": contentval, "imagesurlPath": imagesurlPath, "imagesurlName": imagesurlName });
    }

    $.ajax({
        url: $YSWL.BasePath + "UserCenter/AjAxPReview",
        type: 'post',
        dataType: 'text',
        async: false,
        timeout: 10000,
        data: { PReviewjson: JSON.stringify(json) },
        success: function (resultData) {
            switch (resultData) {
                case "false":
                    ShowServerBusyTip("提交失败");
                    break;
                default:
                    var arr = resultData.split('|');
                    var tipText = "提交成功！";
                    if (arr.length > 1) {
                        if (parseInt(arr[0]) > 0 && parseInt(arr[1]) > 0) {
                            tipText += "+(" + arr[0] + ")积分,(" + arr[1] + ")成长值";
                        } else if (parseInt(arr[0]) > 0) {
                            tipText += "+(" + arr[0] + ")积分";
                        } else if (parseInt(arr[1]) > 0) {
                            tipText += "+(" + arr[1] + ")成长值";
                        }
                    }
                    ShowSuccessTip(tipText);
                    setTimeout(function () {
                        window.location.replace($YSWL.BasePath + "UserCenter/Orders");
                    }, 1000);
                    break;
            }
        },
        error: function (xmlHttpRequest, textStatus, errorThrown) {
            ShowServerBusyTip("服务器没有返回数据，可能服务器忙，请稍候再试！");
        }
    });
}
